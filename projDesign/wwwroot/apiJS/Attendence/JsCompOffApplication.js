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



        $('#ddlCompany').bind("change", function () {

            $("#ddlEmployeeCode option").remove();
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);

        });

        $('#ddlEmployeeCode').bind("change", function () {
            GetEmployeeCompOffData('ddlCompOffDate', $(this).val(), 0);
            GetData($(this).val());
        });


        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
            //$('#loader').show();
            //BindCompanyListForddl('ddlCompany', 0);
            //$("#ddlEmployeeCode option").remove();
            //$("#ddlCompOffDate option").remove();
            //$("#txtCompOffDate").val('');
            //$("#txtRemarks").val('');
            //$('#loader').hide();
        });


        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var ddlCompOffDate = $("#ddlCompOffDate").val();
            var txtCompOffDate = $("#txtCompOffDate").val();
            var txtRemarks = $("#txtRemarks").val();

            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (ddlCompOffDate == null || ddlCompOffDate == '' || ddlCompOffDate == 0) {
                errormsg = "Please Select Against Date...! ";
                iserror = true;
            }
            if (txtCompOffDate == null || txtCompOffDate == '') {
                errormsg = "Please Select CompOff Date...! ";
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'r_e_id': ddlEmployeeCode,
                'compoff_against_date': ddlCompOffDate,
                'compoff_date': txtCompOffDate,
                'requester_remarks': txtRemarks,
                'compoff_request_qty': 1,
                'is_deleted': is_deleted,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_CompOffApplicationRequest';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            //// debugger;;
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

                        //BindCompanyListForddl('ddlCompany', 0);
                        //$("#ddlEmployeeCode option").remove();
                        //$("#ddlCompOffDate option").remove();
                        //$("#txtCompOffDate").val('');
                        //$("#txtRemarks").val('');
                        //messageBox("success", Msg);

                        //GetData(ddlEmployeeCode);
                        alert(Msg);
                        location.reload();



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

function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}


function GetEmployeeCompOffData(ControlId, EmployeeId, SelectedVal) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetEmployeeCompOffData/' + EmployeeId;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                var date = new Date(value.compoff_date);
                $(ControlId).append($("<option></option>").val(GetDateFormatddMMyyyy(date)).html(GetDateFormatddMMyyyy(date)));
            });

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

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

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompOffApplicationRequest/' + id;

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
            $("#tblCompOffApplication").DataTable({
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
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "status", "name": "status", "autoWidth": true },
                    { "data": "requester_remarks", "name": "requester_remarks", "autoWidth": true }
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