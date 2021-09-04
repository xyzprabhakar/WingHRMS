$('#loader').show();

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        // debugger;
        // BindGradeList('ddlgrade', 0);
        // GetKeyDetails(0);
        GetAllSalaryGroup(0);
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

        $('.popupCloseButton').click(function () {
            $('.hover_bkgr_fricc').hide();
        });
        $('#btnreset').bind('click', function () {
            // debugger;
            $("#txtsalerygroup").val('');
            $('#ddlActive').val('2');
            //$('#ddlgrade').val('0');
            $('#txtminrange').val('');
            // $('#txtmaxrange').val('');
            $('#txtdescription').val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
            $("#hdgroupid").val('');
            GetAllSalaryGroup(0);
            // GetKeyDetails(0);
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            // debugger;
            var salerygroup = $("#txtsalerygroup").val();
            //var grade = $('#ddlgrade').val();
            var min_range = $('#txtminrange').val();
            //  var max_range = $('#txtmaxrange').val();
            var active = $('#ddlActive').val();
            var description = $('#txtdescription').val();
            var emp_id = CryptoJS.AES.decrypt(localStorage.getItem('emp_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            //var view = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (salerygroup == null || salerygroup == '') {
                errormsg = "Please enter Salary Group Name !! <br/>";
                iserror = true;
            }
            //if (grade == '' || grade == '0') {
            //    errormsg = errormsg + 'Please select Grade !! <br />';
            //    iserror = true;
            //}
            if (min_range == '' || min_range == null) {
                errormsg = errormsg + 'Please select min range!! <br />';
                iserror = true;
            }
            //if (max_range == '' || max_range == null) {
            //    errormsg = errormsg + 'Please select max range !! <br />';
            //    iserror = true;
            //}
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
            var keydetails = [];
            var table = $("#tblkey").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var key = $(this).val();
                    keydetails.push(key);
                }
            });
            var myData = {

                'group_name': salerygroup,
                'description': description,
                'minvalue': min_range,
                // 'maxvalue': max_range,
                //'grade_Id':0,//grade,
                'is_active': active,
                'created_by': emp_id,
                'key_id': keydetails
            };
            var apiurl = localStorage.getItem("ApiUrl") + "apipayroll/Save_SalaryGroupMaster";
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
                        $("#txtsalerygroup").val('');
                        $('#ddlActive').val('2');
                        // $('#ddlgrade').val('0');
                        $('#txtminrange').val('');
                        //  $('#txtmaxrange').val('');
                        $('#txtdescription').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("#hdgroupid").val('');
                        GetAllSalaryGroup();
                        // GetKeyDetails(0);

                        messageBox("success", Msg);
                        //alert(Msg);
                        //location.reload();
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


        //-------update city data
        $("#btnupdate").bind("click", function () {
            // debugger;
            $('#loader').show();
            var salerygroup = $("#txtsalerygroup").val();
            //var grade = $('#ddlgrade').val();
            var min_range = $('#txtminrange').val();
            //  var max_range = $('#txtmaxrange').val();
            var active = $('#ddlActive').val();
            var description = $('#txtdescription').val();
            var emp_id = CryptoJS.AES.decrypt(localStorage.getItem('emp_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            var group_id = $("#hdgroupid").val();
            var errormsg = '';
            var iserror = false;

            //validation part
            if (salerygroup == null || salerygroup == '') {
                errormsg = "Please enter Salary Group Name !! <br/>";
                iserror = true;
            }
            //if (grade == '' || grade == '0') {
            //    errormsg = errormsg + 'Please select Grade !! <br />';
            //    iserror = true;
            //}
            if (min_range == '' || min_range == null) {
                errormsg = errormsg + 'Please select min range!! <br />';
                iserror = true;
            }
            //if (max_range == '' || max_range == null) {
            //    errormsg = errormsg + 'Please select max range !! <br />';
            //    iserror = true;
            //}
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
            var keydetails = [];
            var table = $("#tblkey").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var key = $(this).val();
                    keydetails.push(key);
                }
            });
            var myData = {
                'group_id': group_id,
                'group_name': salerygroup,
                'description': description,
                'minvalue': min_range,
                //'maxvalue': max_range,
                // 'grade_Id':0, //grade,
                'is_active': active,
                'created_by': emp_id,
                'modified_by': emp_id,
                'key_id': keydetails
            };
            var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Update_SalaryGroupMaster';
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
                        //$("#txtsalerygroup").val('');
                        //$('#ddlActive').val('2');
                        //$('#ddlgrade').val('0');
                        //$('#txtminrange').val('');
                        ////  $('#txtmaxrange').val('');
                        //$('#txtdescription').val('');
                        //$('#btnupdate').hide();
                        //$('#btnsave').show();
                        $("#txtsalerygroup").val('');
                        $('#ddlActive').val('2');
                        // $('#ddlgrade').val('0');
                        $('#txtminrange').val('');
                        //  $('#txtmaxrange').val('');
                        $('#txtdescription').val('');
                        $('#btnupdate').hide();
                        $('#btnsave').show();
                        $("#hdgroupid").val('');
                        GetAllSalaryGroup();
                        // GetKeyDetails(0);

                        messageBox("success", Msg);

                        //alert(Msg);
                        // location.reload();

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



    }, 2000);// end timeout

});
//$(window).load(function () {
//    $('.popupCloseButton').click(function () {
//        $('.hover_bkgr_fricc').hide();
//    });
//});


function BindGradeList(ControlId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_GradeMasterData/0",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.sno).html(value.gradename));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}



function GetAllSalaryGroup() {
    // debugger;  
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_SalaryGroupMasterData/0",
        type: "GET",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $('#loader').hide();

            $("#tblsalgroup").DataTable({
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
                            targets: [3],
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
                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "group_name", "name": "group_name", "title": "Salary Group", "autoWidth": true },
                    //{ "data": "grade_name", "name": "grade_name", "title": "Grade", "autoWidth": true },
                    { "data": "minvalue", "name": "minvalue", "title": "Salary", "autoWidth": true },
                    //{ "data": "key_name", "name": "key_name", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Is Active", "autoWidth": true },
                    //{ "data": "created_by", "name": "created_by", "autoWidth": true },
                    //{ "data": "created_date", "name": "created_date", "autoWidth": true },
                    //{ "data": "modified_by", "name": "modified_by", "autoWidth": true },
                    //{ "data": "modified_date", "name": "modified_date", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="EditSalaryGroup(' + full.group_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                            //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }

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

        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}

function DeleteSalaryGroup(Group_ID_) {
    $('#loader').show();
    if (confirm("Do you want to delete this?")) {
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        // deletion code
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiPayroll/Get_SalaryGroupMasterData" + Group_ID_,
            type: 'Delete',
            dataType: 'json',
            headers: headerss,
            success: function (data) {

                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                //if data save
                if (statuscode == "1") {
                    alert(Msg);
                }
                else if (statuscode == "0") {
                    alert(Msg);
                }
                GetAllSalaryGroup()
            },
            error: function (error) {
                $('#loader').hide();
                _GUID_New();
                messageBox("error", error.responseText);
            }
        });


    }

    $('#loader').hide();
    return false;
}

function EditSalaryGroup(Group_ID_) {
    $('#loader').show();
    if (Group_ID_ == null || Group_ID_ == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdgroupid").val(Group_ID_);


    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryGroupMasterData/' + Group_ID_;
    //debugger;
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
            $("#txtsalerygroup").val(data[0].group_name);
            //$('#ddlgrade').val(data.grade_name);
            $("#txtminrange").val(data[0].minvalue);
            $("#txtmaxrange").val(data[0].maxvalue);
            $("#txtdescription").val(data[0].description);
            // BindGradeList('ddlgrade', data[0].grade_id);
            // $("#ddlActive").selected(data[0].is_active);
            $("#ddlActive").val(data[0].is_active);
            $("#hdgroupid").val(data[0].group_id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            // EditSalaryGroupKeyused(data[0].group_id);
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}

function EditSalaryGroupKeyused(Group_ID_) {
    $('#loader').show();
    if (Group_ID_ == null || Group_ID_ == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdgroipid").val(Group_ID_);

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryGroupKeyUsed/' + Group_ID_;
    // debugger;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblkey").DataTable({
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
                            targets: 2,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        }
                    ],
                "columns": [
                    //{
                    //    "render": function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},

                    { "data": null },
                    { "data": "key_name", "name": "key_name", "autoWidth": true },
                    {

                        "render": function (data, type, full, meta) {


                            if (full.ischecked == true) {

                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked id="chk' + full.key_id + '" value="' + full.key_id + '" />';
                            }
                            else {

                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.key_id + '" value="' + full.key_id + '" />';
                            }
                        }
                    },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },


            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}

function ShowSalaryGroupKeyused(Group_ID_) {
    $('#loader').show();
    if (Group_ID_ == null || Group_ID_ == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    $('.hover_bkgr_fricc').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_SalaryGroupKeyUsed/' + Group_ID_;
    // debugger;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();

            $("#tblKeyUsed").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                "columns": [
                    //{
                    //    "render": function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},

                    { "data": null },
                    { "data": "key_name", "name": "key_name", "autoWidth": true },

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[15, 50, -1], [15, 50, "All"]]


            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}



function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblkey").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(this).prop('checked', true);
        }
        else {
            $(this).prop('checked', false);
        }
    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblkey").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

function GetKeyDetails(key_id) {
    // debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_keyMasterDetails/' + key_id;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            $('#loader').hide();
            $("#tblkey").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollY": 250,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 2,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        }
                    ],

                "columns": [
                    //{
                    //    "render": function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},

                    { "data": null },
                    { "data": "key_name", "name": "key_name", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {

                            return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.key_id + '" value="' + full.key_id + '" />';

                        }
                    },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },


            });
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

}






