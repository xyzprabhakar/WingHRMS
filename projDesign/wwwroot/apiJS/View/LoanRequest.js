$('#loader').show();
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_emp_id, company_id);

        // BindCompanyList('ddlCompany', company_id);
        //  BindRateList('ddlrate', 0);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlempcode', company_id, 0);
        setSelect('ddlempcode', login_emp_id)

        $('#txtltenure').keypress(function (e) {
            var key = (e.keyCode ? e.keyCode : e.which);
            return ((key >= 48 && key <= 57));
        });
        $('#txtloanAmt').keypress(function (e) {
            var key = (e.keyCode ? e.keyCode : e.which);
            return ((key >= 48 && key <= 57));
        });

        $('#Action').hide();

        $("#txteffectivedt").datepicker({
            dateFormat: 'mm/dd/yy',
            //minDate: 0,

        });


        GetData(login_emp_id, company_id);

        $('#loader').hide();

        $("#ddlCompany").bind("change", function () {
            BindEmployeeListUnderLoginEmpFromAllComp('ddlempcode', $(this).val(), login_emp_id, 0);
            setSelect('ddlempcode', login_emp_id);
            if ($(this).val() == 0) {
                if ($.fn.DataTable.isDataTable('#tblloanrequest')) {
                    $('#tblloanrequest').DataTable().clear().draw();
                }
            }
            else {
                GetData(login_emp_id, $(this).val());
            }

            $("#ddlrate option[value!='0']").remove();
            $("#ddlloan").val('0');
            $("#txtloanAmt").val('');
            $("#hdnmaxloanamt").val('');
            $("#txttenure").val('');
            $("#txteffectivedt").val('');
            $("#purpose_of_loan").val('');

        });

        $("#ddlempcode").bind("change", function () {

            if ($(this).val() == 0) {
                if ($.fn.DataTable.isDataTable('#tblloanrequest')) {
                    $('#tblloanrequest').DataTable().clear().draw();
                }
            }
            else {
                GetData($(this).val(), $("#ddlCompany").val());
            }
            $("#ddlrate option[value!='0']").remove();
            $("#ddlloan").val('0');
            $("#txtloanAmt").val('');
            $("#hdnmaxloanamt").val('');
            $("#txttenure").val('');
            $("#txteffectivedt").val('');
            $("#purpose_of_loan").val('');
        });


        $("#ddlloan").bind("change", function () {

            var emp_idd = $("#ddlempcode").val();//login_emp_id;
            $("#txtloanAmt").val('');
            $("#txttenure").val('');
            //$("#ddlrate").empty();
            if (emp_idd == '' || emp_idd == 0 || emp_idd == null) {
                messageBox("error", "Please Select Employee");
                return false;
            }

            if ($("#ddlCompany").val() == "" || $("#ddlCompany") == null || $("#ddlCompany") == "0") {
                messageBox("error", "Please Select company");
                return false;
            }


            BindEmpBasicLoanDtl(emp_idd, $(this).val());
        });



        $('#btnreset').bind('click', function () {
            // window.location.href = '/Payroll/LoanRequest';
            location.reload();
        });

        $('#btnsave').bind("click", function () {
            // debugger;

            var companyidd = $("#ddlCompany").val();
            var emp_id = $("#ddlempcode").val(); //login_emp_id;
            var Loantype = $('#ddlloan').val();
            var amount = $('#txtloanAmt').val();
            var tenure = $('#txttenure').val();
            var purpose = $('#purpose_of_loan').val();
            var empcode = $("#hdnempname").val();
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

            if (companyidd == "" || companyidd == null || companyidd == "0") {
                iserror = true;
                errormsg = errormsg + 'Please select company !! <br/>';
            }

            if (emp_id == '' || emp_id == '0' || emp_id == null) {
                errormsg = errormsg + 'Employee code cannot be blank!! <br />';
                iserror = true;
            }

            if (Loantype == '' || Loantype == '0') {
                errormsg = "Please select Loan Type !! <br/>";
                iserror = true;
            }

            if (rate == null || rate == "" | rate == "0") {
                errormsg = errormsg + 'Please select Rate of Interest !! <br />';
                iserror = true;
            }
            if (amount == '' || amount == null || amount == '0' || amount == '0.00') {
                errormsg = errormsg + 'Please enter Loan/Advance Amount !! <br />';
                iserror = true;
            }
            if ($('#ddlloan').val() == '1') {
                if (tenure == '' || tenure == null) {
                    errormsg = errormsg + 'Please enter Loan Tenure !! <br />';
                    iserror = true;
                }
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
                return false;
            }

            $('#loader').show();
            var myData = {
                'company_id': companyidd,
                'req_emp_id': emp_id,
                'emp_code': empcode,
                'loan_type': Loantype,
                'loan_amt': amount,
                'loan_tenure': tenure,
                'loan_purpose': purpose,
                'policy': is_active,
                'interest_rate': rate,
                'start_date': startdate,
                'created_by': login_emp_id,
                'last_modified_by': companyidd,
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

                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
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



//$('#ddlloan').bind('change', function () {

//    if ($(this).val() == '2') {
//        $('#ddlrate').prop('disabled', 'disabled');
//        $('#txttenure').prop('disabled', 'disabled');

//    }
//    else {
//        $('#ddlrate').prop('disabled', false);
//        $('#txttenure').prop('disabled', false);
//    }
//});




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

function GetData(empidd, companyid) {

    if ($.fn.DataTable.isDataTable('#tblloanrequest')) {
        $('#tblloanrequest').DataTable().clear().draw();
    }
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestByEmpId/' + empidd + '/' + companyid;
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

            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            $("#tblloanrequest").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return data == 1 ? 'Loan' : data == 2 ? 'Advance' : '';
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

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {
                                return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : data == 3 ? 'In Process' : '';
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {
                                return data == 0 ? 'No' : data == 1 ? 'Yes' : '';
                            }
                        }

                    ],

                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
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
        },
        error: function (error) {
            //alert(error);
            $('#loader').hide();
            messageBox("error", error.responseText);
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
            $('#loader').hide();
            _GUID_New();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            else if (res != null) {
                $("#txtloanAmt").val(res.loan_amount);
                $("#hdnmaxloanamt").val(res.loan_amount);
                $("#txttenure").val(res.max_tenure);
                $("#ddlrate").append($("<option></option>").val(res.rate_of_interest).html(res.rate_of_interest));
                //$.each(res.rate_of_interest, function (data, value) {
                //    $("#ddlrate").append($("<option></option>").val(value.rate_of_interest).html(value.rate_of_interest));
                //});
            }
        },
        error: function (response) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error", response.responseText);
        }
    });
}


//function BindEmployeeCodeee(ControlId, CompanyId, SelectedVal) {
//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
//                //$("#txtEmployeeCode").val(value.emp_code);
//            })

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//                //$("#txtEmployeeCode").val(SelectedVal);
//            }

//            var emp_code = $('#ddlempcode option[value="' + $('#ddlempcode').val() + '"]').text();
//            $("#hdnempname").val(emp_code);
//        },
//        error: function (err) {
//            alert(err.responseText);
//        }
//    });
//}