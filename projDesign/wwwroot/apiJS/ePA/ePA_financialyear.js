var default_company;
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        //    var HaveDisplay = ISDisplayMenu("Display Company List");





        // debugger;
        GetData(default_company);


        $('#ddlcompany').bind("change", function () {

            $("#hdnid").val('');
            $("#txtFinancialYearName").val('');
            $('#txtfromdate').val('');
            $('#txttodate').val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
            if ($(this).val() == "0") {
                if ($.fn.DataTable.isDataTable('#tblfinancial')) {
                    $('#tblfinancial').DataTable().clear().draw();
                }
            }
            else {

                GetData($(this).val());
            }
        });


        $('#btnupdate').hide();
        $('#btnsave').show();


        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
                var date2 = $('#txtfromdate').datepicker('getDate');
                $("#txttodate").datepicker('setDate', null);
                $('#txttodate').datepicker('option', 'minDate', date2);
            }
        });

        $("#txttodate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onClose: function () {
                var dt1 = $('#txtfromdate').datepicker('getDate');
                var dt2 = $('#txttodate').datepicker('getDate');
                //check to prevent a user from entering a date below date of dt1
                if (dt2 <= dt1) {
                    var minDate = $('#txttodate').datepicker('option', 'minDate');
                    $('#txttodate').datepicker('setDate', minDate);
                }
            }
        });

        $('#btnreset').bind('click', function () {
            $("#hdnid").val('');
            BindAllEmp_Company('ddlcompany', login_emp, default_company);
            GetData(default_company);
            $("#txtFinancialYearName").val('');
            $('#txtfromdate').val('');
            $('#txttodate').val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
        });
        $('#btnsave').bind("click", function () {
            // debugger;

            var company_id = $("#ddlcompany").val();
            var txtFinancialYearName = $("#txtFinancialYearName").val();
            var from_date = $('#txtfromdate').val();
            var to_date = $('#txttodate').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (company_id == null || company_id == "" || company_id == "0") {
                errormsg = "Please select company !! <br/>";
                iserror = true;
            }
            if (txtFinancialYearName == null || txtFinancialYearName == '') {
                errormsg = "Please enter Financial Year Name !! <br/>";
                iserror = true;
            }
            if (from_date == '' || from_date == null) {
                errormsg = errormsg + 'Please select From Date !! <br />';
                iserror = true;
            }
            if (to_date == '' || to_date == null) {
                errormsg = errormsg + 'Please select To Date !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);

                //  messageBox("info", "eror give");
                return false;
            }
            $('#loader').show();
            var myData = {
                'financial_year_name': txtFinancialYearName,
                'from_date': from_date,
                'to_date': to_date,
                'company_id': company_id,
                'created_by': login_emp,
            };


            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_FinancialMaster';
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
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#txtFinancialYearName").val('');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');
                        GetData(company_id);
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                }
            });
        });

        $("#btnupdate").bind("click", function () {


            var txtFinancialYearName = $("#txtFinancialYearName").val();
            var from_date = $('#txtfromdate').val();
            var to_date = $('#txttodate').val();
            var fyi_id = $("#hdnid").val();
            var company_id = $("#ddlcompany").val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (txtFinancialYearName == null || txtFinancialYearName == '') {
                errormsg = "Please enter Financial Year Name !! <br/>";
                iserror = true;
            }

            if (from_date == '' || from_date == null) {
                errormsg = errormsg + 'Please select From Date !! <br />';
                iserror = true;
            }
            if (to_date == '' || to_date == null) {
                errormsg = errormsg + 'Please select To Date !! <br />';
                iserror = true;
            }

            if (iserror) {
                // messageBox("success", "successfully save data");
                messageBox("error", errormsg);
                //  messageBox("info", "eror give");

                return false;
            }
            $('#loader').show();
            var myData = {
                'fiscal_year_id': fyi_id,
                'financial_year_name': txtFinancialYearName,
                'from_date': from_date,
                'to_date': to_date,
                'company_id': company_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_FinancialYear';
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
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#hdnid").val('');
                        $("#txtFinancialYearName").val('');
                        $('#txtfromdate').val('');
                        $('#txttodate').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        GetData(company_id);
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                    return false;
                }
            });
        });


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData(company_id) {
    // debugger;
    if ($.fn.DataTable.isDataTable('#tblfinancial')) {
        $('#tblfinancial').DataTable().clear().draw();
    }
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_FinancialYearData/0/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            $("#tblfinancial").DataTable({
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
                            targets: [2],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "financial_year_name", "name": "financial_year_name", "autoWidth": true },
                    { "data": "fromdate", "name": "fromdate", "autoWidth": true },
                    { "data": "todate", "name": "todate", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.fyi_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            $("#loader").hide();
            console.log("error");
        }
    });

}

function GetEditData(id) {

    $("#txtFinancialYearName").val('');
    $('#txtfromdate').val('');
    $('#txttodate').val('');
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }
    $('#loader').show();
    var company_id = $("#ddlcompany").val();
    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_FinancialYearData/' + id + '/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }
            $("#txtFinancialYearName").val(data.financial_year_name);
            $('#txtfromdate').val(GetDateFormatMMddyyyy(new Date(data.from_date)));
            $('#txttodate').val(GetDateFormatMMddyyyy(new Date(data.to_date)));

            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


//-------update city data

