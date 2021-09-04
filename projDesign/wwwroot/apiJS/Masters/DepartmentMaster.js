$('#loader').show();
var login_emp_id;
var login_role_id;
var default_company;
//var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        //HaveDisplay = ISDisplayMenu("Display Company List");

        BindCompanyListAll('ddlcompany', login_emp_id, -1);
        setSelect('ddlcompany', default_company);
        // BindAllEmp_Company('ddlcompany', login_emp_id, default_company);

        BindOnlyProbation_Confirmed_emp('ddlheadcode', default_company, 0);
        // BindEmployeeCodeFromEmpMasterByComp('ddlheadcode', default_company, 0);
        GetDataByCompanyId(default_company);
        BindDepartmentList('ddl_department', default_company, -1);
        Get_sub_Dept_Data(default_company);

        $('#btnupdate').hide();
        $('#btn_sub_dept_update').hide();
        $('#btnsave').show();
        $('#btn_sub_dept_save').show();

        $('button').on('click', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust();
        });
        $('#loader').hide();



        $('#ddlcompany').bind("change", function () {
            // $("#ddlheadcode").empty();
            // $("#ddl_department").empty();
            BindOnlyProbation_Confirmed_emp('ddlheadcode', $(this).val(), 0);
            BindDepartmentList('ddl_department', $(this).val(), -1);
            if ($(this).val() == 0) {
                GetData();
                Get_sub_Dept_Data(0);
            }
            else {
                GetDataByCompanyId($(this).val());
                Get_sub_Dept_Data($(this).val());
            }

            //BindEmployeeCodeFromEmpMasterByComp('ddlheadcode', $(this).val(), 0);




        });

        $('#btnreset').bind('click', function () {
            location.reload();
        });

        $("#btn_sub_dept_reset").bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            $('#loader').show();
            //debugger;;
            var department_name = $.trim($("#txtdepartment").val());
            var department_short_name = $.trim($('#txtshortname').val());
            var employee_id = $('#ddlheadcode').val();
            var company_id = $('#ddlcompany').val();
            var department_head_employee_code = $('#ddlheadcode option:selected').text();
            //var department_head_employee_name = $('#txtheadname').val();
            var HdCode = $("#ddlheadcode").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (department_name == null || department_name == '') {
                errormsg = "Please enter department name !! <br/>";
                iserror = true;
            }
            if (department_short_name == null || department_short_name == '') {
                errormsg = errormsg + "Please enter department code !! <br/>";
                iserror = true;
            }
            if (company_id == null || company_id == '0' || company_id == '') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            //if (HdCode == null || HdCode == "0" || HdCode == "") {
            //    errormsg = errormsg + "Please select department head !! <br/>";
            //    iserror = true;
            //}
            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'department_name': department_name,
                'department_short_name': department_short_name,
                'company_id': company_id,
                'employee_id': employee_id,
                'department_head_employee_code': department_head_employee_code,
                //'department_head_employee_name': "",
                'is_active': is_active
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_DepartmentMaster';
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
                    //// debugger;;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtdepartment").val('');
                        $('#txtshortname').val('');
                        $('#ddlcompany').val('0');
                        $('#ddlheadcode').val('0');
                        $('#txtheadname').val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $("btnupdate").text('Update').attr("disabled", false);
                        GetData();
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        messageBox("success", Msg);
                        // alert(Msg);
                        // location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {

                        messageBox("error", Msg);
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
        });


        //-------update city data
        $("#btnupdate").bind("click", function () {
            //debugger;;
            $('#loader').show();
            var department_id = $("#hdnid").val();
            var department_name = $.trim($("#txtdepartment").val());
            var department_short_name = $.trim($('#txtshortname').val());
            var employee_id = $('#ddlheadcode').val();
            var company_id = $('#ddlcompany').val();
            var department_head_employee_code = $('#ddlheadcode option:selected').text();
            // var department_head_employee_name = $('#txtheadname').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (department_name == null || department_name == '') {
                errormsg = "Please enter department name !! <br/>";
                iserror = true;
            }
            if (department_short_name == null || department_short_name == '') {
                errormsg = errormsg + "Please enter department code !! <br/>";
                iserror = true;
            }
            if (company_id == null || company_id == '0' || company_id == '') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (department_head_employee_code == null || department_head_employee_code == "0") {
                errormsg = errormsg + "Please select department head !! ";
                iserror = true;
            }
            if (!$("input[name='chkstatus']:checked").val()) {

                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            //if (employee_id == "0" || employee_id == "" || employee_id == null) {

            //    errormsg = errormsg + "Please select department head !! <br/>";
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'department_id': department_id,
                'department_name': department_name,
                'department_short_name': department_short_name,
                'company_id': company_id,
                'employee_id': employee_id,
                'department_head_employee_code': department_head_employee_code,
                //'department_head_employee_name': department_head_employee_name,
                'is_active': is_active
            };

            $("btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_DepartmentMaster';
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
                    //// debugger;;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        //alert(Msg);
                        $("#txtdepartment").val('');
                        $('#txtshortname').val('');
                        $('#ddlcompany').val('0');
                        $('#ddlheadcode').val('0');
                        $('#txtheadname').val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $("btnupdate").text('Update').attr("disabled", false);
                        $("#hdnid").val('');
                        GetData();
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        messageBox("success", Msg);
                        // location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("btnupdate").text('Update').attr("disabled", false);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);
                }

            });
        });


        //sub department start

        $('#btn_sub_dept_save').bind("click", function () {
            $('#loader').show();
            var sub_department_name = $("#txtsubdeptname_").val();
            var sub_department_code = $('#txtsubdeptcode_').val();
            var department_id = $('#ddl_department').val();
            var company_id = $('#ddlcompany').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus_sub_dept']:checked")) {
                if ($("input[name='chkstatus_sub_dept']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //if (emp_role_idd == "2") {
            //    company_id = company_idd;
            //}

            //validation part
            if (sub_department_name == null || sub_department_name == '') {
                errormsg = "Please enter sub department name !! <br/>";
                iserror = true;
            }
            if (sub_department_code == null || sub_department_code == '') {
                errormsg = errormsg + "Please enter sub department code !! <br/>";
                iserror = true;
            }
            if (company_id == null || company_id == '0' || company_id == '') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (department_id == null || department_id == '0') {
                errormsg = errormsg + "Please select department name !! <br/>";
                iserror = true;
            }
            if (!$("input[name='chkstatus_sub_dept']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'sub_department_name': sub_department_name,
                'sub_department_code': sub_department_code,
                'company_id': company_id,
                'department_id': department_id,
                'is_active': is_active
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_SubDepartmentMaster';
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
                        //alert(Msg);
                        $("#txtsubdeptname_").val('');
                        $('#txtsubdeptcode_').val('');
                        $('#ddl_department').val('0');
                        $('input:radio[name=chkstatus_sub_dept]:checked').prop('checked', false);
                        $("btn_sub_dept_update").text('Update').attr("disabled", false);
                        $("#hdnid").val('');
                        Get_sub_Dept_Data(company_id);
                        $('#btn_sub_dept_update').hide();
                        $('#btn_sub_dept_save').show();
                        messageBox("success", Msg);
                        //  location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });
        });

        $("#btn_sub_dept_update").bind("click", function () {
            $('#loader').show();
            var sub_department_id = $("#hdnid").val();
            var sub_department_name = $("#txtsubdeptname_").val();
            var sub_department_code = $('#txtsubdeptcode_').val();
            var department_id = $('#ddl_department').val();
            var company_id = $('#ddlcompany').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus_sub_dept']:checked")) {
                if ($("input[name='chkstatus_sub_dept']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //if (emp_role_idd == "2") {
            //    company_id = company_idd;
            //}

            //validation part
            if (sub_department_name == null || sub_department_name == '') {
                errormsg = "Please enter sub department name !! <br/>";
                iserror = true;
            }
            if (sub_department_code == null || sub_department_code == '') {
                errormsg = errormsg + "Please enter sub department code !! <br/>";
                iserror = true;
            }
            if (company_id == null || company_id == '0' || company_id == '') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (department_id == null || department_id == '0') {
                errormsg = errormsg + "Please select department name !! <br/>";
                iserror = true;
            }
            if (!$("input[name='chkstatus_sub_dept']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'sub_department_id': sub_department_id,
                'sub_department_name': sub_department_name,
                'sub_department_code': sub_department_code,
                'company_id': company_id,
                'department_id': department_id,
                'is_active': is_active
            };

            $("btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_SubDepartmentMaster';
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
                //added 
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        // alert(Msg);
                        $("#txtsubdeptname_").val('');
                        $('#txtsubdeptcode_').val('');
                        $('#ddl_department').val('0');
                        $('input:radio[name=chkstatus_sub_dept]:checked').prop('checked', false);
                        $("btn_sub_dept_update").text('Update').attr("disabled", false);
                        $("#hdnid").val('');
                        Get_sub_Dept_Data(company_id);
                        $('#btn_sub_dept_update').hide();
                        $('#btn_sub_dept_save').show();
                        messageBox("success", Msg);

                        // location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("btnupdate").text('Update').attr("disabled", false);
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
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);
                }

            });
        });

    }, 2000);// end timeout

});



//--------bind data in jquery data table
function GetData() {
    //// debugger;;

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_DepartmentMaster/0';
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            ////// debugger;;;

            $("#tbldepartment").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [4],
                            render: function (data, type, row) {
                                return row.employee_id == null || row.employee_id == 0 ? '-' : row.emp_head_name_code;
                            }
                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "SNo.", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "title": "Company Name", "autoWidth": true },
                    { "data": "departmentname", "name": "departmentname", "title": "Department Name", "autoWidth": true },
                    { "data": "shortname", "name": "shortname", "title": "Department Code", "autoWidth": true },
                    { "data": "emp_head_name_code", "name": "emp_head_name_code", "title": "Department Head Code", "autoWidth": true },

                    //{ "data": "deptheadname", "name": "deptheadname", "autoWidth": true },

                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "title": "Created On", "autoWidth": true },

                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.departmentid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            $('#loader').hide();
            messageBox("error", error.responseText);

        }
    });

}

function GetDataByCompanyId(company_id) {
    //// debugger;;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_DepartmentMasterByCompanyId/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            ////// debugger;;;
            $('#loader').hide();
            $("#tbldepartment").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [4],
                            render: function (data, type, row) {
                                return row.employee_id == null || row.employee_id == 0 ? '-' : row.emp_head_name_code;
                            }
                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "SNo.", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "title": "Company Name", "autoWidth": true },
                    { "data": "departmentname", "name": "departmentname", "title": "Department Name", "autoWidth": true },
                    { "data": "shortname", "name": "shortname", "title": "Department Code", "autoWidth": true },
                    { "data": "emp_head_name_code", "name": "emp_head_name_code", "title": "Department Head Code", "autoWidth": true },


                    //{ "data": "deptheadname", "name": "deptheadname", "autoWidth": true },

                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true, "title": "Created On" },

                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.departmentid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            // alert(error);
        }
    });

}
function GetEditData(id) {
    //// debugger;;
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }



    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_DepartmentMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            data = res;

            $("#txtdepartment").val(data.department_name);
            $('#txtshortname').val(data.department_short_name);
            BindCompanyListAll('ddlcompany', login_emp_id, data.company_id);
            // BindAllEmp_Company('ddlcompany', login_emp_id, data.company_id);

            BindEmployeeCodeFromEmpMasterByComp('ddlheadcode', data.company_id, data.employee_id);

            // BindEmployeeList('ddlheadcode', 0);
            // $('#ddlheadcode', data.employee_id);
            //$('#txtheadname').val(data.department_head_employee_name);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);

            $("#hdnid").val(id);
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





function Get_sub_Dept_Data(companyidd) {

    var apiurl = "";

    if (companyidd == 0) {
        apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentMaster/0';
    }
    else {
        apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentMasterByCompID/' + companyidd;
    }
    //    



    //if (HaveDisplay == 0) {

    //}
    //else {
    //  
    // }

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblsubdepartment").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "SNo.", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "title": "Company", "autoWidth": true },
                    { "data": "departmentname", "name": "departmentname", "title": "Department", "autoWidth": true },
                    { "data": "subdeptname", "name": "subdeptname", "title": "Sub Department Name", "autoWidth": true },
                    { "data": "subdeptcode", "name": "subdeptcode", "title": "Sub Department Code", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "title": "Created On", "autoWidth": true },

                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="Get_sub_dept_EditData(' + full.subdeptid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
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


function Get_sub_dept_EditData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtsubdeptname_").val(data.sub_department_name);
            $('#txtsubdeptcode_').val(data.sub_department_code);
            //BindCompanyList('ddlcompany', data.company_id);
            //BindAllEmp_Company('ddlcompany', login_emp_id, data.company_id);
            BindCompanyListAll('ddlcompany', login_emp_id, data.company_id);
            //if (HaveDisplay == "0") {

            //    $('#ddlcompany').prop("disabled", "disabled");
            //}

            BindDepartmentList('ddl_department', data.company_id, data.department_id);
            $("input[name=chkstatus_sub_dept][value=" + data.is_active + "]").prop('checked', true);

            $("#hdnid").val(id);
            $('#btn_sub_dept_update').show();
            $('#btn_sub_dept_save').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText)
        }
    });
}




//sub department end

