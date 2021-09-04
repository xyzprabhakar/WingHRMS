var login_emp_id;
var default_company;
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        reset_all();
        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindDepartmentListForddl('ddldepartment', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            $("#ddldepartment option[value='0']").text('---Please Select---');
            BindLocationListForddl('ddllocation', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            $("#ddllocation option[value='0']").text('---Please Select---');
            if (localStorage.getItem("new_emp_id") != null) {

                BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
                $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
                $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
                $("#txtEmployeeCode").val($('#ddlEmployeeCode :selected').text());
                GetEmployeeOfficaialDetail(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

            }
            else {
                BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            }

        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));

            BindDepartmentListForddl('ddldepartment', default_company, 0);
            $("#ddldepartment option[value='0']").text('---Please Select---');
            BindLocationListForddl('ddllocation', default_company, 0);
            $("#ddllocation option[value='0']").text('---Please Select---');

        }

        //BindSubDepartmentListForddl('ddlsubdepartment', 0, 0);
        BindReligionList('ddlReligion', 0);
        //BindSubLocationListForddl('ddlsublocation', 0, 0);
        BindStateList('ddlstate', 0, 1); // all state bind of India
        $("#ddlstate option[value='0']").text('---Please Select---');



        EL("File1").addEventListener("change", readFile, false);

        // BindEmployementTypeForddl('ddl_emloyement_type', 0);


        $('#loader').hide();

        $('#ddlEmployeeCode').change(function () {
            //  localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            $("#txtEmployeeCode").val($('#ddlEmployeeCode :selected').text());
            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                Get_Details_Changes($(this).val());
                GetEmployeeOfficaialDetail($(this).val());
            }


        });


        $('#ddldepartment').bind("change", function () {
            $('#loader').show();
            BindSubDepartmentListForddl('ddlsubdepartment', $(this).val(), 0);
            $('#loader').hide();
        });




        $('#txtOfficialEmailID').bind("change", function () {
            $('#loader').show();
            ValidEmailId("txtOfficialEmailID");
            $('#loader').hide();
        });


        $("#ddldepartment").bind("change", function () {
            BindSubDepartmentListForddl('ddlsubdepartment', $(this).val(), 0);
        });


        $('#ddlCompany').change(function () {
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);
            //  localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            $("#ddldepartment option[value='0']").text('---Please Select---');
            BindLocationListForddl('ddllocation', $(this).val(), 0);
            $("#ddllocation option[value='0']").text('---Please Select---');
            BindSubDepartmentListForddl('ddlsubdepartment', 0, 0);
            reset_all();
        });

        //$("#ddlstate").bind("change", function () {           
        //    BindCityList('ddllocation', $(this).val(), 0);
        //   // BindLocationByCompanyState('ddllocation', default_company, stateid, 0);          
        //});
        //$("#ddllocation").bind("change", function () {
        //    BindSubLocationListForddl('ddlsublocation', $(this).val(), 0);
        //});


        $("#txtDateOfJoining").val("09-Sep-9999");
        $("#txtGroupJoiningDate").val("09-Sep-9999");
        $("#txtDateOfBirth").val("09-Sep-9999");
        $("#txtDepartmentJoiningDate").val("09-Sep-9999");
        $("#txtconfirmation_date").val("09-Sep-9999");

    }, 2000);// end timeout

});




$('#btnReloadEmp').bind("click", function () {

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/ReloadEmployeeCode/' + login_emp_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var emp_under_login_emp = data;

            window.localStorage.removeItem("emp_under_login_emp");

            localStorage.setItem("emp_under_login_emp", JSON.stringify(emp_under_login_emp));

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

});

$('#btnSaveOfficialDetails').bind("click", function () {


    var employee_id = $('#ddlEmployeeCode :selected').val();

    var ddlCompany = $("#ddlCompany").val();
    var ddllocation = $("#ddllocation").val();
    var ddldepartment = $("#ddldepartment").val();
    var ddlsubdepartment = $("#ddlsubdepartment").val();
    var ddlsublocation = $("#ddlsublocation").val();
    var ddlstate = $("#ddlstate").val();
    var ddlgender = $("#ddlgender").val();
    var txtCardNo = $("#txtCardNo").val();
    var ddlSalutation = $("#ddlSalutation").val();
    var txtFirstName = $("#txtFirstName").val();

    var txtMiddleName = $("#txtMiddleName").val();
    var txtLastName = $("#txtLastName").val();
    var txtDateOfJoining = $("#txtDateOfJoining").val();

    var punch_type = $("#ddlpunch_type").val();
    var remarks = $("#txtpunch_remarks").val();
    var _usertype = $("#ddlusertype").val();
    var ddlReligion = $("#ddlReligion").val();
    var ddlMaritalStatus = $("#ddlMaritalStatus").val();
    var txtOfficialEmailID = $("#txtOfficialEmailID").val();
    var ddlemploymenttype = $("#hdnemptype").val();//$("#ddlemploymenttype").val();
    var txtHrSpoc = $("#txtHrSpoc").val();
    var txtGroupJoiningDate = $("#txtGroupJoiningDate").val();

    var txtconfirmation_date = $("#txtconfirmation_date").val();
    var ddlemployment_type = $("#ddlemployment_type").val();
    var txtnotice_period = $("#txtnotice_period").val();


    // var _employementtype = $("#ddl_emloyement_type").val();
    //var empmnt_type_dt = $("#txteffect_empmnt_type_dt").val();



    //if (ddlCompany == null || ddlCompany == '' || ddlCompany == 0) {
    //    messageBox("error", "Please Select Company...!");
    //    $("#ddlCompany").focus();
    //    return;
    //}

    //if (employee_id == null || employee_id == '' || employee_id == 0) {
    //    messageBox("error", "Please Select Employee Code...!");
    //    $("#ddlEmployeeCode").focus();
    //    return;
    //}

    if (ddlstate == "0" || ddlstate == null || ddlstate == "") {
        messageBox("error", "Please Select State...!");
        $("#ddlstate").focus();
        return;
    }
    if (ddllocation == "0" || ddllocation == null || ddllocation == "") {
        messageBox("error", "Please Select Location...!");
        $("#ddllocation").focus();
        return;
    }
    if (ddldepartment == "0" || ddldepartment == null || ddldepartment == "") {
        messageBox("error", "Please Select Department...!");
        $("#ddldepartment").focus();
        return;
    }
    if (ddlsubdepartment == null || ddlsubdepartment == "") {
        messageBox("error", "Please Select Sub Department...!");
        $("#ddlsubdepartment").focus();
        return;
    }
    if (txtCardNo == null || txtCardNo == "") {
        messageBox("error", "Please Enter Card Number...!");
        $("#txtCardNo").focus();
        return;
    }

    if (txtFirstName == null || txtFirstName == "") {
        messageBox("error", "Please enter first Name...!");
        $("#txtFirstName").focus();
        return;
    }

    if (txtGroupJoiningDate == null || txtGroupJoiningDate == "") {
        messageBox("error", "Please Select Group Date of Joining....!");
        $("#txtGroupJoiningDate").focus();
        return;
    }

    if (txtDateOfJoining == null || txtDateOfJoining == "") {
        messageBox("error", "Please Select Date of Joining....!");
        $("#txtDateOfJoining").focus();
        return;
    }

    if (txtDateOfBirth == null || txtDateOfBirth == "") {
        messageBox("error", "Please Select Date of Birth....!");
        $("#txtDateOfBirth").focus();
        return;
    }
    if (txtHrSpoc == "" || txtHrSpoc == null) {
        messageBox("error", "Please enter Hr Spoc....!");
        $("#txtHrSpoc").focus();
        return;
    }



    if (txtOfficialEmailID == null || txtOfficialEmailID == "") {
        messageBox("error", "Please Enter Official Email id....!");
        $("#txtOfficialEmailID").focus();
        return;
    }

    if (ddlMaritalStatus == "0" || ddlMaritalStatus == null || ddlMaritalStatus == "") {
        messageBox("error", "Please Select Marital Status....!");
        $("#ddlMaritalStatus").focus();
        return;
    }
    if (ddlpunch_type == "0" || ddlpunch_type == null || ddlpunch_type == "") {
        messageBox("error", "Please Select Punch Type....!");
        $("#ddlpunch_type").focus();
        return;
    }
    if (_usertype == null || _usertype == "" || _usertype == "0") {
        messageBox("error", "Please Select User Type");
        $("#ddlusertype").focus();
        return false;
    }



    if ($("#txtDateOfBirth").val() != '') {
        txtDateOfBirth = $("#txtDateOfBirth").val();
    }


    var chkOtAllowes = 0; // Not applicable
    if ($("#chkOtAllowes").is(":checked")) {
        chkOtAllowes = 1; //applicable
    }

    var chkCombOff = 0; // Not applicable
    if ($("#chkCombOff").is(":checked")) {
        chkCombOff = 1; //applicable
    }

    var chkIsMobileAttendance = 0; // Not applicable
    if ($("#chkIsMobileAttendance").is(":checked")) {
        chkIsMobileAttendance = 1; //applicable
    }

    var txtMobilePunchFromDate = "2000-01-01";

    if ($("#txtMobilePunchFromDate").val() != '') {
        txtMobilePunchFromDate = $("#txtMobilePunchFromDate").val();
    }

    var txtMobilePunchToDate = "2000-01-01";

    if ($("#txtMobilePunchToDate").val() != '') {
        txtMobilePunchToDate = $("#txtMobilePunchToDate").val();
    }

    var employee_photo_path = "";
    if ($("#HFb64").val() != '') {
        employee_photo_path = $("#HFb64").val();
    }
    //else {
    //    employee_photo_path = $("#hdnEmployee_img").val();
    //}

    ////start age calculation
    //var dob = new Date(txtDateOfBirth);
    //var today = new Date();
    //var age = Math.floor((today - dob) / (365.25 * 24 * 60 * 60 * 1000));
    //// alert(age + ' years old');
    //// $('#age').html(age + ' years old');
    //// end age caluclation

    //if (parseInt(age) < 18 || parseInt(age) > 70) {
    //    messageBox("error", "Employee age should be in between 18 to 70");
    //    $("#txtDateOfBirth").focus();
    //    return false;
    //}
    ////calculate age end
    //if (_employementtype == "" || _employementtype == "0" || _employementtype == null) {
    //    messageBox("error", "Please Select Employement Type");
    //    $("#ddl_emloyement_type").focus();
    //    return false;
    //}

    txtFirstName = $.trim(txtFirstName);
    txtLastName = $.trim(txtLastName);
    txtDepartmentJoiningDate = $("#txtDepartmentJoiningDate").val();
    txtDateOfBirth = $("#txtDateOfBirth").val();


    var myData = {
        'employee_id': employee_id,
        'is_applicable_for_all_comp': ddlCompany,
        'location_id': ddllocation,
        'state_id': ddlstate,
        'sub_location_id': 0,//ddlsublocation,
        'department_id': ddldepartment,
        'sub_dept_id': ddlsubdepartment,
        'card_number': txtCardNo,
        'gender': ddlgender,
        'salutation': ddlSalutation,
        'employee_first_name': txtFirstName,
        'employee_middle_name': txtMiddleName,
        'employee_last_name': txtLastName,
        'group_joining_date': txtGroupJoiningDate,
        'date_of_joining': txtDateOfJoining,
        'department_date_of_joining': txtDepartmentJoiningDate,
        'date_of_birth': txtDateOfBirth,
        'religion_id': ddlReligion,
        'marital_status': ddlMaritalStatus,
        'hr_spoc': txtHrSpoc,
        'official_email_id': txtOfficialEmailID,
        'is_ot_allowed': chkOtAllowes,
        'is_comb_off_allowed': chkCombOff,
        'employee_photo_path': employee_photo_path,
        // 'current_employee_type': $("#hdnemptype").val(),//ddlemploymenttype,
        'mobile_punch_from_date': txtMobilePunchFromDate,
        'mobile_punch_to_date': txtMobilePunchToDate,
        'is_deleted': 0,
        'created_by': login_emp_id,
        'last_modified_by': login_emp_id,
        'punch_type': punch_type,
        'remarks': remarks,
        //'current_employee_type': _employementtype,
        'user_type': _usertype,
        'notice_period': txtnotice_period,
        'employement_type': ddlemployment_type,
        'confirmation_date': txtconfirmation_date,
        'is_mobile_access': chkIsMobileAttendance,
    }

    $('#loader').show();

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/Posttbl_emp_officaial_sec',
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
               // location.reload();
                //window.location.href = '/Employee/OfficaialSection';
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
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);

        }
    });

});



//Get Employee Code
function GetEmployeeOfficaialDetail(employee_id) {

    if (employee_id > 0) {
        var defaultcompany = $('#ddlCompany :selected').val();
        //   defaultcompany = 26;
        $('#loader').show();

        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {
                //(JSON.stringify(data));

                if (data == undefined || data.statusCode != undefined)
                {
                    BindDepartmentListForddl('ddldepartment', defaultcompany, 0);
                    $("#ddldepartment option[value='0']").text('---Please select---');
                  
                    //$("#ddllocation option[value='0']").text('---Please select---');
                    // BindLocationListForddl('ddllocation', defaultcompany, 0);

                    BindSubDepartmentListForddl('ddlsubdepartment', 0, 0);
                    BindReligionList('ddlReligion', 0);
                    // BindSubLocationListForddl('ddlsublocation', 0, 0);
                    reset_all();

                    if (data.statusCode != undefined) {
                        messageBox("error", data.message);
                        $('#loader').hide();
                        return false;
                    }
                    $('#loader').hide();
                }
                debugger;
                if (data.state_id != null) {
                    BindStateList('ddlstate', data.state_id, 1); // all state bind of India
                    $("#ddlstate option[value='0']").text('---Please Select---');
                }
                //if (data.state_id != null) {
                //    $("#ddlstate option[value='" + data.state_id + "']").attr('selected', 'selected');
                //    // $("#ddlstate").val(data.state_id);
                //}
                if (data.location_id != null) {
                   // BindCityList('ddllocation', data.state_id, data.location_id);
                   // BindLocationByCompanyState('ddllocation', defaultcompany, data.state_id, data.location_id);
                    BindLocationListForddl('ddllocation', defaultcompany, data.location_id);
                    $("#ddllocation option[value='0']").text('---Please select---');
                }
                else {
                  //  BindLocationListForddl('ddllocation', defaultcompany, 0);
                    $("#ddllocation option[value='0']").text('---Please Select---');
                }

                if (data.department_id != null) {
                    BindDepartmentListForddl('ddldepartment', defaultcompany, data.department_id);
                    $("#ddldepartment option[value='0']").text('---Please Select---');
                    BindSubDepartmentListForddl('ddlsubdepartment', data.department_id, (data.sub_dept_id != null ? data.sub_dept_id : 0));
                }
                else {
                    BindDepartmentListForddl('ddldepartment', defaultcompany, 0);
                    $("#ddldepartment option[value='0']").text('---Please Select---');
                }

                //if (data.sub_location_id != null) {
                //    BindSubLocationListForddl('ddlsublocation', (data.location_id != null ? data.location_id : 0), data.sub_location_id)
                //}
                //else {
                //    BindSubLocationListForddl('ddlsublocation', (data.location_id != null ? data.location_id : 0), 0);
                //}
                debugger;
                $("#ddlgender").val(data.gender);
                $("#txtCardNo").val(data.card_number);
                $("#ddlSalutation").val(data.salutation);
                $("#txtFirstName").val(data.employee_first_name);
                $("#txtMiddleName").val(data.employee_middle_name);
                $("#txtLastName").val(data.employee_last_name);

                $("#txtGroupJoiningDate").val(GetDateFormatddMMyyyy(new Date(data.group_joining_date)));
                //$("#txtDateOfJoining").val(GetDateFormatddMMyyyy(data.date_of_birth));

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

                //if (data.current_employee_type != null) {
                //    BindEmployementTypeForddl('ddl_emloyement_type', data.current_employee_type);
                //}


                //if (data.empmnt__id != null) {
                //    BindEmployementTypeinlabel('lblemptype', data.empmnt__id);
                //}
                //else if (data.current_employee_type != null) {
                //    BindEmployementTypeinlabel('lblemptype', data.current_employee_type);
                //}

                //$("#hdnemptype").val(data.current_employee_type);

                //$("#lblemptype").html(data.current_employee_type);
                //if (data.empmnt__id != null) {
                //    BindEmployementType('ddlemploymenttype', data.empmnt__id);
                //}
                //else if (data.current_employee_type != null) {
                //    BindEmployementType('ddlemploymenttype', data.current_employee_type);
                //}
                //else {
                //    BindEmployementType('ddlemploymenttype', data.current_employee_type);
                //}


                if (data.is_ot_allowed == 1) {
                    $("#chkOtAllowes").attr('checked', 'checked');
                }
                if (data.is_comb_off_allowed == 1) {
                    $("#chkCombOff").attr('checked', 'checked');
                }
                else {
                    $("#chkCombOff").prop("checked", false);
                }
                if (data.is_mobile_access == 1) {
                    $("#chkIsMobileAttendance").attr('checked', 'checked');
                }

                $("#txtMobilePunchFromDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_from_date)));
                $("#txtMobilePunchToDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_to_date)));
                // $("#hdnEmployee_img").val(data.employee_photo_path);
                $("#txtpunch_remarks").val(data.remarks);
                $("#ddlusertype").val(data.user_type);
                $("#txtnotice_period").val(data.notice_period);
                $("#ddlemployment_type").val(data.employement_type);
                $("#txtconfirmation_date").val(GetDateFormatddMMyyyy(new Date(data.confirmation_date)));


                $('#loader').hide();
            },
            error: function (error) {
                messageBox("info", 'No record found...!');
                $('#loader').hide();
            }
        });

    }


}


function readFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 500) {
            $("#File1").val("");
            alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#File1").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "PNG" || Extension == "JPEG" || Extension == "JPG") {

            }
            else {
                $("#File1").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

function getAge(dateString) {
    var today = new Date();
    var birthDate = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}



function BindEmployementTypeinlabel(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetEmployeeType';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $.each(res, function (data, value) {
                if (parseInt(value.emptypeid) == parseInt(SelectedVal)) {
                    $(ControlId).html(value.emptypename);
                }
            });
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function reset_all() {

    $("#txtEmployeeCode").val('');
    $("#txtCardNo").val('');
    $("#ddlgender").val('0');
    $("#ddlSalutation").val('');
    $("#txtFirstName").val('');
    $("#txtMiddleName").val('');
    $("#txtLastName").val('');
    $("#txtGroupJoiningDate").val('');
    $("#txtDateOfJoining").val('');
    $("#txtDateOfBirth").val('');
    $("#txtDepartmentJoiningDate").val('');
    $("#ddlMaritalStatus").val('0');
    $("#txtHrSpoc").val('');
    $("#txtOfficialEmailID").val('');
    // BindEmployementTypeForddl('ddl_emloyement_type', 0);
    // $("#lblemptype").val('');
    $("#chkOtAllowes").prop("checked", false);
    $("#chkCombOff").prop("checked", true);
    $("#ddlpunch_type").val('0');
    $("#txtpunch_remarks").val('');
    $("#ddlusertype").val(0);
    $("#txtDateOfJoining").val("09-Sep-9999");
    $("#txtGroupJoiningDate").val("09-Sep-9999");
    $("#txtDateOfBirth").val("09-Sep-9999");
    $("#txtDepartmentJoiningDate").val("09-Sep-9999");

}


function BindLocationByCompanyState(ControlId, CompanyId, stateid, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationByCompanyState/' + CompanyId + "/" + stateid,
        data: {},
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;



            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            if (res.statusCode != undefined) {
                $("#loader").hide();
                messageBox("error", res.message);
                return false;
            }

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
            });


            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
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