$('#loader').show();
var login_emp_id;
var emp_role_idd;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();
        $('#btnupdate').hide();
        $('#btnsave').show();

        $('#loader').hide();



        $('#btnreset').bind('click', function () {
            //$("#txtclaimname").val('');
            location.reload();
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ClaimName = $("#txtclaimname").val();
            var errormsg = '';
            var iserror = false;

            if (ClaimName == '') {
                errormsg = errormsg + 'Please enter claim name !! \n';
                iserror = true;
            }

            if (iserror) {
                alert(errormsg);
                $('#loader').hide();
                return false;
            }


            var myData = {

                'claim_master_name': ClaimName,
                'claim_master_id': 0,
            };

            $('#loader').show();
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/SaveClaimMasters';
            var obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                data: obj,
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
                        //$("#txtclaimname").val('');
                        //GetData();
                        //messageBox("success", Msg);
                    }
                    else {
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


        //-------update city data
        $("#btnupdate").bind("click", function () {

            $('#loader').show();
            $('#btnupdate').text('Please wait..').prop("disabled", true);
            var claimid = $('#hdnid').val();

            var ClaimName = $("#txtclaimname").val();
            var errormsg = '';
            var iserror = false;

            if (ClaimName == '') {
                errormsg = errormsg + 'Please enter claim name !! \n';
                iserror = true;
            }
            if (claimid == '') {
                errormsg = errormsg + 'Invalid claimid, please reload page and try again !! \n';
                iserror = true;
            }

            if (iserror) {
                alert(errormsg);
                $('#loader').hide();
                return false;
            }


            var myData = {

                'claim_master_id': claimid,
                'claim_master_name': ClaimName,
            };

            $('#loader').show();
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/UpdateClaimMaster/';
            var obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: apiurl,
                type: "POST",
                data: obj,
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

                    }
                    else {
                        messageBox("error", Msg);


                    }
                    $('#loader').hide();
                    $('#btnupdate').text('Please wait..').prop("disabled", false);
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
                    $('#btnupdate').text('Please wait..').prop("disabled", false);
                    $('#btnupdate').hide();
                    $('#btnsave').show();

                }
            });
        });

    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData() {

    var claimid = $("#hdnid").val();
    claimid = claimid == '' || claimid == 'undefined' ? 0 : claimid;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetClaimMaster/' + claimid;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblclaim").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollY": 150,
                "aaData": res,
                "columnDefs":
                    [
                        {

                        }
                    ],

                "columns": [
                    {
                        "render": function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { "data": "claim_master_id", "name": "claim_master_id", "autoWidth": true },
                    { "data": "claim_master_name", "name": "claim_master_name", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick=EditData("' + full.claim_master_id + '");><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}

function EditData(claimid) {
    $('#loader').show();

    if (claimid != null && claimid != "undefined") {

        $('#loader').show();
        var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetClaimMaster/' + claimid;
        $('#loader').show();

        $.ajax({
            type: "GET",
            url: apiurl,
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {

                $('#hdnid').val(res[0].claim_master_id);
                $("#txtclaimname").val(res[0].claim_master_name);
                $('#btnupdate').show();
                $('#btnsave').hide();

                $('#loader').hide();
            },
            Error: function (err) {
                $('#loader').hide();
                messageBox("error", err.responseText);
            }
        });
    }
    else {
        $('#loader').hide();
        alert('There some problem to process this request !!');
    }
}



