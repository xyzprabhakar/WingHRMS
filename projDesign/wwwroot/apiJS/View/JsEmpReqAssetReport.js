var emp_role_idd;
var company_idd;
var login_emp;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //BindAllEmp_Company('ddlcompany', login_emp, company_idd);
        //BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', company_idd, login_emp, -1);
        // setSelect('ddlemployee', login_emp);

        //   var d = new Date();

        // alert(d.toLocaleDateString());// Display Current Date
        // d.setMonth(d.getMonth() - 1);
        // alert(d.toLocaleDateString()); // Display Previous Month date
        //var HaveDisplay = ISDisplayMenu("Is Company Admin");

        //if (HaveDisplay == 1) {
        //    var HaveDisplayCompList = ISDisplayMenu("Display Company List");
        //    if (HaveDisplayCompList == 0) {
        //        $('#ddlcompany').prop("disabled", true);
        //    }
        //    else {
        //        $('#ddlcompany').prop("disabled", false);
        //    }
        //    BindCompanyList('ddlcompany', 0);
        //    BindAllEmployeeByCompany('ddlemployee', company_idd, 0);
        //}
        //else {
        //    $('#ddlcompany').prop("disabled", false);
        //    BindCompanyList('ddlcompany', company_idd);
        //    BindEmployeeList1('ddlemployee', login_emp);
        //}




        //var cur_dt = new Date();
        //$("#txtToDate").val(GetDateFormatyyyyMMdd(cur_dt));

        //cur_dt.setMonth(cur_dt.getMonth() - 1);


        //$("#txtFromDate").val(GetDateFormatyyyyMMdd(new Date(cur_dt.toLocaleDateString())));

        if ($("#txtFromDate") != "" && $("#txtFromDate") != null && $("#txtToDate") != "" && $("#txtToDate") != null) {
            GetData(company_idd);
        }

        $("#ddlemployee").bind("change", function () {
            //GetData($(this).val());
            if ($.fn.DataTable.isDataTable('#tblassetrequestdtlreport')) {
                $('#tblassetrequestdtlreport').DataTable().clear().draw();
            }

        });



        $("#btn_get_report").bind("click", function () {
            //var from_date = $("#txtFromDate").val();
            //var to_date = $("#txtToDate").val();

            //if (from_date == "" || from_date == null) {
            //    messageBox("error", "Please select from date");
            //    return false;
            //}
            //if (to_date == "" || to_date == null) {
            //    messageBox("error", "Please select to date");
            //    return false;
            //}
            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                messageBox("error", "Please select company");
                return false;
            }

            if ($("#ddlemployee").val() == null || $("#ddlemployee").val() == "" || $("#ddlemployee").val() == "0") {
                messageBox('error', 'Please select employee');
                return false;
            }

            if ($("#dtpToDt").val() == null || $("#dtpToDt").val() == "") {
                messageBox("error", "Please select To Date");
                return false;
            }

            if ($("#dtpFromDt").val() == null || $("#dtpFromDt").val() == "") {
                messageBox("error", "Please select From Date");
                return false;
            }

            GetData($("#ddlcompany").val());


        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });


    }, 2000);// end timeout

});



//$('#ddlcompany').change(function () {
//    // BindAllEmployeeByCompany('ddlEmployeeCode', $(this).val(), 0);
//    BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', $(this).val(), login_emp, login_emp);
//});



//function GetDateFormatyyyyMMdd(date) {
//    var month = (date.getMonth() + 1).toString();
//    month = month.length > 1 ? month : '0' + month;
//    var day = date.getDate().toString();
//    day = day.length > 1 ? day : '0' + day;
//    return date.getFullYear() + '-' + month + '-' + day;
//}




function GetData(companyidd) {

    if ($.fn.DataTable.isDataTable('#tblassetrequestdtlreport')) {
        $('#tblassetrequestdtlreport').DataTable().clear().draw();
    }

    if (new Date($("#dtpToDt").val()) < new Date($("#dtpFromDt").val())) {
        messageBox("error", "To Date must be greater than or equal to from date");
        return false;
    }
    $('#loader').show();

    var BulkEmpID = [];
    if ($("#ddlemployee").val() == "-1") {
        //var options_ = $("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return $(this).val(); }).get();

        var options_ = $("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return { "emp_id": $(this).val() }; }).get();

        BulkEmpID = options_;
    }
    else {
        BulkEmpID.push({ "emp_id": $("#ddlemployee").val() });
    }

    var mydata = {
        asset_req_id: 0,
        from_date: $("#dtpFromDt").val(),
        to_date: $("#dtpToDt").val(),
        assetreqlist: BulkEmpID,
        company_id: companyidd
    }
    $.ajax({


        url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_AssetRequestReport",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $("#tblassetrequestdtlreport").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "filter": true,//to enable search box
                "aaData": res,
                dom: 'lBfrtip',
                "deferLoading": 57,
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Asset Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]
                        }
                    },
                ],
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "req_emp_name", "name": "req_emp_name", "title": "Employee Name", "autoWidth": true, "visible": (emp_role_idd >= 21 && emp_role_idd < 101 ? false : true) },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                    { "data": "asset_number", "name": "asset_number", "title": "Asset Number", "autoWidth": true },
                    { "data": "_asset_type", "name": "_asset_type", "title": "Action Type", "autoWidth": true },
                    {
                        "title": "Is Permanent", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="chkpermanent" ' + (full.is_permanent == 1 ? 'checked' : '') + ' disabled=true>';
                        }
                    },
                    { "data": "description", "name": "description", "title": "Requester Remarks", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "asset_issue_date", "name": "asset_issue_date", "title": "Issue Date", "autoWidth": true },
                    { "data": "replace_dt", "name": "replace_dt", "title": "Replace Date", "autoWidth": true },
                    { "data": "submission_date", "name": "submission_date", "title": "Submission Date", "autoWidth": true },
                    { "data": "_finalapprove", "name": "_finalapprove", "title": "Final Status", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    //{ "data": "asset_issue_date", "name": "asset_issue_date", "title": "Issue Date", "autoWidth": true },
                    //{ "data": "submission_date", "name": "submission_date", "title": "Submission Date", "autoWidth": true },
                    {
                        "title": "View Details",
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewDetails(' + full.asset_req_id + ',' + full.req_employee_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });
            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function ViewDetails(_req_id, empidd) {

    $('#loader').show();

    $("#myAssetReqModal").show();
    var modal = document.getElementById("myAssetReqModal");
    modal.style.display = "block";

    $('#myAssetReqModal').dialog({
        modal: 'true',
        title: 'Approval Details'
    });



    var mydata = {
        asset_req_id: _req_id,
        from_date: $("#dtpFromDt").val(),
        to_date: $("#dtpToDt").val(),
        emp_id: empidd,
        company_id: company_idd
    }


    $.ajax({

        url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_AssetRequestReport",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response.asset_approval_history;
            var asset_dtl = response.asset_req_detail;


            $("#tblapprovalhistory").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,//to process server side
                "orderMulti": false,//to disable multiple column at once
                "filter": true,//enable search box
                "scrollX": 200,
                "bDestroy": true,//to remove previous data
                "aaData": res,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "order_name", "name": "order_name", "title": "Approver Order", "autoWidth": true },
                    { "data": "approver_emp_name_code", "name": "approver_emp_name_code", "title": "Approver's", "autoWidth": true },
                    { "data": "is_approve", "name": "is_approve", "title": "Status", "autoWidth": true }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });


            $('#loader').hide();
        },
        error: function (err) {
            _GUID_New();
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}