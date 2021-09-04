$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindCompanyList('ddlCompany', 0);

        $('#ddlCompany').bind("change", function () {
            BindDepartmentListForddl('ddldepartment', $(this).val(), 0);
        });

        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();

    }, 2000);// end timeout

});

// Save Batch
$('#btnsave').bind("click", function () {
    $('#loader').show();
    var txtBatchName = $("#txtBatchName").val();
    var ddlCompany = $("#ddlCompany").val();
    var ddldepartment = $("#ddldepartment").val();
    var txtFromDate = $("#txtFromDate").val();
    var txtToDate = $("#txtToDate").val();


    //Validation
    if (txtBatchName == '') {
        messageBox("error", "Please Enter Batch Name...!");
        $('#loader').hide();
        return;
    }

    if (ddlCompany == '' && ddlCompany == 0) {
        messageBox("error", "Please Select Company...!");
        $('#loader').hide();
        return;
    }

    if (ddldepartment == '' && ddldepartment == 0) {
        ddldepartment = '';
    }
    if (txtFromDate == '') {
        messageBox("error", "Please Select From Date...!");
        $('#loader').hide();
        return;
    }

    if (txtToDate == '') {
        messageBox("error", "Please Select To Date...!");
        $('#loader').hide();
        return;
    }

    var myData = {
        'batch_name': txtBatchName,
        'company_id': ddlCompany,
        'dept_id': ddldepartment,
        'txtFromDate': txtFromDate,
        'txtToDate': txtToDate,
        'is_deleted': 0,
        'created_by': login_emp_id,
        'last_modified_by': login_emp_id
    };

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiPayroll/Save_Batch",
        type: "POST",
        data: JSON.stringify(myData),
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            //if data save
            if (statuscode == "1") {
                messageBox("success", Msg);
                $("#txtBatchName").val('');
                BindCompanyList('ddlCompany', 0);
                BindDepartmentListForddl('ddldepartment', 0, 0);
                $("#txtFromDate").val('');
                $("#txtToDate").val('');
            }
            else if (statuscode == "0") {
                messageBox("info", Msg);
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

});