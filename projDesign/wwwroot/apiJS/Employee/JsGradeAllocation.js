$('#loader').show();

var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //var HaveDisplay = ISDisplayMenu("Display Company List");
        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));

        }


        if (localStorage.getItem("new_emp_id") != null) {

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeeGradeAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }
        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            $("#ddlgrade").val(0);
            $("#allocfromdate").val('');
            $("#alloctodate").val('');
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            GetEmployeeGradeAllocation($(this).val());
        });



        $('#loader').hide();

    }, 2000);// end timeout

});

//////////////////////////////////////////////////// START GRADE /////////////////////////////////////////////////////


$('#btnSaveGradeAllocation').bind("click", function () {

    $('#loader').show();

    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var ddlgrade = $("#ddlgrade").val();
    var allocfromdate = $("#allocfromdate").val();
    var alloctodate = $("#alloctodate").val();

    if (ddlcompany == "0" || ddlcompany == null) {
        messageBox("error", "Please Select Company ....!");
        $("#ddlCompany").val('');
        $('#loader').hide();
        return;
    }
    if (employee_id == "0" || employee_id == null || employee_id == "") {
        messageBox("error", "Please Select Employee Code ....!");
        $("#ddlEmployeeCode").val('');
        $('#loader').hide();
        return;
    }
    if (ddlgrade == "0" || ddlgrade == null) {
        messageBox("error", "Please Select Grade ....!");
        $("#ddlgrade").val('');
        $('#loader').hide();
        return;
    }
    if (allocfromdate == "0" || allocfromdate == null || allocfromdate == "") {
        messageBox("error", "Enter Date From ....!");
        $("#allocfromdate").val('');
        $('#loader').hide();
        return;
    }
    if (alloctodate == "0" || alloctodate == null || alloctodate == "") {
        messageBox("error", "Enter To Date ....!");
        $("#alloctodate").val('');
        $('#loader').hide();
        return;
    }


    var myData = {
        'employee_id': employee_id,
        'company_id': ddlcompany,
        'grade_id': ddlgrade,
        'applicable_from_date': allocfromdate,
        'applicable_to_date': alloctodate
    };

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeGradeAllocation',
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
                alert(Msg);
                location.reload();
                //window.location.href = '/Employee/GradeAllocation';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");
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
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);

        }
    });

});



function GetEmployeeGradeAllocation(employee_id) {
    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeGradeAllocation/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {

                //(JSON.stringify(data));
                var res = data;
                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }
                //$("#ddlCompany").val(res.ddlcompany);
                //$("#ddlEmployeeCode").val(res.employee_id);

                if (res.grade_id != null) {
                    BindGradeList('ddlgrade', $("#ddlCompany").val(), res.grade_id);
                    $("#ddlgrade").val(res.grade_id);
                    $("#allocfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
                    $("#alloctodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
                }
                else {
                    BindGradeList('ddlgrade', $("#ddlCompany").val(), 0);
                    $("#allocfromdate").val('');
                    $("#alloctodate").val('');
                }


                $('#loader').hide();

            },
            error: function (error) {
                messageBox("error", error.responseText);
                $('#loader').hide();
            }
        });

    }


}




//////////////////////////////////////////////////// END GRADE /////////////////////////////////////////////////////

