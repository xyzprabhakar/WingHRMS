$('#loader').show();
var company_id;
var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            GetLeaveBalance();
        });

    }, 2000);// end timeout


});

function GetLeaveBalance() {
    $('#loader').show();
    var emp_id = $("#ddlemployee").val();

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

    if (companyid == null || companyid == '' || companyid == 0) {
        messageBox('info', 'Please select company...!');
        return false;
    }
    if (emp_id == null || emp_id == '') {
        messageBox('info', 'Please select to employee!!');
        return false;
    }
    var from_date = $("#dtpFromDt").val();
    var to_date = $("#dtpToDt").val();

    var BulkEmpID = [];
    var for_all_emp = 0;

    if (emp_id == -1) {
        for_all_emp = 1;
        var options_ = $("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return $(this).val(); }).get();

        BulkEmpID = options_;

    }
    else {
        BulkEmpID.push(emp_id);
    }


    var mydata = {
        'empdtl': BulkEmpID,
        'all_emp': for_all_emp,
        'from_date': from_date,
        'to_date': to_date
    }
    var from_date = $("#dtpFromDt").val();
    var to_date = $("#dtpToDt").val();


    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiLeave/GetLeaveBalances",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            //  debugger;
            var res = response;
            $("#tblleavebalance").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true,
                "orderMulti": false,
                "scrollX": 200,
                "aaData": res,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Leave Balance',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7]
                        }
                    },
                ],
                "columnDefs": [
                    //{
                    //targets: [5],
                    //render: function (data, type, row) {
                    //    return data == '1' ? 'Leave add by System' : '2' ? 'Consume' : '3' ? 'Expired' : '4' ? 'Cash' : '5' ? 'Manual add' : '6' ? 'Manual delete' : '100' ? 'Previous leave credit by system' : '7' ? 'Deleted by system' :'InActive'
                    //}
                    //},
                ],
                "columns": [
                    { "data": null, "title": "S.No", "autowidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autowidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autowidth": true },
                    {
                        "title": "Leave Type", "autoWidth": true, "render": function (row, full, data) {

                            return '<a href="/View/LeaveDetail?leave_type=' + CryptoJS.AES.encrypt("'" + data.leave_type_id + "," + data.e_id + "'", localStorage.getItem("sit_id")) + '">' + data.leave_type_name + '</a>'
                        }
                    },
                    { "data": "openingBalance", "name": "openingBalance", "title": "Opening", "autowidth": true },
                    { "data": "credit", "name": "credit", "title": "Credit", "autowidth": true },
                    { "data": "dredit", "name": "dredit", "title": "Debit", "autowidth": true },
                    // { "data": "transaction_type", "name": "transaction_type", "title": "Transaction Type", "autowidth": true },
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
                "lengthMenu": [[20, 50, -1], [20, 50, "All"]]
            });

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });

}