
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

        // HaveDisplay = ISDisplayMenu("Display Company List");
        GetData(login_company_id);


        $("#btnupdate").hide();


        $("#ddlcompany").bind("change", function () {
            $("#hdnratingid").val('');
            $("#txtname").val('');
            $("#txtdesc").val('');
            $("#btnupdate").hide();
            $("#btnsave").show();
        });


        $("#btnsave").bind("click", function () {
            var companyid = $("#ddlcompany").val();
            var namee = $("#txtname").val();
            //var displayorder = $("#ddldisplayorder").val();
            var description = $("#txtdesc").val();
            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == "0" || companyid == null) {
                is_error = true;
                error_msg = "Please select company id</br>";
            }
            if (namee == "" || namee == null) {
                is_error = true;
                error_msg = "Please enter name</br>";
            }

            //if (displayorder == "" || displayorder == "0" || displayorder == null) {
            //    is_error = true;
            //    error_msg = "Please select display order</br>";
            //}

            if (description == "" || description == null) {
                is_error = true;
                error_msg = "Please enter description</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                company_id: companyid,
                rating_name: namee,
                description: description,
                // display_order: displayorder,
                created_by: login_emp_id
            }
            $("#loader").show();
            if (confirm("Are you sure you want to save this kpi rating ?")) {

                var headerss = {};
                headerss["Authorization"] = "Bearer " + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();

                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Save_KpiRatingMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var res = response;
                        var statuscode = response.statusCode;
                        var msg = response.message;
                        $("#loader").hide();
                        if (statuscode == "0") {
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            $("#txtname").val('');
                            $("#txtdesc").val('');
                            GetData(login_company_id);

                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            $("#loader").hide();
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
            var namee = $("#txtname").val();
            // var displayorder = $("#ddldisplayorder").val();
            var description = $("#txtdesc").val();
            var is_error = false;
            var error_msg = "";

            if (companyid == "" || companyid == "0" || companyid == null) {
                is_error = true;
                error_msg = "Please select company id</br>";
            }
            if (namee == "" || namee == null) {
                is_error = true;
                error_msg = "Please enter name</br>";
            }

            //if (displayorder == "" || displayorder == "0" || displayorder == null) {
            //    is_error = true;
            //    error_msg = "Please select display order</br>";
            //}

            if (description == "" || description == null) {
                is_error = true;
                error_msg = "Please enter description</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                kpi_rating_id: $("#hdnratingid").val(),
                company_id: companyid,
                rating_name: namee,
                description: description,
                // display_order: displayorder,
                created_by: login_emp_id
            }


            $("#loader").show();
            var headerss = {};
            headerss["Authorization"] = "Bearer " + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiePA/Update_KpiRatingMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    _GUID_New();
                    var res = response;
                    var statuscode = response.statusCode;
                    var msg = response.message;
                    $("#loader").hide();
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                        $("#txtname").val('');
                        $("#txtdesc").val('');
                        GetData(login_company_id);

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


        $("#btnreset").bind("click", function () {

            location.reload();

        });


    }, 2000);// end timeout


});


function GetData(companyidd) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_kpiRatingMaster/" + companyidd + "/0",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblkpirating").DataTable({
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
                            var _modified_date = new Date(data);
                            return new Date(row.modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(_modified_date);
                        }
                    },
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "rating_name", "name": "rating_name", "title": "Rating Name", "autoWidth": true },
                    // { "data": "display_order_name", "name": "display_order_name", "title": "Display Order", "autoWidth": true },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (row, data, full, meta) {
                            return '<a href="#" onclick=GetEditData(' + full.company_id + ',' + full.kpi_rating_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteRating(' + full.company_id + ',' + full.kpi_rating_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                }, // for S.No
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });

            $('#loader').hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}


function GetEditData(companyidd, rating_id) {

    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_kpiRatingMaster/" + companyidd + "/" + rating_id,
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
            // BindCompanyList('ddlcompany', response.company_id);
            BindAllEmp_Company('ddlcompany', login_emp_id, response.company_id);
            $("#ddlcompany").attr("disabled", "disabled");
            $("#txtname").val(response.rating_name);

            $("#txtdesc").val(response.description);
            $("#hdnratingid").val(response.kpi_rating_id);

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


function DeleteRating(companyidd, ratingid) {
    $("#loader").show();

    if (confirm("Do you want to delete this?")) {

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiePA/Delete_kpiRatingMaster/" + companyidd + "/" + ratingid + "/" + login_emp_id,
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
                    BindAllEmp_Company('ddlcompany', login_emp_id, response.company_id);
                    GetData(login_company_id);

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
