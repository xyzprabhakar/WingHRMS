
var default_company;
var login_emp;
var HaveDisplay;
var is_manager;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        BindEPA_FiscallYrByComp('ddlfiscalyr', default_company, 0);
        BindEPA_CycleByComp('ddl_epa_cycle', default_company, 0);

        $("#fianl_reviewdetails").hide();

        $("#ddlcompany").bind("change", function () {

            BindEPA_FiscallYrByComp('ddlfiscalyr', $(this).val(), 0);
            BindEPA_CycleByComp('ddl_epa_cycle', $(this).val(), 0);

        });

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });


        $("#ddlfiscalyr").bind("change", function () {
            $("#loader").show();
            $("#ddl_epa_cycle").val('0');
            $("#emp_epa_bar").empty();
            $("#tblperformancedtl").empty();
            $("#fianl_reviewdetails").hide();

            $("#loader").hide();

        });

        $("#ddl_epa_cycle").bind("change", function () {
            $("#loader").show();
            $("#emp_epa_bar").empty();
            $("#fianl_reviewdetails").hide();
            $("#loader").hide();
        });

    }, 2000);// end timeout

});


$("#btngetdtl").bind("click", function () {
    var companyid = $("#ddlcompany").val();
    var fiscal_yr = $("#ddlfiscalyr").val();
    var cycle_ = $("#ddl_epa_cycle").val();
    var is_error = false;
    var error_msg = "";

    if (companyid == "" || companyid == "0" || companyid == null) {
        is_error = true;
        error_msg = error_msg + "Please select company</br>";
    }

    if (fiscal_yr == "" || fiscal_yr == "0" || fiscal_yr == null) {
        is_error = true;
        error_msg = error_msg + "Please select financial year</br>";
    }
    if (cycle_ == "" || cycle_ == "0" || cycle_ == null) {
        is_error = true;
        error_msg = error_msg + "Please select cycle</br>";
    }

    if (is_error) {
        messageBox("error", error_msg);
        return false;
    }


    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_BarChartDetails/-1/" + fiscal_yr + "/" + cycle_ + "/" + companyid,
        //url: localStorage.getItem("ApiUrl") + "apiePA/Get_BarChartDetails/0/" + login_emp + "/" + fiscal_yr + "/" + cycle_ + "/" + companyid + "/"+ is_manager+,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            var res = response;
            DrawBarChart(res);

            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
});

function DrawBarChart(res) {
    $("#loader").show();
    $("#emp_epa_bar").empty();
    !function ($) {
        "use strict";


        var final_result = [];
        for (var i = 0; i < 6; i++) {
            var _result = {};
            var _total = 0;
            for (var j = 0; j < res.length; j++) {

                if (parseInt(i) == parseInt(res[j].final_review)) {
                    _total = _total + 1;
                }
            }

            _result.total_ = _total;
            _result.statuss = i == 0 ? "Pending" : i == 1 ? "Poor" : i == 2 ? "Need Improvement" : i == 3 ? "Standard" : i == 4 ? "Good" : i == 5 ? "Outstanding" : ""
            _result.labels = _result.statuss + "_" + i;

            final_result.push(_result);

        }



        var MorrisCharts = function () {
        };



        //creates Bar chart
        MorrisCharts.prototype.createBarChart = function (element, final_result, xkey, ykeys, labels, lineColors) {
            Morris.Bar({
                element: element,
                data: final_result,
                xkey: xkey,
                ykeys: ykeys,
                labels: labels,
                gridLineColor: '#eef0f2',
                barSizeRatio: 0.4,
                resize: true,
                hideHover: 'auto',
                barColors: lineColors,
                //barRadius: [5, 5, 0, 0],
            }).on('click', function (i, row) {
                //console.log(i, row);
                Get_Final_reviewdata(i);
            });;
        },

            MorrisCharts.prototype.init = function () {


                var $morrisData = [];
                // for x and y axis values
                $.each(final_result, function (valuee, namee) {
                    $morrisData.push({ x: namee.statuss, y: namee.total_, labels: namee.labels });
                });
                this.createBarChart('emp_epa_bar', $morrisData, 'x', ['y'], ['Total Employee'], ['#44a2d2']);
            },
            //init
            $.MorrisCharts = new MorrisCharts, $.MorrisCharts.Constructor = MorrisCharts
    }(window.jQuery),



        //initializing 
        function ($) {
            "use strict";
            $.MorrisCharts.init();
        }(window.jQuery);
    $("#loader").hide();
}



function Get_Final_reviewdata(idd) {

    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_BarChartDetails/" + idd + "/" + $("#ddlfiscalyr").val() + "/" + $("#ddl_epa_cycle").val() + "/" + $("#ddlcompany").val(),
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            $("#fianl_reviewdetails").show();
            var res = response;

            $("#tblperformancedtl").DataTable({
                "processing": true,//for showing process bar
                "serverSide": false,//for process server side
                "orderMulti": false,
                "bDestroy": true,//for remove previous detail
                "scrollX": 200,
                "filter": true,//for enable serach box
                "aaData": res,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "desig_name", "name": "desig_name", "title": "Designation", "autoWidth": true },
                    { "data": "rm1_name", "name": "rm1_name", "title": "Reporting Manager 1", "autoWidth": true },
                    { "data": "rm2_name", "name": "rm2_name", "title": "Reporting Manager 2", "autoWidth": true },
                    { "data": "rm3_name", "name": "rm3_name", "title": "Reporting Manager 3", "autoWidth": true },
                    {
                        "title": "Score", "autoWidth": true, "render": function (data, type, full, meta) {
                            return 'Total Socre:- ' + full.total_score + ', Get Score:- ' + full.get_score + '';
                        }
                    },
                    { "data": "final_remarks", "name": "final_remarks", "title": "Final Remarks", "autoWidth": true },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            });
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });

}
