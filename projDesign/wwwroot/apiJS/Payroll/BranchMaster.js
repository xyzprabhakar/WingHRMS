$('#loader').show();

var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        $('#btnupdate').hide();
        $("#checkbtn").hide();
        BindBankList('bankname', 0);
        GetData();

        $('#loader').hide();

        $('#btnreset').bind('click', function () {
            window.location.href = '/Payroll/BranchMaster';

        });

        $("#btnupdate").bind("click", function () {
            $('#loader').show();
            // debugger;
            var bankname = $("#bankname").val();
            var branchname = $('#branchname').val();
            var branchloc = $('#branchloc').val();
            var ifsccode = $('#ifsccode').val();
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
                errormsg = errormsg + 'Please select Bank !! <br />';
                iserror = true;
            }
            if (branchname == '' || branchname == '0' || branchname == null) {
                errormsg = errormsg + 'Please enter Branch Name !! <br />';
                iserror = true;
            }
            if (branchloc == '' || branchloc == '0' | branchloc == null) {
                errormsg = errormsg + 'Please enter Branch Location !! <br />';
                iserror = true;
            }
            if (ifsccode == '' || ifsccode == null) {
                errormsg = errormsg + 'Please enter ifsc code !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'bank_id': bankname,
                'branch_name': branchname,
                'loc': branchloc,
                'ifsc_code': ifsccode,
                'is_deleted': is_deleted,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Update_BranchMaster';//need to change this
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
                        //$("#bankname").val('');
                        //$('#branchname').val('');
                        //$('#branchloc').val('');
                        //$('#ifsccode').val('');
                        //$('#btnupdate').hide();
                        //$("#checkbtn").hide();
                        //$('#btnsave').show();
                        //GetData();
                        //messageBox("success", Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (err, exception) {
                    $('#loader').hide();
                    _GUID_New();
                    alert('error found');
                }
            });

        });

        $('#btnsave').bind("click", function () {
            // debugger;
            $('#loader').show();
            var bankname = $("#bankname").val();
            var branchname = $('#branchname').val();
            var branchloc = $('#branchloc').val();
            var ifsccode = $('#ifsccode').val();
            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part

            if (branchname == '' || branchname == '0' || branchname == null) {
                errormsg = errormsg + 'Please enter Branch Name !! <br />';
                iserror = true;
            }
            if (branchloc == '' || branchloc == '0' | branchloc == null) {
                errormsg = errormsg + 'Please enter Branch Location !! <br />';
                iserror = true;
            }
            if (ifsccode == '' || ifsccode == null) {
                errormsg = errormsg + 'Please enter ifsc code !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var myData = {

                'bank_id': bankname,
                'branch_name': branchname,
                'loc': branchloc,
                'ifsc_code': ifsccode,
                'created_by': login_emp_id,
                'modified_by': login_emp_id,
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_BranchMaster';//need to change this
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
                        //$("#bankname").val('');
                        $('#branchname').val('');
                        $('#branchloc').val('');
                        $('#ifsccode').val('');
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
                    alert(err.responseText);
                }
            });
        });

    }, 2000);// end timeout

});


function GetEditData(id) {
    $('#loader').show();
    if (id == "" || id == null) {
        messageBox("info", "invalid id");
        $('#loader').hide();
        return false;
    }
    $("#hdnid").val(id);
    var apiurl = localstorage.getItem("ApiUrl") + "apiPayroll/Get_Branchmaster/" + id;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            $("#bankname").val(data.bank_id);
            $('#branchname').val(data.branch_name);
            $('#branchloc').val(data.loc);
            $('#ifsccode').val(data.ifsc_code);
            $("#checkbtn").show();
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
        }

    });
}

//get data
function GetData() {
    // debugger;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Get_Branchmaster/0'; //change link here

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $("#tblBranchmaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [

                        //{
                        //    targets: [2],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [3],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        {
                            targets: [1],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": "bank_name", "name": "bank_name", "autoWidth": true },
                    { "data": "branch_name", "name": "branch_name", "autoWidth": true },
                    { "data": "loc", "name": "loc", "autoWidth": true },
                    { "data": "ifsc_code", "name": "ifsc_code", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.branch_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]


            });
        },
        error: function (error) {
            //alert(error);
            messageBox("error", error.responseText);
            console.log("error");
        }
    });

}