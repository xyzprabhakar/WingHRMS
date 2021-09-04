$('#loader').show();
var login_emp_id;
var companyid;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindCompanyListAll('ddlcompany', login_emp_id, companyid);
        //BindOnlyProbation_Confirmed_emp('ddlemployee', companyid, 0);
        GetData(companyid, login_emp_id);



        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {
            $('#loader').show();
            if ($.fn.DataTable.isDataTable('#tblsalaryslip')) {
                $('#tblsalaryslip').DataTable().clear().draw();
            }

            // BindOnlyProbation_Confirmed_emp('ddlemployee', $(this).val(), 0);

            GetData($(this).val(), login_emp_id);

            $('#loader').hide();
        });

    }, 2000);// end timeout

});


//$("#ddlemployee").bind("change", function () {
//    if ($.fn.DataTable.isDataTable('#tblsalaryslip')) {
//        $('#tblsalaryslip').DataTable().clear().draw();
//    }
//    //if ($(this).val() == 0) {
//    //    messageBox("error", "Please select employee");
//    //    return false;
//    //}
//    GetData($("#ddlcompany").val(), $(this).val());

//});



function GetData(companyid, employeeid) {

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_SalarySlip/" + companyid + "/" + employeeid,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        async: false,
        headers: headerss,
        success: function (response) {
            var res = response;
            _GUID_New();

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tblsalaryslip").DataTable({
                "processing": true,// for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true, // for Distroy previous data
                "filter": true, //this is for disable filter(search box)
                "orderMulti": false, // for disable multiple column at once,
                "scrollX": 150,
                "aaData": res,
                "columnDefs": [

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
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autowidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autowidth": true },
                    { "data": "payroll_month_year", "name": "payroll_month_year", "title": "Payroll YearMonth", "autowidth": true },
                    //{ "data": "is_freezed", "name": "is_freezed", "title": "Is Freezed", "autowidth": true },
                    //{ "data": "is_lock", "name": "is_lock", "title": "Is Lock", "autowidth": true },
                    //{ "data": "payroll_status", "name": "payroll_status", "title": "Payroll Status", "autowidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created Date", "autowidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "title": "Modified Date", "autowidth": true },
                    {
                        "title": "View", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewSalarySlip(' + full.emp_id + ',' + full.payroll_month_year + ')" >View</a>';

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
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            alert(err.responseText);
        }
    });
}

function ViewSalarySlip(employeeid, monthyear) {

    //var headerss = {};
    //headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/View_Employee_SalarySlip/" + employeeid + "/" + monthyear,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            if (response.statusCode == 1) {
                alert(response.message);
                return;
            }
            window.open(response.path);
        },
        error: function (err) {
            //_GUID_New();
            alert(err.responseText);
        }
    });
}