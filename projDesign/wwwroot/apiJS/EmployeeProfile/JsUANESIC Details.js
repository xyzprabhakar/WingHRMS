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



        // $('#ddlCompany').prop("disabled", "disabled");

        BindCompanyList('ddlCompany', default_company);

        BindEmployeeCodee('ddlEmployeeCode', default_company, login_emp_id);

        //BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
        //setSelect('ddlEmployeeCode', login_emp_id);

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
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePfEsicDetails/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                rest_all();
                return false;
            }

            if (res != null) {


                if (res.pf_details != null) {
                    $("#txtuanno").val(res.pf_details.uan_number);

                    $("input[name=chckpfapp][value=" + res.pf_details.is_pf_applicable + "]").prop('checked', true);
                    if (res.pf_details.is_pf_applicable == 1) {
                        $("#pfdetails").show();
                    }
                    else {
                        $("#pfdetails").hide();
                    }
                    $("#txtpfno").val(res.pf_details.pf_number);

                    if (res.pf_details.pf_group != null && res.pf_details.pf_group != 0) {
                        BindPFGroup('ddlpfgroup', res.pf_details.pf_group);
                    }
                    else {
                        $("#ddlpfgroup").empty();
                    }


                    $("#txtpfceiling").val(res.pf_details.pf_celing);


                    $("input[name=chckvpfapp][value=" + res.pf_details.is_vpf_applicable + "]").prop('checked', true);
                    if (res.pf_details.is_pf_applicable == "1") {
                        $("#vpfdetails").show();
                    }
                    else {
                        $("#vpfdetails").hide();
                    }

                    if (res.pf_details.vpf_Group != null && res.pf_details.vpf_Group != 0) {
                        BindVPFGroup('ddlvpfgroup', res.pf_details.vpf_Group);
                    }
                    else {
                        $("#ddlvpfgroup").empty();
                    }


                    $("#txt_vpf_amount").val(res.pf_details.vpf_amount);
                    if (res.pf_details.bank_id != null && res.pf_details.bank_id != 0) {
                        BindBankMaster('ddlbankname', res.pf_details.bank_id)
                    }
                    else {
                        $("#ddlbankname").empty();
                    }

                    $("#txtbankifsc").val(res.pf_details.ifsc_code);
                    $("#txtbankaccount").val(res.pf_details.bank_acc);

                }

                if (res.esic_details != null) {
                    $("input[name=chckesicapp][value=" + res.esic_details.is_esic_applicable + "]").prop('checked', true);
                    if (res.esic_details.is_esic_applicable == "1") {
                        $("#div_esic").show();
                    }
                    else {
                        $("#div_esic").hide();
                    }

                    $("#txtesicno").val(res.esic_details.esic_number);
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


function rest_all() {

    $("#txtuanno").val('');

    $("input[name=chckpfapp]").prop('checked', false);
    $("#pfdetails").hide();
    $("#txtpfno").val('');
    BindPFGroup('ddlpfgroup', 0);
    $("#txtpfceiling").val('');
    $("input[name=chckvpfapp]").prop('checked', false);
    $("#vpfdetails").hide();
    BindVPFGroup('ddlvpfgroup', 0);
    $("#txt_vpf_amount").val('');
    BindBankMaster('ddlbankname', 0)
    $("#txtbankifsc").val('');
    $("#txtbankaccount").val('');
    $("input[name=chckepsapp]").prop('checked', false);
    $("input[name=chckesicapp]").prop('checked', false);
    $("#div_esic").hide();
    $("#txtesicno").val('');
}