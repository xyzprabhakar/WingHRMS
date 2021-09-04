$('#loader').show();

var login_emp_id;
var emp_role_idd;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;


        GetData();

        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            $("#txtdesignation").val('');
            $('input:radio[name=chkstatus]:checked').prop('checked', false);

            $('#btnupdate').hide();
            $('#btnsave').show();
            $("#hdnid").val('');
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var designation_name = $.trim($("#txtdesignation").val());

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (designation_name == null || designation_name == '') {
                errormsg = "Please enter designation name !! <br/>";
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

                'designation_name': designation_name.trim(),
                'is_active': is_active,
                'created_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_DesignationMaster';
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
                        $("#txtdesignation").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        GetData();
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);
                    $('#loader').hide();
                }

            });
        });


        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var designation_name = $.trim($("#txtdesignation").val());
            var designation_id = $("#hdnid").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            if (designation_id == 0 || designation_id == '' || designation_id == null) {
                errormsg = "Invalid designation id , please reload page and try again !! ";
                iserror = true;
            }
            //validation part
            if (designation_name == null || designation_name == '') {
                errormsg = "Please enter designation name !! <br/>";
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
            $("btnupdate").text('Please wait').html('<i class="fa fa-spinner"></i>').prop("disabled", true);
            var myData = {

                'designation_name': designation_name.trim(),
                'designation_id': designation_id,
                'is_active': is_active,
                'last_modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_DesignationMaster';
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
                        $("#txtdesignation").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').prop("disabled", false);
                        GetData();
                        //alert(Msg);
                        //location.reload();

                        messageBox("success", Msg);

                        //GetData();
                        //messageBox("success", Msg);

                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("btnupdate").text('Update').prop("disabled", false);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);
                    $('#loader').hide();
                }

            });
        });

    }, 2000);// end timeout


});

//--------bind data in jquery data table
function GetData() {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_DesignationMaster/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },

        success: function (res) {
            $('#loader').hide();
            $("#tbldesignation").DataTable({
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
                    { "data": "designationname", "name": "designationname", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },
                    { "data": "modifiedon", "name": "modifiedon", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.designationid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
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



    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_DesignationMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtdesignation").val(data.designation_name);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);
            $("#hdnid").val(id);
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

