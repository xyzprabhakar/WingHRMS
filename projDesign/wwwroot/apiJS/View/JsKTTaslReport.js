var empid;
var company_id;

$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        GetData();


    }, 2000);// end timeout

});

function GetData() {
    $('#loader').show();

    $.ajax({
        type: "GET",
        // url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetails/" + company_id + "/" + empid ,
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_KT_Task_List_all",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();

                return false;
            }

            $("#tbl_kt_task_list").DataTable({
                "processing": true,//to show process bar
                "serverSide": false,// to process server side
                "orderMulti": false,// to disbale multiple column at once
                "bDestroy": true,//remove previous data
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'KT Task Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [0,1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                ],
                "columnDefs": [
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return data == '1' ? 'Active' : 'InActive'
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    
                ],
                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "handoverby", "name": "handoverby", "title": "Handover By", "autoWidth": true },
                    { "data": "task_name", "name": "task_name", "title": "Task Name", "autoWidth": true },
                  
                    {
                        "title": "Team Members", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            var handoverto = "";
                            debugger;
                            for (var i = 0; i < full.empnamelist.length; i++) {
                                handoverto += full.empnamelist[i].empname + ", ";
                            }
                            return handoverto.slice(0, -2);
                        }
                    },
                    { "data": "procedure", "name": "procedure", "title": "Procedure", "autoWidth": true },
                    { "data": "modhandover", "name": "modhandover", "title": "Mode of Handover", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    { "data": "status", "name": "status", "title": "Status as on Date", "autoWidth": true },
                    { "data": "handoverdate", "name": "handoverdate", "title": "Handover Date", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
            $('#loader').hide();

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}
 
