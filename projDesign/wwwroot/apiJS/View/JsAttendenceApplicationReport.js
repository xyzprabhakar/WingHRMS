$('#loader').show();
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //BindAllEmp_Company('ddlCompany', login_emp_id, company_id);

        //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, -1);

        //setSelect('ddlEmployeeCode', login_emp_id);

        //$('#ddlEmployeeCode option[value="' + data.login_emp_id + '"]').attr("selected", "selected");
        //$('#ddllocation_sub_loc').trigger("select2:updated");
        //$('#ddllocation_sub_loc').select2();

        //HaveDisplay = ISDisplayMenu("Is Company Admin")

        //if (HaveDisplay == 1) {
        //    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, 0);
        //}
        //else {
        //    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, login_emp_id);
        //}

        //var HaveDisplay = ISDisplayMenu("Is Company Admin");

        //if (HaveDisplay == 1) {
        //    //var HaveDisplayCompList = ISDisplayMenu("Display Company List");
        //    //if (HaveDisplayCompList == 0) {
        //    //    $('#ddlCompany').prop("disabled", true);
        //    //}
        //    //else {
        //    //    $('#ddlCompany').prop("disabled", false);
        //    //}
        //    BindAllEmp_Company('ddlCompany', login_emp_id, company_id);
        //    BindAllEmployeeByCompany('ddlEmployeeCode', company_id, 0);
        //}
        //else {
        //    $('#ddlCompany').prop("disabled", false);
        //    BindAllEmp_Company('ddlCompany', login_emp_id, company_id);
        //    BindEmployeeList1('ddlEmployeeCode', login_emp_id);
        //}

        //$('#ddlCompany').change(function () {
        //    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, -1);
        //    setSelect('ddlEmployeeCode', login_emp_id);

        //    $("#txtFromDate").val('');
        //    $("#txtToDate").val('');
        //    if ($.fn.DataTable.isDataTable('#tblAttendanceApplication')) {
        //        $('#tblAttendanceApplication').DataTable().clear().draw();
        //    }
        //   // BindAllEmployeeByCompany('ddlEmployeeCode', $(this).val(), 0);
        //});



        $('#btnsave').show();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {

            location.reload();

        });


        $('#btnsave').bind("click", function () {
            if ($.fn.DataTable.isDataTable('#tblAttendanceApplication')) {
                $('#tblAttendanceApplication').DataTable().clear().draw();
            }
            GetData();
        });


    }, 2000);// end timeout

});



function GetData() {

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

    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AttendanceApplicationRequest/';
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: headerss,
        success: function (res) {
            //  debugger;
            _GUID_New();

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }
            $("#tblAttendanceApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Attendance Application Report From : (' + GetDateFormatddMMyyyy(new Date(from_date)) + ') TO : (' + GetDateFormatddMMyyyy(new Date(to_date)) + ')',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]
                        }
                    },
                ],
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [21],
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
                        {
                            targets: [20],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [11],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [14],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [15],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [10],
                            render: $.fn.dataTable.render.number(',', '.', 2)
                        },
                        {
                            targets: [11],
                            render: $.fn.dataTable.render.number(',', '.', 2)
                        },
                        {
                            targets: [12],
                            render: $.fn.dataTable.render.number(',', '.', 2)
                        },
                        {
                            targets: [16],
                            render: $.fn.dataTable.render.number(',', '.', 2)
                        }

                    ],

                "columns": [
                    { "title": "S.No.", "data": null },
                    { "title": "Employee Code", "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    { "title": "Employee Name", "data": "emp_name", "name": "emp_name", "autoWidth": true },
                    { "title": "Department", "data": "dept_name", "name": "dept_name", "autoWidth": true },
                    { "title": "Designation", "data": "designation", "name": "designation", "autoWidth": true },
                    { "title": "Location", "data": "location", "name": "location", "autoWidth": true },
                    { "title": "Date", "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "title": "Shift Name", "data": "shift_Name", "name": "shift_Name", "autoWidth": true },
                    { "title": "Shift In Time", "data": "shift_in_time", "name": "shift_in_time", "autoWidth": true },
                    { "title": "Shift Out Time", "data": "shift_out_time", "name": "shift_out_time", "autoWidth": true },
                    { "title": "Actual In Time", "data": "actual_in_time", "name": "actual_in_time", "autoWidth": true },
                    { "title": "Actual Out Time", "data": "actual_out_time", "name": "actual_out_time", "autoWidth": true },
                    { "title": "Actual Working Hours", "data": "actual_working_hour", "name": "actual_working_hour", "autoWidth": true },
                    { "title": "Actual Status", "data": "actual_status", "name": "actual_status", "autoWidth": true },
                    { "title": "Regularized In Time", "data": "in_time", "name": "in_time", "autoWidth": true },
                    { "title": "Regularized Out Time", "data": "out_time", "name": "out_time", "autoWidth": true },
                    { "title": "Regularized Worked Hours", "data": "worked_hour", "name": "worked_hour", "autoWidth": true },
                    { "title": "New Status", "data": "new_status", "name": "new_status", "autoWidth": true },
                    { "title": "Requestor Remarks", "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "title": "Application Status", "data": "status", "name": "status", "autoWidth": true },
                    { "title": "Application Date", "data": "applied_on", "name": "applied_on", "autoWidth": true },
                    { "title": "Approval Date", "data": "approved_on", "name": "approved_on", "autoWidth": true },
                    { "title": "Approved By", "data": "approved_by", "name": "approved_by", "autoWidth": true },
                    { "title": "Approver Remarks", "data": "approver_remarks", "name": "approver_remarks", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                    //        return (full.is_delted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            $('#loader').hide();
            alert(error.responseText);
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
        'r_e_id': empidd //emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteAttendanceLeaveApplication';
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

