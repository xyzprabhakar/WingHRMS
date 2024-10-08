﻿

var empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData(empid);



        $('[data-toggle="tooltip"]').tooltip();

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Comp Off Application approval has been freezed for this month");
            $("#BtnSave").attr("disabled", true);

        }

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
function GetData(LoginEmployeeId) {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetCompOffLeaveApproval/' + LoginEmployeeId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
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
                        }
                    ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.leave_request_id + '" value="' + full.leave_request_id + '" />';
                        }
                    },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "compoff_against_date", "name": "compoff_against_date", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "data": "my_status", "name": "my_status", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },

                ],
                'order': [[1, 'asc']],
                "select": 'multi',
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

            });



            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();

        }
    });
}


function ApproveRequest(ddlManagerAction, txtManagerRemarks, hdnLeaveRequectId) {

    //alert(ddlManagerAction + ' ' + txtManagerRemarks + ' ' + hdnLeaveRequectId)
    //// debugger;;
    var LoginEmployeeId = empid;

    var errormsg = '';
    var iserror = false;

    //validation part
    if (ddlManagerAction == null || ddlManagerAction == '' || ddlManagerAction == 0) {
        errormsg = "Please Select Your Action For This Request...! ";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }
    $('#loader').show();
    var myData = {
        'is_approved1': ddlManagerAction,
        'approval1_remarks': txtManagerRemarks,
        'comp_off_request_id': hdnLeaveRequectId,
        'a1_e_id': LoginEmployeeId,
        'is_final_approve': ddlManagerAction
    };

    var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_CompOffLeaveForApproval';
    var Obj = JSON.stringify(myData);

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

                alert(Msg);
                location.reload();
                //GetData(empid);
                //messageBox("success", Msg);

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
            $('#loader').hide();
        }

    });
}


//---bulk approval leave request
$('#BtnSave').bind('click', function () {
    $('#loader').show();
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
        $('#loader').hide();
        return false;
    }


    if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

        if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
            alert('There some problem please try after later...!');
            $('#loader').hide();
            return false;
        }

        $('#loader').show();

        var myData = {
            'leave_request_id': BulAppId,
            'a1_e_id': empid,
            'approval1_remarks': $('#txtremarks').val(),
            'is_approved1': action_type,
            // 'is_final_approve': action_type
        };
        var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/Save_CompOffLeaveForApproval';
        var Obj = JSON.stringify(myData);

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

                    alert(Msg);
                    location.reload();
                    //GetData(empid);
                    //messageBox("success", Msg);

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
                $('#loader').hide();
            }

        });
    }
    else {

        $("#loader").hide();
        return false;
    }

});