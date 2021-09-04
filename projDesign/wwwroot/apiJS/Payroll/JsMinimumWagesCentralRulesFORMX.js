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


        $('#btnupdate').hide();
        $('#btnsave').hide();

        BindAllEmp_Company('ddlCompany', login_emp_id, login_comp_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_id, 0);
        BindDesignationList('ddldesignation', login_comp_id, 0);
        GetPayrollMonthYear(login_comp_id);



        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            BindDesignationList('ddldesignation', $(this).val(), 0);
            GetPayrollMonthYear($(this).val());
            $('#loader').hide();
        });

        EL("File1").addEventListener("change", readFile, false);


        $('#ddlPayrollMonth').bind("change", function () {
            if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "0" || $("#ddlCompany").val() == '') {
                messageBox("error", "Please select company");
                return false;
            }
            $('#loader').show();
            // var company_id = localStorage.getItem("company_id")
            var urll;

            urll = apiurl + "apiPayroll/GetPayrollDetailsForMusterForm10/" + $(this).val() + "/" + $("#ddlCompany").val();


            $.ajax({
                type: "GET",
                url: urll,
                data: {},
                contentType: "application/json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                success: function (response) {
                    var res = response;
                    $('#btnFreeze').hide();
                    $('#loader').hide();
                    if (res.data != "" && res.data != null) {
                        if (res.flag == 0) {
                            $('#btnFreeze').show();
                            $('#btnadd').hide();
                            $('#btnresetalldata').hide();
                            $('#btndownload').hide();
                        }
                        else {
                            $('#btnFreeze').hide();
                            $('#btnadd').show();
                            $('#btndownload').show();
                            $('#btnresetalldata').show();


                            //  $('#ddlCompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

                        }
                    }

                    if (res.flag == 0) {
                        $('#div_tblRegisterOfWagesInForm10').css('display', 'block');
                        $('#div_tblRegisterOfWagesInForm102').css('display', 'none');


                        $("#tblRegisterOfWagesInForm10").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": res.data,
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Minimum Wages (Central) Rules Form-X',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-X',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                                    }
                                },

                            ],
                            "columnDefs": [

                            ],
                            "columns": [
                                {
                                    "data": null, "title": "Serial No.", "autoWidth": true
                                },
                                { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
                                { "data": "father_or_husband_name", "name": "father_or_husband_name", "title": "Father/Husband Name", "autoWidth": true },
                                { "data": "designation", "name": "designation", "title": "Designation", "authoWidth": true },
                                {
                                    "data": "basic_minimum_payable", "name": "basic_minimum_payable", "title": "Basic Minimum Rates of Payable", "autoWidth": true
                                },
                                {
                                    "data": "da_minimum_payable", "name": "da_minimum_payable", "title": "DA Minimum Rates of Payable", "autoWidth": true
                                },
                                {
                                    "data": "basic_wages_actually_pay", "name": "basic_wages_actually_pay", "title": "Basic Rate of Wages Actually Paid", "autoWidth": true
                                },
                                {
                                    "data": "da_wages_actually_pay", "name": "da_wages_actually_pay", "title": "DA Rate of Wages Actually Paid", "autoWidth": true
                                },
                                { "data": "total_attand_or_unitof_work_done", "name": "total_attand_or_unitof_work_done", "title": "Total Attandance/ Unit of Work Done", "autoWidth": true },
                                { "data": "overtime_worked", "name": "overtime_worked", "title": "Overtime Worked", "autoWidth": true },
                                { "data": "gross_wages_pay", "name": "gross_wages_pay", "title": "Gross Wages Payable", "autoWidth": true },
                                { "data": "contri_of_employer_pf", "name": "contri_of_employer_pf", "title": "Employer's Contribution to PF", "autoWidth": true },
                                { "data": "hr_deduction", "name": "hr_deduction", "title": "HR Deduction", "autoWidth": true },
                                { "data": "other_deduction", "name": "other_deduction", "title": "Other Deduction", "autoWidth": true },
                                { "data": "total_deduction", "name": "total_deduction", "title": "Total Deduction", "autoWidth": true },
                                { "data": "wages_paid", "name": "wages_paid", "title": "Wages Paid", "autoWidth": true },
                                { "data": "date_of_payment", "name": "date_of_payment", "title": "Date of Payment", "autoWidth": true }
                            ],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                            "lengthMenu": [[50, 100, 500, -1], [50, 100, 500, "All"]]
                        });
                    }
                    if (res.flag == 1) {
                        $('#div_tblRegisterOfWagesInForm10').css('display', 'none');
                        $('#div_tblRegisterOfWagesInForm102').css('display', 'block');

                        $("#tblRegisterOfWagesInForm102").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": res.data,
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Minimum Wages (Central) Rules Form-X',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-X',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                                    }
                                },

                            ],
                            "columnDefs": [

                            ],
                            "columns": [
                                {
                                    "data": null, "title": "Serial No.", "autoWidth": true
                                },
                                { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
                                { "data": "father_or_husband_name", "name": "father_or_husband_name", "title": "Father/Husband Name", "autoWidth": true },
                                { "data": "designation", "name": "designation", "title": "Designation", "authoWidth": true },
                                {
                                    "data": "basic_minimum_payable", "name": "basic_minimum_payable", "title": "Basic Minimum Rates of Payable", "autoWidth": true
                                },
                                {
                                    "data": "da_minimum_payable", "name": "da_minimum_payable", "title": "DA Minimum Rates of Payable", "autoWidth": true
                                },
                                {
                                    "data": "basic_wages_actually_pay", "name": "basic_wages_actually_pay", "title": "Basic Rate of Wages Actually Paid", "autoWidth": true
                                },
                                {
                                    "data": "da_wages_actually_pay", "name": "da_wages_actually_pay", "title": "DA Rate of Wages Actually Paid", "autoWidth": true
                                },
                                { "data": "total_attand_or_unitof_work_done", "name": "total_attand_or_unitof_work_done", "title": "Total Attandance/ Unit of Work Done", "autoWidth": true },
                                { "data": "overtime_worked", "name": "overtime_worked", "title": "Overtime Worked", "autoWidth": true },
                                { "data": "gross_wages_pay", "name": "gross_wages_pay", "title": "Gross Wages Payable", "autoWidth": true },
                                { "data": "contri_of_employer_pf", "name": "contri_of_employer_pf", "title": "Employer's Contribution to PF", "autoWidth": true },
                                { "data": "hr_deduction", "name": "hr_deduction", "title": "HR Deduction", "autoWidth": true },
                                { "data": "other_deduction", "name": "other_deduction", "title": "Other Deduction", "autoWidth": true },
                                { "data": "total_deduction", "name": "total_deduction", "title": "Total Deduction", "autoWidth": true },
                                { "data": "wages_paid", "name": "wages_paid", "title": "Wages Paid", "autoWidth": true },
                                { "data": "date_of_payment", "name": "date_of_payment", "title": "Date of Payment", "autoWidth": true },
                                //{
                                //   // "data": "emp_sign_orthump_exp", "name": "emp_sign_orthump_exp", "title": "Signature or Thump Expression of Employee", "autoWidth": true,
                                //    "title": "Signature or Thump Expression of Employee", "autoWidth": true,
                                //    "render": function (data, type, full, meta) {
                                //        var key = CryptoJS.enc.Base64.parse("#base64Key#");
                                //        var iv = CryptoJS.enc.Base64.parse("#base64IV#");

                                //        var _appSetting_domainn_dec = CryptoJS.AES.decrypt(localStorage.getItem("_appSetting_domainn"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

                                //        return full.emp_sign_orthump_exp == "" || full.emp_sign_orthump_exp == null ? '' : '<img id="profile_imgg" src="' + _appSetting_domainn_dec.toString() + full.emp_sign_orthump_exp + '" style="width: 50%;height: 100px;"/>';
                                //    }
                                //},
                                {
                                    "title": "Edit", "autoWidth": true,
                                    "render": function (data, type, full, meta) {
                                        return '<a href="#" onclick="GetDataById(' + full.employee_id + ', ' + full.payroll_month + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                                    }
                                },
                            ],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                            "lengthMenu": [[50, 100, 500, -1], [50, 100, 500, "All"]]
                        });
                    }

                },
                error: function (err) {
                    alert(err.responseText);
                    $('#loader').hide();
                }
            });
        });



        $("#btnFreeze").bind("click", function () {
            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();

            if (ddlCompany == null || ddlCompany == "0" || ddlCompany == "") {
                messageBox("error", "Please select company");
                return false;
            }

            if (ddlPayrollMonth == null || ddlPayrollMonth == "0" || ddlPayrollMonth == "") {
                messageBox("error", "Please select payroll month");
                return false;
            }


            $('#loader').show();


            var myData = {
                company_id: ddlCompany,
                payroll_month: ddlPayrollMonth,
                emp_sign_orthump_exp: "",
                created_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_PayrollMusterForm10Data';
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
                    _GUID_New();
                    if (statuscode == "0") {
                        $('#loader').hide();
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        $('#loader').hide();
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

        $("#btnresetalldata").bind("click", function () {

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();

            if (ddlCompany == null || ddlCompany == "" || ddlCompany == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            if (ddlPayrollMonth == null || ddlPayrollMonth == "" || ddlPayrollMonth == "0") {
                messageBox("error", "Please select payroll month");
                return false;
            }
            $('#loader').show();
            if (confirm("Are you sure reset all the data for this payroll month?")) {




                var myData = {
                    company_id: ddlCompany,
                    payroll_month: ddlPayrollMonth,
                    modified_by: login_emp_id,
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Reset_PayrollMusterForm10Data';
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

                            alert(Msg);
                            location.reload();
                        }
                        else if (statuscode == "1" || statuscode == '2') {

                            messageBox("error", Msg);
                        }


                    },
                    error: function (err) {
                        $('#loader').hide();
                        _GUID_New();
                        messageBox("error", err.responseText);
                    }
                });
            }
            return false;

        });

        $("#btnadd").bind("click", function () {
            if ($("#ddlCompany").val() == "" | $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            $('#loader').show();
            //var login_comp_id = localStorage.getItem("company_id");

            var login_comp_idd = "";


            login_comp_idd = $("#ddlCompany").val();


            $('#ddlCompany').prop("disabled", "disabled");

            $('#ddlCompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#div_fileds_for_edit').css('display', 'block');

            $('#ddlPayrollMonth').prop("disabled", "disabled");

            $('#ddlPayrollMonth').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_idd, 0);


            $('#btnadd').hide();
            $('#btndownload').hide();
            $('#btnresetalldata').hide();
            $('#btnsave').show();
            $('#loader').hide();
        });


        $("#btnupdate").bind("click", function () {

            $('#loader').show();
            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();
            var ddlemp = $('#ddlemp').val();
            var txt_fatherorhusbandname = $('#txt_fatherorhusbandname').val();
            var txt_BasicMinimumPayable = $('#txt_BasicMinimumPayable').val();
            var txt_DAMinimumPayable = $('#txt_DAMinimumPayable').val();
            var txt_BasicRateofWagesActuallyPay = $('#txt_BasicRateofWagesActuallyPay').val();
            var txt_DARateofWagesActuallyPay = $('#txt_DARateofWagesActuallyPay').val();
            var txt_TotalAttandAndUnitofWorkDone = $('#txt_TotalAttandAndUnitofWorkDone').val();
            var txt_OvertimeWorked = $('#txt_OvertimeWorked').val();
            var txt_GrossWagesPayable = $('#txt_GrossWagesPayable').val();
            var txt_EmployerContributiontoPF = $('#txt_EmployerContributiontoPF').val();
            var txt_HRDeduction = $('#txt_HRDeduction').val();
            var txt_OtherDeduction = $('#txt_OtherDeduction').val();
            var txt_TotalDeduction = $('#txt_TotalDeduction').val();
            var txt_WagesPaid = $('#txt_WagesPaid').val();
            var txt_DateofPayment = $('#txt_DateofPayment').val();


            var form_x_id = $('#form_x_id').val();

            var myData = {
                form_x_id: form_x_id,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                basic_minimum_payable: txt_BasicMinimumPayable,
                da_minimum_payable: txt_DAMinimumPayable,
                basic_wages_actually_pay: txt_BasicRateofWagesActuallyPay,
                da_wages_actually_pay: txt_DARateofWagesActuallyPay,
                total_attand_or_unitof_work_done: txt_TotalAttandAndUnitofWorkDone,
                overtime_worked: txt_OvertimeWorked,
                gross_wages_pay: txt_GrossWagesPayable,
                contri_of_employer_pf: txt_EmployerContributiontoPF,
                hr_deduction: txt_HRDeduction,
                other_deduction: txt_OtherDeduction,
                total_deduction: txt_TotalDeduction,
                wages_paid: txt_WagesPaid,
                date_of_payment: txt_DateofPayment,
                modified_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_PayrollMusterForm10Data';

            var Obj = JSON.stringify(myData);

            var file = document.getElementById("File1").files[0];

            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                dataType: "json",
                data: formData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
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

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();
            var ddlemp = $('#ddlemp').val();
            var txt_fatherorhusbandname = $('#txt_fatherorhusbandname').val();

            var txt_BasicMinimumPayable = $('#txt_BasicMinimumPayable').val();
            var txt_DAMinimumPayable = $('#txt_DAMinimumPayable').val();
            var txt_BasicRateofWagesActuallyPay = $('#txt_BasicRateofWagesActuallyPay').val();
            var txt_DARateofWagesActuallyPay = $('#txt_DARateofWagesActuallyPay').val();
            var txt_TotalAttandAndUnitofWorkDone = $('#txt_TotalAttandAndUnitofWorkDone').val();
            var txt_OvertimeWorked = $('#txt_OvertimeWorked').val();
            var txt_GrossWagesPayable = $('#txt_GrossWagesPayable').val();
            var txt_EmployerContributiontoPF = $('#txt_EmployerContributiontoPF').val();
            var txt_HRDeduction = $('#txt_HRDeduction').val();
            var txt_OtherDeduction = $('#txt_OtherDeduction').val();
            var txt_TotalDeduction = $('#txt_TotalDeduction').val();
            var txt_WagesPaid = $('#txt_WagesPaid').val();
            var txt_DateofPayment = $('#txt_DateofPayment').val();
            var myData = {
                company_id: ddlCompany,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                basic_minimum_payable: txt_BasicMinimumPayable,
                da_minimum_payable: txt_DAMinimumPayable,
                basic_wages_actually_pay: txt_BasicRateofWagesActuallyPay,
                da_wages_actually_pay: txt_DARateofWagesActuallyPay,
                total_attand_or_unitof_work_done: txt_TotalAttandAndUnitofWorkDone,
                overtime_worked: txt_OvertimeWorked,
                gross_wages_pay: txt_GrossWagesPayable,
                contri_of_employer_pf: txt_EmployerContributiontoPF,
                hr_deduction: txt_HRDeduction,
                other_deduction: txt_OtherDeduction,
                total_deduction: txt_TotalDeduction,
                wages_paid: txt_WagesPaid,
                date_of_payment: txt_DateofPayment,
                created_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Add_PayrollMusterForm10Data';
            var Obj = JSON.stringify(myData);

            var file = document.getElementById("File1").files[0];

            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                dataType: "json",
                data: formData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
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


        $("#ddlemp").bind("change", function () {

            BindEmployeeDetailss($(this).val())


        });

        $("#btndownload").bind("click", function () {
            $('#loader').show();

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();


            $.ajax({
                type: "GET",
                url: apiurl + "apiPayroll/PayrollMuster10PdfGenerator/" + ddlCompany + "/" + ddlPayrollMonth,
                data: {},
                contentType: "application/json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    if (statuscode == "0") {
                        var key = CryptoJS.enc.Base64.parse("#base64Key#");
                        var iv = CryptoJS.enc.Base64.parse("#base64IV#");

                        var _appSetting_domainn_dec = CryptoJS.AES.decrypt(localStorage.getItem("_appSetting_domainn"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);


                        var appp_domainnn = _appSetting_domainn_dec;//localStorage.getItem("_appSetting_domainn");
                        window.open(appp_domainnn + Msg, '_blank');
                    }
                    else if (statuscode == "1" || statuscode == '2') {

                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    messageBox("error", err.responseText);
                }
            });

        });

    }, 2000);// end timeout

});


function GetPayrollMonthYear(company_idd) {
    $('#loader').show();
    //var company_id = localStorage.getItem("company_id");


    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/GetProcessedPayrollMonthYear/" + company_idd,
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


function GetDataById(employee_id, payroll_month) {
    if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "0" || $("#ddlCompany").val() == '') {
        messageBox("error", "please select company");
        return false;
    }

    $('#loader').show();
    //var login_comp_id = localStorage.getItem("company_id");

    $('#btnadd').hide();
    $('#btnresetalldata').hide();
    $('#btndownload').hide();
    $('#btnsave').hide();
    $('#btnupdate').show();

    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/GetPayrollDetailsForMusterForm10ByEmployee/" + payroll_month + "/" + employee_id,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            debugger;
            var res = response;
            $('#loader').hide();
            $('#div_fileds_for_edit').css('display', 'block');

            $('#ddlPayrollMonth').prop("disabled", "disabled");

            $('#ddlPayrollMonth').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddlemp').prop("disabled", "disabled");

            $('#ddlemp').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddldesignation').prop("disabled", "disabled");

            $('#ddldesignation').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");




            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $("#ddlCompany").val(), res.employee_id);
            BindDesignationList('ddldesignation', $("#ddlCompany").val(), res.desig_id);

            // BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_id, res.employee_id);

            //$('#ddlemp').prop("disabled", "disabled");

            //$('#ddlemp').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


            $("#txt_fatherorhusbandname").val(res.father_or_husband);
            $('#txt_fatherorhusbandname').prop("disabled", "disabled");

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");



            $("#txt_BasicMinimumPayable").val(res.basic_minimum_payable);
            $("#txt_DAMinimumPayable").val(res.da_minimum_payable);
            $("#txt_BasicRateofWagesActuallyPay").val(res.basic_wages_actually_pay);
            $("#txt_DARateofWagesActuallyPay").val(res.da_wages_actually_pay);
            $("#txt_TotalAttandAndUnitofWorkDone").val(res.total_attand_or_unitof_work_done);
            $("#txt_OvertimeWorked").val(res.overtime_worked);
            $("#txt_GrossWagesPayable").val(res.gross_wages_pay);
            $("#txt_EmployerContributiontoPF").val(res.contri_of_employer_pf);
            $("#txt_HRDeduction").val(res.hr_deduction);
            $("#txt_OtherDeduction").val(res.other_deduction);
            $("#txt_TotalDeduction").val(res.total_deduction);
            $("#txt_WagesPaid").val(res.wages_paid);
            $("#txt_DateofPayment").val(res.date_of_payment);
            $("#emp_sign_orthump_exp").val(res.emp_sign_orthump_exp);
            $("#form_x_id").val(res.form_x_id);
            $('#loader').hide();
        },
        error: function (err) {
            $('#div_fileds_for_edit').css('display', 'none');
            $('#loader').hide();
            alert(err.responseText);
        }
    });

}



function BindEmployeeDetailss(employee_idd) {

    if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
        messageBox("error", "Please select Company");
        return false;
    }

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/GetEmpDetailByEmpIDForPayrollMuster/" + employee_idd,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            //var login_comp_id = localStorage.getItem("company_id");

            BindDesignationList('ddldesignation', $("#ddlCompany").val(), res.desig_id);

            $('#ddldesignation').prop("disabled", "disabled");
            $('#ddldesignation').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $("#txt_fatherorhusbandname").val(res.father_or_husband);
            $('#txt_fatherorhusbandname').prop("disabled", "disabled");

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}

function readFile() {
    $('#loader').show();
    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#File1").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg") {

            }
            else {
                $("#File1").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG,PDF. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);

        $('#loader').hide();
    }
}


function EL(id) { return document.getElementById(id); }





