var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindStatusMaster('ddlstatus', login_company_id, 0);
        GetStatusFlow(login_company_id, 0);
        //    HaveDisplay = ISDisplayMenu("Display Company List");

        $("#btnupdate").hide();

        $("#ddlcompany").bind("change", function () {

            BindStatusMaster('ddlstatus', $(this).val(), 0);
            // GetStatusFlow($(this).val(), 0);

        });

        $("#btnreset").bind("click", function () {
            $('#loader').show();
            location.reload();
            $('#loader').hide();
        });


        $("#ddlstatus").bind("change", function () {

            if ($("#ddlcompany").val() == "0") {
                messageBox("error", "Please Firstly Select Company than Start Status");
                return false;
            }

            GetStatusFlow($("#ddlcompany").val(), $(this).val());

        });



        $("#btnsave").bind("click", function () {

            var companyid = $("#ddlcompany").val();
            var statusmaster_id = $("#ddlstatus").val();
            var nextstatusid = [];
            var description = $("#txtdesc").val();
            var is_error = false;
            var error_msg = "";


            var table = $('#tblstatusflowmaster').DataTable();
            //var require_asset = [];
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var req = {};
                var _ischecked = table.cell(rowIdx, 1).nodes().to$().find('input').is(":checked");
                if (_ischecked == true) {
                    nextstatusid.push(table.cell(rowIdx, 2).nodes().to$().html());
                }
            });


            if (companyid == "" || companyid == "0" || companyid == null) {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (statusmaster_id == "0" || statusmaster_id == "") {
                is_error = true;
                error_msg = error_msg + "Please Select Start Status</br>";
            }

            if (nextstatusid.length == 0) {
                is_error = true;
                error_msg = error_msg + "Select atleast one status from next status</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }


            if (is_error) {
                is_error = true;
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                company_id: companyid,
                start_status_id: statusmaster_id,
                next_status_id: nextstatusid,
                description: description,
                created_by: login_emp_id
            }

            if (confirm("Do you want to save this ?")) {

                $("#loader").show();

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();

                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "/apiePA/Save_StatusFlowMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {

                        _GUID_New();
                        $('#loader').hide();
                        var msg = response.message;
                        var statuscode = response.statusCode;
                        if (statuscode == "0") {
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            BindStatusMaster('ddlstatus', login_company_id, 0);
                            GetStatusFlow(login_company_id, 0);
                            $("#txtdesc").val('');

                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            messageBox("error", msg);
                            return false;
                        }
                    },
                    error: function (request, status, error) {
                        _GUID_New();
                        $('#loader').hide();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }

                });
            }
            else {
                $("#loader").hide();
                return false;
            }

        });


    }, 2000);// end timeout

});


function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblstatusflowmaster").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(this).prop('checked', true);
        }
        else {
            $(this).prop('checked', false);
        }
    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblstatusflowmaster").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}





function BindStatusMaster(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    if (CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiePA/BindStatusMasterForFlow/" + CompanyId + "/0",
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                if (res.statusCode != null && res.statusCode != undefined) {
                    messageBox("error", res.message);
                    return false;
                }

                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.epa_status_id).html(value.status_name));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }
                $('#loader').hide();
            },
            error: function (err) {
                $('#loader').hide();
                messageBox("error", err.responseText);

            }
        });

    }
}



function GetStatusFlow(companyid, status_masterid) {
    $("#loader").show();
    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiePA/Get_StatusFlowMaster/" + companyid + "/" + status_masterid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { "Authorization": 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblstatusflowmaster").DataTable({
                "processing": true,//for show Process bar
                "serverSide": false,//"for Process Server side"
                "orderMulti": false,//for disable multiple column at once
                "filter": true,//this is for disable filter(search box)
                "orderMulti": false,
                "bDestroy": true,//for remove previous data
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [
                    {
                        targets: [1],
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {

                            var modifieddate = new Date(data);
                            return data == "2000-01-01T00:00:00" ? "-" : new Date(row.created_dt) < new Date(row.modified_dt) ? "-" : GetDateFormatddMMyyyy(modifieddate);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            if (full.nextstatus_id != "0") {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked="checked" />';
                            }
                            else {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" />';
                            }

                        }
                    },
                    { "data": "epa_status_id", "name": "epa_status_id", "autoWidth": true, "visible": false },
                    { "data": "status_name", "name": "status_name", "title": "Status", "autoWidth": true },
                    //{ "data": "next_statusname", "name": "next_statusname", "title": "Next Status", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "render": function (data, type, full, meta) {
                            return '<a  onclick="DeleteFlow(' + full.company_id + ',' + full.flow_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a >';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]


            });


            if (status_masterid != "0") {
                for (var i = 0; i < res.length; i++) {
                    if (res[i].start_statusid == status_masterid) {
                        $("#txtdesc").val(res[i].description);
                        break;
                    }
                }
            }
            else if (res[0].description != "" && res[0].description != "-" && res[0].description != null) {
                $("#txtdesc").val(res[0].description);
            }
            else {
                $("#txtdesc").val('');
            }


            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function DeleteFlow(company_id, flowid) {
    $("#loader").show();
    if (confirm("Do you want to delete this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiePA/Delete_StatusFlowMaster/" + company_id + "/" + flowid + "/" + login_emp_id,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: headerss,
            success: function (response) {
                $("#loader").hide();
                _GUID_New();
                var msg = response.message;
                var statuscode = response.statusCode;
                if (statuscode == "0") {
                    BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                    BindStatusMaster('ddlstatus', login_company_id, 0);
                    GetStatusFlow(login_company_id, 0);
                    $("#txtdesc").val('');

                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("error", msg);
                    return false;
                }
            },
            error: function (err) {
                $("#loader").hide();
                _GUID_New();
                messageBox("error", err.responseText);
            }
        });

    }
    else {
        $("#loader").hide();
        return false;
    }

}