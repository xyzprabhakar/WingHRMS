$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        var qs = getQueryStrings();
        var company_id = qs["company_id"];

        if (token == null && company_id == 'undefined') {
            window.location = '/Login';
        }



        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        //var HaveDisplay = ISDisplayMenu("Display Company List");
        BindCompanyListAll('ddlcompany', login_emp_id, 0);
        setSelect('ddlcompany', default_company);

        GetData(default_company);







        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();



        $('#btnsave').bind("click", function () {


            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtEventName = $("#txtEventName").val().trim();
            var txtEventDate = $("#txtEventDate").val();
            var txtEventTime = $("#txtEventTime").val();
            var txtEventPlace = $("#txtEventPlace").val();

            //var HaveDisplay = ISDisplayMenu("Display Company List");

            //if (HaveDisplay == 0) {
            //    ddlcompany = default_company;
            //}

            if (ddlcompany == "0" || ddlcompany == null || ddlcompany == '') {
                messageBox("error", "Please Select Company ....!");
                $("#ddlcompany").val('');
                $('#loader').hide();
                return;
            }

            if (txtEventName == "" || txtEventName == null) {
                messageBox("error", "Please Enter Event Name....!");
                $("#txtEventName").val('');
                $('#loader').hide();
                return;
            }


            if (txtEventDate == "" || txtEventDate == null) {
                messageBox("error", "Please Enter Event Date....!");
                $("#txtEventDate").val('');
                $('#loader').hide();
                return;
            }
            if (txtEventTime == "" || txtEventTime == null) {
                messageBox("error", "Please Enter Event Time....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            if (txtEventPlace == "" || txtEventPlace == null) {
                messageBox("error", "Please Event Place....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            var is_active = 0;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //var emp_id = localStorage.getItem('emp_id');


            var myData = {
                'company_id': ddlcompany,
                'event_name': txtEventName,
                'event_date': txtEventDate,
                'event_time': txtEventTime,
                'is_active': is_active,
                'created_by': login_emp_id,
                'event_place': txtEventPlace
            };
            //  debugger;
            //var csrfToken = $.cookie("CSRF-TOKEN");

            var csrftoken = getCSRFToken();

            var headers = {};
            headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headers["salt"] = $("#hdnsalt").val();
            // var headerss = {};
            // headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            //// headerss["X-CSRF-TOKEN"] = csrftoken;
            // headerss["salt"] = $("#hdnsalt").val();
            var obj = JSON.stringify(myData);
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Save_Event',
                type: "POST",
                data: obj,
                dataType: "json",
                contentType: "application/json",
                headers: headers,
                // headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                //headers: { "X-CSRF-TOKEN": csrftoken },
                //headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    _GUID_New();
                    //if data save
                    if (statuscode == "0") {
                        $("#txtEventName").val('');
                        $("#txtEventDate").val('');
                        $("#txtEventTime").val('');
                        $("#txtEventPlace").val('');
                        $("#hdnid").val('');
                        GetData(ddlcompany);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('#loader').hide();
                        messageBox("success", Msg);
                        // alert(Msg);
                        //location.reload();
                    }
                    else {
                        $('#loader').hide();
                        messageBox("error", Msg);
                        return false;
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

                }

            });


        });

        $("#ddlcompany").bind("change", function () {
            $("#loader").show();
            GetData($(this).val());
            $("#loader").hide();
        });


        $('#btnReset').bind("click", function () {
            location.reload();

        });



        //-------update city data
        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtEventName = $("#txtEventName").val().trim();
            var txtEventDate = $("#txtEventDate").val();
            var txtEventTime = $("#txtEventTime").val();
            var txtEventPlace = $("#txtEventPlace").val();

            //var HaveDisplay = ISDisplayMenu("Display Company List");

            //if (HaveDisplay == 0) {
            //    ddlcompany = default_company;
            //}
            if (ddlcompany == "0" || ddlcompany == null) {
                messageBox("error", "Please Select Company ....!");
                $("#ddlcompany").val('');
                $('#loader').hide();
                return;
            }

            if (txtEventName == "" || txtEventName == null) {
                messageBox("error", "Please Enter Event Name....!");
                $("#txtEventName").val('');
                $('#loader').hide();
                return;
            }


            if (txtEventDate == "" || txtEventDate == null) {
                messageBox("error", "Please Enter Event Date....!");
                $("#txtEventDate").val('');
                $('#loader').hide();
                return;
            }
            if (txtEventTime == "" || txtEventTime == null) {
                messageBox("error", "Please Enter Event Time....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            if (txtEventPlace == "" || txtEventPlace == null) {
                messageBox("error", "Please Event Place....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            var is_active = 0;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //var emp_id = localStorage.getItem('emp_id');

            var event_id = $("#hdnid").val();

            var myData = {
                'event_id': event_id,
                'company_id': ddlcompany,
                'event_name': txtEventName,
                'event_date': txtEventDate,
                'event_time': txtEventTime,
                'is_active': is_active,
                'created_by': login_emp_id,
                'event_place': txtEventPlace
            };
            var Obj = JSON.stringify(myData);
            console.log(Obj);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Update
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Update_Event',
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    _GUID_New();
                    //if data save
                    if (statuscode == "0") {
                        $("#txtEventName").val('');
                        $("#txtEventDate").val('');
                        $("#txtEventTime").val('');
                        $("#txtEventPlace").val('');
                        $("#hdnid").val('');
                        GetData(ddlcompany);

                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('#loader').hide();
                        messageBox("success", Msg);

                        // alert(Msg);
                        //location.reload();

                    }
                    else {
                        $('#loader').hide();
                        messageBox("error", Msg);
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

                }

            });

        });

    }, 2000);// end timeout

});

function getCSRFToken() {
    var cookieValue = null;
    if (document.cookie && document.cookie != '') {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = jQuery.trim(cookies[i]);
            if (cookie.substring(0, 10) == ('csrftoken' + '=')) {
                cookieValue = decodeURIComponent(cookie.substring(10));
                break;
            }
        }
    }
    return cookieValue;
}



function GetData(companyidd) {

    // var company_id = localStorage.getItem("company_id")

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_Event_by_id/0/' + companyidd;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            ////// debugger;;;
            $("#tblevent_list").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [2],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],
                "columns": [
                    { "data": null },
                    { "data": "event_name", "name": "event_name", "autoWidth": true },
                    { "data": "event_date", "name": "event_date", "autoWidth": true },
                    { "data": "event_time", "name": "event_time", "autoWidth": true },
                    { "data": "event_place", "name": "event_place", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetDataById(' + full.event_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });
            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}

function GetDateFormatMMddyyyy(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return month + '/' + day + '/' + date.getFullYear();
}

function GetDataById(event_id) {

    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_Event_by_id/' + event_id + '/0';

    $.ajax({
        url: apiurl,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;
            BindCompanyListAll('ddlcompany', login_emp_id, 0);
            setSelect('ddlcompany', res[0].company_id);

            //$("#ddlcompany").val(res[0].company_id);
            $("#txtEventName").val(res[0].event_name);


            var DateEventDate = new Date(res[0].event_date);

            $("#txtEventDate").val(GetDateFormatMMddyyyy(DateEventDate));

            $("#txtEventTime").val(GetTimeFromDate(res[0].event_time));

            $("#txtEventPlace").val(res[0].event_place);

            if (res[0].is_active == 1) {
                $("#is_active").attr('checked', 'checked');
            }
            else {
                $("#is_in_active").attr('checked', 'checked');
            }
            $("#hdnid").val(event_id);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
}


