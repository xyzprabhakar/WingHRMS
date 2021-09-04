$('#loader').show();
var login_emp_id;
var default_company;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        GetData(default_company, 0);


        $('#btnupdate').hide();
        $('#loader').hide();


        $("#ddlcompany").bind("change", function () {
            $('#loader').show();
            GetData($(this).val(), 0);
            $('#loader').hide();
        });

        $("#btnsave").bind("click", function () {
  
            var company_id = $("#ddlcompany").val();
            var _asset = $("#txtasset").val();
            var asset_short_name = $("#txtassetshortname").val();
            var description = $("#txtdesc").val();
            var status = $("input[name='chkstatus']:checked").val();
            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (_asset == "" || _asset == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter asset Name</br>";
            }

            if (asset_short_name == "" || asset_short_name == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Asset Short Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            if (status == null || status == "") {
                is_error = true;
                error_msg = error_msg + "Please Select status</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                company_id: company_id,
                asset_name: _asset,
                short_name: asset_short_name,
                description: description,
                is_deleted: status,
                created_by: login_emp_id,
            }

            $("#loader").show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            var Obj = JSON.stringify(mydata);
            $.ajax({
                url: localStorage.getItem("ApiUrl") + '/apiMasters/Save_AssetMaster',
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (res) {
                    _GUID_New();
                    $('#loader').hide();
                    var statuscode = res.statusCode;
                    var msg = res.message;
                    if (statuscode == "0") {
                        // HaveDisplay = ISDisplayMenu("Display Company List");

                        //BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
                        GetData($("#ddlcompany").val(), 0);
                        
                        $("#txtasset").val('');
                        $("#txtassetshortname").val('');
                        $("#txtdesc").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        messageBox("success", msg);
                        return false;
                    }
                    else {
                        messageBox("error", msg);
                        return false;
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


        $("#btnupdate").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var _asset = $("#txtasset").val();
            var asset_short_name = $("#txtassetshortname").val();
            var description = $("#txtdesc").val();
            var status = $("input[name='chkstatus']:checked").val();
            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (_asset == "" || _asset == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter asset Name</br>";
            }

            if (asset_short_name == "" || asset_short_name == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Asset Short Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            if (status == null || status == "") {
                is_error = true;
                error_msg = error_msg + "Please Select status</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                asset_master_id: $("#hdnassetmasterid").val(),
                company_id: company_id,
                asset_name: _asset,
                short_name: asset_short_name,
                description: description,
                is_deleted: status,
                modified_by: login_emp_id,
            }

            $("#loader").show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/Update_AssetMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (res) {
                    _GUID_New();
                    $('#loader').hide();
                    var statuscode = res.statusCode;
                    var msg = res.message;
                    if (statuscode == "0") {

                        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
                        GetData(default_company, 0);

                        $("#ddlcompany").prop("disabled", false);
                        $("#txtasset").val('');
                        $("#txtassetshortname").val('');
                        $("#txtdesc").val('');
                        $('input:radio[name=chkstatus]:checked').prop('checked', false);
                        $("#btnupdate").hide();
                        $("#btnsave").show();
                        messageBox("success", msg);
                        return false;
                    }
                    else {
                        messageBox("error", msg);
                        return false;
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


        $("#btnreset").bind("click", function () {
            location.reload();
        });



    }, 2000);// end timeout

});


function GetData(companyid, assset_masterid) {

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_AssetMaster/" + companyid + "/" + assset_masterid,
        type: "GET",
        contentType: "application/json",
        data: {},
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblasset").DataTable({
                "processing": true,//show progress bar,
                "serverSide": false,//for process Server side
                "orderMulti": false, //for disable multiple column at once
                "bDestroy": true,//for remove previous data
                "filter": true,//for disable filter(search box)
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [

                    {
                        targets: [4],
                        render: function (data, type, row) {

                            return data == "0" ? "Active" : data == "1" ? "In Active" : "";
                        }
                    },
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

                            var modified_date = new Date(data);

                            return new Date(row.modified_dt) < new Date(row.created_dt) ? "-" : GetDateFormatddMMyyyy(modified_date);
                        }
                    }
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "asset_name", "name": "asset_name", "title": "Assets Name", "autoWidth": true },
                    { "data": "short_name", "name": "short_name", "title": "Short Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "is_deleted", "name": "is_deleted", "title": "Status", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Edit", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.asset_master_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


function GetEditData(assset_masterid) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_AssetMaster/0/" + assset_masterid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;
            $("#hdnassetmasterid").val(res.asset_master_id);
            BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
            // BindCompanyList('ddlcompany', res.company_id);
            $('#ddlcompany').prop("disabled", "disabled");
            $("#txtasset").val(res.asset_name);
            $("#txtassetshortname").val(res.short_name);
            $("#txtdesc").val(res.description);
            $("input[name=chkstatus][value=" + res.is_deleted + "]").prop('checked', true);
            $("#btnupdate").show();
            $("#btnsave").hide();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}