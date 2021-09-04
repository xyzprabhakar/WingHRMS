var company_id;
var employee_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        employee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindCompanyList('ddlCompany', company_id);

        // BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, employee_id);

        BindEmployeeCodee('ddlEmployeeCode', company_id, employee_id);

        // BindGradeList('ddlgrade', company_id, 0);

        GetEmployeeGradeAlloc(employee_id);


    }, 2000);// end timeout

});



function GetEmployeeGradeAlloc(employee_id) {
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeGradeAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;

            BindGradeList('ddlgrade', company_id, res.grade_id);

            $("#allocfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
            $("#alloctodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
            $('#loader').hide();

        },
        error: function (error) {
            $('#loader').hide();

            messageBox("error", "Server busy please try again later...!");
        }
    });

}


function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}


