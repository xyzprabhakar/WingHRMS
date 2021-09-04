
$(document).ready(function () {


    //$('.editable-select').editableSelect();
    //var employee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //Bind_PieChart(employee_id);
    //drowChart();
   // var CurrentDate = new Date();
   // CurrentDate = (CurrentDate.getMonth() + 1) + "-" + CurrentDate.getDate() + "-" + CurrentDate.getFullYear();
	//DrowCalenders(CurrentDate);
	DrowClock();
  
	EventSlider();
	fixedNavigation();
	EventSlider2();
    downladData();
    //$('.mobile-view-icon , .mobile-view-icon i').on('click', function () { console.log('click') });

    $('.mobile-view-icon , .mobile-view-icon i').on('click', function (e) {
        e.preventDefault();
        console.log('clik event');
    });
    //AttendenceCalender();
   // headerPOPUP();
});



//start add on 25-07-2019 for  dynamic pie char
function Bind_PieChart(employee_id) {

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetPieChart/" + employee_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            var present = response.total_present;
            var absent = response.total_absent;
            var leave = response.total_leave;
            drowChart(leave,present,absent)
        },
        error: function (err) {
            messageBox("error", err.responseText);
        }
    });
}


function drowChart(leave, present, absent) {
    if (leave == 0 && present == 0 && absent == 0) { present = 1; }

    Morris.Donut({
        element: 'donut-user-chart',
        data: [
            { label: "On Leave", value: leave },
            { label: "Present", value: present },
            { label: "Absent", value: absent }
        ],
        resize: true,
        colors: ['#e3eaef', '#ff679b', '#777edd'],
        labelColor: '#888',
        backgroundColor: 'transparent',
        fillOpacity: 0.1,
        formatter: function (x) { return x + "" }
    });

}


//end on 25-07-2019 for  dynamic pie char
//function drowChart(leave, present, absent)
//{

    
//		var config = {
//			type: 'pie',
//			data: {
//				datasets: [{
//					data: [
//						//0,0,200
//                       leave, present, absent
//					],
//					backgroundColor: [
//						window.chartColors.yellow,
//						window.chartColors.green,
//						window.chartColors.red
//					],
//					label: 'Dataset 1'
//				}],
				
//				labels: [
//					'On Leave',
//					'Persent',
//					'Absent',
//			]
//			},
//			options: {
//			    responsive: true,
//			    legend: {
//			        position: 'right',
//			        fontSize:18
//			    },
//			    events: false,
//			    animation: {
//			    duration: 500,
//			    easing: "easeOutQuart",
//			    onComplete: function () {
//			        var ctx = this.chart.ctx;
			        
//			        ctx.textAlign = 'center';
//			        ctx.textBaseline = 'bottom';
			        
//			        this.data.datasets.forEach(function (dataset) {

//			            for (var i = 0; i < dataset.data.length; i++) {
//			                var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
//                                total = dataset._meta[Object.keys(dataset._meta)[0]].total,
//                                mid_radius = model.innerRadius + (model.outerRadius - model.innerRadius) / 2,
//                                start_angle = model.startAngle,
//                                end_angle = model.endAngle,
//                                mid_angle = start_angle + (end_angle - start_angle) / 2;

//			                var x = mid_radius * Math.cos(mid_angle);
//			                var y = mid_radius * Math.sin(mid_angle);

//			                ctx.fillStyle = '#fff';
			                
//			                if (i == 3) { // Darker text color for lighter background
//			                    ctx.fillStyle = '#444';
//			                    ctx.fontSize = 18;
//                            }
//                           // var percent = "";
//                            //if (dataset.data[i] > 0) {
//                            //    percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
//                            //}
//                            //else {
//                               // percent=;
//                            //}
//			                //var percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
//			                ctx.fillText(dataset.data[i], model.x + x, model.y + y);
//			                // Display percent in another line, line break doesn't work for fillText
//			               // ctx.fillText(percent, model.x + x, model.y + y + 15); // show percentage
//			            }
//			        });
//			    }
//			}
//			}
//		};
//		var ctx = document.getElementById('chart-area').getContext('2d');
//        myPie = new Chart(ctx, config);
       
//}
function DrowCalenders(Cdate)
{
    // debugger;
    var CurrentDate = Cdate;  

    var CalenderDate = CurrentDate.format('YYYY-MM-DD'); //'12-25-2018';
    var EmployeeId = '5';

    //bind employeelist
    BindEmployeeList('ddlemployeelist', 0);
    //$('#ddlemployee').prop('disabled', true);

    var apiurl = localStorage.getItem("ApiUrl") + "Attendance/GetEmployeeMonthalyAtten/" + CalenderDate + "/" + EmployeeId;

    $.ajax({

        type: "GET",
        url: apiurl,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            // debugger;
            if (data != null && data != '') {

                var PayrollCalender = data.payrollCalender;
                var AttSummary = data.attSummary;
                var BalanceLeave = data.balanceLeave;
                var CurrentShift = data.currentShift;
                var HolidayList = data.holidayList;
                SetCalenderData(data);
                AttendenceSummary(AttSummary);
                LeaveBalance(BalanceLeave);
                SetCurrentShift(CurrentShift);
            }
        },
        error: function (err, exception) {
            alert('error found');
            $('#loader').hide();
        }
    });

}
function DrowClock()
{
var clock = $('#clock'),
		alarm = clock.find('.alarm'),
		ampm = clock.find('.ampm');

	// Map digits to their names (this will be an array)
	var digit_to_name = 'zero one two three four five six seven eight nine'.split(' ');

	// This object will hold the digit elements
	var digits = {};

	// Positions for the hours, minutes, and seconds
	var positions = [
		'h1', 'h2', ':', 'm1', 'm2', ':', 's1', 's2'
	];

	// Generate the digits with the needed markup,
	// and add them to the clock

	var digit_holder = clock.find('.digits');

	$.each(positions, function(){

		if(this == ':'){
			digit_holder.append('<div class="dots">');
		}
		else{

			var pos = $('<div>');

			for(var i=1; i<8; i++){
				pos.append('<span class="d' + i + '">');
			}

			// Set the digits as key:value pairs in the digits object
			digits[this] = pos;

			// Add the digit elements to the page
			digit_holder.append(pos);
		}

	});

	// Add the weekday names

	var weekday_names = 'MON TUE WED THU FRI SAT SUN'.split(' '),
		weekday_holder = clock.find('.weekdays');

	$.each(weekday_names, function(){
		weekday_holder.append('<span>' + this + '</span>');
	});

	var weekdays = clock.find('.weekdays span');


	// Run a timer every second and update the clock

	(function update_time(){

		// Use moment.js to output the current time as a string
		// hh is for the hours in 12-hour format,
		// mm - minutes, ss-seconds (all with leading zeroes),
		// d is for day of week and A is for AM/PM

		var now = moment().format("hhmmssdA");

		digits.h1.attr('class', digit_to_name[now[0]]);
		digits.h2.attr('class', digit_to_name[now[1]]);
		digits.m1.attr('class', digit_to_name[now[2]]);
		digits.m2.attr('class', digit_to_name[now[3]]);
		digits.s1.attr('class', digit_to_name[now[4]]);
		digits.s2.attr('class', digit_to_name[now[5]]);

		// The library returns Sunday as the first day of the week.
		// Stupid, I know. Lets shift all the days one position down, 
		// and make Sunday last

		var dow = now[6];
		dow--;
		
		// Sunday!
		if(dow < 0){
			// Make it last
			dow = 6;
		}

		// Mark the active day of the week
		weekdays.removeClass('active').eq(dow).addClass('active');

		// Set the am/pm text:
		ampm.text(now[7]+now[8]);

		// Schedule this function to be run again in 1 sec
		setTimeout(update_time, 1000);

	})();

	// Switch the theme

	$('a.button').click(function(){
		clock.toggleClass('light dark');
	});
}
function EventSlider()
{
   // $('.slider-nav > a').on('click', function (e) { e.preventDefault();});
    var slider = {

        // Not sure if keeping element collections like this
        // together is useful or not.
        el: {
            slider: $("#slider"),
            allSlides: $(".slide2"),
            sliderNav: $(".slider-nav2"),
            allNavButtons: $(".slider-nav2 > a")
        },

        timing: 800,
        slideWidth: 300, // could measure this

        // In this simple example, might just move the
        // binding here to the init function
        init: function () {
            this.bindUIEvents();
        },

        bindUIEvents: function () {
            // You can either manually scroll...
            this.el.slider.on("scroll", function (event) {
                slider.moveSlidePosition(event);
            });
            // ... or click a thing
            this.el.sliderNav.on("click", "a", function (event) {
                slider.handleNavClick(event, this);
            });
            
        },

        moveSlidePosition: function (event) {
            // Magic Numbers =(
            this.el.allSlides.css({
                "background-position": $(event.target).scrollLeft() / 6 - 100 + "px 0"
            });
        },

        handleNavClick: function (event, el) {
            event.preventDefault();
            
            var position = $(el).attr("href").split("-").pop();

            this.el.slider.animate({
                scrollLeft: position * this.slideWidth
            }, this.timing);

            this.changeActiveNav(el);
        },

        changeActiveNav: function (el) {
            this.el.allNavButtons.removeClass("active");
            $(el).addClass("active");
        }

    };

    slider.init();
}
function EventSlider2() {
    // $('.slider-nav > a').on('click', function (e) { e.preventDefault();});
    var slider = {

        // Not sure if keeping element collections like this
        // together is useful or not.
        el: {
            slider: $("#slider2"),
            allSlides: $(".slide1"),
            sliderNav: $(".slider-nav1"),
            allNavButtons: $(".slider-nav1 > a")
        },

        timing: 800,
        slideWidth: 300, // could measure this

        // In this simple example, might just move the
        // binding here to the init function
        init: function () {
            this.bindUIEvents();
        },

        bindUIEvents: function () {
            // You can either manually scroll...
            this.el.slider.on("scroll", function (event) {
                slider.moveSlidePosition(event);
            });
            // ... or click a thing
            this.el.sliderNav.on("click", "a", function (event) {
                slider.handleNavClick(event, this);
            });

        },

        moveSlidePosition: function (event) {
            // Magic Numbers =(
            this.el.allSlides.css({
                "background-position": $(event.target).scrollLeft() / 6 - 100 + "px 0"
            });
        },

        handleNavClick: function (event, el) {
            event.preventDefault();

            var position = $(el).attr("href").split("-").pop();

            this.el.slider.animate({
                scrollLeft: position * this.slideWidth
            }, this.timing);

            this.changeActiveNav(el);
        },

        changeActiveNav: function (el) {
            this.el.allNavButtons.removeClass("active");
            $(el).addClass("active");
        }

    };

    slider.init();
}
function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" btn-primary text-light", "  btn-outline-light");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " btn-primary text-light";

}
function fixedNavigation()
{
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();

        if (scroll >= 300) {
            $(".navigationStop").addClass("scrollBarActive");
        } else {
            $(".navigationStop").removeClass("scrollBarActive");
        }
    });
}
function findSaterday()
{
    function calculate() {
        
        var currentTime = new Date();
        var mon = currentTime.getMonth() + 1;
        var yea = currentTime.getFullYear();
        var dat = 1;
        myDays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"]
        myDate = new Date(eval('"' + dat + ',' + mon + ',' + yea + '"'))
        var first = myDays[myDate.getDay()];
        var secnd;
        var forth;
        switch (first) {
            case "Sunday": sec = "14th," + mon + ',' + yea; forth = "28th," + mon + ',' + yea; break; case "Monday": secnd = "13th," + mon + ',' + yea; forth = "27th," + mon + ',' + yea;
                break; case "Tuesday":
                    secnd = "12th," + mon + ',' + yea; forth = "26th," + mon + ',' + yea; break; case "Wednesday": secnd = "11th," + mon + ',' + yea; forth = "25th," + mon + ',' + yea;
                        break; case "Thursday": secnd = "10th," + mon + ',' + yea; forth = "24th," + mon + ',' + yea; break; case "Friday": secnd = "9th," + mon + ',' + yea; forth = "23rd," + mon + ',' + yea; break; case "Saturday": secnd = "8th," + mon + ',' + yea; forth = "22nd," + mon + ',' + yea; break; default: break;
        }
        document.getElementById('secsat').value = secnd;
        document.getElementById('foursat').value = forth;
    }
    function ctck() {
        var sds = document.getElementById("dum"); if (sds == null) { alert("You are using a free package.\n You are not allowed to remove the tag.\n"); } var sdss = document.getElementById("dumdiv"); if (sdss == null) { alert("You are using a free package.\n You are not allowed to remove the tag.\n"); }
    }
    

}
function downladData()
{
    $('.lanRadio').on('click', function () {
        $('#usbpanl').hide();
        $('#lanpanel').show();
    });
    $('.usbRadio').on('click', function () {

        $('#lanpanel').hide();
        $('#usbpanl').show();
    });


    $('.checkEmploye').on('click', function () {

        var value = $(this).val();
        if (value == 'allemp') { $('.allEmployee-panel').show(); }
        else { $('.allEmployee-panel').hide(); }
    });

   


}
function AttendenceCalender() {
    var HolidayList = [];
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,listWeek'
        },
        defaultDate: new Date(),
        navLinks: true, // can click day/week names to navigate views
        defaultView: 'month',
        allDay: false,
        weekNumbers: false,
        weekNumbersWithinDays: true,
        weekNumberCalculation: 'ISO',
        timeFormat: 'hh:mm A', // uppercase H for 24-hour clock,
        editable: true,
        eventLimit: true, // allow "more" link when too many events
        events: function (start, end, timezone, callback) {
            // debugger;
            var d = $('#calendar').fullCalendar('getDate');
            var CalenderDate = moment(d).format('YYYY-MM-DD'); //'12-25-2018';
            var EmployeeId = '5';
            var apiurl = localStorage.getItem("ApiUrl") + "Attendance/GetEmployeeMonthalyAtten/" + CalenderDate + "/" + EmployeeId;

            $.ajax({

                type: "GET",
                url: apiurl,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (data) {
                   // // debugger;
                    var my_events = [];
                    if (data != null && data != '') {

                        var PayrollCalender = data.payrollCalender;
                        var AttSummary = data.attSummary;
                        var BalanceLeave = data.balanceLeave;
                        var CurrentShift = data.currentShift;
                        var HolidaysLists = data.holidayList;
                        //add data to arrey list
                        while (HolidayList.length > 0) {
                            HolidayList.pop();
                        }
                        $.each(HolidaysLists, function (i, item) {
                            HolidayList.push({
                                holidaydate: item.holidaydate,
                                holidayname: item.holidayname
                            });
                        });

                        //add calender events data
                        $.each(PayrollCalender, function (index, elem) {
                            // debugger;
                            var attdate = GetDateFormat(new Date(elem.attendanceDate));
                            var ind = GetDateFormat(new Date(elem.inTime));
                            var outd = GetDateFormat(new Date(elem.outTime));
                            var intime = ind == '01/01/2000' ? '' : formatAMPM(elem.inTime);
                            var outtime = outd == '01/01/2000' ? '' : formatAMPM(elem.outTime);
                            my_events.push({                               
                                start: attdate,
                                end: attdate,
                                attendanceStatus: elem.attendanceStatus,
                                intime: intime,
                                outtime: outtime,
                                colorCode: elem.colorCode
                            });
                        });
                        callback(my_events);
                    }                 
                       
                    
                }
            });
        },
        eventRender: function (event, eventElement) {
          //  // debugger;
           
            /*
             set calender for employee attendence status                   
             */
            //eventElement.css('background-color', '#fff');
            eventElement.find('.fc-content').hide();
            eventElement.find('.fc-event-container').hide();
            if (event.intime != '') {               
                eventElement.append('<div style="background-color:#007FFF;color:#fff;display: block;font-size:11px;">' + event.intime + '</div>');
                
            }
            if (event.outtime != '') {
                eventElement.append('<div style="background-color:#007FFF;color:#fff;display: block;font-size:11px;">' + event.outtime + '</div>');
            }
            eventElement.append('<div style="background-color:' + event.colorCode + ';font-size:12px;">' + event.attendanceStatus + '</div>');
        },
        dayRender: function (date, cell) {
           // // debugger;
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
                    cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
                }
            }
            //check for sunday
            if (day == 0) {
                idx = cell.index() + 1;
                //cell.css("background-color", "Beige");
                //$(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Beige");
                cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
            }
            //check for holidays
            for (i = 0; i < HolidayList.length; i++) {
                var d = GetDateFormat(new Date(HolidayList[i].holidaydate)).toString().split('/');
                var eventname = HolidayList[i].holidayname;               
                if (Cdate.getMonth() == d[0] - 1 && Cdate.getDate() == d[1] && Cdate.getFullYear() == d[2]) {
                    cell.append('<div style="color:black;background-color:pink;font-size:12px;">' + eventname + '</div>');

                }
            }       
          
            //end here
            //higlight current date
            if (ddate == today) {
                idx = cell.index() + 1;
                cell.css("background-color", "Azure");
                $(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Azure");
            }

        },
        
        eventMouseover: function (calEvent, jsEvent) {


        },

        eventMouseout: function (calEvent, jsEvent) {

        },
        dayClick: function () {
            alert('a day has been clicked!');
        }

    });
}
//below function renamed for not in use
function SetCalenderData_(data) {

    var PayrollCalender = data.payrollCalender;
    var AttSummary = data.attSummary;
    var BalanceLeave = data.balanceLeave;
    var CurrentShift = data.currentShift;
    var HolidayList = data.holidayList;

    var d = new Date();
    var cdates = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    $('#attendancecal').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,listWeek'
        },
        defaultDate: cdates,//,
        navLinks: true, // can click day/week names to navigate views

        weekNumbers: false,
        weekNumbersWithinDays: true,
        weekNumberCalculation: 'ISO',

        editable: false,
        eventLimit: true, // allow "more" link when too many events
        events: function (event,element) {           
            //{
            //    title: '\n Present',// \n 10:05 AM \n 6:45 PM',
            //    start: '2019-01-01T10:05:00',
            //    end: '2019-01-01T18:45:00',
            //    allDay:false

            //},

            //let new_description = '<div class="fc-content"><span class="fc-title ' + event.className + '">' + event.title + '</span>'
            //    + '<span class="fc-time">' + event.start.format('hh:mma') + '-' + event.end.format('hh:mma') + '</span>'
            //    + '<span class="' + event.statusCss + '">' + event.status + '</span></div>';                
            //element.html(new_description);
            
        },        
        timeFormat: 'hh:mm A', // uppercase H for 24-hour clock,

        eventMouseover: function (calEvent, jsEvent) {


        },

        eventMouseout: function (calEvent, jsEvent) {

        },
        dayClick: function (date, jsEvent, view) {
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
           // // debugger;
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
                    cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
                }
            }
            //check for sunday
            if (day == 0) {
                idx = cell.index() + 1;
                //cell.css("background-color", "Beige");
                //$(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Beige");
                cell.append('<div style="background-color:gray;color:#fff;font-size:12px;">Week Off</div>').css("background-color", "Beige");
            }
            //check for holidays
            for (i = 0; i < HolidayList.length; i++) {
                var d = GetDateFormat(new Date(HolidayList[i].holidaydate)).toString().split('/');
                var eventname = HolidayList[i].holidayname;

                if (Cdate.getMonth() == d[0] - 1 && Cdate.getDate() == d[1] && Cdate.getFullYear() == d[2]) {
                    idx = cell.index() + 1;
                    cell.append('<div style="color:black;background-color:pink;font-size:12px;">' + eventname + '</div>');
                   
                }
            }
            /*
             set calender for employee attendence status                   
             */
            if (PayrollCalender != null && PayrollCalender != '' && PayrollCalender != 'undefined') {
                // debugger;
                for (i = 0; i < PayrollCalender.length; i++) {
                    var dt = new Date(PayrollCalender[i].attendanceDate);//mm-dd-yyyy
                    dt = GetDateFormat(dt);
                    dt = dt.toString().split('/');
                    var ind = GetDateFormat(new Date(PayrollCalender[i].inTime));
                    var outd = GetDateFormat(new Date(PayrollCalender[i].outTime));
                    var intime = ind == '01/01/2000' ? '' : formatAMPM(PayrollCalender[i].inTime);
                    var outtime = outd == '01/01/2000' ? '' : formatAMPM(PayrollCalender[i].outTime);
                    if (Cdate.getMonth() == dt[0] - 1 && Cdate.getDate() == dt[1] && Cdate.getFullYear() == dt[2]) {
                        idx = cell.index() + 1;
                        if (day != 0) {
                            if ((week == 1 || week == 2) && day == 6) {

                            }
                            else {
                                if (intime != '') {
                                    cell.append('<div style="background-color:#007FFF;color:#fff;display: block;font-size:11px;">' + intime + '</div>');
                                }
                                if (outtime != '') {
                                    cell.append('<div style="background-color:#007FFF;color:#fff;display: block;font-size:11px;">' + outtime + '</div>');
                                }                                               
                                cell.append('<div style="background-color:' + PayrollCalender[i].colorCode + ';font-size:12px;">' + PayrollCalender[i].attendanceStatus + '</div>');//.attr('style', 'background-color:' + PayrollCalender[i].colorCode + ';font-size:12px;');
                            }
                        }
                    }
                }
            }

           
            //end here
            //higlight current date
            if (ddate == today) {
                idx = cell.index() + 1;
                cell.css("background-color", "Azure");
                $(".fc-time-grid .fc-bg table tbody tr td:nth-child(" + idx + ")").css("background-color", "Azure");
            }
            
        }

    });
}
function AttendenceSummary_(Data) {
  //  // debugger;
    $('#lblPresent').text(Data.present);
    $('#lblAbsent').text(Data.absent);
    $('#lblLateIn').text(Data.lateInEar);
    $('#lblLeave').text(Data.leave);
    $('#lblOutdoor').text(Data.outdoor);
    $('#lblCompoff').text(Data.compoff);
    $('#lblWeeklyOff').text(Data.weeklyoff);
    $('#lblHoliday').text(Data.holidays);
    $('#lblHalfDay').text(Data.halfDay);
    $('#lblfromdate').text(GetDateFormat(new Date(Data.fromDate)));
    $('#lbltodate').text(GetDateFormat(new Date(Data.toDate)));
}
function LeaveBalance_(Data) {

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
    //$('#CasualLeave').val();
    //$('#SickLeave').val();
    //$('#CompoffLeave').val();
    //$('#LWP').val();
}
function SetCurrentShift_(Data) {
  //  // debugger;
    $('#cInTime').text(Data.inTime);
    $('#cGInTime').text(Data.graceInTime);
    $('#cOutTime').text(Data.outTime);
    //$('#cGOutTime').text();
    $('#TShiftHrs').text(Data.totalShiftHrs !=null ? Data.totalShiftHrs + ' Hrs' : Data.totalShiftHrs);
  //  $('#AttStatus').text(Data.attendanceStatus);
    $('#cShiftName').text(Data.currentShiftName);
}
function SetSelectedDateData_(CDate) {
    
    var CalenderDate = CDate;
    var EmployeeId = '5';
    

    var apiurl = "https://localhost:44384/atd/" + 'Attendance/GetSelectedDateAtten/' + CalenderDate + "/" + EmployeeId;

    $.ajax({
        url: apiurl,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            // debugger;
        
          
            if (data != null && data != '') {
                let d = new Date(CDate);
                $('#SelectDate').text(CDate);
               // $('#SelectSName').text(data.shiftName);
                //$('#SelectATimeIn').text(data.actualTimeIn);
                //$('#SelectATimeOut').text(data.actualTimeOut);
                $('#SelectATSHrs').text(data.actualToatlShiftHrs + ' Hrs');
               // $('#SelectAAHrs').text();
                $('#SelectAStatus').text(data.actualAttendanceStatus);

                
            }
            else {
                $('#SelectDate').text('');
               // $('#SelectSName').text('');
                //$('#SelectATimeIn').text('');
                //$('#SelectATimeOut').text('');
                $('#SelectATSHrs').text('');
               // $('#SelectAAHrs').text('');
                $('#SelectAStatus').text('');

              
               
            }
            $('html, body').animate({
                scrollTop: 1000,

            }, 2000);
        },
        error: function (err, exception) {
            alert('error found');
            $('#loader').hide();
        }
    });
}
