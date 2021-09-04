$('#loader').show();

var emp_idd;
var default_company;

$(document).ready(function () {

    emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    if (localStorage.getItem("new_compangy_idd") != null) {
        BindAllEmp_Company('ddlCompany', emp_idd, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
    }
    else {
        BindAllEmp_Company('ddlCompany', emp_idd, default_company);
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company,0);
        localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
    }

    // var HaveDisplay = ISDisplayMenu("Display Company List");


    if (localStorage.getItem("new_emp_id") != null) {
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }),0);
        $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
        GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
    }

    $("#btnupdate").hide();
    $('#loader').hide();



    $('#ddlCompany').change(function () {
        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);
        localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
        if ($.fn.DataTable.isDataTable('#tbl_emp_pr_employement_dtl')) {
            $('#tbl_emp_pr_employement_dtl').DataTable().clear().draw();
        }

    });

    $('#ddlEmployeeCode').change(function () {
        // localStorage.setItem("new_emp_id", $(this).val());
        //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
        GetData($(this).val());
    });

});


$("#btnreset").bind("click", function () {
    location.reload();
});

$("#btnsave").bind("click", function () {

    var companyid = $("#ddlCompany").val();
    var empid = $("#ddlEmployeeCode").val();
    var comp_name = $("#txtprev_comp_name").val();
    var comp_address = $("#txtprev_comp_address").val();
    var _doj = $("#txt_doj").val();
    var _relieve_dt = $("#txt_relieve").val();
    var is_relieve = $("#ddl_is_relieve").val();
    var _desig = $("#txtprev_desig").val();
    var _salary = $("#txtprev_salary").val();
    var _jobtype = $("#ddlpr_jobtype").val();
    var _reason = $("#txtprev_relieve_reason").val();
    var reportingto = $("#ddl_prev_reporting_to").val();
    var rpt_name = $("#txtprev_rpt_name").val();
    var contact = $("#txtprev_contact_no").val();
    var email_ = $("#txtprev_email_id").val();
    var remarks = $("#txtremarks").val();
    var relevant_exp = $("#txt_relevant_exp").val();
    var total_exp = $("#txt_total_exp").val();

    var is_error = false;
    var error_msg = "";

    if (companyid == "0" || companyid == null || companyid == "") {
        error_msg = error_msg + "Please select Company </br>";
        is_error = true;
    }

    if (empid == "0" || empid == null || empid == "") {
        error_msg = error_msg + "Please select Employee </br>";
        is_error = true;
    }

    if (comp_name == "" || comp_name === null) {
        error_msg = error_msg + "Please enter company name </br>";
        is_error = true;
    }

    if (comp_address == "" || comp_address == null) {
        error_msg = error_msg + "Please select Company Address </br>";
        is_error = true;
    }

    if (_doj == "" || _doj == null) {
        error_msg = error_msg + "Please select Date of Joining </br>";
        is_error = true;
    }

    if (_relieve_dt == "" || _relieve_dt == null) {
        error_msg = error_msg + "Please select Date of relieving </br>";
        is_error = true;
    }

    if (is_relieve == "" || is_relieve == null || is_relieve=="0") {
        error_msg = error_msg + "Please select Is Relieve </br>";
        is_error = true;
    }

    if (_desig == "" || _desig == null) {
        error_msg = error_msg + "Please enter Designation </br>";
        is_error = true;
    }

    if (_salary == "" || _salary == null) {
        error_msg = error_msg + "Please enter Salary </br>";
        is_error = true;
    }

    if (_jobtype == "" || _jobtype == null || _jobtype=="0") {
        error_msg = error_msg + "Please select Job Type </br>";
        is_error = true;
    }

    if (_reason == "" || _reason == null) {
        error_msg = error_msg + "Please enter reason </br>";
        is_error = true;
    }

    if (reportingto == "" || reportingto == null || reportingto == "0") {
        error_msg = error_msg + "Please enter Reporting to </br>";
        is_error = true;
    }

    if (rpt_name == "" || rpt_name == null) {
        error_msg = error_msg + "Please enter reporting name </br>";
        is_error = true;
    }

    if (contact == "" || contact == null) {
        error_msg = error_msg + "Please enter contact no</br>";
        is_error = true;
    }

    if (email_ == "" || email_ == null) {
        error_msg = error_msg + "Please enter email </br>";
        is_error = true;
    }


    if (new Date(_relieve_dt) < new Date(_doj)) {
        error_msg = error_msg + "Relieveing date cannot be less than joining date </br>";
        is_error = true;
    }
    if (relevant_exp == "" || relevant_exp == null) {
        error_msg = error_msg + "Please enter relevant experience </br>";
        is_error = true;
    }

    if (total_exp == "" || total_exp == null) {
        error_msg = error_msg + "Please enter total experience </br>";
        is_error = true;
    }

    if (parseFloat(total_exp) < parseFloat(relevant_exp)) {
        error_msg = error_msg + "Total experience cannot be less than relevant experience </br>";
        is_error = true;
    }

    if (is_error) {
        messageBox("error", error_msg);
        return false;
    }

    var myData = {
        current_comp_id: companyid,
        emp_id: empid,
        pr_comp_name: comp_name,
        pr_comp_address: comp_address,
        pr_comp_doj: _doj,
        pr_comp_relieve_dt: _relieve_dt,
        is_relieved: is_relieve,
        designation: _desig,
        salary: _salary,
        job_type: _jobtype,
        relieve_reason: _reason,
        reporting_to: reportingto,
        reporting_name: rpt_name,
        rpting_no: contact,
        rpting_email: email_,
        remarks: remarks,
        created_by: emp_idd,
        relevant_exp: relevant_exp,
        total_exp: total_exp,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiEmployee/Save_Previous_EmployementDetail',
        type: "POST",
        data: JSON.stringify(myData),
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
            else {
                messageBox("error", Msg);
                return false;
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


function GetData(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        // url: ,
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/Get_Previous_EmployementDetail/0/' + employee_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            //// debugger;;
            $("#tbl_emp_pr_employement_dtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return new Date(row.last_modifed_dt) < new Date(row.created_dt)?"-" :GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [11],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "pr_comp_name", "name": "pr_comp_name", "title": "Company Name", "autoWidth": true },
                    { "data": "pr_comp_address", "name": "pr_comp_address", "title": "Company Address", "autoWidth": true },
                    { "data": "pr_comp_doj", "name": "pr_comp_doj", "title": "Date of Joining", "autoWidth": true },
                    { "data": "pr_comp_relieve_dt", "name": "pr_comp_relieve_dt", "title": "Date of Relieving", "autoWidth": true },
                    { "data": "is_relieved", "name":"is_relieved","title":"Is Relieved","autoWidth":true},
                    { "data": "salary", "name": "salary", "title": "Salary", "autoWidth": true },
                    { "data": "rpting_no", "name": "rpting_no", "title": "Contact No", "autoWidth": true },
                    { "data": "rpting_email", "name": "rpting_email", "title": "Email Id", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "last_modifed_dt", "name": "last_modifed_dt", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.emp_pr_employment_id + ',' + full.emp_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
}


function GetEditData(prev_idd, empid) {
    $("#loader").show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/Get_Previous_EmployementDetail/' + prev_idd + '/' + empid,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            BindAllEmp_Company('ddlCompany', emp_idd, res.current_comp_id);
            $("#ddlCompany").attr("disabled", true);
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', res.current_comp_id, res.emp_id);

            //$("#ddlCompany").val();
            //$("#ddlEmployeeCode").val();
            $("#txtprev_comp_name").val(res.pr_comp_name);
            $("#txtprev_comp_address").val(res.pr_comp_address);
            $("#txt_doj").val(GetDateFormatddMMyyyy(new Date(res.pr_comp_doj)));
            $("#txt_relieve").val(GetDateFormatddMMyyyy(new Date(res.pr_comp_relieve_dt)));
            $("#ddl_is_relieve").val(res.is_relieved);
            $("#txtprev_desig").val(res.designation);
            $("#txtprev_salary").val(res.salary);
            $("#ddlpr_jobtype").val(res.job_type);
            $("#txtprev_relieve_reason").val(res.relieve_reason);
            $("#ddl_prev_reporting_to").val(res.reporting_to);
            $("#txtprev_rpt_name").val(res.reporting_name);
            $("#txtprev_contact_no").val(res.rpting_no);
            $("#txtprev_email_id").val(res.rpting_email);
            $("#txtremarks").val(res.remarks);
            $("#hdnid").val(res.emp_pr_employment_id);
            $("#txt_relevant_exp").val(res.relevant_exp);
            $("#txt_total_exp").val(res.total_exp);

            $("#btnsave").hide();
            $("#btnupdate").show();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

$("#btnupdate").bind("click", function () {
    var companyid = $("#ddlCompany").val();
    var empid = $("#ddlEmployeeCode").val();
    var comp_name = $("#txtprev_comp_name").val();
    var comp_address = $("#txtprev_comp_address").val();
    var _doj = $("#txt_doj").val();
    var _relieve_dt = $("#txt_relieve").val();
    var is_relieve = $("#ddl_is_relieve").val();
    var _desig = $("#txtprev_desig").val();
    var _salary = $("#txtprev_salary").val();
    var _jobtype = $("#ddlpr_jobtype").val();
    var _reason = $("#txtprev_relieve_reason").val();
    var reportingto = $("#ddl_prev_reporting_to").val();
    var rpt_name = $("#txtprev_rpt_name").val();
    var contact = $("#txtprev_contact_no").val();
    var email_ = $("#txtprev_email_id").val();
    var remarks = $("#txtremarks").val();
    var relevant_exp = $("#txt_relevant_exp").val();
    var total_exp = $("#txt_total_exp").val();

    var is_error = false;
    var error_msg = "";

    if (companyid == "0" || companyid == null || companyid == "") {
        error_msg = error_msg + "Please select Company </br>";
        is_error = true;
    }

    if (empid == "0" || empid == null || empid == "") {
        error_msg = error_msg + "Please select Employee </br>";
        is_error = true;
    }

    if (comp_name == "" || comp_name === null) {
        error_msg = error_msg + "Please enter company name </br>";
        is_error = true;
    }

    if (comp_address == "" || comp_address == null) {
        error_msg = error_msg + "Please select Company Address </br>";
        is_error = true;
    }

    if (_doj == "" || _doj == null) {
        error_msg = error_msg + "Please select Date of Joining </br>";
        is_error = true;
    }

    if (_relieve_dt == "" || _relieve_dt == null) {
        error_msg = error_msg + "Please select Date of relieving </br>";
        is_error = true;
    }

    if (is_relieve == "" || is_relieve == null || is_relieve == "0") {
        error_msg = error_msg + "Please select Is Relieve </br>";
        is_error = true;
    }

    if (_desig == "" || _desig == null) {
        error_msg = error_msg + "Please enter Designation </br>";
        is_error = true;
    }

    if (_salary == "" || _salary == null) {
        error_msg = error_msg + "Please enter Salary </br>";
        is_error = true;
    }

    if (_jobtype == "" || _jobtype == null || _jobtype == "0") {
        error_msg = error_msg + "Please select Job Type </br>";
        is_error = true;
    }

    if (_reason == "" || _reason == null) {
        error_msg = error_msg + "Please enter reason </br>";
        is_error = true;
    }

    if (reportingto == "" || reportingto == null || reportingto == "0") {
        error_msg = error_msg + "Please enter Reporting to </br>";
        is_error = true;
    }

    if (rpt_name == "" || rpt_name == null) {
        error_msg = error_msg + "Please enter reporting name </br>";
        is_error = true;
    }

    if (contact == "" || contact == null) {
        error_msg = error_msg + "Please enter contact no</br>";
        is_error = true;
    }

    if (email_ == "" || email_ == null) {
        error_msg = error_msg + "Please enter email </br>";
        is_error = true;
    }

    if ($("#hdnid").val() == "" || $("#hdnid").val() == null) {
        error_msg = error_msg + "Something went wrong,please try after sometime </br>";
        is_error = true;
    }

    if (new Date(_relieve_dt) < new Date(_doj)) {
        error_msg = error_msg + "Relieveing date cannot be less than joining date";
        is_error = true;
    }

    if (relevant_exp == "" || relevant_exp == null) {
        error_msg = error_msg + "Please enter relevant experience </br>";
        is_error = true;
    }

    if (total_exp == "" || total_exp == null) {
        error_msg = error_msg + "Please enter total experience </br>";
        is_error = true;
    }

    if (parseFloat(total_exp) < parseFloat(relevant_exp)) {
        error_msg = error_msg + "Total experience cannot be less than relevant experience </br>";
        is_error = true;
    }

    if (is_error) {
        messageBox("error", error_msg);
        return false;
    }

    var myData = {
        emp_pr_employment_id: $("#hdnid").val(),
        current_comp_id: companyid,
        emp_id: empid,
        pr_comp_name: comp_name,
        pr_comp_address: comp_address,
        pr_comp_doj: _doj,
        pr_comp_relieve_dt: _relieve_dt,
        is_relieved: is_relieve,
        designation: _desig,
        salary: _salary,
        job_type: _jobtype,
        relieve_reason: _reason,
        reporting_to: reportingto,
        reporting_name: rpt_name,
        rpting_no: contact,
        rpting_email: email_,
        remarks: remarks,
        last_modified_by: emp_idd,
        relevant_exp: relevant_exp,
        total_exp: total_exp,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiEmployee/Update_Previous_EmployementDetail',
        type: "PUT",
        data: JSON.stringify(myData),
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
            else {
                messageBox("error", Msg);
                return false;
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


function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}
