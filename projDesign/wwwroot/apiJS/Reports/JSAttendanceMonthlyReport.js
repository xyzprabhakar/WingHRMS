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

            GetData();
        });


    }, 2000);// end timeout

});


function GetData() {
    $('#loader').show();
    var emp_id = $("#ddlemployee").val();

    var txtMonthYear = $("#txtMonthYear").val();

    if (txtMonthYear == null || txtMonthYear == '') {
        $('#loader').hide();
        messageBox('info', 'Please select month...!');
        return false;
    }
    else {
        var txtmnyr = new Date($('#txtMonthYear').val());
        var yr = txtmnyr.getFullYear();
        var mnth = txtmnyr.getMonth() + 1;
        txtMonthYear = yr + "-" + mnth;
    }

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

    if (companyid == null || companyid == '' || companyid == 0) {
        $('#loader').hide();
        messageBox('info', 'Please select company...!');
        return false;
    }
    if (emp_id == null || emp_id == '') {
        $('#loader').hide();
        messageBox('info', 'Please select to employee!!');
        return false;
    }
    $('#btnsave').hide();

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
        'from_date': txtMonthYear,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeAttendanceMonthlyReport';

    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            _GUID_New();


            var data = res.list;
            var column = res.column;


            $('#loader').hide();
            $("#tblleaveapp").DataTable({
                "processing": false, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Attendance Monthly Report - ' + GetDateFormatddMMyyyy(new Date(txtMonthYear)),
                        extend: 'excelHtml5'
                    }
                ],
                "aaData": data,
                "columnDefs":
                    [
                        {
                            targets: [4],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": column,
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],

            });



        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });


}
