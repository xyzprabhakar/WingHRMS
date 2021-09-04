$('#loader').show();

var login_empid;
var companyidd;
var emp_role_idd;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        companyidd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // BindGradeList('ddlfromgrade', 0, 0);
        //  BindGradeList('ddltograde', 0, 0);
        BindSalaryGroupList('ddlfromsalaygroup', 0, 0);
        BindSalaryGroupList('ddltosalarygroup', 0, 0);
        // $('#ddlsalgroup').prop("disabled", true);
        $('#ddlfromgrade').bind("change", function () {
            //BindSalaryGroupList('ddlfromsalaygroup', 0, $(this).val());
            // $('#ddlfromsalaygroup').prop("disabled", false);
            $('#tblformulacomponent').empty();
        });

        $('#ddlfromsalaygroup').bind("change", function () {

            //BindGradeList('ddltograde', 0, 0);
            // $('#ddltosalarygroup').empty();
            $("#tblformulacomponent").empty();
            GetFormulaComponent($(this).val());
        });

        //$('#ddltograde').bind("change", function () {
        //BindSalaryGroupList('ddltosalarygroup', 0, $(this).val());
        //$('#ddltosalarygroup').prop("disabled", false);
        //});

        var formulalist = new Array();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $('#ddltosalarygroup').bind("change", function () {
            $("#hdn_tosalarygoupid").val($(this).val());
        });

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            formulalist.splice(0, formulalist.length);

            var iserror = false;
            var errormsg = "";

            var company_id = $("#ddlcompany").val();
            // var from_grade = $("#ddlfromgrade").val();
            var from_salaygroup = $("#ddlfromsalaygroup").val();
            //var to_grade = $("#ddltograde").val();
            var to_salarygroup = $("#ddltosalarygroup").val();

            if (company_id == "0") {
                errormsg = errormsg + 'Please Select Company <br/>';
                iserror = true;
            }
            //if (from_grade == "0") {
            //    errormsg = errormsg + 'Please Select From Grade <br/>';
            //    iserror = true;
            //}
            if (from_salaygroup == "0") {
                errormsg = errormsg + 'Please Select From Salary Group <br/>';
                iserror = true;
            }
            //if (to_grade == "0") {
            //    errormsg = errormsg + 'Please Select To Grade <br/>';
            //    iserror = true;
            //}
            if (to_salarygroup == "0") {
                errormsg = errormsg + 'Please Select To Salary Group<br/>';
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var table = $('#tblformulacomponent').DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var formulaa = {};
                formulaa.s_no = table.cell(rowIdx, 0).data();
                formulaa.formula = table.cell(rowIdx, 2).data();
                formulaa.component_id = table.cell(rowIdx, 5).data();
                formulaa.company_id = table.cell(rowIdx, 6).data();
                //formulaa.salary_group_id = table.cell(rowIdx, 6).data(); 
                formulaa.salary_group_id = $("#hdn_tosalarygoupid").val(); // add here which was selected as to salarygroup id
                formulaa.component_type = table.cell(rowIdx, 8).data();
                formulaa.function_calling_order = table.cell(rowIdx, 9).data();
                formulaa.is_salary_comp = table.cell(rowIdx, 10).data();
                formulaa.is_data_entry_comp = table.cell(rowIdx, 11).data();
                formulaa.is_tds_comp = table.cell(rowIdx, 12).data();
                formulaa.payment_type = table.cell(rowIdx, 13).data();
                formulaa.is_user_interface = table.cell(rowIdx, 14).data();
                formulaa.sno = table.cell(rowIdx, 15).data();
                formulaa.is_payslip = table.cell(rowIdx, 16).data();
                formulaa.created_by = login_empid;


                formulalist.push(formulaa);
            });


            var mydata = {
                component_mstr: formulalist
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiPayroll/Copied_Component_Fromula',
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        var table = $('#tblformulacomponent').DataTable();
                        table.clear().draw();
                        // $('#tblformulacomponent').empty();
                        $("#ddlfromsalaygroup").val('0');
                        $("#ddltosalarygroup").val('0');
                        messageBox("success", Msg);
                        //alert(Msg);
                        //location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {

                        if ($('#tblformulacomponent').DataTable().column(0).data().length > 0) {
                            ConfirmDialog(Msg, mydata.component_mstr);
                        }
                        else {
                            messageBox("error", "No Forumula's available");
                            return false;
                        }


                    }
                    else {
                        messageBox("error", Msg);
                        return false
                    }

                },
                error: function (data) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(data.responseText);
                }
            });
        });

        $('#loader').hide();

    }, 2000);// end timeout

});

function GetFormulaComponent(salarygrpid) {
    $('#loader').show();
    if (salarygrpid == '' || salarygrpid == null) {
        messageBox("error", "Something went wrong, Please try again later");
        $('#loader').hide();
        return false;
    }
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiPayroll/Get_FromulaComponent/' + $("#ddlcompany").val() + "/" + salarygrpid,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            var res = response;
            $('#loader').hide();
            _GUID_New();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            $("#tblformulacomponent").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {
                                var created_dt = row.created_dt;
                                var modified_dt = row.modified_dt;
                                var date = new Date(data);
                                //return GetDateFormatddMMyyyy(date);
                                return new Date(modified_dt) < new Date(created_dt) ? "-" : GetDateFormatddMMyyyy(modified_dt);
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "property_details", "name": "property_details", "title": "Salary Component", "auoWidth": true },
                    //{ "data": "s_noo", "name": "s_noo", "autoWidth": true, "title": "SNo" },
                    { "data": "formula", "name": "formula", "autoWidth": true, "title": "Formula" },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true, "title": "Created On" },
                    { "data": "modified_dt", "name": "modified_dt", "autoWidth": true, "title": "Modified On" },
                    { "data": "component_id", "name": "component_id", "autoWidth": true, "visible": false },
                    { "data": "company_id", "name": "company_id", "autoWidth": true, "visible": false },
                    { "data": "salary_group_id", "name": "salary_group_id", "autoWidth": true, "visible": false },
                    { "data": "component_type", "name": "component_type", "autoWidth": true, "visible": false },
                    { "data": "function_calling_order", "name": "function_calling_order", "autoWidth": true, "visible": false },
                    { "data": "is_salary_comp", "name": "is_salary_comp", "autoWidth": true, "visible": false },
                    { "data": "is_data_entry_comp", "name": "is_data_entry_comp", "autoWidth": true, "visible": false },
                    { "data": "is_tds_comp", "name": "is_tds_comp", "autoWidth": true, "visible": false },
                    { "data": "payment_type", "name": "payment_type", "autoWidth": true, "visible": false },
                    { "data": "is_user_interface", "name": "is_user_interface", "autoWidth": true, "visible": false },
                    { "data": "sno", "name": "sno", "autoWidth": true, "visible": false },
                    { "data": "is_payslip", "name": "is_payslip", "autoWidth": true, "visible": false }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (response) {
            $('#loader').hide();
            _GUID_New();
            alert(response.responseText);
        }
    });
}

function ConfirmDialog(message, formulalist) {
    $('<div></div>').appendTo('body')
        .html('<div><h6>' + message + '?</h6></div>')
        .dialog({
            modal: true, title: 'Formula Already Exists', zIndex: 10000, autoOpen: true,
            width: 'auto', resizable: false,
            buttons: {
                Yes: function () {
                    DeleteData(formulalist);
                },
                No: function () {

                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                $(this).remove();
            }
        });
};

function DeleteData(formulalist) {
    $('#loader').show();
    var mydata = {
        component_mstr: formulalist
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Delete_component_Property_Detail',
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (data) {
            $('#loader').hide();
            _GUID_New();
            alert(data.responseText);
        }
    });
}


