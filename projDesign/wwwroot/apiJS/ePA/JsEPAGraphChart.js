
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
        // HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', default_company, login_emp, 0);
        setSelect('ddlemployee', login_emp);


        $("#graphdiv").css("display", "none");


        $("#ddlcompany").bind("change", function () {

            BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', default_company, login_emp, 0);
            setSelect('ddlemployee', login_emp);
            $("#graphdiv").css("display", "none");
            $("#emp_epa_line_graph").empty();

        });

        $("#ddlemployee").bind("change", function () {

            $("#graphdiv").css("display", "none");
            $("#emp_epa_line_graph").empty();

        });

        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });

        $("#btngetdtl").bind("click", function () {
            var is_error = false;
            var error_msg = "";
            if ($("#ddlcompany").val() == "0" || $("#ddlcompany").val() == "" || $("#ddlcompany").val() == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }

            if ($("#ddlemployee").val() == null || $("#ddlemployee").val() == "" || $("#ddlemployee").val() == "0") {
                is_error = true;
                error_msg = error_msg + "Please select employee</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            GetData();

        });

    }, 2000);// end timeout

});



function GetData() {
    var for_all_emp = 0;
    var emp_id = $("#ddlemployee").val();

    if (emp_id == 0) {
        for_all_emp = 1;
        emp_id = $('#ddlemployee option').last().val();
    }
    else {

        emp_id = login_emp;
    }
    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_EmpEPAScoreDetails/" + $("#ddlcompany").val() + "/" + $("#ddlemployee").val() + "/" + for_all_emp,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            $("#loader").hide();
            var res = response;
            if (res.statusCode != undefined && res.statusCode != null) {
                messageBox("error", res.message);
                return false;
            }
            else if (res.length == "0") {
                messageBox("error", "Sorry no record available");
                return false;
            }

            EmpLineGraph(res);


        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}
function EmpLineGraph(res) {
    $("#loader").show();
    $("#graphdiv").css("display", "block");
    $("#emp_epa_line_graph").empty();


    !function ($) {
        "use strict";


        var final_result = [];
        for (var i = res.length - 1; i >= 0; i--) {

            var result = {};
            result.xvalues = (res[i].financial_year_name + "/" + res[i].cycle_name).toString();
            result.total_score = res[i].total_score;
            result.emp_id_ = res[i].emp_id;
            result.get_score = res[i].get_score;

            final_result.push(result);
        }


        var MorrisCharts = function () {
        };

        //creates line chart
        MorrisCharts.prototype.createLineChart = function (element, final_result, xkey, ykeys, labels, lineColors) {
            Morris.Line({

                element: element,
                behaveLikeLine: true,
                data: final_result,
                xkey: xkey,
                ykeys: ykeys,
                labels: labels,
                xLabelAngle: 60,
                pointSize: 2,
                hideHover: 'auto',
                lineColors: lineColors,
                parseTime: false,
                resize: true, //defaulted to true

            });
        },

            MorrisCharts.prototype.init = function () {

                var $morrisData = [];

                $.each(final_result, function (valuee, namee) {
                    $morrisData.push({ y: namee.xvalues, a: namee.total_score, b: namee.get_score, c: namee.emp_id_ })
                });
                this.createLineChart('emp_epa_line_graph', $morrisData, 'y', ['a', 'b'], ['Total Score', 'Get Score'], ['#44a2d2', '#ffbb44']);
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



