var emp_rolee_id;
var loginn_comp_id;
var login_empid;
var jj = jQuery.noConflict();


$(document).ready(function () {
    setTimeout(function () {

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = "/Login";
        }

        emp_rolee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) {
            return m === "'" ? '' : '';
        });
        loginn_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company("ddlCompany", login_empid, loginn_comp_id);
        BindEmployeeCodeFromEmpMasterByComp("ddlEmployee", loginn_comp_id, -1);
        GetData();



        $('#ddlCompany').bind("change", function () {

            $("#ddlEmployee option").remove();
            $("#ddlMonthyear option").remove();
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', $(this).val(), -1);
        });

        $('#ddlEmployee').bind("change", function () {
            //BindEmployeeDetailsByEmp($(this).val(), $('#ddlCompany').val());
            $("#ddlMonthyear option").remove();
            ProcessMonthyear('ddlMonthyear', 0, $(this).val());

        });

        $('#ddlMonthyear').bind("change", function () {
            $('#tbl_SalaryInput').empty();
            GetData();

        });



    });
});

function GetData() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryComponenetDetails/1';
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $('#loader').show();

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }
            debugger;
            var table = $('#tbl_SalaryInput').DataTable({
                "data": aData,
                select: "single",
                destroy: true,
                "columns": [
                    {
                        "className": 'details-control',
                        "orderable": false,
                        "data": null,
                        "defaultContent": '',
                        "render": function () {
                            return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
                        },
                        width: "15px"
                    },
                    { data: 'component_id', visible: false, name: 'component_id', title: 'component_id' },
                    { data: 'property_details', name: 'property_details', title: 'Item' },
                    { data: null, defaultContent: "", name: 'property_details', title: 'Value' },
                ],
                "order": [[1, 'asc']]
            });

            // Add event listener for opening and closing details
            $('#tbl_SalaryInput tbody').on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var tdi = tr.find("i.fa");
                var row = table.row(tr);
                debugger;
                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                    tdi.first().removeClass('fa-minus-square');
                    tdi.first().addClass('fa-plus-square');
                }
                else {
                    // Open this row
                    var t = row.child(ShowChildGrid(row.data()));
                    if (t == undefined) {
                        return;
                    }
                    t.show();
                    tr.addClass('shown');
                    tdi.first().removeClass('fa-plus-square');
                    tdi.first().addClass('fa-minus-square');
                }
            });

            table.on("user-select", function (e, dt, type, cell, originalEvent) {
                if ($(cell.node()).hasClass("details-control")) {
                    e.preventDefault();
                }
            });


            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }


    });
}

function ProcessMonthyear(ControlId, SelectedVal, employee_id) {
    $('#loader').show();
    ControlId = '#' + ControlId;

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_two_ProcessedMonthyear/" + employee_id,
        data: {},
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
            $(ControlId).empty().append('<option selected="selected" value="0">---Please select---</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.monthyear).html(value.monthyear));
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


function ShowChildGrid(d) {
    debugger;
    var company_id = $('#ddlCompany').val();
    var emp_id = $('#ddlEmployee').val();

    var monthyear = $('#ddlMonthyear').val();

    if (company_id == '' || company_id == "0" || company_id == null) {
        alert('Pleae select Company ID !!!!');
        return;
    }
    if (emp_id == '' || emp_id == 0) {
        alert('Pleae select Employee ID !!!!');
        return;
    }
    else if (monthyear == '' || monthyear == null || monthyear == "0") {
        alert('Pleae select MomnthYear !!!!');
        return;
    }
    else {

        $('#loader').show();

        var table_name_ = "tbl_SI_child" + d.component_id.toString();

        // send the parent row primary key to the server so that we know which grid to show
        var childGridURL = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryChildComponenetDetails/' + d.component_id + '/' + $('#ddlEmployee').val() + '/' + $('#ddlMonthyear').val() + "/" + company_id;

        // add a table and pager HTML elements to the parent grid row - we will render the child grid here
        var childTable = '<table class="root" id=' + table_name_ + '></table> <div class=" btn-group"> <button type = "button" class="btn btn-primary btn-lg" id = "btnsave" onclick= SaveData("' + table_name_.toString() + '")> Save </button ></div>';

        $.ajax({
            url: childGridURL,
            type: "GET",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {
                debugger;
                $('#loader').hide();

                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    return false;
                }

                if (res != '' && res != null && res != undefined) {
                    _GUID_New();
                    var aData = res;
                    if (res.statusCode != undefined) {
                        messageBox("info", res.message);
                        return false;
                    }
                    debugger;
                    var table = $('#' + table_name_).DataTable({
                        "data": aData,
                        "searching": false,
                        "bSort": false,
                        "paging": false,
                        "info": false,
                        "columns": [
                            { data: 'component_id', visible: false, name: 'component_id', },
                            { data: 'is_data_entry_comp', name: 'is_data_entry_comp', visible: false },
                            { data: 'parentid', name: 'parentid', visible: false },
                            { data: 'property_details', name: 'property_details' },
                            { data: 'component_value', name: 'component_value' },
                            {
                                "autoWidth": true,
                                "render": function (data, type, full, meta) {
                                    if (full.is_data_entry_comp == "1")
                                        return '<input type="text" class="form-control" id=txtbox' + full.component_id + ' value=' + full.component_value + ' />';
                                    else
                                        return '<input type="text" class="form-control" disabled="disabled" id=txtbox' + full.component_id + ' value=' + full.component_value + ' />';
                                }
                            },

                        ],
                        "lengthMenu": [[-1], ["All"]],
                    });
                }

            }

        });


        return childTable;
    }
}


function SaveData(componentId) {
    $('#loader').show();
    debugger;
    var emp_id = $('#ddlEmployee').val();
    var component_ids = [];
    var component_values = [];
    var previous_values = [];
    var modified_by = 0;
    var monthyear = $('#ddlMonthyear').val();
    var comp_id = $("#ddlCompany").val();

    var tableSalComponent = $("#" + componentId).dataTable();
    $("input:text", tableSalComponent.fnGetNodes()).each(function () {
        var newValue = $(this).val();
        var currentRow = $(this).closest("tr");
        var data = $("#" + componentId).DataTable().row(currentRow).data();

        if (data.component_value != newValue) {
            component_ids.push(data.component_id);
            component_values.push(newValue);
            previous_values.push(data.component_value);

        }
    });

    var myData = {
        'emp_id': emp_id,
        'component_ids': component_ids,
        'component_values': component_values,
        'previous_values': previous_values,
        'modified_by': login_empid,
        'monthyear': monthyear,
        'company_id': comp_id
    };
    var Obj = JSON.stringify(myData);

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    var childGridURL_ = localStorage.getItem("ApiUrl") + "apipayroll/Save_SalaryInputt";
    $.ajax({
        url: childGridURL_,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (res) {
            _GUID_New();
            var message = "";

            if (res != '' && res != undefined && res != null) {
                $("#loader").hide();
                if (res.statusCode == 0) {
                    messageBox("success", res.message);
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                }
                else {
                    messageBox("info", res.message);
                }
            }
            $("#loader").hide();
        },
        error: function (error) {
            _GUID_New();
            $('#loader').hide();
            messageBox("error", "Server busy please try again later...!");
        }

    });

}
