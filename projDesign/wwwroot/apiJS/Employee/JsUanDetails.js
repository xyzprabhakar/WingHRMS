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
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        if (localStorage.getItem("new_emp_id") != null) {
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            rest_all();
            GetEmployeePersonalDetails(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }
    }
    else {
        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
        localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
    }



    $('#ddlCompany').change(function () {
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);
        localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
        $("#PersonalDetailFile").val('');
    });

    $('#ddlEmployeeCode').change(function () {
        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
        rest_all();
        GetEmployeePersonalDetails($(this).val());
    });

    BindPFGroup('ddlpfgroup', 0);
    BindVPFGroup('ddlvpfgroup', 0);
    // BindBankMaster('ddlbankname', 0);


    $('#loader').hide();


    $('input[type=radio][name="chckpfapp"]').change(function () {
        if ($(this).val() == "1") {
            $("#pfdetails").show();
        }
        else {
            $("#pfdetails").hide();
        }
    });
    $('input[type=radio][name="chckvpfapp"]').change(function () {
        if ($(this).val() == "1") {
            $("#vpfdetails").show();
        }
        else {
            $("#vpfdetails").hide();
        }
    });
    $('input[type=radio][name="chckesicapp"]').change(function () {
        if ($(this).val() == "1") {
            $("#div_esic").show();
        }
        else {
            $("#div_esic").hide();
        }
    });

    $('#btnSavePersonalDetails').bind("click", function () {
        $('#loader').show();
        var employee_id = $('#ddlEmployeeCode :selected').val();

        if (!Validate()) {
            $('#loader').hide();
            return false;
        }


        var uan_no = $("#txtuanno").val();
        var pf_app = $('input[name="chckpfapp"]:checked').val();

        var pf_ceiling = 0;
        var pf_no = "0";
        var pf_group = 0;
        //var bank_name = null;
        //var bank_account_no = 0;
        //var bank_ifsc = "";
        var vpf_app = 0;
        var vpf_group = 0;
        var vpf_amount = 0;


        if (pf_app != 1) {
            pf_no = "";
            pf_group = "0";
            pf_ceiling = "0";
            vpf_app = 0;
            // bank_name = null;
            // bank_account_no = "1234567890";
            // bank_ifsc = "";
        }
        else {
            pf_no = $("#txtpfno").val();
            pf_group = $("#ddlpfgroup").val();
            pf_ceiling = $("#txtpfceiling").val();
            if (pf_ceiling.toString().toUpperCase() == 'NA') {
                pf_ceiling = 0;
            }

            vpf_app = $('input[name="chckvpfapp"]:checked').val();
            if (vpf_app == 1) {
                vpf_group = $("#ddlvpfgroup").val();
                vpf_amount = $("#txt_vpf_amount").val();
                if (vpf_amount.toString().toUpperCase() == 'NA') {
                    vpf_amount = 0;
                }
            }

            // bank_name = $("#ddlbankname").val();
            //bank_account_no = $("#txtbankaccount").val();
            // bank_ifsc = $("#txtbankifsc").val();
        }
        if (pf_ceiling == "" || pf_ceiling == null) {
            pf_ceiling = 0;
        }



        var esic_app = $('input[name="chckesicapp"]:checked').val();
        var esic_no = $("#txtesicno").val();

        if (esic_app == "0") {
            esic_no = "0";
        }

        var is_eps_app = $('input[name="chckepsapp"]:checked').val();



        var myData = {
            'employee_id': employee_id,
            'is_pf_applicable': pf_app,
            'uan_number': uan_no,
            'pf_number': pf_no,
            'pf_group': pf_group,
            'is_vpf_applicable': vpf_app,
            'vpf_Group': vpf_group,
            'vpf_amount': vpf_amount,
            'pf_celing': pf_ceiling,
            'bank_id': 0,
            'ifsc_code': "",
            'bank_acc': "",
            'is_esic_applicable': esic_app,
            'esic_number': esic_no,
            'is_eps_applicable': is_eps_app,
            'created_by': login_emp_id,
        }

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        // Save
        $.ajax({
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeePfEsicDetails',
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
                    window.location.href = '/Employee/UanDetails';
                }
                else if (statuscode == "0") {
                    messageBox("info", Msg);
                    return false;
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

    var errormsg = '';
    var iserror = false;

    var ddlcompany = $("#ddlCompany");
    var employee_id = $('#ddlEmployeeCode :selected').val();


    // var bank_name = $("#ddlbankname").val();
    // var bank_account_no = $("#txtbankaccount").val();
    // var bank_ifsc = $("#txtbankifsc").val();
    var uan_no = $("#txtuanno").val();

    var pf_app = $('input[name="chckpfapp"]:checked').val();
    var pf_no = $("#txtpfno").val();
    var pf_group = $("#ddlpfgroup").val();
    var pf_ceiling = $("#txtpfceiling").val();

    var esic_app = $('input[name="chckesicapp"]:checked').val();
    var esic_no = $("#txtesicno").val();

    var vpf_app = $('input[name="chckvpfapp"]:checked').val();
    var vpf_group = $("#ddlvpfgroup").val();
    var vpf_amount = $("#txt_vpf_amount").val();

    var eps_app = $('input[name="chckepsapp"]:checked').val();

    if (ddlcompany == null || ddlcompany == '0' || ddlcompany == "") {
        errormsg = errormsg + "Please select Company!! <br/>";
        iserror = true;
    }
    if (employee_id == null || employee_id == '0' || employee_id == "") {
        errormsg = errormsg + "Please Enter Employee ID!! <br/>";
        iserror = true;
    }

    if (uan_no == "" || uan_no == null) {
        errormsg = errormsg + "Please enter UAN No</br>";
        iserror = true;
    }

    if (pf_app == "" || pf_app == undefined) {
        errormsg = errormsg + "Please select pf is applicable or not</br>";
        iserror = true;
    }
    if (pf_app == 1) {
        //if (bank_name == "0" || bank_name == "" || bank_name == null) {
        //    errormsg = errormsg + "Please select Bank name</br>";
        //    iserror = true;
        //}
        //if (bank_account_no == "" || bank_account_no == null) {
        //    errormsg = errormsg + "Please eneter Bank account No</br>";
        //    iserror = true;
        //}

        //if (bank_ifsc = "" || bank_ifsc == null) {
        //    errormsg = errormsg + "Please enter ifsc code</br>";
        //    iserror = true;
        //}
        if (pf_no == "" || pf_no == null) {
            errormsg = errormsg + "Please enter PF no</br>";
            iserror = true;
        }
        if (pf_group == "" || pf_group == "0" || pf_group == null) {
            errormsg = errormsg + "Please enter pf group</br>";
            iserror = true;
        }

        if (pf_ceiling == "" || pf_ceiling == null) {
            pf_ceiling = 0;
        }

        //if (uan_no == "" || uan_no == null) {
        //    errormsg = errormsg + "Please enter UAN No</br>";
        //    iserror = true;
        //}


    }
    if (vpf_app == 1) {

        if (vpf_group == "" || vpf_group == "0" || pf_group == null) {
            errormsg = errormsg + "Please select vpf group</br>";
            iserror = true;
        }

        if (vpf_amount == "" || vpf_amount == null) {
            vpf_amount = 0;
        }
    }


    if (esic_app == 1) {
        if (esic_no == "" || esic_no == null) {
            errormsg = errormsg + "Please enter ESIC no</br>";
            iserror = true;
        }
    }

    if (eps_app == "" || eps_app == undefined) {
        errormsg = errormsg + "Please Select EPF is applicable or not...</br>";
        iserror = true;
    }



    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }

    return true;

}

//Save Employee Personal Details


function GetEmployeePersonalDetails(employee_id) {

    if (employee_id > 0) {
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
                        if (res.pf_details.pf_number == '') {
                            $("#txtpfno").val('NA');
                        }
                        else {
                            $("#txtpfno").val(res.pf_details.pf_number);
                        }

                        BindPFGroup('ddlpfgroup', res.pf_details.pf_group);
                        $("#txtpfceiling").val(res.pf_details.pf_celing);


                        $("input[name=chckvpfapp][value=" + res.pf_details.is_vpf_applicable + "]").prop('checked', true);
                        if (res.pf_details.is_pf_applicable == "1") {
                            $("#vpfdetails").show();
                        }
                        else {
                            $("#vpfdetails").hide();
                        }

                        $("input[name=chckepsapp][value=" + res.pf_details.is_eps_applicable + "]").prop('checked', true);

                        BindVPFGroup('ddlvpfgroup', res.pf_details.vpf_Group);
                        $("#txt_vpf_amount").val(res.pf_details.vpf_amount);
                        //BindBankMaster('ddlbankname', res.pf_details.bank_id)
                        //$("#txtbankifsc").val(res.pf_details.ifsc_code);
                        //$("#txtbankaccount").val(res.pf_details.bank_acc);

                    }

                    if (res.esic_details != null) {
                        $("input[name=chckesicapp][value=" + res.esic_details.is_esic_applicable + "]").prop('checked', true);
                        if (res.esic_details.is_esic_applicable == "1") {
                            $("#div_esic").show();
                        }
                        else {
                            $("#div_esic").hide();
                        }
                        if (res.esic_details.esic_number == 0) {
                            $("#txtesicno").val('NA');
                        }
                        else {
                            $("#txtesicno").val(res.esic_details.esic_number);
                        }

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
    //BindBankMaster('ddlbankname', 0)
    //$("#txtbankifsc").val('');
    //$("#txtbankaccount").val('');
    $("input[name=chckepsapp]").prop('checked', false);
    $("input[name=chckesicapp]").prop('checked', false);
    $("#div_esic").hide();
    $("#txtesicno").val('');
}