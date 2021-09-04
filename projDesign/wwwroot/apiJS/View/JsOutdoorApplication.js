$('#loader').show();
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_emp_id, 0);
        setSelect('ddlCompany', company_id);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, 0);

        //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, 0);
        //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, 0);
        //setSelect('ddlEmployeeCode', login_emp_id);

        //if (HaveDisplay == 1) {
        //    BindEmployeeListUnderLoginEmpFromAllComp();
        //   // $("#ddlEmployeeCode").val("0").change();

        //}
        //else {
        //    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, login_emp_id);
        //    //$("#ddlEmployeeCode option[value='0']").remove();
        //    $("#ddlEmployeeCode option[value='0']").text('--Please Select--');
        //}

        //BindCompanyList('ddlCompany', company_id);

        //BinddEmployeeCodee('ddlEmployeeCode', company_id, login_emp_id);

        GetData(login_emp_id);

        $('#btnupdate').hide();
        $('#btnsave').show();




        $("#ddlCompany").bind("change", function () {
            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
            //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
            $("#txtDate").val('');
            $("#txtInTime").val('');
            $("#txtOutTime").val('');
            $("#Reason").val('');
        });


        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Outdoor application has been freezed for this month");
            $("#btnsave").attr("disabled", true);

        }

    }, 2000);// end timeout

});

$('#loader').hide();

$('#btnreset').bind('click', function () {
    location.reload();
});

$('#btnsave').bind("click", function () {

    var txtDate = $("#txtDate").val();
    //var timee = $("#txtInTime").val();
    //var combined = new Date(txtDate + ' ' + timee);

    var txtInTime = $("#txtInTime").val();
    var txtOutTime = $("#txtOutTime").val();


    var Reason = $("#Reason").val();
    var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;//localStorage.getItem("emp_id");
    var ddlCompany = $("#ddlCompany").val(); //company_id;//localStorage.getItem("company_id");

    var errormsg = '';
    var iserror = false;

    //validation part

    if (ddlCompany == null || ddlCompany == "" || ddlCompany == "0") {
        errormsg = errormsg + "Please select company...!</br>";
        iserror = true;
    }

    if (ddlEmployeeCode == null || ddlEmployeeCode == "" || ddlEmployeeCode == "0" || ddlEmployeeCode == undefined) {
        errormsg = errormsg + "Employee Code cannot be blank...!</br>";
        iserror = true;
    }

    if (txtOutTime == null || txtOutTime == '') {
        errormsg = errormsg + "Please Enter Out Time...!</br> ";
        iserror = true;
    }
    if (txtInTime == null || txtInTime == '') {
        errormsg = errormsg + "Please Enter In Time...!</br> ";
        iserror = true;
    }
    if (txtDate == null || txtDate == '') {
        errormsg = errormsg + "Please Enter Date...!</br> ";
        iserror = true;
    }

    if (Reason == null || Reason == '') {
        errormsg = errormsg + "Please enter Reason...!</br>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }


    $('#loader').show();
    var myData = {

        'from_date': txtDate,
        'manual_in_time': txtDate.toString() + ' ' + txtInTime, //'2000-01-01' + ' ' + txtInTime + ':00',
        'manual_out_time': txtDate.toString() + ' ' + txtOutTime, //'2000-01-01' + ' ' + txtOutTime + ':00',
        'requester_remarks': Reason,
        'is_deleted': 0,
        'r_e_id': ddlEmployeeCode,
        'created_by': login_emp_id,
        'company_id': ddlCompany,
    };

    var apiurl = localStorage.getItem("ApiUrl") + "apiMasters/Save_OutdoorApplicationRequest";
    var Obj = JSON.stringify(myData);

    //alert(Obj);
    // return;
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        headers: headerss,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
                //BindCompanyListForddl('ddlCompany', 0);
                //$("#ddlEmployeeCode option").remove();
                //$("#txtDate").val('');
                //$("#txtInTime").val('');
                //$("#txtOutTime").val('');
                //$("#Reason").val('');
                //messageBox("success", Msg);
                //GetData(ddlEmployeeCode);
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


function GetData(empid) {
    //debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_OutdoorApplicationRequestbyEmpId?emp_id=' + empid;
    $('#loader').show();
    $.ajax({
        url: apiurl,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            _GUID_New();
            $('#loader').hide();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tblOutdoorApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 800,
                //dom: 'lBfrtip',
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
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "Date", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "title": "In Time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "title": "Out Time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "isauto_approval", "name": "isauto_approval", "title": "Is Auto Approval", "autoWidth": true },
                    //{ "data": "approver_remarks", "name": "approver_remarks", "title": "Approver Remarks", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return full.is_final_approve > 0 ? '' : '<a href="#" onclick="GetEditData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Edit" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                    //,{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //},
                    //{
                    //    "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {

                    //        return '<a href="#" onclick="ViewReqDetails(' + full.leave_request_id + ',' + full.requester_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                    //    }
                    //},
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            //debugger;
            _GUID_New();
            $('#loader').hide();
            alert(error.responseText);
        }
    });
} /// end Get Data

function DeleteLeave(request_id, empidd) {
    $("#loader").show();

    debugger;

    var myData = {
        'leave_request_id': request_id,
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteOutdoorleaveApplication';
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



function GetEditData(id) {
    //debugger;
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetOutdoorApplicationRequestbyId/' + id;
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
            reset_all();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }
            if (res.from_date != null) {
                let f = new Date(res.from_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txtDate').val(f);
            }
            if (res.manual_in_time != null) {
                let f = new Date(res.manual_in_time);
                f = GetTimeFromDate(f);
                $('#txtInTime').val(f);
            }
            if (res.manual_out_time != null) {
                let f = new Date(res.manual_out_time);
                f = GetTimeFromDate(f);
                $('#txtOutTime').val(f);
            }

            BindAllEmp_Company('ddlCompany', login_emp_id, 0);
            setSelect('ddlCompany', res.company_id);

            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', res.company_id, res.r_e_id, 0);

           // BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', res.company_id, 0);
            //BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', res.company_id, login_emp_id, 0);
           // setSelect('ddlEmployeeCode', res.r_e_id);

            $('#Reason').val(res.requester_remarks);

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


}// end Edit form

// Update Data
$("#btnupdate").bind("click", function () {
    //debugger;
    var leave_request_id = $('#hdnid').val();
    var r_e_id = $("#ddlEmployeeCode").val();
    var txtDate = $("#txtDate").val();
    var txtInTime = $("#txtInTime").val();
    var txtOutTime = $("#txtOutTime").val();
    var Reason = $("#Reason").val();
    var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;//localStorage.getItem("emp_id");
    var ddlCompany = $("#ddlCompany").val(); //company_id;//localStorage.getItem("company_id");

    //validation part

    var errormsg = '';
    var iserror = false;


    if (ddlCompany == null || ddlCompany == "" || ddlCompany == "0") {
        errormsg = errormsg + "Please select company...!</br>";
        iserror = true;
    }

    if (ddlEmployeeCode == null || ddlEmployeeCode == "" || ddlEmployeeCode == "0" || ddlEmployeeCode == undefined) {
        errormsg = errormsg + "Employee Code cannot be blank...!</br>";
        iserror = true;
    }

    if (txtOutTime == null || txtOutTime == '') {
        errormsg = errormsg + "Please Enter Out Time...!</br> ";
        iserror = true;
    }
    if (txtInTime == null || txtInTime == '') {
        errormsg = errormsg + "Please Enter In Time...!</br> ";
        iserror = true;
    }
    if (txtDate == null || txtDate == '') {
        errormsg = errormsg + "Please Enter Date...!</br> ";
        iserror = true;
    }

    if (Reason == null || Reason == '') {
        errormsg = errormsg + "Please enter Reason...!</br>";
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
        'from_date': txtDate,
        'manual_in_time': txtDate.toString() + ' ' + txtInTime, //'2000-01-01' + ' ' + txtInTime + ':00',
        'manual_out_time': txtDate.toString() + ' ' + txtOutTime, //'2000-01-01' + ' ' + txtOutTime + ':00',
        'requester_remarks': Reason,
        'is_deleted': 0,
        'r_e_id': ddlEmployeeCode,
        'company_id': ddlCompany,
    };

    var apiurl = localStorage.getItem("ApiUrl") + "apiMasters/Update_OutdoorApplicationRequest";
    var Obj = JSON.stringify(myData);

    $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh fa-spin"></i> Please wait..');

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

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







//$("#btnReport").click(function () { window.location = "/View/OutdoorApplicationReport"; })

function reset_all() {
    $('#txtDate').val('');
    $('#txtInTime').val('');
    $('#txtOutTime').val('');
    $('#Reason').val('');
}



