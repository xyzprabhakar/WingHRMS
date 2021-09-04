$('#loader').show();
var empid;

$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

        $('#loader').hide();

        $('[data-toggle="tooltip"]').tooltip();


        $("#BtnSave").bind("click", function () {

            var BulAppId = [];
            var table = $("#tblCompOffApplication").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    BulAppId.push(no);
                }
            });

            //cehck validation part


            var action_type = $('#ddlaction_type').val();
            if (action_type == null || action_type == '' || action_type == 0) {
                alert("Please select action type ...!");
                return false;
            }



            if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
                messageBox("info", 'Please select request which you want to delete...!');
                return false;
            }

            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {


                $('#loader').show();
                var myData = {
                    'leave_request_id': BulAppId,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    'a1_e_id': empid
                    // 'is_final_approve': action_type
                };
                $('#loader').show


                var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_CompOffLeaveForApprovalByAdmin';
                var Obj = JSON.stringify(myData);

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();

                $('#loader').show();
                $.ajax({
                    type: "POST",
                    url: apiurl,
                    data: Obj,
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    headers: headerss,
                    success: function (res) {
                        //// debugger;;
                        data = res;
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {
                            GetData(empid);
                            messageBox("success", Msg);
                            $('#ddlaction_type').val(0);
                            $('#txtremarks').val('');
                            $("#hdnid").val('');
                            return false;
                        }
                        else {
                            messageBox("error", Msg);
                            return false;
                        }

                    },
                    error: function (request, status, error) {
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
                        $('#loader').hide();
                    }

                });
            }
            else {
                $('#loader').hide();
                return false;
            }
        });
    }, 2000);// end timeout

});

function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblCompOffApplication').DataTable().cells().nodes();
    var currentPages = $('#tblCompOffApplication').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblCompOffApplication").find(".chkRow");
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
    var chkRows = $("#tblCompOffApplication").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function GetData() {
    var company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetCompOffLeaveApprovalByAdmin/' + company_idd;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            if (res.length > 0) {
                $("#compoff_action_div").css("display", "block");
            }
            else {
                $("#compoff_action_div").css("display", "none");
            }


            $("#tblCompOffApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 1,
                            orderable: false,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        },

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
                        }
                    ],
                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' /> Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="chkRow" value=' + full.comp_off_request_id + ' id=' + full.comp_off_request_id + ' />';
                        }
                    },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee name", "autoWidth": true },
                    { "data": "compoff_against_date", "name": "compoff_against_date", "title": "Against Date", "autoWidth": true },
                    { "data": "compoff_date", "name": "compoff_date", "title": "CompOff Date", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Requester Remarks", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<select id="ddlAdminAction" class="form-control" style="width:120px"> <option value="0">Select Action</option><option value="3">Delete</option></select>';

                    //    }
                    //},
                    //{
                    //    "title": "Remarks", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<input type="text" class="form-control"  id="txtadminRemarks" placeholder="Remarks" style="width:120px" />';

                    //    }
                    //},
                    //{
                    //    "title": "Save", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<input type="hidden" id="hdnLeaveRequectId" value="' + full.comp_off_request_id + '" /><div class="col-md-12 text-center btn-group"><button type="button" class="btn btnSaveR btn-success" id="btnSave">Save</button></div>';

                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });


            //$(document).on("click", ".btnSaveR", function () {

            //    var ddlAdminAction = $(this).parents('tr').children('td').children('select').val();
            //    var txtAdminRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();
            //    var hdncompoffrequestid = $(this).parents('tr').children('td').children('input[type="hidden"]').val();

            //    if (ddlAdminAction == "0") {
            //        messageBox("error", "Please Select  Action");
            //        return false;
            //    }
            //    if (txtAdminRemarks == "") {
            //        messageBox("error", "Please enter Remarks");
            //        return false;
            //    }
            //    ApproveRequest(ddlAdminAction, txtAdminRemarks, hdncompoffrequestid);

            //});

            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}


function ApproveRequest(ddlAdminAction, txtAdminRemarks, hdncompoffrequestid) {

    var LoginEmployeeId = empid;

    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlAdminAction == null || ddlAdminAction == '' || ddlAdminAction == 0) {
        errormsg = "Please Select Your Action For This Request...! ";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }

    $('#loader').show();
    var myData = {

        comp_off_request_id: hdncompoffrequestid,
        deleted_by: LoginEmployeeId,
        deleted_remarks: txtAdminRemarks

        //'is_approved1': ddlAdminAction,
        //'approval1_remarks': txtAdminRemarks,
        //'comp_off_request_id': hdnLeaveRequectId,
        //'a1_e_id': LoginEmployeeId,
        //'is_final_approve': ddlManagerAction
    };
    var headers = {};
    headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headers["salt"] = $("#hdnsalt").val();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_CompOffLeaveForApprovalByAdmin';
    var Obj = JSON.stringify(myData);

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headers,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                //GetData();
                //messageBox("success", Msg);
                alert(Msg);
                location.reload();

            }
            else if (statuscode == "1" || statuscode == '2') {

                messageBox("error", Msg);
            }
        },
        error: function (request, status, error) {
            _GUID_New();
            $('#loader').hide();
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