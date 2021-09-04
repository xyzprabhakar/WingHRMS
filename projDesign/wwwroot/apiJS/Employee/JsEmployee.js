$('#loader').show();

var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        var employee_id = "";
        if (localStorage.getItem("new_employee_id") != null) {
            employee_id = CryptoJS.AES.decrypt(localStorage.getItem("new_employee_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        }



        var employee_code = localStorage.getItem('new_employee_code');


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlDefaultCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlDefaultCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }

        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlDefaultCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            //  GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        }


        //$("#txtEmployeeCode").val(employee_code);


        if (default_company != null) {
            BindGradeList('ddlgrade', default_company, 0)
            BindDesignationList('ddldesignation', default_company, 0)

            BindEmpNameAndCodeByeComp('ddlEmployeeManager1', default_company, 0);
            BindEmpNameAndCodeByeComp('ddlEmployeeManager2', default_company, 0);
            BindEmpNameAndCodeByeComp('ddlEmployeeManager3', default_company, 0);

        }

        //BindCompanyListForddl('ddlCompany', default_company);





        BindReligionList('ddlReligion', 0);
        BindEmployementType('ddlemploymenttype', 0);

        $("#File1").bind("change", function () {

            EL("File1").addEventListener("change", readFile, false);
        });




        $('#loader').hide();


        $('#ddlEmployeeCode').bind("change", function () {
            $("#txtEmployeeCode").val($('#ddlEmployeeCode :selected').text());


            localStorage.setItem("new_employee_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

            GetEmployeeOfficaialDetail($(this).val());
            GetEmployeeWeekOff($(this).val());

        });

        $('#ddlDefaultCompany').bind("change", function () {

            //if ($(this).val() != 0) {
            BindLocationListForddl('ddllocation', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            $("#ddlEmployeeCode option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            BindShiftListForddl('ddlShift', $(this).val(), 0);

            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            // GetEmployeeOfficaialDetail(employee_id);

            $("#ddlEmployeeManager1 option").remove();
            BindEmpNameAndCodeByeComp('ddlEmployeeManager1', $(this).val(), 0);
            $("#ddlEmployeeManager2 option").remove();
            BindEmpNameAndCodeByeComp('ddlEmployeeManager2', $(this).val(), 0);
            $("#ddlEmployeeManager3 option").remove();
            BindEmpNameAndCodeByeComp('ddlEmployeeManager3', $(this).val(), 0);
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            // Check_NoofEmployee($(this).val());
            //}
        });

        $('#ddldepartment').bind("change", function () {
            BindSubDepartmentListForddl('ddlsubdepartment', $(this).val(), 0);
        });

        $('#nexttabsBTN').bind("click", function () {
            debugger;
            $('#loader').show();

            var ddlDefaultCompany = $("#ddlDefaultCompany").val();

            if (ddlDefaultCompany == 0) {
                messageBox("error", "Select Default Company...!");
                $('#loader').hide();
                return;
            }

            var txtEmployeeCode = $("#txtEmployeeCode").val();

            if (txtEmployeeCode == '') {
                messageBox("error", "Please Enter Employee Code...!");
                $('#loader').hide();
                return;
            }
            var patt1 = /[^0-9a-zA-Z]/g;
            var expressionresult = txtEmployeeCode.match(patt1);
            if (expressionresult != null) {
                messageBox("error", "Employee code only alphanumeric!");
                $('#loader').hide();
                return;
            }

            var checkstr = confirm('Are you sure to create a new Employee...?');
            if (checkstr == true) {
                // do your codeddlDefaultCompany

            } else {
                return false;
            }

            var emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var myData = {
                'default_company_id': ddlDefaultCompany,
                'is_deleted': 0,
                'created_by': emp_idd,
                'last_modified_by': emp_idd,
                'username': txtEmployeeCode,
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/CreateEmployee',
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    debugger;
                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    var employee_id = data.employee_id;
                    var emp_code = data.emp_code;
                    $('#loader').hide();
                    _GUID_New();


                    //if data save
                    if (employee_id != "") {
                        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + employee_id + "'", localStorage.getItem("sit_id")));
                        localStorage.setItem("new_employee_code", emp_code);

                        localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + ddlDefaultCompany + "'", localStorage.getItem("sit_id")));
                        if (statuscode == "3") {
                            messageBox("info", Msg);
                        }
                        else {
                            if (emp_code == "undefined" || emp_code == null || emp_code == "") {
                                messageBox("error", "Unable to Create Employee, Please try after some time");
                                return false;
                            }
                            else {


                                alert('Employee Successfully created...! Employee Code:- ' + emp_code + '');
                                window.location.href = '/Employee/OfficaialSection';
                            }

                        }


                    }

                    else if (statuscode == "0") {
                        messageBox("error", "Something went wrong please try again...!");
                    }

                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
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
        });

        // Save Shift Master Data
        $('#btnSaveOfficialDetails').bind("click", function () {
            $('#loader').show();

            var employee_id = $('#ddlEmployeeCode :selected').val();

            var ddlCompany = $("#ddlCompany").val();
            var ddllocation = $("#ddllocation").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddlsubdepartment = $("#ddlsubdepartment").val();
            var txtCardNo = $("#txtCardNo").val();
            var ddlSalutation = $("#ddlSalutation").val();
            var txtFirstName = $("#txtFirstName").val();
            var txtMiddleName = $("#txtMiddleName").val();
            var txtLastName = $("#txtLastName").val();

            var txtGroupJoiningDate = "2000-01-01";

            if ($("#txtGroupJoiningDate").val() != '') {
                txtGroupJoiningDate = $("#txtGroupJoiningDate").val();
            }

            var txtDateOfJoining = "2000-01-01";

            if ($("#txtDateOfJoining").val() != '') {
                txtDateOfJoining = $("#txtDateOfJoining").val();
            }

            var txtDepartmentJoiningDate = "2000-01-01";

            if ($("#txtDepartmentJoiningDate").val() != '') {
                txtDepartmentJoiningDate = $("#txtDepartmentJoiningDate").val();
            }

            var txtDateOfBirth = "2000-01-01";

            if ($("#txtDateOfBirth").val() != '') {
                txtDateOfBirth = $("#txtDateOfBirth").val();
            }

            var ddlReligion = $("#ddlReligion").val();
            var ddlMaritalStatus = $("#ddlMaritalStatus").val();
            var txtOfficialEmailID = $("#txtOfficialEmailID").val();
            var ddlemploymenttype = $("#ddlemploymenttype").val();
            var txtHrSpoc = $("#txtHrSpoc").val();

            var chkOtAllowes = 0; // Not applicable
            if ($("#chkOtAllowes").is(":checked")) {
                chkOtAllowes = 1; //applicable
            }

            var chkCombOff = 0; // Not applicable
            if ($("#chkCombOff").is(":checked")) {
                chkCombOff = 1; //applicable
            }


            var txtMobilePunchFromDate = "2000-01-01";

            if ($("#txtMobilePunchFromDate").val() != '') {
                txtMobilePunchFromDate = $("#txtMobilePunchFromDate").val();
            }

            var txtMobilePunchToDate = "2000-01-01";

            if ($("#txtMobilePunchToDate").val() != '') {
                txtMobilePunchToDate = $("#txtMobilePunchToDate").val();
            }

            var employee_photo_path = null;
            if ($("#HFb64").val() != '') {
                employee_photo_path = $("#HFb64").val();
            }
            else {
                employee_photo_path = $("#hdnEmployee_img").val();
            }




            if (ddllocation == 0) {
                messageBox("error", "Select Location...!");
                $('#loader').hide();
                return;
            }

            if (ddldepartment == 0) {
                messageBox("error", "Select Department...!");
                $('#loader').hide();
                return;
            }
            if (ddlsubdepartment == 0) {
                messageBox("error", "Select Sub Department...!");
                $('#loader').hide();
                return;
            }


            if (ddlReligion == 0) {
                messageBox("error", "Select Religion...!");
                $('#loader').hide();
                return;
            }
            if (ddlemploymenttype == "") {
                messageBox("error", "Please Select Employment Type...!");
                $('#loader').hide();
                return;
            }


            var emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            var myData = {
                'employee_id': employee_id,
                'is_applicable_for_all_comp': ddlCompany,
                'location_id': ddllocation,
                'department_id': ddldepartment,
                'sub_dept_id': ddlsubdepartment,
                'card_number': txtCardNo,
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
                'current_employee_type': ddlemploymenttype,
                'mobile_punch_from_date': txtMobilePunchFromDate,
                'mobile_punch_to_date': txtMobilePunchToDate,
                'is_deleted': 0,
                'created_by': emp_idd,
                'last_modified_by': emp_idd,
            }


            console.log(JSON.stringify(myData));

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

                        messageBox("success", Msg);

                        window.location.href = '/Employee/OfficaialSection';
                    }
                    else if (statuscode == "0") {
                        messageBox("success", Msg);
                        $('#loader').hide();
                    }
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
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

        });


    }, 2000);// end timeout

});

//Get Employee Code
function GetEmployeeCode(company_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeCode/' + company_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var EmployeeCode = data.message;
            $("#txtEmployeeCode").val(EmployeeCode);
            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}


//Get Employee Code
function GetEmployeeOfficaialDetail(employee_id) {
    $('#loader').show();
    var defaultcompany = CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //   defaultcompany = 26;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            //(JSON.stringify(data));
            BindLocationListForddl('ddllocation', defaultcompany, data.location_id);

            if (data.department_id != null) {
                BindDepartmentListForddl('ddldepartment', defaultcompany, data.department_id);
                BindSubDepartmentListForddl('ddlsubdepartment', data.department_id, data.sub_dept_id);
            }


            $("#txtCardNo").val(data.card_number);
            $("#ddlSalutation").val(data.salutation);
            $("#txtFirstName").val(data.employee_first_name);
            $("#txtMiddleName").val(data.employee_middle_name);
            $("#txtLastName").val(data.employee_last_name);

            $("#txtGroupJoiningDate").val(GetDateFormatddMMyyyy(new Date(data.group_joining_date)));
            $("#txtDateOfJoining").val(GetDateFormatddMMyyyy(new Date(data.date_of_birth)));

            $("#txtDepartmentJoiningDate").val(GetDateFormatddMMyyyy(new Date(data.department_date_of_joining)));
            $("#txtDateOfBirth").val(GetDateFormatddMMyyyy(new Date(data.date_of_birth)));

            BindReligionList('ddlReligion', data.religion_id);

            $("#ddlMaritalStatus").val(data.marital_status);

            $("#txtHrSpoc").val(data.hr_spoc);
            $("#txtOfficialEmailID").val(data.official_email_id);
            BindEmployementType('ddlemploymenttype', data.empmnt__id);

            if (data.is_ot_allowed == 1) {
                $("#chkOtAllowes").attr('checked', 'checked');
            }
            if (data.is_comb_off_allowed == 1) {
                $("#chkCombOff").attr('checked', 'checked');
            }

            $("#txtMobilePunchFromDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_from_date)));
            $("#txtMobilePunchToDate").val(GetDateFormatddMMyyyy(new Date(data.mobile_punch_to_date)));
            $("#hdnEmployee_img").val(data.employee_photo_path);

            $('#loader').hide();

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}




function readFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 100) {
            $("#File1").val("");
            alert('The size of the photograph should fall between 20KB to 100KB. Your Photo Size is ' + sizekb + 'kb.');
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
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg") {

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



///////////////////////////////////////////////////// START EMPLOYEE WEEK OFF /////////////////////////////////////////

$('#ddlWeekOff').bind("change", function () {
    if ($(this).val() == 2) {
        $("#dynamics").css("display", "block");
    }
    else {
        $("#dynamics").css("display", "none");
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


$('#btnSaveWeekOff').bind("click", function () {
    $('#loader').show();
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var ddlWeekOff = $("#ddlWeekOff").val();


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


    var myData = {
        'employee_id': employee_id,
        'default_company_id': ddlcompany,
        'is_fixed_weekly_off': ddlWeekOff,
        'ShiftWeekOff': WeekOffArray
    };

    //    alert(JSON.stringify(myData));

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/AddWeekOff',
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

                window.location.href = '/Employee/WeekOff';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");

            }
        },
        error: function (request, status, error) {
            _GUID_New();
            $('#loader').hide();
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
});



//Get Employee Code
function GetEmployeeWeekOff(employee_id) {
    $('#loader').show();
    var defaultcompany = CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //   defaultcompany = 26;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeWeekOff/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            //(JSON.stringify(data));
            var res = data;
            $('#loader').hide();

            if (res.is_fixed_weekly_off == 2) {
                $("#dynamics").css("display", "block");
            }
            else {
                $("#dynamics").css("display", "none");
            }

            $("#ddlWeekOff").val(res.is_fixed_weekly_off);


            $.each(res.shiftWeekOff, function (key, value) {


                if (value.week_day == 1) {
                    $("input[name=chkMonday]").prop("checked", true);
                    $("input[name=chkMondayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 2) {
                    $("input[name=chkTuesday]").prop("checked", true);
                    $("input[name=chkTuesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 3) {
                    $("input[name=chkWednesday]").prop("checked", true);
                    $("input[name=chkWednesdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 4) {
                    $("input[name=chkThursday]").prop("checked", true);
                    $("input[name=chkThursdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 5) {
                    $("input[name=chkFriday]").prop("checked", true);
                    $("input[name=chkFridayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 6) {
                    $("input[name=chkSaturday]").prop("checked", true);
                    $("input[name=chkSaturdayDays" + value.days + "]").prop("checked", true);
                }
                else if (value.week_day == 7) {
                    $("input[name=chkSundayday]").prop("checked", true);
                    $("input[name=chkSundayDays" + value.days + "]").prop("checked", true);
                }

            });

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}


///////////////////////////////////////////////////// END EMPLOYEE WEEK OFF /////////////////////////////////////////



//////////////////////////////////////////////////// START GRADE /////////////////////////////////////////////////////


$('#btnSaveGradeAllocation').bind("click", function () {
    $('#loader').show();
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var ddlgrade = $("#ddlgrade").val();
    var allocfromdate = $("#allocfromdate").val();
    var alloctodate = $("#alloctodate").val();


    var myData = {
        'employee_id': employee_id,
        'company_id': ddlcompany,
        'grade_id': ddlgrade,
        'applicable_from_date': allocfromdate,
        'applicable_to_date': alloctodate
    };

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeGradeAllocation',
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

                window.location.href = '/Employee/GradeAllocation';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");

            }
        },
        error: function (request, status, error) {
            _GUID_New();
            $('#loader').hide();
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

});



function GetEmployeeGradeAllocation(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeGradeAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            //(JSON.stringify(data));
            var res = data;
            $('#loader').hide();
            //$("#ddlCompany").val(res.ddlcompany);
            //$("#ddlEmployeeCode").val(res.employee_id);
            $("#ddlgrade").val(res.grade_id);
            $("#allocfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
            $("#alloctodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}




//////////////////////////////////////////////////// END GRADE /////////////////////////////////////////////////////



//////////////////////////////////////////////////// START Designation Allocation /////////////////////////////////////////////////////

$('#btnSaveDesignationAllocation').bind("click", function () {
    $('#loader').show();
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var ddldesignation = $("#ddldesignation").val();
    var Desigfromdate = $("#Desigfromdate").val();
    var Desigtodate = $("#Desigtodate").val();


    var myData = {
        'employee_id': employee_id,
        'company_id': ddlcompany,
        'desig_id': ddldesignation,
        'applicable_from_date': Desigfromdate,
        'applicable_to_date': Desigtodate
    };

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeDesignationAllocation',
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

                window.location.href = '/Employee/DesignationAllocation';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");
                $('#loader').hide();
            }
        },
        error: function (request, status, error) {
            _GUID_New();
            $('#loader').hide();
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

});


function GetEmployeeDesignationAllocation(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeDesignationAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            //(JSON.stringify(data));
            var res = data;
            $('#loader').hide();
            //$("#ddlCompany").val(res.ddlcompany);
            //$("#ddlEmployeeCode").val(res.employee_id);'

            $("#ddldesignation").val(res.desig_id);
            $("#Desigfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
            $("#Desigtodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}




//////////////////////////////////////////////////// END Designation Allocation /////////////////////////////////////////////////////


//////////////////////////////////////////////////// START Manager Allocation /////////////////////////////////////////////////////



$('#btnSaveManagerAllocation').bind("click", function () {
    $('#loader').show();
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();

    var ddlEmployeeManager1 = $("#ddlEmployeeManager1").val();
    var managerallocfromdate1 = $("#managerallocfromdate1").val();
    var manageralloctodate1 = $("#manageralloctodate1").val();

    var chkmanager1 = 0; // Not applicable
    if ($("#chkmanager1").is(":checked")) {
        chkmanager1 = ddlEmployeeManager1; //applicable
    }


    var ddlEmployeeManager2 = $("#ddlEmployeeManager2").val();
    var managerallocfromdate2 = $("#managerallocfromdate2").val();
    var manageralloctodate2 = $("#manageralloctodate2").val();



    if ($("#chkmanager2").is(":checked")) {
        chkmanager1 = ddlEmployeeManager2; //applicable
    }

    var ddlEmployeeManager3 = $("#ddlEmployeeManager3").val();
    var managerallocfromdate3 = $("#managerallocfromdate3").val();
    var manageralloctodate3 = $("#manageralloctodate3").val();


    if ($("#chkmanager3").is(":checked")) {
        chkmanager1 = ddlEmployeeManager3; //applicable
    }


    var myData = {
        'employee_id': employee_id,
        'company_id': ddlcompany,
        'm_one_id': ddlEmployeeManager1,
        'applicable_from_date1': managerallocfromdate1,
        'applicable_to_date1': manageralloctodate1,
        'final_approval1': chkmanager1,

        'm_two_id': ddlEmployeeManager2,
        'applicable_from_date2': managerallocfromdate2,
        'applicable_to_date2': manageralloctodate2,

        'm_three_id': ddlEmployeeManager3,
        'applicable_from_date3': managerallocfromdate3,
        'applicable_to_date3': manageralloctodate3,

    };
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeManagerAllocation',
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

                window.location.href = '/Employee/ManagerAllocation';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");
                $('#loader').hide();
            }
        },
        error: function (request, status, error) {

            _GUID_New();
            $('#loader').hide();
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


});




function GetEmployeeManagerAllocation(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeManagerAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            //(JSON.stringify(data));
            var res = data;

            $('#loader').hide();

            $("#ddlEmployeeManager1").val(res[0].m_one_id);
            $("#managerallocfromdate1").val(GetDateFormatddMMyyyy(new Date(res[0].applicable_from_date)));
            $("#manageralloctodate1").val(GetDateFormatddMMyyyy(new Date(res[0].applicable_to_date)));

            var chkmanager1 = res[0].final_approval;
            if (chkmanager1 == 1) {
                $("input[name=chkmanager1]").prop("checked", true);
            }


            $("#ddlEmployeeManager2").val(res[1].m_two_id);
            $("#managerallocfromdate2").val(GetDateFormatddMMyyyy(new Date(res[1].applicable_from_date)));
            $("#manageralloctodate2").val(GetDateFormatddMMyyyy(new Date(res[1].applicable_to_date)));

            var chkmanager2 = res[1].final_approval;
            if (chkmanager2 == 1) {
                $("input[name=chkmanager2]").prop("checked", true);
            }

            $("#ddlEmployeeManager3").val(res[2].m_two_id);
            $("#managerallocfromdate3").val(GetDateFormatddMMyyyy(new Date(res[2].applicable_from_date)));
            $("#manageralloctodate3").val(GetDateFormatddMMyyyy(new Date(res[2].applicable_to_date)));

            var chkmanager3 = res[2].final_approval;
            if (chkmanager3 == 1) {
                $("input[name=chkmanager3]").prop("checked", true);
            }


        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}



//////////////////////////////////////////////////// END Manager Allocation /////////////////////////////////////////////////////


function DownloadEmployeeOfficaialSectionExcelFile() {
    window.open("/UploadFormat/Employee Officaial Section.xlsx");
}
