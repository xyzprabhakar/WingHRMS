
var login_emp_id;
var default_company;
var emp_role_id;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();
        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        //BindCompanyList('ddlcompany', 0);

        BindParentMenu('ddlparentmenu', 0);

        $("#btnupdate").hide();



        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {

            var iserror = false;
            var errormsg = '';
            var sort_order = "";
            if ($('#txtmenu').val() == '') {
                errormsg = errormsg + 'Please Enter Menu Name<br/>';
                iserror = true;
            }

            //if ($("#txturl").val() == '') {
            //    errormsg = errormsg + 'Please Menu Url/Path<br/>';
            //    iserror = true;
            //}


            if ($('#txtIconUrl').val() == '') {
                errormsg = errormsg + 'Please Enter Icon Url<br/>';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            if ($('#txtsortingorder').val() != '') {
                sort_order = $('#txtsortingorder').val();
            }
            else {
                sort_order = 0;
            }
            var mydata = {
                // companyid: $('#ddlcompany').val(),
                menu_name: $('#txtmenu').val(),
                parent_menu_id: $('#ddlparentmenu').val(),
                IconUrl: $('#txtIconUrl').val(),
                urll: $('#txturl').val(),
                SortingOrder: sort_order,
                created_by: login_emp_id
            }

            $('#loader').show();
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Save_MenuMaster',
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (data) {
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
        });


        $("#btnupdate").bind("click", function () {

            var iserror = false;
            var errormsg = '';
            var sort_order = "";
            if ($('#txtmenu').val() == '') {
                errormsg = errormsg + 'Please Enter Menu Name<br/>';
                iserror = true;
            }

            //if ($("#txturl").val() == '') {
            //    errormsg = errormsg + 'Please Menu Url/Path<br/>';
            //    iserror = true;
            //}


            if ($('#txtIconUrl').val() == '') {
                errormsg = errormsg + 'Please Enter Icon Url<br/>';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            if ($('#txtsortingorder').val() != '') {
                sort_order = $('#txtsortingorder').val();
            }
            else {
                sort_order = 0;
            }

            var mydata = {
                menu_id: $('#hdnmenuid').val(),
                menu_name: $('#txtmenu').val(),
                parent_menu_id: $('#ddlparentmenu').val(),
                IconUrl: $('#txtIconUrl').val(),
                urll: $('#txturl').val(),
                SortingOrder: sort_order,
                modified_by: login_emp_id
            }
            $('#loader').show();
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Edit_MenuMaster',
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (data) {
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
        });

    }, 2000);// end timeout

});



function GetData() {
    // debugger;
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_MenuMaster/0',
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            $("#tblmenumasterdetail").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
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
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": "s_no", "name": "s_no", "autoWidth": true },
                    { "data": "menu_name", "name": "menu_name", "autoWidth": true },
                    { "data": "iconUrl", "name": "iconUrl", "autoWidth": true },
                    { "data": "urll", "name": "urll", "autoWidth": true },
                    { "data": "parent_menu_name", "name": "parent_menu_name", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "autoWidth": true },
                    { "data": "sortingOrder", "name": "sortingOrder", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.menu_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}

function GetEditData(menu_idd) {
    $('#loader').show();
    // debugger;
    if (menu_idd == null || menu_idd == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_MenuMaster/' + menu_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $("#txtmenu").val(response.menu_name);
            $("#txturl").val(response.urll);
            $("#ddlparentmenu").val(response.parent_menu_id);
            $("#txtIconUrl").val(response.iconUrl);
            $("#txtsortingorder").val(response.sortingOrder);
            $("#hdnmenuid").val(menu_idd);

            $("#btnsave").hide();
            $("#btnupdate").show();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    })
}