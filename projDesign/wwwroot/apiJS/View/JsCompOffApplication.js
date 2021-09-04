
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



        BindAllEmp_Company('ddlCompany', login_emp_id, company_id);
        setSelect('ddlCompany', company_id);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', company_id, login_emp_id, 0);
        setSelect('ddlEmployeeCode', login_emp_id);

        GetEmployeeCompOffData('ddlCompOffDate', login_emp_id, 0);

        GetData(login_emp_id);
        $('#divdddlCompOffDate').hide();
        $('#divddlCompOffDate').show();
        $('#btnupdate').hide();
        $('#btnsave').show();


        $("#ddlCompany").bind("change", function () {
            $("#txtCompOffDate").val('');
            $("#txtRemarks").val('');
            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
            GetEmployeeCompOffData('ddlCompOffDate', login_emp_id, 0);

        });

        $("#ddlEmployeeCode").bind("change", function () {
            GetEmployeeCompOffData('ddlCompOffDate', $(this).val(), 0);
        });

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Comp Off Application has been freezed for this month");
            $("#btnsave").attr("disabled", true);

        }

    }, 2000);// end timeout

});

$('#btnsave').bind("click", function () {

    var ddlcompany = $("#ddlCompany").val();
    var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
    var ddlCompOffDate = $("#ddlCompOffDate").val();
    var txtCompOffDate = $("#txtCompOffDate").val();
    var txtRemarks = $("#txtremarks").val();

    var is_deleted = 0;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlcompany == null || ddlcompany == "0" || ddlcompany == "") {
        errormsg = "Please select Company...!";
        iserror = true;
    }

    if (ddlEmployeeCode == "" || ddlEmployeeCode == "0" || ddlEmployeeCode == null) {
        errormsg = "Please select employee code";
        iserror = true;
    }

    if (ddlCompOffDate == null || ddlCompOffDate == '' || ddlCompOffDate == 0) {
        errormsg = "Please Select Against Date...! ";
        iserror = true;
    }
    if (txtCompOffDate == null || txtCompOffDate == '') {
        errormsg = "Please Select CompOff Date...! ";
        iserror = true;
    }

    if (new Date(ddlCompOffDate).getDate() == new Date(txtCompOffDate).getDate()) {
        errormsg = "Compoff against date and Compoff date cannot be same...!!";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }
    $('#loader').show();
    var myData = {

        'r_e_id': ddlEmployeeCode,
        'compoff_against_date': ddlCompOffDate,
        'compoff_date': txtCompOffDate,
        'requester_remarks': txtRemarks,
        'compoff_request_qty': 1,
        'is_deleted': is_deleted,
        'created_by': login_emp_id,
        'company_id': ddlcompany,
    };

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_CompOffApplicationRequest';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    //// debugger;;
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

$('#btnreset').bind('click', function () {
    location.reload();
});


//$("#btnReport").click(function () { window.location = "/View/CompOffApplicationReport"; })

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}


function GetEmployeeCompOffData(ControlId, EmployeeId, SelectedVal) {
    if (EmployeeId == 0) {
        messageBox("error", "Please select Employee");
        return false;
    }

    $('#loader').show();
    ControlId = '#' + ControlId;
    //var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetEmployeeCompOffData/' + EmployeeId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetEmployeeCompOff/' + EmployeeId;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }
            $.each(res, function (data, value) {
                var date = new Date(value.comp_off_date);
                $(ControlId).append($("<option></option>").val(GetDateFormatddMMyyyy(date)).html(GetDateFormatddMMyyyy(date)));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function GetData(empid) {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompOffApplicationRequestByEmpId?emp_id=' + empid;
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            _GUID_New();


            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }
            $("#tblCompOffApplication").DataTable({
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
                //        title: 'CompOff Application Report',
                //        extend: 'excelHtml5',
                //        exportOptions: {
                //            columns: [1, 2, 3, 4, 5, 6]
                //        }
                //    },
                //],
                "aaData": res,
                "columnDefs":
                    [

                        {
                            targets: [2],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "emp_name", "name": "emp_name", "autoWidth": true },
                    { "data": "compoff_date", "name": "compoff_date", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "data": "approver_remarks", "name": "approver_remarks", "autoWidth": true },
                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return full.is_final_approve > 0 ? '' : '<a href="#" onclick="GetEditData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Edit Record !" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.requester_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
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

function DeleteLeave(request_id, empidd) {
    $("#loader").show();
    debugger;

    var myData = {
        'leave_request_id': request_id,
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteCompOffLeaveApplication';
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
                messageBox("success", msg);
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

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompOffApplicationRequestById/' + id;
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            reset_all();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }


            if (res.compoff_date != null) {
                let f = new Date(res.compoff_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txtCompOffDate').val(f);
            }

            BindAllEmp_Company('ddlCompany', login_emp_id, 0);
            setSelect('ddlCompany', res.company_id);

            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', res.company_id, login_emp_id, 0);
            setSelect('ddlEmployeeCode', res.r_e_id);

            $('#txtremarks').val(res.requester_remarks);

            if (res.compoff_against_date != null) {
                $('#ddlCompOffDate').empty();
                $('#ddlCompOffDate').append('<option selected="selected" value="0">--Please select--</option>');
                var d1 = new Date(res.compoff_against_date);
                var d = GetDateFormatddMMyyyy(d1);
                $('#ddlCompOffDate').append($("<option></option>").val(d).html(d));
                setSelect('ddlCompOffDate', d);
                $('#divddlCompOffDate').hide();
                $('#divdddlCompOffDate').show();
                $('#dddlCompOffDate').val(d);

            }

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


// Update Data
$("#btnupdate").bind("click", function () {

    var leave_request_id = $('#hdnid').val();
    var r_e_id = $("#ddlEmployeeCode").val();
    var ddlcompany = $("#ddlCompany").val();
    var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
    var ddlCompOffDate = $("#dddlCompOffDate").val();
    var txtCompOffDate = $("#txtCompOffDate").val();
    var txtRemarks = $("#txtremarks").val();

    var is_deleted = 0;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlcompany == null || ddlcompany == "0" || ddlcompany == "") {
        errormsg = "Please select Company...!";
        iserror = true;
    }

    if (ddlEmployeeCode == "" || ddlEmployeeCode == "0" || ddlEmployeeCode == null) {
        errormsg = "Please select employee code";
        iserror = true;
    }

    if (ddlCompOffDate == null || ddlCompOffDate == '' || ddlCompOffDate == 0) {
        errormsg = "Please Select Against Date...! ";
        iserror = true;
    }
    if (txtCompOffDate == null || txtCompOffDate == '') {
        errormsg = "Please Select CompOff Date...! ";
        iserror = true;
    }

    if (new Date(ddlCompOffDate).getDate() == new Date(txtCompOffDate).getDate()) {
        errormsg = "Compoff against date and Compoff date cannot be same...!!";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }
    $('#loader').show();


    var myData = {

        'comp_off_request_id': leave_request_id,
        'r_e_id': r_e_id,
        'compoff_against_date': ddlCompOffDate,
        'compoff_date': txtCompOffDate,
        'requester_remarks': txtRemarks,
        'compoff_request_qty': 1,
        'is_deleted': is_deleted,
        'company_id': ddlcompany,
    };

    $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh fa-spin"></i> Please wait..');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_CompOffApplicationRequest';
    var Obj = JSON.stringify(myData);

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


function reset_all() {

    $('#txtCompOffDate').val('');
    $('#txtremarks').val('');
    GetEmployeeCompOffData('ddlCompOffDate', login_emp_id, 0);
    $('#divdddlCompOffDate').hide();
    $('#divddlCompOffDate').show();
}