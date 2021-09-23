$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        ///////////////////////

        $('#divPunchInMaxTime').hide();
        $('#divPunchInTime').hide();
        $('#divWorkingHours').hide();
        $('#divPunchOutTime').hide();

        //////////////////////

        //Get Shift Id 
        GetLastShiftId();


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlDefaultCompany', login_emp_id, default_company);
        BindLocationListForddl('ddllocation', default_company, 0);
        BindDepartmentListForddl('ddldepartment', default_company, 0);

        GetAllShift();
        //
        var qs = getQueryStrings();
        var shift_id = qs["shift_id"];
        var emp_role_idd = login_role_id;


        if (shift_id != null) {
            $('#btnUpdate').show();
            $('#btnSave').hide();

            GetDataByShiftId(shift_id);
        }
        else {

            $('#btnUpdate').hide();
            $('#btnSave').show();
        }




        //On Company Change bind Location and Dept
        $('#ddlcompany').bind("change", function () {
            BindLocationListForddl('ddllocation', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
        });

        $('.lunchtime').css('display', 'none');
        $('.teaTime1').css('display', 'none');
        $('.teaTime2').css('display', 'none');
        $('.dyanamicTableToggle').css('display', 'none');
        //On Shift Type cjhange
        $('#ddlShiftType').bind("change", function () {

            var ddlShiftType = $("#ddlShiftType").val();

            if (ddlShiftType == 1) {
                $('.PunchBox').css('display', 'block');
                $('#divPunchInMaxTime').hide();
                $('#divPunchInTime').show();
                $('#divWorkingHours').show();
                $('#divPunchOutTime').show();
            }
            else if (ddlShiftType == 2) {
                $('.PunchBox').css('display', 'block');
                $('#divPunchInMaxTime').show();
                $('#divPunchInTime').show();
                $('#divWorkingHours').show();
                $('#divPunchOutTime').show();
            }
            else {
                $('#divPunchInMaxTime').hide();
                $('#divPunchInTime').hide();
                $('#divWorkingHours').hide();
                $('#divPunchOutTime').hide();
                $('.PunchBox').css('display', 'none');
            }

        });

        $('#txtWorkingHours').bind("change", function () {
            GetPunchOutTime();
        });

        $("a[name='RefTime']").bind("click", function (e) {
            GetPunchOutTime();
            e.preventDefault();//prevernt to referece full page
        });


        $('#loader').hide();

        $("#txtPunchInTime").bind("change", function () {
            $("#txtWorkingHours").val('');
            $("#txtPunchOutTime").val('');
        });

        $('#btnSave').bind("click", function () {

            //Validation


            if (Validation() != false) {

                var ddlcompany = 0;// $("#ddlcompany").val();
                var ddllocation = 0; //$("#ddllocation").val();
                var ddldepartment = 0; //$("#ddldepartment").val();
                var ddlShiftType = $("#ddlShiftType").val();
                var txtShiftName = $("#txtShiftName").val();
                var txtShiftShortName = $("#txtShiftShortName").val();
                var txtPunchInTime = '2000-01-01 ' + $("#txtPunchInTime").val();

                var txtpunch_in_max_time = '2000-01-01 ' + $("#txtpunch_in_max_time").val();
                var txtPunchOutTime = '2000-01-01 ' + $("#txtPunchOutTime").val();
                var maximum_working_hours = HoursOnly($("#txtWorkingHours").val());
                var maximum_working_minute = MinutesOnly($("#txtWorkingHours").val());
                var grace_time_for_late_punch = '2000-01-01 ' + $("#grace_time_for_late_punch").val();


                var txtnumber_of_grace_time_applicable_in_month = 0;

                if ($("#txtnumber_of_grace_time_applicable_in_month").val() != '') {
                    txtnumber_of_grace_time_applicable_in_month = $("#txtnumber_of_grace_time_applicable_in_month").val();
                }

                var chk_is_ot_applicable = 0; // Not applicable
                if ($("#chk_is_ot_applicable").is(":checked")) {
                    chk_is_ot_applicable = 1; //applicable
                }

                var chk_is_default = 0; // Not applicable
                if ($("#chk_is_default").is(":checked")) {
                    chk_is_default = 1; //applicable
                }

                var chkLunchPunchApplicable = 0; // for lunch punch not applicable 

                var txtLunchPunchInTime = "2000-01-01 00:00";
                var txtLunchPunchOutTime = "2000-01-01 00:00";
                var txtmaximum_lunch_time = "2000-01-01 00:00";

                if ($("#chkLunchPunchApplicable").is(":checked")) {
                    chkLunchPunchApplicable = 1; // 1 for applicable 

                    if ($("#txtLunchPunchInTime").val() == '') {
                        alert('Lunch punch in time required field...!');
                        $("#txtLunchPunchInTime").focus();
                        return false;
                    }
                    else {
                        txtLunchPunchInTime = '2000-01-01 ' + $("#txtLunchPunchInTime").val();
                    }
                    if ($("#txtLunchPunchOutTime").val() == '') {
                        alert('Lunch punch out time is required field...!');
                        $("#txtLunchPunchOutTime").focus();
                        return false;
                    }
                    else {
                        txtLunchPunchOutTime = '2000-01-01 ' + $("#txtLunchPunchOutTime").val();
                    }
                    txtmaximum_lunch_time = '2000-01-01 ' + $("#txtmaximum_lunch_time").val();


                    //compare lunch time
                    if (new Date(txtLunchPunchOutTime) < new Date(txtLunchPunchInTime)) {
                        alert('Lunch Out Time must be greater than in time....!');
                        $("#txtLunchPunchOutTime").focus();
                        return false;
                    }
                }

                var chkTeaPunch1 = 0; // 0 for Tea punch 1 not applicable 


                var txtTeaPunchInTime1 = "2000-01-01 00:00";
                var txtTeaPunchOutTime1 = "2000-01-01 00:00";
                var txtmaximum_tea_time_one = "2000-01-01 00:00";


                if ($("#chkTeaPunch1").is(":checked")) {
                    chkTeaPunch1 = 1; // 1 applicable

                    if ($("#txtTeaPunchInTime1").val() == '') {
                        alert('Tea 1 punch in time required field...!');
                        $("#txtTeaPunchInTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchInTime1 = '2000-01-01 ' + $("#txtTeaPunchInTime1").val();
                    }
                    if ($("#txtTeaPunchOutTime1").val() == '') {
                        alert('Tea 1 punch out time is required field...!');
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchOutTime1 = '2000-01-01 ' + $("#txtTeaPunchOutTime1").val();
                    }
                    txtmaximum_tea_time_one = '2000-01-01 ' + $("#txtmaximum_tea_time_one").val();

                    if (new Date(txtTeaPunchOutTime1) < new Date(txtTeaPunchInTime1)) {
                        alert("Tea 1 Punch Out time must be greater than In time...!");
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                }

                var chkTeaPunch2 = 0; // 0 for Tea punch 2 not applicable 

                var txtTeaPunchInTime2 = "2000-01-01 00:00";
                var txtTeaPunchOutTime2 = "2000-01-01 00:00";
                var txtmaximum_tea_time_two = "2000-01-01 00:00";

                if ($("#chkTeaPunch2").is(":checked")) {
                    chkTeaPunch2 = 1; // 1 applicable
                    if ($("#txtTeaPunchInTime2").val() == '') {
                        alert('Tea 2 punch in time required field...!');
                        $("#txtTeaPunchInTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchInTime2 = '2000-01-01 ' + $("#txtTeaPunchInTime2").val();
                    }
                    if ($("#txtTeaPunchOutTime2").val() == '') {
                        alert('Tea 2 punch out time is required field...!');
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchOutTime2 = '2000-01-01 ' + $("#txtTeaPunchOutTime2").val();
                    }
                    txtmaximum_tea_time_two = '2000-01-01 ' + $("#txtmaximum_tea_time_two").val();

                    if (new Date(txtTeaPunchOutTime2) < new Date(txtTeaPunchInTime2)) {
                        alert("Tea 2 Punch out time must be greater than In time....!");
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                }

                var chkIsNightShift = 0; //  0 for not night shift
                if ($("#chkIsNightShift").is(":checked")) {
                    chkIsNightShift = 1; // 1 for is night shift
                }

                var txtmark_as_half_day_for_working_hours_less_than = '2000-01-01 ' + $("#txtmark_as_half_day_for_working_hours_less_than").val();

                var chkWeekOff = 0; //  1 for not night shift
                if ($("#chkWeekOff").is(":checked")) {
                    chkWeekOff = 1; // 2 for night shift
                }


                var ShiftLocation = [];

                ShiftLocation.push({ 'location_id': ddllocation, 'company_id': ddlcompany });

                var ShiftDept = [];

                ShiftDept.push({ 'dept_id': ddldepartment });

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

                //var HaveDisplay = ISDisplayMenu("Display Company List");

                //if (HaveDisplay == 0)
                //{
                //    ddlcompany = default_company;
                //}

                var company_id = 0;
                if (ddlcompany != 0) {
                    company_id = ddlcompany;
                }

                var location_id = 0;
                if (ddllocation != 0) {
                    location_id = ddllocation;
                }

                //var department_id = 0;
                //if (ddldepartment != 0) {
                //    department_id = ddldepartment;
                //}


                var department_id = 0;
                if (ddldepartment != 0) {
                    department_id = 1;
                }

                $('#loader').show();
                var emp_id = login_emp_id;

                var myData = {
                    'shift_for_all_company': company_id,
                    'shift_for_all_location': location_id,
                    'ShiftLocation': ShiftLocation,
                    'shift_for_all_department': department_id,
                    'ShiftDept': ShiftDept,
                    'shift_id': 0,
                    'shift_type': ddlShiftType,
                    'shift_name': txtShiftName,
                    'shift_short_name': txtShiftShortName,
                    'punch_in_time': txtPunchInTime,
                    'punch_in_max_time': txtpunch_in_max_time,
                    'punch_out_time': txtPunchOutTime,
                    'maximum_working_hours': maximum_working_hours,
                    'maximum_working_minute': maximum_working_minute,
                    'grace_time_for_late_punch_in': grace_time_for_late_punch,
                    'number_of_grace_time_applicable_in_month': txtnumber_of_grace_time_applicable_in_month,
                    'is_lunch_punch_applicable': chkLunchPunchApplicable,
                    'lunch_punch_out_time': txtLunchPunchOutTime,
                    'lunch_punch_in_time': txtLunchPunchInTime,
                    'maximum_lunch_time': txtmaximum_lunch_time,
                    'is_ot_applicable': chk_is_ot_applicable,
                    'tea_punch_applicable_one': chkTeaPunch1,
                    'tea_punch_out_time_one': txtTeaPunchOutTime1, //txtTeaPunchInTime1,
                    'tea_punch_in_time_one': txtTeaPunchInTime1, //txtTeaPunchOutTime1,
                    'maximum_tea_time_one': txtmaximum_tea_time_one,
                    'tea_punch_applicable_two': chkTeaPunch2,
                    'tea_punch_out_time_two': txtTeaPunchOutTime2, //txtTeaPunchInTime2,
                    'tea_punch_in_time_two': txtTeaPunchInTime2, //txtTeaPunchOutTime2,
                    'maximum_tea_time_two': txtmaximum_tea_time_two,
                    'mark_as_half_day_for_working_hours_less_than': txtmark_as_half_day_for_working_hours_less_than,
                    'is_night_shift': chkIsNightShift,
                    'punch_type': 0,
                    'weekly_off': chkWeekOff,
                    'ShiftWeekOff': WeekOffArray,
                    'created_by': emp_id,
                    'last_modified_by': emp_id,
                    'is_default': chk_is_default,
                };

                //var myJSON = JSON.stringify(myData);
                //console.log(myJSON);
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
                            //messageBox("success", Msg);
                            window.location.href = '/Shift/View';
                        }
                        else if (statuscode == "0") {
                            alert(Msg);
                        }
                    },
                    error: function (request, status, error) {
                        $('#loader').hide();
                        _GUID_New();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }

                });

            }
        });

        $('#btnUpdate').bind("click", function () {

            //Validation
            Validation();
            if (Validation() != false) {


                var emp_id = login_emp_id;

                var qs = getQueryStrings();
                var shift_id = qs["shift_id"];
                var ddlcompany = 0;//$("#ddlcompany").val();
                var ddllocation = 0;//$("#ddllocation").val();
                var ddldepartment = 0; //$("#ddldepartment").val();
                var ddlShiftType = $("#ddlShiftType").val();
                var txtShiftName = $("#txtShiftName").val();
                var txtShiftShortName = $("#txtShiftShortName").val();

                var txtPunchInTime = '2000-01-01 ' + $("#txtPunchInTime").val();

                var txtpunch_in_max_time = '2000-01-01 ' + $("#txtpunch_in_max_time").val();
                var txtPunchOutTime = '2000-01-01 ' + $("#txtPunchOutTime").val();
                var maximum_working_hours = HoursOnly($("#txtWorkingHours").val());
                var maximum_working_minute = MinutesOnly($("#txtWorkingHours").val());
                var grace_time_for_late_punch = '2000-01-01 ' + $("#grace_time_for_late_punch").val();


                var txtnumber_of_grace_time_applicable_in_month = 0;

                if ($("#txtnumber_of_grace_time_applicable_in_month").val() != '') {
                    txtnumber_of_grace_time_applicable_in_month = $("#txtnumber_of_grace_time_applicable_in_month").val();
                }



                var chk_is_ot_applicable = 0; // Not applicable
                if ($("#chk_is_ot_applicable").is(":checked")) {
                    chk_is_ot_applicable = 1; //applicable
                }


                var chk_is_default = 0; // Not applicable
                if ($("#chk_is_default").is(":checked")) {
                    chk_is_default = 1; //applicable
                }

                var chkLunchPunchApplicable = 0; // for lunch punch not applicable 

                var txtLunchPunchInTime = "2000-01-01 00:00";
                var txtLunchPunchOutTime = "2000-01-01 00:00";
                var txtmaximum_lunch_time = "2000-01-01 00:00";

                if ($("#chkLunchPunchApplicable").is(":checked")) {
                    chkLunchPunchApplicable = 1; // 1 for applicable 

                    if ($("#txtLunchPunchInTime").val() == '') {
                        alert('Lunch punch in time required field...!');
                        $("#txtLunchPunchInTime").focus();
                        return false;
                    }
                    else {
                        txtLunchPunchInTime = '2000-01-01 ' + $("#txtLunchPunchInTime").val();
                    }
                    if ($("#txtLunchPunchOutTime").val() == '') {
                        alert('Lunch punch out time is required field...!');
                        $("#txtLunchPunchOutTime").focus();
                        return false;
                    }
                    else {
                        txtLunchPunchOutTime = '2000-01-01 ' + $("#txtLunchPunchOutTime").val();
                    }
                    txtmaximum_lunch_time = '2000-01-01 ' + $("#txtmaximum_lunch_time").val();

                    if (new Date(txtLunchPunchOutTime) < new Date(txtLunchPunchInTime)) {
                        alert("Lunch Out time must be greater than In time...!");
                        $("#txtLunchPunchOutTime").focus();
                        return false;
                    }
                }

                var chkTeaPunch1 = 0; // 0 for Tea punch 1 not applicable 


                var txtTeaPunchInTime1 = "2000-01-01 00:00";
                var txtTeaPunchOutTime1 = "2000-01-01 00:00";
                var txtmaximum_tea_time_one = "2000-01-01 00:00";


                if ($("#chkTeaPunch1").is(":checked")) {
                    chkTeaPunch1 = 1; // 1 applicable

                    if ($("#txtTeaPunchInTime1").val() == '') {
                        alert('Tea 1 punch in time required field...!');
                        $("#txtTeaPunchInTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchInTime1 = '2000-01-01 ' + $("#txtTeaPunchInTime1").val();
                    }
                    if ($("#txtTeaPunchOutTime1").val() == '') {
                        alert('Tea 1 punch out time is required field...!');
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchOutTime1 = '2000-01-01 ' + $("#txtTeaPunchOutTime1").val();
                    }

                    if (new Date(txtTeaPunchOutTime1) < new Date(txtTeaPunchInTime1)) {
                        alert("Tea 1 Punch out time must be greater than in time...!!");
                        $("#txtTeaPunchOutTime1").focus();
                        return false;
                    }
                    txtmaximum_tea_time_one = '2000-01-01 ' + $("#txtmaximum_tea_time_one").val();


                }

                var chkTeaPunch2 = 0; // 0 for Tea punch 2 not applicable 
                // debugger;
                var txtTeaPunchInTime2 = "2000-01-01 00:00";
                var txtTeaPunchOutTime2 = "2000-01-01 00:00";
                var txtmaximum_tea_time_two = "2000-01-01 00:00";

                if ($("#chkTeaPunch2").is(":checked")) {
                    chkTeaPunch2 = 1; // 1 applicable
                    if ($("#txtTeaPunchInTime2").val() == '') {
                        alert('Tea 2 punch in time required field...!');
                        $("#txtTeaPunchInTime2").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchInTime2 = '2000-01-01 ' + $("#txtTeaPunchInTime2").val();

                    }
                    if ($("#txtTeaPunchOutTime2").val() == '') {
                        alert('Tea 2 punch out time is required field...!');
                        $("#txtTeaPunchOutTime2").focus();
                        return false;
                    }
                    else {
                        txtTeaPunchOutTime2 = '2000-01-01 ' + $("#txtTeaPunchOutTime2").val();
                    }

                    if (new Date(txtTeaPunchOutTime2) < new Date(txtTeaPunchInTime2)) {
                        alert("Tea 2 Punch out time must be greater than in time...!!");
                        $("#txtTeaPunchOutTime2").focus();
                        return false;
                    }
                    txtmaximum_tea_time_two = '2000-01-01 ' + $("#txtmaximum_tea_time_two").val();
                }



                var chkIsNightShift = 0; //  0 for not night shift
                if ($("#chkIsNightShift").is(":checked")) {
                    chkIsNightShift = 1; // 1 for is night shift
                }

                var txtmark_as_half_day_for_working_hours_less_than = '2000-01-01 ' + $("#txtmark_as_half_day_for_working_hours_less_than").val();


                var chkWeekOff = 0; //  1 for not night shift
                if ($("#chkWeekOff").is(":checked")) {
                    chkWeekOff = 1; // 2 for night shift
                }


                var ShiftLocation = [];

                ShiftLocation.push({ 'location_id': ddllocation, 'company_id': ddlcompany });

                var ShiftDept = [];

                ShiftDept.push({ 'dept_id': ddldepartment });

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

                //var HaveDisplay = ISDisplayMenu("Display Company List");

                //if (HaveDisplay == 0)
                //{
                //    ddlcompany = default_company;
                //}


                var company_id = 0;
                if (ddlcompany != 0) {
                    company_id = 1;
                }

                var location_id = 0;
                if (ddllocation != 0) {
                    location_id = 1;
                }

                var department_id = 0;
                if (ddldepartment != 0) {
                    department_id = 1;
                }
                $('#loader').show();
                var myData = {
                    'shift_for_all_company': company_id,
                    'shift_for_all_location': location_id,
                    'ShiftLocation': ShiftLocation,
                    'shift_for_all_department': department_id,
                    'ShiftDept': ShiftDept,
                    'shift_id': 0,
                    'shift_type': ddlShiftType,
                    'shift_name': txtShiftName,
                    'shift_short_name': txtShiftShortName,
                    'punch_in_time': txtPunchInTime,
                    'punch_in_max_time': txtpunch_in_max_time,
                    'punch_out_time': txtPunchOutTime,
                    'maximum_working_hours': maximum_working_hours,
                    'maximum_working_minute': maximum_working_minute,
                    'grace_time_for_late_punch_in': grace_time_for_late_punch,
                    'number_of_grace_time_applicable_in_month': txtnumber_of_grace_time_applicable_in_month,
                    'is_lunch_punch_applicable': chkLunchPunchApplicable,
                    'lunch_punch_out_time': txtLunchPunchOutTime,
                    'lunch_punch_in_time': txtLunchPunchInTime,
                    'maximum_lunch_time': txtmaximum_lunch_time,
                    'is_ot_applicable': chk_is_ot_applicable,
                    'tea_punch_applicable_one': chkTeaPunch1,
                    'tea_punch_out_time_one': txtTeaPunchOutTime1, //txtTeaPunchInTime1,
                    'tea_punch_in_time_one': txtTeaPunchInTime1, //txtTeaPunchOutTime1,
                    'maximum_tea_time_one': txtmaximum_tea_time_one,
                    'tea_punch_applicable_two': chkTeaPunch2,
                    'tea_punch_in_time_two': txtTeaPunchInTime2,
                    'tea_punch_out_time_two': txtTeaPunchOutTime2,
                    'maximum_tea_time_two': txtmaximum_tea_time_two,
                    'mark_as_half_day_for_working_hours_less_than': txtmark_as_half_day_for_working_hours_less_than,
                    'is_night_shift': chkIsNightShift,
                    'punch_type': 0,
                    'weekly_off': chkWeekOff,
                    'ShiftWeekOff': WeekOffArray,
                    'created_by': emp_id,
                    'last_modified_by': emp_id,
                    'is_default': chk_is_default,
                };
                //debugger;
                var asdsd = JSON.stringify(myData);
                console.log(asdsd);
                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({
                    url: localStorage.getItem("ApiUrl") + 'apiShift/' + shift_id,
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
                            //messageBox("success", Msg);
                            window.location.href = '/Shift/View';
                        }
                        else if (statuscode == "0") {
                            messageBox("success", Msg);
                            alert(Msg);
                        }
                    },
                    error: function (request, status, error) {
                        $('#loader').hide();
                        _GUID_New();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }

                });


            }

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

        $('#btnReset').bind("click", function () {

            Reset();

        });


    }, 2000);// end timeout


});


//$(document).on("click", "a[name='RefTime']", function (e) {
//    GetPunchOutTime();
//});



function GetPunchOutTime() {
    $('#loader').show();
    var txtPunchInTime = $("#txtPunchInTime").val();
    


    if (txtPunchInTime == '') {
        alert('Punch in time is required field...!');
        $("#txtPunchInTime").focus();
        $('#loader').hide();
        return;
    }

    var txtWorkingHours = $("#txtWorkingHours").val();
    if (txtWorkingHours == '') {
        alert('Working hours is required field...!');
        $("#txtWorkingHours").focus();
        $('#loader').hide();
        return;
    }

    var txtPunchInTime = $("#txtPunchInTime").val();
    var txtWorkingHours = $("#txtWorkingHours").val();

    //calculate punch out time
    var punchintime = new Date('2000-01-01 ' + txtPunchInTime);
    var workhour = new Date('2000-01-01 ' + txtWorkingHours);;
    var gethh = workhour.getHours();
    var getmm = workhour.getMinutes();
    var getss = workhour.getSeconds();
    punchintime.setHours(punchintime.getHours() + gethh);
    punchintime.setMinutes(punchintime.getMinutes() + getmm);
    punchintime.setSeconds(punchintime.getSeconds() + getss);

    //var newDateObj = new Date();
    //newDateObj.setTime(punchintime.getTime() + (gethh * getmm * getss));

    var PunchOutTime = formatAMPM(punchintime);
    $("#txtPunchOutTime").val(PunchOutTime);
    if ($("#ddlShiftType").val() != 2) {
        $("#txtpunch_in_max_time").val(txtPunchInTime);
    }
    //$("#txtPunchOutTime").val(PunchOutTime);
    $('#loader').hide();
}

//$('.timepicker1').time();

//Get Last Shify Id
function GetLastShiftId() {

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiShift/GetLastShiftId",
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;
            if (res != null) {
                $("#txtshift_code").val('S0000' + (res.shift_id + 1));
            }
            else {
                $("#txtshift_code").val('');
            }

        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
        }
    });
}

function convertTimeAM(time24) {
    var tmpArr = time24.split(':'), time12;
    if (+tmpArr[0] == 12) {
        time12 = tmpArr[0] + ':' + tmpArr[1] + ' PM';
    } else {
        if (+tmpArr[0] == 00) {
            time12 = '12:' + tmpArr[1] + ' AM';
        } else {
            if (+tmpArr[0] > 12) {
                time12 = (+tmpArr[0] - 12) + ':' + tmpArr[1] + ' PM';
            } else {
                time12 = (+tmpArr[0]) + ':' + tmpArr[1] + ' AM';
            }
        }
    }
    return time12;
}
function convertTimePM(time24) {
    var tmpArr = time24.split(':'), time12;
    if (+tmpArr[0] == 12) {
        time12 = tmpArr[0] + ':' + tmpArr[1] + ' AM';
    } else {
        if (+tmpArr[0] == 00) {
            time12 = '12:' + tmpArr[1] + ' PM';
        } else {
            if (+tmpArr[0] > 12) {
                time12 = (+tmpArr[0] - 12) + ':' + tmpArr[1] + ' AM';
            } else {
                time12 = (+tmpArr[0]) + ':' + tmpArr[1] + ' PM';
            }
        }
    }
    return time12;
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


// Validation funcation for shift master
function Validation() {

    var isvalid = false;


    var ddlShiftType = $("#ddlShiftType").val();
    var txtShiftName = $("#txtShiftName").val();
    var txtShiftShortName = $("#txtShiftShortName").val();
    var txtPunchInTime = $("#txtPunchInTime").val();
    var txtWorkingHours = $("#txtWorkingHours").val();
    var txtPunchOutTime = $("#txtPunchOutTime").val();
    var txtpunch_in_max_time = $("#txtpunch_in_max_time").val();
    var txtmark_as_half_day_for_working_hours_less_than = $("#txtmark_as_half_day_for_working_hours_less_than").val();

    var ddlShiftType= $("#ddlShiftType").val();
    

    if (ddlShiftType == '' || ddlShiftType == "0" || ddlShiftType == "") {
        messageBox("error", "Shift type is required field...!");
        isvalid = false;
    }
    else if (txtShiftName == '') {
        messageBox("error", "Shift name is required field...!");
        isvalid = false;
    }
    else if (txtShiftShortName == '') {
        messageBox("error", "Shift short name is required field...!");
        isvalid = false;
    }
    else if ((txtPunchInTime == '') && (ddlShiftType != "0")) {
        messageBox("error", "Punch in time is required field...!");
        isvalid = false;
    }
    else if (txtWorkingHours == '') {
        messageBox("error", "Working hours is required field...!");
        isvalid = false;
    }
    else if (txtPunchOutTime == '') {
        messageBox("error", "Punch out time is required field...!");
        isvalid = false;
    }
    else if (txtpunch_in_max_time == '') {
        messageBox("error", "Punch in max time is required field...!");
        isvalid = false;
    }
    else if (txtmark_as_half_day_for_working_hours_less_than == '') {
        messageBox("error", "Min Half Day Working Hour is required field...!");
        $("#txtmark_as_half_day_for_working_hours_less_than").focus();
        isvalid = false;
    }
    else if ((ddlShiftType==1) && (new Date('2000-01-01 ' + txtpunch_in_max_time) < new Date('2000-01-01 ' + txtPunchInTime) || new Date('2000-01-01 ' + txtpunch_in_max_time) > new Date('2000-01-01 ' + txtPunchOutTime)) ){
        messageBox("error", "Punch in maximum time must be greater than punch in time and less than punch out time");
        $("#txtpunch_in_max_time").focus();
        isvalid = false;
    }
    else if (new Date('2000-01-01 ' + txtPunchOutTime) < new Date('2000-01-01 ' + txtPunchInTime)) {
        messageBox("error", "Punch out time must be greater than in time...!!");
        $("#txtPunchOutTime").focus();
        isvalid = false;
    }

    else {
        isvalid = true
    }

    return isvalid;
}



// Save Shift Master Data




// Reset funcation for shift master
function Reset() {
    //window.location = '/Shift/CreateShift';
    location.reload();
}

function tConv24(time24) {
    // // debugger;
    var ts = time24;
    var H = +ts.substr(0, 2);
    var h = (H % 12) || 12;
    h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
    var ampm = H < 12 ? " AM" : " PM";
    ts = h + ts.substr(2, 3) + ampm;
    return ts;
};

//Edit Company Master
function GetDataByShiftId(shift_id) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiShift/' + shift_id,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            //  // debugger;
            var res = data;



            //if (res.shift_for_all_company == 0) {
            //    //Company Master
            //    BindAllEmp_Company('ddlDefaultCompany', login_emp_id, default_company);
            //    //BindAllEmp_Company('ddlDefaultCompany', login_emp_id, res.company_id);

            //}
            //else {
            //    BindAllEmp_Company('ddlDefaultCompany', login_emp_id,0);

            //}


            //if (res.shift_for_all_location == 0) {
            //   // BindLocationListForddl('ddllocation', res.company_id, res.location_id);
            //    BindLocationListForddl('ddllocation', default_company, res.location_id);

            //}
            //else {
            //    BindLocationListForddl('ddllocation', default_company, 0);
            //    //BindLocationListForddl('ddllocation', res.company_id,0);
            //}

            //if (res.shift_for_all_department == 0) {
            //    BindDepartmentListForddl('ddldepartment', res.company_id, res.department_id);
            //}
            //else {
            //    BindDepartmentListForddl('ddldepartment', res.company_id, 0);
            //}

            if (res.shift_type == 1) {
                $('.PunchBox').css('display', 'block');
                $('#divPunchInMaxTime').hide();
                $('#divPunchInTime').show();
                $('#divWorkingHours').show();
                $('#divPunchOutTime').show();
            }
            else if (res.shift_type == 2) {
                $('.PunchBox').css('display', 'block');
                $('#divPunchInMaxTime').show();
                $('#divPunchInTime').show();
                $('#divWorkingHours').show();
                $('#divPunchOutTime').show();
            }
            else {
                $('#divPunchInMaxTime').hide();
                $('#divPunchInTime').hide();
                $('#divWorkingHours').hide();
                $('#divPunchOutTime').hide();
            }


            if (res.punch_in_time != "2000-01-01T00:00:00") {
                var punch_in_dt = new Date(res.punch_in_time);
                var punch_in_time = '00:00:00';
                if (punch_in_dt.getMinutes() == 0) {
                    punch_in_time = punch_in_dt.getHours() + ":0" + punch_in_dt.getMinutes();//+ ":" + punch_in_dt.getSeconds();
                }
                else if (punch_in_dt.getMinutes() <= 9 && punch_in_dt.getHours() <= 9) {
                    punch_in_time = '0' + punch_in_dt.getHours() + ":0" + punch_in_dt.getMinutes(); //+ ":" + punch_in_dt.getSeconds();
                }
                else if (punch_in_dt.getMinutes() <= 9) {
                    punch_in_time = punch_in_dt.getHours() + ":0" + punch_in_dt.getMinutes(); //+ ":" + punch_in_dt.getSeconds();
                }
                else if (punch_in_dt.getHours() <= 9) {
                    punch_in_time = '0' + punch_in_dt.getHours() + ":" + punch_in_dt.getMinutes(); //+ ":" + punch_in_dt.getSeconds();
                }
                else {
                    punch_in_time = punch_in_dt.getHours() + ":" + punch_in_dt.getMinutes(); //+ ":" + punch_in_dt.getSeconds();
                }
                //var punch_in_time_main = tConv24(punch_in_time);
                var punch_in_time_main = punch_in_time;
                $("#txtPunchInTime").val(punch_in_time_main);
            }

            if (res.punch_in_max_time != "2000-01-01T00:00:00") {
                var punch_in_max_dt = new Date(res.punch_in_max_time);
                var punch_in_max_time = '00:00:00';
                if (punch_in_max_dt.getMinutes() == 0) {
                    punch_in_max_time = punch_in_max_dt.getHours() + ":0" + punch_in_max_dt.getMinutes(); //+ ":" + punch_in_max_dt.getSeconds();
                }
                else if (punch_in_max_dt.getMinutes() <= 9 && punch_in_max_dt.getHours() <= 9) {
                    punch_in_max_time = '0' + punch_in_max_dt.getHours() + ":0" + punch_in_max_dt.getMinutes();// + ":" + punch_in_max_dt.getSeconds();
                }
                else if (punch_in_max_dt.getMinutes() <= 9) {
                    punch_in_max_time = punch_in_max_dt.getHours() + ":0" + punch_in_max_dt.getMinutes(); //+ ":" + punch_in_max_dt.getSeconds();
                }
                else if (punch_in_max_dt.getHours() <= 9) {
                    punch_in_max_time = '0' + punch_in_max_dt.getHours() + ":" + punch_in_max_dt.getMinutes();// + ":" + punch_in_max_dt.getSeconds();
                }
                else {
                    punch_in_max_time = punch_in_max_dt.getHours() + ":" + punch_in_max_dt.getMinutes();// + ":" + punch_in_max_dt.getSeconds();
                }
                //var punch_in_max_time_main = tConv24(punch_in_max_time);
                var punch_in_max_time_main = punch_in_max_time;
                $("#txtpunch_in_max_time").val(punch_in_max_time_main);
            }

            if (res.punch_out_time != "2000-01-01T00:00:00") {
                var punch_out_dt = new Date(res.punch_out_time);
                var punch_out_time = '00:00:00';
                if (punch_out_dt.getMinutes() == 0) {
                    punch_out_time = punch_out_dt.getHours() + ":0" + punch_out_dt.getMinutes(); //+ ":" + punch_out_dt.getSeconds();
                }
                else if (punch_out_dt.getMinutes() <= 9 && punch_out_dt.getHours() <= 9) {
                    punch_out_time = '0' + punch_out_dt.getHours() + ":0" + punch_out_dt.getMinutes(); //+ ":" + punch_out_dt.getSeconds();
                }
                else if (punch_out_dt.getMinutes() <= 9) {
                    punch_out_time = punch_out_dt.getHours() + ":0" + punch_out_dt.getMinutes(); //+ ":" + punch_out_dt.getSeconds();
                }
                else if (punch_out_dt.getHours() <= 9) {
                    punch_out_time = '0' + punch_out_dt.getHours() + ":" + punch_out_dt.getMinutes(); //+ ":" + punch_out_dt.getSeconds();
                }
                else {
                    punch_out_time = punch_out_dt.getHours() + ":" + punch_out_dt.getMinutes(); //+ ":" + punch_out_dt.getSeconds();
                }
                // var punch_out_time_main = tConv24(punch_out_time);
                var punch_out_time_main = punch_out_time;
                $("#txtPunchOutTime").val(punch_out_time_main);
            }

            if (res.grace_time_for_late_punch_in != "2000-01-01T00:00:00") {
                var grace_time_dt = new Date(res.grace_time_for_late_punch_in);
                var grace_time = '00:00:00';
                if (grace_time_dt.getMinutes() == 0) {
                    grace_time = grace_time_dt.getHours() + ":0" + grace_time_dt.getMinutes();
                }
                else if (grace_time_dt.getMinutes() <= 9 && grace_time_dt.getHours() <= 9) {
                    grace_time = '0' + grace_time_dt.getHours() + ":0" + grace_time_dt.getMinutes();
                }
                else if (grace_time_dt.getMinutes() <= 9) {
                    grace_time = grace_time_dt.getHours() + ":0" + grace_time_dt.getMinutes();
                }
                else if (grace_time_dt.getHours() <= 9) {
                    grace_time = '0' + grace_time_dt.getHours() + ":" + grace_time_dt.getMinutes();
                }
                else {
                    grace_time = grace_time_dt.getHours() + ":" + grace_time_dt.getMinutes();
                }
                $("#grace_time_for_late_punch").val(grace_time);
            }

            $("#ddlShiftType").val(res.shift_type);
            $("#txtShiftName").val(res.shift_name);
            $("#txtShiftShortName").val(res.shift_short_name);
            $("#txtshift_code").val('S0000' + res.shift_id);


            var maximum_working_hours = '00';
            var maximum_working_min = '00';
            if (res.maximum_working_minute <= 9 && res.maximum_working_hours <= 9) {
                maximum_working_hours = '0' + res.maximum_working_hours;
                maximum_working_min = '0' + res.maximum_working_minute;
            }
            else if (res.maximum_working_minute <= 9) {
                maximum_working_min = '0' + res.maximum_working_minute;
                maximum_working_hours = + res.maximum_working_hours;
            }
            else if (res.maximum_working_hours <= 9) {
                maximum_working_hours = '0' + res.maximum_working_hours;
                maximum_working_min = + res.maximum_working_minute;
            }

            else {
                maximum_working_hours = res.maximum_working_hours;
                maximum_working_min = res.maximum_working_minute;
            }


            $("#txtWorkingHours").val(maximum_working_hours + ':' + maximum_working_min);

            if (res.number_of_grace_time_applicable_in_month != 0) {
                $("#txtnumber_of_grace_time_applicable_in_month").val(res.number_of_grace_time_applicable_in_month);
            }
            if (res.is_ot_applicable == 1) {
                $("#chk_is_ot_applicable").attr('checked', 'checked');
            }
            if (res.is_default == 1) {
                $("#chk_is_default").attr('checked', 'checked');
            }
            if (res.is_lunch_punch_applicable == 1) {
                $("#chkLunchPunchApplicable").attr('checked', 'checked');
                $('.lunchtime').css('display', 'block');
            }


            if (res.lunch_punch_in_time != "2000-01-01T00:00:00") {
                var lunch_punch_in_dt = new Date(res.lunch_punch_in_time);
                var lunch_punch_in_time = '00:00:00';
                if (lunch_punch_in_dt.getMinutes() == 0) {
                    lunch_punch_in_time = lunch_punch_in_dt.getHours() + ":0" + lunch_punch_in_dt.getMinutes();//+ ":" + lunch_punch_in_dt.getSeconds();
                }
                else if (lunch_punch_in_dt.getMinutes() <= 9 && lunch_punch_in_dt.getHours() <= 9) {
                    lunch_punch_in_time = '0' + lunch_punch_in_dt.getHours() + ":0" + lunch_punch_in_dt.getMinutes(); //+ ":" + lunch_punch_in_dt.getSeconds();
                }
                else if (lunch_punch_in_dt.getMinutes() <= 9) {
                    lunch_punch_in_time = lunch_punch_in_dt.getHours() + ":0" + lunch_punch_in_dt.getMinutes(); //+ ":" + lunch_punch_in_dt.getSeconds();
                }
                else if (lunch_punch_in_dt.getHours() <= 9) {
                    lunch_punch_in_time = '0' + lunch_punch_in_dt.getHours() + ":" + lunch_punch_in_dt.getMinutes(); //+ ":" + lunch_punch_in_dt.getSeconds();
                }
                else {
                    lunch_punch_in_time = lunch_punch_in_dt.getHours() + ":" + lunch_punch_in_dt.getMinutes();//+ ":" + lunch_punch_in_dt.getSeconds();
                }
                //  var lunch_punch_in_time_main = tConv24(lunch_punch_in_time);
                var lunch_punch_in_time_main = lunch_punch_in_time;
                $("#txtLunchPunchInTime").val(lunch_punch_in_time_main);

            }

            if (res.lunch_punch_out_time != "2000-01-01T00:00:00") {
                var lunch_punch_out_dt = new Date(res.lunch_punch_out_time);
                var lunch_punch_out_time = '00:00:00';
                if (lunch_punch_out_dt.getMinutes() == 0) {
                    lunch_punch_out_time = lunch_punch_out_dt.getHours() + ":0" + lunch_punch_out_dt.getMinutes(); //+ ":" + lunch_punch_out_dt.getSeconds();
                }
                else if (lunch_punch_out_dt.getMinutes() <= 9 && lunch_punch_out_dt.getHours() <= 9) {
                    lunch_punch_out_time = '0' + lunch_punch_out_dt.getHours() + ":0" + lunch_punch_out_dt.getMinutes(); //+ ":" + lunch_punch_out_dt.getSeconds();
                }
                else if (lunch_punch_out_dt.getMinutes() <= 9) {
                    lunch_punch_out_time = lunch_punch_out_dt.getHours() + ":0" + lunch_punch_out_dt.getMinutes(); //+ ":" + lunch_punch_out_dt.getSeconds();
                }
                else if (lunch_punch_out_dt.getHours() <= 9) {
                    lunch_punch_out_time = '0' + lunch_punch_out_dt.getHours() + ":" + lunch_punch_out_dt.getMinutes(); //+ ":" + lunch_punch_out_dt.getSeconds();
                }
                else {
                    lunch_punch_out_time = lunch_punch_out_dt.getHours() + ":" + lunch_punch_out_dt.getMinutes(); //+ ":" + lunch_punch_out_dt.getSeconds();
                }
                //var lunch_punch_out_time_main = tConv24(lunch_punch_out_time);
                var lunch_punch_out_time_main = lunch_punch_out_time;
                $("#txtLunchPunchOutTime").val(lunch_punch_out_time_main);
            }

            if (res.maximum_lunch_time != "2000-01-01T00:00:00") {
                var lunch_punch_max_dt = new Date(res.maximum_lunch_time);
                var lunch_punch_max_time = '00:00:00';
                if (lunch_punch_max_dt.getMinutes() == 0) {
                    lunch_punch_max_time = lunch_punch_max_dt.getHours() + ":0" + lunch_punch_max_dt.getMinutes();
                }
                else if (lunch_punch_max_dt.getMinutes() <= 9 && lunch_punch_max_dt.getHours() <= 9) {
                    lunch_punch_max_time = '0' + lunch_punch_max_dt.getHours() + ":0" + lunch_punch_max_dt.getMinutes();
                }
                else if (lunch_punch_max_dt.getMinutes() <= 9) {
                    lunch_punch_max_time = lunch_punch_max_dt.getHours() + ":0" + lunch_punch_max_dt.getMinutes();
                }
                else if (lunch_punch_max_dt.getHours() <= 9) {
                    lunch_punch_max_time = '0' + lunch_punch_max_dt.getHours() + ":" + lunch_punch_max_dt.getMinutes();
                }
                else {
                    lunch_punch_max_time = lunch_punch_max_dt.getHours() + ":" + lunch_punch_max_dt.getMinutes();
                }

                $("#txtmaximum_lunch_time").val(lunch_punch_max_time);

            }


            if (res.tea_punch_applicable_one == 1) {
                $("#chkTeaPunch1").attr('checked', 'checked');
                $('.teaTime1').css('display', 'block');
            }

            if (res.tea_punch_in_time_one != "2000-01-01T00:00:00") {
                var tea1_punch_in_dt = new Date(res.tea_punch_in_time_one);
                var tea1_punch_in_time = '00:00:00';
                if (tea1_punch_in_dt.getMinutes() == 0) {
                    tea1_punch_in_time = tea1_punch_in_dt.getHours() + ":0" + tea1_punch_in_dt.getMinutes();//+ ":" + tea1_punch_in_dt.getSeconds();
                }
                else if (tea1_punch_in_dt.getMinutes() <= 9 && tea1_punch_in_dt.getHours() <= 9) {
                    tea1_punch_in_time = '0' + tea1_punch_in_dt.getHours() + ":0" + tea1_punch_in_dt.getMinutes(); //+ ":" + tea1_punch_in_dt.getSeconds();
                }
                else if (tea1_punch_in_dt.getMinutes() <= 9) {
                    tea1_punch_in_time = tea1_punch_in_dt.getHours() + ":0" + tea1_punch_in_dt.getMinutes(); //+ ":" + tea1_punch_in_dt.getSeconds();
                }
                else if (tea1_punch_in_dt.getHours() <= 9) {
                    tea1_punch_in_time = '0' + tea1_punch_in_dt.getHours() + ":" + tea1_punch_in_dt.getMinutes(); //+ ":" + tea1_punch_in_dt.getSeconds();
                }
                else {
                    tea1_punch_in_time = tea1_punch_in_dt.getHours() + ":" + tea1_punch_in_dt.getMinutes(); //+ ":" + tea1_punch_in_dt.getSeconds();
                }
                //var tea1_punch_in_time_main = tConv24(tea1_punch_in_time);
                var tea1_punch_in_time_main = tea1_punch_in_time;
                $("#txtTeaPunchInTime1").val(tea1_punch_in_time_main);
            }

            if (res.tea_punch_out_time_one != "2000-01-01T00:00:00") {
                var tea1_punch_out_dt = new Date(res.tea_punch_out_time_one);
                var tea1_punch_out_time = '00:00:00';
                if (tea1_punch_out_dt.getMinutes() == 0) {
                    tea1_punch_out_time = tea1_punch_out_dt.getHours() + ":0" + tea1_punch_out_dt.getMinutes(); //+ ":" + tea1_punch_out_dt.getSeconds();
                }
                else if (tea1_punch_out_dt.getMinutes() <= 9 && tea1_punch_out_dt.getHours() <= 9) {
                    tea1_punch_out_time = '0' + tea1_punch_out_dt.getHours() + ":0" + tea1_punch_out_dt.getMinutes(); //+ ":" + tea1_punch_out_dt.getSeconds();
                }
                else if (tea1_punch_out_dt.getMinutes() <= 9) {
                    tea1_punch_out_time = tea1_punch_out_dt.getHours() + ":0" + tea1_punch_out_dt.getMinutes(); //+ ":" + tea1_punch_out_dt.getSeconds();
                }
                else if (tea1_punch_out_dt.getHours() <= 9) {
                    tea1_punch_out_time = '0' + tea1_punch_out_dt.getHours() + ":" + tea1_punch_out_dt.getMinutes(); //+ ":" + tea1_punch_out_dt.getSeconds();
                }
                else {
                    tea1_punch_out_time = tea1_punch_out_dt.getHours() + ":" + tea1_punch_out_dt.getMinutes(); //+ ":" + tea1_punch_out_dt.getSeconds();
                }
                //var tea1_punch_out_time_main = tConv24(tea1_punch_out_time);
                var tea1_punch_out_time_main = tea1_punch_out_time;
                $("#txtTeaPunchOutTime1").val(tea1_punch_out_time_main);
            }

            if (res.maximum_tea_time_one != "2000-01-01T00:00:00") {
                var tea1_punch_max_dt = new Date(res.maximum_tea_time_one);
                var tea1_punch_max_time = '00:00:00';
                if (tea1_punch_max_dt.getMinutes() == 0) {
                    tea1_punch_max_time = tea1_punch_max_dt.getHours() + ":0" + tea1_punch_max_dt.getMinutes();
                }
                else if (tea1_punch_max_dt.getMinutes() <= 9 && tea1_punch_max_dt.getHours() <= 9) {
                    tea1_punch_max_time = '0' + tea1_punch_max_dt.getHours() + ":0" + tea1_punch_max_dt.getMinutes();
                }
                else if (tea1_punch_max_dt.getMinutes() <= 9) {
                    tea1_punch_max_time = tea1_punch_max_dt.getHours() + ":0" + tea1_punch_max_dt.getMinutes();
                }
                else if (tea1_punch_max_dt.getHours() <= 9) {
                    tea1_punch_max_time = '0' + tea1_punch_max_dt.getHours() + ":" + tea1_punch_max_dt.getMinutes();
                }
                else {
                    tea1_punch_max_time = tea1_punch_max_dt.getHours() + ":" + tea1_punch_max_dt.getMinutes();
                }

                $("#txtmaximum_tea_time_one").val(tea1_punch_max_time);
            }




            if (res.tea_punch_applicable_two == 1) {
                $("#chkTeaPunch2").attr('checked', 'checked');
                $('.teaTime2').css('display', 'block');
            }

            if (res.tea_punch_in_time_two != "2000-01-01T00:00:00" || res.tea_punch_in_time_two != '0001-01-01T00:00:00') {
                var tea2_punch_in_dt = new Date(res.tea_punch_in_time_two);
                var tea2_punch_in_time = '00:00:00';
                if (tea2_punch_in_dt.getMinutes() == 0) {
                    tea2_punch_in_time = tea2_punch_in_dt.getHours() + ":0" + tea2_punch_in_dt.getMinutes();// + ":" + tea2_punch_in_dt.getSeconds();
                }
                else if (tea2_punch_in_dt.getMinutes() <= 9 && tea2_punch_in_dt.getHours() <= 9) {
                    tea2_punch_in_time = '0' + tea2_punch_in_dt.getHours() + ":0" + tea2_punch_in_dt.getMinutes();// + ":" + tea2_punch_in_dt.getSeconds();
                }
                else if (tea2_punch_in_dt.getMinutes() <= 9) {
                    tea2_punch_in_time = tea2_punch_in_dt.getHours() + ":0" + tea2_punch_in_dt.getMinutes(); //+ ":" + tea2_punch_in_dt.getSeconds();
                }
                else if (tea2_punch_in_dt.getHours() <= 9) {
                    tea2_punch_in_time = '0' + tea2_punch_in_dt.getHours() + ":" + tea2_punch_in_dt.getMinutes();// + ":" + tea2_punch_in_dt.getSeconds();
                }
                else {
                    tea2_punch_in_time = tea2_punch_in_dt.getHours() + ":" + tea2_punch_in_dt.getMinutes();// + ":" + tea2_punch_in_dt.getSeconds();
                }
                //var tea2_punch_in_time_main = tConv24(tea2_punch_in_time);
                var tea2_punch_in_time_main = tea2_punch_in_time;
                if (tea2_punch_in_time_main != '1200: PM') {
                    $("#txtTeaPunchInTime2").val(tea2_punch_in_time_main);
                }
            }

            if (res.tea_punch_out_time_two != "2000-01-01T00:00:00") {
                var tea2_punch_out_dt = new Date(res.tea_punch_out_time_two);
                var tea2_punch_out_time = '00:00:00';
                if (tea2_punch_out_dt.getMinutes() == 0) {
                    tea2_punch_out_time = tea2_punch_out_dt.getHours() + ":0" + tea2_punch_out_dt.getMinutes();// + ":" + tea2_punch_out_dt.getSeconds();
                }
                else if (tea2_punch_out_dt.getMinutes() <= 9 && tea2_punch_out_dt.getHours() <= 9) {
                    tea2_punch_out_time = '0' + tea2_punch_out_dt.getHours() + ":0" + tea2_punch_out_dt.getMinutes();// + ":" + tea2_punch_out_dt.getSeconds();
                }
                else if (tea2_punch_out_dt.getMinutes() <= 9) {
                    tea2_punch_out_time = tea2_punch_out_dt.getHours() + ":0" + tea2_punch_out_dt.getMinutes();// + ":" + tea2_punch_out_dt.getSeconds();
                }
                else if (tea2_punch_out_dt.getHours() <= 9) {
                    tea2_punch_out_time = '0' + tea2_punch_out_dt.getHours() + ":" + tea2_punch_out_dt.getMinutes();// + ":" + tea2_punch_out_dt.getSeconds();
                }
                else {
                    tea2_punch_out_time = tea2_punch_out_dt.getHours() + ":" + tea2_punch_out_dt.getMinutes();// + ":" + tea2_punch_out_dt.getSeconds();
                }
                //var tea2_punch_out_time_main = tConv24(tea2_punch_out_time);
                var tea2_punch_out_time_main = tea2_punch_out_time;
                $("#txtTeaPunchOutTime2").val(tea2_punch_out_time_main);
            }


            if (res.maximum_tea_time_two != "2000-01-01T00:00:00") {
                var tea2_punch_max_dt = new Date(res.maximum_tea_time_two);
                var tea2_punch_max_time = '00:00:00';
                if (tea2_punch_max_dt.getMinutes() == 0) {
                    tea2_punch_max_time = tea2_punch_max_dt.getHours() + ":0" + tea2_punch_max_dt.getMinutes();
                }
                else if (tea2_punch_max_dt.getMinutes() <= 9 && tea2_punch_max_dt.getHours() <= 9) {
                    tea2_punch_max_time = '0' + tea2_punch_max_dt.getHours() + ":0" + tea2_punch_max_dt.getMinutes();
                }
                else if (tea2_punch_max_dt.getMinutes() <= 9) {
                    tea2_punch_max_time = tea2_punch_max_dt.getHours() + ":0" + tea2_punch_max_dt.getMinutes();
                }
                else if (tea2_punch_max_dt.getHours() <= 9) {
                    tea2_punch_max_time = '0' + tea2_punch_max_dt.getHours() + ":" + tea2_punch_max_dt.getMinutes();
                }
                else {
                    tea2_punch_max_time = tea2_punch_max_dt.getHours() + ":" + tea2_punch_max_dt.getMinutes();
                }

                $("#txtmaximum_tea_time_two").val(tea2_punch_max_time);
            }

            if (res.is_night_shift == 1) {
                $("#chkIsNightShift").attr('checked', 'checked');
            }

            if (res.min_halfday_working_hour != "2000-01-01T00:00:00") {
                var half_day_for_working_hours_dt = new Date(res.min_halfday_working_hour);
                var half_day_for_working_hours = '00:00:00';
                if (half_day_for_working_hours_dt.getMinutes() == 0) {
                    half_day_for_working_hours = half_day_for_working_hours_dt.getHours() + ":0" + half_day_for_working_hours_dt.getMinutes();
                }
                else if (half_day_for_working_hours_dt.getMinutes() <= 9 && half_day_for_working_hours_dt.getHours() <= 9) {
                    half_day_for_working_hours = '0' + half_day_for_working_hours_dt.getHours() + ":0" + half_day_for_working_hours_dt.getMinutes();
                }
                else if (half_day_for_working_hours_dt.getMinutes() <= 9) {
                    half_day_for_working_hours = half_day_for_working_hours_dt.getHours() + ":0" + half_day_for_working_hours_dt.getMinutes();
                }
                else if (half_day_for_working_hours_dt.getHours() <= 9) {
                    half_day_for_working_hours = '0' + half_day_for_working_hours_dt.getHours() + ":" + half_day_for_working_hours_dt.getMinutes();
                }
                else {
                    half_day_for_working_hours = half_day_for_working_hours_dt.getHours() + ":" + half_day_for_working_hours_dt.getMinutes();
                }

                $("#txtmark_as_half_day_for_working_hours_less_than").val(half_day_for_working_hours);
            }

            if (res.weekly_off == 1) {
                $("#chkWeekOff").attr('checked', 'checked');
                $('.dyanamicTableToggle').css('display', 'block');
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

                //debugger;
                //alert(value.week_day + ' , ' + value.days);
                if (value.week_day == 1) {
                    $("input[name=chkMonday]").prop("checked", true);
                    $("input[id=chkMondayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 2) {
                    $("input[name=chkTuesday]").prop("checked", true);
                    $("input[id=chkTuesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 3) {
                    $("input[name=chkWednesday]").prop("checked", true);
                    $("input[id=chkWednesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 4) {
                    $("input[name=chkThursday]").prop("checked", true);
                    $("input[id=chkThursdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 5) {
                    $("input[name=chkFriday]").prop("checked", true);
                    $("input[id=chkFridayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 6) {
                    $("input[name=chkSaturday]").prop("checked", true);
                    $("input[id=chkSaturdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 7) {
                    $("input[name=chkSundayday]").prop("checked", true);
                    $("input[id=chkSundayDays" + value.days + "]").prop("checked", true);
                }

            });

            $('#loader').hide();

            // var res1 = data.ShiftWeekOff;
            // alert(data.ShiftWeekOff);
            //$.each(res.shiftWeekOff, function (key, value) {
            //    alert(key + ": " + value);
            //});

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });


}

function GetAllShift() {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiShift",
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
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
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Shift Details',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
                        }
                    },
                ],
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
                        //{
                        //    targets: [8],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetTimeFromDate(date);
                        //    }
                        //},
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                //var date = new Date(data);
                                return data == 0 ? "No" : data == 1 ? "Yes" : "-";
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
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
                        },
                        {
                            targets: [13],
                            render: function (data, type, row) {

                                //var date = new Date(data);
                                return data == 0 ? "No" : data == 1 ? "Yes" : "-";
                            }
                        },
                        {
                            targets: [14],
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
                    //{ "data": "grace_time_for_late_punch_out", "name": "grace_time_for_late_punch_out", "autoWidth": true },
                    { "data": "number_of_grace_time_applicable_in_month", "name": "number_of_grace_time_applicable_in_month", "autoWidth": true },
                    { "data": "is_lunch_punch_applicable", "name": "is_lunch_punch_applicable", "autoWidth": true },
                    { "data": "lunch_punch_in_time", "name": "lunch_punch_in_time", "autoWidth": true },
                    { "data": "lunch_punch_out_time", "name": "lunch_punch_out_time", "autoWidth": true },

                    { "data": "maximum_lunch_time", "name": "maximum_lunch_time", "autoWidth": true },
                    { "data": "is_ot_applicable", "name": "is_ot_applicable", "autoWidth": true },
                    { "data": "maximum_ot_hours", "name": "maximum_ot_hours", "autoWidth": true },
                    { "data": "is_default", "name": "is_default", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="CreateShift?shift_id=' + full.shift_id + '" title="Edit" style=" float: left;"><i class="fa fa-pencil-square-o"></i>Edit</a>  ';
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


function GetAllShiftByCompanyId(company_id) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiShift/Gettbl_shift_detailsByCompanyId/" + company_id,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

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
                    { "data": "is_default", "name": "is_default", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="CreateShift?shift_id=' + full.shift_id + '" title="Edit" style=" float: left;"><i class="fa fa-pencil-square-o"></i>Edit</a>  ';
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
