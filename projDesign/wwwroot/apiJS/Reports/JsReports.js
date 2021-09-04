$('#loader').show();
var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        var pathname = window.location.pathname;
        if (pathname == '/Report/Present') {
            $("#li_Present").addClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/Absent') {
            $("#li_Absent").addClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/Attendance') {
            $("#li_Attendance").addClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/Mispunch') {
            $("#li_Mispunch").addClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/ManualPunch') {
            $("#li_ManualPunch").addClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/LatePunchIn') {
            $("#li_LatePunchIn").addClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/EarlyPunchOut') {
            $("#li_EarlyPunchOut").addClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
            $("#li_LocationBy").removeClass("btn-primary");
        }
        else if (pathname == '/Report/LocationBy') {
            $("#li_LocationBy").addClass("btn-primary");
            $("#li_EarlyPunchOut").removeClass("btn-primary");
            $("#li_LatePunchIn").removeClass("btn-primary");
            $("#li_ManualPunch").removeClass("btn-primary");
            $("#li_Mispunch").removeClass("btn-primary");
            $("#li_Attendance").removeClass("btn-primary");
            $("#li_Absent").removeClass("btn-primary");
            $("#li_Present").removeClass("btn-primary");
        }


        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp_id, login_company_id);
        BindLocationListForddl('ddllocation', login_company_id, 0);
        BindDepartmentListForddl('ddldepartment', login_company_id, 0);
        BindAllShiftListForddlAll('ddlShift', login_company_id, 0);
        BindLocationForReport('ddlState', login_company_id, 0);


        BindAllEmp_Company('ddlCompanyM', login_emp_id, login_company_id);
        BindLocationListForddl('ddllocationM', login_company_id, 0);
        BindDepartmentListForddl('ddldepartmentM', login_company_id, 0);
        BindAllShiftListForddlAll('ddlShiftM', login_company_id, 0);


        // GetData(26);
        $('#ddlCompany').bind("change", function () {
            BindLocationListForddl('ddllocation', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
            BindAllShiftListForddlAll('ddlShift', $(this).val(), 0);
            BindLocationForReport('ddlState', $(this).val(), 0);


            //  GetData($(this).val());
        });




        // GetData(26);
        $('#ddlCompanyM').bind("change", function () {
            BindLocationListForddl('ddllocationM', $(this).val(), 0);
            BindDepartmentListForddl('ddldepartmentM', $(this).val(), 0);
            BindAllShiftListForddlAll('ddlShiftM', $(this).val(), 0);
            //  GetData($(this).val());


        });

        $("#RadiosAllEmployeeFilter").prop("checked", true);

        $(document).ready(function () {
            $('#txtMonthYear').datepicker({
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


        $('#loader').hide();

        $('#btnViewDailyReoprt').bind("click", function () {
            // debugger;
            $('#loader').hide();
            cleardata();
            var ddlCompany = $("#ddlCompany").val();

            var ddlState = $("#ddlState").val();
            var ddllocation = $("#ddllocation").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddlShift = $("#ddlShift").val();
            var txtFromDate = $("#txtFromDate").val();
            var txtToDate = $("#txtToDate").val();

            var errormsg = '';
            var iserror = false;

            if (ddlCompany == "" || ddlCompany == null || ddlCompany == "0") {
                errormsg = "Please Select Company...! ";
                iserror = true;
            }
            if (txtToDate == null || txtToDate == '') {
                errormsg = "Please Select To Date...! ";
                iserror = true;
            }
            if (txtFromDate == null || txtFromDate == '') {
                errormsg = "Please Select From Date...! ";
                iserror = true;
            }

            if (new Date(txtFromDate) > new Date(txtToDate)) {
                errormsg = "To Date must be greater than from date";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }


            var myData = {
                "company_id": ddlCompany,
                "location_id": ddllocation,
                "department_id": ddldepartment,
                "FromDate": txtFromDate,
                "ToDate": txtToDate,
                "state_id": ddlState
            };

            var apiurl = "";

            if ($("#rptPresentReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/PresentReport';
            }
            else if ($("#rptAbsentReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/AbsentReport';
            }
            else if ($("#rptLeaveReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/LeaveReport';
            }
            else if ($("#rptPerformanceIndividual").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/PerformanceIndividual';
            }
            else if ($("#rptMispunch").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/MispunchReport';
            }
            else if ($("#rptManualPunchReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/ManualPunchReport';
            }
            else if ($("#rptLatePunchIn").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/LatePunchIn';
            }
            else if ($("#rptEarlyPunchOut").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/EarlyPunchOut';
            }
            else if ($("#rptLocationByReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/LocationByReport';
            }
            else {
                messageBox("error", "Please Select Report Type...!");
                return false;
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            if ($("#rptPresentReport").is(":checked")) {
                $("#tblPresentReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once   
                    dom: 'lBfrtip',
                    "deferLoading": 57,
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Present Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Present Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Present Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:' + full.colorCode + '">' + full.attendanceStatus + '</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptAbsentReport").is(":checked")) {
                $("#tblAbsentReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once   
                    "deferLoading": 57,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Absent Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Absent Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Absent Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:' + full.colorCode + '">' + full.attendanceStatus + '</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptPerformanceIndividual").is(":checked")) {
                $("#tblPerformanceDateWise").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once    
                    //"deferLoading": 57,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Attendance Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Attendance Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Attendance Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:' + full.colorCode + '">' + full.attendanceStatus + '</span>';
                                //return '<span style="color:' + full.colorCode + '">' + full.attendanceStatus + '</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptMispunch").is(":checked")) {
                $("#tblMispunch").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once  
                    "deferLoading": 57,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Mis Punch Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Mis Punch Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Mis Punch Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:#7738ff">Mispunch</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptManualPunchReport").is(":checked")) {
                $("#tblManualPunchReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once                
                    "deferLoading": 57,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Manual Punch Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Manual Punch Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Manual Punch Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:#4fefff">Manual Punch</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptLatePunchIn").is(":checked")) {
                $("#tblLatePunchIn").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once                
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Late Punch In Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Late Punch In Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Late Punch In Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:#fd841c">Late Punch In</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else if ($("#rptEarlyPunchOut").is(":checked")) {
                $("#tblEarlyPunchOut").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once                
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Early Punch Out Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Early Punch Out Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Early Punch Out Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();


                                //return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Shift", "autoWidth": true },
                        { "data": "inTime", "name": "InTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "OutTime", "title": "Out Time", "autoWidth": true },
                        { "data": "working_hrs", "name": "working_hrs", "title": "Worked Hrs", "autoWidth": true },
                        {
                            "title": "Status", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<span style="color:#af0239">Early Punch Out</span>';
                            }
                        },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }

            else if ($("#rptLocationByReport").is(":checked")) {
                $("#tblLocationByReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "autoWidth": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once                
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Location By Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            text: 'Export to PDF',
                            title: 'Location By Report',
                            extend: 'pdfHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                        {
                            extend: 'print',
                            title: 'Location By Report',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            },
                            footer: true
                        },
                    ],
                    "ajax": {
                        'type': "POST",
                        "dataSrc": "",
                        'url': apiurl,
                        "data": myData,
                        'beforeSend': function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem('Token'));
                            request.setRequestHeader("salt", $("#hdnsalt").val());
                        }
                    },
                    "columnDefs":
                        [{
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }],
                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "employeeCode", "name": "employeeCode", "title": "Employee Code", "autoWidth": true },
                        {
                            "title": "Employee Name", "autoWidth": true,
                            "data": function (data, type, dataToSet) {
                                var fnamee_ = (data.employee_first_name != null && data.employee_first_name != "") ? data.employee_first_name : "";
                                var mnamee_ = (data.employee_middle_name != null && data.employee_middle_name != "") ? data.employee_middle_name : "";
                                var lnamee_ = (data.employee_last_name != null && data.employee_last_name != "") ? data.employee_last_name : "";
                                return fnamee_.toString() + " " + mnamee_.toString() + " " + lnamee_.toString();

                            }
                        },
                        { "data": "attendanceDate", "name": "attendanceDate", "title": "Date", "autoWidth": true },
                        { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                        { "data": "sub_location_name", "name": "sub_location_name", "title": "Sub Location", "autoWidth": true },
                        { "data": "inTime", "name": "inTime", "title": "In Time", "autoWidth": true },
                        { "data": "outTime", "name": "outTime", "title": "Out Time", "autoWidth": true },
                        { "data": "shiftName", "name": "shiftName", "title": "Working Hrs", "autoWidth": true },
                    ],
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }

            _GUID_New();

        });


        $('#btnMonthlthlyReoprt').bind("click", function () {
            // debugger;
            $('#loader').show();
            cleardata();
            var ddlCompany = $("#ddlCompanyM").val();


            var ddllocation = $("#ddllocationM").val();
            var ddldepartment = $("#ddldepartmentM").val();
            var ddlShift = $("#ddlShiftM").val();
            var txtMonthYear = $("#txtMonthYear").val();


            var errormsg = '';
            var iserror = false;

            if (ddlCompany == null || ddlCompany == "" || ddlCompany == "0") {
                errormsg = "Please select company";
                iserror = true;
            }

            if (txtMonthYear == null || txtMonthYear == '') {
                errormsg = "Please select month year ";
                iserror = true;
            }
            else {
                var txtmnyr = new Date($('#txtMonthYear').val());
                //var frmtdate = txtmnyr.dateFormat("YYYY-MM");
                var yr = txtmnyr.getFullYear();
                var mnth = txtmnyr.getMonth() + 1;
                txtMonthYear = yr + "-" + mnth;
            }



            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }


            var myData = {

                'company_id': ddlCompany,
                'location_id': ddllocation,
                'department_id': ddldepartment,
                'Shift_id': ddlShift,
                'FromDate': txtMonthYear
            };

            var apiurl = "";

            if ($("#rptPerformanceDetailReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/PerformanceDetailReport';
            }
            else if ($("#rptPerformanceSummaryReport").is(":checked")) {
                apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/PerformanceSummaryReport';
            }
            else {
                messageBox("error", "Please Select Report Type...!");
                $('#loader').hide();
                return false;
            }

            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            console.log(Obj);

            $.ajax({
                type: "POST",
                url: apiurl,
                data: Obj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                headers: headerss,
                success: function (res) {

                    // debugger;

                    var PayrollCalenders = res.list;
                    var column = res.column;
                    $('#loader').hide();
                    _GUID_New();
                    if ($("#rptPerformanceDetailReport").is(":checked")) {

                        $("#divPerformanceDetailReport").show();
                        $("#divPerformanceSummaryReport").hide();

                        if (PayrollCalenders != null && PayrollCalenders != '') {
                            $('#tblPerformanceDetail').dataTable({
                                "processing": true,
                                "serverSide": false, // for process server side
                                "bDestroy": true,
                                "filter": true, // this is for disable filter (search box)
                                "orderMulti": false, // for disable multiple column at once
                                "scrollX": 200,
                                "data": PayrollCalenders,
                                dom: 'lBfrtip',
                                buttons: [
                                    {
                                        text: 'Export to Excel',
                                        title: 'Performance Detail Report',
                                        extend: 'excelHtml5'
                                    },
                                    {
                                        extend: 'print',
                                        title: 'Performance Detail Report',
                                        footer: true
                                    },
                                ],
                                "columns": column,
                                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
                            });
                        }
                        else {

                            var table = $('#tblPerformanceDetail').DataTable();
                            table.clear().draw();
                        }



                    }
                    else if ($("#rptPerformanceSummaryReport").is(":checked")) {

                        $("#divPerformanceDetailReport").hide();
                        $("#divPerformanceSummaryReport").show();
                        if (PayrollCalenders != null && PayrollCalenders != '') {
                            $('#tblPerformanceSummaryReport').dataTable({
                                "serverSide": false, // for process server side
                                "bDestroy": true,
                                "filter": true, // this is for disable filter (search box)
                                "orderMulti": false, // for disable multiple column at once
                                "scrollX": 200,
                                "data": PayrollCalenders,
                                "columns": column,
                                dom: 'lBfrtip',
                                buttons: [
                                    {
                                        text: 'Export to Excel',
                                        title: 'Performance Summary Report',
                                        extend: 'excelHtml5'
                                    },
                                ],
                                "lengthMenu": [[20, 50, -1], [20, 50, "All"]],
                                rowGroup: {
                                    // Group by office
                                    dataSrc: 'employee_id'
                                }
                            });
                        }
                        else {

                            var table = $('#tblPerformanceSummaryReport').DataTable();
                            table.clear().draw();
                        }
                    }
                    else {

                    }


                },
                error: function (error) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(error.responseText);
                }
            });

        });

    }, 2000);// end timeout

});



function GetData(CompanyId) {
    $('#loader').show();
    if (CompanyId == null || CompanyId == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_EmployeeDetails/' + CompanyId;

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
            var table = $("#tblEmployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "searching": false,
                "paging": false,
                "info": false,
                "bSort": false,
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [

                    ],

                "columns": [
                    { "data": null },
                    // { "data": "employee_id", "name": "employee_id", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="checkbox"  id="txtManagerRemarks" value="' + full.employee_id + '" />';
                        }
                    },
                    { "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    { "data": "card_number", "name": "card_number", "autoWidth": true },
                    { "data": "employee_first_name", "name": "employee_first_name", "autoWidth": true }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            // updateDataTableSelectAllCtrl(table);

        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}


function cleardata() {
    if ($.fn.DataTable.isDataTable('#tblPresentReport')) {
        $('#tblPresentReport').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblAbsentReport')) {
        $('#tblAbsentReport').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblPerformanceDateWise')) {
        $('#tblPerformanceDateWise').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblMispunch')) {
        $('#tblMispunch').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblManualPunchReport')) {
        $('#tblManualPunchReport').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblLatePunchIn')) {
        $('#tblLatePunchIn').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblEarlyPunchOut')) {
        $('#tblEarlyPunchOut').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblLocationByReport')) {
        $('#tblLocationByReport').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblPerformanceDetail')) {
        $('#tblPerformanceDetail').DataTable().clear().draw();
    }
    if ($.fn.DataTable.isDataTable('#tblPerformanceSummaryReport')) {
        $('#tblPerformanceSummaryReport').DataTable().clear().draw();
    }

}