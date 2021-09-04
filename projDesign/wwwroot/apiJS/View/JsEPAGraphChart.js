$('#loader').show();
var company_id;
var login_emp_id;
var is_manager;
$(document).ready(function () {
    setTimeout(function () {
        



        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', company_id, login_emp_id, login_emp_id);

        //BindCompanyList('ddlcompany', company_id);
        //BindEmployeecodes('ddlemployee', login_emp_id);

        var _manager = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        if (_manager == "yes") {
            is_manager = 1;
        }
        else {
            is_manager = 0;
        }

        $("#divgraph").css("display", "none");

        $('#loader').hide();


        $("#ddlcompany").bind("change", function () {
            BindAllEmp_Company('ddlcompany', login_emp_id, $(this).val());
            BindEmployeeListUnderLoginEmpFromAllComp('ddlemployee', $(this).val(), login_emp_id, login_emp_id);
            $("#emp_epa_line_graph").empty();
            $("#divgraph").css("display", "none");
        });

        $("#ddlemployee").bind("change", function () {
            $("#loader").show();
            $("#emp_epa_line_graph").empty();
            $("#divgraph").css("display", "none");
            $("#loader").hide();
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

            if ($("#ddlemployee").val() == "0" || $("#ddlemployee").val() == "" || $("#ddlemployee").val() == null) {
                is_error = true;
                error_msg = error_msg + "Please select employee</br>";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            $("#divgraph").css("display", "none");
            $("#loader").show();
            GetData();
            $("#loader").hide();
        });


    }, 2000);// end timeout


});





//function BindEmployeecodes(ControlId, SelectedVal) {

//    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
//    // console.log(user_name_dec.toString(CryptoJS.enc.Utf8));
//    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";


//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        url: localStorage.getItem("ApiUrl") + "apiMasters/Get_Employee_Under_LoginEmp/" + login_emp_id,
//        data: {},
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;
//            $(ControlId).empty().append('<option selected="selected" value=' + login_emp_id + '>' + login_name_code + '</option>');
//            $.each(res, function (data, value) {

//                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
//            })
//            //get and set selected value
//            if (SelectedVal != '' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
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




function GetData() {
    var for_all_emp = 1;
    var emp_id = $("#ddlemployee").val();

    if (emp_id == 0) {
        for_all_emp = 0;
        emp_id = $('#ddlemployee option').last().val();
    }
    else {

        emp_id = login_emp_id;
    }



    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_EmpEPAScoreDetails/" + $("#ddlcompany").val() + "/" + $("#ddlemployee").val() + "/" + is_manager + "/" + for_all_emp,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined && res.statusCode != null) {
                messageBox("error", res.message);
                return false;
            }
            else if (res.length == "0") {
                // $("#emp_epa_line_graph").empty();
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

    $("#divgraph").css("display", "block");

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




}
