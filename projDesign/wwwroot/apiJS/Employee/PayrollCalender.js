
var employee_id;
var emp_role_id;
var default_company;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        employee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        HaveDisplay = 1;
        DrowCalender();

        GetEmployeeInAndOutTime(employee_id);
        Bind_PendingRequestDtl(employee_id, default_company, emp_role_id);

        $('#ddlemployeelist').bind('change', function () {
            Reset_all();

            $('#SelectDate').text('');
            // $('#SelectSName').text('');
            // $('#SelectATimeIn').text('');
            // $('#SelectATimeOut').text('');
            $('#SelectATSHrs').text('');
            // $('#SelectAAHrs').text('');
            $('#SelectAStatus').text('');
            $('#lblmanager1').text('');
            $('#lbl_my_in_time').text('00:00');

            $('#lbl_my_out_time').text('00:00');

            if ($(this).val() == null || $(this).val() == "" || $(this).val() == "0" || $(this).val() < 0) {
                messageBox("error", "Please select employee");
                return false;
            }

            $('#loader').show();
            var emp = $(this).val();
            //SetCalenderData(emp);
            var for_all_emp = 0;

            if (employee_id == 0) {
                for_all_emp = 1;
                employee_id = $('#ddlemployeelist option').last().val();
            }


            var d = $('#attendancecal').fullCalendar('getDate');
            var CalenderDate = moment(d).format('YYYY-MM-DD'); //'12-25-2018';

            var apiurl = localStorage.getItem("ApiUrl") + 'Attendance/GetEmployeeMonthalyAtten/' + CalenderDate + "/" + emp + '/' + for_all_emp + '/' + 1;
            var my_events = [];
            $.ajax({
                type: "GET",
                contentType: "application/json",
                data: {},
                url: apiurl,
                dataType: "json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (data) {
                    // // debugger;

                    if (data.statusCode != undefined) {
                        messageBox("info", data.message);
                        $('#loader').hide();
                        return false;
                    }

                    if (data != null && data != '') {

                        var PayrollCalender = data.payrollCalender;
                        var AttSummary = data.attSummary;
                        var BalanceLeave = data.balanceLeave;
                        var CurrentShift = data.currentShift;
                        var HolidaysLists = data.holidayList;


                        //load other data
                        AttendenceSummary(AttSummary);
                        LeaveBalance(BalanceLeave);
                        SetCurrentShift(CurrentShift);

                        //add calender events data
                        $.each(PayrollCalender, function (index, elem) {
                            $('#loader').show();
                            var attdate = GetDateFormat(new Date(elem.attendanceDate));
                            var ind = GetDateFormat(new Date(elem.inTime));
                            var outd = GetDateFormat(new Date(elem.outTime));
                            var intime = ind == '01/01/2000' ? '' : formatAMPM(elem.inTime);
                            var outtime = outd == '01/01/2000' ? '' : formatAMPM(elem.outTime);

                            my_events.push(
                                {
                                    start: attdate,
                                    end: attdate,
                                    attendanceStatus: elem.attendanceStatus,
                                    intime: intime,
                                    outtime: outtime,
                                    colorCode: elem.colorCode,
                                    isweekoff: elem.isWeekOff,
                                    weekoffdate: elem.weekOffDate,
                                    isholiday: elem.isHoliday,
                                    holidayname: elem.holidayName,
                                    holidaydate: elem.holidayDate,
                                    attstatus: elem.attStatus,
                                    leave_request_id: elem.leave_request_id,
                                    is_outdoor: elem.is_outdoor,
                                    is_comp_off: elem.is_comp_off,
                                    is_leave_aprvd: elem.is_leave_aprvd,
                                    is_outdoor_aprvd: elem.is_outdoor_aprvd,
                                    is_shl: elem.is_shl,
                                    is_comp_off_apprvd:elem.is_comp_off_apprvd,
                                });
                        });


                        $('#attendancecal').fullCalendar('removeEvents');
                        $('#attendancecal').fullCalendar('addEventSource', my_events);
                        $('#attendancecal').fullCalendar('rerenderEvents');

                        $('#loader').hide();
                    }

                    $('#loader').hide();
                }
            });


            emp_manager_($(this).val(), default_company);
        });

    }, 2000);// end timeout
});


function DrowCalender() {
    $('#loader').show();
    // // debugger;
    var listapi = localStorage.getItem("ApiUrl");
    //bind employeelist
    //var emp = localStorage.getItem("emp_id");

    var key = CryptoJS.enc.Base64.parse("#base64Key#");
    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";


    SetCalenderData();

    BindAllEmployeeUnderEmp_cal('ddlemployeelist', default_company, employee_id, 0);
    //BindEmployeeListUnderLoginEmpFromAllComp('ddlemployeelist', default_company, employee_id, 0);
    setSelect('ddlemployeelist', employee_id);
    //$("#ddlemployeelist option:selected").val(employee_id);
    //$("#ddlemployeelist").trigger("select2:updated");
    //$("#ddlemployeelist").select2();

    emp_manager_(employee_id, default_company);

    // BindEmployeeList1('ddlemployeelist', employee_id);
    //function BindEmployeeList1(ControlId, SelectedVal) {

    //    ControlId = '#' + ControlId;
    //    $.ajax({
    //        type: "GET",
    //        //url: listapi + "apiMasters/Get_EmployeeHeadList",
    //        url: listapi + "apiMasters/Get_Employee_Under_LoginEmp/" + employee_id,
    //        data: {},
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
    //        success: function (response) {
    //            var res = response;
    //            $(ControlId).empty().append('<option selected="selected" value=' + employee_id + '>' + login_name_code + '</option>');
    //            //$(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
    //            $.each(res, function (data, value) {
    //                // $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code1));

    //                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
    //            })
    //            //get and set selected value
    //            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
    //                $(ControlId).val(SelectedVal);
    //                // $(ControlId).prop('disabled', true);
    //            }

    //            $('#loader').hide();

    //            $(ControlId).trigger("select2:updated");
    //            $(ControlId).select2();

    //        },
    //        error: function (err) {
    //            alert(err.responseText);
    //            $('#loader').hide();
    //        }
    //    });
    //}


}

function getmonthname(monthno) {
    var month = new Array();
    month[0] = "January";
    month[1] = "February";
    month[2] = "March";
    month[3] = "April";
    month[4] = "May";
    month[5] = "June";
    month[6] = "July";
    month[7] = "August";
    month[8] = "September";
    month[9] = "October";
    month[10] = "November";
    month[11] = "December";
    var n = month[monthno];
    return n;
}
var dd = new Date();
var currentmonthyear = getmonthname(dd.getMonth()).toString() + " " + dd.getFullYear().toString();
//alert(currentmonthyear);
function SetCalenderData() {

    $('#loader').show();
    var d = new Date();
    var cdates = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    //  var cdates1 = d.getFullYear() + "-" + (d.getMonth() + 2) + "-" + d.getDate();
    var for_all_emp = 0;

    if (employee_id == 0) {
        for_all_emp = 1;
        employee_id = $('#ddlemployeelist option').last().val();
    }
    debugger;


    $('#attendancecal').fullCalendar({


        header: {
            left: 'prev,next, today',
            // center: 'agendaDay,agendaWeek,month',
            right: 'title'
        },
        contentHeight: 800,
        defaultDate: currentmonthyear.toString(),//,
        navLinks: true, // can click day/week names to navigate views
        defaultView: 'month',
        allDay: false,
        weekNumbers: false,
        weekNumbersWithinDays: true,
        weekNumberCalculation: 'ISO',
        timeFormat: 'hh:mm A', // uppercase H for 24-hour clock,
        editable: false,
        eventLimit: true, // allow "more" link when too many events
        events: function (start, end, timezone, callback) {
            // debugger;;

            $('#loader').show();

            var d = $('#attendancecal').fullCalendar('getDate');

            var CalenderDate = moment(d).format('YYYY-MM-DD'); //'12-25-2018';

            //var EmployeeId = localStorage.getItem("emp_id");
            EmployeeId = $('#ddlemployeelist').val() == null || $('#ddlemployeelist').val() == '0' ? employee_id : $('#ddlemployeelist').val();
            var apiurl = localStorage.getItem("ApiUrl") + "Attendance/GetEmployeeMonthalyAtten/" + CalenderDate + "/" + EmployeeId + '/' + for_all_emp + '/' + 1;

            $.ajax({
                type: "GET",
                contentType: "application/json",
                data: {},
                url: apiurl,
                dataType: "json",
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (data) {
                    //   // debugger;

                    if (data.statusCode != undefined) {
                        messageBox("info", data.message);
                        $('#loader').hide();
                        return false;
                    }

                    var my_events = [];
                    var my_wh = [];
                    if (data != null && data != '') {

                        var PayrollCalender = data.payrollCalender;
                        var AttSummary = data.attSummary;
                        var BalanceLeave = data.balanceLeave;
                        var CurrentShift = data.currentShift;
                        var HolidaysLists = data.holidayList;

                        //load other data
                        AttendenceSummary(AttSummary);
                        LeaveBalance(BalanceLeave);
                        SetCurrentShift(CurrentShift);

                        //add calender events data
                        $.each(PayrollCalender, function (index, elem) {
                            $('#loader').show();
                            var attdate = GetDateFormat(new Date(elem.attendanceDate));
                            var ind = GetDateFormat(new Date(elem.inTime));
                            var outd = GetDateFormat(new Date(elem.outTime));
                            var intime = ind == '01/01/2000' ? '' : formatAMPM(elem.inTime);
                            var outtime = outd == '01/01/2000' ? '' : formatAMPM(elem.outTime);
                            my_events.push(
                                {
                                    start: attdate,
                                    end: attdate,
                                    attendanceStatus: elem.attendanceStatus,
                                    intime: intime,
                                    outtime: outtime,
                                    colorCode: elem.colorCode,
                                    isweekoff: elem.isWeekOff,
                                    weekoffdate: elem.weekOffDate,
                                    isholiday: elem.isHoliday,
                                    holidayname: elem.holidayName,
                                    holidaydate: elem.holidayDate,
                                    attstatus: elem.attStatus,
                                    leave_request_id: elem.leave_request_id,
                                    is_outdoor: elem.is_outdoor,
                                    is_comp_off: elem.is_comp_off,
                                    is_leave_aprvd: elem.is_leave_aprvd,
                                    is_outdoor_aprvd: elem.is_outdoor_aprvd,
                                    is_shl: elem.is_shl,
                                    is_comp_off_apprvd:elem.is_comp_off_apprvd,
                                });
                            $('#loader').hide();
                        });


                        $('#attendancecal').fullCalendar('removeEvents');
                        $('#attendancecal').fullCalendar('rerenderEvents');
                        callback(my_events);

                        $('#loader').hide();
                    }


                    $('#loader').hide();
                }
            });

        },
        eventRender: function (event, eventElement) {
            //  //// debugger;;
            $('#loader').show();
            //set calender for employee attendence status
            //eventElement.css('background-color', '#fff');
            eventElement.find('.fc-content').hide();
            eventElement.find('.fc-event-container').hide();


           // if (event.intime != '') {
              //  eventElement.append('<div class=fc-content><span class=fc-time>' + event.intime + '</span></div>');

           // }
           // if (event.outtime != '') {
               // eventElement.append('<div class=fc-content><span class=fc-time>' + event.outtime + '</span></div>');
           // }

            if ((event.isholiday == 1 || event.isweekoff==1) && event.is_comp_off_apprvd==1) { // check if compoff is approved and day is holiday or weekday
                if (event.intime != '') {
                    eventElement.append('<div class=fc-content><span class=fc-time>' + event.intime + '</span></div>');

                }
                if (event.outtime != '') {
                    eventElement.append('<div class=fc-content><span class=fc-time>' + event.outtime + '</span></div>');
                }
            }
            else if(event.isholiday == 0 && event.isweekoff==0)
            {
                 if (event.intime != '') {
                    eventElement.append('<div class=fc-content><span class=fc-time>' + event.intime + '</span></div>');

                }
                if (event.outtime != '') {
                    eventElement.append('<div class=fc-content><span class=fc-time>' + event.outtime + '</span></div>');
                }
            }

            if (event.isholiday == 1) {

                //eventElement.append('<div style="background-color:pink;font-size:12px;">' + event.holidayname + '</div>');
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color:' + event.colorCode + '; display: block;padding: 5px;margin-bottom: 3px;">' + event.holidayname + '</span></div>');
            }

            if (event.isweekoff == 1) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(241,239,239);color: #4C5650;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Week Off</span></div>');
            }

            // if ((event.attstatus == 1 || event.attstatus == 5) && event.is_shl == 0) {
            if ((event.attstatus == 1 || event.attstatus == 5)) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(10, 207, 151, 0.18);color: #059a70;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Present</span></div>');
            }
            if (event.is_comp_off == 1) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(255,228,237);color: #FFA7A6;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">CompOff</span></div>');
            }

            // if (event.attstatus == 2 && event.leave_request_id == 0 && event.isholiday != 1 && event.isweekoff != 1) {
            if (event.attstatus == 2) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(241, 85, 108, 0.18);color: #f1556c;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Absent</span></div>');
            }

            //if (event.attstatus == 4 || event.attstatus == 6 || event.is_shl == 1) {
            if (event.attstatus == 4 || event.attstatus == 6) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color:rgb(249,217,198);color: #985C38;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Half Day</span></div>');
            }
            if (event.is_outdoor == 1 || event.is_outdoor_aprvd == 1) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(230,232,249);color: #2d7bf4;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Outdoor</span></div>');
            }
            if (event.attstatus == 3 || event.leave_request_id > 0 || event.is_leave_aprvd == 1) {
                eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(45, 123, 244, 0.18);color: #2d7bf4;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;margin-top: 5px;">On Leave</span></div>');
            }

            //if (event.isholiday == 1) {
            //    debugger;
            //    //eventElement.append('<div style="background-color:pink;font-size:12px;">' + event.holidayname + '</div>');
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color:' + event.colorCode + '; display: block;padding: 5px;margin-bottom: 3px;">' + event.holidayname + '</span></div>');
            //}
            //if (event.isweekoff == 1) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(241,239,239);color: #4C5650;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Week Off</span></div>');
            //}

            //// if ((event.attstatus == 1 || event.attstatus == 5) && event.is_shl == 0) {
            //if ((event.attstatus == 1 || event.attstatus == 5)) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(10, 207, 151, 0.18);color: #059a70;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Present</span></div>');
            //}
            //if (event.is_comp_off == 1) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(255,228,237);color: #FFA7A6;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">CompOff</span></div>');
            //}

            //if (event.attstatus == 2 && event.leave_request_id == 0 && event.isholiday != 1 && event.isweekoff != 1) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(241, 85, 108, 0.18);color: #f1556c;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Absent</span></div>');
            //}
            ////if (event.attstatus == 4 || event.attstatus == 6 || event.is_shl == 1) {
            //if (event.attstatus == 4 || event.attstatus == 6) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color:rgb(249,217,198);color: #985C38;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Half Day</span></div>');
            //}
            //if (event.is_outdoor == 1 || event.is_outdoor_aprvd == 1) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgb(230,232,249);color: #2d7bf4;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;    margin-top: 5px;">Outdoor</span></div>');
            //}
            //if (event.attstatus == 3 || event.leave_request_id > 0 || event.is_leave_aprvd == 1) {
            //    eventElement.append('<div class=fc-content><span style="font-weight:bold;background-color: rgba(45, 123, 244, 0.18);color: #2d7bf4;font-size:10px;display: block;padding: 5px;margin-bottom: 3px;margin-top: 5px;">On Leave</span></div>');
            //}


            $('#loader').hide();
        },

        eventMouseover: function (calEvent, jsEvent) {


        },

        eventMouseout: function (calEvent, jsEvent) {

        },
        dayClick: function (date, jsEvent, view) {
            date = new Date(date);
            var ClickedDate = date.getMonth() + 1 + "-" + date.getDate() + "-" + date.getFullYear();
            SetSelectedDateData(ClickedDate);
        },
        eventClick: function (calEvent, jsEvent) {
            // // debugger;
            var date = calEvent.start.format();
            date = new Date(date);
            var ClickedDate = date.getMonth() + 1 + "-" + date.getDate() + "-" + date.getFullYear();
            SetSelectedDateData(ClickedDate);
        },
        viewRender: function (view, element) {//calender next/prev button click event            
            var b = $('#attendancecal').fullCalendar('getDate');

        },
        loading: function (bool) {
            if (bool) $('#loading').show();
            else $('#loading').hide();
        },
        dayRender: function (date, cell) {
            //// debugger;;
            $('#loader').show();
            var idx = null;
            var today = new Date().toDateString();
            var ddate = date.toDate().toDateString();
            //calculate week offs
            var Cdate = new Date(date);
            var day = Cdate.getDay(), Sunday = 0, Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6;
            var week = 0 | Cdate.getDate() / 7 //get the week

            //check if it's second week or third week --0 first week,1 second,2 third,3 fourth week
            if (week == 1 || week == 2) {
                if (day == 6) { //check for satruday
                    idx = cell.index() + 1;
                    // cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
                }
            }
            //check for sunday
            if (day == 0) {
                idx = cell.index() + 1;
                //cell.css("background-color", "Beige");
                //$(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Beige");
                //cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
            }
            //check for holidays
            //for (i = 0; i < HolidayList.length; i++) {
            //    var d = GetDateFormat(new Date(HolidayList[i].holidaydate)).toString().split('/');
            //    var eventname = HolidayList[i].holidayname;

            //    if (Cdate.getMonth() == d[0] - 1 && Cdate.getDate() == d[1] && Cdate.getFullYear() == d[2]) {
            //        idx = cell.index() + 1;
            //        cell.append('<div style="color:black;background-color:pink;font-size:12px;">' + eventname + '</div>');

            //    }
            //}

            //higlight current date
            if (ddate == today) {
                idx = cell.index() + 1;
                cell.css("background-color", "Azure");
                $(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Azure");
            }

            $('#loader').hide();

        }

    });

    $('#loader').hide();
}

function AttendenceSummary(Data) {
    //// debugger;;
    $('#loader').show();
    $('#lblPresent').text(Data.present);
    $('#lblAbsent').text(Data.absent);
    $('#lblLateIn').text(Data.lateInEar);
    $('#lblLeave').text(Data.leave);
    $('#lblOutdoor').text(Data.outdoor);
    $('#lblCompoff').text(Data.compoff);
    $('#lblWeeklyOff').text(Data.weeklyOff);
    $('#lblHoliday').text(Data.holidays);
    $('#lblHalfDay').text(Data.halfDay);
    $('#lblfromdate').text(GetDateFormat(new Date(Data.fromDate)));
    $('#lbltodate').text(GetDateFormat(new Date(Data.toDate)));
    $('#loader').hide();
}

function LeaveBalance(Data) {
    $('#loader').show();
    $("#tbleavebalance").DataTable({
        "bJQueryUI": true,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": false,
        "bDestroy": true,
        //"scrollY": 200,
        "aaData": Data,
        "columnDefs":
            [
                {
                    targets: [1],
                    render: $.fn.dataTable.render.number(',', '.', 2)
                }
            ],

        "columns": [
            { "data": "leaveInfoName", "name": "leaveInfoName", "autoWidth": true },
            { "data": "leaveBalance", "name": "leaveBalance", "autoWidth": true },



        ],
        "fnInitComplete": function (oSettings, json) {
            // Find the wrapper and remove all thead                
            $('#tbleavebalance').parents('.dataTables_wrapper').first().find('thead').hide();
        }


    });
    $('#tbleavebalance_wrapper').attr('style', 'margin-top:-7px;');

    $('#loader').hide();
    //$('#CasualLeave').val();
    //$('#SickLeave').val();
    //$('#CompoffLeave').val();
    //$('#LWP').val();
}

function SetCurrentShift(Data) {
    //// debugger;;
    $('#loader').show();
    $('#cInTime').text(Data.inTime);
    $('#cGInTime').text(Data.graceInTime);
    $('#cOutTime').text(Data.outTime);
    // $('#cGOutTime').text();
    $('#TShiftHrs').text(Data.totalShiftHrs != null ? Data.totalShiftHrs + ' Hrs' : Data.totalShiftHrs);
    // $('#AttStatus').text(Data.attendanceStatus);
    $('#cShiftName').text(Data.currentShiftName);
    $('#loader').hide();
}

function SetSelectedDateData(CDate) {
    // //
    $('#loader').show();
    var CalenderDate = CDate;
    //var EmployeeId = localStorage.getItem('emp_id');


    var apiurl = localStorage.getItem("ApiUrl") + 'Attendance/GetSelectedDateAtten/' + CalenderDate + "/" + $("#ddlemployeelist").val();//employee_id;

    $.ajax({
        url: apiurl,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            ////


            if (data != null && data != '') {
                let d = new Date(CDate);
                $('#SelectDate').text(CDate);
                // $('#SelectSName').text(data.shiftName);
                // $('#SelectATimeIn').text(data.actualTimeIn);
                // $('#SelectATimeOut').text(data.actualTimeOut);
                $('#SelectATSHrs').text(data.actualToatlShiftHrs + ' Hrs');
                //$('#SelectAAHrs').text();
                //$('#SelectAAHrs').text(data.actualToatlShiftHrs + ' Hrs');
                $('#SelectAStatus').text(data.actualAttendanceStatus);

                $('#lbl_my_in_time').text(data.actualTimeIn);

                $('#lbl_my_out_time').text(data.actualTimeOut);

                if (data.leave_d_per_day_list != null && data.leave_d_per_day_list.length > 0) {
                    $("#tbl_attendance_details").empty();
                    $("#tbl_attendance_details").append('<tr><th>Leave Type</th><th>Duration</th><th>From Date</th><th>To Date</th><th>Day Part</th><th>Remakrs</th><th>Status</th></tr>');
                    $("#tbl_attendance_details").append('<tr><td>&nbsp;</td></tr>');
                    for (var i = 0; i < data.leave_d_per_day_list.length; i++) {
                        $("#tbl_attendance_details").append('<tr><td>' + data.leave_d_per_day_list[i].leavetype + '</td><td>' + data.leave_d_per_day_list[i].duration + '</td><td>' + data.leave_d_per_day_list[i].fromdate + '</td><td>' + data.leave_d_per_day_list[i].todate + '</td><td>' + data.leave_d_per_day_list[i].partialdays + '</td><td>' + data.leave_d_per_day_list[i].reason + '</td><td>' + data.leave_d_per_day_list[i].attendance_status + '</td></tr>');
                    }
                    // $("#popupopp_attendnce_details").show();
                    $("#popupopp_attendnce_details").show();
                    var modal = document.getElementById("popupopp_attendnce_details");
                    modal.style.display = "block";

                    $('#popupopp_attendnce_details').dialog({
                        modal: 'true',
                        title: 'Leave Details'
                    });

                }
                if (data.outdoor_details_list != null && data.outdoor_details_list.length > 0) {
                    $("#tbl_outdoor_details").empty();
                    $("#tbl_outdoor_details").append('<tr><th>Outdoor Date</th><th>In Time</th><th>Out Time</th><th>Reason</th><th>Status</th><th> Remarks </th></tr>');
                    $("#tbl_outdoor_details").append('<tr><td>&nbsp;</td></tr>');
                    for (var i = 0; i < data.outdoor_details_list.length; i++) {
                        $("#tbl_outdoor_details").append('<tr><td>' + data.outdoor_details_list[i].outdoor_date + '</td><td>' + data.outdoor_details_list[i].in_time + '</td><td>' + data.outdoor_details_list[i].out_time + '</td><td>' + data.outdoor_details_list[i].user_reason + '</td><td>' + data.outdoor_details_list[i].outdoor_status + '</td><td>' + data.outdoor_details_list[i].aprv_remarks + '</td></tr>');
                    }
                    // $("#popupopp_attendnce_details").show();
                    $("#popupopp_outdoor_details").show();
                    var modal = document.getElementById("popupopp_outdoor_details");
                    modal.style.display = "block";

                    $('#popupopp_outdoor_details').dialog({
                        modal: 'true',
                        title: 'Outdoor Details'
                    });

                }

            }
            else {
                $('#SelectDate').text('');
                // $('#SelectSName').text('');
                //  $('#SelectATimeIn').text('');
                //  $('#SelectATimeOut').text('');
                $('#SelectATSHrs').text('');
                // $('#SelectAAHrs').text('');
                $('#SelectAStatus').text('');

                $('#lbl_my_in_time').text('00:00');

                $('#lbl_my_out_time').text('00:00');


            }



            $('#loader').hide();
        },
        error: function (err, exception) {
            alert('error found');
            $('#loader').hide();
        }
    });
}

function GetEmployeeInAndOutTime(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeInAdnOutTime/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#lbl_my_in_time').text("00:00");
                $('#lbl_my_out_time').text("00:00");
                $("#loader").hide();
                return false;
            }

            $('#lbl_my_in_time').text(res.in_time);
            if (res.in_time == "01-01-2000 12:00 AM") {
                $('#lbl_my_in_time').text("00:00");
            }

            $('#lbl_my_out_time').text(res.out_time);
            if (res.out_time == "01-01-2000 12:00 AM") {
                $('#lbl_my_out_time').text("00:00");
            }

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });

}


function emp_manager_(employee_id, companyidd) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetEmployeeManagers/" + employee_id + "/" + companyidd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            if (data.statusCode != undefined) {
                messageBox("info", data.message);
                $("#loader").hide();
                return;
            }

            //$("#div_left_user").show();
            //$("#div_chart").hide();
            //$("#divpendingRequest").hide();
            //$("#div_show_manager").show();
            //$("#div_show_event_notification").hide();
            //debugger;
            var res = data;

            if (res != null && res.length > 0) {
                $('#lblmanager1').text('RM 1 - ' + (res[0].manager_name_code == null || res[0].manager_name_code == '' ? '' : res[0].manager_name_code));
                if (res[0].final_approval == 1) {
                    $('#lblmanager1_is_final').text(': Final Approval');
                }
                else if (res[0].final_approval == 2) {
                    $('#lblmanager2_is_final').text(': Final Approval');
                }
                else if (res[0].final_approval == 3) {
                    $('#lblmanager3_is_final').text(': Final Approval');
                }

                if (res[0].m_two_name_code != "" && res[0].m_two_name_code != null) {
                    $('#lblmanager2').text('RM 2 - ' + res[0].m_two_name_code);
                }

                if (res[0].m_three_name_code != "" && res[0].m_three_name_code != null) {
                    $('#lblmanager3').text('RM 3 - ' + res[0].m_three_name_code);
                }
            }
            else {
                $('#lblmanager1').text('');
                $('#lblmanager2').text('');
                $('#lblmanager3').text('');
                $('#lblmanager1_is_final').text('');
                $('#lblmanager2_is_final').text('');
                $('#lblmanager3_is_final').text('');
            }
            //$('#pTest').text('test')

            $('#loader').hide();

        },
        error: function (err, exception) {
            //alert(err);
            $('#loader').hide();
            window.localStorage.removeItem("Token");
            // alert('Your session has expired please login again...!');
            window.location.href = "/Login";
            return;
        }
    });
}

function Bind_PendingRequestDtl(loginempid, company_idd, emp_role_id) {

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetAll_PendingRequetsCount/" + loginempid + "/" + company_idd + "/" + emp_role_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;

            $('#lblleaver_rqst').html('<i class="fas fa-calendar-alt"></i>' + ' ' + (response != null && response != "" ? response.leave_pending : ''));
            $('#lblattandance_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.attendance_pending : ''));
            $('#lbloutdoor_rqst').html('<i class="fa fa-history"></i>' + ' ' + (response != null && response != "" ? response.outdoor_pending : ''));
            $('#lblcompoff_rqst').html('<i class="fas fa-recycle"></i>' + ' ' + (response != null && response != "" ? response.compoff_pending : ''));
            $('#lblloan_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.total_loan_request : ''));
            // $('#lblreimbursment_rqst').html('<i class="fas fa-clock"></i>' + response.total_reimbursment_reqst);
            $('#lblasset_rqst').html('<i class="fas fa-clock"></i>' + ' ' + (response != null && response != "" ? response.asset_request : ''));

            // $("#lblleaver_rqst").text(response.leave_pending);
        },
        error: function (response) {
            //alert('Your session has expired please login again...!');
            window.location.href = "/Login";
            return false;
        }
    });
}
function Reset_all() {
    $('#loader').show();
    $('#SelectDate').text('');
    // $('#SelectSName').text('');
    // $('#SelectATimeIn').text('');
    // $('#SelectATimeOut').text('');
    $('#SelectATSHrs').text('');
    // $('#SelectAAHrs').text('');
    $('#SelectAStatus').text('');
    $('#lblmanager1').text('');
    $('#lbl_my_in_time').text('00:00');

    $('#lbl_my_out_time').text('00:00');

    $("#lbl_my_in_time").text('');
    $("#lbl_my_out_time").text('');
    $("#cInTime").text('');
    $("#cGInTime").text('');
    $("#cOutTime").text('');
    $("#TShiftHrs").text('');
    //$("#SelectATimeIn").text('');
    //$("#SelectATimeOut").text('');
    $("#SelectATSHrs").text('');
    $("#SelectAStatus").text('');
    $("#lblPresent").text('');
    $("#lblAbsent").text('');
    $("#lblLateIn").text('');
    $("#lblLeave").text('');
    $("#lblOutdoor").text('');
    $("#lblCompoff").text('');
    $("#lblWeeklyOff").text('');
    $("#lblHoliday").text('');
    $("#lblHalfDay").text('');

    if ($.fn.DataTable.isDataTable('#tbleavebalance')) {
        $('#tbleavebalance').DataTable().clear().draw();
    }

    $("#lblleaver_rqst").text('');
    $("#lblattandance_rqst").text('');
    $("#lbloutdoor_rqst").text('');
    $("#lblcompoff_rqst").text('');
    $("#lblloan_rqst").text('');
    $("#lblasset_rqst").text('');
    $("#lblmanager1").text('');
    $("#lblmanager1_is_final").text('');
    $("#lblmanager2").text('');
    $("#lblmanager2_is_final").text('');
    $("#lblmanager3").text('');
    $("#lblmanager3_is_final").text('');

    $('#loader').hide();
}