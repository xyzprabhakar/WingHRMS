
var login_emp;
var company_id;

$(document).ready(function () {
    setTimeout(function () {
        

        // debugger;

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        $('#btnupdate').hide();
        $('#btnsave').show();

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp, 0);
        setSelect('ddlcompany', company_id)

        //var HaveDisplay = ISDisplayMenu("Display Company List");

        //if (HaveDisplay == 0) {
        //    BindOnlyUser('ddlrole', 0);
        //}
        //else {

        BindOnlyUserRole('ddlrole', 0);
        //}

        GetData(0, company_id);


        $('#btnreset').bind('click', function () {
            //$("#ddlrole").val('');
            location.reload();
        });

        $("#ddlcompany").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tbluserlist')) {
                $('#tbluserlist').DataTable().clear().draw();
            }
            if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            else {
                GetData($("#ddlrole").val(), $(this).val());
            }
        });

        $("#ddlrole").bind('change', function () {
            var roleid = $("#ddlrole").val();
            GetData(roleid, $("#ddlcompany").val());
        });

        $('#btnsave').bind("click", function () {

            // debugger;
            var EmpIds = [];
            var table = $("#tbluserlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    EmpIds.push(no);
                }
            });
            //selectedIds.forEach(function (selectedId) {
            //    alert(selectedId);
            //});

            var roleid = $("#ddlrole").val();
            var errormsg = '';
            var iserror = false;

            if (roleid == '' || roleid == '0') {
                errormsg = errormsg + 'Please select role !! <br />';
                iserror = true;
            }
            if (EmpIds.length <= 0) {
                errormsg = errormsg + 'Please select Employee ID !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var loginn_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var myData = {

                'userid': EmpIds,
                'roleid': roleid,
                'created_by': loginn_empid
            };

            $('#loader').show();
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/SaveUserRoleMapping';
            var obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                data: obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    // debugger;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        var roleid = $("#ddlrole").val();
                        GetData(roleid, $("#ddlcompany").val());
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
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });


        });
        //BindRoleMaster('ddlrole', 0);

    }, 2000);// end timeout

});


//--------bind data in jquery data table
function GetData(roleid, compid) {
    $('#loader').show();
    // debugger;
    var apiurl = "";
    //if (emp_role_id == "2")//For Admin
    //{
    apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetEmployeeUserListBycompID/' + compid + "/" + roleid;
    //}

    //else {
    //   apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetEmployeeUserList/' + roleid;
    //}


    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            // debugger;
            $("#tbluserlist").DataTable({
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
                            targets: 0,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        }
                    ],

                "columns": [
                    //{
                    //    "render": function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},

                    {
                        "render": function (data, type, full, meta) {
                            if (full.ischecked == true) {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked id="chk' + full.userid + '" value="' + full.userid + '" />';
                            }
                            else {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.userid + '" value="' + full.userid + '" />';
                            }
                        }
                    },
                    { "data": "empcode", "name": "empcode", "autoWidth": true },
                    { "data": "empname", "name": "empname", "autoWidth": true },
                    { "data": "empdept", "name": "empdept", "autoWidth": true },
                    { "data": "empdesig", "name": "empdesig", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, 'All']]
                //"paging": false,
                //"ordering": false,
                //"info": false

            });
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}


function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tbluserlist").find(".chkRow");
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
    var chkRows = $("#tbluserlist").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}



function BindOnlyUser(ControlId, SelectedVal) {
    ControlId = "#" + ControlId;
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetRoleUser",
        type: "GET",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer' + localStorage.getItem('Token') },
        data: "{}",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option value=0>--Please Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.role_id).html(value.role_name));

            });

            //get and set value

            if (SelectedVal != "0" || SelectedVal != "" || SelectedVal != "undefined") {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            messageBox(err.responseText);
            $('#loader').hide();
        }
    });
}

