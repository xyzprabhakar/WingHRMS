
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        $('#loader').show();
        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#loader').hide();

        GetData();

    }, 2000);// end timeout

});


function GetData() {
    //debugger;
    var apiurl = localStorage.getItem("ApiUrl") + "apiMasters/Get_MobileApplicationAccess"

    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblReport").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Mobile Application Access Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6]
                        }
                    },
                ],
                "columnDefs": [],
                "columns": [
                    { "data": "emp_code", "title": "Employee Code", "name": "emp_code", "class": "text-center", "autoWidth": true },
                    { "data": "emp_name", "title": "Employee Name", "name": "emp_name", "class": "text-center", "autoWidth": true },
                    { "data": "company", "title": "Company", "name": "company", "class": "text-center", "autoWidth": true },
                    { "data": "department", "title": "Department", "name": "department", "class": "text-center", "autoWidth": true },
                    { "data": "location", "title": "Location", "name": "location", "class": "text-center", "autoWidth": true },
                    { "data": "mobile_access", "title": "Mobile Application Access", "name": "mobile_access", "class": "text-center", "autoWidth": true },
                    { "data": "mobile_attendence_access", "title": "Mobile Attendence Access", "name": "mobile_attendence_access", "class": "text-center", "autoWidth": true },
                ],
                'order': [[1, 'asc']],
                "select": 'multi',
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, "All"]],
            });

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}
