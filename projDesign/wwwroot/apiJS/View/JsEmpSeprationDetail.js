var login_emp_id;
var default_company;
var HaveDisplay;
var is_manager = 0;
var for_all_emp = 1;
var emp_dtl = [];


$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        //HaveDisplay = ISDisplayMenu("Display Company List");

        //var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);


        //if (is_managerr_dec == 'yes') {
        //    is_manager = 1
        //    for_all_emp = 0;
        //}
        //else if (HaveDisplay != 0) {
        //    is_manager = 1
        //    for_all_emp = 0;
        //}

        //BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', default_company, login_emp_id, login_emp_id);

        //if (HaveDisplay == 0) {
        //   // BindCompanyList('ddlcompany', default_company);
        //   var EmpList = ISDisplayMenu("Is Company Admin"); // check its a user or not
        //    if (EmpList == 0) {


        //       // BindEmployeeListUnderLoginEmp('ddlemployee', login_emp_id);
        //       // BindEmployeeDetails(login_emp_id);
        //        //BindEmpResignReason('ddlreason', default_company, 0);
        //    }
        //    else {
        //        BindEmployeeCodeFromEmpMasterByComp('ddlemployee', default_company, 0);
        //    }
        //}
        //else {
        //    BindCompanyList('ddlcompany', 0);
        //}


        Get_ESepration_dtl(login_emp_id, default_company);

        $('[data-toggle="tooltip"]').tooltip();
        // Get_under_emp();
        //$('#loader').hide();


        $("#btnsave_cancel").bind("click", function () {

            raise_cancel_request();

        });

        $("#ddlcompany").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tblempsepration_dtl')) {
                $('#tblempsepration_dtl').DataTable().clear().draw();
            }
            if ($(this).val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            else {

                Get_ESepration_dtl(login_emp_id, $(this).val());
            }
        });

    }, 2000);// end timeout

});






function Get_ESepration_dtl(emp_idd, company_id) {

    if ($.fn.DataTable.isDataTable('#tblempsepration_dtl')) {
        $('#tblempsepration_dtl').DataTable().clear().draw();
    }

    $('#loader').show();

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetails/" + company_id + "/" + emp_idd,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblempsepration_dtl").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "filter": true,//to enable search box
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee Sepration Details',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    },
                ],
                "aaData": res,
                "info": false,
                "paging": false,
                "columnDefs": [
                    {
                        targets: [4],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [7, 8],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name_code", "title": "Employee Name", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "title": "Date of Joining", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },

                    { "data": "resignation_dt", "name": "resignation_dt", "title": "Date of Resignation", "autoWidth": true },
                    { "data": "req_relieving_date", "name": "req_relieving_date", "title": "Req Relieving Date", "autoWidth": true },
                    { "data": "req_reason", "name": "req_reason", "title": "Reason", "autoWidth": true },
                    { "data": "req_remarks", "name": "req_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "final_status", "name": "final_status", "title": "Final Status", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,

                        "render": function (data, type, full, meta) {

                            if (full.ref_doc_path != undefined && full.ref_doc_path != null && full.ref_doc_path != "")
                            {
                                var path = localStorage.getItem("ApiUrl").replace("api/", "") + full.ref_doc_path;
                                return '<a href="" onclick="OpenDocumentt(\'' + path + '\')"><i class="fa fa-paperclip"></i></a>';
                            }
                            else return "";
                    }
                    },
                {
                    "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {

                        return '<a href="#" onclick="ViewDetails(' + full.sepration_id + ',' + full.emp_id + ',1,' + full.company_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                    }
                },
                {
                    "title": "Cancel Request ?", "render": function (data, type, full, meta) {

                        if (full.final_status == "Approved" || full.final_status == "Pending") {
                            return '<a href="#" onclick="CancelRequest(' + full.sepration_id + ',' + full.emp_id + ',0,' + full.company_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-window-close"></i></a>';
                        }
                        else { return ""; }
                    }
                }

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });

    $('#loader').hide();

},
error: function (err) {
    $("#loader").hide();
    messageBox("error", err.responseText);
    return false;
}
    });
}

function OpenDocumentt(path) {
    debugger;

    window.open(path);
}

function ViewDetails(sep_id, emp_id, view_dtl, companyid) {
    $('#loader').show();

    if (view_dtl == 1) {
        $("#cancel_submit_div").css("display", "none");
        $("#txt_cncl_remarks").attr('disabled', 'disabled');
    }
    else {
        $("#cancel_submit_div").css("display", "block");
        $("#txt_cncl_remarks").removeAttr('disabled');
    }

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + companyid,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response.data;
            var _app_history = response._approval_history;

            $("#txtdob_").val(GetDateFormatddMMyyyy(new Date(res.dob_)));
            $("#txtdoj_").val(GetDateFormatddMMyyyy(new Date(res.date_of_joining)));
            $("#txtdept_").val(res.department_name);
            $("#txtdesignation_").val(res.designation_name);
            $("#txtgrade_").val(res.grade_name);
            $("#txtloc_").val(res.location_name);
            var actual_notice = res.diff_notice_days != 0 ? parseFloat(res.req_notice_days) - parseFloat(res.diff_notice_days) : res.req_notice_days;
            $("#txtnoticeperiod_days_").val(actual_notice);
            $("#txtresign_dt_").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt_").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notices_").val(res.req_notice_days);
            $("#txt_notice_diff_days_").val(res.diff_notice_days);
            $("#txt_policy_reliving_").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason_").val(res.req_reason);
            $("#txtremarks_").val(res.req_remarks);
            $("#txtfinal_working_dt_").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));

            $("#txt_cncl_rqst").val(res.is_cancel);
            $("#txt_cncl_remarks").val(res.cancel_remarks);
            $("#txtcancel_dt").val('');


            $("#_approver_dtl").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "filter": false,//to enable search box
                "aaData": _app_history,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "_name", "name": "_name", "title": "Approver Name(Code)", "autoWidth": true },
                    { "data": "_remarks", "name": "_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "_action", "name": "_action", "title": "Status", "autoWidth": true },
                    { "data": "_dt", "name": "_dt", "title": "Action On", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
            return false;
        }

    });
}

function CancelRequest(sep_id, emp_id, view_dtl, companyid) {
    $('#loader').show();

    $("#hdnemp_id").val(emp_id);
    $("#hdnreq_id").val(sep_id);



    if (view_dtl == 1) {
        $("#cancel_submit_div").css("display", "none");
        $("#txt_cncl_remarks").attr('disabled', 'disabled');
    }
    else {
        $("#cancel_submit_div").css("display", "block");
        $("#txt_cncl_remarks").removeAttr('disabled');
    }

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + companyid,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response.data;
            var _app_history = response._approval_history;

            $("#txtdob_").val(GetDateFormatddMMyyyy(new Date(res.dob_)));
            $("#txtdoj_").val(GetDateFormatddMMyyyy(new Date(res.date_of_joining)));
            $("#txtdept_").val(res.department_name);
            $("#txtdesignation_").val(res.designation_name);
            $("#txtgrade_").val(res.grade_name);
            $("#txtloc_").val(res.location_name);
            var actual_notice = res.diff_notice_days != 0 ? parseFloat(res.req_notice_days) - parseFloat(res.diff_notice_days) : res.req_notice_days;
            $("#txtnoticeperiod_days_").val(actual_notice);
            $("#txtresign_dt_").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt_").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notices_").val(res.req_notice_days);
            $("#txt_notice_diff_days_").val(res.diff_notice_days);
            $("#txt_policy_reliving_").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason_").val(res.req_reason);
            $("#txtremarks_").val(res.req_remarks);
            $("#txtfinal_working_dt_").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));
            $("#txt_cncl_rqst").val(res.is_cancel);
            $("#txt_cncl_remarks").val(res.cancel_remarks).prop("disabled", res.is_cancel == "Yes" ? true : false);

            $("#txtcancel_dt").val(GetDateFormatddMMyyyy(new Date()));
            $("#hdnresign_dt").val(res.resignation_dt);
            $("#hdncomp_id").val(res.company_id);

            if (res.is_cancel == "Yes" || (res.is_final_approve == "2" || res.is_final_approve == "3")) {
                $("#cancel_submit_div").css("display", "none");
            }
            else {
                $("#cancel_submit_div").css("display", "block");
            }

            $("#_approver_dtl").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "filter": false,//to enable search box
                "aaData": _app_history,
                "info": false,
                "paging": false,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "_name", "name": "_name", "title": "Approver Name(Code)", "autoWidth": true },
                    { "data": "_remarks", "name": "_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "_action", "name": "_action", "title": "Status", "autoWidth": true },
                    { "data": "_dt", "name": "_dt", "title": "Action On", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });


            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
            return false;
        }

    });
}

function raise_cancel_request() {

    var cancel_remarks = $("#txt_cncl_remarks").val();


    var emp_id = $("#hdnemp_id").val();
    var req_id = $("#hdnreq_id").val();
    var resign_dt = $("#hdnresign_dt").val();
    var company_id = $("#hdncomp_id").val();

    var req_dtl = [];

    if (emp_id == "" || emp_id == null || emp_id == undefined) {
        messageBox("error", "Some thing wrong with request can't raised");
        return false;
    }

    if (req_id == "" || req_id == null || req_id == undefined) {
        messageBox("error", "Some thing wrong with request can't raised");
        return false;
    }
    if (resign_dt == "" || resign_dt == null || resign_dt == undefined) {
        messageBox("error", "something wrong with request can't raised");
        return false;
    }
    if (cancel_remarks == "" || cancel_remarks == null) {
        messageBox("error", "Please enter Cancellation Remarks");
        return false;
    }

    if (company_id == "" || company_id == null || company_id == "0") {
        messageBox("error", "Some thing wrong with request can't raised");
        return false;
    }

    req_dtl.push({ 'req_id': req_id, 'emp_id': emp_id, 'remarks': cancel_remarks, 'final_relieve_dt': resign_dt });

    var mydata = {
        company_id: company_id,
        emp_req: req_dtl,
        //emp_id: emp_id,
        //req_id:req_id,
        //req_remarks: cancel_remarks,
        created_by: login_emp_id,
    };

    $("#loader").show();

    if (confirm("Do you want to process this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();


        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Cancel_Employee_Separation_Request",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(mydata),
            headers: headerss,
            success: function (response) {
                _GUID_New();
                var res = response;
                var statuscode = res.statusCode;
                var msg = res.message;
                $("#loader").hide();
                if (statuscode != "0") {
                    messageBox("error", msg);
                    return false;
                }
                else {
                    alert(msg);
                    location.reload();
                    //alert(msg);
                    //location.reload();
                }
            },
            error: function (request, status, error) {
                $('#loader').hide();
                _GUID_New();
                var error = "";
                var errordata = JSON.parse(request.responseText);
                try {
                    var i = 0;
                    while (Object.keys(errordata).length > i) {
                        var j = 0;
                        while (errordata[Object.keys(errordata)[i]].length > j) {
                            error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                            j = j + 1;
                        }
                        i = i + 1;
                    }

                } catch (err) { }
                messageBox("error", error);
                return false;
            }


        });


    }
    else {
        $("#loader").hide();
        return false;
    }
}





