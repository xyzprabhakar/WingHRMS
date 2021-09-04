var default_company;
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //  var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        GetData(default_company);
        BindDepartmentList('ddldepartment', default_company, -1);


        // debugger;
        // GetData(default_company);


        $('#ddlcompany').bind("change", function () {
            $('#loader').show();
            $('#working_role_name').val('');
            BindDepartmentList('ddldepartment', $(this).val(), -1);

            $('input:radio[name=chkstatus]:checked').prop('checked', false);
            $('input:checkbox[name=is_defaultwrkrole]:checked').prop('checked', false);
            $('#loader').hide();
            if ($(this).val() == "0") {
                if ($.fn.DataTable.isDataTable('#tblfinancial')) {
                    $('#tblfinancial').DataTable().clear().draw();
                }
            }
            else {
                GetData($(this).val());
            }
        });


        $('#btnupdate').hide();
        $('#btnsave').show();



        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {

            }
        });

        $('#btnreset').bind('click', function () {
            $('#working_role_name').val('');
            BindAllEmp_Company('ddlcompany', login_emp, default_company);
            GetData(default_company);
            BindDepartmentList('ddldepartment', default_company, -1);
            $('input:radio[name=chkstatus]:checked').prop('checked', false);
            $('input:checkbox[name=is_defaultwrkrole]:checked').prop('checked', false);
            $('#btnupdate').hide();
            $('#btnsave').show();
        });
        $('#btnsave').bind("click", function () {
            // debugger;  $('#loader').show();
            var company_id = $("#ddlcompany").val();
            var ddldepartment = $("#ddldepartment").val();
            var txt_working_role_name = $("#working_role_name").val();
            var is_default = $("#is_default").prop("checked") == true ? 1 : 0;


            var is_active = 0;
            var errormsg = '';
            var iserror = false;


            if (company_id == null || company_id == '' || company_id == "0") {
                errormsg = "Please Select Company <br/>";
                iserror = true;
            }

            if (ddldepartment == null || ddldepartment == '') {
                errormsg = "Please Select Department <br/>";
                iserror = true;
            }

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            //validation part
            if (txt_working_role_name == null || txt_working_role_name == '') {
                errormsg = "Please Enter Working Role Name <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);

                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {
                'working_role_name': txt_working_role_name,
                'dept_id': ddldepartment,
                'is_active': is_active,
                'company_id': company_id,
                'created_by': login_emp,
                'is_default': is_default,
            };
            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_EpaWorkingRoleMaster';
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

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $('#working_role_name').val('');
                        BindDepartmentList('ddldepartment', default_company, -1);
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('input:checkbox[name=is_defaultwrkrole]:checked').prop('checked', false);
                        BindDepartmentList('ddldepartment', default_company, -1);
                        GetData(default_company);
                        messageBox("success", Msg);
                        return false;
                    }
                    else {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            });
        });

        $("#btnupdate").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var txt_working_role_name = $("#working_role_name").val();
            var working_role_id = $("#hdnid").val();
            var ddldepartment = $("#ddldepartment").val();
            var is_default = $("#is_default").prop("checked") == true ? 1 : 0;
            var is_active = 0;
            var errormsg = '';
            var iserror = false;


            if (company_id == null || company_id == '' || company_id == "0") {
                errormsg = "Please Select Company <br/>";
                iserror = true;
            }


            if (ddldepartment == null || ddldepartment == '') {
                errormsg = "Please Select Department <br/>";
                iserror = true;
            }


            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }
            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            //validation part
            if (txt_working_role_name == null || txt_working_role_name == '') {
                errormsg = "Please Working Role Name <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {
                'working_role_id': working_role_id,
                'working_role_name': txt_working_role_name,
                'dept_id': ddldepartment,
                'is_active': is_active,
                'company_id': company_id,
                'created_by': login_emp,
                'is_default': is_default,
            };
            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_EpaWorkingRoleMaster';
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

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#hdnid").val('');
                        $('#working_role_name').val('');
                        BindDepartmentList('ddldepartment', default_company, -1);
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $('input:checkbox[name=is_defaultwrkrole]:checked').prop('checked', false);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        GetData(default_company);
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                }
            });
        });


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData(company_id) {
    // debugger;
    $('#loader').show();

    if ($.fn.DataTable.isDataTable('#tblfinancial')) {
        $('#tblfinancial').DataTable().clear().draw();
    }
    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_EpaWorkingRoleMaster/0/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblfinancial").DataTable({
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
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return new Date(row.last_modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                    ],

                "columns": [
                    { "data": null },
                    { "data": "working_role_name", "name": "working_role_name", "autoWidth": true },
                    { "data": "dept_name", "name": "dept_name", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true },
                    {
                        "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<input type=\"checkbox\" value="' + full.is_default + '" ' + (full.is_default == 1 ? 'checked' : '') + ' name=is_default class=call-checkbox disabled=true>';
                        }
                    },
                    { "data": "created_date", "name": "created_date", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.working_role_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },

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
            //alert(error);
            console.log("error");
        }
    });

}

function GetEditData(id) {

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }
    if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null || $("#ddlcompany").val() == "0") {
        messageBox("error", "Please select company");
        return false;
    }
    var company_id = $("#ddlcompany").val();
    $('#loader').show();
    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_EpaWorkingRoleMaster/' + id + '/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#working_role_name").val(data.working_role_name);
            BindDepartmentList('ddldepartment', company_id, data.dept_id);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);
            $("#hdnid").val(id);
            $("#is_default").prop("checked", (res.is_default == 1 ? true : false));

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

//-------update city data



function default_working_role_info() {
    messageBox("info", "In case of working role not assign to any employee for any reason, than that employee will automatically lies in this working role.<br/><br/>But only one working role is to be set as default working role.");
    return false;
}