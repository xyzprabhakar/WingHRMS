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
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;


        $('#btnupdate').hide();
        $('#btnsave').hide();

        BindAllEmp_Company('ddlCompany', login_emp_id, login_comp_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_id, 0);
        BindDepartmentListForddl('ddldepartment', login_emp_id, 0);
        BindDesignationList('ddldesignation', login_emp_id, 0);

        GetPayrollMonthYear(login_comp_id);


        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            BindDesignationList('ddldesignation', $(this).val(), 0);
            GetPayrollMonthYear($(this).val());
            $('#loader').hide();
        });


        $('#ddlPayrollMonth').bind("change", function () {
            if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "" || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            $('#loader').show();
            //var company_id = localStorage.getItem("company_id")

            var urll;

            urll = apiurl + "apiPayroll/GetPayrollDetailsForMusterForm4/" + $(this).val() + "/" + $("#ddlCompany").val();


            $.ajax({
                type: "GET",
                url: urll,
                data: "{}",
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
                        $('#loader').hide();
                        $('#div_tblRegisterOfOvertimeInForm4').css('display', 'block');
                        $('#div_tblRegisterOfOvertimeInForm42').css('display', 'none');


                        $("#tblRegisterOfOvertimeInForm4").DataTable({
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
                                    title: 'Minimum Wages (Central) Rules Form-IV',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-IV',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
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
                                { "data": "gender", "name": "gender", "title": "Sex", "authoWidth": true },
                                { "data": "designation_anddepartment", "name": "designation_anddepartment", "title": "Designation and Department", "autoWidth": true },
                                {
                                    "data": "overtime_work_dt", "name": "overtime_work_dt", "title": "Date on which Overtime Worked", "autoWidth": true
                                },
                                {
                                    "data": "extent_overtime", "name": "extent_overtime", "title": "Extent of Overtime on each occasion", "autoWidth": true
                                },
                                {
                                    "data": "total_overtime_worked", "name": "total_overtime_worked", "title": "Total Overtime worked or production in case of piece-workers", "autoWidth": true
                                },
                                {
                                    "data": "normal_hr", "name": "normal_hr", "title": "Normal hours", "autoWidth": true
                                },
                                { "data": "normal_rate", "name": "normal_rate", "title": "Normald Rate", "autoWidth": true },
                                { "data": "overtime_rate", "name": "overtime_rate", "title": "Overtime Rate", "autoWidth": true },
                                { "data": "normal_earning", "name": "normal_earning", "title": "Normal Earnings", "autoWidth": true },
                                { "data": "overtime_earning", "name": "overtime_earning", "title": "Overtime Earnings", "autoWidth": true },
                                { "data": "total_earning", "name": "total_earning", "title": "Total Earnings", "autoWidth": true },
                                { "data": "date_ofpayment", "name": "date_ofpayment", "title": "Date on which Overtime Payment Made", "autoWidth": true }
                            ],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                            "lengthMenu": [[50, 100, 500, -1], [50, 100, 500, "All"]]
                        });
                    }
                    if (res.flag == 1) {
                        $('#div_tblRegisterOfOvertimeInForm4').css('display', 'none');
                        $('#div_tblRegisterOfOvertimeInForm42').css('display', 'block');

                        $('#loader').hide();

                        $("#tblRegisterOfOvertimeInForm42").DataTable({
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
                                    title: 'Minimum Wages (Central) Rules Form-IV',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-IV',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
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
                                { "data": "employee_name", "name": "employee_name", "title": "Name", "autoWidth": true },
                                { "data": "father_or_husband_name", "name": "father_or_husband_name", "title": " Father's/Husband's Name", "autoWidth": true },
                                { "data": "gender", "name": "gender", "title": "Sex", "authoWidth": true },
                                { "data": "designation_anddepartment", "name": "designation_anddepartment", "title": "Designation and Department", "autoWidth": true },
                                {
                                    "data": "overtime_work_dt", "name": "overtime_work_dt", "title": "Date on which Overtime Worked", "autoWidth": true
                                },
                                {
                                    "data": "extent_overtime", "name": "extent_overtime", "title": "Extent of Overtime on each occasion", "autoWidth": true
                                },
                                {
                                    "data": "total_overtime_worked", "name": "total_overtime_worked", "title": "Total Overtime worked or production in case of piece-workers", "autoWidth": true
                                },
                                {
                                    "data": "normal_hr", "name": "normal_hr", "title": "Normal hours", "autoWidth": true
                                },
                                {
                                    "data": "normal_rate", "name": "normal_rate", "title": "Normald Rate", "autoWidth": true
                                },
                                { "data": "overtime_rate", "name": "overtime_rate", "title": "Overtime Rate", "autoWidth": true },
                                { "data": "normal_earning", "name": "normal_earning", "title": "Normal Earnings", "autoWidth": true },
                                { "data": "overtime_earning", "name": "overtime_earning", "title": "Overtime Earnings", "autoWidth": true },
                                { "data": "total_earning", "name": "total_earning", "title": "Total Earnings", "autoWidth": true },
                                { "data": "date_ofpayment", "name": "date_ofpayment", "title": "Date on which Overtime Payment Made", "autoWidth": true },
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

            $('#loader').show();
            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();

            var myData = {
                'company_id': ddlCompany,
                'payroll_month': ddlPayrollMonth,
                'created_by': login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_PayrollMusterForm4Data';
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
            $('#loader').show();
            if (confirm("Are you sure reset all the data for this payroll month?")) {


                var ddlCompany = $('#ddlCompany').val();
                var ddlPayrollMonth = $('#ddlPayrollMonth').val();


                var myData = {
                    'company_id': ddlCompany,
                    'payroll_month': ddlPayrollMonth,
                    'modified_by': login_emp_id,
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Reset_PayrollMusterForm4Data';
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
            if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            $('#loader').show();
            //var login_comp_id = localStorage.getItem("company_id");
            var login_comp_idd = "";


            login_comp_idd = $("#ddlCompany").val();


            $('#ddlCompany').prop("disabled", "disabled");

            // $('#ddlCompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


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
            var txt_DateofOvertimeWorked = $('#txt_DateofOvertimeWorked').val();
            var txt_ExtentOvertime = $('#txt_ExtentOvertime').val();
            var txt_TotalOvertimeWorked = $('#txt_TotalOvertimeWorked').val();
            var txt_NormalHr = $('#txt_NormalHr').val();
            var txt_NormalRate = $('#txt_NormalRate').val();
            var txt_OvertimeRate = $('#txt_OvertimeRate').val();
            var txt_NormalEarning = $('#txt_NormalEarning').val();
            var txt_OvertimeEarning = $('#txt_OvertimeEarning').val();
            var txt_TotalEarning = $('#txt_TotalEarning').val();
            var txt_DateofOvertimePaymentMade = $('#txt_DateofOvertimePaymentMade').val();


            var form_iv_id = $('#form_iv_id').val();

            var myData = {
                form_iv_id: form_iv_id,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                overtime_work_dt: txt_DateofOvertimeWorked,
                extent_overtime: txt_ExtentOvertime,
                total_overtime_worked: txt_TotalOvertimeWorked,
                normal_hr: txt_NormalHr,
                normal_rate: txt_NormalRate,
                overtime_rate: txt_OvertimeRate,
                normal_earning: txt_NormalEarning,
                overtime_earning: txt_OvertimeEarning,
                total_earning: txt_TotalEarning,
                date_ofpayment: txt_DateofOvertimePaymentMade,
                modified_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_PayrollMusterForm4Data';
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

            var txt_DateofOvertimeWorked = $('#txt_DateofOvertimeWorked').val();
            var txt_ExtentOvertime = $('#txt_ExtentOvertime').val();
            var txt_TotalOvertimeWorked = $('#txt_TotalOvertimeWorked').val();
            var txt_NormalHr = $('#txt_NormalHr').val();
            var txt_NormalRate = $('#txt_NormalRate').val();
            var txt_OvertimeRate = $('#txt_OvertimeRate').val();
            var txt_NormalEarning = $('#txt_NormalEarning').val();
            var txt_OvertimeEarning = $('#txt_OvertimeEarning').val();
            var txt_TotalEarning = $('#txt_TotalEarning').val();
            var txt_DateofOvertimePaymentMade = $('#txt_DateofOvertimePaymentMade').val();

            var myData = {
                company_id: ddlCompany,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                overtime_work_dt: txt_DateofOvertimeWorked,
                extent_overtime: txt_ExtentOvertime,
                total_overtime_worked: txt_TotalOvertimeWorked,
                normal_hr: txt_NormalHr,
                normal_rate: txt_NormalRate,
                overtime_rate: txt_OvertimeRate,
                normal_earning: txt_NormalEarning,
                overtime_earning: txt_OvertimeEarning,
                total_earning: txt_TotalEarning,
                date_ofpayment: txt_DateofOvertimePaymentMade,
                created_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Add_PayrollMusterForm4Data';
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
                url: apiurl + "apiPayroll/PayrollMuster4PdfGenerator/" + ddlCompany + "/" + ddlPayrollMonth,
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


                        var appp_domainnn = _appSetting_domainn_dec;

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

    if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
        messageBox("error", "Please select company");
        return false;
    }
    $('#loader').show();
    // var login_comp_id = localStorage.getItem("company_id");

    $('#btnadd').hide();
    $('#btnresetalldata').hide();
    $('#btndownload').hide();
    $('#btnsave').hide();
    $('#btnupdate').show();

    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/GetPayrollDetailsForMusterForm4ByEmployee/" + payroll_month + "/" + employee_id,
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


            $('#ddldepartment').prop("disabled", "disabled");

            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddldesignation').prop("disabled", "disabled");

            $('#ddldesignation').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddldepartment').prop("disabled", "disabled");

            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");



            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $("#ddlCompany").val(), res.employee_id);
            BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);
            BindDesignationList('ddldesignation', $("#ddlCompany").val(), res.desig_id);
            // BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);


            // BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_id, res.employee_id);

            //$('#ddlemp').prop("disabled", "disabled");

            //$('#ddlemp').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");




            $("#txt_fatherorhusbandname").val(res.father_or_husband);
            $('#txt_fatherorhusbandname').prop("disabled", "disabled");

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $("#txt_DateofOvertimeWorked").val(res.overtime_work_dt);
            $("#txt_ExtentOvertime").val(res.extent_overtime);
            $("#txt_TotalOvertimeWorked").val(res.total_overtime_worked);
            $("#txt_NormalHr").val(res.normal_hr);
            $("#txt_NormalRate").val(res.normal_rate);
            $("#txt_OvertimeRate").val(res.overtime_rate);
            $("#txt_NormalEarning").val(res.normal_earning);
            $("#txt_OvertimeEarning").val(res.overtime_earning);
            $("#txt_TotalEarning").val(res.total_earning);
            $("#txt_DateofOvertimePaymentMade").val(res.date_ofpayment);
            $("#form_iv_id").val(res.form_iv_id);

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
        messageBox("error", "Please select company");
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
            // var login_comp_id = localStorage.getItem("company_id");


            BindDesignationList('ddldesignation', $("#ddlCompany").val(), res.desig_id);
            BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);

            $('#ddldesignation').prop("disabled", "disabled");
            $('#ddldesignation').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddldepartment').prop("disabled", "disabled");
            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $("#txt_fatherorhusbandname").val(res.father_or_husband);
            $('#txt_fatherorhusbandname').prop("disabled", "disabled");

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
}











