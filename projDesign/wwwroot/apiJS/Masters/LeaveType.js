$('#loader').show();

var login_emp_id;
var emp_role_id;
var default_company;


$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        GetData();


        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            $("#txtleavetype").val('');
            $('#txtdescription').val('');
            $('input:radio[name=chkstatus]:checked').prop('checked', false);

            $('#btnupdate').hide();
            $('#btnsave').show();
            $("#hdnid").val('');
        });

        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var leave_type_name = $.trim($("#txtleavetype").val());
            var description = $.trim($('#txtdescription').val());

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (leave_type_name == null || leave_type_name == '') {
                errormsg = "Please enter leave type name !! <br/>";
                iserror = true;
            }
            //if (description == null || description == '') {
            //    errormsg = errormsg + "Please enter leave description !! <br/>";
            //    iserror = true;
            //}
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

                'leave_type_name': leave_type_name.trim(),
                'description': description,
                'is_active': is_active
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_LeaveTypeMaster';
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
                        //$("#txtleavetype").val('');
                        //$('#txtdescription').val('');
                        //$('input:radio[name=chkstatus]:checked').prop('checked', false);

                        //GetData();
                        //messageBox("success", Msg);
                        alert(Msg)
                        location.reload();
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
            var leave_type_name = $.trim($("#txtleavetype").val());
            var description = $.trim($('#txtdescription').val());
            var leave_type_id = $('#hdnid').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            //validation part
            if (leave_type_name == null || leave_type_name == '') {
                errormsg = "Please enter leave type name !! <br/>";
                iserror = true;
            }
            //if (description == null || description == '') {
            //    errormsg = errormsg + "Please enter leave description !! <br/>";
            //    iserror = true;
            //}
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

                'leave_type_id': leave_type_id,
                'leave_type_name': leave_type_name.trim(),
                'description': description,
                'is_active': is_active
            };

            $("#btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_LeaveTypeMaster';
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
                        //$("#hdnid").val('');
                        //$("#txtleavetype").val('');
                        //$('#txtdescription').val('');
                        //$('input:radio[name=chkstatus]:checked').prop('checked', false);

                        //$('#btnupdate').hide();
                        //$('#btnsave').show();
                        //GetData();
                        //messageBox("success", Msg);
                        //$("#btnupdate").text('Update').attr("disabled", false);
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("#btnupdate").text('Update').attr("disabled", false);
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

    //var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_LeaveTypeMaster/0';
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AllLeaveType/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var dt=$("#tblleavetype").DataTable({
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
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? `<spam class="badge badge-success">Active</spam>` : '<spam class="badge badge-danger">InActive</spam>'
                            }
                        },
                        {
                            targets: [11],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        
                        {
                            targets: [12],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "leavetype", "name": "leavetype", "autoWidth": true },
                    { "data": "descriptions", "name": "descriptions", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },
                    { "data": "modifiedon", "name": "modifiedon", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {                            
                            return '<a href="#" onclick="GetEditData(' + full.leavetypeid + ')" ><i class="fa fa-pencil-square-o"></i></a>';                            
                        }
                    }
                    
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],                
            });
            dt.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        },
        error: function (error) {
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

    //var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_LeaveTypeMaster/' + id;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AllLeaveType/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtleavetype").val(data.leave_type_name.trim());
            $('#txtdescription').val(data.description);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);

            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            //alert(err.responseText)
            $('#loader').hide();
        }
    });


}

//delete data
function DeleteData(id) {
    $('#loader').show();
    if (confirm("Do you want to delete this?")) {

        if (id == null || id == '') {
            messageBox('info', 'There some problem please try after later !!');
            $('#loader').hide();
            return false;
        }

        var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Delete_LeaveTypeMaster/' + id;
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({
            type: "DELETE",
            url: apiurl,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: headerss,
            success: function (res) {

                data = res;
                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                if (statuscode == "0") {
                    GetData();
                    messageBox("success", Msg);
                    $("#hdnid").val('');
                }
                else {
                    messageBox("error", Msg);
                }

            },
            error: function (err) {
                _GUID_New();
                messageBox("error", err.responseText);
                //alert(err.responseText);
                $('#loader').hide();
            }
        });
    }
    else {
        $('#loader').hide();
        return false;
    }
}

//-------update city data

