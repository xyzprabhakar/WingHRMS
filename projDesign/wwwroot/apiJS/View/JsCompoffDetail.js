
var company_id;
var login_emp_id;
var login_role_id;
var arr = new Array("1", "101", "105");

$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetEmployeeCompOffData(login_emp_id);
        
        if (!arr.includes(login_role_id)) {
            $("#ddl_year").prop("disabled", true);
        }

        $('#ddl_year').bind("change", function () {
            GetEmployeeCompOffData(login_emp_id);
        });


        $('#BtnDelete').bind('click', function () {
            debugger;
            $('#loader').show();
            var BulAppId = [];
            var table = $("#tblcompoff").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var requestss = {};
                    //var currentRow = $(this).closest("tr");

                    //var data = $('#tblcompoff').DataTable().row(currentRow).data();
                    //requestss.emp_id = data["employee_id"];
                    //requestss.emp_comp_id = data["leave_request_id"];
                    //requestss.comp_off_date = data["from_date"];
                    var no = $(this).val();
                    BulAppId.push(no);
                }
            });

            //cehck validation part


            var action_type = $('#ddlaction_type').val();
            if (action_type == null || action_type == '' || action_type == 0) {
                alert("Please select action type ...!");
                $('#loader').hide();
                return false;
            }

            if ($('#txtDelremarks').val() == null || $('#txtDelremarks').val() == '' || $('#txtDelremarks').val() == undefined) {
                alert("Please Enter Remarks...!");
                $('#txtDelremarks').focus();
                $('#loader').hide();
                return false;
            }



            if (confirm("Total " + BulAppId.length + " application selected to Process. \nDo you want to process this?")) {

                if (BulAppId == null || BulAppId == '') {
                    alert('There some problem please try after later...!');
                    $('#loader').hide();
                    return false;
                }
                else if (BulAppId.length <= 0) {
                    $('#loader').hide();
                    return false;
                }


                var myData = {
                    'compoff_id': BulAppId,
                    'a1_e_id': login_emp_id,
                    'remarks': $('#txtDelremarks').val(),
                    'is_deleted': action_type,
                };

                $('#loader').show();
                var apiurl = localStorage.getItem("ApiUrl") + '/apiMasters/DeleteCompoff';
                var Obj = JSON.stringify(myData);

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    type: "POST",
                    url: apiurl,
                    data: Obj,
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    headers: headerss,
                    success: function (res) {
                        debugger;
                        data = res;
                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        $('#loader').hide();
                        _GUID_New();
                        if (statuscode == "0") {
                            GetEmployeeCompOffData(login_emp_id);
                            messageBox("success", Msg);
                        }
                        else {
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

                        } catch (err) { }
                        messageBox("error", error);
                        $('#loader').hide();
                    }

                });
            }
            else {
                $('#loader').hide();
                return false;
            }
        });



    }, 2000);// end timeout

});


function GetEmployeeCompOffData(emp_id) {
    $("#loader").show();
    var year = $("#ddl_year option:selected").val();
    $.ajax({

        type: "GET",
        url: localStorage.getItem("ApiUrl") + '/apiMasters/GetEmployeeCompOffDataForAdd/' + emp_id + "/" + year,
        data: {},
        dataType: "json",
        contentType: 'application/json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }
            if (res.length > 0) {
                if (arr.includes(login_role_id)) {
                    $("#empselectiondiv").css("display", "block");
                }
            }
            else {
                $("#empselectiondiv").css("display", "none");
            }


            $("#tblcompoff").DataTable({
                "processing": true,//to show progress bar
                "serverSide": false,// to process server side
                "scrollX": 200,
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "aaData": res,
                "columnDefs": [
                    {
                        targets: [0],
                        orderable: false,
                        "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                ],
                "columns": [
                    {
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.sno + '" value="' + full.sno + '" />';
                        }
                    },
                    { "data": null, "title": "SNo", "autoWidth": true },
                    { "data": "sno", "name": "sno", "visible": false },
                    { "data": "e_id", "name": "e_id", "visible": false },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "title": "Department", "data": "dept_name", "name": "dept_name", "autoWidth": true },
                    { "data": "compoff_date", "name": "compoff_date", "title": "Compoff Date", "autoWidth": true },
                    {
                        "title": "Remarks", "autoWidth": true, "render": function () {
                            return '<input type="text"  id="txtremarks" placeholder="Remarks" class=form-control style="width:120px" />';
                        }
                    },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<div class="col-md-12 text-center btn-group"><button type="button" class="btn-primary btnSaveR"  id="btnSave">Save</button></div>';
                        }
                    }
                    //, {
                    //    "title": "Delete", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a  onclick="DeleteCompoff(' + full.sno + ' )" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(2)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });

            $("#loader").hide();

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

function selectAll() {

    var chkAll = $('#selectAll');

    var allPages = $('#tblcompoff').DataTable().cells().nodes();
    var currentPages = $('#tblcompoff').DataTable().rows({ page: 'current' }).nodes();
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblcompoff").find(".chkRow");
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
    var chkRows = $("#tblcompoff").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}


function DeleteCompoff(id) {
    debugger;
    $("#loader").show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/DeleteCompoff/' + id;
    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                debugger;
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                GetEmployeeCompOffData(login_emp_id);
                $("#loader").hide();
                if (statuscode == "0") {
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("info", msg);
                    return false;
                }
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}

$(document).on("click", ".btnSaveR", function () {

    var currentRow = $(this).closest("tr");

    var data = $('#tblcompoff').DataTable().row(currentRow).data();

    var compoffsno = data['sno'];
    var emp_id = data["e_id"];
    var compoffdate = data['compoff_date'];
    var txtremarks = $(this).parents('tr').children('td').children('input[id="txtremarks"]').val();

    if (txtremarks == "") {
        _GUID_New();
        messageBox("error", "Please enter Remarks");
        return false;
    }


    AddCompoffForRequest(emp_id, compoffsno, compoffdate, txtremarks);
});
function AddCompoffForRequest(emp_id, compoffsno, compoffdate, txtremarks) {


    var mydata = {
        emp_comp_id: compoffsno,
        comp_off_date: compoffdate,
        emp_id: emp_id,
        requester_remarks: txtremarks,
        created_by: login_emp_id
    }
    $('#loader').show();
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/AddCompoffForRequest",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (data) {
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                GetEmployeeCompOffData(login_emp_id);
                messageBox("success", Msg);
                return false;
            }

            else {
                messageBox("error", Msg);
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
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);
            $('#loader').hide();
        }


    });
}