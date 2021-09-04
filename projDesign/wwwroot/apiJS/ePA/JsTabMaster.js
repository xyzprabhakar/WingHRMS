$('#loader').show();
var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);

        GetData(login_company_id);


        $("#btnupdate").hide();
        $('#loader').hide();


        $("#ddlcompany").bind("change", function () {
            $('#loader').show();
            $("#txttabname").val('');
            $("#txtdesc").val('');
            $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
            GetData($(this).val());
            $('#loader').hide();
        });

        $("#btnsave").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var tab_name = $("#txttabname").val();
            var description = $("#txtdesc").val();
            var _for_rm1 = $("#chkdisplayfor1").prop("checked") == true ? 1 : 0;
            var _for_rm2 = $("#chkdisplayfor2").prop("checked") == true ? 1 : 0;
            var _for_rm3 = $("#chkdisplayfor3").prop("checked") == true ? 1 : 0;
            var _for_user = $("#chkdisplayfor4").prop("checked") == true ? 1 : 0;

            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0" || company_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }

            if (tab_name == "" || tab_name == null) {
                is_error = true;
                error_msg = error_msg + "Please enter tab name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please enter description</br>";
            }

            if (_for_rm1 == "0" && _for_rm2 == "0" && _for_rm3 == "0" && _for_user == "0") {
                is_error = true;
                error_msg = error_msg + "Please select atleast one for whom tab will display"
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                company_id: company_id,
                tab_name: tab_name,
                description: description,
                for_rm1: _for_rm1,
                for_rm2: _for_rm2,
                for_rm3: _for_rm3,
                for_user: _for_user,
                created_by: login_emp_id
            }

            $("#loader").show();
            if (confirm("Do you want to save this ?")) {

                var headerss = {}
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();

                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Save_TabMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        //$('#loader').hide();
                        var msg = response.message;
                        var statuscode = response.statusCode;
                        if (statuscode == "0") {

                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            GetData(login_company_id);

                            $("#txttabname").val('');
                            $("#txtdesc").val('');
                            $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
                            $('#loader').hide();
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            $('#loader').hide();
                            messageBox("error", msg);
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
            }
            else {
                $("#loader").hide();
                return false;
            }
        });

        $("#btnupdate").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var tab_name = $("#txttabname").val();
            var description = $("#txtdesc").val();
            var _for_rm1 = $("#chkdisplayfor1").prop("checked") == true ? 1 : 0;
            var _for_rm2 = $("#chkdisplayfor2").prop("checked") == true ? 1 : 0;
            var _for_rm3 = $("#chkdisplayfor3").prop("checked") == true ? 1 : 0;
            var _for_user = $("#chkdisplayfor4").prop("checked") == true ? 1 : 0;

            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0" || company_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }

            if (tab_name == "" || tab_name == null) {
                is_error = true;
                error_msg = error_msg + "Please enter tab name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please enter description</br>";
            }

            if (_for_rm1 == "0" && _for_rm2 == "0" && _for_rm3 == "0" && _for_user == "0") {
                is_error = true;
                error_msg = error_msg + "Please select atleast one for whom tab will display"
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                tab_mstr_id: $("#hdntabid").val(),
                company_id: company_id,
                tab_name: tab_name,
                description: description,
                for_rm1: _for_rm1,
                for_rm2: _for_rm2,
                for_rm3: _for_rm3,
                for_user: _for_user,
                modified_by: login_emp_id
            }

            $("#loader").show();
            if (confirm("Do you want to save this ?")) {

                var headerss = {}
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();

                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Update_TabMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        //$('#loader').hide();
                        var msg = response.message;
                        var statuscode = response.statusCode;
                        if (statuscode == "0") {
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            GetData(login_company_id);

                            $("#ddlcompany").attr("disabled", false);
                            $("#txttabname").val('');
                            $("#txtdesc").val('');
                            $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });

                            $("#btnupdate").hide();
                            $("#btnsave").show();

                            $('#loader').hide();
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            $('#loader').hide();
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
            }
            else {
                $("#loader").hide();
                return false;
            }
        });

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });


    }, 2000);// end timeout

});


function GetData(companyid) {
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_TabMaster/0/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tbltabmaster").DataTable({
                "processing": true,//for showing process bar
                "serverSide": false,//for process server side
                "bDestroy": true,
                "orderMulti": false,//for disable multiple column at once
                "filter": true,//this is for disable filter(search box)
                "aaData": res,
                "scrollX": 200,
                "columnDefs": [

                    {
                        targets: [8],
                        render: function (data, type, row) {
                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            var _modified_date = new Date(data);
                            return new Date(row.modified_date) < new Date(row.created_dt) ? "-" : GetDateFormatddMMyyyy(_modified_date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                    { "data": "tab_name", "name": "tab_name", "title": "Tab Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "enable_for_rm1", "name": "enable_for_rm1", "title": "Enable for RM1", "autoWidth": true },
                    { "data": "enable_for_rm2", "name": "enable_for_rm2", "title": "Enable for RM2", "autoWidth": true },
                    { "data": "enable_for_rm3", "name": "enable_for_rm3", "title": "Enable for RM3", "autoWidth": true },
                    { "data": "enable_for_user", "name": "enable_for_user", "title": "Enable for User", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick=GetEditData(' + full.tab_mstr_id + ',' + full.company_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteTabMaster(' + full.tab_mstr_id + ',' + full.company_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]

            });
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
    $("#loader").hide();
}


function GetEditData(tabid, companyidd) {
    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_TabMaster/" + tabid + "/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            BindAllEmp_Company('ddlcompany', login_emp_id, res.company_id);
            $("#ddlcompany").attr("disabled", "disabled");
            $("#txttabname").val(res.tab_name);
            $("#txtdesc").val(res.description);
            $("#chkdisplayfor1").prop("checked", res.for_rm1 == 1 ? true : res.for_rm1 == 0 ? false : "");
            $("#chkdisplayfor2").prop("checked", res.for_rm2 == 1 ? true : res.for_rm2 == 0 ? false : "");
            $("#chkdisplayfor3").prop("checked", res.for_rm3 == 1 ? true : res.for_rm3 == 0 ? false : "");
            $("#chkdisplayfor4").prop("checked", res.for_user == 1 ? true : res.for_user == 0 ? false : "");

            //if (res.for_rm1 == 1) {
            //    $("#chkdisplayfor1").prop("checked", true);
            //}

            //if (res.for_rm2 == 1) {
            //    $("#chkdisplayfor2").prop("checked", true);
            //}

            //if (res.for_rm3 == 1) {
            //    $("#chkdisplayfor3").prop("checked", true);
            //}

            //if (res.for_user == 1) {
            //    $("#chkdisplayfor4").prop("checked", true);
            //}

            $("#hdntabid").val(res.tab_mstr_id);

            $("#btnupdate").show();
            $("#btnsave").hide();

            $("#loader").hide();

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function DeleteTabMaster(tabid, companyidd) {
    $("#loader").show();

    if (confirm("Do you want to delete this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiePA/Delete_TabMaster/" + tabid + "/" + companyidd + "/" + login_emp_id,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: headerss,
            success: function (response) {
                _GUID_New();

                var msg = response.message;
                var statuscode = response.statusCode;
                $('#loader').hide();
                if (statuscode == "0") {
                    BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                    GetData(login_company_id);

                    $("#ddlcompany").attr("disabled", false);
                    $("#txttabname").val('');
                    $("#txtdesc").val('');
                    $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });

                    $("#btnupdate").hide();
                    $("#btnsave").show();

                    $('#loader').hide();
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("success", msg);
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


    }
    else {
        $('#loader').hide();
        return false;
    }


}

