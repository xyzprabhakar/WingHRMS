
var emp_role_idd;
var company_idd;
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        $('#div_approval').hide();
        $('#btn_view').hide();




        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp, company_idd);
        Get_PendingAssetRequestFromEmpMasterByCom('ddlassetreq', company_idd, 0);
        //var login_emp_role_id = localStorage.getItem("emp_role_id");


        GetData(login_emp, emp_role_idd);


        $('#div_approval').hide();
        $('#btn_view').hide();
        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#ddlCompany').bind("change", function () {

            $("#ddlassetreq option").remove();

            Get_PendingAssetRequestFromEmpMasterByCom('ddlassetreq', $(this).val(), 0);
        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        //$("#btnSubmit").bind("click", function () {
        //    $('#loader').show();
        //    var asset_req_id = $("#hdnasset_req_id").val();
        //    var emp_and_asset_req_id = $("#ddlassetreq").val().toString().split('/');

        //    var emp_id = emp_and_asset_req_id[0];
        //    var designation = $("#txtdesignation").val();
        //    var dept = $("#txtdepartment").val();
        //    var grade = $("#txtgrade").val();
        //    var assetname = $("#txtassetname").val();
        //    var assetdesc = $("#txtassetdescription").val();
        //    //var salary = $("#txtmonthlysalary").val();
        //    //var rate = $("#txtroi").val();
        //    //var loantype = $("#txtloantype").val();
        //    //var amt = $("#txtloanamt").val();
        //    //var tenure = $("#txtloantenure").val();
        //    //var reason = $("#txtloanpurpose").val();
        //    var status = $("#ddlstarus").val();

        //    // var is_active = 0;
        //    var errormsg = '';
        //    var iserror = false;
        //    var error = 0;



        //    if (asset_req_id == '' || asset_req_id == '0') {
        //        errormsg = errormsg + 'Please fill loan request id !! <br />';
        //        iserror = true;
        //    }
        //    if (designation == '' || designation == null) {
        //        errormsg = errormsg + 'Please enter Designation !! <br />';
        //        iserror = true;
        //    }
        //    if (dept == '' || dept == null) {
        //        errormsg = "Please enter Department !! <br/>";
        //        iserror = true;
        //    }
        //    if (grade == '' || grade == null) {
        //        errormsg = errormsg + 'Please enter Grade !! <br />';
        //        iserror = true;
        //    }
        //    if (assetname == '' || assetname == null) {
        //        errormsg = errormsg + 'Please enter Asset Name !! <br />';
        //        iserror = true;
        //    }
        //    if (assetdesc == '' || assetdesc == null) {
        //        errormsg = errormsg + 'Please enter Asset Description !! <br />';
        //        iserror = true;
        //    }

        //    if (status == '' || status == '0') {
        //        errormsg = errormsg + 'Please Select Asset Approval !! <br />';
        //        iserror = true;
        //    }

        //    if (iserror) {
        //        messageBox("error", errormsg);
        //        $('#loader').hide();
        //        //  messageBox("info", "eror give");
        //        return false;
        //    }

        //    //var myData = {

        //    //    'loan_req_id': loan_req_id,
        //    //    'emp_id': emp_id,
        //    //    'req_emp_id': emp_id,
        //    //    'is_approve': status,
        //    //    'is_final_approval': status,
        //    //    'last_modified_by': localStorage.getItem('emp_id'),
        //    //};




        //    var mydata = {
        //        asset_req_id: asset_req_id,
        //        emp_id: emp_id,
        //        req_employee_id: emp_id,
        //        is_approve: status,
        //        is_final_approver: status,
        //        last_modified_by: login_emp,
        //        //login_role_id: localStorage.getItem('emp_role_id')
        //        approver_role_id: emp_role_idd
        //    }

        //    var headerss = {};
        //    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        //    headerss["salt"] = $("#hdnsalt").val();
        //    $.ajax({
        //        url: localStorage.getItem("ApiUrl") + "/apiPayroll/AssetRequestApproval",
        //        type: "POST",
        //        contentType: "application/json",
        //        dataType: "json",
        //        data: JSON.stringify(mydata),
        //        headers: headerss,
        //        success: function (data) {
        //            var statuscode = data.statusCode;
        //            var Msg = data.message;
        //            $('#loader').hide();
        //            _GUID_New();
        //            if (statuscode == "0") {
        //                alert(Msg);
        //                location.reload();
        //            }
        //            else if (statuscode == "1" || statuscode == '2') {
        //                messageBox("error", Msg);
        //            }
        //        },
        //        error: function (request, status, error) {
        //            $('#loader').hide();
        //            _GUID_New();
        //            var error = "";
        //            var errordata = JSON.parse(request.responseText);
        //            try {
        //                var i = 0;
        //                while (Object.keys(errordata).length > i) {
        //                    var j = 0;
        //                    while (errordata[Object.keys(errordata)[i]].length > j) {
        //                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
        //                        j = j + 1;
        //                    }
        //                    i = i + 1;
        //                }

        //            } catch (err) { }
        //            messageBox("error", error);

        //        }
        //    });
        //});

        //if ($("#txtToDate").val() != undefined) {
        //    var cur_dt = new Date();
        //    $("#txtToDate").val(GetDateFormatyyyyMMdd(cur_dt));

        //    cur_dt.setMonth(cur_dt.getMonth() - 1);


        //    $("#txtFromDate").val(GetDateFormatyyyyMMdd(new Date(cur_dt.toLocaleDateString())));



        //    GetReportData(login_emp, emp_role_idd);

        //}

        GetReportData(login_emp, emp_role_idd);

        $('#BtnSave').bind('click', function () {

            var BulAppId = [];
            // var AssetNumber = [];

            var table = $("#tblassetrequestdtl").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    //var no = $(this).val();
                    //BulAppId.push(no);

                    //var AssetNu = $(this).parents('tr').children('td').children('input[type="text"]').val();
                    //AssetNumber.push(AssetNu);

                    var requestss = {};
                    var currentRow = $(this).closest("tr");
                    var data = $('#tblassetrequestdtl').DataTable().row(currentRow).data();
                    requestss.emp_id = data["emp_id"];
                    requestss.asset_req_id = data["asset_req_id"];
                    requestss.asset_number = $(this).parents('tr').children('td').children('input[type="text"]').val();

                    // var no = $(this).val();
                    BulAppId.push(requestss);

                }
            });

            var action_type = $('#ddlaction_type').val();
            if (action_type == null || action_type == '' || action_type == 0) {
                alert("Please select action type ...!");
                return false;
            }

            if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
                alert('Please Select The Check Box...!!!');
                return false;
            }
            $('#loader').show();
            if (confirm("Do you want to process this?")) {


                //if (AssetNumber == null || AssetNumber == '' || AssetNumber.length <= 0) {
                //    alert('Please Enter Asset Number...!!!');
                //    $('#loader').hide();
                //    return false;
                //}
                //$('#loader').show();

                var mydata = {
                    asset_req_dtl: BulAppId,
                    // emp_id: login_emp,
                    is_approve: action_type,
                    is_final_approver: action_type,
                    last_modified_by: login_emp,
                    approver_role_id: emp_role_idd,
                    // asset_number: AssetNumber
                }

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "/apiPayroll/AssetRequestApproval",
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

                    }
                });


            }
            else {
                $('#loader').hide();
                return false;
            }

        });


        $('#btn_get_report').bind('click', function () {
            $('#loader').show();

            GetReportData(login_emp, emp_role_idd);

            $('#loader').hide();
        });


    }, 2000);// end timeout

});



function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}



function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblassetrequestdtl").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(this).prop('checked', true);
        }
        else {
            $(this).prop('checked', false);
        }
    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblassetrequestdtl").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function GetData(login_emp, login_emp_role_id) {
    $('#loader').show();
    var apiurll = "";
    //var HaveDisplay = ISDisplayMenu("Display Company List");
    //if (HaveDisplay == 1) {
    //    apiurll = localStorage.getItem("ApiUrl") + "/apiPayroll/Get_PendingAssetRequestDetailByCompIDD/" + login_emp + "/" + login_emp_role_id + "/" + company_idd;
    //}
    //else {
    apiurll = localStorage.getItem("ApiUrl") + "/apiPayroll/Get_PendingAssetRequestDetail/" + login_emp + "/" + login_emp_role_id;
    //}

    $.ajax({

        type: "GET",
        url: apiurll,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $("#tblassetrequestdtl").DataTable({

                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs": [
                    {
                        targets: 0,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
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
                        targets: [9],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {

                            return data == 0 ? "New Asset Request" : data == 1 ? "Replacement" : data == 2 ? "Submission" : "";
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {

                            return data == 0 ? "Pending" : data == 1 ? "Approve" : data == 2 ? "Reject" : data == 3 ? "In Process" : "";
                        }
                    }
                ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.asset_req_id + '" value="' + full.asset_req_id + '" />';
                        }
                    },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                    { "data": "asset_type", "name": "asset_type", "title": "Asset Type", "autoWidth": true },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset Name", "autoWidth": true },
                    { "data": "asset_description", "name": "asset_description", "title": "Description", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    //{ "data": "asset_number", "name":"asset_number","title":"Asset No.","autoWidth":true},
                    {
                        "title": "Asset Number", "autoWidth": true,

                        "render": function (data, type, full, meta) {
                            var dataa = (full.asset_number != "" && full.asset_number != null) ? full.asset_number : "";
                            var dis_able = dataa == "" ? "" : "readonly";
                            return '<input type="text" class=form-control  id="txtasset_number" placeholder="Asset Number" maxlength="10" style="width:120px" value="' + dataa + '" ' + dis_able + ' />';
                            //    if (full.approval_order == 1) {
                            //        if (full.asset_number != '' && full.asset_number != null) {
                            //            return '<label>' + full.asset_number + '</label>';
                            //        }
                            //        else {
                            //            return '<input type="text" class=form-control  id="txtasset_number" placeholder="Asset Number" style="width:120px" />';
                            //        }
                            //    }
                            //    else {
                            //        return '<label></label>';
                            //    }

                        }
                    },
                    { "data": "is_final_approver", "name": "is_final_approver", "title": "Final Status", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },

                    //{
                    //    "title": "Asset Number", "autoWidth": true,

                    //    "render": function (data, type, full, meta) {
                    //        return full.asset_number!"" && full.asset_number != null ? '<label></label>' : '<input type="text" class=form-control  id="txtasset_number" placeholder="Asset Number" style="width:120px" />';
                    //    //    if (full.approval_order == 1) {
                    //    //        if (full.asset_number != '' && full.asset_number != null) {
                    //    //            return '<label>' + full.asset_number + '</label>';
                    //    //        }
                    //    //        else {
                    //    //            return '<input type="text" class=form-control  id="txtasset_number" placeholder="Asset Number" style="width:120px" />';
                    //    //        }
                    //    //    }
                    //    //    else {
                    //    //        return '<label></label>';
                    //    //    }

                    //    }
                    //},
                    //{
                    //    "title": "View", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<a href="#" onclick="ViewDetails(' + full.asset_req_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                    //    }
                    //}
                ],
                'order': [[0, 'asc']],
                "select": 'multi',
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });

            $('#loader').hide();

        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}

function Get_PendingAssetRequestFromEmpMasterByCom(ControlId, CompanyId) {
    $('#loader').show();
    ControlId = "#" + ControlId;
    var login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var login_emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_PendingAssetRequestFromEmpMasterByCom/" + login_emp_id + "/" + login_emp_role_id + "/" + CompanyId,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem("Token") },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.emp_id + "/" + value.asset_req_id).html(value.emp_name_code));
            })

            ////get and set selected value
            //if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            //    $(ControlId).val(SelectedVal);
            //}
            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function BindEmployeeDetail(req_emp_id, asset_req_id) {

    $("#txtdepartment").val('');
    $("#txtdesignation").val('');
    $("#txtgrade").val('');
    $("#txtassetname").val('');
    $("#txtassetdescription").val('');
    $('#loader').show();
    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiPayroll/GetEmployeeDetailForAssetApproval/" + req_emp_id + "/" + asset_req_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer' + localStorage.getItem("Token") },
        success: function (response) {
            var res = response;
            $("#txtdepartment").val(res.dept_name);
            $("#txtdesignation").val(res.des_name);
            $("#txtgrade").val(res.grade_name);
            $("#txtassetname").val(res.asset_name);
            $("#txtassetdescription").val(res.asset_description);
            $("#hdnasset_req_id").val(res.asset_req_id);

            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function ViewDetails(asset_req_id) {

    $('#loader').show();

    $("#ddlassetreq").val('10022/1');

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $('#myModal').dialog({
        modal: 'true',
        title: 'Approver Detail'
    });


    var apiurll = "";

    apiurll = localStorage.getItem("ApiUrl") + "/apiPayroll/Get_PendingAssetRequestDetailByAssetReqId/" + asset_req_id;


    $.ajax({

        type: "GET",
        url: apiurll,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var asset_appr_history = response;

            if (response.statusCode != undefined) {
                $('#loader').hide();
                messageBox("error", response.message);
                return false;
            }

            $("#tblapprovalhistory").DataTable({
                "processing": true,// to show process bar
                "serverSide": false,//to process server side
                "filter": true,//to enable search box
                "orderMulti": false,//to disable multiple columns
                "bDestroy": true,
                "aaData": asset_appr_history,
                "scrollX": 200,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "order_name", "name": "order_name", "title": "Approver Order", "autoWidth": true },
                    { "data": "approver_emp_name_code", "name": "approver_emp_name_code", "title": "Approver's", "autoWidth": true },
                    { "data": "is_approve", "name": "is_approve", "title": "Status", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });


            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }

    });

}



function GetReportData(login_emp, login_emp_role_id) {
    $('#loader').show();
    var apiurll = "";

    //var from_date = $("#txtFromDate").val();

    //var to_date = $("#txtToDate").val();

    //if (from_date == null || from_date == '') {
    //    messageBox('info', 'Please select from date...!');
    //    return false;
    //}
    //if (to_date == null || to_date == '') {
    //    messageBox('info', 'Please select to date...!');
    //    return false;
    //}

    //var HaveDisplay = ISDisplayMenu("Display Company List");
    //if (HaveDisplay == 1) {
    //    apiurll = localStorage.getItem("ApiUrl") + "/apiPayroll/Get_PendingAssetRequestDetailByCompIDD/" + login_emp + "/" + login_emp_role_id + "/" + company_idd;
    //}
    //else {
    apiurll = localStorage.getItem("ApiUrl") + "/apiPayroll/Get_AssetApprovalReport/" + login_emp + "/" + login_emp_role_id + '/2000-01-01/2000-01-01';// + from_date + '/' + to_date;
    //}

    $.ajax({

        type: "GET",
        url: apiurll,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            $("#tblassetrequestdtlreport").DataTable({

                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs": [

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
                            return row.is_permanent == 1 ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return row.asset_issue_date < row.from_date ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return row.replace_dt < row.from_date ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [11],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return row.submission_date < row.from_date ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "requester_name_code", "name": "requester_name_code", "title": "Requester Name Code", "autoWidth": true },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                    { "data": "asset_number", "name": "asset_number", "title": "Asset Number", "autoWidth": true },
                    { "data": "asset_type", "name": "asset_type", "title": "Action Type", "autoWidth": true },
                    {
                        "title": "Is Permanent", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="chkpermanent" ' + (full.is_permanent == 1 ? 'checked' : '') + ' disabled=true/>'
                        }
                    },
                    { "data": "req_remarks", "name": "req_remarks", "title": "Requester Remarks", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "asset_issue_date", "name": "asset_issue_date", "title": "Issue Date", "autoWidth": true },
                    { "data": "replace_dt", "name": "replace_dt", "title": "Replace Date", "autoWidth": true },
                    { "data": "submission_date", "name": "submission_date", "title": "Submission Date", "autoWidth": true },
                    { "data": "mystatus", "name": "mystatus", "title": "My Status", "autoWidth": true },
                    { "data": "final_status", "name": "final_status", "title": "Final Status", "autoWidth": true },
                    {
                        "title": "View", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewDetails(' + full.asset_req_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });




            //$("#tblassetrequestdtlreport").DataTable({

            //    "processing": true, // for show progress bar
            //    "serverSide": false, // for process server side
            //    "bDestroy": true,
            //    "filter": true, // this is for disable filter (search box)
            //    "orderMulti": false, // for disable multiple column at once
            //    "scrollX": 200,
            //    "aaData": response,
            //    "columnDefs": [

            //        {
            //            targets: [4],
            //            render: function (data, type, row) {

            //                var date = new Date(data);
            //                return GetDateFormatddMMyyyy(date);
            //            }
            //        },
            //        {
            //            targets: [2],
            //            render: function (data, type, row) {

            //                return data == 0 ? "New Asset Request" : data == 1 ? "Replacement" : data == 2 ? "Submission" : "";
            //            }
            //        },

            //        {
            //            targets: [5],
            //            render: function (data, type, row) {

            //                return data == 0 ? "Pending" : data == 1 ? "Approve" : data == 2 ? "Reject" : data==3?"In Process":"";
            //            }
            //        }
            //    ],
            //    "columns": [
            //        { "data": null },
            //        { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
            //        { "data": "asset_type", "name": "asset_type", "title": "Asset Type", "autoWidth": true },
            //        { "data": "asset_name", "name": "asset_name", "title": "Asset Name", "autoWidth": true },
            //        { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
            //        { "data": "is_final_approver", "name": "is_final_approver", "title": "Final Status", "autoWidth": true },

            //        {
            //            "title": "View", "autoWidth": true,
            //            "render": function (data, type, full, meta) {

            //                return '<a href="#" onclick="ViewDetails(' + full.asset_req_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

            //            }
            //        }
            //    ],
            //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            //        return nRow;
            //    },
            //    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            //});
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}

