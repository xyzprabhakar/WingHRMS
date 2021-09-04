$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        $("#openingdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,

        });
        $("#checkbtn").hide();
        $('#btnupdate').hide();
        //bind bank & Branch too
        BindBankList('bank', 0);
        BindBranchList('branch', 0);

        //$('#btnreset').hide();




        GetData();

        $('#loader').hide();

    }, 2000);// end timeout

});

$('#btnreset').bind('click', function () {
    window.location.href = '/Payroll/EmployeeAccount';

});

function GetEditData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_empacc/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            $('#bank').val(data.bank_name);
            $('#branch').val(data.branch_id);
            $('#Employeeaccno').val(data.emp_ac_no);
            $('#openingdate').val(data.opn_date);

            $('#btnupdate').show();
            $("#checkbtn").show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
        }
    });
}

$("#btnupdate").bind("click", function () {
    // debugger;
    $('#loader').show();
    var bank = $('#bank').val();
    var branch = $('#branch').val();
    var companyacc = $('#Employeeaccno').val();
    var openingdate = $('#openingdate').val();

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

    if (bank == '' || bank == '0') {
        errormsg = errormsg + 'Please select Bank !! <br />';
        iserror = true;
    }
    if (companyacc == '' || companyacc == '0') {
        errormsg = errormsg + 'Please enter Employee Account no. !! <br />';
        iserror = true;
    }
    if (openingdate == '' || openingdate == null) {
        errormsg = errormsg + 'Please select Opening Date!! <br />';
        iserror = true;
    }
    if (branch == '' || branch == null) {
        errormsg = errormsg + 'Please select the Branch !! <br />';
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {

        'bank_name': bank,
        'branch_id': branch,
        'emp_ac_no': companyacc,
        'opn_date': openingdate,
        'created_by': login_emp_id,
        'modified_by': login_emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_EmpaccMaster';
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
                $('#bank').val('');
                $('#openingdate').val('');
                $('#branch').val('');
                $('#Employeeaccno').val('0');
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
        error: function (err, exception) {
            $('#loader').hide();
            _GUID_New();
            alert('error found');
        }
    });

});

$('#btnsave').bind("click", function () {
    $('#loader').show();
    // debugger;
    var bank = $('#bank').val();
    var branch = $('#branch').val();
    var companyacc = $('#Employeeaccno').val();
    var openingdate = $('#openingdate').val();

    //var view = 0;
    var errormsg = '';
    var iserror = false;

    //validation part

    if (bank == '' || bank == '0') {
        errormsg = errormsg + 'Please select Bank !! <br />';
        iserror = true;
    }
    if (companyacc == '' || companyacc == '0') {
        errormsg = errormsg + 'Please enter Employee Account no. !! <br />';
        iserror = true;
    }
    if (openingdate == '' || openingdate == null) {
        errormsg = errormsg + 'Please select Opening Date!! <br />';
        iserror = true;
    }
    if (branch == '' || branch == null) {
        errormsg = errormsg + 'Please select the Branch !! <br />';
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {

        'bank_name': bank,
        'branch_id': branch,
        'emp_ac_no': companyacc,
        'opn_date': openingdate,
        'created_by': login_emp_id,
        'modified_by': login_emp_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_EmployeeAccount';
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
                $('#bank').val('');
                $('#openingdate').val('');
                $('#branch').val('');
                $('#Employeeaccno').val('0');
                GetData();
                messageBox("success", Msg);
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err, exception) {
            $('#loader').hide();
            _GUID_New();
            alert('error found');
        }
    });
});

function GetData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_empacc/0'; //change link here

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $("#empmstr").DataTable({
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
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [1],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": "bank_name", "name": "bank_name", "autoWidth": true },
                    { "data": "branch_name", "name": "branch_name", "autoWidth": true },
                    { "data": "emp_ac_no", "name": "emp_ac_no", "autoWidth": true },
                    { "data": "opn_date", "name": "opn_date", "autoWidth": true },
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
            console.log("error");
        }
    });

}