$('#loader').show();
var company_id;
var login_emp_id;
var HaveDisplay;
var EmpList;
var is_managerr_dec;
$(document).ready(function () {
    setTimeout(function () {
        
        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }




        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        // HaveDisplay = ISDisplayMenu("Display Company List");


        var cur_dt = new Date();
        $("#txtToDate").val(GetDateFormatddMMyyyy(cur_dt));

        cur_dt.setMonth(cur_dt.getMonth() - 1);


        $("#txtFromDate").val(GetDateFormatddMMyyyy(new Date(cur_dt.toLocaleDateString())));


        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        // BindEmployeeCodeFromEmpMasterByComp('ddlemployee', company_id, 0);
        BindOnlyProbation_Confirmed_emp('ddlemployee', company_id, 0);
        Get_AssetReport(0, company_id);

        Get_AssetRequest(0, company_id);

        $("#asset_req_div").css('display', 'none');
        $("#myAssetModal").hide();

        //$("#tab2_btn").on('click', function (e) {
        //    $($.fn.dataTable.tables(true)).css('width', '100%');
        //    $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
        //});

        $('#tab2_btn').on('click', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust();
        });

        $('#tabb1_btn').on('click', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust();
        });

        //$("#tab2_btn").on('click', function (e) {
        //    $($.fn.dataTable.tables(true)).css('width', '100%');
        //    $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
        //});

        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {


            //if ($(this).val() == "0" || $(this).val() == "" || $(this).val() == null) {
            //    messageBox("error", "Please select company");
            //    return false;
            //}

            BindOnlyProbation_Confirmed_emp('ddlemployee', $(this).val(), 0);
            $('#loader').show();
            var empid = 0;
            if ($("#ddlemployee").val() != "" || $("#ddlemployee").val() != "0" || $("#ddlemployee").val() != null) {
                empid = $("#ddlemployee").val();
                Get_AssetRequest(empid, $(this).val());
            }
            Get_AssetReport(empid, $(this).val());

            $("#asset_req_div").css('display', 'none');
            $('#loader').hide();
        });


        $("#btnget_detail").bind("click", function () {
            if ($("#ddlcompany").val() == "0" || $("#ddlcompany").val() == "" || $("#ddlcompany").val() == null) {
                messageBox("error", "Please select company");
                return false;
            }

            if ($("#txtToDate").val() == "" || $("#txtToDate") == null) {
                messageBox("error", "Please select to date");
                return false;
            }

            if ($("#txtFromDate").val() == "" || $("#txtFromDate") == null) {
                messageBox("error", "Plase select from date");
                return false;
            }

            var empid = 0;
            if ($("#ddlemployee").val() != "" && $("#ddlemployee").val() != "0" && $("#ddlemployee").val() != null) {
                empid = $("#ddlemployee").val();
            }



            $("#loader").show();
            Get_AssetReport(empid, $("#ddlcompany").val());
            $("#loader").hide();
        });

        $("#ddlemployee").bind("change", function () {

            $("#asset_req_div").css('display', 'none');
            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }

            if ($(this).val() == "0" || $(this).val() == "" || $(this).val() == null) {
                messageBox("error", "Please select Employee");
                return false;
            }
            $('#loader').show();
            Get_AssetRequest($(this).val(), $("#ddlcompany").val());
            Get_AssetReport($(this).val(), $("#ddlcompany").val());
            $('#loader').hide();
        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {

            var errormsg = "";
            var iserror = false;
            var txtremarks = $("#txtremarks").val();

            var _company_id = $("#ddlcompany").val();
            var employee_id = $("#ddlemployee").val();

            //if (HaveDisplay == 0) {
            //    _company_id = company_id;
            //    employee_id = login_emp_id;
            //}
            //else {
            //    _company_id = $("#ddlcompany").val();
            //    employee_id = $("#ddlemployee").val();
            //}



            var table = $('#tblassetrqt').DataTable();
            var require_asset = [];
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var req = {};
                var _ischecked = table.cell(rowIdx, 2).nodes().to$().find('input').is(":checked");
                //var asset_no = table.cell(rowIdx, 9).nodes().to$().html();
                //req._ischecked = table.cell(rowIdx, 2).nodes().to$().find('input').is(":checked");
                if (_ischecked == true) {
                    req.assets_master_id = table.cell(rowIdx, 3).nodes().to$().html();
                    req.asset_name = table.cell(rowIdx, 4).nodes().to$().html()
                    req.asset_description = table.cell(rowIdx, 5).nodes().to$().html();
                    req.from_date = table.cell(rowIdx, 6).nodes().to$().find('input').val();
                    req.is_permanent = table.cell(rowIdx, 7).nodes().to$().find('input').is(":checked") == true ? 1 : 0;
                    if (req.is_permanent == 1) {
                        req.to_date = "2500-01-01";
                    }
                    else {
                        req.to_date = table.cell(rowIdx, 8).nodes().to$().find('input').val();
                    }

                    req.asset_type = table.cell(rowIdx, 10).nodes().to$().find(':selected').val();
                    req.reqt_type = table.cell(rowIdx, 10).nodes().to$().find(':selected').text();

                    require_asset.push(req);
                }

            });

            if (require_asset.length == 0) {
                messageBox("error", "Please select those assets through checkbox,whose request to be raised...");
                return false;
            }



            var invaliddtl = [];

            for (var i = 0; i < require_asset.length; i++) {
                if (require_asset[i].from_date != "" && require_asset[i].from_date != null && require_asset[i].to_date != "" && require_asset[i].to_date != null) {
                    if (require_asset[i].is_permanent == 0) {
                        if (new Date(require_asset[i].to_date) < new Date(require_asset[i].from_date)) {
                            invaliddtl.push(require_asset[i])
                        }
                    }

                }
                else {
                    if (require_asset[i].from_date == "" || require_asset[i].from_date == null) {
                        invaliddtl.push(require_asset[i]);
                    }
                    else {
                        if (require_asset[i].is_permanent == 0) {
                            invaliddtl.push(require_asset[i]);
                        }
                    }

                }

            }


            if (invaliddtl.length > 0) {

                $("#myAssetModal").show();
                var modal = document.getElementById("myAssetModal");
                modal.style.display = "block";

                $('#myAssetModal').dialog({
                    modal: 'true',
                    title: 'Invalid Details(To Date must be greater than from date)'
                });

                $.extend($.fn.dataTable.defaults, {
                    sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                });

                $("#tblassetdtl").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    "scrollX": 200,
                    "aaData": invaliddtl,
                    "columnDefs":
                        [],

                    "columns": [
                        { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                        { "data": "asset_description", "name": "asset_description", "title": "Description", "autoWidth": true },
                        { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                        { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                        { "data": "reqt_type", "name": "reqt_type", "title": "Request Type", "autoWidth": true },
                    ],
                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],

                });


                return false;
            }

            // alert(require_asset);


            if (_company_id == "" || _company_id == "0" || _company_id == null) {
                errormsg = errormsg + "Please Select Company </br>!!";
                iserror = true;
            }

            if (employee_id == "" || employee_id == "0" || employee_id == null) {
                errormsg = errormsg + "Please Select Employee </br>!!";
                iserror = true;
            }


            if (txtremarks == "" || txtremarks == null) {
                errormsg = errormsg + "Please Enter Remarks </br>!!";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();

            var mydata = {
                company_id: _company_id,
                emp_id: employee_id,
                assetreqlist: require_asset,
                asset_description: txtremarks,
                created_by: login_emp_id,

            }

            if (confirm("Do you want to process this?")) {

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({

                    url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_AssetRequest",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (data) {
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {
                            alert(Msg);
                            location.reload();
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

                    }

                });

            }
            else {

                return false;
            }



        });

    }, 2000);// end timeout
});



function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}

function Get_AssetReport(emp_idd, company_idd) {

    if ($("#txtFromDate").val() == "" || $("#txtFromDate").val() == null) {
        messageBox("error", "Please Select From Date");
        return false;
    }

    if ($("#txtToDate").val() == "" || $("#txtToDate").val() == null) {
        messageBox("error", "Please select To Date");
        return false;
    }

    if (new Date($("#txtToDate").val()) < new Date($("#txtFromDate").val())) {
        messageBox("error", "To Date must be greater than From Date");
        return false;
    }

    var for_all_emp = 1;

    if (emp_idd == 0) {
        for_all_emp = 0;
        emp_idd = $('#ddlemployee option').last().val();
    }

    var is_manager = 0;

    var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    if (is_managerr_dec == 'yes') {
        is_manager = 1
    }
    else if (HaveDisplay != 0) {
        is_manager = 1
    }

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_AssetRequestReport/0/" + $("#txtFromDate").val() + "/" + $("#txtToDate").val() + "/" + emp_idd + "/" + company_idd + "/" + for_all_emp + "/" + is_manager,
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }


            $("#tbl_emp_asset_dtl").empty();
            $("#tbl_emp_asset_dtl").DataTable({
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "data": res,
                "columnDefs": [

                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "req_emp_name", "name": "req_emp_name", "title": "Employee Name", "autoWidth": true, },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                    { "data": "asset_number", "name": "asset_number", "title": "Asset Number", "autoWidth": true },
                    { "data": "_asset_type", "name": "_asset_type", "title": "Action Type", "autoWidth": true },
                    {
                        "title": "Is Permanent", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="chkpermanent" ' + (full.is_permanent == 1 ? 'checked' : '') + ' disabled=true>';
                        }
                    },
                    { "data": "description", "name": "description", "title": "Requester Remarks", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "asset_issue_date", "name": "asset_issue_date", "title": "Issue Date", "autoWidth": true },
                    { "data": "replace_dt", "name": "replace_dt", "title": "Replace Date", "autoWidth": true },
                    { "data": "submission_date", "name": "submission_date", "title": "Submission Date", "autoWidth": true },
                    { "data": "_finalapprove", "name": "_finalapprove", "title": "Final Status", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    {
                        "title": "View Details", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewDetails(' + full.asset_req_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });

        },
        error: function (err) {
            _GUID_New();
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}



function Get_AssetRequest(emp_id, company_id) {
    var for_all_emp = 1;

    if (emp_id == 0) {
        for_all_emp = 0;
        emp_id = $('#ddlemployee option').last().val();
    }

    var is_manager = 0;
    var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    if (is_managerr_dec == 'yes') {
        is_manager = 1
    }
    else if (HaveDisplay != "0") {
        is_manager = 1
    }

    $("#asset_req_div").css('display', 'block');

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_AssetRequestByEmpID/" + emp_id + '/' + for_all_emp + '/' + is_manager + '/' + company_id,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tblassetrqt").empty();
            $('#tblassetrqt').DataTable({
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "columnDefs": [

                    {
                        targets: [2],
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },

                    {
                        targets: [9],
                        render: function (data, type, row) {

                            return data == "" ? "-" : data;
                        }
                    },

                ],
                "data": res,
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee Name(Code)", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            // return '<input type="checkbox" class="chkRow"  ' + (full.from_date != "2000-01-01T00:00:00" ? (full.is_final_approve == 1 || full.is_final_approve == 0 ? "readonly" : "") : "") + '/>'
                            var requestedd = full.from_date != "2000-01-01T00:00:00" ? "checked" : "";
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow"  />';
                        }
                    },
                    { "data": "asset_master_id", "name": "asset_master_id", "autoWidth": true, "visible": false },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Asset Description", "autoWidth": true },
                    {
                        "title": "From Date", "autoWidth": true, "render": function (data, type, full, meta) {
                            if (full.from_date != "2000-01-01T00:00:00") {
                                var fromdt = new Date(full.from_date);
                                return '<input type="date" id="txtfromdate" name="txttodate" class="form-control" placeholder="From Date" value="' + GetDateFormatddMMyyyy(fromdt) + '" disabled=true/>';
                            }
                            else {
                                return '<input type="date" id="txtfromdate" class="form-control" name="txtfromdate" placeholder="From Date" />';
                            }

                        }
                    },
                    {
                        "title": "<input type='checkbox' onchange='all_permanent(this)' id='all_permanent'></input>Select All Permanent", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" id="permanentchk" name="permanentchk" class="pchk" ' + (full.is_permanent == 1 ? 'checked' : '') + ' ' + (full.from_date != "2000-01-01T00:00:00" ? (full.is_final_approve == 1 || full.is_final_approve == 0 ? "readonly" : "") : "") + '/>';
                        }
                    },
                    {
                        "title": "To Date", "autoWidth": true, "render": function (data, type, full, meta) {

                            return '<input type="date" id="txttodate" class="form-control" name="txttodate" placeholder="To Date" ' + (full.is_permanent == 1 ? "style='display:none'" : "value=" + (full.to_date != "2000-01-01T00:00:00" ? GetDateFormatddMMyyyy(new Date(full.to_date)) : '') + "") + ' disabled=' + (full.asset_req_id != 0 ? 'true' : 'false') + ' />';

                        }
                    },
                    { "data": "asset_no", "name": "asset_no", "title": "Asset No.", "autoWidth": true },


                    {
                        "title": "Action Type", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            var $select = $("<select id=ddlassettype name=ddlassettype class='form-control' ></select>", {});
                            var action_ = (full.next_action_).toString().split(',');

                            for (var j = 0; j < action_.length; j++) {
                                var $option = $("<option></option>",
                                    {
                                        text: action_[j] == 0 ? "New" : action_[j] == 1 ? "Replace" : action_[j] == 2 ? "Submit" : "",
                                        value: action_[j]
                                    });


                                if ($option.length > 0) {
                                    $select.append($option);
                                }

                            }
                            return $select.prop("outerHTML");
                        }

                    },
                ],
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });






            $(document).on("change", ".pchk", function () {


                if ($(this).is(":checked")) {
                    $(this).parents('tr').children('td').children('input[id="txttodate"]').css('display', 'none');
                }
                else {
                    $(this).parents('tr').children('td').children('input[id="txttodate"]').css('display', 'block');
                }
            });

            $(document).on("change", ".chkRow", function () {

                if ($(this).is(":checked")) {
                    $(this).prop("checked", true);
                }
                else {
                    $(this).prop("checked", false);
                }
            });


        },
        error: function (err) {
            _GUID_New();
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}



function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblassetrqt").find(".chkRow");
    chkRows.each(function () {

        var frm_dt = $(this).parents('tr').children('td').children('input[id="txtfromdate"]').val();
        if (frm_dt == "") {
            if (chkAll.is(':checked')) {
                $(this).prop('checked', true);
            }
            else {
                $(this).prop('checked', false);
            }
        }

    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblassetrqt").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop('checked', false);
        }

    });
}



function all_permanent() {

    var allpmnt = $("#all_permanent");
    //Fetch all row checkboxes in the Table

    var chkpermanet = $("#tblassetrqt").find(".pchk");
    chkpermanet.each(function () {
        var frm_dt = $(this).parents('tr').children('td').children('input[id="txtfromdate"]').val();
        if (frm_dt == "") {


            if (allpmnt.is(':checked')) {
                $(this).prop('checked', true);
                $(this).parents('tr').children('td').children('input[id="txttodate"]').css('display', 'none');

            }
            else {
                $(this).prop('checked', false);
                $(this).parents('tr').children('td').children('input[id="txttodate"]').css('display', 'block');
            }

        }
    });
}











