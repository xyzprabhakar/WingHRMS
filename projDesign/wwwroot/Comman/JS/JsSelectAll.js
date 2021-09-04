
$(document).ready(function () {

});


function BindCompanyListForddl(ControlId, SelectedVal) {

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompanyList',
        data: {},
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}



//bind loaction by company id 

function BindLocationListForddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationByCompany/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;



            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            if (res.statusCode != undefined) {
                $("#loader").hide();
                messageBox("error", res.message);
                return false;
            }

            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
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



function check_uncheck_checkbox(isChecked) {
    if (isChecked) {
        $('input[name="location"]').each(function () {
            this.checked = true;
        });
    } else {
        $('input[name="location"]').each(function () {
            this.checked = false;
        });
    }
}


//bind department by company id 

function BindDepartmentListForddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_DepartmentByCompany/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> --Select-- </option>');
            if (res.statusCode != undefined) {
                $("#loader").hide();
                messageBox("error", res.message);
                return false;
            }
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.department_id).html(value.department_name));
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

//bind sub department by company id 

function BindSubDepartmentListForddl(ControlId, DepartmentId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubDepartmentByDepartment/' + DepartmentId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> --Please Select-- </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.sub_department_id).html(value.sub_department_name));
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

//bind shift by company id 
function BindShiftListForddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_ShiftList/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.shift_details_id).html(value.shift_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//bind shift by company id 
function BindAllShiftListForddl(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        data: {},
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_AllShiftList/' + CompanyId,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> Select Shift </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
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


//bind grade by company id 
function BindGradeList(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_GradeMaster/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> --Please select-- </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.grade_id).html(value.grade_name));
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


//bind Designation List by company id
function BindDesignationList(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_DesignationList/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.designation_id).html(value.designation_name));
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

function BindAllShiftListForddlAll(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_AllShiftList/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            if (res.statusCode != undefined) {
                $("#loader").hide();
                messageBox("error", res.message);
                return false;
            }
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.shift_id).html(value.shift_name));
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


function BindSubLocationListForddl(ControlId, LocationId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubLocationByDepartment/' + LocationId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.sub_location_id).html(value.location_name));
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


function BindLoanByEmployee(ControlId, EmployeeId, SelectedVal) {


    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/GetLoanByEmployee/' + EmployeeId + '/0',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0"> -- Please Select -- </option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.loan_req_id).html(value.loan_purpose));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}


//bind Designation List by company id
function BindWorkingRoleList(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_WorkingRoleList/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.working_role_id).html(value.working_role_name));
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

function BindLocationForReport(ControlId, CompanyId, SelectedVal) {

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationForReport/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0"> All </option>');

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
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
        }
    });
}