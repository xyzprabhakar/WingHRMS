$('#loader').show();

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        GetLastOtId();
        GetLastCombOffId();
        $('#loader').hide();



        // Save OT Rules
        $('#btnSaveOt').bind("click", function () {
            $('#loader').show();
            var GraceWorkingHour = $("#txtGraceWorkingHour").val();

            //Validation
            if (GraceWorkingHour == '') {
                messageBox("error", "Please Enter Working Hour And Minutes...!");
                $('#loader').hide();
                return;
            }


            // debugger
            var myData = {
                'grace_working_hour': HoursOnly(GraceWorkingHour),
                'grace_working_minute': MinutesOnly(GraceWorkingHour),
            };

            var data = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiShift/SaveOtRules',
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "0") {
                        messageBox("error", "Server busy please try again later...!");
                    }
                },
                error: function (err, exception) {
                    _GUID_New();
                    //alert(JSON.stringify(err));
                    messageBox("error", "Server busy please try again later...!");
                    $('#loader').hide();
                }
            });
        });

        // Save Comb Off Rules
        $('#btnSaveCombOffRules').bind("click", function () {
            $('#loader').show();
            var MinimumWorkingHours = $("#txtMinimumWorkingHours").val();

            //Validation
            if (MinimumWorkingHours == '') {
                messageBox("error", "Please Enter Minimum Working Hours And Minutes...!");
                $('#loader').hide();
                return;
            }

            var myData = {
                'minimum_working_hours': HoursOnly(MinimumWorkingHours),
                'minimum_working_minute': MinutesOnly(MinimumWorkingHours),
            };

            var vdata = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiShift/SaveCombOff',
                type: "POST",
                data: vdata,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "0") {
                        alert(Msg);
                    }
                },
                error: function (err, exception) {
                    _GUID_New();
                    messageBox("error", "Server busy please try again later...!");
                }
            });

        });

        $('#OTRules').bind("click", function () {
            $('#DivCombOffRule').css('display', 'none');
            $('#DivOtRule').css('display', 'block');
        });
        $('#CompOffRule').bind("click", function () {
            $('#DivCombOffRule').css('display', 'block');
            $('#DivOtRule').css('display', 'none');
        });

    }, 2000);// end timeout

});


function HoursOnly(time) {


    if (time.value !== "") {
        var hours = time.split(":")[0];
        hours = hours % 12 || 12;
        hours = hours < 10 ? "0" + hours : hours;
        return hours
    }
}

function MinutesOnly(time) {

    if (time.value !== "") {

        var minutes = time.split(":")[1];
        return minutes
    }
}




//Get Last Ot Id
function GetLastOtId() {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiShift/GetLastOtId',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: {},
        success: function (data) {
            var res = data;
            var grace_working_hour = res.grace_working_hour
            var grace_working_minute = res.grace_working_minute

            var fTime = grace_working_hour + ':' + grace_working_minute;
            $("#txtGraceWorkingHour").val(fTime);

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });

}


//Get Last Comb off Id
function GetLastCombOffId() {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiShift/GetLastCombOffId',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: {},
        success: function (data) {
            var res = data;
            var grace_working_hour = res.minimum_working_hours
            var grace_working_minute = res.minimum_working_minute

            var fTime = grace_working_hour + ':' + grace_working_minute;
            $("#txtMinimumWorkingHours").val(fTime);

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });

}