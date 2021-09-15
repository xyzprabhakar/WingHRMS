$(document).ready(function () {
    user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    
    var company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    
    UpdateNotification(company_id);
    function UpdateNotification(company_id) {

        if (company_id == 0) {
            company_id = 1;
        }
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEventNotification/" + company_id,
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: "{}",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                var j = "";

                if (res.statusCode != undefined) {
                    $("#notificationlist").html();
                    $("#notificationlist").append(j);
                    messageBox("error", res.message);
                    return false;
                }

                if (res.length > 0) {
                    for (var i = 0; i < response.length; i++) {
                        j += "<li><img src='/images/blink.gif'/>" + response[i].event_name + "-" + response[i].event_place + "(" + response[i].event_date_time + ")</li></br>";
                    }
                }

                $("#notificationlist").html();
                $("#notificationlist").append(j);
            },
            error: function (err) {
                Customajaxerror(err);
            }

        });
    }

});








