var company_idd;
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        GetData();

        $("#BtnSave").bind("click", function () {

            var BulAppId = [];
            var table = $("#tblleaveapproval").dataTable();
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
                alert('There some problem please try after later...!');
                $('#loader').hide();
                return false;
            }

            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {


                $('#loader').show();
                var myData = {
                    'leave_request_id': BulAppId,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    'a1_e_id': login_emp_id
                    // 'is_final_approve': action_type
                };
                $('#loader').show


                var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_LeaveApplicationForApprovelByAdmin';
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
                        $('#txtremarks').val("");
                        data = res;
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {
                            GetData();
                            messageBox("success", Msg);
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
    }, 2000); // end timeout
});


function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblleaveapproval').DataTable().cells().nodes();
    var currentPages = $('#tblleaveapproval').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblleaveapproval").find(".chkRow");
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
    var chkRows = $("#tblleaveapproval").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}



function GetData() {
    $('#loader').show();
    // var company_id = localStorage.getItem('company_id');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationForAdmin/' + company_idd;

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
                messageBox("error", res);
                $('#loader').hide();
                return false;
            }

            if (res.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }


            $("#tblleaveapproval").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "aaData": res,
                "scrollX": 200,
                "columnDefs": [
                    {
                        targets: 1,
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
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

                            return data == "1" ? "Full Day" : data == "2" ? "Half Day" : "";
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? " " : GetDateFormatddMMyyyy(date);

                        }
                    }

                ],
                "columns": [
                    { "data": null, "title": "S.No", "autowidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' /> Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="chkRow" value=' + full.leave_request_id + ' id=' + full.leave_request_id + ' />';
                        }
                    },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee", "autowidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autowidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autowidth": true },
                    {
                        "title": "Leave Type", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return full.leave_type;
                        }
                    },
                    // { "data": "leave_type", "name": "leave_type", "title": "Leave Type", "autowidth": true },
                    { "data": "leave_applicable_for", "name": "leave_applicable_for", "title": "Leave Applicable for", "autowidth": true },
                    { "data": "leave_qty", "name": "leave_qty", "title": "Leave Quantity", "autowidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Requested Date", "autowidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Requester Remarks", "autowidth": true },
                    //{
                    //    "title": "Action", "autowidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        if (full.approver_remarks != '' && full.approver_remarks != null) {
                    //            return '<label>' + full.my_status + '</label>';
                    //        }
                    //        else {
                    //            return '<select id="ddlManagerAction" class="form-control" style="width:120px"> <option value="0">Select Action</option><option value="3">Delete</option></select>';
                    //        }


                    //    }
                    //},
                    //{
                    //    "title": "Remarks", "autowidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        if (full.approver_remarks != '' && full.approver_remarks != null) {
                    //            return '<label>' + full.approver_remarks + '</label>';
                    //        }
                    //        else {
                    //            return '<input type="text"  id="txtManagerRemarks" class="form-control" placeholder="Remarks" style="width:120px" />';
                    //        }

                    //    }
                    //},
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        if (full.approver_remarks != '' && full.approver_remarks != null) {
                    //            return '<label>' + full.approver_remarks + '</label>';
                    //        }
                    //        else {
                    //            return '<input type="hidden" id="hdnLeaveRequectId" value="' + full.leave_request_id + '" /><div class="col-md-12 text-center btn-group"><button type="button" class="btn btnSaveR btn-success"  id="btnSave">Save</button></div>';
                    //        }

                    //    }
                    //}

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },
                "length": [[10, 20, 50, -1], [10, 20, 50, "All"]]
            });


            $('#loader').hide();

            //$(document).on("click", ".btnSaveR", function () {

            //    var ddlManagerAction = $(this).parents('tr').children('td').children('select').val();
            //    var txtRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();
            //    var hdnLeaveRequectId = $(this).parents('tr').children('td').children('input[type="hidden"]').val();

            //    if (ddlManagerAction == 0) {
            //        messageBox("error", "Please Select Action");
            //        return false;
            //    }

            //    if (txtRemarks == '') {
            //        messageBox("error", "Please enter Remarks");
            //        return false;
            //    }

            //    BindApprovedorReject(ddlManagerAction, txtRemarks, hdnLeaveRequectId);

            //});

        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function BindApprovedorReject(ddlManagerAction, txtRemarks, hdnLeaveRequectId) {
    $('#loader').show();
    var empid = login_emp_id;

    var mydata = {
        is_deleted: ddlManagerAction,
        deleted_remarks: txtRemarks,
        leave_request_id: hdnLeaveRequectId,
        deleted_by: empid
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_LeaveApplicationForApprovelByAdmin';

    var Obj = JSON.stringify(mydata);

    var headers = {};
    headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headers["salt"] = $("#hdnsalt").val();

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
            $('#txtremarks').val("");
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);

                location.reload();
                //GetData(localStorage.getItem("emp_id"));
                //messageBox("success", Msg);               

            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);

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