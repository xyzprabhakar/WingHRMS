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

        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
        setSelect('ddlEmployeeCode', login_emp_id);
        Bind_Task_List('ddl_task_list', login_emp_id);
        GetData();

        $('#btnreset').bind('click', function () {
            location.reload();
        });
        $('#ddlEmployeeCode').bind("change", function () {
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            Bind_Task_List('ddl_task_list', ddlEmployeeCode);
        });
        $('#btnsave').bind("click", function () {
            $('#loader').show();

            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddl_task_list = $("#ddl_task_list").val();
            var uploadfilee = $("#f_kttaskstatus").val();

            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                messageBox("error", "Handover By can not blank...! ");
                $('#loader').hide();
                return;
            }
            debugger;
            if (ddl_task_list == '0') {
                messageBox("error", "Please Select Task...! ");
                $('#loader').hide();
                return;
            }
            if (uploadfilee == "" || uploadfilee == null) {
                messageBox("error", "Please Upload File...! ");
                $('#loader').hide();
                return;
            }
            var files = document.getElementById("f_kttaskstatus").files;
            var mydata = {
                'Kt_Master_id': ddl_task_list,
                'created_by': login_emp_id,
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
                        $("#txtTaskStatus").val("");
                        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        setSelect('ddlEmployeeCode', login_emp_id);
                        GetData();
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
            //var txtTaskStatus = $("#txtTaskStatus").val();

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

            //if (txtTaskStatus == null || txtTaskStatus == '') {
            //    messageBox("error", "Please Enter Task Status...! ");
            //    $('#loader').hide();
            //    return;
            //}

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
                        BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, login_emp_id, 0);
                        setSelect('ddlEmployeeCode', login_emp_id);
                        GetData();
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
    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/Get_KT_Task_Status_List/' + login_emp_id;

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
                    {
                        "title": "Uploaded KT Doc", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            if (full.is_file_upload == 1) {
                                return '<a href="' + localStorage.getItem("ApiUrl") + "/" + full.kt_file + '">Download</a>';
                            }
                            else {
                                return '------';
                            }
                        }
                    },
                    { "data": "created_date", "name": "created_date", "title": "Status Created Date", "autoWidth": true },
                    { "data": "updated_date", "name": "updated_date", "title": "Status Updated Date", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
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
            login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            BindEmployeeListUnderLoginEmpFromAllComp('ddlEmployeeCode', login_company_id, data.handoverby, 0);
            debugger;

            setSelect('ddlEmployeeCode', data.handoverby);

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
