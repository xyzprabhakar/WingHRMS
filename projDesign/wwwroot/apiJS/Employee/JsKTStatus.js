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
        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
       // BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
       // setSelect('ddlEmployeeCode', login_emp_id);
        Bind_Task_List('ddl_task_list', login_emp_id);

        GetData(login_emp_id);

        $('#btnreset').bind('click', function () {
            location.reload();
        });
        $('#ddlcompany').bind("change", function () {
             
            $("#ddlEmployeeCode").empty();
            
            BindEmployees_handoverby('ddlEmployeeCode', $(this).val(), login_emp_id, 0);
           
        });
        $('#ddlEmployeeCode').bind("change", function () {
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            Bind_Task_List('ddl_task_list', ddlEmployeeCode);
            GetData(ddlEmployeeCode);
        });
        $('#btnsave').bind("click", function () {
            $('#loader').show();

            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddl_task_list = $("#ddl_task_list").val();
            var txtTaskStatus = $("#txtTaskStatus").val();

            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (ddl_task_list == '0') {
                messageBox("error", "Please Select Task...! ");
                $('#loader').hide();
                return;
            }

            if (txtTaskStatus == null || txtTaskStatus == '') {
                messageBox("error", "Please Enter Task Status...! ");
                $('#loader').hide();
                return;
            }

            var myData = {
                'Kt_Master_id': ddl_task_list,
                'Status': txtTaskStatus,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Save_KT_Task_Status';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            console.log(Obj);
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
                        $("#txtTaskStatus").val("");
                       // BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        //setSelect('ddlEmployeeCode', login_emp_id);
                        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
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

        $("#btnupdate").bind("click", function () {
            // debugger;
            $('#loader').show();
            var k_id = $("#hdnid").val();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddl_task_list = $("#ddl_task_list").val();
            var txtTaskStatus = $("#txtTaskStatus").val();

            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            if (ddl_task_list == null || ddl_task_list == '') {
                messageBox("error", "Please Select Task...! ");
                $('#loader').hide();
                return;
            }

            if (txtTaskStatus == null || txtTaskStatus == '') {
                messageBox("error", "Please Enter Task Status...! ");
                $('#loader').hide();
                return;
            }

            var myData = {
                'id': k_id,
                'Kt_Master_id': ddl_task_list,
                'Status': txtTaskStatus,
                'last_modified_by': login_emp_id,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Update_KT_Task_Status';
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
                        $("#txtTaskStatus").val("");
                        BindEmployees_handoverby('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        GetData(login_emp_id);
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
function GetData(emp_id) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_Status_List/' + emp_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblKTTaskStatus").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "task_name", "name": "task_name", "title": "Task Name", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Status Created Date", "autoWidth": true },
                    { "data": "updated_date", "name": "updated_date", "title": "Status Updated Date", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.sid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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
function GetEditData(id) {
    debugger;
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_Status_ById/' + id;

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
            $("#txtTaskStatus").val(data.status);
            $("#ddl_task_list").val(data.taskid);
            
            BindAllEmp_Company_hlday('ddlcompany', login_emp_id, data.company_id);
           
            BindEmployees_handoverby('ddlEmployeeCode', data.company_id, login_emp_id, data.handoverby);
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

function Bind_Task_List(ControlId, LoginEmpID) {
    var listapi = localStorage.getItem("ApiUrl");

    ControlId = '#' + ControlId;
    $('#loader').show();
    $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');

    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiEmployee/Get_KT_Task_List/" + LoginEmpID,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            emp_lst_under_login_emp = res;
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });

    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {

      //  var selected_data = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

        $.each(emp_lst_under_login_emp, function (data, value) {

            $(ControlId).append($("<option></option>").val(value.id).html(value.task_name));
        });

    }
   
}

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}

$('.number').keypress(function (event) {

    var $this = $(this);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
        ((event.which < 48 || event.which > 57) &&
            (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

$('.number').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});