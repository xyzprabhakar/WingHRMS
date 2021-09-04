
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

        $('#BtnSave').bind('click', function () {
            $('#loader').show();
            var BulAppId = [];
            var table = $("#tblOutdoorApplication").dataTable();
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
                $('#loader').hide();
                return false;
            }


            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

                if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
                    alert('There some problem please try after later...!');
                    $('#loader').hide();
                    return false;
                }

                var myData = {
                    'leave_request_id': BulAppId,
                    'a1_e_id': empid,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    // 'is_final_approve': action_type
                };
                $('#loader').show();
                var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_OutdoorLeaveForApproval';
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

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Outdoor application approval has been freezed for this month");
            $("#BtnSave").attr("disabled", true);

        }

    }, 2000);// end timeout

});



function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblOutdoorApplication').DataTable().cells().nodes();
    var currentPages = $('#tblOutdoorApplication').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblOutdoorApplication").find(".chkRow");
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
    var chkRows = $("#tblOutdoorApplication").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


//--------bind data in jquery data table
function GetData(LoginEmployeeId) {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetOutdoorLeaveForApproval/' + LoginEmployeeId;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }

            if (res.length > 0) {
                $("#actiondiv").css("display", "block");
            }
            else {
                $("#actiondiv").css("display", "none");
            }

            $("#tblOutdoorApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "order": [[0, "desc"]],
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 0,
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
                            targets: [4, 5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },

                    ],
                "columns": [
                    //{ "data": "leave_request_id", "name": "leave_request_id", "visible": false },
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.leave_request_id + '" value="' + full.leave_request_id + '" />';
                        }
                    },
                    { "data": "employee_code", "name": "employee_code", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "data": "my_status", "name": "my_status", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                ],

                "select": 'multi',
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

            });


        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
            return false;
        }
    });

}

//---bulk approval outdoor request

function ApproveRequest(ddlManagerAction, txtManagerRemarks, hdnLeaveRequectId) {

    var LoginEmployeeId = empid;

    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlManagerAction == null || ddlManagerAction == '') {
        errormsg = "Please Select Your Action For This Request...! ";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }


    var myData = {
        'is_approved1': ddlManagerAction,
        'approval1_remarks': txtManagerRemarks,
        'leave_request_id': hdnLeaveRequectId,
        'a1_e_id': LoginEmployeeId,
        'is_final_approve': ddlManagerAction
    };
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_OutdoorLeaveForApproval';
    var Obj = JSON.stringify(myData);
    console.log(Obj);

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
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