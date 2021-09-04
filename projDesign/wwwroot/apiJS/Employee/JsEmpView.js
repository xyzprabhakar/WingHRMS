$('#loader').show();

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetAllEmployee(default_company);

        $('#loader').hide();

    }, 2000);// end timeout

});

function GetAllEmployee(company_id) {
    // // debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiEmployee/GetAllEmployee",
        // url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetAllEmployeeByCompany/' + company_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $('#loader').hide();
            // // debugger;
            $("#tblEmployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee List',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                ],
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
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "employee_first_name", "name": "employee_first_name", "autoWidth": true },
                    { "data": "employee_code", "name": "employee_code", "autoWidth": true },
                    { "data": "card_number", "name": "card_number", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "autoWidth": true },
                    { "data": "date_of_birth", "name": "date_of_birth", "autoWidth": true },
                    { "data": "official_email_id", "name": "official_email_id", "autoWidth": true },
                    { "data": "username", "name": "username", "autoWidth": true },
                    { "data": "password", "name": "password", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[50, -1], [50, "All"]]

            });
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}
