$('#loader').show();
var login_emp_id;
var login_company_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        $('#Divhhmm').hide();
        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);

        GetData();



        $('#ddlcompany').bind('change', function () {
            BindLocationList('ddllocation', $(this).val(), 0);
            BindDepartmentList('ddldepartment', $(this).val(), 0);
        });

        BindReligionList('ddlReligion', 0);
        BindLeaveType('ddlleavename', 0);
        $('#btnupdate').hide();
        $('#btnsave').show();

        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
                $("#txttodate").datepicker('setDate', null);
                $("#txttodate").datepicker({
                    dateFormat: 'mm/dd/yy',
                    minDate: fromselected,
                    onSelect: function (toselected, evnt) {

                        if (Date.parse(fromselected) >= Date.parse(toselected)) {
                            messageBox('error', 'To Date must be greater than from date !!');
                            //$('#txtfromdate').val('');
                            $('#txttodate').val('');
                        }
                    }
                });
            }
            //changeMonth: true,
            //changeYear: true
        });

        $('#ddlapplicable').bind('change', function () {
            // debugger;
            if ($(this).val() == '3') {
                $('#Divhhmm').show();
            }
            else {
                $('#Divhhmm').hide();
            }
        });


        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            //$("#ddlleavename").val('0');
            //$('#ddlleavetype').val('0');
            //$('#txtfromdate').val('');
            //$('#txttodate').val('');

            //$('#btnupdate').hide();
            //$('#btnsave').show();
            //$("#hdnid").val('');
            location.reload();
        });

        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var leave_type_id = $("#ddlleavename").val();
            var leave_type = $('#ddlleavetype').val();
            var leave_tenure_from_date = $('#txtfromdate').val();
            var leave_tenure_to_date = $('#txttodate').val();

            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (leave_type_id == null || leave_type_id == '0') {
                errormsg = "Please select leave name !! <br/>";
                iserror = true;
            }
            if (leave_type == null || leave_type == '0') {
                errormsg = errormsg + "Please select leave type !! <br/>";
                iserror = true;
            }
            if (leave_tenure_from_date == null || leave_tenure_from_date == '') {
                errormsg = errormsg + "Please select leave tenure from date !! <br/>";
                iserror = true;
            }
            if (leave_tenure_to_date == null || leave_tenure_to_date == '') {
                errormsg = errormsg + "Please select leave tenure to date !! <br/>";
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
                'leave_type': leave_type,
                'leave_tenure_from_date': leave_tenure_from_date,
                'leave_tenure_to_date': leave_tenure_to_date
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/';
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
                        alert(Msg);
                        location.reload();
                        //$("#ddlleavename").val('0');
                        //$('#ddlleavetype').val('0');
                        //$('#txtfromdate').val('');
                        //$('#txttodate').val('');

                        //GetData();
                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
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
                    $('#loader').hide();
                }

            });
        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var leave_info_id = $('#hdnid').val();
            var leave_type_id = $("#ddlleavename").val();
            var leave_type = $('#ddlleavetype').val();
            var leave_tenure_from_date = $('#txtfromdate').val();
            var leave_tenure_to_date = $('#txttodate').val();

            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (leave_type_id == null || leave_type_id == '0') {
                errormsg = "Please select leave name !! <br/>";
                iserror = true;
            }
            if (leave_type == null || leave_type == '0') {
                errormsg = errormsg + "Please select leave type !! <br/>";
                iserror = true;
            }
            if (leave_tenure_from_date == null || leave_tenure_from_date == '') {
                errormsg = errormsg + "Please select leave tenure from date !! <br/>";
                iserror = true;
            }
            if (leave_tenure_to_date == null || leave_tenure_to_date == '') {
                errormsg = errormsg + "Please select leave tenure to date !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'leave_info_id': leave_info_id,
                'leave_type_id': leave_type_id,
                'leave_type': leave_type,
                'leave_tenure_from_date': leave_tenure_from_date,
                'leave_tenure_to_date': leave_tenure_to_date
            };

            $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh"></i> Please wait..');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave';
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
                        $("#ddlleavename").val('0');
                        $('#ddlleavetype').val('0');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');

                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        GetData();
                        //messageBox("success", Msg);
                        $("#btnupdate").text('Update').attr("disabled", false);
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("#btnupdate").text('Update').attr("disabled", false);
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
                    return false;
                }
            });
        });


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData() {


    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/0';
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblleaveinfo").DataTable({
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
                            targets: [1],
                            render: function (data, type, row) {
                                return data == '1' ? 'Paid' : 'UnPaid'
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
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "leavetype", "name": "leavetype", "autoWidth": true },
                    { "data": "leavetypename", "name": "leavetypename", "autoWidth": true },
                    { "data": "fromdate", "name": "fromdate", "autoWidth": true },
                    { "data": "todate", "name": "todate", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="GetEditData(' + full.leaveinfoid + ')" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    }
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" id="A1" onclick="DeleteData(' + full.leaveinfoid + ')" ><i class="fa fa-trash"></i></a>';

                    //    }
                    //}
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
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

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (data.leave_tenure_from_date != null && data.leave_tenure_from_date != '') {
                let a = new Date(data.leave_tenure_from_date);
                a = GetDateFormatddMMyyyy(a);
                $('#txtfromdate').val(a);
            }
            if (data.leave_tenure_to_date != null && data.leave_tenure_to_date != '') {
                let b = new Date(data.leave_tenure_to_date);
                b = GetDateFormatddMMyyyy(b);
                $('#txttodate').val(b);
                $("#txttodate").datepicker({
                    dateFormat: 'mm/dd/yy',
                    minDate: b
                });
            }
            BindLeaveType('ddlleavename', data.leave_type_id);
            $('#ddlleavetype').val(data.leave_type);


            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();

        },
        Error: function (err) {
            alert(err.responseText);
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

        var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/' + id;
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
                    return false;
                }
                else {
                    messageBox("error", Msg);
                    return false;
                }

            },
            error: function (err) {
                _GUID_New();
                alert(err.responseText)
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

