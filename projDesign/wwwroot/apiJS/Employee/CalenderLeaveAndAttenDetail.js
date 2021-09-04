
$(document).ready(function () {
    GetCalenderEmpLeaveAndAttendanceDetails();
});

function GetCalenderEmpLeaveAndAttendanceDetails() {
    //// debugger;
    var CurrentDate = new Date();
    CurrentDate = (CurrentDate.getMonth() + 1) + "-" + CurrentDate.getDate() + "-" + CurrentDate.getFullYear();

    var CalenderDate = '12-25-2018';
    var EmployeeId = '1';

    var apiurl = localStorage.getItem("ApiUrl") + 'Attendance/GetEmployeeMonthalyAtten/' + CalenderDate + "/" + EmployeeId;


    $.ajax({
        url: apiurl,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            //  // debugger;

        },
        error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}