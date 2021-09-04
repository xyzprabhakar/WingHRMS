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

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindComponent('ddlMinBaiscPayable', 0, companyid);
        BindComponent('ddlMinDAPayable', 0, companyid);
        BindComponent('ddlBasicWagesActuallyPay', 0, companyid);
        BindComponent('ddlDAWagesActuallyPay', 0, companyid);
        BindComponent('ddlTotalAttendAndUnitOfWorkDone', 0, companyid);
        BindComponent('ddlOvertimeWorked', 0, companyid);
        BindComponent('ddlGrossWagesPay', 0, companyid);
        BindComponent('ddlEmployerPF', 0, companyid);
        BindComponent('ddlHRDeduction', 0, companyid);
        BindComponent('ddlOtherDeduction', 0, companyid);
        BindComponent('ddlTotalDeduction', 0, companyid);
        BindComponent('ddlWagesPaid', 0, companyid);
        BindComponent('ddlDateofPay', 0, companyid);


        GetData(companyid);

        //BindComponent('ddlEmployeeSignOrThumb', 0);

        $('#loader').hide();



        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {

            var errormsg = "";
            var iserror = false;
            var company_id = $("#ddlcompany").val();
            var ddlMinBaiscPayable = $("#ddlMinBaiscPayable").val();
            var ddlMinDAPayable = $("#ddlMinDAPayable").val();
            var ddlBasicWagesActuallyPay = $("#ddlBasicWagesActuallyPay").val();
            var ddlDAWagesActuallyPay = $("#ddlDAWagesActuallyPay").val();
            var ddlTotalAttendAndUnitOfWorkDone = $("#ddlTotalAttendAndUnitOfWorkDone").val();
            var ddlOvertimeWorked = $("#ddlOvertimeWorked").val();
            var ddlGrossWagesPay = $("#ddlGrossWagesPay").val();
            var ddlEmployerPF = $("#ddlEmployerPF").val();
            var ddlHRDeduction = $("#ddlHRDeduction").val();
            var ddlOtherDeduction = $("#ddlOtherDeduction").val();
            var ddlTotalDeduction = $("#ddlTotalDeduction").val();
            var ddlWagesPaid = $("#ddlWagesPaid").val();
            var ddlDateofPay = $("#ddlDateofPay").val();
            //var ddlEmployeeSignOrThumb = $("#ddlEmployeeSignOrThumb").val();


            if (ddlMinBaiscPayable == "0") {
                ddlMinBaiscPayable = "";
            }

            if (ddlMinDAPayable == "0") {
                ddlMinDAPayable = "";
            }

            if (ddlBasicWagesActuallyPay == "0") {
                ddlBasicWagesActuallyPay = "";
            }

            if (ddlDAWagesActuallyPay == "0") {
                ddlDAWagesActuallyPay = "";
            }

            if (ddlTotalAttendAndUnitOfWorkDone == "0") {
                ddlTotalAttendAndUnitOfWorkDone = "";
            }

            if (ddlOvertimeWorked == "0") {
                ddlOvertimeWorked = "";
            }

            if (ddlGrossWagesPay == "0") {
                ddlGrossWagesPay = "";
            }

            if (ddlEmployerPF == "0") {
                ddlEmployerPF = "";
            }

            if (ddlHRDeduction == "0") {
                ddlHRDeduction = "";
            }

            if (ddlOtherDeduction == "0") {
                ddlOtherDeduction = "";
            }

            if (ddlTotalDeduction == "0") {
                ddlTotalDeduction = "";
            }

            if (ddlWagesPaid == "0") {
                ddlWagesPaid = "";
            }

            if (ddlDateofPay == "0") {
                ddlDateofPay = "";
            }


            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var mydata = {
                comp_id: company_id,
                basic_min_rate_pay_c_id: ddlMinBaiscPayable,
                da_min_rate_pay_c_id: ddlMinDAPayable,
                basic_wages_actually_pay_c_id: ddlBasicWagesActuallyPay,
                da_wages_actually_pay_c_id: ddlDAWagesActuallyPay,
                total_attan_orunit_ofworkdone_c_id: ddlTotalAttendAndUnitOfWorkDone,
                overtime_worked_c_id: ddlOvertimeWorked,
                gross_wages_pay_c_id: ddlGrossWagesPay,
                employers_pf_c_id: ddlEmployerPF,
                hr_deduction_c_id: ddlHRDeduction,
                other_deduction_c_id: ddlOtherDeduction,
                total_deduction_c_id: ddlTotalDeduction,
                wages_paid_c_id: ddlWagesPaid,
                date_ofpayment_c_id: ddlDateofPay,
                created_by: login_emp_id
            }
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterofOvertimeFormXMaster",
                type: "POST",
                dataType: "json",
                data: JSON.stringify(mydata),
                contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#ddlMinBaiscPayable").val('0');
                        $("#ddlMinDAPayable").val('0');
                        $("#ddlBasicWagesActuallyPay").val('0');
                        $("#ddlDAWagesActuallyPay").val('0');
                        $("#ddlTotalAttendAndUnitOfWorkDone").val('0');
                        $("#ddlOvertimeWorked").val('0');
                        $("#ddlGrossWagesPay").val('0');
                        $("#ddlEmployerPF").val('0');
                        $("#ddlHRDeduction").val('0');
                        $("#ddlOtherDeduction").val('0');
                        $("#ddlTotalDeduction").val('0');
                        $("#ddlWagesPaid").val('0');
                        $("#ddlDateofPay").val('0');
                        messageBox("success", Msg);
                        //alert(Msg);
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

            BindComponent('ddlMinBaiscPayable', 0, $(this).val());
            BindComponent('ddlMinDAPayable', 0, $(this).val());
            BindComponent('ddlBasicWagesActuallyPay', 0, $(this).val());
            BindComponent('ddlDAWagesActuallyPay', 0, $(this).val());
            BindComponent('ddlTotalAttendAndUnitOfWorkDone', 0, $(this).val());
            BindComponent('ddlOvertimeWorked', 0, $(this).val());
            BindComponent('ddlGrossWagesPay', 0, $(this).val());
            BindComponent('ddlEmployerPF', 0, $(this).val());
            BindComponent('ddlHRDeduction', 0, $(this).val());
            BindComponent('ddlOtherDeduction', 0, $(this).val());
            BindComponent('ddlTotalDeduction', 0, $(this).val());
            BindComponent('ddlWagesPaid', 0, $(this).val());
            BindComponent('ddlDateofPay', 0, $(this).val());


            GetData($(this).val());
        });

        //  GetData(companyid);

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
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });

}


function GetData(companyid) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterOfFinesFormXMaster/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.basic_min_rate_pay_c_id != null) {
                BindComponent('ddlMinBaiscPayable', res.basic_min_rate_pay_c_id, companyid);
            }
            if (res.da_min_rate_pay_c_id != null) {
                BindComponent('ddlMinDAPayable', res.da_min_rate_pay_c_id, companyid);
            }
            if (res.basic_wages_actually_pay_c_id != null) {
                BindComponent('ddlBasicWagesActuallyPay', res.basic_wages_actually_pay_c_id, companyid);
            }
            if (res.da_wages_actually_pay_c_id != null) {
                BindComponent('ddlDAWagesActuallyPay', res.da_wages_actually_pay_c_id, companyid);
            }
            if (res.total_attan_orunit_ofworkdone_c_id != null) {
                BindComponent('ddlTotalAttendAndUnitOfWorkDone', res.total_attan_orunit_ofworkdone_c_id, companyid);
            }
            if (res.overtime_worked_c_id != null) {
                BindComponent('ddlOvertimeWorked', res.overtime_worked_c_id, companyid);
            }
            if (res.gross_wages_pay_c_id != null) {
                BindComponent('ddlGrossWagesPay', res.gross_wages_pay_c_id, companyid);
            }
            if (res.employers_pf_c_id != null) {
                BindComponent('ddlEmployerPF', res.employers_pf_c_id, companyid);
            }
            if (res.hr_deduction_c_id != null) {
                BindComponent('ddlHRDeduction', res.hr_deduction_c_id, companyid);
            }
            if (res.other_deduction_c_id != null) {
                BindComponent('ddlOtherDeduction', res.other_deduction_c_id, companyid);
            }
            if (res.total_deduction_c_id != null) {
                BindComponent('ddlTotalDeduction', res.total_deduction_c_id, companyid);
            }
            if (res.wages_paid_c_id != null) {
                BindComponent('ddlWagesPaid', res.wages_paid_c_id, companyid);
            }
            if (res.date_ofpayment_c_id != null) {
                BindComponent('ddlDateofPay', res.date_ofpayment_c_id, companyid);
            }
            //if (res.emp_sign_c_id != null) {
            //    BindComponent('ddlEmployeeSignOrThumb', res.emp_sign_c_id);
            //}
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}


