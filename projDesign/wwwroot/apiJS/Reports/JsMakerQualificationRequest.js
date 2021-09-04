
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        //debugger;
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
    // debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeQualificationReq/',
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //   debugger;
            $('#loader').hide();

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            //// debugger;;
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
                        title: 'Maker Qualifacation Request Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    }
                ],
                "columnDefs":
                    [
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                        //{
                        //    targets: [5],
                        //    "class": "text-center"

                        //}
                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autoWdith": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "board_or_university", "name": "board_or_university", "title": "Board/Universty", "autoWidth": true },
                    { "data": "institute_or_school", "name": "institute_or_school", "title": "Institue/School", "autoWidth": true },
                    { "data": "passing_year", "name": "passing_year", "title": "Passign Year", "autoWidth": true },
                    { "data": "stream", "name": "stream", "title": "Course", "autoWidth": true },
                    { "data": "education_level_", "name": "education_level_", "title": "Education Level", "autoWidth": true },
                    { "data": "education_type_", "name": "education_type_", "title": "Education Type", "autoWdith": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });


}
