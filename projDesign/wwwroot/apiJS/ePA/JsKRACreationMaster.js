var login_company_id;
var HaveDisplay;
var login_emp_id;
var is_manager;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var _manager = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        if (_manager == "yes") {
            is_manager = 1;
        }
        else {
            is_manager = 0;
        }

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindDepartmentList_except_all('ddldepartment', login_company_id, 0);
        BindEmployeeCodeFromEmpMasterByComp('ddlempcodee', login_company_id, 0);
        GetData(login_company_id);



        $("#empdivv").hide();
        $("#btnupdate").hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#ddlcompany").bind("change", function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlempcodee', $(this).val(), 0);
            BindDepartmentList_except_all('ddldepartment', $(this).val(), 0);
            if ($(this).val() > 0) {
                GetData($(this).val());
                BindWorkingRoleByDeptID('ddlworkingrole', $(this).val(), 0, 0);
            }
            else {
                if ($.fn.DataTable.isDataTable('#tblkramaster')) {
                    $('#tblkramaster').DataTable().clear().draw();
                }
                $("#ddlworkingrole option[value!='']").remove();

            }

            $("#txtdesc").val('');
            $("#txtfactor_result").val('');
            $("#hdnkramasterid").val()
        });


        $("#ddldepartment").bind("change", function () {
            if ($("#ddlcompany").val() == null || $("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0") {
                $("#ddldepartment").val('0');
                messageBox("error", "Please firstly select company");
                return false;
            }

            BindWorkingRoleByDeptID('ddlworkingrole', $("#ddlcompany").val(), $(this).val(), 0);
        });

        $("#btnsave").bind("click", function () {

            var companyidd = $("#ddlcompany").val();
            // var fiscal_yr = $("#ddlfinancialyr").val();
            // var epa_cycle = $("#ddlfinancialcycl").val();
            var depart_id = $("#ddldepartment").val();
            var work_id = $("#ddlworkingrole").val();
            var description = $("#txtdesc").val();
            var factor_result = $("#txtfactor_result").val();


            var is_error = false;
            var error_msg = "";

            if (companyidd == "" || companyidd == "0" || companyidd == null) {
                is_error = true;
                error_msg = error_msg + "Please select company </br>";
            }

            //if (fiscal_yr == null || fiscal_yr == "0" || fiscal_yr == "") {
            //    is_error = true;
            //    error_msg = error_msg + "Please select financial year</br>";
            //}

            //if (epa_cycle == "" || epa_cycle == null || epa_cycle == "0") {
            //    is_error = true;
            //    error_msg = error_msg + "Please enter Factor/Result</br>";
            //}

            if (depart_id == "" || depart_id == "0" || depart_id == null) {
                is_error = true;
                error_msg = error_msg + "Please Select Department</br>";
            }

            if (work_id == "" || work_id == "0" || work_id == null) {
                is_error = true;
                error_msg = error_msg + "Please Working Rule</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please enter Description</br>";
            }

            if (factor_result == "" || factor_result == null) {
                is_error = true;
                error_msg = error_msg + "Please enter Factor/Result</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            $("#loader").show();

            var for_all_emp = 1;


            var mydata = {
                company_id: companyidd,
                //financial_yr: fiscal_yr,
                //cycle_id: epa_cycle,
                department_id: depart_id,
                wrk_role_id: work_id,
                description: description,
                factor_result: factor_result,
                created_by: login_emp_id,
                emp_id: login_emp_id,//emp_id,
                for_all_emp: for_all_emp,
                is_manager: is_manager,

            }








            if (confirm("Do you want to save this ?")) {
                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Save_KRAMaster",
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
                            //alert(msg);
                            //location.reload();

                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            BindDepartmentList_except_all('ddldepartment', login_company_id, 0);
                            BindEmployeeCodeFromEmpMasterByComp('ddlempcodee', login_company_id, 0);
                            GetData(login_company_id);

                            // $("#ddlworkingrole").empty();
                            $("#ddlworkingrole option[value!='']").remove();
                            $("#txtdesc").val('');
                            $("#txtfactor_result").val('');
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


        $("#btnupdate").bind("click", function () {

            var companyidd = $("#ddlcompany").val();
            //var fiscal_yr = $("#ddlfinancialyr").val();
            //var epa_cycle = $("#ddlfinancialcycl").val();
            var depart_id = $("#ddldepartment").val();
            var work_id = $("#ddlworkingrole").val();
            var description = $("#txtdesc").val();
            var factor_result = $("#txtfactor_result").val();


            var is_error = false;
            var error_msg = "";

            if (companyidd == "" || companyidd == "0" || companyidd == null) {
                is_error = true;
                error_msg = error_msg + "Please select company </br>";
            }

            //if (fiscal_yr == null || fiscal_yr == "0" || fiscal_yr == "") {
            //    is_error = true;
            //    error_msg = error_msg + "Please select financial year</br>";
            //}

            //if (epa_cycle == "" || epa_cycle == null || epa_cycle == "0") {
            //    is_error = true;
            //    error_msg = error_msg + "Please enter Factor/Result</br>";
            //}

            if (depart_id == "" || depart_id == "0" || depart_id == null) {
                is_error = true;
                error_msg = error_msg + "Please Select Department</br>";
            }

            if (work_id == "" || work_id == "0" || work_id == null) {
                is_error = true;
                error_msg = error_msg + "Please Working Rule</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please enter Description</br>";
            }

            if (factor_result == "" || factor_result == null) {
                is_error = true;
                error_msg = error_msg + "Please enter Factor/Result</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var for_all_emp = 1;
            //var emp_id = 0;

            //if (is_manager == 1) {
            //    emp_id = $('#ddlempcodee option').last().val();
            //}
            //else {
            //    for_all_emp = 0;
            //    emp_id = login_emp;
            //}


            var mydata = {
                kra_mstr_id: $("#hdnkramasterid").val(),
                company_id: companyidd,
                //financial_yr: fiscal_yr,
                //cycle_id: epa_cycle,
                department_id: depart_id,
                wrk_role_id: work_id,
                description: description,
                factor_result: factor_result,
                modified_by: login_emp_id,
                emp_id: login_emp_id, //emp_id,
                for_all_emp: for_all_emp,
                is_manager: is_manager,


            }

            $("#loader").show();

            if (confirm("Do you want to save this ?")) {



                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Update_KRAMaster",
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
                            //alert(msg);
                            //location.reload();
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            BindDepartmentList_except_all('ddldepartment', login_company_id, 0);
                            BindEmployeeCodeFromEmpMasterByComp('ddlempcodee', login_company_id, 0);
                            GetData(0);
                            $("#hdnkramasterid").val('');
                            $("#ddlworkingrole option[value!='']").remove();
                            $("#txtdesc").val('');
                            $("#txtfactor_result").val('');
                            $("#btnupdate").hide();
                            $("#btnsave").show();
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



function GetData(company_id) {
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_KRAMaster/0/" + company_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblkramaster").DataTable({
                "processing": true,//for showing process bar
                "serverSide": false,//for process server side
                "bDestroy": true,
                "orderMulti": false,//for disable multiple column at once
                "filter": true,//this is for disable filter(search box)
                "aaData": res,
                "scrollX": 200,
                "columnDefs": [

                    {
                        targets: [6],
                        render: function (data, type, row) {
                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            var date = new Date(data);

                            return new Date(row.modified_dt) < new Date(row.created_dt) ? "-" : GetDateFormatddMMyyyy(date);

                            // return GetDateFormatddMMyyyy(date);
                        }
                    }
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                    //{ "data": "financial_name", "name": "financial_name", "title": "Financial Year", "autoWidth": true },
                    //{ "data": "cycle_name", "name": "cycle_name", "title": "Financial Cycle", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "work_role_name", "name": "work_role_name", "title": "Working Role", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "factor_result", "name": "factor_result", "title": "Factor/Result", "autoWidth": true },
                    { "data": "creatd_dt", "name": "creatd_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick=GetEditData(' + full.company_id + ',' + full.kra_mstr_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteKRAMaster(' + full.company_id + ',' + full.kra_mstr_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]

            });
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function GetEditData(companyidd, kramaster_id) {

    $("#loader").show();

    $.ajax({

        url: localStorage.getItem("ApiUrl") + "apiePA/Get_KRAMaster/" + kramaster_id + "/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            BindAllEmp_Company('ddlcompany', login_emp_id, res.company_id);

            $("#ddlcompany").attr("disabled", "disabled");
            //BindEPA_FiscallYrByComp('ddlfinancialyr', res.company_id,res.financial_yr);
            //BindEPA_CycleByComp('ddlfinancialcycl', res.company_id, res.cycle_id);
            BindDepartmentList_except_all('ddldepartment', res.company_id, res.department_id);


            BindWorkingRoleByDeptID('ddlworkingrole', res.company_id, res.department_id, res.wrk_role_id);


            $("#txtdesc").val(res.description);
            $("#txtfactor_result").val(res.factor_result);

            $("#hdnkramasterid").val(res.kra_mstr_id);

            $("#btnupdate").show();
            $("#btnsave").hide();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function DeleteKRAMaster(companyidd, kramasterid) {
    $("#loader").show();

    var for_all_emp = 1;
    var emp_id = 0;

    if (is_manager == 1) {
        emp_id = $('#ddlempcodee option').last().val();
    }
    else {
        for_all_emp = 0;
        emp_id = login_emp_id;
    }

    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiePA/Delete_KRAMaster/" + kramasterid + "/" + companyidd + "/" + emp_id + "/" + is_manager + "/" + for_all_emp,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                _GUID_New();
                var statuscode = response.statusCode;
                var msg = response.message;

                if (statuscode == "0") {
                    $("#txtdesc").val('');
                    $("#txtfactor_result").val('');
                    $("#hdnkramasterid").val();
                    $("#ddlworkingrole option[value!='0']").remove();
                    BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                    BindDepartmentList_except_all('ddldepartment', login_company_id, 0);
                    BindEmployeeCodeFromEmpMasterByComp('ddlempcodee', login_company_id, 0);
                    GetData(login_company_id);
                }




                $("#loader").hide();
                messageBox("success", msg);
                return false;
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
                return false;
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}