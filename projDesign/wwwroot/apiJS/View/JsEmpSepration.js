$('#loader').show();
var login_emp_id;
var default_company;
var HaveDisplay;
var arr = new Array("1", "101", "105");
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, 0);
        setSelect('ddlcompany', default_company);
        BindEmployeeListUnderLoginEmpFromAllCompForEsep('ddlemployee', default_company, login_emp_id, 0);
        setSelect('ddlemployee', login_emp_id);
        BindEmpResignReason('ddlreason', default_company, 0);
        BindEmployeeDetails(login_emp_id);

        if (!arr.includes(login_role_id)) {
            $("#divFUP").hide();
            debugger;
            $("#ddlemployee option[value!='" + login_emp_id + "']").remove();
        }

        $("#txt_req_reliving_dt").change(function () {
            var req_dt = $(this).val();
            var resign_dt = $("#txtresign_dt").val();

            if (Date.parse(resign_dt) > Date.parse(req_dt)) {
                $(this).val('');
                messageBox("error", "Requested Last working date should be greater than Resignation date");
                return false;
            }

            var days_bw_date = Get_Days_bw_two_dates(resign_dt, req_dt);

            days_bw_date = days_bw_date != "NAN" ? days_bw_date + 1 : 0;

            $("#txt_req_notice_").val(parseInt(days_bw_date));

            $("#txt_notice_diff_days").val(parseInt(days_bw_date) - parseInt($("#txtnoticeperiod_days").val()));
        });

        $("#ddlcompany").change(function () {
            clear_all_dt();
            if ($(this).val() == 0) {

                $("#ddlemployee").empty();
                $("#ddlreason").empty();

                return false;
            }
            BindEmployeeListUnderLoginEmpFromAllCompForEsep('ddlemployee', $(this).val(), login_emp_id, 0);
            setSelect('ddlemployee', login_emp_id);

            var exists = $('#ddlemployee option').filter(function () { return $(this).val() == login_emp_id; }).length;

            if (exists != undefined && exists != null && exists != 0) {

                BindEmpResignReason('ddlreason', $(this).val(), 0);
                BindEmployeeDetails(login_emp_id);
            }

            // BindEmpResignReason('ddlreason', $(this).val(), 0);
            //BindEmployeeDetails(login_emp_id);
        });

        $('#loader').hide();

        $('#ddlemployee').change(function () {
            clear_all_dt();

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                messageBox("error", "Please select Company");
                return false;
            }

            if ($(this).val() == "" || $(this).val() == "0") {
                messageBox("error", "Please Select Employee");
                return false;
            }

            BindEmployeeDetails($(this).val());
            BindEmpResignReason('ddlreason', $("#ddlcompany").val(), 0);

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

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });

        $("#btnsave").bind("click", function () {
            var is_error = false;
            var error_msg = "";

            var fileUpload = $("#txtfupDoc").get(0);
            var _files = fileUpload.files;


            var formData = new FormData();
            if (_files.length > 0)
                for (var i = 0; i < _files.length; i++) {

                    var n = _files[i].name.split('.')[1].toLowerCase();
                    if (_files[i].name.split('.')[1].toLowerCase() != 'pdf' && _files[i].name.split('.')[1].toLowerCase() != 'jpg' && _files[i].name.split('.')[1].toLowerCase() != 'jpeg' && _files[i].name.split('.')[1].toLowerCase() != 'png') {
                        messageBox("error", "Please select either image or pdf file only....");
                        return false;
                    }
                    formData.append('file' + i + '', _files[i]);
                }
            var companyid = $("#ddlcompany").val();
            var emp_id = $("#ddlemployee").val();
            var resignation_dt = $("#txtresign_dt").val();
            var req_relieve_dt = $("#txt_req_reliving_dt").val() == "" ? "2000-01-01" : $("#txt_req_reliving_dt").val();
            var req_notice_day = $("#txt_req_notice_").val();
            var diff_notice_days = $("#txt_notice_diff_days").val();
            var policy_relieve_dt = $("#txt_policy_reliving").val();
            var req_reason = $("#ddlreason option:selected").text();
            var req_remarks = $("#txtremarks").val();

            if (companyid == "" || companyid == "0" || companyid == null) {
                messageBox("error", "Please select company");
                return false;
            }

            if (emp_id == "" || emp_id == "0" || emp_id == null) {
                is_error = true;
                error_msg = error_msg + "Please Select Employee</br>";
            }

            if (req_reason == " --Please Select-- ") {
                is_error = true;
                error_msg = error_msg + "Please Select Reason</br>";
            }
            else {
                if (req_reason == "Other") {
                    if ($("#txtotherreason").val() == "" || $("#txtotherreason").val() == null) {
                        is_error = true;
                        error_msg = error_msg + "Please Enter Reason";
                    }
                }
            }

            if (req_relieve_dt == "" || req_relieve_dt == null || req_relieve_dt == "2000-01-01") {
                is_error = true;
                error_msg = error_msg + "Please Enter Requested Last Working Date";
            }

            if ($("#txtempstatus").val().toLowerCase() == 'fnf' || $("#txtempstatus").val().toLowerCase() == 'notice' || $("#txtempstatus").val() == 'separated') {
                alert('Resignation application can not be raised for already resigned employee...');
                return false;
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var mydata = {
                emp_id: emp_id,
                //resignation_dt: resignation_dt,
                req_relieving_date1: (req_relieve_dt == "" || req_relieve_dt == null) ? $("#txtresign_dt").val() : req_relieve_dt,
                req_notice_days: (req_notice_day == "" || req_notice_day == null || req_notice_day == "NaN") ? 0 : req_notice_day,
                diff_notice_days: (req_notice_day == "" || req_notice_day == null || req_notice_day == "NaN" || req_notice_day == 0) ? $("#txtnoticeperiod_days").val() : diff_notice_days,
                // policy_relieving_dt: policy_relieve_dt,
                req_reason: req_reason == "Other" ? $("#txtotherreason").val() : req_reason,
                req_remarks: req_remarks,
                created_by: login_emp_id,
                company_id: companyid,
            };

            var Obj = JSON.stringify(mydata);
            formData.append('AllData', Obj);

            $("#loader").show();

            if (confirm("Do you want to process this?")) {

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiEmployee/Save_Employee_Separation/",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: formData,
                    processData: false,  // tell jQuery not to process the data
                    contentType: false,  // tell jQuery not to set contentType
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
                            alert(msg);
                            window.location.href = "/View/EmpSeprationDetail";
                            //alert(msg);
                            //location.reload();
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







function BindEmployeeDetails(emp_id) {

    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select company");
        return false;
    }

    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/UserProfile/" + emp_id + "/" + $("#ddlcompany").val(),
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode != undefined && res.statusMessage != undefined) {
                messageBox("info", res.statusMessage);
                $("#loader").hide();
                return false;
            }
            if (response[0] != undefined) {
                $("#div_emp_resing_dtl").css("display", "block");

                $("#txtdob").val(GetDateFormatddMMyyyy(new Date(res[0].dob_)));
                $("#txtdoj").val(GetDateFormatddMMyyyy(new Date(res[0].dobj_)));
                $("#txtdept").val(res[0].dept_name);
                $("#txtdesignation").val(res[0].designation_name);
                $("#txtgrade").val(res[0].grade_name);
                $("#txtloc").val(res[0].loc_name);
                $("#txtempstatus").val(res[0].current_emp_status);
                $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date));
                $("#txtnoticeperiod_days").val(res[0].required_notice_period_);
                var dayss_ = res[0].required_notice_period_ != "0" && res[0].required_notice_period_ != null ? parseInt(res[0].required_notice_period_) - 1 : 1;

                $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(new Date().getTime() + (dayss_ * 24 * 60 * 60 * 1000))));
                if (res[0].current_emp_status.toLowerCase() == 'fnf' || res[0].current_emp_status.toLowerCase() == 'notice' || res[0].current_emp_status.toLowerCase() == 'separated') {
                    $("#btnsave").attr('disabled', true);
                    messageBox("info", "Employee is already on Notice or Separated...");
                }

                debugger;
                if (response[0].emp_sep_Detail != null) {
                    var d = response[0].emp_sep_Detail;
                    $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date(d.resignation_dt)));
                    $("#txt_req_reliving_dt").val(GetDateFormatddMMyyyy(new Date(d.req_relieving_date)));
                    $("#txt_req_notice_").val(d.req_notice_days);
                    $("#txt_notice_diff_days").val(d.diff_notice_days);
                    $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(d.policy_relieving_dt)));
                    $("#txtremarks").val(d.req_remarks);
                    $("#ddlreason option:contains(" + d.req_reason + ")").attr("selected", "selected");
                    $("#ddlreason").val($("#ddlreason").val());
                    $("#ddlreason").trigger("select2:updated");
                    $("#ddlreason").select2();
                    $("#btnsave").text('Update');
                }
                else {
                    $("#btnsave").text('Save');
                }

                $("#txtnoticeperiod").val();


            }
            $("#loader").hide();
            //$("#txtremarks").val();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function Get_Days_bw_two_dates(start_dt, end_dt) {

    start_dt = new Date(start_dt);
    end_dt = new Date(end_dt);
    var millisecondsPerDay = 1000 * 60 * 60 * 24;

    var millisBetween = start_dt.getTime() - end_dt.getTime();
    var days = millisBetween / millisecondsPerDay;


    var days_btw_month = Math.abs(Math.floor(days));

    return days_btw_month;
}

function policy_notice_period(notice_period_dtl) {

    var current_month = (new Date).getMonth() + 1;

    current_month = current_month <= 9 ? "0" + current_month : current_month;

    var after_notice_date = addMonths(new Date(), ((notice_period_dtl.notice_period != "" && notice_period_dtl.notice_period != null) ? notice_period_dtl.notice_period : 0));

    var days_btw_month = Get_Days_bw_two_dates(GetDateFormatddMMyyyy(new Date()), after_notice_date);

    var total_days = parseInt(days_btw_month) + parseInt(notice_period_dtl.notice_period_days);

    return total_days;

}

function addMonths(date, months) {
    var d = date.getDate();
    date.setMonth(date.getMonth() + +months);
    if (date.getDate() != d) {
        date.setDate(0);
    }

    return GetDateFormatyyyyMMdd(date);

}

function clear_all_dt() {
    $("#loader").show();
    $("#txtdob").val('');
    $("#txtdoj").val('');
    $("#txtdept").val('');
    $("#txtdesignation").val('');
    $("#txtgrade").val('');
    $("#txtloc").val('');
    $("#txtempstatus").val('');
    $("#txtnoticeperiod_month").val('');
    $("#txtnoticeperiod_days").val('');
    $("#txtresign_dt").val('');
    $("#txt_req_reliving_dt").val('');
    $("#txt_req_notice_").val('');
    $("#txt_notice_diff_days").val('');
    $("#txt_policy_reliving").val('');
    $("#hdn_policy_notice_total_days").val('');
    $("#txtremarks").val('');
    $("#txtfupDoc").val('');

    $("#div_emp_resing_dtl").css("display", "none");
    $("#loader").hide();

    //$("#ddlreason").val();
}

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}

