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

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        GetData(login_company_id);


        $("#btnupdate").hide();

        $("#btnreset").bind("click", function () {
            $('#loader').show();
            location.reload();
            $('#loader').hide();
        });


        $("#ddlcompany").bind("change", function () {

            GetData($(this).val());
            $("#txtstatusname").val('');
            $('input:radio[name=chkstatus]').each(function () { $(this).prop('checked', false); });
            $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
            $("#txtdesc").val('');

        });

        $("#btnsave").bind("click", function () {

            var companyid = $("#ddlcompany").val();
            var status_name = $("#txtstatusname").val();
            var description = $("#txtdesc").val();
            //var display_order = $("#ddldisplayorder").val();
            // var display_for ="";
            var status = $("input[name='chkstatus']:checked").val();
            var display_for_rm1 = $("#chkdisplayfor1").prop("checked") == true ? 1 : 0;
            var display_for_rm2 = $("#chkdisplayfor2").prop("checked") == true ? 1 : 0;
            var display_for_rm3 = $("#chkdisplayfor3").prop("checked") == true ? 1 : 0;
            var display_for = $("#chkdisplayfor4").prop("checked") == true ? 1 : 0;

            // var display_for = [];

            //$.each($("input[name='chkdisplayfor']:checked"), function () {
            //    display_for.push($(this).val());
            //});

            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == null || companyid == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (status_name == "" || status_name == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Status Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            //if (display_order == "0" || display_order == "") {
            //    is_error = true,
            //    error_msg = error_msg + "Please Select Display Order";
            //}

            if (status == "" || status == null || status == undefined) {
                is_error = true;
                error_msg = error_msg + "Please Select Status</br>";
            }

            if (display_for_rm1 == "0" && display_for_rm2 == "0" && display_for_rm3 == "0" && display_for == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select atleast one from display for</br>";
            }


            //if (display_for.length==0) {
            //    is_error = true;
            //    error_msg = error_msg + "Please Select Display for</br>";
            //}

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var mydata = {
                company_id: companyid,
                status_name: status_name,
                description: description,
                display_for: display_for,
                display_for_rm1: display_for_rm1,
                display_for_rm2: display_for_rm2,
                display_for_rm3: display_for_rm3,
                // display_order: display_order,
                status: status,
                created_by: login_emp_id
            }
            $("#loader").show();
            if (confirm("Do you want to save this ?")) {



                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "/apiePA/Save_StatusMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        $('#loader').hide();
                        var msg = response.message;
                        var statuscode = response.statusCode;
                        if (statuscode == "0") {
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            GetData(login_company_id);
                            $("#txtstatusname").val('');
                            // $("#ddldisplayorder").val(0);
                            $('input:radio[name=chkstatus]').each(function () { $(this).prop('checked', false); });
                            $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
                            $("#txtdesc").val('');

                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            messageBox("error", msg);
                            return false;
                        }

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
            }
            else {
                $("#loader").hide();
                return false;
            }

        });

        $("#btnupdate").bind("click", function () {

            var companyid = $("#ddlcompany").val();
            var status_name = $("#txtstatusname").val();
            var description = $("#txtdesc").val();
            // var display_order = $("#ddldisplayorder").val();
            // var display_for ="";
            var status = $("input[name='chkstatus']:checked").val();

            // var display_for = [];

            var display_for_rm1 = $("#chkdisplayfor1").prop("checked") == true ? 1 : 0;
            var display_for_rm2 = $("#chkdisplayfor2").prop("checked") == true ? 1 : 0;
            var display_for_rm3 = $("#chkdisplayfor3").prop("checked") == true ? 1 : 0;
            var display_for = $("#chkdisplayfor4").prop("checked") == true ? 1 : 0;


            //$.each($("input[name='chkdisplayfor']:checked"), function () {
            //    display_for.push($(this).val());
            //});

            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == null || companyid == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (status_name == "" || status_name == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Status Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            //if (display_order == "0" || display_order == "") {
            //    is_error = true,
            //        error_msg = error_msg + "Please Select Display Order";
            //}

            if (status == "" || status == null || status == undefined) {
                is_error = true;
                error_msg = error_msg + "Please Select Status</br>";
            }

            if (display_for_rm1 == "0" && display_for_rm2 == "0" && display_for_rm3 == "0" && display_for == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select atleast one from display for</br>";
            }

            //if (display_for.length == 0) {
            //    is_error = true;
            //    error_msg = error_msg + "Please Select Display for</br>";
            //}

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var mydata = {
                epa_status_id: $("#hdnmasterid").val(),
                company_id: companyid,
                status_name: status_name,
                description: description,
                //display_order: display_order,
                display_for: display_for,
                display_for_rm1: display_for_rm1,
                display_for_rm2: display_for_rm2,
                display_for_rm3: display_for_rm3,
                //display_for: display_for,
                status: status,
                last_modified_by: login_emp_id
            }

            $("#loader").show();
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiePA/Update_StatusMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    _GUID_New();
                    $('#loader').hide();
                    var msg = response.message;
                    var statuscode = response.statusCode;

                    if (statuscode == "0") {

                        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                        GetData(login_company_id);

                        $("#ddlcompany").attr("disabled", false);
                        $("#txtstatusname").val('');
                        $("#txtstatusname").attr("readonly", false);
                        // $("#ddldisplayorder").val(0);
                        $('input:radio[name=chkstatus]').each(function () { $(this).prop('checked', false); });
                        $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
                        $("#txtdesc").val('');

                        $("#btnupdate").hide();
                        $("#btnsave").show();
                        messageBox("success", msg);
                        return false;
                    }
                    else {
                        messageBox("error", msg);
                        return false;
                    }

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


    }, 2000);// end timeout

});


function GetData(companyid) {
    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_StatusMaster/" + companyid + "/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { "Authorization": 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblstatusmaster").DataTable({
                "processing": true,//for showing process bar
                "serverSide": false,//for process server side
                "bDestroy": true,
                "orderMulti": false,//for disable multiple column at once
                "filter": true,//this is for disable filter(search box)
                "aaData": res,
                "scrollX": 200,
                "columnDefs": [
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            var modified_date = new Date(data);
                            return new Date(row.last_modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(modified_date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "status_name", "name": "status_name", "title": "Status Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    //{
                    //    "title": "Display For", "autoWidth": true, "render": function (data, type, full, meta) {
                    //        var displaydetill = "";
                    //        if (full.display_for_rm1 == 1) {
                    //            displaydetill +='<input type=checkbox checked="checked" id=chkdisplay disabled="disabled"/> Reporting Manager1</br>'
                    //        }
                    //        if (full.display_for_rm2 == 1) {
                    //            displaydetill += '<input type=checkbox checked="checked" id=chkdisplay disabled="disabled"/> Reporting Manager2</br>';
                    //        }
                    //        if (full.display_for_rm3 == 1) {
                    //            displaydetill += '<input type=checkbox checked="checked" id=chkdisplay disabled="disabled"/> Reporting Manager3</br>';
                    //        }
                    //        if (full.display_for == 1) {
                    //            displaydetill +='<input type=checkbox checked="checked" id=chkdisplay disabled="disabled"/> User</br>';
                    //        }

                    //        return displaydetill;
                    //    }
                    //},
                    // { "data": "order_name", "name":"order_name", "title": "Display Order", "autoWidth": true },
                    { "data": "radio_status", "name": "radio_status", "title": "Status", "autoWidth": true },
                    { "data": "display_rm1", "name": "display_rm1", "title": "Display for RM1", "autoWidth": true },
                    { "data": "display_rm2", "name": "display_rm2", "title": "Display for RM2", "autoWidth": true },
                    { "data": "display_rm3", "name": "display_rm3", "title": "Display for RM3", "autoWidth": true },
                    { "data": "display_user", "name": "display_user", "title": "Display for User", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (row, data, full, meta) {
                            //return '<a href="#" onclick="GetEditData(' + full.epa_status_id + ')" ><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteStatus(' + full.epa_status_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a >';

                            return '<a href="#" onclick=GetEditData(' + full.company_id + ',' + full.epa_status_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteEpaStatus(' + full.company_id + ',' + full.epa_status_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').show();
            messageBox("error", err.responseText);
            return false;
        }
    });

}

function GetEditData(companyidd, epa_status_master) {

    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_StatusMaster/" + companyidd + "/" + epa_status_master,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { "Authorization": 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            BindAllEmp_Company('ddlcompany', login_emp_id, response.company_id);

            $("#ddlcompany").attr("disabled", "disabled");
            // $("#ddlcompany").val(response.company_id)
            $("#txtstatusname").val(response.status_name);
            // $("#txtstatusname").attr("readonly", "readonly");
            // $("#ddldisplayorder").val(response.display_order);


            $("#chkdisplayfor1").prop("checked", response.display_for_rm1 == 1 ? true : response.display_for_rm1 == 0 ? false : "");
            $("#chkdisplayfor2").prop("checked", response.display_for_rm2 == 1 ? true : response.display_for_rm2 == 0 ? false : "");
            $("#chkdisplayfor3").prop("checked", response.display_for_rm3 == 1 ? true : response.display_for_rm3 == 0 ? false : "");
            $("#chkdisplayfor4").prop("checked", response.display_for == 1 ? true : response.display_for == 0 ? false : "");


            //if (response.display_for == 1) {
            //    $("#chkdisplayfor4").prop("checked", true);
            //}

            //if (response.display_for_rm1 == 1) {
            //    $("#chkdisplayfor1").prop("checked", true);
            //}

            //if (response.display_for_rm2 == 1) {
            //    $("#chkdisplayfor2").prop("checked", true);
            //}

            //if (response.display_for_rm3 == 1) {
            //    $("#chkdisplayfor3").prop("checked", true);
            //}

            //$("input[id=chkdisplayfor1][value=" + response.display_for_rm1 + "]").prop('checked', true);
            //$("input[id=chkdisplayfor2][value=" + response.display_for_rm2 + "]").prop('checked', true);
            //$("input[id=chkdisplayfor3][value=" + response.display_for_rm3 + "]").prop('checked', true);
            //$("input[id=chkdisplayfor4][value=" + response.display_for + "]").prop('checked', true);
            //$("input[name=chkdisplayfor][value=" + response.display_for + "]").prop('checked', true);
            $("input[name=chkstatus][value=" + response.status + "]").prop('checked', true);

            $("#txtdesc").val(response.description);
            $("#hdnmasterid").val(response.epa_status_id);

            $("#btnsave").hide();
            $("#btnupdate").show();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}



function DeleteEpaStatus(companyidd, epa_status_master) {
    $("#loader").show();

    if (confirm("Do you want to process this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiePA/Delete_StatusMaster/" + companyidd + "/" + epa_status_master + "/" + login_emp_id,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: {},
            headers: headerss,
            success: function (response) {
                _GUID_New();
                var msg = response.message;
                var statuscode = response.statusCode;
                $('#loader').hide();
                if (statuscode == "0") {

                    BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                    GetData(login_company_id);

                    $("#ddlcompany").attr("disabled", false);
                    $("#txtstatusname").val('');
                    $("#txtstatusname").attr("readonly", false);
                    // $("#ddldisplayorder").val(0);
                    $('input:radio[name=chkstatus]').each(function () { $(this).prop('checked', false); });
                    $('input:checkbox[name=chkdisplayfor]').each(function () { $(this).prop('checked', false); });
                    $("#txtdesc").val('');

                    $("#btnupdate").hide();
                    $("#btnsave").show();
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("error", msg);
                    return false;
                }

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


    }
    else {
        $('#loader').hide();
        return false;
    }


}
