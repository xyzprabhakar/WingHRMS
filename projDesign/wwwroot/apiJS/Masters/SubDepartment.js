$('#loader').show();
var emp_role_idd;
var company_idd;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var HaveDisplay = ISDisplayMenu("Display Company List");

        if (HaveDisplay == 0) {
            BindCompanyList('ddlcompany', company_idd);

            $('#ddlcompany').prop("disabled", "disabled");



            BindDepartmentList('ddldepartment', company_idd, -1);

        }
        else {
            BindCompanyList('ddlcompany', 0);
        }
        GetData();

        $('#ddlcompany').bind("change", function () {
            BindDepartmentList('ddldepartment', $(this).val(), -1);
        });

        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();

    }, 2000);// end timeout

});

$('#btnreset').bind('click', function () {
    //$("#txtsubdeptname").val('');
    //$('#txtsubdeptcode').val('');
    //$('#ddlcompany').val('0');
    //$('#ddldepartment').val('0');
    //$('input:radio[name=chkstatus]:checked').prop('checked', false);

    //$('#btnupdate').hide();
    //$('#btnsave').show();
    //$("#hdnid").val('');
    location.reload();
});

$('#btnsave').bind("click", function () {
    $('#loader').show();
    var sub_department_name = $("#txtsubdeptname").val();
    var sub_department_code = $('#txtsubdeptcode').val();
    var department_id = $('#ddldepartment').val();
    var company_id = $('#ddlcompany').val();

    var is_active = 0;
    var errormsg = '';
    var iserror = false;

    if ($("input[name='chkstatus']:checked")) {
        if ($("input[name='chkstatus']:checked").val() == '1') {
            is_active = 1;
        }
    }

    if (emp_role_idd == "2") {
        company_id = company_idd;
    }

    //validation part
    if (sub_department_name == null || sub_department_name == '') {
        errormsg = "Please enter sub department name !! <br/>";
        iserror = true;
    }
    if (sub_department_code == null || sub_department_code == '') {
        errormsg = errormsg + "Please enter sub department code !! <br/>";
        iserror = true;
    }
    if (company_id == null || company_id == '0') {
        errormsg = errormsg + "Please select company name !! <br/>";
        iserror = true;
    }
    if (department_id == null || department_id == '0') {
        errormsg = errormsg + "Please select department name !! <br/>";
        iserror = true;
    }
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

            // var resp = JSON.parse(data);
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
                //$("#txtsubdeptname").val('');
                //$('#txtsubdeptcode').val('');
                //$('#ddlcompany').val('0');
                //$('#ddldepartment').val('0');
                //$('input:radio[name=chkstatus]:checked').prop('checked', false);

                //GetData();
                //messageBox("success", Msg);
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

//--------bind data in jquery data table
function GetData() {

    var apiurl = "";
    if (emp_role_idd == "2") {
        apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentMasterByCompID/' + company_idd;
    }
    else {
        apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentMaster/0';
    }

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
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "subdeptcode", "name": "subdeptcode", "autoWidth": true },
                    { "data": "subdeptname", "name": "subdeptname", "autoWidth": true },
                    { "data": "departmentname", "name": "departmentname", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    //{ "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.subdeptid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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

function GetEditData(id) {
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

            $("#txtsubdeptname").val(data.sub_department_name);
            $('#txtsubdeptcode').val(data.sub_department_code);
            BindCompanyList('ddlcompany', data.company_id);

            if (emp_role_idd == "2") {

                $('#ddlcompany').prop("disabled", "disabled");

                $('#ddlcompany').attr("style", "border : none !important; background-color : #f5f5f5 !important; -webkit-appearance : none; pointer-events :none; ");


            }

            BindDepartmentList('ddldepartment', data.company_id, data.department_id);
            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);

            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText)
        }
    });
}


//-------update city data
$("#btnupdate").bind("click", function () {
    $('#loader').show();
    var sub_department_id = $("#hdnid").val();
    var sub_department_name = $("#txtsubdeptname").val();
    var sub_department_code = $('#txtsubdeptcode').val();
    var department_id = $('#ddldepartment').val();
    var company_id = $('#ddlcompany').val();

    var is_active = 0;
    var errormsg = '';
    var iserror = false;

    if ($("input[name='chkstatus']:checked")) {
        if ($("input[name='chkstatus']:checked").val() == '1') {
            is_active = 1;
        }
    }

    if (emp_role_idd == "2") {
        company_id = company_idd;
    }

    //validation part
    if (sub_department_name == null || sub_department_name == '') {
        errormsg = "Please enter sub department name !! <br/>";
        iserror = true;
    }
    if (sub_department_code == null || sub_department_code == '') {
        errormsg = errormsg + "Please enter sub department code !! <br/>";
        iserror = true;
    }
    if (company_id == null || company_id == '0') {
        errormsg = errormsg + "Please select company name !! <br/>";
        iserror = true;
    }
    if (department_id == null || department_id == '0') {
        errormsg = errormsg + "Please select department name !! <br/>";
        iserror = true;
    }
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
                alert(Msg);
                location.reload();
                //$("#hdnid").val('');
                //$("#txtsubdeptname").val('');
                //$('#txtsubdeptcode').val('');
                //$('#ddlcompany').val('0');
                //$('#ddldepartment').val('0');
                //$('input:radio[name=chkstatus]:checked').prop('checked', false);

                //$('#btnupdate').hide();
                //$('#btnsave').show();
                //GetData();
                //messageBox("success", Msg);
                //$("btnupdate").text('Update').attr("disabled", false);
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

