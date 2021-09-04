$('#loader').show();
var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        GetData();

        $('#btnupdate').hide();

    }, 2000);// end timeout

});



$('#btnsave').bind("click", function () {
    $('#loader').show();
    debugger;
    var fileUpload = $("#txtfupDoc").get(0);
    var _files = fileUpload.files;

    if (_files.length < 1) {
        messageBox("error", "Please Select a file to Upload...!");
        $('#loader').hide();
        return;
    }

    var formData = new FormData();
    for (var i = 0; i < _files.length; i++) {
        formData.append('file' + i + '', _files[i]);
    }

    var txtPolicies = $("#txtPolicies").val();
    var txtRemark = $("#txtRemark").val();


    //validation part

    if (txtPolicies == null || txtPolicies == '') {
        messageBox("error", "Please Enter Policy Name...!");
        $('#loader').hide();
        return;
    }
    if (txtRemark == null || txtRemark == '') {
        messageBox("error", "Please Enter Remarks...!");
        $('#loader').hide();
        return;
    }


    var myData = {
        'policy_name': txtPolicies.trim(),
        'remarks': txtRemark.trim(),
        'created_by': login_emp_id,
    };

    var Obj = JSON.stringify(myData);
    formData.append('AllData', Obj);


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    console.log(Obj);
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/Save_Policies/",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        headers: headerss,
        success: function (response) {
            debugger;
            var statuscode = response.statusCode;
            var Msg = response.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                $("#txtRemark").val("");
                $("#txtPolicies").val("");
                $('#btnupdate').hide();
                $('#btnsave').show();
                $("btnupdate").text('Update').attr("disabled", false);
                messageBox("success", Msg);
                GetData();
            }
            else {
                messageBox("error", Msg);
                return false;
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

    });// ajax end

});

function GetData() {
    //debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_Policies';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblReport").DataTable({
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
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                    ],

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "policy_name", "name": "policy_name", "title": "Policy Name", "autoWidth": true },
                    { "data": "created_by", "name": "created_by", "title": "Created By", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Attachment", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="" onclick="OpenDocumentt(\'' + full.doc_path + '\')"><i class="fa fa-paperclip"></i>Attachment</a>';
                        }
                    },
                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.pkid_policy + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a  onclick="DeletePolicy(' + full.pkid_policy + ' )" > <i class="fa fa-trash"></i></a > ';
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
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });

}

function OpenDocumentt(path) {
    debugger;
    window.open(path);
}


function GetEditData(id) {
    //debugger;

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + '/apiMasters/Get_PoliciesById/' + id;
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //debugger;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            $('#txtPolicies').val(res.policy_name);
            $('#txtRemark').val(res.remarks);

            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            alert(err.responseText)
            $('#loader').hide();
        }
    });

}

function DeletePolicy(request_id) {
    debugger;
    $("#loader").show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/DeletePoliciesById/' + request_id;

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                debugger;
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                GetData();

                $("#loader").hide();
                if (statuscode == "0") {
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("info", msg);
                    return false;
                }
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}

$('#btnupdate').bind("click", function () {
    debugger;

    $('#loader').show();

    var fileUpload = $("#txtfupDoc").get(0);
    var _files = fileUpload.files;

    //if (_files.length < 1) {
    //    messageBox("error", "Please Select a file to Upload...!");
    //    $('#loader').hide();
    //    return;
    //}

    var formData = new FormData();
    for (var i = 0; i < _files.length; i++) {
        formData.append('file' + i + '', _files[i]);
    }

    var txtPolicies = $("#txtPolicies").val();
    var txtRemark = $("#txtRemark").val();
    var policy_id = $("#hdnid").val();


    //validation part

    if (txtPolicies == null || txtPolicies == '') {
        messageBox("error", "Please Enter Policy Name...!");
        $('#loader').hide();
        return;
    }
    if (txtRemark == null || txtRemark == '') {
        messageBox("error", "Please Enter Remarks...!");
        $('#loader').hide();
        return;
    }


    var myData = {
        'pkid_policy': policy_id,
        'policy_name': txtPolicies.trim(),
        'remarks': txtRemark.trim(),
        'last_modified_by': login_emp_id,
    };

    var Obj = JSON.stringify(myData);
    formData.append('AllData', Obj);


    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    console.log(Obj);
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/Update_Policies/",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        headers: headerss,
        success: function (response) {
            debugger;
            var statuscode = response.statusCode;
            var Msg = response.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                $("#txtRemark").val("");
                $("#txtPolicies").val("");
                $('#btnupdate').hide();
                $('#btnsave').show();
                $("btnupdate").text('Update').attr("disabled", false);
                messageBox("success", Msg);
                GetData();
            }
            else {
                messageBox("error", Msg);
                return false;
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

    });// ajax end


}); //end btn Update


$('#btnreset').bind('click', function () {

    location.reload();
});


