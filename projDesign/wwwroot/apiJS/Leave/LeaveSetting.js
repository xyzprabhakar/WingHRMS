var emp_role_idd;
var login_compnay_id;
var login_emp_id;

$('#loader').show();
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_compnay_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //$("#txtfromdate").val('01-Jan-' + new Date().getFullYear());
        //$("#txttodate").val('31-Dec-' + new Date().getFullYear());
        // $("#txtfromdate").val('01-01-' + new Date().getFullYear()+'');
        //$("#txttodate").val('31-12-' + new Date().getFullYear() + '');

        var qs = getQueryStrings();
        var leaveinfoid = qs["id"];
        if (leaveinfoid == null || leaveinfoid == '') {

            $('#Divhhmm').hide();
            $('#DivCertificate').hide();
            $('#DivMaxForward').hide();
            //$('#DivApplyAdvance').hide();
            $('#DivMaxLeaveAccure').hide();
            $('#DivCashebalDays').hide();
            $('#DivCashebalType').hide();
            $('#Divnoofd').hide();
            $('#DivDayPart').hide();

            // GetData();

            // BindAllEmp_Company('ddlcompany', login_emp_id, login_compnay_id);
            BindCompanyListAll('ddlcompany', login_emp_id, -1);
            setSelect('ddlcompany', login_compnay_id);
            BindLocationList_ddl('ddllocation', login_compnay_id, 0);
            BindDepartment_ddl('ddldepartment', login_compnay_id, 0);
            //BindLocationList('ddllocation', login_compnay_id, 0);
            // BindDepartmentList('ddldepartment', login_compnay_id, 0);

            //        var HaveDisplay = ISDisplayMenu("Display Company List");


            BindReligionList('ddlReligion', 0);
            BindLeaveType('ddlleavename', 0);
            BindFrequencyType('ddlfrequency', 0);
            BindEmployementType('ddlemployeetype', 0);

            $('#btnupdate').hide();
            $('#btnsave').show();
        }
        else {
            //add logic for edit details
            $("#hdnid").val(leaveinfoid);
            $('#btnupdate').show();
            $('#btnsave').hide();
            GetEditData(leaveinfoid);
        }




        $('#ddlcompany').bind('change', function () {
            BindLocationList_ddl('ddllocation', $(this).val(), 0);
            BindDepartment_ddl('ddldepartment', $(this).val(), 0);
            //$("#ddllocation option[value!='0']").remove();
            // BindAllDepartmentFromDepartmentMaster('ddldepartment', 0);
            // BindLocationList('ddllocation', $(this).val(), 0);
            //BindDepartmentList('ddldepartment', $(this).val(), 0);
            //BindSubDepartmentListForddl('ddldepartment', $(this).val(), 0);
        });


        $("#txtfromdate").change(function (toselectedDates) {
            $("#txttodate").val('');
        });


        $("#txttodate").change(function () {
            if (Date.parse($("#txtfromdate").val()) >= Date.parse($("#txttodate").val())) {
                messageBox('error', 'To Date must be greater than from date !!');
                $('#txttodate').val('');
            }
        });

        $("#txtfromdate").val('01-Jan-' + new Date().getFullYear());
        $("#txttodate").val('31-Dec-' + new Date().getFullYear());
        //$('#txtleavedefinedate').datepicker({
        //    dateFormat: 'mm/dd/yy',
        //    minDate: 0
        //});

        $('#ddlapplicable').bind('change', function () {
            // debugger;
            if ($(this).val() == '3') {
                $('#Divhhmm').hide();
                $('#DivDayPart').hide();
            }
            else if ($(this).val() == '2') {
                $('#DivDayPart').show();
                $('#Divhhmm').hide();
            }
            else {
                $('#Divhhmm').hide();
                $('#DivDayPart').hide();
            }
        });

        //$('#ddlcertification').bind('change', function () {
        //    if ($(this).val() == '1') {
        //        $('#DivCertificate').show();
        //    }
        //    else {
        //        $('#DivCertificate').hide();
        //    }
        //});
        $('#ddlcarriedforward').bind('change', function () {
            if ($(this).val() == '1') {
                $('#DivMaxForward').show();
            }
            else {
                $('#DivMaxForward').hide();
            }
        });
        //$('#ddlapplyadvance').bind('change', function () {
        //    if ($(this).val() == '1') {
        //        $('#DivApplyAdvance').show();
        //    }
        //    else {
        //        $('#DivApplyAdvance').hide();
        //    }
        //});


        //$("#ddlleavetakentype").bind('change', function () {

        //    if ($(this).val() == 1) {
        //        $("#divmaxpermonth").show();
        //        $("#divmaxperquater").hide();
        //    }
        //    else if ($(this).val() == 2) {
        //        $("#divmaxperquater").show();
        //        $("#divmaxpermonth").hide();
        //    }

        //});


        $('#ddlleaveaccrue').bind('change', function () {
            if ($(this).val() == '1') {
                $('#DivMaxLeaveAccure').show();
            }
            else {
                $('#DivMaxLeaveAccure').hide();
            }
        });
        //is cashebal
        //$('#ddlcashable').bind('change', function () {
        //    if ($(this).val() == '1') {
        //        $('#DivCashebalDays').show();
        //        $('#DivCashebalType').show();
        //        $('#Divnoofd').show();
        //    }
        //    else {
        //        $('#DivCashebalDays').hide();
        //        $('#DivCashebalType').hide();
        //        $('#Divnoofd').hide();
        //    }
        //});

        //file validation
        $('#certificatefile').change(function () {
            // debugger;
            var file = this.files[0];
            let name = file.name;
            let size = file.size;
            let type = file.type;
            let ext = file.name.split('.').pop().toLowerCase();

            if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                $("#certificatefile").val("");
                messageBox("error", "File only allows file types of PDF, JPG, JPEG. ");
                return;
            }

        });
        $('#DivViewFile').hide()

        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            window.location.href = 'AddLeaveSetting';
        });

        $('#btnsave').bind("click", function () {
            // debugger;

            var leave_info_id = 0;
            var leave_type = $('#ddlleavetype').val();
            var leave_tenure_from_date = $('#txtfromdate').val();
            var leave_tenure_to_date = $('#txttodate').val();
            var leave_type_id = $("#ddlleavename").val();
            //leave applicability
            var leave_applicablity_id = 0;
            var is_aplicable_on_all_company = $('#ddlcompany').val();
            var is_aplicable_on_all_location = $('#ddllocation').val();
            var is_aplicable_on_all_department = $('#ddldepartment').val();
            var is_aplicable_on_all_religion = $('#ddlReligion').val();
            var leave_applicable_for = $('#ddlapplicable').val();
            var day_part = $('#ddldaypart').val();
            var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
            var employee_can_apply = "1";
            var admin_can_apply = "1";
            var is_aplicable_on_all_emp_type = $('#ddlemployeetype').val();
            //tbl_leave_app_on_emp_type 
            var employment_type = $('#ddlemployeetype').val();
            //tbl_leave_appcbl_on_company   
            var c_id = $('#ddlcompany').val();
            var location_id = $('#ddllocation').val();
            //tbl_leave_app_on_dept
            var id = $('#ddldepartment').val();
            //tbl_leave_appcbl_on_religion
            var religion_id = $('#ddlReligion').val();
            //leave credit
            var leave_credit_id = 0;
            var frequency_type = $('#ddlfrequency').val();
            var leave_credit_day = "1";//$('#txtleavecreditno').val();
            var leave_credit_number = $('#txtleavecreditno').val(); //"1";
            var is_half_day_applicable = "1"; // yes
            var is_applicable_for_advance = "1"; //yes
            var advance_applicable_day = $('#txtdayspriortoleave').val() == '' ? 0 : $('#txtdayspriortoleave').val();
            var is_leave_accrue = $('#ddlleaveaccrue').val();
            var max_accrue = $('#txtmaxleaveaccure').val() == '' ? 0 : $('#txtmaxleaveaccure').val();
            var is_required_certificate = "2"; //No
            var certificate_path = '';// $('#certificatefile').val();
            //leave rule
            var applicable_if_employee_joined_before_dt = $('#txtleavedefinedate').val();
            var maximum_leave_clubbed_in_tenure_number_of_leave = $('#noofleavecollapsed').val();
            var maxi_negative_leave_applicable = 0;
            var certificate_require_for_min_no_of_day = 0/*$('#txtcertificatedays').val() == '' ? 0 : $('#txtcertificatedays').val()*/;
            var minimum_leave_applicable = 0;
            var number_maximum_negative_leave_balance = 0;
            var maximum_leave_can_be_taken_type = 0;
            var maximum_leave_can_be_taken = 0;
            var maximum_leave_can_be_in_day = $('#txtmaxleaveperday').val() == '' ? 0 : $('#txtmaxleaveperday').val();
            var maximum_leave_can_be_taken_in_quater = $('#txtmaxleaveperquarter').val() == '' ? 0 : $('#txtmaxleaveperquarter').val();
            var can_carried_forward = $('#ddlcarriedforward').val();
            var maximum_carried_forward = $('#txtmaxleaveforward').val() == '' ? 0 : $('#txtmaxleaveforward').val();
            var applied_sandwich_rule = '0';// $('#ddlsandwichrule').val();
            //encashment
            var is_cashable = "2"; //No
            var cashable_type = $('#ddlcahabletype').val();
            var cashable_after_year = '1';//by default 1
            var maximum_cashable_leave = $('#txtnoofleave').val() == '' ? 0 : $('#txtnoofleave').val();
            var is_active = '0';

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            if (is_cashable == '2') {
                cashable_type = 0;
                maximum_cashable_leave = 0;
            }
            if (is_applicable_for_advance == '2' || is_applicable_for_advance == '0') {

                advance_applicable_day = '0';
            }
            if (maximum_leave_clubbed_in_tenure_number_of_leave == '') {
                maximum_leave_clubbed_in_tenure_number_of_leave = 0;
            }
            if (number_maximum_negative_leave_balance == '') {
                number_maximum_negative_leave_balance = 0;
            }


            if (!Validate()) {
                return false;
            }


            var myData = {

                'is_active': is_active,
                'leave_type': leave_type,
                'leave_tenure_from_date': leave_tenure_from_date,
                'leave_tenure_to_date': leave_tenure_to_date,
                'leave_type_id': leave_type_id,
                'leave_applicablity_id': leave_applicablity_id,
                'is_aplicable_on_all_company': is_aplicable_on_all_company,
                'is_aplicable_on_all_location': is_aplicable_on_all_location,
                'is_aplicable_on_all_department': is_aplicable_on_all_department,
                'is_aplicable_on_all_religion': is_aplicable_on_all_religion,
                'leave_applicable_for': leave_applicable_for,
                'day_part': day_part,
                'leave_applicable_in_hours_and_minutes': leave_applicable_in_hours_and_minutes,
                'employee_can_apply': employee_can_apply,
                'admin_can_apply': admin_can_apply,
                'is_aplicable_on_all_emp_type': is_aplicable_on_all_emp_type,
                'employment_type': employment_type,
                'c_id': c_id,
                'location_id': location_id,
                'id': id,
                'religion_id': religion_id,
                'leave_credit_id': leave_credit_id,
                'frequency_type': frequency_type,
                'leave_credit_day': leave_credit_day,
                'leave_credit_number': leave_credit_number,
                'is_half_day_applicable': is_half_day_applicable,
                'is_applicable_for_advance': is_applicable_for_advance,
                'advance_applicable_day': advance_applicable_day,
                'is_leave_accrue': is_leave_accrue,
                'max_accrue': max_accrue,
                'is_required_certificate': is_required_certificate,
                'certificate_path': certificate_path,
                'applicable_if_employee_joined_before_dt': applicable_if_employee_joined_before_dt,
                'maximum_leave_clubbed_in_tenure_number_of_leave': maximum_leave_clubbed_in_tenure_number_of_leave,
                'maxi_negative_leave_applicable': maxi_negative_leave_applicable,
                'certificate_require_for_min_no_of_day': certificate_require_for_min_no_of_day,
                'minimum_leave_applicable': minimum_leave_applicable,
                'number_maximum_negative_leave_balance': number_maximum_negative_leave_balance,
                'maximum_leave_can_be_taken_type': maximum_leave_can_be_taken_type,
                'maximum_leave_can_be_taken': maximum_leave_can_be_taken,
                'maximum_leave_can_be_in_day': maximum_leave_can_be_in_day,
                'maximum_leave_can_be_taken_in_quater': maximum_leave_can_be_taken_in_quater,
                'can_carried_forward': can_carried_forward,
                'maximum_carried_forward': maximum_carried_forward,
                'applied_sandwich_rule': applied_sandwich_rule,
                'is_cashable': is_cashable,
                'cashable_type': cashable_type,
                'cashable_after_year': cashable_after_year,
                'maximum_cashable_leave': maximum_cashable_leave
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/';
            var Obj = JSON.stringify(myData);

            var file = document.getElementById("certificatefile").files[0];
            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            //show loader
            $('#loader').show();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
                    // debugger;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#ddlleavename").val('0');
                        $('#ddlleavetype').val('0');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');

                        // GetData();
                        // messageBox("success", Msg);
                        alert(Msg);
                        //redirect to detail page
                        // MsgBoxAndRedirect(Msg, '/Leave/LeaveSettingDetail');
                        window.location.href = '/Leave/LeaveSettingDetail';
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }

                },
                error: function (err) {
                    //alert('error found');
                    _GUID_New();
                    messageBox("error", err.responseText);
                    $('#loader').hide();
                }
            });
        });

        $("#btnupdate").bind("click", function () {
            var leave_info_id = $('#hdnid').val();
            var leave_type = $('#ddlleavetype').val();
            var leave_tenure_from_date = $('#txtfromdate').val();
            var leave_tenure_to_date = $('#txttodate').val();
            var leave_type_id = $("#ddlleavename").val();
            //leave applicability
            var leave_applicablity_id = 0;
            var is_aplicable_on_all_company = $('#ddlcompany').val();
            var is_aplicable_on_all_location = $('#ddllocation').val();
            var is_aplicable_on_all_department = $('#ddldepartment').val();
            var is_aplicable_on_all_religion = $('#ddlReligion').val();
            var leave_applicable_for = $('#ddlapplicable').val();
            var day_part = $('#ddldaypart').val();
            var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
            var employee_can_apply = "1"; //yes
            var admin_can_apply = "1"; //yes
            var is_aplicable_on_all_emp_type = $('#ddlemployeetype').val();
            //tbl_leave_app_on_emp_type 
            var employment_type = $('#ddlemployeetype').val();
            //tbl_leave_appcbl_on_company   
            var c_id = $('#ddlcompany').val();
            var location_id = $('#ddllocation').val();
            //tbl_leave_app_on_dept
            var id = $('#ddldepartment').val();
            //tbl_leave_appcbl_on_religion
            var religion_id = $('#ddlReligion').val();
            //leave credit
            var leave_credit_id = 0;
            var frequency_type = $('#ddlfrequency').val();
            var leave_credit_day = "1";//$('#txtleavecreditno').val();
            var leave_credit_number = $('#txtleavecreditno').val();//"1" /*$('#txtleavecreditday').val()*/; //yes
            var is_half_day_applicable = "1"; //yes
            var is_applicable_for_advance = "1" /*$('#ddlapplyadvance').val()*/; //yes
            var advance_applicable_day = $('#txtdayspriortoleave').val() == '' ? 0 : $('#txtdayspriortoleave').val();
            var is_leave_accrue = $('#ddlleaveaccrue').val();
            var max_accrue = $('#txtmaxleaveaccure').val() == '' ? 0 : $('#txtmaxleaveaccure').val();
            var is_required_certificate = "2"; //No
            var certificate_path = '';// $('#certificatefile').val();
            //leave rule
            var applicable_if_employee_joined_before_dt = $('#txtleavedefinedate').val();
            var maximum_leave_clubbed_in_tenure_number_of_leave = $('#noofleavecollapsed').val();
            var maxi_negative_leave_applicable = 0/*$('#txtmaxnegleave').val()*/;
            var certificate_require_for_min_no_of_day = 0;
            var minimum_leave_applicable = 0;
            var number_maximum_negative_leave_balance = 0;
            var maximum_leave_can_be_taken_type = 0;
            var maximum_leave_can_be_taken = 0;
            var maximum_leave_can_be_in_day = $('#txtmaxleaveperday').val() == '' ? 0 : $('#txtmaxleaveperday').val();
            var maximum_leave_can_be_taken_in_quater = $('#txtmaxleaveperquarter').val() == '' ? 0 : $('#txtmaxleaveperquarter').val();
            var can_carried_forward = $('#ddlcarriedforward').val();
            var maximum_carried_forward = $('#txtmaxleaveforward').val() == '' ? 0 : $('#txtmaxleaveforward').val();
            var applied_sandwich_rule = '0';// $('#ddlsandwichrule').val();
            //encashment
            var is_cashable = "2";
            var cashable_type = $('#ddlcahabletype').val();
            var cashable_after_year = '1';//by default 1
            var maximum_cashable_leave = $('#txtnoofleave').val() == '' ? 0 : $('#txtnoofleave').val();
            var is_active = '0';

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }


            if (is_cashable == '2') {
                cashable_type = 0;
                maximum_cashable_leave = 0;
            }
            if (is_applicable_for_advance == '2' || is_applicable_for_advance == '0') {

                advance_applicable_day = '0';
            }
            if (maximum_leave_clubbed_in_tenure_number_of_leave == '') {
                maximum_leave_clubbed_in_tenure_number_of_leave = 0;
            }
            if (number_maximum_negative_leave_balance == '') {
                number_maximum_negative_leave_balance = 0;
            }
            if (certificate_require_for_min_no_of_day == '') {
                certificate_require_for_min_no_of_day = 0;
            }

            if (!Validate()) {
                return false;
            }

            var myData = {

                'is_active': is_active,
                'leave_info_id': leave_info_id,
                'leave_type': leave_type,
                'leave_tenure_from_date': leave_tenure_from_date,
                'leave_tenure_to_date': leave_tenure_to_date,
                'leave_type_id': leave_type_id,
                'leave_applicablity_id': leave_applicablity_id,
                'is_aplicable_on_all_company': is_aplicable_on_all_company,
                'is_aplicable_on_all_location': is_aplicable_on_all_location,
                'is_aplicable_on_all_department': is_aplicable_on_all_department,
                'is_aplicable_on_all_religion': is_aplicable_on_all_religion,
                'leave_applicable_for': leave_applicable_for,
                'day_part': day_part,
                'leave_applicable_in_hours_and_minutes': leave_applicable_in_hours_and_minutes,
                'employee_can_apply': employee_can_apply,
                'admin_can_apply': admin_can_apply,
                'is_aplicable_on_all_emp_type': is_aplicable_on_all_emp_type,
                'employment_type': employment_type,
                'c_id': c_id,
                'location_id': location_id,
                'id': id,
                'religion_id': religion_id,
                'leave_credit_id': leave_credit_id,
                'frequency_type': frequency_type,
                'leave_credit_day': leave_credit_day,
                'leave_credit_number': leave_credit_number,
                'is_half_day_applicable': is_half_day_applicable,
                'is_applicable_for_advance': is_applicable_for_advance,
                'advance_applicable_day': advance_applicable_day,
                'is_leave_accrue': is_leave_accrue,
                'max_accrue': max_accrue,
                'is_required_certificate': is_required_certificate,
                'certificate_path': certificate_path,
                'applicable_if_employee_joined_before_dt': applicable_if_employee_joined_before_dt,
                'maximum_leave_clubbed_in_tenure_number_of_leave': maximum_leave_clubbed_in_tenure_number_of_leave,
                'maxi_negative_leave_applicable': maxi_negative_leave_applicable,
                'certificate_require_for_min_no_of_day': certificate_require_for_min_no_of_day,
                'minimum_leave_applicable': minimum_leave_applicable,
                'number_maximum_negative_leave_balance': number_maximum_negative_leave_balance,
                'maximum_leave_can_be_taken_type': maximum_leave_can_be_taken_type,
                'maximum_leave_can_be_taken': maximum_leave_can_be_taken,
                'maximum_leave_can_be_in_day': maximum_leave_can_be_in_day,
                'maximum_leave_can_be_taken_in_quater': maximum_leave_can_be_taken_in_quater,
                'can_carried_forward': can_carried_forward,
                'maximum_carried_forward': maximum_carried_forward,
                'applied_sandwich_rule': applied_sandwich_rule,
                'is_cashable': is_cashable,
                'cashable_type': cashable_type,
                'cashable_after_year': cashable_after_year,
                'maximum_cashable_leave': maximum_cashable_leave
            };

            $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh"></i> Please wait..');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Puttbl_leave_info';
            var Obj = JSON.stringify(myData);


            console.log(JSON.stringify(myData));


            // var file = document.getElementById("certificatefile").files[0];
            var formData = new FormData();
            formData.append('AllData', Obj);
            // formData.append('file', file);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,
                contentType: false,
                headers: headerss,
                success: function (data) {
                    // debugger;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        $('#btnupdate').hide();
                        $('#btnsave').show();

                        //messageBox("success", Msg);
                        $("#btnupdate").text('Update').attr("disabled", false);

                        //redirect to detail page
                        window.location.href = '/Leave/LeaveSettingDetail';
                        //MsgBoxAndRedirect(Msg, '/Leave/LeaveSettingDetail');
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("#btnupdate").text('Update').attr("disabled", false);
                    }
                    $('#loader').hide();
                },
                error: function (err, exception) {
                    _GUID_New();
                    alert('error found');
                    $("#btnupdate").text('Update').attr("disabled", false);
                    $('#loader').hide();
                }
            });
        });

    }, 2000);// end timeout


});


function GetEditData(id) {
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/' + id;
    $('#loader').show();
    $.ajax({

        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            //var emp_role_idd = localStorage.getItem("emp_role_id");
            //var loginn_company_id = localStorage.getItem("company_id");
            var ress = res;

            var leaveinfo = res["objleaveinfo"];
            var applica = res["objapplicability"];
            var leaveemp = res["objleave_emp"];
            var appcompany = res["objappcompany"];
            var dept = res["objdept"];
            var religion = res["objreligion"];
            var credit = res["objlcredit"];
            var rule = res["objlrule"];
            var cash = res["objlcash"];



            //leave info details
            if (leaveinfo != null && leaveinfo != '') {
                if (leaveinfo.leave_tenure_from_date != null && leaveinfo.leave_tenure_from_date != '') {
                    let a = new Date(leaveinfo.leave_tenure_from_date);
                    a = GetDateFormatddMMyyyy(a);
                    $('#txtfromdate').val(a);
                }
                if (leaveinfo.leave_tenure_to_date != null && leaveinfo.leave_tenure_to_date != '') {
                    let b = new Date(leaveinfo.leave_tenure_to_date);
                    b = GetDateFormatddMMyyyy(b);
                    $('#txttodate').val(b);
                }
                BindLeaveType('ddlleavename', leaveinfo.leave_type_id);
                $('#ddlleavetype').val(leaveinfo.leave_type);

                $("input[name=chkstatus][value=" + leaveinfo.is_active + "]").prop('checked', true);
            }
            //leave applicability details
            if (applica != null && applica != '') {
                //company
                if (appcompany != null && appcompany != '') {

                    BindCompanyListAll('ddlcompany', login_emp_id, -1);
                    setSelect('ddlcompany', appcompany.c_id);

                    //BindAllEmp_Company('ddlcompany', login_emp_id, appcompany.c_id);
                    // BindCompanyListAll('ddlcompany', appcompany.c_id);
                    //if (emp_role_idd == 2) {

                    //    $('#ddlcompany').prop("disabled", "disabled");

                    //  //  $('#ddlcompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");
                    //}

                    var g = "";
                    if (appcompany.location_id != "" && appcompany.location_id != null) {
                        g = appcompany.location_id;
                    }
                    else {
                        g = applica.is_aplicable_on_all_location == 1 ? 0 : applica.is_aplicable_on_all_location;
                    }


                    ////BindLocationList('ddllocation', appcompany.c_id, appcompany.location_id);
                    //BindLocationList('ddllocation', appcompany.c_id, g);
                    BindLocationList_ddl('ddllocation', appcompany.c_id, 0);
                    setSelect('ddllocation', g);
                    //department
                    if (dept != null && dept != '') {
                        BindDepartment_ddl('ddldepartment', appcompany.c_id, 0);
                        setSelect('ddldepartment', dept.id);
                        // BindDepartmentList('ddldepartment', appcompany.c_id, dept.id);
                    }
                    else {

                        let D = applica.is_aplicable_on_all_department == 1 ? 0 : applica.is_aplicable_on_all_department;

                        //let C = applica.is_aplicable_on_all_company == 1 ? 0 : applica.is_aplicable_on_all_company;
                        let C = "";
                        if (emp_role_idd == 2) // For Admin
                        {
                            C = login_compnay_id;
                        }
                        else {
                            // C = applica.is_aplicable_on_all_company == 1 ? 0 : applica.is_aplicable_on_all_company;
                            C = applica.is_aplicable_on_all_company == 1 ? 0 : appcompany.c_id;

                        }

                        // BindDepartmentList('ddldepartment', C, D);
                        BindDepartment_ddl('ddldepartment', C, 0);
                        setSelect('ddldepartment', D);
                    }
                }
                else {
                    var C = "";
                    if (emp_role_idd == 2)//For Admin
                    {
                        C = login_compnay_id;

                    }
                    else {
                        C = applica.is_aplicable_on_all_company == 1 ? 0 : applica.is_aplicable_on_all_company;
                    }

                    let L = applica.is_aplicable_on_all_location == 1 ? 0 : applica.is_aplicable_on_all_location;
                    BindCompanyListAll('ddlcompany', login_emp_id, -1);
                    setSelect('ddlcompany', C);

                    // BindAllEmp_Company('ddlcompany', login_emp_id, C);
                    //BindCompanyListAll('ddlcompany', C);  
                    //if (emp_role_idd == 2) {
                    //    $('#ddlcompany').prop("disabled", "disabled");

                    //    $('#ddlcompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");
                    //}
                    // BindLocationList('ddllocation', C, L);
                    BindLocationList_ddl('ddllocation', C, 0);
                    setSelect('ddllocation', L);
                }

                //religion
                if (religion != null && religion != '') {
                    BindReligionList('ddlReligion', religion.religion_id);
                } else {
                    let R = applica.is_aplicable_on_all_religion == 1 ? 0 : applica.is_aplicable_on_all_religion;
                    BindReligionList('ddlReligion', R);
                }

                //$('input[name="radioemp"][value="' + applica.employee_can_apply + '"]').prop('checked', true);
                //$('input[name="radioadmin"][value="' + applica.admin_can_apply + '"]').prop('checked', true);
                //employement type
                if (leaveemp != null && leaveemp != '') {
                    BindEmployementType('ddlemployeetype', leaveemp.employment_type);
                }
                else {
                    let E = applica.is_aplicable_on_all_emp_type == 1 ? 0 : applica.is_aplicable_on_all_emp_type;
                    BindEmployementType('ddlemployeetype', E);
                }

                $('#ddlapplicable').val(applica.leave_applicable_for);
                //if (applica.leave_applicable_for == 3) {
                //    if (applica.leave_applicable_in_hours_and_minutes != null && applica.leave_applicable_in_hours_and_minutes != '') {
                //        let a = getHHMM(applica.leave_applicable_in_hours_and_minutes);
                //        $('#txthhmm').val(a);
                //    }
                //    $('#Divhhmm').show();
                //    $('#DivDayPart').hide();
                //}
                if (applica.leave_applicable_for == 2) {
                    $('#ddldaypart').val(applica.day_part);
                    $('#DivDayPart').show();
                    $('#Divhhmm').hide();
                }
                else {
                    $('#Divhhmm').hide();
                    $('#DivDayPart').hide();
                }


            }
            //leave credit details
            if (credit != null && credit != '') {

                BindFrequencyType('ddlfrequency', credit.frequency_type);
                $('#txtleavecreditno').val(credit.leave_credit_number);
                //$('#txtleavecreditno').val(credit.leave_credit_day);
                $('#txtdayspriortoleave').val(credit.advance_applicable_day);
                //$('#txtleavecreditday').val(credit.leave_credit_number);
                //$('#ddlhalfday').val(credit.is_half_day_applicable);
                //$('#ddlapplyadvance').val(credit.is_applicable_for_advance);
                //if (credit.is_applicable_for_advance == 1) {
                //    $('#txtdayspriortoleave').val(credit.advance_applicable_day);
                //    $('#DivApplyAdvance').show();
                //}
                //else {
                //    $('#DivApplyAdvance').hide();
                //}
                $('#ddlleaveaccrue').val(credit.is_leave_accrue);
                if (credit.is_leave_accrue == 1) {
                    if (credit.max_accrue > 0 && credit.max_accrue != null) {
                        $('#txtmaxleaveaccure').val(credit.max_accrue);
                    }
                    else {
                        $('#txtmaxleaveaccure').val('');
                    }

                    $('#DivMaxLeaveAccure').show();
                }
                else {
                    $('#DivMaxLeaveAccure').hide();
                }
                //$('#ddlcertification').val(credit.is_required_certificate);
                //if (credit.is_required_certificate == 1) {                   
                //    $('#DivCertificate').show();
                //    var certificate_path = $('#certificatefile').val();
                //    $('#viewfile').attr('href', credit.certificate_path);
                //    $('#DivViewFile').show();
                //}
                //else {
                //    $('#DivCertificate').hide();
                //    $('#DivViewFile').hide()
                //}


            }
            //leave rule details
            if (rule != null && rule != '') {
                if (rule.applicable_if_employee_joined_before_dt != null && rule.applicable_if_employee_joined_before_dt != '') {
                    let a = new Date(rule.applicable_if_employee_joined_before_dt);
                    a = GetDateFormatddMMyyyy(a);
                    $('#txtleavedefinedate').val(a);
                }
                $('#noofleavecollapsed').val(rule.maximum_leave_clubbed_in_tenure_number_of_leave);
                //$('#txtmaxnegleave').val(rule.maxi_negative_leave_applicable);
                //$('#txtcertificatedays').val(rule.certificate_require_for_min_no_of_day);
                //$('#txtmaxnegleave').val(rule.number_maximum_negative_leave_balance);
                // $('#ddlleavetakentype').val(rule.maximum_leave_can_be_taken_type);
                //$('#ddlleavetakentype').val(rule.maximum_leave_can_be_taken);
                $('#txtmaxleaveperday').val(rule.maximum_leave_can_be_in_day);
                $('#txtmaxleaveperquarter').val(rule.maximum_leave_can_be_taken_in_quater);
                $('#ddlcarriedforward').val(rule.can_carried_forward);
                if (rule.can_carried_forward == 1) {
                    $('#txtmaxleaveforward').val(rule.maximum_carried_forward);
                    $('#DivMaxForward').show();
                }
                else {
                    $('#DivMaxForward').hide();
                }

                // $('#ddlsandwichrule').val(rule.applied_sandwich_rule);
            }
            //leave encashment          
            //if (cash != null && cash != '') {
            //    $('#ddlcashable').val(cash.is_cashable);
            //    if (cash.is_cashable == 1) {
            //        $('#ddlcahabletype').val(cash.cashable_type);
            //        $('#txtnoofleave').val(cash.maximum_cashable_leave);
            //        $('#DivCashebalDays').show();
            //        $('#DivCashebalType').show();
            //        $('#Divnoofd').show();
            //    }
            //    else {
            //        $('#DivCashebalDays').hide();
            //        $('#DivCashebalType').hide();
            //        $('#Divnoofd').hide();
            //    }

            //}

            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText)
            $('#loader').hide();
        }
    });


}

//delete data
function DeleteData(id) {
    // debugger;
    if (confirm("Do you want to delete this?")) {

        if (id == null || id == '') {
            messageBox('info', 'There some problem please try after later !!');
            return false;
        }

        var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/' + id;
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({
            type: "DELETE",
            url: apiurl,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: headerss,
            success: function (res) {
                // debugger;
                data = res;
                var statuscode = data.statusCode;
                var Msg = data.message;
                _GUID_New();
                if (statuscode == "0") {
                    //GetData();
                    //messageBox("success", Msg);
                    $("#hdnid").val('');
                    alert(Msg);
                    location.reload();
                }
                else {
                    messageBox("error", Msg);
                }

            },
            error: function (err) {
                alert(err.responseText)
            }
        });
    }
    else {
        return false;
    }
}

//-------update city data

//validate 
function Validate() {

    var leave_type = $('#ddlleavetype').val();
    var leave_tenure_from_date = $('#txtfromdate').val();
    var leave_tenure_to_date = $('#txttodate').val();
    var leave_type_id = $("#ddlleavename").val();
    //leave applicability
    var leave_applicablity_id = 0;
    var is_all_comp = $('#ddlcompany').val();
    var is_all_location = $('#ddllocation').val();
    var is_all_dept = $('#ddldepartment').val();
    var is_all_religion = $('#ddlReligion').val();
    var leave_applicable_for = $('#ddlapplicable').val();
    var day_part = $('#ddldaypart').val();
    var leave_hhmm = $('#txthhmm').val();
    var employee_can_apply = "1"/*$('input[name=radioemp]:checked').val()*/; // yes
    var admin_can_apply = "1" /*$('input[name=radioadmin]:checked').val()*/; //yes
    var is_all_emp_type = $('#ddlemployeetype').val();
    //tbl_leave_app_on_emp_type 
    var employment_type = $('#ddlemployeetype').val();
    //tbl_leave_appcbl_on_company   
    var c_id = $('#ddlcompany').val();
    var location_id = $('#ddllocation').val();
    //tbl_leave_app_on_dept
    var id = $('#ddldepartment').val();
    //tbl_leave_appcbl_on_religion
    var religion_id = $('#ddlReligion').val();
    //leave credit
    var leave_credit_id = 0;
    var frequency_type = $('#ddlfrequency').val();
    var leave_credit_day = "1"; //$('#txtleavecreditno').val();
    var leave_credit_number = $('#txtleavecreditno').val(); //"1"; //yes
    var is_half_day_applicable = "1"; //yes
    var is_applicable_for_advance = "1"; //Yes
    var advance_applicable_day = $('#txtdayspriortoleave').val();
    var is_leave_accrue = $('#ddlleaveaccrue').val();
    var max_accrue = $('#txtmaxleaveaccure').val();
    var is_required_certificate = "2"; //No
    var certificate_path = $('#certificatefile').val();
    //leave rule
    var apli_join_before_dt = $('#txtleavedefinedate').val();
    var max_leave_clubed_tenure = $('#noofleavecollapsed').val();
    var maxi_negative_leave_applicable = 0;
    var certificate_require_for_min_no_of_day = 0;
    var minimum_leave_applicable = 0;
    var max_neg_leave = 0;
    var maximum_leave_can_be_taken_type = 0;
    var maximum_leave_can_be_taken = 0;
    var maximum_leave_can_be_in_day = $('#txtmaxleaveperday').val();
    var maximum_leave_can_be_taken_in_quater = $('#txtmaxleaveperquarter').val();
    var can_carried_forward = $('#ddlcarriedforward').val();
    var maximum_carried_forward = $('#txtmaxleaveforward').val();
    var applied_sandwich_rule = '0';// $('#ddlsandwichrule').val();
    //encashment
    var is_cashable = "2";
    var cashable_type = $('#ddlcahabletype').val();
    var cashable_after_year = '1';//by default 1
    var maximum_cashable_leave = $('#txtnoofleave').val();

    var is_active = 1;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (leave_type_id == null || leave_type_id == '0') {
        errormsg = "Please select leave name !! <br/>";
        iserror = true;
    }
    if (leave_type == null || leave_type == '0') {
        errormsg = errormsg + "Please select leave type !! <br/>";
        iserror = true;
    }
    if (leave_tenure_from_date == null || leave_tenure_from_date == '') {
        errormsg = errormsg + "Please select leave from date !! <br/>";
        iserror = true;
    }
    if (leave_tenure_to_date == null || leave_tenure_to_date == '') {
        errormsg = errormsg + "Please select leave to date !! <br/>";
        iserror = true;
    }
    if (is_all_comp == null || is_all_comp == '') {
        errormsg = errormsg + "Please select company !! <br/>";
        iserror = true;
    }
    if (is_all_location == null || is_all_location == '') {
        errormsg = errormsg + "Please select location !! <br/>";
        iserror = true;
    }
    if (is_all_dept == null || is_all_dept == '') {
        errormsg = errormsg + "Please select department !! <br/>";
        iserror = true;
    }
    if (is_all_religion == null || is_all_religion == '') {
        errormsg = errormsg + "Please select religion !! <br/>";
        iserror = true;
    }
    if (leave_applicable_for == null || leave_applicable_for == '' || leave_applicable_for == "0") {
        errormsg = errormsg + "Please select leave applicable for !! <br/>";
        iserror = true;
    }
    //else {
    //    if (leave_applicable_for == '3') {
    //        if (leave_hhmm == null || leave_hhmm == '') {
    //            errormsg = errormsg + "Please select hour and minute !! <br/>";
    //            iserror = true;
    //        }
    //    }
    //}
    if (day_part == null || day_part == '') {
        errormsg = errormsg + "Please select leave day part !! <br/>";
        iserror = true;
    }
    if (employee_can_apply == null || employee_can_apply == '') {//$("#radio1").is(":checked")
        errormsg = errormsg + "Please select employee can apply !! <br/>";
        iserror = true;
    }
    if (admin_can_apply == null || admin_can_apply == '') {
        errormsg = errormsg + "Please select admin/manager can apply !! <br/>";
        iserror = true;
    }
    if (is_all_emp_type == null || is_all_emp_type == '') {
        errormsg = errormsg + "Please select applicable on all employee type !! <br/>";
        iserror = true;
    }
    if (frequency_type == null || frequency_type == '0') {
        errormsg = errormsg + "Please select Credit frequency !! <br/>";
        iserror = true;
    }
    //if (leave_credit_day == null || leave_credit_day < 0 || leave_credit_day > 30 || leave_credit_day == '') {
    //    errormsg = errormsg + "Please enter month calender day no. in leave credit day !! <br/>";
    //    iserror = true;
    //}
    //if (leave_credit_number == null || leave_credit_number < 0 || leave_credit_day > 16 || leave_credit_number == '') {
    //    errormsg = errormsg + "Please enter no. of leave credit !! <br/>";
    //    iserror = true;
    //}


    if (leave_credit_day == null || leave_credit_day < 0 || leave_credit_number > 16 || leave_credit_day == '') {
        errormsg = errormsg + "Please enter no. of leave credit !! <br/>";
        iserror = true;
    }


    //if (is_half_day_applicable == null || is_half_day_applicable == '0') {
    //    errormsg = errormsg + "Please select half day applicable !! <br/>";
    //    iserror = true;
    //}
    //if (is_applicable_for_advance == null || is_applicable_for_advance == '0') {
    //    errormsg = errormsg + "Please select leave applicable for advance !! <br/>";
    //    iserror = true;
    //}
    //else {

    //    if (is_applicable_for_advance == '1' && (advance_applicable_day == null || advance_applicable_day == '')) {
    //        errormsg = errormsg + "Please enter no of days prior to leave !! <br/>";
    //        iserror = true;
    //    }
    //}
    if (is_leave_accrue == null || is_leave_accrue == '0') {
        errormsg = errormsg + "Please select leave accrue !! <br/>";
        iserror = true;
    }
    else {
        if (is_leave_accrue == '1' && (max_accrue == null || max_accrue == '')) {
            //errormsg = errormsg + "Please select leave applicable for advance !! <br/>";
            errormsg = errormsg + "Please enter Maximum Leave accure !! <br/>";
            iserror = true;
        }
        else {
            if (is_leave_accrue == '1' && (parseInt(max_accrue) <= 0)) {
                errormsg = errormsg + "Maximum Leave accure must be greater than 0 !! <br/>";
                iserror = true;
            }
        }
    }
    if (is_required_certificate == null || is_required_certificate == '0') {
        errormsg = errormsg + "Please select certificate !! <br/>";
        iserror = true;
    }
    //else {
    //    if (is_required_certificate == '1' && (certificate_path == null || certificate_path == '')) {
    //        errormsg = errormsg + "Please select certificate file !! <br/>";
    //        iserror = true;
    //    }

    //}

    if (apli_join_before_dt == null || apli_join_before_dt == '0') {
        errormsg = errormsg + "Please select leave define date !! <br/>";
        iserror = true;
    }

    if (max_leave_clubed_tenure == null || max_leave_clubed_tenure == '') {
        errormsg = errormsg + "Please enter no of leave collapsed !! <br/>";
        iserror = true;
    }
    //if (maxi_negative_leave_applicable == null || maxi_negative_leave_applicable == '') {
    //    errormsg = errormsg + "Please enter max. negative leave applicable !! <br/>";
    //    iserror = true;
    //}
    //if (certificate_require_for_min_no_of_day == null || certificate_require_for_min_no_of_day == '') {
    //    errormsg = errormsg + "Please enter certificate require for min no of day !! <br/>";
    //    iserror = true;
    //}
    //if (max_neg_leave == null || max_neg_leave == '') {
    //    errormsg = errormsg + "Please enter max. negative leave balance !! <br/>";
    //    iserror = true;
    //}
    //if (maximum_leave_can_be_taken_type == null || maximum_leave_can_be_taken_type == '0' || maximum_leave_can_be_taken_type == '') {
    //    errormsg = errormsg + "Please select max. leave can be taken type !! <br/>";
    //    iserror = true;
    //}

    if (maximum_leave_can_be_in_day == null || maximum_leave_can_be_in_day == '') {
        errormsg = errormsg + "Please enter max leave can be taken in per day !! <br/>";
        iserror = true;

    }
    if (maximum_leave_can_be_taken_in_quater == null || maximum_leave_can_be_taken_in_quater == '') {
        errormsg = errormsg + "Please enter max leave can be taken in per quater !! <br/>";
        iserror = true;

    }

    if (can_carried_forward == null || can_carried_forward == '0' || can_carried_forward == '') {
        errormsg = errormsg + "Please select leave can carried forward !! <br/>";
        iserror = true;
    }
    else {
        if (can_carried_forward == '1' && (maximum_carried_forward == null || maximum_carried_forward == '')) {
            errormsg = errormsg + "Please enter maximum carried forward at year end !! <br/>";
            iserror = true;
        }
    }

    if (applied_sandwich_rule == null || applied_sandwich_rule == '') {
        errormsg = errormsg + "Please select sandwich rule !! <br/>";
        iserror = true;
    }
    //if (is_cashable == null || is_cashable == '0') {
    //    errormsg = errormsg + "Please select cashebal !! <br/>";
    //    iserror = true;
    //}
    //else {
    //    if (is_cashable == '1' && (cashable_type == null || cashable_type == '')) {
    //        errormsg = errormsg + "Please select cashebal type !! <br/>";
    //        iserror = true;
    //    }
    //    if (is_cashable == '1' && (maximum_cashable_leave == null || maximum_cashable_leave == '')) {
    //        errormsg = errormsg + "Please enter maximum cashable leave !! <br/>";
    //        iserror = true;
    //    }

    //}
    if (leave_credit_number == '') {
        errormsg = errormsg + "Please Enter No of Leave Credit !! <br/>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }

    return true;
}


function BindLocationList_ddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected"> --Please Select --</option>');
    $(ControlId).append('<option value="0">All</option>');
    if (CompanyId != null && CompanyId != "" && CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationByCompany/' + CompanyId,
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

}


function BindDepartment_ddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option value="">--Please Select--</option>');
    $(ControlId).append('<option selected="selected" value="0"> All </option>');

    if (CompanyId != null && CompanyId != "" && CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_DepartmentByCompany/' + CompanyId,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;



                if (res.statusCode != undefined) {
                    $("#loader").hide();
                    messageBox("error", res.message);
                    return false;
                }
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.department_id).html(value.department_name));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }
                $('#loader').hide();
            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });
    }

}

