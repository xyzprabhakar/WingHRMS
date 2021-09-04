var default_company;
var login_emp;
var criteria_number;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        BindKPIObjectiveType('dll_objective_type', default_company, 0);
        BindDepartmentList('ddldepartment', default_company, -1);
        GetkpiCriteria(default_company);

        // debugger;
        GetData(default_company);




        $('#btnupdate').hide();
        $('#btnsave').show();


        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {

            }
        });

        $("#txttodate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
            }
        });

        $('#loader').hide();


        $('#ddlcompany').bind("change", function () {
            if ($(this).val() > 0) {
                BindDepartmentList('ddldepartment', $(this).val(), -1);
                BindWorkingRoleByDeptID('ddl_working_role', $(this).val(), 0, 0);
                BindKPIObjectiveType('dll_objective_type', $(this).val(), 0)
                GetkpiCriteria($(this).val());
                GetData($(this).val());
            }
            else {
                if ($.fn.DataTable.isDataTable('#tbl_kpi_key_area_master')) {
                    $('#tbl_kpi_key_area_master').DataTable().clear().draw();
                }
                $("#dll_objective_type option[value!=0]").remove();
                $("#ddldepartment option[value!=0]").remove();
                $("#ddl_working_role option[value!=0]").remove();
                $("#txt_key_area").val('');
                $("#txt_expected_result").val('');
                $("#txtremarks").val('');
                $("#txt_wtg").val('');
                $('#tbla').find('input:text').each(function () {
                    $('input:text[id=' + $(this).attr('id') + ']').val('');
                }
                );
                $('#btnupdate').hide();
                $('#btnsave').show();
            }


        });

        $('#ddldepartment').bind("change", function () {
            var comp_id = $('#ddlcompany').val();
            if (comp_id > 0) {
                BindWorkingRoleByDeptID('ddl_working_role', comp_id, $(this).val(), 0);
            }
            else {
                $("#ddl_working_role option[value!=0]").remove();
            }

        });


        $('#btnreset').bind('click', function () {
            reset_fi();
        });
        $('#btnsave').bind("click", function () {
            // debugger;

            var company_id = $("#ddlcompany").val();
            var dll_objective_type = $("#dll_objective_type").val();
            var txt_key_area = $("#txt_key_area").val();
            var txt_expected_result = $("#txt_expected_result").val();
            var txt_wtg = $("#txt_wtg").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddl_working_role = $("#ddl_working_role").val();
            var txtremarks = $("#txtremarks").val();

            var criteria_key = [];
            for (var i = 1; i <= criteria_number; i++) {
                if ($('#txt_criteria_' + i).val() != '') {
                    criteria_key.push(i);
                }
            }


            var criteria_value = [];

            for (var i = 1; i <= criteria_number; i++) {
                if ($('#txt_criteria_' + i).val() != '') {
                    criteria_value.push($('#txt_criteria_' + i).val());
                }
            }

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (company_id == null || company_id == '' || company_id == "0") {
                errormsg = "Please select company !! <br/>";
                iserror = true;
            }

            if (dll_objective_type == '' || dll_objective_type == null) {
                errormsg = errormsg + 'Please select objective type !! <br />';
                iserror = true;
            }
            if (txt_key_area == '' || txt_key_area == null) {
                errormsg = errormsg + 'Please enter key area  !! <br />';
                iserror = true;
            }
            if (txt_expected_result == '' || txt_expected_result == null) {
                errormsg = errormsg + 'Please Enter Expected Result !! <br />';
                iserror = true;
            }
            if (txt_wtg == '' || txt_wtg == null) {
                errormsg = errormsg + 'Please Enter wtg !! <br />';
                iserror = true;
            }
            if (ddldepartment == '' || ddldepartment == null) {
                errormsg = errormsg + 'Please Select Department !! <br />';
                iserror = true;
            }

            if (ddl_working_role == '' || ddl_working_role == null) {
                errormsg = errormsg + 'Please Select Working Role !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();
            var myData = {
                'company_id': company_id,
                'w_r_id': ddl_working_role,
                'otype_id': dll_objective_type,
                'key_area': txt_key_area,
                'expected_result': txt_expected_result,
                'wtg': txt_wtg,
                'created_by': login_emp,
                'criteria_number': criteria_key,
                'criteria': criteria_value,
                'comment': txtremarks,
            };


            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_KpiKeyAreaMaster';
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
                    if (statuscode == "0") {
                        GetData(default_company);
                        reset_fi();
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            });
        });


        $("#btnupdate").bind("click", function () {
            // debugger;

            var company_id = $("#ddlcompany").val();
            var dll_objective_type = $("#dll_objective_type").val();
            var txt_key_area = $("#txt_key_area").val();
            var txt_expected_result = $("#txt_expected_result").val();
            var txt_wtg = $("#txt_wtg").val();
            var ddldepartment = $("#ddldepartment").val();
            var ddl_working_role = $("#ddl_working_role").val();
            var hdnid = $("#hdnid").val();
            var txtremarks = $("#txtremarks").val();

            var criteria_key = [];
            for (var i = 1; i <= criteria_number; i++) {
                if ($('#txt_criteria_' + i).val() != '') {
                    criteria_key.push(i);
                }
            }


            var criteria_value = [];

            for (var i = 1; i <= criteria_number; i++) {
                if ($('#txt_criteria_' + i).val() != '') {
                    criteria_value.push($('#txt_criteria_' + i).val());
                }
            }

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (company_id == null || company_id == '') {
                errormsg = "Please select company !! <br/>";
                iserror = true;
            }

            if (dll_objective_type == '' || dll_objective_type == null) {
                errormsg = errormsg + 'Please select objective type !! <br />';
                iserror = true;
            }
            if (txt_key_area == '' || txt_key_area == null) {
                errormsg = errormsg + 'Please enter key area  !! <br />';
                iserror = true;
            }
            if (txt_expected_result == '' || txt_expected_result == null) {
                errormsg = errormsg + 'Please Enter Expected Result !! <br />';
                iserror = true;
            }
            if (txt_wtg == '' || txt_wtg == null) {
                errormsg = errormsg + 'Please Enter wtg !! <br />';
                iserror = true;
            }
            if (ddldepartment == '' || ddldepartment == null) {
                errormsg = errormsg + 'Please Select Department !! <br />';
                iserror = true;
            }

            if (ddl_working_role == '' || ddl_working_role == null) {
                errormsg = errormsg + 'Please Select Working Role !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            $('#loader').show();
            var myData = {
                'key_area_id': hdnid,
                'company_id': company_id,
                'w_r_id': ddl_working_role,
                'otype_id': dll_objective_type,
                'key_area': txt_key_area,
                'expected_result': txt_expected_result,
                'wtg': txt_wtg,
                'created_by': login_emp,
                'criteria_number': criteria_key,
                'criteria': criteria_value,
                'comment': txtremarks,
            };


            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_KpiKeyAreaMaster';
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
                    if (statuscode == "0") {
                        GetData(default_company);
                        reset_fi();
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            });
        });


    }, 2000);// end timeout

});


function reset_fi() {
    BindAllEmp_Company('ddlcompany', login_emp, default_company);
    BindEPA_FiscallYrByComp('ddl_session', default_company, 0);
    BindKPIObjectiveType('dll_objective_type', default_company, 0)
    BindDepartmentList('ddldepartment', default_company, -1);
    GetkpiCriteria(default_company);
    BindWorkingRoleByDeptID('ddl_working_role', default_company, 0, 0);
    $("#txt_key_area").val('');
    $("#txt_expected_result").val('');
    $("#txtremarks").val('');
    $("#txt_wtg").val('');
    $('#tbla').find('input:text').each(function () {
        $('input:text[id=' + $(this).attr('id') + ']').val('');
    }
    );
    $('#btnupdate').hide();
    $('#btnsave').show();
}


//--------bind data in jquery data table
function GetData(company_id) {
    // debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_KpiKeyAreaMasterByCompany/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tbl_kpi_key_area_master").DataTable({
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
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "working_role_name", "name": "working_role_name", "autoWidth": true, "title": "Working Role" },
                    { "data": "objective_name", "name": "objective_name", "autoWidth": true, "title": "Objective" },
                    { "data": "key_area", "name": "key_area", "autoWidth": true, "title": "key Area" },
                    { "data": "expected_result", "name": "expected_result", "autoWidth": true, "title": "Expected Result" },
                    { "data": "comment", "name": "comment", "title": "Remarks/Comment", "autoWidth": true },
                    { "data": "wtg", "name": "wtg", "wtg": true, "title": "Wtg" },
                    { "data": "created_date", "name": "todate", "created_date": true, "title": "Created Date" },
                    {
                        "title": "View", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="ViewDetails(' + full.key_area_id + ')" data-toggle="tooltip" title="View" ><i class="fas fa-eye"></i></a>';

                        }
                    },
                    {
                        "title": "Edit", "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.key_area_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            console.log("error");
            $('#loader').hide();
        }
    });

}

function GetEditData(key_area_id) {

    if (key_area_id == null || key_area_id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    $("#hdnid").val(key_area_id);

    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_KpiKeyAreaBYCriteriaId/' + key_area_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var get_kpi_key_area = res.get_kpi_key_area;
            $('#dll_objective_type option').remove();
            BindKPIObjectiveType('dll_objective_type', default_company, get_kpi_key_area.otype_id);
            BindAllEmp_Company('ddlcompany', login_emp, get_kpi_key_area.company_id);
            BindDepartmentList('ddldepartment', get_kpi_key_area.company_id, get_kpi_key_area.dept_id);
            BindWorkingRoleByDeptID('ddl_working_role', get_kpi_key_area.company_id, get_kpi_key_area.dept_id, get_kpi_key_area.w_r_id);
            $("#txt_key_area").val(get_kpi_key_area.key_area);
            $("#txt_expected_result").val(get_kpi_key_area.expected_result);
            $("#txt_wtg").val(get_kpi_key_area.wtg);
            $("#txtremarks").val(get_kpi_key_area.comment);


            var get_criteria_data = res.get_criteria_data;

            //  $("#tbla").remove();

            //            var criteria_number = res.count;
            var tableString = "<table id='tbla' class='col-md-12 text-srigth' style='width:100%'>";
            for (var i = 1; i <= criteria_number; i++) {

                if (i <= res.count) {
                    tableString += gg_stringformat("<tr><td>{0}{1}</td></tr>", "<div class='col-md-6 form-group'><div class='col-md-4 text-rigth><label class='col-form-label'>Criteria(" + i + ")</label></div>", "<div class='col-md-8'><input class='form-control' type='text' id='txt_criteria_" + i + "' value='" + get_criteria_data[i - 1].criteria + "' style='autocomplete='off'/></div></div>");
                }
                else {
                    tableString += gg_stringformat("<tr><td>{0}{1}</td></tr>", "<div class='col-md-6 form-group'><div class='col-md-4 text-rigth><label class='col-form-label'>Criteria(" + i + ")</label></div>", "<div class='col-md-8'><input class='form-control' type='text' id='txt_criteria_" + i + "' style='autocomplete='off'/></div></div>");
                }
            }
            tableString += "</table>";
            $('#divb').html(tableString);
            $('#loader').hide();




            $('#btnupdate').show();
            $('#btnsave').hide();

        },
        error: function (err) {
            $('#loader').hide();
        }
    });
}


function gg_stringformat() {
    var argcount = arguments.length,
        string,
        i;

    if (!argcount) {
        return "";
    }
    if (argcount === 1) {
        return arguments[0];
    }
    string = arguments[0];
    for (i = 1; i < argcount; i++) {
        string = string.replace(new RegExp('\\{' + (i - 1) + '}', 'gi'), arguments[i]);
    }
    return string;
}


function GetkpiCriteria(company_id) {


    if (company_id == null || company_id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_KpiCriteria/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            criteria_number = data.criteria_count;
            var tableString = "<table id='tbla' style='width:100%'>";
            for (var i = 1; i <= criteria_number; i++) {
                tableString += gg_stringformat("<tr><td>{0}{1}</td></tr>", "<div class='col-md-6 form-group'><div class='col-md-4 text-rigth><label class='col-form-label'> Criteria(" + i + ") </label></div>", "<div class='col-md-8'><input type='text' class='form-control' id='txt_criteria_" + i + "' autocomplete='off'/> </div></div>");
            }
            tableString += "</table>";
            $('#divb').html(tableString);
            $('#loader').hide();
        },
        error: function (err) {

        }
    });
}

//-------update city data


function ViewDetails(key_area_id) {

    $('#loader').show();

    $("#myModal").show();
    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $('#myModal').dialog({
        modal: 'true',
        title: 'Criteria Detail'
    });

    var apiurll = localStorage.getItem("ApiUrl") + "/apiePA/Get_KpiKeyAreaBYCriteriaId/" + key_area_id;


    $.ajax({

        type: "GET",
        url: apiurll,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var get_criteria_data = response.get_criteria_data;

            $("#tbl_criterias").DataTable({
                "bDestroy": true,
                "aaData": get_criteria_data,
                "columnDefs": [
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "title": "S. No.", "autoWidth": true },
                    { "data": "criteria", "name": "criteria", "title": "Criteria", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });

            $('#loader').hide();

        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }

    });
    $('#loader').hide();
}