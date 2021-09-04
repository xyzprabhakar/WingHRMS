$('#loader').show();

var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }



        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);


        $('#ddlcompany').bind("change", function () {



            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);

        });



        $('#ddlEmployeeCode').bind("change", function () {
            BindLoanByEmployee('ddlloan', $(this).val(), 0);
        });

        $('#ddlloan').bind("change", function () {
            GetLoanPaymentDeatil($(this).val());
        });


        $(function () {
            $("input[id*='txtprincipal_amount']").keydown(function (event) {


                if (event.shiftKey == true) {
                    event.preventDefault();
                }

                if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

                } else {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                    event.preventDefault();

            });
        });


        $('#loader').hide();

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtDate = $("#txtDate").val();
            var txt_amount = $("#txt_amount").val();
            var ddlloan = $("#ddlloan").val();
            var txt_remark = $("#txt_remark").val();

            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (txtDate == null || txtDate == '') {
                errormsg = "Please Select Date ...! ";
                iserror = true;
            }
            if (txt_amount == null || txt_amount == '') {
                errormsg = "Please Enter Amount ...! ";
                iserror = true;
            }
            if (ddlloan == null || ddlloan == '') {
                errormsg = "Please Select Loan ...! ";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var myData = {
                'req_emp_id': ddlEmployeeCode,
                'date': txtDate,
                'principal_amount': txt_amount,
                'loan_id': ddlloan,
                'remark': txt_remark,
                'status': 1,
                'is_deleted': is_deleted,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_LoanRepayment';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            // debugger;
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
                    if (statuscode == "1") {
                        alert(Msg);
                        GetLoanPaymentDeatil(ddlloan);
                        //BindCompanyList('ddlcompany', 0);
                        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
                        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
                        $("#txtDate").val('');
                        $("#txt_amount").val('');
                        BindLoanByEmployee('ddlloan', 0, 0);
                        $("#txt_remark").val('');

                    }
                    else if (statuscode == "0") {
                        alert(Msg);
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



//--------bind data in jquery data table
function GetLoanPaymentDeatil(loan_id) {
    //// debugger;;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/GetLoanPaymentDeatil/' + loan_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            $('#loader').hide();
            $("#tblRepayment").DataTable({
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
                            targets: [1],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [2],
                            render: function (data, type, row) {
                                return data == '0' ? 'Deduction' : 'Repayment'
                            }
                        },
                    ],

                "columns": [
                    { "data": null },
                    { "data": "date", "name": "date", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    { "data": "principal_amount", "name": "principal_amount", "autoWidth": true },
                    { "data": "interest_component", "name": "interest_component", "autoWidth": true },
                    { "data": "loan_balance", "name": "loan_balance", "autoWidth": true },
                    { "data": "remark", "name": "remark", "autoWidth": true }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}
