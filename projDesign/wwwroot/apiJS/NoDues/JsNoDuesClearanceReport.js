$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {


        $("#btnupdate").hide();
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        //BindDepartmentList('ddldepartment', default_company, -1);
        //BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', default_company, -1);
        var d = new Date();
        $("#txtFromDate").val(GetDateFormatddMMyyyy(d));
        d = new Date(d.setMonth(d.getMonth() + 1));
        $("#txtToDate").val(GetDateFormatddMMyyyy(d));

        $("#txtToDate").change(function () {
            var toDate = $(this).val();
            var fromDate = $("#txtFromDate").val();

            if (Date.parse(fromDate) > Date.parse(toDate)) {
                $(this).val('');
                messageBox("error", "To date should be greater than From date...");
                return false;
            }
        });

        $("#ddlcompany").bind("change", function () {

            if ($.fn.DataTable.isDataTable('#tblReport')) {
                $('#tblReport').DataTable().clear().draw();
            }

            BindDepartmentList('ddldepartment', $(this).val(), -1);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', $(this).val(), -1);
        });

        $('#loader').hide();

    }, 2000);// end timeout

});

$("#btnGetReport").bind("click", function () {

    GetData();
});



function GetData() {
    debugger;
    $('#loader').show();

    var ddlcompany = $("#ddlcompany").val();
    var fromDate = $("#txtFromDate").val();
    var toDate = $("#txtToDate").val();
    //var txtMonthYear = $("#txtMonthYear").val();

    if (ddlcompany == '' || ddlcompany == 0 || ddlcompany == null) {
        messageBox("error", "Please Select Company...!");
        $('#loader').hide();
        return;
    }
    if (toDate == '' || toDate == null) {
        messageBox("error", "Please Select To Date...!");
        $('#loader').hide();
        return;
    }
    if (fromDate == '' || fromDate == null) {
        messageBox("error", "Please Select From Date...!");
        $('#loader').hide();
        return;
    }

    //if (txtMonthYear == null || txtMonthYear == '') {
    //    $('#loader').hide();
    //    messageBox('info', 'Please select Month...!');
    //    return false;
    //}
    //else {
    //    var txtmnyr = new Date($('#txtMonthYear').val());
    //    var yr = txtmnyr.getFullYear();
    //    var mnth = txtmnyr.getMonth() + 1;
    //    txtMonthYear = String(yr) + '-' + String(mnth);
    //}



    var myData = {
        'company_id': ddlcompany,
        'fromDate': fromDate,
        'toDate': toDate,
        //'monthYear': txtMonthYear,
    };

    //const date = new Date(txtMonthYear.split('-')[0], (txtMonthYear.split('-')[1] - 1), 01);
    //const _month = date.toLocaleString('default', { month: 'long' });
    //const _year = txtMonthYear.split('-')[0];

    //debugger;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiNoDues/GetNoDuesClearenceReport",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(myData),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            _GUID_New();

            $("#tblReport").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'No Dues Clearance Report from ' + fromDate + '  to  ' + toDate + ' ',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]
                        }
                    },
                    {
                        text: 'Export to PDF',
                        title: 'No Dues Clearance Reportfrom ' + fromDate + '  to  ' + toDate + ' ',
                        orientation: 'landscape',
                        extend: 'pdfHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]
                        }
                    },
                ],
                "columnDefs":
                    [
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                var d = GetDateFormatddMMyyyy(date);
                                if (d == '01/Jan/1') {
                                    return "";
                                }
                                else
                                    return d;
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                var d = GetDateFormatddMMyyyy(date);
                                if (d == '01/Jan/1') {
                                    return "";
                                }
                                else
                                    return d;
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                var d = GetDateFormatddMMyyyy(date);
                                if (d == '01/Jan/1') {
                                    return "";
                                }
                                else
                                    return d;
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "dept_Name", "name": "dept_Name", "title": "Department", "autoWidth": true },
                    { "data": "designation", "name": "designation", "title": "Designation", "autoWidth": true },
                    //{ "data": "location", "name": "location", "title": "Location", "autoWidth": true },
                    //{ "data": "_date_of_joining", "name": "_date_of_joining", "title": "Date of Joining", "autoWidth": true },
                    { "data": "date_of_Resign", "name": "date_of_Resign", "title": "Resignation Date ", "autoWidth": true },
                    { "data": "date_of_Reliving", "name": "date_of_Reliving", "title": "Last Working Date", "autoWidth": true },
                    { "data": "date_of_ndc_request", "name": "date_of_ndc_request", "title": "NDC Request Date", "autoWidth": true },
                    { "data": "reporting_manager", "name": "reporting_manager", "title": "Reporting Manager Name", "autoWidth": true },
                    { "data": "ndc_department", "name": "ndc_department", "title": "NDC Department", "autoWidth": true },
                    { "data": "ndc_department_authority_name", "name": "ndc_department_authority_name", "title": "NDC Department Authority Name", "autoWidth": true },
                    { "data": "ndc_department_outstanding", "name": "ndc_department_outstanding", "title": "Outstanding", "autoWidth": true },
                    { "data": "ndc_department_remark", "name": "ndc_department_remark", "title": "Remarks", "autoWidth": true },
                    { "data": "ndc_department_status", "name": "ndc_department_status", "title": "Status", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.machine_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });



            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });

}

