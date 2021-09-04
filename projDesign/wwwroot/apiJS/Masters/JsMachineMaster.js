$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        $('#btnupdate').hide();
        $('#btnsave').show();


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



        //var HaveDisplay = ISDisplayMenu("Display Company List");
        BindCompanyListAll('ddlcompany', login_emp_id, 0);
        setSelect('ddlcompany', default_company);
        BindLocationList('ddllocation', default_company, 0);

        GetData(default_company);




        $('#loader').hide();




        $('#ddlcompany').bind("change", function () {
            $("#txtIpAddress").val('');
            $("#txtPort").val('');
            $("#txtMachineNumber").val('');
            $("#ddlMachineType").val('');
            $("#txtDescription").val('');
            $("#btnupdate").hide();
            $("#btnsave").show();
            BindLocationList('ddllocation', $(this).val(), 0);
            BindSubLocationListForddl('ddlsublocation', 0, 0);
            GetData($(this).val());
        });

        $('#ddllocation').bind("change", function () {
            BindSubLocationListForddl('ddlsublocation', $(this).val(), 0);
        });

        $('#txtIpAddress').bind("change", function () {
            ValidateIPaddress($(this).val());
        });



        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var txtIpAddress = $("#txtIpAddress").val();
            var txtPort = $("#txtPort").val();
            var txtMachineNumber = $("#txtMachineNumber").val();
            var ddlMachineType = $("#ddlMachineType").val();
            var txtDescription = $("#txtDescription").val();
            var ddlcompany = $("#ddlcompany").val();
            var ddllocation = $("#ddllocation").val();
            var ddlsublocation = $("#ddlsublocation").val();

            var is_active = 1;

            //validation part

            if (ddlcompany == '' || ddlcompany == 0 || ddlcompany == null) {
                messageBox("error", "Please Select Company...!");
                $('#loader').hide();
                return;
            }
            if (ddllocation == '' || ddllocation == 0) {
                messageBox("error", "Please Select Location...!");
                $('#loader').hide();
                return;
            }
            if (ddlsublocation == '' || ddlsublocation == 0) {
                messageBox("error", "Please Select Sub Location...!");
                $('#loader').hide();
                return;
            }

            if (txtIpAddress == null || txtIpAddress == '') {
                messageBox("error", "Please Enter IP Address...!");
                $('#loader').hide();
                return;
            }
            if (txtPort == null || txtPort == '') {
                messageBox("error", "Please Enter Port Number...!");
                $('#loader').hide();
                return;
            }
            if (txtMachineNumber == null || txtMachineNumber == '') {
                messageBox("error", "Please Enter Machine Number...!");
                $('#loader').hide();
                return;
            }
            if (ddlMachineType == null || ddlMachineType == '') {
                messageBox("error", "Please Enter Machine Type...!");
                $('#loader').hide();
                return;
            }


            //if (iserror) {
            //    messageBox("error", errormsg);
            //    //  messageBox("info", "eror give");
            //    return false;
            //}

            //  var loginn_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            var myData = {

                'ip_address': txtIpAddress,
                'port_number': txtPort,
                'machine_number': txtMachineNumber,
                'machine_type': ddlMachineType,
                'machine_description': txtDescription,
                'is_active': is_active,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id,
                'company_id': $("#ddlcompany").val(),
                'location_id': $("#ddllocation").val(),
                'sub_location_id': $("#ddlsublocation").val(),
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_MachineMaster';
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

                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtIpAddress").val('');
                        $("#txtPort").val('');
                        $("#txtMachineNumber").val('');
                        $("#ddlMachineType").val('');
                        $("#ddllocation").val('0');
                        $("#ddlsublocation").val('0');
                        $("#txtDescription").val('');
                        $("#hdnid").val('');
                        GetData(ddlcompany);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('#loader').hide();
                        messageBox("success", Msg);
                        // alert(Msg);
                        // location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        $('#loader').hide();
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });


        $("#btnupdate").bind("click", function () {
            $('#loader').show();

            var txtIpAddress = $("#txtIpAddress").val();
            var txtPort = $("#txtPort").val();
            var txtMachineNumber = $("#txtMachineNumber").val();
            var ddlMachineType = $("#ddlMachineType").val();
            var txtDescription = $("#txtDescription").val();
            var machine_id_ = $("#hdnid").val();
            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (txtIpAddress == null || txtIpAddress == '') {
                errormsg = "Please Enter IP Address...! ";
                iserror = true;
            }
            if (txtPort == null || txtPort == '') {
                errormsg = "Please Enter Port Number...! ";
                iserror = true;
            }
            if (txtMachineNumber == null || txtMachineNumber == '') {
                errormsg = "Please Enter Machine Number...! ";
                iserror = true;
            }
            if (ddlMachineType == null || ddlMachineType == '') {
                errormsg = "Please Enter Machine Type...! ";
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            //  var loginn_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var myData = {

                'ip_address': txtIpAddress,
                'port_number': txtPort,
                'machine_number': txtMachineNumber,
                'machine_type': ddlMachineType,
                'machine_description': txtDescription,
                'is_active': is_active,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id,
                'company_id': $("#ddlcompany").val(),
                'location_id': $("#ddllocation").val(),
                'sub_location_id': $("#ddlsublocation").val(),
                'machine_id': machine_id_
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_MachineMaster';
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
                    //// debugger;;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtIpAddress").val('');
                        $("#txtPort").val('');
                        $("#txtMachineNumber").val('');
                        $("#ddlMachineType").val('');
                        $("#ddllocation").val('0');
                        $("#ddlsublocation").val('0');
                        $("#txtDescription").val('');
                        $("#hdnid").val('');
                        GetData(ddlcompany);
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("btnupdate").text('Update').attr("disabled", false);
                        $('#loader').hide();
                        messageBox("success", Msg);
                        //alert(Msg);
                        //location.reload();

                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        $('#loader').hide();
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });

    }, 2000);// end timeout

});



//--------bind data in jquery data table
function GetData(company_id) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_MachineMasterDataByCompany/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tblgrade").DataTable({
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
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "_location", "name": "_location", "title": "Location", "autoWidth": true },
                    { "data": "_sub_location", "name": "", "title": "Sub Location", "autoWidth": true },
                    { "data": "ip_address", "name": "ip_address", "title": "IP Address", "autoWidth": true },
                    { "data": "port_number", "name": "port_number", "title": "Port", "autoWidth": true },
                    { "data": "is_active", "name": "status", "title": "Status", "autoWidth": true },
                    { "data": "machine_number", "name": "Machine No.", "title": "Machine No.", "autoWidth": true },
                    //{ "data": "created_by", "name": "createdby", "autoWidth": true },
                    { "data": "createdon", "name": "createdon", "title": "Created On", "autoWidth": true },
                    //adding new coloumn


                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.machine_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
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

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_MachineMasterData/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            data = res;


            if (data.company_id != null) {
                //    BindCompanyList('ddlcompany', data.company_id);
                BindAllEmp_Company('ddlcompany', login_emp_id, data.company_id)
            }

            if (data.location_id != null) {
                //BindLocationListForddl('ddllocation', data.company_id, data.location_id);
                BindLocationList('ddllocation', data.company_id, data.location_id);
            }

            if (data.sub_location_id != null) {
                BindSubLocationListForddl('ddlsublocation', data.location_id, data.sub_location_id);
            }
            $("#ddlcompany").val(data.company_id);
            $("#ddllocation").val(data.location_id);
            $("#ddlsublocation").val(data.sub_location_id);

            $("#txtIpAddress").val(data.ip_address);
            $("#txtPort").val(data.port_number);
            $("#txtMachineNumber").val(data.machine_number);
            $("#ddlMachineType").val(data.machine_type);
            $("#txtDescription").val(data.machine_description);

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

//-------update city data

