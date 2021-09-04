
var default_company;
var login_emp;
var HaveDisplay;
var is_manager;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        // HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        BindEPA_FiscallYrByComp('ddlfiscalyr', default_company, 0);
        BindEPA_CycleByComp('ddlepa_cycle', default_company, 0);
        BindTabs('ddltabs', default_company, 0)
        // BindEmployeeList1('ddlempcodee', login_emp);

        setSelect('ddlempcodee', login_emp);

        var _manager = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        if (_manager == "yes") {
            is_manager = 1;
        }
        else {
            is_manager = 0;
        }
        // $("#fianl_reviewdetails").hide();

        $("#kradtldiv").hide();
        $("#kpidtldiv").hide();
        $("#tabdtldiv").hide();
        $("#finalreviewdiv").hide();


        $("#ddlcompany").bind("change", function () {
            BindEPA_FiscallYrByComp('ddlfiscalyr', $(this).val(), 0);
            BindEPA_CycleByComp('ddlepa_cycle', $(this).val(), 0);

            BindTabs('ddltabs', $(this).val(), 0)


        });

        $("#ddlfiscalyr").bind("change", function () {
            $("#kradtldiv").hide();
            $("#tblkpidtl").empty();
            $("#kpidtldiv").hide();
            $("#tbltabdtl").empty();
            $("#tabdtldiv").hide();
            $("#tblfinaldtl").empty();
            $("#finalreviewdiv").hide();
        });

        $("#ddlepa_cycle").bind("change", function () {
            $("#kradtldiv").hide();
            $("#tblkpidtl").empty();
            $("#kpidtldiv").hide();
            $("#tbltabdtl").empty();
            $("#tabdtldiv").hide();
            $("#tblfinaldtl").empty();
            $("#finalreviewdiv").hide();
        });

        $("#ddltabs").bind("change", function () {
            $("#kradtldiv").hide();
            $("#tblkpidtl").empty();
            $("#kpidtldiv").hide();
            $("#tbltabdtl").empty();
            $("#tabdtldiv").hide();
            $("#tblfinaldtl").empty();
            $("#finalreviewdiv").hide();
        });

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });

        $("#btnget").bind("click", function () {
            var is_error = false;
            var err_msg = "";

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                is_error = true;
                err_msg = err_msg + "Please select company</br>";
            }

            if ($("#ddlfiscalyr").val() == "" || $("#ddlfiscalyr").val() == "0" || $("#ddlfiscalyr").val() == null) {
                is_error = true;
                err_msg = err_msg + "Please select financial year</br>";
            }

            if ($("#ddlepa_cycle").val() == "" || $("#ddlepa_cycle").val() == "0" || $("#ddlepa_cycle").val() == null) {
                is_error = true;
                err_msg = err_msg + "Please select epa cycle</br>";
            }

            if ($("#ddltabs").val() == "" || $("#ddltabs").val() == "0" || $("#ddltabs").val() == null) {
                is_error = true;
                err_msg = err_msg + "Please select tab name </br>";
            }

            if (is_error) {
                messageBox("error", err_msg);
                return false;
            }

            //if ($.fn.DataTable.isDataTable('#tbltabwisedtl')) {
            //    $('#tbltabwisedtl').DataTable().destroy();
            //}

            //  $('#tbltabwisedtl tbody').empty();

            GetDetails();


        });


    }, 2000);// end timeout


});



function BindTabs(ControlId, companyid, SelectedVal) {
    ControlId = "#" + ControlId;
    $(ControlId).empty().append("<option value=0>--Please select--</option>");

    if (companyid > 0) {
        $("#loader").show();
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiePA/Get_TabMaster/0/" + companyid,
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;
                $(ControlId).empty().append("<option value=0>--Please select--</option>");
                $(ControlId).append("<option value=-1>KRA</option>");
                $(ControlId).append("<option value=-2>KPI</option>");
                $(ControlId).append("<option value=-3>Final Review</option>");
                $(ControlId).append("<option value=-4>EPA Status</option>");
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.tab_mstr_id).html(value.tab_name));
                });

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                }
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
                $("#loader").hide();
            },
            error: function (err) {
                $("#loader").hide();
                messageBox("error", err.responseText);
            }
        });

    }
}

function GetDetails() {

    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_TabDetals/" + $("#ddltabs").val() + "/" + $("#ddlcompany").val() + "/" + $("#ddlfiscalyr").val() + "/" + $("#ddlepa_cycle").val(),
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined && res.statusCode != null) {
                messageBox("error", res.message);
                return false;
            }

            var lst_data = [];
            // var ColumnData = [];


            if ($("#ddltabs").val() == "-1") {
                //var lst_data = [];

                $("#kradtldiv").show();
                $("#tblkpidtl").empty();
                $("#kpidtldiv").hide();
                $("#tbltabdtl").empty();
                $("#tabdtldiv").hide();
                $("#tblfinaldtl").empty();
                $("#finalreviewdiv").hide();



                for (var i = 0; i < res.length; i++) {
                    var code = res[i].emp_code != null ? res[i].emp_code : "-";
                    var _name = res[i].emp_name != null ? res[i].emp_name : "-";
                    var _dept = res[i].department_name != null ? res[i].department_name : "-";
                    var _wrname = res[i].w_r_name != null ? res[i].w_r_name : "-";
                    if (res[i].mdlEpaKRADetails != null) {
                        for (j = 0; j < res[i].mdlEpaKRADetails.length; j++) {
                            var _data = {};
                            _data["emp_code"] = code;
                            _data["emp_name"] = _name;
                            _data["department_name"] = _dept;
                            _data["w_r_name"] = _wrname;
                            _data["factor_result"] = res[i].mdlEpaKRADetails[j].factor_result != null ? res[i].mdlEpaKRADetails[j].factor_result : "-";
                            _data["description"] = res[i].mdlEpaKRADetails[j].description != null ? res[i].mdlEpaKRADetails[j].description : "-";
                            _data["rating_name"] = res[i].mdlEpaKRADetails[j].rating_name != null ? res[i].mdlEpaKRADetails[j].rating_name : "-";


                            lst_data.push(_data);
                        }
                    }
                    else {
                        var _data = {};
                        _data["emp_code"] = code;
                        _data["emp_name"] = _name;
                        _data["department_name"] = _dept;
                        _data["w_r_name"] = _wrname;
                        _data["factor_result"] = "-";
                        _data["description"] = "-";
                        _data["rating_name"] = "-";


                        lst_data.push(_data);
                    }
                }


                $("#tblkradtl").DataTable({
                    "processing": true,//for show progress bar
                    "serverSide": false,// for process server side
                    "orderMulti": false,
                    "bDestroy": true,//for remove previous element
                    "filter": true, // this is for disable filter (search box)
                    "scrollX": 200,
                    "aaData": lst_data,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'KRA List',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7]
                            }
                        },
                    ],
                    "columnDefs": [],
                    "columns": [
                        { "data": null, "autoWidth": true, "title": "SNo." },
                        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                        { "data": "w_r_name", "name": "w_r_name", "title": "Working Role", "autoWidth": true },
                        { "data": "factor_result", "name": "factor_result", "title": "KRA Factor Result", "autoWidth": true },
                        { "data": "description", "name": "description", "title": "KRA Description", "autoWidth": true },
                        { "data": "rating_name", "name": "rating_name", "title": "Rating Name", "autoWidth": true },

                    ],//ColumnData,
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });


            }
            else if ($("#ddltabs").val() == "-2") {
                $("#tblkradtl").empty();
                $("#kradtldiv").hide();
                $("#tblkpidtl").empty();
                $("#kpidtldiv").show();
                $("#tbltabdtl").empty();
                $("#tabdtldiv").hide();
                $("#tblfinaldtl").empty();
                $("#finalreviewdiv").hide();

                for (var i = 0; i < res.length; i++) {
                    var code = res[i].emp_code != null ? res[i].emp_code : "-";
                    var _name = res[i].emp_name != null ? res[i].emp_name : "-";
                    var _dept = res[i].department_name != null ? res[i].department_name : "-";
                    var _wrname = res[i].w_r_name != null ? res[i].w_r_name : "-";
                    if (res[i].mdlEpaKPIDetails != null) {
                        for (j = 0; j < res[i].mdlEpaKPIDetails.length; j++) {

                            var objective_name_ = res[i].mdlEpaKPIDetails[j].objective_name != null ? res[i].mdlEpaKPIDetails[j].objective_name : "-";
                            var key_area_ = res[i].mdlEpaKPIDetails[j].key_area != null ? res[i].mdlEpaKPIDetails[j].key_area : "-";
                            var _score = res[i].mdlEpaKPIDetails[j].score != null ? res[i].mdlEpaKPIDetails[j].score : "-";

                            var _data = {};

                            _data["emp_code"] = code;
                            _data["emp_name"] = _name;
                            _data["department_name"] = _dept;
                            _data["w_r_name"] = _wrname;
                            _data["objective_name"] = objective_name_;
                            _data["key_area"] = key_area_;
                            _data["score"] = _score;


                            lst_data.push(_data);

                            //if (res[i].mdlEpaKPIDetails[j].mdlEpaKPICriteriaDetails != null) {

                            //    for (m = 0; m < res[i].mdlEpaKPIDetails[j].mdlEpaKPICriteriaDetails.length; m++) {

                            //        _data["criteria_name"] = res[i].mdlEpaKPIDetails[j].mdlEpaKPICriteriaDetails[m].criteria_name != null ? res[i].mdlEpaKPIDetails[j].mdlEpaKPICriteriaDetails[m].criteria_name : "-";



                            //    }
                            //}
                            //else {

                            //    var _data1 = {};

                            //    _data1["emp_code"] = code;
                            //    _data1["emp_name"] = _name;
                            //    _data1["department_name"] = _dept;
                            //    _data1["w_r_name"] = _wrname;
                            //    _data1["objective_name"] = objective_name_;
                            //    _data1["key_area"] = key_area_;
                            //    _data1["score"] = _score;
                            //    _data1["criteria_name"] = "-";

                            //    lst_data.push(_data1);
                            //}

                        }
                    }
                    else {
                        var _data2 = {};

                        _data2["emp_code"] = code;
                        _data2["emp_name"] = _name;
                        _data2["department_name"] = _dept;
                        _data2["w_r_name"] = _wrname;
                        _data2["objective_name"] = "-";
                        _data2["key_area"] = "-";
                        _data2["score"] = "-";
                        // _data2["criteria_name"] = "-";

                        lst_data.push(_data2);
                    }
                }


                $("#tblkpidtl").DataTable({
                    "processing": true,//for show progress bar
                    "serverSide": false,// for process server side
                    "orderMulti": false,
                    "bDestroy": true,//for remove previous element
                    "filter": true, // this is for disable filter (search box)
                    "scrollX": 200,
                    "aaData": lst_data,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'KPI List',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7]
                            }
                        },
                    ],
                    "columnDefs": [],
                    "columns": [
                        { "data": null, "title": "SNo.", "autoWidth": true },
                        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                        { "data": "w_r_name", "name": "w_r_name", "title": "Working Role", "autoWidth": true },
                        { "data": "objective_name", "name": "objective_name", "title": "KPI Objective Name", "autoWidth": true },
                        { "data": "key_area", "name": "key_area", "title": "KPI Key Area", "autoWidth": true },
                        { "data": "score", "name": "score", "title": "KPI Score", "autoWidth": true },
                        //{ "data": "criteria_name", "name": "criteria_name", "title": "KPI Criteria Name", "autoWidth": true },
                    ], //ColumnData,
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });


            }
            else if ($("#ddltabs").val() == "-3") {
                $("#tblkradtl").empty();
                $("#kradtldiv").hide();
                $("#tblkpidtl").empty();
                $("#kpidtldiv").hide();
                $("#tbltabdtl").empty();
                $("#tabdtldiv").hide();
                $("#tblfinaldtl").empty();
                $("#finalreviewdiv").show();


                for (i = 0; i < res.length; i++) {
                    var data = {};
                    data["emp_code"] = res[i].emp_code != null ? res[i].emp_code : "-";
                    data["emp_name"] = res[i].emp_name != null ? res[i].emp_name : "-";
                    data["desig_name"] = res[i].desig_name != null ? res[i].desig_name : "-";
                    data["department_name"] = res[i].department_name != null ? res[i].department_name : "-";
                    data["w_r_name"] = res[i].w_r_name != null ? res[i].w_r_name : "-";
                    data["final_review"] = res[i].final_review != null ? (res[i].final_review == "0" ? "Pending" : res[i].final_review == "1" ? "Poor" : res[i].final_review == "2" ? "Need Improvement" : res[i].final_review == "3" ? "Standard" : res[i].final_review == "4" ? "Good" : res[i].final_review == "5" ? "Outstanding" : "") : "";
                    data["final_remarks"] = res[i].final_remarks != null ? res[i].final_remarks : "-";

                    lst_data.push(data);
                }


                $("#tblfinaldtl").DataTable({
                    "processing": true,//for show progress bar
                    "serverSide": false,// for process server side
                    "orderMulti": false,
                    "bDestroy": true,//for remove previous element
                    "filter": true, // this is for disable filter (search box)
                    "scrollX": 200,
                    "aaData": lst_data,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Final Review List',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7]
                            }
                        },
                    ],
                    "columnDefs": [],
                    "columns": [
                        { "data": null, "autoWidth": true, "title": "SNo." },
                        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "desig_name", "name": "desig_name", "title": "Designation", "autoWidth": true },
                        { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                        { "data": "w_r_name", "name": "w_r_name", "title": "Working Role", "autoWidth": true },
                        { "data": "final_review", "name": "final_review", "title": "Final Review", "autoWidth": true },
                        { "data": "final_remarks", "name": "final_remarks", "title": "Final Remarks", "autoWidth": true },

                    ],//ColumnData,
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });

            }
            else if ($("#ddltabs").val() == "-4") {
                $("#tblkradtl").empty();
                $("#kradtldiv").hide();
                $("#tblkpidtl").empty();
                $("#kpidtldiv").hide();
                $("#tbltabdtl").empty();
                $("#tabdtldiv").hide();
                $("#tblfinaldtl").empty();
                $("#finalreviewdiv").show();


                for (i = 0; i < res.length; i++) {
                    var data = {};
                    data["emp_code"] = res[i].emp_code != null ? res[i].emp_code : "-";
                    data["emp_name"] = res[i].emp_name != null ? res[i].emp_name : "-";
                    data["desig_name"] = res[i].desig_name != null ? res[i].desig_name : "-";
                    data["department_name"] = res[i].department_name != null ? res[i].department_name : "-";
                    data["w_r_name"] = res[i].w_r_name != null ? res[i].w_r_name : "-";
                    data["epa_close_status_name"] = res[i].epa_close_status_name != null ? res[i].epa_close_status_name : "-";
                    data["epa_current_status_name"] = res[i].epa_current_status_name != null ? res[i].epa_current_status_name : "-";

                    lst_data.push(data);
                }


                $("#tblfinaldtl").DataTable({
                    "processing": true,//for show progress bar
                    "serverSide": false,// for process server side
                    "orderMulti": false,
                    "bDestroy": true,//for remove previous element
                    "filter": true, // this is for disable filter (search box)
                    "scrollX": 200,
                    "aaData": lst_data,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'EPA Status List',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7]
                            }
                        },
                    ],
                    "columnDefs": [],
                    "columns": [
                        { "data": null, "autoWidth": true, "title": "SNo." },
                        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "desig_name", "name": "desig_name", "title": "Designation", "autoWidth": true },
                        { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                        { "data": "w_r_name", "name": "w_r_name", "title": "Working Role", "autoWidth": true },
                        { "data": "epa_current_status_name", "name": "epa_current_status_name", "title": "Current Status", "autoWidth": true },
                        { "data": "epa_close_status_name", "name": "epa_close_status_name", "title": "Close Status", "autoWidth": true },


                    ],//ColumnData,
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });
            }
            else {
                $("#tblkradtl").empty();
                $("#kradtldiv").hide();
                $("#tblkpidtl").empty();
                $("#kpidtldiv").hide();
                $("#tbltabdtl").empty();
                $("#tabdtldiv").show();
                $("#tblfinaldtl").empty();
                $("#finalreviewdiv").hide();



                for (var i = 0; i < res.length; i++) {
                    var emp_code = res[i].emp_code != null ? res[i].emp_code : "-";
                    var emp_name = res[i].emp_name != null ? res[i].emp_name : "-";
                    var dept = res[i].department_name != null ? res[i].department_name : "-";
                    var wrole = res[i].w_r_name != null ? res[i].w_r_name : "-";

                    if (res[i].mdlEpaTabQuestions != null) {
                        for (j = 0; j < res[i].mdlEpaTabQuestions.length; j++) {
                            var _data = {};
                            _data["emp_code"] = emp_code;
                            _data["emp_name"] = emp_name;
                            _data["department_name"] = dept;
                            _data["w_r_name"] = wrole;
                            _data["ques"] = res[i].mdlEpaTabQuestions[j].ques != null ? res[i].mdlEpaTabQuestions[j].ques : "-";
                            _data["ans_type"] = res[i].mdlEpaTabQuestions[j].ans_type != null ? (res[i].mdlEpaTabQuestions[j].ans_type == "1" ? "" + "TextBox" + "" : res[i].mdlEpaTabQuestions[j].ans_type == "2" ? "" + "Drop Down" + "" : res[i].mdlEpaTabQuestions[j].ans_type == "3" ? "" + "Yes/No with Remarks" + "" : res[i].mdlEpaTabQuestions[j].ans_type == "4" ? "" + "Yes/No without Remarks" + "" : "-") : "-";
                            _data["ans_type_ddl"] = res[i].mdlEpaTabQuestions[j].ans_type_ddl != null ? res[i].mdlEpaTabQuestions[j].ans_type_ddl : "-";
                            _data["question_answer"] = res[i].mdlEpaTabQuestions[j].question_answer != null ? res[i].mdlEpaTabQuestions[j].question_answer : "-";

                            lst_data.push(_data);
                        }

                    }
                    else {
                        var _data1 = {};
                        _data1["emp_code"] = emp_code;
                        _data1["emp_name"] = emp_name;
                        _data1["department_name"] = dept;
                        _data1["w_r_name"] = wrole;
                        _data1["ques"] = "-";
                        _data1["ans_type"] = "-";
                        _data1["ans_type_ddl"] = "-";
                        _data1["question_answer"] = "-";

                        lst_data.push(_data1);
                    }
                }


                $("#tbltabdtl").DataTable({
                    "processing": true,//for show progress bar
                    "serverSide": false,// for process server side
                    "orderMulti": false,
                    "bDestroy": true,//for remove previous element
                    "filter": true, // this is for disable filter (search box)
                    "scrollX": 200,
                    "aaData": lst_data,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Tab List',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8]
                            }
                        },
                    ],
                    "columnDefs": [],
                    "columns": [
                        { "data": null, "autoWidth": true, "title": "SNo." },
                        { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                        { "data": "w_r_name", "name": "w_r_name", "title": "Working Role", "autoWidth": true },
                        { "data": "ques", "name": "ques", "title": "Questions", "autoWidth": true },
                        { "data": "ans_type", "name": "ans_type", "title": "Answer Type", "autoWidth": true },
                        { "data": "ans_type_ddl", "name": "ans_type_ddl", "title": "Data Source", "autoWidth": true },
                        { "data": "question_answer", "name": "question_answer", "title": "Answer", "autoWidth": true },

                    ],//ColumnData,
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },

                });


            }
            $("#loader").hide();

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}




