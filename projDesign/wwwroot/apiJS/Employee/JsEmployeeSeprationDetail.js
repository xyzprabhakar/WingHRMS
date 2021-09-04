$('#loader').show();
var login_emp_id;
var default_company;
var HaveDisplay;
var is_manager = 0;
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // HaveDisplay = ISDisplayMenu("Display Company List");

        var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

        if (is_managerr_dec == 'yes') {
            is_manager = 1
        }
        else {
            $("#divfromtodate").css("display", "none");
        }



        //var date = new Date();
        //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        //var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);


        //$("#txtFromDate").val(GetDateFormatddMMyyyy(firstDay));
        //$("#txtToDate").val(GetDateFormatddMMyyyy(lastDay));


        //BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        //BindEmployeeCodeFromEmpMasterByComp('ddlemployee', default_company, 0);


        $('[data-toggle="tooltip"]').tooltip();

        $('#loader').hide();



        $("#btnsave_cancel_").bind("click", function () {

            raise_cancel_request();

        });

        Getdata();

        //$("#ddlcompany").bind("change", function () {
        //    BindEmployeeCodeFromEmpMasterByComp('ddlemployee', default_company, 0);
        //});

        $("#btnget_detail").bind("click", function () {

            //if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
            //    messageBox("error", "Please select Company");
            //    return false;
            //}

            //if ($("#txtFromDate").val() == "" || $("#txtFromDate").val() == null) {
            //    messageBox("error", "Please select From Date");
            //    return false;
            //}

            //if ($("#txtToDate").val() == "" || $("#txtToDate").val() == null) {
            //    messageBox("error", "Please select To Date");
            //    return false;
            //}

            //if (new Date($("#txtToDate").val()) < new Date($("#txtFromDate").val())) {
            //    messageBox("error", "To Date must be greater than From Date");
            //    return false;
            //}

            //var emp_idd = 0;
            //var for_all_emp = 1;

            //if (emp_idd == 0) {
            //    for_all_emp = 0;
            //    emp_idd = $('#ddlemployee option').last().val();
            //}

            Getdata()


        });


        $("#btnsave").bind("change", function () {
            //if ($("#ddlcompany").val() == undefined || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "") {
            //    messageBox("error", "Please select company");
            //    return false;
            //}
        });

    }, 2000);// end timeout

});

function Getdata() {
    $("#loader").show();

    $('#tblempsepration_dtl').DataTable({
        "processing": true,
        "serverSide": false,
        "bDestroy": true,
        "orderMulti": false,
        "filter": true,
        "scrollX": 200,
        "scrollY": 200,
        ajax: {
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsList",
            //+ $("#txtFromDate").val() + "/" + $("#txtToDate").val() + "/" + $("#ddlcompany").val() + "/" + login_emp_id,//+ for_all_emp + "/" + emp_idd + "/" + is_manager,
            type: 'GET',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            dataSrc: function (json) {
                return json;
            },
            error: function (err) {
                debugger
                $("#loader").hide();
                messageBox("error", err.responseText);
                return false;
            }
        },
        "columnDefs": [
            {
                targets: [5],
                render: function (data, type, row) {

                    var date = new Date(data);
                    return GetDateFormatddMMyyyy(date);
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {

                    var date = new Date(data);
                    return GetDateFormatddMMyyyy(date);
                }
            },
            {
                targets: [8],
                render: function (data, type, row) {

                    var date = new Date(data);
                    return new Date(row.my_approver_dt) < new Date(row.resignation_dt) ? "-" : GetDateFormatddMMyyyy(date);
                }
            }
        ],
        columns: [
            { "data": null, "title": "S.No.", "autoWidth": true },
            { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
            { "data": "emp_name", "title": "Employee Name", "autoWidth": true },
            { "data": "department_name", "title": "Department", "autoWidth": true },
            { "data": "location_name", "title": "Location", "autowidth": true },
            { "data": "date_of_joining", "title": "Joining Date", "autoWidth": true },
            { "data": "resignation_dt", "title": "Resignation Date", "autoWidth": true },
            { "data": "mystatus", "title": "My Status", "autoWidth": true },
            { "data": "my_approver_dt", "title": "Action On", "autoWidth": true },
            { "data": "myremarks", "title": "My Remarks", "autoWidth": true },
            { "data": "final_status", "title": "Final Status", "autoWidth": true },
            {
                "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {

                    return '<a href="#" onclick="ViewDetails(' + full.sepration_id + ',' + full.emp_id + ',1)" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                }
            },
            {
                "title": "Cancel Request ?", "render": function (data, type, full, meta) {
                    return '<a href="#" onclick="CancelRequest(' + full.sepration_id + ',' + full.emp_id + ',0)" data-toggle="tooltip" title="View" ><i class="fas fa-window-close"></i></a>';
                }
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
    });


    $("#loader").hide();
}

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}


function ViewDetails(sep_id, emp_id, view_dtl) {

    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select Company");
        return false;
    }


    if (view_dtl == 1) {
        $("#cancel_submit_div").css("display", "none");
        $("#txt_cncl_remarkss").attr('disabled', 'disabled');
    }
    else {
        $("#cancel_submit_div").css("display", "block");
        $("#txt_cncl_remarkss").removeAttr('disabled');
    }

    $('#loader').show();

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + $("#ddlcompany").val(),
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response.data;
            var _app_history = response._approval_history;

            $("#txtdob").val(GetDateFormatddMMyyyy(new Date(res.dob_)));
            $("#txtdoj").val(GetDateFormatddMMyyyy(new Date(res.date_of_joining)));
            $("#txtdept").val(res.department_name);
            $("#txtdesignation").val(res.designation_name);
            $("#txtgrade").val(res.grade_name);
            $("#txtloc").val(res.location_name);
            var actual_notice = res.diff_notice_days != 0 ? parseInt(res.req_notice_days) - parseInt(res.diff_notice_days) : res.req_notice_days;
            $("#txtnoticeperiod_days").val(res.actual_notice);
            $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notice_").val(res.req_notice_days);
            $("#txt_notice_diff_days").val(res.diff_notice_days);
            $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason").val(res.req_reason);
            $("#txtremarks").val(res.req_remarks);
            $("#txtfinal_working_dt").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));
            $("#txt_cncl_rqstt").val(res.is_cancel);
            $("#txt_cncl_remarkss").val(res.cancel_remarks);
            $("#txtcancel_dtt").val(res.cancelation_dt);


            $("#_approver_dtll").DataTable({
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


function CancelRequest(sep_id, emp_id, view_dtl) {

    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select Company");
        return false;
    }

    $('#loader').show();

    $("#hdnemp_id").val(emp_id);
    $("#hdnreq_id").val(sep_id);


    if (view_dtl == 1) {
        $("#cancel_submit_div").css("display", "none");
        $("#txt_cncl_remarkss").attr('disabled', 'disabled');
    }
    else {
        $("#cancel_submit_div").css("display", "block");
        $("#txt_cncl_remarkss").removeAttr('disabled');
    }

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + $("#ddlcompany").val(),
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response.data;
            var _app_history = response._approval_history;

            $("#txtdob").val(GetDateFormatddMMyyyy(new Date(res.dob_)));
            $("#txtdoj").val(GetDateFormatddMMyyyy(new Date(res.date_of_joining)));
            $("#txtdept").val(res.department_name);
            $("#txtdesignation").val(res.designation_name);
            $("#txtgrade").val(res.grade_name);
            $("#txtloc").val(res.location_name);
            var actual_notice = res.diff_notice_days != 0 ? parseInt(res.req_notice_days) - parseInt(res.diff_notice_days) : res.req_notice_days;
            $("#txtnoticeperiod_days").val(res.actual_notice);
            $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notice_").val(res.req_notice_days);
            $("#txt_notice_diff_days").val(res.diff_notice_days);
            $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason").val(res.req_reason);
            $("#txtremarks").val(res.req_remarks);
            $("#txtfinal_working_dt").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));
            $("#txt_cncl_rqstt").val(res.is_cancel);
            $("#txt_cncl_remarkss").val(res.cancel_remarks);
            $("#txtcancel_dtt").val(res.cancelation_dt);
            $("#hdnresign_dt").val(res.resignation_dt);
            if (res.is_cancel == "Yes" && (res.is_final_approve == "2" || res.is_final_approve == "3")) {
                $("#cancel_submit_div").css("display", "none");
            }
            else {
                $("#cancel_submit_div").css("display", "block");
            }

            $("#_approver_dtll").DataTable({
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

    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select Company");
        return false;
    }

    var cancel_remarks = $("#txt_cncl_remarkss").val();


    var emp_id = $("#hdnemp_id").val();
    var req_id = $("#hdnreq_id").val();
    var resign_dt = $("#hdnresign_dt").val();
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
        messageBox("error", "Something went wrong with request can't raised");
    }

    if (cancel_remarks == "" || cancel_remarks == null) {
        messageBox("error", "Please enter Cancellation Remarks");
        return false;
    }

    req_dtl.push({ 'req_id': req_id, 'emp_id': emp_id, 'remarks': cancel_remarks, 'final_relieve_dt': resign_dt });

    var mydata = {
        company_id: $("#ddlcompany").val(),
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
                    // window.location.href = "/View/EmpSeprationDetail";
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


