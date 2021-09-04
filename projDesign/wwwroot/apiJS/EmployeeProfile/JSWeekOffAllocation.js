var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        BindCompanyList('ddlCompany', company_id);


        BindEmployeeCodee('ddlEmployeeCode', company_id, login_emp_id);

        //$('#ddlCompany').bind("change", function () {
        //    BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
        //});


        //$('#ddlEmployeeCode').bind("change", function () {
        GetEmployeeWeekOff(login_emp_id);

        //  });

    }, 2000);// end timeout

});



function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}


///////////////////////////////////////////////////// START EMPLOYEE WEEK OFF /////////////////////////////////////////



var chkMondayclicked = false;
$("input[name=chkMonday]").change(function () {
    $("input[name=chkMondayDays1]").prop("checked", !chkMondayclicked);
    chkMondayclicked = !chkMondayclicked;
});
var chkTuesdayclicked = false;
$("input[name=chkTuesday]").change(function () {
    $("input[name=chkTuesdayDays1]").prop("checked", !chkTuesdayclicked);
    chkTuesdayclicked = !chkTuesdayclicked;
});
var chkWednesdayclicked = false;
$("input[name=chkWednesday]").change(function () {
    $("input[name=chkWednesdayDays1]").prop("checked", !chkWednesdayclicked);
    chkWednesdayclicked = !chkWednesdayclicked;
});
var chkThursdayclicked = false;
$("input[name=chkThursday]").change(function () {
    $("input[name=chkThursdayDays1]").prop("checked", !chkThursdayclicked);
    chkThursdayclicked = !chkThursdayclicked;
});
var chkFridayclicked = false;
$("input[name=chkFriday]").change(function () {
    $("input[name=chkFridayDays1]").prop("checked", !chkFridayclicked);
    chkFridayclicked = !chkFridayclicked;
});
var chkSaturdayclicked = false;
$("input[name=chkSaturday]").change(function () {
    $("input[name=chkSaturdayDays1]").prop("checked", !chkSaturdayclicked);
    chkSaturdayclicked = !chkSaturdayclicked;
});
var chkSundaydayclicked = false;
$("input[name=chkSundayday]").change(function () {
    $("input[name=chkSundayDays1]").prop("checked", !chkSundaydayclicked);
    chkSundaydayclicked = !chkSundaydayclicked;
});

//Get Employee Code
function GetEmployeeWeekOff(employee_id) {
    if ($.fn.DataTable.isDataTable('#tblWeekoffAllocation')) {
        $('#tblWeekoffAllocation').DataTable().clear().draw();
    }
    $('#loader').show();

    //  var defaultcompany = localStorage.getItem('new_default_company');
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

                    ],
                    //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    //    return nRow;
                    //},
                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                });
            }

            //if (res.is_fixed_weekly_off == 2) {
            //    $("#dynamics").css("display", "block");
            //}
            //else {
            //    $("#dynamics").css("display", "none");
            //}

            //$("#ddlWeekOff").val(res.is_fixed_weekly_off);


            //$.each(res.shiftWeekOff, function (key, value) {


            //    if (value.week_day == 1) {
            //        $("input[name=chkMonday]").prop("checked", true);
            //        $("input[id=chkMondayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 2) {
            //        $("input[name=chkTuesday]").prop("checked", true);
            //        $("input[id=chkTuesdayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 3) {
            //        $("input[name=chkWednesday]").prop("checked", true);
            //        $("input[id=chkWednesdayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 4) {
            //        $("input[name=chkThursday]").prop("checked", true);
            //        $("input[id=chkThursdayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 5) {
            //        $("input[name=chkFriday]").prop("checked", true);
            //        $("input[id=chkFridayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 6) {
            //        $("input[name=chkSaturday]").prop("checked", true);
            //        $("input[id=chkSaturdayDays" + value.days + "]").prop("checked", true);
            //    }
            //    else if (value.week_day == 7) {
            //        $("input[name=chkSundayday]").prop("checked", true);
            //        $("input[id=chkSundayDays" + value.days + "]").prop("checked", true);
            //    }

            //});

            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();

            messageBox("error", "Server busy please try again later...!");
        }
    });

}


///////////////////////////////////////////////////// END EMPLOYEE WEEK OFF /////////////////////////////////////////