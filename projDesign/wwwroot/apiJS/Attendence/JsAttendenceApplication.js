
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

        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);

        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);

        // BindCompanyListForddl('ddlCompany', 0);

        $('#ddlCompany').bind("change", function () {

            $("#ddlEmployeeCode option").remove();
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);

        });

        $('#ddlEmployeeCode').bind("change", function () {
            GetData($(this).val());
        });


        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();
        //$("#txtInTime").prop('disabled', true);

        $('#btnGetDetail').bind('click', function () {
            $('#loader').show();
            var errormsg = '';
            var iserror = false;


            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtRegularizeDate = $("#txtRegularizeDate").val();
            //var Nin = $("#txtInTime").val().text();
            //var Nout = $("#txtOutTime").val().text();

            //validation part
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (txtRegularizeDate == null || txtRegularizeDate == '') {
                errormsg = "Please Select Regularize Date ...! ";
                iserror = true;
            }
            //if (Nin == null || Nin == '' || Nin == 0) {
            //    errormsg = "Please Enter New In Time ...! ";
            //    messageBox("error", errormsg);
            //    return false;
            //}
            //if (Nout == null || Nout == '' || Nout == 0) {
            //    errormsg = "Please Enter New OutTime ...! ";
            //    messageBox("error", errormsg);
            //    return false;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            GetInOrOutTimeByDate('lblInTime', txtRegularizeDate, ddlEmployeeCode);
            $('#loader').hide();
        });


        $('#btnreset').bind('click', function () {
            location.reload();
            //BindCompanyListForddl('ddlCompany', 0);
            //$("#ddlEmployeeCode option").remove();
            //$("#txtRegularizeDate").val('');
            //$("#lblInTime").empty();
            //$("#lblOutTime").empty();
            //$("#txtInTime").val('');
            //$("#txtOutTime").val('');
            //$("#txtRemarks").val('');
        });


        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txtRegularizeDate = $("#txtRegularizeDate").val();
            var lblInTime = '2000-01-01 ' + $("#lblInTime").text();
            var lblOutTime = '2000-01-01 ' + $("#lblOutTime").text();
            var txtInTime = $("#txtInTime").val();
            var txtOutTime = $("#txtOutTime").val();
            var txtRemarks = $("#txtRemarks").val();

            var dateregularise = new Date($('#txtRegularizeDate').val());
            var dayreg = dateregularise.getDate();
            var datein = new Date($('#txtInTime').val());
            var dateout = new Date($('#txtOutTime').val());
            var dayin = datein.getDate();
            var monthin = datein.getMonth();
            var yearin = datein.getFullYear();
            var dayout = dateout.getDate();
            //var monthout = dateout.getMonth();
            var datesub = dayout - dayin;



            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (txtRegularizeDate == null || txtRegularizeDate == '') {
                errormsg = "Please Select Regularize Date ...! ";
                iserror = true;
            }
            if (txtInTime == null || txtInTime == '') {
                errormsg = "Please Enter In Time ...! ";
                iserror = true;
            }
            if (txtOutTime == null || txtOutTime == '') {
                errormsg = "Please Enter Out Time ...! ";
                iserror = true;
            }
            if (dayin != dayreg) {
                //// debugger;
                errormsg = "Please Enter the same In date as Regularise Date Selected...! ";
                iserror = true;
            }
            if (datesub > 1 || datesub < 0) {
                // // debugger;
                errormsg = "Please Enter the same day or at max Day+1 ...! ";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var myData = {

                'from_date': txtRegularizeDate,
                'system_in_time': lblInTime,
                'system_out_time': lblOutTime,
                'manual_in_time': txtInTime,
                'manual_out_time': txtOutTime,
                'requester_remarks': txtRemarks,
                'is_deleted': is_deleted,
                'r_e_id': ddlEmployeeCode
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_AttendanceApplicationRequest';
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
                        //GetData(ddlEmployeeCode);
                        //BindCompanyListForddl('ddlCompany', 0);
                        //$("#ddlEmployeeCode option").remove();
                        //$("#txtRegularizeDate").val('');
                        //$("#lblInTime").empty();
                        //$("#lblOutTime").empty();
                        //$("#txtInTime").val('');
                        //$("#txtOutTime").val('');
                        //$("#txtRemarks").val('');
                        //messageBox("success", Msg);


                    }
                    else if (statuscode == "1" || statuscode == '2') {

                        messageBox("error", Msg);

                    }
                },
                error: function (request, status, error) {
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

        });


    }, 2000);// end timeout

});



function GetInOrOutTimeByDate(ControlId, RegularizeDate, EmployeeId) {

    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetInOrOutTimeByDate/' + RegularizeDate + '/' + EmployeeId;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            $.each(res, function (data, value) {

                $("#lblInTime").empty();
                var in_time = new Date(value.in_time);
                var in_time1 = GetTimeFromDate(in_time);
                $("#lblInTime").append(in_time1);

                $("#lblOutTime").empty();
                var out_time = new Date(value.out_time);
                var out_time1 = GetTimeFromDate(out_time);
                $("#lblOutTime").append(out_time1);

            });
            $('#loader').hide();

        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }

    });

}





function GetData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_AttendanceApplicationRequest/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblAttendanceApplication").DataTable({
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
                            targets: [1],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [2],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                        ,
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetTimeFromDate(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
                    { "data": "system_in_time", "name": "system_in_time", "autoWidth": true },
                    { "data": "system_out_time", "name": "system_out_time", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });
}