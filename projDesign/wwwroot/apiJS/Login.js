
$(document).ready(function () {

    //signout();  
    resetCaptchaImage();
    $("#hdnEmail").val('');
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
    // localStorage.clear();
    window.localStorage.removeItem("Token");
    window.localStorage.removeItem("ApiUrl");
    window.localStorage.removeItem("emp_id");
    window.localStorage.removeItem("sit_id");
    window.localStorage.removeItem("user_name");
    localStorage.setItem("class", "link1");

    window.localStorage.removeItem("new_compangy_idd");
    window.localStorage.removeItem("new_emp_id");
    window.localStorage.removeItem("company_name");
    window.localStorage.removeItem("emp_role_id");
    window.localStorage.removeItem("user_id");
    window.localStorage.removeItem("is_managerr");
    window.localStorage.removeItem("company_id");
    window.localStorage.removeItem("login_emp_name");
    window.localStorage.removeItem("employee_photo_path");
    window.localStorage.removeItem("_appSetting_domainn");
    //show();
});

//STAR CAPTCHA

//function show() {
//  //  document.getElementById('txtcaptchaverification').value = "";
//    //event.preventDefault(); //stop Reload page
//    var a, b, c, d;
//    a = Math.ceil(Math.random() * 10) + '';
//    var main = document.getElementById('txtautocaptcha');
//    a = makeid('1');
//    b = makeid('A');
//    c = makeid('a');
//    d = makeid('A');
//    main.value = a + b + c + d;
//}


function makeid(flag) {
    var possible;
    var text = "";
    if (flag == 1) {
        possible = "0123456789";
        text = possible.charAt(Math.floor(Math.random() * possible.length));
    }
    else if (flag == 'A') {
        possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        text = possible.charAt(Math.floor(Math.random() * possible.length));
    }
    else if (flag == 'a') {
        possible = "abcdefghijklmnopqrstuvwxyz";
        text = possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
}

//END CAPTCHA

$("#CaptchaCode").bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        performLogin();
    }
});

$("#txtUsername").bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        document.getElementById("txtPassword").focus();
    }
});

$("#txtPassword").bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        document.getElementById("CaptchaCode").focus();
    }
});

$('#btnLogin').bind("click", function () {
    performLogin();
});

function performLogin() {
    var isgauth = 0;
    var CompanyCode = $("#txtCompanyCode").val();
    var Username = $("#txtUsername").val();
    var Password = $("#txtPassword").val();
    var CaptchaCode = $("#CaptchaCode").val();
    var CcCode = $("#hdnCcCode").val();
    var email = '';
    var encryptedpwd = '';

    if ($("#hdnEmail").val() == null || $("#hdnEmail").val() == "" || $("#hdnEmail").val() == undefined) {

        var CompanyCode = $("#txtCompanyCode").val();
        var Username = $("#txtUsername").val();
        var Password = $("#txtPassword").val();
        var CaptchaCode = $("#CaptchaCode").val();
        var CcCode = $("#hdnCcCode").val();
        $('#btnLogin').attr("disabled", true).html('<i class="fa fa-circle-o-notch fa-spin"></i> Please wait..').css({ backgroundColor: '#d4cdcd' });
        //$(this).css({ backgroundColor: '#d4cdcd' }).text('Processing..').attr('disabled',true);
        var key2 = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv2 = CryptoJS.enc.Utf8.parse('8080808080808080');

        if (Username == '') {
            alert('All Fields Are Mandatory...!');
            $("#txtUsername").focus();
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            //$('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            return false;
        }

        if (Password == '') {
            alert('All Fields Are Mandatory...!');
            $("#txtPassword").focus();
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            return false;
        }

        if ($("#CaptchaCode").val() == "" || $("#CaptchaCode").val() == null) {
            alert('Please enter Captcha');
            $("#CaptchaCode").focus();
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            return false;
        }


        encryptedpwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(Password), key2,
            {
                keySize: 128 / 8,
                iv: iv2,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
    }
    else {
        email = $("#hdnEmail").val();
        isgauth = 1;
    }


    var myData = {
        'user_name': Username,
        'password': encryptedpwd.toString(),//Password,
        "emp_id": 1,
        "emp_name": "hi",
        "tokken": "sdas",
        "CaptchaCode": CaptchaCode,
        "CcCode": CcCode,
        "Email": email,
        "isgauth": isgauth
    };

    var Obj = JSON.stringify(myData);
    $('#loader').show();
    $.ajax({
        url: apiurl + "/Login",
        type: "POST",
        data: Obj,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.statusMessage;
            var wrong_attempt = data.wrong_attempt;
            var Token = data.token;
            var emp_id = data.emp_id;
            var user_name = data.user_name;
            var employee_photo_path = data.employee_photo_path;
            // get role id
            var emp_role_idd = data.employee_role_id;
            var company_idd = data.default_company;
            var dashboardd_menu_master_data = data.dashboardd_menu_master_data;
            var dashboardd_role_menu_data = data.dashboardd_role_menu_data;
            var filter_comp_ = data.filter_comp_;
            var manager_emp_list = data.manager_emp_list;
            var is_managerr = manager_emp_list != null && manager_emp_list != "" ? "yes" : "no";
            var _appSetting_domainn = data._appSetting_domainn;
            var user_id = data.user_id;
            var user_Dep_id = data.user_Dep_id;
            var user_Dep_name = data.user_Dep_name;
            //add by supriya on 09-07-2019
            var first_namee = data.employee_first_name != null && data.employee_first_name != "" ? data.employee_first_name : "";
            var middle_namee = data.employee_middle_name != null && data.employee_middle_name != "" ? data.employee_middle_name : "";
            var last_namee = data.employee_last_name != null && data.employee_last_name != "" ? data.employee_last_name : "";
            var full_nameee = first_namee + " " + middle_namee + " " + last_namee;
            var company_name = data.company_name;
            var company_logo = data.company_logo;
            var emp_companies = data.emp_company_lst;
            var emp_under_login_emp = data._under_emp_lst;
            //var is_attendence_freezed = data.app_setting;
            if (data.app_setting_dic != null && data.app_setting_dic != undefined) {
                var is_attendence_freezed_for_Emp = data.app_setting_dic["attandance_application_freezed_for_Emp"];
                var is_attendence_freezed_for_Admin = data.app_setting_dic["attandance_application_freezed__for_Admin"];
            }

            if (statuscode == "1") {
                if (Token == '' || Token == 'undefined') {
                    alert("you don't have permission to access...!");
                    $("#txtUsername").val('');
                    $("#txtPassword").val('');
                    $("#CaptchaCode").val('');
                    $(this).css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
                }
                else {
                    var ApiUrl = apiurl;
                    localStorage.setItem("sit_id", 'AiIsIjgxMjoiODeAiLcI4mcR6IjgwIiJwiNzkioEiI3OSIsEijc4IjoTiNzgiLCI4OCI6Ijg4IiwiMTA3IjoiMTA3IiwiM');
                    document.cookie = "loginstatus=loggedin";
                    localStorage.setItem("Token", Token);
                    localStorage.setItem("ApiUrl", ApiUrl);
                    localStorage.setItem("IsAttendenceFreezedForEmp", is_attendence_freezed_for_Emp.toLowerCase());
                    localStorage.setItem("IsAttendenceFreezedForAdmin", is_attendence_freezed_for_Admin.toLowerCase());
                    localStorage.setItem("is_super_admin", emp_role_idd.includes(1));
                    var emp_id_enc = CryptoJS.AES.encrypt("'" + emp_id + "'", localStorage.getItem("sit_id"));
                    var emp_role_id_enc = CryptoJS.AES.encrypt("'" + emp_role_idd + "'", localStorage.getItem("sit_id"));
                    var user_id_enc = CryptoJS.AES.encrypt("'" + user_id + "'", localStorage.getItem("sit_id"));
                    var user_Dep_id_enc = CryptoJS.AES.encrypt("'" + user_Dep_id + "'", localStorage.getItem("sit_id"));
                    var company_idd_enc = CryptoJS.AES.encrypt("'" + company_idd + "'", localStorage.getItem("sit_id"));
                    var key = CryptoJS.enc.Base64.parse("#base64Key#");
                    var iv = CryptoJS.enc.Base64.parse("#base64IV#");
                    var _firsttimelogin = CryptoJS.AES.encrypt("'" + data._firsttimelogin + "'", localStorage.getItem("sit_id"));
                    var user_name_enc = CryptoJS.AES.encrypt(user_name, key, { iv: iv });
                    //console.log(user_name_enc.toString());
                    var full_nameee_enc = CryptoJS.AES.encrypt(full_nameee, key, { iv: iv });

                    var employee_photo_path_enc = CryptoJS.AES.encrypt(employee_photo_path, key, { iv: iv });

                    var _appSetting_domainn_enc = CryptoJS.AES.encrypt(_appSetting_domainn, key, { iv: iv });

                    var is_managerr_enc = CryptoJS.AES.encrypt(is_managerr, key, { iv: iv });
                    
                    localStorage.setItem("emp_id", emp_id_enc);
                    localStorage.setItem("emp_role_id", emp_role_id_enc);
                    localStorage.setItem("emp_dep_id", emp_role_id_enc);
                    localStorage.setItem("user_id", user_id_enc);
                    localStorage.setItem("user_Dep_id", user_Dep_id_enc);
                    localStorage.setItem("user_Dep_name", user_Dep_name);
                    localStorage.setItem("company_id", company_idd_enc);
                    localStorage.setItem("user_name", user_name_enc);
                    localStorage.setItem("login_emp_name", full_nameee_enc);
                    localStorage.setItem("employee_photo_path", employee_photo_path_enc);
                    localStorage.setItem("_appSetting_domainn", _appSetting_domainn_enc);
                    localStorage.setItem("is_managerr", is_managerr_enc);
                    localStorage.setItem("company_name", company_name);
                    localStorage.setItem("company_logo", company_logo);
                    localStorage.setItem("Is Company Admin", 1);
                    localStorage.setItem("ISDisplayMenu", 1);



                    localStorage.setItem("dashboardd_role_menu_data", JSON.stringify(dashboardd_role_menu_data));
                    //localStorage.setItem("dashboardd_menu_master_data", JSON.stringify(dashboardd_menu_master_data));
                    localStorage.setItem("menu_filter_comp_", JSON.stringify(filter_comp_));

                    //21-05-2020
                    localStorage.setItem("emp_companies_lst", JSON.stringify(emp_companies));

                    localStorage.setItem("emp_under_login_emp", JSON.stringify(emp_under_login_emp));

                    localStorage.setItem("_menu_lst", JSON.stringify(data.menu_lst));
                    //21-05-2020

                    localStorage.setItem("_firsttimelogin", _firsttimelogin);

                    // Delete_previous_Guid();
                    //if ((emp_role_idd >= 1 && emp_role_idd <= 10) || emp_role_idd >= 101) {
                    window.location.href = '/Dashboard';
                    //}
                    //else {
                    //    window.location.href = 'view/Dashboard';
                    //}

                    //if (emp_role_idd ==6) {
                    //    window.location.href = 'view/Dashboard';
                    //}
                    //else {
                    //    window.location.href = '/Dashboard';
                    //}
                }
            }
            else if (statuscode == "0") {
                $('#btnLogin').css({ backgroundColor: '#44a2d2' }).text('Login').attr('disabled', false);
                if (wrong_attempt > 0) {
                    alert(Msg + ' Wrong Attempt ' + wrong_attempt);
                }
                else {
                    alert(Msg);
                }
                $("#txtCompanyCode").val('');
                $("#txtPassword").val('');
                $("#CaptchaCode").val('');
                $("#txtUsername").val('').focus();
            }
            else if (statuscode == "2") {
                $("#txtCompanyCode").val('');
                $("#txtPassword").val('');
                $("#CaptchaCode").val('');
                $("#txtUsername").val('').focus();
            }
            else if (statuscode == "3") {
                alert(Msg);
                $("#CaptchaCode").val('').focus();
                $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            }
            resetCaptchaImage();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            if (err.responseText == null || err == '') {
                alert('Server is busy please try again later...!!!');
            }
            else {
                alert(err.responseText);
            }
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            resetCaptchaImage();
            $("#CaptchaCode").val('');
        }
    });

}

$("#btnimg_captcha_change").bind("click", function () {
    resetCaptchaImage();
});

//$("#btnfeedback").bind("click", function () {
//    window.open('https://docs.google.com/forms/d/e/1FAIpQLSdH72FLZgHi-4dIO5urvUFgVWgVokBzC2AwhwKciO-7DrTeCg/viewform?vc=0&c=0&w=1', '_blank');
//});


// Get the button that opens the modal
var btn = document.getElementById("forgotten_password");
// Get the <span> element that closes the modal
// var span = document.getElementsByClassName("close")[0];
// When the user clicks the button, open the modal
btn.onclick = function () {
    $('#exampleModal').modal({
        keyboard: false,
        backdrop: false
    })
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function resetCaptchaImage() {

    var guid = $('#hdnCcCode').val();
    var apiurl_ = '';
    if (guid == '') {
        apiurl_ = apiurl + 'Login/get-captcha-image/0';
    }
    else {
        apiurl_ = apiurl + 'Login/get-captcha-image/' + guid;
    }

    $.ajax({
        type: "GET",
        url: apiurl_,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var res = response;

            $("#img-captcha").attr("src", res.img);
            $("#hdnCcCode").val(res.code)

        },
        error: function (err) {
           // window.location.href = "/Login";
           // return false;
            //alert(err.responseText);
        }

    });

}
$('#btnForgotPassword').bind("click", function () {
    var username = $("#txtUsernameForgot").val();
    if (username == "") {
        alert("Please Enter Username...!");
        return false;
    }

    $("#btnForgotPassword").attr("disabled", true).html('<i class="fa fa-circle-o-notch fa-spin"></i> Please wait..').css({ backgroundColor: '#d4cdcd' });

    $.ajax({
        url: apiurl + "apiMasters/ForgotUserPwd/" + username,
        type: "POST",
        data: {},
        dataType: "json",
        contentType: "application/json",
        headers: { 'salt': $("#loginhdnsalt").val() },
        success: function (data) {
            $("#loginhdnsalt").val("@Html.ValueForModel(new projDesign.GenrateGUID().GetGuid)");
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#btnForgotPassword').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                alert(Msg);
            }
        },
        error: function (request, status, error) {

            $("#loginhdnsalt").val("@Html.ValueForModel(new projDesign.GenrateGUID().GetGuid)");
            $('#btnForgotPassword').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
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

            }
            catch (err) { }
            messageBox("error", error);
        }

    });

});


////< !--Google Authentication-- >

//var auth2; // The Sign-In object.
//var googleUser; // The current user.


///**
// * Calls startAuth after Sign in V2 finishes setting up.
// */
//var appStart = function () {
//    gapi.load('auth2', initSigninV2);
//};


///**
// * Initializes Signin v2 and sets up listeners.
// */
//var initSigninV2 = function () {
//    auth2 = gapi.auth2.init({
//        client_id: '89420726146-1u3lestpp1bo3htm33gjn4rvne8sgtn6.apps.googleusercontent.com',
//    });

//    // Listen for sign-in state changes.
//    //auth2.isSignedIn.listen(signinChanged);

//    // Listen for changes to current user.
//    //auth2.currentUser.listen(userChanged);

//    // Sign in the user if they are currently signed in.
//    if (auth2.isSignedIn.get() == true) {
//        auth2.signIn();
//    }

//    // Start with the current live values.
//    //refreshValues();
//};


//function onSuccess(googleUser) {
//    console.log('Logged in as: ' + googleUser.getBasicProfile().getName());
//    var profile = googleUser.getBasicProfile();
//    var id_token = googleUser.getAuthResponse().id_token;
//    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
//    console.log('Name: ' + profile.getName());
//    console.log('Image URL: ' + profile.getImageUrl());
//    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
//    var a = profile.getEmail();
//    $("#hdnEmail").val(a);
//    myfunction(a);
//}

//function onFailure(error) {
//    alert("Error in Authenticating User...");
//}

//var clicked = false;//Global Variable
//function ClickLogin() {
//    clicked = true;
//}

//function onSignIn(googleUser) {
//    if (clicked) {
//        var profile = googleUser.getBasicProfile();
//        console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
//        console.log('Name: ' + profile.getName());
//        console.log('Image URL: ' + profile.getImageUrl());
//        console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
//        var a = profile.getEmail();
//        $("#hdnEmail").val(a);
//        //signout();
//        performLogin();
//    }
//    else {

//        var profile = googleUser.getBasicProfile();
//        console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
//        console.log('Name: ' + profile.getName());
//        console.log('Image URL: ' + profile.getImageUrl());
//        console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
//        signout();
//    }
//}

////function renderButton() {

////    gapi.signin2.render('my-signin2', {
////        'scope': 'profile email',
////        'width': 415,
////        'class': 'btn-btn-primary',
////        'longtitle': true,
////        'theme': 'dark',
////        'onsuccess': onSuccess,
////        'onfailure': onFailure
////    });
////}

//function signout() {
//    if (gapi.auth2 != undefined) {
//        var auth2 = gapi.auth2.getAuthInstance();
//        auth2.signOut().then(function () {
//            console.log('User signed out.');
//        });
//    }
//}
 
////< !--Google Authentication-- >