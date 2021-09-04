$('#loader').show();
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetLeaveBalance(login_emp_id);

        $('#loader').hide();

    }, 2000);// end timeout

});


function GetLeaveBalance(login_emp_id) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveBalancesByEmpID/" + login_emp_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem("Token") },
        success: function (response) {
            var res = response;
            $("#tblleavebalance").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true,
                "orderMulti": false,
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "S.No", "autowidth": true },
                    // { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autowidth": true },
                    { "data": "leave_type_id", "name": "leave_type_id", "visible": false },
                    {
                        "title": "Leave Type", "autoWidth": true, "render": function (row, full, data) {
                            return '<a href="/View/LeaveDetail?leave_type=' + CryptoJS.AES.encrypt("'" + data.leave_type_id + "," + data.e_id + "'", localStorage.getItem("sit_id")) + '">' + data.leave_type_name + '</a>'
                        }
                    },
                    //{ "data": "leave_type_name", "name": "leave_type_name", "title": "Leave Type", "autowidth": true },
                    { "data": "credit", "name": "credit", "title": "Credit", "autowidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Debit", "autowidth": true },
                    {
                        "title": "Balance", "autowidth": true,
                        render: function (row, full, data) {
                            if (data.balance >= 0) {
                                return "<label>" + data.balance + "</label>";
                            }
                            else {
                                return "<label>0</label>";
                            }
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}