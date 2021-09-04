$('#loader').show();

var login_emp_id;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        // debugger;
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        $('#btnreset').show();
        $('#btnsave').show();
        $('#btnupdate').hide();

        GetData();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/KeyMaster';

        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            // debugger;
            var key = $("#txtkey").val();
            var display = $('#txtdisplay').val();
            var func_name = $('#txtfunc_name').val();
            var datatype = $('#ddltype').val();
            var formula_name = $('#txtformula').val();
            var description = $('#txtdescription').val();


            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            //validation part
            if (key == '' || key == null) {
                errormsg = "Please enter Key Name !! <br/>";
                iserror = true;
            }
            if (display == '' || display == null) {
                errormsg = errormsg + 'Please enter Key display Name !! <br />';
                iserror = true;
            }
            if (func_name == '' || func_name == null) {
                errormsg = errormsg + 'Please enter calling function Name !! <br />';
                iserror = true;
            }
            if (datatype == '' || datatype == '0') {
                errormsg = "Please select Data type !! <br/>";
                iserror = true;
            }
            if (formula_name == '' || formula_name == null) {
                errormsg = errormsg + 'Please enter Formula Name !! <br />';
                iserror = true;
            }
            if (description == '' || description == null) {
                errormsg = errormsg + 'Please enter Key description !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'key_name': key,
                'display_name': display,
                'description': description,
                'calling_function_name': func_name,
                'data_type': datatype,
                'forumal_name': formula_name,
                'is_active': is_active,
                'created_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_KeyMaster';
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
                        $("#txtkey").val('');
                        $('#txtdisplay').val('');
                        $('#txtdescription').val('');
                        $('#txtfunc_name').val('');
                        $('#ddltype').val('0');
                        $('#txtformula').val('');
                        $('#btnupdate').hide();

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


        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var id = $("#hdnid").val();
            var key = $("#txtkey").val();
            var display = $('#txtdisplay').val();
            var func_name = $('#txtfunc_name').val();
            var datatype = $('#ddltype').val();
            var formula_name = $('#txtformula').val();
            var description = $('#txtdescription').val();


            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            //validation part
            if (key == '' || key == null) {
                errormsg = "Please enter Key Name !! <br/>";
                iserror = true;
            }
            if (display == '' || display == null) {
                errormsg = errormsg + 'Please enter Key display Name !! <br />';
                iserror = true;
            }
            if (func_name == '' || func_name == null) {
                errormsg = errormsg + 'Please enter calling function Name !! <br />';
                iserror = true;
            }
            if (datatype == '' || datatype == '0') {
                errormsg = "Please select Data type !! <br/>";
                iserror = true;
            }
            if (formula_name == '' || formula_name == null) {
                errormsg = errormsg + 'Please enter Formula Name !! <br />';
                iserror = true;
            }
            if (description == '' || description == null) {
                errormsg = errormsg + 'Please enter Key description !! <br />';
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

                'key_id': id,
                'key_name': key,
                'display_name': display,
                'description': description,
                'calling_function_name': func_name,
                'data_type': datatype,
                'forumal_name': formula_name,
                'is_active': is_active,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_KeyMaster';
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
                        $("#txtkey").val('');
                        $('#txtdisplay').val('');
                        $('#txtdescription').val('');
                        $('#txtfunc_name').val('');
                        $('#ddltype').val('0');
                        $('#txtformula').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
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


//--------bind data in jquery data table
function GetData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_KeyMaster/0';
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $('#loader').hide();
            $("#tblkeyMaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [

                        //{
                        //    targets: [2],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},                      
                        {
                            targets: [5],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": "key_id", "name": "key_id", "autoWidth": true },
                    { "data": "key_name", "name": "key_name", "autoWidth": true },
                    { "data": "display_name", "name": "display_name", "autoWidth": true },
                    { "data": "calling_function_name", "name": "calling_function_name", "autoWidth": true },
                    { "data": "forumal_name", "name": "forumal_name", "autoWidth": true },
                    { "data": "description", "name": "description", "autoWidth": true },
                    { "data": "data_type", "name": "data_type", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.key_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            //alert(error);
            messageBox("error", error.responseText);
            $('#loader').hide();
            console.log("error");
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

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_KeyMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtkey").val(data.key_name);
            $('#txtdisplay').val(data.display_name);
            $('#txtfunc_name').val(data.calling_function_name);
            $('#ddltype').val(data.data_type);
            $('#txtformula').val(data.forumal_name);
            $('#txtdescription').val(data.description);

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
