
//$(document).ready(function () {
//    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryComponenetDetails/1';

//    var token = localStorage.getItem('Token');
//    if (token == null) {
//        window.location = '/Login';
//    }
//    debugger;
//    BindCompanyList('ddlCompany', 0);
//    GetProcessedMonthyear();

//    //nested data.
//    $("#jqGrid").jqGrid({

//        url: apiurl,
//        mtype: "GET",
//        datatype: "json",
//        page: 1,
//        colNames: ['Component ID', 'Item', 'Values'],
//        colModel: [
//            { name: 'component_id', key: true, width: 100, hidden: true },
//            { name: 'property_details', width: 400, editable: true },
//            { name: '0', width: 150, editable: true }
//            //{ label: 'Contact Name', name: 'applicable_value', width: 150 }

//        ],
//        loadonce: true,
//        width: '100%',
//        height: '100%',
//        rowNum: 50,
//        subGrid: true,

//        subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid


//        //isHasSubGrid: function (rowid) {
//        //    // if custommerid begin with B, do not use subgrid
//        //    var cell = $(this).jqGrid('getCell', rowid, 1);
//        //    //console.log(cell, rowid);
//        //    if (cell && cell.substring(0, 1) === "B") {
//        //        return false;
//        //    }
//        //    return true;
//        //},
//        //subGridOptions: {
//        //    // configure the icons from theme rolloer
//        //    plusicon: "ui-icon-triangle-1-e",
//        //    minusicon: "ui-icon-triangle-1-s",
//        //    openicon: "ui-icon-arrowreturn-1-e"
//        //},

//    });


//});
//$('#ddlCompany').bind("change", function () {

//    $("#ddlEmployee option").remove();

//    BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', $(this).val(), 0);
//});

//$('#ddlEmployee').bind("change", function () {

//    BindEmployeeDetailsByEmp($(this).val());

//});
//function BindCompanyList(ControlId, SelectedVal) {


//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_CompanyList",
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//            })
//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }
//        },
//        error: function (err) {
//            alert(err.responseText);
//        }
//    });
//}

//function GetProcessedMonthyear() {


//    debugger;
//    $.ajax({
//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + 'apipayroll/Get_ProcessedMonthyear',
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;


//            // console.log(JSON.stringify(response))
//            $("#txtmonthyear").val('');

//            $("#txtmonthyear").val(response);
//        },
//        error: function (err) {
//            alert(err.responseText);
//        }
//    });
//}
//function BindEmployeeDetailsByEmp(employee_id) {

//    debugger;
//    $("#lblName").val('');
//    $("#lblJoiningDate").val('');
//    $("#lblDOB").val('');
//    $("#lblGender").val('');
//    $("#lblLocation").val('');
//    $("#lblIsPFEligible").val('');
//    $("#lblIsESIEligible").val('');
//    //$("#lbljoiningDate").val('');


//    $.ajax({

//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_EmployeeData/' + employee_id,
//        dataType: "json",
//        contentType: 'application/json; charset=utf-8',
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (data) {

//            $("#lblName").html(data.employeeName);
//            $("#lblDOB").html(data.dob);
//            $("#lblJoiningDate").html(data.doj);
//            if (data.gender == '1') {
//                $("#lblGender").html("Male");
//            }
//            else if (data.gender == '2') {
//                $("#lblGender").html("Female");
//            }

//            $("#lblLocation").html(data.location_name);

//            if (data.isPFEligible == '1') {
//                $("#lblIsPFEligible").html("Yes");
//            }
//            else if (data.isPFEligible == '0') {
//                $("#lblIsPFEligible").html("No");
//            }

//            if (data.isESIEligible == '1') {
//                $("#lblIsESIEligible").html("Yes");
//            }
//            else if (data.isESIEligible == '0') {
//                $("#lblIsESIEligible").html("No");
//            }

//            //$("#lbljoiningDate").val(data[0].loan_type);


//        },
//        error: function (error) {
//            messageBox("error", "Server busy please try again later...!");
//        }
//    });

//}
//// the event handler on expanding parent row receives two parameters
//// the ID of the grid tow  and the primary key of the row
//function showChildGrid(parentRowID, parentRowKey) {
//    debugger;
//    var company_id = $('#ddlCompany').val();
//    var emp_id = $('#ddlEmployee').val();

//    var monthyear = $('#txtmonthyear').val();

//    if (company_id == '')
//    {
//        alert('Pleae select Company ID !!!!')
//        return false;
//    }
//    if (emp_id == '') {
//        alert('Pleae select Employee ID !!!!')
//        return false;
//    }
//    else if (monthyear == '') {
//        alert('Pleae enter Momnthyear !!!!')
//        return false;
//    }
//    else {
//        var childGridID = parentRowID + "_table";
//        var childGridPagerID = parentRowID + "_pager";

//        // send the parent row primary key to the server so that we know which grid to show
//        var childGridURL = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/' + $('#ddlEmployee').val() + '/' + $('#txtmonthyear').val() ;

//        // add a table and pager HTML elements to the parent grid row - we will render the child grid here
//        $('#' + parentRowID).append('<table class="root" id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

//        $.ajax({
//            url: childGridURL,
//            type: 'GET',
//            dataType: 'json',
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            success: function (res) {
//                if (res != '') {

//                    $("#" + childGridID).jqGrid({
//                        url: childGridURL,
//                        mtype: "GET",
//                        datatype: "json",

//                        page: 1,
//                        colNames: ['', ''],
//                        colModel: [

//                            { name: 'component_id', key: true, width: 100, hidden: true },
//                            { name: 'property_details', width: 400 }
//                        ],
//                        loadonce: true,
//                        width: '100%',
//                        height: '100%',
//                        subGrid: true, // set the subGrid property to true to show expand buttons for each row
//                        subGridRowExpanded: showThirdLevelChildGrid // javascript function that will take care of showing the child grid

//                    });
//                }
//                else {
//                    $("#" + childGridID).jqGrid({
//                        subGrid: false
//                    });

//                }
//            }

//        });

//        return true;
//    }
//}
//// the event handler on expanding parent row receives two parameters
//// the ID of the grid tow  and the primary key of the row
//function TheOnEditFunction(rowid) {

//    $("#btn_save_" + rowid).css("visibility", "visible");
//    $("#btn_cancel_" + rowid).css("visibility", "visible");
//    $("#btn_edit_" + rowid).hide();
//}
//function getCellValue(rowId, cellId) {
//    var cell = jQuery('#' + rowId + '_' + cellId);
//    var val = cell.val();
//    return val;
//}
//function getValue(rowId, cellId) {
//    var val = $("[rowId='" + rowId + "'][name='" + cellId + "']").val();
//    return val;
//}
////getGridData: function () {
////    var gridData = $('#jqGrid_1_table_25_table').jqGrid('getRowData');
////    for (var i = 0; i < gridData.length; i++) {
////        $('#jqGrid_1_table_25_table').jqGrid('saveRow', gridData[i]['id'], false, 'clientArray');
////    }
////    gridData = $('#jqGrid_1_table_25_table').jqGrid('getRowData');
////    var pattern = /(id=".+_SortOrder")/ig;
////    for (var j = 0; j < gridData.length; j++) {
////        var sortOrder = gridData[j]['SortOrder'];
////        if (sortOrder.toLowerCase().indexOf('<input') >= 0) {
////            var matched = sortOrder.match(pattern)[0];
////            var numberOfInput = matched.toUpperCase().replace("ID="", "").replace("_SORTORDER"", "");
////            sortOrder = $('#' + numberOfInput + '_SortOrder').val();
////        }
////        gridData[j] = {
////            Name: gridData[j]['component_value'],
////            SortOrder: sortOrder
////        };
////    }
////    return gridData;
////}, 

//function SaveSalaryInput(rowid) {

//    debugger;
//    var parentRowID = 'jqGrid_1_table_25_table'
//    var childGridID = parentRowID + "_table";
//    var childGridPagerID = parentRowID + "_pager";

//    // send the parent row primary key to the server so that we know which grid to show
//    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Save_SalaryInput';
//    var grid_ = $("#" + childGridID);
//    var ids = jQuery($("#" + parentRowID)).jqGrid('getRowData');
//    var ids1 = jQuery($("#" + parentRowID)).jqGrid('getDataIDs');

//    var emp_id = $('#ddlEmployee').val();
//    var component_id = rowid;
//    var values = 0.00
//    var previous_value = 0.00;
//    var modified_by = 10;
//    var monthyear = $('#txtmonthyear').val();

//    // var objRowData = $("#jqGrid_1_table_25_table").getRowData(cl);


//    var rowData = $("#jqGrid_1_table_25_table").jqGrid("getRowData");

//    for (var i = 0; i < ids.length; i++) {
//        var cl = ids[i].component_id;

//        if (cl == component_id) {
//            var rowval = rowData[i].component_value;
//            previous_value = rowData[i].componet_value_old;
//            if (rowval.indexOf("input") != -1) {
//                values = $("#" + component_id + "_component_value").val();
//            }

//        }
//    }
//    var myData = {

//        'emp_id': emp_id,
//        'component_id': component_id,
//        'values': values,
//        'previousvalues': previous_value,
//        'modified_by': modified_by,
//        'monthyear': monthyear

//    };

//    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
//   // $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
//    var Obj = JSON.stringify(myData);


//    $.ajax({
//        url: childGridURL_,
//        type: 'Post',
//        data: Obj,
//        dataType: 'json',
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (data) {

//            //var resp = JSON.parse(data);
//            //var statuscode = data.statusCode;
//            //var Msg = data.message;
//            //alert(statuscode)

//            //if (statuscode == "0") {



//            if (data != '')
//            {
//                window.location.href = '/payroll/salaryinput';


//                //$("#" + childGridID).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');



//                //$("#" + childGridID).jqGrid({
//                //    url: childGridURL_,
//                //    mtype: "Get",
//                //    datatype: "json",
//                //    colNames: ['Actions', '', '', '', '',''],
//                //    colModel: [
//                //        { name: 'act', index: 'act', width: 200, sortable: false },
//                //        { name: 'component_id', key: true, width: 75, hidden: true },
//                //        { name: 'property_details', width: 200 },
//                //        {
//                //          name: 'component_value', width: 100, edittype: 'textbox', editable: true
//                //        },
//                //        { name: 'is_data_entry_comp', width: 150, hidden: true },
//                //        { name: 'componet_value_old', width: 150, hidden: true }
//                //    ],

//                //    cellsubmit: 'clientArray',
//                //    loadonce: true,
//                //    width: '100%',
//                //    height: '100%',
//                //    //onSelectRow: editRow,
//                //    rowNum: 10,
//                //    rowList: [10, 20, 30],
//                //    pager: '#prowed2',
//                //    sortname: 'component_id',
//                //    viewrecords: true,
//                //    sortorder: "desc",
//                //    editurl: childGridURL_,
//                //    onSelectRow: function (id) {
//                //        if (id && id !== lastsel) {
//                //            $('#scrub').saveRow(lastsel);
//                //            lastsel = id;
//                //        }

//                //    },
//                //    gridComplete: function () {
//                //        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
//                //        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
//                //        for (var i = 0; i < ids.length; i++) {
//                //            var cl = ids[i].component_id;
//                //            var cl1 = ids[i].is_data_entry_comp;
//                //            if (cl1 == "1") {
//                //                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('#jqGrid_1_table_25_table').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
//                //                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('#jqGrid_1_table_25_table').saveRow('" + cl + "', false, 'clientArray');\"   />";
//                //                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('#jqGrid_1_table_25_table').restoreRow('" + cl + "', DisplayEditButton);\" />";

//                //                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

//                //                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


//                //            }
//                //        }
//                //    }

//                //});

//            }

//        }

//    });



//    //$("#btn_save_" + rowid).css("visibility", "hidden");
//    //$("#btn_cancel_" + rowid).css("visibility", "hidden");
//    //$("#btn_edit_" + rowid).show();

//}


//function DisplayEditButton(rowid) {

//    $("#btn_save_" + rowid).css("visibility", "hidden");
//    $("#btn_cancel_" + rowid).css("visibility", "hidden");
//    $("#btn_edit_" + rowid).show();
//}
//function showThirdLevelChildGrid(parentRowID, parentRowKey, rowid) {


//    debugger;
//    var childGridID = parentRowID + "_table";
//    var childGridPagerID = parentRowID + "_pager";

//    // send the parent row primary key to the server so that we know which grid to show
//    //  var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/1/201905';
//    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryInput/' + $('#ddlEmployee').val() + '/' + $('#txtmonthyear').val() + '/' + parentRowKey;


//    var grid_ = $("#" + childGridID);

//    //$('#' + parentRowID).remove('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
//    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
//    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
//    var lastrow;
//    var lastcell;

//    $.ajax({
//        url: childGridURL_,
//        type: 'GET',
//        dataType: 'json',
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (res) {

//            if (res != '') {
//                //$("#" + childGridID).jqGrid('clearGridData')
//                //    .jqGrid('setGridParam', { data: null, datatype: 'json' });


//                $("#" + childGridID).jqGrid({
//                    url: childGridURL_,
//                    mtype: "GET",
//                    datatype: "json",
//                    colNames: ['Actions', '', '', '', '', ''],
//                    colModel: [
//                        { name: 'act', index: 'act', width: 150, sortable: false },
//                        { name: 'component_id', key: true, width: 75, hidden: true },
//                        { name: 'property_details', width: 200 },
//                        {
//                            name: 'component_value', width: 100, edittype: 'textbox', editable: true
//                        },
//                        { name: 'is_data_entry_comp', width: 150, hidden: true },
//                        { name: 'componet_value_old', width: 150, hidden: true }
//                    ],

//                    cellsubmit: 'clientArray',
//                    loadonce: true,
//                    width: '100%',
//                    height: '100%',
//                    //onSelectRow: editRow,
//                    rowNum: 10,
//                    rowList: [10, 20, 30],
//                    pager: '#prowed2',
//                    sortname: 'component_id',
//                    viewrecords: true,
//                    sortorder: "desc",
//                    editurl: childGridURL_,

//                    gridComplete: function () {
//                        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
//                        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
//                        for (var i = 0; i < ids.length; i++) {
//                            var cl = ids[i].component_id;
//                            var cl1 = ids[i].is_data_entry_comp;



//                            if (cl1 == "1") {


//                                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('#jqGrid_1_table_25_table').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
//                                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('#jqGrid_1_table_25_table').saveRow('" + cl + "', false,SaveSalaryInput('" + cl + "')); jQuery('#jqGrid_1_table_25_table').restoreRow('" + cl + "' ,true);\"  />";
//                                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('#jqGrid_1_table_25_table').restoreRow( DisplayEditButton('" + cl + "'));\" />";

//                                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

//                                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


//                            }
//                        }
//                    }

//                });

//            }

//        }

//    });

//};

//function ReloadChildGrid(parentRowID, parentRowKey, rowid) {


//    debugger;
//    var childGridID = parentRowID + "_table";
//    var childGridPagerID = parentRowID + "_pager";

//    // send the parent row primary key to the server so that we know which grid to show
//    //  var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/1/201905';
//    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryInput/' + $('#ddlEmployee').val() + '/' + $('#txtmonthyear').val() + '/' + parentRowKey;

//    var grid_ = $("#" + childGridID);

//   // $('#' + parentRowID).remove('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
//    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
//    //$('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
//    var lastrow;
//    var lastcell;


//    $.ajax({
//        url: childGridURL_,
//        type: 'GET',
//        dataType: 'json',
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (res) {

//            if (res != '') {

//                //jQuery("#" + childGridID).jqGrid('clearGridData')
//                //    .jqGrid('setGridParam', { data: null, datatype: 'json' })
//                //    .trigger('reloadGrid');

//                $("#" + childGridID).jqGrid('clearGridData')
//                $("#" + childGridID).jqGrid('setGridParam', { data: null, page: 1 })
//                $("#" + childGridID).trigger('reloadGrid');

//                $("#" + childGridID).jqGrid({
//                    url: childGridURL_,
//                    mtype: "GET",
//                    datatype: "json",
//                    colNames: ['Actions', '', '', '', '', ''],
//                    colModel: [
//                        { name: 'act', index: 'act', width: 150, sortable: false },
//                        { name: 'component_id', key: true, width: 75, hidden: true },
//                        { name: 'property_details', width: 200 },
//                        {
//                            name: 'component_value', width: 100, edittype: 'textbox', editable: true
//                        },
//                        { name: 'is_data_entry_comp', width: 150, hidden: true },
//                        { name: 'componet_value_old', width: 150, hidden: true }
//                    ],

//                    cellsubmit: 'clientArray',
//                    loadonce: true,
//                    width: '100%',
//                    height: '100%',
//                    //onSelectRow: editRow,
//                    rowNum: 10,
//                    rowList: [10, 20, 30],
//                    pager: '#prowed2',
//                    sortname: 'component_id',
//                    viewrecords: true,
//                    sortorder: "desc",
//                    editurl: childGridURL_,

//                    gridComplete: function () {
//                        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
//                        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
//                        for (var i = 0; i < ids.length; i++) {
//                            var cl = ids[i].component_id;
//                            var cl1 = ids[i].is_data_entry_comp;



//                            if (cl1 == "1") {


//                                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('#jqGrid_1_table_25_table').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
//                                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('#jqGrid_1_table_25_table').saveRow('" + cl + "', false,SaveSalaryInput('" + cl + "')); jQuery('#jqGrid_1_table_25_table').restoreRow('" + cl + "' ,true);\"  />";
//                                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('#jqGrid_1_table_25_table').restoreRow( DisplayEditButton('" + cl + "'));\" />";

//                                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

//                                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


//                            }
//                        }
//                    }

//                });

//            }

//        }

//    });

//};

$('#loader').show();
var loginn_comp_id;
var login_empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryComponenetDetails/1';

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        $("#info_dialog").hide();

        loginn_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



        BindAllEmp_Company('ddlCompany', login_empid, loginn_comp_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', loginn_comp_id, 0);

        // GetProcessedMonthyear();
        ProcessMonthyear('ddlMonthyear', 0);
        // Get_ProcessedMonthyear()
        // fillGrid();
        //nested data.
        $("#jqGrid").jqGrid({

            url: apiurl,
            mtype: "GET",
            datatype: "json",
            page: 1,
            colNames: ['Component ID', 'Item', 'Values'],
            colModel: [
                { name: 'component_id', key: true, width: 100, hidden: true },
                { name: 'property_details', width: 400, editable: true },
                { name: '0', width: 150, editable: true }
                //{ label: 'Contact Name', name: 'applicable_value', width: 150 }

            ],
            loadonce: true,
            width: '100%',
            height: '100%',
            rowNum: 50,
            subGrid: true,

            subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid


        });

        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {

            $("#ddlEmployee option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', $(this).val(), 0);
            $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        });

        $('#ddlEmployee').bind("change", function () {

            BindEmployeeDetailsByEmp($(this).val());

            $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

        });
        $('#ddlMonthyear').bind("change", function () {
            $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        });

    }, 2000);// end timeout

});
function fillGrid() {

    //$("#jqGrid").jqGrid('clearGridData')
    //$("#jqGrid").jqGrid('setGridParam', { data: null, page: 1 })
    //$("#jqGrid").trigger('reloadGrid');

    $("#jqGrid").jqGrid({

        url: apiurl,
        mtype: "GET",
        datatype: "json",
        page: 1,
        colNames: ['Component ID', 'Item', 'Values'],
        colModel: [
            { name: 'component_id', key: true, width: 100, hidden: true },
            { name: 'property_details', width: 400, editable: true },
            { name: '0', width: 150, editable: true }
            //{ label: 'Contact Name', name: 'applicable_value', width: 150 }

        ],
        loadonce: true,
        width: '100%',
        height: '100%',
        rowNum: 50,
        subGrid: true,

        subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid


    });
}

//function BindCompanyList(ControlId, SelectedVal) {
//    $('#loader').show();

//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_CompanyList",
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//            })
//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (err) {
//            $('#loader').hide();
//            alert(err.responseText);
//        }
//    });
//}

function ProcessMonthyear(ControlId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_two_ProcessedMonthyear",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.monthyear).html(value.monthyear));
            })
            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function BindEmployeeDetailsByEmp(employee_id) {

    $('#loader').show();
    $("#lblName").val('');
    $("#lblJoiningDate").val('');
    $("#lblDOB").val('');
    $("#lblGender").val('');
    $("#lblLocation").val('');
    $("#lblIsPFEligible").val('');
    $("#lblIsESIEligible").val('');
    //$("#lbljoiningDate").val('');


    $.ajax({

        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_EmployeeData/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            $("#lblName").html(data.employeeName);
            $("#lblDOB").html(data.dob);
            $("#lblJoiningDate").html(data.doj);
            if (data.gender == '1') {
                $("#lblGender").html("Male");
            }
            else if (data.gender == '2') {
                $("#lblGender").html("Female");
            }

            $("#lblLocation").html(data.location_name);

            if (data.isPFEligible == '1') {
                $("#lblIsPFEligible").html("Yes");
            }
            else if (data.isPFEligible == '0') {
                $("#lblIsPFEligible").html("No");
            }

            if (data.isESIEligible == '1') {
                $("#lblIsESIEligible").html("Yes");
            }
            else if (data.isESIEligible == '0') {
                $("#lblIsESIEligible").html("No");
            }

            //$("#lbljoiningDate").val(data[0].loan_type);

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", "Server busy please try again later...!");
        }
    });

}
// the event handler on expanding parent row receives two parameters
// the ID of the grid tow  and the primary key of the row
function showChildGrid(parentRowID, parentRowKey) {

    var company_id = $('#ddlCompany').val();
    var emp_id = $('#ddlEmployee').val();

    var monthyear = $('#ddlMonthyear').val();

    if (company_id == '' || company_id == "0" || company_id == null) {
        alert('Pleae select Company ID !!!!')
        return false;
    }
    if (emp_id == '' || emp_id == "0" || emp_id == null) {
        alert('Pleae select Employee ID !!!!')
        return false;
    }
    else if (monthyear == '') {
        alert('Pleae enter Momnthyear !!!!')
        return false;
    }
    else {
        var childGridID = parentRowID + "_table";
        var childGridPagerID = parentRowID + "_pager";

        //#jqGrid_1_table_3_table
        //jqGrid_3_table_3_table
        var tab_name = "#" + childGridID;

        // send the parent row primary key to the server so that we know which grid to show
        var childGridURL = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/' + emp_id + '/' + monthyear + "/" + company_id;

        // add a table and pager HTML elements to the parent grid row - we will render the child grid here
        $('#' + parentRowID).append('<table class="root" id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

        $.ajax({
            url: childGridURL,
            type: 'GET',
            dataType: 'json',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {

                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    $("#loader").hide();
                    return false;
                }

                if (res != '') {

                    $("#" + childGridID).jqGrid({
                        url: childGridURL,
                        mtype: "GET",
                        datatype: "json",

                        page: 1,
                        colNames: ['', '', '', '', '', '', ''],
                        colModel: [
                            { name: 'act', index: 'act', width: 150, sortable: false },
                            { name: 'component_id', key: true, width: 75, hidden: true },
                            { name: 'property_details', width: 200 },
                            {
                                name: 'component_value', width: 100, edittype: 'textbox', editable: true
                            },
                            { name: 'is_data_entry_comp', width: 150, hidden: true },
                            { name: 'componet_value_old', width: 150, hidden: true },
                            { name: 'parentid', width: 150, hidden: true }
                        ],
                        loadonce: true,
                        width: '100%',
                        height: '100%',
                        subGrid: true, // set the subGrid property to true to show expand buttons for each row
                        subGridRowExpanded: showThirdLevelChildGrid,// javascript function that will take care of showing the child grid
                        gridComplete: function () {
                            var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
                            var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
                            for (var i = 0; i < ids.length; i++) {
                                var cl = ids[i].component_id;
                                var cl1 = ids[i].is_data_entry_comp;
                                var p_id = ids[i].parentid;



                                if (cl1 == "1") {


                                    edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('" + tab_name + "').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
                                    save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('" + tab_name + "').saveRow('" + cl + "', false,SaveSalaryInput('" + cl + "," + p_id + "," + parentRowID + "'));jQuery('" + tab_name + "').restoreRow('" + cl + "' ,true);\"  />";
                                    cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('" + tab_name + "').restoreRow( DisplayEditButton('" + cl + "'));\" />";

                                    // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

                                    jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


                                }
                            }
                        }

                    });
                }
                else {
                    $("#" + childGridID).jqGrid({
                        subGrid: false
                    });

                }
            }

        });

        return true;
    }
}
// the event handler on expanding parent row receives two parameters
// the ID of the grid tow  and the primary key of the row
function TheOnEditFunction(rowid) {

    $("#btn_save_" + rowid).css("visibility", "visible");
    $("#btn_cancel_" + rowid).css("visibility", "visible");
    $("#btn_edit_" + rowid).hide();
}
function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
function getValue(rowId, cellId) {
    var val = $("[rowId='" + rowId + "'][name='" + cellId + "']").val();
    return val;
}
//getGridData: function () {
//    var gridData = $('#jqGrid_1_table_25_table').jqGrid('getRowData');
//    for (var i = 0; i < gridData.length; i++) {
//        $('#jqGrid_1_table_25_table').jqGrid('saveRow', gridData[i]['id'], false, 'clientArray');
//    }
//    gridData = $('#jqGrid_1_table_25_table').jqGrid('getRowData');
//    var pattern = /(id=".+_SortOrder")/ig;
//    for (var j = 0; j < gridData.length; j++) {
//        var sortOrder = gridData[j]['SortOrder'];
//        if (sortOrder.toLowerCase().indexOf('<input') >= 0) {
//            var matched = sortOrder.match(pattern)[0];
//            var numberOfInput = matched.toUpperCase().replace("ID="", "").replace("_SORTORDER"", "");
//            sortOrder = $('#' + numberOfInput + '_SortOrder').val();
//        }
//        gridData[j] = {
//            Name: gridData[j]['component_value'],
//            SortOrder: sortOrder
//        };
//    }
//    return gridData;
//}, 

function SaveSalaryInput(rowid) {
    if ($("#ddlCompany").val() == "" || $("#ddlCompany").val() == "0" || $("#ddlCompany").val() == null) {
        messageBox("error", "Please select company");
        return false;
    }

    if ($('#ddlEmployee').val() == "" || $('#ddlEmployee').val() == "0" || $('#ddlEmployee').val() == null) {
        messageBox("error", "Please select Employee");
        return false;
    }

    if ($('#ddlMonthyear').val() == "" || $('#ddlMonthyear').val() == null || $('#ddlMonthyear').val() == "0") {
        messageBox("error", "Please select Month Year");
        return false;
    }

    var fields = rowid.split(',');

    var c_id = fields[0];
    var p_id = fields[1];
    var grid_name = fields[2];
    //alert(rowid);
    //alert(c_id);
    //alert(p_id);
    //alert(grid_name);
    //return;
    var parentRowID = 'jqGrid_1_table_25_table';

    var newTable = 'jqGrid_1_table_' + p_id + '_table';

    var newTable = grid_name + '_table';
    //var new_new_table = grid_name + '_table';
    //alert(grid_name)
    //alert(newTable)
    //alert(new_new_table)
    //jqGrid_1_table_25_table
    //jqGrid_1_table_25_table_25_table

    //jqGrid_1_table_25
    //jqGrid_1_table_25_table
    //jqGrid_1_table_25_25_table

    var childGridID = newTable + "_table";
    var childGridPagerID = newTable + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Save_SalaryInput';
    var grid_ = $("#" + childGridID);
    var ids = jQuery($("#" + newTable)).jqGrid('getRowData');
    var ids1 = jQuery($("#" + newTable)).jqGrid('getDataIDs');

    var emp_id = $('#ddlEmployee').val();
    var component_id = c_id;
    var values = 0.00
    var previous_value = 0.00;
    var modified_by = 10;
    var monthyear = $('#ddlMonthyear').val();

    // var objRowData = $("#jqGrid_1_table_25_table").getRowData(cl);


    var rowData = $("#" + newTable).jqGrid("getRowData");

    for (var i = 0; i < ids.length; i++) {
        var cl = ids[i].component_id;

        if (cl == component_id) {
            var rowval = rowData[i].component_value;
            previous_value = rowData[i].componet_value_old;
            if (rowval.indexOf("input") != -1) {
                values = $("#" + component_id + "_component_value").val();
            }

        }
    }
    var myData = {

        'emp_id': emp_id,
        'component_id': component_id,
        'values': values,
        'previousvalues': previous_value,
        'modified_by': modified_by,
        'monthyear': monthyear,
        'company_id': $("#ddlCompany").val()

    };

    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    // $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    var Obj = JSON.stringify(myData);
    //  $("#info_dialog").hide();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: childGridURL_,
        type: 'POST',
        data: Obj,
        dataType: 'json',
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            //  $("#info_dialog").hide();
            //var resp = JSON.parse(data);
            //var statuscode = data.statusCode;
            //var Msg = data.message;
            //alert(statuscode)

            //if (statuscode == "0") {

            _GUID_New();

            if (data != '') {
                // messageBox("Successfull", "Record updated successfully !!!");
                $("#info_dialog").hide();
                // messageBox("Successfull", "Record updated successfully !!!");
                $("#btn_save_" + c_id).css("visibility", "hidden");
                $("#btn_cancel_" + c_id).css("visibility", "hidden");
                $("#btn_edit_" + c_id).show();

                // .DataTable().ajax.reload();


                // messageBox("Succeed", "record Saved Successfully !!!");
                //  fillGrid();
                //window.location.href = '/payroll/salaryinput';


                // $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                $("#" + newTable).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');


                //$("#" + childGridID).jqGrid({
                //    url: childGridURL_,
                //    mtype: "Get",
                //    datatype: "json",
                //    colNames: ['Actions', '', '', '', '',''],
                //    colModel: [
                //        { name: 'act', index: 'act', width: 200, sortable: false },
                //        { name: 'component_id', key: true, width: 75, hidden: true },
                //        { name: 'property_details', width: 200 },
                //        {
                //          name: 'component_value', width: 100, edittype: 'textbox', editable: true
                //        },
                //        { name: 'is_data_entry_comp', width: 150, hidden: true },
                //        { name: 'componet_value_old', width: 150, hidden: true }
                //    ],

                //    cellsubmit: 'clientArray',
                //    loadonce: true,
                //    width: '100%',
                //    height: '100%',
                //    //onSelectRow: editRow,
                //    rowNum: 10,
                //    rowList: [10, 20, 30],
                //    pager: '#prowed2',
                //    sortname: 'component_id',
                //    viewrecords: true,
                //    sortorder: "desc",
                //    editurl: childGridURL_,
                //    onSelectRow: function (id) {
                //        if (id && id !== lastsel) {
                //            $('#scrub').saveRow(lastsel);
                //            lastsel = id;
                //        }

                //    },
                //    gridComplete: function () {
                //        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
                //        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
                //        for (var i = 0; i < ids.length; i++) {
                //            var cl = ids[i].component_id;
                //            var cl1 = ids[i].is_data_entry_comp;
                //            if (cl1 == "1") {
                //                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('#jqGrid_1_table_25_table').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
                //                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('#jqGrid_1_table_25_table').saveRow('" + cl + "', false, 'clientArray');\"   />";
                //                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('#jqGrid_1_table_25_table').restoreRow('" + cl + "', DisplayEditButton);\" />";

                //                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

                //                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


                //            }
                //        }
                //    }

                //});

            }

        },
        //afterSubmit: function () {
        //    $("#jqGrid").jqGrid("setGridParam", { datatype: 'json' });
        //    return [true];
        //},
        error: function (error) {
            _GUID_New();
            messageBox("error", "Server busy please try again later...!");
        }

    });
    // $("#info_dialog").hide();
    //$("#jqGrid").jqGrid('clearGridData')
    //$("#jqGrid").jqGrid('setGridParam', { data: null, page: 1 })
    ////$("#jqGrid").trigger('reloadGrid');




    //$("#jqGrid").jqGrid({

    //    url: apiurl,
    //    mtype: "GET",
    //    datatype: "json",
    //    page: 1,
    //    colNames: ['Component ID', 'Item', 'Values'],
    //    colModel: [
    //        { name: 'component_id', key: true, width: 100, hidden: true },
    //        { name: 'property_details', width: 400, editable: true },
    //        { name: '0', width: 150, editable: true }
    //        //{ label: 'Contact Name', name: 'applicable_value', width: 150 }

    //    ],
    //    loadonce: true,
    //    width: '100%',
    //    height: '100%',
    //    rowNum: 50,
    //    subGrid: true,

    //    subGridRowExpanded: showChildGrid // javascript function that will take care of showing the child grid


    //});


    //$("#btn_save_" + rowid).css("visibility", "hidden");
    //$("#btn_cancel_" + rowid).css("visibility", "hidden");
    //$("#btn_edit_" + rowid).show();

}


function DisplayEditButton(rowid) {

    $("#btn_save_" + rowid).css("visibility", "hidden");
    $("#btn_cancel_" + rowid).css("visibility", "hidden");
    $("#btn_edit_" + rowid).show();
}
function showThirdLevelChildGrid(parentRowID, parentRowKey, rowid) {

    //alert('Errorrrrrrrrrrrrrrrrrrrrrr !!!!!!!!!!!');
    alert(parentRowID)
    var table_name = "#jqGrid_1_table_" + parentRowKey + "_table";
    alert(table_name)
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    //  var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/1/201905';
    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryInput/' + $('#ddlEmployee').val() + '/' + $('#ddlMonthyear').val() + '/' + parentRowKey;


    var grid_ = $("#" + childGridID);

    //$('#' + parentRowID).remove('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    var lastrow;
    var lastcell;

    $.ajax({
        url: childGridURL_,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            if (res != '') {
                //$("#" + childGridID).jqGrid('clearGridData')
                //    .jqGrid('setGridParam', { data: null, datatype: 'json' });


                $("#" + childGridID).jqGrid({
                    url: childGridURL_,
                    mtype: "GET",
                    datatype: "json",
                    colNames: ['', '', '', '', '', '', ''],
                    colModel: [
                        { name: 'act', index: 'act', width: 150, sortable: false },
                        { name: 'component_id', key: true, width: 75, hidden: true },
                        { name: 'property_details', width: 200 },
                        {
                            name: 'component_value', width: 100, edittype: 'textbox', editable: true
                        },
                        { name: 'is_data_entry_comp', width: 150, hidden: true },
                        { name: 'componet_value_old', width: 150, hidden: true },
                        { name: 'parentid', width: 150, hidden: true }
                    ],

                    cellsubmit: 'clientArray',
                    loadonce: true,
                    width: '100%',
                    height: '100%',
                    //onSelectRow: editRow,
                    rowNum: 10,
                    rowList: [10, 20, 30],
                    pager: '#prowed2',
                    sortname: 'component_id',
                    viewrecords: true,
                    sortorder: "desc",
                    //  editurl: childGridURL_,
                    subGrid: true, // set the subGrid property to true to show expand buttons for each row
                    subGridRowExpanded: showThirdLevelChildGrid, // javascript function that will take care of showing the child grid

                    gridComplete: function () {
                        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
                        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            var cl = ids[i].component_id;
                            var cl1 = ids[i].is_data_entry_comp;
                            var pr_id = ids[i].parentid;


                            if (cl1 == "1") {


                                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('" + table_name + "').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
                                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('" + table_name + "').saveRow('" + cl + "', false,SaveSalaryInput('" + cl + "," + pr_id + "," + parentRowID + "'));jQuery('" + table_name + "').restoreRow('" + cl + "' ,true); \"  />";
                                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('" + table_name + "').restoreRow( DisplayEditButton('" + cl + "'));\" />";

                                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

                                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


                            }
                        }
                    }

                });

            }

        }

    });

};

function ReloadChildGrid(parentRowID, parentRowKey, rowid) {



    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    //  var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + parentRowKey + '/1/201905';
    var childGridURL_ = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryInput/' + $('#ddlEmployee').val() + '/' + $('#ddlMonthyear').val() + '/' + parentRowKey;

    var grid_ = $("#" + childGridID);

    // $('#' + parentRowID).remove('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    //$('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    var lastrow;
    var lastcell;


    $.ajax({
        url: childGridURL_,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            if (res != '') {

                //jQuery("#" + childGridID).jqGrid('clearGridData')
                //    .jqGrid('setGridParam', { data: null, datatype: 'json' })
                //    .trigger('reloadGrid');

                $("#" + childGridID).jqGrid('clearGridData')
                $("#" + childGridID).jqGrid('setGridParam', { data: null, page: 1 })
                $("#" + childGridID).trigger('reloadGrid');

                $("#" + childGridID).jqGrid({
                    url: childGridURL_,
                    mtype: "GET",
                    datatype: "json",
                    colNames: ['Actions', '', '', '', '', ''],
                    colModel: [
                        { name: 'act', index: 'act', width: 150, sortable: false },
                        { name: 'component_id', key: true, width: 75, hidden: true },
                        { name: 'property_details', width: 200 },
                        {
                            name: 'component_value', width: 100, edittype: 'textbox', editable: true
                        },
                        { name: 'is_data_entry_comp', width: 150, hidden: true },
                        { name: 'componet_value_old', width: 150, hidden: true }
                    ],

                    cellsubmit: 'clientArray',
                    loadonce: true,
                    width: '100%',
                    height: '100%',
                    //onSelectRow: editRow,
                    rowNum: 10,
                    rowList: [10, 20, 30],
                    pager: '#prowed2',
                    sortname: 'component_id',
                    viewrecords: true,
                    sortorder: "desc",
                    editurl: childGridURL_,

                    gridComplete: function () {
                        var ids = jQuery($("#" + childGridID)).jqGrid('getRowData');
                        var ids1 = jQuery($("#" + childGridID)).jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            var cl = ids[i].component_id;
                            var cl1 = ids[i].is_data_entry_comp;



                            if (cl1 == "1") {


                                edit_ = "<input class=\"edit\" style='height:28px;width:50px;' type='button' id='btn_edit_" + cl + "' value='Edit' onclick=\"jQuery('#jqGrid_1_table_25_table').editRow('" + cl + "', true, TheOnEditFunction('" + cl + "') );\"  />";
                                save_ = "<input class=\"save\" style='height:28px;width:53px; visibility: hidden;' type='button' id='btn_save_" + cl + "' value='Save' onclick=\"jQuery('#jqGrid_1_table_25_table').saveRow('" + cl + "', false,SaveSalaryInput('" + cl + "')); \"  />";
                                cancel_ = "<input class=\"cancel\" style='height:28px;width:68px; visibility: hidden;' type='button' id='btn_cancel_" + cl + "'  value='Cancel' onclick=\"jQuery('#jqGrid_1_table_25_table').restoreRow( DisplayEditButton('" + cl + "'));\" />";

                                // jQuery("#jqGrid_1_table_25_table").jqGrid('setRowData', ids[i], { Action: edit_ + save_ + cancel_ });

                                jQuery($("#" + childGridID)).jqGrid('setRowData', ids1[i], { act: edit_ + save_ + cancel_ });


                            }
                        }
                    }

                });

            }

        }

    });

};



