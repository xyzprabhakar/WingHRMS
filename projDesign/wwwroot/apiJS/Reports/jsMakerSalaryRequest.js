
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
    //debugger;
    var apiurl = localStorage.getItem("ApiUrl") + "apiPayroll/GetSalaryReqReport"

    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }


            $("#tblSalRequest").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                "aaData": aData,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Maker Salary Request Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                        }
                    },
                ],
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],
                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "applicable_from_dt", "name": "applicable_from_dt", "title": "Applicable From", "autoWidth": true },
                    { "data": "gross_salary", "name": "GROSS SALARY", "title": "GROSS SALARY", "autoWidth": true },
                    { "data": "ctc", "name": "CTC", "title": "CTC", "autoWidth": true },
                    { "data": "basic", "name": "BASIC", "title": "BASIC", "autoWidth": true },
                    { "data": "hra", "name": "HRA", "title": "HRA", "autoWidth": true },
                    { "data": "conveyance", "name": "CONVEYANCE", "title": "CONVEYANCE", "autoWidth": true },
                    { "data": "special_allowance", "name": "SPECIAL ALLOWANCE", "title": "SPECIAL ALLOWANCE", "autoWidth": true },
                    { "data": "da", "name": "DA", "title": "DA", "autoWidth": true },
                    { "data": "net", "name": "NET", "title": "NET", "autoWidth": true },
                    { "data": "esic", "name": "ESIC", "title": "ESIC", "autoWidth": true },
                    { "data": "pf", "name": "PF", "title": "PF", "autoWidth": true },
                    { "data": "vpf", "name": "VPF", "title": "VPF", "autoWidth": true },
                    { "data": "total_deduction", "name": "TOTAL DEDUCTION", "title": "TOTAL DEDUCTION", "autoWidth": true },
                    { "data": "er_pf", "name": "ER PF", "title": "ER PF", "autoWidth": true },
                    { "data": "maker_remark", "name": "maker_remark", "title": "Remarks", "autoWidth": true },

                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
            $('#loader').hide();
        },
        error: function (error) {
            //debugger;
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
    $('#loader').hide();
}
