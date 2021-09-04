$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        $("#checkbtn").hide();
        //$('#btnreset').hide();
        $('#btnupdate').hide();

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/BankMaster';

        });
        $('#loader').hide();


        $("#btnupdate").bind("click", function () {
            // debugger;
            $('#loader').show();
            var bankname = $("#bankname").val();
            //var view = 0;
            var errormsg = '';
            var iserror = false;
            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_deleted = 0;
                }
                else {
                    is_deleted = 1;
                }
            }

            //validation part
            if (bankname == '' || bankname == '0' || bankname == null) {
                errormsg = "Please enter bankname !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'bank_name': bankname,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,

            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_BankMaster';//need to change this
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
                    $('#btnupdate').hide();
                    $("#checkbtn").hide();
                    $('#btnsave').show();
                    $('#loader').hide();
                    _GUID_New();
                    //GetData();
                    //messageBox("success", Msg);

                    if (statuscode == "0") {
                        $("#bankname").val('');
                        alert(Msg);
                        location.reload();
                        //GetData();
                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });
        });


        //save
        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var bankname = $("#bankname").val();
            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (bankname == '' || bankname == '0' || bankname == null) {
                errormsg = "Please enter bankname !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'bank_name': bankname,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_BankMaster';//need to change this
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
                        $("#bankname").val('');
                        //GetData();
                        alert("success", Msg);
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

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });


    }, 2000);// end timeout

});

//edit

function GetEditData(id) {
    $('#loader').show();
    if (id == "" || id == null) {
        messageBox("info", "invalid id pass");
        $('#loader').hide();
        return false;
    }
    $("#hdnid").val(id);
    var apiurl = localStorage.getItem("ApiUrl") + "apiPayroll/Get_Bankname/" + id;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            $("#bankname").val(data.bank_name);

            $('#btnupdate').show();
            $("#checkbtn").show();
            $('#btnsave').hide();
            $('#loader').hide();

        },
        Error: function (err) {
            $('#loader').hide();
        }
    });
}

//update

//get data

function GetData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Bankname/0'; //change link here

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $("#tbl_bankmaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [

                    ],
                "columns": [

                    { "data": null },
                    { "data": "bank_name", "name": "bank_name", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.bank_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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
            messageBox("error", error.responseText);
            console.log("error");
            $('#loader').hide();
        }
    });

}