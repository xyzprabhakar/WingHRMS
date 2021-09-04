$('#loader').show();
var company_id;
var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        // HaveDisplay = ISDisplayMenu("Is Company Admin");

        BindAllEmp_Company('ddlcompany', login_emp_id, 0);
        setSelect('ddlcompany', company_id);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, 0);
        setSelect('ddlEmployeeCode', login_emp_id);

        $('#btnupdate').hide();
        $('#btnsave').show();

        GetData(login_emp_id);

        $('#loader').hide();



        $('#btnGetDetail').bind('click', function () {

            var errormsg = '';
            var iserror = false;

            var ddlcompany = $("#ddlcompany").val();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
            var txtRegularizeDate = $("#txtRegularizeDate").val();
            //var Nin = $("#txtInTime").val().text();
            //var Nout = $("#txtOutTime").val().text();

            //validation part
            //if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
            //    errormsg = "Please Select Employee Code...! ";
            //    iserror = true;
            //}

            if (ddlcompany == "" || ddlcompany == null || ddlcompany == "0") {
                errormsg = errormsg + "Please select comapny...!</br>";
                iserror = true;
            }

            if (ddlEmployeeCode == "" || ddlEmployeeCode == null || ddlEmployeeCode == "0") {
                errormsg = errormsg + "Employee Code cannot be blank...!</br>";
                iserror = true;
            }

            if (txtRegularizeDate == null || txtRegularizeDate == '') {
                errormsg = errormsg + "Please Select Regularize Date ...! ";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);

                return false;
            }
            $('#loader').show();
            GetInOrOutTimeByDate('lblInTime', txtRegularizeDate, ddlEmployeeCode);
            $('#loader').hide();
        });

        $('#btnreset').bind('click', function () {

            location.reload();

        });


        $('#btnsave').bind("click", function () {

            var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
            var companyid = $("#ddlcompany").val();
            var txtRegularizeDate = $("#txtRegularizeDate").val();
            var lblInTime = txtRegularizeDate.toString() + ' ' + $("#lblInTime").text(); //'2000-01-01 ' + $("#lblInTime").text();
            var lblOutTime = txtRegularizeDate.toString() + ' ' + $("#lblOutTime").text(); //'2000-01-01 ' + $("#lblOutTime").text();
            var txtInTime = $("#txtInTime").val();
            var txtOutTime = $("#txtOutTime").val();
            var txtRemarks = $("#txtremarks").val();

            var dateregularise = new Date($('#txtRegularizeDate').val());
            var dayreg = dateregularise.getDate();
            var datein = new Date($('#txtInTime').val());
            var dateout = new Date($('#txtOutTime').val());
            var dayin = datein.getDate();
            var monthin = datein.getMonth();
            var yearin = datein.getFullYear();
            var dayout = dateout.getDate();
            //var monthout = dateout.getMonth();
            var datesub = dayout - dayin;



            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            //if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
            //    errormsg = "Please Select Employee Code...! ";
            //    iserror = true;
            //}

            if (companyid == "" || companyid == null || companyid == "0") {
                errormsg = "Please select company...! </br>";
                iserror = true;
            }

            if (ddlEmployeeCode == "" || ddlEmployeeCode == null || ddlEmployeeCode == "0") {
                errormsg = "Employee code cannot be blank </br>";
                iserror = true;
            }

            if (txtRegularizeDate == null || txtRegularizeDate == '') {
                errormsg = "Please Select Regularize Date ...! </br>";
                iserror = true;
            }
            if (txtInTime == null || txtInTime == '') {
                errormsg = "Please Enter In Time ...! </br>";
                iserror = true;
            }
            if (txtOutTime == null || txtOutTime == '') {
                errormsg = "Please Enter Out Time ...! </br>";
                iserror = true;
            }

            if (new Date(txtOutTime) < new Date(txtInTime)) {
                errormsg = "Outtime must be greater than In time...! </br>";
                iserror = true;
            }

            if (dayin != dayreg) {
                //// debugger;
                errormsg = "Please Enter the same In date as Regularise Date Selected...! </br>";
                iserror = true;
            }
            if (datesub > 1 || datesub < 0) {
                // // debugger;
                errormsg = "Please Enter the same day or at max Day+1 ...! </br>";
                iserror = true;
            }

            //if ($("#lblInTime").text() == "00:00" || $("#lblOutTime").text() == "00:00") {
            //    errormsg = "Selected Regularize Date attandance not availble </br>";
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();

            var myData = {

                'from_date': txtRegularizeDate,
                'system_in_time': lblInTime,
                'system_out_time': lblOutTime,
                'manual_in_time': txtInTime,
                'manual_out_time': txtOutTime,
                'requester_remarks': txtRemarks,
                'is_deleted': is_deleted,
                'r_e_id': ddlEmployeeCode,
                'created_by': login_emp_id,
                'company_id': companyid,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_AttendanceApplicationRequest';
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
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
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



        $("#ddlcompany").bind("change", function () {
            Reset_all();
            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
        });

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Attendence Application has been freezed for this month");
            $("#btnsave").attr("disabled", true);

        }


    }, 2000);// end timeout

});



//$("#btnReport").click(function () { window.location = "/View/AttendenceApplicationReport"; })


function GetInOrOutTimeByDate(ControlId, RegularizeDate, EmployeeId) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetInOrOutTimeByDate/' + RegularizeDate + '/' + EmployeeId;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode == undefined && res.length > 0) {
                $("#lblInTime").empty();
                var in_time = new Date(res[0].in_time);
                var in_time1 = GetTimeFromDate(res[0].in_time);

                $("#lblInTime").append(in_time1);

                $("#lblOutTime").empty();
                var out_time = new Date(res[0].out_time);
                var out_time1 = GetTimeFromDate(out_time);
                $("#lblOutTime").append(out_time1);

            }
            else {

                $("#lblInTime").empty();

                $("#lblInTime").append("00:00");

                $("#lblOutTime").empty();

                $("#lblOutTime").append("00:00");

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }

            }

            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);

        }

    });

}

function Reset_all() {
    $("#txtRegularizeDate").val('');
    $("#lblInTime").text(''); //'2000-01-01 ' + $("#lblInTime").text();
    $("#lblOutTime").text(''); //'2000-01-01 ' + $("#lblOutTime").text();
    $("#txtInTime").val('');
    $("#txtOutTime").val('');
    $("#txtremarks").val('');

    $('#txtInTime').val('');
    $('#txtOutTime').val('');
}

function GetData(empid) {
    //debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AttendanceApplicationByEmpId?emp_id=' + empid;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            _GUID_New();

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }
            $("#tblAttendanceApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                //dom: 'lBfrtip',
                //buttons: [
                //    {
                //        text: 'Export to Excel',
                //        title: 'Attendance Application Report',
                //        extend: 'excelHtml5',
                //        exportOptions: {
                //            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                //        }
                //    },
                //],
                "aaData": res,
                "columnDefs":
                    [

                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4, 5, 7, 8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                    ],

                "columns": [
                    { "data": null },
                    { "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "data": "system_in_time", "name": "system_in_time", "autoWidth": true },
                    { "data": "system_out_time", "name": "system_out_time", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    //{ "data": "approver_remarks", "name": "approver_remarks", "autoWidth": true },
                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return full.is_final_approve > 0 ? '' : '<a href="#" onclick="GetEditData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Edit Record !" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    }
                    ,
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return (full.is_delted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}

function GetEditData(id) {
    //debugger;

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AttendanceApplicationRequestById/' + id;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            Reset_all();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            if (res.from_date != null) {
                let f = new Date(res.from_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txtRegularizeDate').val(f);
            }

            if (res.system_in_time != null) {
                let f = new Date(res.system_in_time);
                f = GetTimeFromDate(f);
                $("#lblInTime").empty();
                $("#lblInTime").append(f);
            }
            if (res.system_out_time != null) {
                let f = new Date(res.system_out_time);
                f = GetTimeFromDate(f);
                $("#lblOutTime").empty();
                $('#lblOutTime').append(f);
            }

            if (res.manual_in_time != null) {
                let f = new Date(res.manual_in_time);
                f = GetDateFormatddMMyyyy(f) + ' ' + GetTimeFromDate(f);
                $('#txtInTime').val(f);
            }
            if (res.manual_out_time != null) {
                let f = new Date(res.manual_out_time);
                f = GetDateFormatddMMyyyy(f) + ' ' + GetTimeFromDate(f);
                $('#txtOutTime').val(f);
            }

            BindAllEmp_Company('ddlCompany', login_emp_id, 0);
            setSelect('ddlCompany', res.company_id);

            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', res.company_id, login_emp_id, 0);
            setSelect('ddlEmployeeCode', res.r_e_id);

            $('#txtremarks').val(res.requester_remarks);

            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            alert(err.responseText)
            $('#loader').hide();
        }
    });


}

function DeleteLeave(request_id, empidd) {
    $("#loader").show();

    debugger;

    var myData = {
        'leave_request_id': request_id,
        'r_e_id': empidd //emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteAttendanceLeaveApplication';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: Obj,
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                GetData(login_emp_id);

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
    //debugger;
    var leave_request_id = $('#hdnid').val();
    var r_e_id = $("#ddlEmployeeCode").val();
    var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
    var companyid = $("#ddlcompany").val();
    var txtRegularizeDate = $("#txtRegularizeDate").val();
    var lblInTime = txtRegularizeDate.toString() + ' ' + $("#lblInTime").text(); //'2000-01-01 ' + $("#lblInTime").text();
    var lblOutTime = txtRegularizeDate.toString() + ' ' + $("#lblOutTime").text(); //'2000-01-01 ' + $("#lblOutTime").text();
    var txtInTime = $("#txtInTime").val();
    var txtOutTime = $("#txtOutTime").val();
    var txtRemarks = $("#txtremarks").val();

    var dateregularise = new Date($('#txtRegularizeDate').val());
    var dayreg = dateregularise.getDate();
    var datein = new Date($('#txtInTime').val());
    var dateout = new Date($('#txtOutTime').val());
    var dayin = datein.getDate();
    var monthin = datein.getMonth();
    var yearin = datein.getFullYear();
    var dayout = dateout.getDate();
    var monthout = dateout.getMonth();
    var datesub = dayout - dayin;

    var is_deleted = 0;
    var errormsg = '';
    var iserror = false;

    //validation part

    if (companyid == "" || companyid == null || companyid == "0") {
        errormsg = "Please select company...! </br>";
        iserror = true;
    }

    if (ddlEmployeeCode == "" || ddlEmployeeCode == null || ddlEmployeeCode == "0") {
        errormsg = "Employee code cannot be blank </br>";
        iserror = true;
    }

    if (txtRegularizeDate == null || txtRegularizeDate == '') {
        errormsg = "Please Select Regularize Date ...! </br>";
        iserror = true;
    }
    if (txtInTime == null || txtInTime == '') {
        errormsg = "Please Enter In Time ...! </br>";
        iserror = true;
    }
    if (txtOutTime == null || txtOutTime == '') {
        errormsg = "Please Enter Out Time ...! </br>";
        iserror = true;
    }

    if (new Date(txtOutTime) < new Date(txtInTime)) {
        errormsg = "Outtime must be greater than In time...! </br>";
        iserror = true;
    }

    if (dayin != dayreg) {
        //// debugger;
        errormsg = "Please Enter the same In date as Regularise Date Selected...! </br>";
        iserror = true;
    }
    if (datesub > 1 || datesub < 0) {
        // // debugger;
        errormsg = "Please Enter the same day or at max Day+1 ...! </br>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }

    $('#loader').show();

    var myData = {
        'leave_request_id': leave_request_id,
        'r_e_id': r_e_id,
        'from_date': txtRegularizeDate,
        'system_in_time': lblInTime,
        'system_out_time': lblOutTime,
        'manual_in_time': txtInTime,
        'manual_out_time': txtOutTime,
        'requester_remarks': txtRemarks,
        'is_deleted': is_deleted,
        'r_e_id': ddlEmployeeCode,
        'company_id': companyid,
    };

    $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh fa-spin"></i> Please wait..');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_AttendanceApplicationRequest';
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

            // var resp = JSON.parse(data);
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {

                //$("#ddlapplicable").val('0');
                //$('#ddlleavetype').val('0');
                //$('#ddldaypart').val('0');
                //$('#txtfromdate').val('');
                //$('#txttodate').val('');
                //$('#txthhmm').val('');
                //$('#txtremark').val('');

                //$('#DivDayPart').hide();
                //$('#Divhhmm').hide();

                //GetData();
                //messageBox("success", Msg);
                $("#btnupdate").text('Update').attr("disabled", false);
                $('#btnupdate').hide();
                $('#btnsave').show();
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
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

