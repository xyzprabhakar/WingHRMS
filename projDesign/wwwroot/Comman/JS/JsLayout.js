var login_role_id;
var login_emp_id;
var company_idd;
$(document).ready(function () {



    update_login_user_login_time();

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 123) { // Prevent F12
            return false;
        } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
            return false;
        }
    });

    var token = localStorage.getItem('Token');

    if (token == null) {
        window.location = '/Login';
    }

    isUserLoggedIn();
    // var login_role_id = localStorage.getItem("emp_role_id");

    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    emp_x_value = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var newlogin = CryptoJS.AES.decrypt(localStorage.getItem("_firsttimelogin"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



    Bind_Right_Menu();
    //if (parseInt(11) >= parseInt(emp_x_value) <= parseInt(100)) {
    //    $("#hrms_logo_link").attr("href", "/View/Dashboard");
    //}
    //else {
    $("#hrms_logo_link").attr("href", "/Dashboard");
    //}




    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);


    var is_managerr = is_managerr_dec;
    //var HaveDisplay = ISDisplayMenu("Is Company Admin");
    var HaveDisplay = 1;
    if (HaveDisplay == 1) {
        $("#metabb").hide();
        $("#limyteam").hide();
        $("#div_left_superadmin").show();
        //$("#div_left_admin").hide();
        $("#div_show_manager").show();
        $("#div_show_event_notification").hide();
        // $("#div_left_user").hide();
        $("#div_chart").hide();
        if (is_managerr == "no") {
            $("#divpendingRequest").hide();
        }
        else {
            $("#divpendingRequest").show();
        }




        // $('#hrms_logo_link').attr("href", "/Dashboard");
        var comp_logo = localStorage.getItem("company_logo");
        if (comp_logo == 'null') {
            $("#img_company_logo").attr("src", 'http://sakshemitAPI.sakshemithrms.net/' + '/CompanyLogo/no_logo.png');
            //$("#img_company_logo").attr("src", 'http://192.168.10.129:1011/' + '/CompanyLogo/no_logo.png');
            //$("#img_company_logo").attr("src", 'https://localhost:44384/' + '/CompanyLogo/no_logo.png');
            //$("#img_company_logo").attr("src", 'http://1.6.99.134:1011/' + '/CompanyLogo/no_logo.png');
        }
        else {
            $("#img_company_logo").attr("src", 'http://sakshemitAPI.sakshemithrms.net/' + localStorage.getItem("company_logo"));
            //$("#img_company_logo").attr("src", 'http://192.168.10.129:1011/' + localStorage.getItem("company_logo"));
            //$("#img_company_logo").attr("src", 'https://localhost:44384/' + localStorage.getItem("company_logo"));
        }

        //$("#admin_dashboard_manager_list").show();
    }
    else {
        $("#metabb").show();
        $("#div_left_superadmin").show();
        // $("#div_left_superadmin").hide();
        //$("#div_left_admin").hide();
        if (is_managerr == "no") {
            $('#divpendingRequest').hide();
            $('#div_chart').hide();
        }
        else {
            $("#limyteam").show();
        }

        // $("#admin_dashboard_manager_list").hide();

        //var employee_id = localStorage.getItem('emp_id');

        // Bind_Reporting_Manager(login_emp_id, company_idd);

        $("#div_left_user").show();
        $("#img_company_logo").attr("src", 'http://sakshemitAPI.sakshemithrms.net/' + localStorage.getItem("company_logo"));
        //$("#img_company_logo").attr("src", 'http://192.168.10.129:1011/' + localStorage.getItem("company_logo"));
        //$("#img_company_logo").attr("src", 'https://localhost:44384/' + localStorage.getItem("company_logo"));
        //  $('#hrms_logo_link').attr("href", "/view/Dashboard");
    }

});




function Bind_Right_Menu() {
    //var company_id = localStorage.getItem("company_id");

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/Get_Right_Menu_Link/-1/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.length > 0) {
                for (var i = 0; i < res.length; i++) {
                    $("#ul_right_menu").append('<a class="dropdown-item" href=' + res[i].url + ' target="_blank" ><i class="' + res[i].icon_url + ' text-muted mr-2 "></i> ' + res[i].menu_name + '</a>');
                }
            }
            $("#ul_right_menu").append('<a class="dropdown-item" href="/EmployeeProfile/OfficialSection"><span class="fa fa-user text-muted mr-2"></span>Profile</a>');
            $("#ul_right_menu").append('<a class="dropdown-item" href="/Masters/ChangePassword"><span class="fa fa-lock text-muted mr-2"></span>Change Password</a>');
            $("#ul_right_menu").append('<a class="dropdown-item" href="/Masters/QRCode"><span class="fa fa-qrcode text-muted mr-2"></span>QR Code</a>');
            //$("#ul_right_menu").append('<a class="dropdown-item" href="/epa/EpaSubmissionForm" ><span class="fa fa-user text-muted mr-2"></span>EPA</a>');
            //$("#ul_right_menu").append('<li><a href="/Login"><span class="fa fa-power-off"></span>LogOut</a></li>');
            $("#ul_right_menu").append('<a class="dropdown-item" href="#" onclick="Update_LoginLog()"><span class="fa fa-power-off text-muted mr-2"></span>LogOut</a>');

        },
        error: function (err, exception) {
            window.localStorage.removeItem("Token");
            //alert('Your session has expired please login again...!');
            window.location.href = "/Login";
            return false;
        }
    });
}

//function Bind_Reporting_Manager(employee_id,companyidd) {

//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEmployeeManagers/" + employee_id + "/" + companyidd,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (data) {

//            if (data == undefined) {
//                return;
//            }
//            //debugger;
//            var res = data;
//            //$('#pTest').text('test')
//            $('#lblmanager1').text('RM 1 - ' + (res[0].manager_name_code == null || res[0].manager_name_code == '' ? '' : res[0].manager_name_code));
//            if (res[0].final_approval == 1) {
//                $('#lblmanager1_is_final').text('Final Approval');
//            }
//            else if (res[0].final_approval == 2) {
//                $('#lblmanager2_is_final').text('Final Approval');
//            }
//            else if (res[0].final_approval == 3) {
//                $('#lblmanager3_is_final').text('Final Approval');
//            }

//            if (res[0].m_two_name_code != "" && res[0].m_two_name_code != null) {
//                $('#lblmanager2').text('RM 2 - ' + res[0].m_two_name_code);
//            }

//            if (res[0].m_three_name_code != "" && res[0].m_three_name_code != null) {
//                $('#lblmanager3').text('RM 3 - ' + res[0].m_three_name_code);
//            }

//        },
//        error: function (err, exception) {
//            //alert(err);
//            window.localStorage.removeItem("Token");
//            alert('Your session has expired please login again...!');
//            window.location.href = "/Login";
//            return;
//        }
//    });
//}

function Update_LoginLog() {
    var emp_id;
    var user_id;

    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/Login/Updata_Login_Logs/" + login_emp_id + "/" + user_id,
        type: "POST",
        contentType: "applicaiton/json",
        headers: { 'Authorization': 'Bearer' + localStorage.getItem("Token") },
        dataType: "json",
        data: "{}",
        success: function (response) {
            $("#loader").hide();
            window.location.href = "/Login";
        },
        error: function (err) {
            $("#loader").hide();
            messageBox(err, responseText);
        }
    });
}

function Customajaxerror(err) {
    if (err.status == 401) {
        window.localStorage.clear();
        window.localStorage.removeItem("Token");
        window.location.href = "/Login";
    }
    else if (err.response == "" || err.responseText == "") {
        window.localStorage.clear();
        window.localStorage.removeItem("Token");
        window.location.href = "/Login";
    }
    else {
        alert(err.responseText);
    }
}


function update_login_user_login_time() {
    var emp_id;
    var user_id;

    emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/Login/update_login_user_login_time/" + emp_id + "/" + user_id,
        type: "POST",
        contentType: "applicaiton/json",
        headers: { 'Authorization': 'Bearer' + localStorage.getItem("Token") },
        dataType: "json",
        data: "{}",
        async: false,
        timeout: 5000,
        success: function (response) {

            if (response.statusCode == 1 || response.statusCode == undefined) {
                window.localStorage.removeItem("Token");
                window.location.href = "/Login";
                return false;
            }
        },
        error: function (err) {
            Customajaxerror(err);
            //window.localStorage.removeItem("Token");
            ////alert('Your session has expired please login again...!');
            //window.location.href = "/Login";
            //return false;
        }
    });
}



function isUserLoggedIn() {
    
    if (getCookie("loginstatus") != 'loggedin') {
        window.localStorage.removeItem("Token");
        window.location.href = "/Login";
    }

    let session = localStorage.getItem('Token');
    if (session == null) {
        window.location.href = "/Login";
    }
}

function getCookie(name) {
    // Split cookie string and get all individual name=value pairs in an array
    var cookieArr = document.cookie.split(";");

    // Loop through the array elements
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");

        /* Removing whitespace at the beginning of the cookie name
        and compare it with the given string */
        if (name == cookiePair[0].trim()) {
            // Decode the cookie value and return
            if (cookiePair[1] != undefined || cookiePair[1] != '' || cookiePair[1] != null)
                return cookiePair[1];
            else return null;
        }
    }

    // Return null if not found
    return null;
}