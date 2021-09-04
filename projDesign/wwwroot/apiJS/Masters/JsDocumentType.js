$('#loader').show();
var login_emp_id;
var login_role_id;
var default_company;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // HaveDisplay = ISDisplayMenu("Display Company List");
        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);


        GetData(default_company);

        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();


        //$("#ddlcompany").bind("change", function () {
        //    $('#loader').show();
        //    GetData($(this).val());
        //    $('#loader').hide();
        //});


        $("#btnReset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {
            var company_idd = $("#ddlcompany").val();
            var doc_namee = $("#txtdocname").val();
            var remarks = $("#txtremarks").val();
            var errormsg = "";
            var iserror = false;
            //var doc_type_master_id = $("#hdndoctypemasterid").val();

            if (company_idd == "0" || company_idd == null || company_idd == "") {
                errormsg = errormsg + "Please Select Company";
                iserror = true;
            }

            if (doc_namee == "" || doc_namee == null) {
                errormsg = errormsg + "Please Enter Document Name";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }
            var mydata = {
                doc_type_id: "0",
                company_id: company_idd,
                doc_name: doc_namee,
                remarks: remarks,
                created_by: login_emp_id
            }

            $('#loader').show();


            var Obj = JSON.stringify(mydata);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/AddUpdateDocumentTypeMaster",
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
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
            var company_idd = $("#ddlcompany").val();
            var doc_namee = $("#txtdocname").val();
            var remarks = $("#txtremarks").val();
            var doc_type_master_id = $("#hdndoctypemasterid").val();

            var errormsg = "";
            var iserror = false;

            if (doc_type_master_id == null || doc_type_master_id == "" || doc_type_master_id == "0") {
                errormsg = errormsg + "Invalid ID";
                iserror = true;
            }

            if (company_idd == "0" || company_idd == null || company_idd == "") {
                errormsg = errormsg + "Please Select Company";
                iserror = true;
            }

            if (doc_namee == "" || doc_namee == null) {
                errormsg = errormsg + "Please Enter Document Name";
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }
            var mydata = {
                doc_type_id: doc_type_master_id,
                company_id: company_idd,
                doc_name: doc_namee,
                remarks: remarks,
                modified_by: login_emp_id
            }

            $('#loader').show();

            var Obj = JSON.stringify(mydata);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/AddUpdateDocumentTypeMaster",
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
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

    }, 2000);// end timeout

});


function GetData(comapny_idd) {

    var urll;

    //if (HaveDisplay == 0) {
    urll = localStorage.getItem("ApiUrl") + "apiMasters/GetDocumentTypeMaster/0/0"; //+ comapny_idd;
    //}
    //else {
    //    urll= localStorage.getItem("ApiUrl") + "apiMasters/GetDocumentTypeMaster/0/0";
    //}

    $.ajax({
        type: "GET",
        url: urll,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },

        success: function (res) {
            $('#loader').hide();
            $("#tbldoctypelist").DataTable({
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
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);

                                return new Date(row.modified_date) < new Date(row.created_date) ? '-' : GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "doc_name", "name": "doc_name", "title": "Document Name", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },

                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.doc_type_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="DeleteData(' + full.doc_type_id + ')"><i class="fa fa-trash"></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
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

function GetEditData(id) {
    $("#hdndoctypemasterid").val(id);
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiMasters/GetDocumentTypeMaster/" + id + "/0",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },

        success: function (response) {
            $('#loader').hide();
            var res = response;

            if (res != null) {
                BindAllEmp_Company('ddlcompany', login_emp_id, res.company_id)
                // BindCompanyList('ddlcompany', res.company_id);
                $("#txtdocname").val(res.doc_name);
                $("#txtremarks").val(res.remarks);
            }

            $('#btnupdate').show();
            $('#btnsave').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });

}



function DeleteData(id) {

    $('#loader').show();
    if (confirm("Do you want to Delete it?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            type: "DELETE",
            url: localStorage.getItem("ApiUrl") + "apiMasters/DeleteDocumentTypeMaster/" + id,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: headerss,

            success: function (response) {
                $('#loader').hide();
                var res = response;
                var msg = response.message;
                var statuscode = response.statusCode;

                _GUID_New();
                if (statuscode == "0") {
                    $('#btnupdate').hide();
                    $('#btnsave').show();
                    GetData();
                    messageBox("success", msg);
                }
                else if (statuscode == "1" || statuscode == '2') {
                    messageBox("error", Msg);
                }


            },
            error: function (error) {
                $('#loader').hide();
                GetData();
                messageBox("error", error.responseText);
            }
        });
    }
    else {
        $('#loader').hide();
        return false;
    }

}