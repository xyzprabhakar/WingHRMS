var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        // debugger;
        $('#loader').show();
        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#loader').hide();

        GetData();


    }, 2000);// end timeout

});




function selectAll() {
    //debugger;
    var chkAll = $('#selectAll');

    var allPages = $('#tblSalRequest').DataTable().cells().nodes();
    var currentPages = $('#tblSalRequest').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblSalRequest").find(".chkRow");
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
    var chkRows = $("#tblSalRequest").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function GetData() {
    //debugger;
    var apiurl = localStorage.getItem("ApiUrl") + "apiPayroll/GetSalaryReqReport"

    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            var aData = res;

            if (aData.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }

            $("#tblSalRequest").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                "aaData": aData,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Checker Salary Request Approval Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                        }
                    },
                ],
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
                        }
                    ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.emp_id + ',' + full.applicable_from_dt + '" value="' + full.emp_id + ',' + full.applicable_from_dt + '" />';
                        }
                    },
                    {
                        "title": "Employee Code", "autowidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onClick="GetPrevSalaryDetails(' + full.company_id + ',' + full.emp_id + ',\'' + full.applicable_from_dt + '\')" >' + full.emp_code + '</a>';
                        }
                    },
                    //{ "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "applicable_from_dt", "name": "applicable_from_dt", "title": "Applicable From", "autoWidth": true },
                    //{ "data": "gross_salary", "name": "GROSS SALARY", "title": "GROSS SALARY", "autoWidth": true },
                    //{ "data": "ctc", "name": "CTC", "title": "CTC", "autoWidth": true },
                    //{ "data": "basic", "name": "BASIC", "title": "BASIC", "autoWidth": true },
                    //{ "data": "hra", "name": "HRA", "title": "HRA", "autoWidth": true },
                    //{ "data": "conveyance", "name": "CONVEYANCE", "title": "CONVEYANCE", "autoWidth": true },
                    //{ "data": "special_allowance", "name": "SPECIAL ALLOWANCE", "title": "SPECIAL ALLOWANCE", "autoWidth": true },
                    //{ "data": "da", "name": "DA", "title": "DA", "autoWidth": true },
                    //{ "data": "net", "name": "NET", "title": "NET", "autoWidth": true },
                    //{ "data": "esic", "name": "ESIC", "title": "ESIC", "autoWidth": true },
                    //{ "data": "pf", "name": "PF", "title": "PF", "autoWidth": true },
                    //{ "data": "vpf", "name": "VPF", "title": "VPF", "autoWidth": true },
                    //{ "data": "total_deduction", "name": "TOTAL DEDUCTION", "title": "TOTAL DEDUCTION", "autoWidth": true },
                    //{ "data": "er_pf", "name": "ER PF", "title": "ER PF", "autoWidth": true },
                    { "data": "maker_remark", "name": "maker_remark", "title": "Remarks", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},

            });
            $('#loader').hide();
        },
        error: function (error) {
            //debugger;
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
    $('#loader').hide();
}


$('#BtnSave').bind('click', function () {
    //debugger;
    $('#loader').show();
    var eIds = [];
    var appDates = [];
    var table = $("#tblSalRequest").dataTable();
    $("input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            eIds.push(no.split(",")[0]);
            appDates.push(no.split(",")[1]);
        }
    });

    //cehck validation part


    var action_type = $('#ddlaction_type').val();
    var checker_remark = $('#txtremarks').val();
    if (action_type == null || action_type == '' || action_type == 2) {
        alert("Please select action type ...!");
        $('#loader').hide();
        return false;
    }


    if (confirm("Total " + eIds.length + " application selected to Process. \nDo you want to process this?")) {

        if (eIds == null || eIds == '' || eIds.length <= 0) {
            alert('There some problem please try after later...!');
            $('#loader').hide();
            return false;
        }


        var myData = {
            'emp_ids': eIds,
            'applicable_dates': appDates,
            'is_active': action_type,
            'checker_remark': checker_remark
        };

        var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/ApproveSalaryRequest/';
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
                //debugger;
                data = res;
                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                if (statuscode == "0") {
                    messageBox("success", Msg);
                    GetData();
                    $("#ddlaction_type").val('2');
                    $("#txtremarks").val(' ');
                }
                else {
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
    else {
        $('#loader').hide();
        return false;
    }
});


function GetPrevSalaryDetails(comp_id, empid, applicable_from_dt) {


    $('#loader').show();

    $("#divModal").show();
    var modal = document.getElementById("divModal");
    modal.style.display = "block";

    $('#divModal').dialog({
        modal: 'true',
        title: 'Employee Salary Detail'
    });

    var myData = {
        'emp_id': empid,
        'monthyear': applicable_from_dt,
        'company_id': comp_id
    };

    debugger;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/GetPreviousSalaryDetails",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(myData),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            debugger;
            var res = response;

            _GUID_New();
            $('#loader').hide();

            $("#tbl_Report").DataTable({

                "processing": true, // for show progress bar
                // "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                // "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columns": [
                    { "data": "component_id", "name": "component_id", "autoWidth": true, "visible": false },
                    { "data": "property_details", "name": "property_details", "autoWidth": true },
                    { "data": "applicable_value", "name": "applicable_value", "autoWidth": true },
                    { "data": "requested_value", "name": "requested_value", "autoWidth": true },
                ],
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (err) {
            debugger;
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });


}



