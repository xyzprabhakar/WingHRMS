$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

        $("#txteffdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,

        });
        //$('#btnreset').hide();
        $('#btnupdate').hide();
        $("#checkbtn").hide();

        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/LoanMaster';

        });

        $('#loader').hide();

        $("#btnupdate").bind("click", function () {
            // debugger;
            $('#loader').show();
            var roi = $("#txtroi").val();
            var minworkdays = $('#txtminworkdays').val();
            var maxloan = $('#maxloan').val();
            var nooftenure = $('#nooftenure').val();
            var empstatus = $('#ddlempstatus').val();
            var effective_dt = $('#txteffdate').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;
            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_deleted = 0;
                }
                else {
                    is_deleted = 1;
                }
            }

            //validation part
            if (roi == '' || roi == '0') {
                errormsg = "Please enter roi !! <br/>";
                iserror = true;
            }
            if (minworkdays == '' || minworkdays == null || minworkdays == '0') {
                errormsg = errormsg + 'Please enter Min work days !! <br />';
                iserror = true;
            }
            if (maxloan == '' || maxloan == '0') {
                errormsg = errormsg + 'Please enter Max Loan !! <br />';
                iserror = true;
            }
            if (empstatus == '' || empstatus == '0') {
                errormsg = errormsg + 'Please enter Max Loan !! <br />';
                iserror = true;
            }
            if (effective_dt == '' || effective_dt == null) {
                errormsg = errormsg + 'Please select effective Date !! <br />';
                iserror = true;
            }
            if (nooftenure == '' || nooftenure == null) {
                errormsg = errormsg + 'Please select effective Date !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'rate_of_interest': roi,
                'min_no_days_work': minworkdays,
                'max_loan': maxloan,
                'no_of_tenure': nooftenure,
                'emp_status': empstatus,
                'from_date': effective_dt,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_LoanMaster';
            var Obj = JSON.stringify(myData);
            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtroi").val('');
                        $('#txtminworkdays').val('');
                        $('#maxloan').val('');
                        $('#txteffdate').val('');
                        $('#nooftenure').val('');
                        $('#ddlempstatus').val('0');
                        $('#txteffdate').val('');
                        $('#btnupdate').hide();
                        $("#checkbtn").hide();
                        $('#btnsave').show();
                        GetData();
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert('error found');
                }
            });
        });

        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var roi = $("#txtroi").val();
            var minworkdays = $('#txtminworkdays').val();
            var maxloan = $('#maxloan').val();
            var nooftenure = $('#nooftenure').val();
            var empstatus = $('#ddlempstatus').val();
            var effective_dt = $('#txteffdate').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (roi == '' || roi == '0') {
                errormsg = "Please enter roi !! <br/>";
                iserror = true;
            }
            if (minworkdays == '' || minworkdays == null || minworkdays == '0') {
                errormsg = errormsg + 'Please enter Min work days !! <br />';
                iserror = true;
            }
            if (maxloan == '' || maxloan == '0') {
                errormsg = errormsg + 'Please enter Max Loan !! <br />';
                iserror = true;
            }
            if (empstatus == '' || empstatus == '0') {
                errormsg = errormsg + 'Please enter Max Loan !! <br />';
                iserror = true;
            }
            if (effective_dt == '' || effective_dt == null) {
                errormsg = errormsg + 'Please select effective Date !! <br />';
                iserror = true;
            }
            if (nooftenure == '' || nooftenure == null) {
                errormsg = errormsg + 'Please select effective Date !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'rate_of_interest': roi,
                'min_no_days_work': minworkdays,
                'max_loan': maxloan,
                'no_of_tenure': nooftenure,
                'emp_status': empstatus,
                'from_date': effective_dt,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/save_loanmaster';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#txtroi").val('');
                        $('#txtminworkdays').val('');
                        $('#maxloan').val('');
                        $('#txteffdate').val('');
                        $('#nooftenure').val('');
                        $('#ddlempstatus').val('0');
                        $('#txteffdate').val('');
                        GetData();
                        messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    _GUID_New();
                    alert('error found');
                }
            });
        });


    }, 2000);// end timeout

});


function GetEditData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_loanmaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            $("#txtroi").val(data.rate_of_interest);
            $('#txtminworkdays').val(data.min_no_days_work);
            $('#maxloan').val(data.max_loan);
            $('#nooftenure').val(data.no_of_tenure);
            $('#ddlempstatus').val(data.emp_status);
            $('#txteffdate').val(data.from_date);

            $('#btnupdate').show();
            $("#checkbtn").show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
        }
    });
}


function GetData() {
    // debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_loanmaster/0'; //change link here

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $('#loader').hide();
            $("#loanmstr").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [

                        //{
                        //    targets: [2],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        {
                            targets: [2],
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
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "rate_of_interest", "name": "rate_of_interest", "autoWidth": true },
                    { "data": "min_no_days_work", "name": "min_no_days_work", "autoWidth": true },
                    { "data": "max_loan", "name": "max_loan", "autoWidth": true },
                    { "data": "no_of_tenure", "name": "no_of_tenure", "autoWidth": true },
                    { "data": "emp_status", "name": "emp_status", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.map_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            //alert(error);
            $('#loader').hide();
            console.log("error");
        }
    });

}
