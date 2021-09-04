$('#loader').show();

var login_emp_id;
var companyid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_emp_id, companyid);
        //BindEmployeeCodeFromEmpMasterByComp('ddlempcode', companyid, 0);
        BindOnlyProbation_Confirmed_emp('ddlempcode', companyid, 0);

        $('#txtltenure').keypress(function (e) {
            var key = (e.keyCode ? e.keyCode : e.which);
            return ((key >= 48 && key <= 57));
        });
        $('#txtloanAmt').keypress(function (e) {
            var key = (e.keyCode ? e.keyCode : e.which);
            return ((key >= 48 && key <= 57));
        });
        BindRateList('ddlrate', 0);
        GetData();

        //GetEmployeeOfficaialDetail(localStorage.getItem('emp_id'));
        //$('#txtreqdate').datepicker().datepicker('setDate', 'today');

        $('#btnupdate').hide();
        $('#btnsave').show();
        $('#Action').hide();

        $("#txteffectivedt").datepicker({
            dateFormat: 'mm/dd/yy',
            //minDate: 0,

        });

        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlempcode").empty();
            BindOnlyProbation_Confirmed_emp('ddlempcode', $(this).val(), 0);
            // BindEmployeeCodeFromEmpMasterByComp('ddlempcode', $(this).val(), 0);
            $('#loader').hide();
        });

        $("#ddlempcode").bind("change", function () {
            $("#ddlloan").val(0);
            $("#txtloanAmt").val('');
            $("#txttenure").val('');
            $("#ddlrate").empty();
        });


        $("#ddlloan").bind("change", function () {

            var emp_idd = $("#ddlempcode").val();
            $("#txtloanAmt").val('');
            $("#txttenure").val('');
            $("#ddlrate").empty();
            if (emp_idd == '' || emp_idd == 0 || emp_idd == null) {
                messageBox("error", "Please Select Employee");
                return false;
            }

            if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            $('#loader').show();
            BindEmpBasicLoanDtl(emp_idd, $(this).val());
            $('#loader').hide();
        });



        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/LoanRequest';

        });


        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var companyidd = $("#ddlCompany").val();
            var emp_id = $("#ddlempcode").val();
            var empName = $('#txtFirstName').val();
            var Loantype = $('#ddlloan').val();
            var amount = $('#txtloanAmt').val();
            var tenure = $('#txttenure').val();
            var purpose = $('#purpose_of_loan').val();
            var empcode = $("#ddlempcode option:selected").text();
            var rate = $("#ddlrate").val();
            var startdate = $("#txteffectivedt").val();
            var max_loan_amt = $("#hdnmaxloanamt").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;
            var error = 0;


            if ($("input[name='chkpolicy']:checked")) {
                if ($("input[name='chkpolicy']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            //validation part

            if (companyidd == "0" || companyidd == "" || companyidd == null) {
                errormsg = errormsg + 'Please select company !! <br/>';
                iserror = true;
            }

            if (emp_id == '' || emp_id == '0' || emp_id == null) {
                errormsg = errormsg + 'Please select Employee Code !! <br />';
                iserror = true;
            }
            //if (empName == '' || empName == null) {
            //    errormsg = errormsg + 'Please enter Employee Name !! <br />';
            //    iserror = true;
            //}
            if (Loantype == '' || Loantype == '0') {
                errormsg = "Please select Loan Type !! <br/>";
                iserror = true;
            }
            if (amount == '' || amount == null) {
                errormsg = errormsg + 'Please enter Loan/Advance Amount !! <br />';
                iserror = true;
            }
            if (tenure == '' || tenure == null) {
                errormsg = errormsg + 'Please enter Loan Tenure !! <br />';
                iserror = true;
            }
            if (purpose == '' || purpose == null) {
                errormsg = errormsg + 'Please enter Purpose of Loan !! <br />';
                iserror = true;
            }

            if (parseFloat(max_loan_amt) < parseFloat(amount)) {
                errormsg = errormsg + 'Loan Amount Should not be greater than ' + $("#hdnmaxloanamt").val() + '</br>';
                iserror = true;
            }

            if (!($('#chkpolicy').is(':checked'))) {
                error = 1;
                errormsg = errormsg + 'Please Check Policy !! <br />';
                iserror = true;
            }

            if (startdate == "" || startdate == null) {
                errormsg = errormsg + 'Please Select Loan Start Date !! <br />';
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'req_emp_id': emp_id,
                'emp_code': empcode,
                'loan_type': Loantype,
                'loan_amt': amount,
                'loan_tenure': tenure,
                'loan_purpose': purpose,
                'is_deleted': is_active,
                'interest_rate': rate,
                'start_date': startdate,
                'created_by': login_emp_id,
                'company_id': companyidd,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_LoanRequest';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {

                        $('#ddlloan').val('0');
                        $('#txtloanAmt').val('');
                        $('#txttenure').val('');
                        $('#purpose_of_loan').val('');
                        $('#btnupdate').hide();
                        BindAllEmp_Company('ddlCompany', login_emp_id, companyid);
                        BindEmployeeCodeFromEmpMasterByComp('ddlempcode', companyid, 0);

                        GetData();
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });
        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var id = $("#hdnid").val();
            var emp_id = $("#ddlempcode").val();
            var empName = $('#txtFirstName').val();
            var Loantype = $('#ddlloan').val();
            var amount = $('#txtloanAmt').val();
            var tenure = $('#txttenure').val();
            var purpose = $('#purpose_of_loan').val();
            var empcode = $("#ddlempcode option:selected").text();
            var rate = $("#ddlrate").val();
            var startdate = $("#txteffectivedt").val();
            var max_loan_amt = $("#hdnmaxloanamt").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;
            if (empcode == '' || empcode == '0') {
                errormsg = errormsg + 'Please select Employee Code !! <br />';
                iserror = true;
            }
            //if (empName == '' || empName == null) {
            //    errormsg = errormsg + 'Please enter Employee Name !! <br />';
            //    iserror = true;
            //}
            if (Loantype == '' || Loantype == '0') {
                errormsg = "Please select Loan Type !! <br/>";
                iserror = true;
            }
            if (amount == '' || amount == null) {
                errormsg = errormsg + 'Please enter Loan/Advance Amount !! <br />';
                iserror = true;
            }
            if (tenure == '' || tenure == null) {
                errormsg = errormsg + 'Please enter Loan Tenure !! <br />';
                iserror = true;
            }
            if (purpose == '' || purpose == null) {
                errormsg = errormsg + 'Please enter Purpose of Loan !! <br />';
                iserror = true;
            }

            if (parseFloat(max_loan_amt) < parseFloat(amount)) {
                errormsg = errormsg + 'Loan Amount Should not be greater than ' + $("#hdnmaxloanamt").val() + '</br>';
                iserror = true;
            }

            if (!($('#chkpolicy').is(':checked'))) {
                error = 1;
                errormsg = errormsg + 'Please Check Policy !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'loan_req_id': id,
                'req_emp_id': emp_id,
                'emp_code': empcode,
                'loan_type': Loantype,
                'loan_amt': amount,
                'loan_tenure': tenure,
                'loan_purpose': purpose,
                'policy': is_active,
                'interest_rate': rate,
                'start_date': startdate,
                'is_deleted': 0,
                'last_modified_by': login_emp_id
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_LoanRequest';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {


                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#hdnid").val('');
                        $('#ddlloan').val('0');
                        $('#txtloanAmt').val('');
                        $('#txttenure').val('');
                        $('#purpose_of_loan').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        GetData();
                        BindAllEmp_Company('ddlCompany', login_emp_id, companyid);
                        BindEmployeeCodeFromEmpMasterByComp('ddlempcode', companyid, 0);

                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });


    }, 2000);// end timeout

});


function BindRateList(ControlId, SelectedVal) {
    $('#loader').show();
    // debugger;
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/GetInterestRate';
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            console.log(JSON.stringify(res));
            $(ControlId).empty().append('<option selected="selected" value="-1">--Please select--</option>');
            $.each(res, function (data, value) {
                console.log(JSON.stringify(value));
                $(ControlId).append($("<option></option>").val(value.funValue).html(value.funText));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function GetData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequest/0';
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;

            $("#tblloanrequest").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [2],
                            render: function (data, type, row) {
                                return data == 1 ? 'Loan' : data == 2 ? 'Advance' : '';
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {
                                return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {
                                return data == 0 ? 'No' : data == 1 ? 'Yes' : '';
                            }
                        }

                    ],

                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee", "autoWidth": true },
                    { "data": "loan_type", "name": "loan_type", "title": "Loan Type", "autoWidth": true },
                    { "data": "interest_rate", "name": "interest_rate", "title": "Rate of Interest(ROI)", "autoWidth": true },
                    { "data": "loan_amt", "name": "loan_amt", "title": "Amount", "autoWidth": true },
                    { "data": "loan_tenure", "name": "loan_tenure", "title": "Tenure(In Month)", "autoWidth": true },
                    { "data": "start_date", "name": "start_date", "title": "Effictive Date", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "is_final_approval", "name": "is_final_approval", "title": "Final Loan Status", "autoWidth": true },
                    { "data": "is_closed", "name": "is_closed", "title": "Closed ?", "autoWidth": true }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            $('#loader').show();
            messageBox(error.responseText);
            console.log("error");
        }
    });

}

function GetEditData(id) {
    $('#loader').show();
    // debugger;

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequest/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            BindAllEmp_Company('ddlCompany', login_emp_id, data.default_company_id);
            BindEmployeeCodeFromEmpMasterByComp('ddlempcode', data.default_company_id, data.req_emp_id);
            $("#ddlempcode").val(data.req_emp_id);
            GetLoanType_ROI('ddlloan', data.loan_type_id, data.default_company_id)
            $('#ddlloan').val(data.loan_type_id);
            $('#txtloanAmt').val(data.loan_amt);
            $('#txttenure').val(data.loan_tenure);
            $('#purpose_of_loan').val(data.loan_purpose);
            Bind_ROI('ddlrate', data.interest_rate, data.default_company_id);
            $("#ddlrate").val(data.interest_rate);
            $("#txteffectivedt").val(GetDateFormatddMMyyyy(new Date(data.start_date)));
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox(err.responseText);
        }
    });
}



//Get Employee Code
function GetEmployeeOfficaialDetail(employee_id) {
    $('#loader').show();
    // debugger;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Employee/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            // debugger;
            console.log(data);
            $("#ddlempcode").append($("<option></option>").val(data.employee_id).html(data.emp_code));
            // $("#txtFirstName").val(data.employee_first_name);
            $("#txtFirstName").val(data.employee_first_name + ' ' + data.employee_middle_name + ' ' + data.employee_last_name);

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", "Server busy please try again later...!");
        }
    });

}

function GetLoanType_ROI(ControlId, SelectedVal, CompanyId) {
    $('#loader').show();
    // debugger;
    ControlId = '#' + ControlId;
    $.ajax({

        type: "GET",
        url: apiurl + "apiPayroll/Get_LoanType_ROI/0/" + CompanyId + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.loan_type).html(value.loan_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

function Bind_ROI(ControlId, SelectedVal, CompanyId) {
    // debugger;
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({

        type: "GET",
        url: apiurl + "apiPayroll/Get_LoanType_ROI/0/" + CompanyId + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.rate_of_interest).html(value.rate_of_interest));
            })
            $('#txtloanAmt').val(res[0].loan_amount);
            $('#txttenure').val(res[0].max_tenure);

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function BindEmpBasicLoanDtl(empidd, loantype) {
    //alert(empidd);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/GetLoanRequestMasterByEmpandGrade/" + empidd + "/" + loantype + "/" + $("#ddlCompany").val(),
        type: "POST",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: headerss,
        success: function (response) {
            var res = response;
            _GUID_New();
            if (res != null) {
                $("#txtloanAmt").val(res.loan_amount);
                $("#hdnmaxloanamt").val(res.loan_amount);
                $("#txttenure").val(res.max_tenure);
                $("#ddlrate").append($("<option></option>").val(res.rate_of_interest).html(res.rate_of_interest));
                //$.each(res.rate_of_interest, function (data, value) {
                //    $("#ddlrate").append($("<option></option>").val(value.rate_of_interest).html(value.rate_of_interest));
                //});
            }

            $('#loader').hide();
        },
        error: function (response) {
            _GUID_New();
            messageBox("error", response.responseText);
            $('#loader').hide();
        }
    });
}

