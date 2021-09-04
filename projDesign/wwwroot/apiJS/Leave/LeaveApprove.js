$('#loader').show();
//global var for get empid
var empid;
var is_attendence_freezed;
$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData(empid);

        $('#loader').hide();
        $('#btnsave').prop('disabled', true);
        $('[data-toggle="tooltip"]').tooltip();

        $('#BtnSave').bind('click', function () {
            
            //debugger;
            var BulAppId = [];
            var table = $("#tblleaveapp").dataTable();
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

            if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
                alert('Please select at least one application to process...!');
                $('#loader').hide();
                return false;
            }

            $('#loader').show();

            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

                var myData = {
                    'leave_request_id': BulAppId,
                    'a1_e_id': empid,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    // 'is_final_approve': action_type
                };

                var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/ApproveLeaveApplication/';
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
                            GetData(empid);
                            messageBox("success", Msg);
                            $("#hdnid").val('');
                            $("#txtremarks").val('');
                            $("#ddlaction_type").val('0');
                        }
                        else {
                            messageBox("error", Msg);
                            GetData(empid);
                            $("#txtremarks").val('');
                            $("#ddlaction_type").val('0');
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

        $('#btnreset').bind('click', function () {
            var table = $('#tblleaveapp').DataTable();
            $("input[name='chkapp']:checkbox").prop('checked', false);
            //$('input', table.cells().nodes()).prop('checked', false);
            while (BulAppId.length > 0) {
                BulAppId.pop();
            }
            $('#btnsave').show();
            $("#hdnid").val('');
            $('#txtremarks').val('');
            $('#btnsave').prop('disabled', true);
        });

        is_attendence_freezed = FunIsApplicationFreezed();
        if (is_attendence_freezed == true) {
            alert("Leave application approval has been freezed for this month");
            $("#BtnSave").attr("disabled", true);

        }

        $('#ddl_year').bind("change", function () {
            GetData(empid);
        });

    }, 2000);// end timeout

});


//---bulk approval leave request


function selectAll() {
    //debugger;
    var chkAll = $('#selectAll');

    var allPages = $('#tblleaveapp').DataTable().cells().nodes();
    var currentPages = $('#tblleaveapp').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblleaveapp").find(".chkRow");
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
    var chkRows = $("#tblleaveapp").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

//--------bind data in jquery data table
function GetData(empid) {
    //// debugger;;
    //var apiurl = localStorage.getItem("ApiUrl") + 'apiLeave/GetLeaveApplicationByEmp/' + 0;
    $('#loader').show();
    var year = $("#ddl_year option:selected").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveApplicationForApproval/" + empid + "/" + year,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }


            var aData = res;

            if (aData.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }
            //// debugger;;
            //var aData = res["obj_leave_req"];
            //var lfor = res["leavefor"];
            //var leavefor = [];
            //leavefor.push(lfor);

            $("#tblleaveapp").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 800,
                'order': [[0, 'asc']],
                "aaData": aData,
                "columnDefs":
                    [
                        {
                            targets: 0,
                            orderable: false,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        },
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        return data == 1 ? "Full Day" : data == 2 ? "Half Day" : "";
                        //    }
                        //},
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
                            targets: [8],
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
                    { "data": "employee_code", "name": "employee_code" },
                    { "data": "employee_name", "name": "employee_name" },
                    { "data": "leave_type", "name": "leave_type" },
                    { "data": "leave_applicable_for", "name": "leave_applicable_for" },
                    { "data": "from_date", "name": "from_date" },
                    { "data": "to_date", "name": "to_date" },
                    { "data": "leave_qty", "name": "leave_qty" },
                    { "data": "requester_date", "name": "requester_date" },
                    { "data": "requester_remarks", "name": "requester_remarks" },
                    { "data": "my_status", "name": "my_status" },
                    { "data": "status", "name": "status" },

                ],

                "select": 'multi',
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                //"language": {
                //    "processing": "<i class='fa fa-refresh fa-spin'></i>",
                //}
            });
            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}
