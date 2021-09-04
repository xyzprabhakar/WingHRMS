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

        //BindAllEmp_Company('ddlCompany', login_emp_id, company_id);

        //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, -1);

        //setSelect('ddlEmployeeCode', login_emp_id);

        // HaveDisplay = ISDisplayMenu("Is Company Admin");

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
        //    //$('#ddlCompany').prop("disabled", false);
        //    BindAllEmp_Company('ddlCompany', login_emp_id, company_id);
        //    BindEmployeeList1('ddlEmployeeCode', login_emp_id);
        //}

        //$('#ddlCompany').change(function () {
        //    // BindEmployeeList1('ddlEmployeeCode', login_emp_id); 
        //    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, -1);

        //    setSelect('ddlEmployeeCode', login_emp_id);

        //    $("#txtFromDate").val('');
        //    $("#txtToDate").val('');

        //});


        //BinddEmployeeCodee('ddlEmployeeCode', company_id, login_emp_id);

        //GetData(login_emp_id);
        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            location.reload();
        });

        $('#btnsave').bind("click", function () {
            if ($.fn.DataTable.isDataTable('#tblOutdoorApplication')) {
                $('#tblOutdoorApplication').DataTable().clear().draw();
            }
            GetData();
        });


    }, 2000);// end timeout

});




function GetData() {
    debugger;
    var from_date = $("#dtpFromDt").val();

    var to_date = $("#dtpToDt").val();

    var emp_id = $("#ddlemployee").val();

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;


    var for_all_emp = 1;

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
        //$("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return $(this).val(); }).get();

    }
    else {
        BulkEmpID.push(emp_id);
    }




    var mydata = {
        'empdtl': BulkEmpID,
        'from_date': from_date,
        'to_date': to_date,
        'all_emp': for_all_emp,
        'is_ar_dtl': '0',
    }


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_OutdoorApplicationRequest';


    $('#loader').show();
    $.ajax({
        url: apiurl,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            _GUID_New();
            $('#loader').hide();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tblOutdoorApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Outdoor Application Report From : (' + GetDateFormatddMMyyyy(new Date(from_date)) + ') TO : (' + GetDateFormatddMMyyyy(new Date(to_date)) + ')',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
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
                            targets: [12],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee", "autoWidth": true },
                    { "data": "dept_Name", "name": "dept_Name", "title": "Department Name", "autoWidth": true },
                    { "data": "designation", "name": "designation", "title": "Designation ", "autoWidth": true },
                    { "data": "location", "name": "location", "title": "Location", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "Date", "autoWidth": true },
                    { "data": "day_type", "name": "day_type", "title": "Day Type", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "title": "In Time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "title": "Out Time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Application Date", "autoWidth": true },
                    { "data": "approved_by", "name": "approved_by", "title": "Approved By", "autoWidth": true },
                    { "data": "approver_remarks", "name": "approver_remarks", "title": "Approver Remarks", "autoWidth": true },
                    { "data": "isauto_approval", "name": "isauto_approval", "title": "Is Auto Approval", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                    //        return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //},
                    //{
                    //    "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {

                    //        return '<a href="#" onclick="ViewReqDetails(' + full.leave_request_id + ',' + full.requester_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                    //    }
                    //},
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
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
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteOutdoorleaveApplication';
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



function ViewReqDetails(requestid_, empid_) {

    $.ajax({
        url: "apiMasters/",
        type: "GET",
        dataType: "application.json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $("#loader").hide();


        },
        error: function (err) {
            $("#loader").hide();
            messageBox(err.responseText);
            return false;
        }

    });
}
