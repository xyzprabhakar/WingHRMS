$('#loader').show();
var login_company_id;
var login_emp_id;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindEmployeeCodeFromEmpMasterByComp('ddlemployee', login_company_id, -1);



        $("#btnupdate").hide();

        GetData(login_company_id);

        $('#loader').hide();

        $('#txtmonthyear').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
            },

            beforeShow: function () {
                if ((selDate = $(this).val()).length > 0) {
                    iYear = selDate.substring(selDate.length - 4, selDate.length);
                    iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
                }
            }
        });

        $("#ddlcompany").bind("change", function () {
            $('#loader').show();
            GetData($(this).val());
            BindEmployeeCodeFromEmpMasterByComp('ddlemployee', $(this).val(), 0);
            $("#txtcompoffdate").val('');
            $("#txtmonthyear").val('');
            $("#txtremarks").val('');
            $('#loader').hide();
        });

        $("#btnreset").bind("click", function () {
            window.location.reload();
        });

        $("#btnsave").bind("click", function () {
            var companyidd = $("#ddlcompany").val();

            var compoffdate = $("#txtcompoffdate").val();
            var emp_idd = $("#ddlemployee").val();
            var remarks = $("#txtremarks").val();
            //var monthyear = $("#txtmonthyear").val();


            var txtMonthYear = $("#txtmonthyear").val();
            var txtmnyr = new Date($('#txtmonthyear').val());
            var yr = txtmnyr.getFullYear();
            var mnth = txtmnyr.getMonth() + 1;

            if (mnth <= 9) {
                txtMonthYear = yr.toString() + "0" + mnth.toString();
            }
            else {
                txtMonthYear = yr.toString() + mnth.toString();
            }
            //txtMonthYear = yr.toString() + mnth >= 9 ? mnth.toString() : "0" + mnth.toString();


            var is_error = false;
            var errormsg = "";

            if (companyidd == "0" || companyidd == "" || companyidd == null) {
                is_error = true;
                errormsg = errormsg + "Please select company </br>";
            }

            if (emp_idd == "0" || emp_idd == "" || emp_idd == null) {
                is_error = true;
                errormsg = errormsg + "Please Select Employee</br>";
            }

            if (compoffdate == "") {
                is_error = true;
                errormsg = errormsg + "Please Select Compoff Date</br>";
            }

            if (txtMonthYear == "" || txtMonthYear == null) {
                is_error = true;
                errormsg = errormsg + "Please Month year</br>";
            }

            var mydata = {
                compoff_date1: compoffdate,
                e_id: emp_idd,
                remarks: remarks,
                monthyear: txtMonthYear
            }

            $('#loader').show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiMasters/Save_MannualCompoff",
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
                        GetData(login_company_id);
                        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                        BindEmployeeCodeFromEmpMasterByComp('ddlemployee', login_company_id, 0);
                        //if (HaveDisplay == 0) {
                        //    BindCompanyList('ddlcompany', login_company_id);
                        //    $('#ddlcompany').prop("disabled", "disabled");
                        //    BindEmployeeCodeFromEmpMasterByComp('ddlemployee', login_company_id, 0);
                        //}
                        //else {
                        //    BindCompanyList('ddlcompany', 0);
                        //}

                        $("#txtcompoffdate").val('');
                        $("#txtmonthyear").val('');
                        $("#txtremarks").val('');
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode != "0") {
                        messageBox("error", Msg);
                        return false;
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



    }, 2000);// end timeout

});

function GetData(company_idd) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetMannaulCompOff/" + company_idd,
        type: "GET",
        contentType: "application/json",
        datatype: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $("#tblmannualcompoff").DataTable({

                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [
                    {
                        targets: [3],
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
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "compoff_date", "name": "compoff_date", "title": "Compoff Date", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "transaction_date", "name": "transaction_date", "title": "Created On", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}