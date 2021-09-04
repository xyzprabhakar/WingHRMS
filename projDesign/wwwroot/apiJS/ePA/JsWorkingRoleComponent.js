$('#loader').show();
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindDepartmentList_except_all('ddlfrom_depart', login_company_id, 0);
        BindDepartmentList_except_all('ddlto_depart', login_company_id, 0);

        //HaveDisplay = ISDisplayMenu("Display Company List");


        $("#detailssdiv").hide();
        $('#loader').hide();


        $("#ddlcompany").bind("change", function () {

            BindDepartmentList_except_all('ddlfrom_depart', $(this).val(), 0);
            BindDepartmentList_except_all('ddlto_depart', $(this).val(), 0);
            $("#detailssdiv").hide();
            if ($(this).val() > 0) {
                BindWorkingRoleByDeptID('ddlfrom_wrkrole', $(this).val(), 0, 0);
            }
            else {
                $('#ddlfrom_wrkrole option[value!=0]').remove();
            }
            if ($.fn.DataTable.isDataTable('#tblkpi_keyarea_master')) {
                $('#tblkpi_keyarea_master').DataTable().clear().draw();
            }


        });

        $("#ddlfrom_depart").bind("change", function () {

            $("#detailssdiv").hide();
            if ($(this).val() > 0) {
                BindWorkingRoleByDeptID('ddlfrom_wrkrole', $("#ddlcompany").val(), $(this).val(), 0);
            }
        });

        $("#ddlto_depart").bind("change", function () {
            if ($(this).val() > 0) {
                BindWorkingRoleByDeptID('ddlto_wrkrole', $("#ddlcompany").val(), $(this).val(), 0);
            }


        });

        $("#ddlfrom_wrkrole").bind("change", function () {

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                messageBox("error", "Please select company");
                return false;
            }

            GetData($("#ddlcompany").val(), $(this).val());

        });

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });

        $("#btnsave").bind("click", function () {
            var companyid = $("#ddlcompany").val();
            var fromdept = $("#ddlfrom_depart").val();
            var from_wrk_role = $("#ddlfrom_wrkrole").val();
            var todept = $("#ddlto_depart").val();
            var to_wrk_role = $("#ddlto_wrkrole").val();

            var is_error = false;
            var error_msg = "";
            if (companyid == "0" || companyid == "" || companyid == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }
            if (fromdept == "" || fromdept == "0" || fromdept == null) {
                is_error = true;
                error_msg = error_msg + "Please select from Department</br>";
            }
            if (from_wrk_role == "" || from_wrk_role == "0" || from_wrk_role == null) {
                is_error = true;
                error_msg = error_msg + "Please select from work role</br>";
            }
            if (todept == "" || todept == "0" || todept == null) {
                is_error = true;
                error_msg = error_msg + "Please select to department</br>";
            }
            if (to_wrk_role == "" || to_wrk_role == "0" || to_wrk_role == null) {
                is_error = true;
                error_msg = error_msg + "Please select to work role</br>";
            }
            if (from_wrk_role == to_wrk_role) {
                is_error = true;
                error_msg = error_msg + "From Working Role and To Working role cannot be same</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }
            var mydata = {
                companyid: companyid,
                from_dept: fromdept,
                from_wrk_role: from_wrk_role,
                to_dept: todept,
                to_wrk_role: to_wrk_role,
                created_by: login_emp_id,
            }


            $("#loader").show();

            if (confirm("Are you sure you want to copy it into '" + $("#ddlto_wrkrole option:selected").text() + "'")) {
                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("hdnsalt").val();

                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Save_WorkingRoleComponent",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var statuscode = response.statusCode;
                        var msg = response.message;
                        if (statuscode == "0") {

                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);

                            BindDepartmentList_except_all('ddlfrom_depart', login_company_id, 0);
                            BindDepartmentList_except_all('ddlto_depart', login_company_id, 0);
                            BindWorkingRoleByDeptID('ddlfrom_wrkrole', login_company_id, 0, 0);
                            $("#tblkpi_keyarea_master").empty();
                            $("#tblkra_master").empty();
                            $("#tblques_master").empty();
                            $("#detailssdiv").hide();

                            $("#loader").hide();
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            $("#loader").hide();
                            messageBox("error", msg);
                            return false;
                        }
                    },
                    error: function (request, status, error) {
                        _GUID_New();
                        $('#loader').hide();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }
                });
            }
            else {
                $("#loader").hide();
                return false;
            }

        });

    }, 2000);// end timeout


});


function GetData(companyid, wrkid) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_WorkingRoleComponents/" + companyid + "/" + wrkid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined && res.statusCode != null) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#detailssdiv").show();

            $("#tblkpi_keyarea_master").DataTable({
                "processing": true,
                "serverSide": false,
                "bDestroy": true,
                "orderMulti": false,
                "filter": true,
                "aaData": res.kpi_key_area_mstr,
                "scrollX": 200,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    //{ "data": "financial_year_name", "name": "financial_year_name", "title": "Financial Year", "autoWidth": true },
                    { "data": "objective_name", "name": "objective_name", "title": "Objective Type", "autoWidth": true },
                    { "data": "key_area", "name": "key_area", "title": "Key Area", "autoWidth": true },
                    { "data": "expected_result", "name": "expected_result", "title": "Expected Result", "autoWidth": true },
                    { "data": "wtg", "name": "wtg", "title": "Wtg", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });


            $("#tblkra_master").DataTable({
                "processing": true,
                "serverSide": false,
                "bDestroy": true,
                "orderMulti": false,
                "filter": true,
                "aaData": res.kra_mstr,
                "scrollX": 200,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    //{ "data": "financial_name", "name": "financial_name", "title": "Financial Year", "autoWidth": true },
                    //{ "data": "cycle_name", "name": "cycle_name", "title": "Cycle Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "factor_result", "name": "factor_result", "title": "Factor Result", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });


            $("#tblques_master").DataTable({
                "processing": true,
                "serverSide": false,
                "bDestroy": true,
                "orderMulti": false,
                "filter": true,
                "aaData": res.ques_master,
                "scrollX": 200,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "tab_name", "name": "tab_name", "title": "Tab Name", "autoWidth": true },
                    { "data": "ques", "name": "ques", "title": "Question", "autoWidth": true },
                    { "data": "anss_type", "name": "anss_type", "title": "Ans Type", "autoWidth": true },
                    {
                        "title": "Data Source", "autoWidth": true, "render": function (data, type, full, meta) {

                            return (full.ans_type == "2" && full.ans_type_ddl != "") ? full.ans_type_ddl : "-";

                        }
                    },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}


