$('#loader').show();

var default_company;
var login_emp_id;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp_id, 0);

        $("#myModal").hide();
        $('#loader').hide();

    }, 2000);// end timeout

});


function DownloadEmployeeOfficaialSectionExcelFile() {

    var ValUploadSection = $('#ddlUploadSection').val();

    if (ValUploadSection == '0') {
        alert('Please select upload section...!');
        return false;
    }
    else if (ValUploadSection == 'FullUpload') {
        window.open("/UploadFormat/Employee Full Entery.xlsx");
    }
    if (ValUploadSection == 'QualificationSection') {
        window.open("/UploadFormat/Employee Qualification Section.xlsx");
    }
    else if (ValUploadSection == 'FamilySection') {
        window.open("/UploadFormat/Employee Family Section.xlsx");
    }
    else if (ValUploadSection == 'ShiftAllocation') {
        window.open("/UploadFormat/Employee Shift Allocation.xlsx");
    }
    else if (ValUploadSection == 'WeekOffAllocation') {
        window.open("/UploadFormat/Employee WeekOff Allocation.xlsx");
    }
    else if (ValUploadSection == 'GradeAllocation') {
        window.open("/UploadFormat/Employee Grade Allocation.xlsx");
    }
    else if (ValUploadSection == 'DesignationAllocation') {
        window.open("/UploadFormat/Employee Designation Allocation.xlsx");
    }
    else if (ValUploadSection == 'ManagerAllocation') {
        window.open("/UploadFormat/Employee Manager Allocation.xlsx");
    }
    else if (ValUploadSection == 'PersonalSection') {
        window.open("/UploadFormat/Employee  Personal Details.xlsx");
    }
    else if (ValUploadSection == 'AccountSection') {
        window.open("/UploadFormat/Employee Account Details.xlsx");
    }
    else if (ValUploadSection == 'UANSection') {
        window.open("/UploadFormat/Employee UAN and ESIC Details.xlsx");
    }
    else if (ValUploadSection == 'EmployeePunch') {
        window.open("/UploadFormat/Employee Punch.xlsx");
    }
    else if (ValUploadSection == 'EmployeeAttendanceStatus') {
        window.open("/UploadFormat/Employee Attendance Status.xlsx");
    }
    else if (ValUploadSection == 'DeleteManualPunch') {
        window.open("/UploadFormat/Employee Delete Manual Punch.xlsx");
    }
   
}

$("#btnsave").bind("click", function () {


    var errormsg = "";
    var iserror = false;

    var company_id = $("#ddlCompany").val();
    var ValUploadSection = $('#ddlUploadSection').val();


    if (company_id == "" || company_id == null || company_id == "undefined") {
        errormsg = errormsg + "Please Select Company </br>";
        iserror = true;
    }
    if (ValUploadSection == '' || ValUploadSection == null || ValUploadSection == '0') {
        errormsg = errormsg + 'Please select upload section...!';
        iserror = true;
    }

    if ($("#FileBulkUpload").val() == "" || $("#FileBulkUpload").val() == null) {
        errormsg = errormsg + 'Please select File...!';
        iserror = true;
    }


    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }

    var mydata = {
        default_company_id: company_id,
        created_by: login_emp_id,
        UploadSection: ValUploadSection
    };

    var file = document.getElementById("FileBulkUpload").files[0];

    var Obj = JSON.stringify(mydata);


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    var formData = new FormData();
    formData.append('AllData', Obj);
    formData.append('files', file);

    $('#loader').show();

    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiEmployee/BulkUploadEmployeeData/",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        headers: headerss,
        success: function (data) {
            _GUID_New();
            $('#loader').hide();
            $("#FileBulkUpload").val('');
            if (data.objresponse != undefined) {
                if (data.objresponse.statusCode != undefined) {
                    if (data.objresponse.statusCode == "0") {
                        $('#ddlUploadSection').val(0);
                        $("#FileBulkUpload").val('')
                        //messageBox("success", data.message);
                    }
                    else {
                        messageBox("info", data.objresponse.message);
                        return false;
                    }

                }
            }
            else {
                $('#ddlUploadSection').val(0);
                $("#FileBulkUpload").val('')
                messageBox("info", data.message);
                return false;
            }
           
            
            if (ValUploadSection == 'QualificationSection') {
                var duplist_q = data.duplicateQualification;
                var missingdtllist = data.missingQualification;
                var issuelist = data.issueQualificationl;
                var deetailmsg = data.missingDtlMessage;

                if (issuelist != null && issuelist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Incorrect Data Detail in Excel'
                    });


                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        dom: 'lBfrtip',
                        "aaData": issuelist,
                        "columnDefs": [],
                        buttons: [
                            {
                                text: 'Export to Excel',
                                title: 'Bulk Upload Qualification Issue Detail',
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: [0, 1]
                                }
                            },
                        ],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "error_message", "name": "error_message", "title": "Issue Detail", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });

                    messageBox("success", data.objresponse.message);
                    return false;

                }
                else if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "board_or_university", "name": "board_or_university", "title": "Board/Universty", "autoWidth": true },
                            { "data": "institute_or_school", "name": "institute_or_school", "title": "Institute/School", "autoWidth": true },
                            { "data": "passing_year", "name": "passing_year", "title": "Passign Year", "autoWidth": true },
                            { "data": "stream", "name": "stream", "title": "Stream", "autoWidth": true },
                            { "data": "education_type", "name": "education_type", "title": "Education Type", "autoWidth": true },
                            { "data": "education_level", "name": "education_level", "title": "Eduction Level", "autoWidth": true },
                            { "data": "marks_division_cgpa", "name": "marks_division_cgpa", "title": "Marks/Division/CGPA", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (duplist_q != null && duplist_q.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist_q,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "board_or_university", "name": "board_or_university", "title": "Board/Universty", "autoWidth": true },
                            { "data": "institute_or_school", "name": "institute_or_school", "title": "Institute/School", "autoWidth": true },
                            { "data": "passing_year", "name": "passing_year", "title": "Passign Year", "autoWidth": true },
                            { "data": "stream", "name": "stream", "title": "Course", "autoWidth": true },
                            { "data": "education_type", "name": "education_type", "title": "Education Type", "autoWidth": true },
                            { "data": "education_level", "name": "education_level", "title": "Eduction Level", "autoWidth": true },
                            { "data": "marks_division_cgpa", "name": "marks_division_cgpa", "title": "Marks/Division/CGPA", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.objresponse.statusCode;
                    var Msg = data.objresponse.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }

            }
            else if (ValUploadSection == 'FamilySection') {
                var missingdtllist = data.missingFamily;
                var duplicate_fmly_dtl = data.duplicateFamily;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [
                                {
                                    targets: [4],
                                    render: function (data, type, row) {

                                        var date = new Date(data);
                                        return GetDateFormatddMMyyyy(date);
                                    }
                                }

                            ],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "relation", "name": "relation", "title": "Relation", "autoWidth": true },
                            { "data": "occupation", "name": "occupation", "title": "Occupation", "autoWidth": true },
                            { "data": "name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name", "autoWidth": true },
                            { "data": "date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                            { "data": "gender", "name": "gender", "title": "Gender", "autoWidth": true },
                            { "data": "dependent", "name": "dependent", "title": "Dependent", "autoWidth": true },
                            { "data": "is_nominee", "name": "is_nominee", "title": "Nominee", "autoWidth": true },
                            { "data": "nominee_percentage", "name": "nominee_percentage", "title": "Nominee Percentage(Only No.)", "autoWidth": true },
                            { "data": "aadhar_card_no", "name": "aadhar_card_no", "title": "Aadhar Card No.", "autoWidth": true },
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });

                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (duplicate_fmly_dtl != null && duplicate_fmly_dtl.length > 0) {

                    messageBox("error", deetailmsg);
                    return false;

                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'ShiftAllocation') {
                var missingdtllist = data.missingShiftAlloc;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "shift_id", "name": "shift_id", "title": "shift", "autoWidth": true },
                            { "data": "applicable_from_date", "name": "applicable_from_date", "title": "From Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'WeekOffAllocation') {
                var missingdtllist = data.missingdetaillist;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "weekly_off", "name": "weekly_off", "title": "Week Off", "autoWidth": true },
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'GradeAllocation') {
                var missingdtllist = data.missingGradeAlloc;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "grade_id", "name": "grade_id", "title": "Grade", "autoWidth": true },
                            { "data": "applicable_from_date", "name": "applicable_from_date", "title": "From Date", "autoWidth": true },
                            { "data": "applicable_to_date", "name": "applicable_to_date", "title": "To Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'DesignationAllocation') {
                var missingdtllist = data.missingDesignationAlloc;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "designation_id", "name": "designation_id", "title": "Designation", "autoWidth": true },
                            { "data": "applicable_from_date", "name": "applicable_from_date", "title": "From Date", "autoWidth": true },
                            { "data": "applicable_to_date", "name": "applicable_to_date", "title": "To Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'ManagerAllocation') {
                var missingdtllist = data.missingManagerAlloc;
                var deetailmsg = data.missingDtlMessage;

                if (missingdtllist != null && missingdtllist.length > 0) {
                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],
                        "columns": [
                            { "data": "m_one_id", "name": "m_one_id", "title": "Reporting Manager 1", "autoWidth": true },
                            { "data": "m_two_id", "name": "m_two_id", "title": "Reporting Manager 2", "autoWidth": true },
                            { "data": "m_three_id", "name": "m_three_id", "title": "Reporting Manager 3", "autoWidth": true },
                            { "data": "applicable_from_date", "name": "applicable_from_date", "title": "From Date", "autoWidth": true },
                            { "data": "applicable_to_date", "name": "applicable_to_date", "title": "To Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                        "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {
                            $('.footer').html('anshuman');
                        }
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }

                }
            }
            else if (ValUploadSection == 'PersonalSection') {

                var duplist = data.duplicatepersonaldtl;
                var missingdtllist = data.missingpersonaldtl;
                var deetailmsg = data.missingDtlMessage;

                if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "blood_group_name", "name": "blood_group_name", "title": "Blood Group", "autoWidth": true },
                            { "data": "primary_contact_number", "name": "primary_contact_number", "title": "Primary Contact Number", "autoWidth": true },
                            { "data": "primary_email_id", "name": "primary_email_id", "title": "Primary Email ID", "autoWidth": true },
                            { "data": "permanent_address_line_one", "name": "permanent_address_line_one", "title": "Permanent Address", "autoWidth": true },
                            { "data": "permanent_country", "name": "permanent_country", "title": "Country", "autoWidth": true },
                            { "data": "permanent_state", "name": "permanent_state", "title": "State", "autoWidth": true },
                            { "data": "permanent_city", "name": "permanent_city", "title": "City", "autoWidth": true },
                            { "data": "permanent_pin_code", "name": "permanent_pin_code", "title": "PIN Code", "autoWidth": true },
                            { "data": "emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Name", "autoWidth": true },
                            { "data": "emergency_contact_relation", "name": "emergency_contact_relation", "title": "Relation", "autoWidth": true },
                            { "data": "emergency_contact_mobile_number", "name": "emergency_contact_mobile_number", "title": "Emergency Contact Number", "autoWidth": true },

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }

            }
            else if (ValUploadSection == 'AccountSection') {

                var duplist = data.duplicateAccountdtl;
                var missingdtllist = data.missingAccountdtlc;
                var deetailmsg = data.missingDtlMessage;

                if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "payment_mode", "name": "payment_mode", "title": "Payment Mode", "autoWidth": true },
                            { "data": "bank_id", "name": "bank_id", "title": "Bank", "autoWidth": true },
                            { "data": "branch_name", "name": "branch_name", "title": "Branch Name", "autoWidth": true },
                            { "data": "ifsc_code", "name": "ifsc_code", "title": "ifsc Code", "autoWidth": true },
                            { "data": "bank_acc", "name": "bank_acc", "title": "Bank Acc", "autoWidth": true },
                            { "data": "pan_card_name", "name": "pan_card_name", "title": "Pan Name", "autoWidth": true },
                            { "data": "pan_card_number", "name": "pan_card_number", "title": "Pan Number", "autoWidth": true },
                            { "data": "aadha_card_name", "name": "aadha_card_name", "title": "Aadha Name", "autoWidth": true },
                            { "data": "aadha_card_number", "name": "aadha_card_number", "title": "Aadha Card Number", "autoWidth": true },
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }
            }
            else if (ValUploadSection == 'UANSection') {

                var duplist = data.duplicateAccountdtl;
                var missingdtllist = data.missingAccountdtlc;
                var deetailmsg = data.missingDtlMessage;

                if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "uan_number", "name": "uan_number", "title": "Uan Number", "autoWidth": true },
                            { "data": "pf_number", "name": "pf_number", "title": "PF Number", "autoWidth": true },

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }
            }
            else if (ValUploadSection == 'EmployeePunch') {

                var duplist = data.duplicateAttendancedtl;
                var missingdtllist = data.missinAttendancetlc;
                var deetailmsg = data.missingDtlMessage;

                if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "attendance_dt", "name": "attendance_dt", "title": "Attendance Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }

            }
            else if (ValUploadSection == 'EmployeeAttendanceStatus') {
                var duplist = data.duplicateAttendancelist;
                var missingdtllist = data.missinAttendancelist;
                var issuedtllist = data.issueAttendencelist;
                var deetailmsg = data.missingDtlMessage;

                if (issuedtllist != null && issuedtllist.length > 0) {


                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Incorrect Data Detail in Excel'
                    });
                    //$.extend($.fn.dataTable.defaults, {
                    //    sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    //});

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        dom: 'lBfrtip',
                        "aaData": issuedtllist,
                        "columnDefs": [
                            {
                                targets: [1],
                                render: function (data, type, row) {
                                    var date = new Date(data);
                                    return data == "0001-01-01T00:00:00" ? " # " : GetDateFormatddMMyyyy(date);
                                }
                            }
                        ],
                        buttons: [
                            {
                                text: 'Export to Excel',
                                title: 'Bulk Upload All Detail',
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: [0, 1, 2]
                                }
                            },
                        ],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "attendance_dt", "name": "attendance_dt", "title": "Attendance Date", "autoWidth": true },
                            { "data": "error_message", "name": "error_message", "title": "Issue Detail", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });

                    messageBox("success", data.objresponse.message);
                    return false;

                }
                else if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "attendance_dt", "name": "attendance_dt", "title": "Attendance Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }
            }
            else if (ValUploadSection == 'DeleteManualPunch') {

                var duplist = data.duplicateAttendancedtl;
                var missingdtllist = data.missinAttendancetlc;
                var deetailmsg = data.missingDtlMessage;

                if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "attendance_dt", "name": "attendance_dt", "title": "Attendance Date", "autoWidth": true }
                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }
            }
            else if (ValUploadSection == 'FullUpload') {

                var duplist = data.duplicateFullUpladdtl;
                var missingdtllist = data.missinFullUpladtlc;
                var issuelist = data.issueFullUpladdtl;
                var deetailmsg = data.missingDtlMessage;

                if (issuelist != null && issuelist.length > 0) {


                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Incorrect Data Detail in Excel'
                    });


                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        dom: 'lBfrtip',
                        "aaData": issuelist,
                        "columnDefs": [],
                        buttons: [
                            {
                                text: 'Export to Excel',
                                title: 'Bulk Upload All Detail',
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: [0, 1, 2, 3, 4, 5]
                                }
                            },
                        ],
                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "emp_name", "name": "emp_name", "title": "Emp Name", "autoWidth": true },
                            { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                            { "data": "email_work", "name": "email_work", "title": "Email Work", "autoWidth": true },
                            { "data": "card_number", "name": "card_number", "title": "Card No.", "autoWidth": true },
                            { "data": "error_message", "name": "error_message", "title": "Issue Detail", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });

                    messageBox("success", data.objresponse.message);
                    return false;

                }
                else if (duplist != null && duplist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Duplicate Detail in Excel'
                    });

                    //$.extend($.fn.dataTable.defaults, {
                    //    sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    //});

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": duplist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "card_number", "name": "card_number", "title": "Card No.", "autoWidth": true },
                            { "data": "email_work", "name": "email_work", "title": "Email ID", "autoWidth": true },
                            { "data": "paN_No", "name": "paN_No", "title": "PAN Card", "autoWidth": true },
                            { "data": "pF_number", "name": "pf_number", "title": "PF Number", "autoWidth": true },
                            { "data": "esiC_number", "name": "esic_number", "title": "ESIC Number", "autoWidth": true }

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else if (missingdtllist != null && missingdtllist.length > 0) {

                    $("#myModal").show();
                    var modal = document.getElementById("myModal");
                    modal.style.display = "block";

                    $('#myModal').modal({
                        modal: 'true',
                        title: 'Missing / Already exist in DB Details'
                    });
                    $.extend($.fn.dataTable.defaults, {
                        sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                    });

                    $("#tbl_exceldtl").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        "scrollX": 200,
                        "aaData": missingdtllist,
                        "columnDefs":
                            [],

                        "columns": [
                            { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "emp_name", "name": "emp_name", "title": "Emp Name", "autoWidth": true },
                            { "data": "gender", "name": "gender", "title": "Gender", "autoWidth": true },
                            { "data": "blood_group", "name": "blood_group", "title": "Blood Group", "autoWidth": true },
                            { "data": "marital_status", "name": "marital_status", "title": "Marital Status", "autoWidth": true },
                            { "data": "employee_status", "name": "employee_status", "title": "Employee Status", "autoWidth": true },
                            { "data": "card_number", "name": "card_number", "title": "Card Number", "autoWidth": true },
                            { "data": "email_work", "name": "email_work", "title": "Email Work", "autoWidth": true },
                            { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                            { "data": "location_name", "name": "location_name", "title": "Location Name", "autoWidth": true },
                            { "data": "department_name", "name": "department_name", "title": "Department Name", "autoWidth": true },
                            { "data": "designation_name", "name": "designation_name", "title": "Designation Name", "autoWidth": true },
                            { "data": "grade_name", "name": "grade_name", "title": "Grade Name", "autoWidth": true },
                            { "data": "salary_group", "name": "salary_group", "title": "Salary Group", "autoWidth": true },
                            { "data": "bank_name", "name": "bank_name", "title": "Bank Name", "autoWidth": true },
                            { "data": "bank_IFSC_Code", "name": "bank_IFSC_Code", "title": "Bank IFSC Code", "autoWidth": true },
                            { "data": "salary_account_no", "name": "salary_account_no", "title": "salary Account No", "autoWidth": true },
                            { "data": "salary_account_no", "name": "salary_account_no", "title": "salary Account No", "autoWidth": true },

                        ],
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                    });


                    messageBox("error", deetailmsg);
                    return false;
                }
                else {

                    var statuscode = data.objresponse.statusCode;
                    var Msg = data.objresponse.message;
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                        return false;
                    }
                }

            }
            
           
        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });

});




function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#FileBulkUpload").val("");
            alert("Upload only Excel file and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#FileBulkUpload").val("");
                alert("Upload only Excel file and on define format. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }