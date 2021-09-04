$('#loader').show();

var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        $("#hdnid").val('');
        $('#btnupdate').hide();
        $('#btnsave').show();


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        BindDepartmentList('ddlDepartment', default_company, -1);


        $('#ddlcompany').bind('change', function () {
            if ($.fn.DataTable.isDataTable('#tblemployee')) {
                $('#tblemployee').DataTable().clear().draw();
            }
            BindDepartmentList('ddlDepartment', $(this).val(), -1);
        });

        $('#ddlDepartment').bind('change', function () {
            var comp = $('#ddlcompany').val();
            GetEmployeeListbyDepartment(comp, $(this).val());
        });

        $('#loader').hide();

    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetEmployeeListbyDepartment(companyid, dept_id) {

    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_EmployeeByCompAndDeptId/' + companyid + '/' + dept_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblemployee").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 0,
                            orderable: false,
                            "class": "text-center",
                            "sTitle": "Is Mobile Access <input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        },
                        {
                            targets: 1,
                            orderable: false,
                            "class": "text-center",
                            "sTitle": "Is Mobile Attendence Access <input type='checkbox' onchange='selectAll1(this)' id='selectAll1'></input>"
                        }
                    ],
                "columns": [
                    {

                        "render": function (data, type, full, meta) {
                            if (full.mobile_access == 1)
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked=true id="chk' + full.employee_id + '" value="' + full.employee_id + '" />';
                            else
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.employee_id + '" value="' + full.employee_id + '" />';

                        }
                    },
                    {

                        "render": function (data, type, full, meta) {
                            if (full.mobile_attendence_access == 1)
                                return '<input type="checkbox" onchange="selectRows1(this);" class="chkRow1" checked=true id="chk' + full.employee_id + '" value="' + full.employee_id + '" />';
                            else
                                return '<input type="checkbox" onchange="selectRows1(this);" class="chkRow1" id="chk' + full.employee_id + '" value="' + full.employee_id + '" />';

                        }
                    },
                    { "data": "emp_code", "title": "Employee Code", "name": "emp_code", "class": "text-center", "autoWidth": true },
                    { "data": "emp_name", "title": "Employee Name", "name": "emp_name", "class": "text-center", "autoWidth": true }
                ],
                'order': [[1, 'asc']],
                "select": 'multi',
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, "All"]],
            });

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}

$("#btnsave").bind("click", function () {
    $('#loader').show();
    var comp_id = $('#ddlcompany').val();

    var BulAppId = [];
    var BulAppId1 = [];
    var table = $("#tblemployee").dataTable();
    $("td:first-child input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            BulAppId.push(no);
        }
    });
    $("td:nth-child(2) input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            BulAppId1.push(no);
        }
    });

    //if (BulAppId == null || BulAppId == '' || BulAppId.length <= 0) {
    //    alert('There some problem please try after later...!');
    //    $('#loader').hide();
    //    return false;
    //}
    //if (BulAppId1 == null || BulAppId1 == '' || BulAppId1.length <= 0) {
    //    alert('There some problem please try after later...!');
    //    $('#loader').hide();
    //    return false;
    //}

    if (confirm("Total (" + BulAppId.length + " mobile access application) and (" + BulAppId1.length + " mobile attendence access application) selected to Process. \nDo you want to process this?"))
    {

        var myData = {
            'mobile_access_employee_ids': BulAppId,
            'mobile_attendence_access_employee_ids': BulAppId1,
            'company_id': comp_id,
        };

        var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_MobileApplicationAccessMaster';
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
                }
                catch (err) { }
                messageBox("error", error);
                $('#loader').hide();
            }

        });
    }
    else {
        $("#loader").hide();
        return false;
    }



});

function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblemployee').DataTable().cells().nodes();
    var currentPages = $('#tblemployee').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblemployee").find(".chkRow");
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
    var chkRows = $("#tblemployee").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function selectAll1() {

    var chkAll1 = $('#selectAll1');

    var allPages1 = $('#tblemployee').DataTable().cells().nodes();
    var currentPages1 = $('#tblemployee').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows1 = $("#tblemployee").find(".chkRow1");
    chkRows1.each(function () {
        if (chkAll1.is(':checked')) {
            $(allPages1).find('.chkRow1').prop('checked', false);
            $(currentPages1).find('.chkRow1').prop('checked', true);
        }
        else {
            $(allPages1).find('.chkRow1').prop('checked', false);
        }
    });

}

function selectRows1() {
    debugger;
    var chkAll1 = $("#selectAll1");
    chkAll1.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows1 = $("#tblemployee").find(".chkRow1");

    chkRows1.each(function () {
        if (!$(this).is(":checked")) {
            chkAll1.prop("checked", false);
            return;
        }
    });
}

$("#btnreset").bind("click", function () {
    location.reload();
});

