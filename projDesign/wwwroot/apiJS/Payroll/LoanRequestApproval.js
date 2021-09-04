var emp_role_idd;
var company_idd;
var login_emp;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp, company_idd);
        Get_EmployeeDetailsFromEmpMasterByComp('ddlloanreq', company_idd, login_emp, emp_role_idd, 0);





        $('#btnUpdate').hide();

        $('#div_approval').hide();
        $('#btn_view').hide();
        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/LoanRequestApproval';

        });
        GetData(login_emp, emp_role_idd);

        $('#ddlCompany').bind("change", function () {
            if ($(this).val() == 0) {
                $("#ddlloanreq").empty();
                $("#hdn_loan_req_Id").val('');
                $("#txtdesignation").val('');
                $("#txtdepartment").val('');
                $("#txtgrade").val('');
                $("#txtmonthlysalary").val('');
                $("#txtroi").val('');
                $("#txtloantype").val('');
                $("#txtloanamt").val('');
                $("#txtloantenure").val('');
                $("#txtloanpurpose").val('');
            }
            else {
                Get_EmployeeDetailsFromEmpMasterByComp('ddlloanreq', $(this).val(), login_emp, emp_role_idd, 0);
                BindEmployeeDetailsByEmp(0, 0, $(this).val());
            }

        });


        $('#btn_view').bind('click', function () {
            $('#btn_apprval').show();
            $('#div_report').show();
            $('#div_approval').hide();
            $('#btn_view').hide();


        });
        $('#btn_apprval').bind('click', function () {
            $('#btn_view').show();
            $('#div_approval').show();
            $('#div_report').hide();
            $('#btn_apprval').hide();

        });


        $('#ddlloanreq').bind("change", function () {
            if ($(this).val() != "") {
                var fullvalue = $(this).val().split('/');
                var emp_id = fullvalue[0];
                var loan_req_id = fullvalue[1];
                BindEmployeeDetailsByEmp(emp_id, loan_req_id, $("#ddlCompany").val());
            }
            else {
                $("#hdn_loan_req_Id").val('');
                $("#txtdesignation").val('');
                $("#txtdepartment").val('');
                $("#txtgrade").val('');
                $("#txtmonthlysalary").val('');
                $("#txtroi").val('');
                $("#txtloantype").val('');
                $("#txtloanamt").val('');
                $("#txtloantenure").val('');
                $("#txtloanpurpose").val('');
            }


        });


        $('#btnSubmit').bind("click", function () {
            // debugger;

            var loan_req_id = $("#hdn_loan_req_Id").val();
            var emp_and_loan_req_id = $("#ddlloanreq").val().toString().split('/');

            var companyidd = $("#ddlCompany").val();
            var emp_id = emp_and_loan_req_id[0];
            var designation = $("#txtdesignation").val();
            var dept = $("#txtdepartment").val();
            var grade = $("#txtgrade").val();
            var salary = $("#txtmonthlysalary").val();
            var rate = $("#txtroi").val();
            var loantype = $("#txtloantype").val();
            var amt = $("#txtloanamt").val();
            var tenure = $("#txtloantenure").val();
            var reason = $("#txtloanpurpose").val();
            var status = $("#ddlstarus").val();

            // var is_active = 0;
            var errormsg = '';
            var iserror = false;
            var error = 0;


            //if ($("input[name='chkpolicy']:checked")) {
            //    if ($("input[name='chkpolicy']:checked").val() == '1') {
            //        is_active = 1;
            //    }
            //}
            //validation part
            if (companyidd == "" || companyidd == null || companyidd == "0") {
                iserror = true;
                errormsg = errormsg + 'Please select company !! <br/>';
            }
            if (loan_req_id == '' || loan_req_id == '0') {
                errormsg = errormsg + 'Please fill loan request id !! <br />';
                iserror = true;
            }
            //if (designation == '' || designation == null) {
            //    errormsg = errormsg + 'Please enter Designation !! <br />';
            //    iserror = true;
            //}
            //if (dept == '' || dept == null) {
            //    errormsg = "Please enter Department !! <br/>";
            //    iserror = true;
            //}
            //if (grade == '' || grade == null) {
            //    errormsg = errormsg + 'Please enter Grade !! <br />';
            //    iserror = true;
            //}
            //if (salary == '' || salary == '0') {
            //    errormsg = errormsg + 'Please enter Salary !! <br />';
            //    iserror = true;
            //}
            if (status == '' || status == '0') {
                errormsg = errormsg + 'Please Select loan Approval !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'loan_req_id': loan_req_id,
                'emp_id': emp_id,
                'req_emp_id': emp_id,
                'is_approve': status,
                //'is_final_approval': status, 
                'is_final_approver': status,
                'last_modified_by': login_emp,
                'approver_role_id': emp_role_idd,
                'company_id': companyidd,
            };
            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/LoanRequestApproval';
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
                        window.location.href = localStorage.getItem("ApiUrl") + "/apiPayroll/LoanRequestApproval";
                        location.reload();
                        //location.reload();
                        //$("#hdn_loan_req_Id").val('');
                        //$("#txtdesignation").val('');
                        //$("#txtdepartment").val('');
                        //$("#txtgrade").val('');
                        //$("#txtmonthlysalary").val('');
                        //$("#txtroi").val('');
                        //$("#txtloantype").val('');
                        //$("#txtloanamt").val('');
                        //$("#txtloantenure").val('');
                        //$("#txtloanpurpose").val(''); 
                        //$('#btnupdate').hide();

                        // messageBox("success", Msg);
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
function Get_EmployeeDetailsFromEmpMasterByComp(ControlId, CompanyId, login_emp_id, login_emp_rol_id, SelectedVal) {
    $('#loader').show();

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        //url: apiurl + "apipayroll/Get_EmployeeDetailsFromEmpMasterByComp/" + CompanyId,
        url: apiurl + "apiPayroll/Get_Pending_Loan_Request/" + CompanyId + "/" + login_emp_id + "/" + login_emp_rol_id,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.emp_id + "/" + value.loan_req_id).html(value.emp_code));
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
function BindEmployeeDetailsByEmp(employee_id, loan_req_id, companyid) {
    $('#loader').show();
    // debugger;
    $("#hdn_loan_req_Id").val('');
    $("#txtdesignation").val('');
    $("#txtdepartment").val('');
    $("#txtgrade").val('');
    $("#txtmonthlysalary").val('');
    $("#txtroi").val('');
    $("#txtloantype").val('');
    $("#txtloanamt").val('');
    $("#txtloantenure").val('');
    $("#txtloanpurpose").val('');

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_EmpLoanData/' + employee_id + "/" + loan_req_id + "/" + companyid,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                $('#loader').hide();
                return false
            }

            var data = response.emp_loan_data;
            var monthly_salary_data = response.monthly_salary;
            if (data != null && data.length > 0) {
                $("#hdn_loan_req_Id").val(data[0].loan_req_id);
                $("#txtdesignation").val(data[0].des_name);
                $("#txtdepartment").val(data[0].dept_name);
                $("#txtgrade").val(data[0].grade_name);
                //$("#txtmonthlysalary").val(0);
                $("#txtmonthlysalary").val(monthly_salary_data);
                $("#txtroi").val(data[0].roi);
                $("#txtloantype").val(data[0].loan_type);
                $("#txtloanamt").val(data[0].loan_amt);
                $("#txtloantenure").val(data[0].loan_tenure);
                $("#txtloanpurpose").val(data[0].loan_purpose);

            }

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

}
//function BindCompanyList(ControlId, SelectedVal) {

//    $('#loader').show();
//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiMasters/Get_CompanyList",
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//            })
//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (err) {
//            $('#loader').hide();
//            alert(err.responseText);
//        }
//    });
//}


function GetData(login_emp, login_emp_rol_id) {
    // debugger;

    if ($.fn.DataTable.isDataTable('#tblloanreqdetails')) {
        $('#tblloanreqdetails').DataTable().clear().draw();
    }

    $('#loader').show();
    var apiurl = "";
    // var HaveDisplay = ISDisplayMenu("Display Company List");

    // if (HaveDisplay == 0) {
    //// if (emp_role_idd == "2" || emp_role_idd=="6") {
    //     apiurl = localStorage.getItem("ApiUrl") + '/apiPayroll/Get_LoanRequestDetailsByCompIDD/' + login_emp + "/" + login_emp_rol_id + "/" + company_idd;
    // }
    // else {
    apiurl = localStorage.getItem("ApiUrl") + '/apiPayroll/Get_LoanRequestDetails/' + login_emp + "/" + login_emp_rol_id;
    //}

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

            $("#tblloanreqdetails").DataTable({
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

                                return data == 1 ? 'Loan' : 'Advance';
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
                                return data == '0' ? 'Pending' : data == '1' ? 'Accepted' : data == '2' ? 'Rejected' : data == '3' ? 'In Process' : '';
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {
                                return data == '0' ? 'Pending' : data == '1' ? 'Accepted' : data == '2' ? 'Rejected' : data == '3' ? 'In Process' : '';
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {
                                return data == '0' ? 'No' : data == '1' ? 'Yes' : '';
                            }
                        }

                        //{
                        //    targets: [5],
                        //    "class": "text-center"
                        //}
                    ],

                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    //{ "data": "loan_req_id", "name": "loan_req_id", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee", "autoWidth": true },
                    { "data": "loan_type", "name": "loan_type", "title": "Loan Type", "autoWidth": true },
                    { "data": "loan_amt", "name": "loan_amt", "title": "Loan Amount", "autoWidth": true },
                    { "data": "interest_rate", "name": "interest_rate", "title": "Interest Rate", "autoWidth": true },
                    { "data": "loan_tenure", "name": "loan_tenure", "title": "Tenure(In Months)", "autoWidth": true },
                    { "data": "loan_purpose", "name": "loan_purpose", "title": "Loan Purpose", "autoWidth": true },
                    { "data": "start_date", "name": "start_date", "title": "Effective Date", "autoWidth": true },
                    { "data": "is_approve", "name": "is_approve", "title": "Current Status", "autoWidth": true },
                    { "data": "is_final_approver", "name": "is_final_approver", "title": "Final Status", "autoWidth": true },
                    { "data": "is_closed", "name": "is_closed", "title": "Closed ?", "autoWidth": true }

                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.loan_req_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ], "fnRowCallback": function (nRow, aData, iDisplayIndex) {
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
            console.log("error");
        }
    });

}

function SetInitialData(id) {
    $('#loader').show();
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

            $("#ddlempcode").val(data.emp_code);
            $('#txtloanreq').val(data.employee_first_name);
            $('#txtloantype').val(data.loan_type);
            $('#txtloanamt').val(data.loan_amt);
            $('#txtloantenure').val(data.loan_tenure);
            $('#txtloanpurpose').val(data.loan_purpose);

            $('#content').hide();

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}