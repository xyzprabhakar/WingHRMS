
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
    debugger;

    $('#loader').show();
    $.ajax({
        type: "GET",
        // url: ,
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetFamilySectionReq',
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $('#loader').hide();
            var aData = res;

            if (aData.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }
            //// debugger;;
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
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {


                                return data == 1 ? "Male" : data == 2 ? "Female" : data == 3 ? "Other" : "";
                            }
                        },
                        //{
                        //    targets: [7],
                        //    "class": "text-center"
                        //}
                    ],

                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.em.emp_family_section_id + '" value="' + full.em.emp_family_section_id + '" />';
                        }
                    },
                    { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name As Per Aadhar Card", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name", "autoWidth": true },
                    { "data": "em.relation", "name": "relation", "title": "Relation", "autoWidth": true },
                    { "data": "em.gender", "name": "gender", "title": "Gender", "autoWidth": true },
                    { "data": "em.occupation", "name": "occupation", "title": "Occupation", "autoWidth": true },
                    { "data": "em.date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                    // { "data": "remark", "name": "passing_year", "title": "Remark", "remark": true },
                    // { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.emp_family_section_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);

        }
    });

}
function GetData_not_editing() {
    debugger;

    $('#loader').show();
    $.ajax({
        type: "GET",
        // url: ,
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetFamilySectionReq',
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $('#loader').hide();
            var aData = res;

            $("#leave_action_div").css("display", "none");

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
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {


                                return data == 1 ? "Male" : data == 2 ? "Female" : data == 3 ? "Other" : "";
                            }
                        },
                        //{
                        //    targets: [7],
                        //    "class": "text-center"
                        //}
                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name As Per Aadhar Card", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name", "autoWidth": true },
                    { "data": "em.relation", "name": "relation", "title": "Relation", "autoWidth": true },
                    { "data": "em.gender", "name": "gender", "title": "Gender", "autoWidth": true },
                    { "data": "em.occupation", "name": "occupation", "title": "Occupation", "autoWidth": true },
                    { "data": "em.date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                    // { "data": "remark", "name": "passing_year", "title": "Remark", "remark": true },
                    // { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.emp_family_section_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);

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
    //debugger;
    $('#loader').show();
    var QRIDs = [];
    var table = $("#tblReport").dataTable();
    $("input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            QRIDs.push(no.split(",")[0]);
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
            'request_ids': QRIDs,
            'is_deleted': action_type
        };

        var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/ApproveEmpFamilySecRequests/';
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

