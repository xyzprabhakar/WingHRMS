$('#loader').show();

var login_role_id;
var default_company;
var login_emp_id;
var user_id;
var login_user_id_dec

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        // var HaveDisplay = ISDisplayMenu("Display Company List");
        login_user_id_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
        var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

        var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

        $("#txtloginuserid").val(login_name_code);
        // $("#txtloginuserid").prop("disabled", "disabled");
        $('#txtloginuserid').attr('readonly', true);
        $('#loader').hide();

    }, 2000);// end timeout

});

function validatePassword_() {
    var p = document.getElementById('txtconfirmpwd').value,
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

$("#btnsave").bind("click", function () {

    var iserror = false;
    var errormsg = "";

    if ($("#txtoldpwd").val() == "") {
        iserror = true;
        errormsg = errormsg + "Old password cannot be blank.</br>";
    }

    if ($("#txtnewpwd").val() == "") {
        iserror = true;
        errormsg = errormsg + "New password cannot be blank.</br>";
    }

    if ($("#txtconfirmpwd").val() == "") {
        iserror: true;
        errormsg = errormsg + "Confirm Password cannot be blank.</br>";
    }

    if ($("#txtnewpwd").val() != $("#txtconfirmpwd").val()) {
        iserror = true;
        errormsg = errormsg + "New and confirm passsord not matched.</br>";
    }

    var resu = validatePassword_();
    if (resu == false) {
        return false;
    }

    //var _encryptedpwd = CryptoJS.AES.encrypt("encryptpwd", $("#txtconfirmpwd").val()).toString();;

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }



    var key1 = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv1 = CryptoJS.enc.Utf8.parse('8080808080808080');


    var encryptedoldpwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($("#txtoldpwd").val()), key1,
        {
            keySize: 128 / 8,
            iv: iv1,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    var encryptednewpwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($("#txtconfirmpwd").val()), key1,
        {
            keySize: 128 / 8,
            iv: iv1,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    var mydata = {
        user_id: user_id,
        // username: login_user_id_dec,
        old_password: encryptedoldpwd.toString(),//$("#txtoldpwd").val(),
        new_password: encryptednewpwd.toString(),//CryptoJS.AES.encrypt("'" + $("#txtconfirmpwd").val() + "'", localStorage.getItem("sit_id")).toString(),
        last_modified_by: login_emp_id,
        default_company_id: default_company
    }


    $('#loader').show();

    var headers = {};
    headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headers["salt"] = $("#hdnsalt").val();

    var Obj = JSON.stringify(mydata);


    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/ChangeUserPassword",
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headers,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                //GetData();
                //messageBox("success", Msg);
                alert(Msg);
                location.reload();

            }
            else if (statuscode == "1" || statuscode == '2') {

                messageBox("error", Msg);
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

        }

    });
});

$("#btnreset").bind("click", function () {
    location.reload();
});