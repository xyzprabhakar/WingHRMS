var login_company_id;
var login_emp_id;
var emp_sep;
var empId;
var sepId;
var rndNo;
$(document).ready(function () {
    setTimeout(function () {



        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split("=");

        emp_sep = CryptoJS.AES.decrypt(url[1], localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        GetData(emp_sep);

        emp_Id = emp_sep.toString().split('/')[0];
        sep_Id = emp_sep.toString().split('/')[1];
        $("#hdnSepId").val(sep_Id);
        rndNo = Math.floor((Math.random() * 100) + 1);
        debugger;
        GetFNFReport(login_company_id, login_emp_id);

        $('#noticerecovery_days_app').change(function () {
            if (this.checked) {
                $('#divnotice_days').attr("style", "display:block");
            }
            else {
                $('#divnotice_days').attr("style", "display:none");
            }

        });
        $('#notice_payment_app').change(function () {
            if (this.checked) {
                $('#divnotice_payemnt').attr("style", "display:block");
            } else {
                $('#divnotice_payemnt').attr("style", "display:none");
            }
        });

    }, 2000);// end timeout


});

//function openSelectedTab(evt, cityName) {


//    var i, tabcontent, tablinks;
//    tabcontent = document.getElementsByClassName("tabcontent");
//    for (i = 0; i < tabcontent.length; i++) {
//        tabcontent[i].style.display = "none";
//    }
//    tablinks = document.getElementsByClassName("tablinks");
//    for (i = 0; i < tablinks.length; i++) {
//        tablinks[i].className = tablinks[i].className.replace(" btn-primary text-light", "  btn-outline-light");
//    }
//    document.getElementById(cityName).style.display = "block";
//    evt.currentTarget.className += " btn-primary text-light";

//    if (cityName == "tab2attendance") {
//        var current_month = new Date().getMonth() + 1;
//        var current_year = new Date().getFullYear();
//        $("#txtpayrollmonth").val("" + current_year + '-' + (current_month <= 9 ? "0" + current_month : current_month) + "");

//        Get_AttendanceData(emp_sep);
//    }
//    else if (cityName == "tab4remibursment") {
//        GetReimData(emp_sep);
//    }
//    //else if (cityName =="tab3Loan") {
//    //    GetLoanTab(emp_sep);
//    //}
//    else if (cityName == "tab6settlement")
//    {
//        $("#txtsettlement_dt").val(GetDateFormatyyyyMMdd(new Date));
//    }
//    else if (cityName == "tab7report") {
//        var win = window.open("/FNFProcess/FNFReport", "blank");
//        win.focus();
//    }

//}



function GetData(emp_sep) {
    if (emp_sep == undefined || emp_sep == null || emp_sep == "") {
        messageBox("error", "Something went wrong can't get the FNF Get");
        return false;
    }
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/GetFNFDetails/" + emp_sep.toString().split('/')[0] + "/" + emp_sep.toString().split('/')[1],
        type: "GET",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        data: {},
        success: function (response) {
            debugger;
            var data_ = response
            clearall();
            $("#loader").hide();
            if (data_.statusCode != undefined) {
                messageBox("error", data_.message);
                return false;
            }

            $("#hdnEmpId").val(data_[0].emp_id);
            $("#hdnCompany_id").val(data_[0].company_id);
            $("#txtcompany").val(data_[0].company_name);
            $("#txtempcode").val(data_[0].emp_code);
            $("#txtemployee").val(data_[0].emp_name);
            $("#txtdepartment").val(data_[0].dept_name);
            //$("#txtdob").val(GetDateFormatddMMyyyy(new Date(data_[0].dob)));
            $("#txtdesignation").val(data_[0].desig_name);
            $("#txtdoj").val(GetDateFormatddMMyyyy(new Date(data_[0].doj)));
            $("#txtlcoation").val(data_[0].lcoation);
            $("#txtnationality").val(data_[0].nationality);
            $("#txtempstatus").val(data_[0].emptype);
            //$("#txtreligion").val(data_[0].religion);
            $("#txtgrade").val(data_[0].grade);
            $("#txtresigndt").val(GetDateFormatddMMyyyy(new Date(data_[0].resign_dt)));
            $("#txtfinal_relievedt").val(GetDateFormatddMMyyyy(new Date(data_[0].last_working_date)));
            //$("#txtpfno").val(data_[0].pf_number);
            //$("#txtpan_no").val(data_[0].pan_card_number);
            //$("#txt_esicno").val(data_[0].esic_number)

            //Sattlement tab data
            debugger;
            $("#txtsettlement_net_amt").val(data_[0].net_amt);
            $("#txtsettlement_amt").val(data_[0].settlment_amt);
            $("#ddlsettlement_type").val(data_[0].settlement_type);
            var st_date = GetDateFormatddMMyyyy(new Date(data_[0].settlment_dt));
            $("#txtsettlement_dt").val(st_date);

            // Salary Tab
            if (data_[0].notice_recovery_days != null && data_[0].notice_recovery_days != undefined && data_[0].notice_recovery_days > 0) {
                $("#noticerecovery_days_app").prop('checked', true);
                $('#divnotice_days').attr("style", "display:block");
                $('#notice_payment_app').prop('checked', true);
                $('#divnotice_payemnt').attr("style", "display:block");


                $("#txtnotice_recovery_day").val(data_[0].notice_recovery_days);
                $("#txtnotice_payment").val(data_[0].notice_Payment);
            }


            if (data_[0].mdl_assetrequest != null && data_[0].mdl_assetrequest.length > 0) {
                $("#divassetsave").show();
            }
            else {
                $("#divassetsave").hide();
            }

            var Leave_Enc = data_[0].mdl_attendance;

            // Leave Encashment Data
            $("#tbl_leave_dtl").DataTable({
                "processing": true,
                "serverSide": false,
                "orderMulti": false,
                "bDestroy": true,
                "scrollX": 200,
                "filter": true,
                "data": Leave_Enc.mdl_LeaveEncashed,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                    { "data": "balance", "name": "balance", "title": "Leave Balance", "autoWidth": true },
                    //{ "data": "credit", "name": "credit", "title": "Leave Encash", "autoWidth": true },
                    {
                        "title": "Leave Encash Days", "autoWidth": true, "render": function (data, type, full, meta) {
                            if (full.credit > 0) {
                                return '<input type="text" id="txtempcode" class="form-control" value="' + full.credit + '"  style="width: 100px;" />'
                            }
                            else {
                                return '<input type="text" id="txtempcode" class="form-control" value="' + full.balance + '"  style="width: 100px;" />'
                            }
                        }
                    },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });

            debugger;
            // Attendence Tab Data
            var attendenceDetail = Leave_Enc.mdl_FNFAttendance;



            // Variable Pay Tab Data
            var variablePayDetail = Leave_Enc.mdl_VariablePay;


            //$("#tbl_asset_request").DataTable({
            //    "processing": true,
            //    "serverSide": false,
            //    "orderMulti": false,
            //    "bDestroy": true,
            //    "scrollX": 200,
            //    "filter": true,
            //    "data": data_[0].mdl_assetrequest,
            //    "columnDefs": [
            //        {
            //            targets: 1,
            //            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
            //        },
            //        {
            //            targets: [5],
            //            render: function (data, type, row) {

            //                var date = new Date(data);
            //                return GetDateFormatddMMyyyy(date);
            //            }
            //        },
            //        {
            //            targets: [7],
            //            render: function (data, type, row) {

            //                var date = new Date(data);
            //                return GetDateFormatddMMyyyy(date);
            //            }
            //        },
            //        {
            //            targets: [8],
            //            render: function (data, type, row) {

            //                var date = new Date(data);
            //                return GetDateFormatddMMyyyy(date);
            //            }
            //        },
            //        {
            //            targets: [9],
            //            render: function (data, type, row) {

            //                var date = new Date(data);
            //                return GetDateFormatddMMyyyy(date);
            //            }
            //        },
            //    ],
            //    "columns": [
            //        { "data": null, "title": "SNo.", "autoWidth": true },
            //        {
            //            "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' />Select All", "autoWidth": true,
            //            "render": function (data, type, full, meta) {
            //                return full.final_status == 1 ? '<input type="checkbox" class="chkRow" id=' + full.asset_req_id + ' />' : '';
            //            }
            //        },
            //        { "data": "asset_name", "name": "asset_name", "title": "Asset Name", "autoWidth": true },
            //        { "data": "asset_no", "name": "", "title": "Asset Number", "autoWidth": true },
            //        { "data": "req_remarks", "name": "req_remarks", "title": "Requester Remarks", "autoWidth": true },
            //        { "data": "from_dt", "name": "from_dt", "title": "From Date", "autoWidth": true },
            //        {
            //            "title": "To Date", "autoWidth": true, "render": function (data, type, full, meta) {
            //                return full.is_permanent == 0 ? '' + GetDateFormatddMMyyyy(new Date(full.to_dt)) + '' : '';
            //            }
            //        },
            //        { "data": "issue_dt", "name": "issue_dt", "title": "Issue Date", "autoWidth": true },
            //        { "data": "replace_dt", "name": "replace_dt", "title": "Replace Date", "autoWidth": true },
            //        { "data": "submission_dt", "name": "submission_dt", "title": "Submission Date", "autoWidth": true },
            //        { "data": "final_approve", "name": "final_approve", "title": "Final Status", "autoWidth": true },
            //        {
            //            "title": "Recovery Amount", "autoWidth": true, "render": function (data, type, full, meta) {
            //                return (full.is_process == 0 && full.final_status == 1) ? '<input type=text maxlength="10" id=txt_' + full.asset_req_id + '_' + full.empid + ' value=' + full.recovery_amt + ' class="form-control"  onkeypress="return isNumber(event,this)"/>' : '';
            //            }
            //        }
            //    ],
            //    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
            //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            //        return nRow;
            //    },
            //});


        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false
        }
    });
}

function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tbl_asset_request').DataTable().cells().nodes();
    var currentPages = $('#tbl_asset_request').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbl_asset_request").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(allPages).find('.chkRow').prop('checked', false);
            $(currentPages).find('.chkRow').prop('checked', true);
        }
        else {
            $(allPages).find('.chkRow').prop('checked', false);
        }
    });

}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbl_asset_request").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function setassetprocessdata(data_) {
    $("#loader").show();

    //$("#tbl_leave_encash").DataTable({
    //    "processing": true,
    //    "serverSide": false,
    //    "orderMulti": false,
    //    "bDestroy": true,
    //    "scrollX": 200,
    //    "filter": true,
    //    "data": data_[0].mdl_attendance,
    //    "columnDefs": [],
    //    "columns": [
    //        { "data": null, "title": "SNo.", "autoWidth": true },
    //        { "data": "leave_type_id", "autoWidth": true, "visible": false },
    //        { "data": "e_id", "autoWidth": true, "visible":false},
    //        { "data": "leave_type_name","title": "Leave Type", "autoWidth": true },
    //        { "data": "balance", "title": "Asset Number", "autoWidth": true },
    //        {
    //            "title": "Leave Encash", "autoWidth": true, "render": function (data, type, full, meta) {
    //                return '<input type="text" id="txtleave_cash" placeholder="Leave Encash" onkeypress="return isNumber(event, this);" maxlength="5" autocomplete="off" />';
    //            }
    //        },
    //    ],
    //    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
    //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
    //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
    //        return nRow;
    //    },
    //});

    $("#loader").hide();
}


function clearall() {
    $("#txtcompany").val('');
    $("#txtempcode").val('');
    $("#txtemployee").val('');
    $("#txtdepartment").val('');
    //$("#txtdob").val('');
    $("#txtdesignation").val('');
    $("#txtdoj").val('');
    $("#txtlcoation").val('');
    $("#txtnationality").val('');
    $("#txtempstatus").val('');
    //$("#txtreligion").val('');
    $("#txtgrade").val('');
    $("#txtresigndt").val('');
    $("#txtfinal_relievedt").val('');
    if ($.fn.DataTable.isDataTable('#tbl_asset_request')) {
        $('#tbl_asset_request').DataTable().clear().draw();
    }
    //if ($.fn.DataTable.isDataTable('#tbl_reimbursment')) {
    //    $('#tbl_reimbursment').DataTable().clear().draw();
    //}
}

$("#btnasset_save").bind("click", function () {
    var BulAppId = [];
    var table = $("#tbl_asset_request").DataTable();

    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        var data = this.data();

        var _ischecked = table.cell(rowIdx, 1).nodes().to$().find('input').is(":checked");

        if (_ischecked == true) {
            var requestss = {};

            var req = table.cell(rowIdx, 11).nodes().to$().find('input').attr('id').split('txt_')[1];

            requestss.asset_req_id = req.split('_')[0];
            requestss.empid = req.split('_')[1];
            requestss.recovery_amt = table.cell(rowIdx, 11).nodes().to$().find('input').val();
            BulAppId.push(requestss);
        }
    });

    if (BulAppId.length == 0 || BulAppId.length < 0) {
        messageBox("error", "Please select asset which by entering amount which you want to recover");
        return false;
    }

    var diff_id = BulAppId.filter(p => parseInt(p.empid) != parseInt(emp_sep.toString().split('/')[0]));
    if (diff_id != null || diff_id != "" || diff_id.length > 0) {
        messageBox("error", "Something went wrong can't proceed request");
        return false;
    }
    if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

        mydata = {
            mdl_assetrequest: BulAppId,
        }

        $("#loader").show();
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();


        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_Emp_FNF_asset",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(mydata),
            headers: headerss,
            success: function (response) {
                _GUID_New();
                var res = response;
                var statuscode = res.statusCode;
                var msg = res.message;
                $("#loader").hide();
                if (statuscode != "0") {
                    messageBox("error", msg);
                    return false;
                }
                else {
                    //set_reimbursment();
                    GetData(emp_sep);
                    messageBox("success", msg);
                    return false;
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
                return false;
            }


        });
    }
    else {
        $('#loader').hide();
        return false;
    }
});

$("#btnasset_cncl").bind("click", function () {
    var BulAppId = [];
    var table = $("#tbl_asset_request").DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        table.cell(rowIdx, 11).nodes().to$().find('input').val('0');
        table.cell(rowIdx, 1).nodes().to$().find('input').prop("checked", false);
    });

});

$("#btn_add_variablepay").bind("click", function () {

    var emp_Id = $("#hdnEmpId").val();
    var processmonthyear;
    var txtmnyr = new Date($('#txtProcessMonthYear').val());
    if ($('#txtProcessMonthYear').val() == undefined || $('#txtProcessMonthYear').val() == "") {
        alert('Please select valid month in variable pay..');
        $('#txtProcessMonthYear').focus();
        return;
    }
    var yr = txtmnyr.getFullYear();
    var mnth = txtmnyr.getMonth() + 1;
    if (mnth <= 9) {
        processmonthyear = yr.toString() + "0" + mnth.toString();
    }
    else {
        processmonthyear = yr.toString() + mnth.toString();
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryComponenetDetailsALL/' + emp_Id + "/" + processmonthyear;
    $("#loader").show();
    $.ajax({
        url: apiurl,
        type: "GET",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        data: {},
        success: function (response) {
            $('#varModelHeader').text('Variable Pay');
            $("#myVariableModal").show();
            var modal = document.getElementById("myVariableModal");
            modal.style.display = "block";

            $('#myVariableModal').modal({
                modal: 'true',
                title: 'Attendence Details',
                backdrop: 'false'
            });


            var data_ = response;
            $("#tbl_variable_pay").DataTable({
                "processing": true,
                "serverSide": false,
                "orderMulti": false,
                "bDestroy": true,
                "scrollX": 200,
                "filter": true,
                "data": data_,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "component_id", "name": "component_id", "title": "component_id", "autoWidth": true, "visible": false },
                    { "data": "component_name", "name": "component_name", "title": "component_name", "autoWidth": true, "visible": false },
                    { "data": "component_type", "name": "component_type", "title": "component_type", "autoWidth": true, "visible": false },
                    { "data": "property_details", "name": "property_details", "title": "Item", "autoWidth": true },
                    { "data": "component_value", "name": "component_value ", "title": "Value", "autoWidth": true },
                    {
                        "title": "New Value", "autoWidth": true, "render": function (data, type, full, meta) {
                            debugger
                            if ($("#chk_isGratuity").is(":checked") == false && full.property_details == "Gratuity") {

                                return '<input type="text" id="txtempcode" readonly class="form-control" value="' + full.component_value + '"  style="width: 100px;" />'
                            }
                            return '<input type="text" id="txtempcode" class="form-control" value="' + full.component_value + '"  style="width: 100px;" />'
                        }
                    },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false
        }
    });

});

$("#btn_add_attendance").bind("click", function () {

    var emp_Id = $("#hdnEmpId").val();
    var company_id = $("#hdnCompany_id").val();
    var payrollmonth;
    var txtmnyr = new Date($('#txtProcessMonthYear').val());
    if ($('#txtProcessMonthYear').val() == undefined || $('#txtProcessMonthYear').val() == "") {
        alert('Please select valid payroll month for Attendence..');
        $('#txtProcessMonthYear').focus();
        return;
    }
    var yr = txtmnyr.getFullYear();
    var mnth = txtmnyr.getMonth() + 1;
    if (mnth <= 9) {
        payrollmonth = yr.toString() + "0" + mnth.toString();
    }
    else {
        payrollmonth = yr.toString() + mnth.toString();
    }


    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_Lod_MasterByEmp/" + emp_Id + "/" + company_id + "/" + payrollmonth,
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
            debugger;
            if (response != null) {
                $('#attModelHeader').text('Attendence Details');
                $("#myAttendenceModal").show();
                var modal = document.getElementById("myAttendenceModal");
                modal.style.display = "block";

                $('#myAttendenceModal').modal({
                    modal: 'true',
                    title: 'Attendence Details',
                    backdrop: 'false'
                });

                $("#div_updatedtll").show();

                //BindAllEmp_Company('ddlcompany', login_emp_id, response.company_id);

                //BindEmployeeCodeFromEmpMasterByComp('ddlemployee', response.company_id, response.emp_id);
                debugger;
                $('#div_Emp_rem').hide();
                $('#div_comp_mont').hide();
                $('#btnreset').hide();


                $("#txtholiday").val(response[0].holiday_days);
                $("#txtWeekOff").val(response[0].week_off_days);
                $("#txtPresent").val(response[0].present_days);
                $("#txtAbsent").val(response[0].absent_days);
                $("#txtLeave").val(response[0].leave_days);
                $("#txtActualPaid").val(response[0].total_Paid_days);
                $("#txtactuallop").val(response[0].acutual_lop_days);
                $("#txtfinallop").val(response[0].final_lop_days);
                $("#txttotlDays").val(response[0].totaldays);
                $("#txtAdditionalPaidDays").val(response[0].additional_Paid_days);

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

});

//Save Leave Encashment Data
$("#btnLeaveEncash_save").bind("click", function () {
    SaveLeaveEncash();
});

function SaveLeaveEncash() {

    var requestss = {};
    var leaverequestss = [];

    var emp_Id = $("#hdnEmpId").val();
    var sep_id = $("#hdnSepId").val();
    var comp_id = $("#hdnCompany_id").val();
    var resignDate = $("#txtresigndt").val();
    var lastWorkingDate = $("#txtfinal_relievedt").val();
    var isgratuity = 0;

    if ($("#chk_isGratuity").is(":checked") == true) {
        isgratuity = 1;
    }

    var tblLeaveEncash = $("#tbl_leave_dtl").dataTable();

    $("input:text", tblLeaveEncash.fnGetNodes()).each(function () {
        var newValue = $(this).val();
        var currentRow = $(this).closest("tr");
        var data = $("#tbl_leave_dtl").DataTable().row(currentRow).data();
        var leaverequest = {};
        if (newValue >= 1) {
            leaverequest.leave_type_id = data.leave_type_id;
            leaverequest.leave_encash = newValue;
            leaverequest.balance = data.balance;
        }
        leaverequestss.push(leaverequest);
    });

    requestss.emp_id = emp_Id;
    requestss.sep_id = sep_id;
    requestss.company_id = comp_id;
    requestss.resign_dt = resignDate;
    requestss.last_working_date = lastWorkingDate;
    requestss.is_gratuity = isgratuity;
    requestss.x_id = rndNo;
    requestss.mdl_attendance = {};
    requestss.mdl_attendance.mdl_LeaveEncashed = leaverequestss;

    $("#loader").show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_Emp_LeaveEncash",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(requestss),
        headers: headerss,
        success: function (response) {
            _GUID_New();
            var res = response;
            var statuscode = res.statusCode;
            var msg = res.message;
            $("#loader").hide();
            if (statuscode != "0") {
                messageBox("error", msg);
                return false;
            }
            else {
                //set_reimbursment();
                //GetData(emp_sep);
               // $("#btnLeaveEncash_save").prop("disabled", true);
                messageBox("success", msg);
                return false;
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
            return false;
        }


    }); // Ajax End

}

// Save Attendence Data
$("#btnLOPupdate").bind("click", function () {
    SaveFNFAttendence();
});

function SaveFNFAttendence() {

    var requestss = {};
    var att_request = {};
    var attendencerequestss = [];

    var emp_Id = $("#hdnEmpId").val();
    var sep_id = $("#hdnSepId").val();
    var comp_id = $("#hdnCompany_id").val();
    var resignDate = $("#txtresigndt").val();
    var lastWorkingDate = $("#txtfinal_relievedt").val();
    var isgratuity = 0;
    var payrollmonth = 0;

    if ($("#chk_isGratuity").is(":checked") == true) {
        isgratuity = 1;
    }
    var txtmnyr = new Date($('#txtProcessMonthYear').val());
    if ($('#txtProcessMonthYear').val() == undefined || $('#txtProcessMonthYear').val() == "") {
        alert('Please select valid payroll month for Attendence..');
        $('#txtProcessMonthYear').focus();
        return;
    }
    var yr = txtmnyr.getFullYear();
    var mnth = txtmnyr.getMonth() + 1;
    if (mnth <= 9) {
        payrollmonth = yr.toString() + "0" + mnth.toString();
    }
    else {
        payrollmonth = yr.toString() + mnth.toString();
    }

    debugger;
    att_request.Holiday_days = $("#txtholiday").val();
    att_request.Week_off_days = $("#txtWeekOff").val();
    att_request.Present_days = $("#txtPresent").val();
    att_request.Absent_days = $("#txtAbsent").val();
    att_request.Leave_days = $("#txtLeave").val();
    att_request.Actual_Paid_days = $("#txtActualPaid").val();
    att_request.Additional_Paid_days = $("#txtAdditionalPaidDays").val();
    att_request.acutual_lop_days = $("#txtactuallop").val();
    att_request.final_lop_days = $("#txtfinallop").val();
    att_request.totaldays = $("#txttotlDays").val();
    att_request.remarks = $("#txtRemarks").val();
    attendencerequestss.push(att_request);

    requestss.emp_id = emp_Id;
    requestss.sep_id = sep_id;
    requestss.company_id = comp_id;
    requestss.resign_dt = resignDate;
    requestss.last_working_date = lastWorkingDate;
    requestss.is_gratuity = isgratuity;
    requestss.x_id = rndNo;
    requestss.monthyear = payrollmonth;
    requestss.mdl_attendance = {};
    requestss.mdl_attendance.mdl_FNFAttendance = attendencerequestss;
    $("#loader").show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_FNF_Attendence",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(requestss),
        headers: headerss,
        success: function (response) {
            _GUID_New();
            var res = response;
            var statuscode = res.statusCode;
            var msg = res.message;
            $("#loader").hide();
            $("#myAttendenceModal").modal('hide');

            if (statuscode != "0") {
                messageBox("error", msg);
                return false;
            }
            else {
                //set_reimbursment();
                GetData(emp_sep);
               // $("#btn_add_attendance").prop("disabled", true);
                messageBox("success", msg);
                return false;
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
            return false;
        }


    }); // Ajax End

}

// Save Variable Pay Data
$("#btn_SaveVariablePay").bind("click", function () {
    SaveVariablePay();
});

function SaveVariablePay() {
    debugger;
    var requestss = {};
    var componentrequestss = [];
    var payrollmonth = 0;
    var emp_Id = $("#hdnEmpId").val();
    var sep_id = $("#hdnSepId").val();
    var comp_id = $("#hdnCompany_id").val();
    var resignDate = $("#txtresigndt").val();
    var lastWorkingDate = $("#txtfinal_relievedt").val();
    var isgratuity = 0;

    if ($("#chk_isGratuity").is(":checked") == true) {
        isgratuity = 1;
    }

    var txtmnyr = new Date($('#txtProcessMonthYear').val());
    if ($('#txtProcessMonthYear').val() == undefined || $('#txtProcessMonthYear').val() == "") {
        alert('Please select valid payroll month for Attendence..');
        $('#txtProcessMonthYear').focus();
        return;
    }
    var yr = txtmnyr.getFullYear();
    var mnth = txtmnyr.getMonth() + 1;
    if (mnth <= 9) {
        payrollmonth = yr.toString() + "0" + mnth.toString();
    }
    else {
        payrollmonth = yr.toString() + mnth.toString();
    }


    var tblVrPay = $("#tbl_variable_pay").dataTable();

    $("input:text", tblVrPay.fnGetNodes()).each(function () {
        var newValue = $(this).val();
        var currentRow = $(this).closest("tr");
        var data = $("#tbl_variable_pay").DataTable().row(currentRow).data();
        var componentequest = {};
        if (newValue != null || newValue != undefined) {
            componentequest.component_id = data.component_id;
            componentequest.amt = newValue;
            componentequest.component_value = data.component_value;
            componentequest.component_type = data.component_type;
        }
        componentrequestss.push(componentequest);
    });


    requestss.emp_id = emp_Id;
    requestss.sep_id = sep_id;
    requestss.company_id = comp_id;
    requestss.resign_dt = resignDate;
    requestss.last_working_date = lastWorkingDate;
    requestss.is_gratuity = isgratuity;
    requestss.monthyear = payrollmonth;
    requestss.x_id = rndNo;
    requestss.mdl_attendance = {};
    requestss.mdl_attendance.mdl_VariablePay = componentrequestss;

    $("#loader").show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_Emp_VariablePay",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(requestss),
        headers: headerss,
        success: function (response) {
            debugger;
            _GUID_New();
            var res = response;
            var statuscode = res.statusCode;
            var msg = res.message;
            $("#loader").hide();
            $("#myVariableModal").modal('hide');

            if (statuscode != "0") {
                messageBox("error", msg);
                return false;
            }
            else {
                //set_reimbursment();
                //GetData(emp_sep);
               // $("#btn_add_variablepay").prop("disabled", true);
                messageBox("success", msg);
                return false;
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
            return false;
        }


    }); // Ajax End

}

// Save FNF Process Data 
$("#btnfnf_process").bind("click", function () {
    SaveFNFProcess();
});

function SaveFNFProcess() {

    var requestss = {};
    var payrollmonth = 0;
    var emp_Id = $("#hdnEmpId").val();
    var sep_id = $("#hdnSepId").val();
    var comp_id = $("#hdnCompany_id").val();
    var resignDate = $("#txtresigndt").val();
    var lastWorkingDate = $("#txtfinal_relievedt").val();
    var notice_recovery_day = $("#txtnotice_recovery_day").val();
    var notice_payment = $("#txtnotice_payment").val();
    var isgratuity = 0;
    var is_notice_recovery = 0;
    var is_notice_payment = 0;


    if ($("#chk_isGratuity").is(":checked") == true) {
        isgratuity = 1;
    }
    if ($("#noticerecovery_days_app").is(":checked") == true) {
        is_notice_recovery = 1;
    }
    if ($("#notice_payment_app").is(":checked") == true) {
        is_notice_payment = 1;
    }

    var txtmnyr = new Date($('#txtProcessMonthYear').val());
    if ($('#txtProcessMonthYear').val() == undefined || $('#txtProcessMonthYear').val() == "") {
        alert('Please select valid payroll month for Attendence..');
        $('#txtProcessMonthYear').focus();
        return;
    }
    var yr = txtmnyr.getFullYear();
    var mnth = txtmnyr.getMonth() + 1;
    if (mnth <= 9) {
        payrollmonth = yr.toString() + "0" + mnth.toString();
    }
    else {
        payrollmonth = yr.toString() + mnth.toString();
    }


    requestss.emp_id = emp_Id;
    requestss.sep_id = sep_id;
    requestss.company_id = comp_id;
    requestss.resign_dt = resignDate;
    requestss.monthyear = payrollmonth;
    requestss.last_working_date = lastWorkingDate;
    requestss.is_gratuity = isgratuity;
    requestss.is_notice_recovery = is_notice_recovery;
    requestss.is_notice_payment = is_notice_payment;
    requestss.notice_recovery_days = notice_recovery_day;
    requestss.notice_Payment = notice_payment;
    requestss.x_id = rndNo;

    $("#loader").show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_FNF_Process",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(requestss),
        headers: headerss,
        success: function (response) {
            _GUID_New();
            var res = response;
            var statuscode = res.statusCode;
            var msg = res.message;
            $("#loader").hide();

            if (statuscode != "0") {
                messageBox("error", msg);
                return false;
            }
            else {
                //set_reimbursment();
                GetData(emp_sep);
                messageBox("success", msg);
                return false;
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
            return false;
        }


    }); // Ajax End

}



//Save Settlement data made by Anil 
$("#btnsettlement_save").bind("click", function () {
    //var settlementd = [];
    var requestss = {};
    if ($("#txtsettlement_net_amt").val() == "") {
        messageBox("error", "Net Amount should not be blank !!!");
        return false;
    }
    if ($("#txtsettlement_amt").val() == "") {
        messageBox("error", "Settlement Amount should not be blank !!!");
        return false;
    }
    if ($("#ddlsettlement_type").val() == "0") {
        messageBox("error", "Please select settlement type !!!");
        return false;
    }
    if ($("#txtsettlement_dt").val() == "") {
        messageBox("error", "Settlement Date should not be blank!!!");
        return false;
    }
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split("=");
    emp_sep = CryptoJS.AES.decrypt(url[1], localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    emp_Id = emp_sep.toString().split('/')[0];
    sep_Id = emp_sep.toString().split('/')[1];

    requestss.net_amt = $("#txtsettlement_net_amt").val();
    requestss.settlment_amt = $("#txtsettlement_amt").val();
    requestss.settlement_type = $("#ddlsettlement_type").val();
    requestss.settlment_dt = $("#txtsettlement_dt").val();
    requestss.emp_id = emp_Id;
    requestss.fkid_empSepration = sep_Id;
    // settlementd.push(requestss);
    debugger;

    $("#loader").show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_Settlement_FNF",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(requestss),
        // data: JSON.stringify(settlementd),
        headers: headerss,
        success: function (response) {
            _GUID_New();
            var res = response;
            var statuscode = res.statusCode;
            var msg = res.message;
            $("#loader").hide();
            if (statuscode != "0") {
                messageBox("error", msg);
                return false;
            }
            else {
                //set_reimbursment();
                GetData(emp_sep);
                messageBox("success", msg);
                return false;
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
            return false;
        }


    });

});


function GetFNFReport(companyid, employeeid) {

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_FNFReport/" + companyid + "/" + employeeid,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            var res = response;
            _GUID_New();
            $("#tblemptypedtl").DataTable({
                "processing": true,// for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true, // for Distroy previous data
                "filter": true, //this is for disable filter(search box)
                "orderMulti": false, // for disable multiple column at once,
                "scrollX": 150,
                "aaData": res,
                "columnDefs": [

                    {
                        targets: [3],
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
                        targets: [12],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {

                            return data == 0 ? "No" : "Yes";
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {

                            return data == 1 ? "Settled" : data == 2 ? "Forfeited" : data == 3 ? "On Hold" : "No Type";
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autowidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autowidth": true },
                    { "data": "resignationdate", "name": "resignationdate", "title": "Resignation Date", "autowidth": true },
                    { "data": "isdue", "name": "isdue", "title": "Is Due", "autowidth": true },
                    { "data": "reqreason", "name": "reqreason", "title": "Reason", "autowidth": true },
                    { "data": "noticedays", "name": "noticedays", "title": "Notice Day", "autowidth": true },
                    { "data": "net_amt", "name": "net_amt", "title": "Net Amount", "autowidth": true },
                    { "data": "settlment_amt", "name": "settlment_amt", "title": "Settlment Amount", "autowidth": true },
                    { "data": "settlement_type", "name": "settlement_type", "title": "Settlment Type", "autowidth": true },
                    { "data": "settlement_date", "name": "settlement_date", "title": "Settlment Date", "autowidth": true },
                    { "data": "fnf_month_year", "name": "fnf_month_year", "title": "FNF Month Year", "autowidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created Date", "autowidth": true },
                    {
                        "title": "View",
                        "render": function (data, type, full, meta) {
                            //if (full.isfreezed == 1) {
                            return '<a href="#" onclick="ViewFNFSlip(' + full.emp_id + ',' + full.fnf_month_year + ')" >View</a>';
                            //}
                            //else {
                            //    return '<label></label>';
                            //}
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (err) {
            _GUID_New();
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

function ViewFNFSlip(employeeid, fnf_month_year) {
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //headerss["salt"] = $("#hdnsalt").val();

    var doc = new jsPDF();

    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetFNFSlip/" + employeeid + "/" + fnf_month_year,
        type: "GET",
        contentType: "application.json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            //_GUID_New();
            //window.open(response.path);

            if (response.pdf_data == null || response.pdf_data == "") {
                messageBox("error", "FNF slip not available");
                $("#loader").hide();
                return false
            }

            newWin = window.open("");
            newWin.document.write(response.pdf_data);
            newWin.print();
            newWin.close();
            $("#loader").hide();

            $('#div_show_d').empty();

        },
        error: function (err) {
            alert(err.responseText);
            // _GUID_New();
            $("#loader").hide();

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




