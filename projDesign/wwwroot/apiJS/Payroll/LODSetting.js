$('#loader').show();

var companyidd;
var emp_role_idd;
var login_empid;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        companyidd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_empid, companyidd);
        GetData(0);




        $("#btnupdate").hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;
            var companyy = $("#ddlcompany").val();

            if (companyy == "0" || companyy == null || companyy == "") {
                errormsg = errormsg + 'Please Select Company <br/>';
                iserror = true;
            }

            if ($("#ddl_lop_setting").val() == "0") {
                errormsg = errormsg + 'Please Select LOP Setting<br/>';
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var mydata = {

                companyid: companyy,
                lop_setting_name: $("#ddl_lop_setting").val(),
                created_by: login_empid
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + '/apiPayroll/Save_Lod_Setting',
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
                        return false
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
            // debugger;
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var compny = $("#ddlcompany").val();



            if (compny == "0" || compny == null || compny == null) {
                errormsg = errormsg + 'Please Select Company <br/>';
                iserror = true;
            }

            if ($("#ddl_lop_setting").val() == "0") {
                errormsg = errormsg + 'Please Select LOP Setting<br/>';
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var mydata = {
                companyid: compny,
                lop_setting_name: $("#ddl_lop_setting").val(),
                modified_by: login_empid,
                lop_setting_id: $("#hdn_lop-setting_id").val()
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + '/apiPayroll/Edit_Lod_setting',
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

        $('#loader').hide();

    }, 2000);// end timeout

});

function GetData(companyid) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_LOD_Setting/0/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            $("#tbllodsetting").DataTable({
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
                            targets: [2],
                            render: function (data, type, row) {
                                return data == '1' ? 'Monthly' : 'Fixed'
                                return data == '1' ? $("input[name=abc]").prop("checked", true) : ''
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "lop_setting", "name": "lop_setting", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="EditData(' + full.lop_setting_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function EditData(idd) {
    // debugger;
    $('#loader').show();
    $("#hdn_lop-setting_id").val(idd);
    //var company_id = localStorage.getItem("company_id");
    //$("#ddlcompany").prop('disabled', true);

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_LOD_Setting/" + idd + "/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $('#loader').hide();

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                return false;
            }

            if (response != null) {
                $("#ddlcompany").val(response.companyid);
                $("#ddl_lop_setting").val(response.lop_setting_name);

                $("#btnupdate").show();
                $("#btnsave").hide();
            }
            else {
                alert("Something went wrong");
            }
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

function info() {
    messageBox("info", "<ul>Fixed:</ul> Universal Payout 30 days.</br>According to this Rule, the salary of an employee is calculated on the basis of 30 days. </br>So the per day Salary of the employee becomes(Gross Salary / 30).</br> E.g. for an employee whose salary is 30,000.The per day salary will be 30,000 / 30=1,000.This will be applicable in all 12 months.</br> </br> <ul> Monthly:</ul> Month Based Payroll Rule According to this Rule, the salary of an employee is calculated on the basis of actual number of days in a month.</br>So the per day Salary of the employee becomes(Gross Salary / No of days in a month).");
}