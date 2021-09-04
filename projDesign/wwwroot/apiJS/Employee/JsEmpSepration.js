$('#loader').show();
var login_emp_id;
var default_company;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        BindOnlyProbation_Confirmed_emp('ddlemployee', default_company, 0);
        BindEmpResignReason('ddlreason', default_company, 0);
        // HaveDisplay = ISDisplayMenu("Display Company List");



        $("#txt_req_reliving_dt").datepicker({
            dateFormat: 'yy-mm-dd',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
                var req_dt = $(this).val();
                // req_dt = req_dt[2] + "-" + req_dt[1] + "-" + req_dt[0];


                var resign_dt = $("#txtresign_dt").val();
                // resign_dt = resign_dt[2] + "-" + resign_dt[1] + "-" + resign_dt[0];

                var days_bw_date = Get_Days_bw_two_dates(resign_dt, req_dt);

                days_bw_date = days_bw_date != "NAN" ? days_bw_date + 1 : 0;

                $("#txt_req_notice_").val(parseInt(days_bw_date));

                $("#txt_notice_diff_days").val(parseInt(days_bw_date) - parseInt($("#txtnoticeperiod_days").val()));
            }
        });


        $('#loader').hide();



        $('#ddlcompany').change(function () {
            //BindEmployeeCodeFromEmpMasterByComp('ddlemployee', $(this).val(), 0);
            clear_all_dt();
            $("#loader").show();
            BindOnlyProbation_Confirmed_emp('ddlemployee', $(this).val(), 0);
            BindEmpResignReason('ddlreason', $(this).val(), 0);
            $("#loader").hide();
        });

        $('#ddlemployee').change(function () {
            clear_all_dt();

            if ($(this).val() == "" || $(this).val() == "0") {
                messageBox("error", "Please Select Employee");
                return false;
            }
            $("#loader").show();
            BindEmployeeDetails($(this).val());
            BindEmpResignReason('ddlreason', $('#ddlcompany').val(), 0);
            $("#loader").hide();
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

    }, 2000);// end timeout

});








function BindEmployeeDetails(emp_id) {

    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select company");
        return false;
    }

    $("#div_emp_resing_dtl").css("display", "block");

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/UserProfile/" + emp_id + "/" + $("#ddlcompany").val(),
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

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



            $("#txtnoticeperiod").val();
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
    // var testt = datediff(parseDate(GetDateFormatddMMyyyy(new Date)), parseDate(after_notice_date));
}


function addMonths(date, months) {
    var d = date.getDate();
    date.setMonth(date.getMonth() + +months);
    if (date.getDate() != d) {
        date.setDate(0);
    }

    return GetDateFormatddMMyyyy(date);
    // return GetDateFormatddMMyyyy(date);
    //return date;
}



function clear_all_dt() {


    //if (HaveDisplay == 0) {
    //    $('#ddlcompany').prop("disabled", "disabled");
    //    BindCompanyList('ddlcompany', default_company);
    //    BindEmployeeCodeFromEmpMasterByComp('ddlemployee', default_company, 0);
    //}
    //else {
    //    BindCompanyList('ddlCompany', 0);
    //}
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

    $("#div_emp_resing_dtl").css("display", "none");
    $("#loader").hide();
    //$("#ddlreason").val();
}

$("#btnreset").bind("click", function () {
    $("#loader").show();
    location.reload();
    $("#loader").hide();
});


//var getDaysInMonth = function (month, year) {
//    // Here January is 1 based
//    //Day 0 is the last day in the previous month
//    return new Date(year, month, 0).getDate();
//    // Here January is 0 based
//    // return new Date(year, month+1, 0).getDate();
//};

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}

$("#btnsave").bind("click", function () {
    var is_error = false;
    var error_msg = "";

    var emp_id = $("#ddlemployee").val();
    var resignation_dt = $("#txtresign_dt").val();
    var req_relieve_dt = $("#txt_req_reliving_dt").val() == "" ? "2000-01-01" : $("#txt_req_reliving_dt").val();
    var req_notice_day = $("#txt_req_notice_").val();
    var diff_notice_days = $("#txt_notice_diff_days").val();
    var policy_relieve_dt = $("#txt_policy_reliving").val();
    var req_reason = $("#ddlreason option:selected").text();
    var req_remarks = $("#txtremarks").val();
    var companyidd = $("#ddlcompany").val();

    if (companyidd == "" || companyidd == null || companyidd == "0") {
        is_error = true;
        error_msg = error_msg + "Please select Company </br>";
    }

    if (emp_id == "" || emp_id == "0") {
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

    if (req_remarks == "" || req_remarks == null) {
        is_error = true;
        error_msg = error_msg + "Please Enter Remarks";
    }

    if (is_error) {
        messageBox("error", error_msg);
        return false;
    }


    var mydata = {
        company_id: companyidd,
        emp_id: emp_id,
        //resignation_dt: resignation_dt,
        req_relieving_date: (req_relieve_dt == "" || req_relieve_dt == null) ? policy_relieve_dt : req_relieve_dt,
        req_notice_days: (req_notice_day == "" || req_notice_day == null || req_notice_day == "NaN") ? $("#txtnoticeperiod_days").val() : req_notice_day,
        diff_notice_days: (req_notice_day == "" || req_notice_day == null || req_notice_day == "NaN" || req_notice_day == 0) ? 0 : diff_notice_days,
        // policy_relieving_dt: policy_relieve_dt,
        req_reason: req_reason == "Other" ? $("#txtotherreason").val() : req_reason,
        req_remarks: req_remarks,
        created_by: login_emp_id,

    };
    $("#loader").show();

    if (confirm("Do you want to process this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();


        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Save_Employee_Separation",
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
                    alert(msg);
                    location.reload();
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