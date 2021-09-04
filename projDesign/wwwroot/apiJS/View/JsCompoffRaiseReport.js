$("#loader").show();
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

        $("#loader").hide();

        $("#btnreset").bind("click", function () {
            window.location.reload();
        });

        $("#btnGet").bind("click", function () {
            var from_date = $("#dtpFromDt").val();

            var to_date = $("#dtpToDt").val();

            var emp_id = $("#ddlemployee").val();

            var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

            if (companyid == null || companyid == '' || companyid == 0) {
                messageBox('info', 'Please select company...!');
                return false;
            }

            if (from_date == null || from_date == '') {
                messageBox('info', 'Please select from date...!');
                return false;
            }
            if (to_date == null || to_date == '') {
                messageBox('info', 'Please select to date...!');
                return false;
            }
            if (emp_id == null || emp_id == '') {
                messageBox('info', 'Please select to employee!!');
                return false;
            }


            if (new Date(to_date) < new Date(from_date)) {
                messageBox('info', 'To Date must be greater than from date');
                return false;
            }

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
                'from_date': from_date,
                'to_date': to_date,
                'all_emp': for_all_emp,

            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            $("#loader").show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + '/apiMasters/GetCompoffRaisedReport/',
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    _GUID_New();
                    var res = response;
                    // var lengg = response.length;
                    if (res.statusCode != undefined) {
                        messageBox("info", res.message);
                        $("#loader").hide();
                        return false;
                    }


                    $("#tblcompoffdtl").DataTable({
                        "processing": true,//to show progress bar
                        "serverSide": false,// to process server side
                        "scrollX": 200,
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        dom: 'lBfrtip',
                        buttons: [
                            {
                                text: 'Export to Excel',
                                title: 'Comp off Credit Report From : (' + GetDateFormatddMMyyyy(new Date(from_date)) + ') TO : (' + GetDateFormatddMMyyyy(new Date(to_date)) + ')',
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
                                }
                            },
                        ],
                        "aaData": res,
                        "columnDefs": [

                            {
                                targets: [6],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetDateFormatddMMyyyy(date);
                                }
                            },
                            {
                                targets: [11],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetDateFormatddMMyyyy(date);
                                }
                            },
                            {
                                targets: [8],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetTimeFromDate(date);
                                }
                            },
                            {
                                targets: [9],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetTimeFromDate(date);
                                }
                            },
                            {
                                targets: [15],
                                render: function (data, type, row) {

                                    return row.is_deleted == "0" ? (data == "0" ? "Pending" : data == "1" ? "Approve" : data == "2" ? "Rejected" : "") : "Deleted";
                                }
                            },
                            {
                                targets: [13],
                                render: function (data, type, row) {
                                    var date = new Date(data);
                                    return data == "2000-01-01T00:00:00" ? " " : GetDateFormatddMMyyyy(date);
                                }
                            },

                        ],
                        "columns": [
                            { "data": null, "title": "SNo", "autoWidth": true },
                            { "data": "req_code", "name": "req_code", "title": "Employee Code", "autoWidth": true },
                            { "data": "req_name", "name": "req_name", "title": "Employee Name", "autoWidth": true },
                            { "title": "Department", "data": "dept_name", "name": "dept_name", "autoWidth": true },
                            { "title": "Designation", "data": "designation", "name": "designation", "autoWidth": true },
                            { "title": "Location", "data": "location", "name": "location", "autoWidth": true },
                            { "data": "comp_off_date", "name": "comp_off_date", "title": "Compoff Date", "autoWidth": true },
                            { "data": "comp_off_day", "name": "comp_off_day", "title": "Compoff Day", "autoWidth": true },
                            //         { "data": "duration", "name": "duration", "title": "Duration", "autoWidth": true },
                            { "data": "actual_in_time", "name": "actual_in_time", "title": "Actual In Time", "autoWidth": true },
                            { "data": "actual_out_time", "name": "actual_out_time", "title": "Actual Out Time", "autoWidth": true },
                            { "data": "tot_working_hours", "name": "tot_working_hours", "title": "Total Working Hours", "autoWidth": true },
                            { "data": "created_date", "title": "Created On", "name": "created_date", "autoWidth": true },
                            { "data": "requester_remarks", "title": "Request Remarks", "name": "requester_remarks", "autoWidth": true },
                            { "data": "approved_on", "title": "Approved On", "name": "approved_on", "autoWidth": true },
                            { "data": "approved_by", "title": "Approved By", "name": "approved_by", "autoWidth": true },
                            { "data": "is_final_approve", "name": "is_final_approve", "title": "Final Status", "autoWidth": true },
                            { "data": "approver_remarks", "title": "Approver Remarks", "name": "approver_remarks", "autoWidth": true },
                        ],
                        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                            return nRow;
                        },
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
                    });

                    $("#loader").hide();

                },
                error: function (err) {
                    _GUID_New();
                    $("#loader").hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            });

        });

    }, 2000);// end timeout

});


