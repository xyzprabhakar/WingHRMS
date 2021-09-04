
$(document).ready(function () {
});
var apiurl = localStorage.getItem("ApiUrl");

function BindCountryList(ControlId, SelectedVal) {

    var apiurls = apiurl + "apiMasters/GetCountryList/0";
    ControlId = '#' + ControlId;

    $.ajax({
        type: "GET",
        url: apiurls,
        dataType: "json",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.country_id).html(value.name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindStateList(ControlId, SelectedVal, CountryId) {

    ControlId = '#' + ControlId;

    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/GetStateList/0/" + CountryId + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.state_id).html(value.name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindCityList(ControlId, StateId, SelectedVal) {

    ControlId = '#' + ControlId;

    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/GetCityList/" + StateId + "/0",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.city_id).html(value.name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindLocationType(ControlId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/GetLocationType/",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.locationId).html(value.locationName));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//bind company list
function BindCompanyList(ControlId, SelectedVal) {
    dataSrc = JSON.parse(localStorage.getItem("emp_under_login_emp"));
    if (dataSrc == null) {
        alert("Data Source Could Not Be Null");
        return;
    }

    var companyDataSrc = Array.from(new Set(dataSrc.map(s => s.company_id))).
        map(company_id => {
            return {
                company_id: company_id,
                company_name: dataSrc.find(s => s.company_id == company_id).company_name
            };
        });
    var eleCompany = document.getElementById(ControlId);
    eleCompany.length = 0
    $.each(companyDataSrc, function (key, value) {
        var selectOption = document.createElement("option");
        selectOption.text = value.company_name;
        selectOption.value = value.company_id;
        eleCompany.add(selectOption);

    });
    if (SelectedVal == null) {
        var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        eleCompany.value = parseInt(default_Company);
    }
    else {
        eleCompany.value = parseInt(SelectedVal);
    }

}

//function BindCompanyList(ControlId, SelectedVal) {
//    ControlId = '#' + ControlId;
//    var default_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
//    //var HaveDisplay = JSON.parse(localStorage.getItem('menu_filter_comp_')).findIndex(data => data.text == "Display Company List");
//    //alert(HaveDisplay);
//    var HaveDisplay = ISDisplayMenu("Display Company List");
//    if (HaveDisplay == 0) {
//        $(ControlId).empty().append($("<option selected='selected'></option>").val(default_company_id).html(localStorage.getItem("company_name")));
//    }
//    else {
//        $.ajax({
//            type: "GET",
//            url: apiurl + "apiMasters/Get_CompanyList",
//            data: {},
//            contentType: "application/json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            dataType: "json",
//            async: false,
//            success: function (response) {
//                var res = response;

//                $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//                $.each(res, function (data, value) {
//                    $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//                })

//                //get and set selected value
//                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                    $(ControlId).val(SelectedVal);
//                }

//            },
//            error: function (err) {
//                alert(err.responseText);

//            }
//        });
//    }

//}

//bind company list

function BindEmployeeList(ControlId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmployeeHeadList",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            ;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//bind department by company id 

function BindDepartmentListAll(ControlId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_DepartmentListAll/",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            if (SelectedVal == -1) {
                $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value=" ">--Please Select--</option>');
                $(ControlId).empty().append('<option  value="0">All</option>');
            }


            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.department_id).html(value.department_name));
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();
        },
        error: function (err) {
            debugger;
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindDepartmentList(ControlId, CompanyId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_DepartmentByCompany/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            if (SelectedVal == -1) {
                $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value=" ">--Please Select--</option>');
                $(ControlId).empty().append('<option  value="0">All</option>');
            }


            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.department_id).html(value.department_name));
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();
        },
        error: function (err) {
            debugger;
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//Bind Leave Type
function BindLeaveType(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/Get_LeaveTypeMaster/0';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            ;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.leavetypeid).html(value.leavetype));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//bind loaction by company id 

function BindLocationList(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_LocationByCompany/" + CompanyId,
        data: {},
        async: false,
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }
            //if (SelectedVal == -1) {

            //}
            //else {
            //    $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            //}


            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
            });


            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }


            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();


            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//bind religion list 
function BindReligionList(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/Get_ReligionMaster';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            if (SelectedVal == -1) {
                $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            }
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.religion_id).html(value.religion_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != null && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//bind employement type
function BindEmployementType(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetEmployeeType';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;
            ;

            $(ControlId).empty().append('<option value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.emptypeid).html(value.emptypename));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


//bind employement type
function BindEmployementTypeForddl(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetEmployeeType';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.emptypeid).html(value.emptypename));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


//bind frequency type
function BindFrequencyType(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetFrequencyType';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.frequencyid).html(value.frequencyname));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindCompanyListAll(ControlId, EmployeeId, SelectedVal) {
    $("#loader").show();
    ControlId = '#' + ControlId;

    if (SelectedVal == 0) {
        $(ControlId).empty().append('<option value=" ">--Please Select--</option>');
        $(ControlId).empty().append('<option value="0"> All </option>');
    }
    else {
        $(ControlId).empty().append('<option value="0">--Please Select--</option>');
    }


    var company_lst;
    if (localStorage.getItem("emp_companies_lst") != null || localStorage.getItem("emp_companies_lst") != "" || localStorage.getItem("emp_companies_lst") != undefined || JSON.parse(localStorage.getItem("emp_companies_lst")).length > 0) {
        company_lst = JSON.parse(localStorage.getItem("emp_companies_lst"));
    }
    else {

        var listapi = localStorage.getItem("ApiUrl");

        $.ajax({
            type: "GET",
            url: listapi + "apiEmployee/Get_Emp_all_Company/" + EmployeeId,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }

                if (lst == "" || lst == null || lst.length == 0) {
                    localStorage.setItem("emp_companies_lst", JSON.stringify(res));
                }

                company_lst = res;


            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });

    }

    if (company_lst != undefined && company_lst != null && company_lst != "") {
        $.each(company_lst, function (data, value) {

            $(ControlId).append($("<option></option>").val(value.company_id).html(value.company_name));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        }

        $(ControlId).trigger("select2:updated");
        $(ControlId).select2();
    }

    $("#loader").hide();


}

function BindEmployeeCodeFromEmpMaster(ControlId, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMaster",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            // $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option><option value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);

            }

            $('#loader').hide();

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}
function BindAllEmployeeUnderEmp_cal(ControlId, Companyid, EmpId, SelectedVal) {
    var listapi = localStorage.getItem("ApiUrl");

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }

    $('#loader').show();
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_Active_Inactive/" + Companyid + "/" + EmpId,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $.each(res, function (data, value) {

                $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });


}
function BindEmployeeCodeFromEmpMasterByComp(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">All</option>');
    }

    if (CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            // url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
            url: apiurl + "/apiEmployee/GetallEmployee_before_today_dt/" + CompanyId,
            data: {},
            async: false,
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            success: function (response) {

                var res = response;
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '-1' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                }
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
                $('#loader').hide();
                //$('#loader').hide();

            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });
    }
    else {
        messageBox("error", "Please select Company");
        return false;
    }
}

function BindEmployeeListbyDepartment(ControlId, CompanyId, Department_id, SelectedVal) {
    ControlId = '#' + ControlId;
    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option value="0" selected="selected" >--Please select--</option>');
    }
    else {
        $(ControlId).empty().append('<option value="0" selected="selected" >All</option>');
    }

    if (CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            // url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
            url: apiurl + "/apiMasters/Get_EmployeeByCompAndDeptId/" + CompanyId + '/' + Department_id,
            data: {},
            async: false,
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            success: function (response) {

                var res = response;
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '-1' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                }
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
                $('#loader').hide();
                //$('#loader').hide();

            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });
    }
    else {
        messageBox("error", "Please select Company");
        return false;
    }
}


//function BindAllEmployeeByCompany(ControlId, CompanyId, SelectedVal) {
//    ControlId = '#' + ControlId;
//    $('#loader').show();
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
//        data: {},
//        async: false,
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {

//            var res = response;
//            $('#loader').show();



//            var HaveDisplayCompList = ISDisplayMenu("Is Company Admin");

//            $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');


//            if (HaveDisplayCompList == 0) {
//                $.each(res, function (data, value) {
//                    $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
//                })
//            }

//            if (res.length > 0) {

//                //get and set selected value

//                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
//                    $(ControlId).val(SelectedVal);
//                    $(ControlId).trigger("select2:updated");
//                    $(ControlId).select2();
//                }
//                $(ControlId).trigger("select2:updated");
//                $(ControlId).select2();
//            }

//            $('#loader').hide();

//        },
//        error: function (err) {
//            alert(err.responseText);
//            $('#loader').hide();
//        }
//    });
//}



//bind employee leave type info
function BindLeaveTypeInfo(ControlId, SelectedVal) {
    ;
    var urls = apiurl + 'apiLeave/GetLeaveTypeInfo/';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.leave_info_id).html(value.leave_type_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}



function BindEmpNameAndCodeByeComp(ControlId, CompanyId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmpNameAndCodeByeComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.employee_first_name + ' ' + value.employee_middle_name + ' ' + value.employee_last_name + '(' + value.emp_code + ')'));
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

//allow numeric values only
function OnlyNumeric(event) {
    var key_code = window.event.keyCode;
    var oElement = window.event.srcElement;
    if (!window.event.shiftKey && !window.event.ctrlKey && !window.event.altKey) {
        if ((key_code > 47 && key_code < 58) ||
            (key_code > 95 && key_code < 106)) {

            if (key_code > 95)
                key_code -= (95 - 47);
            oElement.value = oElement.value;
        } else if (key_code == 8) {
            oElement.value = oElement.value;
        } else if (key_code != 9) {
            event.returnValue = false;
        }
    }
}
$(document).ready(function () {
    $('#myTextbox').keydown(OnlyNumeric);
});

//date format function
function GetDateFormat(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return month + '/' + day + '/' + date.getFullYear();
}

function GetDateFormatddMMyyyy(date) {
    var month = (date.getMonth() + 1).toString();
    // month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    return day + '-' + months[month - 1] + '-' + date.getFullYear();
}

function GetDateFormatddmmyyyy(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + '-' + (month) + '-' + date.getFullYear();
}


function GetOnlyDate(date) {
    var new_val = date.split('-'),
        dateYear = parseInt(new_val[0]),
        dateMonth = parseInt(new_val[1]),
        dateDay = parseInt(new_val[2]);

    var fulldate = '2000-01-01';

    if (dateMonth <= 9 && dateDay <= 9) {
        fulldate = dateYear + '-0' + dateMonth + '-0' + dateDay;
    }
    else if (dateMonth <= 9) {
        fulldate = dateYear + '-0' + dateMonth + '-' + dateDay;
    }
    else if (dateDay <= 9) {
        fulldate = dateYear + '-' + dateMonth + '-0' + dateDay;
    }
    else {
        fulldate = dateYear + '-' + dateMonth + '-' + dateDay;
    }

    return fulldate;
}

function GetTimeFromDate(date) {
    date = new Date(date);
    var time = null;

    if (date.getMinutes() <= 9) {
        time = date.getHours() + ":0" + date.getMinutes();
    }
    else if (date.getMinutes() > 9) {
        time = date.getHours() + ":" + date.getMinutes();
    }
    else {
        time = date.getHours() + ":00";
    }

    return time;
}

function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function isNumberKey1(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return true;

    return false;
}
function isBS(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode || 8)

        return false;

    return true;


}
function IsAlphabet(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    var txt = String.fromCharCode(charCode)

    if (txt.match(/^[a-zA-Z\b ]+$/))

        return true;

    return false;

}

function AlphaNumeric(e) {
    var regex = /^[0-9a-zA-Z_\,\ ]+$/;
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }

    e.preventDefault();
    return false;
}
function ValidMobile(ControlId) {
    ControlId = '#' + ControlId;
    var MobileNo = $(ControlId).val();
    ;
    var mobmatch1 = /^[456789]\d{9}$/;
    if (!mobmatch1.test(MobileNo)) {
        // messageBox("error", "Please Enter valid mobile Number.");
        $(ControlId).addClass('generatedErorr');
        $(ControlId).val('');
        return false;
    }
    $(ControlId).removeClass('generatedErorr');
}

function ValidEmailId(ControlId) {
    ControlId = '#' + ControlId;
    var EmailID = $(ControlId).val();

    var mobmatch1 = /^([0-9a-zA-Z]([-_\\.]*[0-9a-zA-Z]+)*)@([0-9a-zA-Z]([-_\\.]*[0-9a-zA-Z]+)*)[\\.]([a-zA-Z]{2,9})$/// /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (!mobmatch1.test(EmailID)) {
        // messageBox("error", "Please Enter Valid Email Address.");
        $(ControlId).addClass('generatedErorr');
        $(ControlId).val('');
        $(ControlId).focus();
        return false;
    }
    $(ControlId).removeClass('generatedErorr');
}
//added logic for aadhar 
function ValidAdhar(ControlId) {
    // debugger;
    ControlId = '#' + ControlId;
    var AadharLen = $(ControlId).val().length;
    // bind it is a bool if it is true then length validation for aadhar card passed
    var bindit = false;
    if (AadharLen == 12) {
        bindit = true;
    } else {
        bindit = false;
        $(ControlId).val('');
        messageBox("info", "Invalid aadhaar no !!");

    }
}

function printDiv(DIVID) {
    var divToPrint = document.getElementById(DIVID);
    var htmlToPrint = "";
    var htmlToPrint = '' + '<style type="text/css">'
        + 'table th, table td {' + 'padding:0;' + '' + 'font-size:10px;' + '' + 'line-height:initial;' + ' }' +
        '</style>';

    htmlToPrint += divToPrint.outerHTML;
    newWin = window.open("");
    newWin.document.write(htmlToPrint);
    newWin.print();
    newWin.close();
}

function getHHMM(strval) {
    var d = new Date(strval);
    var h = addZero(d.getHours());
    var m = addZero(d.getMinutes());
    var s = addZero(d.getSeconds());
    var result = h + ":" + m + ":" + s;
    return result;
}

function addZero(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

/*-----Image file upload logic-----------*/

//function readFile() {
//    ;

//    if (this.files && this.files[0]) {

//        var imgsizee = this.files[0].size;
//        var sizekb = imgsizee / 1024;
//        sizekb = sizekb.toFixed(0);

//        $('#HFSizeOfPhoto').val(sizekb);
//        if (sizekb < 10 || sizekb > 100) {
//            $("#File1").val("");
//            swal("Photograph Size Validate!", 'The size of the photograph should fall between 20KB to 100KB. Your Photo Size is ' + sizekb + 'kb.', "error");
//            return false;
//        }
//        var ftype = this;
//        var fileupload = ftype.value;
//        if (fileupload == '') {
//            $("#File1").val("");
//            swal("Photograph Format !", "Photograph only allows file types of PNG, JPG, JPEG. ", "error");
//            return;
//        }
//        else {
//            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
//            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg") {

//            }
//            else {
//                $("#File1").val("");
//                swal("Photograph Format !", "Photograph only allows file types of PNG, JPG, JPEG. ", "error");
//                return;
//            }
//        }

//        var FR = new FileReader();
//        FR.onload = function (e) {
//            EL("myImg").src = e.target.result;
//            EL("HFb64").value = e.target.result;

//        };
//        FR.readAsDataURL(this.files[0]);
//    }
//}

//function EL(id) { return document.getElementById(id); } // Get el by ID helper function

function formatAMPM(date) {
    date = new Date(date);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    if (strTime == "12:00 AM") {
        strTime = "";
    }
    return strTime;
}

function BindRoleMaster(ControlId, SelectedVal) {

    ControlId = '#' + ControlId;
    var url = apiurl + "apiMasters/GetRoleMaster";
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.roleid).html(value.rolename));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

$(function () {
    $('input , textarea , select').on('click', function () {
        $('.message-pop').hide('slow');
    });
});

function ValidateURL(ControlID) {
    ControlID = '#' + ControlID;
    var matchurl = /^(www)+\.[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;
    if (!matchurl.test($(ControlID).val())) {
        $(ControlID).addClass('generatedErorr');
        $(ControlID).val('');
        $(ControlID).focus();
        return false;
    }
    $(ControlID).removeClass('generatedErorr');

}

function ValidatePanCard(ControlID) {
    ControlID = '#' + ControlID;
    var regex = /[A-Z]{5}[0-9]{4}[A-Z]{1}/;
    if (!regex.test($(ControlID).val())) {
        $(ControlID).addClass('generatedErorr');
        $(ControlID).val('');
        $(ControlID).focus();
        return false;
    }
    $(ControlID).removeClass('generatedErorr');
}

//insert space for everey 4 charecter
function IbanSpace(ControlID) {
    ControlID = '#' + ControlID;
    var reformat = $(ControlID).val().replace(/(\[a-z0-9]{4})/g, function (match) {
        return match + " ";
    });
    $(ControlID).val(reformat);
}


function BindFinancialList(ControlId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiePA/Get_FinancialList",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.fiscal_year_id).html(value.financial_year_Name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindQuarterList(ControlId, SelectedVal, fyi_Id) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiePA/GetQuarterList/0/" + fyi_Id + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.quarter_id).html(value.quarter_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });

    // it.intern 2 region 
    function BindBankList(ControlId, SelectedVal) {

        var apiurls = apiurl + "apiMasters/Get_Bankname/0";
        ControlId = '#' + ControlId;
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: apiurls,
            dataType: "json",
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.bank_id).html(value.name));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }

                $('#loader').hide();
            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });
    }

    function BindBranchList(ControlId, SelectedVal) {

        var apiurls = apiurl + "apiMasters/Get_Branchmaster/0";
        ControlId = '#' + ControlId;
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: apiurls,
            dataType: "json",
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.branch_id).html(value.name));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }

                $('#loader').hide();
            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });
    }
}

//supriya start for loan request master on 23-05-2019 
function BindGradeMaster(ControlId, SelectedVal) {
    // // debugger;
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        url: apiurl + "apiPayroll/Get_GradeMaster",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: "{}",
        success: function (response) {
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(response, function (data, value) {
                $(ControlId).append("<option value='" + value.grade_id + "'>" + value.grade_name + "</option>");
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        erro: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

// Enter only Number
function isNumber(evt, element) {
    // // debugger;
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}
//supriya end for loan request master on 23-05-2019


//supriya For Parent Menu start on 24-05-2019
function BindParentMenu(ControlId, SelectedVal) {
    //// debugger;
    ControlId = '#' + ControlId;
    var url_getmenu = "";
    $('#loader').show();
    $.ajax({
        url: apiurl + "apiMasters/Get_MenuMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: "{}",
        success: function (response) {
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            if (response.length > 0) {
                $.each(response, function (data, value) {
                    $(ControlId).append("<option value='" + value.menu_id + "'>" + value.menu_name + "</option>");
                });
            }

            if (response.length == "0") {
                $("#parentmenudiv").hide();
            }
            $('#loader').hide();
            //get and set selected value 

            //Hide parent menu drop down and sub menu div
            // if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            // $(ControlId).val(SelectedVal);
            // $(ControlId).hide();
            //  $("#parentmenudiv").hide();
            // }
        },
        erro: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}
//supriya For Parent Menu end on 24-05-2019

//supriya Bind Menu Master in Checkbox start on 25-05-2019
function BindMenu_in_checkbox(ControlId, SelectedVal) {
    // debugger;
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        url: apiurl + "apiMasters/Get_MenuMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: "{}",
        success: function (response) {
            if (response.length > 0) {
                $.each(response, function (data, value) {
                    $(ControlId).append("<tr><td><input type=checkbox name='menuidd' value='" + value.menu_id + "'/></td><td><label name='menunamee'>" + value.menu_name + "</label></td></tr>")
                });
            }

            $('#loader').hide();
        },
        error: function () {
            alert("error while getting menu master detail");
            $('#loader').hide();
        }
    });
}

//supriya Bind Menu Master in Checkbox start on 25-05-2019.


function BindBatchList(ControlId, SelectedVal, Company_Id) {


    ControlId = '#' + ControlId;

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiPayroll/Get_Batch/" + Company_Id,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.batch_id).html(value.batch_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}



function BindRelationType(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetRelation';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.relationid).html(value.relationname));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                // $(ControlId).val(SelectedVal);
                var valuee = SelectedVal.toString().substr(0, 1).toUpperCase() + SelectedVal.toString().substr(1);
                $(ControlId).val(valuee);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function AllowDotAlphaNumeric(e) {
    var regex = /^[0-9a-zA-Z_\,\.\ ]+$/;
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }

    e.preventDefault();
    return false;

}



function BindApproverType(ControlId, SelectedVal) {

    var urls = apiurl + 'apiMasters/GetApproverType';
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.approvertype_id).html(value.approvertype_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function BindOnlyUserRole(ControlId, SelectedVal) {
    $("#loader").show();
    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetOnlyUserRole",
        type: "GET",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: {},
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option value=0>--Please Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.role_id).html(value.role_name));

            });

            //get and set value

            if (SelectedVal != "0" || SelectedVal != "" || SelectedVal != "undefined") {
                $(ControlId).val(SelectedVal);
            }

            $("#loader").hide();

        },
        error: function (err) {
            messageBox(err.responseText);
            $('#loader').hide();
        }
    });
}

function BinddEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
                //$("#txtEmployeeCode").val(value.emp_code);
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
                //$("#txtEmployeeCode").val(SelectedVal);
            }
            $('#loader').hide();
            //var emp_code = $('#ddlEmployeeCode option[value="' + $('#ddlEmployeeCode').val() + '"]').text();
            //$("#txtEmployeeCode").val(emp_code);
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}
//function ISDisplayMenu(name_of_menu) {

//    var menu_data = JSON.parse(localStorage.getItem('menu_filter_comp_'));
//    var Index = menu_data.findIndex(data => data.text == name_of_menu);

//    if (Index > -1) {
//        var menu_id = menu_data[Index].id;
//        try {
//            var menu_rights = JSON.parse(localStorage.getItem('dashboardd_role_menu_data')).split(',');

//            var finded = menu_rights.findIndex(data => data == menu_id);
//            if (finded > -1) {
//                return 1;
//            }
//            else {
//                return 0;
//            }
//        }
//        catch (ex) {
//            return 0;
//        }
//    }
//    else {
//        return 0;
//    }

//}


function _GUID_New() {
    var _login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    $.ajax({
        url: "/api/Values/" + _login_empid,
        type: "GET",
        contentType: "application/json",
        async: false,
        //headers: {'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: {},
        dataType: "text",
        success: function (response) {
            var res = response;
            $("#hdnsalt").val(res);

        },
        error: function (err) {
            //alert(err.responseText);
        }
    });
}


function BindDocumentTypeByComp(ControlId, CompanyId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/GetDocumentTypeMaster/0/" + CompanyId,
        data: {},
        async: false,
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;
            $('#loader').show();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.doc_type_id).html(value.doc_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}

function BindEmployeeList1(ControlId, SelectedVal) {


    var listapi = localStorage.getItem("ApiUrl");

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiMasters/Get_Employee_Under_LoginEmp/" + SelectedVal,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');

            $(ControlId).append('<option selected="selected" value=' + SelectedVal + '>' + login_name_code + '</option>');


            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code1));

                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
                // $(ControlId).prop('disabled', true);
            }

            //$('#loader').hide();

            //$(ControlId).trigger("liszt:updated");
            //$(ControlId).chosen();
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function BindEPA_FiscallYrByComp(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiePA/Get_EPAFiscalYearByComp/" + CompanyId,
        data: {},
        async: false,
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.fiscal_year_id).html(value.financial_year_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                //$(ControlId).trigger("chosen:updated");
                //$(ControlId).chosen();
            }
            //$(ControlId).trigger("chosen:updated");
            //$(ControlId).chosen();
            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);

        }
    });
}


function BindEPA_CycleByComp(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiePA/Get_EPACycleByComp/" + CompanyId,
        data: {},
        async: false,
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;
            $('#loader').show();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.cycle_id).html(value.monthname));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                //$(ControlId).trigger("chosen:updated");
                //$(ControlId).chosen();
            }
            //$(ControlId).trigger("chosen:updated");
            //$(ControlId).chosen();
            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);

        }
    });
}


function BindWorkingRoleByDeptID(ControlId, CompanyId, DeptId, SelectedVal) {
    ControlId = '#' + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    if (CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: apiurl + "apiePA/Get_EpaWorkingRoleMasterByDeptID/" + CompanyId + "/" + DeptId,
            data: {},
            async: false,
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            success: function (response) {

                var res = response;
                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    $('#loader').hide();
                    return false;
                }
                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.working_role_id).html(value.working_role_name));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                    //$(ControlId).trigger("chosen:updated");
                    //$(ControlId).chosen();
                }
                //$(ControlId).trigger("chosen:updated");
                //$(ControlId).chosen();
                $('#loader').hide();

            },
            error: function (err) {
                $('#loader').hide();
                alert(err.responseText);

            }
        });
    }

}


function BindKPIObjectiveType(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiePA/Get_KPIObjective/" + CompanyId + "/0",
        data: {},
        async: false,
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            $('#loader').show();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.obj_type_id).html(value.objective_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);

            }
            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function BindDepartmentList_except_all(ControlId, CompanyId, SelectedVal) {

    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_DepartmentByCompany/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.department_id).html(value.department_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function BindTabMaster(ControlId, CompanyId, SelectedVal) {
    ControlId = "#" + ControlId;
    $(ControlId).empty().append("<option value=0>--Please Select--</option>");
    if (CompanyId > 0) {
        $('#loader').show();
        $.ajax({
            url: apiurl + "apiePA/Get_TabMaster/0/" + CompanyId,
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;


                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.tab_mstr_id).html(value.tab_name));
                });

                if (SelectedVal != null && SelectedVal != "" && SelectedVal != undefined) {
                    $(ControlId).val(SelectedVal);
                }
                $('#loader').hide();
            },
            error: function (err) {
                $('#loader').hide();
                messageBox("error", err.responseText);
            }
        });
    }

}


function BindkpiRatingList(ControlId, CompanyId, SelectedVal) {
    ControlId = "#" + ControlId;

    $.ajax({
        url: apiurl + "apiePA/Get_kpiRatingMaster/" + CompanyId + "/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            $(ControlId).empty().append("<option value=0>--Please Select--</option>");

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.kpi_rating_id).html(value.rating_name));
            });

            if (SelectedVal != null && SelectedVal != "" && SelectedVal != undefined) {
                $(ControlId).val(SelectedVal);
            }
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}


function BindBankMaster(ControlId, SelectedVal) {

    ControlId = "#" + ControlId
    $.ajax({
        url: apiurl + "apiMasters/Get_BankMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.bank_id).html(value.bank_name));
            });


            if (SelectedVal != null && SelectedVal != "" && SelectedVal != undefined) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

function BindEducationLevel(ControlId, SelectedVal) {
    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_EducationLevel",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.edu_level_id).html(value.edu_level_name));
            });

            if (SelectedVal != "" && SelectedVal != undefined && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function BindAllDepartmentFromDepartmentMaster(ControlId, SelectedVal) {

    ControlId = "#" + ControlId;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_DepartmentMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (SelectedVal == -1) {
                $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value="">--Please Select--</option>');
                $(ControlId).append('<option value="0"> All </option>');
            }


            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.departmentid).html(value.departmentname));
            });


            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != -1) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function BindAllLocationsFromLocatinMaster(ControlId, SelectedVal) {

    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiLocationMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (SelectedVal == -1) {
                $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value="">--Please Select--</option>');
                $(ControlId).append('<option value="0"> All </option>');
            }




            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.locationid).html(value.locationname));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();


        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });


}

function BindShift_ddl(ControlId, Company_id, SelectedVal) {
    ControlId = "#" + ControlId;

    //var api_urll = "";
    //if (Company_id == "0") {
    //    api_urll = localStorage.getItem("ApiUrl") + "/apiShift";
    //}
    //else {
    //    api_urll = localStorage.getItem("ApiUrl") + "apiShift/Gettbl_shift_detailsByCompanyId/" + Company_id
    //}

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiShift",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (SelectedVal == 0) {
                $(ControlId).empty().append('<option selected="selected" value=" "> --Please Select-- </option>');
                $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            }
            else {
                $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            }


            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.shift_mstr_id).html(value.shift_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal == '0' && SelectedVal != 'undefined' && SelectedVal != null && SelectedVal > 0) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();


        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}

function BindOnlyProbation_Confirmed_emp(ControlId, Company_id, SelectedVal) {

    ControlId = "#" + ControlId;

    $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/GetallEmployee_before_today_dt/" + Company_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        async: false,
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            //  localStorage.setItem("emp_lst", JSON.stringify(res));
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
                $(ControlId).val(SelectedVal);

            }
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });


    //var get_data = false;
    //var _emp_bind;
    //if (localStorage.getItem("emp_lst") == null) {
    //    get_data = true;
    //}
    //else {

    //    get_data = true;//need to load data from Db because of New employee creation Does not display
    //    //_emp_bind = JSON.parse(localStorage.getItem("emp_lst"));
    //    //_emp_bind = _emp_bind.filter(p => p.company_id == Company_id);
    //    //if (_emp_bind.length == 0) {
    //    //    get_data = true;
    //    //}

    //}


}


function BindEmpResignReason(ControlId, Company_id, SelectedVal) {
    $("#loader").show();
    ControlId = "#" + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
    if (Company_id > 0) {
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiMasters/GetResignReasons/" + Company_id,
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;



                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.reason_id).html(value.reason_));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                    $(ControlId).trigger("select2:updated");
                    $(ControlId).select2();
                }
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();

                $("#loader").hide();
            },
            error: function (err) {
                $("#loader").hide();
                messageBox("error", err.responseText);
                return false;
            }
        });

    }

}


function BindEmployeeListUnderLoginEmp(ControlId, CompanyId, SelectedVal) //Bind Only Employee Under Manager
{

    var listapi = localStorage.getItem("ApiUrl");

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiMasters/Get_Employee_Under_LoginEmp/" + SelectedVal + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value=' + SelectedVal + '>' + login_name_code + '</option>');


            $.each(res, function (data, value) {
                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code1));

                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
                // $(ControlId).prop('disabled', true);
            }

            //$('#loader').hide();

            //$(ControlId).trigger("liszt:updated");
            //$(ControlId).chosen();
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
            $('#loader').hide();

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function FunIsApplicationFreezed() {
    is_attendence_freezed_for_Emp = localStorage.getItem("IsAttendenceFreezedForEmp");
    is_attendence_freezed_for_Admin = localStorage.getItem("IsAttendenceFreezedForAdmin");
    is_Admin = localStorage.getItem("is_super_admin");

    if (is_Admin) {
        if (is_attendence_freezed_for_Admin == "true" || is_attendence_freezed_for_Admin == "yes") { return true; }
        else { return false; }
    }
    else {
        if (is_attendence_freezed_for_Emp == "true" || is_attendence_freezed_for_Emp == "yes") { return true; }
        else { return false; }
    }

}

//SelectedVal = 0 TO INCLUDE ALL, -1 TO SELECT ONLY, AND >1 FOR SELECTED VALUE
function BindAllEmp_Company(ControlId, EmployeeId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;

    var emp_company_lst;
    var lst = localStorage.getItem("emp_companies_lst");

    if (lst != undefined && lst != null && lst != "" && lst.length > 0) {

        emp_company_lst = JSON.parse(lst);
    }
    else {

        var listapi = localStorage.getItem("ApiUrl");

        $.ajax({
            type: "GET",
            url: listapi + "apiEmployee/Get_Emp_all_Company/" + EmployeeId,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }

                if (lst == "" || lst == null || lst.length == 0) {
                    localStorage.setItem("emp_companies_lst", JSON.stringify(res));
                }

                emp_company_lst = res;


            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });

    }

    if (emp_company_lst != null && emp_company_lst != "" && emp_company_lst.length > 0) {


        if (SelectedVal == 0) {
            $(ControlId).empty().append('<option selected="selected" value="">--Please Select--</option>');
            $(ControlId).append('<option value="0">All</option>');
        }
        else if (SelectedVal == -1) { $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>'); }
        else { $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>'); }

        $.each(emp_company_lst, function (data, value) {

            $(ControlId).append($("<option></option>").val(value.company_id).html(value.company_name));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        }

        $(ControlId).trigger("select2:updated");
        $(ControlId).select2();
    }

    $('#loader').hide();

}


function BindEmployeeListUnderLoginEmpFromAllCompForEsep(ControlId, CompanyId, LoginEmpID, SelectedVal) {
    var listapi = localStorage.getItem("ApiUrl");
    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }
    
    var emp_lst_under_login_emp;
    
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_CompanyForEsep/" + CompanyId + "/" + LoginEmpID,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            emp_lst_under_login_emp = res;

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });





    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {

        var selected_data = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

        $.each(selected_data, function (data, value) {

            $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
        }

    }
    $(ControlId).trigger("select2:updated");
    $(ControlId).select2();

}

function BindEmployeeListUnderLoginEmpFromAllComp(ControlId, CompanyId, LoginEmpID, SelectedVal) {
    debugger;
    var listapi = localStorage.getItem("ApiUrl");
    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }
    
    var emp_lst_under_login_emp;

    var lst_ = localStorage.getItem("emp_under_login_emp");

    if (lst_ != null && lst_ != "" && lst_.length > 0) {

        emp_lst_under_login_emp = JSON.parse(lst_);
    }
    else {
        $.ajax({
            type: "GET",
            //url: listapi + "apiMasters/Get_EmployeeHeadList",
            url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId + "/" + LoginEmpID,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;
                emp_lst_under_login_emp = res;

            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
            }
        });

    }



    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {

        var selected_data = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

        $.each(selected_data, function (data, value) {

            $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
        }

    }
    $(ControlId).trigger("select2:updated");
    $(ControlId).select2();

}

//function BindEmployeeListUnderLoginEmpFromAllComp(ControlId, CompanyId, LoginEmpID, SelectedVal) {

//    var listapi = localStorage.getItem("ApiUrl");
//    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

//    var key = CryptoJS.enc.Base64.parse("#base64Key#");
//    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

//    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
//    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
//    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

//    ControlId = '#' + ControlId;


//    var HaveDisplay = ISDisplayMenu("Display For User");
//    if (HaveDisplay == 1) {
//        var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//        if (is_managerr_dec == 'yes') {
//            $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
//            $(ControlId).append('<option selected="selected" value=' + LoginEmpID + '>' + login_name_code + '</option>');
//        }
//        else {
//            $(ControlId).empty().append('<option selected="selected" value=' + LoginEmpID + '>' + login_name_code + '</option>');
//        }
//        //else {
//        //    $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
//        //}


//    }
//    else {
//        var _display = ISDisplayMenu("Is Company Admin");
//        if (_display == 1) {
//            $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
//        }
//        else {
//            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
//        }

//    }


//    var emp_lst_under_login_emp;

//    var lst_ = localStorage.getItem("emp_under_login_emp");

//    if (lst_ != null && lst_ != "" && lst_.length > 0) {

//        emp_lst_under_login_emp = JSON.parse(lst_);
//    }
//    else {
//        $.ajax({
//            type: "GET",
//            //url: listapi + "apiMasters/Get_EmployeeHeadList",
//            url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId + "/" + LoginEmpID,
//            data: {},
//            async:false,
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            success: function (response) {
//                var res = response;

//                //if (lst_ == null == lst_ == "" || lst_.length == "0") {
//                //    localStorage.setItem("emp_under_login_emp", JSON.stringify(res));
//                //}


//                emp_lst_under_login_emp = res;

//            },
//            error: function (err) {
//                alert(err.responseText);
//                $('#loader').hide();
//            }
//        });

//    }



//    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {

//        var selected_data = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

//        $.each(selected_data, function (data, value) {

//            $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
//        });

//        //get and set selected value
//        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//            $(ControlId).val(SelectedVal);
//            $(ControlId).trigger("select2:updated");
//            $(ControlId).select2();

//        }

//    }
//    $(ControlId).trigger("select2:updated");
//    $(ControlId).select2();
//    $('#loader').hide();


//}


//function BindEmployeeListUnderLoginEmpFromAllComp_except_all_option(ControlId, CompanyId, LoginEmpID, SelectedVal) {

//    ControlId = "#" + ControlId;
//    $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');


//    var listapi = localStorage.getItem("ApiUrl");
//    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

//    var emp_lst_under_login_emp;

//    var lst_ = localStorage.getItem("emp_under_login_emp");

//    if (lst_ != null && lst_ != "" && lst_.length > 0 ) {

//        emp_lst_under_login_emp = JSON.parse(lst_);
//    }
//    else {
//        $.ajax({
//            type: "GET",
//            url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + CompanyId + "/" + LoginEmpID,
//            data: {},
//            async: false,
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            success: function (response) {
//                var res = response;


//                emp_lst_under_login_emp = res;

//            },
//            error: function (err) {
//                alert(err.responseText);
//                $('#loader').hide();
//            }
//        });

//    }



//    if (emp_lst_under_login_emp != undefined && emp_lst_under_login_emp.length > 0) {
//        emp_lst_under_login_emp = emp_lst_under_login_emp.filter(p => p.company_id == CompanyId);

//        $.each(emp_lst_under_login_emp, function (data, value) {

//            $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
//        });

//        //get and set selected value
//        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//            $(ControlId).val(SelectedVal);
//            $(ControlId).trigger("select2:updated");
//            $(ControlId).select2();

//        }

//    }
//    $(ControlId).trigger("select2:updated");
//    $(ControlId).select2();
//    $('#loader').hide();


//}



function BindPaymentMode(ControlId, SelectedVal) {
    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_PaymentMode",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.payment_mode_id).html(value.payment_mode_name));
            });

            if (SelectedVal != "" && SelectedVal != undefined && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


function BindPFGroup(ControlId, SelectedVal) {
    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_PFGroup",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.pf_group_id).html(value.pf_group_name));
            });

            if (SelectedVal != "" && SelectedVal != undefined && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


function BindVPFGroup(ControlId, SelectedVal) {
    ControlId = "#" + ControlId;

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_VPFGroup",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append("<option value=0>--Please Select--</option>");
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.vpf_group_id).html(value.vpf_group_name));
            });

            if (SelectedVal != "" && SelectedVal != undefined && SelectedVal != null) {
                $(ControlId).val(SelectedVal);
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


function setSelect(ControlId, SelectedVal) {

    ControlId = "#" + ControlId;

    if ($('' + ControlId + ' option[value="' + SelectedVal + '"]').prop("selected", true).length) {
        $(ControlId).trigger("select2:updated");
        $(ControlId).select2();
    }



    //$("#selectVal").select();
}

function BindAllEmployeeUnderEmp(ControlId, Companyid, EmpId, SelectedVal) {
    var listapi = localStorage.getItem("ApiUrl");

    ControlId = '#' + ControlId;

    if (SelectedVal == -1) {
        $(ControlId).empty().append('<option selected="selected" value="0">--All--</option>');
    }
    else {
        $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
    }

    $('#loader').show();
    $.ajax({
        type: "GET",
        //url: listapi + "apiMasters/Get_EmployeeHeadList",
        url: listapi + "apiEmployee/Get_Employee_Under_LoginEmp_from_all_Company/" + Companyid + "/" + EmpId,
        data: {},
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $.each(res, function (data, value) {

                $(ControlId).append($("<option></option>").val(value._empid).html(value.emp_name_code));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $(ControlId).trigger("select2:updated");
            $(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });


}

function BindWithdrawalType(ControlId, CompanyId, SelectedVal) {
    ControlId = "#" + ControlId;
    $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
    if (CompanyId > 0) {
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiMasters/GetWithdrawalType/" + CompanyId,
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value).html(value));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
                    $(ControlId).val(SelectedVal);
                    $(ControlId).trigger("select2:updated");
                    $(ControlId).select2();
                }
                $(ControlId).trigger("select2:updated");
                $(ControlId).select2();


            },
            error: function (err) {
                $("#loader").hide();
                messageBox("error", err.responseText);
                return false;
            }
        });

    }
}


function PFNumber(e) {
    var regex = /^[0-9a-zA-Z_\,\/ ]+$/;
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }

    e.preventDefault();
    return false;
}



function checkFileExist(url) {
    debugger;
    var location = window.location.href;
    var directoryPath = location.substring(0, location.lastIndexOf("/"));
    directoryPath = directoryPath.substring(0, directoryPath.lastIndexOf("/"));
    var http;
    if (window.XMLHttpRequest)
        http = new XMLHttpRequest();
    else
        http = new ActiveXObject("Microsoft.XMLHTTP");
   // url = directoryPath + "/" + url;
    
    if (url.length === 0) {
        alert("No attachment (file) is being uploaded by the user");
        return false;
    }
    else {
        http.open('HEAD', url, false);
        http.send();
        if (http.status === 200) {
            window.open(url, '_blank');
            return false;
        } else {
            alert("No attachment (file) is being uploaded by the user");
            return false;
        }
    }
}


function ValidNA_INT(ControlId) {
    //debugger;
    ControlId = "#" + ControlId;
    var a = $(ControlId).val();
    if (a.toString().toUpperCase() != "NA") {
        if (isNaN(a)) {
            alert('Invalid Value');
            $(ControlId).val('');
            $(ControlId).focus();
        }
    }
}

function ValidNA_MobNo(ControlId) {
    //debugger;
    ControlId = "#" + ControlId;
    var a = $(ControlId).val();
    var mobmatch1 = /^[456789]\d{9}$/;
    if (a.toString().toUpperCase() != "NA") {
        if (isNaN(a)) {
            alert('Invalid Value');
            $(ControlId).val('');
            $(ControlId).focus();
            return false;
        }

        if (!mobmatch1.test(a)) {
            // messageBox("error", "Please Enter valid mobile Number.");
            $(ControlId).addClass('generatedErorr');
            $(ControlId).val('');
            return false;
        }
    }
    $(ControlId).removeClass('generatedErorr');
}

function ValidNA_EmailId(ControlId) {
    // debugger;
    ControlId = "#" + ControlId;
    var a = $(ControlId).val();
    var mobmatch1 = /^([0-9a-zA-Z]([-_\\.]*[0-9a-zA-Z]+)*)@([0-9a-zA-Z]([-_\\.]*[0-9a-zA-Z]+)*)[\\.]([a-zA-Z]{2,9})$/// /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (a.toString().toUpperCase() != "NA") {

        if (!mobmatch1.test(a)) {
            // messageBox("error", "Please Enter Valid Email Address.");
            $(ControlId).addClass('generatedErorr');
            $(ControlId).val('');
            $(ControlId).focus();
            return false;
        }
    }
    $(ControlId).removeClass('generatedErorr');
}



