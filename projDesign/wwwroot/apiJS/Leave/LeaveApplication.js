
$('#loader').show();
var login_emp_id;
var login_comp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindAllEmp_Company('ddlCompany', login_emp_id, login_comp_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', login_comp_id, 0);

        //remove the code underneath after testing  till u find till here 
        //BindCompanyList('ddlCompany', 0);
        $('#ddlCompany').bind("change", function () {

            var ddlCompany = $("#ddlCompany").val();

            if (ddlCompany > 0) {
                $("#ddlEmployeeCode option").remove();
                BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            }
            else {
                $("#ddlEmployeeCode option").remove();
            }
        });

        $('#ddlEmployeeCode').bind("change", function () {

            GetMyData($(this).val());
        });

        //till here ********************************

        var qs = getQueryStrings();
        var leave_request_id = qs["id"];
        if (leave_request_id == null || leave_request_id == '') {

            $('#Divhhmm').hide();
            $('#DivDayPart').hide();
            BindLeaveTypeInfo('ddlleavetype', 0);
            $('#btnupdate').hide();
            $('#btnsave').show();
            GetData();
        }
        else {
            //add logic for edit details
            $("#hdnid").val(leave_request_id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            // GetEditData(leaveinfoid);
        }



        $("#txtfromdate").change(function (toselectedDates) {
            $("#txttodate").val('');
        });

        $('#txttodate').change(function () {
            if (Date.parse($("#txtfromdate").val()) > Date.parse($("#txttodate").val())) {
                messageBox('error', 'To Date must be greater than from date !!');
                $('#txttodate').val('');
            }

        });


        //$("#txtfromdate").datepicker({
        //    dateFormat: 'mm/dd/yy',
        //    //minDate: 0,
        //    onSelect: function (fromselected, evnt) {
        //        $("#txttodate").datepicker('setDate', null);
        //        $("#txttodate").datepicker({
        //            dateFormat: 'mm/dd/yy',
        //            minDate: fromselected,
        //            onSelect: function (toselected, evnt) {

        //                if (Date.parse(fromselected) > Date.parse(toselected)) {
        //                    messageBox('error', 'To Date must be greater than from date !!');
        //                    //$('#txtfromdate').val('');
        //                    $('#txttodate').val('');
        //                }
        //            }
        //        });
        //    }
        //    //changeMonth: true,
        //    //changeYear: true
        //});

        $('#ddlapplicable').bind('change', function () {

            if ($(this).val() == '3') {
                $('#Divhhmm').show();
            }
            else {
                $('#Divhhmm').hide();
            }
            if ($(this).val() == '2') {
                $('#DivDayPart').show();
            }
            else {
                $('#DivDayPart').hide();

            }
        });

        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            location.reload();
            //$("#ddlapplicable").val('0');
            //$('#ddlleavetype').val('0');
            //$('#txtfromdate').val('');
            //$('#txttodate').val('');
            //$('#txthhmm').val('');
            //$('#ddldaypart').val('0');
            //$('#txtremark').val('');

            //$('#DivDayPart').hide();
            //$('#Divhhmm').hide();

            //$('#btnupdate').hide();
            //$('#btnsave').show();
            //$("#hdnid").val('');
        });

        $('#btnsave').bind("click", function () {

            //remove the next two lines when out of test 
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddlCompany = $("#ddlCompany").val();

            var r_e_id = ddlEmployeeCode; // here replace ddlEmployeeCode with empid after test 
            var from_date = $('#txtfromdate').val();
            var to_date = $('#txttodate').val();
            var leave_type_id = 0;
            var leave_qty = 0;
            var leave_info_id = $('#ddlleavetype').val();;
            var leave_applicable_for = $("#ddlapplicable").val();
            var day_part = $('#ddldaypart').val();
            var leave_applicable_in_hours_and_minutes = $('#txthhmm').val();
            var requester_remarks = $('#txtremark').val();


            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (ddlCompany == null || ddlCompany == '' || ddlCompany == 0) {
                errormsg = "Please Select Company...! ";
                iserror = true;
            }
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
                'requester_remarks': requester_remarks
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
                        $("#ddlapplicable").val('0');
                        $('#ddlleavetype').val('0');
                        $('#ddldaypart').val('0');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');
                        $('#txthhmm').val('');
                        $('#txtremark').val('');

                        $('#DivDayPart').hide();
                        $('#Divhhmm').hide();

                        alert(Msg);
                        location.reload();
                        // GetData();
                        //messageBox("success", Msg);
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

            var leave_request_id = $('#hdnid').val();
            var r_e_id = empid;
            var from_date = $('#txtfromdate').val();
            var to_date = $('#txttodate').val();
            var leave_type_id = 0;
            var leave_qty = 0;
            var leave_info_id = $('#ddlleavetype').val();;
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
            //var file = document.getElementById("certificatefile").files[0];
            //var formData = new FormData();
            //formData.append('AllData', Obj);
            //formData.append('file', file);

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

                        $("#ddlapplicable").val('0');
                        $('#ddlleavetype').val('0');
                        $('#ddldaypart').val('0');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');
                        $('#txthhmm').val('');
                        $('#txtremark').val('');

                        $('#DivDayPart').hide();
                        $('#Divhhmm').hide();

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


    }, 2000);// end timeout

});

//global var for get empid
var empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



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
            BindLeaveTypeInfo('ddlleavetype', res.leave_info_id);
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
function DeleteData(id) {

    if (confirm("Do you want to delete this?")) {

        if (id == null || id == '') {
            messageBox('info', 'There some problem please try after later !!');
            return false;
        }
        var myData = {

            'requestid': id,
            'empid': empid

        };
        var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/DeleteLeaveApplication/' + id + '/' + empid;
        var Obj = JSON.stringify(myData);

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $('#loader').show();
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
                    //GetData();
                    //messageBox("success", Msg);
                    $("#hdnid").val('');
                    alert(Msg);
                    location.reload();

                }
                else {
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
    }
    else {
        return false;
    }
}

//-------update city data

//validate 
function Validate() {

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
        errormsg = errormsg + "Please select leave applicable for !! <br/>";
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
    if (empid == null || empid == '') {
        errormsg = errormsg + "Invalid employee id please login again !! <br/>";
        iserror = true;
    }


    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }

    return true;
}


//--------bind data in jquery data table
function GetData() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationByEmp/' + empid;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var aData = res["data"];
            var lfor = res["leavefor"];
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
                            targets: [3],
                            render: function (data, type, row) {

                                if (data != null && data != 0) {
                                    return data == '1' ? 'First Half' : 'Second Half';
                                }
                                else {
                                    return '';
                                }
                            }

                        },
                        //{
                        //    targets: [4],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},

                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
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
                            targets: [9],
                            render: function (data, type, row) {

                                return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                            }
                        },
                        {
                            targets: [10, 11],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "S.No.", "autoWidth": true },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    {
                        "title": "Leave App For", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            var returnedData = $.grep(leavefor[0], function (element, index) {
                                return element.id == full.leave_applicable_for;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].name;
                            //return '';
                        }
                    },
                    //{ "data": "leave_applicable_in_hours_and_minutes", "name": "leave_applicable_in_hours_and_minutes", "title": "", "autoWidth": true },
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
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return full.is_final_approve > 0 ? '' : '<a href="#" id="A1" onclick="DeleteData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Delete Record !" ><i class="fa fa-trash"></i></a>';

                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
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
function GetMyData(id) {

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationByEmp/' + id;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var aData = res["data"];
            var lfor = res["leavefor"];
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

                                if (data != null && data != 0) {
                                    return data == '1' ? 'First Half' : 'Second Half';
                                }
                                else {
                                    return '';
                                }
                            }

                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                            }
                        },
                        //{
                        //    //targets: [3],
                        //    //render: function (data, type, row) {

                        //    //    var date = new Date(data);
                        //    //    let a = GetTimeFromDate(date);
                        //    //    return a == '0:00' ? '' : a;
                        //    //}

                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [4],
                        //    render: function (data, type, row) {

                        //        if (data != null && data != 0) {
                        //            return data == '1' ? 'First Half' : 'Second Half';
                        //        }
                        //        else {
                        //            return '';
                        //        }
                        //    }

                        //},

                        {
                            targets: [6],
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
                        //{
                        //    targets: [8],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [10],
                        //    render: function (data, type, row) {

                        //        return data == 0 ? 'Pending' : data == 1 ? 'Approved' : data == 2 ? 'Rejected' : '';
                        //    }
                        //},
                        {
                            targets: [10, 11],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "S.No.", "autoWidth": true },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    {
                        "title": "Leave App For", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            var returnedData = $.grep(leavefor[0], function (element, index) {
                                return element.id == full.leave_applicable_for;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].name;
                            //return '';
                        }
                    },
                    { "data": "requester_date", "name": "requester_date", "title": "Applied On", "autoWidth": true },


                    // { "data": "leave_applicable_in_hours_and_minutes", "name": "leave_applicable_in_hours_and_minutes", "title": "", "autoWidth": true },
                    { "data": "day_part", "name": "day_part", "title": "Day Part", "autoWidth": true },

                    { "data": "leave_qty", "name": "leave_qty", "title": "Leave Qty.", "autoWidth": true },

                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Requester Remarks", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "is_final_approve", "name": "is_final_approve", "title": "Status", "autoWidth": true },



                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return full.is_final_approve > 0 ? '' : '<a href="#" onclick="GetEditData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Edit Record !" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    },
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return full.is_final_approve > 0 ? '' : '<a href="#" id="A1" onclick="DeleteData(' + full.leave_request_id + ')" data-toggle="tooltip" title="Delete Record !" ><i class="fa fa-trash"></i></a>';

                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "language": {
                    "processing": "<i class='fa fa-refresh fa-spin'></i>",
                }
            });

            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}
