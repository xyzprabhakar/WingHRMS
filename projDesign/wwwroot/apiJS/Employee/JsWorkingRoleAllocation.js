$('#loader').show();

var login_emp_id;
var default_company;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            BindWorkingRoleList('ddl_working_role', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);

        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            BindWorkingRoleList('ddl_working_role', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeeWorkingRoleAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            BindWorkingRoleList('ddl_working_role', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            BindWorkingRoleList('ddl_working_role', default_company, 0);
            GetEmployeeWorkingRoleAllocation(0);
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            GetEmployeeWorkingRoleAllocation($(this).val());
        });

        $('#btnSaveWorkingRoleAllocation').bind("click", function () {
            $('#loader').show();
            var employee_id = $('#ddlEmployeeCode :selected').val();
            var ddlcompany = $("#ddlCompany").val();
            var ddl_working_role = $("#ddl_working_role").val();
            //var Desigfromdate = $("#Desigfromdate").val();
            //var Desigtodate = $("#Desigtodate").val();
            if (ddlcompany == "0" || ddlcompany == null) {
                messageBox("error", "Select Company ....!");
                $("#ddlCompany").val('');
                $('#loader').hide();
                return;
            }
            if (employee_id == "0" || employee_id == null || employee_id == "") {
                messageBox("error", "Select Employee Code ....!");
                $("#ddlEmployeeCode").val('');
                $('#loader').hide();
                return;
            }
            if (ddl_working_role == "0" || ddl_working_role == null || ddl_working_role == "") {
                messageBox("error", "Select Working Role....!");
                $("#ddl_working_role").val('');
                $('#loader').hide();
                return;
            }
            //if (Desigfromdate == "0" || Desigfromdate == null || Desigfromdate == "") {
            //    messageBox("error", "Enter Date From ....!");
            //    $("#Desigfromdate").val('');
            //    $('#loader').hide();
            //    return;
            //}
            //if (Desigtodate == "0" || Desigtodate == null || Desigtodate == "") {
            //    messageBox("error", "Enter To Date....!");
            //    $("#Desigtodate").val('');
            //    $('#loader').hide();
            //    return;
            //}



            var myData = {
                'employee_id': employee_id,
                'company_id': ddlcompany,
                'emp_working_role_id': ddl_working_role,
                //'applicable_from_date': Desigfromdate,
                //'applicable_to_date': Desigtodate
            };

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeWorkingRoleAllocation',
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

                        //window.location.href = '/Employee/DesignationAllocation';
                    }
                    else if (statuscode == "0") {
                        messageBox("info", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
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


    }, 2000);// end timeout

});


//////////////////////////////////////////////////// START Designation Allocation /////////////////////////////////////////////////////



function GetEmployeeWorkingRoleAllocation(employee_id) {

    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeWorkingRoleAllocation/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {

                //(JSON.stringify(data));
                var res = data;
                if (res != undefined) {
                    if (res.statusCode != undefined) {
                        messageBox("info", res.message);
                        if (localStorage.getItem("new_compangy_idd") != null) {
                            BindWorkingRoleList('ddl_working_role', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
                        }
                        else {
                            BindWorkingRoleList('ddl_working_role', default_company, 0);
                        }
                        $('#loader').hide();
                        return false;
                    }

                }
                if (res != null && res != 'emp_working_role_id') {
                    //$("#ddlCompany").val(res.ddlcompany);
                    //$("#ddlEmployeeCode").val(res.employee_id);'

                    BindWorkingRoleList('ddl_working_role', $("#ddlCompany").val(), res.work_r_id);



                    $("#ddl_working_role").val(res.work_r_id);
                    $("#Desigfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
                    $("#Desigtodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
                }
                else {
                    if (localStorage.getItem("new_compangy_idd") != null) {
                        BindWorkingRoleList('ddl_working_role', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
                    }
                    else {
                        BindWorkingRoleList('ddl_working_role', default_company, 0);
                    }

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




//////////////////////////////////////////////////// END Designation Allocation /////////////////////////////////////////////////////
