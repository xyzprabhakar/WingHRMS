$('#loader').show();
var emp_role_id;
var company_idd;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        $("#txtMonthYear").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',//'yy mm',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                // $("#hdnmonthyear").val(iYear.toString() + iMonth.toString());

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
        // GetSalaryInput(0);



        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $("#btnmonthyrdtl").bind("click", function () {

            $("#tblsalaryinputreport").table2excel({

                exclude: ".noExl",
                name: "Download",
                filename: "Download", //do not include extension
                fileext: ".xls" // file extension

            });

        });

        $('#loader').hide();

        $("#btnGetData").bind("click", function () {

            // var txtMonthYear = $("#txtMonthYear").val();//$("#hdnmonthyear").val();


            var txtMonthYear = $("#txtMonthYear").val();  // $("#hdnmonthyear").val();

            if (txtMonthYear == "" || txtMonthYear == null) {
                messageBox("error", "Please select date");
                return false;
            }

            var mnth = monthNameToNum(txtMonthYear.split(' ')[0]);
            mnth = parseInt(mnth) <= 9 ? "0" + mnth.toString() : mnth;
            var yr = txtMonthYear.split(' ')[1];
            txtMonthYear = yr.toString() + mnth.toString();

            //var txtMonthYear = $("#txtMonthYear").val();
            GetSalaryInput(txtMonthYear);

        });

    }, 2000);// end timeout

});

function GetSalaryInput(monthyear) {
    monthyear = monthyear.trim().replace(/\s/g, '')

    var _apiurl = "";
    var _type = "";
    var headerss = {};

    //var HaveDisplay = ISDisplayMenu("Display Company List");

    if (monthyear != "" && monthyear != null) {
        //if (HaveDisplay == 0) {
        //    _apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Salary_input_Report_by_monthyearandcompID/' + monthyear + "/" + company_idd+"/1";
        //    _type = "GET";
        //    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        //}
        //else {
        //    _apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Salary_input_Report_by_monthyear/' + monthyear +"/1";
        //    _type = "POST";

        //    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        //    headerss["salt"] = $("#hdnsalt").val();
        //}

        _apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Salary_input_Report_by_monthyear/' + monthyear + "/1";
        _type = "POST";

        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();


    }
    else {
        _apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Salary_input_Report',
            _type = "GET";
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    }
    $('#loader').show();

    $.ajax({
        url: _apiurl,
        type: _type,
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: headerss,
        success: function (response) {
            $('#loader').hide();

            if (monthyear != "" && monthyear != null) {
                _GUID_New();
            }

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                return false;
            }


            var ColumnData = [];
            ColumnData.push({
                "data": "emp_code",
                "name": "emp_code", "autoWidth": true,
                "title": "Employee Code"
            });
            ColumnData.push({
                "data": "EmployeeName",
                "name": "EmployeeName", "autoWidth": true,
                "title": "Employee Name"
            });
            ColumnData.push({
                "data": "PayrolMonthYear",
                "name": "PayrolMonthYear", "autoWidth": true,
                "title": "Payrol Month Year"
            });

            for (i = 0; i < response._componentdata_.length; i++) {
                ColumnData.push({
                    "data": response._componentdata_[i].property_details,
                    "name": response._componentdata_[i].property_details, "autoWidth": true,
                    "title": response._componentdata_[i].property_details
                });
            }

            var listdata = [];
            for (i = 0; i < response.empIDData.length; i++) {
                var templist = {};
                templist["emp_code"] = response.empIDData[i].emp_code;
                templist["EmployeeName"] = response.empIDData[i].employee_name;
                templist["PayrolMonthYear"] = response.empIDData[i].monthyear;
                for (j = 0; j < response._componentdata_.length; j++) {
                    templist[response._componentdata_[j].property_details] = '-';
                }

                for (j = 0; j < response._componentdata_.length; j++) {
                    for (k = 0; k < response.salarydata.length; k++) {
                        if (response._componentdata_[j].component_id == response.salarydata[k].component_id &&
                            response.empIDData[i].emp_id == response.salarydata[k].emp_id
                        ) {
                            templist[response._componentdata_[j].property_details] = response.salarydata[k].valuess;
                        }
                    }
                }
                listdata.push(templist);
            }

            //  console.log(listdata);
            $('#tblsalaryinputreport').empty();
            $('#tblsalaryinputreport').dataTable({
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "columnDefs": ColumnData,
                "data": listdata,
                "columns": ColumnData,
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
            });
        },
        error: function (response) {
            $('#loader').hide();
            _GUID_New();
            aler(response.responseText);
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