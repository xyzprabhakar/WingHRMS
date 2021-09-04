
var default_company;
var login_emp_id;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        //var HaveDisplay = ISDisplayMenu("Display Company List");
        $('#ddlCompany').prop("disabled", "disabled");

        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
        setSelect('ddlEmployeeCode', login_emp_id);
        //BindEmployeeCodee('ddlEmployeeCode', default_company, login_emp_id);
        GetEmployeeManagerAllocation(login_emp_id);

    }, 2000);// end timeout

});

function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));
}



function GetEmployeeManagerAllocation(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiMasters/GetEmployeeManagers/" + employee_id + "/" + default_company,
        // url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeManagerAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            //(JSON.stringify(data));
            var res = data;

            if (res != undefined && res != null && res.length > 0) {
                if (res[0].m_one_id != null && res[0].m_one_id != 0) {
                    $("#ddlEmployeeManager1").empty().append('<option selected="selected" value="">' + res[0].manager_name_code + '</option>');
                }

                if (res[0].m_two_id != null && res[0].m_two_id != 0) {
                    $("#ddlEmployeeManager2").empty().append('<option selected="selected" value="">' + res[0].m_two_name_code + '</option>');
                }

                if (res[0].m_three_id != null && res[0].m_three_id != 0) {
                    $("#ddlEmployeeManager3").empty().append('<option selected="selected" value="">' + res[0].m_three_name_code + '</option>');
                }


                if (res[0].final_approval == 1) {
                    $("#chkmanager1").attr('checked', 'checked');
                }
                else if (res[0].final_approval == 2) {
                    $("#chkmanager2").attr('checked', 'checked');
                }
                else if (res[0].final_approval == 3) {
                    $("#chkmanager3").attr('checked', 'checked');
                }

                if (res[0].notify_manager_1 == 1) {
                    $("#ManagerAllocNotifybyMail1").attr('checked', 'checked');
                }
                else {
                    $("#ManagerAllocNotifybyMail1").prop('checked', false);
                }

                if (res[0].notify_manager_2 == 1) {
                    $("#ManagerAllocNotifybyMail2").attr('checked', 'checked');
                }
                else {
                    $("#ManagerAllocNotifybyMail2").prop('checked', false);
                }

                if (res[0].notify_manager_3 == 1) {
                    $("#ManagerAllocNotifybyMail3").attr('checked', 'checked');
                }
                else {
                    $("#ManagerAllocNotifybyMail3").prop('checked', false);
                }
            }



            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });

}




//////////////////////////////////////////////////// END Manager Allocation /////////////////////////////////////////////////////
