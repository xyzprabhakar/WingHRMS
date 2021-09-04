var login_company_id;
var HaveDisplay;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        //    HaveDisplay = ISDisplayMenu("Display Company List");


        GetData(login_company_id);

        $("#btnupdate").hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {

            var companyid = $("#ddlcompany").val();
            var objectivename = $("#txtname").val();
            var description = $("#txtdesc").val();

            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == null || companyid == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (objectivename == "" || objectivename == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Status Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var mydata = {
                company_id: companyid,
                objective_name: objectivename,
                description: description,
                created_by: login_emp_id
            }
            $("#loader").show();
            if (confirm("Do you want to save this ?")) {



                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();
                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "/apiePA/Save_KPIObjective",
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

                            $("#txtname").val('');
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

        $("#ddlcompany").bind("change", function () {

            if ($.fn.DataTable.isDataTable('#tblkpiobjective')) {
                $('#tblkpiobjective').DataTable().clear().draw();
            }
            $("#txtname").val('');
            $("#txtdesc").val('');
            $('#loader').hide();
            if ($(this).val() != "0") {
                GetData($(this).val());
            }
        });

        $("#btnupdate").bind("click", function () {

            var companyid = $("#ddlcompany").val();
            var objectivename = $("#txtname").val();
            var description = $("#txtdesc").val();

            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == null || companyid == "0") {
                is_error = true;
                error_msg = error_msg + "Please Select Company</br>";
            }

            if (objectivename == "" || objectivename == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Status Name</br>";
            }

            if (description == "" || description == null) {
                is_error = true;
                error_msg = error_msg + "Please Enter Description</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }


            var mydata = {
                obj_type_id: $("#hdnobjectiveype").val(),
                company_id: companyid,
                objective_name: objectivename,
                description: description,
                last_modified_by: login_emp_id
            }

            $("#loader").show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiePA/Update_KPIObjective",
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
                        //alert(msg);
                        //location.reload();
                        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                        GetData(login_company_id);

                        $("#txtname").val('');
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



        });


    }, 2000);// end timeout

});


function GetData(companyid) {
    $('#loader').show();
    if ($.fn.DataTable.isDataTable('#tblkpiobjective')) {
        $('#tblkpiobjective').DataTable().clear().draw();
    }
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiePA/Get_KPIObjective/" + companyid + "/0",
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

            $("#tblkpiobjective").DataTable({
                "processing": true,//for showing process bar
                "serverSide": false,//for process server side
                "bDestroy": true,
                "orderMulti": false,//for disable multiple column at once
                "filter": true,//this is for disable filter(search box)
                "aaData": res,
                "scrollX": 200,
                "columnDefs": [
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            var modified_date = new Date(data);
                            return new Date(row.last_modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(modified_date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "objective_name", "name": "objective_name", "title": "Objective Name", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (row, data, full, meta) {
                            return '<a href="#" onclick=GetEditData(' + full.company_id + ',' + full.obj_type_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteObjectiveType(' + full.company_id + ',' + full.obj_type_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
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



function GetEditData(companyidd, objectivetype) {

    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_KPIObjective/" + companyidd + "/" + objectivetype,
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
            //BindCompanyList('ddlcompany', response.company_id);
            $("#ddlcompany").attr("disabled", "disabled");

            $("#txtname").val(response.objective_name);
            $("#txtdesc").val(response.description);
            $("#hdnobjectiveype").val(response.obj_type_id);

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


function DeleteObjectiveType(companyidd, objectivetype) {
    $("#loader").show();

    if (confirm("Do you want to process this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiePA/Delete_KPIobjectType/" + companyidd + "/" + objectivetype + "/" + login_emp_id,
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
                    //BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                    GetData(companyidd);
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


