$('#loader').show();

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        BindFinancialList('ddlfianancial', 0);
        GetData();

        $('#btnupdate').hide();
        $('#btnsave').show();

        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
                $("#txttodate").datepicker('setDate', null);
                $("#txttodate").datepicker({
                    dateFormat: 'mm/dd/yy',
                    minDate: fromselected,
                    onSelect: function (toselected, evnt) {

                        if (Date.parse(fromselected) > Date.parse(toselected)) {
                            messageBox('error', 'To Date must be greater than from date !!');
                            //$('#txtfromdate').val('');
                            $('#txttodate').val('');
                        }
                    }
                });
            }
            //changeMonth: true,
            //changeYear: true
        });

        $('#loader').hide();

    }, 2000);// end timeout

});

$('#btnreset').bind('click', function () {
    $("#ddlfianancial").val('0');
    $('#txtquarter').val('');
    $('#txtfromdate').val('');
    $('#txttodate').val('');
    $('#btnupdate').hide();
    $('#btnsave').show();
});

$('#btnsave').bind("click", function () {
    // debugger;
    $('#loader').show();

    var financial_year = $("#ddlfianancial").val();
    var quarter = $('#txtquarter').val();
    var from_date = $('#txtfromdate').val();
    var to_date = $('#txttodate').val();

    var errormsg = '';
    var iserror = false;

    //validation part
    if (financial_year == null || financial_year == '0') {
        errormsg = "Please select Financial Year !! <br/>";
        iserror = true;
    }
    if (quarter == '' || quarter == '') {
        errormsg = errormsg + 'Please enter Quarter !! <br />';
        iserror = true;
    }
    if (from_date == '' || from_date == null) {
        errormsg = errormsg + 'Please select Start Date !! <br />';
        iserror = true;
    }
    if (to_date == '' || to_date == null) {
        errormsg = errormsg + 'Please select End Date !! <br />';
        iserror = true;
    }
    if (iserror) {
        messageBox("error", errormsg);
        //  messageBox("info", "eror give");
        $('#loader').hide();
        return false;
    }

    var myData = {

        'fiscal_year_id': financial_year,
        'quarter_name': quarter,
        'start_date': from_date,
        'enddate': to_date
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_QuaterMaster';
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
                $("#ddlfianancial").val('0');
                $('#txtquarter').val('');
                $('#txtfromdate').val('');
                $('#txttodate').val('');
                GetData();
                messageBox("success", Msg);
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err, exception) {
            alert('error found');
            $('#loader').hide();
        }
    });
});

function GetData() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_QuarterMasterData/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblquartermstr").DataTable({
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
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },

                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "financialyear", "name": "financialyear", "autoWidth": true },
                    { "data": "quarter", "name": "quarter", "autoWidth": true },
                    { "data": "startdate", "name": "startdate", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.sno + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            //alert(error);
            console.log("error");
        }
    });

}

function GetEditData(id) {
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_QuarterMasterData/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;


            $("#ddlfianancial").val(data.fiscal_year_id);
            $('#txtquarter').val(data.quarter_name);
            $('#txtfromdate').val(GetDateFormatddMMyyyy(new Date(data.start_date)));
            $('#txttodate').val(GetDateFormatddMMyyyy(new Date(data.enddate)));

            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
        }
    });
}

//-------update city data
$("#btnupdate").bind("click", function () {
    $('#loader').show();
    var financial_year = $("#ddlfianancial").val();
    var quarter = $('#txtquarter').val();
    var from_date = $('#txtfromdate').val();
    var to_date = $('#txttodate').val();
    var quarter_id = $("#hdnid").val();
    //var view = 0;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (financial_year == null || financial_year == '0') {
        errormsg = "Please Select Financial Year !! <br/>";
        iserror = true;
    }
    if (quarter == '' || quarter == null) {
        errormsg = errormsg + 'Please enter Quarter !! <br />';
        iserror = true;
    }
    if (from_date == '' || from_date == null) {
        errormsg = errormsg + 'Please select From Date !! <br />';
        iserror = true;
    }
    if (to_date == '' || to_date == null) {
        errormsg = errormsg + 'Please select To Date !! <br />';
        iserror = true;
    }

    if (iserror) {
        // messageBox("success", "successfully save data");
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {
        'quarter_id': quarter_id,
        'fiscal_year_id': financial_year,
        'quarter_name': quarter,
        'start_date': from_date,
        'enddate': to_date
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_QuarterMaster';
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
                $("#hdnid").val('');
                $("#ddlfianancial").val('0');
                $('#txtquarter').val('');
                $('#txtfromdate').val('');
                $('#txttodate').val('');
                $('#btnupdate').hide();
                $('#btnsave').show();
                GetData();
                messageBox("success", Msg);
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err, exception) {
            alert('error found');
            $('#loader').hide();
        }
    });
});
