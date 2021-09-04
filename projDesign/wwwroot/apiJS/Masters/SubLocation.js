$('#loader').show();
var login_emp_id;
var login_role_id;
var default_company;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }




        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



        var HaveDisplay = ISDisplayMenu("Display Company List");

        if (HaveDisplay == 0) {

            $('#ddlcompany').prop("disabled", "disabled");

            BindCompanyList('ddlcompany', default_company);

            GetDataByCompany(default_company);

            BindLocationList('ddllocation', default_company, -1);
        }
        else {
            BindCompanyList('ddlcompany', 0);
            GetData();

        }


        $('#ddlcompany').bind("change", function () {
            BindLocationList('ddllocation', $(this).val(), -1);
        });

        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();

    }, 2000);// end timeout

});

$('#btnreset').bind('click', function () {
    $('input:radio[name=chkstatus]:checked').prop('checked', false);
    location.reload();
    //$("#txtsublocationname").val('');
    //$('#txtsublocationcode').val('');
    //$('#ddlcompany').val('0');
    //$('#ddllocation').val('0');


    //$('#btnupdate').hide();
    //$('#btnsave').show();
    //$("#hdnid").val('');
});

$('#btnsave').bind("click", function () {
    $('#loader').show();
    // debugger;
    var location_name = $("#txtsublocationname").val();
    // var sub_location_code = $('#txtsublocationcode').val();
    var location_id = $('#ddllocation').val();
    var company_id = $('#ddlcompany').val();

    var is_active = 0;
    var errormsg = '';
    var iserror = false;

    if ($("input[name='chkstatus']:checked")) {
        if ($("input[name='chkstatus']:checked").val() == '1') {
            is_active = 1;
        }
    }

    //validation part
    if (location_name == null || location_name == '') {
        errormsg = "Please enter sub location name !! <br/>";
        iserror = true;
    }
    //if (sub_location_code == null || sub_location_code == '') {
    //    errormsg = errormsg + "Please enter sub location code !! <br/>";
    //    iserror = true;
    //}
    if (company_id == null || company_id == '0') {
        errormsg = errormsg + "Please select company name !! <br/>";
        iserror = true;
    }
    if (location_id == null || location_id == '0') {
        errormsg = errormsg + "Please select location name !! <br/>";
        iserror = true;
    }

    if (!$("input[name='chkstatus']:checked").val()) {

        errormsg = errormsg + "Please select active/in-active status !! <br/>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {

        'location_id': location_id,
        'location_name': location_name,
        // 'sub_location_code': sub_location_code,
        'company_id': company_id,
        // 'department_id': department_id,
        'is_active': is_active
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_SubLocationMaster';
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
                //$("#txtsublocationname").val('');
                //// $('#txtsublocationcode').val('');
                //$('#ddlcompany').val('0');
                //$('#ddllocation').val('0');
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
            var error = "";
            _GUID_New();
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
function GetDataByCompany(company_id) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubLocationMasterByCompany/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblsublocation").DataTable({
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
                            targets: [4],
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
                        }
                        //,
                        //{
                        //    targets: [8],
                        //    "class": "text-center"

                        //}
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    //  { "data": "companyid", "name": "companyid", "autoWidth": true },
                    { "data": "sublocationname", "name": "sublocationname", "autoWidth": true },
                    { "data": "locationname", "name": "locationname", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    { "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.sublocid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
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

function GetData() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubLocationMaster/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblsublocation").DataTable({
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
                            targets: [4],
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
                        }
                        //,
                        //{
                        //    targets: [8],
                        //    "class": "text-center"

                        //}
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    //  { "data": "companyid", "name": "companyid", "autoWidth": true },
                    { "data": "sublocationname", "name": "sublocationname", "autoWidth": true },
                    { "data": "locationname", "name": "locationname", "autoWidth": true },
                    { "data": "companyname", "name": "companyname", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    { "data": "createdby", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.sublocid + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            //alert(error);
            messageBox("error", error.responseText);
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

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SublocationMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtsublocationname").val(data.location_name);
            // $('#txtsublocationcode').val(data.sub_department_code);
            BindCompanyList('ddlcompany', data.company_id);
            BindLocationList('ddllocation', data.company_id, data.location_id);
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
    // debugger;
    var sub_location_id = $("#hdnid").val();
    var location_name = $("#txtsublocationname").val();
    var location_id = $('#ddllocation').val();
    var company_id = $('#ddlcompany').val();

    var is_active = 0;
    var errormsg = '';
    var iserror = false;

    if ($("input[name='chkstatus']:checked")) {
        if ($("input[name='chkstatus']:checked").val() == '1') {
            is_active = 1;
        }
    }

    //validation part
    if (location_name == null || location_name == '') {
        errormsg = "Please enter sub location name !! <br/>";
        iserror = true;
    }
    if (company_id == null || company_id == '0') {
        errormsg = errormsg + "Please select company name !! <br/>";
        iserror = true;
    }
    if (location_id == null || location_id == '0') {
        errormsg = errormsg + "Please select location name !! <br/>";
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

        'sub_location_id': sub_location_id,
        'location_name': location_name,
        'company_id': company_id,
        'location_id': location_id,
        'is_active': is_active
    };

    $("btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_SubLocationMaster';
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
                //$("#hdnid").val('');
                //$("#txtsublocationname").val('');
                //$('#ddlcompany').val('0');
                //$('#ddllocation').val('0');
                //$('input:radio[name=chkstatus]:checked').prop('checked', false);

                //$('#btnupdate').hide();
                //$('#btnsave').show();
                //GetData();
                //messageBox("success", Msg);
                $("btnupdate").text('Update').attr("disabled", false);
                alert(Msg);
                location.reload();
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

