$('#loader').show();
var empid;
var company_id;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', empid, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', empid, company_id);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + company_id + "'", localStorage.getItem("sit_id")));
        }


        //var HaveDisplay = ISDisplayMenu("Display Company List");




        if (localStorage.getItem("new_emp_id") != null) {

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            //GetEmployeeGradeAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

            showEmployeementType(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }


        $('#loader').hide();

        $("#ddlCompany").bind("change", function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            if ($.fn.DataTable.isDataTable('#tblemptypemaster')) {
                $('#tblemptypemaster').DataTable().clear().draw();
            }
            if ($.fn.DataTable.isDataTable('#tblemptypedtl')) {
                $('#tblemptypedtl').DataTable().clear().draw();
            }
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
        });


        $("#ddlEmployeeCode").bind("change", function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            showEmployeementType($(this).val());
        });

    }, 2000);// end timeout

});




function showEmployeementType(emp_idd) {
    if (emp_idd > 0) {
        $.ajax({

            url: localStorage.getItem("ApiUrl") + "/apiEmployee/GetEmployeement_Type_Master/" + emp_idd,
            type: "GET",
            contentType: 'application/json',
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {

                var res1 = response;
                if (response.statusCode != undefined) {
                    messageBox("info", response.message);
                    return false;
                }

                $("#tblemptypemaster").DataTable({
                    "processing": false,//for show progress bar
                    "serverSide": false,// for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at one
                    "scrollX": 200,
                    "aaData": response,
                    "columnDefs": [],
                    "columns": [
                        //{
                        //    "visible": false, "render": function (res, type, row) {
                        //        return '<input type="text" id="txtemptypeid" value=' + row.emptypeid+'/>';
                        //    }
                        //},
                        {
                            "title": "EmployementType", "autoWidth": true,
                            "render": function (res, type, row) {
                                var notice_period = (row.notice_period_ != null && row.notice_period_ != "") ? (row.notice_period_.notice_period_month.toString() + "," + row.notice_period_.notice_period_days.toString()) : "0";
                                return '<input type ="hidden" id="hdnemptypeid" value="' + row.emptypeid + '"/><input type ="hidden" id="hdn_notice" value="' + notice_period + '"/><input type="text" id="txtemptypename" class="form-control"  readonly="readonly" value="' + row.emptypename + '"/>';
                            }
                        },
                        {
                            "title": "Start Date", "autoWidth": true,
                            "render": function (data, type, row) {

                                var dsdate = new Date(row.duration_start_period);
                                return '<input type="text" id="txtstartdate" value="' + GetDateFormatddMMyyyy(dsdate) + '" readonly="readonly" class="form-control" style="width:100;">';
                                // return '<input type="date"  autocomplete="off" id="txtstartdate" value="' + row.date_of_joining+'"/ readonly="readonly">';
                            }
                        },
                        {
                            "title": "End Date", "autoWidth": true,
                            "render": function (data, type, row) {
                                var dedate = new Date(row.duration_end_period);
                                return '<input type="text" id="txtstartdate" value="' + GetDateFormatddMMyyyy(dedate) + '" readonly="readonly" class="form-control" style="width:100;">';
                                // return '<input type="date"  autocomplete="off" id="txtenddate" value="' + row.date_of_joining +'"/ readonly="readonly"/>';
                            }
                        },
                        {
                            "title": "Actual Start Date", "autoWidth": true,
                            "render": function (data, type, row) {
                                var asdate = new Date(row.actual_duration_start_period);
                                return '<input type="date"  autocomplete="off" id="txtactualstartdate" class="form-control" value="' + GetDateFormatddMMyyyy(asdate) + '"/>';
                            }
                        },
                        {
                            "title": "Actual End Date", "autoWidth": true,
                            "render": function (data, type, row) {
                                var aedate = new Date(row.actual_duration_end_period);
                                return '<input type="date" autocomplete="off" id="txtactualenddate" class="form-control getdays" value="' + GetDateFormatddMMyyyy(aedate) + '"/>';
                            }
                        },
                        {
                            "title": "Duration(in days)", "autoWidth": true,
                            "render": function (data, type, row) {
                                return '<input type="text" id="txtdurationdays" readonly="readonly" class="form-control" value="' + row.actual_duration_days + '"/>';
                            }
                        },
                        {
                            "title": "Save", "autoWidth": true,
                            render: function (data, type, row) {

                                return '<div class="col-md-12 text-center btn-group"><button type="button" class="btn btnSaveR btn-outline-primary"  id="btnSave">Save</button></div>';

                            }
                        },
                        //{
                        //    render: function (data, type, row) {

                        //        return '<input style="width:100px" class="form-control"  id="value1" maxlength="8" name="value1" type="text" onkeypress="return isNumberKey(event)" value = 0 >'

                        //    }
                        //},
                        //{
                        //    render: function (data, type, row) {

                        //        return '<input style="width:100px" class="maintCostField" id="reqamt"  maxlength="8" onkeypress="return isNumberKey(event)" name="reqamt" type="text" value = 0 >'


                        //    }
                        //},
                        //{ 
                        //    render: function (data, type, row) {
                        //        return '<input type="text" readonly   autocomplete="off" id="txtmonthyAmt">'
                        //    }
                        //},
                        //{ 
                        //    render: function (data, type, row) {
                        //        return '<input type="text" readonly   autocomplete="off"  id="txtyearlyAmt">'
                        //    }
                        //},

                    ],

                    //"columns": [

                    //    { "data": null, "title": "SNo.", "autoWidth": true },
                    //],
                    //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    //    return nRow;
                    //},
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
                });


                $("#tblemptypedtl").DataTable({
                    "processing": false,//for show progress bar
                    "serverSide": false,//for process server side
                    "bDestroy": true,
                    "filter": true,
                    "orderMulti": false,//for disable multiple column at one
                    "scrollX": 200,
                    "aaData": response,
                    "columnDefs": [

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
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
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
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },

                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var modified_date = new Date(data);
                                return new Date(row.created_date) <= new Date(row.last_modified_date) ? GetDateFormatddMMyyyy(modified_date) : "-";

                            }
                        },
                    ],
                    "columns": [
                        { "data": null, "title": "SNo.", "autoWidth": true },
                        { "data": "emptypename", "name": "emptypename", "title": "Employment Type", "autoWidth": true },
                        {
                            "title": "Required Notice Period(in months or Days)", "autoWidth": true, "render": function (data, type, full, meta) {
                                return full.notice_period_ != null ? ('Months:-' + full.notice_period_.notice_period_month + ', Days:-' + full.notice_period_.notice_period_days + ' ') : '-';
                            }
                        },
                        { "data": "duration_start_period", "name": "duration_start_period", "title": "Duration Start", "autoWidth": true },
                        { "data": "duration_end_period", "name": "duration_end_period", "title": "Duration End", "autoWidth": true },
                        { "data": "duration_days", "name": "duration_days", "title": "Duration(in days)", "autoWidth": true },
                        { "data": "actual_duration_start_period", "name": "actual_duration_start_period", "title": "Actual Duration Start", "autoWidth": true },
                        { "data": "actual_duration_end_period", "name": "actual_duration_end_period", "title": "Actual Duration End", "autoWidth": true },
                        { "data": "actual_duration_days", "name": "actual_duration_days", "title": "Actual Duration(in days)", "autoWidth": true },
                        { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                        { "data": "last_modified_date", "name": "last_modified_date", "title": "Modified On", "autoWidth": true },

                    ],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },
                    "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
                });

                $(document).on("change", ".getdays", function () {

                    var _end_datee = new Date($(this).val());
                    var _startdate = new Date($(this).parents('tr').children('td').children('input[id="txtactualstartdate"]').val());


                    var _notice = $(this).parents('tr').children('td').children('input[id="hdn_notice"]').val();


                    if (_notice == "0") {
                        _GUID_New();
                        messageBox("error", "Please Set the Notice Period of Selected Employeement Type");
                        return false;
                    }


                    // var diff = (_end_datee.getTime() - _startdate.getTime()) / 1000;
                    // diff /= (60 * 60 * 24 * 7 * 4);

                    // var diff_noofmonth = Math.abs(Math.floor(diff));
                    // $(this).parents('tr').children('td').children('input[id="txtdurationdays"]').val(diff_noofmonth);

                    var millisecondsPerDay = 1000 * 60 * 60 * 24;

                    var millisBetween = _startdate.getTime() - _end_datee.getTime();
                    var days = millisBetween / millisecondsPerDay;


                    var total_days = Math.abs(Math.floor(days));
                    $("#hdntotaldaysduration").val(total_days);
                    $(this).parents('tr').children('td').children('input[id="txtdurationdays"]').val(total_days);
                    // if (parseInt(diff_noofmonth) <= parseInt(_notice)) {
                    //  _GUID_New();
                    //  messageBox("error", "Difference between start and end date is " + diff_noofmonth + " months, so please select other end date because notice of selected employment type is " + _notice + " month.");
                    //  return false;
                    //   }
                });

                $(document).on("click", ".btnSaveR", function () {


                    var hdnemptypeid = $(this).parents('tr').children('td').children('input[id="hdnemptypeid"]').val();
                    var hdnnoticeperid = $(this).parents('tr').children('td').children('input[id="hdn_notice"]').val();
                    var _startdate = $(this).parents('tr').children('td').children('input[id="txtactualstartdate"]').val();

                    var _enddate = $(this).parents('tr').children('td').children('input[id="txtactualenddate"]').val();
                    if (_startdate == "") {
                        _GUID_New();
                        messageBox("error", "Please Select Start Date");
                        return false;
                    }
                    if (_enddate == "") {
                        _GUID_New();
                        messageBox("error", "Please Select End Date");
                        return false;
                    }

                    if (hdnnoticeperid.split(',')[0] == "0" && hdnnoticeperid.split(',')[1] == "0") {
                        _GUID_New();
                        messageBox("error", "Please Set the Notice Period of Selected Employeement Type");
                        return false;
                    }

                    //if (hdnnoticeperid.split(',')[1] == "0") {
                    //    _GUID_New();
                    //    messageBox("error", "Please Set the Notice Period of Selected Employeement Type");
                    //    return false;
                    //}


                    // var diff = (new Date(_enddate).getTime() - new Date(_startdate).getTime()) / 1000;
                    //  diff /= (60 * 60 * 24 * 7 * 4);

                    //  var diff_noofmonth = Math.abs(Math.floor(diff));
                    //  $(this).parents('tr').children('td').children('input[id="txtdurationdays"]').val(diff_noofmonth);

                    //   if (parseInt(diff_noofmonth) <= parseInt(hdnnoticeperid)) {
                    //    _GUID_New();
                    //      messageBox("error", "Difference between start and end date is " + diff_noofmonth + " months, so please select other end date because notice of selected employment type is " + _notice + " month.");
                    //      return false;
                    //   }


                    UpdateEmpEmployeementStatusempid(hdnemptypeid, _startdate, _enddate)

                });
            },
            error: function (err) {
                $('#loader').hide();
                _GUID_New();
                messageBox("error", err.responseText);
            }
        });
    }

}

//function GetData() {
//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEmployeementTypeMaster/0/0",
//        type: "GET",
//        contentType: 'application/json',
//        dataType: "json",
//        data: {},
//        success: function (res) {

//            $("#tblemptypemaster").DataTable({
//                "processing": false,//for show progress bar
//                "serverSide": false,// for process server side
//                "bDestroy": true,
//                "filter": true, // this is for disable filter (search box)
//                "orderMulti": false, // for disable multiple column at one
//                "scrollX": 200,
//                "aaData": response,
//                "columnDefs": [],
//                "columns": [

//                    { "data": null, "title": "SNo.", "autoWidth": true },
//                ],
//                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
//                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
//                    return nRow;
//                },
//                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
//            });
//        },
//        error: function (err) {
//            $('#loader').hide();
//            _GUID_New();
//            messageBox("error", err.responseText);
//        }
//    });
//}


function UpdateEmpEmployeementStatusempid(_emptypeid, _startdatee, _enddatee) {

    var mydata = {
        employee_id: $("#ddlEmployeeCode").val(),
        employment_type: _emptypeid,
        duration_start_period: _startdatee,
        duration_end_period: _enddatee,
        created_by: empid
    }


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/UpdateEmpEmployementType",
        type: "POST",
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (response) {
            var res = response;
            var statuscode = response.statusCode;
            var Msg = response.message;
            $('#loader').hide();
            _GUID_New();


            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode != "0") {
                messageBox("error", Msg);
            }
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error", err.responseText);
        }
    });
}


function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}