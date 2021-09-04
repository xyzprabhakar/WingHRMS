$('#loader').show();

var default_company;
var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAppSetting();
        BindData();

        $('#btnsave').hide();
        $('#loader').hide();

    }, 2000);// end timeout

});

function BindAppSetting() {
    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AppSettings';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            debugger;
            var aData = res;
            $("#ddlAppSettName").empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $("#ddlAppSettName").append($("<option></option>").val(value.appSettingKey).html(value.appSettingKeyDisplay));
            })



            $('#loader').hide();
        },
        error: function (error) {

            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function BindData() {
    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AppSettings';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var aData = res;

            $("#tblReport").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "scrollX": 800,
                "aaData": aData,
                "columnDefs":
                    [
                        //{
                        //    targets: [4],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "title": "Name", "data": "appSettingKeyDisplay", "name": "appSettingKeyDisplay", "autoWidth": true, },
                    { "title": "Value", "data": "appSettingValue", "name": "appSettingValue", "autoWidth": true, },
                    //{ "title": "Created By", "data": "_created_by", "name": "_created_by", "autoWidth": true, },
                    //{ "title": "Created Date", "data": "created_dt", "name": "created_dt", "autoWidth": true, },
                    //{ "title": "Status", "data": "_is_active", "name": "_is_active", "autoWidth": true, },
                    //{
                    //    "title": "Edit", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<a href="#" onclick="GetEditData(' + full.pkid_setting + ')" data-toggle="tooltip"><i class="fa fa-pencil-square-o"></i></a>';

                    //    }
                    //},
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a  onclick="DeleteData(' + full.pkid_setting + ' )"> <i class="fa fa-trash"></i></a > ';
                    //    }
                    //},
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
            $('#loader').hide();
        },
        error: function (error) {

            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

$('#btnsave').bind("click", function () {

    var name = $('#ddlAppSettName').val();
    var value = $('#ddlAppSettValue').val();

    if (name == null || name == '') {
        errormsg = "App Setting Name is required...! ";
        $('#ddlAppSettName').focus();
        return false;
    }
    if (value == null || value == '') {
        errormsg = "App Setting Value is required...! ";
        $('#ddlAppSettValue').focus();
        return false;
    }

    var myData = {
        'AppSettingKey': name,
        'AppSettingValue': value,
        'created_by': login_emp_id
    };

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_ApplicationSetting';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else {
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

function GetEditData(request_id) {
    debugger;

    if (request_id == null || request_id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_ApplicationSettingById/' + request_id;
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            debugger;
            reset_all();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }
            if (res.appSettingKey != null) {
                $('#ddlAppSettName').val(res.appSettingKey);
            }
            if (res.appSettingValue != null) {
                $('#ddlAppSettValue').val(res.appSettingValue);
            }

            $("#hdnid").val(request_id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText)
            $('#loader').hide();
        }
    });


}

function DeleteData(request_id) {
    $("#loader").show();
    debugger;

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Delete_ApplicationSettingById/' + request_id;

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {
        $.ajax({
            url: apiurl,
            type: "POST",
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                reset_all();
                BindData();

                $("#loader").hide();
                if (statuscode == "0") {
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("info", msg);
                    return false;
                }
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}

// Update Data
$("#btnupdate").bind("click", function () {
    debugger;
    var name = $('#ddlAppSettName').val();
    var value = $('#ddlAppSettValue').val();
    //var id = $('#hdnid').val();

    if (name == null || name == '') {
        errormsg = "App Setting Name is required...! ";
        $('#ddlAppSettName').focus();
        return false;
    }
    if (value == null || value == '') {
        errormsg = "App Setting Value is required...! ";
        $('#ddlAppSettValue').focus();
        return false;
    }

    var myData = {
        //'pkid_setting': id,
        'AppSettingKey': name,
        'AppSettingValue': value,
        'created_by': login_emp_id
    };

    $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh fa-spin"></i> Please wait..');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_ApplicationSetting';
    var Obj = JSON.stringify(myData);

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: headerss,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                $("#btnupdate").attr("disabled", false);
                $('#btnupdate').hide();
                $('#btnsave').show();
                alert(Msg);
                location.reload();
            }
            else {
                messageBox("error", Msg);
                $("#btnupdate").text('Update').attr("disabled", false);
            }
        },
        error: function (request, status, error) {
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
            $("#btnupdate").text('Update').attr("disabled", false);
            $('#loader').hide();
        }
    });
});



function reset_all() {
    $('#ddlAppSettName').val(0);
    $('#ddlAppSettValue').val(0);
    $('#hdnid').val('');
}