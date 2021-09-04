
var login_comp_id;
var emp_role_id;
var login_empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        // GetReport();
        //$("#txtfromdate").datepicker({
        //    dateFormat: 'mm/dd/yy',
        //    //minDate: 0,
        //    onSelect: function (fromselected, evnt) {
        //        $("#txttodate").datepicker('setDate', null);
        //        $("#txttodate").datepicker({
        //            dateFormat: 'mm/dd/yy',
        //            minDate: fromselected,
        //            onSelect: function (toselected, evnt) {

        //                if (Date.parse(fromselected) > Date.parse(toselected)) {
        //                    messageBox('error', 'To Date must be greater than from date !!');
        //                    //$('#txtfromdate').val('');
        //                    $('#txttodate').val('');
        //                }
        //            }
        //        });
        //    }

        //});
        //$("#txteffectivedt").datepicker({
        //    dateFormat: 'mm/dd/yy',

        //    //minDate: 0,
        //});

        $(document).ready(function () {
            $('#txteffectivedt').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'MM yy',

                onClose: function () {
                    var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                    var _monthh = parseInt(iMonth) + 1;
                    $("#hdneffectivedt").val(iYear + "-" + _monthh + "-01");
                    if ($("#ddlemp").val() != null && $("#ddlemp").val() != "0") {
                        BindEmployeeDetailsByEmp($("#ddlemp").val());
                        GetComponents($("#ddlemp").val());
                        $('#div_emp_salay').show();
                    }
                },

                beforeShow: function () {
                    if ((selDate = $(this).val()).length > 0) {
                        iYear = selDate.substring(selDate.length - 4, selDate.length);
                        iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                        $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                        $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                    }

                }
            });


            $('#txteffective_dt').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'MM yy',

                onClose: function () {
                    var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
                },

                beforeShow: function () {
                    if ((selDate = $(this).val()).length > 0) {
                        iYear = selDate.substring(selDate.length - 4, selDate.length);
                        iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                        $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                        $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                    }

                }
            });

        });



        BindAllEmp_Company('ddlCompany', login_empid, login_comp_id);
        BindSalaryGroupEmployee('ddlemp', login_comp_id, 0);



        // Effectivefromdate();

        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();

            $("#txteffectivedt").val('');
            $("#txtjoiningdt").val('');
            $("#txtdesignation").val('');
            $("#txtexperience").val('');
            $("#txtdepartment").val('');
            $("#txtgrade").val('');
            $("#ddlsg").val('');
            $("#txtpayoutmonth").val('');

            if ($.fn.DataTable.isDataTable('#tblsalaryrevision')) {
                $('#tblsalaryrevision').DataTable().clear().draw();
            }

            //BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);
            //BindSalaryGroupEmployee('ddlemp', $(this).val(), 0);
            BindSalaryGroupEmployee('ddlemp', $(this).val(), 0);

            $('#loader').hide();
        });


        $('#btnback').show();
        $('#btnaddnewrevision').show();

        $("#SalaryInput_File").bind("change", function () {
            EL("SalaryInput_File").addEventListener("change", readFile, false);
        });

        $('#ddlemp').bind("change", function () {
            $("#txtjoiningdt").val('');
            $("#txtdesignation").val('');
            $("#txtdepartment").val('');
            $("#txtgrade").val('');
            $("#ddlsg").empty();
            if ($("#txteffectivedt").val() != "" && $("#txteffectivedt").val() != null) {
                BindEmployeeDetailsByEmp($("#ddlemp").val());
                GetComponents($("#ddlemp").val());
                $('#div_emp_salay').show();
            }
            //$("#txteffectivedt").val('');

            //var emp_id = $(this).val();
            //if (emp_id == '' || emp_id == null) {
            //    emp_id = 0;
            //}

            //if ($("#txteffectivedt").val() == "") {
            //    messageBox("error", "Please Select Effective From Date");
            //    $('#loader').hide();
            //    return false;
            //}

            //BindEmployeeDetailsByEmp(emp_id);
            //GetComponents(emp_id);
            //$('#div_emp_salay').show();
        });


        $('#btnsearch').bind("click", function () {
            $('#loader').show();

            var ddlemp = $("#ddlemp").val();
            var txtFromDate = $("#txtfromdate").val();
            var txtToDate = $("#txttodate").val();


            var errormsg = '';
            var iserror = false;
            if (ddlemp == null || ddlemp == '0') {
                errormsg = "Please Select Employee...! ";
                iserror = true;
            }
            if (txtFromDate == null || txtFromDate == '') {
                errormsg = "Please Select From Date...! ";
                iserror = true;
            }
            if (txtToDate == null || txtToDate == '') {
                errormsg = "Please Select To Date...! ";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //messageBox("info", "eror give");
                return false;
            }

            var myData = {
                'emp_id': ddlemp,
                'FromDate': txtFromDate,
                'ToDate': txtToDate,
            };

            var urls = apiurl + "apiPayroll/GetSalaryReport"

            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                type: "POST",
                url: urls,
                data: Obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                headers: headerss,
                success: function (res) {

                    // debugger;
                    // alert(JSON.stringify(res));
                    //var report = res.list;
                    //var column = res.column;

                    //$('#loader').hide();
                    //_GUID_New();
                    ////start by supriya

                    //if (res.statusCode != undefined) {
                    //    messageBox("error", res.message);
                    //    return false;
                    //}

                    //var ColumnData = [];
                    //for (i = 0; i < column.length; i++) {
                    //    ColumnData.push({
                    //        "data": column[i].title,
                    //        "name": column[i].title, "autoWidth": true,
                    //        "title": column[i].title
                    //    });
                    //}


                    //var rowdata = []
                    //for (j = 0; j < report.length; j++) {

                    //    rowdata.push(report[j]);
                    //}

                    ////end by supriya


                    //if (report.length > 0) {

                    //$('#tblsalaryrevisionrpt tbody').empty();
                    console.log(res);
                    $('#tblsalaryrevisionrpt').dataTable({
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": res,
                        //"data": rowdata,
                        //"columns": ColumnData,
                        "columnDefs":
                            [
                                {
                                    targets: [1],
                                    render: function (data, type, row) {
                                        var date = new Date(data);
                                        return GetDateFormatddMMyyyy(date);
                                    }
                                }
                            ],

                        "columns": [
                            { "data": null, "title": "S.No.", "autoWidth": true },
                            { "data": "applicable_from_dt", "name": "applicable_from_dt", "title": "Applicable From", "autoWidth": true },
                            //{ "data": "emp_id", "name": "emp_id", "title": "Employee Id", "autoWidth": true },
                            //{ "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                            { "data": "gross_salary", "name": "GROSS SALARY", "title": "GROSS SALARY", "autoWidth": true },
                            { "data": "ctc", "name": "CTC", "title": "CTC", "autoWidth": true },
                            { "data": "basic", "name": "BASIC", "title": "BASIC", "autoWidth": true },
                            { "data": "hra", "name": "HRA", "title": "HRA", "autoWidth": true },
                            { "data": "conveyance", "name": "CONVEYANCE", "title": "CONVEYANCE", "autoWidth": true },
                            { "data": "special_allowance", "name": "SPECIAL ALLOWANCE", "title": "SPECIAL ALLOWANCE", "autoWidth": true },
                            { "data": "da", "name": "DA", "title": "DA", "autoWidth": true },
                            { "data": "net", "name": "NET", "title": "NET", "autoWidth": true },
                            { "data": "esic", "name": "ESIC", "title": "ESIC", "autoWidth": true },
                            { "data": "pf", "name": "PF", "title": "PF", "autoWidth": true },
                            { "data": "vpf", "name": "VPF", "title": "VPF", "autoWidth": true },
                            { "data": "total_deduction", "name": "TOTAL DEDUCTION", "title": "TOTAL DEDUCTION", "autoWidth": true },
                            { "data": "er_pf", "name": "ER PF", "title": "ER PF", "autoWidth": true },
                            { "data": "maker_remark", "name": "maker_remark", "title": "Maker Remark", "autoWidth": true },
                            { "data": "checker_remark", "name": "checker_remark", "title": "Checker Remark", "autoWidth": true },
                            { "data": "_status", "name": "_status", "title": "Status", "autoWidth": true },

                        ],

                        "lengthMenu": [[20, 50, -1], [20, 50, "All"]],
                        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                            return nRow;
                        }
                    });
                    //}
                    //else {
                    //    alert('Record not found !!!')
                    //    if ($.fn.DataTable.isDataTable('#tblsalaryrevisionrpt')) {
                    //        $('#tblsalaryrevisionrpt').DataTable().clear().draw();
                    //    }

                    //    //var table = $('#tblsalaryrevisionrpt').DataTable();
                    //    // table.clear().draw();
                    //    // table.clear().destroy();
                    //}

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

            $('#loader').hide();
        });

        $('#btnsave').bind("click", function () {

            $('#loader').show();
            var salrevvalue = $("#hdn_salaryrevisionvalue").val();

            //Loop through the Table rows and build a JSON array.
            var Salary = new Array()
            var table = $('#tblsalaryrevision').DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var sal = {};

                sal.component_id = table.cell(rowIdx, 0).data();
                sal.componentvalue = table.cell(rowIdx, 3).nodes().to$().find('input').val();

                Salary.push(sal);
            });

            var ddlemp = $("#ddlemp").val();
            var ddlsg = $("#ddlsg").val();
            //var effective_date = $("#txteffectivedt").val();
            //var ddlmonth = $("#txtpayoutmonth").val();

            var effective_date = $("#hdneffectivedt").val();
            var maker_remark = $("#txtremarks").val();

            var errormsg = '';
            var iserror = false;

            if (ddlemp == '' || ddlemp == '0') {
                errormsg = errormsg + 'Please select employee !! <br />';
                iserror = true;
            }
            //if (ddlmonth == '' || ddlmonth == null) {
            //    errormsg = errormsg + 'Please select Payout Month !! <br />';
            //    iserror = true;
            //}
            //if (Salary.length <= 0) {
            //    errormsg = errormsg + 'Please revise Salary !! <br />';
            //    iserror = true;
            //}

            if (effective_date == "") {
                errormsg = errormsg + "Please Select Effective Date";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }
            if (!confirm("Do you want to process this?")) {
                $('#loader').hide();
                return false;
            }



            var myData = {

                'SalaryValue': Salary,
                'emp_id': ddlemp,
                'applicable_date': effective_date, //ddlmonth,//
                'created_by': login_empid,
                'is_active': 2,
                'salaryrevision': salrevvalue,
                'maker_remark': maker_remark
            };

            $('#loader').show();
            var apiurl = localStorage.getItem("ApiUrl") + "apiPayroll/Save_SalaryRevision";
            var obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    _GUID_New();
                    if (statuscode == "0") {
                        $('#loader').hide();
                        alert(Msg);
                        $("#hdn_salaryrevisionvalue").val('');
                        window.location.href = '/payroll/SalaryRevision';
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

        $('#btndelete').bind("click", function () {
            $('#loader').show();

            //Loop through the Table rows and build a JSON array.
            var Salary = new Array()

            var table = $('#tblsalaryrevision').DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var sal = {};

                sal.component_id = table.cell(rowIdx, 0).data();
                sal.componentvalue = table.cell(rowIdx, 3).nodes().to$().find('input').val();

                Salary.push(sal);
            });

            var ddlemp = $("#ddlemp").val();
            var ddlmonth = $("#txtpayoutmonth").val();

            var errormsg = '';
            var iserror = false;

            if (ddlemp == '' || ddlemp == '0') {
                errormsg = errormsg + 'Please select employee !! <br />';
                iserror = true;
            }
            if (ddlmonth == '' || ddlmonth == null) {
                errormsg = errormsg + 'Please select Payout Month !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            if (confirm("Do you want to delete this?")) {


                var myData = {
                    'SalaryValue': Salary,
                    'emp_id': ddlemp,
                    'applicable_date': ddlmonth,
                    'modified_by': login_empid,
                };

                $('#loader').show();
                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Delete_SalaryRevision';
                var obj = JSON.stringify(myData);
                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({
                    type: "DELETE",
                    url: apiurl,
                    data: obj,
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    headers: headerss,
                    success: function (res) {

                        data = res;
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {

                            messageBox("success", Msg);
                        }
                        else {
                            messageBox("error", Msg);
                        }

                    },
                    error: function (err) {
                        _GUID_New();
                        messageBox("error", err.responseText)
                    }
                });
            }
            else {
                $('#loader').hide();
                return false;
            }
        });

        $('#btncalculate').bind("click", function () {

            var employee_salary = $("#txtempelasry").val();


            calculate(employee_salary);

        });

        $("#txteffectivedt").bind("change", function () {
            if ($("#ddlemp").val() == "0" || $("#ddlemp").val() == null || $("#ddlemp").val() == "") {
                messageBox("error", "Please Select Employee");
                return false;
            }

            BindEmployeeDetailsByEmp($("#ddlemp").val());
            GetComponents($("#ddlemp").val());
            $('#div_emp_salay').show();
        });

        $("#btnuploaddtl").bind("click", function () {
            var companyid = $("#ddlCompany").val();
            var monthyear = $("#txteffective_dt").val();


            var uploadfilee = $("#SalaryInput_File").val()
            var iserror = false;
            var errormsg = "";

            if (companyid == "0" || companyid == "" || companyid == null) {
                iserror = true;
                errormsg = errormsg + "Please Select Company!</br>";
            }
            if (monthyear == "" || monthyear == null || monthyear == "0") {
                iserror = true;
                errormsg = errormsg + "Please Select Processed MonthYear!</br>";
            }
            if (uploadfilee == "" || uploadfilee == null) {
                iserror = true;
                errormsg = errormsg + "Please Upload File!</br>";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var txtmnyr = new Date($('#txteffective_dt').val());
            var yr = txtmnyr.getFullYear();
            var mnth = txtmnyr.getMonth() + 1;
            if (mnth <= 9) {
                monthyear = yr.toString() + "0" + mnth.toString();
            }
            else {
                monthyear = yr.toString() + mnth.toString();
            }

            var mydata = {
                company_id: companyid,
                monthyear: monthyear,
            };

            var obj = JSON.stringify(mydata);
            var files = document.getElementById("SalaryInput_File").files;
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();;

            var formdata = new FormData();
            formdata.append('AllData', obj);

            for (var i = 0; i < files.length; i++) {
                formdata.append('Files', files[i]);
            }
            $('#loader').show();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiPayroll/Upload_SalaryRevision",
                type: "POST",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                dataType: "json",
                data: formdata,
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    _GUID_New();


                    var statuscode = data.objresponse.statusCode;
                    var msg = data.objresponse.message;

                    $('#loader').hide();

                    $("#SalaryInput_File").val('');
                    var duplist = data.dataaa.duplicatesalaryinputlst;
                    var missingdtllist = data.dataaa.missingsalaryinputlst;
                    var deetailmsg = data.dataaa.dtlMessage;
                    var employeenotexist = data.dataaa.employeenotexist;

                    if (deetailmsg != '') {
                        messageBox("success", msg);
                    }
                    if (employeenotexist != null && employeenotexist.length > 0) {
                        $('#exampleModalLabel').text('Employee code does not exist in selected company');
                        $("#mySalaryModal").show();
                        var modal = document.getElementById("mySalaryModal");
                        modal.style.display = "block";

                        $('#mySalaryModal').modal({
                            modal: 'true',
                            title: 'Employee code does not exist',
                            backdrop: 'false'
                        });

                        //$.extend($.fn.dataTable.defaults, {
                        //    sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        //});

                        $("#tbl_excel_dtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once              
                            "scrollX": 800,
                            dom: 'lBfrtip',
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Employee code does not exist',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1]
                                    }
                                },
                            ],
                            "aaData": employeenotexist,
                            "columnDefs":
                                [

                                ],

                            "columns": [
                                { "data": null, "title": "S.No.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },

                            ],
                            "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },

                        });


                        //$("#tbl_excel_dtl").DataTable({
                        //    "processing": true, // for show progress bar
                        //    "serverSide": false, // for process server side
                        //    "bDestroy": true,
                        //    "filter": true, // this is for disable filter (search box)
                        //    "orderMulti": false, // for disable multiple column at once
                        //    "scrollX": 200,
                        //    "aaData": employeenotexist,
                        //    dom: 'lBfrtip',
                        //    buttons: [
                        //        {
                        //            text: 'Export to Excel',
                        //            title: 'Employee code does not exist',
                        //            extend: 'excelHtml5',
                        //            exportOptions: {
                        //                columns: [1,2]
                        //            }
                        //        },
                        //    ],
                        //    "columnDefs":
                        //        [],
                        //    "columns": [
                        //        { "data": null, "title": "SNo.", "autoWidth": true },
                        //        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }                        
                        //    ],
                        //    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                        //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        //        return nRow;
                        //    },
                        //});


                        //   messageBox("error", deetailmsg);
                        $('#loader').hide();
                        modal.style.opacity = "inherit";
                        modal.style.top = "5%";
                    }
                    else if (duplist != null && duplist.length > 0) {
                        $('#exampleModalLabel').text('Duplicate Detail in Excel');
                        $("#mySalaryModal").show();
                        var modal = document.getElementById("mySalaryModal");
                        modal.style.display = "block";

                        $('#mySalaryModal').modal({
                            modal: 'true',
                            title: 'Duplicate Detail in Excel',
                            backdrop: 'false'
                        });

                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_excel_dtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": duplist,
                            dom: 'lBfrtip',
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Employee Data',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3]
                                    }
                                },
                            ],
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "property_details", "name": "property_details", "title": "Salary Component", "autoWidth": true },
                                { "data": "component_value", "name": "component_value", "title": "Amount", "autoWidth": true },
                            ],
                            "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                        });


                        //messageBox("error", deetailmsg);
                        $('#loader').hide();
                        modal.style.opacity = "inherit";
                        modal.style.top = "5%";
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {
                        $('#exampleModalLabel').text('Missing / Already exist in DB Details');
                        $("#mySalaryModal").show();
                        var modal = document.getElementById("mySalaryModal");
                        modal.style.display = "block";

                        $('#mySalaryModal').modal({
                            modal: 'true',
                            title: 'Missing / Already exist in DB Details',
                            backdrop: 'false'
                        });
                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_excel_dtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": missingdtllist,
                            dom: 'lBfrtip',
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Employee Data',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [1, 2, 3]
                                    }
                                },
                            ],
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "property_details", "name": "property_details", "title": "Salary Component", "autoWidth": true },
                                { "data": "component_value", "name": "component_value", "title": "Amount", "autoWidth": true },
                            ],
                            "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },

                        });


                        // messageBox("error", deetailmsg);
                        //   $('#loader').hide();
                        modal.style.opacity = "inherit";
                        modal.style.top = "5%";
                    }
                    else {

                        var statuscode = data.objresponse.statusCode;
                        var msg = data.objresponse.message;
                        if (statuscode == "0") {

                            BindAllEmp_Company('ddlCompany', login_empid, login_comp_id);
                            $("#txteffective_dt").val('');
                            $("#SalaryInput_File").val('');
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            messageBox("error", msg);
                            return false;
                        }
                        $('#loader').hide();
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);
                    $('#loader').hide();
                }
            });
        });


        $("#btn_modal_close").bind("click", function () {
            location.reload();
        });

    }, 2000);// end timeout

});


function BindEmployeeDetailsByEmp(employee_id) {

    $('#loader').show();
    $("#txtjoiningdt").val('');
    $("#txtdesignation").val('');
    $("#txtdepartment").val('');
    $("#txtgrade").val('');
    $("#ddlsg").empty();
    //24-10-2019 start

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_SalaryRevisionEmpData/' + employee_id + "/" + $("#hdneffectivedt").val(), //$("#txteffectivedt").val(),  //24-10-2019 end
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            //alert(JSON.stringify(data));
            $('#loader').hide();

            if (data.statusCode != undefined) {
                messageBox("error", data.message);
                return false;
            }

            if (data != null && data != "undefined") {
                var joinintdate = data.combine_dtl[0].date_of_joining;
                var department_name = data.combine_dtl[0].dept_name;

                $("#txtjoiningdt").val(GetDateFormatddMMyyyy(new Date(joinintdate)));
                //$("#txtjoiningdt").val(GetOnlyDate(data.joining_date));
                $("#txtdesignation").val(data.des_name);
                //$("#txtdepartment").val(data.dept_name);
                $("#txtdepartment").val(department_name);
                $("#txtgrade").val(data.grade_name);
                //$("#txtsg").val(data[0].group_name);
                $("#ddlsg option").remove();
                $('#ddlsg').append($("<option></option>").val(data.salary_group_id).html(data.group_name));
            }


            //var date_ = $("#txtjoiningdt").val();

            //var calculateExp = function (date_) {
            //    var now = new Date();  
            //    var past = new Date(date_);
            //    var nowday = now.getDay();
            //    var nowmonth = now.getMonth();
            //    var nowYear = now.getFullYear();
            //    var pastday = past.getDay();
            //    var pastmonth = past.getMonth();
            //    var pastYear = past.getFullYear();
            //    //var a = parseInt(nowYear + "-" + nowmonth + "-" + nowday) - date_;
            //    var exp = nowYear - pastYear;
            //    return exp;               
            //};
            //$("#txtexperience").val(calculateExp);


        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

}
//function BindEmpList(ControlId, SelectedVal) {
//    $('#loader').show();
//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiEmployee/GetAllEmployee",
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.employee_first_name));
//            })

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (err) {
//            $('#loader').hide();
//            messageBox("error",err.responseText);
//        }
//    });
//}

function GetComponents(emp_id) {
    $('#loader').show();
    var urls = apiurl + 'apiPayroll/GetComponentsByempId/' + emp_id + "/" + $("#hdneffectivedt").val();//$("#txteffectivedt").val();

    $("#tblsalaryrevision").DataTable().destroy();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (res) {

            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            $("#tblsalaryrevision").DataTable({

                "processing": true, // for show progress bar
                // "serverSide": false, // for process server side
                //"bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                // "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,

                "columns": [
                    { "data": "component_id", "name": "component_id", "autoWidth": true, "visible": false },
                    { "data": "property_details", "name": "property_details", "autoWidth": true },
                    { "data": "applicable_value", "name": "applicable_value", "autoWidth": true },
                    {
                        render: function (data, type, row) {
                            // debugger;
                            //if (row.property_details == 'ANNUAL CTC')
                            //    return '<input style="width:100px" class="form-control" id="value3" onchange="calculate(this.value ,' + row.component_id +')" maxlength="8" onkeypress="return isNumberKey(event)" name="value3" type="text" value = 0 >';
                            //else
                            return '<input style="width:100px" class="form-control" readonly id="value3" maxlength="8" name="value3" type="text" value = ' + row.requested_value + ' >';
                        }
                    },
                    //{
                    //    render: function (data, type, row) {
                    //        return '<input style="width:100px; display:none" class="form-control" readonly id="value3" maxlength="8" name="value3" type="text" value = 0 >';
                    //    }
                    //},
                ],
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]

            });

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

function getmonthyear(date) {
    var now = new Date(date);
    var y = now.getFullYear();
    var m = now.getMonth() + 1;
    var d = now.getDate();
    var mm = m < 10 ? '0' + m : m;
    //var dd = d < 10 ? '0' + d : d;
    return '' + y + mm;
}

function calculate(val) {
    $('#loader').show();
    var ddlemp = $("#ddlemp").val();
    var ddlsg = $("#ddlsg").val();



    var ddlmonth = $("#txteffectivedt").val(); //$("#hdneffectivedt").val()//;

    $("#hdn_salaryrevisionvalue").val(val);

    var errormsg = '';
    var iserror = false;
    if (ddlmonth == null || ddlmonth == '') {
        errormsg = "Please Select Effective from date !!! ";
        iserror = true;
    }

    if (ddlsg == '' || ddlsg == 0 || ddlsg == null) {
        errormsg = "Salary group not define for Employee...!";
        iserror = true;
    }
    if (val == null || val == '' || val == '0' || val == '00' || val == '000' || val == '0000' || val == '00000') {
        errormsg = "Please enter salary !!! ";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        return false;
    }
    // debugger;
    var myData = {
        'emp_id': ddlemp,
        'monthyear': getmonthyear(ddlmonth),
        'sgid': ddlsg,
        'componentvalue': val,
        'company_id': $('#ddlCompany').val()

    };

    var urls = apiurl + "apiPayroll/Salaryrevision_Calculation"
    //alert(urls);
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        type: "POST",
        url: urls,
        data: Obj,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: headerss,
        success: function (res) {
            debugger;
            // debugger;
            _GUID_New();
            $('#tblsalaryrevision').dataTable().fnClearTable();
            $('#tblsalaryrevision').dataTable().fnDestroy();

            $('#loader').hide();

            $("#tblsalaryrevision").DataTable({


                "processing": true, // for show progress bar
                // "serverSide": false, // for process server side
                //"bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                // "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,

                "columns": [
                    { "data": "component_id", "name": "component_id", "autoWidth": true, "visible": false },
                    { "data": "property_details", "name": "property_details", "autoWidth": true },
                    { "data": "applicable_value", "name": "applicable_value", "autoWidth": true },
                    {
                        render: function (data, type, row) {
                            // debugger;
                            //if (row.property_details == 'ANNUAL CTC')
                            //    return '<input style="width:100px" class="form-control" id="value3" onchange="calculate(this.value ,' + Math.round(row.component_id,2) + ')" maxlength="8" onkeypress="return isNumberKey(event)" name="value3" type="text" value = ' + row.compValue +' >';
                            //else

                            return '<input style="width:100px" class="form-control" readonly id="value3" maxlength="8" name="value3" type="text" value = ' + Math.round(row.compValue, 2) + ' >';
                        }
                    },
                    //{
                    //    render: function (data, type, row) {
                    //        return '<input style="width:100px;display:none" class="form-control" readonly id="value3" maxlength="8" name="value3" type="text" value = "0" >';
                    //    }
                    //},
                ],
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]

            });

        },
        error: function (request, status, error) {
            debugger;
            _GUID_New();
            if (error.message != null) {
                messageBox("error", error.message);
                $('#loader').hide();
            }
            else {
                $('#loader').hide();
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

        }
    });

}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 8 || charCode == 37) {
        return true;
    } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
        return false;
    } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        return true;
    }
}

//bind company list

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
//            messageBox("error",err.responseText);
//        }
//    });
//}

//bind company list

//function BindEmployeeList(ControlId, SelectedVal) {
//    $('#loader').show();
//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiMasters/Get_EmployeeHeadList",
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;


//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
//            })

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (err) {
//            $('#loader').hide();
//            messageBox("error",err.responseText);
//        }
//    });
//}

function GetReport() {
    $('#loader').show();
    var urls = apiurl + 'apiPayroll/GetSalaryReport'

    $.ajax({
        type: "GET",
        url: urls,
        data: null,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            // debugger;
            // alert(JSON.stringify(res));
            var report = res.list;
            var column = res.column;

            $('#loader').hide();

            if (report != null && report != '') {
                $('#tblsalaryrevisionrpt').dataTable({
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    //"scrollX": 200,
                    "data": report,
                    "columns": column,
                    "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
                });
            }
            else {
                alert('uyttiutiut')
                var table = $('#tblsalaryrevisionrpt').DataTable();
                table.clear().draw();
            }

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}



//function Effectivefromdate() {
//    var dtToday = new Date();

//    var month = dtToday.getMonth() + 1;
//    var day = dtToday.getDate();
//    var year = dtToday.getFullYear();
//    if (month < 10)
//        month = '0' + month.toString();
//    if (day < 10)
//        day = '0' + day.toString();

//    var maxDate = year + '-' + month + '-' + day;
//    //alert(maxDate);
//    $('#txteffectivedt').attr('min', maxDate);
//}


function BindSalaryGroupEmployee(ControlId, company_id, SelectedVal) {
    $('#loader').show();
    // debugger;
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_EmpSalaryGroup/' + company_id;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            //console.log(JSON.stringify(res));
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {

                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}



//function BindSalaryGroupEmployee(ControlId, company_id, SelectedVal) {

//    ControlId = '#' + ControlId;


//    $.ajax({
//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_EmpSalaryGroup/" + company_id,
//        url: urls,
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.emp_id).html(value.emp_name_code));
//            })
//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (error) {
//            $('#loader').hide();
//            messageBox("error", error.responseText);
//        }
//    });
//}



function DownloadSalaryUploadFormat() {
    window.open("/UploadFormat/SalaryRevision.xlsx");
}






function readFile() {

    if (this.files.length > 0) {
        messageBox("error", "Only one excel file can be upload");
        return false;
    }

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#SalaryInput_File").val("");
            alert("Upload only Excel file and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#SalaryInput_File").val("");
                alert("Upload only Excel file and on define format. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            EL("HFb64uploaddtl").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

