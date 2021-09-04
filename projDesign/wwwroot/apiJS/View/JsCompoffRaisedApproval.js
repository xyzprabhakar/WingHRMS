
var empid;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        GetRequest(empid);

        $('#BtnSave').bind('click', function () {
            $('#loader').show();
            var BulAppId = [];
            var table = $("#tblCompOffRaisedApplication").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var requestss = {};
                    var currentRow = $(this).closest("tr");

                    var data = $('#tblCompOffRaisedApplication').DataTable().row(currentRow).data();
                    requestss.emp_id = data["employee_id"];
                    requestss.emp_comp_id = data["leave_request_id"];
                    requestss.comp_off_date = data["from_date"];
                    var no = $(this).val();
                    BulAppId.push(requestss);
                }
            });

            //cehck validation part


            var action_type = $('#ddlaction_type').val();
            if (action_type == null || action_type == '' || action_type == 0) {
                alert("Please select action type ...!");
                $('#loader').hide();
                return false;
            }



            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

                if (BulAppId == null || BulAppId == '') {
                    alert('There some problem please try after later...!');
                    $('#loader').hide();
                    return false;
                }
                else if (BulAppId.length <= 0) {
                    $('#loader').hide();
                    return false;
                }


                var myData = {
                    'compoff_raise_id': BulAppId,
                    'a1_e_id': empid,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    // 'is_final_approve': action_type
                };
                $('#loader').show();
                var apiurl = localStorage.getItem("ApiUrl") + '/apiMasters/RaisedCompoffApproveReject';
                var Obj = JSON.stringify(myData);

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


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
                            GetRequest(empid);
                            messageBox("success", Msg);
                        }
                        else {
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

function GetRequest(emp_id) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetCompOffRaisedRequest/" + emp_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }


            if (res.length > 0) {
                $("#empselectiondiv").css("display", "block");
            }
            else {
                $("#empselectiondiv").css("display", "none");
            }

            $("#tblCompOffRaisedApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [

                    {
                        targets: [0],
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
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
                            return GetTimeFromDate(date);
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetTimeFromDate(date);
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },


                ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.leave_request_id + '" value="' + full.leave_request_id + '" />';
                        }
                    },
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "employee_id", "name": "employee_id", "visible": false },
                    { "data": "leave_request_id", "name": "leave_request_id", "visible": false },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "CompOff Date", "autoWidth": true },
                    { "data": "system_in_time", "name": "system_in_time", "title": "In Time", "autoWidth": true },
                    { "data": "system_out_time", "name": "system_out_time", "title": "Out Time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "title": "Employee Remarks", "autoWidth": true },
                    { "data": "requester_date", "name": "requester_date", "title": "Created On", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(2)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();

            //$(document).on("click", ".btnSaveR", function () {

            //    var currentRow = $(this).closest("tr");

            //    var data = $('#tblCompOffRaisedApplication').DataTable().row(currentRow).data();

            //    var emp_id = data["employee_id"];
            //    var emp_comp_id = data["leave_request_id"];
            //    var comp_off_date = data["from_date"];
            //    var ddlManagerAction = $(this).parents('tr').children('td').children('select').val();
            //    var txtManagerRemarks = $(this).parents('tr').children('td').children('input[id="txtManagerRemarks"]').val();


            //    if (ddlManagerAction == "0") {
            //        _GUID_New();
            //        messageBox("error", "Please Select Approve or Reject from Action");
            //        return false;
            //    }
            //    if (txtManagerRemarks == "") {
            //        _GUID_New();
            //        messageBox("error", "Please enter Remarks");
            //        return false;
            //    }
            //    ApproveRequest(ddlManagerAction, txtManagerRemarks, emp_comp_id, emp_id, comp_off_date);

            //});


        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}



function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblCompOffRaisedApplication').DataTable().cells().nodes();
    var currentPages = $('#tblCompOffRaisedApplication').DataTable().rows({ page: 'current' }).nodes();
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblCompOffRaisedApplication").find(".chkRow");
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
    var chkRows = $("#tblCompOffRaisedApplication").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function ApproveRequest(ddlManagerAction, txtManagerRemarks, emp_comp_id, emp_id, comp_off_date) {

    var mydata = {
        emp_comp_id: emp_comp_id,
        comp_off_date: comp_off_date,
        emp_id: emp_id,
        a1_e_id: empid,
        approval1_remarks: txtManagerRemarks,
        is_approved1: ddlManagerAction,
        is_final_approve: ddlManagerAction
    }
    $('#loader').show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/RaisedCompoffApproveReject",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (data) {
            $('#loader').hide();

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
                //GetRequest(empid);
                // messageBox("success", Msg);
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
            $('#loader').hide();
        }
    });

}



