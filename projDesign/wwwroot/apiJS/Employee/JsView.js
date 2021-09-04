
//var HaveDisplay;
var arr = new Array("1", "101");

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        var login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (!arr.includes(login_role_id)) {
            $("#ddlEmpType").hide();
        }

        // HaveDisplay = ISDisplayMenu("Display Company List");
        $("#ddlEmpType").val(1);
        GetAllEmployee(1);

        $("#ddlEmpType").bind("change", function () {
            debugger;
            var empType = $("#ddlEmpType").val();
            GetAllEmployee(empType);
        });

    }, 2000);// end timeout

});

function GetAllEmployee(empType) {
    // // debugger;

    var apiurl_ = "";

    //var company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //if (HaveDisplay == 0) {
    //    apiurl_ = localStorage.getItem("ApiUrl") + 'apiEmployee/GetAllEmployeeByCompany/' + company_id;
    //}
    //else {
    apiurl_ = localStorage.getItem("ApiUrl") + 'apiEmployee/GetAllEmployee/' + empType;
    //}
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl_,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            // // debugger;
            $("#tblEmployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee List',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    },
                ],
                "columnDefs":
                    [
                        //{
                        //    targets: [5],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [0],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    //{ "data": "company_name", "name": "company_name", "title":"Company","autoWidth": true },
                    { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "employee_first_name", "name": "employee_first_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "designation_name", "name": "designation_name", "title": "Designation", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "title": "Grade", "autoWidth": true },
                    //{ "data": "card_number", "name": "card_number","title":"Card No.", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "title": "Date of Joining", "autoWidth": true },
                    //{ "data": "date_of_birth", "name": "date_of_birth","title":"Date of Birth", "autoWidth": true },
                    { "data": "official_email_id", "name": "official_email_id", "title": "Email ID", "autoWidth": true },
                    { "data": "username", "name": "username", "title": "User Name", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[50, -1], [50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
            return false;
        }
    });

}

