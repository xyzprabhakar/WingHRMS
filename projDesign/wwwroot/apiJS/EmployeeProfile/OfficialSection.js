
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

        var key = CryptoJS.enc.Base64.parse("#base64Key#");
        var iv = CryptoJS.enc.Base64.parse("#base64IV#");

        var employee_photo_path_dec = CryptoJS.AES.decrypt(localStorage.getItem("employee_photo_path"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

        BindCompanyList('ddlCompany', company_id);

        BindEmployeeCodee('ddlEmployeeCode', company_id, login_emp_id);
        Get_Details_Changes(login_emp_id);
        //BindLocationListForddl('ddllocation', company_id, login_emp_id);
        // BindDepartmentListForddl('ddldepartment', company_id, login_emp_id);



        GetEmployeeOfficaialDetail(login_emp_id);


        var profile_img_path = employee_photo_path_dec;

        $('#profile_imgg').attr("src", profile_img_path);


    }, 2000);// end timeout

});

function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;

    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

    $("#txtEmployeeCode").val(data[0].emp_code);
}


function GetEmployeeOfficaialDetail(employee_id) {

    //var defaultcompany = localStorage.getItem("company_id");
    //   defaultcompany = 26;
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {



            if (data == undefined || data.statusCode != undefined) {
                BindDepartmentListForddl('ddldepartment', company_id, 0);
                BindLocationListForddl('ddllocation', company_id, 0);
                BindSubDepartmentListForddl('ddlsubdepartment', 0, 0);
                BindReligionList('ddlReligion', 0);
                BindSubLocationListForddl('ddlsublocation', 0, 0);
                reset_all();

                if (data.statusCode != undefined) {
                    messageBox("error", data.message);
                    $('#loader').hide();
                    return false;
                }
                $('#loader').hide();
            }


            if (data.location_id != null) {
                BindLocationListForddl('ddllocation', company_id, data.location_id);
            }
            else {
                BindLocationListForddl('ddllocation', company_id, 0);
            }
            if (data.department_id != null) {
                BindDepartmentListForddl('ddldepartment', company_id, data.department_id);
                BindSubDepartmentListForddl('ddlsubdepartment', data.department_id, (data.sub_dept_id != null ? data.sub_dept_id : 0));
            }
            else {
                BindDepartmentListForddl('ddldepartment', company_id, 0);
            }

            if (data.sub_location_id != null) {
                BindSubLocationListForddl('ddlsublocation', (data.location_id != null ? data.location_id : 0), data.sub_location_id)
            }
            else {
                BindSubLocationListForddl('ddlsublocation', (data.location_id != null ? data.location_id : 0), 0);
            }

            if (data.state_id != null) {
                BindStateList('ddlstate', data.state_id, 1); // all state bind of India
                $("#ddlstate option[value='0']").text('---Please Select---');
            }


            $("#ddlgender").val(data.gender);
            $("#txtCardNo").val(data.card_number);
            $("#ddlSalutation").val(data.salutation);
            $("#txtFirstName").val(data.employee_first_name);
            $("#txtMiddleName").val(data.employee_middle_name);
            $("#txtLastName").val(data.employee_last_name);

            $("#txtGroupJoiningDate").val(GetDateFormatddMMyyyy(new Date(data.group_joining_date)));
            //$("#txtDateOfJoining").val(GetOnlyDate(data.date_of_birth));

            $("#txtDateOfJoining").val(GetDateFormatddMMyyyy(new Date(data.date_of_joining)));
            $("#txtDepartmentJoiningDate").val(GetDateFormatddMMyyyy(new Date(data.department_date_of_joining)));
            $("#txtDateOfBirth").val(GetDateFormatddMMyyyy(new Date(data.date_of_birth)));

            if (data.religion_id != null) {
                BindReligionList('ddlReligion', data.religion_id);
            }
            else {
                BindReligionList('ddlReligion', 0);
            }


            $("#ddlMaritalStatus").val(data.marital_status);
            $("#ddlpunch_type").val(data.punch_type);

            $("#txtHrSpoc").val(data.hr_spoc);
            $("#txtOfficialEmailID").val(data.official_email_id);


            if (data.is_ot_allowed == 1) {
                $("#chkOtAllowes").attr('checked', 'checked');
            }
            if (data.is_comb_off_allowed == 1) {
                $("#chkCombOff").attr('checked', 'checked');
            }

            $("#txtMobilePunchFromDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_from_date)));
            $("#txtMobilePunchToDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_to_date)));
            // $("#hdnEmployee_img").val(data.employee_photo_path);
            $("#txtpunch_remarks").val(data.remarks);
            $("#ddlusertype").val(data.user_type);

            $("#txtnotice_period").val(data.notice_period);
            $("#ddlemployment_type").val(data.employement_type);
            $("#txtconfirmation_date").val(GetDateFormatddMMyyyy(new Date(data.confirmation_date)));









            // //(JSON.stringify(data));
            // BindLocationListForddl('ddllocation', company_id, data.location_id);

            // if (data.department_id != null) {
            //     BindDepartmentListForddl('ddldepartment', company_id, data.department_id);

            //     BindSubDepartmentListForddl('ddlsubdepartment', data.department_id, data.sub_dept_id);

            // }


            // $("#txtCardNo").val(data.card_number);
            // $("#ddlSalutation").val(data.salutation);
            // $("#txtFirstName").val(data.employee_first_name);
            // $("#txtMiddleName").val(data.employee_middle_name);
            // $("#txtLastName").val(data.employee_last_name);

            // $("#txtGroupJoiningDate").val(GetOnlyDate(data.group_joining_date));
            // $("#txtDateOfJoining").val(GetOnlyDate(data.date_of_joining));

            // $("#txtDepartmentJoiningDate").val(GetOnlyDate(data.department_date_of_joining));
            // $("#txtDateOfBirth").val(GetOnlyDate(data.date_of_birth));

            // BindReligionList('ddlReligion', data.religion_id);

            // $("#ddlMaritalStatus").val(data.marital_status);

            // $("#txtHrSpoc").val(data.hr_spoc);
            // $("#txtOfficialEmailID").val(data.official_email_id);


            // if (data.is_ot_allowed == 1) {
            //     $("#chkOtAllowes").attr('checked', 'checked');
            // }
            // if (data.is_comb_off_allowed == 1) {
            //     $("#chkCombOff").attr('checked', 'checked');
            // }
            //// $("#txtMobilePunchFromDate").val(GetOnlyDate(data.mobile_punch_from_date));
            // // $("#txtMobilePunchToDate").val(GetOnlyDate(data.mobile_punch_to_date));

            // $("#txtpunchtype").val(data.punch_type == "0" ? "Single Punch,Absent" : data.punch_type == "1" ? "Single Punch,Present" : data.punch_type=="2"?"Punch Exempted":"");
            // $("#hdnEmployee_img").val(data.employee_photo_path);
            // $("#ddlgender").val(data.gender);
            // if (data.current_employee_type != null) {
            //     BindEmployementTypeForddl('ddlemploymenttype', data.current_employee_type);
            // }
            // $("#txtpunch_remarks").val(data.remarks);

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", "Server busy please try again later...!");
        }
    });

}


function Get_Details_Changes(employee_id) {
    //debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeOfficial_changes/' + employee_id,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var data = res;
            debugger;
            if (data.statusCode != undefined) {
                messageBox("info", data.message);
                $('#loader').hide();
                return false;
            }
            if (data.state_id != null && data.state_id != "" && data.state_id != "0") {
                $("#lbl_state").css("color", "red");
            }
            if (data.location_id != null && data.location_id != "" && data.location_id != "0") {
                $("#lbl_location").css("color", "red");
            }
            if (data.department_id != null && data.department_id != "" && data.department_id != "0") {
                $("#lbl_department").css("color", "red");
            }
            if (data.sub_dept_id != null && data.sub_dept_id != "") {
                $("#lbl_sub_department").css("color", "red");
            }
            //if (data.date_of_birth != null && data.date_of_birth != "") {
            //    $("#lbl_employee_code").css("color", "red");
            //}
            if (data.card_number != null && data.card_number != "") {
                $("#lbl_card_no").css("color", "red");
            }
            if (data.gender != null && data.gender != "") {
                $("#lbl_gender").css("color", "red");
            }
            if (data.salutation != null && data.salutation != "") {
                $("#lbl_salutation").css("color", "red");
            }
            if (data.employee_first_name != null && data.employee_first_name != "") {
                $("#lbl_first_name").css("color", "red");
            }
            if (data.employee_middle_name != null && data.employee_middle_name != "") {
                $("#lbl_middle_name").css("color", "red");
            }
            if (data.employee_last_name != null && data.employee_last_name != "") {
                $("#lbl_last_name").css("color", "red");
            }
            if (data.group_joining_date != null && data.group_joining_date != "") {
                $("#lbl_group_joining_date").css("color", "red");
            }
            if (data.date_of_joining != null && data.date_of_joining != "") {
                $("#lbl_date_of_joining").css("color", "red");
            }
            if (data.date_of_birth != null && data.date_of_birth != "") {
                $("#lbl_dob").css("color", "red");
            }
            if (data.department_date_of_joining != null && data.department_date_of_joining != "") {
                $("#lbl_department_joining_date").css("color", "red");
            }
            if (data.religion_id != null && data.religion_id != "") {
                $("#lbl_religion").css("color", "red");
            }
            if (data.marital_status != null && data.marital_status != "") {
                $("#lbl_marital_status").css("color", "red");
            }
            if (data.hr_spoc != null && data.hr_spoc != "") {
                $("#lbl_hr_spoc").css("color", "red");
            }
            if (data.official_email_id != null && data.official_email_id != "") {
                $("#lbl_official_email").css("color", "red");
            }
            if (data.user_type != null && data.user_type != "") {
                $("#lbl_emp_category").css("color", "red");
            }
            if (data.notice_period != null && data.notice_period != "") {
                $("#lbl_notice_period").css("color", "red");
            }
            if (data.employement_type != null && data.employement_type != "") {
                $("#lbl_employment_type").css("color", "red");
            }
            if (data.confirmation_date != null && data.confirmation_date != "") {
                $("#lbl_confirmation_date").css("color", "red");
            }
            if (data.is_ot_allowed != null && data.is_ot_allowed != "") {
                $("#lbl_ot_allow").css("color", "red");
            }
            if (data.is_comb_off_allowed != null && data.is_comb_off_allowed != "") {
                $("#lbl_comp_off").css("color", "red");
            }
            if (data.is_mobile_attendence_access != null && data.is_mobile_attendence_access != "") {
                $("#lbl_mobile_attendance").css("color", "red");
            }
            if (data.punch_type != null && data.punch_type != "") {
                $("#lbl_punch_type").css("color", "red");
            }

            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}
