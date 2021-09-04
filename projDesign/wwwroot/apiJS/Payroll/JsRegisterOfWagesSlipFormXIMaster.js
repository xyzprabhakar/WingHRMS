$('#loader').show();
var emp_role_id;
var companyid;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindComponent('ddlBaiscWagesPay', 0, companyid);
        BindComponent('ddlDAWagesPay', 0, companyid);
        BindComponent('ddlTotalAttandOrWorkDone', 0, companyid);
        BindComponent('ddlOvertimeWages', 0, companyid);
        BindComponent('ddlGrossWagesPay', 0, companyid);
        BindComponent('ddlNetWagesPay', 0, companyid);
        BindComponent('ddlPayInCharge', 0, companyid);
        BindDeductionComponent('ddlTotalDeduction', 0, companyid);


        GetData(companyid);






        $('#loader').hide();

        // GetData(companyid);

        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var ddlBaiscWagesPay = $("#ddlBaiscWagesPay").val();
            var ddlDAWagesPay = $("#ddlDAWagesPay").val();
            var ddlTotalAttandOrWorkDone = $("#ddlTotalAttandOrWorkDone").val();
            var ddlOvertimeWages = $("#ddlOvertimeWages").val();
            var ddlGrossWagesPay = $("#ddlGrossWagesPay").val();
            var ddlNetWagesPay = $("#ddlNetWagesPay").val();
            var ddlPayInCharge = $("#ddlPayInCharge").val();
            var ddlTotalDeduction = $("#ddlTotalDeduction").val();
            var errormsg = "";
            var iserror = false;
            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }
            if (ddlBaiscWagesPay == "0") {
                ddlBaiscWagesPay = "";
            }
            if (ddlDAWagesPay == "0") {
                ddlDAWagesPay = "";
            }
            if (ddlTotalAttandOrWorkDone == "0") {
                ddlTotalAttandOrWorkDone = "";
            }
            if (ddlOvertimeWages == "0") {
                ddlOvertimeWages = "";
            }
            if (ddlGrossWagesPay == "0") {
                ddlGrossWagesPay = "";
            }
            if (ddlNetWagesPay == "0") {
                ddlNetWagesPay = "";
            }
            if (ddlPayInCharge == "0") {
                ddlPayInCharge = "";
            }
            if (ddlTotalDeduction == "0") {
                ddlTotalDeduction = "";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var mydata = {
                basic_wages_pay_c_id: ddlBaiscWagesPay,
                da_wages_pay_c_id: ddlDAWagesPay,
                total_attand_orwork_done_c_id: ddlTotalAttandOrWorkDone,
                gross_wages_pay_c_id: ddlGrossWagesPay,
                overtime_wages_c_id: ddlOvertimeWages,
                total_deduction_c_id: ddlTotalDeduction,
                net_wages_pay_c_id: ddlNetWagesPay,
                pay_incharge_c_id: ddlPayInCharge,
                comp_id: company_id,
                created_by: login_emp_id
            }
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterofWageSlipFormXIMaster",
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
                        alert(Msg);
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


        $("#ddlcompany").bind("change", function () {
            BindComponent('ddlBaiscWagesPay', 0, $(this).val());
            BindComponent('ddlDAWagesPay', 0, $(this).val());
            BindComponent('ddlTotalAttandOrWorkDone', 0, $(this).val());
            BindComponent('ddlOvertimeWages', 0, $(this).val());
            BindComponent('ddlGrossWagesPay', 0, $(this).val());
            BindComponent('ddlNetWagesPay', 0, $(this).val());
            BindComponent('ddlPayInCharge', 0, $(this).val());
            BindDeductionComponent('ddlTotalDeduction', 0, $(this).val());


            GetData($(this).val());
        });

    }, 2000);// end timeout

});



function BindComponent(ControlId, SelectedVal, company_id) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/GetComponents/0/" + company_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
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


function BindDeductionComponent(ControlId, SelectedVal, company_id) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_DeductionComponent/2/" + company_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.component_id).html(value.property_details));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}

function GetData(companyidd) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterOfWageSlipFormXIMaster/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.basic_wages_pay_c_id != null) {
                BindComponent('ddlBaiscWagesPay', res.basic_wages_pay_c_id, companyidd);
            }
            if (res.da_wages_pay_c_id != null) {
                BindComponent('ddlDAWagesPay', res.da_wages_pay_c_id, companyidd);
            }
            if (res.total_attand_orwork_done_c_id != null) {
                BindComponent('ddlTotalAttandOrWorkDone', res.total_attand_orwork_done_c_id, companyidd);
            }
            if (res.overtime_wages_c_id != null) {
                BindComponent('ddlOvertimeWages', res.overtime_wages_c_id, companyidd);
            }
            if (res.gross_wages_pay_c_id != null) {
                BindComponent('ddlGrossWagesPay', res.gross_wages_pay_c_id, companyidd);
            }
            if (res.net_wages_pay_c_id != null) {
                BindComponent('ddlNetWagesPay', res.net_wages_pay_c_id, companyidd);
            }
            if (res.pay_incharge_c_id != null) {
                BindComponent('ddlPayInCharge', res.pay_incharge_c_id, companyidd);
            }
            if (res.total_deduction_c_id != null) {
                BindDeductionComponent('ddlTotalDeduction', res.total_deduction_c_id, companyidd);
            }

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

