$('#loader').show();
var emp_role_id;
var companyid;
var login_emp_idd;


$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_idd, companyid);
        BindComponent('ddlNatureAndDate', 0, companyid);
        BindComponent('ddlWhetherWorkmanShowed', 0, companyid);
        BindComponent('ddlRateOfWages', 0, companyid);
        BindComponent('ddldateofFineImposed', 0, companyid);
        BindComponent('ddlAmtofFineImposed', 0, companyid);
        BindComponent('ddlDateOnWhichFineRealised', 0, companyid);
        BindComponent('ddlRemarks', 0, companyid);

        GetData(companyid);



        $('#loader').hide();



        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {
            var company_id = $("#ddlcompany").val();

            if (company_id == "" || company_id == "0" || company_id == null) {
                messageBox("error", "Please select company");
                return false;
            }

            var ddlNatureAndDate = $("#ddlNatureAndDate").val();
            if (ddlNatureAndDate == "0") {
                ddlNatureAndDate = "";
            }
            var ddlWhetherWorkmanShowed = $("#ddlWhetherWorkmanShowed").val();
            if (ddlWhetherWorkmanShowed == "0") {
                ddlWhetherWorkmanShowed = "";
            }
            var ddlRateOfWages = $("#ddlRateOfWages").val();
            if (ddlRateOfWages == "0") {
                ddlRateOfWages = "";
            }
            var ddlDatOfFineImposed = $("#ddldateofFineImposed").val();
            if (ddlDatOfFineImposed == "0") {
                ddlDatOfFineImposed = "";
            }
            var ddlAmountOfFineImposed = $("#ddlAmtofFineImposed").val();
            if (ddlAmountOfFineImposed == "0") {
                ddlAmountOfFineImposed = "";
            }
            var ddlDateOnWhichFineRealised = $("#ddlDateOnWhichFineRealised").val();
            if (ddlDateOnWhichFineRealised == "0") {
                ddlDateOnWhichFineRealised = "";
            }
            var ddlRemarks = $("#ddlRemarks").val();
            if (ddlRemarks == "0") {
                ddlRemarks = "";
            }

            var errormsg = "";
            var iserror = false;
            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();
            var mydata = {
                comp_id: company_id,
                nature_and_date_c_id: ddlNatureAndDate,
                whether_workman_c_id: ddlWhetherWorkmanShowed,
                rate_of_wages_c_id: ddlRateOfWages,
                date_c_id: ddlDatOfFineImposed,
                amt_c_id: ddlAmountOfFineImposed,
                date_realised_c_id: ddlDateOnWhichFineRealised,
                remarks_c_id: ddlRemarks,
                created_by: login_emp_idd
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterOfFinesFormIMaster",
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

                        $('#ddlWhetherWorkmanShowed').val('0');
                        $('#ddlRateOfWages').val('0');
                        $('#ddldateofFineImposed').val('0');
                        $('#ddlAmtofFineImposed').val('0');
                        $('#ddlDateOnWhichFineRealised').val('0');
                        $('#ddlRemarks').val('0');
                        //GetData(company_id);
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


        $("#ddlcompany").bind("change", function () {

            BindComponent('ddlNatureAndDate', 0, $(this).val());
            BindComponent('ddlWhetherWorkmanShowed', 0, $(this).val());
            BindComponent('ddlRateOfWages', 0, $(this).val());
            BindComponent('ddldateofFineImposed', 0, $(this).val());
            BindComponent('ddlAmtofFineImposed', 0, $(this).val());
            BindComponent('ddlDateOnWhichFineRealised', 0, $(this).val());
            BindComponent('ddlRemarks', 0, $(this).val());

            GetData($(this).val());
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
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            if (res.length > 0) {
                //$(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');

                $.each(res, function (data, value) {

                    $(ControlId).append($("<option></option>").val(value.component_id).html(value.property_details));

                });

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

function GetData(companyidd) {

    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterOfFinesFormIMaster/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.nature_and_date_c_id != null) {
                BindComponent('ddlNatureAndDate', res.nature_and_date_c_id, companyidd);
            }
            if (res.whether_workman_c_id != null) {
                BindComponent('ddlWhetherWorkmanShowed', res.whether_workman_c_id, companyidd);
            }

            if (res.rate_of_wages_c_id != null) {
                BindComponent('ddlRateOfWages', res.rate_of_wages_c_id, companyidd);
            }

            if (res.date_c_id != null) {
                BindComponent('ddldateofFineImposed', res.date_c_id, companyidd);
            }

            if (res.amt_c_id != null) {
                BindComponent('ddlAmtofFineImposed', res.amt_c_id, companyidd);
            }

            if (res.date_c_id != null) {
                BindComponent('ddlDateOnWhichFineRealised', res.date_realised_c_id, companyidd);
            }

            if (res.remarks_c_id != null) {
                BindComponent('ddlRemarks', res.remarks_c_id, companyidd);
            }
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

