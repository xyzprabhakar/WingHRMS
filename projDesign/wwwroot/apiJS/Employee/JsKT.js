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
        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
        setSelect('ddlEmployeeCode', login_emp_id);

        GetData();

        $('#btnreset').bind('click', function () {
            location.reload();
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();

            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtTaskName = $("#txtTaskName").val();
            var txtprocedure = $("#txtprocedure").val();
            var txtremark = $("#txtremark").val();
            var txtModHandover = $("#txtModHandover").val();

            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }
            if ($("#is_in_active").is(":checked")) {
                is_active = 0;
            }


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
            debugger;
            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);

                if (empitm.employee_id == 0) {
                    selectempall = 1; // all employee selected 
                }
            });
            if (employeelist.length == 0) {
                messageBox("error", "Please Select Employee (Handover To)...! ");
                $('#loader').hide();
                return;
            }
            if (txtremark == null || txtremark == '') {
                messageBox("error", "Please Enter Remarks...! ");
                $('#loader').hide();
                return;
            }

            if (!$("#is_active").is(":checked") && !$("#is_in_active").is(":checked")) {
                messageBox("error", "Please Select Status...! ");
                $('#loader').hide();
                return;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            
            debugger;
            var myData = {
                'emp_sepration_Id': ddlEmployeeCode.trim(),
                'taskName': txtTaskName,
                'Procedure': txtprocedure,
                'ModHandover': txtModHandover,
                'remarks': txtremark,
                'is_active': is_active,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id,
                'emp_id_list': employeelist
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Save_KT_Task_Master';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            console.log(Obj);
            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                //data: {
                //    'Obj': JSON.stringify(Obj),
                //    'locationlist': JSON.stringify(locationlist),
                //    'employeelist': JSON.stringify(employeelist)
                //}, 
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
                        $('input:radio[name=Status]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        messageBox("success", Msg);

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

            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }
            if ($("#is_in_active").is(":checked")) {
                is_active = 0;
            }

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

            if (!$("#is_active").is(":checked") && !$("#is_in_active").is(":checked")) {
                messageBox("error", "Please Select Status...! ");
                $('#loader').hide();
                return;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);

                if (empitm.employee_id == 0) {
                    selectempall = 1; // all employee selected 
                }
            });
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
                'emp_id_list': employeelist
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
                        $('input:radio[name=Status]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
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
                        $(this).attr('selected', 'selected');
                    }
                });
            }

            $('#ddlemployeetype').trigger("select2:updated");
            $('#ddlemployeetype').select2();

            $('#btnupdate').show();
            $('#btnsave').hide();

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
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
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
