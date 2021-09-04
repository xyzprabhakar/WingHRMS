$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;
var login_role_id;
var HaveDisplay;
var arr = new Array("1", "101");
$("document").ready(function () {

    default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    if (!arr.includes(login_role_id)) {
        $("#btnAddMannualLeave").hide();
    }

    //  HaveDisplay = ISDisplayMenu("Display Company List");

    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);


    $("#txtFromDate").val(GetDateFormatddMMyyyy(firstDay));
    $("#txtToDate").val(GetDateFormatddMMyyyy(lastDay));


    BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
    BindLocationListForddl('ddllocation', default_company, 0);
    BindDepartmentList('ddldept', default_company, 0);
    //BindAllLocationsFromLocatinMaster('ddllocation', 0, 0);
    //BindAllDepartmentFromDepartmentMaster('ddldept', 0, 0);




    //if (HaveDisplay == 0) {
    //    BindCompanyList('ddlcompany', default_company);
    //    $('#ddlcompany').prop("disabled", "disabled");
    //    BindLocationList('ddllocation', default_company, 0);
    //}
    //else {
    //    BindCompanyListAll('ddlcompany', 0);
    //    BindAllLocationsFromLocatinMaster('ddllocation', 0, 0);
    //}


    $('#loader').hide();

    $("#ddlcompany").bind("change", function () {
        if ($.fn.DataTable.isDataTable('#tbl_shift_assign_dtl')) {
            $('#tbl_shift_assign_dtl').DataTable().clear().draw();
        }

        BindLocationListForddl('ddllocation', $(this).val(), 0);
        BindDepartmentList('ddldept', $(this).val(), 0);

    });

    $("#ddllocation").bind("change", function () {
        if ($.fn.DataTable.isDataTable('#tbl_shift_assign_dtl')) {
            $('#tbl_shift_assign_dtl').DataTable().clear().draw();
        }
    });

    $("#ddldept").bind("change", function () {
        if ($.fn.DataTable.isDataTable('#tbl_shift_assign_dtl')) {
            $('#tbl_shift_assign_dtl').DataTable().clear().draw();
        }
    });

    $("#btnget_detail").bind("click", function () {

        if ($("#ddlcompany").val() == "" || $("#ddlcompany").val() == null) {
            messageBox("error", "Please select Company");
            return false;
        }

        if ($("#ddllocation").val() == "" || $("#ddllocation").val() == null) {
            messageBox("error", "Please select Location");
            return false;
        }

        if ($("#ddldept").val() == "" || $("#ddldept").val() == null) {
            messageBox("error", "Please select Department");
            return false;
        }

        if ($("#txtFromDate").val() == "" || $("#txtFromDate") == null) {
            messageBox("error", "Please select from date...!");
            return false;
        }

        if ($("#txtToDate").val() == "" || $("#txtToDate") == null) {
            messageBox("error", "Please select to date...!");
            return false;
        }
        if ($("#ddlEmpType").val() == "" || $("#ddlEmpType") == null) {
            messageBox("error", "Please select Employee Type...!");
            $("#ddlEmpType").focus();
            return false;
        }

        $("#loader").show();
        $('#tbl_shift_assign_dtl').DataTable({
            "processing": true,
            "serverSide": false,
            "bDestroy": true,
            "orderMulti": false,
            "filter": true,
            "scrollX": 200,
            dom: 'Bfrtip',
            buttons: [
                {
                    text: 'Export to Excel',
                    title: 'Shift Assignment Detail',
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [2, 3, 4, 5, 6, 7, 8, 9]
                    }
                },
            ],
            ajax: {
                url: localStorage.getItem("ApiUrl") + "/apiShift/Get_EmpShiftDetails/" + $("#txtFromDate").val().replaceAll("/", "-") + "/" + $("#txtToDate").val().replaceAll("/", "-") + "/" + $("#ddlcompany").val() + "/" + $("#ddllocation").val() + "/" + $("#ddldept").val() + "/" + $("#ddlEmpType").val(),
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                dataSrc: function (json) {
                    $("#loader").hide();
                    if (json.statusCode != undefined) {
                        messageBox("error", json.message);
                        return false;
                    }
                    return json;
                },
                error: function (err) {
                    $("#loader").hide();
                    messageBox("error", err.responseText);
                    return false;
                }
            },
            "columnDefs": [
                {
                    targets: [7],
                    render: function (data, type, row) {

                        var date = new Date(data);
                        return GetDateFormatddMMyyyy(date);
                    }
                },
                {
                    targets: [8],
                    render: function (data, type, row) {

                        var date = new Date(data);
                        return GetTimeFromDate(date);
                    }
                },
                {
                    targets: [9],
                    render: function (data, type, row) {

                        var date = new Date(data);
                        return GetTimeFromDate(date);
                    }
                },
            ],
            columns: [
                { "data": null, "title": "S.No.", "autoWidth": true },
                { "data": "emp_id", "visible": false },
                { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
                { "data": "emp_name", "title": "Employee Name", "autoWidth": true },
                { "data": "location_name", "title": "Location", "autoWidth": true },
                { "data": "department_name", "title": "Department", "autoWidth": true },
                { "data": "shift_name", "title": "Shift Name", "autowidth": true },
                { "data": "effective_Dt", "title": "Applicable Date", "autoWidth": true },
                { "data": "shift_in_time", "title": "Shift In Time", "autoWidth": true },
                { "data": "shift_out_time", "title": "Shift Out Time", "autoWidth": true },
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                return nRow;
            },
            "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
        });




    });

});



function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}


