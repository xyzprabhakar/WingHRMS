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


        BindCompanyList('ddlcompany', company_id);

        BinddEmployeeCodee('ddlemployee', company_id, login_emp_id);

        //BindRequestDetail(0, company_id);

        BindRequestDetail(login_emp_id, company_id);

        $('#loader').hide();

    }, 2000);// end timeout

});


function BindRequestDetail(login_emp_id, companyId) {
    // debugger;
    var table = $('#tblrrstatus').DataTable();
    table.clear();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_ReimRquestDetail_forReport/' + login_emp_id + '/' + companyId,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblrrstatus").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? 'Yearly' : 'Monthly'
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {
                                return row.is_approvred == '0' && row.is_delete == 0 ? 'Pending' : data == "1" ? 'Accepted' : data == '2' ? 'Rejected' : row.is_approvred == '0' && row.is_delete == 1 ? 'Cancel' : ''
                            }
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "request_type", "name": "request_type", "autoWidth": true },
                    { "data": "fiscal_year_id", "name": "fiscal_year_id", "autoWidth": true },
                    { "data": "request_month_year", "name": "request_month_year", "autoWidth": true },
                    { "data": "total_request_amount", "name": "total_request_amount", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "autoWidth": true },
                    { "data": "is_approvred", "name": "is_approvred", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "autoWidth": true },


                    {
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewRequestDetail(' + full.rrm_id + ')" >View</a>';
                        }
                    },

                    {
                        render: function (data, type, full, row) {
                            if (full.is_approvred == 0 && full.is_delete == 0) {
                                return '<input type="hidden" id="rr_id" value="' + full.rrm_id + '" /><input style="width:120px" class="form-control"  id="txtRemarks" type="text" placeholder="Enter Remarks">&nbsp;&nbsp;<a href="#" class="btnCancelR">Cancel</a>';
                            }
                            else {
                                return '<lable></lable>';
                            }

                        }
                    },


                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });



            //Start for cancel Request

            $(document).on("click", ".btnCancelR", function () {

                var rrm_id = $(this).parents('tr').children('td').children('input[type="hidden"]').val();
                var txtRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();

                if (txtRemarks == '') {
                    messageBox("error", "Please enter Remarks");
                    return false;
                }

                //BindAcceptRejectRequest(rr_id, txtRemarks, 1);

                BindCancelRequest(rrm_id, txtRemarks);
            });



            //End for cancel Request
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function ViewRequestDetail(reim_request_master_id) {
    // debugger;
    if (reim_request_master_id == null || reim_request_master_id == '') {
        messageBox('info', 'There are some problem please try again later');
        return false;
    }

    var modal = document.getElementById("myModal");
    modal.style.display = "block";
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/View_ReimbursementRequestDetail/' + reim_request_master_id,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            var ress = response;

            _GUID_New();
            $("#tblreimb_req_dtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
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
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                    { "data": "bill_amount", "name": "bill_amount", "autoWidth": true },
                    { "data": "request_amount", "name": "request_amount", "autoWidth": true },
                    { "data": "bill_date", "name": "bill_date", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "autoWidth": true },
                ],
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

function BindCancelRequest(rrm_id, _remarkss) {
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    var mydata = {
        req_type: rrm_id, // get reimbursment request master id
        remarks: _remarkss,
        is_deleted: 1,
        modified_by: login_emp_id
    }
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Cancel_ReimbursementRequest',
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (response) {
            var statuscode = response.statusCode;
            var Msg = response.message;
            _GUID_New();
            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (request, status, error) {
            $('#loader').hide();
            _GUID_New();
            var error = "";
            var errordata = JSON.parse(request.responseText);
            try {
                var i = 0;
                while (Object.keys(errordata).length > i) {
                    var j = 0;
                    while (errordata[Object.keys(errordata)[i]].length > j) {
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);

        }

    });
}
