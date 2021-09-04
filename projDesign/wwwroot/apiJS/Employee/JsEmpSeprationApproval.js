
var empid;
var company_id;

$(document).ready(function () {
    setTimeout(function () {

        $('#loader').show();
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', empid, company_id);
        GetData(empid, company_id);
        Get_Emp_Sep_cncl_dtl(empid, company_id);
        // var HaveDisplay = ISDisplayMenu("Display Company List");




        $('[data-toggle="tooltip"]').tooltip();


        $("#ddlcompany").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tbladmin_empsepration_dtl')) {
                $('#tbladmin_empsepration_dtl').DataTable().clear().draw();
            }
            if ($.fn.DataTable.isDataTable('#tbl_admin_cncl_sep_dtl')) {
                $('#tbl_admin_cncl_sep_dtl').DataTable().clear().draw();
            }
            $("#div_action_save").css("display", "none");
            $("#div_cncl_action").css("display", "none");
            if ($(this).val() > 0) {
                GetData(empid, $(this).val());
                Get_Emp_Sep_cncl_dtl(empid, $(this).val());
            }

        });


        $('#loader').hide();

    }, 2000);// end timeout
});




function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tbladmin_empsepration_dtl').DataTable().cells().nodes();
    var currentPages = $('#tbladmin_empsepration_dtl').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbladmin_empsepration_dtl").find(".chkRow");
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
    var chkRows = $("#tbladmin_empsepration_dtl").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function GetData(login_emp, company_idd) {

    $("#div_action_save").css("display", "block");

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_PendingESeprationDetails/" + login_emp + "/" + company_idd,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        headers: { "Authorization": "Bearer " + localStorage.getItem("Token") },
        success: function (response) {


            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            if (res.length == 0) {
                $("#div_action_save").css("display", "none");
            }

            $("#tbladmin_empsepration_dtl").DataTable({
                "bDestroy": true,
                "processing": true,
                "serverSide": false,
                "orderMulti": false,
                "scrollX": 200,
                // "scrollY": 200,
                "filter": true,
                "data": res,
                "columnDefs": [
                    {
                        targets: 1,
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            var date = new Date(data);
                            return data == "0001-01-01T00:00:00" ? " " : GetDateFormatddMMyyyy(date);
                        }
                    },


                ],
                "columns": [
                    { "data": null, "title": "Sno.", "autoWidth": true },
                    { "data": "sepration_id", "name": "sepration_id", "title": "sepration_id", "autoWidth": true, "visible": false },
                    { "data": "emp_id", "name": "emp_id", "title": "emp_id", "autoWidth": true, "visible": false },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' />Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="chkRow" />';
                        }
                    },
                    { "data": "emp_name_code", "name": "emp_name_code", "autoWidth": true, "title": "Employee Name(Code)" },
                    { "data": "resignation_dt", "name": "resignation_dt", "autoWidth": true, "title": "Resignation Date" },
                    // { "data": "policy_relieving_dt", "name": "policy_relieving_dt", "autoWidth": true, "title": "Last Working Date as per Policy" },
                    {
                        "title": "Is Relieving Date Change", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="chk_relieving_change" name="chk_relieving_change" class="chk_relieving" />';
                        }
                    },
                    {
                        "title": "Final Last Working Date", "autoWidth": true, "render": function (data, type, full, meta) {
                            var datee_ = new Date(full.final_relieve_dt);
                            return '<input type="date" id=txtfinal_relieve_dt class=form-control value="' + GetDateFormatddMMyyyy(datee_) + '" disabled="disabled" />';
                        }
                    },
                    {
                        "title": "Remarks", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<textarea id="txtremarks" name="txtreamrks" placeholder="Remarks" onkeypress="return AlphaNumeric(event);" class="form-control"></textarea>'
                        }
                    },
                    { "data": "req_reason", "name": "req_reason", "autoWidth": true, "title": "Request Reason" },
                    //{ "data": "req_remarks", "name": "req_remarks", "autoWidth": true, "title": "Request Remarks" },
                    { "data": "mystatus", "name": "mystatus", "title": "My Status", "autoWidth": true },
                    { "data": "final_status", "name": "final_status", "title": "Final Status", "autoWidth": true },
                    {
                        "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="ViewDetails(' + full.sepration_id + ',' + full.emp_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';
                        }
                    },
                    { "data": "final_relieve_dt", "name": "final_relieve_dt", "visible": false, "autoWidth": true }
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });

            $(document).on("change", ".chk_relieving", function () {

                if ($(this).is(":checked")) {
                    $(this).parents('tr').children('td').children('input[id="txtfinal_relieve_dt"]').prop("disabled", false);
                }
                else {

                    var currentRow = $(this).closest("tr");

                    var data = $('#tbladmin_empsepration_dtl').DataTable().row(currentRow).data();

                    var bfr_chk_final_relieve = data['final_relieve_dt'];
                    $(this).parents('tr').children('td').children('input[id="txtfinal_relieve_dt"]').val(GetDateFormatddMMyyyy(new Date(bfr_chk_final_relieve)));

                    $(this).parents('tr').children('td').children('input[id="txtfinal_relieve_dt"]').prop("disabled", true);
                }
            });
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
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


function ViewDetails(sep_id, emp_id) {
    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select company");
        return false;
    }
    $('#loader').show();

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetailsBySep_Id/" + sep_id + "/" + emp_id + "/" + $("#ddlcompany").val(),
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response.data;
            var _app_historyy = response._approval_history;

            $("#txtdob").val(GetDateFormatddMMyyyy(new Date(res.dob_)));
            $("#txtdoj").val(GetDateFormatddMMyyyy(new Date(res.date_of_joining)));
            $("#txtdept").val(res.department_name);
            $("#txtdesignation").val(res.designation_name);
            $("#txtgrade").val(res.grade_name);
            $("#txtloc").val(res.location_name);
            var actual_notice = res.diff_notice_days != 0 ? parseInt(res.req_notice_days) - parseInt(res.diff_notice_days) : res.req_notice_days;
            $("#txtnoticeperiod_days").val(actual_notice);
            $("#txtresign_dt").val(GetDateFormatddMMyyyy(new Date(res.resignation_dt)));
            $("#txt_req_reliving_dt").val(GetDateFormatddMMyyyy(new Date(res.req_relieving_date)));
            $("#txt_req_notice_").val(res.req_notice_days);
            $("#txt_notice_diff_days").val(res.diff_notice_days);
            $("#txt_policy_reliving").val(GetDateFormatddMMyyyy(new Date(res.policy_relieving_dt)));
            $("#txtreason").val(res.req_reason);
            $("textarea#txtremarks").val(res.req_remarks);
            //$("#txtremarks").val(res.req_remarks);
            $("#txtfinal_working_dt").val(GetDateFormatddMMyyyy(new Date(res.final_relieve_dt)));

            $("#txt_is_cncl_").val(res.is_cancel);
            $("#txt_cncl_remarks_").val(res.cancel_remarks);
            $("#txt_cncl_dtl_").val(res.cancelation_dt);


            $("#app_histroy_").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "filter": true,//to enable search box
                "data": _app_historyy,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "_name", "name": "_name", "title": "Approver Name(Code)", "autoWidth": true },
                    { "data": "_remarks", "name": "_remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "_action", "name": "_action", "title": "Status", "autoWidth": true },
                    { "data": "_dt", "name": "_dt", "title": "Action On", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });

            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
            return false;
        }

    });
}


$("#btnsave").bind("click", function () {

    if ($("#ddlaction_type").val() == "0" || $("#ddlaction_type").val() == "" || $("#ddlaction_type").val() == null) {
        messageBox("error", "Please select action");
        return false;
    }

    var BulAppId = [];
    var table = $("#tbladmin_empsepration_dtl").DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        var _ischecked = table.cell(rowIdx, 3).nodes().to$().find('input').is(":checked");

        if (_ischecked == true) {
            var requestss = {};

            requestss.req_id = table.cell(rowIdx, 1).nodes().to$().html();
            requestss.emp_id = table.cell(rowIdx, 2).nodes().to$().html();

            if ($("#ddlaction_type").val() != "2") {
                var resign_dtt = table.cell(rowIdx, 5).nodes().to$().html().toString().split('/');
                requestss.resign_dt = resign_dtt[2] + "-" + resign_dtt[1] + "-" + resign_dtt[0];//table.cell(rowIdx, 5).nodes().to$().html();
                requestss.is_relieve_change = table.cell(rowIdx, 6).nodes().to$().find('input').is(":checked") == true ? 1 : 0;
                requestss.final_relieve_dt = table.cell(rowIdx, 7).nodes().to$().find('input').val();
                requestss.remarks = table.cell(rowIdx, 8).nodes().to$().find('textarea').val();
            }
            else {
                requestss.resign_dt = table.cell(rowIdx, 5).nodes().to$().html();
                requestss.is_relieve_change = 0;
                requestss.final_relieve_dt = table.cell(rowIdx, 13).nodes().to$().html();//find('input').val();
                requestss.remarks = table.cell(rowIdx, 8).nodes().to$().find('textarea').val();
            }


            BulAppId.push(requestss);
            //var no = $(this).val();
            //BulAppId.push(no);
        }
    });





    if (BulAppId.length == 0) {
        messageBox("error", "Please select request");
        return false;
    }
    else {
        for (var j = 0; j < BulAppId.length; j++) {
            if ($("#ddlaction_type").val() != 2) {
                if ((BulAppId[j].remarks == "" || BulAppId[j].remarks == null)) {
                    messageBox("error", "Please enter remarks of each request you want to process");
                    return false;
                    break;
                }
                else {
                    if (new Date(BulAppId[j].final_relieve_dt) < new Date(BulAppId[j].resign_dt)) {
                        messageBox("error", "All Final Last working Date of selected requests must be greater than resignation date");
                        return false;
                        break;
                    }
                }

            }

        }
    }


    $('#loader').show();

    if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

        var mydata = {
            company_id: $("#ddlcompany").val(),
            emp_req: BulAppId,
            is_approve: $("#ddlaction_type").val(),
            created_by: empid,
        };

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({

            url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_EmpSeprationApproval",
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
                else {
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


// Start E-Sepration Cancellation

function selectAll_cncl() {

    var chkAll = $('#selectAll_cncl');

    var allPages = $('#tbl_admin_cncl_sep_dtl').DataTable().cells().nodes();
    var currentPages = $('#tbl_admin_cncl_sep_dtl').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbl_admin_cncl_sep_dtl").find(".cncl_chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(allPages).find('.cncl_chkRow').prop('checked', false);
            $(currentPages).find('.cncl_chkRow').prop('checked', true);
        }
        else {
            $(allPages).find('.cncl_chkRow').prop('checked', false);
        }
    });

}

function selectRows_cncl() {

    var chkAll = $("#selectAll_cncl");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbl_admin_cncl_sep_dtl").find(".cncl_chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function Get_Emp_Sep_cncl_dtl(emp_idd, companyidd) {

    $("#div_cncl_action").css("display", "block");
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_Pending_EmpSepration_Cancel_Req/" + companyidd + "/" + emp_idd,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        headers: { "Authorization": "Bearer " + localStorage.getItem("Token") },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }

            if (res.length == 0) {
                $("#div_cncl_action").css("display", "none");
            }

            $("#tbl_admin_cncl_sep_dtl").DataTable({
                "processing": true,
                "serverSide": false,
                "orderMulti": false,
                "bDestroy": true,
                "filter": true,
                "data": res,
                "scrollX": 200,
                //"scrollY": 200,
                "columnDefs": [
                    {
                        targets: 1,
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll_cncl(this)' id='selectAll'></input>"
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

                ],
                "columns": [
                    { "data": null, "title": "Sno.", "autoWidth": true },
                    { "data": "sepration_id", "name": "sepration_id", "title": "sepration_id", "autoWidth": true, "visible": false },
                    { "data": "emp_id", "name": "emp_id", "title": "emp_id", "autoWidth": true, "visible": false },
                    {
                        "title": "<input type='checkbox' onchange='selectAll_cncl(this)' id='selectAll_cncl' />Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="cncl_chkRow" />';
                        }
                    },
                    { "data": "emp_name_code", "name": "emp_name_code", "autoWidth": true, "title": "Employee Name(Code)" },
                    { "data": "resignation_dt", "name": "resignation_dt", "autoWidth": true, "title": "Resignation Date" },
                    { "data": "policy_relieving_dt", "name": "policy_relieving_dt", "autoWidth": true, "title": "Last Working Date as per Policy" },
                    { "data": "final_relieve_dt", "name": "final_relieve_dt", "title": "Final Last Working Date", "autoWidth": true },
                    { "data": "cancel_remarks", "name": "cancel_remarks", "title": "Requeter Cancellation Remarks", "autoWidth": true },
                    { "data": "cancelation_dt", "name": "cancelation_dt", "title": "Cancellation Applied On", "autoWidth": true },
                    {
                        "title": "Remarks", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<textarea id="txtremarks" name="txtreamrks" placeholder="Remarks" onkeypress="return AlphaNumeric(event);" class="form-control"></textarea>'
                        }
                    },
                    { "data": "mystatus", "name": "mystatus", "title": "My Status", "autoWidth": true },
                    { "data": "final_status", "name": "final_status", "title": "Final Status", "autoWidth": true },
                    {
                        "title": "View Details", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="ViewDetails(' + full.sepration_id + ',' + full.emp_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';
                        }
                    },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });


        },
        error: function (err) {
            $('#loader').hide();
            messageBox(err.responseText);
            return false;
        }
    });
}

$("#btn_cncl_save").bind("click", function () {

    var BulAppId = [];
    var table = $("#tbl_admin_cncl_sep_dtl").DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        var _ischecked = table.cell(rowIdx, 3).nodes().to$().find('input').is(":checked");

        if (_ischecked == true) {
            var requestss = {};

            requestss.req_id = table.cell(rowIdx, 1).nodes().to$().html();
            requestss.emp_id = table.cell(rowIdx, 2).nodes().to$().html();
            requestss.resign_dt = table.cell(rowIdx, 5).nodes().to$().html();
            requestss.remarks = table.cell(rowIdx, 10).nodes().to$().find('textarea').val();

            BulAppId.push(requestss);
            //var no = $(this).val();
            //BulAppId.push(no);
        }
    });

    if ($("#ddl_cncl_action_type").val() == "0" || $("#ddl_cncl_action_type").val() == "" || $("#ddl_cncl_action_type").val() == null) {
        messageBox("error", "Please select action");
        return false;
    }

    if (BulAppId.length == 0) {
        messageBox("error", "Please select request");
        return false;
    }
    else {
        if ($("#ddl_cncl_action_type").val() != "2") {
            for (var j = 0; j < BulAppId.length; j++) {
                if (BulAppId[j].remarks == "" || BulAppId[j].remarks == null) {
                    messageBox("error", "Please enter remarks of each request you want to process");
                    return false;
                    break;
                }
            }
        }

    }


    $('#loader').show();

    if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

        var mydata = {
            company_id: $("#ddlcompany").val(),
            emp_req: BulAppId,
            is_approve: $("#ddl_cncl_action_type").val(),
            created_by: empid,
        };

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({

            url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_EmpSepration_Cncl_approval",
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
                else {
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

// Emp E-Sepration Cancellation


