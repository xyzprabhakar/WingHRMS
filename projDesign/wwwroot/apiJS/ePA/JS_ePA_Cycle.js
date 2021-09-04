
var default_company;
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        GetData(default_company);




        // debugger;
        // GetData(default_company);


        $('#ddlcompany').bind("change", function () {
            $('#loader').show();

            $("#cycle_type").val('');
            $('#txtfromdate').val('');
            $('#txt_number_of_day').val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
            $('#loader').hide();
            if ($(this).val() == "0") {
                if ($.fn.DataTable.isDataTable('#tblfinancial')) {
                    $('#tblfinancial').DataTable().clear().draw();
                }
            }
            else {
                GetData($(this).val());
            }
        });


        $('#btnupdate').hide();
        $('#btnsave').show();



        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {

            }
        });

        $('#btnreset').bind('click', function () {
            $('#loader').show();
            BindAllEmp_Company('ddlcompany', login_emp, default_company);
            GetData(default_company);
            $('#txtfromdate').val('');
            $('#txt_number_of_day').val('');
            $("#cycle_type").val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
            $('#loader').hide();
        });
        $('#btnsave').bind("click", function () {
            // debugger;

            var company_id = $("#ddlcompany").val();
            var cycle_type = $("#cycle_type").val();
            var from_date = $('#txtfromdate').val();
            var txt_number_of_day = $('#txt_number_of_day').val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (company_id == null || company_id == "" || company_id == "0") {
                errormsg = "Please Select select company <br/>";
                iserror = true;
            }
            if (cycle_type == null || cycle_type == '') {
                errormsg = "Please Select Cycle Type <br/>";
                iserror = true;
            }
            if (from_date == '' || from_date == null) {
                errormsg = errormsg + 'Please select From Date !! <br />';
                iserror = true;
            }
            if (txt_number_of_day == '' || txt_number_of_day == null) {
                errormsg = errormsg + 'Please Enter Number Of Day !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var myData = {
                'cycle_type': cycle_type,
                'from_date': from_date,
                'number_of_day': txt_number_of_day,
                'company_id': company_id,
                'created_by': login_emp,
            };
            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_Epa_Cycle_Master';
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
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#txtFinancialYearName").val('');
                        $("#cycle_type").val('');
                        $('#txtfromdate').val('');
                        $("#txt_number_of_day").val('');
                        GetData(company_id);
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                    return false;
                }
            });
        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            var company_id = $("#ddlcompany").val();
            var cycle_type = $("#cycle_type").val();
            var from_date = $('#txtfromdate').val();
            var txt_number_of_day = $('#txt_number_of_day').val();
            var cycle_id = $("#hdnid").val();

            //var view = 0;
            var errormsg = '';
            var iserror = false;
            if (company_id == null || company_id == "" || company_id == "0") {
                errormsg = "Please Select company";
                iserror = true;

            }

            if (cycle_type == null || cycle_type == '') {
                errormsg = "Please Select Cycle Type <br/>";
                iserror = true;
            }
            if (from_date == '' || from_date == null) {
                errormsg = errormsg + 'Please select From Date !! <br />';
                iserror = true;
            }
            if (txt_number_of_day == '' || txt_number_of_day == null) {
                errormsg = errormsg + 'Please Enter Number Of Day !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {
                'cycle_id': cycle_id,
                'cycle_type': cycle_type,
                'from_date': from_date,
                'number_of_day': txt_number_of_day,
                'company_id': company_id,
                'created_by': login_emp,

            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_Epa_Cycle_Master';
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
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#hdnid").val('');
                        $("#cycle_type").val('');
                        $('#txtfromdate').val('');
                        $('#txt_number_of_day').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        GetData(company_id);
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                    return false;
                }
            });
        });


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData(company_id) {
    // debugger;
    if ($.fn.DataTable.isDataTable('#tblfinancial')) {
        $('#tblfinancial').DataTable().clear().draw();
    }
    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_Epa_Cycle_Master/0/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();

                return false;
            }
            $("#tblfinancial").DataTable({
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
                            targets: [2],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [1],
                            render: function (data, type, row) {
                                return data == 0 ? "Monthly" : data == 1 ? "Quaterly" : data == 2 ? "Half Yearly" : data == 3 ? "Yearly" : "";
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    { "data": "cycle_type", "name": "cycle_type", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "autoWidth": true },
                    { "data": "number_of_day", "name": "number_of_day", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.cycle_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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
            $('#loader').hide();
            messageBox("error", error.responseText);
            return false;
            //alert(error);
            // console.log("error");
        }
    });

}

function GetEditData(id) {

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    $('#loader').show();


    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_Epa_Cycle_Master/' + id + '/' + default_company;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();

                return false;
            }
            $("#cycle_type").val(data.cycle_type);
            $('#txtfromdate').val(GetDateFormatMMddyyyy(new Date(data.from_date)));
            $('#txt_number_of_day').val(data.number_of_day);

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


//-------update city data
