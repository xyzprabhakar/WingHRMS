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

        BindBankMaster('ddlbankname', 0);
        BindPaymentMode('ddlPaymentMode', 0);

        $('#ddlCompany').prop("disabled", "disabled");

        BindCompanyList('ddlCompany', default_company);

        BindEmployeeCodee('ddlEmployeeCode', default_company, login_emp_id);

        // BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        //  BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, login_emp_id);
        GetEmployeePersonalDetails(login_emp_id);


        $('#loader').hide();

    }, 2000);// end timeout

});

function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}





function GetEmployeePersonalDetails(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeAccountDetailsForEmp/' + employee_id,
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

