$('#loader').show();

var emp_role_id;
var login_company_idd;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_idd);
        $("#div_payroll_dashboard_monthyear li").remove();
        $("#div_tab_content div").remove();
        GetPayrollProcessedMonthyear('div_payroll_dashboard_monthyear', login_company_idd);


        $('#ddlcompany').bind("change", function () {
            $("#div_payroll_dashboard_monthyear li").remove();
            $("#div_tab_content div").remove();
            GetPayrollProcessedMonthyear('div_payroll_dashboard_monthyear', $(this).val());
        });


        $('#loader').hide();

        $(document).on('click', '.btnProcessPayroll', function () {
            window.location.href = '/Payroll/ProcessPayroll';
        });

    }, 2000);// end timeout

});


function GetPayrollProcessedMonthyear(ControlId, CompanyId) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/GetPayrollProcessedMonthyear/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                $('#loader').hide();
                messageBox("error", res.message);
                return false;
            }
            $.each(res, function (data, value) {

                var full_monthyearr = value.toString();
                var _monthh = full_monthyearr.slice(-2);
                var _yearr = full_monthyearr.slice(2, 4);
                var _monthname = GetMonthName(_monthh);
                var month_yearr = _monthname + " " + _yearr;
                //$(ControlId).append($('<li role="presentation" ><a href="#tab' + value + '" role="tab" data-toggle="tab" onClick="GetPayrollProcessedMonthyearData(' + CompanyId + ', ' + value + ' )">' + value + '</a></li>'));

                $(ControlId).append("<li class='nav-item'><a class='nav-link' data-toggle='tab' href='#tab" + value + "'  role='tab' onClick='GetPayrollProcessedMonthyearData(" + CompanyId + ", " + value + ")'>" + month_yearr + "</a></li>");

                //  $(ControlId).append($('<li role="presentation" ><a href="#tab' + value + '" role="tab" data-toggle="tab" onClick="GetPayrollProcessedMonthyearData(' + CompanyId + ', ' + value + ' )">' + month_yearr + '</a></li>'));
            });

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}


function GetPayrollProcessedMonthyearData(CompanyId, Monthyear) {
    $('#loader').show();
    ControlId = '#div_tab_content';
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/GetPayrollProcessedMonthyearData/' + CompanyId + '/' + Monthyear,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            // debugger;
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $('#loader').hide();
                return false;
            }

            $("#div_tab_content div").remove();

            var full_monthyearr = Monthyear.toString();
            var _monthh = full_monthyearr.slice(-2);
            var _yearr = full_monthyearr.slice(2, 4);
            var _monthname = GetMonthName(_monthh);
            var month_yearr = _monthname + " " + _yearr;

            // $(ControlId).append($('<div role="tabpanel" class="tab-pane active" id="tab' + Monthyear + '"><div class= "tabsControllar" ></div ><div class="typography"><h1 style="float: left;"> ' + Monthyear + ' Payroll </h1><div> <button type="button" style="float: right; margin-top: 2%;" class="btn btnProcessPayroll">Process Payroll</button></div><div class="clear"></div><hr /><div class="col-md-3"><p>Net Payout</p><h2>Rs <span>' + res.net_payout + '</span></h2><table><tr><td>Gross Pay</td><td>Rs <span>' + res.gross_pay + '</span></td></tr><tr><td>Deductions</td><td>Rs <span>-' + res.deductions + '</span></td></tr><tr><td>Word Days</td><td>Rs <span>' + res.days + '</span></td></tr></table></div><div class="col-md-3"><p>Employee</p><h2>' + res.number_of_employee_procssed + '</h2><table><tr><td>Addition</td><td>0</td></tr><tr><td>Separation</td><td>0</td></tr><tr><td>Exclusion</td><td>0</td></tr><tr><td>Settlements</td><td>0</td></tr></table></div><div class="clear"></div><div class="bottom-sky"><div class="col-md-6 text-center"><h3 class="text-center">Payout Pending </h3><div class="btran">Bank Transfer : <span>' + res.pending_employee_procssed_ + '</span></div></div><div class="col-md-6"><h3>To Do</h3><ul><li>Lock Previous Payroll</li><li>New Employee additions</li><li>Employee Separations</li><li>Confirmation Updates</li><li>Employee data updates</li><li>Salary Revisions</li><li>Update One Time Payment</li><li>Update One Time Deductions</li><li>Update any other salary changes</li></ul></div><div class="clear"></div></div><div class="clear"></div></div></div >'));

            //$(ControlId).append($('<div role="tabpanel" class="tab-pane active" id="tab' + Monthyear + '"><div class= "tabsControllar" ></div ><div class="typography"><h1 style="float: left;"> ' + month_yearr + ' Payroll </h1><div> <button type="button" style="float: right; margin-top: 2%;" class="btn btnProcessPayroll">Process Payroll</button></div><div class="clear"></div><hr /><div class="col-md-3"><p>Net Payout</p><h2>Rs <span>' + res.net_payout + '</span></h2><table><tr><td>Gross Pay</td><td>Rs <span>' + res.gross_pay + '</span></td></tr><tr><td>Deductions</td><td>Rs <span>-' + res.deductions + '</span></td></tr><tr><td>Work Days</td><td> <span>' + res.days + '</span></td></tr></table></div><div class="col-md-3"><p>Employee</p><h2>' + res.number_of_employee_procssed + '</h2><table><tr><td>Addition</td><td>0</td></tr><tr><td>Separation</td><td>0</td></tr><tr><td>Exclusion</td><td>0</td></tr><tr><td>Settlements</td><td>0</td></tr></table></div><div class="clear"></div><div class="bottom-sky"><div class="col-md-6 text-center"><h3 class="text-center">Payout Pending </h3><div class="btran">Bank Transfer : <span>' + res.pending_employee_procssed_ + '</span></div></div><div class="col-md-6"><h3>To Do</h3><ul><li>Lock Previous Payroll</li><li>New Employee additions</li><li>Employee Separations</li><li>Confirmation Updates</li><li>Employee data updates</li><li>Salary Revisions</li><li>Update One Time Payment</li><li>Update One Time Deductions</li><li>Update any other salary changes</li></ul></div><div class="clear"></div></div><div class="clear"></div></div></div >'));



            var data_ = "";
            data_ = data_ + " <div class=col-xl-12><div class=panel><div class=card-body>";
            data_ = data_ + "<div role=tabpanel class=tab-pane active id=tab" + Monthyear + "></div ><div class=tabsControllar></div >";
            data_ = data_ + "<div class=col-md-12><div class=col-md-6 typography><h3 style=float:left;> " + month_yearr + " Payroll </h3></div>";
            data_ = data_ + "<div class=col-md-6><button type=button style=float:right;margin-top:2%; class=\"" + "btn btn-light" + "\">Process Payroll</button></div><div class=col-md-12><hr /></div></div>";
            data_ = data_ + "<div class=col-md-6><div class=col-md-6><p>Net Payout</p><h4>Rs <span>" + res.net_payout + "</span></h4></div>";
            data_ = data_ + "<table class=table><tr><td>Gross Pay</td><td>Rs <span>" + res.gross_pay + "</span></td></tr><tr><td>Deductions</td><td>Rs <span>-" + res.deductions + "</span></td></tr>";
            data_ = data_ + "<tr><td>Work Days</td><td> <span>" + res.days + "</span></td></tr></table></div> <div class=col-md-6><div class=col-md-6><p>Employee</p><h4>" + res.number_of_employee_procssed + "</h4></div>";
            data_ = data_ + "<table class=table><tr><td>Addition</td><td>0</td></tr><tr><td>Separation</td><td>0</td></tr><tr><td>Exclusion</td><td>0</td></tr><tr><td>Settlements</td><td>0</td></tr></table></div><div class=clearfix></div>";
            data_ = data_ + "<div class=bottom-sky><div class=col-md-6 text-center><h3 class=text-center>Payout Pending </h3><div class=btran style=text-align:center;>Bank Transfer : <span>" + res.pending_employee_procssed_ + "</span></div></div>";
            data_ = data_ + "<div class=clearfix></div></div>";

            data_ = data_ + "</div></div></div>"


            $(ControlId).append($(data_));


            //$(ControlId).append($('<div role="tabpanel" class="tab-pane active" id="tab' + Monthyear + '"><div class= "tabsControllar" ></div ><div class="typography"><h4 style="float: left;"> ' + month_yearr + ' Payroll </h4><div> <button type="button" style="float: right; margin-top: 2%;" class="btn btnProcessPayroll">Process Payroll</button></div><div class="clear"></div><hr /><div class="col-md-3"><p>Net Payout</p><h2>Rs <span>' + res.net_payout + '</span></h2><table><tr><td>Gross Pay</td><td>Rs <span>' + res.gross_pay + '</span></td></tr><tr><td>Deductions</td><td>Rs <span>-' + res.deductions + '</span></td></tr><tr><td>Work Days</td><td> <span>' + res.days + '</span></td></tr></table></div><div class="col-md-3"><p>Employee</p><h2>' + res.number_of_employee_procssed + '</h2><table><tr><td>Addition</td><td>0</td></tr><tr><td>Separation</td><td>0</td></tr><tr><td>Exclusion</td><td>0</td></tr><tr><td>Settlements</td><td>0</td></tr></table></div><div class="clear"></div><div class="bottom-sky"><div class="col-md-6 text-center"><h3 class="text-center">Payout Pending </h3><div class="btran">Bank Transfer : <span>' + res.pending_employee_procssed_ + '</span></div></div><div class="clear"></div></div><div class="clear"></div></div></div >'));


            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}





function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}