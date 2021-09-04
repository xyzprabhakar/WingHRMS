$('#loader').show();
var emp_role_idd;
var login_company_id;
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', login_company_id, login_emp_id, 0);
       // BindAllEmployeeUnderEmp_on_mleave('ddlemployee', login_company_id, login_emp_id, 0);
        // BindEmployeeCodeFromEmpMasterByComp('ddlemployee', login_company_id, 0);
        //BindOnlyProbation_Confirmed_emp('ddlemployee', login_company_id, 0);

        //BindLeaveLedger();

        BindLeaveTpye();

        $("#btnupdate").hide();

        $('#loader').hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#ddlcompany").bind("change", function () {
            //BindAllEmployeeUnderEmp_on_mleave('ddlemployee', $(this).val(), 0, 0);
            //BindOnlyProbation_Confirmed_emp('ddlemployee', $(this).val(), 0);
            //$("#ddlemployee").empty();
            BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', $(this).val(), login_emp_id, 0);
          //  BindEmployeeCodeFromEmpMasterByComp('ddlemployee', $(this).val(), 0);
        });

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var leave_type_id = $("#ddlleavetype").val();
            //var transaction_date = $("#txttrandate").val();

            var transaction_type = "";
            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '5') {
                    transaction_type = 5; // Credit
                }
                else if ($("input[name='chkstatus']:checked").val() == '6') {
                    transaction_type = 6; //Debit
                }
            }

            var txtMonthYear = $("#txtmonthyear").val();
            //var txtMonthYear = $("#txtmonthyear").val();
            //var txtMonthYear = $("#txtmonthyear").val();
            var txtmnyr = new Date($('#txtmonthyear').val());
            //var frmtdate = txtmnyr.dateFormat("YYYY-MM");
            var yr = txtmnyr.getFullYear();
            var mnth = txtmnyr.getMonth() + 1;
            //txtMonthYear = yr + "0" + mnth;


            if (mnth <= 9) {
                txtMonthYear = yr.toString() + "0" + mnth.toString();
            }
            else {
                txtMonthYear = yr.toString() + mnth.toString();
            }

            var monthyear = txtMonthYear;
            var leave_addition_type = $("#ddlleaveadditiontype").val();
            var amount = $("#txtamount").val();
            var credit = "";
            var dedit = "";
            if (transaction_type == 5) {
                credit = amount;
                dedit = 0;
            }
            else if (transaction_type == 6) {
                dedit = amount;
                credit = 0;
            }

            var remarks = $("#txtremarks").val();
            var e_id = $("#ddlemployee").val();

            var iserror = false;
            var errormsg = "";

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                iserror = true;
                errormsg = errormsg + "Please select company </br>";
            }

            if (leave_type_id == "0") {
                errormsg = errormsg + "Please Select Leave Type</br>";
                iserror = true;
            }

            //if (transaction_date == "") {
            //    errormsg = errormsg + "Please Select Transaction Date</br>";
            //    iserror = true;
            //}

            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please Select Transaction Type !! <br/>";
                iserror = true;
            }

            //if ($("#txtmonthyear").val() == "") {
            //    errormsg = errormsg + "Please Select Month Year</br>";
            //    iserror = true;
            //}

            //if (leave_addition_type == "0") {
            //    errormsg = errormsg + "Please Select Leave Addition Type</br>";
            //    iserror = true;
            //}

            if (amount == "" || amount == 0 || amount == null) {
                errormsg = errormsg + "Please Select Amount</br>";
                iserror = true;
            }

            if (e_id == "0") {
                errormsg = errormsg + "Please Select Employee</br>";
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }


            var mydata = {
                company_id: $("#ddlcompany").val(),
                leave_type_id: leave_type_id,
                // leave_info_id: $("#hdnleaveinfoid").val(),
                //transaction_date: transaction_date,
                transaction_type: transaction_type,
                //monthyear: monthyear,
                //transaction_no: monthyear,
                //leave_addition_type: leave_addition_type,
                credit: credit,
                dredit: dedit,
                remarks: remarks,
                e_id: e_id

            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiLeave/Save_Leave_Ledger",
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


        $("#mannualleavefile").bind("change", function () {
            EL("mannualleavefile").addEventListener("change", readFile, false);
        });


        $("#btnupload_mannual_dtl").bind("click", function () {

            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlcompany").val();


            if (company_id == "0" || company_id == null || company_id == "undefined" || company_id == "") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if ($("#mannualleavefile").length == 0) {
                errormsg = errormsg + "Please select File </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }



            var mydata = {

                company_id: company_id,
                created_by: login_emp_id,
            };

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("mannualleavefile").files[0];

            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $('#loader').show();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiLeave/Upload_LeavefromExcel/",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    var duplist = data.duplicateleavelst;
                    var missingdtllist = data.missingleavelst;
                    var deetailmsg = data.message_;
                    _GUID_New();

                    if (duplist != null && duplist.length > 0) {
                        $("#mannualleavefile").val('');
                        $("#Excel_upload_Modal").show();
                        var modal = document.getElementById("Excel_upload_Modal");
                        modal.style.display = "block";

                        $('#Excel_upload_Modal').dialog({
                            modal: 'true',
                            title: 'Duplicate Detail'
                        });

                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            dom: 'lBfrtip',
                            "aaData": duplist,
                            "columnDefs": [],
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Bulk Upload Leave Issue Detail',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4]
                                    }
                                },
                            ],
                            "columns": [
                                { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                                { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
                                { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
                                { "data": "remarks", "name": "remarks", "title": "Error Message", "autoWidth": true },
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                        });


                        // messageBox("error", deetailmsg);
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {
                        $("#mannualleavefile").val('');
                        $("#Excel_upload_Modal").show();
                        var modal = document.getElementById("Excel_upload_Modal");
                        modal.style.display = "block";

                        $('#Excel_upload_Modal').dialog({
                            modal: 'true',
                            title: 'Missing/Invalid Detail'
                        });


                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": missingdtllist,
                            dom: 'lBfrtip',
                            "columnDefs": [],
                            buttons: [
                                {
                                    text: 'Export to Excel',
                                    title: 'Bulk Upload Leave Issue Detail',
                                    extend: 'excelHtml5',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4]
                                    }
                                },
                            ],

                            "columns": [
                                { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
                                { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
                                { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
                                { "data": "remarks", "name": "remarks", "title": "Error Message", "autoWidth": true },
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                        });

                        // messageBox("error", deetailmsg);
                    }
                    else {

                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        if (statuscode == "0") {
                            alert(Msg);
                            location.reload();
                        }
                        else {
                            $("#mannualleavefile").val('');
                            messageBox("error", Msg);
                            return false;
                        }
                    }


                },
                error: function (error) {
                    _GUID_New();
                    messageBox("error", error.responseText);
                    $('#loader').hide();
                }
            });
        });

    }, 2000);// end timeout

});


function BindLeaveTpye() {

    var apiurll = "";

    apiurll = localStorage.getItem("ApiUrl") + "/apiLeave/Get_LeaveInfo_for_LeaveType";

    $.ajax({
        url: apiurll,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $("#ddlleavetype").empty().append('<option value=0>--Please Select--</option>');
            if (response.length > 0) {

                $.each(response, function (data, value) {
                    $("#ddlleavetype").append($("<option></option>").val(value.leave_type_id).html(value.leave_type_name));
                });
            }
            else if (response.statusCode == 1) {
                messageBox("error", response.message);
                return false;
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}


//function BindLeaveLedger() {
//    var apiurll = "";

//        apiurll = localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveLedger";

//    $.ajax({
//        url: apiurll ,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;

//            $("#tblmannualleavedtl").DataTable({
//                "processing": true,//for show progress bar
//                "serverSide": false, //for process server side
//                "bDestroy": true,
//                "filter": true, // this is for diable filter(search box)
//                "orderMulti": false, // for diable multiple column at once
//                "scrollX": 200,
//                "aaData": res,
//                dom: 'lBfrtip',
//                buttons: [
//                    {
//                        text: 'Export to Excel',
//                        title: 'Leave Detail',
//                        extend: 'excelHtml5',
//                        exportOptions: {
//                            columns: [1, 2, 3, 4, 5]
//                        }
//                    },
//                ],
//                "columnDefs": [
//                    //{
//                    //    targets: [1],
//                    //    render: function (data, type, row) {

//                    //        var date = new Date(data);
//                    //        return GetDateFormatddMMyyyy(date);
//                    //    }
//                    //},
//                    //{
//                    //    targets: [4],
//                    //    render: function (data, type, row) {
//                    //        return data=="1"?"Monthly":data=="2"?"Quaterly":data=="3"?"Half Yearly" :data=="4"?"Annually":""
//                    //    }
//                    //},
//                    //{
//                    //    targets: [9],
//                    //    render: function (data, type, row) {

//                    //        var date = new Date(data);
//                    //        return GetDateFormatddMMyyyy(date);
//                    //    }
//                    //}
//                ],
//                "columns": [
//                    { "data": null, "title": "SNo.", "autowidth": true },
//                    //{ "data": "transaction_date", "name": "transaction_date", "title": "Transaction Date", "autowidth": true },
//                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autowidth": true },
//                    {
//                        "title": "Leave Type", "autowidth": true, "render": function (data, type, full, meta) {
//                            return '<a href="#" onClick=GetLeaveDetails(' + full.company_id + ',' + full.leave_type_id + ',' + full.e_id + ')>' + full.leave_type_name+'</a>'
//                        }
//                    },
//                    //{ "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autowidth": true },
//                    //{ "data": "leave_addition_type", "name":"leave_addition_type","title":"Leave Addition Type","autowidth":true},
//                    //{ "data": "monthyear", "name": "monthyear", "title": "Year Month", "autowidth": true },
//                    { "data": "credit", "name": "credit", "title": "Total Credit", "autowidth": true },
//                    { "data": "dredit", "name": "dredit", "title": "Total Debit", "autowidth": true },
//                    { "data": "balance", "name": "balance", "title": "Leave Balance", "autowidth": true },
//                    //{ "data": "remarks", "name": "remarks", "title": "remarks", "autowidth": true },
//                    //{ "data": "entry_date", "name":"entry_date","title":"Created Date","autowidth":true}
//                ],
//                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
//                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
//                    return nRow;
//                }, // for S.No
//                "lengthMenu":[[10,50,-1],[10,50,"All"]]

//            });               


//        },
//        error: function (err) {
//            $("#loader").hide();
//            messageBox("error", err.responseText);
//        }
//    });
//}

//function GetLeaveDetails(company_idd, leve_type_id, emp_id) {


//    $('#loader').show();

//    $("#myLeaveModal").show();
//    var modal = document.getElementById("myLeaveModal");
//    modal.style.display = "block";

//    $('#myLeaveModal').dialog({
//        modal: 'true',
//        title: 'Leave Detail'
//    });

//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "apiLeave/GetLeaveLedgerByCompID/" + company_idd + "/" + leve_type_id + "/" + emp_id,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: {},
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;


//            $("#tbl_leavetype_dtl").DataTable({
//                "processing": true,//for show progress bar
//                "serverSide": false, //for process server side
//                "bDestroy": true,
//                "filter": true, // this is for diable filter(search box)
//                "orderMulti": false, // for diable multiple column at once
//                "scrollX": 200,
//                "aaData": res,
//                dom: 'lBfrtip',
//                buttons: [
//                    {
//                        text: 'Export to Excel',
//                        title: 'Leave Type Detail',
//                        extend: 'excelHtml5',
//                        exportOptions: {
//                            columns: [1, 2, 3, 4, 5,6,7,8,9]
//                        }
//                    },
//                ],
//                "columnDefs": [
//                      {
//                        targets: [3],
//                        render: function (data, type, row) {

//                            var date = new Date(data);
//                            return GetDateFormatddMMyyyy(date);
//                        }
//                    },
//                ],
//                "columns": [
//                    { "data": null, "title": "SNo.", "autoWidth": true },
//                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
//                    { "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autoWidth": true },
//                    { "data": "transaction_date", "name": "transaction_date", "title": "Transaction Date", "autoWidth": true },
//                    { "data": "transaction_type_name", "name": "transaction_type_name", "title": "Transaction Type", "autoWidth": true },
//                    { "data": "monthyear", "name": "monthyear", "title": "MonthYear", "autoWidth": true },
//                    { "data": "leave_addition_type_name", "name": "leave_addition_type_name", "title": "Leave Addition ", "autoWidth": true },
//                    { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
//                    { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
//                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
//                ],
//                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
//                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
//                    return nRow;
//                },
//                "lengthMenu":[[10,50,-1],[10,50,"All"]],
//            });

//            $('#loader').hide();
//        },
//        error: function (err) {
//            $("#loader").hide();
//            messageBox("error", err.responseText);
//        }
//    });
//}



// start upload

function DownloadMannualLeaveExcelFile() {
    window.open("/UploadFormat/MannualLeave.xlsx");
}


function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#mannualleavefile").val("");
            alert("Upload only Excel file and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#mannualleavefile").val("");
                alert("Upload only Excel file and on define format. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

function BindAllEmployeeUnderEmp_on_mleave(ControlId, Companyid, EmpId, SelectedVal) {
    var listapi = localStorage.getItem("ApiUrl");

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }

    $('#loader').show();
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_Active_Inactive/" + Companyid + "/" + EmpId,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $.each(res, function (data, value) {

                $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });


}

//end upload