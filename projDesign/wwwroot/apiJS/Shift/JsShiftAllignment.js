$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        // BindAllLocationsFromLocatinMaster('ddllocation', 0);
        //BindAllDepartmentFromDepartmentMaster('ddldepartment', 0);
        BindShift_ddl('ddlshift', default_company, -1);
        BindLocationListForddl('ddllocation', default_company, 0);
        BindDepartmentList('ddldepartment', default_company, 0);
        //if (HaveDisplay == 0) {
        //    BindCompanyList('ddlcompany', default_company);
        //    $('#ddlcompany').prop("disabled", "disabled");
        //    BindLocationList('ddllocation', default_company, 0);
        //    BindDepartmentList('ddldepartment', default_company, 0);
        //    BindShift_ddl('ddlshift', default_company, 0);

        //}
        //else {
        //    BindCompanyListAll('ddlcompany', 0);

        //}

        $("#btnUpdate").hide();



        $('#loader').hide();

        $("#btnReset").bind("click", function () {
            location.reload();
        });

        $("#ddllocation").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tblempdetail')) {
                $('#tblempdetail').DataTable().clear().draw();
            }
        });

        $("#ddldepartment").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tblempdetail')) {
                $('#tblempdetail').DataTable().clear().draw();
            }
        });

        $("#ddlcompany").bind("change", function () {

            if ($.fn.DataTable.isDataTable('#tblempdetail')) {
                $('#tblempdetail').DataTable().clear().draw();
            }

            BindLocationListForddl('ddllocation', $(this).val(), 0);
            BindDepartmentList('ddldepartment', $(this).val(), 0);
            if ($(this).val() == 0) {
                $("#ddlshift").empty();
            }
            else {
                BindShift_ddl('ddlshift', 0, -1);
            }
        });

        $("#btnsearch").bind("click", function () {
            if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == "null") {
                messageBox("error", "Please select  company");
                return false;
            }
            var loc_id = "";
            if ($("#ddllocation").val() == null || $("#ddllocation").val() == "" || $("#ddllocation").val() == "0") {
                loc_id = -1;
            }
            else {
                loc_id = $("#ddllocation").val();
            }

            var dep_id = "";
            if ($("#ddldepartment").val() == null || $("#ddldepartment").val() == "" || $("#ddldepartment").val() == "0") {
                dep_id = -1;
            }
            else {
                dep_id = $("#ddldepartment").val();
            }

            if ($.fn.DataTable.isDataTable('#tblempdetail')) {
                $('#tblempdetail').DataTable().clear().draw();
            }

            BindEmployeeDetail($("#ddlcompany").val(), $("#ddllocation").val(), $("#ddldepartment").val());

        });


        $("#btnSave").bind("click", function () {
            var company_id = $("#ddlcompany").val();
            var loc_id = $("#ddllocation").val();
            var dept_id = $("#ddldepartment").val();
            var effective_dt = $("#txteffective_dt").val();
            var shift_id = $("#ddlshift").val();

            var is_error = false;
            var err_msg = "";

            if (company_id == "" || company_id == null) {
                err_msg = err_msg + "Please Select Company </br>";
                is_error = true;
            }

            if (shift_id == "" || shift_id == "0" || shift_id == null) {
                err_msg = err_msg + "Please select Shift </br>";
                is_error = true;
            }

            if (effective_dt == "" || effective_dt == null) {
                err_msg = err_msg + "Please select Effective Date </br>";
                is_error = true;
            }

            if (new Date(effective_dt) < (new Date).getDate()) {
                err_msg = err_msg + "Effective Date cannot less than today date";
                is_error = true;
            }

            if (is_error) {
                messageBox("error", err_msg);
                return false;
            }

            if (loc_id == "" || loc_id == null) {
                loc_id = 0;
            }

            if (dept_id == "" || dept_id == null) {
                dept_id = 0;
            }


            var emp_id = [];

            var table = $('#tblempdetail').DataTable();

            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var _ischecked = table.cell(rowIdx, 1).nodes().to$().find('input').is(":checked");
                if (_ischecked) {

                    emp_id.push(table.cell(rowIdx, 2).nodes().to$().html());

                }
            });


            var mydata = {
                company_id: company_id,
                loc_id: loc_id,
                dept_id: dept_id,
                effective_dt: effective_dt,
                emp_: emp_id,
                created_by: login_emp_id,
                shift_id: shift_id,
            };

            $("#loader").show();
            if (confirm("Total " + emp_id.length + " application selected to Process. \nDo you want to process this?")) {

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiShift/Save_ShiftAllignment",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var res = response;
                        var statuscode = res.statusCode;
                        var msg = res.message;
                        $("#loader").hide();
                        if (statuscode != "0") {
                            messageBox("error", msg);
                            return false;
                        }
                        else {
                            alert(msg);
                            location.reload();
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
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);
                        return false;
                    }


                });


            }
            else {
                $("#loader").hide();
                return false;
            }
        });

    }, 2000);// end timeout


});

function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblempdetail').DataTable().cells().nodes();
    var currentPages = $('#tblempdetail').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblempdetail").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(allPages).find('.chkRow').prop('checked', false);
            $(currentPages).find('.chkRow').prop('checked', true);
        }
        else {
            $(allPages).find('.chkRow').prop('checked', false);
        }
    });

}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblempdetail").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function BindEmployeeDetail(company_id, location_id, dept_id) {
    var apiurl_ = "";

    if ((company_id == "0" || company_id > 0) && (location_id == "" || location_id == "0") && (dept_id == "" || dept_id == "0")) {
        //apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetAllEmployee";

        apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetallEmployee_before_today_dt/" + company_id
    }

    else if ((company_id == "0" || company_id > 0) && location_id > 0 && (dept_id == "" || dept_id == "0")) {
        apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetEmployeeByCL/" + company_id + "/" + location_id;
    }
    else if ((company_id == "0" || company_id > 0) && (location_id == "" || location_id == "0") && dept_id > 0) {
        apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetEmployeeByCD/" + company_id + "/" + dept_id;
    }
    //else if (company_id > 0 && location_id == "" && dept_id == "") {
    //    apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetAllEmployeeByCompany/" + company_id;
    //}
    else if ((company_id == "0" || company_id > 0) && location_id > 0 && dept_id > 0) {
        apiurl_ = localStorage.getItem("ApiUrl") + "apiEmployee/GetEmployeeByCLD/" + company_id + "/" + location_id + "/" + dept_id;
    }

    $.ajax({
        url: apiurl_,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $("#tblempdetail").DataTable({
                "bDestroy": true,
                "processing": true,
                "serverSide": false,
                "orderMulti": false,
                "scrollX": 200,
                "scrollY": 200,
                "filter": true,
                "data": res,
                "columnDefs": [
                    {
                        targets: 1,
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    {
                        "title": "<input type='checkbox' onchange='selectAll(this)' id='selectAll' />Select All", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" class="chkRow" />';
                        }
                    },
                    { "data": "employee_id", "name": "employee_id", "title": "Employee Id", "autoWidth": true, "visible": false },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee Name(Code)", "autoWidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });

            $(document).bind("change", ".chkRow", function () {
                if ($(this).is(':checked')) {
                    $(this).prop("checked", true);
                }
                else {
                    $(this).prop("checked", false);
                }
            });
        },
        error: function (err) {
            _GUID_New();
            $("#loader").hide();
            messageBox("err", err.responseText);
            return false;
        }
    });
}

