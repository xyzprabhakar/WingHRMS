
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

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
            BindManagers(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
            BindManagers(default_company);
        }


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeeManagerAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        }



        //BindManagers('ddlEmployeeManager1', default_company, 0);
        //BindManagers('ddlEmployeeManager2', default_company, 0);
        //BindManagers('ddlEmployeeManager3', default_company, 0);

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            reset_all();
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                GetEmployeeManagerAllocation($(this).val());
            }

        });


    }, 2000);// end timeout

});





//////////////////////////////////////////////////// START Manager Allocation /////////////////////////////////////////////////////



$('#btnSaveManagerAllocation').bind("click", function () {

    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var ddlEmployeeManager1 = $("#ddlEmployeeManager1").val();
    //var Fromdate = $("#managerallocfromdate1").val();
    //var Todate = $("#manageralloctodate1").val();
    var ManagerAllocNotifybyMail1 = $("#ManagerAllocNotifybyMail1").val();



    if (ddlcompany == "0" || ddlcompany == null) {
        messageBox("error", "Please Select Company ....!");
        return false;
    }
    if (employee_id == "0" || employee_id == null || employee_id == "") {
        messageBox("error", "Please Select Employee ....!");
        return false;
    }
    //if (Fromdate == "0" || Fromdate == null || Fromdate == "") {
    //    messageBox("error", "Enter From Date ....!");
    //    $("#managerallocfromdate1").val('');
    //    $('#loader').hide();
    //    return;
    //}
    //if (Todate == "0" || Todate == null || Todate == "") {
    //    messageBox("error", "Enter To Date ....!");
    //    $("#manageralloctodate1").val('');
    //    $('#loader').hide();
    //    return;
    //}
    //Validation
    if (ddlEmployeeManager1 == '' || ddlEmployeeManager1 == null) {
        messageBox("error", "Please Select Reporting Manager 1...!");
        return false;
    }



    var managerallocfromdate1 = $("#managerallocfromdate1").val();
    var manageralloctodate1 = $("#manageralloctodate1").val();

    var chkmanager1 = 0; // Not applicable
    if (ddlEmployeeManager1 != "0" && ddlEmployeeManager1 != "") {
        if ($("#chkmanager1").is(":checked")) {
            chkmanager1 = 1; //applicable
        }

    }

    var MagAllocNotifyMail1 = 0; // Not applicable
    if ($("#ManagerAllocNotifybyMail1").is(":checked")) {
        MagAllocNotifyMail1 = 1; //applicable
    }

    if (MagAllocNotifyMail1 == 0) {
        messageBox("error", "Please Select Reporting Manager 1 Notify by mail...!");
        return;
    }
    var MagAllocNotifyMail2 = 0; // Not applicable
    if ($("#ManagerAllocNotifybyMail2").is(":checked")) {
        MagAllocNotifyMail2 = 1; //applicable
    }
    var MagAllocNotifyMail3 = 0; // Not applicable
    if ($("#ManagerAllocNotifybyMail3").is(":checked")) {
        MagAllocNotifyMail3 = 1; //applicable
    }

    var ddlEmployeeManager2 = $("#ddlEmployeeManager2").val();
    if (ddlEmployeeManager2 == '' || ddlEmployeeManager2 == null) {
        ddlEmployeeManager2 = 0;
    }
    var managerallocfromdate2 = $("#managerallocfromdate2").val();
    var manageralloctodate2 = $("#manageralloctodate2").val();


    if (ddlEmployeeManager2 != "0" && ddlEmployeeManager2 != "" && ddlEmployeeManager2 != null) {
        if ($("#chkmanager2").is(":checked")) {
            chkmanager1 = 2; //applicable
        }

    }

    var ddlEmployeeManager3 = $("#ddlEmployeeManager3").val();
    if (ddlEmployeeManager3 == '' || ddlEmployeeManager3 == null) {
        ddlEmployeeManager3 = 0;
    }
    var managerallocfromdate3 = $("#managerallocfromdate3").val();
    var manageralloctodate3 = $("#manageralloctodate3").val();

    if (ddlEmployeeManager3 != "0" && ddlEmployeeManager3 != "" && ddlEmployeeManager3 != null) {
        if ($("#chkmanager3").is(":checked")) {
            chkmanager1 = 3; //applicable
        }
    }

    if (chkmanager1 == 0) {
        messageBox("error", "Please Select Final Approver from Selected Manager");
        return false;
    }



    if ((parseInt(employee_id) == parseInt(ddlEmployeeManager1)) || (parseInt(employee_id) == parseInt(ddlEmployeeManager2)) || (parseInt(employee_id) == parseInt(ddlEmployeeManager3))) {
        messageBox("error", "Employee and Reporting Manager Cannot be Same");
        return false;

    }

    //start first,second and third manager are not same
    if (ddlEmployeeManager1 != "0" && ddlEmployeeManager1 != "" && ddlEmployeeManager1 != null) {
        if ((parseInt(ddlEmployeeManager1) == parseInt(ddlEmployeeManager2)) || parseInt(ddlEmployeeManager1) == parseInt(ddlEmployeeManager3)) {
            messageBox("error", "Reporting Manager 1,2 or 3 Cannot be Same");
            return false;
        }

    }
    if (ddlEmployeeManager2 != "0" && ddlEmployeeManager2 != "" && ddlEmployeeManager2 != null) {
        if ((parseInt(ddlEmployeeManager1) == parseInt(ddlEmployeeManager2)) || parseInt(ddlEmployeeManager2) == parseInt(ddlEmployeeManager3)) {
            messageBox("error", "Reporting Manager 1,2 or 3 Cannot be Same");
            return false;
        }
    }

    if (ddlEmployeeManager3 != "0" && ddlEmployeeManager3 != "" && ddlEmployeeManager3 != null) {
        if ((parseInt(ddlEmployeeManager1) == parseInt(ddlEmployeeManager3)) || parseInt(ddlEmployeeManager2) == parseInt(ddlEmployeeManager3)) {
            messageBox("error", "Reporting Manager 1,2 or 3 Cannot be Same");
            return false;
        }
    }
    if (managerallocfromdate3 == "" || manageralloctodate3 == "") {
        manageralloctodate3 = "2500-01-01";
        managerallocfromdate3 = "2000-01-01";
    }
    if (managerallocfromdate2 == "" || manageralloctodate2 == "") {
        manageralloctodate2 = "2500-01-01";
        managerallocfromdate2 = "2000-01-01";
    }
    //if (parseInt(ddlEmployeeManager2) == parseInt(ddlEmployeeManager3))
    //{
    //    messageBox("error", "Reporting Manager 1,2 or 3 Cannot be Same");
    //    $('#loader').hide();
    //    return false;

    //}
    // end first,second and third manager are not same

    var myData = {
        'employee_id': employee_id,
        'company_id': ddlcompany,
        'm_one_id': ddlEmployeeManager1,
        'applicable_from_date1': new Date(),// "2000-01-01", //managerallocfromdate1,
        'applicable_to_date1': "2500-01-01",
        'final_approval': chkmanager1,

        'm_two_id': ddlEmployeeManager2,
        'applicable_from_date2': managerallocfromdate2,
        'applicable_to_date2': manageralloctodate2,

        'm_three_id': ddlEmployeeManager3,
        'applicable_from_date3': managerallocfromdate3,
        'applicable_to_date3': manageralloctodate3,


        'notify_manager_1': MagAllocNotifyMail1,
        'notify_manager_2': MagAllocNotifyMail2,
        'notify_manager_3': MagAllocNotifyMail3,
    };


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();
    // Save
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeManagerAllocation',
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
                // window.location.href = '/Employee/ManagerAllocation';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");
                return false;
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




function GetEmployeeManagerAllocation(employee_id) {
    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeManagerAllocation/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {

                //(JSON.stringify(data));
                var res = data;

                // // debugger;
                BindManagers($("#ddlCompany").val())
                if (res.length > 0) {
                    setSelect('ddlEmployeeManager1', res[0].m_one_id);
                    var chkmanager1 = res[0].final_approval;
                    if (chkmanager1 == 1) {
                        $("#chkmanager1").attr('checked', 'checked');
                    }
                    setSelect('ddlEmployeeManager2', res[0].m_two_id);
                    var chkmanager2 = res[0].final_approval;
                    if (chkmanager2 == 2) {
                        $("#chkmanager2").attr('checked', 'checked');
                    }
                    setSelect('ddlEmployeeManager3', res[0].m_three_id);
                    var chkmanager3 = res[0].final_approval;
                    if (chkmanager3 == 3) {
                        $("#chkmanager3").attr('checked', 'checked');
                    }
                    $("#manageralloctodate1").val(GetDateFormatddMMyyyy(new Date(res[0].applicable_to_date)));
                    $("#managerallocfromdate2").val(GetDateFormatddMMyyyy(new Date(res[0].applicable_from_date)));
                    $("#manageralloctodate2").val(GetDateFormatddMMyyyy(new Date(res[0].applicable_to_date)));
                    var manager_noti1 = res[0].notify_manager_1;

                    if (manager_noti1 == 1) {
                        //  $("#ManagerAllocNotifybyMail1").attr('checked', 'checked');
                        //$("input[name=chkmanager2]").prop("checked", true);
                        $("#ManagerAllocNotifybyMail1").prop("checked", true);
                    }
                    else {
                        $("#ManagerAllocNotifybyMail1").prop("checked", false);
                    }


                    var manager_noti2 = res[0].notify_manager_2;

                    if (manager_noti2 == 1) {
                        // $("#ManagerAllocNotifybyMail2").attr('checked', 'checked');
                        //$("input[name=chkmanager2]").prop("checked", true);
                        $("#ManagerAllocNotifybyMail2").prop("checked", true);
                    }
                    else {
                        $("#ManagerAllocNotifybyMail2").prop("checked", false);
                    }

                    var manager_noti3 = res[0].notify_manager_3;
                    if (manager_noti3 == 1) {
                        //$("#ManagerAllocNotifybyMail3").attr('checked', 'checked');
                        //$("input[name=chkmanager2]").prop("checked", true);
                        $("#ManagerAllocNotifybyMail3").prop("checked", true);
                    }
                    else {
                        $("#ManagerAllocNotifybyMail3").prop("checked", false);
                    }
                }
                else {
                    $("#chkmanager1").attr('checked', false);
                    $("#chkmanager2").attr('checked', false);
                    $("#chkmanager3").attr('checked', false);
                    $("#managerallocfromdate1").val('');
                    $("#manageralloctodate1").val('');
                    $("#managerallocfromdate2").val('');
                    $("#manageralloctodate2").val('');
                    $("#ManagerAllocNotifybyMail1").attr('checked', false);
                    $("#ManagerAllocNotifybyMail2").attr('checked', false);
                    $("#ManagerAllocNotifybyMail3").attr('checked', false);
                }












                $('#loader').hide();
            },
            error: function (error) {
                messageBox("error", "Server busy please try again later...!");
                $('#loader').hide();
            }
        });

    }


}



function BindManagers(CompanyId) {
    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/BindManagerByCompID/" + CompanyId,
        type: "GET",
        data: {},
        contentType: "application/json",
        async: false,
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $("#ddlEmployeeManager1").empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.employee_first_name + ' ' + value.employee_middle_name + ' ' + value.employee_last_name + '(' + value.emp_code + ')'));
                $("#ddlEmployeeManager1").append($("<option></option>").val(value.employee_id).html(value.manager_name_code));
            });

            $("#ddlEmployeeManager1").trigger("select2:updated");
            $("#ddlEmployeeManager1").select2();

            $("#ddlEmployeeManager2").empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.employee_first_name + ' ' + value.employee_middle_name + ' ' + value.employee_last_name + '(' + value.emp_code + ')'));
                $("#ddlEmployeeManager2").append($("<option></option>").val(value.employee_id).html(value.manager_name_code));
            });

            $("#ddlEmployeeManager2").trigger("select2:updated");
            $("#ddlEmployeeManager2").select2();

            $("#ddlEmployeeManager3").empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.employee_first_name + ' ' + value.employee_middle_name + ' ' + value.employee_last_name + '(' + value.emp_code + ')'));
                $("#ddlEmployeeManager3").append($("<option></option>").val(value.employee_id).html(value.manager_name_code));
            });

            $("#ddlEmployeeManager3").trigger("select2:updated");
            $("#ddlEmployeeManager3").select2();

            //get and set selected value

            $('#loader').hide();
        },
        error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
            return false;
        }
    });
}

//////////////////////////////////////////////////// END Manager Allocation /////////////////////////////////////////////////////

function reset_all() {

    //BindManagers('ddlEmployeeManager1', $("#ddlCompany").val(), 0);
    //BindManagers('ddlEmployeeManager2', $("#ddlCompany").val(), 0);
    //BindManagers('ddlEmployeeManager3', $("#ddlCompany").val(), 0);
    BindManagers(default_company);
    $("#ManagerAllocNotifybyMail1").prop("checked", false);
    $("#chkmanager1").prop("checked", false);
    $("#ManagerAllocNotifybyMail2").prop("checked", false);
    $("#chkmanager2").prop("checked", false);
    $("#ManagerAllocNotifybyMail3").prop("checked", false);
    $("#chkmanager3").prop("checked", false);
}
