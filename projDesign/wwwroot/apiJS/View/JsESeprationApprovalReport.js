var empid;
var company_id;

$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        GetData();


    }, 2000);// end timeout

});

function GetData() {
    $('#loader').show();

    $.ajax({
        type: "GET",
        // url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetails/" + company_id + "/" + empid ,
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_ApproveRejectEseprationDetail/" + empid,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();

                return false;
            }

            $("#tblempsepration_rpt").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee Separation Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
                        }
                    },
                ],
                "columnDefs": [
                    {
                        targets: [4],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    //{
                    //    targets: [11],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "title": "Date of Joining", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "req_reason", "name": "req_reason", "title": "Request Reason", "autoWidth": true },
                    { "data": "resignation_dt", "name": "resignation_dt", "title": "Resignation Date", "autoWidth": true },
                    { "data": "policy_relieving_dt", "name": "policy_relieving_dt", "title": "Policy Relieving Date", "autoWidth": true },
                    { "data": "final_relieve_dt", "name": "final_relieve_dt", "title": "Final Relieve Date", "autoWidth": true },
                    { "data": "mystatus", "name": "mystatus", "title": "My Status", "autoWidth": true },
                    { "data": "final_status", "name": "final_status", "title": "Final Status", "autoWidth": true },
                    {
                        "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="ViewDetails(' + full.sepration_id + ',' + full.emp_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';
                        }
                    },

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


function ViewDetails(sep_id, emp_id) {

    $('#loader').show();

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + company_id,
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
            $("#txtnoticeperiod_days").val(actual_notice);
            $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notice_").val(res.req_notice_days);
            $("#txt_notice_diff_days").val(res.diff_notice_days);
            $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason").val(res.req_reason);
            $("textarea#txtremarks").val(res.req_remarks);
            $("#txtfinal_working_dt").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));
            $("#txt_is_cncl").val(res.is_cancel);
            $("#txt_cncl_remarks").val(res.cancel_remarks);
            $("#txt_withdrawal").val(res.is_withdrawal);
            $("#txt_cncl_dtl").val(GetDateFormatddMMyyyy(new Date(res.cancelation_dt)));

            $("#tbl_app_histroy").DataTable({
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


