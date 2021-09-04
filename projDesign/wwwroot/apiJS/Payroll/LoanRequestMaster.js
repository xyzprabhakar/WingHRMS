$('#loader').show();

var emp_role_id;
var company_idd;
var emp_idd;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', emp_idd, company_idd);




        BindGradeMaster('ddlgrade', 0);

        GetData();
        //BindEmployementTypeForddl('ddlempstatus', 0);

        $("#btnupdate").hide();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
            //window.location.href = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMaster/0';
            // GetData();
        });

        $('#btnsave').bind("click", function () {
            //  // debugger;
            $('#loader').show();
            var errormsg = '';
            var iserror = false;

            var company_idd = $('#ddlcompany').val();


            if (company_idd == '' || company_idd == "0" || company_idd == null) {
                errormsg = errormsg + 'Please Select Company !! <br />';
                iserror = true;
            }

            if ($('#ddlgrade').val() == '' || $('#ddlgrade').val() == "0") {
                errormsg = errormsg + 'Please Select Grade !! <br />';
                iserror = true;
            }

            if ($('#ddlloantype').val() == '' || $('#ddlloantype').val() == "0") {
                errormsg = errormsg + 'Please Select Loan Type !! <br />';
                iserror = true;
            }

            if ($('#txtloanamt').val() == '') {
                errormsg = errormsg + 'Please Enter Loan Amount !! <br />';
                iserror = true;
            }

            if ($('#txtmaxtenure').val() == '') {
                errormsg = errormsg + 'Please Enter maximum Tenure !! <br />';
                iserror = true;
            }

            if ($('#txtmin_topup_duration').val() == '') {
                errormsg = errormsg + 'Please Enter Minimum Top up Duration !! <br />';
                iserror = true;
            }
            if ($('#txt_roi').val() == '') {
                errormsg = errormsg + 'Please Enter Rate of Interest !! <br />';
                iserror = true;
            }
            if ($('#txt_onsalary').val() == '') {
                errormsg = errormsg + 'Please Enter on Salary !! <br />';
                iserror = true;
            }
            //if ($('#ddlempstatus').val() == "0" || $('#ddlempstatus').val() == '') {
            //    errormsg = errormsg + 'Please Select Employee Type<br/>';
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var mydata = {
                //loan_type: $('#ddlloantype option:selected').text(),
                loan_type: $('#ddlloantype option:selected').val(),
                loan_amount: $('#txtloanamt').val(),
                // on_grade: $('#txt_ongrade').val(),
                on_salary: $('#txt_onsalary').val(),
                max_tenure: $('#txtmaxtenure').val(),
                min_top_up_duration: $('#txtmin_topup_duration').val(),
                rate_of_interest: $('#txt_roi').val(),
                companyid: company_idd,
                grade_id: $('#ddlgrade').val(),
                created_by: login_empid
            }
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiPayroll/Save_LoanRequestMaster',
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(mydata),
                dataType: "json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {

                        //  messageBox("success", Msg);
                        alert(Msg);
                        location.reload();
                        // GetData();
                        //  window.location.href = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMaster/0';
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

                    } catch (err) { }
                    messageBox("error", error);

                }

            });


        });


        $('#btnupdate').bind("click", function () {
            // // debugger;
            $('#loader').show();
            var errormsg = '';
            var iserror = false;


            var company_idd = $('#ddlcompany').val();


            if (company_idd == null || company_idd == '' || company_idd == "0") {
                errormsg = errormsg + 'Please Select Company !! <br />';
                iserror = true;
            }

            if ($('#ddlgrade').val() == '' || $('#ddlgrade').val() == "0") {
                errormsg = errormsg + 'Please Select Grade !! <br />';
                iserror = true;
            }

            if ($('#ddlloantype').val() == '' || $('#ddlloantype').val() == "0") {
                errormsg = errormsg + 'Please Select Loan Type !! <br />';
                iserror = true;
            }

            if ($('#txtloanamt').val() == '') {
                errormsg = errormsg + 'Please Enter Loan Amount !! <br />';
                iserror = true;
            }

            if ($('#txtmaxtenure').val() == '') {
                errormsg = errormsg + 'Please Enter maximum Tenure !! <br />';
                iserror = true;
            }

            if ($('#txtmin_topup_duration').val() == '') {
                errormsg = errormsg + 'Please Enter Minimum Top up Duration !! <br />';
                iserror = true;
            }
            if ($('#txt_roi').val() == '') {
                errormsg = errormsg + 'Please Enter Rate of Interest !! <br />';
                iserror = true;
            }

            if ($('#txt_onsalary').val() == '') {
                errormsg = errormsg + 'Please Enter on Salary !! <br />';
                iserror = true;
            }

            //if ($('#ddlempstatus').val() == "0" || $('#ddlempstatus').val() == '') {
            //    errormsg = errormsg + 'Please Select Employee Type<br/>';
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var mydata = {
                sno: $("#hdnsnoo").val(),
                loan_type: $('#ddlloantype option:selected').val(),
                loan_amount: $('#txtloanamt').val(),
                on_salary: $('#txt_onsalary').val(),
                max_tenure: $('#txtmaxtenure').val(),
                min_top_up_duration: $('#txtmin_topup_duration').val(),
                rate_of_interest: $('#txt_roi').val(),
                companyid: company_idd,
                grade_id: $('#ddlgrade').val(),
                last_modified_by: login_empid
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiPayroll/Edit_LoanRequestMaster',
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(mydata),
                dataType: "json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                        // GetData();
                        // messageBox("success", Msg);
                        // window.location.href = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMaster/0';
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

                    } catch (err) { }
                    messageBox("error", error);

                }

            });


        });



    }, 2000);// end timeout

});




function GetData() {

    // // debugger;
    var apiurll = "";
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0) {
    //    apiurll = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMasterByCompID/0/' + company_idd;
    //}
    //else {
    apiurll = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMaster/0';
    //}

    $('#loader').show();

    $.ajax({
        url: apiurll,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $('#loader').hide();

            if (response.statusCode != undefined) {
                messageBox("error", response.message);
                return false;
            }

            $("#tblloanrequestmaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {
                        //        return data == '1' ? 'Temporary' : data == '2' ? 'Probation' : data == '3' ? 'Confirmed' : data == '4' ? 'Contract' : data == '10' ? 'Notice' :  data=='99'?'FNF':data == '100' ?'Terminate':''
                        //    }
                        //},
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? 'Loan' : 'Advance'
                            }
                        },
                        {
                            targets: [9],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(row.last_modified_date);
                                var modified_date = GetDateFormatddMMyyyy(date);

                                return new Date(row.last_modified_date) < new Date(row.created_dt) ? '-' : modified_date;
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "autoWidth": true },
                    //{ "data": "em_status", "name": "em_status", "autoWidth": true},
                    { "data": "loan_type", "name": "loan_type", "autoWidth": true },
                    { "data": "loan_amount", "name": "loan_amount", "autoWidth": true },
                    { "data": "rate_of_interest", "name": "rate_of_interest", "autoWidth": true },
                    { "data": "on_salary", "name": "on_salary", "autoWidth": true },
                    { "data": "max_tenure", "name": "max_tenure", "autoWidth": true },
                    { "data": "min_top_up_duration", "name": "min_top_up_duration", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "autoWidth": true },
                    //{ "data": "last_modified_date", "name": "last_modified_date", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.sno + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                //"lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}


function GetEditData(id) {
    // debugger;
    reset_all();
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurll = "";
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0) {
    //    apiurll = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMasterByCompID/' + id + "/" + company_idd;
    //}
    //else {
    apiurll = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_LoanRequestMaster/' + id;
    // }

    $.ajax({
        type: "GET",
        url: apiurll,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            // data = res;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                BindAllEmp_Company('ddlcompany', emp_idd, company_idd);

                BindGradeMaster('ddlgrade', 0);
                return false;
            }


            BindAllEmp_Company('ddlcompany', emp_idd, company_idd);

            BindGradeMaster('ddlgrade', res[0].grade_id);
            //BindEmployementTypeForddl('ddlempstatus', res[0].em_status);
            //if (res[0].loan_type == 1) {
            //    $("#ddlloantype").val('Advance');
            //}
            //else {
            //    $("#ddlloantype option:selected").val()=="Loan";
            //}
            $("#ddlloantype").val(res[0].loan_type);
            //$("#ddlloantype").val(res.loan_type);
            $("#txtloanamt").val(res[0].loan_amount);
            $("#txtmaxtenure").val(res[0].max_tenure);
            $("#txtmin_topup_duration").val(res[0].min_top_up_duration);
            $("#txt_ongrade").val(res[0].on_grade);
            $("#txt_onsalary").val(res[0].on_salary);
            $("#txt_roi").val(res[0].rate_of_interest);

            $("#hdnsnoo").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}

function reset_all() {
    $("#ddlloantype").val('');
    //$("#ddlloantype").val(res.loan_type);
    $("#txtloanamt").val('');
    $("#txtmaxtenure").val('');
    $("#txtmin_topup_duration").val('');
    $("#txt_ongrade").val('');
    $("#txt_onsalary").val('');
    $("#txt_roi").val('');

    $("#hdnsnoo").val('');
    $('#btnupdate').hide();
    $('#btnsave').show();

}


