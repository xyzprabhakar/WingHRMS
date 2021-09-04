var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {



        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_emp_id, login_company_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', login_company_id, login_emp_id);
        //GetData(login_company_id);

        BindEmpResignReason('ddlreason', login_company_id, 0);
        //BindWithdrawalType('ddlwithdrawaltype', login_company_id, 0);

        $('#btnupdate').hide();
        $('#btnsave').show();

        GetData(login_company_id);

        $("#ddlCompany").bind("change", function () {

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            setSelect('ddlEmployeeCode', login_emp_id);

            BindEmpResignReason('ddlreason', $(this).val(), 0);
            //BindWithdrawalType('ddlwithdrawaltype', $(this).val(), 0);

            if ($(this).val() > 0) {
                GetData($(this).val());
            }
            else {
                if ($.fn.DataTable.isDataTable('#tbl_withdrawal_dtl')) {
                    $('#tbl_withdrawal_dtl').DataTable().clear().draw();
                }
            }

            //GetData(login_company_id);

        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#txtresign_dt").bind("change", function () {
            if ($("#txt_last_working_dt").val() == null || $("#txt_last_working_dt").val() == "") {
                $("#txt_notice_days").val('');
            }
            else {
                if ($(this).val() == null || $(this).val() == "") {
                    $("#txt_notice_days").val('');
                }
                else {

                    if (new Date($(this).val()) > new Date($("#txt_last_working_dt").val())) {
                        messageBox("error", "Resignation Date must be less than Last working Date");
                        $("#txtresign_dt").val('').focus();
                        $("#txt_notice_days").val('');
                        return false;
                    }
                    var days_bw_date = Get_Days_bw_two_dates($(this).val(), $("#txt_last_working_dt").val());

                    days_bw_date = days_bw_date != "NAN" ? days_bw_date + 1 : 0;

                    $("#txt_notice_days").val(parseInt(days_bw_date));
                }

            }
        });


        $("#txt_last_working_dt").bind("change", function () {
            var resign_ = $("#txtresign_dt").val();
            var last_dt = $(this).val();
            if (new Date(last_dt) < new Date(resign_)) {
                messageBox("error", "Last working date must be greater than resignation date");
                $("#txt_last_working_dt").val('').focus();
                return false;
            }

            if (resign_ == null || resign_ == "") {
                $("#txt_notice_days").val('');
                return false;
            }

            var days_bw_date = Get_Days_bw_two_dates(resign_, last_dt);

            days_bw_date = days_bw_date != "NAN" ? days_bw_date + 1 : 0;

            $("#txt_notice_days").val(parseInt(days_bw_date));
        });

        $("#ddlreason").bind("change", function () {
            $("#loader").show();
            if ($(this).find("option:selected").text() == "Other") {
                $("#other_reason_div").css("display", "block");
            }
            else {
                $("#other_reason_div").css("display", "none");
            }
            $("#loader").hide();
        });

        //$("#ddlwithdrawaltype").bind("change", function () {
        //    $("#loader").show();
        //    if ($(this).find("option:selected").text() == "Other") {
        //        $("#divotherwithdrawaltype").css("display", "block");
        //    }
        //    else {
        //        $("#divotherwithdrawaltype").css("display", "none");
        //    }
        //    $("#loader").hide();
        //});

        $("#btnsave").bind("click", function () {
            var is_error = false;
            var error_msg = "";

            var companyid = $("#ddlCompany").val();
            var empid = $("#ddlEmployeeCode").val();
            var resign_dt = $("#txtresign_dt").val();
            var last_working_dt = $("#txt_last_working_dt").val();
            var notice_ = $("#txt_notice_days").val();
            var reason = $("#ddlreason option:selected").text();
            var salary_process_type = $("#ddlprocesstype").val();
            var isNoDueReq = $("#chkNoDueRequired").is(':checked');//$('input[id="#chkNoDueRequired"]:checked');
            var isKTRequired = $("#chkKTReq").is(':checked');// $('input[id="#chkKTReq"]:checked');
            //var withdrawal_type = $("#ddlwithdrawaltype").val();
            //var gratuity = $("#gratuity").prop("checked") == true ? 1 : 0;
            var remarks = $("#txtremarks").val();

            if (isNoDueReq == true)
                isNoDueReq = 1;
            else isNoDueReq = 0;
            if (isKTRequired == true)
                isKTRequired = 1;
            else isKTRequired = 0;

            if (companyid == null || companyid == "" || companyid == "0" || companyid == undefined) {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (empid == null || empid == "" || empid == "0" || empid == undefined) {
                is_error = true;
                error_msg = error_msg + "Please select Employee</br>";
            }

            if (resign_dt == null || resign_dt == "") {
                is_error = true;
                error_msg = error_msg + "Please select resign date</br>";
            }

            if (last_working_dt == null || last_working_dt == "") {
                is_error = true;
                error_msg = error_msg + "Please select Last working date</br>";
            }

            if (notice_ == null || notice_ == "" || notice_ == NaN) {
                is_error = true;
                error_msg = error_msg + "Invalid Notice Period</br>";
            }

            if (reason == null || reason == "" || reason == "0") {
                is_error = true;
                error_msg = error_msg + "Please select reason</br>";
            }
            else {
                if (reason == "Other" || reason == "Others") {
                    if ($("#txtotherreason").val() == null || $("#txtotherreason").val() == "") {
                        is_error = true;
                        error_msg = error_msg + "Please enter reason</br>";
                    }
                }
            }

            //if (salary_process_type == null || salary_process_type == "" || salary_process_type == "0") {
            //    is_error = true;
            //    error_msg = error_msg + "Please select Salary Process Type</br>";
            //}

            //if (withdrawal_type == null || withdrawal_type == "" || withdrawal_type == "0") {
            //    is_error = true;
            //    error_msg = error_msg + "Please select Withdrawal Type</br>";
            //}
            //else {
            //    if (withdrawal_type == "Other" || withdrawal_type == "Others") {
            //        if ($("#txtother_withdrawal").val() == null || $("#txtother_withdrawal").val() == "") {
            //            is_error = true;
            //            error_msg = error_msg + "Please enter other withdrawal type</br>";
            //        }

            //    }
            //}

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            $("#loader").show();

            if (confirm("Do you want to process this?")) {


                var mydata = {
                    company_id: companyid,
                    emp_id: empid,
                    resignation_dt: resign_dt,
                    last_wrking_dt: last_working_dt,
                    notice_day: notice_,
                    req_reason: (reason == "Other" || reason == "Others") ? $("#txtotherreason").val() : reason,
                    //withdrawal_type: (withdrawal_type == "Other" || withdrawal_type == "Others") ? $("#txtother_withdrawal").val() : withdrawal_type,
                    salary_process_type: salary_process_type,
                    //  gratuity: gratuity,
                    is_no_due_cleared: isNoDueReq,
                    is_kt_transfered: isKTRequired,
                    req_remarks: remarks,

                };

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiEmployee/Save_Emp_Withdrawal",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var res = response;
                        var statuscode = res.statusCode;
                        var msg = res.message;
                        $("#loader").hide();
                        if (statuscode != "0") {
                            messageBox("error", msg);
                            return false;
                        }
                        else {
                            messageBox("success", msg);
                            GetData(companyid);
                            clear_all_dt();
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
                        return false;
                    }


                });


            }
            else {
                $("#loader").hide();
                return false;
            }
        });

        $("#btnupdate").bind("click", function () {
            var is_error = false;
            var error_msg = "";

            var sepration_id = $("#hdn_spr_id").val();
            var companyid = $("#ddlCompany").val();
            var empid = $("#ddlEmployeeCode").val();
            var resign_dt = $("#txtresign_dt").val();
            var last_working_dt = $("#txt_last_working_dt").val();
            var notice_ = $("#txt_notice_days").val();
            var reason = $("#ddlreason option:selected").text();
            var salary_process_type = $("#ddlprocesstype").val();
            var isNoDueReq = $("#chkNoDueRequired").is(':checked');//$('input[id="#chkNoDueRequired"]:checked');
            var isKTRequired = $("#chkKTReq").is(':checked');// $('input[id="#chkKTReq"]:checked');
            //var withdrawal_type = $("#ddlwithdrawaltype").val();
            //var gratuity = $("#gratuity").prop("checked") == true ? 1 : 0;
            var remarks = $("#txtremarks").val();

            if (isNoDueReq == true)
                isNoDueReq = 1;
            else isNoDueReq = 0;
            if (isKTRequired == true)
                isKTRequired = 1;
            else isKTRequired = 0;

            if (companyid == null || companyid == "" || companyid == "0" || companyid == undefined) {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (empid == null || empid == "" || empid == "0" || empid == undefined) {
                is_error = true;
                error_msg = error_msg + "Please select Employee</br>";
            }

            if (resign_dt == null || resign_dt == "") {
                is_error = true;
                error_msg = error_msg + "Please select resign date</br>";
            }

            if (last_working_dt == null || last_working_dt == "") {
                is_error = true;
                error_msg = error_msg + "Please select Last working date</br>";
            }

            if (notice_ == null || notice_ == "" || notice_ == NaN) {
                is_error = true;
                error_msg = error_msg + "Invalid Notice Period</br>";
            }

            if (reason == null || reason == "" || reason == "0") {
                is_error = true;
                error_msg = error_msg + "Please select reason</br>";
            }
            else {
                if (reason == "Other" || reason == "Others") {
                    if ($("#txtotherreason").val() == null || $("#txtotherreason").val() == "") {
                        is_error = true;
                        error_msg = error_msg + "Please enter reason</br>";
                    }
                }
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            $("#loader").show();

            if (confirm("Do you want to process this?")) {
                $("btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

                var mydata = {
                    sepration_id: sepration_id,
                    company_id: companyid,
                    emp_id: empid,
                    resignation_dt: resign_dt,
                    last_wrking_dt: last_working_dt,
                    notice_day: notice_,
                    req_reason: (reason == "Other" || reason == "Others") ? $("#txtotherreason").val() : reason,
                    //withdrawal_type: (withdrawal_type == "Other" || withdrawal_type == "Others") ? $("#txtother_withdrawal").val() : withdrawal_type,
                    salary_process_type: salary_process_type,
                    //  gratuity: gratuity,
                    is_no_due_cleared: isNoDueReq,
                    is_kt_transfered: isKTRequired,
                    req_remarks: remarks,

                };

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiEmployee/Update_Emp_Withdrawal",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var res = response;
                        var statuscode = res.statusCode;
                        var msg = res.message;
                        $("#loader").hide();

                        if (statuscode != "0") {
                            messageBox("error", msg);
                            return false;
                        }
                        else {
                            messageBox("success", msg);
                            $('#btnupdate').hide();
                            $('#btnsave').show();
                            GetData(companyid);
                            clear_all_dt();
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
                        return false;
                    }


                });


            }
            else {
                $("#loader").hide();
                return false;
            }
        });

    }, 2000);// end timeout


});




function Get_Days_bw_two_dates(start_dt, end_dt) {

    start_dt = new Date(start_dt);
    end_dt = new Date(end_dt);

    var millisecondsPerDay = 1000 * 60 * 60 * 24;

    var millisBetween = start_dt.getTime() - end_dt.getTime();
    var days = millisBetween / millisecondsPerDay;


    var days_btw_month = Math.abs(Math.floor(days));

    return days_btw_month;
}



function clear_all_dt() {

    $("#loader").show();
    BindAllEmp_Company('ddlCompany', login_emp_id, login_company_id);
    BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', login_company_id, login_emp_id);
    BindEmpResignReason('ddlreason', login_company_id, 0);
    //BindWithdrawalType('ddlwithdrawaltype', login_company_id, 0);
    $("#txtresign_dt").val('');
    $("#txt_last_working_dt").val('');
    $("#txt_notice_days").val('');
    $("#ddlprocesstype").val('0');
    //$("#gratuity").prop("checked", false);
    $("#txtremarks").val('');

    $("#other_reason_div").css("display", "none");
    $("#divotherwithdrawaltype").css("display", "none");
    $("#loader").hide();
}

function GetData(companyid) {

    if ($.fn.DataTable.isDataTable('#tbl_withdrawal_dtl')) {
        $('#tbl_withdrawal_dtl').DataTable().clear().draw();
    }

    $("#loader").show();

    $('#tbl_withdrawal_dtl').DataTable({
        "processing": true,
        "serverSide": false,
        "bDestroy": true,
        "orderMulti": false,
        "filter": true,
        "scrollX": 200,
        ajax: {
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmpWithdrawal/" + companyid,
            type: 'GET',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            dataSrc: function (json) {
                $("#loader").hide();
                if (json.statusCode != undefined) {
                    messageBox("error", json.message);
                    return false;
                }
                return json;
            },
            error: function (err) {
                $("#loader").hide();
                messageBox("error", err.responseText);
                return false;
            }
        },
        "columnDefs": [
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
        ],
        columns: [
            { "data": null, "title": "S.No.", "autoWidth": true },
            { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
            { "data": "emp_name", "title": "Employee Name", "autoWidth": true },
            { "data": "resignation_dt", "title": "Resign Date", "autoWidth": true },
            { "data": "last_wrking_dt", "title": "Last Working Date", "autoWidth": true },
            { "data": "notice_day", "title": "Notice Day", "autowidth": true },
            { "data": "req_reason", "title": "Reason", "autoWidth": true },
            //{ "data": "withdrawal_type", "title": "Withdrawal Type", "autoWidth": true }, 
            { "data": "salary_process_type", "title": "Salary Process Type", "autoWidth": true },
            //{ "data": "gratuity", "title": "Gratuity", "autoWidth": true },
            { "data": "req_remarks", "title": "Rremarks", "autoWidth": true },
            {
                "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<a href="#" onclick="DeleteData(' + full.emp_id + ',' + full.sepration_id + ')" ><i class="fa fa-trash"></i></a>';
                }
            },
            {
                "title": "Action", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return '<a href="#" onclick="GetEditData(' + full.emp_id + ',' + full.sepration_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                }
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
    });
}

function DeleteData(empid, idd) {
    $("#loader").show();

    if (confirm("Do you want to process this?")) {

        var mydata = {
            sepration_id: idd,
            emp_id: empid
        };

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Delete_Emp_Withdrawal",
            type: "Delete",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(mydata),
            headers: headerss,
            success: function (response) {
                _GUID_New();
                var res = response;
                var statuscode = res.statusCode;
                var msg = res.message;
                $("#loader").hide();
                if (statuscode != "0") {
                    messageBox("error", msg);
                    return false;
                }
                else {
                    messageBox("success", msg);
                    GetData(login_company_id);
                    clear_all_dt();
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
                return false;
            }


        });


    }
    else {
        $("#loader").hide();
        return false;
    }
}

function GetEditData(empid, idd) {
     debugger;;
    $('#loader').show();
 
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    var apiurl = localStorage.getItem("ApiUrl") + "apiEmployee/Get_Emp_Withdrawal_details/" + idd + "/" + empid;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            data = res;

            BindCompanyListAll('ddlcompany', login_emp_id, data.company_id);

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', data.company_id, data.emp_id);

            var resignation_date = new Date(data.resignation_dt);
            $("#txtresign_dt").val(GetDateFormatddMMyyyy(resignation_date));

            var last_wrkng_date = new Date(data.last_wrking_dt);
            $("#txt_last_working_dt").val(GetDateFormatddMMyyyy(last_wrkng_date));
            $("#hdn_spr_id").val(data.sepration_id);
            $("#txt_notice_days").val(data.req_notice_days);
            $("#ddlprocesstype").val(data.salary_process_type);
            debugger;
            BindEmpResignReason('ddlreason', login_company_id, data.req_reason);
            // $("#ddlreason").val(data.req_reason);
            if (data.req_reason == "Other") {
                $("#other_reason_div").css("display", "block");
            }
            else
            {
                $("#other_reason_div").css("display", "none");
            }
            $("#txtremarks").val(data.req_remarks);

            if (data.is_kt_transfered == 1) {
                $("input[id=chkKTReq][value=" + data.is_active + "]").prop('checked', true);
            }
            else {
                $("input[id=chkKTReq][value=" + data.is_active + "]").prop('checked', false);
            }
            
            if (data.is_no_due_cleared == 1) {
                $("input[id=chkNoDueRequired][value=" + data.is_active + "]").prop('checked', true);
            }
            else {
                $("input[id=chkNoDueRequired][value=" + data.is_active + "]").prop('checked', false);
            }

            $("#ddlreason").val(data.req_reason);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}




