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

        BindAllEmp_Company('ddlCompany', employee_id, company_id);

        // BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, employee_id);

        BindEmployeeCodee('ddlEmployeeCode', company_id, employee_id);

        //BindDesignationList('ddldesignation', company_id, 0)

        GetEmployeeDesignationAlloc(employee_id);


    }, 2000);// end timeout

});


function GetEmployeeDesignationAlloc(employee_id) {
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeDesignationAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            //(JSON.stringify(data));
            var res = data;

            if (res.statusCode == undefined && res.desig_id != null) {
                BindDesignationList('ddldesignation', company_id, res.desig_id);
                $("#Desigfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
                $("#Desigtodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
            }
            else {
                $("#Desigfromdate").val('');
                $("#Desigtodate").val('');
            }
            $('#loader').hide();


        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
        }
    });

}


function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}
