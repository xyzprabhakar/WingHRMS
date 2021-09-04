$('#loader').show();

var company_id;
var emp_role_idd;
var login_empid;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_empid, company_id);
        GetData(0, 0);








        $("#btnupdate").hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $(document).on('change', '#ddldate', function () {
            if ($("#ddldate").val() > 0) {
                var _from_date = $("#ddldate").val();
                var to_datee = "";
                if (_from_date == 0) {
                    to_datee = "";
                }
                else if (_from_date == 1) {
                    to_datee = 31;
                }
                else {
                    to_datee = parseInt(_from_date) - 1;
                }
                $("#txttodatee").val(to_datee);
            }
            else {
                $("#txttodatee").val('');
            }

        });


        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlcompany").val();



            //var date_month = $("#txtdatemonth").val();
            // var from_date = $("#txtfromdate").val();
            var _date = $("#ddldate").val();
            //var applicable_from_datee = $("#txtapplicablefromdate").val();
            //var applicable_to_datee = $("#txtapplicabletodate").val();

            var applicable_from_datee = new Date().getFullYear() + '-04-01';

            var applicable_to_datee = parseInt(new Date().getFullYear()) + 1 + '-03-31';

            var created_by = login_empid;
            if (company_id == "0" || company_id == "" || company_id == null) {
                errormsg = errormsg + 'Please Select Company<br/>';
                iserror = true;
            }

            if (_date == "0") {
                errormsg = errormsg + 'Please Select From Date<br/>';
                iserror = true;
            }

            //if (applicable_from_datee == '') {
            //    errormsg = errormsg + 'Please Select Applicable From Date<br/>';
            //    iserror = true;
            //}

            //if (applicable_to_datee == '') {
            //    errormsg = errormsg + 'Please Select Applicable To Date<br/>';
            //    iserror = true;
            //}

            //if (new Date(applicable_to_datee) < new Date(applicable_from_datee)) {
            //    errormsg = errormsg + 'Please Select Applicable To date greater than from date<br/>';
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var app_from = applicable_from_datee.split('-');
            var app_from_date = app_from[0] + '' + app_from[1] + '' + app_from[2];

            var app_to = applicable_to_datee.split('-');
            var app_to_date = app_to[0] + '' + app_to[1] + '' + app_to[2];
            var mydata = {
                'from_month': app_from_date,
                'from_date': _date,
                'applicable_from_date': app_from_date,
                'applicable_to_date': app_to_date,
                'created_by': login_empid,
                'company_id': company_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiPayroll/Save_Payroll_Monthly_Setting',
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
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }

            });
        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlcompany").val();
            var _date = $("#ddldate").val();




            // var date_month = $("#txtdatemonth").val();
            // var from_date = $("#txtfromdate").val();

            var applicable_from_datee = new Date().getFullYear() + '-04-01';

            //var applicable_from_datee =  $("#txtapplicablefromdate").val();

            var applicable_to_datee = parseInt(new Date().getFullYear()) + 1 + '-03-31';

            //var applicable_to_datee =  $("#txtapplicabletodate").val();
            var created_by = login_empid;
            if (company_id == "0" || company_id == null || company_id == "") {
                errormsg = errormsg + 'Please Select Company<br/>';
                iserror = true;
            }

            if (_date == "0") {
                errormsg = errormsg + 'Please Select From Date<br/>';
                iserror = true;
            }

            //if (applicable_from_datee == '') {
            //    errormsg = errormsg + 'Please Select Applicable From Date<br/>';
            //    iserror = true;
            //}

            //if (applicable_to_datee == '') {
            //    errormsg = errormsg + 'Please Select Applicable To Date<br/>';
            //    iserror = true;
            //}

            //if (new Date(applicable_to_datee) < new Date(applicable_from_datee))  {
            //    errormsg = errormsg + 'Please Select Applicable To date greater than from date<br/>';
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var app_from = applicable_from_datee.split('-');
            var app_from_date = app_from[0] + '' + app_from[1] + '' + app_from[2];

            var app_to = applicable_to_datee.split('-');
            var app_to_date = app_to[0] + '' + app_to[1] + '' + app_to[2];
            var mydata = {
                payroll_month_setting_id: $("#hdnmonthlyid").val(),
                from_month: app_from_date,
                from_date: _date,
                applicable_from_date: app_from_date,
                applicable_to_date: app_to_date,
                last_modified_by: login_empid,
                company_id: company_id
            }
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiPayroll/Edit_Payroll_Monthly_Setting',
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

function GetData(id, companyid) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_Payroll_Monthly_Setting/" + id + "/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: "{}",
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            $("#tblpayrollmonthcircle").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once

                "aaData": response,
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
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "title": "S.No", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    {
                        "title": "To Date", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            if (full.from_date == 1) {
                                return '<label>31</label>';
                            }
                            else if (full.from_date > 1) {
                                var to_datee = parseInt(full.from_date) - 1;
                                return '<label>' + to_datee + '</label>';
                            }
                            else {
                                return '<label></label>';
                            }
                            // return full.from_date == 0 ? '<label></label>' : full.from_date == 1 ? '<label>31</label>' : full.from_date > 1 ? '<label>' + parseInt(full.from_date) - parseInt(1) + '</label>' : '';
                        }
                    },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "title": "Modified On", "autoWidth": true },

                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.payroll_month_setting_id + ',' + full.company_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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


function GetEditData(id, companyid) {
    $('#loader').show();
    $("#hdnmonthlyid").val(id);
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_Payroll_Monthly_Setting/" + id + "/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $('#loader').hide();
            if (response != null) {

                BindAllEmp_Company('ddlcompany', login_empid, response.company_id);

                $("#ddlcompany").attr("disabled", true);
                $("#ddldate").val(response.from_date);

                //var full_appfromdate = response.applicable_from_date.toString();
                //var _datee = full_appfromdate.slice(-2);
                //var _yearr = full_appfromdate.substring(0, 4);
                //var _monthh = full_appfromdate.substring(6, 4);

                //var _app_from_dateee = _yearr + "-" + _monthh + "-" + _datee;

                //$("#txtapplicablefromdate").val(_app_from_dateee);

                //var full_apptodate = response.applicable_to_date.toString();
                //var _dateee = full_apptodate.slice(-2);
                //var _yearrr = full_apptodate.substring(0, 4);
                //var _monthhh = full_apptodate.substring(6, 4);

                //var _app_to_dateee = _yearrr + "-" + _monthhh + "-" + _dateee;

                //$("#txtapplicabletodate").val(_app_to_dateee);
                // var _toodate = 31 - (response.from_date);

                //$("#txttodatee").val(_toodate);


                var _from_date = response.from_date;
                var to_datee = "";
                if (_from_date == 0) {
                    to_datee = "";
                }
                else if (_from_date == 1) {
                    to_datee = 31;
                }
                else {
                    to_datee = parseInt(_from_date) - 1;
                }
                $("#txttodatee").val(to_datee);

                $("#btnsave").hide();
                $("#btnupdate").show();


            }
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


