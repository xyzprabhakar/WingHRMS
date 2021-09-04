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

        BindAllEmp_Company_hlday('ddlcompany', login_emp_id, login_company_id);
       

        // BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);

        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
        Get_KT_file_ById(login_emp_id);
        //setSelect('ddlEmployeeCode', login_emp_id);

        GetData(login_emp_id);

        $('#ddlcompany').bind("change", function () {
           // $('#loader').show();
            $("#ddlemployeetype").empty();
            $("#ddlEmployeeCode").empty();
            //BindEmployeeCodeFromEmpMasterIncludingAll('ddlemployeetype', $(this).val(), 0);
          //  BindEmployees_handoverto($(this).val(), login_emp_id);
           
            BindEmployees_handoverby('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
           // $('#loader').hide();
        });

        $('#ddlEmployeeCode').bind("change", function () {
           // $('#loader').show();
            debugger;
            var compid = $("#ddlcompany").val();
            var empid = $("#ddlEmployeeCode").val();
            BindEmployees_handoverto(compid, empid, 0, '');
            Get_KT_file_ById(empid);
            GetData(empid);
           // $('#loader').hide();
        });


        $('#btnreset').bind('click', function () {
            location.reload();
        });
        var kt_task_list = [];
        $('#btnsave_test').bind("click", function () {
            $("#tblkttask_insert TBODY TR").each(function () {
                var emp_ids_list1 = "";
                var employeelist1 = [];
                var row = $(this);
                var item = {};
                var taskName = row.find("TD").find("input[type=hidden]").eq(0);
                if (taskName.length != 0) {

                    item.emp_sepration_Id = ddlEmployeeCode; //row.find("TD").find("input[type=hidden]").eq(0).val();
                    item.taskName = row.find("TD").find("input[type=hidden]").eq(0).val();
                    item.Procedure = row.find("TD").find("input[type=hidden]").eq(1).val();
                    item.ModHandover = row.find("TD").find("input[type=hidden]").eq(2).val();

                    // var ddl_dyn = row.find("TD").find("select");

                    var dynamic_ddl1 = row.find("TD").find("select option:selected");
                    dynamic_ddl1.each(function () {
                        var empitm1 = {};
                        empitm1.employee_id = $(this).val();
                        employeelist1.push(empitm1);
                        emp_ids_list1 += $(this).val() + ",";
                    });
                    debugger;
                    var dddd = emp_ids_list1;
                    //$('#' + dynamic_ddl1 + ' option:selected').each(function () {
                    //    var empitm1 = {};
                    //    debugger;
                    //    empitm1.employee_id = $(this).val();
                    //    employeelist1.push(empitm);
                    //    emp_ids_list1 += $(this).val() + ",";
                    //});
                    // item.emp_id_list = emp_ids_list1;
                    //item.emp_id_list = row.find("TD").find("input[type=hidden]").eq(3).val();
                    item.remarks = row.find("TD").find("input[type=hidden]").eq(4).val();
                    item.is_active = 1;
                    item.created_by = login_emp_id;
                    item.last_modified_by = login_emp_id;
                    kt_task_list.push(item);
                }
            });
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtTaskName = $("#txtTaskName").val();
            var txtprocedure = $("#txtprocedure").val();
            var txtremark = $("#txtremark").val();
            var ddlmoh = $("#ddlmoh").val();
            //var txtModHandover = $("#txtModHandover").val();

            if (ddlEmployeeCode == null || ddlEmployeeCode == '' || ddlEmployeeCode == '0') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (txtTaskName == null || txtTaskName == '') {
                messageBox("error", "Please Enter Task Name...! ");
                $('#loader').hide();
                return;
            }
            if (ddlmoh == null || ddlmoh == '' || ddlmoh == '0') {
                messageBox("error", "Mode of handover can not blank...! ");
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
                var emp_ids_list1 = "";
                var employeelist1 = [];

                $("#tblkttask_insert TBODY TR").each(function () {
                    var row = $(this);
                    var item = {};
                    var taskName = row.find("TD").find("input[type=hidden]").eq(0);
                    if (taskName.length != 0) {
                        debugger;
                        item.emp_sepration_Id = ddlEmployeeCode; //row.find("TD").find("input[type=hidden]").eq(0).val();
                        item.taskName = row.find("TD").find("input[type=hidden]").eq(0).val();
                        item.Procedure = row.find("TD").find("input[type=hidden]").eq(1).val();
                        item.ModHandover = row.find("TD").find("select option:selected").eq(0).val();
                        //row.find("TD").find("input[type=hidden]").eq(2).val();

                        //var dynamic_ddl = row.find("TD").find("select").eq(0);
                        //
                        var dynamic_ddl = row.find("TD").find("select option:selected").eq(1);
                        dynamic_ddl.each(function () {
                            var empitm1 = {};
                            empitm1.employee_id = $(this).val();
                            employeelist1.push(empitm1);
                            emp_ids_list1 += $(this).val() + ",";
                        });
                        emp_ids_list1 = emp_ids_list1.slice(0, -1);
                        item.emp_id_list = emp_ids_list1;
                       // item.emp_id_list = row.find("TD").find("input[type=hidden]").eq(3).val();
                        item.remarks = row.find("TD").find("input[type=hidden]").eq(4).val();
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
            item1.ModHandover = ddlmoh;
            item1.remarks = txtremark;
            item1.emp_id_list = emp_ids_list;//employeelist;
            item1.is_active = 1;
            item1.created_by = login_emp_id;
            item1.last_modified_by = login_emp_id;
            kt_task_list.push(item1);


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
                       // BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
                        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);

                        // setSelect('ddlEmployeeCode', login_emp_id);
                        GetData(login_emp_id);
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
            // 
            $('#loader').show();
            var k_id = $("#hdnid").val();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtTaskName = $("#txtTaskName").val();
            var txtprocedure = $("#txtprocedure").val();
            var txtremark = $("#txtremark").val();
            var ddlmoh = $("#ddlmoh").val();

            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //if ($("#is_active").is(":checked")) {
            //    is_active = 1;
            //}
            //if ($("#is_in_active").is(":checked")) {
            //    is_active = 0;
            //}

            if (ddlEmployeeCode == null || ddlEmployeeCode == '' || ddlEmployeeCode == '0') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (txtTaskName == null || txtTaskName == '') {
                messageBox("error", "Please Enter Task Name...! ");
                $('#loader').hide();
                return;
            }
            if (ddlmoh == null || ddlmoh == '' || ddlmoh == '0') {
                messageBox("error", "Mode of handover can not blank...! ");
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

            var myData = {
                'id': k_id,
                'emp_sepration_Id': ddlEmployeeCode.trim(),
                'taskName': txtTaskName,
                'Procedure': txtprocedure,
                'ModHandover': ddlmoh,
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
                        $("#ddlmoh").val("0");
                       // BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
                        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        //  setSelect('ddlEmployeeCode', login_emp_id);

                        GetData(login_emp_id);
                        //  $('input:radio[name=Status]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('#btnAdd').show();
                        messageBox("success", Msg);
                        setTimeout(function () { location.reload(); }, 3000);
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

        $('#btnupload').bind("click", function () {
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            if (ddlEmployeeCode == null || ddlEmployeeCode == '' || ddlEmployeeCode == '0') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            var uploadfilee = $("#f_kttaskstatus").val();
            if (uploadfilee == "" || uploadfilee == null) {
                messageBox("error", "Please upload File...! ");
                $('#loader').hide();
                return;
            }

            var allowedFiles = [".pdf", ".jpeg", ".jpg"];
            var fileUpload = document.getElementById("f_kttaskstatus");
            
            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
            if (!regex.test(fileUpload.value.toLowerCase())) {
              //  messageBox("error", "Please upload files having extensions: <b>" + allowedFiles.join(', ') + "</b> only.");
                messageBox("error", "Please upload only pdf and jpg File...! ");
                $('#loader').hide();
                return;
            }
            
            //var ext = $('#f_kttaskstatus').val().split('.').pop().toLowerCase();
            //if ($.inArray(ext, ['xlsx,pdf,jpeg,jpg,png,doc,docx,xls']) == -1) {
            //    messageBox("error", "Invalid File Format <br> Please upload only .xlsx,.pdf,.doc,.jpeg,.png file!");
            //    $('#loader').hide();
            //    return;
            //}

            var files = document.getElementById("f_kttaskstatus").files;
            var mydata = {
                'seperation_id': ddlEmployeeCode,
                'uploaded_by': login_emp_id,
                'last_modified_by': login_emp_id
            };
            var obj = JSON.stringify(mydata);
            var formdata = new FormData();
            formdata.append('AllData', obj);

            for (var i = 0; i < files.length; i++) {
                formdata.append('Files', files[i]);
            }
            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Upload_KTTaskStatus';

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // console.log(Obj);
            $.ajax({
                url: apiurl,
                type: "POST",
                contentType: false,
                processData: false,
                data: formdata,
                dataType: "json",
                // contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {

                        // BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
                        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        //setSelect('ddlEmployeeCode', login_emp_id);
                        GetData(login_emp_id);
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

    }, 2000);// end timeout

});



$("body").on("click", "#btnAdd", function () {
    //if (confirm("Are you sure you want to proceed?")) {
    $('#loader').show();

    var ddlEmployeeCode = $("#ddlEmployeeCode option:selected");
    var txtTaskName = $("#txtTaskName");
    var txtprocedure = $("#txtprocedure");
    var txtremark = $("#txtremark");
    var ddlmoh = $("#ddlmoh");

    if (ddlEmployeeCode.val() == null || ddlEmployeeCode.val() == '' || ddlEmployeeCode.val() == '0') {
        messageBox("error", "Handover By can not blank...! ");
        $('#loader').hide();
        return;
    }
    if (txtTaskName.val() == null || txtTaskName.val() == '') {
        messageBox("error", "Please Enter Task Name...! ");
        $('#loader').hide();
        return;
    }
    if (ddlmoh.val() == null || ddlmoh.val() == '' || ddlmoh.val() == '0') {
        messageBox("error", "Mode of handover can not blank...! ");
        $('#loader').hide();
        return;
    }
    var employeelist = [];
    var emp_ids_list = "";
    var emp_name_list = "";
    var newddlemp = "<select multiple='multiple' class='form-control'>";
    //$('#ddlemployeetype > option:selected').each(function () {
    //    var empitm = {};
    //    empitm.employee_id = $(this).val();
    //    employeelist.push(empitm);
    //    emp_ids_list += $(this).val() + ",";
    //    emp_name_list += $(this).text() + ",";
    //    newddlemp += "<option selected value=" + $(this).val() + ">" + $(this).text() + "</option>";
    //});
    //  $('#ddlemployeetype').find('option').each(function () {
    $('#ddlemployeetype > option').each(function () {
        if ($(this).is(':selected')) {
            var empitm = {};
            empitm.employee_id = $(this).val();
            employeelist.push(empitm);
            emp_ids_list += $(this).val() + ",";
            emp_name_list += $(this).text() + ",";
            newddlemp += "<option selected value=" + $(this).val() + ">" + $(this).text() + "</option>";
        }
        else {
            newddlemp += "<option value=" + $(this).val() + ">" + $(this).text() + "</option>";
        }
    });
    newddlemp += "</select>";

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
    var modehndovr = ddlmoh.val();

    var row = tBody.insertRow(-1);

    var moh = "<select class='form-control'>"
    $('#ddlmoh > option').each(function () {
        if ($(this).is(':selected')) {            
            moh += "<option selected value=" + $(this).val() + ">" + $(this).text() + "</option>";
        }
        else {
            moh += "<option value=" + $(this).val() + ">" + $(this).text() + "</option>";
        }
    });
    moh += "</select>";

    var cell = $(row.insertCell(-1));
    cell.html("<input type = 'hidden' value = '" + tskname + "' /><input type = 'hidden' value = '" + prcname + "' /><input type = 'hidden' value = '" + modehndovr + "' /><input type='hidden' value=" + emp_ids_list + " /><input type = 'hidden' value = " + txtremark.val() + " /><input type='text' value='" + tskname + "' class='form-control'  placeholder='Task Name' />");

    //cell = $(row.insertCell(-1));
    //cell.html("<input type='text' value='" + tskname + "' class='form-control'  placeholder='Task Name' />");
    cell = $(row.insertCell(-1));
    if (txtprocedure.val() == "") { cell.html("<input type='text' class='form-control' placeholder='Procedure' />"); }
    else { cell.html("<input type='text' class='form-control' value='" + prcname + "'  placeholder='Procedure' />"); }
    cell = $(row.insertCell(-1));
    cell.html(moh);
    //if (ddlmoh.val() == "") { cell.html("<input type='text' class='form-control' placeholder='Mode of Handover' />"); }
    //else { cell.html("<input type='text' class='form-control' value='" + modehndovr + "'  placeholder='Mode of Handover' />"); }

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
    ddlmoh.val("0");
    txtremark.val("");
    $("#ddlemployeetype option:selected").removeAttr("selected");
    $('#loader').hide();
    // }
});

 
function BindAllEmp_Company_hlday(ControlId, EmployeeId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;

    var emp_company_lst;
    var lst = localStorage.getItem("emp_companies_lst");

    if (lst != undefined && lst != null && lst != "" && lst.length > 0) {

        emp_company_lst = JSON.parse(lst);
    }
    else {

        var listapi = localStorage.getItem("ApiUrl");

        $.ajax({
            type: "GET",
            url: listapi + "apiEmployee/Get_Emp_all_Company/" + EmployeeId,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }

                if (lst == "" || lst == null || lst.length == 0) {
                    localStorage.setItem("emp_companies_lst", JSON.stringify(res));
                }

                emp_company_lst = res;


            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });

    }

    if (emp_company_lst != null && emp_company_lst != "" && emp_company_lst.length > 0) {


        if (SelectedVal == 0) {
            // $(ControlId).empty().append('<option selected="selected" value="">--Please Select--</option>');
            $(ControlId).empty().append('<option value="0">All</option>');
        }
        else if (SelectedVal == -1) { $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>'); }
        else {
            $(ControlId).empty();//.append('<option selected="selected" value="0">--Please Select--</option>');
        }

        $.each(emp_company_lst, function (data, value) {

            $(ControlId).append($("<option></option>").val(value.company_id).html(value.company_name));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        }

        //$(ControlId).trigger("select2:updated");
        //$(ControlId).select2();
    }


    $('#loader').hide();

}
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
function GetData(emp_id_) {
    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_List/' + emp_id_;

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
                            targets: [9],
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

                    // { "data": "company", "name": "company", "title": "Company", "autoWidth": true },
                    { "data": "handoverby", "name": "handoverby", "title": "Handover By", "autoWidth": true },
                    { "data": "task_name", "name": "task_name", "title": "Task Name", "autoWidth": true },
                    // { "data": "empnamelist", "name": "empnamelist", "title": "Team Members", "autoWidth": true },
                    {
                        "title": "Team Members", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            var handoverto = "";

                            for (var i = 0; i < full.empnamelist.length; i++) {
                                handoverto += full.empnamelist[i].empname + ", ";
                            }
                            return handoverto.slice(0, -2);
                        }
                    },
                    { "data": "procedure", "name": "procedure", "title": "Procedure", "autoWidth": true },
                    { "data": "modhandover", "name": "modhandover", "title": "Mode of Handover", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                   // { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
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
            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });
}

function GetEditData(id) {
    // 
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }


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

            data = res;
            $("#hdnsno").val(data.s_no);
            $("#txtTaskName").val(data.task_name);
            $("#txtprocedure").val(data.procedure);
            $("#txtremark").val(data.remarks);
            $("#ddlmoh").val(data.modhandover);

            if (data.is_active == 0) {
                $("#is_in_active").prop('checked', true);
            }
            else {
                $("#is_active").prop("checked", true)
            }
            BindAllEmp_Company_hlday('ddlcompany', login_emp_id, data.company_id);
            debugger;
            BindEmployees_handoverby('ddlEmployeeCode', data.company_id, login_emp_id, data.handoverby);
          //  setSelect('ddlEmployeeCode', data.handoverby);
            //BindEmployees_handoverto('ddlemployeetype', login_company_id, 0);
            BindEmployees_handoverto(data.company_id, data.handoverby, 1, data.emplist);

            

            // setTimeout(function () {
          
            //}, 2000);

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

function BindEmployees_handoverby(ControlId, CompanyId, loginid, SelectedVal) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiEmployee/Get_EmpName_ESeperation/" + CompanyId + "/" + loginid,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            $(ControlId).empty();
            $(ControlId).empty().append('<option selected="selected" value="0">--Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
            $('#loader').hide();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}


function BindEmployees_handoverto(CompanyId, empid, onedit,elist) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmpNameAndCodeByDept/" + CompanyId + "/" + empid,
        //url: apiurl + "apiMasters/Get_EmpNameAndCodeByeComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            $("#ddlemployeetype").empty();
            // $(ControlId).empty().append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $("#ddlemployeetype").append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })
            if (onedit == 1) {
                for (var i = 0; i < elist.length; i++) {
                    $('#ddlemployeetype').find('option').each(function () {
                        debugger;
                        if (elist[i] == $(this).val()) {
                            $(this).attr('selected', 'selected');
                            // $("#ddlemployeetype option[value='" + data.emplist[i] + "']").attr('selected', 'selected');
                            //$("#ddlemployeetype").val(data.emplist[i]);
                        }
                    });
                }
            }
            //get and set selected value
            //if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            //    $(ControlId).val(SelectedVal);
            //}
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
            $('#loader').hide();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}


function Download_KTExcelFile() {
    window.open(localStorage.getItem("ApiUrl").replace("api/", "") + "/UploadFormat/KTFormat.xlsx");
}
function Get_KT_file_ById(id) {

    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_file_ById/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (data.ktfile != "" && data.ktfile != null) {
                $("#lnk_kt_file").attr("href", localStorage.getItem("ApiUrl").replace("api/", "") + "/KTTask/" + data.ktfile);
                $("#lnk_kt_file").show();
            }
            else {
                $("#lnk_kt_file").hide();
            }

        },
        error: function (err) {
            $('#loader').hide();
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
function opentab(evt, csName) {
    debugger;
    var i, tabcontent, tablinks, setclsname ="ktform";
    if (csName == "ktform") {
        setclsname = "ktfile";
    }
    tabcontent = document.getElementsByClassName(setclsname);
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", " inactive");
    }

    tabcontent = document.getElementsByClassName(csName);
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "block";
    }
    //document.getElementsByClassName(csName).style.display = "block";
    evt.currentTarget.className += " active";
    return false;
}
