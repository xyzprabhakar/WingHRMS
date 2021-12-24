//Version 3
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    }
    //else {
    //    x.innerHTML = "Geolocation is not supported by this browser.";
    //}
}

function showPosition(position) {
    window.localStorage.setItem("latitude", position.coords.latitude);
    window.localStorage.setItem("longitude", position.coords.longitude);    
}

$(document).ready(function () {
    resetData();
    resetCaptchaImage();
    //show();
});

function resetData()
{
    localStorage.removeItem("token");
    localStorage.removeItem("isReloadAllData");
    localStorage.removeItem("baseUrl");
    localStorage.setItem("isReloadAllData", true);
    localStorage.setItem("baseUrl", baseUrl);
    getLocation();
}
$('#btnLogin').bind("click", function () {
    performLogin();
});

$("#CaptchaCode").bind('keydown', function (event) {
    if (event.key == 'Enter') {        
        event.preventDefault();
        performLogin();        
    }
});
$("#txtUsername").bind('keydown', function (event) {
    if (event.key == 'Enter') {        
        event.preventDefault();
        document.getElementById("txtPassword").focus();        
    }
});
$("#txtPassword").bind('keydown', function (event) {
    if (event.key == 'Enter') {        
        event.preventDefault();
        document.getElementById("CaptchaCode").focus();        
    }
});
function performLogin() {
    var Username = $("#txtUsername").val();
    var Password = $("#txtPassword").val();
    var CaptchaCode = $("#CaptchaCode").val();
    var CcCode = $("#hdnCcCode").val();
    
    if ($("#hdnEmail").val() == null || $("#hdnEmail").val() == "" || $("#hdnEmail").val() == undefined) {

        //var CompanyCode = $("#txtCompanyCode").val();
        var Username = $("#txtUsername").val();
        var Password = $("#txtPassword").val();
        var CaptchaCode = $("#CaptchaCode").val();
        var CcCode = $("#hdnCcCode").val();
        $('#btnLogin').attr("disabled", true).html('<i class="fa fa-circle-o-notch fa-spin"></i> Please wait..').css({ backgroundColor: '#d4cdcd' });        
        if (Username == '') {
            alert('All Fields Are Mandatory...!');
            $("#txtUsername").focus();
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
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
    }
    
    var myData = {
        'TempUserId': window.localStorage.getItem("tempUserId"),
        'UserName': Username,//Password,    
        "Password": Password,
        "CaptchaId": CcCode ,
        "CaptchaValue": CaptchaCode,
        "OrgCode": "ADM",
        "Longitute": window.localStorage.getItem("longitude"),
        "Latitude": window.localStorage.getItem("latitude"),
        "FromLocation": "",
        "UserType": 2
    };
    
    var apiurl = baseUrl+("User/Login");
    $('#loader').show();
    $.ajax({
        url: apiurl,
        type: "POST",
        data: JSON.stringify( myData),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.messageType == 1) {
                window.localStorage.setItem("userId", data.returnId.userId);
                window.localStorage.setItem("normalizedName", data.returnId.normalizedName);
                window.localStorage.setItem("employee_id", data.returnId.employee_id);
                window.localStorage.setItem("user_type", data.returnId.user_type);
                window.localStorage.setItem("customerId", data.returnId.customerId);
                window.localStorage.setItem("distributorId", data.returnId.distributorId);
                window.localStorage.setItem("token", data.returnId.jsonWebToken);
                window.localStorage.setItem("currentApplication", 0);//Need to set on user click
                window.localStorage.setItem("refreshData", 1);
                window.localStorage.setItem("dBVersion", 3);
                document.cookie = `userId=${data.returnId.userId}`;
                window.location.href = '/index';    
            }
            else {
                alert(data.message);
                resetCaptchaImage();
            }
            $('#btnLogin').css({ backgroundColor: '#d05858' }).text('Login').attr('disabled', false);
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
    $('#loader').show();
    var tempUserId = localStorage.getItem('tempUserId');
    var baseUrl = localStorage.getItem('baseUrl');
    var apiurl_ = baseUrl+"user/GenrateLoginCaptcha";
    if (tempUserId == null || tempUserId == "") {
        apiurl_ = apiurl_ + "/" + tempUserId;
    }
    else {
        apiurl_ = apiurl_ + "/0";
    }
    $.ajax({
        type: "GET",
        url: apiurl_,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.messageType == 1) {
                $("#img-captcha").attr("src", response.returnId.captchaImage);                
                $("#hdnCcCode").val(response.returnId.captchaId)
                if (tempUserId == null || tempUserId == "" || tempUserId == "null") {
                    window.localStorage.setItem("tempUserId", response.returnId.tempUserId);
                }
            }
            else {
                alert(response.message);
            }
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert("someting went wrong please try after some time");
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