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
        GetPayrollMonthYear(login_comp_id);


        $('#loader').hide();

        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            //BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            GetPayrollMonthYear($(this).val());
            $('#loader').hide();
        });

        $("#ddlemp").bind("change", function () {
            $('#loader').show();
            BindEmployeeDetailss($(this).val());
            $('#loader').hide();
        });

        $('#ddlPayrollMonth').bind("change", function () {
            if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == null || $("#ddlCompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            $('#loader').show();

            //var company_id = localStorage.getItem("company_id")
            var urll;

            // var company_id = localStorage.getItem("company_id")

            urll = apiurl + "apiPayroll/GetPayrollDetailsForMusterForm3/" + $(this).val() + "/" + $("#ddlCompany").val();

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

                            $('#ddlCompany').prop("disabled", "disabled");

                        }
                    }

                    if (res.flag == 0) {
                        $('#div_tblRegisterOfdeductionInForm3').css('display', 'block');
                        $('#div_tblRegisterOfdeductionInForm32').css('display', 'none');


                        $("#tblRegisterOfdeductionInForm3").DataTable({
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
                                    title: 'Register of Advance Made to Employeed Persons, Form-III(PW)',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Register of Advance Made to Employeed Persons, Form-III(PW)',
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
                                    "data": "advance_date", "name": "advance_date", "title": "Date of advance Made", "autoWidth": true
                                },
                                {
                                    "data": "advance_amount", "name": "advance_amount", "title": "Amount of advance Made", "autoWidth": true
                                },
                                {
                                    "data": "advance_purpose", "name": "advance_purpose", "title": "Purpose(s) for which Advance Made", "autoWidth": true
                                },
                                {
                                    "data": "no_of_installment", "name": "no_of_installment", "title": "No of Installments by which advance to be repaid", "autoWidth": true
                                },
                                {
                                    "data": "postponement_granted", "name": "postponement_granted", "title": "Postponement Granted", "autoWidth": true
                                },
                                {
                                    "data": "date_of_repaid", "name": "date_of_repaid", "title": "Date on which Total Repaid", "autoWidth": true
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
                        $('#div_tblRegisterOfdeductionInForm3').css('display', 'none');
                        $('#div_tblRegisterOfdeductionInForm32').css('display', 'block');

                        $("#tblRegisterOfdeductionInForm32").DataTable({
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
                                    title: 'Register of Advance Made to Employed Persons, Form-III(PW)',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                                    }
                                },
                                {
                                    text: 'Export to PDF',
                                    title: 'Register of Advance Made to Employeed Persons, Form-III(PW)',
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
                                    "data": "advance_date", "name": "advance_date", "title": "Date of advance Made", "autoWidth": true
                                },
                                {
                                    "data": "advance_amount", "name": "advance_amount", "title": "Amount of advance Made", "autoWidth": true
                                },
                                {
                                    "data": "advance_purpose", "name": "advance_purpose", "title": "Purpose(s) for which Advance Made", "autoWidth": true
                                },
                                {
                                    "data": "no_of_installment", "name": "no_of_installment", "title": "No of Installments by which advance to be repaid", "autoWidth": true
                                },
                                {
                                    "data": "postponement_granted", "name": "postponement_granted", "title": "Postponement Granted", "autoWidth": true
                                },
                                {
                                    "data": "date_of_repaid", "name": "date_of_repaid", "title": "Date on which Total Repaid", "autoWidth": true
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
                    alert(err.responseText);
                }
            });



        });

        $("#btnFreeze").bind("click", function () {
            $('#loader').show();

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();

            if (ddlCompany == null || ddlCompany == "0" || ddlCompany == "") {
                messageBox("error", "Please select company");
                return false;
            }
            if (ddlPayrollMonth == "" || ddlPayrollMonth == null) {
                messageBox("error", "Please select payroll month");
                return false;
            }

            var myData = {
                'company_id': ddlCompany,
                'payroll_month': ddlPayrollMonth,
                'created_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_PayrollMusterForm3Data';
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
            var txt_AdvanceAmt = $('#txt_AdvanceAmt').val();
            var txt_AdvanceDate = $('#txt_AdvanceDate').val();
            var txt_AdvancePurpose = $('#txt_AdvancePurpose').val();
            var txt_noofinstallment = $('#txt_noofinstallment').val();
            var txt_PostponementGranted = $('#txt_PostponementGranted').val();
            var txt_DateofTotalRepaid = $('#txt_DateofTotalRepaid').val();
            var txt_Remarks = $('#txt_Remarks').val();
            var form_iii_id = $('#form_iii_id').val();



            var myData = {
                form_iii_id: form_iii_id,
                company_id: ddlCompany,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                advance_date: txt_AdvanceDate,
                advance_amount: txt_AdvanceAmt,
                purpose: txt_AdvancePurpose,
                no_of_installment: txt_noofinstallment,
                postponse_granted: txt_PostponementGranted,
                date_of_repaid: txt_DateofTotalRepaid,
                remarks: txt_Remarks,
                modified_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_PayrollMusterForm3Data';
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
            var txt_AdvanceAmt = $('#txt_AdvanceAmt').val();
            var txt_AdvanceDate = $('#txt_AdvanceDate').val();
            var txt_AdvancePurpose = $('#txt_AdvancePurpose').val();
            var txt_noofinstallment = $('#txt_noofinstallment').val();
            var txt_PostponementGranted = $('#txt_PostponementGranted').val();
            var txt_DateofTotalRepaid = $('#txt_DateofTotalRepaid').val();
            var txt_Remarks = $('#txt_Remarks').val();


            var myData = {
                company_id: ddlCompany,
                emp_id: ddlemp,
                payroll_month: ddlPayrollMonth,
                father_or_husband_name: txt_fatherorhusbandname,
                advance_date: txt_AdvanceDate,
                advance_amount: txt_AdvanceAmt,
                purpose: txt_AdvancePurpose,
                no_of_installment: txt_noofinstallment,
                postponse_granted: txt_PostponementGranted,
                date_of_repaid: txt_DateofTotalRepaid,
                remarks: txt_Remarks,
                created_by: login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Add_PayrollMusterForm3Data';
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
                    'created_by': login_emp_id,
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Reset_PayrollMusterForm3Data';
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
                        _GUID_New();
                        messageBox("error", err.responseText);
                        $('#loader').hide();
                    }
                });

            }
            return false;
        });

        $("#btndownload").bind("click", function () {
            $('#loader').show();

            var ddlCompany = $('#ddlCompany').val();
            var ddlPayrollMonth = $('#ddlPayrollMonth').val();


            $.ajax({
                type: "GET",
                url: apiurl + "apiPayroll/PayrollMuster3PdfGenerator/" + ddlCompany + "/" + ddlPayrollMonth,
                data: {},
                contentType: "application/json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    if (statuscode == "0") {
                        window.open(Msg, '_blank');
                        console.log(Msg);
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

        //GetPayrollMonthYear();

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
    if ($("#ddlCompany").val() == null || $("#ddlCompany").val() == "0" || $("#ddlCompany").val() == "") {
        messageBox("error", "Please select company");
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
        url: apiurl + "apiPayroll/GetPayrollDetailsForMusterForm3ByEmployee/" + payroll_month + "/" + employee_id,
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

            //  $('#ddlemp').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $('#ddldepartment').prop("disabled", "disabled");

            $('#ddldepartment').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");



            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $("#ddlCompany").val(), res.employee_id);
            BindDepartmentListForddl('ddldepartment', $("#ddlCompany").val(), res.department_id);


            $("#txt_fatherorhusbandname").val(res.father_or_husband);

            $('#txt_fatherorhusbandname').prop("disabled", "disabled");

            $('#txt_fatherorhusbandname').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");

            $("#txt_AdvanceDate").val(res.advance_date);
            $("#txt_AdvanceAmt").val(res.advance_amount);
            $("#txt_AdvancePurpose").val(res.purpose);
            $("#txt_noofinstallment").val(res.no_of_installment);
            $("#txt_PostponementGranted").val(res.postponse_granted);
            $("#txt_DateofTotalRepaid").val(res.date_of_repaid);
            $("#txt_Remarks").val(res.remarks);
            $("#form_iii_id").val(res.form_iii_id);

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
