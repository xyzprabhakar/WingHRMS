$('#loader').show();
var emp_role_idd;
var login_comp_id;
var login_emp_idd;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#btnupdate').hide();
        $('#btnsave').hide();
        $('#loader').hide();

        BindAllEmp_Company('ddlCompany', login_emp_idd, login_comp_id);
        $('#ddlgender').prop("disabled", "disabled");
        $('#ddldepartment').prop("disabled", "disabled");

        BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_comp_id, 0);
        BindDepartmentListForddl('ddldepartment', login_comp_id, 0);
        GetPayrollMonthYear(login_comp_id);



        $('#loader').hide();
        //GetPayrollMonthYear();
        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            GetPayrollMonthYear($(this).val());
            $('#loader').hide();
        });

        $('#ddlemp').bind("change", function () {
            $('#loader').show();

            //var defaultcompany = localStorage.getItem("company_id");
            BindEmployeeDetailss($(this).val());
            $('#loader').hide();
        });

        $('#ddlPayrollMonth').bind("change", function () {
            $('#loader').show();
            if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "" || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            var urll;

            // var company_id = localStorage.getItem("company_id")

            urll = apiurl + "apiPayroll/GetPayrollDetailsForMusterForm1/" + $(this).val() + "/" + $("#ddlCompany").val();


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



                            // $('#ddlCompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


                        }
                    }

                    if (res.flag == 0) {
                        $('#div_tblRegisterOfFimesInForm1').css('display', 'block');
                        $('#div_tblRegisterOfFimesInForm12').css('display', 'none');


                        $("#tblRegisterOfFimesInForm1").DataTable({
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
                                    title: 'Minimum Wages (Central) Rules Form-I',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-I',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
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
                                { "data": "employe_name", "name": "employe_name", "title": "Name", "autoWidth": true },
                                { "data": "father_or_husband", "name": "father_or_husband", "title": " Father's/Husband's Name", "autoWidth": true },
                                { "data": "department", "name": "department", "title": "Department", "autoWidth": true },
                                {
                                    "data": "nature_and_date", "name": "nature_and_date", "title": "Nature and date of the offence for which Fine imposed", "autoWidth": true
                                },
                                {
                                    "data": "whether_workman", "name": "whether_workman", "title": "Whether Workman Showed Cause Against Fine or not, if so enter date", "autoWidth": true
                                },
                                {
                                    "data": "rate_of_wages", "name": "rate_of_wages", "title": "Rate of Wages", "autoWidth": true
                                },
                                {
                                    "data": "date_of_fine_imposed", "name": "date_of_fine_imposed", "title": "Date of fine Imposed", "autoWidth": true
                                },
                                {
                                    "data": "amt_of_fine_imposed", "name": "amt_of_fine_imposed", "title": "Amount of fine Imposed", "autoWidth": true
                                },
                                {
                                    "data": "date_realised", "name": "date_realised", "title": "Date on which fine realised", "autoWidth": true
                                },
                                { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                            ],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                            "lengthMenu": [[50, 100, 500, -1], [50, 100, 500, "All"]]
                        });
                    }
                    if (res.flag == 1) {
                        $('#div_tblRegisterOfFimesInForm1').css('display', 'none');
                        $('#div_tblRegisterOfFimesInForm12').css('display', 'block');

                        $("#tblRegisterOfFimesInForm12").DataTable({
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
                                    title: 'Minimum Wages (Central) Rules Form-I',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Minimum Wages (Central) Rules Form-I',
                                    extend: 'pdfHtml5',
                                    orientation: 'landscape',
                                    pageSize: 'LEGAL',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
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
                                { "data": "employe_name", "name": "employe_name", "title": "Name", "autoWidth": true },
                                { "data": "father_or_husband", "name": "father_or_husband", "title": " Father's/Husband's Name", "autoWidth": true },
                                { "data": "department", "name": "department", "title": "Department", "autoWidth": true },
                                {
                                    "data": "nature_and_date", "name": "nature_and_date", "title": "Nature and date of the offence for which Fine imposed", "autoWidth": true
                                },
                                {
                                    "data": "whether_workman", "name": "whether_workman", "title": "Whether Workman Showed Cause Against Fine or not, if so enter date", "autoWidth": true
                                },
                                {
                                    "data": "rate_of_wages", "name": "rate_of_wages", "title": "Rate of Wages", "autoWidth": true
                                },
                                {
                                    "data": "date_of_fine_imposed", "name": "date_of_fine_imposed", "title": "Date of fine Imposed", "autoWidth": true
                                },
                                {
                                    "data": "amt_of_fine_imposed", "name": "amt_of_fine_imposed", "title": "Amount of fine Imposed", "autoWidth": true
                                },
                                {
                                    "data": "date_realised", "name": "date_realised", "title": "Date on which fine realised", "autoWidth": true
                                },
                                { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
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
                    $('#loader').hide();
                    messageBox("error", err.responseText);

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
                'created_by': login_emp_idd,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_PayrollMusterForm1Data';
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

        $("#btnadd").bind("click", function () {

            if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "please select company");
                return false;
            }
            $('#loader').show();
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

        $("#btndownload").bind("click", function () {

            $('#loader').show();

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();


            $.ajax({
                type: "GET",
                url: apiurl + "apiPayroll/PayrollMuster1PdfGenerator/" + ddlCompany + "/" + ddlPayrollMonth,
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

        $("#btnupdate").bind("click", function () {
            $('#loader').show();

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();
            var ddlemp = $('#ddlemp').val();
            var txt_fatherorhusbandname = $('#txt_fatherorhusbandname').val();
            var txt_NatureAndDateOfTheOffence = $('#txt_NatureAndDateOfTheOffence').val();
            var txt_WhetherWorkmanShowedCause = $('#txt_WhetherWorkmanShowedCause').val();
            var txt_RateOfWages = $('#txt_RateOfWages').val();
            var txt_DateOfFineImposed = $('#txt_DateOfFineImposed').val();
            var txtDateOnWhichFineRealised = $('#txtDateOnWhichFineRealised').val();
            var txt_AmtofFineImposed = $("#txt_AmtofFineImposed").val();
            var txt_Remarks = $('#txt_Remarks').val();
            var form_i_id = $('#form_i_id').val();

            var myData = {
                "form_i_id": form_i_id,
                'company_id': ddlCompany,
                'emp_id': ddlemp,
                'payroll_month': ddlPayrollMonth,
                'father_or_husband_name': txt_fatherorhusbandname,
                'nature_and_date': txt_NatureAndDateOfTheOffence,
                'whether_workman': txt_WhetherWorkmanShowedCause,
                'rate_of_wages': txt_RateOfWages,
                'date_of_fine_imposed': txt_DateOfFineImposed,
                'amt_of_fine_imposed': txt_AmtofFineImposed,
                'date_realised': txtDateOnWhichFineRealised,
                'remarks': txt_Remarks,
                'created_by': login_emp_idd,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_PayrollMusterForm1Data';
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
            var txt_NatureAndDateOfTheOffence = $('#txt_NatureAndDateOfTheOffence').val();
            var txt_WhetherWorkmanShowedCause = $('#txt_WhetherWorkmanShowedCause').val();
            var txt_RateOfWages = $('#txt_RateOfWages').val();
            var txt_DateOfFineImposed = $('#txt_DateOfFineImposed').val();
            var txtDateOnWhichFineRealised = $('#txtDateOnWhichFineRealised').val();
            var txt_AmtofFineImposed = $('#txt_AmtofFineImposed').val();
            var txt_Remarks = $('#txt_Remarks').val();


            var myData = {
                'company_id': ddlCompany,
                'emp_id': ddlemp,
                'payroll_month': ddlPayrollMonth,
                'father_or_husband_name': txt_fatherorhusbandname,
                'nature_and_date': txt_NatureAndDateOfTheOffence,
                'whether_workman': txt_WhetherWorkmanShowedCause,
                'rate_of_wages': txt_RateOfWages,
                'date_of_fine_imposed': txt_DateOfFineImposed,
                'amt_of_fine_imposed': txt_AmtofFineImposed,
                'date_realised': txtDateOnWhichFineRealised,
                'remarks': txt_Remarks,
                'created_by': login_emp_idd
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Add_PayrollMusterForm1Data';
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


        })

        $("#btnresetalldata").bind("click", function () {
            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();

            if (ddlCompany == "" || ddlCompany == null || ddlCompany == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            if (ddlPayrollMonth == "" || ddlPayrollMonth == null || ddlPayrollMonth == "0") {
                messageBox("error", "Please select month year");
                return false;
            }

            $('#loader').show();
            if (confirm("Are you sure reset all the data for this payroll month?")) {





                var myData = {
                    'company_id': ddlCompany,
                    'payroll_month': ddlPayrollMonth,
                    'created_by': login_emp_idd,
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Reset_PayrollMusterForm1Data';
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


    }, 2000);// end timeout

});



function GetPayrollMonthYear(company_id) {
    $('#loader').show();
    // var company_id = localStorage.getItem("company_id")

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
            messageBox("error", err.responseText);

        }
    });
}




function GetDataById(employee_id, payroll_month) {
    $('#loader').show();
    //var login_comp_id = localStorage.getItem("company_id");

    $('#btnadd').hide();
    $('#btnresetalldata').hide();
    $('#btndownload').hide();
    $('#btnsave').hide();
    $('#btnupdate').show();

    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/GetPayrollDetailsForMusterForm1ByEmployee/" + payroll_month + "/" + employee_id,
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

            // $('#ddlemp').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


            $('#ddldepartment').prop("disabled", "disabled");

            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            BindAllEmp_Company('ddlCompany', login_emp_idd, $("#ddlCompany").val());
            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $("#ddlCompany").val(), res.employee_id);
            BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);

            $("#txt_fatherorhusbandname").val(res.father_or_husband);

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $("#txt_NatureAndDateOfTheOffence").val(res.nature_and_date);
            $("#txt_WhetherWorkmanShowedCause").val(res.whether_workman);
            $("#txt_RateOfWages").val(res.rate_of_wages);
            $("#txt_DateOfFineImposed").val(res.date_of_fine_imposed);
            $("#txt_AmtofFineImposed").val(res.amt_of_fine_imposed);
            $("#txtDateOnWhichFineRealised").val(res.date_realised);
            $("#txt_Remarks").val(res.remarks);
            $("#form_i_id").val(res.form_i_id);

        },
        error: function (err) {
            $('#div_fileds_for_edit').css('display', 'none');
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}



function BindEmployeeDetailss(employee_idd) {
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

            BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);


            $('#ddldepartment').prop("disabled", "disabled");
            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

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
