$('#loader').show();
var emp_role_id;
var login_emp_id;
var companyid;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindComponent('ddlAdvanceDate', 0, companyid);
        BindComponent('ddlAmtofadvance', 0, companyid);
        BindComponent('ddlpurpose', 0, companyid);
        BindComponent('ddlPostponeGranted', 0, companyid);
        BindComponent('ddlnoofInstallment', 0, companyid);
        BindComponent('ddldateoftotalrepaid', 0, companyid);
        BindComponent('ddlRemarks', 0, companyid);


        GetData(companyid);


        $('#loader').hide();
        $("#ddlcompany").bind("change", function () {

            BindComponent('ddlAdvanceDate', 0, $(this).val());
            BindComponent('ddlAmtofadvance', 0, $(this).val());
            BindComponent('ddlpurpose', 0, $(this).val());
            BindComponent('ddlPostponeGranted', 0, $(this).val());
            BindComponent('ddlnoofInstallment', 0, $(this).val());
            BindComponent('ddldateoftotalrepaid', 0, $(this).val());
            BindComponent('ddlRemarks', 0, $(this).val());



            GetData($(this).val());

        });



        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var ddlAmtofadvance = $("#ddlAmtofadvance").val();
            var ddlAdvanceDate = $("#ddlAdvanceDate").val();
            var ddlpurpose = $("#ddlpurpose").val();
            var ddlPostponeGranted = $("#ddlPostponeGranted").val();
            var ddlnoofInstallment = $("#ddlnoofInstallment").val();
            var ddldateoftotalrepaid = $("#ddldateoftotalrepaid").val();
            var ddlRemarks = $("#ddlRemarks").val();

            var errormsg = "";
            var iserror = false;
            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (ddlAdvanceDate == "0") {
                ddlAdvanceDate = "";
            }

            if (ddlAmtofadvance == "0") {
                ddlAmtofadvance = "";
            }

            if (ddlpurpose == "0") {
                ddlpurpose = "";
            }

            if (ddlPostponeGranted == "0") {
                ddlPostponeGranted = "";
            }
            if (ddlnoofInstallment == "0") {
                ddlnoofInstallment = "";
            }

            if (ddldateoftotalrepaid == "0") {
                ddldateoftotalrepaid = "";
            }

            if (ddlRemarks == "0") {
                ddlRemarks = "";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var mydata = {
                advance_date_c_id: ddlAdvanceDate,
                advance_amt_c_id: ddlAmtofadvance,
                purpose_c_id: ddlpurpose,
                no_of_installment_c_id: ddlnoofInstallment,
                postponement_granted_c_id: ddlPostponeGranted,
                date_total_repaid_c_id: ddldateoftotalrepaid,
                remarks_c_id: ddlRemarks,
                comp_id: company_id,

                created_by: login_emp_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterofAdvanceFormIIIMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#ddlAmtofadvance").val('0');
                        $("#ddlAdvanceDate").val('0');
                        $("#ddlpurpose").val('0');
                        $("#ddlPostponeGranted").val('0');
                        $("#ddlnoofInstallment").val('0');
                        $("#ddldateoftotalrepaid").val('0');
                        $("#ddlRemarks").val('0');
                        messageBox("success", Msg);
                        // alert(Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });
        });

    }, 2000);// end timeout

});


function BindComponent(ControlId, SelectedVal, company_idd) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/GetComponents/0/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');;
            if (res.length > 0) {
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.component_id).html(value.property_details));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }
            }



        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}


function GetData(company_idd) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterofAdvanceFormIIIMaster/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.advance_date_c_id != null) {
                BindComponent('ddlAdvanceDate', res.advance_date_c_id, companyid);
            }
            if (res.advance_amt_c_id != null) {
                BindComponent('ddlAmtofadvance', res.advance_amt_c_id, companyid);
            }
            if (res.purpose_c_id != null) {
                BindComponent('ddlpurpose', res.purpose_c_id, companyid);
            }
            if (res.no_of_installment_c_id != null) {
                BindComponent('ddlnoofInstallment', res.no_of_installment_c_id, companyid);
            }
            if (res.postponement_granted_c_id != null) {
                BindComponent('ddlPostponeGranted', res.postponement_granted_c_id, companyid);
            }
            if (res.date_total_repaid_c_id != null) {
                BindComponent('ddldateoftotalrepaid', res.date_total_repaid_c_id, companyid);
            }
            if (res.remarks_c_id != null) {
                BindComponent('ddlRemarks', res.remarks_c_id, companyid);
            }
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }


    });
}
