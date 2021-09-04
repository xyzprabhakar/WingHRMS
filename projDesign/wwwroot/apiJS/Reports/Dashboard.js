$('#loader').show();
$(document).ready(function () {
    
    drowChart_();
    $('#loader').hide();
});


function drowChart_() {
    // debugger;
    var DateRange = new Date();
    //DateRange = moment(DateRange).format('MM-DD-YYYY');
    var apiurl = localStorage.getItem("ApiUrl") + 'Attendance/GetAttendanceStatusCount';
    var AttenData = [];
    var LabelN = [];
    var BGColor = [];
    
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        // headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            for (var i = 0; i < res.length; i++) {
                AttenData.push(res[i].totalCount);
                LabelN.push(res[i].statusName);
                BGColor.push(res[i].dayStatus == 1 ? window.chartColors.green : res[i].dayStatus == 2 ? window.chartColors.yellow : res[i].dayStatus == 3 ? window.chartColors.red : '#ddd');
            }

            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: AttenData,
                        backgroundColor: BGColor,
                        label: 'Dataset 1'
                    }],

                    labels: LabelN,
                },
                options: {
                    responsive: true,
                    legend: {
                        position: 'right',
                        fontSize: 18
                    },
                    events: false,
                    animation: {
                        duration: 500,
                        easing: "easeOutQuart",
                        onComplete: function () {
                            var ctx = this.chart.ctx;

                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset) {

                                for (var i = 0; i < dataset.data.length; i++) {
                                 
                                    var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
                                        total = dataset._meta[Object.keys(dataset._meta)[0]].total,
                                        mid_radius = model.innerRadius + (model.outerRadius - model.innerRadius) / 2,
                                        start_angle = model.startAngle,
                                        end_angle = model.endAngle,
                                        mid_angle = start_angle + (end_angle - start_angle) / 2;

                                    var x = mid_radius * Math.cos(mid_angle);
                                    var y = mid_radius * Math.sin(mid_angle);

                                    ctx.fillStyle = '#fff';

                                    if (i == 3) { // Darker text color for lighter background
                                        ctx.fillStyle = '#444';
                                        ctx.fontSize = 18;
                                    }
                                    var percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
                                    ctx.fillText(dataset.data[i], model.x + x, model.y + y);
                                    // Display percent in another line, line break doesn't work for fillText
                                    ctx.fillText(percent, model.x + x, model.y + y + 15);
                                }
                            });
                        }
                    }
                }
            }
            var ctx = document.getElementById('chart-area1').getContext('2d');
            myPie = new Chart(ctx, config);

        },
        error: function (err) {

        }

    });


   

}


