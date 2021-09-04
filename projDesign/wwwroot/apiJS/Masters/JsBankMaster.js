$("#loader").show();
var login_emp_id;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

        $("#btnupdate").hide();
        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {

            var is_error = false;
            var error_msg = "";
            var bankname = $("#txtbankname").val();
            var statuss = $("input[name='chkstatus']:checked").val();

            if (bankname == "" || bankname == null) {
                is_error = true;
                error_msg = error_msg + "Please enter bank name</br>";
            }
            if (statuss == "" || statuss == null) {
                is_error = true;
                error_msg = error_msg + "Please select status";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            $("#loader").show();

            var mydata = {
                bank_name: bankname,
                bank_status: statuss,
                created_by: login_emp_id
            }


            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/Save_BankMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    var res = response;
                    var statuscode = res.statusCode;
                    var msg = res.message;

                    if (statuscode == "0") {
                        $("#txtbankname").val('');
                        $("input[name='chkstatus']").prop("checked", false);
                        GetData();
                        messageBox("success", msg);
                    }
                    else if (statuscode != "0") {
                        messageBox("error", msg);
                    }

                    $("#loader").hide();
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });


        $("#btnupdate").bind("click", function () {

            var is_error = false;
            var error_msg = "";
            var bankname = $("#txtbankname").val();
            var statuss = $("input[name='chkstatus']:checked").val();

            if (bankname == "" || bankname == null) {
                is_error = true;
                error_msg = error_msg + "Please enter bank name</br>";
            }
            if (statuss == "" || statuss == null) {
                is_error = true;
                error_msg = error_msg + "Please select status";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            $("#loader").show();


            var mydata = {
                bank_id: $("#hdnbank_id").val(),
                bank_name: bankname,
                bank_status: statuss,
                created_by: login_emp_id
            }



            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/Update_BankMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    var res = response;
                    var statuscode = res.statusCode;
                    var msg = res.message;

                    if (statuscode == "0") {
                        GetData();
                        $("#btnsave").show();
                        $("#btnupdate").hide();
                        $("#hdnbank_id").val('');
                        $("#txtbankname").val('');
                        $("input[name='chkstatus']").prop("checked", false);

                        messageBox("success", msg);
                    }
                    else if (statuscode != "0") {
                        messageBox("error", msg);
                    }

                    $("#loader").hide();
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });


        $('#btnreset').bind('click', function () {
            location.reload();
        });

    }, 2000);// end timeout

});


function GetData() {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_BankMaster/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tbl_bankmaster").DataTable({
                "processing": true,//to show progress bar
                "serverSide": false,//to process server side
                "orderMulti": false,// to display multiple column at once
                "bDestroy": true,
                "scrollX": 200,
                "filter": true,//to enable search box
                "aaData": res,
                "columnDefs": [

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

                            var modified_date = new Date(data);
                            return new Date(row.modified_dt) >= new Date(row.created_dt) ? GetDateFormatddMMyyyy(modified_date) : "-";
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "bank_name", "name": "bank_name", "title": "Bank", "autoWidth": true },
                    { "data": "bank_status", "name": "bank_status", "title": "Status", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.bank_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }

    });
}


function GetEditData(idd) {
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_BankMaster/" + idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#txtbankname").val(res.bank_name);
            $('input[type=radio][name=chkstatus][value=' + res.bank_status + ']').prop('checked', true);



            $("#hdnbank_id").val(res.bank_id);
            $("#btnupdate").show();
            $("#btnsave").hide();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}





