
var login_emp_id;
var emp_role_idd;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        BindRoleMaster('ddlemp_role', 0);
        BindMenusList();

        // BindMenu_in_checkbox('bindmenumaster', 0);

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        $('#loader').hide();



        //$(document).on("change", "#ddlemp_role", function () {

        //    if ($("#ddlemp_role").val() > 0) {
        //        GetData($("#ddlemp_role").val());
        //    }
        //    else {
        //        window.location.reload();
        //    }

        //});

    }, 2000);// end timeout

});

$("#btnreset").bind("click", function () {
    location.reload();
});

$("#ddlemp_role").bind("change", function () {
    if ($(this).val() == "0") {
        messageBox("error", "Please select role");
        return false;
    }

    $("#hdn_role_menu_id").val($(this).val());
    GetData($(this).val());

});


function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#bindmenumaster').DataTable().cells().nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#bindmenumaster").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(allPages).find('.chkRow').prop('checked', true);
        }
        else {
            $(allPages).find('.chkRow').prop('checked', false);
        }
    });

}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#bindmenumaster").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function GetData(roleid) {
    $('#loader').show();
    // $("#hdn_role_menu_id").val(roleid); //role id

    // // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiMasters/Get_AssignRoleMenu/' + roleid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }


            Binddata(res);


            //if (response != null) {
            //    $("input[name=menuidd]").prop("checked", false);
            //    var checked_menuid = [];
            //    checked_menuid = response.menu_id.split(',');
            //    $.each(checked_menuid, function (i, val) {
            //        $("input[value='" + val + "']").prop("checked", true);

            //    });

            //    $('#loader').hide();
            //}
            //else {
            //    $("input[name=menuidd]").prop("checked", false);
            //    $('#loader').hide();
            //}

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindMenusList() {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiMasters/Get_MenuMaster/0',
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            Binddata(res);
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function Binddata(res) {
    $("#bindmenumaster").DataTable({
        "processing": true,
        "serverSide": false,
        "bDestroy": true,
        "orderMulti": false,
        "filter": true,
        "scrollX": 200,
        "data": res,
        "columnDefs": [],
        columns: [
            { "data": null, "title": "S.No.", "autoWidth": true },
            { "data": "menu_id", "autoWidth": true, "visible": false },
            {
                "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' />Select All", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return '<input type="checkbox" class="chkRow" ' + (full.is_active != undefined && full.is_active != null ? (full.is_active == 1 ? 'checked' : '') : '') + ' />';
                }
            },
            { "data": "menu_id", "visible": false },
            { "data": "menu_name", "title": "Menu", "autoWidth": true },
            { "data": "parent_menu_name", "title": "Parent Menu", "autoWidth": true },
            { "data": "urll", "title": "Url", "autoWidth": true },
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
    });

}


$("#btnsave").bind("click", function () {
    // // debugger;

    var errormsg = "";
    var iserror = false;
    if ($('#ddlemp_role').val() == "" || $('#ddlemp_role').val() == "0") {
        errormsg = errormsg + 'Please Select Role </br>';
        iserror = true;
    }




    var checked_menu = [];


    var table = $("#bindmenumaster").DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        var _ischecked = table.cell(rowIdx, 2).nodes().to$().find('input').is(":checked");

        if (_ischecked == true) {


            checked_menu.push(table.cell(rowIdx, 1).nodes().to$().html());
            //var no = $(this).val();
            //BulAppId.push(no);
        }
    });

    if (checked_menu.length == 0) {
        messageBox("info", "Please select users");
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        return false;
    }


    var hdnidd = "";

    if ($("#hdn_role_menu_id").val() != "") {
        hdnidd = $("#hdn_role_menu_id").val()
    }

    var mydata = {
        role_id: $("#ddlemp_role").val(),
        menu_id: checked_menu.join(','),
        created_by: login_emp_id,
        role_menu_id: hdnidd

    }
    $('#loader').show();

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    var Obj = JSON.stringify(mydata);
    $.ajax({
        url: localStorage.getItem("ApiUrl") + '/apiMasters/Save_Edit_AssignRoleMenu',
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (reponse) {
            var statuscode = reponse.statusCode;
            var Msg = reponse.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
                return false;
            }
        },
        error: function (err) {
            _GUID_New();
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });


});




