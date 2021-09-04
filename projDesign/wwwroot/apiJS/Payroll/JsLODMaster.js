$('#loader').show();

var login_emp_id;
var emp_role_id;
var companyid;


$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindEmployeeCodeFromEmpMaster('ddlemployee', companyid);


        $("#ddlcompany").bind("change", function () {
            BindEmployeeCodeFromEmpMaster('ddlemployee', $(this).val());
        });

        // $('#ddlcompany').attr("disabled", true);


        //  $('#ddlemployee').attr("disabled", true);

        $("#btnupdate").hide();

        $("#div_updatedtll").hide();

        $('#txtpayoutmonth').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
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

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnLOPupdate").bind("click", function () {
            $('#loader').show();
            var company_id = $("#ddlcompany").val();
            var employee_code = $("#ddlemployee").val();
            var actual_lop = $("#txtactuallop").val();
            var final_lop = $("#txtfinallop").val();
            var holidayDays = $("#txtholiday").val();
            var weekOffDays = $("#txtWeekOff").val();
            var presentDays = $("#txtPresent").val();
            var absentDays = $("#txtAbsent").val();
            var leaveDays = $("#txtLeave").val();
            var actualPaidDays = $("#txtActualPaid").val();
            var additionalPaidDays = $("#txtAdditionalPaidDays").val();
            var txtRemarks = $("#txtRemarks").val();
            /*var startDate = $("#startDate").val();*/ // month year selection 

            var startDate = $("#hdnmonthyear").val();
            var txttotlDays = $("#txttotlDays").val();

            var errormsg = "";
            var iserror = false;

            if (final_lop == '') {
                errormsg = errormsg + 'Please Enter Final LOD <br/>';
                iserror = true;
            }

            if (company_id == '' || company_id == "0" || company_id == null) {
                errormsg = errormsg + 'Please Select Company <br/>';
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
                Holiday_days: holidayDays,
                Week_off_days: weekOffDays,
                Present_days: presentDays,
                Absent_days: absentDays,
                Leave_days: leaveDays,
                Actual_Paid_days: actualPaidDays,
                Additional_Paid_days: additionalPaidDays,
                remarks: txtRemarks,
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

        $("#btnDetails").bind("click", function () {
            $('#loader').show();
            //var monthyear = $("#txtpayoutmonth").val();

            var txtMonthYear = $("#txtpayoutmonth").val();
            var txtmnyr = new Date($('#txtpayoutmonth').val());
            //var frmtdate = txtmnyr.dateFormat("YYYY-MM");
            var yr = txtmnyr.getFullYear();
            var mnth = txtmnyr.getMonth() + 1;
            if (mnth <= 9) {
                txtMonthYear = yr.toString() + "0" + mnth.toString();
            }
            else {

                txtMonthYear = yr.toString() + mnth.toString();
            }



            GetData(txtMonthYear, $("#ddlcompany").val());

            $('#loader').hide();
        });


        $('#loader').hide();



    }, 2000);// end timeout

});



function GetData(monthyear, company_id) {
    $('#loader').show();
    var apiurll = "";
    if (monthyear == '') {
        messageBox("error", "Please Select Date");
        $('#loader').hide();
        return false;
    }

    if (company_id > 0) {
        apiurll = localStorage.getItem("ApiUrl") + "apiPayroll/Get_Lod_MasterByMonthyearandCompID/" + monthyear + "/" + company_id;
    }
    else {
        apiurll = localStorage.getItem("ApiUrl") + "apiPayroll/Get_Lod_MasterByMonthyear/" + monthyear;
    }


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: apiurll,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            $('#loader').hide();
            _GUID_New();

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
                            targets: [13, 14],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                    ],

                "columns": [
                    { "data": "s_no", "name": "s_no", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "monthyear", "name": "monthyear", "autoWidth": true },
                    { "data": "totaldays", "name": "totaldays", "autoWidth": true },
                    { "data": "holiday_days", "name": "holiday_days", "autoWidth": true },
                    { "data": "week_off_days", "name": "week_off_days", "autoWidth": true },
                    { "data": "present_days", "name": "present_days", "autoWidth": true },
                    { "data": "absent_days", "name": "absent_days", "autoWidth": true },
                    { "data": "leave_days", "name": "leave_days", "autoWidth": true },
                    { "data": "total_Paid_days", "name": "total_Paid_days", "autoWidth": true },
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
            _GUID_New();
            alert(err.responseText);
        }
    })
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

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                $('#loader').hide();
                return false;
            }

            if (response != null) {
                $('#exampleModalLabel').text('L W P Details');
                $("#myLWPModal").show();
                var modal = document.getElementById("myLWPModal");
                modal.style.display = "block";

                $('#myLWPModal').modal({
                    modal: 'true',
                    title: 'L W P Details',
                    backdrop: 'false'
                });

                $("#div_updatedtll").show();

                BindAllEmp_Company('ddlcompany', login_emp_id, response.company_id);

                BindEmployeeCodeFromEmpMasterByComp('ddlemployee', response.company_id, response.emp_id);

                $('#ddlemployee').prop("disabled", "disabled");

                // $('#ddlemployee').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


                var full_monthyearr = response.monthyear.toString();
                var _monthh = full_monthyearr.slice(-2);
                var _yearr = full_monthyearr.substring(0, 4);
                $('#divAdditionalPaid').hide();
                var _monthname = GetMonthName(_monthh);
                $("#startDate").val(_monthname + "-" + _yearr);
                $("#hdnmonthyear").val(_yearr + _monthh);
                $("#txtholiday").val(response.holiday_days);
                $("#txtWeekOff").val(response.week_off_days);
                $("#txtPresent").val(response.present_days);
                $("#txtAbsent").val(response.absent_days);
                $("#txtLeave").val(response.leave_days);
                $("#txtActualPaid").val(response.actual_Paid_days);
                $("#txtAdditionalPaidDays").val(response.additional_Paid_days);
                $("#txtactuallop").val(response.acutual_lop_days);
                $("#txtfinallop").val(response.final_lop_days);
                $("#txttotlDays").val(response.totaldays);
                $("#txtRemarks").val(response.remarks);

                $("#btnsave").hide();
                $("#btnupdate").show();

                $('#loader').hide();
            }
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }

    });
}


function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}
