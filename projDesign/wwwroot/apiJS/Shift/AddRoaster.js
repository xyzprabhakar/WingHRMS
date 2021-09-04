$('#loader').show();

var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        $("#hdnid").val('');
        $('#btnupdate').hide();
        $('#btnsave').show();




        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        GetEmployeeList(default_company);
        GetShiftMaster(default_company);



        //calender
        //$("#txtfromdate").datepicker({
        //    dateFormat: 'mm/dd/yy',
        //    minDate: 0,
        //    onSelect: function (fromselected, evnt) {
        //        $("#txttodate").datepicker('setDate', null);
        //        $("#txttodate").datepicker({
        //            dateFormat: 'mm/dd/yy',
        //            minDate: fromselected,
        //            onSelect: function (toselected, evnt) {

        //                if (Date.parse(fromselected) >= Date.parse(toselected)) {
        //                    messageBox('error', 'To Date must be greater than from date !!');
        //                    //$('#txtfromdate').val('');
        //                    $('#txttodate').val('');
        //                }
        //            }
        //        });
        //    }
        //});

        $("#txtfromdate").change(function (toselectedDates) {
            $("#txttodate").val('');
        });


        $("#txttodate").change(function () {
            if (Date.parse($("#txtfromdate").val()) >= Date.parse($("#txttodate").val())) {
                messageBox('error', 'To Date must be greater than from date !!');
                $('#txttodate').val('');
            }
        });


        //bind employee list on change of company
        $('#ddlcompany').bind('change', function () {


            GetEmployeeList($(this).val());
            GetShiftMaster($(this).val());

        });



        $('#loader').hide();

        $('#btnsave').bind("click", function () {

            var shift_roster_id = 0;
            var applicable_from_date = $('#txtfromdate').val();
            var applicable_to_date = $('#txttodate').val();
            var shift_rotat_in_day = $('#txtrotationday').val();
            //var emp_id=
            var shft_id1 = $('#ddlshift1').val();
            var shft_id2 = $('#ddlshift2').val();
            var shft_id3 = $('#ddlshift3').val();
            var shft_id4 = $('#ddlshift4').val();
            var shft_id5 = $('#ddlshift5').val();
            var company = $("#ddlcompany").val();


            if (!Validate()) {
                return false;
            }


            var myData = {

                'shift_roster_id': shift_roster_id,
                'applicable_from_date': applicable_from_date,
                'applicable_to_date': applicable_to_date,
                'shift_rotat_in_day': shift_rotat_in_day,
                'emp_id': EmpIds,
                'shft_id1': shft_id1,
                'shft_id2': shft_id2,
                'shft_id3': shft_id3,
                'shft_id4': shft_id4,
                'shft_id5': shft_id5

            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiShift/SaveRoasterMaster/';
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
                contentType: 'application/json; charset=utf-8',
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    _GUID_New();
                    if (statuscode == "0") {
                        //$("#ddlcompany").val('0');
                        //$('#txtrotationday').val('');
                        //$('#txtfromdate').val('');
                        //$('#txttodate').val('');
                        //ResetAddShift();
                        // GetData();
                        //messageBox("success", Msg);
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                    //$('#loader').hide();
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);

                }
            });
        });


        $('#btnupdate').bind("click", function () {
            // debugger;
            var shift_roster_id = $('#hdnid').val();
            var applicable_from_date = $('#txtfromdate').val();
            var applicable_to_date = $('#txttodate').val();
            var shift_rotat_in_day = $('#txtrotationday').val();
            //var emp_id=
            var shft_id1 = $('#ddlshift1').val();
            var shft_id2 = $('#ddlshift2').val();
            var shft_id3 = $('#ddlshift3').val();
            var shft_id4 = $('#ddlshift4').val();
            var shft_id5 = $('#ddlshift5').val();
            var company = $("#ddlcompany").val();
            EmpIds.push(shift_roster_id);

            if (!Validate()) {
                return false;
            }


            var myData = {

                'shift_roster_id': shift_roster_id,
                'applicable_from_date': applicable_from_date,
                'applicable_to_date': applicable_to_date,
                'shift_rotat_in_day': shift_rotat_in_day,
                'emp_id': EmpIds,
                'shft_id1': shft_id1,
                'shft_id2': shft_id2,
                'shft_id3': shft_id3,
                'shft_id4': shft_id4,
                'shft_id5': shft_id5

            };
            $("#btnupdate").text('Please wait..').attr("disabled", true);
            var apiurl = localStorage.getItem("ApiUrl") + 'apiShift/UpdateRoasterMaster/';
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
                contentType: 'application/json; charset=utf-8',
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    _GUID_New();
                    if (statuscode == "0") {
                        $("#btnupdate").text('Update').attr("disabled", false);
                        alert(Msg);
                        location.reload();
                        //$("#ddlcompany").val('0');
                        //$('#txtrotationday').val('');
                        //$('#txtfromdate').val('');
                        //$('#txttodate').val('');
                        //ResetAddShift();
                        //// GetData();
                        //messageBox("success", Msg);
                        //$('#detail').click();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                    //$('#loader').hide();
                    //$("#btnupdate").text('Update').attr("disabled", false);
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);

                    $("#btnupdate").text('Update').attr("disabled", false);
                }
            });
        });


        $('#detail').bind('click', function () {
            GetData();
        });

        $('#addnew').bind('click', function () {
            window.location.reload();
        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });

    }, 2000);// end timeout

});

//declare gloabal array for emp id list
var EmpIds = [];

//--------bind data in jquery data table
function GetEmployeeList(companyid) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_EmployeeCodeFromEmpMasterByComp/' + companyid;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {


            $('#loader').hide();
            $("#tblemployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollY": 300,
                "bPaginate": false,
                "bFilter": false,
                "bInfo": false,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [1],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="chk' + full.employee_id + '" onclick="GetEmpId(' + full.employee_id + ')" />';

                        }
                    },
                    { "data": "emp_code", "title": "Employee Code", "name": "emp_code", "autoWidth": true },
                    { "data": "emp_name", "title": "Employee Name", "name": "emp_name", "autoWidth": true }
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return full.employee_first_name + ' ' + full.employee_middle_name + ' ' + full.employee_last_name;

                    //    }
                    //}
                ]


            });
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}


function GetEmpId(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    if ($('#chk' + id).is(":checked")) {
        //add valie in arrey list

        EmpIds.push(id);
    }
    else {
        //remove value from array list
        for (var i = 0; i < EmpIds.length; i++) {
            //Here we are going to remove
            if (EmpIds[i] == id) {
                EmpIds.splice(i, 1);
            }
        }
    }
    $('#loader').hide();
}

//Get shift master
function GetShiftMaster(companyid) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiShift/GetShiftForRoaster/" + companyid,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            $('#loader').hide();
            var res = data;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            //bind all shift
            BindShift1('ddlshift1', res, 0);
            BindShift2('ddlshift2', res, 0);
            BindShift3('ddlshift3', res, 0);
            BindShift4('ddlshift4', res, 0);
            BindShift5('ddlshift5', res, 0);

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", "Server busy please try again later...!");
        }
    });
}

function BindShift1(ControlId, res, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    $.each(res, function (data, value) {
        $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name + ' (' + value.shift_short_name + ')'));
    })

    //get and set selected value
    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null) {
        $(ControlId).val(SelectedVal);
    }

}

function BindShift2(ControlId, res, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    $.each(res, function (data, value) {
        $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name + ' (' + value.shift_short_name + ')'));
    })

    //get and set selected value
    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null) {
        $(ControlId).val(SelectedVal);
    }

}

function BindShift3(ControlId, res, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    $.each(res, function (data, value) {
        $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name + ' (' + value.shift_short_name + ')'));
    })

    //get and set selected value
    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null) {
        $(ControlId).val(SelectedVal);
    }

}

function BindShift4(ControlId, res, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    $.each(res, function (data, value) {
        $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name + ' (' + value.shift_short_name + ')'));
    })

    //get and set selected value
    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null) {
        $(ControlId).val(SelectedVal);
    }

}
function BindShift5(ControlId, res, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    $.each(res, function (data, value) {
        $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name + ' (' + value.shift_short_name + ')'));
    })

    //get and set selected value
    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null) {
        $(ControlId).val(SelectedVal);
    }

}

//shift validation

function AddShiftValidate() {
    $('#loader').show();
    var Shift1 = $('#ddlshift1').val();
    var Shift2 = $('#ddlshift2').val();
    var Shift3 = $('#ddlshift3').val();
    var Shift4 = $('#ddlshift4').val();
    var Shift5 = $('#ddlshift5').val();

    if ((Shift1 == null || Shift1 == 0) && (Shift2 > 0)) {
        messageBox('error', 'Please select in sequence !!');
        ResetAddShift();
        $('#loader').hide();
        return false;
    }
    if ((Shift2 <= 0) && (Shift3 > 0)) {
        messageBox('error', 'Please select in sequence !!');
        ResetAddShift();
        $('#loader').hide();
        return false;
    }
    if ((Shift3 <= 0) && (Shift4 > 0)) {
        messageBox('error', 'Please select in sequence !!');
        ResetAddShift();
        $('#loader').hide();
        return false;
    }
    if ((Shift4 <= 0) && (Shift5 > 0)) {
        messageBox('error', 'Please select in sequence !!');
        ResetAddShift();
        $('#loader').hide();
        return false;
    }
    //if ((Shift5 <=0) && (Shift2 > 0 || Shift3 > 0 || Shift4 > 0 || Shift5 > 0)) {
    //    messageBox('error', 'Please select in sequence !!');
    //    return false;
    //}

    $('#loader').hide();
}

function ResetAddShift() {
    $('#ddlshift1').val(0);
    $('#ddlshift2').val(0);
    $('#ddlshift3').val(0);
    $('#ddlshift4').val(0);
    $('#ddlshift5').val(0);
}




//validate 
function Validate() {

    var shift_roster_id = 0;
    var applicable_from_date = $('#txtfromdate').val();
    var applicable_to_date = $('#txttodate').val();
    var shift_rotat_in_day = $('#txtrotationday').val();
    //var emp_id=
    var Shift1 = $('#ddlshift1').val();
    var Shift2 = $('#ddlshift2').val();
    var Shift3 = $('#ddlshift3').val();
    var Shift4 = $('#ddlshift4').val();
    var Shift5 = $('#ddlshift5').val();
    var company = $("#ddlcompany").val();

    var is_active = 1;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (applicable_from_date == null || applicable_from_date == '') {
        errormsg = "Please select from date !! <br/>";
        iserror = true;
    }
    if (applicable_to_date == null || applicable_to_date == '') {
        errormsg = errormsg + "Please select to date !! <br/>";
        iserror = true;
    }

    if (new Date(applicable_to_date) < new Date(applicable_from_date)) {
        errormsg = errormsg + "To Date must be greater than from Date";
        iserror = true;
    }
    if (shift_rotat_in_day == null || shift_rotat_in_day == '' || shift_rotat_in_day == 0) {
        errormsg = errormsg + "Please enter valid shift rotat in day !! <br/>";
        iserror = true;
    }
    if ((Shift1 == null && Shift1 == 0 && Shift2 < 0 && Shift3 <= 0 && Shift4 <= 0 && Shift5 <= 0)) {
        errormsg = errormsg + "Please select shift !! <br/>";
        iserror = true;
    }
    if (company <= 0) {
        errormsg = errormsg + "Please select company !! <br/>";
        iserror = true;
    }
    if (EmpIds.length <= 0) {
        errormsg = errormsg + "Please select at least one employee !! <br/>";
        iserror = true;
    }


    if (Shift1 != "" && Shift1 != "0" && Shift1 != null) {
        if (Shift1 == Shift2 || Shift1 == Shift3 || Shift1 == Shift4 || Shift1 == Shift5) {
            errormsg = errormsg + "All shifts must be different";
            iserror = true;
        }
    }
    else if (Shift2 != "" && Shift2 != null && Shift2 != "0") {
        if (Shift2 == Shift3 || Shift2 == Shift4 || Shift2 == Shift5) {
            errormsg = errormsg + "All shifts must be different";
            iserror = true;
        }
    }
    else if (Shift3 != "" && Shift3 != null && Shift3 != "0") {
        if (Shift3 == Shift4 || Shift3 == Shift5) {
            errormsg = errormsg + "All shifts must be different";
            iserror = true;
        }
    }
    else if (Shift4 != "" && Shift4 != null && Shift4 != "0") {
        if (Shift4 == Shift5) {
            errormsg = errormsg + "All shifts must be different";
            iserror = true;
        }
    }


    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }

    return true;
}



function GetEditData(id) {


    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiShift/GetRoasterDetails/' + id;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            //open tab 1
            var aData = res['data'];
            var ad = [];
            ad.push(aData);
            var ShiftList = res['shiftdata'];

            if (aData.applicable_to_date != null) {
                let a = new Date(aData.applicable_to_date)
                $('#txttodate').val(GetDateFormatddMMyyyy(a));
            }
            if (aData.applicable_from_date != null) {
                let a = new Date(aData.applicable_from_date)
                $('#txtfromdate').val(GetDateFormatddMMyyyy(a));
            }

            $('#txtrotationday').val(aData.shift_rotat_in_day);


            BindAllEmp_Company('ddlcompany', login_emp_id, aData.company_id);


            $('#ddlcompany').prop('disabled', true);
            //bind employee list on change of company          
            BindShift1('ddlshift1', ShiftList, aData.shft_id1);
            BindShift2('ddlshift2', ShiftList, aData.shft_id2);
            BindShift3('ddlshift3', ShiftList, aData.shft_id3);
            BindShift4('ddlshift4', ShiftList, aData.shft_id4);
            BindShift5('ddlshift5', ShiftList, aData.shft_id5);
            //bind grid

            $("#tblemployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                // "scrollY": 300,
                "bPaginate": false,
                "bFilter": false,
                "bInfo": false,
                "aaData": ad,
                "columnDefs":
                    [
                        {
                            targets: [1],
                            "class": "text-center",

                        }
                    ],

                "columns": [
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" disabled checked="checked" id="chk' + full.emp_id + '" onclick="GetEmpId(' + full.emp_id + ')" />';

                        }
                    },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Name(Code)", "autoWidth": true },
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return full.employee_first_name + ' ' + full.employee_middle_name + ' ' + full.employee_last_name;

                    //    }
                    //}
                ],
                "language": {
                    "infoEmpty": "Record not found !!"
                }


            });

            openCity(event, 'tab1');
            document.getElementById('addnew').className += " btn-primary";
            //$('#addnew').click();

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

//--------bind data in jquery data table

function GetData() {

    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiShift/GetRoasterDetails/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var aData = res['data'];
            var ShiftList = res['shiftdata'];
            var a = [];
            a.push(ShiftList);

            //datatable editor

            $("#tblroaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollY": 300,
                "aaData": aData,
                "columnDefs":
                    [

                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }

                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [11, 6],
                            "class": "text-center",

                        }

                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_id", "name": "emp_id", "autoWidth": true, "visible": false },

                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return full.employee_first_name + ' ' + full.employee_middle_name + ' ' + full.employee_last_name;

                    //    }
                    //},
                    { "data": "applicable_from_date", "name": "applicable_from_date", "title": "From Date", "autoWidth": true },
                    { "data": "applicable_to_date", "name": "applicable_to_date", "title": "To Date", "autoWidth": true },
                    { "data": "shift_rotat_in_day", "name": "shift_rotat_in_day", "title": "SR IN Days", "autoWidth": true },
                    {
                        "title": "Shift 1", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //var $select = $("<select></select>", {
                            //});
                            //$.each(ShiftList, function (k, v) {

                            //    var $option = $("<option></option>", {
                            //        "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //        "value": v.shift_id
                            //    });
                            //    if (full.shft_id1 === v.shift_id) {
                            //        $option.attr("selected", "selected")
                            //    }
                            //    $select.append($option);
                            //});
                            //return $select.prop("outerHTML");
                            var returnedData = $.grep(a[0], function (element, index) {
                                return element.shift_id === full.shft_id1;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].shift_name + ' (' + returnedData[0].shift_short_name + ')';
                        }
                    },
                    {
                        "title": "Shift 2", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //    var $select = $("<select></select>", {
                            //    });
                            //    $.each(ShiftList, function (k, v) {

                            //        var $option = $("<option></option>", {
                            //            "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //            "value": v.shift_id
                            //        });
                            //        if (full.shft_id2 === v.shift_id) {
                            //            $option.attr("selected", "selected")
                            //        }
                            //        $select.append($option);
                            //    });
                            //    return $select.prop("outerHTML");

                            var returnedData = $.grep(a[0], function (element, index) {
                                return element.shift_id === full.shft_id2;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].shift_name + ' (' + returnedData[0].shift_short_name + ')';
                        }
                    },
                    {
                        "title": "Shift 3", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //var $select = $("<select></select>", {
                            //});
                            //$.each(ShiftList, function (k, v) {

                            //    var $option = $("<option></option>", {
                            //        "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //        "value": v.shift_id
                            //    });
                            //    if (full.shft_id3 === v.shift_id) {
                            //        $option.attr("selected", "selected")
                            //    }
                            //    $select.append($option);
                            //});
                            //return $select.prop("outerHTML");
                            var returnedData = $.grep(a[0], function (element, index) {
                                return element.shift_id === full.shft_id3;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].shift_name + ' (' + returnedData[0].shift_short_name + ')';
                        }
                    },
                    {
                        "title": "Shift 4", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //var $select = $("<select></select>", {
                            //});
                            //$.each(ShiftList, function (k, v) {

                            //    var $option = $("<option></option>", {
                            //        "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //        "value": v.shift_id
                            //    });
                            //    if (full.shft_id4 === v.shift_id) {
                            //        $option.attr("selected", "selected")
                            //    }
                            //    $select.append($option);
                            //});
                            //return $select.prop("outerHTML");
                            var returnedData = $.grep(a[0], function (element, index) {
                                return element.shift_id === full.shft_id4;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].shift_name + ' (' + returnedData[0].shift_short_name + ')';
                        }
                    },
                    {
                        "title": "Shift 5", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //var $select = $("<select></select>", {
                            //});
                            //$.each(ShiftList, function (k, v) {

                            //    var $option = $("<option></option>", {
                            //        "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //        "value": v.shift_id
                            //    });
                            //    if (full.shft_id5 === v.shift_id) {
                            //        $option.attr("selected", "selected")
                            //    }
                            //    $select.append($option);
                            //});
                            //return $select.prop("outerHTML");
                            var returnedData = $.grep(a[0], function (element, index) {
                                return element.shift_id === full.shft_id5;
                            });

                            return returnedData == null || returnedData == '' ? '' : returnedData[0].shift_name + ' (' + returnedData[0].shift_short_name + ')';
                        }
                    },

                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="GetEditData(' + full.shift_roster_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';

                        }
                    }

                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}
