var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {


        $('#btnupdate').hide();
        $('#btnsave').show();
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
        setSelect('ddlEmployeeCode', login_emp_id);

        GetData();

        $('#btnreset').bind('click', function () {
            location.reload();
        });
        var kt_task_list = [];
        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtTaskName = $("#txtTaskName").val();
            var txtprocedure = $("#txtprocedure").val();
            var txtremark = $("#txtremark").val();
            var txtModHandover = $("#txtModHandover").val();

            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (txtTaskName == null || txtTaskName == '') {
                messageBox("error", "Please Enter Task Name...! ");
                $('#loader').hide();
                return;
            }
            var emp_ids_list = "";
            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);
                emp_ids_list += $(this).val() + ",";
            });
            if (employeelist.length == 0) {
                messageBox("error", "Please Select Employee (Handover To)...! ");
                $('#loader').hide();
                return;
            }
            else {
                emp_ids_list = emp_ids_list.slice(0, -1);
            }

            if (txtremark == null || txtremark == '') {
                messageBox("error", "Please Enter Remarks...! ");
                $('#loader').hide();
                return;
            }
            kt_task_list = [];

            var rowCount = $('#tblkttask_insert TBODY tr').length;
            if (rowCount > 1) {
                $("#tblkttask_insert TBODY TR").each(function () {
                    debugger;
                    var row = $(this);
                    var item = {};
                    var emplength = row.find("TD").find("input[type=hidden]").eq(0);
                    if (emplength.length != 0) {
                        item.emp_sepration_Id = row.find("TD").find("input[type=hidden]").eq(0).val();
                        item.taskName = row.find("TD").find("input[type=hidden]").eq(1).val();
                        item.Procedure = row.find("TD").find("input[type=hidden]").eq(2).val();
                        item.ModHandover = row.find("TD").find("input[type=hidden]").eq(3).val();
                        item.emp_id_list = row.find("TD").find("input[type=hidden]").eq(4).val();
                        item.remarks = row.find("TD").find("input[type=hidden]").eq(5).val();
                        item.is_active = 1;
                        item.created_by = login_emp_id;
                        item.last_modified_by = login_emp_id;
                        kt_task_list.push(item);
                    }
                });
            }
            var item1 = {};

            item1.emp_sepration_Id = ddlEmployeeCode;
            item1.taskName = txtTaskName;
            item1.Procedure = txtprocedure;
            item1.ModHandover = txtModHandover;
            item1.remarks = txtremark;
            item1.emp_id_list = emp_ids_list;//employeelist;
            item1.is_active = 1;
            item1.created_by = login_emp_id;
            item1.last_modified_by = login_emp_id;
            kt_task_list.push(item1);
            debugger;

            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Save_KT_Task';
            var Obj = JSON.stringify(kt_task_list);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            //console.log(Obj);
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

                        BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
                        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        setSelect('ddlEmployeeCode', login_emp_id);
                        GetData();
                        //$('input:radio[name=Status]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        messageBox("success", Msg);
                        setTimeout(function () { location.reload(); }, 3000);
                    }
                    else {
                        messageBox("error", Msg);
                        return false;
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
            //}
        });

        $("#btnupdate").bind("click", function () {
            // debugger;
            $('#loader').show();
            var k_id = $("#hdnid").val();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtTaskName = $("#txtTaskName").val();
            var txtprocedure = $("#txtprocedure").val();
            var txtremark = $("#txtremark").val();
            var txtModHandover = $("#txtModHandover").val();

            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //if ($("#is_active").is(":checked")) {
            //    is_active = 1;
            //}
            //if ($("#is_in_active").is(":checked")) {
            //    is_active = 0;
            //}

            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (txtTaskName == null || txtTaskName == '') {
                messageBox("error", "Please Enter Task Name...! ");
                $('#loader').hide();
                return;
            }
            if (txtremark == null || txtremark == '') {
                messageBox("error", "Please Enter Remarks...! ");
                $('#loader').hide();
                return;
            }

            //if (!$("#is_active").is(":checked") && !$("#is_in_active").is(":checked")) {
            //    messageBox("error", "Please Select Status...! ");
            //    $('#loader').hide();
            //    return;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }
            var emp_ids_list = "";
            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);
                emp_ids_list += $(this).val() + ",";
            });
            if (employeelist.length == 0) {
                messageBox("error", "Please Select Employee (Handover To)...! ");
                $('#loader').hide();
                return;
            }
            else {
                emp_ids_list = emp_ids_list.slice(0, -1);
            }
            debugger;
            var myData = {
                'id': k_id,
                'emp_sepration_Id': ddlEmployeeCode.trim(),
                'taskName': txtTaskName,
                'Procedure': txtprocedure,
                'ModHandover': txtModHandover,
                'remarks': txtremark,
                'is_active': is_active,
                'last_modified_by': login_emp_id,
                'emp_id_list': emp_ids_list
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Update_KT_Task_Master';
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
                        $("#txtTaskName").val("");
                        $("#txtprocedure").val("");
                        $("#txtremark").val("");
                        $("#txtModHandover").val("");
                        BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
                        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        setSelect('ddlEmployeeCode', login_emp_id);

                        GetData();
                      //  $('input:radio[name=Status]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('#btnAdd').show();
                        messageBox("success", Msg);
                        //alert(Msg);
                        //location.reload();
                    }
                    else {
                        messageBox("error", Msg);
                        return false;
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

        });

    }, 2000);// end timeout

});

$("body").on("click", "#btnAdd", function () {
    //if (confirm("Are you sure you want to proceed?")) {
    $('#loader').show();

    var ddlEmployeeCode = $("#ddlEmployeeCode option:selected");
    var txtTaskName = $("#txtTaskName");
    var txtprocedure = $("#txtprocedure");
    var txtremark = $("#txtremark");
    var txtModHandover = $("#txtModHandover");

    if (ddlEmployeeCode.val() == null || ddlEmployeeCode.val() == '') {
        messageBox("error", "Handover By can not blank...! ");
        $('#loader').hide();
        return;
    }
    if (txtTaskName.val() == null || txtTaskName.val() == '') {
        messageBox("error", "Please Enter Task Name...! ");
        $('#loader').hide();
        return;
    }
    var employeelist = [];
    var emp_ids_list = "";
    var emp_name_list = "";
    var newddlemp = "<select multiple='multiple' class='form-control'>";
    $('#ddlemployeetype > option:selected').each(function () {
        var empitm = {};
        empitm.employee_id = $(this).val();
        employeelist.push(empitm);
        emp_ids_list += $(this).val() + ",";
        emp_name_list += $(this).text() + ",";
        newddlemp += "<option selected value=" + $(this).val() + ">" + $(this).text() + "</option>";
    });
    newddlemp += "</select>";
    debugger;
    emp_ids_list = emp_ids_list.slice(0, -1);
    emp_name_list = emp_name_list.slice(0, -1);
    if (employeelist.length == 0) {
        messageBox("error", "Please Select Employee (Handover To)...! ");
        $('#loader').hide();
        return;
    }
    if (txtremark.val() == null || txtremark.val() == '') {
        messageBox("error", "Please Enter Remarks...! ");
        $('#loader').hide();
        return;
    }
    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblkttask_insert > TBODY")[0];
    //Add Row.
    var tskname = txtTaskName.val();
    var prcname = txtprocedure.val();
    var modehndovr = txtModHandover.val();

    var row = tBody.insertRow(-1);
    debugger;
    var cell = $(row.insertCell(-1));
    cell.html("<input type = 'hidden' value = " + ddlEmployeeCode.val() + " /><input type = 'hidden' value = '" + tskname + "' /><input type = 'hidden' value = '" + prcname + "' /><input type = 'hidden' value = '" + modehndovr + "' /><input type='hidden' value=" + emp_ids_list + " /><input type = 'hidden' value = " + txtremark.val() + " /><select  class='form-control'><option value=" + ddlEmployeeCode.val() + ">" + ddlEmployeeCode.text() + "</option></select>");

    cell = $(row.insertCell(-1));
    cell.html("<input type='text' value='" + tskname + "' class='form-control'  placeholder='Task Name' />");
    cell = $(row.insertCell(-1));
    if (txtprocedure.val() == "") { cell.html("<input type='text' class='form-control' placeholder='Procedure' />"); }
    else { cell.html("<input type='text' class='form-control' value='" + prcname + "'  placeholder='Procedure' />"); }
    cell = $(row.insertCell(-1));
    if (txtModHandover.val() == "") { cell.html("<input type='text' class='form-control' placeholder='Mode of Handover' />"); }
    else { cell.html("<input type='text' class='form-control' value='" + modehndovr + "'  placeholder='Mode of Handover' />"); }
    cell = $(row.insertCell(-1));
    cell.html(newddlemp);
    cell = $(row.insertCell(-1));
    cell.html("<textarea class='form-control'>" + txtremark.val() + "</textarea>");

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("class", "removecell");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("-");

    cell.append(btnRemove);

    //Clear the TextBoxes.

    txtTaskName.val("");
    txtprocedure.val("");
    txtModHandover.val("");
    txtremark.val("");
    $("#ddlemployeetype option:selected").removeAttr("selected");
    $('#loader').hide();
    // }
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete this?")) {
        //Get the reference of the Table.
        var table = $("#tblkttask_insert")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

//--------bind data in jquery data table
function GetData() {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_List/' + login_emp_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblKTTask").DataTable({
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
                            targets: [7],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        //{
                        //    targets: [4],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [6],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [5],
                        //    "class": "text-center"

                        //}
                    ],

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "handoverby", "name": "handoverby", "title": "Handover By", "autoWidth": true },
                    { "data": "task_name", "name": "task_name", "title": "Task Name", "autoWidth": true },
                    // { "data": "empnamelist", "name": "empnamelist", "title": "Team Members", "autoWidth": true },
                    {
                        "title": "Team Members", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            var handoverto = "";
                            debugger;
                            for (var i = 0; i < full.empnamelist.length; i++) {
                                handoverto += full.empnamelist[i].empname + ", ";
                            }
                            return handoverto.slice(0, -2);
                        }
                    },
                    { "data": "procedure", "name": "procedure", "title": "Procedure", "autoWidth": true },
                    { "data": "modhandover", "name": "modhandover", "title": "Mode of Handover", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status as on Date", "autoWidth": true },
                    { "data": "handoverdate", "name": "handoverdate", "title": "Handover Date", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });
}

function GetEditData(id) {
    // debugger;
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_ById/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            debugger;
            data = res;
            $("#hdnsno").val(data.s_no);
            $("#txtTaskName").val(data.task_name);
            $("#txtprocedure").val(data.procedure);
            $("#txtremark").val(data.remarks);
            $("#txtModHandover").val(data.modhandover);

            if (data.is_active == 0) {
                $("#is_in_active").prop('checked', true);
            }
            else {
                $("#is_active").prop("checked", true)
            }

            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
            debugger;

            setSelect('ddlEmployeeCode', data.handoverby);
            for (var i = 0; i < data.emplist.length; i++) {
                $('#ddlemployeetype').find('option').each(function () {
                    if (data.emplist[i] == $(this).val()) {
                        //$(this).attr('selected', 'selected');
                        $("#ddlemployeetype option[value='" + data.emplist[i] + "']").attr('selected', 'selected');
                    }
                });
            }

            //$('#ddlemployeetype').trigger("select2:updated");
            //$('#ddlemployeetype').select2();

            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#btnAdd').hide();

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}

function BindEmployees_handoverto(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmpNameAndCodeByeComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            $(ControlId).empty();
            // $(ControlId).empty().append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });
}

function BindEmployees_handoverby(ControlId, CompanyId, LoginEmpID, SelectedVal) {
    var listapi = localStorage.getItem("ApiUrl");
    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }

    var emp_lst_under_login_emp;

    var lst_ = localStorage.getItem("emp_under_login_emp");

    if (lst_ != null && lst_ != "" && lst_.length > 0) {

        emp_lst_under_login_emp = JSON.parse(lst_);
    }
    else {
        $.ajax({
            type: "GET",
            //url: listapi + "apiMasters/Get_EmployeeHeadList",
            url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId + "/" + LoginEmpID,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;
                emp_lst_under_login_emp = res;

            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });

    }



    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {

        var selected_data = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

        $.each(selected_data, function (data, value) {

            $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
        }

    }
    // $(ControlId).trigger("select2:updated");
    // $(ControlId).select2();

}
function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}
