$('#loader').show();
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }



        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetEmployeeWeekOff(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }
        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            GetEmployeeWeekOff(0);
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            GetEmployeeWeekOff($(this).val());
        });


        $('#loader').hide();



        $('#ddlWeekOff').bind("change", function () {
            if ($(this).val() == 2) {
                $("#dynamics").css("display", "block");
            }
            else {
                $("#dynamics").css("display", "none");
            }
        });


        var chkMondayclicked = false;
        $("input[name=chkMonday]").change(function () {
            //$("input[name=chkMondayDays1]").prop("checked", !chkMondayclicked);
            //chkMondayclicked = !chkMondayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkMondayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkMondayDays1]").prop("checked", false);
            }
        });
        var chkTuesdayclicked = false;
        $("input[name=chkTuesday]").change(function () {
            //$("input[name=chkTuesdayDays1]").prop("checked", !chkTuesdayclicked);
            //chkTuesdayclicked = !chkTuesdayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkTuesdayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkTuesdayDays1]").prop("checked", false);
            }
        });
        var chkWednesdayclicked = false;
        $("input[name=chkWednesday]").change(function () {
            //$("input[name=chkWednesdayDays1]").prop("checked", !chkWednesdayclicked);
            //chkWednesdayclicked = !chkWednesdayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkWednesdayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkWednesdayDays1]").prop("checked", false);
            }
        });
        var chkThursdayclicked = false;
        $("input[name=chkThursday]").change(function () {
            //$("input[name=chkThursdayDays1]").prop("checked", !chkThursdayclicked);
            //chkThursdayclicked = !chkThursdayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkThursdayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkThursdayDays1]").prop("checked", false);
            }
        });
        var chkFridayclicked = false;
        $("input[name=chkFriday]").change(function () {
            //$("input[name=chkFridayDays1]").prop("checked", !chkFridayclicked);
            //chkFridayclicked = !chkFridayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkFridayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkFridayDays1]").prop("checked", false);
            }
        });
        var chkSaturdayclicked = false;
        $("input[name=chkSaturday]").change(function () {
            //$("input[name=chkSaturdayDays1]").prop("checked", !chkSaturdayclicked);
            //chkSaturdayclicked = !chkSaturdayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkSaturdayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkSaturdayDays1]").prop("checked", false);
            }
        });
        var chkSundaydayclicked = false;
        $("input[name=chkSundayday]").change(function () {
            //$("input[name=chkSundayDays1]").prop("checked", !chkSundaydayclicked);
            //chkSundaydayclicked = !chkSundaydayclicked;
            if ($(this).prop("checked") == true) {
                $("input[name=chkSundayDays1]").prop("checked", true);
            }
            else {
                $("input[name=chkSundayDays1]").prop("checked", false);
            }
        });


        ///////////////////////////////////////////////////// SAVE EMPLOYEE WEEK OFF /////////////////////////////////////////

        $('#btnSaveWeekOff').bind("click", function () {
            $('#loader').show();
            var employee_id = $('#ddlEmployeeCode :selected').val();
            var ddlcompany = $("#ddlCompany").val();
            var ddlWeekOff = $("#ddlWeekOff").val();
            var effectiveFromDate = $("#txtWeekoffEffetctiveFromDt").val();
            if (ddlCompany == "0" || ddlCompany == null) {
                messageBox("error", "Enter the company");
                $('#loader').hide();
                return;
            }
            if (employee_id == "0" || employee_id == null || employee_id == "") {
                messageBox("error", "Enter the Employee Code");
                $('#loader').hide();
                return;
            }
            if (ddlWeekOff == "0" || ddlWeekOff == null) {
                messageBox("error", "Select the weekly off ");
                $('#loader').hide();
                return;
            }
            if (effectiveFromDate == "0" || effectiveFromDate == null || effectiveFromDate == "") {
                messageBox("error", "Enter the Effective From Date");
                $('#loader').hide();
                return;
            }

            var WeekOffArray = [];
            $.each($("input[name='chkMondayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '1', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkTuesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '2', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkWednesdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '3', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkThursdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '4', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkFridayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '5', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkSaturdayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '6', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });
            $.each($("input[name='chkSundayDays1']:checked"), function () {
                WeekOffArray.push({ 'week_day': '7', 'days': $(this).val(), 'company_id': ddlcompany, 'is_active': '1', 'created_by': login_emp_id, 'last_modified_by': login_emp_id });
            });


            var myData = {
                'employee_id': employee_id,
                'default_company_id': ddlcompany,
                'is_fixed_weekly_off': ddlWeekOff,
                'effectiveFromDt': effectiveFromDate,
                'ShiftWeekOff': WeekOffArray,
                'created_by': login_emp_id
            };

            //    alert(JSON.stringify(myData));
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/AddWeekOff',
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
                    if (statuscode == "1") {
                        alert(Msg);
                        location.reload();
                        //window.location.href = '/Employee/WeekOff';
                    }
                    else if (statuscode == "0") {
                        messageBox("error", "Something went wrong please try again...!");
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


///////////////////////////////////////////////////// START EMPLOYEE WEEK OFF /////////////////////////////////////////





//Get Employee Code
function GetEmployeeWeekOff(employee_id) {
    if ($.fn.DataTable.isDataTable('#tblWeekoffAllocation')) {
        $('#tblWeekoffAllocation').DataTable().clear().draw();
    }
    if (employee_id > 0) {

        $('#loader').show();
        var defaultcompany = localStorage.getItem('new_default_company');
        //   defaultcompany = 26;
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeWeekOff/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {
                //(JSON.stringify(data));
                var res = data;

                if (res != null && res != "") {


                    $("#tblWeekoffAllocation").DataTable({
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
                                    targets: [1, 10],
                                    render: function (data, type, row) {

                                        var date = new Date(data);
                                        return GetDateFormatddMMyyyy(date);
                                    }
                                }
                                //{
                                //    targets: [5],
                                //    "class": "text-center"

                                //}
                            ],

                        "columns": [
                            { "data": "emp_weekoff_id", "name": "emp_weekoff_id", "title": "SNo.", "autoWdith": true, "visible": false },
                            { "data": "effectiveFromDt", "name": "effectiveFromDt", "title": "Effective FromDt", "autoWidth": true },
                            { "data": "weeklyOff", "name": "weeklyOff", "title": "Weekly Off", "autoWidth": true },
                            { "data": "mondayOff", "name": "mondayOff", "title": "Monday", "autoWidth": true },
                            { "data": "tuesdayOff", "name": "tuesdayOff", "title": "Tuesday", "autoWidth": true },
                            { "data": "wednessdayOff", "name": "wednessdayOff", "title": "Wednessday", "autoWidth": true },
                            { "data": "thursdayOff", "name": "thursdayOff", "title": "Thursday", "autoWidth": true },
                            { "data": "fridayOff", "name": "fridayOff", "title": "Friday", "autoWidth": true },
                            { "data": "saturdayOff", "name": "saturdayOff", "title": "Saturday", "autoWidth": true },
                            { "data": "sundayOff", "name": "sundayOff", "title": "Sunday", "autoWidth": true },
                            { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                            {
                                "title": "Action", "autoWidth": true,
                                "render": function (data, type, full, meta) {
                                    return '<a href="#" onclick="DeleteEmpWeekOff(' + full.emp_weekoff_id + ')" ><i class="fa fa-pencil-square-o">Delete</i></a>';
                                }
                            }
                        ],
                        //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        //    return nRow;
                        //},
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                    });




                    //if (res.is_fixed_weekly_off == 2) {
                    //    $("#dynamics").css("display", "block");
                    //}
                    //else {
                    //    $("#dynamics").css("display", "none");
                    //}

                    //$("#ddlWeekOff").val(res.is_fixed_weekly_off);

                    //var WeekMondayArray = [];
                    //var WeekTuesdayArray = [];
                    //var WeekWednesdayArray = [];
                    //var WeekThursdayArray = [];
                    //var WeekFridayArray = [];
                    //var WeekSaturdayArray = [];
                    //var WeekSundayArray = [];

                    //$("input[name=chkMonday]").prop("checked", false);
                    //$("input[name=chkMondayDays1]").prop("checked", false);

                    //$("input[name=chkTuesday]").prop("checked", false);
                    //$("input[name=chkTuesdayDays1]").prop("checked", false);

                    //$("input[name=chkWednesday]").prop("checked", false);
                    //$("input[name=chkWednesdayDays1]").prop("checked", false);

                    //$("input[name=chkThursday]").prop("checked", false);
                    //$("input[name=chkThursdayDays1]").prop("checked", false);

                    //$("input[name=chkFriday]").prop("checked", false);
                    //$("input[name=chkFridayDays1]").prop("checked", false);
                    //$("input[name=chkSaturday]").prop("checked", false);
                    //$("input[name=chkSaturdayDays1]").prop("checked", false);
                    //$("input[name=chkSundayday]").prop("checked", false);
                    //$("input[name=chkSundayDays1]").prop("checked", false);

                    //$.each(res.shiftWeekOff, function (key, value) {


                    //    if (value.week_day == 1) {


                    //        WeekMondayArray.push({ 'day': value.days });

                    //        if (WeekMondayArray.length > 4) {
                    //            $("input[name=chkMonday]").prop("checked", true);
                    //        }

                    //        $("input[id=chkMondayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 2) {


                    //        WeekTuesdayArray.push({ 'day': value.days });
                    //        if (WeekTuesdayArray.length > 4) {
                    //            $("input[name=chkTuesday]").prop("checked", true);
                    //        }

                    //        $("input[id=chkTuesdayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 3) {


                    //        WeekWednesdayArray.push({ 'day': value.days });
                    //        if (WeekWednesdayArray.length > 4) {
                    //            $("input[name=chkWednesday]").prop("checked", true);
                    //        }


                    //        $("input[id=chkWednesdayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 4) {


                    //        WeekThursdayArray.push({ 'day': value.days });
                    //        if (WeekThursdayArray.length > 4) {
                    //            $("input[name=chkThursday]").prop("checked", true);
                    //        }


                    //        $("input[id=chkThursdayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 5) {


                    //        WeekFridayArray.push({ 'day': value.days });
                    //        if (WeekFridayArray.length > 4) {
                    //            $("input[name=chkFriday]").prop("checked", true);
                    //        }

                    //        $("input[id=chkFridayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 6) {


                    //        WeekSaturdayArray.push({ 'day': value.days });
                    //        if (WeekSaturdayArray.length > 4) {
                    //            $("input[name=chkSaturday]").prop("checked", true);
                    //        }

                    //        $("input[id=chkSaturdayDays" + value.days + "]").prop("checked", true);
                    //    }
                    //    else if (value.week_day == 7) {


                    //        WeekSundayArray.push({ 'day': value.days });
                    //        if (WeekSundayArray.length > 4) {
                    //            $("input[name=chkSundayday]").prop("checked", true);
                    //        }

                    //        $("input[id=chkSundayDays" + value.days + "]").prop("checked", true);
                    //    }

                    //});
                }
                else {
                    $("#ddlWeekOff").val(0);
                }

                $('#loader').hide();

            },
            error: function (error) {
                messageBox("error", error.responseText);
                $('#loader').hide();
            }
        });

    }

}

function DeleteEmpWeekOff(weekoffId) {
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/DeleteEmpWeekOff/' + weekoffId + '/' + login_emp_id,
        type: "POST",
        //data: JSON.stringify(myData),
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            var statuscode = data.statusCode;
            var Msg = data.message;

            $('#loader').hide();
            _GUID_New();
            //if data save
            if (statuscode == "1") {
                alert(Msg);
                location.reload();
                //window.location.href = '/Employee/WeekOff';
            }
            else if (statuscode == "0") {
                messageBox("error", "Something went wrong please try again...!");
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
}


///////////////////////////////////////////////////// END EMPLOYEE WEEK OFF /////////////////////////////////////////