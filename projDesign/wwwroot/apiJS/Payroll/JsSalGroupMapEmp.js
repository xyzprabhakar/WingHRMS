$('#loader').show();

var emp_role_idd;
var login_company_idd;
var login_empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindAllEmp_Company('ddlCompany', login_empid, login_company_idd);
        BindEmployeeCodeFromEmpMasterByComp('ddlemp', login_company_idd, 0);



        GetData();
        //BindEmpList('ddlemp', 0);
        BindGradeListt('ddlgrade', 0);
        BindSalaryGroupList('ddlsalgroup', 0, 0);
        //$('#ddlgrade').bind("change", function () {
        //    BindSalaryGroupList('ddlsalgroup', 0, $(this).val());
        //});

        $('#btnreset').show();
        $('#btnsave').show();

        //GetFunctionData();

        //$('#ddlsalgroup').bind("change", function () {
        //    BindFormulaList('ul_textbox', $(this).val(), $('#ddlgrade').val());

        //});

        $('#ddlemp').bind("change", function () {
            $('#loader').show();
            GetEmployeeGradeAllocation($(this).val(), 1);

            $('#loader').hide();
        });

        $('#btnupdate').hide();
        $('#btncal').bind("click", function () {
            var values = "";
            var total = 0;
            $("input[name=DynamicTextBox]").each(function () {
                if (!isNaN(this.value) && this.value.length != 0) {
                    total += parseFloat(this.value);
                }
                values += $(this).val() + "\n";

            });
            alert(values);
            alert(total);

            if (total == "" || total == null) {
                return false;
            }
            else if (total != "" || total != null) {
                $('#btnreset').show();
                $('#btnsave').show();

            }

        });
        //$("#txteffdate").datepicker({
        //    dateFormat: 'yy-mm-dd',
        //    minDate: 0,
        //    // onSelect: function (fromselected, evnt) {
        //    //$("#txttodate").datepicker('setDate', null);
        //    //$("#txttodate").datepicker({
        //    //    dateFormat: 'mm/dd/yy',
        //    //    minDate: fromselected,
        //    //    onSelect: function (toselected, evnt) {

        //    //        if (Date.parse(fromselected) > Date.parse(toselected)) {
        //    //            messageBox('error', 'To Date must be greater than from date !!');
        //    //            //$('#txtfromdate').val('');
        //    //            $('#txttodate').val('');
        //    //        }
        //    //    }
        //    //});
        //    // }

        //});






        $('#txteffdate').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                var _monthh = parseInt(iMonth) + 1;
                $("#hdneffectivedt").val(iYear + "-" + _monthh + "-01");
                //if ($("#ddlemp").val() != null && $("#ddlemp").val() != "0") {
                //    BindEmployeeDetailsByEmp($("#ddlemp").val());
                //    GetComponents($("#ddlemp").val());
                //    $('#div_emp_salay').show();
                //}
            },

            beforeShow: function () {
                if ((selDate = $(this).val()).length > 0) {
                    iYear = selDate.substring(selDate.length - 4, selDate.length);
                    iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                }

            }
        });


        $('#loader').hide();

        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlgrade option").remove();
            $("#ddlemp option").remove();

            BindEmployeeCodeFromEmpMasterByComp('ddlemp', $(this).val(), 0);


            $('#loader').hide();
        });


        $('#btnreset').bind('click', function () {
            //window.location.href = '/Payroll/SalaryGroupMapEmp';
            location.reload();
        });
        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var empId = $("#ddlemp").val();
            var grade = $('#ddlgrade').val();
            var sal_group = $('#ddlsalgroup').val();
            var effective_dt = $("#hdneffectivedt").val();//$('#txteffdate').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (empId == '' || empId == '0') {
                errormsg = "Please select Employee !! <br/>";
                iserror = true;
            }
            //if (grade == '' || grade == '0') {
            //    errormsg = errormsg + 'Please select Grade !! <br />';
            //    iserror = true;
            //}
            if (sal_group == '' || sal_group == '0') {
                errormsg = errormsg + 'Please select Salary Group !! <br />';
                iserror = true;
            }
            if (effective_dt == '' || effective_dt == null) {
                errormsg = errormsg + 'Please select effective Date !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'emp_id': empId,
                //'grade': grade,
                'salary_group_id': sal_group,
                'applicable_from_dt': effective_dt,
                'created_by': login_empid,
                'is_active': 1

            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_SalaryGroupMapEmp';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });


        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var empId = $("#ddlemp").val();
            var grade = $('#ddlgrade').val();
            var sal_group = $('#ddlsalgroup').val();
            var effective_dt = $("#hdneffectivedt").val(); //$('#txteffdate').val();
            var map_id = $("#hdnid").val();
            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (empId == '' || empId == '0') {
                errormsg = "Please select Employee !! <br/>";
                iserror = true;
            }
            //if (grade == '' || grade == '0') {
            //    errormsg = errormsg + 'Please select Grade !! <br />';
            //    iserror = true;
            //}
            if (sal_group == '' || sal_group == '0') {
                errormsg = errormsg + 'Please select Salary Group !! <br />';
                iserror = true;
            }
            //if (effective_dt == '' || effective_dt == null) {
            //    errormsg = errormsg + 'Please select effective Date !! <br />';
            //    iserror = true;
            //}

            if (iserror) {
                // messageBox("success", "successfully save data");
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {
                'map_id': map_id,
                'emp_Id': empId,
                'salary_group_id': sal_group,
                'applicable_from_dt': effective_dt,
                'modified_by': login_empid,
                'is_active': 1
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_SalaryGroupMapEmp';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                        //GetData();
                        //$("#hdnid").val('');
                        //$("#ddlemp").val('0');
                        //$('#ddlgrade').val('0');
                        //$('#ddlsalgroup').val('0');
                        //$('#txteffdate').val('');
                        //$('#btnupdate').hide();
                        //$('#btnsave').show();

                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });


    }, 2000);// end timeout

});

function GetEmployeeGradeAllocation(employee_id, status) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeGradeAllocation/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false;
            }


            if (res.grade_id != null) {

                BindGradeListt('ddlgrade', res.grade_id);
                $("#ddlgrade").val(res.grade_id);
                //if (status == 1) {
                //     BindSalaryGroupList('ddlsalgroup', 0, res.grade_id);
                //}
            }
            else {
                BindGradeListt('ddlgrade', 0);
            }
            $('#loader').hide();

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        return true;
    }
}
function BindSalaryGroupList(ControlId, SelectedVal, GradeId) {
    // debugger;
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({

        type: "GET",
        url: apiurl + "apiPayroll/GetSalaryGroupList/0/0", //+ GradeId + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.group_id).html(value.group_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
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

function BindFormulaList(ControlId, salgroupId, GradeId) {
    // debugger;
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({

        type: "GET",
        url: apiurl + "apiPayroll/GetFormulaList/" + salgroupId + "/" + GradeId + "",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $.each(res, function (data, value) {
                $("#ul_textbox").append($("<li style='padding-right:1%;'><input name = 'DynamicTextBox' type='text' placeholder='" + value.key_name + "' onkeypress='return isNumberKey(event)' style=' width:50%'></li>"));
            });

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}

//function BindEmpList(ControlId, CompanyId, SelectedVal) {
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
//            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
//            })

//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined' && SelectedVal != null) {
//                $(ControlId).val(SelectedVal);
//                $(ControlId).trigger("chosen:updated");
//                $(ControlId).chosen();
//            }
//            $(ControlId).trigger("chosen:updated");
//            $(ControlId).chosen();
//            $('#loader').hide();

//        },
//        error: function (err) {
//            alert(err.responseText);
//            $('#loader').hide();
//        }
//    });
//}
function BindGradeListt(ControlId, SelectedVal) {
    $('#loader').show();

    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_GradeMasterData/0",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            //$(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            //$.each(res, function (data, value) {
            //    $(ControlId).append($("<option></option>").val(value.gradeid).html(value.gradename));
            //})


            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.gradeid).html(value.gradename));
            })


            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
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

//--------bind data in jquery data table
function GetData() {

    // debugger;
    var apiurl = "";
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0) {

    //    apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_SalaryGroupMapEmpDataByComID/' + login_company_idd;
    //}
    //else {
    apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_SalaryGroupMapEmpData/0';
    //}
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;

            $('#loader').hide();
            $("#tblsalgroupmap").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"
                        },

                        {
                            "targets": [1],
                            "visible": false
                        }
                    ],
                "columns": [
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "map_id", "name": "map_id", "autoWidth": true, },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "title": "Grade", "autoWidth": true },
                    { "data": "group_name", "name": "group_name", "title": "Salary Group", "autoWidth": true },
                    //{ "data": "map_id", "name": "map_id", "autoWidth": true ,},
                    //{ "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    //{ "data": "emp_name", "name": "emp_name", "autoWidth": true },
                    //{ "data": "grade_name", "name": "grade_name", "autoWidth": true },
                    //{ "data": "group_name", "name": "group_name", "autoWidth": true },
                    { "data": "effective_dt", "name": "effective_dt", "title": "Effective Date", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.map_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });
        },
        error: function (error) {
            //alert(error);
            $('#loader').hide();
            messageBox("error", error.responseText);
            console.log("error");
        }
    });

}

function GetEditData(map_id) {
    $('#loader').show();
    if (map_id == null || map_id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    // debugger;
    $("#hdnid").val(map_id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_SalaryGroupMapEmpData/' + map_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            $("#ddlCompany").val(data[0].default_company_id);


            $("#ddlemp option").remove();
            BindEmployeeCodeFromEmpMasterByComp('ddlemp', data[0].default_company_id, data[0].emp_id);


            //BindGradeListt('ddlgrade', data[0].grade_id);

            BindSalaryGroupList('ddlsalgroup', data[0].salary_group_id, 0);
            //BindSalaryGroupList('ddlsalgroup', data[0].salary_group_id, data[0].grade_id);


            GetEmployeeGradeAllocation(data[0].emp_id, 2);

            //alert(data[0].grade_id + ' ' + data[0].salary_group_id);

            $('#ddlsalgroup').val(data[0].salary_group_id);

            // $('#txteffdate').val(data[0].effective_dt.substring(0, 10));

            var _month = data[0].effective_dt.substring(0, 10).toString().replace("-", "").substring(6, 4);
            var _yearr = data[0].effective_dt.substring(0, 10).toString().substring(0, 4);

            var monthname = GetMonthName(_month);
            $('#txteffdate').val(monthname + "-" + _yearr);
            $('#hdneffectivedt').val(monthname + "-" + _yearr);
            setselectedddate(data[0].effective_dt.substring(0, 10));

            //$('#ddlgrade').val(data[0].grade_id);           
            // SetGradeList('ddlsalgroup', data[0].grade_id, data[0].salary_group_id );      
            //$("#ddlsalgroup").val(data[0].salary_group_id);



            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

//-------update city data

function GetFunctionData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_FunctionList';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        //success: function (response) {
        //    var res = response;

        //    $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
        //    $.each(res, function (data, value) {
        //        $(ControlId).append($("<option></option>").val(value.sno).html(value.gradename));
        //    })

        //    //get and set selected value
        //    if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
        //        $(ControlId).val(SelectedVal);
        //    }
        //},
        error: function (error) {
            //alert(error);
            $('#loader').hide();
            console.log("error");
            messageBox("error", error.responseText);

        }
    });

}



function setselectedddate(datee) {

    //var fulldate = datee.toString().remove('-');


}


function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}