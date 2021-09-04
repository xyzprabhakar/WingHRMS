$('#loader').show();
var default_company;
var login_emp_id;

$(document).ready(function () {
    var token = localStorage.getItem('Token');

    if (token == null) {
        window.location = '/Login';
    }
    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


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
    });

    $('#ddlEmployeeCode').change(function () {
        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
        GetEmployeePersonalDetails($(this).val());
    });

    BindBankMaster('ddlbankname', 0);

    BindPaymentMode('ddlPaymentMode', 0);
    $('#loader').hide();



    EL("pan_card_image").addEventListener("change", ReadPanFile, false);
    $("#pan_card_image").bind("change", function () {
        EL("pan_card_image").addEventListener("change", ReadPanFile, false);
    });

    EL("aadha_card_image").addEventListener("change", ReadAadharFile, false);
    $("#aadha_card_image").bind("change", function () {
        EL("aadha_card_image").addEventListener("change", ReadAadharFile, false);
    });


    $('#btnSavePersonalDetails').bind("click", function () {
        $('#loader').show();
        var employee_id = $('#ddlEmployeeCode :selected').val();

        var pan_card_name = $("#pan_card_name").val();

        var pan_card_number = $("#pan_card_number").val();
        if (!Validate()) {
            $('#loader').hide();
            return false;
        }
        $('#loader').show();
        var pan_card_image = "";

        var pan_card_image = null;
        if ($("#HFb64pan_card_name").val() != '') {
            pan_card_image = $("#HFb64pan_card_name").val();
        }
        else {
            pan_card_image = $("#hdnpan_card_image").val();
        }
        var aadha_card_name = $("#aadha_card_name").val();
        var aadha_card_number = $("#aadha_card_number").val();

        var aadha_card_image = "";
        var aadha_card_image = null;
        if ($("#HFb64aadha_card_image").val() != '') {
            aadha_card_image = $("#HFb64aadha_card_image").val();
        }
        else {
            aadha_card_image = $("#hdnaadha_card_image").val();
        }



        var bank_name = $("#ddlbankname").val();
        var bank_account_no = $("#txtbankaccount").val();
        var bank_ifsc = $("#txtbankifsc").val();
        var branch_name = $("#txt_branch_name").val();
        var PaymentMode = $("#ddlPaymentMode").val();

        var myData = {
            'employee_id': employee_id,
            'pan_card_name': pan_card_name,
            'pan_card_number': pan_card_number,
            'pan_card_image': pan_card_image,
            'aadha_card_name': aadha_card_name,
            'aadha_card_number': aadha_card_number,
            'aadha_card_image': aadha_card_image,
            'bank_acc': bank_account_no,
            'ifsc_code': bank_ifsc,
            'bank_id': bank_name,
            'branch_name': branch_name,
            'payment_mode': PaymentMode,
            'created_by': login_emp_id
        }

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        // Save
        $.ajax({
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeAccountDetails',
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
                    window.location.href = '/Employee/AccountDetails';
                }
                else if (statuscode == "0") {
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

    });


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
    var pan_card_name = $("#pan_card_name").val();
    var aadha_card_name = $("#aadha_card_name").val();
    var pan_card_no = $("#pan_card_number").val();
    var aadha_card_no = $("#aadha_card_number").val();


    var bank_name = $("#ddlbankname").val();
    var bank_account_no = $("#txtbankaccount").val();
    var bank_ifsc = $("#txtbankifsc").val();

    if (ddlcompany == null || ddlcompany == '0' || ddlcompany == "") {
        errormsg = errormsg + "Please select Company!! <br/>";
        iserror = true;
    }
    if (employee_id == null || employee_id == '0' || employee_id == "") {
        errormsg = errormsg + "Please Select Employee ID!! <br/>";
        iserror = true;
    }
    if (pan_card_name == null || pan_card_name == "") {
        errormsg = errormsg + "Please Enter Pan Card Name!! <br/>";
        iserror = true;
    }
    if (aadha_card_name == null || aadha_card_name == "") {
        errormsg = errormsg + "Please Enter Adhaar card name!! <br/>";
        iserror = true;
    }
    if (pan_card_no == null || pan_card_no == "") {
        errormsg = errormsg + "Please Enter Pan Card Number!! <br/>";
        iserror = true;
    }
    if (aadha_card_no == null || aadha_card_no == "") {
        errormsg = errormsg + "Please Enter Adhaar Card Number!! <br/>";
        iserror = true;
    }

    if (bank_name == "0" || bank_name == "" || bank_name == null) {
        errormsg = errormsg + "Please select Bank name</br>";
        iserror = true;
    }

    if (bank_account_no == "" || bank_account_no == null) {
        errormsg = errormsg + "Please eneter Bank account No</br>";
        iserror = true;
    }

    if (bank_ifsc == "" || bank_ifsc == null) {
        errormsg = errormsg + "Please enter IFSC code</br>";
        iserror = true;
    }


    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        return false;
    }
    $('#loader').hide();
    return true;

}

//Save Employee Details


function GetEmployeePersonalDetails(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeAccountDetails/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#pan_card_name").val('');
                $("#pan_card_number").val('');
                $("#hdnpan_card_image").val('');
                $("#aadha_card_name").val('');
                $("#aadha_card_number").val('');
                $("#hdnaadha_card_image").val('');

                BindBankMaster('ddlbankname', 0)
                $("#txtbankaccount").val('');
                $("#txtbankifsc").val('');
                $("#txt_branch_name").val('');

                BindPaymentMode('ddlPaymentMode', 0);
                $('#loader').hide();
                return false;
            }

            if (res != null) {
                if (res.pan_details != null) {
                    $("#pan_card_name").val(res.pan_details.pan_card_name);
                    $("#pan_card_number").val(res.pan_details.pan_card_number);
                    $("#hdnpan_card_image").val(res.pan_details.pan_card_image);
                }
                else {
                    $("#pan_card_name").val('');
                    $("#pan_card_number").val('');
                    $("#hdnpan_card_image").val('');
                }
                if (res.adhar_details != null) {
                    $("#aadha_card_name").val(res.adhar_details.aadha_card_name);
                    $("#aadha_card_number").val(res.adhar_details.aadha_card_number);
                    $("#hdnaadha_card_image").val(res.adhar_details.aadha_card_image);
                }
                else {
                    $("#aadha_card_name").val('');
                    $("#aadha_card_number").val('');
                    $("#hdnaadha_card_image").val('');

                }
                if (res.bank_details != null) {
                    BindBankMaster('ddlbankname', res.bank_details.bank_id)
                    $("#txtbankaccount").val(res.bank_details.bank_acc);
                    $("#txtbankifsc").val(res.bank_details.ifsc_code);
                    $("#txt_branch_name").val(res.bank_details.branch_name);
                    BindPaymentMode('ddlPaymentMode', res.bank_details.payment_mode);

                }
                else {
                    BindBankMaster('ddlbankname', 0)
                    $("#txtbankaccount").val('');
                    $("#txtbankifsc").val('');
                    $("#txt_branch_name").val('');

                    BindPaymentMode('ddlPaymentMode', 0);
                }




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

function ReadPanFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 500) {
            $("#pan_card_image").val("");
            alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#pan_card_image").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "PNG" || Extension == "JPEG" || Extension == "JPG") {

            }
            else {
                $("#pan_card_image").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            // EL("HFb64pan_card_name").src = e.target.result;
            //EL("HFb64").value = e.target.result;
            EL("HFb64pan_card_name").value = e.target.result;
        };
        FR.readAsDataURL(this.files[0]);
    }
}

function ReadAadharFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 500) {
            $("#aadha_card_image").val("");
            alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#aadha_card_image").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "PNG" || Extension == "JPEG" || Extension == "JPG") {

            }
            else {
                $("#aadha_card_image").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            EL("HFb64aadha_card_image").value = e.target.result;
        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

function ValidNA_INT(ControlId) {
   // debugger;
    ControlId = "#" + ControlId;
    var a = $(ControlId).val();
    if (a != "NA") {
        if (isNaN(a)) {
            alert('Invalid Value');
            $(ControlId).val('');
            $(ControlId).focus();
        }
    }
}