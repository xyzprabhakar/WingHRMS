$('#loader').show();
var emp_role_idd;
var company_idd;
var login_emp_;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_ = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_, company_idd);




        $("#txtMonthYear").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',//'yy mm',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                //  $("#hdnmonthyear").val(iYear.toString() + iMonth.toString());

            },

            beforeShow: function () {
                if ((selDate = $(this).val()).length > 0) {
                    iYear = selDate.substring(selDate.length - 4, selDate.length);
                    iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
                }
            },

        });

        var rpt_id = $(location).attr("href").split('?')[1];
        if (rpt_id == "1") {
            $("#lblrpttitle").text("Salary Report(Arrear)");
            $("#lblrpttitle_").text("Salary Report(Arrear)");
        }
        else {
            $("#lblrpttitle").text("Salary Report");
            $("#lblrpttitle_").text("Salary Report");
        }



        $("#btnExport_to_excel").hide();
        $('#loader').hide();

        $("#btnExport_to_excel").bind("click", function () {
            $("#tblsalaryinputreport").table2excel({

                exclude: ".noExl",
                name: "Download",
                filename: "Download", //do not include extension
                fileext: ".xls" // file extension

            });

        });


        $('#btnreset').bind('click', function () {

            location.reload();
        });

        $("#btnGetData").bind("click", function () {

            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "0" || $("#ddlcompany").val() == null) {
                messageBox("error", "Please select company");
                return false;
            }


            var txtMonthYear = $("#txtMonthYear").val();  // $("#hdnmonthyear").val();

            if (txtMonthYear == "" || txtMonthYear == null) {
                messageBox("error", "Please select date");
                return false;
            }

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);
            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];
            txtMonthYear = yr.toString() + mnth.toString();


            GetSalaryInput(txtMonthYear);

        });

        $("#btnmonthyrdtl").bind("click", function () {

            $("#tblsalaryinputreport").table2excel({

                exclude: ".noExl",
                name: "Download",
                filename: "Download", //do not include extension
                fileext: ".xls" // file extension

            });
        });


    }, 2000);// end timeout


});


function GetSalaryInput(monthyear) {
    monthyear = monthyear.trim().replace(/\s/g, '')

    var _apiurl = "";
    var _type = "";
    var headerss = {};

    var rpt_id = $(location).attr("href").split('?')[1];

    //var HaveDisplay = ISDisplayMenu("Display Company List");


    _type = "GET";
    _apiurl = localStorage.getItem("ApiUrl") + 'apiDynamicReport/Get_Salary_input_Report_by_monthyearandcompID/' + monthyear + "/" + $("#ddlcompany").val() + "/" + rpt_id;
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();


    $.ajax({
        url: _apiurl,
        type: _type,
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: headerss,
        success: function (response) {

            var res = response;
            $("#loader").hide();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            if (res.data.length > 0) {
                $("#btnExport_to_excel").show();
            }
            else {
                $("#btnExport_to_excel").hide();
            }

            $('#tblsalaryinputreport').DataTable({
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "columnDefs": [],
                "data": res.data,
                "columns": res.allcolumns,
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
            });


        },
        error: function (response) {
            $('#loader').hide();
            _GUID_New();
            alert(response.responseText);
        }

    });
}


function monthNameToNum(monthname) {

    var months = [
        'January', 'February', 'March', 'April', 'May',
        'June', 'July', 'August', 'September',
        'October', 'November', 'December'
    ];


    var month = months.indexOf(monthname);
    return month != null ? month + 1 : 0;
}






