var api_urls;
var login_emp_id;
var emp_role_id;
var company_idd;
var user_id;
$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = localStorage.getItem("emp_id");
        login_emp_id = CryptoJS.AES.decrypt(login_emp_id, localStorage.getItem("sit_id"));
        login_emp_id = login_emp_id.toString(CryptoJS.enc.Utf8);
        login_emp_id = login_emp_id.replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //api_urls = localStorage.getItem("ApiUrl");
        //api_urls = CryptoJS.AES.decrypt(api_urls, localStorage.getItem("sit_id"));
        //api_urls = api_urls.toString(CryptoJS.enc.Utf8);
        //api_urls = api_urls.replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

        var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";


        var newlogin = CryptoJS.AES.decrypt(localStorage.getItem("_firsttimelogin"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //var emp_role_id = localStorage.getItem("emp_role_id");
        //Bind_PendingRequestDtl(login_emp_id, company_idd, emp_role_id);
        // Bind_Employee_Dtl(login_emp_id);
        Bind_LogDtl();
        Bind_Event();
        Bind_CurrentOpenings();
        Bind_policies();
        // Bind_PieChart(login_emp_id);


        BindEmployeeDtlFromEmpMasterByCompID(company_idd, login_emp_id);
        BindEmployee_Birthday_ByCompID(company_idd, login_emp_id);
        BindEmployee_Annivery_ByCompID(company_idd, login_emp_id);

        // Bind_PieChartByCompany(company_idd, login_emp_id);

        if (newlogin == 1) {
            // $("#myAssetReqModal").show();
            var modal = document.getElementById("myFirstLoginModal");
            // modal.style.display = "block";
            $('#myFirstLoginModal').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });
            //$('#myFirstLoginModal').dialog({
            //    modal: 'true',
            //    title: 'Change Password'
            //}); 

            $("#txtloginuserid_").val(login_name_code);
        }
        else {
            var modal = document.getElementById("myFirstLoginModal");

            if (modal != undefined && modal != null) {
                $('#myFirstLoginModal').modal({
                    show: false
                });
            }

        }


        $('#chngpwdclosepop').on('click', function (e) {
            window.location.href = '/Login';
            return;
        });
    
    }, 2000); // End timeout
});

function BindEmployee_Birthday_ByCompID(CompanyId, LoginEmpID) {
    // debugger;
    var emp_lst_under_login_emp;

    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Employee_Birthday_ByCompID/" + CompanyId + "/" + LoginEmpID,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            emp_lst_under_login_emp = res;
            // localStorage.setItem("emp_under_login_emp", res);

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });

    //  }

    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
        var empimg = "";
        for (var i = 0; i < emp_lst_under_login_emp.length; i++) {

            empimg = emp_lst_under_login_emp[i].emp_img == null ? "" : emp_lst_under_login_emp[i].emp_img;
            if (empimg == "") {
                empimg = "EmployeeImage/DefaultUser/defaultimage.jpg";
            }

            $("#tbl_emp_birthdaylist").append('<tr class="marquee"><td><img src="' + localStorage.getItem("ApiUrl").replace("api/", "") + empimg + '" style="width:80;height:80" /></td><td>' + emp_lst_under_login_emp[i].emp_name_code + '</td><td>' + emp_lst_under_login_emp[i].dept_name + '</td><td>' + GetDateFormatddMMyyyy(new Date(emp_lst_under_login_emp[i].dob)).slice(0, -5) + '</td></tr>');
        }

    }
    else {
        $("#tbl_emp_birthdaylist").append('<tr class="marquee"><td></td><td colspan="2">No Record Found...</td><td></td></tr>');
    }
}

function BindEmployee_Annivery_ByCompID(CompanyId, LoginEmpID) {

    var emp_lst_under_login_emp;

    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Employee_Annivery_ByCompID/" + CompanyId + "/" + LoginEmpID,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            emp_lst_under_login_emp = res;
            // localStorage.setItem("emp_under_login_emp", res);

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });

    //  }

    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
        var empimg = "";
        for (var i = 0; i < emp_lst_under_login_emp.length; i++) {
            empimg = emp_lst_under_login_emp[i].emp_img == null ? "" : emp_lst_under_login_emp[i].emp_img;
            if (empimg == "") {
                empimg = "EmployeeImage/DefaultUser/defaultimage.jpg";
            }
            $("#tbl_emp_anniversarylist").append('<tr class="marquee"><td><img src="' + localStorage.getItem("ApiUrl").replace("api/", "") + empimg + '" style="width:80;height:80" /></td><td>' + emp_lst_under_login_emp[i].emp_name_code + '</td><td>' + emp_lst_under_login_emp[i].dept_name + '</td><td>' + GetDateFormatddMMyyyy(new Date(emp_lst_under_login_emp[i].doanv)).slice(0, -5) + '</td></tr>');
        }
    }
    else {
        $("#tbl_emp_anniversarylist").append('<tr class="marquee"><td></td><td colspan="2">No Record Found...</td><td></td></tr>');
    }
}


//function Bind_PendingRequestDtl(loginempid, company_idd, emp_role_id) {

//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetAll_PendingRequetsCount/" + loginempid + "/" + company_idd + "/" + emp_role_id,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {

//            var res = response;

//            $('#lblleaver_rqst').html('<i class="fas fa-calendar-alt"></i>' + ' ' + (response != null && response != "" ? response.leave_pending : ''));
//            $('#lblattandance_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.attendance_pending : ''));
//            $('#lbloutdoor_rqst').html('<i class="fa fa-history"></i>' + ' ' + (response != null && response != "" ? response.outdoor_pending : ''));
//            $('#lblcompoff_rqst').html('<i class="fas fa-recycle"></i>' + ' ' + (response != null && response != "" ? response.compoff_pending : ''));
//            $('#lblloan_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.total_loan_request : ''));
//            // $('#lblreimbursment_rqst').html('<i class="fas fa-clock"></i>' + response.total_reimbursment_reqst);
//            $('#lblasset_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.asset_request : ''));

//            // $("#lblleaver_rqst").text(response.leave_pending);
//        },
//        error: function (response) {

//            alert('Your session has expired please login again...!');
//            window.location.href = "~/Login";
//            return false;
//        }
//    });
//}

//function Bind_Employee_Dtl(loginemp_id) {

//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiMasters/Get_Employee_Under_LoginEmp/" + loginemp_id,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;
//            if (res.length > 0) {
//                for (var i = 0; i < res.length; i++) {
//                    $("#tblemplistt").append('<tr><td>' + res[i].empcode + '</td><td>' + res[i].empname + '</td></tr>');
//                }
//            }
//        },
//        error: function (response) {
//            alert(response);
//        }
//    });
//}

function Bind_LogDtl() {
    //debugger;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/Get_Logs_Detail/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            var html = "";
            if (res.daily_attandance_last_processdt != null) {
                html += "<tr><td>Data Process</td><td>" + (res != null && res != "" ? (GetDateFormatddMMyyyy(new Date(res.daily_attandance_last_processdt.last_process_dt))) : '') + "</td><td>" + (res != null && res != "" ? (GetTimeFromDate(new Date(res.daily_attandance_last_processdt.last_process_dt))) : '') + "</td></tr>";
            }

            if (res.emp_last_modified_record != null) {
                html += "<tr><td>Employee Modify</td><td>" + (res != null && res != "" ? (GetDateFormatddMMyyyy(new Date(res.emp_last_modified_record.created_date))) : '') + "</td><td>" + (res != null && res != "" ? (GetTimeFromDate(new Date(res.emp_last_modified_record.created_date))) : '') + "</td></tr>";
            }
            if (res.last_entytime_attandancedtl != null) {
                html += "<tr><td>Import Data</td><td>" + (res != null && res != "" ? (GetDateFormatddMMyyyy(new Date(res.last_entytime_attandancedtl.entry_time))) : '') + "</td><td>" + (res != null && res != "" ? (GetTimeFromDate(new Date(res.last_entytime_attandancedtl.entry_time))) : '') + "</td></tr>";
            }
            $("#tbllogdtl").append(html);
            //$("#tbllogdtl").append("<tr><td>Data Process</td><td>" + res.daily_attandance_last_processdt.last_process_dt.split('T')[0] + "</td><td>" + res.daily_attandance_last_processdt.last_process_dt.split('T')[1] + "</td></tr><tr><td>Employee Modify</td><td>" + res.emp_last_modified_record.last_modified_date.split('T')[0] + "</td><td>" + res.emp_last_modified_record.last_modified_date.split('T')[1] + "</td></tr><tr><td>Import Data</td><td>" + res.last_entytime_attandancedtl.entry_time.split('T')[0] + "</td><td>" + res.last_entytime_attandancedtl.entry_time.split('T')[1] + "</td></tr>");


        },
        error: function (err, exception) {

            alert(err);
        }
    });
}

function OpenDocumentt(path) {
    debugger;
    window.open(path);
}


function GetDateFormatMMddyyyy(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return month + '/' + day + '/' + date.getFullYear();
}

function Bind_Event() {

    //  var company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    $.ajax({
        //url: localStorage.getItem("ApiUrl") + "/apiMasters/Get_Event_by_id/-1/" + company_id,
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEventNotification/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            var html = "";

            if (res.statusCode != undefined) {
                $("#tbleventlist").append(html);
                messageBox("error", res.message);
                return false;
            }

            if (res.length > 0) {
                for (var i = 0; i < res.length; i++) {

                    var DateEventDate = new Date(res[i].event_date);

                    html += '<tr><td>' + res[i].company_name + '</td><td>' + res[i].event_name + '</td><td>' + GetDateFormatddMMyyyy(DateEventDate) + '</td><td>' + GetTimeFromDate(res[i].event_time) + '</td></tr>';
                }
            }

            $("#tbleventlist").append(html);


        },
        error: function (err, exception) {

            alert(err);
        }
    });
}

function BindEmployeeDtlFromEmpMasterByCompID(CompanyId, LoginEmpID) {
    //debugger;
    var emp_lst_under_login_emp;
    // var tkn = localStorage.getItem('Token');
    var lst_ = localStorage.getItem("emp_under_login_emp");

    if (lst_ != null && lst_ != "" && lst_.length > 0) {

        emp_lst_under_login_emp = JSON.parse(lst_);
    }
    else {
        $.ajax({
            type: "GET",
            //url: listapi + "apiMasters/Get_EmployeeHeadList",
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId + "/" + LoginEmpID,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;
                emp_lst_under_login_emp = res;
                localStorage.setItem("emp_under_login_emp", res);

            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });

    }

    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
        for (var i = 0; i < emp_lst_under_login_emp.length; i++) {
            $("#tblemplistt").append('<tr><td>' + emp_lst_under_login_emp[i].company_name + '</td><td>' + emp_lst_under_login_emp[i].emp_code + '</td><td>' + emp_lst_under_login_emp[i].emp_name + '</td></tr>');
        }
    }


    //$.ajax({
    //    type: "GET",
    //   // url: localStorage.getItem("ApiUrl") + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
    //    url: localStorage.getItem("ApiUrl") + "/apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId,
    //    data: {},
    //    contentType: "application/json",
    //    headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
    //    dataType: "json",
    //    success: function (response) {
    //        var res = response;

    //        if (res.length > 0 || (res != null && res != "")) {
    //            for (var i = 0; i < res.length; i++) {

    //                var full_emp_code = res[i].emp_code.split('(');
    //                var codee = "(" + full_emp_code[1];
    //                var empnamee = full_emp_code[0];

    //                $("#tblemplistt").append('<tr><td>' + codee + '</td><td>' + empnamee + '</td></tr>');
    //            }
    //        }




    //        //$(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    //        //$.each(res, function (data, value) {
    //        //    $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
    //        //})

    //        ////get and set selected value
    //        //if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
    //        //    $(ControlId).val(SelectedVal);
    //        //}
    //    },
    //    error: function (err) {
    //        alert(err.responseText);
    //    }
    //});
}

function Bind_PieChartByCompany(company_id, empid) {

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetPieChartByCompany/" + company_id + "/" + empid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            var present = response.total_present;
            var absent = response.total_absent;
            var leave = response.total_leave;
            drawPieChart(leave, present, absent)
        },
        error: function (err) {

            messageBox("error", err.responseText);
        }
    });
}
function drawPieChart(leave, present, absent) {

    if (leave == 0 && present == 0 && absent == 0) { present = 1; }
    Morris.Donut({
        element: 'donut-example',
        data: [
            { label: "On Leave", value: leave },
            { label: "Present", value: present },
            { label: "Absent", value: absent }
        ],
        resize: true,
        colors: ['#e3eaef', '#ff679b', '#777edd'],
        labelColor: '#888',
        backgroundColor: 'transparent',
        fillOpacity: 0.1,
        formatter: function (x) { return x + "" }
    });



}


//function drawPieChart(leave, present, absent) {


//    var config = {
//        type: 'pie',
//        data: {
//            datasets: [{
//                data: [
//                    //0,0,200
//                    leave, present, absent
//                ],
//                backgroundColor: [
//                    window.chartColors.yellow,
//                    window.chartColors.green,
//                    window.chartColors.red
//                ],
//                label: 'Dataset 1'
//            }],

//            labels: [
//                'On Leave',
//                'Persent',
//                'Absent',
//            ]
//        },
//        options: {
//            responsive: true,
//            legend: {
//                position: 'right',
//                fontSize: 18
//            },
//            events: false,
//            animation: {
//                duration: 500,
//                easing: "easeOutQuart",
//                onComplete: function () {
//                    var ctx = this.chart.ctx;

//                    ctx.textAlign = 'center';
//                    ctx.textBaseline = 'bottom';

//                    this.data.datasets.forEach(function (dataset) {

//                        for (var i = 0; i < dataset.data.length; i++) {
//                            var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
//                                total = dataset._meta[Object.keys(dataset._meta)[0]].total,
//                                mid_radius = model.innerRadius + (model.outerRadius - model.innerRadius) / 2,
//                                start_angle = model.startAngle,
//                                end_angle = model.endAngle,
//                                mid_angle = start_angle + (end_angle - start_angle) / 2;

//                            var x = mid_radius * Math.cos(mid_angle);
//                            var y = mid_radius * Math.sin(mid_angle);

//                            ctx.fillStyle = '#fff';

//                            if (i == 3) { // Darker text color for lighter background
//                                ctx.fillStyle = '#444';
//                                ctx.fontSize = 18;
//                            }
//                            // var percent = "";
//                            //if (dataset.data[i] > 0) {
//                            //    percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
//                            //}
//                            //else {
//                            // percent=;
//                            //}
//                            //var percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
//                            ctx.fillText(dataset.data[i], model.x + x, model.y + y);
//                            // Display percent in another line, line break doesn't work for fillText
//                            // ctx.fillText(percent, model.x + x, model.y + y + 15); // show percentage
//                        }
//                    });
//                }
//            }
//        }
//    };

//    if (document.getElementById('chart-area11') != null) {
//        var ctx = document.getElementById('chart-area11').getContext('2d');
//        myPie = new Chart(ctx, config);
//    }
//    else {
//        myPie = "";
//    }
//    // var ctx = document.getElementById('chart-area11').getContext('2d');
//    //myPie = new Chart(ctx, config);

//}



$("#btn_save_pwd").bind("click", function () {
    var iserror = false;
    var errormsg = "";

    if ($("#txtoldpwd_").val() == "") {
        iserror = true;
        errormsg = errormsg + "Old password cannot be blank.</br>";
    }

    if ($("#txtnewpwd_").val() == "") {
        iserror = true;
        errormsg = errormsg + "New password cannot be blank.</br>";
    }

    if ($("#txtconfirmpwd_").val() == "") {
        iserror: true;
        errormsg = errormsg + "Confirm Password cannot be blank.</br>";
    }

    if ($("#txtnewpwd_").val() != $("#txtconfirmpwd_").val()) {
        iserror = true;
        errormsg = errormsg + "New and confirm passsord not matched.</br>";
    }


    //var _encryptedpwd = CryptoJS.AES.encrypt("encryptpwd", $("#txtconfirmpwd").val()).toString();;

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }


    var resu = validatePassword();
    if (resu == false) {
        return false;
    }

    var key1 = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv1 = CryptoJS.enc.Utf8.parse('8080808080808080');


    var encryptedoldpwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($("#txtoldpwd_").val()), key1,
        {
            keySize: 128 / 8,
            iv: iv1,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    var encryptednewpwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($("#txtconfirmpwd_").val()), key1,
        {
            keySize: 128 / 8,
            iv: iv1,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    $('#loader').show();

    var mydata = {
        user_id: user_id,
        old_password: encryptedoldpwd.toString(),
        new_password: encryptednewpwd.toString(),
        last_modified_by: login_emp_id,
        default_company_id: company_idd,
    }

    var headers = {};
    headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headers["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/ChangeUserPassword",
        type: "POST",
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: "application/json",
        headers: headers,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                window.location.href = "/Login";
            }
            else {

                messageBox("error", Msg);
                return false;
            }
        },
        error: function (request, status, error) {
            _GUID_New();
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
            return false;
        }

    });

});


function validatePassword() {
    var p = document.getElementById('txtconfirmpwd_').value,
        errors = [];
    if (p.length < 8) {
        errors.push("Your password must be at least 8 characters");
    }
    if (p.search(/[a-z]/i) < 0) {
        errors.push("Your password must contain at least one letter.");
    }
    if (p.search(/[0-9]/) < 0) {
        errors.push("Your password must contain at least one digit.");
    }
    if (errors.length > 0) {
        alert(errors.join("\n"));
        return false;
    }
    return true;
}


//function BindEmployee_Birthday_ByCompID(CompanyId, LoginEmpID) {
//    //debugger;
//    var emp_lst_under_login_emp;
//    // var tkn = localStorage.getItem('Token');
//    var lst_ = localStorage.getItem("emp_under_login_emp");

//    if (lst_ != null && lst_ != "" && lst_.length > 0) {

//        emp_lst_under_login_emp = JSON.parse(lst_);
//    }
//    else {

//        $.ajax({
//            type: "GET",
//            //url: listapi + "apiMasters/Get_EmployeeHeadList",
//            url: localStorage.getItem("ApiUrl") + "/apiEmployee/Employee_Birthday_ByCompID/" + CompanyId + "/" + LoginEmpID,
//            data: {},
//            async: false,
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            success: function (response) {
//                var res = response;
//                emp_lst_under_login_emp = res;
//                localStorage.setItem("emp_under_login_emp", res);
//            },
//            error: function (err) {
//                alert(err.responseText);
//                $('#loader').hide();
//            }
//        });

//    }

//    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
//        var empimg = "";

//        for (var i = 0; i < emp_lst_under_login_emp.length; i++) {
//            empimg = emp_lst_under_login_emp[i].emp_img == null ? "" : emp_lst_under_login_emp[i].emp_img;
//            if (empimg == "") {
//                empimg = "EmployeeImage/DefaultUser/defaultimage.jpg";
//            }

//            $("#tbl_emp_birthdaylist").append('<tr class="marquee"><td><img src="' + localStorage.getItem("ApiUrl").replace("api/", "") + empimg + '" style="width:80;height:80" /></td><td>' + emp_lst_under_login_emp[i].emp_name_code + '</td><td>' + emp_lst_under_login_emp[i].dept_name + '</td><td>' + GetDateFormatddMMyyyy(new Date(emp_lst_under_login_emp[i].dob)).slice(0, -5) + '</td></tr>');
//        }
//    }
//}

//function BindEmployee_Annivery_ByCompID(CompanyId, LoginEmpID) {

//    var emp_lst_under_login_emp;
//    // var tkn = localStorage.getItem('Token');
//    var lst_ = localStorage.getItem("emp_under_login_emp");

//    if (lst_ != null && lst_ != "" && lst_.length > 0) {

//        emp_lst_under_login_emp = JSON.parse(lst_);
//    }
//    else {

//        $.ajax({
//            type: "GET",
//            //url: listapi + "apiMasters/Get_EmployeeHeadList",
//            url: localStorage.getItem("ApiUrl") + "apiEmployee/Employee_Annivery_ByCompID/" + CompanyId + "/" + LoginEmpID,
//            data: {},
//            async: false,
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            success: function (response) {
//                var res = response;
//                emp_lst_under_login_emp = res;
//                localStorage.setItem("emp_under_login_emp", res);

//            },
//            error: function (err) {
//                alert(err.responseText);
//                $('#loader').hide();
//            }
//        });

//    }

//    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
//        var empimg = "";
//        for (var i = 0; i < emp_lst_under_login_emp.length; i++) {
//            empimg = emp_lst_under_login_emp[i].emp_img == null ? "" : emp_lst_under_login_emp[i].emp_img;
//            if (empimg == "") {
//                empimg = "EmployeeImage/DefaultUser/defaultimage.jpg";
//            }
//            $("#tbl_emp_anniversarylist").append('<tr class="marquee"><td><img src="' + localStorage.getItem("ApiUrl").replace("api/", "") + empimg + '" style="width:80;height:80" /></td><td>' + emp_lst_under_login_emp[i].emp_name_code + '</td><td>' + emp_lst_under_login_emp[i].dept_name + '</td><td>' + GetDateFormatddMMyyyy(new Date(emp_lst_under_login_emp[i].doanv)).slice(0, -5) + '</td></tr>');
//        }

//    }
//}


function Bind_CurrentOpenings() {
    // debugger;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_CurrentOpenings',
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            $('#loader').hide();
            $("#tblCurrentOpenings").DataTable({
                "processing": true, // for show progress bar
                "bPaginate": false, // for hiding pagination
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [],
                "columns": [
                    // { "data": null, "title": "S.No" },
                    { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                    { "data": "dept_name", "name": "dept_name", "title": "Department Name", "autoWidth": true },
                    { "data": "opening_detail", "name": "opening_detail", "title": "Opening", "autoWidth": true },
                    //  { "data": "posted_date", "name": "posted_date", "title": "Posted On", "autoWidth": true },
                    //  { "data": "experience", "name": "experience", "title": "Experience", "autoWidth": true },
                    { "data": "job_description", "name": "job_description", "title": "Job Description", "autoWidth": true },
                    { "data": "role_responsibility", "name": "role_responsibility", "title": "Role and Responsibility", "autoWidth": true },
                    { "data": "current_status", "name": "current_status", "title": "Current Satus", "autoWidth": true },

                ],
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},
                //"lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });


        },
        error: function (err, exception) {

            alert(err);
        }
    });
}

function OpenDocumentt(path) {
    debugger;
    window.open(path);
}

function Bind_policies() {
    //debugger;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/Get_Policies",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $("#tblPolicies").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bPaginate": false, //for hiding Paging
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [],
                "columns": [
                    //{ "data": null, "title": "S.No" },
                    { "data": "policy_name", "name": "policy_name", "title": "Policy Name", "autoWidth": true },
                    //{ "data": "created_by", "name": "created_by", "title": "Created By", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    //{ "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Attachment", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="" onclick="OpenDocumentt(\'' + full.doc_path + '\')"><i class="fa fa-paperclip"></i>Attachment</a>';
                        }
                    },
                    //{
                    //    "title": "Edit", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.pkid_policy + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //},
                    //{
                    //    "title": "Delete", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a  onclick="DeletePolicy(' + full.pkid_policy + ' )" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //}
                ],
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},
                //"lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

        },
        error: function (err, exception) {

            alert(err);
        }
    });
}