$('#loader').show();
var empid;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData(empid);

        $('#loader').hide();

        $('[data-toggle="tooltip"]').tooltip();

    }, 2000);// end timeout

});

function GetData(empid) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveApplicationForApproval/" + empid,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $("#tblleaveapproval").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "aaData": res,
                "columnDefs": [
                    {
                        targets: [2],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [3],
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
                        targets: [5],
                        render: function (data, type, row) {

                            return data == 1 ? "Full Day" : data == 2 ? "Half Day" : "";
                        }
                    }
                ],
                "columns": [
                    { "data": null, "title": "S.No", "autowidth": true },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee", "autowidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autowidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autowidth": true },
                    { "data": "leave_type", "name": "leave_type", "title": "Leave Type", "autowidth": true },
                    { "data": "leave_applicable_for", "name": "leave_applicable_for", "title": "Leave Applicable for", "autowidth": true },
                    { "data": "leave_qty", "name": "leave_qty", "title": "Leave Quantity", "autowidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Requested Date", "autowidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Requester Remarks", "autowidth": true },
                    { "data": "status", "name": "status", "title": "Status", "autowidth": true },
                    { "data": "my_status", "name": "my_status", "title": "My Status", "autowidth": true },
                    {
                        "title": "Action", "autowidth": true,
                        "render": function (data, type, full, meta) {
                            if (full.approver_remarks != '' && full.approver_remarks != null) {
                                return '<label>' + full.my_status + '</label>';
                            }
                            else {
                                return '<select id="ddlManagerAction" style="width:120px"> <option value="0">Select Action</option><option value="1">Approve</option> <option value="2">Reject</option> </select>';
                            }
                        }
                    },
                    {
                        "title": "Remarks", "autowidth": true,
                        "render": function (data, type, full, meta) {
                            if (full.approver_remarks != '' && full.approver_remarks != null) {
                                return '<label>' + full.approver_remarks + '</label>';
                            }
                            else {
                                return '<input type="text"  id="txtManagerRemarks" placeholder="Manager Remarks" style="width:120px" />';
                            }

                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            if (full.approver_remarks != '' && full.approver_remarks != null) {
                                return '<label>' + full.approver_remarks + '</label>';
                            }
                            else {
                                return '<input type="hidden" id="hdnLeaveRequectId" value="' + full.leave_request_id + '" /><div class="col-md-12 text-center btn-group"><button type="button" class="btn btnSaveR"  id="btnSave">Save</button></div>';
                            }

                        }
                    }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },
                "length": [[10, 20, 50, -1], [10, 20, 50, "All"]]
            });

            $(document).on("click", ".btnSaveR", function () {

                var ddlManagerAction = $(this).parents('tr').children('td').children('select').val();
                var txtRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();
                var hdnLeaveRequectId = $(this).parents('tr').children('td').children('input[type="hidden"]').val();

                if (ddlManagerAction == 0) {
                    messageBox("error", "Please Select Action");
                    return false;
                }

                if (txtRemarks == '') {
                    messageBox("error", "Please enter Remarks");
                    return false;
                }

                BindApprovedorReject(ddlManagerAction, txtRemarks, hdnLeaveRequectId);

            });

            $('#loader').hide();
        },
        error: function (error) {
            alert(error);
            $('#loader').hide();
        }
    });
}

function BindApprovedorReject(ddlManagerAction, txtRemarks, hdnLeaveRequectId) {

    var mydata = {
        is_approved1: ddlManagerAction,
        approval1_remarks: txtRemarks,
        leave_request_id: hdnLeaveRequectId,
        a1_e_id: empid, // Login Employee ID
        is_final_approve: ddlManagerAction
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/Save_LeaveApplicationForApprover",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        headers: headerss,
        data: JSON.stringify(mydata),
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {

                GetData(empid);
                messageBox("success", Msg);

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