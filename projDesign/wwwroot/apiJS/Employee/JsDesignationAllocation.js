$('#loader').show();

var login_emp_id;
var default_company;

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


        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            //BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        if (localStorage.getItem("new_emp_id") != null) {
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeeDesignationAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }
        else {
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
        }

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            $("#ddldesignation").val(0);
            $("#Desigfromdate").val('');
            $("#Desigtodate").val('');
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            GetEmployeeDesignationAllocation($(this).val());
        });

        $('#btnSaveDesignationAllocation').bind("click", function () {
            $('#loader').show();
            var employee_id = $('#ddlEmployeeCode :selected').val();
            var ddlcompany = $("#ddlCompany").val();
            var ddldesignation = $("#ddldesignation").val();
            var Desigfromdate = $("#Desigfromdate").val();
            var Desigtodate = $("#Desigtodate").val();
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
            if (ddldesignation == "0" || ddldesignation == null || ddldesignation == "") {
                messageBox("error", "Please Select Designation....!");
                $("#ddldesignation").val('');
                $('#loader').hide();
                return;
            }
            if (Desigfromdate == "0" || Desigfromdate == null || Desigfromdate == "") {
                messageBox("error", "Enter Date From ....!");
                $("#Desigfromdate").val('');
                $('#loader').hide();
                return;
            }
            if (Desigtodate == "0" || Desigtodate == null || Desigtodate == "") {
                messageBox("error", "Enter To Date....!");
                $("#Desigtodate").val('');
                $('#loader').hide();
                return;
            }



            var myData = {
                'employee_id': employee_id,
                'company_id': ddlcompany,
                'desig_id': ddldesignation,
                'applicable_from_date': Desigfromdate,
                'applicable_to_date': Desigtodate
            };

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeDesignationAllocation',
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
                        messageBox("error", "Something went wrong please try again...!");

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



function GetEmployeeDesignationAllocation(employee_id) {
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

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }

            //$("#ddlCompany").val(res.ddlcompany);
            //$("#ddlEmployeeCode").val(res.employee_id);'
            if (res.desig_id != null) {
                BindDesignationList('ddldesignation', $("#ddlCompany").val(), res.desig_id);
                $("#ddldesignation").val(res.desig_id);
                $("#Desigfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
                $("#Desigtodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
            }
            else {
                BindDesignationList('ddldesignation', $("#ddlCompany").val(), 0);
                $("#ddldesignation").val(res.desig_id);
                $("#Desigfromdate").val('');
                $("#Desigtodate").val('');
            }


            $('#loader').hide();

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}




//////////////////////////////////////////////////// END Designation Allocation /////////////////////////////////////////////////////
