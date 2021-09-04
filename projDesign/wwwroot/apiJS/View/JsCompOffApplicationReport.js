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


        $('#btnsave').show();


        $('#loader').hide();

        $('#btnsave').bind("click", function () {
            if ($.fn.DataTable.isDataTable('#tblCompOffApplication')) {
                $('#tblCompOffApplication').DataTable().clear().draw();
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


    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompOffApplicationRequest';

    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            _GUID_New();


            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }
            $("#tblCompOffApplication").DataTable({
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
                        title: 'CompOff Application Report From : (' + GetDateFormatddMMyyyy(new Date(from_date)) + ') TO : (' + GetDateFormatddMMyyyy(new Date(to_date)) + ')',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                        }
                    },
                ],
                "aaData": res,
                "columnDefs":
                    [

                        //{
                        //    targets: [6],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
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
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [13],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "title": "S.No", "data": null },
                    { "title": "Employee Code", "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    { "title": "Employee Name", "data": "emp_name", "name": "emp_name", "autoWidth": true },
                    { "title": "Branch Name", "data": "location", "name": "location", "autoWidth": true },
                    { "title": "Department", "data": "dept_name", "name": "dept_name", "autoWidth": true },
                    { "title": "Designation", "data": "designation", "name": "designation", "autoWidth": true },
                    { "title": "Compoff Date", "data": "compoff_date", "name": "compoff_date", "autoWidth": true },
                    { "title": "Compoff Day", "data": "compoff_day", "name": "compoff_day", "autoWidth": true },
                    { "title": "In Time", "data": "compoff_date_in", "name": "compoff_date_in", "autoWidth": true },
                    { "title": "Out Time", "data": "compoff_date_out", "name": "compoff_date_out", "autoWidth": true },
                    //        { "data": "duration", "name": "duration", "title": "Duration", "autoWidth": true },
                    { "title": "Application Against Date", "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "title": "Status", "data": "status", "name": "status", "autoWidth": true },
                    { "title": "Remarks", "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "title": "Application Date", "data": "applied_on", "name": "applied_on", "autoWidth": true },
                    { "title": "Approved By", "data": "approved_by", "name": "approved_by", "autoWidth": true },
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //},
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
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteCompOffLeaveApplication';
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
                messageBox("success", msg);
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