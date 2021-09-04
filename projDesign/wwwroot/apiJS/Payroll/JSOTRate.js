$('#loader').show();

var emp_role_id;
var login_company_id;
var login_empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlCompany', login_empid, login_company_id);
        BindOnlyProbation_Confirmed_emp('ddlemp', login_company_id, 0);

        //BindEmpList('ddlemp', 0);
        BindGradeList('ddlgrade', 0);
        $('#ddlgrade').bind("change", function () {
            BindSalaryGroupList('ddlsalgroup', 0, $(this).val());
        });

        $('#btnreset').show();
        $('#btnsave').show();
        $('#btnupdate').hide();
        GetData();

        $('#loader').hide();

        $('#txtamt').keypress(function (e) {
            var key = (e.keyCode ? e.keyCode : e.which);
            return ((key >= 48 && key <= 57));
        });
        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlemp option").remove();
            BindOnlyProbation_Confirmed_emp('ddlemp', $(this).val(), 0);
            $('#loader').hide();
        });

        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/OTRateMaster';

        });
        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var company = $("#ddlCompany").val();
            var empId = $("#ddlemp").val();
            var grade = $('#ddlgrade').val();
            var ot_amt = $('#txtamt').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;


            //validation part
            if (company == '' || company == '0') {
                errormsg = "Please select Company !! <br/>";
                iserror = true;
            }
            if (empId == '' || empId == '-1') {
                errormsg = "Please select Employee !! <br/>";
                iserror = true;
            }
            if (empId == '0') {

                if (grade == '' || grade == '0') {
                    errormsg = errormsg + 'Please select Grade !! <br />';
                    iserror = true;
                }
            }

            if (ot_amt == '' || ot_amt == null) {
                errormsg = errormsg + 'Please enter Over time rate !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var myData = {

                'companyid': company,
                'emp_id': empId,
                'grade_id': grade,
                'ot_amt': ot_amt,
                'is_active': 1,
                'created_by': login_empid,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_OTRateMaster';
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
                        //$("#ddlCompany").val('0');
                        //$("#ddlemp").val('-1');
                        //$('#ddlgrade').val('0');
                        //$('#txtamt').val('');
                        //$('#btnupdate').hide();

                        //GetData();
                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
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
            // debugger;
            var company = $("#ddlCompany").val();
            var empId = $("#ddlemp").val();
            var grade = $('#ddlgrade').val();
            var ot_amt = $('#txtamt').val();
            var ot_id = $("#hdnid").val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (company == '' || company == '0') {
                errormsg = "Please select Company !! <br/>";
                iserror = true;
            }
            if (empId == '' || empId == '-1') {
                errormsg = "Please select Employee !! <br/>";
                iserror = true;
            }
            if (empId == '0') {
                if (grade == '' || grade == '0') {
                    errormsg = errormsg + 'Please select Grade !! <br />';
                    iserror = true;
                }
            }


            if (ot_amt == '' || ot_amt == null) {
                errormsg = errormsg + 'Please enter Over time rate !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var myData = {

                'ot_id': ot_id,
                'companyid': company,
                'emp_id': empId,
                'grade_id': grade,
                'ot_amt': ot_amt,
                'is_active': 1,
                'created_by': login_empid,
            };

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_OTRateMaster';
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
                        // $("#ddlCompany").val('0');
                        //$("#ddlemp").val('-1');
                        // $('#ddlgrade').val('0');
                        //$('#txtamt').val('');
                        //$('#btnupdate').hide();
                        //$('#btnsave').show();

                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
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


//function BindEmpList(ControlId, CompanyId, SelectedVal) {
//    $('#loader').show();

//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: apiurl + "apiPayroll/Get_EmployeeDetailsByComp/" + CompanyId,
//        data: {},
//        contentType: "application/json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        dataType: "json",
//        success: function (response) {
//            var res = response;

//            $(ControlId).empty().append('<option selected="selected" value="0">All</option>');
//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
//            })


//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != 'undefined' && SelectedVal != null) {
//                $(ControlId).val(SelectedVal);
//                $(ControlId).trigger("select2:updated");
//                $(ControlId).select2();
//            }
//            $(ControlId).trigger("select2:updated");
//            $(ControlId).select2();
//            $('#loader').hide();
//        },
//        error: function (err) {
//            $('#loader').hide();
//            alert(err.responseText);
//        }
//    });
//}
function BindGradeList(ControlId, SelectedVal) {
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

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.sno).html(value.gradename));
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

    apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_OTRateMaster/0';

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
            $("#tblOT").DataTable({
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
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": null, "autoWidth": true },
                    //{ "data": "ot_id", "name": "ot_id", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "employee_name_code", "name": "employee_name_code", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "autoWidth": true },
                    { "data": "ot_amt", "name": "ot_amt", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.ot_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });

}

function GetEditData(ot_id) {
    $('#loader').show();
    if (ot_id == null || ot_id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    // debugger;
    $("#hdnid").val(ot_id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_OTRateMaster/' + ot_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            BindAllEmp_Company('ddlCompany', login_empid, data.company_id);

            if (data.emp_id == null) {

                BindOnlyProbation_Confirmed_emp('ddlemp', data.company_id, 0);
            }
            else {
                BindOnlyProbation_Confirmed_emp('ddlemp', data.company_id, data.emp_id);
            }

            if (data.grade_id == null) {
                BindGradeList('ddlgrade', 0)
            }
            else {
                $('#ddlgrade').val(data.grade_id);
            }

            $('#txtamt').val(data.ot_amt);

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            messageBox(err.responseText);
        }
    });
}

//-------update OT Rate data


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
