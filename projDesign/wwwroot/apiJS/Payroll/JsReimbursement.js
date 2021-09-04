$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        // debugger;
        GetAllCategory(0);
        // Get Quarter On Financial Year Change
        //$('#ddlfyi').bind("change", function () {
        //    BindQuarterList('ddlquarter', 0, $(this).val());
        //});
        //GetData();

        //$('#btnreset').hide();
        //$('#btnsave').hide();
        $('#btnupdate').hide();
        $('#btnsave').show();
        $('#loader').hide();

    }, 2000);// end timeout

});

function GetAllCategory(category_id) {
    $('#loader').show();
    // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_ReimbursementCategoryMaster/" + category_id + "",
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblCategory").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [2],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'De-Active'
                            }
                        }
                        //},
                        //{
                        //    targets: [6],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [8],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //}
                    ],

                "columns": [
                    { "data": null },
                    { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                    //{ "data": "key_name", "name": "key_name", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true, "formatter": activeFormatter },
                    //{
                    //    "render": function (data, type, row) {

                    //        if ({ data: "is_active" } == '1') {

                    //            return 'Active';
                    //        }

                    //        else {

                    //            return 'De-Active';

                    //        }
                    //    }
                    //},
                    //{ "data": "created_by", "name": "created_by", "autoWidth": true },
                    //{ "data": "created_date", "name": "created_date", "autoWidth": true },
                    //{ "data": "modified_by", "name": "modified_by", "autoWidth": true },
                    //{ "data": "modified_date", "name": "modified_date", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="EditCategory(' + full.rcm_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                            //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },

                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" class="trigger_popup_fricc" onclick="ShowSalaryGroupKeyused(' + full.group_id + ')" >View</a>';
                    //        //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            function activeFormatter(cellvalue, options, rowObject) {
                if (cellvalue == 1)
                    return "Active";
                else if (cellvalue == 0)
                    return "De-Active";
            }
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}
function EditCategory(category_id) {
    // debugger;
    $('#loader').show();
    if (category_id == null || category_id == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnCategory").val(category_id);


    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryMaster/' + category_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            //alert(JSON.stringify(data))
            $("#txtCategoryMaster").val(data.reimbursement_category_name);
            $("#ddlActive").val(data.is_active);
            $("#hdgroupid").val(data.rcm_id);
            $('#btnupdate').show();
            $('#btnsave').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });

}

$('#btnreset').bind('click', function () {
    // debugger;
    $("#txtCategoryMaster").val('');
    $('#ddlActive').val('2');
    $('#btnupdate').hide();
    $('#btnsave').show();
    $("#hdgroupid").val('');
    GetAllCategory(0);

});

$('#btnsave').bind("click", function () {
    // debugger;
    $('#loader').show();
    var category = $("#txtCategoryMaster").val();

    var active = $('#ddlActive').val();

    var emp_id = login_emp_id;
    //var view = 0;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (category == null || category == '') {
        errormsg = "Please enter Category Name !! <br/>";
        iserror = true;
    }


    if (active == '' || active == '2') {
        errormsg = errormsg + 'Please select Active Status !! <br />';
        iserror = true;
    }
    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {
        'reimbursement_category_name': category,
        'is_active': active,
        'created_by': emp_id,

    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Save_ReimbursementCategoryMaster';
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
                $("#txtCategoryMaster").val('');
                $('#ddlActive').val('2');
                $('#btnupdate').hide();
                $('#btnsave').show();
                $("#hdgroupid").val('');
                GetAllCategory(0);


                messageBox("success", Msg);
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


//-------update city data
$("#btnupdate").bind("click", function () {
    // debugger;
    $('#loader').show();
    var category = $("#txtCategoryMaster").val();

    var active = $('#ddlActive').val();

    var emp_id = login_emp_id;
    var category_id = $("#hdnCategory").val();
    var errormsg = '';
    var iserror = false;

    //validation part
    if (category == null || category == '') {
        errormsg = "Please enter Salary Group Name !! <br/>";
        iserror = true;
    }

    if (active == '' || active == '2') {
        errormsg = errormsg + 'Please select Active Status !! <br />';
        iserror = true;
    }
    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var myData = {
        'rcm_id': category_id,
        'reimbursement_category_name': category,
        'is_active': active,
        'created_by': emp_id,
        'modified_by': emp_id,

    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Update_ReimbursementCategoryMaster';
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
                $("#txtCategoryMaster").val('');
                $('#ddlActive').val('2');
                $('#btnupdate').hide();
                $('#btnsave').show();
                $("#hdgroupid").val('');
                GetAllCategory(0);

                messageBox("success", Msg);

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
