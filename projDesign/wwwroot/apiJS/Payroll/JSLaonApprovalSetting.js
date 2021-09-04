$('#loader').show();

var emp_role_id;
var company_idd;
var emp_idd;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //BindAllEmp_Company('ddlCompany', emp_idd, company_idd);
        BindCompanyListAll('ddlCompany', emp_idd, -1);
        setSelect('ddlCompany', company_idd)
        Bind_OnlyManagers('ddlempcode', company_idd, 0);







        //BindRoleMaster('ddlrole', 0);

        BindOnlyUserRole('ddlrole', 0);
        BindApproverType('ddlapprovertype', 0);
        GetData(company_idd);
        $('#btnsave').show();
        $('#Action').hide();

        $("#txteffectivedt").datepicker({
            dateFormat: 'mm/dd/yy',
            //minDate: 0,

        });

        $('#loader').hide();


        $('#ddlCompany').bind("change", function () {
            //if ($(this).val() == "0" || $(this).val() == null || $(this).val() == "") {
            //    messageBox("error", "Please select company");
            //    return false;
            //}



            // BindEmployeeCodeFromEmpMasterByComp('ddlempcode', $(this).val(), 0);

            Bind_OnlyManagers('ddlempcode', $(this).val(), 0);
            GetData($(this).val());
        });

        $('#btnreset').bind('click', function () {
            // window.location.href = '/Payroll/LoanApprovalSettingMaster';
            location.reload();

        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            // debugger;
            var company = $("#ddlCompany").val();
            var emp = $('#ddlempcode').val();
            var order = $('#ddlorder').val();
            var is_finalapprover = "";

            var approver_role_id = $("#ddlrole").val();
            var approver_type = $("#ddlapprovertype").val();
            // var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '0') {
                    is_finalapprover = 0;
                }
                else if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_finalapprover = 1;
                }
            }




            //validation part
            if (company == '0' || company == null || company == "") {
                errormsg = errormsg + "Please select Company !! </br>";
                iserror = true;
            }

            if ((approver_role_id == "0" || approver_role_id == null || approver_role_id == "") && (emp == "0" || emp == null)) {
                errormsg = errormsg + 'Please select either  Employee or Approver Role  !! </br>';
                iserror = true;
            }
            //if (emp == '0' || emp == null) {
            //    errormsg = errormsg + 'Please select Employee !! <br />';
            //    iserror = true;
            //}
            if ((emp == "0" || emp == null) && (approver_role_id == "0" || approver_role_id == null)) {
                messageBox("error", "Please select employee or approver role </br>");
                $('#loader').hide();
                return false;
            }

            if (emp != "0" && emp != null) {
                if (approver_role_id != "0" && approver_role_id != null && approver_role_id != "") {
                    //errormsg = errormsg + 'Please Only one either approver role or employee  !! <br />';
                    //iserror = true;

                    messageBox("error", "Please Select Only one either approver role or employee");
                    $('#loader').hide();
                    return false;
                }
            }

            if (approver_role_id != "0" && approver_role_id != null && approver_role_id != "") {
                if (emp != "0" && emp != null) {
                    //errormsg = errormsg + 'Please Select Only one either approver role or employee  !! <br />';
                    // iserror = true;
                    messageBox("error", "Please Select Only one either approver role or employee");
                    $('#loader').hide();
                    return false;
                }
            }

            if (approver_type == "0" || approver_type == null) {
                errormsg = errormsg + 'Please Select Approver Type  !! <br />';
                iserror = true;
            }

            if (order == '0' || order == null) {
                errormsg = errormsg + 'Please select Approval Order !! <br />';
                iserror = true;
            }

            if (!$("input[name='chkstatus']:checked").val()) {
                errormsg = errormsg + "Please select Is Final Approval status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            if ((emp != "0" && emp != null) && (approver_role_id == "" || approver_role_id == "0" || approver_role_id == null)) {

                approver_role_id = null;
            }
            //if (approver_role_id != "0") {
            //    empid = 0;
            //}

            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var myData = {

                'company_id': company,
                'emp_id': emp,
                'order': order,
                'is_final_approver': is_finalapprover,
                'created_by': login_empid,
                'approver_role_id': approver_role_id,
                'approver_type': approver_type
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_LoanApprovalSetting';
            var Obj = JSON.stringify(myData);
            //alert(Obj)
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
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });
        });



    }, 2000);// end timeout


});

function GetData(companyidd) {

    if ($.fn.DataTable.isDataTable('#tblloanApproval')) {
        $('#tblloanApproval').DataTable().clear().draw();
    }

    // debugger;
    var apiurl = "";

    if (companyidd > 0) {
        apiurl = localStorage.getItem("ApiUrl") + '/apiPayroll/Get_LoanApprovalSettingByCompID/0/' + company_idd;
    }
    else {
        apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanApprovalSetting/0';
    }
    //var HaveDisplay = ISDisplayMenu("Display Company List");
    //if (HaveDisplay == 0) {
    //    
    //}
    //else {

    //}

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var response = res;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }
            // debugger;
            $("#tblloanApproval").DataTable({
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
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? "Approver 1" : data == '2' ? "Approver 2" : data == '3' ? "Approver 3" : data == '4' ? "Approver 4" : data == '5' ? "Approver 5" : "";
                            }
                        },

                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? "Loan" : data == '2' ? "Assets" : "";
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    //{ "data": "approver_emp_name_code", "name": "approver_emp_name_code", "title": "Approver Name", "autoWidth": true },
                    {
                        "title": "Approver Name", "autoWidth": true,
                        "render": function (data, type, row) {
                            return row.emp_id == 0 || row.emp_id == "" ? '<label></label>' : '<label>' + row.approver_emp_name_code + '</label>'
                        }
                    },
                    { "data": "order", "name": "order", "title": "Approver Order", "autoWidth": true },
                    { "data": "role_name", "name": "role_name", "title": "Approver Role", "autoWidth": true },
                    { "data": "approver_type", "name": "approver_type", "title": "Approver Type", "autoWidth": true },
                    {
                        "title": "Final Approver", "autoWidth": true,
                        render: function (data, type, row) {
                            if (row.is_final_approver == "1") {
                                return '<input type=\"checkbox\" value="' + row.is_final_approver + '" checked="checked" name=final_approve class=call-checkbox>';
                            }
                            else {
                                return '<input type=\"checkbox\" value="' + row.is_final_approver + '" name=final_approve class=call-checkbox>';
                            }
                        }
                    },
                    {
                        "title": "Is_Active", "autoWidth": true,
                        render: function (data, type, row) {
                            if (row.is_active == "1") {
                                return '<input type=\"checkbox\" value="' + row.is_active + '" checked="checked" name=abc class=call-checkbox>';
                            } else {
                                return '<input type=\"checkbox\" value="' + row.is_active + '" name=abc class=call-checkbox>';
                            }
                        }
                    },
                    {
                        "title": "Action", "autoWidth": true,
                        render: function (data, type, row) {

                            return '<input type="hidden" id="snoo" value="' + row.sno + '" /><input type="hidden" id="hdncompanyid" value="' + row.company_id + '"/><input type="hidden" id="hdnapprovertype" value="' + row.approver_type + '"> <a href="#" class="btnSaveE"><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],

                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }
                //"lengthMenu": [[5, 10, 50, 1], [5, 10, 50, "All"]]

            });

            $(document).on("click", ".btnSaveE", function () {

                // var row_idd = $(this).parents('tr').children('td').children('input[id="snoo"]').val();
                // var company_id = $(this).parents('tr').children('td').children('input[id="hdncompanyid"]').val();
                var approver_typee = $(this).parents('tr').children('td').children('input[id="hdnapprovertype"]').val();
                //// alert(company_id);
                //// var row_idd = $(this).parents('tr').children('td').children('input[type="hidden"]').val();
                ////alert(row_idd);
                var checkedvaluee = $(this).parents('tr').children('td').children('input[name=abc]:checked');

                var _checkfinal_approver = $(this).parents('tr').children('td').children('input[name=final_approve]:checked');

                //alert(checkedvaluee.length);
                if (checkedvaluee.length > 0) {
                    $("#hdnchkvaluee").val(checkedvaluee.length); // check active deactive status
                }
                else {
                    $("#hdnchkvaluee").val(0);
                }


                if (_checkfinal_approver.length > 0) {
                    $("#checkfinalapprover").val(_checkfinal_approver.length);//checked final approver
                }
                else {
                    $("#checkfinalapprover").val(0);
                }

                // $("#hdncompanyid").val(company_id);

                $("#hdnapprovertype").val(approver_typee);

                var currentRow = $(this).closest("tr");

                var data = $('#tblloanApproval').DataTable().row(currentRow).data();

                //var empidd = data['emp_id'];

                //var approverorder = data['order'];

                //var is_active = data['is_active'];
                //var is_final_approve = data['is_final_approver'];
                //var company_id = data['company_id'];
                //var approver_typee = data['approver_type']


                //alert(empidd);
                UpdateValue(JSON.stringify(data));

            });

            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            //console.log("error");
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}

function UpdateValue(data) {
    //alert(data);

    var data1 = JSON.parse(data);
    var active_ = $("#hdnchkvaluee").val(); //data1['is_active']; 
    var _final_approver_check = $("#checkfinalapprover").val(); //data1['is_final_approver'];
    var approvertypee = data1['approver_type'];
    var id = data1['sno'];
    $('#loader').show();
    var errormsg = '';
    var iserror = false;
    if (confirm("Are you sure you want to Update Status ?")) {

        // var id = data;
        // var company_id = $("#hdncompanyid").val();
        //$("#hdnchkvaluee").val(); // active deactive status
        // var approvertypee = $("#hdnapprovertype").val();

        //= $("#checkfinalapprover").val(); //checked unchecked final approver
        //alert(selectedVal);

        if (active_ == null) {
            errormsg = errormsg + 'Please check Status !! <br />';
            iserror = true;
        }

        if (_final_approver_check == null) {
            errormsg = errormsg + 'Please Select Final Approver !! <br />';
            iserror = true;
        }

        if (iserror) {
            messageBox("error", errormsg);
            $('#loader').hide();
            return false;
        }
        var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        var myData = {

            Sno: id,
            company_id: data1['company_id'],
            emp_id: data1['emp_id'],
            is_final_approver: _final_approver_check,
            is_active: active_,
            last_modified_by: login_empid,
            approver_type: approvertypee,
            order: data1['order'],
        };
        var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_LoanApprovalSetting';
        var Obj = JSON.stringify(myData);

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        // alert(Obj);
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
                _GUID_New();
                if (statuscode == "0") {
                    alert(Msg);
                    location.reload();
                }
                else if (statuscode == "1" || statuscode == '2') {
                    messageBox("error", Msg);
                }
            },
            error: function (err) {
                $('#loader').hide();
                _GUID_New();
                messageBox("error", err.responseText);
            }
        });


    }
    else {
        GetData(company_idd);
        $('#loader').hide();
        return false;
    }
}


function Bind_OnlyManagers(ControlId, company_id, SelectedVal) {

    $("#loader").show();
    ControlId = "#" + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + '/apiMasters/Get_OnlyManagersEmpCode/' + company_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.emp_mgr_id).html(value.manager_name_code));
            });

            if (SelectedVal != "" && SelectedVal != null && SelectedVal != undefined) {
                $(ControlId).val(SelectedVal);
            }

            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });

}