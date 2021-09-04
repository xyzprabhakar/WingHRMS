$('#loader').show();
var emp_role_idd;
var company_idd;
var login_emp_id;
var HaveDisplay = 0;
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        // HaveDisplay = ISDisplayMenu("Display Company List");
        BindAllEmp_Company('ddlcompany', login_emp_id, company_idd);


        //GetData
        $('#btnGetData').bind("click", function () {
            $('button[name="btnResetPayroll"]').hide();
            $('button[name="btnCalculate"]').hide();
            $('button[name="btnLock"]').hide();
            $('button[name="btnFreeze"]').hide();

            var txtMonthYear = $("#txtMonthYear").val();

            if (txtMonthYear == "" || txtMonthYear == "NaN0NaN" || txtMonthYear == "NaN" || txtMonthYear == "NaN00NaN" || txtMonthYear == "NaNNaN") {
                messageBox("error", "Please Enter Payroll Month...!");
                return;
            }
            if (!($(ddlcompany).val() > 0)) {
                messageBox("error", "Please select company...!");
                return
            }

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);
            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];
            txtMonthYear = yr.toString() + mnth.toString();
            // debugger;
            //GetPayrollStatus($(this).val(), txtMonthYear);
            GetEmpPayrollProcessData($('#ddlcompany').val(), txtMonthYear);
            $('#tbluserlist').show();
        });



        $('#txtMonthYear').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                if ($.fn.DataTable.isDataTable('#tbluserlist')) {
                    $('#tbluserlist').DataTable().clear().draw();
                }
            },

            beforeShow: function () {
                if ((selDate = $(this).val()).length > 0) {
                    iYear = selDate.substring(selDate.length - 4, selDate.length);
                    iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                }
            }
        });

        jQuery('#txtMonthYear').on('input', function () {
            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_idd);
        });


        $('#loader').hide();

        //Calculate Payroll
        $('button[name="btnCalculate"]').bind("click", function () {
            // debugger;
            $('#loader').show();
            var modal = document.getElementById("myModal");


            var ddlcompany = $("#ddlcompany").val();
            var txtMonthYear = $("#txtMonthYear").val();
            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);

            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];

            txtMonthYear = yr.toString() + mnth.toString();



            //Validation
            if (ddlcompany == '' || ddlcompany == 0) {
                messageBox("error", "Please Select Company...!");
                modal.style.display = "none";
                $('#loader').hide();
                return;
            }


            if (txtMonthYear == '') {
                messageBox("error", "Please Enter Payroll Month...!");
                modal.style.display = "none";
                $('#loader').hide();
                return;
            }


            var errormsg = '';
            var iserror = false;

            var EmpIds = [];
            var table = $("#tbluserlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    EmpIds.push(no);
                }
            });


            if (EmpIds.length <= 0) {
                errormsg = errormsg + 'Please Select Employee !! <br />';
                modal.style.display = "none";
                $('#loader').hide();
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var full_monthyearr = txtMonthYear.toString();

            var _monthh = full_monthyearr.slice(-2);
            var _yearr = full_monthyearr.slice(0, 4);
            var _monthname = GetMonthName(_monthh);
            var month_yearr = _monthname + " " + _yearr;


            //var checkstr = confirm('Are You Sure Want To Process The Payroll For ' + txtMonthYear + ' Month Year...?');

            var checkstr = confirm('Are You Sure Want To Process The Payroll For ' + month_yearr + '...?');
            if (checkstr == true) {
                // do your code
            } else {
                modal.style.display = "none";
                $('#loader').hide();
                return false;
            }



            var myData = {
                'company_id': ddlcompany,
                'payroll_month_year': txtMonthYear,
                'emp_id': EmpIds,
                'created_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/ProcessPayroll';
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
                    if (statuscode == "1") {
                        modal.style.display = "none";
                        alert(Msg);
                        location.reload();
                        //messageBox("success", Msg);
                        // GetEmpPayrollProcessData(ddlcompany, txtMonthYear);
                        // GetPayrollStatus(ddlcompany, txtMonthYear);
                    }
                    else if (statuscode == "0") {
                        modal.style.display = "none";
                        messageBox("error", Msg);
                        return false;
                        // alert(JSON.stringify(Msg));
                    }
                },
                error: function (xhr, status, error) {
                    modal.style.display = "none";
                    $('#loader').hide();
                    _GUID_New();
                    alert(xhr.responseText);
                    return false;
                }

            });
        });

        //Freeze Payroll
        $('button[name="btnFreeze"]').bind("click", function () {
            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtMonthYear = $("#txtMonthYear").val();
            //var txtmnyr = new Date($('#txtMonthYear').val());
            ////var frmtdate = txtmnyr.dateFormat("YYYY-MM");
            //var yr = txtmnyr.getFullYear();
            //var mnth = txtmnyr.getMonth() + 1;
            //// txtMonthYear = yr + "0" + mnth;
            //if (mnth <= 9) {
            //    txtMonthYear = yr.toString() + "0" + mnth.toString();
            //}
            //else {
            //    txtMonthYear = yr.toString() + mnth.toString();
            //}

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);

            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];

            txtMonthYear = yr.toString() + mnth.toString();

            var errormsg = '';
            var iserror = false;


            //Validation
            if (ddlcompany == '' || ddlcompany == 0) {
                messageBox("error", "Please Select Company...!");
                $('#loader').hide();
                return;
            }


            if (txtMonthYear == '') {
                messageBox("error", "Please Enter Payroll Month...!");
                $('#loader').hide();
                return;
            }



            var EmpIds = [];
            var table = $("#tbluserlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    EmpIds.push(no);
                }
            });


            if (EmpIds.length <= 0) {
                errormsg = errormsg + 'Please Select Employee !! <br />';
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var full_monthyearr = txtMonthYear.toString();

            var _monthh = full_monthyearr.slice(-2);
            var _yearr = full_monthyearr.slice(0, 4);
            var _monthname = GetMonthName(_monthh);
            var month_yearr = _monthname + " " + _yearr;

            //var checkstr = confirm('Are You Sure Want To Freeze The Payroll For ' + txtMonthYear + ' Month Year...?');

            var checkstr = confirm('Are You Sure Want To Freeze The Payroll For ' + month_yearr + '...?');
            if (checkstr == true) {
                // do your code
            } else {
                $('#loader').hide();
                return false;
            }

            var myData = {
                'company_id': ddlcompany,
                'payroll_month_year': txtMonthYear,
                'emp_id': EmpIds,
                'created_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/ProcessForFreeze';
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
                    if (statuscode == "1") {
                        alert(Msg);
                        location.reload();
                        // messageBox("success", Msg);
                        // GetEmpPayrollProcessData(ddlcompany, txtMonthYear);
                        //GetPayrollStatus(ddlcompany, txtMonthYear);
                    }
                    else if (statuscode == "0") {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err, exception) {
                    $('#loader').hide();
                    _GUID_New();
                    alert('error found');
                    return false;
                }
            });
        });

        //Lock Payroll
        $('button[name="btnLock"]').bind("click", function () {
            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtMonthYear = $("#txtMonthYear").val();
            //var txtmnyr = new Date($('#txtMonthYear').val());
            //var yr = txtmnyr.getFullYear();
            //var mnth = txtmnyr.getMonth() + 1;
            //if (mnth <= 9) {
            //    txtMonthYear = yr.toString() + "0" + mnth.toString();
            //}
            //else {
            //    txtMonthYear = yr.toString() + mnth.toString();
            //}

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);

            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];

            txtMonthYear = yr.toString() + mnth.toString();


            //Validation
            if (ddlcompany == '' || ddlcompany == 0) {
                messageBox("error", "Please Select Company...!");
                $('#loader').hide();
                return;
            }


            if (txtMonthYear == '') {
                messageBox("error", "Please Enter Payroll Month...!");
                $('#loader').hide();
                return;
            }

            var errormsg = '';
            var iserror = false;

            var EmpIds = [];
            var table = $("#tbluserlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    EmpIds.push(no);
                }
            });


            if (EmpIds.length <= 0) {
                errormsg = errormsg + 'Please Select Employee !! <br />';
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var full_monthyearr = txtMonthYear.toString();

            var _monthh = full_monthyearr.slice(-2);
            var _yearr = full_monthyearr.slice(0, 4);
            var _monthname = GetMonthName(_monthh);
            var month_yearr = _monthname + " " + _yearr;


            //var checkstr = confirm('Are You Sure Want To Lock The Payroll For ' + txtMonthYear + ' Month Year...?');
            var checkstr = confirm('Are You Sure Want To Lock The Payroll For ' + month_yearr + '...?');
            if (checkstr == true) {
                // do your code
            } else {
                $('#loader').hide();
                return false;
            }

            var myData = {
                'company_id': ddlcompany,
                'payroll_month_year': txtMonthYear,
                'emp_id': EmpIds,
                'created_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/LockPayrollProcess';
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
                    if (statuscode == "1") {
                        alert(Msg);
                        location.reload();

                        // messageBox("success", Msg);
                        // GetEmpPayrollProcessData(ddlcompany, txtMonthYear);
                        //GetPayrollStatus(ddlcompany, txtMonthYear);
                    }
                    else if (statuscode == "0") {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert('error found');
                    return false;
                }
            });
        });

        //Reset Btn
        $('button[name="btnResetPayroll"]').bind("click", function () {
            $('#loader').show();
            var EmpIds = [];
            var table = $("#tbluserlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    EmpIds.push(no);
                }
            });


            //if (EmpIds.length <= 0) {
            //    errormsg = errormsg + 'Please Select Employee !! <br />';
            //    modal.style.display = "none";
            //    $('#loader').hide();
            //    iserror = true;
            //}

            var ddlcompany = $("#ddlcompany").val();
            var txtMonthYear = $("#txtMonthYear").val();
            //var txtmnyr = new Date($('#txtMonthYear').val());
            //var yr = txtmnyr.getFullYear();
            //var mnth = txtmnyr.getMonth() + 1;
            //if (mnth <= 9) {
            //    txtMonthYear = yr.toString() + "0" + mnth.toString();
            //}
            //else {
            //    txtMonthYear = yr.toString() + mnth.toString();
            //}

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);

            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];

            txtMonthYear = yr.toString() + mnth.toString();



            //Validation
            if (ddlcompany == '' || ddlcompany == 0) {
                messageBox("error", "Please Select Company...!");
                $('#loader').hide();
                return;
            }

            if (txtMonthYear == '') {
                messageBox("error", "Please Enter Payroll Month...!");
                $('#loader').hide();
                return;
            }


            var full_monthyearr = txtMonthYear.toString();

            var _monthh = full_monthyearr.slice(-2);
            var _yearr = full_monthyearr.slice(0, 4);
            var _monthname = GetMonthName(_monthh);
            var month_yearr = _monthname + " " + _yearr;

            //var checkstr = confirm('Do You Want To Reset The Payroll Processed Data For ' + txtMonthYear + ' Month Year...?');
            var checkstr = confirm('It Reset Whole Employee Data, still do You Want To Reset The Payroll Processed Data For ' + month_yearr + '...?');
            if (checkstr == true) {
                var myData = {
                    'company_id': ddlcompany,
                    'payroll_month_year': txtMonthYear,
                    'emp_id': EmpIds,
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/ResetPayrollProcess';
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
                        if (statuscode == "1") {
                            // messageBox("success", Msg);
                            //  GetEmpPayrollProcessData(ddlcompany, txtMonthYear);
                            $('#btnLock').hide();
                            $('#btnResetPayroll').hide();
                            alert(Msg);
                            location.reload();
                            // messageBox("success", Msg);
                            //  return false;
                        }
                        else if (statuscode == "0") {
                            messageBox("info", Msg);
                            return false;
                        }
                    },
                    error: function (xhr, status, error) {
                        $('#loader').hide();
                        _GUID_New();
                        alert(xhr.responseText);
                        return false;
                    }
                });
            } else {
                $('#loader').hide();
                return false;
            }



        });

        // On Company Change
        $("#ddlcompany").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tbluserlist')) {
                $('#tbluserlist').DataTable().clear().draw();
            }
        });

    }, 2000);// end timeout

});


function GetEmpPayrollProcessData(company_id, month_year) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/GetEmployeeForPayroll/' + company_id + '/' + month_year;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tbluserlist").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 0,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {
                                return data == 'No' ? '<i class="fas fa-times" style="font-size: 20px;color: red;"></i>' : '<i class="fas fa-check" style="font-size: 18px; color : green"></i>'
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {
                                return data == 'No' ? '<i class="fas fa-times" style="font-size: 20px;color: red;"></i>' : '<i class="fas fa-check" style="font-size: 18px; color : green"></i>'
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {
                                return data == 'No' ? '<i class="fas fa-times" style="font-size: 20px;color: red;"></i>' : '<i class="fas fa-check" style="font-size: 18px; color : green"></i>'
                            }
                        }
                    ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            if (full.ischecked == true) {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked id="chk' + full.empid + '" value="' + full.empid + '" />';
                            }
                            else {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.empid + '" value="' + full.empid + '" />';
                            }
                        }
                    },
                    { "data": "empcode", "name": "empcode", "autoWidth": true },
                    { "data": "empname", "name": "empname", "autoWidth": true },
                    { "data": "empdept", "name": "empdept", "width": "50px" },
                    { "data": "salary", "name": "salary", "autoWidth": true },
                    { "data": "salary_group", "name": "salary_group", "autoWidth": true },
                    { "data": "partially_calculated", "name": "partially_calculated", "autoWidth": true },
                    { "data": "partially_freezed", "name": "partially_freezed", "autoWidth": true },
                    { "data": "is_lock", "name": "is_lock", "autoWidth": true },

                ],
                "lengthMenu": [[-1], ["All"]]
            });
            GetPayrollStatus(res);
            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}



function GetPayrollStatus(emp_data) {

    var is_calculated = 0, is_all_calculated = 1, is_freezed = 0, is_all_freezed = 1, is_lock = 0, is_all_lock = 1;
    for (var i = 0; i < emp_data.length; i++) {
        if (emp_data[i].partially_calculated == "Yes") { is_calculated = 1 }
        if (emp_data[i].partially_calculated == "No") { is_all_calculated = 0 }
        if (emp_data[i].partially_freezed == "Yes") { is_freezed = 1 }
        if (emp_data[i].partially_freezed == "No") { is_all_freezed = 0 }
        if (emp_data[i].is_lock == "Yes") { is_lock = 1 }
        if (emp_data[i].is_lock == "No") { is_all_lock = 0 }
    }

    if (is_calculated == 0) {
        $('button[name="btnResetPayroll"]').hide();
        $('button[name="btnCalculate"]').show();
    }
    if (is_calculated == 1) {
        $('button[name="btnFreeze"]').show();
        $('button[name="btnCalculate"]').show();
    }
    if (is_freezed == 1) {
        $('button[name="btnLock"]').show();
    }
    if (is_lock == 1) {
        $('button[name="btnResetPayroll"]').hide();
    }
    if (is_all_calculated == 1) {
        $('button[name="btnResetPayroll"]').show();
        $('button[name="btnCalculate"]').hide();
    }
    if (is_all_freezed == 1) {
        $('button[name="btnLock"]').show();
        $('button[name="btnFreeze"]').hide();
    }
    if (is_all_lock == 1) {
        $('button[name="btnResetPayroll"]').hide();
        $('button[name="btnCalculate"]').hide();
        $('button[name="btnLock"]').hide();
        $('button[name="btnFreeze"]').hide();
    }
    if (HaveDisplay == 1) {
        $('button[name="btnResetPayroll"]').show();
    }

    //$('#loader').show();
    //var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/GetPayrollStatus/' + company_id + '/' + month_year;
    //$.ajax({
    //    type: "GET",
    //    url: apiurl,
    //    data: {},
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
    //    success: function (data) {

    //        var res = data;

    //        //if any one calculated 
    //        if (res.calculated == 1) {
    //            $('#btnLock').show();
    //            $('#btnResetPayroll').show();
    //        }
    //        else if (res.freezed == 1) {
    //            $('#btnLock').show();
    //            $('#btnResetPayroll').show();
    //        }

    //        $('#loader').hide();


    //    },
    //    error: function (err) {
    //        $('#loader').hide();
    //        alert(err.responseText);

    //    }

    //});

}


function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbluserlist").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbluserlist").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(this).prop('checked', true);
        }
        else {
            $(this).prop('checked', false);
        }
    });
}


function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}



function monthNameToNum(monthname) {

    var months = [
        'January', 'February', 'March', 'April', 'May',
        'June', 'July', 'August', 'September',
        'October', 'November', 'December'
    ];


    var month = months.indexOf(monthname);
    return month != null ? month + 1 : 0;
}

