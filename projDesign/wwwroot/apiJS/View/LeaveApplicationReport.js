$('#loader').show();
var company_id;
var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {



        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#ddlapplicable').bind('change', function () {

            if ($(this).val() == '3') {
                $('#Divhhmm').show();
            }
            else {
                $('#Divhhmm').hide();
            }
            if ($(this).val() == '2') {
                $('#DivDayPart').show();
            }
            else {
                $('#DivDayPart').hide();

            }
        });

        $('#loader').hide();

        $('#ddlcompany').bind('change', function () {
            if ($.fn.DataTable.isDataTable('#tblleaveapp')) {
                $('#tblleaveapp').DataTable().clear().draw();
            }
        });

        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            GetData();
        });


    }, 2000);// end timeout

});




function GetData() {
    $('#tblleaveapp').show();
    var from_date = $("#dtpFromDt").val();

    var to_date = $("#dtpToDt").val();

    var emp_id = $("#ddlemployee").val();

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

    if (companyid == null || companyid == '' || companyid == 0) {
        messageBox('info', 'Please select company...!');
        return false;
    }

    if (from_date == null || from_date == '') {
        messageBox('info', 'Please select from date...!');
        return false;
    }
    if (to_date == null || to_date == '') {
        messageBox('info', 'Please select to date...!');
        return false;
    }
    if (emp_id == null || emp_id == '') {
        messageBox('info', 'Please select to employee!!');
        return false;
    }


    if (new Date(to_date) < new Date(from_date)) {
        messageBox('info', 'To Date must be greater than from date');
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


    var mydata = {
        'empdtl': BulkEmpID,
        'from_date': from_date,
        'to_date': to_date,
        'all_emp': for_all_emp,
        'company_id': $("#ddlcompany").val(),
        'department_id': $("#ddldepartment").val(),
        'location_id': $("#ddllocation").val(),
    }

   
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationByEmp';
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }
            //var aData = res["data"];
            //var lfor = res["leavefor"];
            //var leavefor = [];

            //leavefor.push(lfor);

            $("#tblleaveapp").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "scrollX": 800,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Application Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                        }
                    },
                ],
                "aaData": aData,
                "columnDefs":
                    [
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        let a = GetTimeFromDate(date);
                        //        return a == '0:00' ? '' : a;
                        //    }
                        //},
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                if (data != null && data != 0) {
                                    return data == '1' ? 'First Half' : 'Second Half';
                                }
                                else {
                                    return '';
                                }
                            }

                        },

                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [12],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [15],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                var d = GetDateFormatddMMyyyy(date);
                                if (d == '01/Jan/1') {
                                    return "";
                                }
                                else
                                    return d;
                            }
                        },
                        //{
                        //    targets: [10],
                        //    render: function (data, type, row) {

                        //        return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                        //    }
                        //},
                        //{
                        //    targets: [10, 11],
                        //    "class": "text-center",

                        //}
                        //{
                        //    targets: [10, 11, 12],
                        //    "class": "text-center",

                        //}
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "title": "Employee Code", "data": "emp_code", "name": "emp_code", "autoWidth": true, },
                    { "title": "Employee Name", "data": "emp_name", "name": "emp_name", "autoWidth": true, },
                    { "data": "location", "name": "location", "title": "Location", "autoWidth": true },
                    { "data": "designation", "name": "designation", "title": "Designation ", "autoWidth": true },
                    { "data": "dept_Name", "name": "dept_Name", "title": "Department", "autoWidth": true },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "leave_applicable_for", "name": "leave_applicable_for", "title": "Duration", "autoWidth": true, },
                    //{ "data": "leave_applicable_in_hours_and_minutes", "name": "leave_applicable_in_hours_and_minutes", "autoWidth": true },
                    { "data": "day_part", "name": "day_part", "title": "Day Part", "autoWidth": true },
                    { "data": "leave_qty", "name": "leave_qty", "title": "Leave Qty.", "autoWidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Applied On", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "status_", "name": "status_", "title": "Status", "autoWidth": true },
                    { "data": "approved_on", "name": "approved_on", "title": "Approved on", "autoWidth": true },
                    { "data": "approved_by", "name": "approved_by", "title": "Approved by", "autoWidth": true },
                    { "data": "approver_remarks", "name": "approver_remarks", "title": "Approver Remarks", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                    //        return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.r_e_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //},

                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                //"language": {
                //    "processing": "<i class='fa fa-refresh fa-spin'></i>",
                //}
            });
            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function DeleteLeave(request_id, empidd) {
    $("#loader").show();


    //if (is_manager == 1) {
    //    emp_id = $('#ddlempcodee option').last().val();
    //}
    //else {
    //    for_all_emp = 0;
    //    emp_id = login_emp_id;
    //}

    // emp_id = login_emp_id;

    var myData = {
        'leave_request_id': request_id,
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteLeaveApplication';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: Obj,
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                GetData();

                $("#loader").hide();
                if (statuscode == "0") {
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("info", msg);
                    return false;
                }
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}
