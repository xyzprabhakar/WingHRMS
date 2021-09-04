$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null && company_id == 'undefined') {
            window.location = '/Login';
        }

        var qs = getQueryStrings();
        var company_id = qs["company_id"];


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        //var HaveDisplay = ISDisplayMenu("Display Company List");


        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);

        GetData(default_company);



        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {
            GetData($(this).val());
        });



        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtMenuName = $("#txtMenuName").val();
            var txtUrl = $("#txtUrl").val();
            var txtIconUrl = $("#txtIconUrl").val();
            var txtsorting_order = $("#txtsorting_order").val();


            if (ddlcompany == "0" || ddlcompany == null || ddlcompany == '') {
                messageBox("error", "Please Select Company ...!");
                $("#ddlcompany").val('');
                $('#loader').hide();
                return;
            }

            if (txtMenuName == "" || txtMenuName == null) {
                messageBox("error", "Please Enter Menu Name...!");
                $("#txtEventName").val('');
                $('#loader').hide();
                return;
            }


            if (txtUrl == "" || txtUrl == null) {
                messageBox("error", "Please Enter Url...!");
                $("#txtEventDate").val('');
                $('#loader').hide();
                return;
            }
            if (txtIconUrl == "" || txtIconUrl == null) {
                messageBox("error", "Please Enter Icon Url....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            var is_active = 0;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //var emp_id = localStorage.getItem('emp_id');
            if (txtsorting_order == "" || txtsorting_order == null) {
                txtsorting_order = 0;
            }

            var myData = {
                'company_id': ddlcompany,
                'menu_name': txtMenuName,
                'icon_url': txtIconUrl,
                'url': txtUrl,
                'sorting_order': txtsorting_order,
                'is_active': is_active,
                'created_by': login_emp_id
            };
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Save_Right_Menu_Link',
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

                        alert(Msg);
                        location.reload();
                    }
                    else {
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


        $('#btnupdate').bind("click", function () {
            $('#loader').show();
            var ddlcompany = $("#ddlcompany").val();
            var txtMenuName = $("#txtMenuName").val();
            var txtUrl = $("#txtUrl").val();
            var txtIconUrl = $("#txtIconUrl").val();
            var txtsorting_order = $("#txtsorting_order").val();


            if (ddlcompany == "0" || ddlcompany == null || ddlcompany == '') {
                messageBox("error", "Please Select Company ...!");
                $("#ddlcompany").val('');
                $('#loader').hide();
                return;
            }

            if (txtMenuName == "" || txtMenuName == null) {
                messageBox("error", "Please Enter Menu Name...!");
                $("#txtEventName").val('');
                $('#loader').hide();
                return;
            }


            if (txtUrl == "" || txtUrl == null) {
                messageBox("error", "Please Enter Url...!");
                $("#txtEventDate").val('');
                $('#loader').hide();
                return;
            }
            if (txtIconUrl == "" || txtIconUrl == null) {
                messageBox("error", "Please Enter Icon Url....!");
                $("#txtEventTime").val('');
                $('#loader').hide();
                return;
            }

            var is_active = 0;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            if (txtsorting_order == "" || txtsorting_order == null) {
                txtsorting_order = 0;
            }

            //var emp_id = localStorage.getItem('emp_id');

            var menu_id = $("#hdnid").val();

            var myData = {
                'menu_id': menu_id,
                'company_id': ddlcompany,
                'menu_name': txtMenuName,
                'icon_url': txtIconUrl,
                'url': txtUrl,
                'sorting_order': txtsorting_order,
                'is_active': is_active,
                'created_by': login_emp_id
            };

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Update
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Update_Right_Menu_Link',
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
                        alert(Msg);
                        location.reload();
                    }
                    else {
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


        $('#btnReset').bind("click", function () {
            location.reload();

        });

    }, 2000);// end timeout


});


function GetData(company_id) {

    //var company_id = localStorage.getItem("company_id")

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_Right_Menu_Link/0/' + company_id;
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
            $("#tbl_menu_list").DataTable({
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
                    { "data": null },
                    { "data": "menu_name", "name": "menu_name", "autoWidth": true },
                    { "data": "icon_url", "name": "icon_url", "autoWidth": true },
                    { "data": "url", "name": "url", "autoWidth": true },
                    { "data": "sorting_order", "name": "sorting_order", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetDataById(' + full.menu_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
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



function GetDataById(menu_id) {

    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_Right_Menu_Link/' + menu_id + '/0';

    $.ajax({
        url: apiurl,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;

            BindAllEmp_Company('ddlcompany', login_emp_id, res[0].company_id);

            // $("#ddlcompany").val(res[0].company_id);

            $("#txtMenuName").val(res[0].menu_name);
            $("#txtUrl").val(res[0].url);
            $("#txtIconUrl").val(res[0].icon_url);
            $("#txtsorting_order").val(res[0].sorting_order);

            if (res[0].is_active == 1) {
                $("#is_active").attr('checked', 'checked');
            }
            else {
                $("#is_in_active").attr('checked', 'checked');
            }
            $("#hdnid").val(menu_id);

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
}



