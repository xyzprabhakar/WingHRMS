$('#loader').show();
var default_company;
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //Get Last Shift Id
        GetLastShiftId();
        GetAllShift();



        var qs = getQueryStrings();
        var shift_id = qs["shift_id"];



        // alert(shift_id);
        if (shift_id != null) {
            $('#btnUpdate').show();
            $('#btnSave').hide();

            GetDataByShiftId(shift_id);
        }
        else {

            BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
            //BindCompanyListForddl('ddlcompany', 0);

            $('#ddlcompany').bind("change", function () {
                BindLocationListForddl('ddllocation', $(this).val(), 0);
                BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            });

            $('#btnUpdate').hide();
            $('#btnSave').show();
        }


        $('#loader').hide();



        // Save Shift Master Data
        $('#btnSave').bind("click", function () {

            var ddlcompany = $("#ddlcompany").val();
            var ddllocation = $("#ddllocation").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddlShiftType = $("#ddlShiftType").val();
            var txtShiftName = $("#txtShiftName").val();
            var txtShiftShortName = $("#txtShiftShortName").val();
            var txtshift_code = $("#txtshift_code").val();
            var txtPunchInTime = $("#txtPunchInTime").val();
            var txtpunch_in_max_time = $("#txtpunch_in_max_time").val();
            var txtPunchOutTime = $("#txtPunchOutTime").val();
            var txtWorkingHours = $("#txtWorkingHours").val();
            var txtWorkingMinutes = $("#txtWorkingHours").val();
            var txtgrace_time_for_late_punch_in = $("#txtgrace_time_for_late_punch_in").val();
            var txtgrace_time_for_late_punch_out = $("#txtgrace_time_for_late_punch_out").val();
            var txtnumber_of_grace_time_applicable_in_month = $("#txtnumber_of_grace_time_applicable_in_month").val();

            var chk_is_ot_applicable = 0; // Not applicable
            if ($("#chk_is_ot_applicable").is(":checked")) {
                chk_is_ot_applicable = 1; //applicable
            }

            var chkLunchPunchApplicable = 0; // for lunch punch not applicable 

            if ($("#chkLunchPunchApplicable").is(":checked")) {
                chkLunchPunchApplicable = 1; // 1 for applicable 
            }

            var txtLunchPunchInTime = $("#txtLunchPunchInTime").val();
            var txtLunchPunchOutTime = $("#txtLunchPunchOutTime").val();
            var txtmaximum_lunch_time = $("#txtmaximum_lunch_time").val();

            var chkTeaPunch1 = 0; // 0 for Tea punch 1 not applicable 

            if ($("#chkTeaPunch1").is(":checked")) {
                chkTeaPunch1 = 1; // 1 applicable


            }
            var txtTeaPunchInTime1 = $("#txtTeaPunchInTime1").val();
            var txtTeaPunchOutTime1 = $("#txtTeaPunchOutTime1").val();
            var txtmaximum_tea_time_one = $("#txtmaximum_tea_time_one").val();
            var chkTeaPunch2 = 0; // 0 for Tea punch 2 not applicable 

            if ($("#chkTeaPunch2").is(":checked")) {
                chkTeaPunch2 = 1; // 1 applicable


            }
            var txtTeaPunchInTime2 = $("#txtTeaPunchInTime2").val();
            var txtTeaPunchOutTime2 = $("#txtTeaPunchOutTime2").val();
            var txtmaximum_tea_time_two = $("#txtmaximum_tea_time_two").val();

            var chkIsNightShift = 0; //  0 for not night shift
            if ($("#chkIsNightShift").is(":checked")) {
                chkIsNightShift = 1; // 1 for is night shift
            }

            var txtmark_as_half_day_for_working_hours_less_than = $("#txtmark_as_half_day_for_working_hours_less_than").val();

            var chkWeekOff = 0; //  1 for not night shift
            if ($("#chkWeekOff").is(":checked")) {
                chkWeekOff = 1; // 2 for night shift
            }

            var PunchIgnore = 2; // No Punch Ignore

            if ($("#chkEarlyPunchInIgnore").is(":checked")) {
                PunchIgnore = 0 // 0 for Early punch in ignore
            }
            else if ($("#chkLatePunchOutIgnore").is(":checked")) {
                PunchIgnore = 1  // 1 for Early punch out ignore
            }

            var ShiftLocation = [];

            ShiftLocation.push({ 'location_id': ddllocation, 'company_id': ddlcompany });


            var WeekOffArray = [];
            $.each($("input[name='chkMondayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '1', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkTuesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '2', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkWednesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '3', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkThursdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '4', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkFridayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '5', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkSaturdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '6', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkSundayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '7', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });

            $('#loader').show();
            var myData = {
                'shift_for_all_company': ddlcompany,
                'shift_for_all_location': ddllocation,
                'ShiftLocation': ShiftLocation,
                'department_id': ddldepartment,
                'shift_id': 0,
                'shift_type': ddlShiftType,
                'shift_name': txtShiftName,
                'shift_short_name': txtShiftShortName,
                'punch_in_time': txtPunchInTime,
                'punch_in_max_time': txtpunch_in_max_time,
                'punch_out_time': txtPunchOutTime,
                'maximum_working_hours': HoursOnly(txtWorkingHours),
                'maximum_working_minute': MinutesOnly(txtWorkingMinutes),
                'grace_time_for_late_punch_in': txtgrace_time_for_late_punch_in,
                'grace_time_for_late_punch_out': txtgrace_time_for_late_punch_out,
                'number_of_grace_time_applicable_in_month': txtnumber_of_grace_time_applicable_in_month,
                'is_lunch_punch_applicable': chk_is_ot_applicable,
                'lunch_punch_out_time': txtLunchPunchOutTime,
                'lunch_punch_in_time': txtLunchPunchInTime,
                'maximum_lunch_time': txtmaximum_lunch_time,
                'is_ot_applicable': chk_is_ot_applicable,
                'maximum_ot_hours': txtWorkingHours,
                'tea_punch_applicable_one': chkTeaPunch1,
                'tea_punch_out_time_one': txtTeaPunchInTime1,
                'tea_punch_in_time_one': txtTeaPunchOutTime1,
                'maximum_tea_time_one': txtmaximum_tea_time_one,
                'tea_punch_applicable_two': chkTeaPunch2,
                'tea_punch_out_time_two': txtTeaPunchInTime2,
                'tea_punch_in_time_two': txtTeaPunchOutTime2,
                'maximum_teaid_time_two': txtmaximum_tea_time_two,
                'mark_as_half_day_for_working_hours_less_than': txtmark_as_half_day_for_working_hours_less_than,
                'is_night_shift': chkIsNightShift,
                'is_punch_ignore': PunchIgnore,
                'punch_type': 0,
                'weekly_off': chkWeekOff,
                'ShiftWeekOff': WeekOffArray,
                'created_by': 1,
                'last_modified_by': 1,
            };

            //var myJSON = JSON.stringify(myData);
            // console.log(myJSON);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiShift/',
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        alert(Msg);
                        window.location.href = '/Shift/View';
                    }
                    else if (statuscode == "0") {
                        alert(Msg);
                    }
                },
                error: function (err) {
                    //alert(JSON.stringify(err));
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });

            // alert(myJSON);

        });


        var chkMondayclicked = false;
        $("input[name=chkMonday]").change(function () {
            $("input[name=chkMondayDays1]").prop("checked", !chkMondayclicked);
            chkMondayclicked = !chkMondayclicked;
        });
        var chkTuesdayclicked = false;
        $("input[name=chkTuesday]").change(function () {
            $("input[name=chkTuesdayDays1]").prop("checked", !chkTuesdayclicked);
            chkTuesdayclicked = !chkTuesdayclicked;
        });
        var chkWednesdayclicked = false;
        $("input[name=chkWednesday]").change(function () {
            $("input[name=chkWednesdayDays1]").prop("checked", !chkWednesdayclicked);
            chkWednesdayclicked = !chkWednesdayclicked;
        });
        var chkThursdayclicked = false;
        $("input[name=chkThursday]").change(function () {
            $("input[name=chkThursdayDays1]").prop("checked", !chkThursdayclicked);
            chkThursdayclicked = !chkThursdayclicked;
        });
        var chkFridayclicked = false;
        $("input[name=chkFriday]").change(function () {
            $("input[name=chkFridayDays1]").prop("checked", !chkFridayclicked);
            chkFridayclicked = !chkFridayclicked;
        });
        var chkSaturdayclicked = false;
        $("input[name=chkSaturday]").change(function () {
            $("input[name=chkSaturdayDays1]").prop("checked", !chkSaturdayclicked);
            chkSaturdayclicked = !chkSaturdayclicked;
        });
        var chkSundaydayclicked = false;
        $("input[name=chkSundayday]").change(function () {
            $("input[name=chkSundayDays1]").prop("checked", !chkSundaydayclicked);
            chkSundaydayclicked = !chkSundaydayclicked;
        });

        $('#btnUpdate').bind("click", function () {
            var ddlcompany = $("#ddlcompany").val();
            var ddllocation = $("#ddllocation").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddlShiftType = $("#ddlShiftType").val();
            var txtShiftName = $("#txtShiftName").val();
            var txtShiftShortName = $("#txtShiftShortName").val();
            var txtshift_code = $("#txtshift_code").val();
            var txtPunchInTime = $("#txtPunchInTime").val();
            var txtpunch_in_max_time = $("#txtpunch_in_max_time").val();
            var txtPunchOutTime = $("#txtPunchOutTime").val();
            var txtWorkingHours = $("#txtWorkingHours").val();
            var txtWorkingMinutes = $("#txtWorkingHours").val();
            var txtgrace_time_for_late_punch_in = $("#txtgrace_time_for_late_punch_in").val();
            var txtgrace_time_for_late_punch_out = $("#txtgrace_time_for_late_punch_out").val();
            var txtnumber_of_grace_time_applicable_in_month = $("#txtnumber_of_grace_time_applicable_in_month").val();

            var chk_is_ot_applicable = 0; // Not applicable
            if ($("#chk_is_ot_applicable").is(":checked")) {
                chk_is_ot_applicable = 1; //applicable
            }

            var chkLunchPunchApplicable = 0; // for lunch punch not applicable 

            if ($("#chkLunchPunchApplicable").is(":checked")) {
                chkLunchPunchApplicable = 1; // 1 for applicable 
            }

            var txtLunchPunchInTime = $("#txtLunchPunchInTime").val();
            var txtLunchPunchOutTime = $("#txtLunchPunchOutTime").val();
            var txtmaximum_lunch_time = $("#txtmaximum_lunch_time").val();

            var chkTeaPunch1 = 0; // 0 for Tea punch 1 not applicable 

            if ($("#chkTeaPunch1").is(":checked")) {
                chkTeaPunch1 = 1; // 1 applicable


            }
            var txtTeaPunchInTime1 = $("#txtTeaPunchInTime1").val();
            var txtTeaPunchOutTime1 = $("#txtTeaPunchOutTime1").val();
            var txtmaximum_tea_time_one = $("#txtmaximum_tea_time_one").val();
            var chkTeaPunch2 = 0; // 0 for Tea punch 2 not applicable 

            if ($("#chkTeaPunch2").is(":checked")) {
                chkTeaPunch2 = 1; // 1 applicable


            }
            var txtTeaPunchInTime2 = $("#txtTeaPunchInTime2").val();
            var txtTeaPunchOutTime2 = $("#txtTeaPunchOutTime2").val();
            var txtmaximum_tea_time_two = $("#txtmaximum_tea_time_two").val();
            var chkIsNightShift = 0; //  0 for not night shift
            if ($("#chkIsNightShift").is(":checked")) {
                chkIsNightShift = 1; // 1 for is night shift
            }

            var txtmark_as_half_day_for_working_hours_less_than = $("#txtmark_as_half_day_for_working_hours_less_than").val();

            var chkWeekOff = 0; // 0 for not night shift
            if ($("#chkWeekOff").is(":checked")) {
                chkWeekOff = 1; // 1 for night shift
            }

            var PunchIgnore = 2; // No Punch Ignore

            if ($("#chkEarlyPunchInIgnore").is(":checked")) {
                PunchIgnore = 0 // 0 for Early punch in ignore
            }
            else if ($("#chkLatePunchOutIgnore").is(":checked")) {
                PunchIgnore = 1  // 1 for Early punch out ignore
            }

            var ShiftLocation = [];

            ShiftLocation.push({ 'location_id': ddllocation, 'company_id': ddlcompany });


            var WeekOffArray = [];
            $.each($("input[name='chkMondayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '1', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkTuesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '2', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkWednesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '3', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkThursdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '4', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkFridayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '5', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkSaturdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '6', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });
            $.each($("input[name='chkSundayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '7', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': ddlcompany, 'last_modified_by': ddlcompany });
            });

            $('#loader').show();
            // debugger;
            var myData = {
                'shift_for_all_company': ddlcompany,
                'shift_for_all_location': ddllocation,
                'ShiftLocation': ShiftLocation,
                'department_id': ddldepartment,
                'shift_id': 0,
                'shift_type': ddlShiftType,
                'shift_name': txtShiftName,
                'shift_short_name': txtShiftShortName,
                'punch_in_time': txtPunchInTime,
                'punch_in_max_time': txtpunch_in_max_time,
                'punch_out_time': txtPunchOutTime,
                'maximum_working_hours': HoursOnly(txtWorkingHours),
                'maximum_working_minute': MinutesOnly(txtWorkingMinutes),
                'grace_time_for_late_punch_in': txtgrace_time_for_late_punch_in,
                'grace_time_for_late_punch_out': txtgrace_time_for_late_punch_out,
                'number_of_grace_time_applicable_in_month': txtnumber_of_grace_time_applicable_in_month,
                'is_lunch_punch_applicable': chk_is_ot_applicable,
                'lunch_punch_out_time': txtLunchPunchOutTime,
                'lunch_punch_in_time': txtLunchPunchInTime,
                'maximum_lunch_time': txtmaximum_lunch_time,
                'is_ot_applicable': chk_is_ot_applicable,
                'maximum_ot_hours': txtWorkingHours,
                'tea_punch_applicable_one': chkTeaPunch1,
                'tea_punch_out_time_one': txtTeaPunchInTime1,
                'tea_punch_in_time_one': txtTeaPunchOutTime1,
                'maximum_tea_time_one': txtmaximum_tea_time_one,
                'tea_punch_applicable_two': chkTeaPunch2,
                'tea_punch_out_time_two': txtTeaPunchInTime2,
                'tea_punch_in_time_two': txtTeaPunchOutTime2,
                'maximum_teaid_time_two': txtmaximum_tea_time_two,
                'mark_as_half_day_for_working_hours_less_than': txtmark_as_half_day_for_working_hours_less_than,
                'is_night_shift': chkIsNightShift,
                'is_punch_ignore': PunchIgnore,
                'punch_type': 0,
                'weekly_off': chkWeekOff,
                'ShiftWeekOff': WeekOffArray,
                'created_by': 1,
                'last_modified_by': 1,
            };

            var myJSON = JSON.stringify(myData);
            console.log(myJSON);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiShift/',
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        alert(Msg);
                        window.location.href = '/Shift/View';
                    }
                    else if (statuscode == "0") {
                        alert(Msg);
                    }
                },
                error: function (err) {
                    //alert(JSON.stringify(err));
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });
        });



    }, 2000);// end timeout

});

//Get Last Shify Id
function GetLastShiftId() {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiShift/GetLastShiftId",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        //headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;
            $("#txtshift_code").val('S0000' + (res.shift_id + 1));
            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
}


function GetAllShift() {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiShift",
        type: 'GET',
        dataType: 'json',
        // headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblShiftMaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,

                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
                        {
                            targets: [8],
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
                        }
                        ,
                        {
                            targets: [12],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
                        {
                            targets: [13],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
                        {
                            targets: [15],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "shift_name", "name": "shift_name", "autoWidth": true },
                    { "data": "shift_short_name", "name": "shift_short_name", "autoWidth": true },
                    { "data": "punch_in_time", "name": "punch_in_time", "autoWidth": true },
                    { "data": "punch_in_max_time", "name": "punch_in_max_time", "autoWidth": true },
                    { "data": "punch_out_time", "name": "punch_out_time", "autoWidth": true },
                    { "data": "maximum_working_hours", "name": "maximum_working_hours", "autoWidth": true },
                    { "data": "grace_time_for_late_punch_in", "name": "grace_time_for_late_punch_in", "autoWidth": true },
                    { "data": "grace_time_for_late_punch_out", "name": "grace_time_for_late_punch_out", "autoWidth": true },
                    { "data": "number_of_grace_time_applicable_in_month", "name": "number_of_grace_time_applicable_in_month", "autoWidth": true },
                    { "data": "is_lunch_punch_applicable", "name": "is_lunch_punch_applicable", "autoWidth": true },
                    { "data": "lunch_punch_out_time", "name": "lunch_punch_out_time", "autoWidth": true },
                    { "data": "lunch_punch_in_time", "name": "lunch_punch_in_time", "autoWidth": true },
                    { "data": "maximum_lunch_time", "name": "maximum_lunch_time", "autoWidth": true },
                    { "data": "is_ot_applicable", "name": "is_ot_applicable", "autoWidth": true },
                    { "data": "maximum_ot_hours", "name": "maximum_ot_hours", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="Create?shift_id=' + full.shift_id + '" title="Edit" style=" float: left;"><i class="fa fa-pencil-square-o"></i></a>  <a  onclick="DeleteCompany(' + full.company_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}


function FunctionCallMonday(checkbox) {
    alert();
    var cbl = document.getElementById('chkMondayDays').getElementsByTagName("input");
    for (i = 0; i < cbl.length; i++) cbl[i].checked = checkbox.checked;
}

function FullTime(time) {

    if (time.value !== "") {
        var hours = time.split(":")[0];
        var minutes = time.split(":")[1];
        var suffix = hours >= 12 ? "PM" : "AM";
        hours = hours % 12 || 12;
        hours = hours < 10 ? "0" + hours : hours;

        var displayTime = hours + ":" + minutes + " " + suffix;
        return displayTime
    }
}

function HoursOnly(time) {

    if (time.value !== "") {
        var hours = time.split(":")[0];
        hours = hours % 12 || 12;
        hours = hours < 10 ? "0" + hours : hours;
        return hours
    }
}

function MinutesOnly(time) {

    if (time.value !== "") {

        var minutes = time.split(":")[1];
        return minutes
    }
}





//Edit Company Master
function GetDataByShiftId(shift_id) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiShift/' + shift_id,
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            // debugger;
            var res = data;
            //var a = res.shiftWeekOff;
            //alert(JSON.stringify(res));

            var punch_in_time = new Date(res.punch_in_time);
            //  var formatted = punch_in_time.getHours() + ":" + punch_in_time.getMinutes() + ":" + punch_in_time.getSeconds();
            // alert(formatted);

            // BindCompanyListForddl('ddlcompany', res.shift_for_all_company);
            BindAllEmp_Company('ddlcompany', login_emp_id, res.shift_for_all_company);
            BindLocationListForddl('ddllocation', res.shift_for_all_company, res.shift_for_all_location);
            BindDepartmentListForddl('ddldepartment', res.shift_for_all_company, res.department_id);

            $("#ddlShiftType").val(res.shift_type);
            $("#txtShiftName").val(res.shift_name);
            $("#txtShiftShortName").val(res.shift_short_name);
            $("#txtshift_code").val('S0000' + res.shift_id);

            $("#txtPunchInTime").val(punch_in_time);
            $("#txtpunch_in_max_time").val(res.punch_in_max_time);
            $("#txtPunchOutTime").val(res.punch_out_time);

            $("#txtWorkingHours").val(res.maximum_working_hours + '-' + res.maximum_working_minute);

            $("#txtgrace_time_for_late_punch_in").val(res.grace_time_for_late_punch_in);
            $("#txtgrace_time_for_late_punch_out").val(res.grace_time_for_late_punch_out);
            $("#txtnumber_of_grace_time_applicable_in_month").val(res.number_of_grace_time_applicable_in_month);

            if (res.is_ot_applicable == 1) {
                $("#chk_is_ot_applicable").attr('checked', 'checked');
            }
            if (res.is_lunch_punch_applicable == 1) {
                $("#chkLunchPunchApplicable").attr('checked', 'checked');
            }
            $("#txtLunchPunchInTime").val(res.lunch_punch_in_time);
            $("#txtLunchPunchOutTime").val(res.lunch_punch_out_time);
            $("#txtmaximum_lunch_time").val(res.maximum_lunch_time);

            if (res.tea_punch_applicable_one == 1) {
                $("#chkTeaPunch1").attr('checked', 'checked');
            }
            $("#txtTeaPunchInTime1").val(res.tea_punch_in_time_one);
            $("#txtTeaPunchOutTime1").val(res.tea_punch_out_time_one);
            $("#txtmaximum_tea_time_one").val(res.maximum_tea_time_one);


            if (res.tea_punch_applicable_two == 1) {
                $("#chkTeaPunch2").attr('checked', 'checked');
            }
            $("#txtTeaPunchInTime2").val(res.tea_punch_in_time_two);
            $("#txtTeaPunchOutTime2").val(res.tea_punch_out_time_two);
            $("#txtmaximum_tea_time_two").val(res.maximum_tea_time_two);

            if (res.is_night_shift == 1) {
                $("#chkIsNightShift").attr('checked', 'checked');
            }

            $("#txtmark_as_half_day_for_working_hours_less_than").val(res.mark_as_half_day_for_working_hours_less_than);

            if (res.weekly_off == 1) {
                $("#chkWeekOff").attr('checked', 'checked');
            }

            if (res.is_punch_ignore == 0) {
                $("#chkEarlyPunchInIgnore").attr('checked', 'checked');
            }
            else if (res.is_punch_ignore == 1) {
                $("#chkLatePunchOutIgnore").attr('checked', 'checked');
            }

            // alert(res.ShiftWeekOff);

            //res.ShiftWeekOff.forEach(function (e) {
            //    alert(e.week_day);
            //});

            $.each(res.shiftWeekOff, function (key, value) {


                //alert(value.week_day + ' , ' + value.days);
                if (value.week_day == 1) {
                    $("input[name=chkMondayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 2) {
                    $("input[name=chkTuesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 3) {
                    $("input[name=chkWednesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 4) {
                    $("input[name=chkThursdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 5) {
                    $("input[name=chkFridayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 6) {
                    $("input[name=chkSaturdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 7) {
                    $("input[name=chkSundayDays" + value.days + "]").prop("checked", true);
                }

            });

            // var res1 = data.ShiftWeekOff;
            // alert(data.ShiftWeekOff);
            //$.each(res.shiftWeekOff, function (key, value) {
            //    alert(key + ": " + value);
            //});

            $('#loader').hide();

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });


}



