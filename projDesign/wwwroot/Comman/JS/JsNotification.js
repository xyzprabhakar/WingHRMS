$(document).ready(function () {

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












//function UpdateNotification(company_id) {
//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEventNotification/" + company_id,
//        type: "GET",
//        headers: { 'Authorization': 'Bearer' + localStorage.getItem("Token") },
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        success: function (response) {
//            var res = response;



//            var j = ""; var p = 0; var Q = "";


//            //$("#myNotifyList").html("");
//           // $("#myNotifyList").append(j);


//            $.each(response, function (k, v) {
//                p++;
//                if (p <= response.length) {
//                  //  j += "<li><a><strong>  " + v.event_name.toString() + " </strong> <br /> <div style='text-align:right;font-size:smaller;font-style:italic'> <i style='font-size:smaller'> updated : " + v.event_name.toString() + "</i></div></a></li>";
//                }
//               // Q += "<a><img src='assets/img/nty.png' style='height:20px;width:20px'/> <strong>  " + v.event_date.toString() + " </strong> <br /> <div style='text-align:right;font-size:smaller;font-style:italic'> <i style='font-size:smaller'> updated : " + v.LastUpdated.toString() + "</i></div></a>";
//            });
//            $('#myNotifyList').html("");
//           j += "<li><a data-toggle='modal' data-target='#myModal'><div style='text-align:center'><strong> Show more </strong></div></li>";
//            $('#myNotifyList').append(j);
//            $("#countNotify").html(response.length);
//           // $('#myNotifyList').hide();
//           // $('#countNotify').html("<span>" + p + "</span>");
//           // $('#myAllNotifications').html("");
//           // $('#myAllNotifications').append(Q);  


//        }

//    });
//}

