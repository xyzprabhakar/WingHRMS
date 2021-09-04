$("#loader").show();
var empid;
var company_id;
$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData(company_id);

        $("#loader").hide();
        $('[data-toggle="tooltip"]').tooltip();


        $("#BtnSave").bind("click", function () {

            var BulAppId = [];

            var table = $("#tblCompOffadditonApplication").DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {

                var _ischecked = table.cell(rowIdx, 1).nodes().to$().find('input').is(":checked");

                if (_ischecked == true) {
                    var requestss = {};

                    requestss.emp_comp_id = table.cell(rowIdx, 2).nodes().to$().html();
                    requestss.comp_off_date = table.cell(rowIdx, 5).nodes().to$().html();
                    requestss.emp_id = table.cell(rowIdx, 3).nodes().to$().html();

                    BulAppId.push(requestss);
                }
            });


            //cehck validation part


            var action_type = $('#ddlaction_type').val();
            if (action_type == null || action_type == '' || action_type == 0) {
                messageBox("error", "Please select action...!");
                return false;
            }



            if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
                messageBox("error", "Select those request which you want to delete");
                return false;
            }

            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {


                $('#loader').show();
                var myData = {
                    'compoff_raise_id': BulAppId,
                    'approval1_remarks': $('#txtremarks').val(),
                    'is_approved1': action_type,
                    'a1_e_id': empid
                    // 'is_final_approve': action_type
                };
                $('#loader').show


                var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/DeleteCompOffRaiseByAdmin';
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
                        var data = res;
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {
                            GetData(company_id);
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
    }, 2000); // end timeout

});



function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblCompOffadditonApplication').DataTable().cells().nodes();
    var currentPages = $('#tblCompOffadditonApplication').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblCompOffadditonApplication").find(".chkRow");
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
    var chkRows = $("#tblCompOffadditonApplication").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function GetData(company_idd) {
    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetCompOffRaisedRequestByAdmin/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            if (res.length > 0) {
                $("#compoffaddition_action_div").css("display", "block");
            }
            else {
                $("#compoffaddition_action_div").css("display", "none");
            }

            $("#tblCompOffadditonApplication").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [
                    {
                        targets: 1,
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
                        targets: [7, 8],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetTimeFromDate(date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' /> Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="chkRow"/>';
                        }
                    },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "emp_comp_id", "name": "emp_comp_id", "visible": false },
                    { "data": "emp_id", "name": "emp_id", "visible": false },
                    { "data": "comp_off_date", "name": "comp_off_date", "title": "CompOff Date", "autoWidth": true },
                    { "data": "system_in_time", "name": "system_in_time", "title": "In Time", "autoWidth": true },
                    { "data": "system_out_time", "name": "system_out_time", "title": "Out Time", "autoWidth": true },
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

                    //        return '<input type="text"  id="txtadminRemarks" class="form-control" placeholder="Remarks" style="width:120px" />';

                    //    }
                    //},
                    //{
                    //    "title": "Save", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {

                    //        return '<div class="col-md-12 text-center btn-group"><button type="button" class="btn btnSaveR btn-success"   id="btnSave">Save</button></div>';

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

            //    var currentRow = $(this).closest("tr");

            //    var data = $('#tblCompOffadditonApplication').DataTable().row(currentRow).data();

            //    var emp_idd = data["emp_id"];
            //    var emp_raise_id = data["emp_comp_id"];
            //    var compoff_date = data["comp_off_date"];

            //    var ddlAdminAction = $(this).parents('tr').children('td').children('select').val();
            //    var txtAdminRemarks = $(this).parents('tr').children('td').children('input[id="txtadminRemarks"]').val();

            //    if (ddlAdminAction == "0") {
            //        messageBox("error", "Please Select  Action");
            //        return false;
            //    }
            //    if (txtAdminRemarks == "") {
            //        messageBox("error", "Please enter Remarks");
            //        return false;
            //    }
            //    ActiononRequest(emp_raise_id, emp_idd, compoff_date, ddlAdminAction, txtAdminRemarks);

            //});



        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}





function ActiononRequest(emp_raise_id, emp_idd, compoff_date, ddlAdminAction, txtAdminRemarks) {

    var mydata = {
        emp_comp_id: emp_raise_id,
        comp_off_date: compoff_date,
        emp_id: emp_idd,
        deleted_remarks: txtAdminRemarks,
        deleted_by: empid,
        is_deleted: ddlAdminAction
    }

    $("#loader").show();

    var headers = {};
    headers["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headers["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/DeleteCompOffRaiseByAdmin",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headers,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                GetData(company_id);
                messageBox("success", Msg);
            }

            else if (statuscode != "0") {
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
        }

    });
}