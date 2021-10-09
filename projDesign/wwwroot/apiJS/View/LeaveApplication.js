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
        setSelect('ddlEmployeeCode', login_emp_id);

        bindleavetypee('ddlleavetype', company_id, 0);

        //till here ********************************

        var qs = getQueryStrings();
        var leave_request_id = qs["id"];
        if (leave_request_id == null || leave_request_id == '') {

            $('#Divhhmm').hide();
            $('#DivDayPart').hide();
            //BindLeaveTypeInfo('ddlleavetype', 0);
            //bindleavetypee('ddlleavetype', 0)
            $('#btnupdate').hide();
            $('#btnsave').show();
            GetData(login_emp_id);
        }
        else {
            //add logic for edit details
            $("#hdnid").val(leave_request_id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            // GetEditData(leaveinfoid);
        }


        $("#txtfromdate").change(function (toselectedDates) {
            $("#txttodate").val($("#txtfromdate").val());
        });

        $('#txttodate').change(function () {
            if (Date.parse($("#txtfromdate").val()) > Date.parse($("#txttodate").val())) {
                messageBox('error', 'To Date must be greater than from date !!');
                $('#txttodate').val('');
            }

        });


        $('#ddlleavetype').bind('change', function () {

            if ($(this).val() == '3') {
                $("#ddlapplicable option[value=" + 1 + "]").hide();
            }
            else {
                $("#ddlapplicable option[value=" + 1 + "]").show();
            }
            $("#ddlapplicable").val(0);
        });

        $('#ddlapplicable').bind('change', function () {

            if ($(this).val() == '3') {
                $('#Divhhmm').show();
            }
            else {
                $('#Divhhmm').hide();
            }

            if ($(this).val() == '2') {
                $('#DivDayPart').show();
                $('#txttodate').val($('#txttodate').val());
                $('#DivToDate').hide();
            }
            else {
                $('#DivDayPart').hide();
                $('#DivToDate').show();
            }
        });

        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            location.reload();
        });



        $('#btnsave').bind("click", function () {

            //remove the next two lines when out of test 
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();//login_emp_id;
            var ddlCompany = $("#ddlCompany").val();//company_id;

            var r_e_id = ddlEmployeeCode; // here replace ddlEmployeeCode with empid after test 
            var from_date = $('#txtfromdate').val();
            var to_date = $('#txttodate').val();
            var leave_type_id = $('#ddlleavetype').val();
            var leave_qty = 0;
            var leave_info_id = 0;
            var leave_applicable_for = $("#ddlapplicable").val();
            var day_part = $('#ddldaypart').val();
            var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
            var requester_remarks = $('#txtremark').val();


            //if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
            //    errormsg = "Please Select Employee Code...! ";
            //    iserror = true;
            //}
            //if (ddlCompany == null || ddlCompany == '' || ddlCompany == 0) {
            //    errormsg = "Please Select Company...! ";
            //    iserror = true;
            //}
            if (!Validate()) {
                return false;
            }


            var myData = {

                'r_e_id': r_e_id,
                'from_date': from_date,
                'to_date': to_date,
                'leave_type_id': leave_type_id,
                'leave_qty': leave_qty,
                'leave_info_id': leave_info_id,
                'leave_applicable_for': leave_applicable_for,
                'day_part': day_part,
                'leave_applicable_in_hours_and_minutes': leave_applicable_in_hours_and_minutes,
                'requester_remarks': requester_remarks,
                'created_by': login_emp_id,
                'company_id': ddlCompany,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/SaveLeaveApplication';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            //show loader
            $('#loader').show();

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


        $("#ddlCompany").bind("change", function () {
            reset_all();
            // BinddEmployeeCodee('ddlEmployeeCode', $(this).val(), login_emp_id);
            bindleavetypee('ddlleavetype', $(this).val(), 0);
            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
        });

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Leave application has been freezed for this month");
            $("#btnsave").attr("disabled", true);

        }

    }, 2000);// end timeout

});

// Update Data
$("#btnupdate").bind("click", function () {
    //debugger;
    var leave_request_id = $('#hdnid').val();
    var r_e_id = $("#ddlEmployeeCode").val();
    var from_date = $('#txtfromdate').val();
    var to_date = $('#txttodate').val();
    var leave_type_id = $('#ddlleavetype').val();
    var leave_qty = 0;
    var leave_info_id = 0;
    var leave_applicable_for = $("#ddlapplicable").val();
    var day_part = $('#ddldaypart').val();
    var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
    var requester_remarks = $('#txtremark').val();

    if (!Validate()) {
        return false;
    }

    var myData = {

        'leave_request_id': leave_request_id,
        'r_e_id': r_e_id,
        'from_date': from_date,
        'to_date': to_date,
        'leave_type_id': leave_type_id,
        'leave_qty': leave_qty,
        'leave_info_id': leave_info_id,
        'leave_applicable_for': leave_applicable_for,
        'day_part': day_part,
        'leave_applicable_in_hours_and_minutes': leave_applicable_in_hours_and_minutes,
        'requester_remarks': requester_remarks
    };

    $("#btnupdate").attr("disabled", true).html('<i class="fa fa-refresh fa-spin"></i> Please wait..');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/UpdateLeaveApplication/';
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



//$("#btnReport").click(function () { window.location = "/View/LeaveApplicationReport"; })


function GetEditData(id) {


    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveAppRequest/' + id;
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
            if (res.from_date != null) {
                let f = new Date(res.from_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txtfromdate').val(f);
            }
            if (res.to_date != null) {
                let f = new Date(res.to_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txttodate').val(f);
            }

            BindAllEmp_Company('ddlCompany', login_emp_id, 0);
            setSelect('ddlCompany', res.company_id);

            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', res.company_id, login_emp_id, 0);
            setSelect('ddlEmployeeCode', res.r_e_id);






            // BindLeaveTypeInfo('ddlleavetype', res.leave_info_id);
            bindleavetypee('ddlleavetype', res.company_id, res.leave_info_id)

            $("#ddlapplicable").val(res.leave_applicable_for);

            if (res.leave_applicable_for == 3) {
                if (res.leave_applicable_in_hours_and_minutes != null && res.leave_applicable_in_hours_and_minutes != '') {
                    let a = getHHMM(res.leave_applicable_in_hours_and_minutes);
                    $('#txthhmm').val(a);
                }
                $('#Divhhmm').show();
            }
            else {
                $('#Divhhmm').hide();
            }
            if (res.leave_applicable_for == 2) {
                $('#ddldaypart').val(res.day_part);
                $('#DivDayPart').show();
            }
            else {
                $('#DivDayPart').hide();
            }
            $('#txtremark').val(res.requester_remarks);
            let f = new Date(res.from_date);
            f = GetDateFormatddMMyyyy(f);


            $("#hdnid").val(id);
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

//delete data
function DeleteLeave(request_id, empidd) {
    $("#loader").show();

    var myData = {
        'leave_request_id': request_id,
        'r_e_id': empidd,//emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteLeaveApplication';
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

//-------update city data


//validate 
function Validate() {

    var ddlcompany = $("#ddlCompany").val();
    var ddlemp_id = $("#ddlEmployeeCode").val();

    var r_e_id = 0;
    var from_date = $('#txtfromdate').val();
    var to_date = $('#txttodate').val();
    var leave_type_id = 0;
    var leave_qty = 0;
    var leave_info_id = $('#ddlleavetype').val();;
    var leave_applicable_for = $("#ddlapplicable").val();
    var day_part = $('#ddldaypart').val();
    var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
    var requester_remarks = $('#txtremark').val();

    var is_active = 1;
    var errormsg = '';
    var iserror = false;

    if (ddlcompany == "" || ddlcompany == null || ddlcompany == "0") {
        errormsg = errormsg + "Please select company !! <br/>";
        iserror = true;
    }

    if (ddlemp_id == "" || ddlemp_id == null || ddlemp_id == "0") {
        errormsg = errormsg + "Employee code cannot be blank <br/>";
        iserror = true;
    }

    if (from_date == null || from_date == '') {
        errormsg = errormsg + "Please select from date !! <br/>";
        iserror = true;
    }
    if (to_date == null || to_date == '') {
        errormsg = errormsg + "Please select to date !! <br/>";
        iserror = true;
    }
    if (leave_info_id == null || to_date == '0') {
        errormsg = errormsg + "Please select leave type !! <br/>";
        iserror = true;
    }
    if (leave_applicable_for == null || leave_applicable_for == '0') {
        errormsg = errormsg + "Please select Duration !! <br/>";
        iserror = true;
    }
    else {
        if (leave_applicable_for == 2) {
            if (day_part == null || day_part == '0') {
                errormsg = errormsg + "Please select day part !! <br/>";
                iserror = true;
            }
        }
        else if (leave_applicable_for == 3) {
            if (leave_applicable_in_hours_and_minutes == null || leave_applicable_in_hours_and_minutes == '') {
                errormsg = errormsg + "Please select hh and mm !! <br/>";
                iserror = true;
            }
        }
    }
    if (requester_remarks == null || requester_remarks == '') {
        errormsg = errormsg + "Please enter leave remarks !! <br/>";
        iserror = true;
    }
    if (leave_info_id == 9 && leave_applicable_for != 1) {
        errormsg = errormsg + "Weekoff only applicable for full day <br/>";
        iserror = true;
    }
    //if (empid == null || empid == '') {
    //    errormsg = errormsg + "Invalid employee id please login again !! <br/>";
    //    iserror = true;
    //}


    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }

    return true;
}

function GetData(empid) {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationByEmpId?emp_id=' + empid;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var aData = res;
            var lfor = res;
            var leavefor = [];

            leavefor.push(lfor);

            $("#tblleaveapp").DataTable({
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
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        let a = GetTimeFromDate(date);
                        //        return a == '0:00' ? '' : a;
                        //    }
                        //},
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                if (data != null && data != 0) {
                                    return data == '1' ? 'First Half' : 'Second Half';
                                }
                                else {
                                    return '';
                                }
                            }

                        },

                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [11],
                            render: function (data, type, row) {

                                return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                            }
                        },
                        //{
                        //    targets: [10, 11],
                        //    "class": "text-center",

                        //}
                        //{
                        //    targets: [10, 11, 12],
                        //    "class": "text-center",

                        //}
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "title": "Employee Code", "data": "emp_code", "name": "emp_code", "autoWidth": true, },
                    { "title": "Employee Name", "data": "emp_name", "name": "emp_name", "autoWidth": true, },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    { "data": "leave_applicable_for", "name": "leave_applicable_for", "title": "Duration", "autoWidth": true, },
                    //{ "data": "leave_applicable_in_hours_and_minutes", "name": "leave_applicable_in_hours_and_minutes", "autoWidth": true },
                    { "data": "day_part", "name": "day_part", "title": "Day Part", "autoWidth": true },
                    { "data": "leave_qty", "name": "leave_qty", "title": "Leave Qty.", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Applied On", "autoWidth": true },
                    { "data": "is_final_approve", "name": "is_final_approve", "title": "Status", "autoWidth": true },

                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return full.is_final_approve > 0 ? '' : '<a href="#" onclick="GetEditData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Edit Record !" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return (full.is_deleted == 1 || full.is_final_approve == 2 || full.is_final_approve == 1) ? '' : '<a  onclick="DeleteLeave(' + full.leave_request_id + ',' + full.r_e_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                //"language": {
                //    "processing": "<i class='fa fa-refresh fa-spin'></i>",
                //}
            });
            $('#loader').hide();
        },
        error: function (error) {

            alert(error.responseText);
            $('#loader').hide();
        }
    });
}


function bindleavetypee(ControlId, companyid, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/Get_LeaveInfo_forLeavetypebycurrentdateandCompID/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem("Token") },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.leave_type_id).html(value.leave_type_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
            return false;
        }
    });


}

function reset_all() {
    $('#txtfromdate').val('');
    $('#txttodate').val('');
    $('#ddlleavetype').val(0);
    $("#ddlapplicable").val(0);
    $('#txtremark').val('');
}