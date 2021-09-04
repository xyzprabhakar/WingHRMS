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
        //BindCompanyList('ddlCompany', 0);
        $('#ddlCompany').bind("change", function () {

            var ddlCompany = $("#ddlCompany").val();

            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
        });

        $('#ddlEmployeeCode').bind("change", function () {
            GetData($(this).val());
        });


        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            $('#loader').show();
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);

            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
            $("#ddlEmployeeCode option").remove();
            $("#txtDate").val('');
            $("#txtInTime").val('');
            $("#txtOutTime").val('');
            $("#Reason").val('');
            $('#loader').hide();
        });




        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var txtDate = $("#txtDate").val();
            var txtInTime = $("#txtInTime").val();
            var txtOutTime = $("#txtOutTime").val();
            var Reason = $("#Reason").val();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddlCompany = $("#ddlCompany").val();


            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part

            if (txtOutTime == null || txtOutTime == '') {
                errormsg = "Please Enter Out Time...! ";
                iserror = true;
            }
            if (txtInTime == null || txtInTime == '') {
                errormsg = "Please Enter In Time...! ";
                iserror = true;
            }
            if (txtDate == null || txtDate == '') {
                errormsg = "Please Enter Date...! ";
                iserror = true;
            }
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (ddlCompany == null || ddlCompany == '' || ddlCompany == 0) {
                errormsg = "Please Select Company...! ";
                iserror = true;
            }



            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'from_date': txtDate,
                'manual_in_time': txtInTime,
                'manual_out_time': txtOutTime,
                'requester_remarks': Reason,
                'is_deleted': is_deleted,
                'r_e_id': ddlEmployeeCode,
                'company_id': ddlCompany
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_OutdoorApplicationRequest';
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
                        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);

                        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
                        $("#txtDate").val('');
                        $("#txtInTime").val('');
                        $("#txtOutTime").val('');
                        $("#Reason").val('');

                        messageBox("success", Msg);

                        GetData(ddlEmployeeCode);
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



function GetData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_OutdoorApplicationRequest/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            //// debugger;;
            $("#tblOutdoorApplication").DataTable({
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
                    ],

                "columns": [
                    { "data": null },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "manual_in_time", "name": "manual_in_time", "autoWidth": true },
                    { "data": "manual_out_time", "name": "manual_out_time", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true },
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
