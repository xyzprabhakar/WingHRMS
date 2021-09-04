$('#loader').show();
var login_company_id;
var login_role_id;
var login_emp_id;
var arr = new Array("1", "101", "105");

$(document).ready(function () {
    var token = localStorage.getItem('Token');

    if (token == null) {
        window.location = '/Login';
    }

    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    if (!arr.includes(login_role_id)) {
        $("#btnAddMannualLeave").hide();
    }

    $("#btnupdate").hide();

    $('#loader').hide();

    $("#btnget_detail").bind("click", function () {

        GetData_LeaveLedger();
    });


    $('#ddl_year').bind("change", function () {
        GetData_LeaveLedger();
    });
});
function GetData_LeaveLedger() {
    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null) {
        messageBox("error", "Please select Company");
        return false;
    }

    if ($("#ddllocation").val() == "" || $("#ddllocation").val() == null) {
        messageBox("error", "Please select Location");
        return false;
    }

    if ($("#ddldepartment").val() == "" || $("#ddldepartment").val() == null) {
        messageBox("error", "Please select Department");
        return false;
    }

    var emp_id = $("#ddlemployee").val();

    if (emp_id == null || emp_id == '') {
        messageBox('info', 'Please select to employee!!');
        return false;
    }

    var BulkEmpID = [];
    var for_all_emp = 0;

    if (emp_id == -1) {
        for_all_emp = 1;
        var options_ = $("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return $(this).val(); }).get();

        BulkEmpID = options_;

    }
    else {
        BulkEmpID.push(emp_id);
    }
    //var txtMonthYear = $("#txtMonthYear").val();

    //if (txtMonthYear == null || txtMonthYear == '') {
    //    $('#loader').hide();
    //    messageBox('info', 'Please select Year...!');
    //    return false;
    //}
    //else {
    //    var txtmnyr = new Date($('#txtMonthYear').val());
    //    var yr = txtmnyr.getFullYear();
    //    var mnth = txtmnyr.getMonth() + 1;
    //    txtMonthYear = String(yr) + String(mnth);
    //}

    $("#loader").show();


    var myData = {
        'company_id': $("#ddlcompany").val(),
        'department_id': $("#ddldepartment").val(),
        'location_id': $("#ddllocation").val(),
        'emp_ids': BulkEmpID,
        'year': $("#ddl_year option:selected").val(),
    }


    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    //$('#tblmannualleavedtl').DataTable({
    //    "processing": true,
    //    "serverSide": false,
    //    "bDestroy": true,
    //    "orderMulti": false,
    //    "filter": true,
    //    "scrollX": 200,
    //    dom: 'lBfrtip',
    //    buttons: [
    //        {
    //            text: 'Export to Excel',
    //            title: 'Manual Leave Report',
    //            extend: 'excelHtml5',
    //            exportOptions: {
    //                columns: [1, 2, 3, 4, 5]
    //            }
    //        },

    //    ],
    //    ajax: {
    //        //url: localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveLedgerByCLD/" + $("#ddlcompany").val() + "/" + $("#ddldepartment").val() + "/" + $("#ddllocation").val() + "/" + BulkEmpID,
    //        type: "POST",
    //        url: localStorage.getItem("ApiUrl") + '/apiLeave/GetLeaveLedgerByCLD',
    //        data: Obj,
    //        dataType: "json",
    //        contentType: 'application/json; charset=utf-8',
    //        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
    //        dataSrc: function (json) {
    //            $("#loader").hide();
    //            return json;
    //        },
    //        error: function (err) {
    //            $("#loader").hide();
    //            messageBox("error", err.responseText);
    //            return false;
    //        }
    //    },
    //    "columnDefs": [],
    //    columns: [
    //        { "data": null, "title": "SNo.", "autowidth": true },
    //        { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autowidth": true },
    //        {
    //            "title": "Leave Type", "autowidth": true, "render": function (data, type, full, meta) {
    //                return '<a href="#" onClick=GetLeaveDetails(' + full.company_id + ',' + full.leave_type_id + ',' + full.e_id + ')>' + full.leave_type_name + '</a>'
    //            }
    //        },
    //        { "data": "credit", "name": "credit", "title": "Total Credit", "autowidth": true },
    //        { "data": "dredit", "name": "dredit", "title": "Total Debit", "autowidth": true },
    //        { "data": "balance", "name": "balance", "title": "Leave Balance", "autowidth": true },
    //    ],
    //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
    //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
    //        return nRow;
    //    },
    //    "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
    //});


    $.ajax({
        type: "POST",
        url: localStorage.getItem("ApiUrl") + '/apiLeave/GetLeaveLedgerByCLD',
        data: Obj,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();

            var ret_data = res;

            $("#tblmannualleavedtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                dom: 'lBfrtip',
                "aaData": res,
                "columnDefs": [],
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Manual Leave Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6]
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autowidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autowidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autowidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autowidth": true },
                    {
                        "title": "Leave Type", "autowidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onClick=GetLeaveDetails(' + full.company_id + ',' + full.leave_type_id + ',' + full.e_id + ')>' + full.leave_type_name + '</a>'
                        }
                    },
                    //{ "data": "credit", "name": "credit", "title": "Total Credit", "autowidth": true },
                    // { "data": "dredit", "name": "dredit", "title": "Total Debit", "autowidth": true },
                    { "data": "balance", "name": "balance", "title": "Leave Balance", "autowidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);

        }
    });

}
function BindLeaveLedger() {
    var apiurll = "";

    apiurll = localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveLedger";

    $.ajax({
        url: apiurll,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblmannualleavedtl").DataTable({
                "processing": true,//for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true,
                "filter": true, // this is for diable filter(search box)
                "orderMulti": false, // for diable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Detail',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5]
                        }
                    },
                ],
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autowidth": true },
                    {
                        "title": "Leave Type", "autowidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onClick=GetLeaveDetails(' + full.company_id + ',' + full.leave_type_id + ',' + full.e_id + ')>' + full.leave_type_name + '</a>'
                        }
                    },
                    { "data": "credit", "name": "credit", "title": "Total Credit", "autowidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Total Debit", "autowidth": true },
                    { "data": "balance", "name": "balance", "title": "Leave Balance", "autowidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]

            });


        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function GetLeaveDetails(company_idd, leve_type_id, emp_id) {

    $('#loader').show();

    $("#myLeaveModal").show();
    var modal = document.getElementById("myLeaveModal");
    modal.style.display = "block";

    $('#myLeaveModal').dialog({
        modal: 'true',
        title: 'Leave Detail Month Wise'
    });
    var year = $("#ddl_year option:selected").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiLeave/GetLeaveLedgerByCompID/" + company_idd + "/" + leve_type_id + "/" + emp_id + "/" + year,
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

            $("#tbl_leavetype_dtl").DataTable({
                "processing": true,//for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true,
                "filter": true, // this is for diable filter(search box)
                "orderMulti": false, // for diable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Type Detail',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    },
                ],
                //"columnDefs": [
                //    {
                //        targets: [3],
                //        render: function (data, type, row) {

                //            var date = new Date(data);
                //            return GetDateFormatddMMyyyy(date);
                //        }
                //    },
                //],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    //{ "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    //{ "data": "department_name", "name": "department_name", "title": "Department Name", "autoWidth": true },
                    //{ "data": "location_name", "name": "location_name", "title": "Work Location Name", "autoWidth": true },
                    //{ "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    {
                        "title": "Leave Type", "autowidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onClick=GetLeaveDetails_monthwise(' + full.company_id + ',' + full.leave_type_id + ',' + full.e_id + ',' + full.monthyear + ')>' + full.transactionmonth + '</a>'
                        }
                    },
                    //{ "data": "transactionmonth", "name": "transactionmonth", "title": "Month", "autoWidth": true },
                    { "data": "openingbalance", "name": "openingbalance", "title": "Leave Opening Balance ", "autoWidth": true },
                    { "data": "credit", "name": "credit", "title": "Leave Credit", "autoWidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Leave Avail", "autoWidth": true },
                    { "data": "balance", "name": "balance", "title": "Leave Balance", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
            });

            $('#loader').hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}
function GetLeaveDetails_monthwise(company_id, leve_type_id, emp_id,monthyear) {

    $('#loader').show();

    $("#myLeaveModal_monthwise").show();
    var modal = document.getElementById("myLeaveModal_monthwise");
    modal.style.display = "block";

    $('#myLeaveModal_monthwise').dialog({
        modal: 'true',
        title: 'Leave Detail'
    });
    //var year = $("#ddl_year option:selected").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiLeave/GetLeaveDetails_monthwise/" + company_id + "/" + leve_type_id + "/" + emp_id + "/" + monthyear,
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

            $("#tbl_leave_details_monthwise").DataTable({
                "processing": true,//for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true,
                "filter": true, // this is for diable filter(search box)
                "orderMulti": false, // for diable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Type Detail',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    },
                ],
                //"columnDefs": [
                //    {
                //        targets: [3],
                //        render: function (data, type, row) {

                //            var date = new Date(data);
                //            return GetDateFormatddMMyyyy(date);
                //        }
                //    },
                //],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    //{ "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                    //{ "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    { "data": "transaction_date", "name": "transaction_date", "title": "Transaction Date", "autoWidth": true },
                    { "data": "transaction_type_name", "name": "transaction_type_name", "title": "Transaction Type", "autoWidth": true },
                   // { "data": "monthyear", "name": "monthyear", "title": "MonthYear", "autoWidth": true },
                    { "data": "leave_addition_type_name", "name": "leave_addition_type_name", "title": "Leave Addition ", "autoWidth": true },
                    { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
            });

            $('#loader').hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}
function GetLeaveDetails_old(company_idd, leve_type_id, emp_id) {

    $('#loader').show();

    $("#myLeaveModal").show();
    var modal = document.getElementById("myLeaveModal");
    modal.style.display = "block";

    $('#myLeaveModal').dialog({
        modal: 'true',
        title: 'Leave Detail'
    });
    var year = $("#ddl_year option:selected").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiLeave/GetLeaveLedgerByCompID/" + company_idd + "/" + leve_type_id + "/" + emp_id + "/" + year,
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

            $("#tbl_leavetype_dtl").DataTable({
                "processing": true,//for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true,
                "filter": true, // this is for diable filter(search box)
                "orderMulti": false, // for diable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Type Detail',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    },
                ],
                "columnDefs": [
                    {
                        targets: [3],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    { "data": "transaction_date", "name": "transaction_date", "title": "Transaction Date", "autoWidth": true },
                    { "data": "transaction_type_name", "name": "transaction_type_name", "title": "Transaction Type", "autoWidth": true },
                    { "data": "monthyear", "name": "monthyear", "title": "MonthYear", "autoWidth": true },
                    { "data": "leave_addition_type_name", "name": "leave_addition_type_name", "title": "Leave Addition ", "autoWidth": true },
                    { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
            });

            $('#loader').hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

