$('#loader').show();
var login_role_id;
var default_company;
var login_empid;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        // debugger;


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_empid = CryptoJS.AES.decrypt(localStorage.getItem('emp_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_empid, default_company);
        GetAllComponents(0, default_company);
        GetAllTreeComponents(-1, default_company);
        //GetAllComponents(0, 0);
        //GetAllTreeComponents(-1, 0);


        // document.getElementById('tab_Functions_t').className += " active";

        //BindGradeList('ddlgrade', 0, 0);
        BindSalaryGroupList('ddlsalgroup', 0, 0);
        //Functions
        GetFunctions();

        //Operator
        GetOperator();


        //$('#ddlsalgroup').prop("disabled", true);
        //dragdivkeydeductions
        $('#dragdivkeydeductions1').each(function () {
            $(this).has('li').prop("disabled", true);
        });
        //$('#ddlgrade').bind("change", function () {
        //    BindSalaryGroupList('ddlsalgroup', 0, $(this).val());
        //    $('#ddlsalgroup').prop("disabled", false);
        //});


        $('#ddlsalgroup').bind("change", function () {

            $('#loader').show();
            var ddlcompany = $.trim($("#ddlcompany").val());

            var hdnComponentId = $.trim($("#hdnComponentId").val());

            //var ddlgrade = $.trim($("#ddlgrade").val());

            var hdnResetVal = $("#hdnResetVal").val();

            if (ddlcompany == '' || ddlcompany == 0 && hdnResetVal == 0) {
                alert("Please Select Company...!");
                BindSalaryGroupList('ddlsalgroup', 0, 0);
                $('#loader').hide();
                return;
            }

            if (hdnComponentId == '' || hdnComponentId == 0 && hdnResetVal == 0) {
                alert("Please Select Component...!");
                BindSalaryGroupList('ddlsalgroup', 0, 0);
                $('#loader').hide();
                return;
            }

            $("#hdnsalgroup").val($(this).val());

            //if (ddlgrade == '' || ddlgrade == 0 && hdnResetVal == 0) {
            //    alert("Please Select Grade...!");
            //    BindSalaryGroupList('ddlsalgroup', 0, 0);
            //    $('#loader').hide();
            //    return;
            //}

            GetFormulaBySalaryGroupId(hdnComponentId, $(this).val(), ddlcompany);

            $('#loader').hide();
        });


        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {
            GetAllComponents(0, $(this).val());
            GetAllTreeComponents(-1, $(this).val());
        });

        $('#evts_button').on("click", function () {
            var instance = $('#evts').jstree(true);
            instance.deselect_all();
            instance.select_node('1');
        });

        $('#btnNext').bind("click", function () {

            document.getElementById('tab2_btn').className += " active";

        });


        $('#btnBack').bind("click", function () {

            document.getElementById('tab1_btn').className += " active";

        });

        $('#btnAddChild').bind("click", function () {
            Reset();
        });

        $('#btnSaveRepository').click(function () {

            var company_id = $("#ddlcompany").val();
            if (company_id == '' || company_id == 0) {
                alert("Please Select Company...!");
                return;
            }

            var regexp = /^[a-zA-Z0-9-_]+$/;

            var txtNameComponent = $.trim($("#txtNameComponent").val());
            if (txtNameComponent == '') {
                alert("Please Enter Component Name...!");
                return;
            }

            var NameComponentFirstChar = txtNameComponent.charAt(0);
            if (NameComponentFirstChar != "@") {
                alert('Component Name Start With @...!');
                return;
            }
            var txtNameCom = txtNameComponent.substring(1, txtNameComponent.length);

            if (txtNameCom.search(regexp) == -1) {
                alert('Component Name Contains only alphabets, digits and under score only...!');
                return;
            }



            var hdnparentid = $("#hdnparentid").val();

            if (hdnparentid == '') {
                alert("Please Select Component...!");
                return;
            }

            var txtDescription = $.trim($("#txtDescription").val());
            if (txtDescription == '') {
                alert("Please Enter Component Description...!");
                return;
            }


            var SalaryType = 1;

            if ($("#is_Deduction").is(":checked")) {
                SalaryType = 2;
            }
            else if ($("#is_Others").is(":checked")) {
                SalaryType = 3;
            }
            else if ($("#is_Others_Income").is(":checked")) {
                SalaryType = 4;
            }
            else if ($("#is_Others_Deduction").is(":checked")) {
                SalaryType = 5;
            }

            // var payment_type = $("#payment_type").val();
            var data_type = $("#data_type").val();

            var is_SalaryMasterComponent = 0;
            if ($("#is_SalaryMasterComponent").is(":checked")) {
                is_SalaryMasterComponent = 1;
            }

            var is_TDSComponent = 0;
            if ($("#is_TDSComponent").is(":checked")) {
                is_TDSComponent = 1;
            }

            var is_DataEntryComponent = 0;
            if ($("#is_DataEntryComponent").is(":checked")) {
                is_DataEntryComponent = 1;
            };

            var txtfunction_calling_order = $.trim($("#txtfunction_calling_order").val());
            if (txtfunction_calling_order == '') {
                txtfunction_calling_order = 0;
            }

            var is_user_interface = 0;
            if ($("#is_UserInterface").is(":checked")) {
                is_user_interface = 1;
            };


            var is_payslip = 0;
            if ($("#is_payslip").is(":checked")) {
                is_payslip = 1;
            };

            var ddlsalgroup = $("#ddlsalgroup").val();

            var Formula = $.trim($("#dropdiv").val());
            //if (Formula == '') {
            //    alert("Please Create Formula...!");
            //    return;
            //}
            var is_active = 1;
            //var emp_id = localStorage.getItem('emp_id');


            //Validation


            if (ddlsalgroup == null || ddlsalgroup == 0) {
                alert("Please Select Salary Group...!");
                return;
            }


            var hdnComponentId = $("#hdnComponentId").val();
            var hdnComponentIdForCheck = $("#hdnComponentIdForCheck").val();

            if (hdnComponentId > 0) {
                var myData = {
                    'component_id': hdnComponentId,
                    'component_name': txtNameComponent,
                    'parentid': parseInt(hdnparentid) == parseInt(hdnComponentId) ? 0 : hdnparentid,
                    'property_details': txtDescription,
                    'component_type': SalaryType,
                    'payment_type': 0,
                    'is_salary_comp': is_SalaryMasterComponent,
                    'is_tds_comp': is_TDSComponent,
                    'formula': Formula,
                    'salary_group_id': ddlsalgroup,
                    'is_active': is_active,
                    'created_by': login_empid,
                    'modified_by': login_empid,
                    'is_data_entry_comp': is_DataEntryComponent,
                    'company_id': company_id,
                    'function_calling_order': txtfunction_calling_order,
                    'user_interface': is_user_interface,
                    'is_payslip': is_payslip,
                    'datatype': data_type,

                }
            }
            else {
                var myData = {
                    'component_id': 0,
                    'component_name': txtNameComponent,
                    'parentid': hdnComponentIdForCheck,
                    'property_details': txtDescription,
                    'component_type': SalaryType,
                    'payment_type': 0,
                    'is_salary_comp': is_SalaryMasterComponent,
                    'is_tds_comp': is_TDSComponent,
                    'formula': Formula,
                    'salary_group_id': ddlsalgroup,
                    'is_active': is_active,
                    'created_by': login_empid,
                    'modified_by': login_empid,
                    'is_data_entry_comp': is_DataEntryComponent,
                    'company_id': company_id,
                    'function_calling_order': txtfunction_calling_order,
                    'user_interface': is_user_interface,
                    'is_payslip': is_payslip,
                    'datatype': data_type,
                }
            }
            // debugger;
            // console.log(JSON.stringify(myData));
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            //  return;
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiPayroll/CreateFormula/",
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        alert(Msg);
                        window.location.href = '/payroll/repository';
                    }
                    else if (statuscode == "0") {
                        alert(Msg);
                    }
                },
                error: function (err, exception) {
                    //alert(JSON.stringify(err));
                    _GUID_New();
                    messageBox("error", "Server busy please try again later...!");
                }
            });
        });


    }, 2000);// end timeout

});





//jstree

// html demo
$('#html').jstree();
// interaction and events


//For Drag And Drop
$(function () {
    $("#dragdivkeyearnings li").draggable({
        appendTo: "body",
        helper: "clone",
        cursor: "move",
        revert: "invalid"
    });



    $("#dragdivkeydeductions li").draggable({
        appendTo: "body",
        helper: "clone",
        cursor: "move",
        revert: "invalid"
    });

    initDroppable($("#dropdiv"));
    function initDroppable($elements) {
        var txtfunction_calling_order = 0;
        $elements.droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-drop-hover",
            accept: ":not(.ui-sortable-helper)",

            over: function (event, ui) {
                var $this = $(this);
            },
            drop: function (event, ui) {
                var $this = $(this);
                if ($this.val() == '') {
                    $this.val(ui.draggable.data("value"));
                    // debugger;
                    var comp_calling_order = ui.draggable.data("id");
                    if (comp_calling_order >= txtfunction_calling_order) {
                        txtfunction_calling_order = comp_calling_order + 1;
                        $("#txtfunction_calling_order").val(txtfunction_calling_order);
                    }

                } else {
                    // debugger;
                    $this.val($this.val() + "" + ui.draggable.data("value"));
                    var comp_calling_order = ui.draggable.data("id");
                    if (comp_calling_order >= txtfunction_calling_order) {
                        txtfunction_calling_order = comp_calling_order + 1;
                        $("#txtfunction_calling_order").val(txtfunction_calling_order);
                    }
                }
            }
        });
    }
});

//Fromula Tab 
//function OnFromulaTabClick(evt, TabName) {

//    //alert(evt);
//    if (TabName == 'tab_Functions') {
//        document.getElementById('tab_Functions').style.display = "block";
//        document.getElementById('tab_Components').style.display = "none";
//        document.getElementById('tab_Operator').style.display = "none";
//        document.getElementById('tab_Functions_t').className += " active";
//        document.getElementById('tab_Operator_t').classList.remove("active");
//        document.getElementById('tab_Components_t').classList.remove("active");
//    }
//    else if (TabName == 'tab_Operator') {
//        document.getElementById('tab_Operator').style.display = "block";
//        document.getElementById('tab_Functions').style.display = "none";
//        document.getElementById('tab_Components').style.display = "none";
//        document.getElementById('tab_Operator_t').className += " active"
//        document.getElementById('tab_Functions_t').classList.remove("active");
//        document.getElementById('tab_Components_t').classList.remove("active");

//    }
//    else if (TabName == 'tab_Components') {
//        document.getElementById('tab_Components').style.display = "block";
//        document.getElementById('tab_Functions').style.display = "none";
//        document.getElementById('tab_Operator').style.display = "none";
//        document.getElementById('tab_Components_t').className += " active"
//        document.getElementById('tab_Functions_t').classList.remove("active");
//        document.getElementById('tab_Operator_t').classList.remove("active");
//    }
//    else {
//        document.getElementById('tab_Functions').style.display = "none";
//        document.getElementById('tab_Components').style.display = "none";
//        document.getElementById('tab_Operator').style.display = "none";
//    }
//}




function clear_btn() {
    $('#dropdiv').val('');
}

//Bind Salary Group List
function BindSalaryGroupList(ControlId, SelectedVal, GradeId) {
    // debugger;
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({

        type: "GET",
        url: apiurl + "apiPayroll/GetSalaryGroupList/0/" + GradeId + "",
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


//bind Functions
function GetFunctions() {

    var urls = apiurl + 'apiPayroll/GetFunctions';

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {

            var res = response;

            $.each(res, function (data, value) {
                $('#dragdivfuncation ul').append('<li data-value=' + value.funValue + ' >' + value.funText + '</li>');
            });
            $("#dragdivfuncation li").draggable({
                appendTo: "body",
                helper: "clone",
                cursor: "move",
                revert: "invalid"
            });
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//bind GetOperator
function GetOperator() {
    var urls = apiurl + 'apiPayroll/GetOperator';

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            // debugger;
            var res = response;

            $.each(res, function (data, value) {
                $('#dragdivkeyearnings ul').append('<li data-value="' + value.funValue + '"  >' + value.funText + '</li>');
            });
            $("#dragdivkeyearnings li").draggable({
                appendTo: "body",
                helper: "clone",
                cursor: "move",
                revert: "invalid"
            });
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//bind GetComponents
function GetComponentsById(component_id, company_id) {

    // var default_company = localStorage.getItem('company_id');
    //var HaveDisplay = ISDisplayMenu("Display Company List");
    //if (HaveDisplay == 0) {
    //    BindCompanyList('ddlcompany', default_company);
    //}
    //else {
    //   // BindCompanyList('ddlcompany', 0);
    //    BindCompanyList('ddlcompany', company_id);
    //}


    // BindGradeList('ddlgrade', 0, 0)

    // BindSalaryGroupList('ddlsalgroup', 0, 0);


    $("#dropdiv").val('');

    var urls = apiurl + 'apiPayroll/GetComponents/' + component_id + '/' + company_id;

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            //debugger;
            var res = response;
            //console.log(JSON.stringify(res));

            //Bind data in fields

            //if (res[0].company_id != '') {
            //    BindCompanyList('ddlcompany', res[0].company_id);
            //}
            if (res.component_name != '') {
                $("#txtNameComponent").val(res.component_name);
            }

            if (res.parentid != '') {
                $("#hdnparentid").val(res.parentid);
            }

            if (res.property_details != null) {
                $("#txtDescription").val(res.property_details);
            }
            if (res.component_type == 1) {
                $("#is_Income").prop('checked', true);
            }
            else if (res.component_type == 2) {
                $("#is_Deduction").prop('checked', true);
            }
            else if (res.component_type == 3) {
                $("#is_Others").prop('checked', true);
            }
            else if (res.component_type == 4) {
                $("#is_Others_Income").prop('checked', true);
            }
            else if (res.component_type == 5) {
                $("#is_Others_Deduction").prop('checked', true);
            }
            else {
                $("#is_Income").prop('checked', false);
                $("#is_Deduction").prop('checked', false);
                $("#is_Others").prop('checked', false);
                $("#is_Others_Income").prop('checked', false);
                $("#is_Others_Deduction").prop('checked', false);
            }


            $("#data_type").val(res.datatype);

            if (res.is_salary_comp == 1) {
                $("#is_SalaryMasterComponent").prop('checked', true);
            }
            else {
                $("#is_SalaryMasterComponent").prop('checked', false);
            }
            if (res.is_tds_comp == 1) {
                $("#is_TDSComponent").prop('checked', true);
            }
            else {
                $("#is_TDSComponent").prop('checked', false);
            }

            if (res.is_data_entry_comp == 1) {
                $("#is_DataEntryComponent").prop('checked', true);
            }
            else {
                $("#is_DataEntryComponent").prop('checked', false);
            }

            if (res.is_user_interface == 1) {
                $("#is_UserInterface").prop('checked', true);
            }
            else {
                $("#is_UserInterface").prop('checked', false);
            }

            if (res.is_payslip == 1) {
                $("#is_payslip").prop('checked', true);
            }
            else {
                $("#is_payslip").prop('checked', false);
            }


            if ($("#hdnsalgroup").val() != null && $("#hdnsalgroup").val() != "0" && $("#hdnsalgroup").val() != "") {
                BindSalaryGroupList('ddlsalgroup', $("#hdnsalgroup").val(), 0);
                GetFormulaBySalaryGroupId(component_id, $("#hdnsalgroup").val(), company_id);

            }
            else {
                BindSalaryGroupList('ddlsalgroup', 0, 0);
            }



            //if (res[0].function_calling_order != '') {
            //    $("#txtfunction_calling_order").val(res[0].function_calling_order);
            //}

            //BindGradeList('ddlgrade', res[0].company_id, res[0].grade_Id);

            //BindSalaryGroupList('ddlsalgroup', res[0].salary_group_id, res[0].grade_Id);
            //// debugger;
            //if (res[0].formula != '') {
            //    $("#dropdiv").val(res[0].formula);
            //}
            //else {
            //    $("#dropdiv").val('');
            //}

        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}


function GetFormulaBySalaryGroupId(component_id, salary_group_id, company_id) {

    var urls = apiurl + 'apiPayroll/GetFormulaBySalaryGroupId/' + component_id + '/' + salary_group_id + '/' + company_id;

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            // debugger;
            var res = response;
            //console.log(JSON.stringify(res));
            //Bind data in fields


            if (res.function_calling_order != '') {
                $("#txtfunction_calling_order").val(res.function_calling_order);
            }

            // debugger;
            if (res.formula != '') {
                $("#dropdiv").val(res.formula);
            }
            else {
                $("#dropdiv").val('');
            }

        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//bind GetComponents
function GetAllComponents(component_id, company_id) {

    var urls = apiurl + 'apiPayroll/GetComponents/0/' + company_id;

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
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
            // console.log(JSON.stringify(res));

            $.each(res, function (data, value) {
                $('#dragdivkeydeductions ul').append('<li data-id=' + value.function_calling_order + ' data-value=' + value.component_name + ' >' + value.property_details + '</li>');
            });
            $("#dragdivkeydeductions li").draggable({
                appendTo: "body",
                helper: "clone",
                cursor: "move",
                revert: "invalid"
            });



        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//bind Get All Tree Components
function GetAllTreeComponents(component_id, company_id) {

    var urls = apiurl + 'apiPayroll/GetComponents/-1/' + company_id;

    $.ajax({
        type: "GET",
        url: urls,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            // // debugger;

            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            var treedate = JSON.stringify(res);

            // console.log(treedate);
            $('#evts').on("changed.jstree", function (e, data) {
                if (data.selected.length) {
                    document.getElementById('hdnComponentId').value = data.instance.get_node(data.selected[0]).id;
                    document.getElementById('hdnparentid').value = data.instance.get_node(data.selected[0]).id;
                    document.getElementById('hdnComponentIdForCheck').value = data.instance.get_node(data.selected[0]).id;
                    document.getElementById('txtNameComponent').disabled = true;
                    GetComponentsById($("#hdnparentid").val(), $("#ddlcompany").val());

                }
            }).jstree({
                'core': {
                    'multiple': false,
                    'data': JSON.parse(treedate)
                }
            });

        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//Save Add Child


function Reset() {
    document.getElementById('txtNameComponent').disabled = false;
    $("#hdnComponentId").val(0);
    $("#hdnResetVal").val(1);

    //var default_company = localStorage.getItem('company_id');

    BindAllEmp_Company('ddlcompany', login_empid, default_company);


    $("#txtNameComponent").val('');
    $("#txtDescription").val('');
    $("#data_type").val(0);
    $('#is_Income').prop('checked', true);
    $('#is_SalaryMasterComponent').prop('checked', false);
    $('#is_TDSComponent').prop('checked', false);
    $('#is_DataEntryComponent').prop('checked', false);
    $('#is_UserInterface').prop('checked', false);
    // $("#ddlgrade").val(0);
    $("#txtfunction_calling_order").val('');
    $("#ddlsalgroup").val(0);
    $("#dropdiv").val('');
}







function salary_component_info() {
    messageBox("info", "By selection of this option the defined component will be added in Salary Structure.");
}


function data_entry_component_info() {
    messageBox("info", "By selection of this option the defined component will not bear a formula, but a Positive integer value.");
}

function user_interface_info() {
    messageBox("info", "By selection of this option the defined component will be added and dislayed in the Payroll Input dashboard.");
}


function show_in_payslip_info() {
    messageBox("info", "By selection of this option the defined component will appear in the Salary Slip.");
}



