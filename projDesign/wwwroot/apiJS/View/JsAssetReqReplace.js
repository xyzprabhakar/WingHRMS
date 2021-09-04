
var company_id;
var login_emp_id;
//var HaveDisplay;
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
        //HaveDisplay = ISDisplayMenu("Display Company List"); //its a admin or not

        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        BindOnlyProbation_Confirmed_emp('ddlemployee', company_id, login_emp_id);

        GetData(login_emp_id, company_id);

        //if (HaveDisplay == 0)
        //{
        //    BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        //    //BindEmployees('ddlemployee', company_id, 0);
        //    EmpList = ISDisplayMenu("Is Company Admin"); // check its a user or not
        //    if (EmpList == 0) {
        //        BindEmployeeListUnderLoginEmp('ddlemployee', company_id, login_emp_id);
        //            GetData(login_emp_id);
        //    }
        //    else {
        //        BindEmployeeCodeFromEmpMasterByComp('ddlemployee', company_id, 0);
        //    }
        //}
        //else {
        //    BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        //}


        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#myAssetModal").hide();

        $("#ddlcompany").bind("change", function () {
            $('#loader').show();
            BindOnlyProbation_Confirmed_emp('ddlemployee', $(this).val(), 0);
            $('#loader').hide();
            if ($("#ddlemployee").val() == "0" || $("#ddlemployee").val() == undefined || $("#ddlemployee").val() == null) {
                if ($.fn.DataTable.isDataTable('#tblassetrqt')) {
                    $('#tblassetrqt').DataTable().clear().draw();
                }
            }
            else {
                //GetData(0, $(this).val());
            }

            // BindEmployeeCodeFromEmpMasterByComp('ddlemployee', $(this).val(), 0);

        });

        $("#ddlemployee").bind("change", function () {

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                messageBox('error', 'Please select company');
                return false;
            }

            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                messageBox('error', 'Please select Employee');
                return false;
            }
            $('#loader').show();
            GetData($(this).val(), $("#ddlcompany").val());
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




function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblassetrqt").find(".chkRow");
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
    var chkRows = $("#tblassetrqt").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
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


function GetData(emp_id, companyidd) {
    if ($.fn.DataTable.isDataTable('#tblassetrqt')) {
        $('#tblassetrqt').DataTable().clear().draw();
    }
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_AssetRequestByEmpID/" + emp_id + '/' + companyidd,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }


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
                    //{
                    //    targets: [12],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                    //{
                    //    targets: [12],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return data =="2000-01-01T00:00:00"?"-": GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                    //{
                    //    targets: [13],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return data == "2000-01-01T00:00:00" ? "-" : GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                    //{
                    //    targets: [13],
                    //    render: function (data, type, row) {
                    //        return row.from_date !="2000-01-01T00:00:00"?(data == "0" ? "Pending" : data == "1" ? "Approve" : data == "2" ? "Reject" :data=="3"?"In Process": ""):"-";
                    //    }
                    //},
                ],
                "data": res,
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee Name(Code)", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            var requestedd = full.from_date != "2000-01-01T00:00:00" ? "checked" : "";
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow"  />';
                        }
                    },
                    { "data": "asset_master_id", "name": "asset_master_id", "autoWidth": true, "visible": false },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Asset Description", "autoWidth": true },
                    {
                        "title": "From Date", "autoWidth": true, "render": function (data, type, full, meta) {
                            if (full.from_date != "2000-01-01T00:00:00" && full.is_final_approve != 2) {
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
                            return '<input type="checkbox" id="permanentchk" name="permanentchk" class="pchk" ' + (full.is_final_approve != 2 ? (full.is_permanent == 1 ? 'checked' : '') : '') + ' ' + (full.from_date != "2000-01-01T00:00:00" ? (full.is_final_approve == 1 || full.is_final_approve == 0 ? "readonly" : "") : "") + '/>';
                        }
                    },
                    {
                        "title": "To Date", "autoWidth": true, "render": function (data, type, full, meta) {
                            //if (full.to_date > "2000-01-01T00:00:00") {
                            //    var todate = new Date(full.to_date);
                            //    return '<input type="date" id="txttodate" class="form-control" name="txttodate" placeholder="To Date" value="' + GetDateFormatyyyyMMdd(todate)+'" /> ';
                            //}
                            //else {
                            return '<input type="date" id="txttodate" class="form-control" name="txttodate" placeholder="To Date" ' + (full.is_final_approve != 2 ? (full.is_permanent == 1 ? "style='display:none'" : "value=" + (full.to_date != "2000-01-01T00:00:00" ? GetDateFormatddMMyyyy(new Date(full.to_date)) : '') + "") : "") + ' ' + (full.asset_req_id != 0 && full.is_final_approve != 2 ? 'disabled' : '') + ' />';
                            //}

                        }
                    },
                    { "data": "asset_no", "name": "asset_no", "title": "Asset No.", "autoWidth": true },


                    {
                        "title": "Action Type", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            //var $select = $("<select id=ddlassettype name=ddlassettype class='form-control' ><option value=0>Please Select</option></select>", {});

                            var $select = $("<select id=ddlassettype name=ddlassettype class='form-control' ></select>", {});
                            var action_ = (full.next_action_).toString().split(',');
                            //var i = action_[0];
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
                    // { "data": "asset_type", "name": "asset_type", "title": "Asset Type", "autoWidth": true },
                    // { "data": "req_remarks", "name": "req_remarks", "title": "Remarks", "autoWidth": true },
                    // { "data": "issue_date", "name": "issue_date", "title": "Issue Date", "autoWidth": true },
                    //{ "data": "submission_date", "name": "submission_date", "title": "Submission Date", "autoWidth": true },
                    //{ "data": "req_remarks", "name":"req_remarks","title":"Requester Remarks","autoWidth":true},
                    // { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    //{ "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },
                    //{ "data": "is_final_approve", "name": "is_final_approve", "title": "Status", "autoWidth": true },
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
            $('#loader').hide();

            // $("#txtremarks").val(res[0].req_remarks);
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });


}


function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}


//function BindEmployeeListUnderLoginEmp(ControlId, SelectedVal) {


//    var listapi = localStorage.getItem("ApiUrl");

//    var key = CryptoJS.enc.Base64.parse("#base64Key#");
//    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

//    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
//    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
//    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        //url: listapi + "apiMasters/Get_EmployeeHeadList",
//        url: listapi + "apiMasters/Get_Employee_Under_LoginEmp/" + SelectedVal,
//        data: {},
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;

//            $(ControlId).append('<option selected="selected" value=' + SelectedVal + '>' + login_name_code + '</option>');


//            $.each(res, function (data, value) {
//                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code1));

//                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
//            });

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//                $(ControlId).trigger("select2:updated");
//                $(ControlId).select2();
//                // $(ControlId).prop('disabled', true);
//            }

//            //$('#loader').hide();

//            //$(ControlId).trigger("liszt:updated");
//            //$(ControlId).chosen();
//            $(ControlId).trigger("select2:updated");
//            $(ControlId).select2();
//            $('#loader').hide();

//        },
//        error: function (err) {
//            alert(err.responseText);
//            $('#loader').hide();
//        }
//    });
//}


