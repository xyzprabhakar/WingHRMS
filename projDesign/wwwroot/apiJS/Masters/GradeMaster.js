$('#loader').show();
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        $('#btnupdate').hide();
        $('#btnsave').show();
        GetData();

        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            $("#txtgrade").val('');
            $('input:radio[name=chkstatus]:checked').prop('checked', false);
            $('#btnupdate').hide();
            $('#btnsave').show();
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var grade_name = $.trim($("#txtgrade").val());

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (grade_name == null || grade_name == '') {
                errormsg = "Please enter grade name !! <br/>";
                iserror = true;
            }
            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'grade_name': grade_name.trim(),
                'is_active': is_active,
                'created_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_GradeMaster';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtgrade").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        // alert(Msg);
                        GetData();
                        messageBox("success", Msg);
                        //location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });
        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var grade_name = $.trim($("#txtgrade").val());
            var grade_id = $("#hdnid").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            if (grade_id == 0 || grade_id == '' || grade_id == null) {
                errormsg = "Invalid grade id , please reload page and try again !! <br/>";
                iserror = true;
            }
            //validation part
            if (grade_name == null || grade_name == '') {
                errormsg = "Please enter grade name !! <br/>";
                iserror = true;
            }
            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                // messageBox("success", "successfully save data");
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'grade_name': grade_name.trim(),
                'grade_id': grade_id,
                'is_active': is_active,
                'last_modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_GradeMaster';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#hdnid").val('');
                        $("#txtgrade").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        //alert(Msg);
                        //location.reload();
                        GetData();
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
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

//--------bind data in jquery data table
function GetData() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_GradeMasterData/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblgrade").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [2],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return new Date(row.modifiedon) < new Date(row.createdon) ? "-" : GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "gradename", "name": "gradename", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },
                    { "data": "modifiedon", "name": "modifiedon", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.gradeid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $("#loader").hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $("#loader").hide();
        }
    });

}

function GetEditData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_GradeMasterData/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtgrade").val(data.grade_name);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);

            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}


//-------update city data

