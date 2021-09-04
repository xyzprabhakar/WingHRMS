$('#loader').show();
var company_id;
var login_emp_id;
var emp_leave;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split("=");

        emp_leave = CryptoJS.AES.decrypt(url[1], localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindEmployeeCodee('ddlEmployeeCode', emp_leave.split(',')[1]);

        //    BinddEmployeeCodee('ddlEmployeeCode', company_id, login_emp_id);

        $('#ddlEmployeeCode').prop("disabled", "disabled");


        $('#loader').hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnGet").bind("click", function () {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            var _leavetypeid = url[0].split('=')[1];


            //var regex = new RegExp("^[0-9]+$");
            //var key = String.fromCharCode(!_leavetypeid.charCode ? _leavetypeid.which : _leavetypeid.charCode);
            //if (!regex.test(key)) {
            //    _leavetypeid = _leavetypeid.replace("#", "");
            //}


            var fromdate = $("#txtfromdate").val();
            var todate = $("#txttodate").val();
            var iserror = false;
            var errormsg = "";
            if (fromdate == null || fromdate == "") {
                iserror = true;
                errormsg = errormsg + "Please Select Leave From Date</br>";
            }
            if (todate == null || todate == "") {
                iserror = true;
                errormsg = errormsg + "Please Select Leave to Date</br>";
            }


            if (new Date(todate) < new Date(fromdate)) {
                iserror = true;
                errormsg = errormsg + "To Date must be greater than from date </br>";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiLeave/LeaveDetailByEmpID/" + emp_leave.split(',')[0] + "/" + emp_leave.split(',')[1] + "/" + fromdate + "/" + todate,
                type: "GET",
                contentType: "application/json",
                data: {},
                dataType: "json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (response) {
                    var res = response;

                    var _leavedata = res.leavedata;
                    var _total = res.leaveledgertotal;

                    $('#loader').hide();
                    $("#tblleavedetail").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        //"scrollY": 200,
                        "aaData": _leavedata,
                        "columnDefs": [
                            {
                                targets: [3],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetDateFormatddMMyyyy(date);
                                }
                            }
                        ],
                        "columns": [
                            { "data": null, "title": "SNo.", "autoWidth": true },
                            { "data": "remarks", "name": "remarks", "title": "Description", "autoWidth": true },
                            { "data": "sno", "name": "sno", "autoWidth": true, "visible": false },
                            { "data": "transaction_date", "name": "transaction_date", "title": "Date", "autoWidth": true },
                            { "data": "credit", "name": "credit", "title": "Credit", "autoWidth": true },
                            { "data": "dredit", "name": "dredit", "title": "Debit", "autoWidth": true },
                            {
                                "title": "viewDetail", "autoWidth": true, "render": function (row, full, data) {
                                    //var datee = GetDateFormatddMMyyyy(new Date(data.transaction_date));
                                    return '<a href="#" class="btn btnSaveR">View Details</a>';

                                }
                            }
                        ],
                        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                            return nRow;
                        }, // for S.No
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                    });


                    $(document).on("click", ".btnSaveR", function () {

                        var currentRow = $(this).closest("tr");

                        var data = $('#tblleavedetail').DataTable().row(currentRow).data();

                        var snoo = data['sno'];
                        var transactionn_date = data["transaction_date"];

                        ViewLeaveRequest(snoo, transactionn_date);

                        // AddCompoffForRequest(emp_id, compoffsno, compoffdate, txtremarks);
                    });

                },
                error: function (err) {
                    $('#loader').hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            });

        });


    }, 2000);// end timeout

});

function ViewLeaveRequest(sno, _leavedate) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiLeave/GetLeaveApplicationByEmpIDandDate/" + emp_leave.split(',')[1] + "/" + sno + "/" + _leavedate,
        type: "GET",
        contentType: "application/json",
        data: {},
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            var statuscode = res.statusCode
            $('#loader').hide();

            if (statuscode == undefined) {
                var modal = document.getElementById("myleaveModal");
                $('#myleaveModal').modal({

                    show: true
                });
                $("#lblemployeecode").val(res.emp_code);
                $("#lblrequester_empname").val(res.emp_name);
                $("#lblleavetype").val(res.leave_applicable_for == "1" ? "Full Day" : res.leave_applicable_for == "2" ? "Half Day" : "");
                $("#lbldaypart").val(res.day_part == "1" ? "First Half" : res.day_part == "2" ? "Second Half" : "");
                $("#lblfromdate").val(GetDateFormatddMMyyyy(new Date(res.from_date)));
                $("#lbltodate").val(GetDateFormatddMMyyyy(new Date(res.to_date)));
                $("#lblleaveqty").val(res.leave_qty);
                $("#lblreason").val(res.requester_remarks);
                $("#lblapprover1_name").val(res.approver1_);
                $("#tblapprover1_reamarks").val(res.approval1_remarks);
                $("#lblapprover2_name").val(res.approver2_);
                $("#lblapprover2_remarks").val(res.approval2_remarks);
                $("#lblapprover3_name").val(res.approver3_);
                $("#lblapprover3_remarks").val(res.approval3_remarks);
                $("#lblapprover1_date").val(res.approval_date1 != "0001-01-01T00:00:00" && res.approval_date1 != "2000-01-1T00:00:00" ? GetDateFormatddMMyyyy(new Date(res.approval_date1)) : "");
                $("#lblapprover2_date").val(res.approval_date2 != "0001-01-01T00:00:00" && res.approval_date2 != "2000-01-01T00:00:00" ? GetDateFormatddMMyyyy(new Date(res.approval_date2)) : "");
                $("#lblapprover3_date").val(res.approval_date3 != "0001-01-01T00:00:00" && res.approval_date3 != "2000-01-01T00:00:00" ? GetDateFormatddMMyyyy(new Date(res.approval_date3)) : "");

                $("#txtadmin_name").val(res.admin_);
                $("#txtadmin_remarks").val(res.admin_remarks);
                $("#txtadmin_date").val(res.admin_ar_date != "0001-01-01T00:00:00" && res.admin_ar_date != "2000-01-01T00:00:00" ? GetDateFormatddMMyyyy(new Date(res.admin_ar_date)) : "");
            }
            else {
                messageBox("error", res.message);
                return false;
            }
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}



function BindEmployeeCodee(ControlId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}

