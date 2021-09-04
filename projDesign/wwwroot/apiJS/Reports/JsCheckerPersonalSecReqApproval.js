
var company_id;
var login_emp_id;
var login_role_id;

$(document).ready(function () {
    setTimeout(function () {
        

        //debugger;
        $('#loader').show();
        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        $('#loader').hide();
        if (login_role_id == 103) {
            GetData_not_editing();
        }
        else {
            GetData();
        }

    }, 2000);// end timeout

});

function GetData() {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePersonalDetailsReq',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            var aData = res;

            if (aData.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }

            if (res != null) {

                $("#tblReport").DataTable({
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
                                targets: [3],
                                render: function (data, type, row) {
                                    return data == 1 ? "A+" : data == 2 ? "O+" : data == 3 ? "B+" : data == 4 ? "AB+" : data == 5 ? "A-" : data == 6 ? "O-" : data == 7 ? "B-" : data == 8 ? "AB-" : "";
                                }
                            },
                        ],

                    "columns": [
                        {
                            "render": function (data, type, full, meta) {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.employeePersonalDetailData.emp_personal_section_id + '" value="' + full.employeePersonalDetailData.emp_personal_section_id + '" />';
                            }
                        },
                        { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "employeePersonalDetailData.blood_group", "name": "blood_group", "title": "Blood Group", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_contact_number", "name": "primary_contact_number", "title": "Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_email_id", "name": "primary_email_id", "title": "Email", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_address_line_one", "name": "permanent_address_line_one", "title": "Address", "autoWidth": true },
                        { "data": "permanentCity", "name": "permanent_city", "title": "City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_pin_code", "name": "permanent_pin_code", "title": " Pin Code", "autoWidth": true },
                        { "data": "permanentState", "name": "permanent_state", "title": "State", "autoWidth": true },
                        { "data": "permanentCountry", "name": "permanent_country", "title": "Country", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Person", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_relation", "name": "emergency_contact_relation", "title": "Emergency Contact Relation", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_mobile_number", "name": "emergency_contact_mobile_number", "title": "Emergency Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_line_one", "name": "emergency_contact_line_one", "title": "Emergency Contact Address", "autoWidth": true },
                        { "data": "emergencyCity", "name": "emergency_contact_city", "title": "Emergency Contact City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_pin_code", "name": "emergency_contact_pin_code", "title": "Emergency Contact Pin Code", "autoWidth": true },
                        { "data": "emergencyState", "name": "emergency_contact_state", "title": "Emergency Contact State", "autoWidth": true },
                        { "data": "emergencyCountry", "name": "emergency_contact_country", "title": "Emergency Contact Country", "autoWidth": true }

                    ],
                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                });
            }

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();

        }
    });

}
function GetData_not_editing() {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePersonalDetailsReq',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            var aData = res;
 
                $("#leave_action_div").css("display", "none");
             
            if (res != null) {

                $("#tblReport").DataTable({
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
                               
                            },
                            {
                                targets: [3],
                                render: function (data, type, row) {
                                    return data == 1 ? "A+" : data == 2 ? "O+" : data == 3 ? "B+" : data == 4 ? "AB+" : data == 5 ? "A-" : data == 6 ? "O-" : data == 7 ? "B-" : data == 8 ? "AB-" : "";
                                }
                            },
                        ],

                    "columns": [
                        { "data": null, "title": "SNo.", "autowidth": true },
                        { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "employeePersonalDetailData.blood_group", "name": "blood_group", "title": "Blood Group", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_contact_number", "name": "primary_contact_number", "title": "Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_email_id", "name": "primary_email_id", "title": "Email", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_address_line_one", "name": "permanent_address_line_one", "title": "Address", "autoWidth": true },
                        { "data": "permanentCity", "name": "permanent_city", "title": "City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_pin_code", "name": "permanent_pin_code", "title": " Pin Code", "autoWidth": true },
                        { "data": "permanentState", "name": "permanent_state", "title": "State", "autoWidth": true },
                        { "data": "permanentCountry", "name": "permanent_country", "title": "Country", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Person", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_relation", "name": "emergency_contact_relation", "title": "Emergency Contact Relation", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_mobile_number", "name": "emergency_contact_mobile_number", "title": "Emergency Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_line_one", "name": "emergency_contact_line_one", "title": "Emergency Contact Address", "autoWidth": true },
                        { "data": "emergencyCity", "name": "emergency_contact_city", "title": "Emergency Contact City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_pin_code", "name": "emergency_contact_pin_code", "title": "Emergency Contact Pin Code", "autoWidth": true },
                        { "data": "emergencyState", "name": "emergency_contact_state", "title": "Emergency Contact State", "autoWidth": true },
                        { "data": "emergencyCountry", "name": "emergency_contact_country", "title": "Emergency Contact Country", "autoWidth": true }

                    ],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },
                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                });
            }

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();

        }
    });

}
function selectAll() {
    //debugger;
    var chkAll = $('#selectAll');

    var allPages = $('#tblReport').DataTable().cells().nodes();
    var currentPages = $('#tblReport').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblReport").find(".chkRow");
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
    var chkRows = $("#tblReport").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

$('#BtnSave').bind('click', function () {

    $('#loader').show();
    var QRIDs = [];
    var table = $("#tblReport").dataTable();
    $("input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            QRIDs.push(no);
        }
    });

    //cehck validation part


    var action_type = $('#ddlaction_type').val();
    if (action_type == null || action_type == '' || action_type == 2) {
        alert("Please select action type ...!");
        $('#loader').hide();
        return false;
    }


    if (confirm("Total " + QRIDs.length + " application selected to Process. \nDo you want to process this?")) {

        if (QRIDs == null || QRIDs == '' || QRIDs.length <= 0) {
            alert('There some problem please try after later...!');
            $('#loader').hide();
            return false;
        }


        var myData = {
            'requests_ids': QRIDs,
            'is_deleted': action_type
        };

        var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/ApproveEmpPersonalSecRequests';
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

