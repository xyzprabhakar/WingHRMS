$('#loader').show();
var companyid;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        $("#div_updatedtll").hide();

        GetLOP_Setting_Data(companyid);
        GetData(companyid);

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);



        BindMonthYearOnly('txttotlDays', 'hdnmonthyear', 0);

        $('#ddlcompany').bind("change", function () {

            BindEmployeeCodeFromEmpMasterByComp('ddlemployee', $(this).val(), 0);
        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnupdate").hide();

        $('#loader').hide();

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var company_id = $("#ddlcompany").val();
            var employee_code = $("#ddlemployee").val();
            var actual_lop = $("#txtactuallop").val();
            var final_lop = $("#txtfinallop").val();
            /*var startDate = $("#startDate").val();*/ // month year selection 

            var startDate = $("#hdnmonthyear").val();

            //  var startDate = $("#txtmonthyear").val();
            var txttotlDays = $("#txttotlDays").val();

            var errormsg = "";
            var iserror = false;

            if (company_id == "0") {
                errormsg = errormsg + 'Please Select Company<br/>';
                iserror = true;
            }
            if (employee_code == "0") {
                errormsg = errormsg + 'Please Select Employee <br/>';
                iserror = true;
            }
            if (startDate == '') {
                errormsg = errormsg + 'Please Select Month Year<br/>';
                iserror = true;
            }
            if (txttotlDays == '') {
                errormsg = errormsg + 'Please Enter Total Days <br/>';
                iserror = true;
            }
            if (actual_lop == '') {
                errormsg = errormsg + 'Please Enter Actual LOD <br/>';
                iserror = true;
            }
            if (final_lop == '') {
                errormsg = errormsg + 'Please Enter Final LOD <br/>';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var mydata = {
                emp_id: employee_code,
                company_id: company_id,
                monthyear: startDate,
                totaldays: txttotlDays,
                acutual_lop_days: actual_lop,
                final_lop_days: final_lop,
                created_by: login_emp_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_Lod_Master",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
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
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });


        $("#btnupdate").bind("click", function () {
            $('#loader').show();

            var company_id = $("#ddlcompany").val();
            var employee_code = $("#ddlemployee").val();
            var actual_lop = $("#txtactuallop").val();
            var final_lop = $("#txtfinallop").val();
            /*var startDate = $("#startDate").val();*/ // month year selection 

            var startDate = $("#hdnmonthyear").val();
            var txttotlDays = $("#txttotlDays").val();

            var errormsg = "";
            var iserror = false;

            if (company_id == "0") {
                errormsg = errormsg + 'Please Select Company<br/>';
                iserror = true;
            }
            if (employee_code == "0") {
                errormsg = errormsg + 'Please Select Employee <br/>';
                iserror = true;
            }
            if (startDate == '') {
                errormsg = errormsg + 'Please Select Month Year<br/>';
                iserror = true;
            }
            if (txttotlDays == '') {
                errormsg = errormsg + 'Please Enter Total Days <br/>';
                iserror = true;
            }
            if (actual_lop == '') {
                errormsg = errormsg + 'Please Enter Actual LOD <br/>';
                iserror = true;
            }
            if (final_lop == '') {
                errormsg = errormsg + 'Please Enter Final LOD <br/>';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var mydata = {
                lop_master_id: $("#hdnlopmasterid").val(),
                emp_id: employee_code,
                company_id: company_id,
                monthyear: startDate,
                totaldays: txttotlDays,
                acutual_lop_days: actual_lop,
                final_lop_days: final_lop,
                modified_by: login_emp_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Edit_Lod_Master",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
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
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });


        $("#btnDetails").bind("click", function () {

            var iserror = false;
            var errormsg = "";

            var monthdtl = $("#hdnmonthyear").val();
            var _year = monthdtl.substring(0, 4);
            var _month = monthdtl.slice(-2);

            var month_dtll = _month + "-01-" + _year;

            var employee_idd = $("#ddlemployee").val();

            if (employee_idd == "0") {
                errormsg = errormsg + 'Please Select Employee<br/>';
                iserror = true;
            }

            if (monthdtl == '') {
                errormsg = errormsg + 'Please Select Month and Year';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            window.location.href = '/Attendence/Attendence?' + month_dtll + "/" + employee_idd;
        });

    }, 2000);// end timeout

});


function GetData(companyidd) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_Lod_Master/0" + "/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $('#loader').hide();

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                return false;
            }

            $("#tbllopmaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
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
                        }
                    ],

                "columns": [
                    { "data": "s_no", "name": "s_no", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "monthyear", "name": "monthyear", "autoWidth": true },
                    { "data": "totaldays", "name": "totaldays", "autoWidth": true },
                    { "data": "acutual_lop_days", "name": "acutual_lop_days", "autoWidth": true },
                    { "data": "final_lop_days", "name": "final_lop_days", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="EditData(' + full.lop_master_id + ',' + full.company_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    })
}



function GetLOP_Setting_Data(companyidd) {
    $('#loader').show();
    // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_LOD_Setting/0/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.length > 0) {
                if (res[0].lop_setting == 1) {
                    $('#txttotlDays').attr('readonly', true);
                }
            }
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

function EditData(id, companyid) {
    $('#loader').show();
    $("#hdnlopmasterid").val(id);
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_Lod_Master/" + id + "/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $('#loader').hide();

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                return false;
            }

            if (response != null) {
                $("#div_updatedtll").show();
                $("#ddlcompany").val(response.company_id);

                BindEmployeeCodeFromEmpMasterByComp('ddlemployee', response.company_id, response.emp_id);

                var full_monthyearr = response.monthyear.toString();
                var _monthh = full_monthyearr.slice(-2);
                var _yearr = full_monthyearr.substring(0, 4);

                var _monthname = GetMonthName(_monthh);
                $("#startDate").val(_monthname + "-" + _yearr);
                $("#hdnmonthyear").val(_yearr + _monthh);
                $("#txtactuallop").val(response.acutual_lop_days);
                $("#txtfinallop").val(response.final_lop_days);
                $("#txttotlDays").val(response.totaldays);

                $("#btnsave").hide();
                $("#btnupdate").show();
            }
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }

    });
}



function BindMonthYearOnly(ControlId1, hdnControlId, SelectedVal) {
    var year, month;
    ControlId1 = "#" + ControlId1;
    hdnControlId = "#" + hdnControlId;
    $('.date-picker').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'MM-yy',
        onClose: function (dateText, inst) {
            //$(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));

            year = inst.selectedYear, month = inst.selectedMonth + 1;

            $(this).datepicker('setDate', new Date(year, month, 0));
            $(ControlId1).val(new Date(year, month, 0).getDate());

            if (month <= 9) {
                month = "0" + month
            }
            else {
                month = month;
            }
            var monthyear = year + '' + month;
            $(hdnControlId).val(monthyear);

        },
        onChangeMonthYear: function (year, month, date) {
            $(ControlId1).val(new Date(year, month, 0).getDate());

            if (month <= 9) {
                month = "0" + month
            }
            else {
                month = month;
            }
            var monthyear = year + '' + month;
            $(hdnControlId).val(monthyear);
        }
    });

}


function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}