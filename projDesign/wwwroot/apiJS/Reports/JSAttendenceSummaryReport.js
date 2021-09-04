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
    var fromDate = $("#dtpFromDt").val();
    var toDate = $("#dtpToDt").val();
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
        'from_date': fromDate,
        'to_date': toDate,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/GetAttendenceSummaryReport';


    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //    debugger;
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }


            $("#tblAttSummReport").DataTable({
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
                        title: 'Attendence Summary Report From : (' + GetDateFormatddMMyyyy(new Date(fromDate)) + ') TO : (' + GetDateFormatddMMyyyy(new Date(toDate)) + ')',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22]
                        }
                    },
                ],
                "aaData": aData,
                "columnDefs":
                    [

                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    //{ "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                    { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department Name", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location Name", "autoWidth": true },
                    { "data": "designation_name", "name": "designation_name", "title": "Designation Name", "autoWidth": true },
                    { "data": "manager_code", "name": "manager_code", "title": "Manager Code", "autoWidth": true },
                    { "data": "manager_name", "name": "manager_name", "title": "Manager Name", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "title": "Date of joining", "autoWidth": true },
                    { "data": "no_of_working_Days", "name": "no_of_working_Days", "title": "No of working days", "autoWidth": true },
                    { "data": "no_of_days_worked", "name": "no_of_days_worked", "title": "No of days worked", "autoWidth": true },
                    { "data": "no_of_Week_off", "name": "no_of_Week_off", "title": "No of Week off", "autoWidth": true },
                    { "data": "no_of_Holidays", "name": "no_of_Holidays", "title": "No of Holidays", "autoWidth": true },
                    { "data": "no_of_leaves_taken", "name": "no_of_leaves_taken", "title": "No of leaves taken", "autoWidth": true },
                    { "data": "no_of_Half_days_leave_Applied", "name": "no_of_Half_days_leave_Applied", "title": "No of Half days leave Applied", "autoWidth": true },
                    { "data": "no_of_days_worked_less_than_8_hours", "name": "no_of_days_worked_less_than_8_hours", "title": "No of days worked less than 8 hours", "autoWidth": true },
                    { "data": "no_of_days_worked_for_more_than_8_hours_but_less_than_9_hours", "name": "no_of_days_worked_for_more_than_8_hours_but_less_than_9_hours", "title": "No of days worked for more than 8 hours but less than 9 hours", "autoWidth": true },
                    { "data": "no_of_day_applied_on_Date_Outdoor", "name": "no_of_day_applied_on_Date_Outdoor", "title": "No of day applied on Date Outdoor", "autoWidth": true },
                    { "data": "no_of_Regularised_Days", "name": "no_of_Regularised_Days", "title": "No of Regularised Days", "autoWidth": true },
                    { "data": "comp_off_Availed", "name": "comp_off_Availed", "title": "Comp off Availed", "autoWidth": true },
                    { "data": "optional_Holiday_Availed", "name": "optional_Holiday_Availed", "title": "Optional Holiday Availed", "autoWidth": true },
                    { "data": "no_Of_absent_day_Unapplied_leaves", "name": "no_Of_absent_day_Unapplied_leaves", "title": "No Of absent day Unapplied leaves", "autoWidth": true },
                    { "data": "average_Working_hours", "name": "average_Working_hours", "title": "Average Working hours", "autoWidth": true }
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
            //         debugger;
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}
