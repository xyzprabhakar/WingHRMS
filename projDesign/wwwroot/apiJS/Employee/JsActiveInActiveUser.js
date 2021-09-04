$('#loader').show();
var login_emp_id;
var login_comp_id;
var emp_role_id;

$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_comp_id);
        GetData(login_comp_id);


        $('#ddlcompany').bind("change", function () {
            $('#loader').show();
            GetData($(this).val());
            $('#loader').hide();
            //BindInactiveUsers('ddlemployee', $(this).val(), 0);
        });


        $('#loader').hide();


    }, 2000);// end timeout

});



function GetData(company_id) {

    if ($.fn.DataTable.isDataTable('#tblempdetail')) {
        $('#tblempdetail').DataTable().clear().draw();
    }

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/GetAllEmployeeByCompany/" + company_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblempdetail").DataTable({
                "processing": true,//for show progress bar
                "serverSide": false,//for process server side
                "bDestroy": true,//for remove previous data
                "filter": true,//this is for disable filter (search box)
                "orderMulti": false,//for disable multiple column at once
                "scrollX": 200,
                "scrollY": 500,
                "aaData": res,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "username", "name": "username", "title": "Login User ID", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    {
                        "title": "Is Active", "autoWidth": true,
                        render: function (data, type, row) {
                            if (row.um_is_active == "1") {
                                return '<input type=\"checkbox\" value="' + row.um_is_active + '" checked="checked" name=useractive class=call-checkbox>';
                            }
                            else {
                                return '<input type=\"checkbox\" value="' + row.is_final_approver + '" name=useractive class=call-checkbox>';
                            }
                        }
                    },
                    {
                        "title": "Is Logged Block", "autoWidth": true,
                        render: function (data, type, row) {
                            if (row.um_is_logged_blocked == "1") {
                                return '<input type=\"checkbox\" value="' + row.um_is_logged_blocked + '" checked="checked" name=userloginblocked class=call-checkbox>';
                            } else {
                                return '<input type=\"checkbox\" value="' + row.um_is_logged_blocked + '" name=userloginblocked class=call-checkbox>';
                            }
                        }
                    },
                    {
                        "title": "Save", "autoWidth": true,
                        render: function (data, type, row) {

                            return '<a href="#" class="btnSaveE"><i class="fas fa-save"></i></a>';
                        }
                    },
                    {
                        "title": "Reset Password", "autoWidth": true,
                        render: function (data, type, row) {

                            return '<a href="#" class="btnresetpwd"><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },
                    { "data": "employee_id", "name": "employee_id", "visible": false, "autoWidth": true },
                    { "data": "um_user_id", "autoWidth": true, "visible": false },
                    { "data": "username", "autoWidth": true, "visible": false }

                ],

                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[50, -1], [50, "All"]]
            });



            $(document).on("click", ".btnSaveE", function () {

                var currentRow = $(this).closest("tr");

                var data = $('#tblempdetail').DataTable().row(currentRow).data();

                var user_id = data["um_user_id"]; //$(this).parents('tr').children('td').children('input[id="hdnuser_id"]').val();
                var user_name = data["username"]; //$(this).parents('tr').children('td').children('input[id="hdnusername"]').val();
                var checkedactivestatus = $(this).parents('tr').children('td').children('input[name=useractive]:checked');
                var _checkloginblockedstatus = $(this).parents('tr').children('td').children('input[name=userloginblocked]:checked');
                var empid = data["employee_id"];//$(this).parents('tr').children('td').html();


                if (checkedactivestatus.length > 0) {
                    $("#hdnchkactivestatus").val(checkedactivestatus.length); // check active deactive status
                }
                else {
                    $("#hdnchkactivestatus").val(0);
                }


                if (_checkloginblockedstatus.length > 0) {
                    $("#hdnchkloginstatus").val(_checkloginblockedstatus.length);//checked final approver
                }
                else {
                    $("#hdnchkloginstatus").val(0);
                }

                UpdateStatus(user_id, user_name, empid);

            });


            $(document).on("click", ".btnresetpwd", function () {
                var currentRow = $(this).closest("tr");

                var data = $('#tblempdetail').DataTable().row(currentRow).data();

                var user_id = data["um_user_id"];  //$(this).parents('tr').children('td').children('input[id="hdnuser_id"]').val();
                var user_name = data["username"];// $(this).parents('tr').children('td').children('input[id="hdnusername"]').val();


                ResetPasswordByAdmin(user_id, user_name);
            });

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error", err.responseText);
        }
    });
}


function UpdateStatus(user_id, user_name, empid) {

    var myData = {
        user_id: user_id,
        is_active: $("#hdnchkactivestatus").val(),
        is_logged_blocked: $("#hdnchkloginstatus").val(),
        created_by: login_emp_id,
        username: user_name,
        employee_id: empid,
    }


    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/UnBlock_EmployeeLogin';
    var Obj = JSON.stringify(myData);
    //alert(Obj)
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $('#loader').show();
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
                //location.reload();
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




function ResetPasswordByAdmin(userid, username) {
    var myData = {

        user_id: userid,
        username: username,
        //  password: encryptednewpwd,
        last_modified_by: login_emp_id
    }


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $('#loader').show();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiEmployee/ChangePwdByAdmin',
        type: "POST",
        data: JSON.stringify(myData),
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            //if data save
            if (statuscode == "0") {
                $('#loader').hide();
                $("#txtnewpwd").val('');
                $("#txtconfirmpwd").val('');
                messageBox("success", Msg);
            } //if error in save
            else if (statuscode != "0") {
                messageBox("error", Msg);
                // $('#loader').hide();
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
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);

        }

    });
}



//$(".btnOpenPopUp, $(#btnresett)").on("click", function () {
//    $("#txtnewpwd").val('');
//    $("#txtconfirmpwd").val('');
//});


//$("#btnresett").bind("click", function () {
//    $("#txtnewpwd").val('');
//    $("#txtconfirmpwd").val('');
//});



//$("#btnreset").bind("click", function () {
    //    location.reload();
    //});


    //$("#btnsave").bind("click", function () {
    //    $('#loader').show();

    //        var errormsg = "";
    //        var iserror = false;

    //        var company_id = $("#ddlcompany").val();
    //        var user_emp_id = $("#ddlemployee").val();
    //        var user_id = "";
    //        var emp_id = "";
    //        var statuss = $("input[name='chkstatus']:checked").length;
    //        if (user_emp_id != "0") {
    //            user_id = $("#ddlemployee").val().split('/')[0];
    //            emp_id = $("#ddlemployee").val().split('/')[1];
    //        }

    //        if (company_id == "0") {
    //            errormsg = errormsg + "Please Select Company</br>";
    //            iserror = true;
    //        }

    //        if (user_emp_id == "0") {
    //            errormsg = errormsg + "Please Select Employee</br>";
    //            iserror = true;
    //        }

    //        if (statuss == "0") {
    //            errormsg = errormsg + "Please Select UnBlock</br>";
    //            iserror = true;
    //        }

    //        if (iserror) {
    //            messageBox("error", errormsg);
    //            $('#loader').hide();
    //            return false;
    //        }
    //    if (confirm("Do you want to Unblock it?")) {
    //        var mydata = {
    //            user_id: user_id,
    //            emp_id: emp_id,
    //            created_by: login_emp_id,
    //        }


    //        var headerss = {};
    //        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //        headerss["salt"] = $("#hdnsalt").val();

    //        $.ajax({
    //            url: localStorage.getItem("ApiUrl") + "/apiEmployee/UnBlock_EmployeeLogin",
    //            type: "POST",
    //            contentType: "application/json",
    //            dataType: "json",
    //            data: JSON.stringify(mydata),
    //            headers: headerss,
    //            success: function (data) {
    //                var status = data.statusCode;
    //                var Msg = data.message;

    //                $('#loader').hide();
    //                _GUID_New();

    //                alert(Msg);
    //                location.reload();



    //            },
    //            error: function (err) {
    //                _GUID_New();
    //                messageBox("error", err.responseText);
    //                $('#loader').hide();
    //            }
    //        });
    //    }

    //    $('#loader').hide();
    //});




//function BindInactiveUsers(ControlId, CompanyId, SelectedVal) {
//    $('#loader').show();
//    ControlId = "#" + ControlId;
//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiEmployee/Get_InactiveUser/" + CompanyId+"/"+ SelectedVal,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: {},
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.user_id + "/" + value.employee_id).html(value.emp_user_name));
//            })

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();
//        },
//        error: function (err) {
//            messageBox("error", err.responseText);
//            $('#loader').hide();
//        }
//    });
//}


//function GetData(company_id) {
//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiEmployee/Get_ActiveInactiveUserLog/" + company_id,
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;

//            $("#tblempdetail").DataTable({
//                "processing": true, //for show progress bar
//                "serverSide": false,// for show progress bar
//                "bDestroy": true,
//                "filter": true, // this is for disable filter(search box)
//                "orderMulti":false,// for disable multiple column at once
//                "scrollX": 200,
//                "aaData": res,
//                "columnDefs": [
//                    {
//                        targets: [4],
//                        render: function (data, type, row) {

//                            return data == 0 ? "Blocked" : data == 1 ? "UnBlocked" : "";
//                        }
//                    },
//                    {
//                        targets: [5],
//                        render: function (data, type, row) {

//                            var date = new Date(data);
//                            return GetDateFormatddMMyyyy(date);
//                        }
//                    },
//                    {
//                        targets: [6],
//                        render: function (data, type, row) {

//                            var date = new Date(data);
//                            var asset_issue_dt = GetDateFormatddMMyyyy(date);

//                            return new Date(row.modified_date) < new Date(row.created_on) ? '-' : modified_date;
//                        }
//                    }

//                ],
//                "columns": [
//                    { "data": null, "title": "SNo.", "autoWidth": true },
//                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
//                    { "data": "username", "name": "username", "title": "User ID", "autoWidth": true },
//                    { "data": "emp_user_name", "name": "emp_user_name", "title": "Employee", "autoWidth": true },
//                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
//                    { "data": "created_on", "name": "created_on", "title": "Created On", "autoWidth": true },
//                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },

//                ],
//                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
//                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
//                    return nRow;
//                },
//                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

//            });
//        },
//        error: function (err) {
//            messageBox("error", err.responseText);
//        }
//    })
//}