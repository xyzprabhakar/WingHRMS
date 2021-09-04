﻿$('#loader').show();

var login_emp_id;
var company_idd;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

        BindAllEmp_Company('ddlCompany', login_emp_id, company_idd);
        // BindCompanyList('ddlCompany', 0);

        $("#ddlCompany").bind("change", function () {
            $("#ddlempcode").empty();
            BindEmployeeCodeFromEmpMasterByComp('ddlempcode', $(this).val(), 0)
        });


        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlCompany").val();
            var employee_id = $("#ddlempcode").val();
            var assetname = $("#txtassetname").val();
            var description = $("#txtassetdescription").val();

            if (company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>!!";
                iserror = true;
            }

            if (employee_id == "0" || employee_id == null) {
                errormsg = errormsg + "Please Select Employee </br>!!";
                iserror = true;
            }

            if (assetname == "") {
                errormsg = errormsg + "Please Enter Asset Name </br>!!";
                iserror = true;
            }

            if (description == "") {
                errormsg = errormsg + "Please Enter Asset Description </br>!!";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }


            var mydata = {
                company_id: company_id,
                req_employee_id: employee_id,
                asset_name: assetname,
                description: description,
                created_by: login_emp_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_AssetRequest",
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

        $('#loader').hide();

    }, 2000);// end timeout

});

function GetData() {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_AssetRequest/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Application': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $("#tblassetrqstdtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs": [

                    {
                        targets: [5],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            var asset_issue_dt = GetDateFormatddMMyyyy(date);

                            return new Date(row.asset_issue_dt) < new Date(row.created_dt) ? '-' : asset_issue_dt;
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);

                            //var date = new Date(row.last_modified_date);
                            //var modified_date = GetDateFormatddMMyyyy(date);

                            //return new Date(row.last_modified_date) < new Date(row.created_dt) ? '-' : modified_date;
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {

                            return data == "0" ? "Pending" : data == "1" ? "Approved" : data == "2" ? "Reject" : data == "3" ? "Cancel" : "";
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {

                            return data == "0" ? "No" : data == "1" ? "Yes" : "";
                        }
                    },

                ],
                "columns": [

                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "req_emp_name_code", "name": "req_emp_name_code", "title": "Employee", "autoWidth": true },
                    { "data": "asset_name", "name": "asset_name", "title": "Asset Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "asset_issue_date", "name": "asset_issue_date", "title": "Asset Issue Date", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "is_finalapprove", "name": "is_finalapprove", "title": "Final Status", "autoWidth": true },
                    { "data": "is_closed", "name": "is_closed", "title": "Closed", "autoWidth": true },

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
            alert(error.responseText);
        }

    });
}