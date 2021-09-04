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

        var emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        var login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        var companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindComponent('ddldateofOvertime', 0, companyid);
        BindComponent('ddlExtendOvertime', 0, companyid);
        BindComponent('ddlTotalOvertime', 0, companyid);
        BindComponent('ddlNormalhr', 0, companyid);
        BindComponent('ddlNormalRate', 0, companyid);
        BindComponent('ddlOvertimeRate', 0, companyid);
        BindComponent('ddlNormalEarning', 0, companyid);
        BindComponent('ddlOvertimeEarning', 0, companyid);
        BindComponent('ddlTotalEarning', 0, companyid);
        BindComponent('ddlOvertimePaymentDate', 0, companyid);

        GetData(companyid);



        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {

            BindComponent('ddldateofOvertime', 0, $(this).val());
            BindComponent('ddlExtendOvertime', 0, $(this).val());
            BindComponent('ddlTotalOvertime', 0, $(this).val());
            BindComponent('ddlNormalhr', 0, $(this).val());
            BindComponent('ddlNormalRate', 0, $(this).val());
            BindComponent('ddlOvertimeRate', 0, $(this).val());
            BindComponent('ddlNormalEarning', 0, $(this).val());
            BindComponent('ddlOvertimeEarning', 0, $(this).val());
            BindComponent('ddlTotalEarning', 0, $(this).val());
            BindComponent('ddlOvertimePaymentDate', 0, $(this).val());

            GetData($(this).val());
        });


        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {

            var errormsg = "";
            var iserror = false;
            var company_id = $("#ddlcompany").val();
            var ddldateofOvertime = $("#ddldateofOvertime").val();
            var ddlExtendOvertime = $("#ddlExtendOvertime").val();
            var ddlTotalOvertime = $("#ddlTotalOvertime").val();
            var ddlNormalhr = $("#ddlNormalhr").val();
            var ddlNormalRate = $("#ddlNormalRate").val();
            var ddlOvertimeRate = $("#ddlOvertimeRate").val();
            var ddlNormalEarning = $("#ddlNormalEarning").val();
            var ddlOvertimeEarning = $("#ddlOvertimeEarning").val();
            var ddlTotalEarning = $("#ddlTotalEarning").val();
            var ddlOvertimePaymentDate = $("#ddlOvertimePaymentDate").val();


            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }


            if (ddldateofOvertime == "0") {
                ddldateofOvertime = "";
            }

            if (ddlExtendOvertime == "0") {
                ddlExtendOvertime = "";
            }

            if (ddlTotalOvertime == "0") {
                ddlTotalOvertime = "";
            }

            if (ddlNormalhr == "0") {
                ddlNormalhr = "";
            }

            if (ddlNormalRate == "0") {
                ddlNormalRate = "";
            }
            if (ddlOvertimeRate == "0") {
                ddlOvertimeRate = "";
            }

            if (ddlNormalEarning == "0") {
                ddlNormalEarning = "";
            }

            if (ddlOvertimeEarning == "0") {
                ddlOvertimeEarning = "";
            }

            if (ddlTotalEarning == "0") {
                ddlTotalEarning == "";
            }

            if (ddlOvertimePaymentDate == "0") {
                ddlOvertimePaymentDate = "";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var mydata = {
                comp_id: company_id,
                overtime_date_c_id: ddldateofOvertime,
                extent_overtime_c_id: ddlExtendOvertime,
                total_overtime_c_id: ddlTotalOvertime,
                normal_hr_c_id: ddlNormalhr,
                normal_rate_c_id: ddlNormalRate,
                overtime_rate_c_id: ddlOvertimeRate,
                normal_earning_c_id: ddlNormalEarning,
                overtime_earning_c_id: ddlOvertimeEarning,
                total_earning_c_id: ddlTotalEarning,
                date_ofpayment_c_id: ddlOvertimePaymentDate,
                created_by: login_emp_id
            }
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterofOvertimeFormIVMaster",
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
                        $("#ddldateofOvertime").val('0');
                        $("#ddlExtendOvertime").val('0');
                        $("#ddlTotalOvertime").val('0');
                        $("#ddlNormalhr").val('0');
                        $("#ddlNormalRate").val('0');
                        $("#ddlOvertimeRate").val('0');
                        $("#ddlNormalEarning").val('0');
                        $("#ddlOvertimeEarning").val('0');
                        $("#ddlTotalEarning").val('0');
                        $("#ddlOvertimePaymentDate").val('0');
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


            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.response.Text);
        }
    });

}


function GetData(companyid) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterOfOvertimeFormIVMaster/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.overtime_date_c_id != null) {
                BindComponent('ddldateofOvertime', res.overtime_date_c_id, companyid);
            }
            if (res.extent_overtime_c_id != null) {
                BindComponent('ddlExtendOvertime', res.extent_overtime_c_id, companyid);
            }

            if (res.total_overtime_c_id != null) {
                BindComponent('ddlTotalOvertime', res.total_overtime_c_id, companyid);
            }
            if (res.normal_hr_c_id != null) {
                BindComponent('ddlNormalhr', res.normal_hr_c_id, companyid);
            }
            if (res.normal_rate_c_id != null) {
                BindComponent('ddlNormalRate', res.normal_rate_c_id, companyid);
            }
            if (res.overtime_rate_c_id != null) {
                BindComponent('ddlOvertimeRate', res.overtime_rate_c_id, companyid);
            }
            if (res.normal_earning_c_id != null) {
                BindComponent('ddlNormalEarning', res.normal_earning_c_id, companyid);
            }
            if (res.overtime_earning_c_id != null) {
                BindComponent('ddlOvertimeEarning', res.overtime_earning_c_id, companyid);
            }
            if (res.total_earning_c_id != null) {
                BindComponent('ddlTotalEarning', res.total_earning_c_id, companyid)
            }
            if (res.date_ofpayment_c_id != null) {
                BindComponent('ddlOvertimePaymentDate', res.date_ofpayment_c_id, companyid);
            }

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}


