$('#loader').show();

var emp_role_idd;
var login_comp_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_emp_id, login_comp_id);
        BindLocationListForddl('ddllocation', login_comp_id, 0);

        GetPayrollMonthYear(login_comp_id);


        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            //BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            BindLocationListForddl('ddllocation', $(this).val(), 0);
            // BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            GetPayrollMonthYear($(this).val());

            $('#loader').hide();

        });



        $('#ddlPayrollMonth').bind("change", function () {
            //$('#loader').show();

            GetData();
            //$('#loader').hide();
        });

        $('#btndownload').bind("click", function () {


            $('#loader').show();

            //var company_id = localStorage.getItem("company_id");
            var ddlPayrollMonth = $("#ddlPayrollMonth").val();
            var ddllocation = $("#ddllocation").val();

            var errormsg = '';
            var iserror = false;

            //validation part
            if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                iserror = true;
            }
            if (ddlPayrollMonth == null || ddlPayrollMonth == '') {
                errormsg = "Please Select Payroll Month...! ";
                iserror = true;
            }
            if (ddllocation == null || ddllocation == '' || ddllocation == "0") {
                errormsg = "Please select location";
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }


            $.ajax({
                type: "GET",
                url: apiurl + "apiPayroll/GetPayrollDetailsForMusterForm5DownloadPdf/" + login_comp_id + "/" + ddlPayrollMonth + "/" + ddllocation,
                data: {},
                contentType: "application/json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                success: function (response) {

                    var statuscode = response.statusCode;
                    var Msg = response.message;
                    $('#loader').hide();
                    if (statuscode == "0") {

                        var key = CryptoJS.enc.Base64.parse("#base64Key#");
                        var iv = CryptoJS.enc.Base64.parse("#base64IV#");

                        var _appSetting_domainn_dec = CryptoJS.AES.decrypt(localStorage.getItem("_appSetting_domainn"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);


                        var appp_domainnn = _appSetting_domainn_dec; //localStorage.getItem("_appSetting_domainn");
                        window.open(appp_domainnn + Msg, '_blank');

                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        $('#loader').hide();
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    messageBox("error", err.responseText);
                    $('#loader').hide();
                }
            });
        });

        $('#ddllocation').bind("change", function () {

            GetData();

        });

    }, 2000);// end timeout

});



function GetPayrollMonthYear(company_id) {
    $('#loader').show();
    //var company_id = localStorage.getItem("company_id");

    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/GetProcessedPayrollMonthYear/" + company_id,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $("#ddlPayrollMonth").empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $("#ddlPayrollMonth").append($("<option></option>").val(value.payroll_month_year).html(value.payroll_month_year));
            });
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function GetData() {
    $('#loader').show();
    // var company_id = localStorage.getItem("company_id");
    var ddlPayrollMonth = $("#ddlPayrollMonth").val();
    var ddllocation = $("#ddllocation").val();

    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlPayrollMonth == null || ddlPayrollMonth == '' || ddlPayrollMonth == "0") {
        errormsg = "Please Select Payroll Month...! ";
        iserror = true;
    }
    if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "" || $("#ddlCompany").val() == "0") {
        errormsg = "Please select company </br>";
        iserror = true;

    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        return false;
    }


    var urll;

    urll = apiurl + "apiPayroll/GetPayrollDetailsForMusterForm5/" + $("#ddlCompany").val() + "/" + ddlPayrollMonth + "/" + ddllocation;


    $.ajax({
        type: "GET",
        url: urll,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;
            var PayrollCalenders = res.list;
            var column = res.column;
            $('#loader').hide();

            $('#btndownload').show();

            $('#tblPerformanceDetail').dataTable({
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "data": PayrollCalenders,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Minimum Wages (Central) Rules FORM V Muster Roll Rule 26(5)',
                        extend: 'excelHtml5'
                    },
                ],
                "columns": column,
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
            });


        },
        error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}

