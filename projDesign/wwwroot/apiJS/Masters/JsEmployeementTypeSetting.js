$('#loader').show();
var empid;
var company_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $("#btnupdate").hide();

        BindAllEmp_Company('ddlcompany', empid, company_id);

        GetData(company_id);
        // HaveDisplay = ISDisplayMenu("Display Company List");

        BindGradeMaster('ddlgrade', 0);
        BindEmployementTypeForddl('ddlemployementtype', 0);

        GetData(0);

        $('#loader').hide();


        $("#ddlcompany").bind("change", function () {
            GetData($(this).val());
        });

        $("#btnreset").bind("click", function () {

            location.reload();
        });

        $("#btnsave").bind("click", function () {

            var grade_id = $("#ddlgrade").val();
            var employeement_type = $("#ddlemployementtype").val();
            var notice_period_months = $("#txtNoticePeriod_month").val();
            var notice_period_days = $("#txtNoticePeriod_days").val();
            var remarks = $("#txtremarks").val();
            var companyid = $("#ddlcompany").val();

            var is_error = false;
            var error_msg = "";

            if (companyid == "0" || companyid == "" || companyid == null) {

                error_msg = error_msg + "Please Select Company</br>";
                is_error = true;
            }


            if (grade_id == 0 || grade_id == null) {
                error_msg = error_msg + "Please Select Grade</br>";
                is_error = true;
            }

            if (employeement_type == 0 || employeement_type == null) {
                error_msg = error_msg + "Please Select Employeement Type</br>";
                is_error = true;
            }

            if (notice_period_months == "" || notice_period_months == null || notice_period_months == "0") {
                notice_period_months = 0;
                if (notice_period_days == "" || notice_period_days == null || notice_period_days == "0") {
                    error_msg = error_msg + "Please fill either duration in months or days</br>";
                    is_error = true;
                }
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            if (notice_period_days == "" || notice_period_days == null || notice_period_days == "0") {
                notice_period_days = 0;
            }

            var myData = {

                grade_id: grade_id,
                employeement_type: employeement_type,
                notice_period: notice_period_months,
                notice_period_days: notice_period_days,
                //notice_period: notice_period,
                remarks: remarks,
                company_id: companyid,
                created_by: empid
            }


            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiMasters/Save_EmployeementTypeSetting",
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    $('#loader').hide();
                    _GUID_New();

                    //if data save
                    if (statuscode == "0") {

                        BindAllEmp_Company('ddlcompany', empid, company_id);
                        GetData(company_id);


                        BindGradeMaster('ddlgrade', 0);
                        BindEmployementTypeForddl('ddlemployementtype', 0);
                        $("#txtNoticePeriod_days").val('');
                        $("#txtNoticePeriod_month").val('');
                        $("#txtremarks").val('');
                        messageBox("success", Msg);
                        return false;

                    }
                    else {
                        messageBox("error", Msg);

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

        $("#btnupdate").bind("click", function () {

            var grade_id = $("#ddlgrade").val();
            var employeement_type = $("#ddlemployementtype").val();
            var notice_period_month = $("#txtNoticePeriod_month").val();
            var notice_period_days = $("#txtNoticePeriod_days").val();
            var remarks = $("#txtremarks").val();
            var companyid = $("#ddlcompany").val();

            var is_error = false;
            var error_msg = "";

            if (companyid == "0" || companyid == "" || companyid == null) {

                error_msg = error_msg + "Please Select Company</br>";
                is_error = true;
            }


            if (grade_id == 0 || grade_id == null) {
                error_msg = error_msg + "Please Select Grade</br>";
                is_error = true;
            }

            if (employeement_type == 0 || employeement_type == null) {
                error_msg = error_msg + "Please Select Employeement Type</br>";
                is_error = true;
            }

            if (notice_period_month == "" || notice_period_month == null || notice_period_month == "0") {
                if (notice_period_days == "" || notice_period_days == null || notice_period_days == "0") {
                    error_msg = error_msg + "Please fill either duration in months or days</br>";
                    is_error = true;
                }

            }



            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var myData = {
                typesetting_id: $("#hdntypesetting_id").val(),
                grade_id: grade_id,
                employeement_type: employeement_type,
                notice_period: notice_period_month,
                notice_period_days: notice_period_days,
                //notice_period: notice_period,
                remarks: remarks,
                company_id: companyid,
                modified_by: empid
            }


            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiMasters/Edit_EmployeementTypeSetting',
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;

                    $('#loader').hide();
                    _GUID_New();

                    //if data save
                    if (statuscode == "0") {

                        $("#btnsave").show();
                        $("#btnupdate").hide();
                        BindAllEmp_Company('ddlcompany', empid, company_id);
                        GetData(company_id);


                        BindGradeMaster('ddlgrade', 0);
                        BindEmployementTypeForddl('ddlemployementtype', 0);
                        $('#ddlcompany').prop("disabled", false);
                        $("#txtNoticePeriod_days").val('');
                        $("#txtNoticePeriod_month").val('');
                        $("#txtremarks").val('');
                        messageBox("success", Msg);
                        return false;

                    }
                    else if (statuscode != "0") {
                        messageBox("error", Msg);

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




function GetData(companyidd) {
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEmployeementTypeSetting/" + companyidd + "/0",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            $("#tblemployeementtypedtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [3],
                            render: function (data, type, row) {

                                return data == "1" ? "Temporary" : data == "2" ? "Probation" : data == "3" ? "Confirmed" : data == "4" ? "Contract" : data == "10" ? "Notice" : data == "99" ? "FNF" : data == "100" ? "Separate" : "-";
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);

                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(row.modified_date);
                                var modifiedd_date = GetDateFormatddMMyyyy(date);;
                                //  return GetDateFormatddMMyyyy(date);
                                return new Date(row.modified_date) < new Date(row.created_date) ? "-" : modifiedd_date;
                            }

                        }

                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "title": "Grade", "autoWidth": true },
                    { "data": "employeement_type", "name": "employeement_type", "title": "Employeement Type", "autoWidth": true },
                    { "data": "notice_period", "name": "notice_period", "title": "Duration (in months)", "autoWidth": true },
                    { "data": "notice_period_days", "name": "notice_period_days", "title": "Duration (in days)", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Edit", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.typesetting_id + ')"><i class="fa fa-pencil-square-o"></i></a>';
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
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}

function GetEditData(_typesettingid) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEmployeementTypeSetting/0/" + _typesettingid,
        type: "GET",
        contentType: 'application/json;charset=utf-8',
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;
            BindAllEmp_Company('ddlcompany', empid, res.company_id);

            BindGradeMaster('ddlgrade', res.grade_id);
            BindEmployementTypeForddl('ddlemployementtype', res.employeement_type);
            $("#txtNoticePeriod_month").val(res.notice_period);
            $("#txtNoticePeriod_days").val(res.notice_period_days);
            $("#txtremarks").val(res.remarks);
            $("#hdntypesetting_id").val(res.typesetting_id);
            $("#btnsave").hide();
            $("#btnupdate").show();
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error", err.responseText);
        }
    });

}