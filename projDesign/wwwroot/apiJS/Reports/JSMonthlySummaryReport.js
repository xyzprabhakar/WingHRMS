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


        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            GetData();
        });

    }, 2000);// end timeout


});


function GetData() {
    $('#loader').show();
    var emp_id = $("#ddlemployee").val();

    var dtpFromDt = $("#dtpFromDt").val();
    var dtpToDt = $("#dtpToDt").val();

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

    if (companyid == null || companyid == '' || companyid == 0) {
        messageBox('info', 'Please select company...!');
        return false;
    }
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


    var mydata = {
        'empdtl': BulkEmpID,
        'all_emp': for_all_emp,
        'from_date': dtpFromDt,
        'to_date': dtpToDt,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/GetMonthlySummaryReport';

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

            $('#loader').show();

            $("#tblleaveapp").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee Monthly Summary Report - ' + GetDateFormatddMMyyyy(new Date(dtpFromDt)) + ' To ' + GetDateFormatddMMyyyy(new Date(dtpToDt)),
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
                        }
                    },
                ],
                "aaData": aData,
                "columnDefs":
                    [

                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "company_name", "name": "leave_type_name", "title": "Company Name", "autoWidth": true },
                    { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "designation_name", "name": "designation_name", "title": "Designation", "autoWidth": true },
                    { "data": "present_count", "name": "present_count", "title": "Present Count", "autoWidth": true },
                    { "data": "absent_count", "name": "absent_count", "title": "Absent Count", "autoWidth": true },
                    { "data": "leave_count", "name": "leave_count", "title": "Leave Count", "autoWidth": true },
                    { "data": "unpaid_leave", "name": "unpaid_leave", "title": "Unpaid Leave", "autoWidth": true },
                    { "data": "total", "name": "total", "title": "Total", "autoWidth": true },
                    { "data": "weekly_off_count", "name": "weekly_off_count", "title": "Weekly Off Count", "autoWidth": true },
                    { "data": "comp_off_count", "name": "comp_off_count", "title": "CompOff Count", "autoWidth": true },
                    { "data": "holidays", "name": "holidays", "title": "Holidays", "autoWidth": true },
                    { "data": "total_working_days", "name": "total_working_days", "title": "Total Working Days", "autoWidth": true },
                    { "data": "total_days", "name": "total_days", "title": "Total Days", "autoWidth": true },

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
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}
