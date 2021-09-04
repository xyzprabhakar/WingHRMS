$('#loader').show();
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindCountryList('permanent_country', 0);
        BindCountryList('corresponding_country', 0);
        BindCountryList('emergency_contact_country', 0);
        BindRelationType('emergency_contact_relation', 0);

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeePersonalDetails(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }
        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            $("#PersonalDetailFile").val('');
            reset_all();
        });
        //Get_Details_Changes($(this).val());
        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                Get_Details_Changes($(this).val());
                GetEmployeePersonalDetails($(this).val());
            }

        });

        //Get All Countery

        //BindBankMaster('ddlbankname', 0);

        // Get State On Countery Change
        $('#permanent_country').bind("change", function () {
            BindStateList('permanent_state', 0, $(this).val());
        });

        //Get City On State Change
        $('#permanent_state').bind("change", function () {
            BindCityList('permanent_city', $(this).val(), 0);
        });

        // Get State On Countery Change
        $('#corresponding_country').bind("change", function () {
            BindStateList('corresponding_state', 0, $(this).val());
        });

        //Get City On State Change
        $('#corresponding_state').bind("change", function () {
            BindCityList('corresponding_city', $(this).val(), 0);
        });


        // Get State On Countery Change
        $('#emergency_contact_country').bind("change", function () {
            BindStateList('emergency_contact_state', 0, $(this).val());
        });

        //Get City On State Change
        $('#emergency_contact_state').bind("change", function () {
            BindCityList('emergency_contact_city', $(this).val(), 0);
        });

        $('#loader').hide();


        $("#SameAsPermanentAddress").change(function () {
            if (this.checked) {
                $('#loader').show();
                //alert('check');
                var permanent_address_line_one = $("#permanent_address_line_one").val();
                var permanent_address_line_two = $("#permanent_address_line_two").val();
                var permanent_country = $("#permanent_country").val();

                var permanent_state = 0;
                if ($("#permanent_state").val() != 'Select State') {
                    permanent_state = $("#permanent_state").val();
                }

                var permanent_city = 0;
                if ($("#permanent_city").val() != "Select City" && $("#permanent_city").val() != 0) {
                    permanent_city = $("#permanent_city").val();
                }


                var permanent_pin_code = 0;
                if ($("#permanent_pin_code").val() != '') {
                    permanent_pin_code = $("#permanent_pin_code").val();
                }


                var permanent_document_type = $("#permanent_document_type").val();

                var permanent_address_proof_document = null;
                if ($("#HFb64permanent_address_proof_document").val() != '') {
                    permanent_address_proof_document = $("#HFb64permanent_address_proof_document").val();
                }
                else {
                    permanent_address_proof_document = $("#hdnpermanent_address_proof_document").val();
                }


                if (permanent_country != 0) {
                    BindCountryList('corresponding_country', permanent_country);

                }
                if (permanent_state != 0) {
                    BindStateList('corresponding_state', permanent_state, permanent_country);

                }
                // // debugger;
                if (permanent_city != 0 && permanent_city != "Select City") {
                    BindCityList('corresponding_city', permanent_state, permanent_city);
                }

                if (permanent_pin_code != 0) {
                    $("#corresponding_pin_code").val(permanent_pin_code);
                }

                $("#hdncorresponding_address_proof_document").val(permanent_address_proof_document);
                $("#corresponding_document_type").val(permanent_document_type).attr('disabled', true);
                $("#corresponding_pin_code").attr('disabled', true);
                $("#corresponding_country").attr('disabled', true);
                $("#corresponding_state").attr('disabled', true);
                $("#corresponding_city").attr('disabled', true);
                $("#corresponding_address_line_one").val(permanent_address_line_one).attr('disabled', true);
                $("#corresponding_address_line_two").val(permanent_address_line_two).attr('disabled', true);
                $("#corresponding_address_proof_document").attr('disabled', true);

                $('#loader').hide();
            }
            else {
                //alert('un check');
                $('#loader').show();
                $("#corresponding_document_type").val(permanent_document_type).attr('disabled', false);
                $("#corresponding_pin_code").attr('disabled', false);
                $("#corresponding_city").attr('disabled', false);
                $("#corresponding_state").attr('disabled', false);
                $("#corresponding_country").attr('disabled', false);
                $("#corresponding_address_line_one").val(permanent_address_line_one).attr('disabled', false);
                $("#corresponding_address_line_two").val(permanent_address_line_two).attr('disabled', false);
                $("#corresponding_address_proof_document").attr('disabled', false);

                BindCountryList('corresponding_country', 0);
                BindCityList('corresponding_city', 0, 0);
                BindStateList('corresponding_state', 0, 0);


                $("#corresponding_pin_code").val('');
                $("#hdncorresponding_address_proof_document").val('');
                $("#corresponding_document_type").val('');

                $('#loader').hide();
            }
        });

        $("#chkSameAsPermanentAddressEmer").change(function () {
            if (this.checked) {
                $('#loader').show();
                //alert('check');
                var permanent_address_line_one = $("#permanent_address_line_one").val();
                var permanent_address_line_two = $("#permanent_address_line_two").val();
                var permanent_country = $("#permanent_country").val();

                var permanent_state = 0;
                if ($("#permanent_state").val() != 'Select State') {
                    permanent_state = $("#permanent_state").val();
                }
                else {
                    BindStateList('emergency_contact_state', 1, 0);

                }

                var permanent_city = 0;
                if ($("#permanent_city").val() != 'Select City') {
                    permanent_city = $("#permanent_city").val();
                }
                else {
                    BindCityList('emergency_contact_city', 1, 0);
                }


                var permanent_pin_code = 0;
                if ($("#permanent_pin_code").val() != '') {
                    permanent_pin_code = $("#permanent_pin_code").val();
                }


                var permanent_document_type = $("#permanent_document_type").val();

                var permanent_address_proof_document = null;
                if ($("#HFb64permanent_address_proof_document").val() != '') {
                    permanent_address_proof_document = $("#HFb64permanent_address_proof_document").val();
                }
                else {
                    permanent_address_proof_document = $("#hdnpermanent_address_proof_document").val();
                }

                $("#hdnemergency_contact_address_proof_document").val(permanent_address_proof_document);

                if (permanent_country != 0) {
                    BindCountryList('emergency_contact_country', permanent_country);
                }
                else {
                    BindCountryList('emergency_contact_country', 0);
                }
                if (permanent_country != 0) {
                    BindStateList('emergency_contact_state', permanent_state, permanent_country);
                }
                if (permanent_city != 0) {
                    BindCityList('emergency_contact_city', permanent_state, permanent_city);
                }

                if (permanent_pin_code != 0) {
                    $("#emergency_contact_pin_code").val(permanent_pin_code);
                }
                else {
                    $("#emergency_contact_pin_code").val('');
                }


                $("#emergency_contact_document_type").val(permanent_document_type).attr('disabled', true);
                $("#emergency_contact_country").attr('disabled', true);
                $("#emergency_contact_state").attr('disabled', true);
                $("#emergency_contact_city").attr('disabled', true);
                $("#emergency_contact_pin_code").attr('disabled', true);
                $("#emergency_contact_line_one").val(permanent_address_line_one).attr('disabled', true);
                $("#emergency_contact_line_two").val(permanent_address_line_two).attr('disabled', true);
                $("#emergency_contact_address_proof_document").attr('disabled', true);


                $('#loader').hide();
            }
            else {
                //alert('un check');
                $('#loader').show();
                $("#emergency_contact_document_type").val('').attr('disabled', false);
                $("#emergency_contact_country").attr('disabled', false);
                $("#emergency_contact_state").attr('disabled', false);
                $("#emergency_contact_city").attr('disabled', false);
                $("#emergency_contact_pin_code").attr('disabled', false);
                $("#emergency_contact_line_one").val('').attr('disabled', false);
                $("#emergency_contact_line_two").val('').attr('disabled', false);
                $("#emergency_contact_address_proof_document").attr('disabled', false);

                $("#emergency_contact_pin_code").val('')
                BindCountryList('emergency_contact_country', 0);

                $("#emergency_contact_state option").remove();
                $("#emergency_contact_city option").remove();
                $("#hdnemergency_contact_address_proof_document").val('');

                BindCountryList('emergency_contact_country', 0);


                $("#emergency_contact_state option").remove();
                $("#emergency_contact_city option").remove();

                $("#emergency_contact_pin_code").val('');
                $("#emergency_contact_document_type").val('');

                $('#loader').hide();
            }
        });


        $("#chkSameAsCorrespondingAddressEmer").change(function () {
            if (this.checked) {
                //alert('check');

                $('#loader').show();
                var corresponding_address_line_one = $("#corresponding_address_line_one").val();
                var corresponding_address_line_two = $("#corresponding_address_line_two").val();

                var corresponding_country = $("#corresponding_country").val();

                var corresponding_state = 0;
                if ($("#corresponding_state").val() != 'Select State') {
                    corresponding_state = $("#corresponding_state").val();
                }
                else {
                    BindStateList('emergency_contact_state', 1, 0);
                }



                var corresponding_city = 0;
                if ($("#corresponding_city").val() != 'Select City') {
                    corresponding_city = $("#corresponding_city").val();
                }
                else {
                    BindCityList('emergency_contact_city', 1, 0);
                }

                var corresponding_pin_code = 0;
                if ($("#corresponding_pin_code").val() != '') {
                    corresponding_pin_code = $("#corresponding_pin_code").val();
                }



                var corresponding_document_type = $("#corresponding_document_type").val();

                var corresponding_address_proof_document = null;
                if ($("#HFb64corresponding_address_proof_document").val() != '') {
                    corresponding_address_proof_document = $("#HFb64corresponding_address_proof_document").val();
                }
                else {
                    corresponding_address_proof_document = $("#hdncorresponding_address_proof_document").val();
                }

                $("#hdnemergency_contact_address_proof_document").val(corresponding_address_proof_document);

                if (corresponding_country != 0) {
                    BindCountryList('emergency_contact_country', corresponding_country);
                }
                else {
                    BindCountryList('emergency_contact_country', 0);
                }
                if (corresponding_country != 0) {

                    BindStateList('emergency_contact_state', corresponding_state, corresponding_country);
                }
                if (corresponding_city != 0) {
                    BindCityList('emergency_contact_city', corresponding_state, corresponding_city);
                }

                if (corresponding_pin_code != 0) {
                    $("#emergency_contact_pin_code").val(corresponding_pin_code);
                }
                else {
                    $("#emergency_contact_pin_code").val('');
                }



                $("#emergency_contact_document_type").val(corresponding_document_type).attr('disabled', true);
                $("#emergency_contact_country").attr('disabled', true);
                $("#emergency_contact_state").attr('disabled', true);
                $("#emergency_contact_city").attr('disabled', true);
                $("#emergency_contact_pin_code").attr('disabled', true);
                $("#emergency_contact_line_one").val(corresponding_address_line_one).attr('disabled', true);
                $("#emergency_contact_line_two").val(corresponding_address_line_two).attr('disabled', true);
                $("#emergency_contact_address_proof_document").attr('disabled', true);

                $('#loader').hide();
            }
            else {
                //alert('un check');
                $('#loader').show();
                $("#emergency_contact_document_type").val('').attr('disabled', false);
                $("#emergency_contact_country").attr('disabled', false);
                $("#emergency_contact_state").attr('disabled', false);
                $("#emergency_contact_city").attr('disabled', false);
                $("#emergency_contact_pin_code").attr('disabled', false);
                $("#emergency_contact_line_one").val('').attr('disabled', false);
                $("#emergency_contact_line_two").val('').attr('disabled', false);
                $("#emergency_contact_address_proof_document").attr('disabled', false);

                $("#emergency_contact_pin_code").val('')
                BindCountryList('emergency_contact_country', 0);

                $("#emergency_contact_state option").remove();
                $("#emergency_contact_city option").remove();
                $("#hdnemergency_contact_address_proof_document").val('');

                BindCountryList('emergency_contact_country', 0);


                $("#emergency_contact_state option").remove();
                $("#emergency_contact_city option").remove();

                $("#emergency_contact_pin_code").val('');
                $("#emergency_contact_document_type").val('');

                $('#loader').hide();
            }

        });


        $("#chkOtherAddressEmer").change(function () {
            if (this.checked) {
                $('#loader').show();
                $("#emergency_contact_document_type").val('').attr('disabled', false);
                $("#emergency_contact_country").attr('disabled', false);
                $("#emergency_contact_state").attr('disabled', false);
                $("#emergency_contact_city").attr('disabled', false);
                $("#emergency_contact_pin_code").attr('disabled', false);
                $("#emergency_contact_line_one").val('').attr('disabled', false);
                $("#emergency_contact_line_two").val('').attr('disabled', false);
                $("#emergency_contact_address_proof_document").attr('disabled', false);

                $("#emergency_contact_pin_code").val('')
                BindCountryList('emergency_contact_country', 0);

                $("#emergency_contact_state option").remove();
                $("#emergency_contact_city option").remove();
                $('#loader').hide();
            }
        });


        $("#blood_group_doc").bind("change", function () {
            //ReadBloodGroupFile();
            // EL("blood_group_doc").addEventListener("change", ReadBloodGroupFile, false);\

            if (this.files && this.files[0]) {

                var imgsizee = this.files[0].size;
                var sizekb = imgsizee / 1024;
                sizekb = sizekb.toFixed(0);

                //  $('#HFSizeOfPhoto').val(sizekb);
                if (sizekb < 10 || sizekb > 500) {
                    $("#blood_group_doc").val("");
                    alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
                    return false;
                }
                var ftype = this;
                var fileupload = ftype.value;
                if (fileupload == '') {
                    $("#blood_group_doc").val("");
                    messageBox("error", "Please select file");
                    return false;
                    // alert("Photograph only allows file types of PNG, JPG, JPEG , PDF, Doc ");
                    return;
                }
                else {
                    var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toUpperCase();
                    if (Extension != "PNG" && Extension != "JPEG" && Extension != "JPG" && Extension != "PDF" && Extension != "DOC") {
                        $("#blood_group_doc").val("");
                        messageBox("error", "Only  PNG, JPG, JPEG , PDF, Doc files are allow");
                        return false;
                    }
                }
            }

            //var FR = new FileReader();
            //FR.onload = function (e) {
            //    EL("HFb64blood_group_doc").value = e.target.result;
            //};
            //FR.readAsDataURL(this.files[0]);


        });

    }, 2000);// end timeout

});


function DownloadEmployeeOfficaialSectionExcelFile() {
    window.open("/UploadFormat/Employee  Personal Details.xlsx");
}


function Validate() {
    $('#loader').show();
    var errormsg = '';
    var iserror = false;

    var ddlcompany = $("#ddlCompany");
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var primary_contact_number = $("#primary_contact_number").val();
    var primary_email_id = $("#primary_email_id").val();
    var permanent_address_line_one = $("#permanent_address_line_one").val();
    var permanent_country = $("#permanent_country").val();
    var pin = $("#permanent_pin_code").val();
    var BloodGroup = $("#blood_group").val();
    var state = $("#permanent_state").val();
    var per_city = $("#permanent_city").val();
    var corresponding_address = $("#corresponding_address_line_one").val();
    var corresponding_state = $("#corresponding_state").val();
    var corresponding_country = $("#corresponding_country").val();
    var corresponding_city = $("#corresponding_city").val();
    var corresponding_pin = $("#corresponding_pin_code").val();

    var emergency_contact_name = $("#emergency_contact_name").val();
    var emergency_contact_relation = $("#emergency_contact_relation").val();
    var emergency_contact_mobile_number = $("#emergency_contact_mobile_number").val();
    var emergency_contact_line_one = $("#emergency_contact_line_one").val();
    var emergency_contact_pin_code = $("#emergency_contact_pin_code").val();
    var emergency_contact_country = $("#emergency_contact_country").val();
    var emergency_state = $("#emergency_contact_state").val();
    var emergency_contact_city = $("#emergency_contact_city").val();
    var blood_group_doc = $("#blood_group_doc").val();




    if (ddlcompany == null || ddlcompany == '0' || ddlcompany == "") {
        errormsg = errormsg + "Please select company!! <br/>";
        iserror = true;
    }
    if (employee_id == null || employee_id == '0' || employee_id == "") {
        errormsg = errormsg + "Please select employee id!! <br/>";
        iserror = true;
    }

    if (BloodGroup == null || BloodGroup == '0' || BloodGroup == "") {
        errormsg = errormsg + "Please select blood group!! <br/>";
        iserror = true;
    }

    if (primary_contact_number == null || primary_contact_number == "") {
        errormsg = errormsg + "Please enter primary contact no.!! <br/>";
        iserror = true;
    }
    if (primary_email_id == null || primary_email_id == "") {
        errormsg = errormsg + "Please enter primary email id!! <br/>";
        iserror = true;
    }
    if (permanent_address_line_one == null || permanent_address_line_one == "") {
        errormsg = errormsg + "Please enter permanent address line 1 !! <br/>";
        iserror = true;
    }
    if (permanent_country == null || permanent_country == '0' || permanent_country == "") {
        errormsg = errormsg + "Please select permanent country!! <br/>";
        iserror = true;
    }
    if (state == null || state == "0" || state == "" || state.includes("Select State")) {
        errormsg = errormsg + "Please select permanent state!! <br/>";
        iserror = true;
    }

    if (per_city == null || per_city == "" || per_city == "0") {
        errormsg = errormsg + "Please select permanent city!! <br/>";
        iserror = true;
    }
    if (pin == null || pin == "") {
        errormsg = errormsg + "Please enter permanent Pin Code!! <br/>";
        iserror = true;
    }

    if (corresponding_address == "" || corresponding_address == null || corresponding_address == "0") {
        errormsg = errormsg + "Please enter corresponding address Line 1 <br/>";
        iserror = true;
    }

    if (corresponding_country == null || corresponding_country == "" || corresponding_country == "0") {
        errormsg = errormsg + "Please select corresponding country <br/>";
        iserror = true;
    }

    if (corresponding_state == null || corresponding_state == "" || corresponding_state == "0" || corresponding_state.includes("Select State")) {
        errormsg = errormsg + "Please select corresponding state <br/>";
        iserror = true;
    }

    if (corresponding_city == null || corresponding_city == "" || corresponding_city == "0") {
        errormsg = errormsg + "Please select corresponding city <br/>";
        iserror = true;
    }

    if (corresponding_pin == "" || corresponding_pin == null) {
        errormsg = errormsg + "Please enter corresponding Pin Code <br/>";
        iserror = true;
    }


    if (emergency_contact_name == null || emergency_contact_name == '0' || emergency_contact_name == "") {
        errormsg = errormsg + "Please Enter emergency contact name!! <br/>";
        iserror = true;
    }
    if (emergency_contact_relation == null || emergency_contact_relation == '0' || emergency_contact_relation == "") {
        errormsg = errormsg + "Please Enter emergency contact relation!! <br/>";
        iserror = true;
    }
    if (emergency_contact_mobile_number == null || emergency_contact_mobile_number == "") {
        errormsg = errormsg + "Please Enter emergency contact mobile no.!! <br/>";
        iserror = true;
    }
    //if (emergency_contact_line_one == null || emergency_contact_line_one == '0' || emergency_contact_line_one == "") {
    //    errormsg = errormsg+ "Please Enter emergency contact address line one!! <br/>";
    //    iserror = true;
    //}
    //if (emergency_contact_country == null || emergency_contact_country == '0' || emergency_contact_country == "") {
    //    errormsg = errormsg+ "Please Enter emergency contact country !! <br/>";
    //    iserror = true;
    //}
    //if (emergency_state == null || emergency_state == '0' || emergency_state == "") {
    //    errormsg = errormsg + "Please Enter emergency contact state!! <br/>";
    //    iserror = true;
    //}
    //if (emergency_contact_city == null || emergency_contact_city == '0' || emergency_contact_city == "") {
    //    errormsg = errormsg + "Please Enter emergency contact city!! <br/>";
    //    iserror = true;
    //}

    //if (emergency_contact_pin_code == null || emergency_contact_pin_code == '0' || emergency_contact_pin_code == "") {
    //    errormsg = errormsg+"Please Enter emergency contact pin code!! <br/>";
    //    iserror = true;
    //}


    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }
    $('#loader').hide();
    return true;

}

//Save Employee Personal Details
$('#btnSavePersonalDetails').bind("click", function () {
    // debugger;
    if (Validate()) {
        var employee_id = $('#ddlEmployeeCode :selected').val();

        var blood_group = 0;
        if ($("#blood_group").val() != '') {
            blood_group = $("#blood_group").val();
        }

        var blood_group_doc = "";
        var blood_group_doc = null;
        // if ($("#HFb64blood_group_doc").val() != '' && $("#HFb64blood_group_doc").val() != null && $("#HFb64blood_group_doc").val() != undefined) {
        if ($("#blood_group_doc").val() != undefined && $("#blood_group_doc").val() != "" && $("#blood_group_doc").val() != null) {
            blood_group_doc = $("#blood_group_doc").val(); //$("#HFb64blood_group_doc").val();
        }
        else {
            blood_group_doc = $("#hdnblood_group_doc").val();
        }



        var primary_contact_number = $("#primary_contact_number").val();
        var secondary_contact_number = $("#secondary_contact_number").val();
        var primary_email_id = $("#primary_email_id").val();
        var secondary_email_id = $("#secondary_email_id").val();

        var permanent_address_line_one = $("#permanent_address_line_one").val();
        var permanent_address_line_two = $("#permanent_address_line_two").val();
        var permanent_country = $("#permanent_country").val();

        var permanent_state = 0;
        if ($("#permanent_state").val() != 'Select State') {
            permanent_state = $("#permanent_state").val();
        }

        var permanent_city = 0;
        if ($("#permanent_city").val() != 'Select City') {
            permanent_city = $("#permanent_city").val();
        }


        var permanent_pin_code = 0;
        if ($("#permanent_pin_code").val() != '') {
            permanent_pin_code = $("#permanent_pin_code").val();
        }
        if ($("#permanent_pin_code").val().toUpperCase() == 'NA') {
            permanent_pin_code = 0;
        }


        var permanent_document_type = $("#permanent_document_type").val();
        var permanent_address_proof_document = "";
        //var permanent_address_proof_document = null;
        //if ($("#HFb64permanent_address_proof_document").val() != '') {
        //    permanent_address_proof_document = $("#HFb64permanent_address_proof_document").val();
        //}
        //else {
        //    permanent_address_proof_document = $("#hdnpermanent_address_proof_document").val();
        //}



        var corresponding_address_line_one = $("#corresponding_address_line_one").val();
        var corresponding_address_line_two = $("#corresponding_address_line_two").val();

        var corresponding_country = $("#corresponding_country").val();

        var corresponding_state = 0;
        if ($("#corresponding_state").val() != 'Select State') {
            corresponding_state = $("#corresponding_state").val();
        }



        var corresponding_city = 0;
        if ($("#corresponding_city").val() != 'Select City') {
            corresponding_city = $("#corresponding_city").val();
        }

        var corresponding_pin_code = 0;
        if ($("#corresponding_pin_code").val() != '') {
            corresponding_pin_code = $("#corresponding_pin_code").val();
        }
        if ($("#corresponding_pin_code").val().toUpperCase() == 'NA') {
            corresponding_pin_code = 0;
        }



        var corresponding_document_type = $("#corresponding_document_type").val();
        var corresponding_address_proof_document = "";
        //var corresponding_address_proof_document = null;
        //if ($("#HFb64corresponding_address_proof_document").val() != '') {
        //    corresponding_address_proof_document = $("#HFb64corresponding_address_proof_document").val();
        //}
        //else {
        //    corresponding_address_proof_document = $("#hdncorresponding_address_proof_document").val();
        //}


        var emergency_contact_name = $("#emergency_contact_name").val();
        var emergency_contact_relation = $("#emergency_contact_relation").val();
        var emergency_contact_mobile_number = $("#emergency_contact_mobile_number").val();
        var emergency_contact_line_one = $("#emergency_contact_line_one").val();
        var emergency_contact_line_two = $("#emergency_contact_line_two").val();
        var emergency_contact_country = $("#emergency_contact_country").val();

        var emergency_contact_state = 0;
        if ($("#emergency_contact_state").val() != 'Select State') {
            emergency_contact_state = $("#emergency_contact_state").val();
        }

        var emergency_contact_city = 0;
        if ($("#emergency_contact_city").val() != 'Select City') {
            emergency_contact_city = $("#emergency_contact_city").val();
        }

        var emergency_contact_pin_code = 0;
        if ($("#emergency_contact_pin_code").val() != '') {
            emergency_contact_pin_code = $("#emergency_contact_pin_code").val();
        }
        if ($("#emergency_contact_pin_code").val().toUpperCase() == 'NA') {
            emergency_contact_pin_code = 0;
        }

        var emergency_contact_document_type = $("#emergency_contact_document_type").val();

        var emergency_contact_address_proof_document = "";
        //var emergency_contact_address_proof_document = null;
        //if ($("#HFb64emergency_contact_address_proof_document").val() != '') {
        //    emergency_contact_address_proof_document = $("#HFb64emergency_contact_address_proof_document").val();
        //}
        //else {
        //    emergency_contact_address_proof_document = $("#hdnemergency_contact_address_proof_document").val();
        //}


        var myData = {
            'employee_id': employee_id,
            'blood_group': blood_group,
            'blood_group_doc': blood_group_doc,
            'primary_contact_number': primary_contact_number,
            'secondary_contact_number': secondary_contact_number,
            'primary_email_id': primary_email_id,
            'secondary_email_id': secondary_email_id,
            'permanent_address_line_one': permanent_address_line_one,
            'permanent_address_line_two': permanent_address_line_two,
            'permanent_country': permanent_country,
            'permanent_state': permanent_state,
            'permanent_city': permanent_city,
            'permanent_pin_code': permanent_pin_code,
            'permanent_document_type': permanent_document_type,
            'permanent_address_proof_document': permanent_address_proof_document,
            'corresponding_address_line_one': corresponding_address_line_one,
            'corresponding_address_line_two': corresponding_address_line_two,
            'corresponding_country': corresponding_country,
            'corresponding_state': corresponding_state,
            'corresponding_city': corresponding_city,
            'corresponding_pin_code': corresponding_pin_code,
            'corresponding_document_type': corresponding_document_type,
            'corresponding_address_proof_document': corresponding_address_proof_document,
            'emergency_contact_name': emergency_contact_name,
            'emergency_contact_relation': emergency_contact_relation,
            'emergency_contact_mobile_number': emergency_contact_mobile_number,
            'emergency_contact_line_one': emergency_contact_line_one,
            'emergency_contact_line_two': emergency_contact_line_two,
            'emergency_contact_country': emergency_contact_country,
            'emergency_contact_state': emergency_contact_state,
            'emergency_contact_city': emergency_contact_city,
            'emergency_contact_pin_code': emergency_contact_pin_code,
            'emergency_contact_document_type': emergency_contact_document_type,
            'emergency_contact_address_proof_document': emergency_contact_address_proof_document,
            'created_by': login_emp_id
        }
        var Obj = JSON.stringify(myData);

        var files = document.getElementById("blood_group_doc").files;

        var formData = new FormData();
        formData.append('AllData', Obj);

        formData.append("fileInput", files[0]);



        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $('#loader').show();

        // Save
        $.ajax({
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeePersonalDetails',
            type: "POST",
            data: formData,
            dataType: "json",
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            headers: headerss,
            success: function (data) {

                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                //if data save
                if (statuscode == "1") {
                    alert(Msg);
                    window.location.href = '/Employee/PersonalDetails';
                }
                else if (statuscode == "0") {
                    messageBox("error", Msg);
                    //messageBox("error", "Something went wrong please try again...!");
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
    }

});


function GetEmployeePersonalDetails(employee_id) {
    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePersonalDetails/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {
                var res = data;

                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    $("#loader").hide();
                    return false;
                }

                if (res != null) {

                    $("#blood_group").val(res.blood_group);
                    $("#hdnblood_group_doc").val(res.blood_group_doc);
                    $("#primary_contact_number").val(res.primary_contact_number);
                    $("#secondary_contact_number").val(res.secondary_contact_number);
                    $("#primary_email_id").val(res.primary_email_id);
                    $("#secondary_email_id").val(res.secondary_email_id);
                    $("#permanent_address_line_one").val(res.permanent_address_line_one);
                    $("#permanent_address_line_two").val(res.permanent_address_line_two);

                    if (res.permanent_country != 0) {
                        BindCountryList('permanent_country', res.permanent_country);
                    }
                    if (res.permanent_state != 0) {
                        BindStateList('permanent_state', res.permanent_state, res.permanent_country);
                    }
                    if (res.permanent_city != 0) {
                        BindCityList('permanent_city', res.permanent_state, res.permanent_city);
                    }

                    $("#permanent_pin_code").val(res.permanent_pin_code);


                    $("#permanent_document_type").val(res.permanent_document_type);
                    $("#hdnpermanent_address_proof_document").val(res.permanent_address_proof_document);
                    $("#corresponding_address_line_one").val(res.corresponding_address_line_one);
                    $("#corresponding_address_line_two").val(res.corresponding_address_line_two);
                    $("#hdncorresponding_address_proof_document").val(res.corresponding_address_proof_document);

                    if (res.corresponding_country != 0) {
                        BindCountryList('corresponding_country', res.corresponding_country);
                    }
                    if (res.corresponding_country != 0) {
                        BindStateList('corresponding_state', res.corresponding_state, res.corresponding_country);
                    }
                    if (res.corresponding_city != 0) {
                        BindCityList('corresponding_city', res.corresponding_state, res.corresponding_city);
                    }


                    $("#corresponding_pin_code").val(res.corresponding_pin_code);


                    $("#corresponding_document_type").val(res.corresponding_document_type);

                    $("#emergency_contact_name").val(res.emergency_contact_name);

                    BindRelationType('emergency_contact_relation', res.emergency_contact_relation);
                    $("#emergency_contact_mobile_number").val(res.emergency_contact_mobile_number);
                    $("#emergency_contact_line_one").val(res.emergency_contact_line_one);
                    $("#emergency_contact_line_two").val(res.emergency_contact_line_two);
                    $("#hdnemergency_contact_address_proof_document").val(res.emergency_contact_address_proof_document);

                    if (res.emergency_contact_country != 0) {
                        BindCountryList('emergency_contact_country', res.emergency_contact_country);
                    }
                    if (res.emergency_contact_country != 0) {
                        BindStateList('emergency_contact_state', res.emergency_contact_state, res.emergency_contact_country);
                    }
                    if (res.emergency_contact_city != 0) {
                        BindCityList('emergency_contact_city', res.emergency_contact_state, res.emergency_contact_city);
                    }


                    $("#emergency_contact_pin_code").val(res.emergency_contact_pin_code);


                    $("#emergency_contact_document_type").val(res.emergency_contact_document_type);

                    $('#loader').hide();
                }

                $('#loader').hide();
            },
            error: function (error) {
                messageBox("error", error.responseText);
                $('#loader').hide();
                //messageBox("error", "Server busy please try again later...!");
            }
        });

    }

}


function reset_all() {
    $("#blood_group").val('');
    $("#blood_group_doc").val('');
    $("#primary_contact_number").val('');
    $("#secondary_contact_number").val('');
    $("#primary_email_id").val('');
    $("#secondary_email_id").val('');
    $("#permanent_address_line_one").val('');
    $("#permanent_address_line_two").val('');
    $("#permanent_pin_code").val('');
    $("#permanent_document_type").val('');
    $("#SameAsPermanentAddress").prop("checked", false);
    $("#corresponding_address_line_one").val('');
    $("#corresponding_pin_code").val('');
    $("#corresponding_document_type").val('');
    $("#emergency_contact_name").val('');
    $("#emergency_contact_mobile_number").val();
    $("#chkSameAsPermanentAddressEmer").prop("checked", false);
    $("#chkSameAsCorrespondingAddressEmer").prop("checked", false);
    $("#chkOtherAddressEmer").prop("checked", false);
    $("#emergency_contact_line_one").val('');
    $("#emergency_contact_line_two").val('');
    $("#emergency_contact_address_proof_document").val('');
    $("#emergency_contact_pin_code").val('');
    $("#emergency_contact_document_type").val('');
    BindCountryList('permanent_country', 0);
    BindStateList('permanent_state', 0, 0);
    BindCityList('permanent_city', 0, 0);
    BindCountryList('corresponding_country', 0);
    BindStateList('corresponding_state', 0, 0);
    BindCityList('corresponding_city', 0, 0);
    BindCountryList('emergency_contact_country', 0);
    BindStateList('emergency_contact_state', 0, 0);
    BindCityList('emergency_contact_city', 0, 0);
    BindRelationType('emergency_contact_relation', 0);
    BindBankMaster('ddlbankname', 0);

}




function ValidNA_INT_P(ControlId) {
    // debugger;
    ControlId = "#" + ControlId;
    debugger;
    var a = $(ControlId).val();
    if (a != "NA") {
        if (isNaN(a)) {
            alert('Invalid Value');
            $(ControlId).val('');
            $(ControlId).focus();
        }
    }
   // var clr = $(ControlId).css("color");
    //alert(clr);
    if ($(ControlId).css("color") == "rgb(255, 0, 0)") {
        alert('Invalid Pin Code format');
        $(ControlId).val('');
        $(ControlId).focus();
    }
}

function Get_Details_Changes(employee_id) {
    //debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeePersonal_changes/' + employee_id,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var data = res;
            debugger;
            //if (data.statusCode != undefined) {
            //    messageBox("info", data.message);
            //    $('#loader').hide();
            //    return false;
            //}
            if (data != null && data.blood_group != null && data.blood_group != "") {
                $("#lbl_blood_group").css("color", "red");
            }
            else {
                $("#lbl_blood_group").css("color", "#6c757d");
            }

            if (data != null && data.primary_contact_number != null && data.primary_contact_number != "") {
                $("#lbl_primary_mobile_no").css("color", "red");
            }
            else {
                $("#lbl_primary_mobile_no").css("color", "#6c757d");
            }

            if (data != null && data.secondary_contact_number != null && data.secondary_contact_number != "") {
                $("#lbl_secondary_mobile_no").css("color", "red");
            }
            else {
                $("#lbl_secondary_mobile_no").css("color", "#6c757d");
            }

            if (data != null && data.primary_email_id != null && data.primary_email_id != "") {
                $("#lbl_primary_email_id").css("color", "red");
            }
            else {
                $("#lbl_primary_email_id").css("color", "#6c757d");
            }

            if (data != null && data.secondary_email_id != null && data.secondary_email_id != "") {
                $("#lbl_secondary_email_id").css("color", "red");
            }
            else {
                $("#lbl_secondary_email_id").css("color", "#6c757d");
            }

            if (data != null && data.permanent_address_line_one != null && data.permanent_address_line_one != "") {
                $("#lbl_permanent_address_line1").css("color", "red");
            }
            else {
                $("#lbl_permanent_address_line1").css("color", "#6c757d");
            }

            if (data != null && data.permanent_address_line_two != null && data.permanent_address_line_two != "") {
                $("#lbl_permanent_address_line2").css("color", "red");
            }
            else {
                $("#lbl_permanent_address_line2").css("color", "#6c757d");
            }

            if (data != null && data.permanent_country != null && data.permanent_country != "") {
                $("#lbl_country").css("color", "red");
            }
            else {
                $("#lbl_country").css("color", "#6c757d");
            }

            if (data != null && data.permanent_state != null && data.permanent_state != "") {
                $("#lbl_state").css("color", "red");
            }
            else {
                $("#lbl_state").css("color", "#6c757d");
            }

            if (data != null && data.permanent_city != null && data.permanent_city != "") {
                $("#lbl_city").css("color", "red");
            }
            else {
                $("#lbl_city").css("color", "#6c757d");
            }

            if (data != null && data.permanent_pin_code != null && data.permanent_pin_code != "") {
                $("#lbl_pincode").css("color", "red");
            }
            else {
                $("#lbl_pincode").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_address_line_one != null && data.corresponding_address_line_one != "") {
                $("#lbl_corresponding_address_line1").css("color", "red");
            }
            else {
                $("#lbl_corresponding_address_line1").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_address_line_two != null && data.corresponding_address_line_two != "") {
                $("#lbl_corresponding_address_line2").css("color", "red");
            }
            else {
                $("#lbl_corresponding_address_line2").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_country != null && data.corresponding_country != "") {
                $("#lbl_corresponding_country").css("color", "red");
            }
            else {
                $("#lbl_corresponding_country").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_state != null && data.corresponding_state != "") {
                $("#lbl_corresponding_state").css("color", "red");
            }
            else {
                $("#lbl_corresponding_state").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_city != null && data.corresponding_city != "") {
                $("#lbl_corresponding_city").css("color", "red");
            }
            else {
                $("#lbl_corresponding_city").css("color", "#6c757d");
            }

            if (data != null && data.corresponding_pin_code != null && data.corresponding_pin_code != "") {
                $("#lbl_corresponding_pincode").css("color", "red");
            }
            else {
                $("#lbl_corresponding_pincode").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_name != null && data.emergency_contact_name != "") {
                $("#lbl_emergency_name").css("color", "red");
            }
            else {
                $("#lbl_emergency_name").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_relation != null && data.emergency_contact_relation != "") {
                $("#lbl_emergency_relation").css("color", "red");
            }
            else {
                $("#lbl_emergency_relation").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_mobile_number != null && data.emergency_contact_mobile_number != "") {
                $("#lbl_emergency_mobile").css("color", "red");
            }
            else {
                $("#lbl_emergency_mobile").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_line_one != null && data.emergency_contact_line_one != "") {
                $("#lbl_emergency_contact_line1").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_line1").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_line_two != null && data.emergency_contact_line_two != "") {
                $("#lbl_emergency_contact_line2").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_line2").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_country != null && data.emergency_contact_country != "") {
                $("#lbl_emergency_contact_country").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_country").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_state != null && data.emergency_contact_state != "") {
                $("#lbl_emergency_contact_state").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_state").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_city != null && data.emergency_contact_city != "") {
                $("#lbl_emergency_contact_city").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_city").css("color", "#6c757d");
            }

            if (data != null && data.emergency_contact_pin_code != null && data.emergency_contact_pin_code != "") {
                $("#lbl_emergency_contact_pincode").css("color", "red");
            }
            else {
                $("#lbl_emergency_contact_pincode").css("color", "#6c757d");
            }



            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}