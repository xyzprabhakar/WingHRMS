$('#loader').show();
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

        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
        //BindCompanyList('ddlcompany', company_id);

        //  BinddEmployeeCodee('ddlemployee', company_id, login_emp_id);

        GetData(company_id, login_emp_id);

        $('#loader').hide();

        $("#ddlcompany").bind("change", function () {
            if ($.fn.DataTable.isDataTable('#tblsalaryslip')) {
                $('#tblsalaryslip').DataTable().clear().draw();
            }
            // BinddEmployeeCodee('ddlemployee', $(this).val(), login_emp_id);
            GetData($(this).val(), login_emp_id);
        });


    }, 2000);// end timeout

});


function GetData(companyid, employeeid) {
    //if (companyid == 0) {
    //    messageBox("error", "Please Select Company");
    //    $("#tblsalaryslip").DataTable().clear().draw();
    //    return false;
    //}

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_SalarySlip/" + companyid + "/" + employeeid,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            var res = response;
            _GUID_New();
            $("#tblsalaryslip").DataTable({
                "processing": true,// for show progress bar
                "serverSide": false, //for process server side
                "bDestroy": true, // for Distroy previous data
                "filter": true, //this is for disable filter(search box)
                "orderMulti": false, // for disable multiple column at once,
                "scrollX": 150,
                "aaData": res,
                "columnDefs": [

                    {
                        targets: [4],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    },
                    //{
                    //    targets: [4],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return GetDateFormatddMMyyyy(date);
                    //    }
                    //}
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autowidth": true },
                    { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autowidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autowidth": true },
                    { "data": "payroll_month_year", "name": "payroll_month_year", "title": "Payroll YearMonth", "autowidth": true },
                    //{ "data": "is_freezed", "name": "is_freezed", "title": "Is Freezed", "autowidth": true },
                    //{ "data": "is_lock", "name": "is_lock", "title": "Is Lock", "autowidth": true },
                    //{ "data": "payroll_status", "name": "payroll_status", "title": "Payroll Status", "autowidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created Date", "autowidth": true },
                    //{ "data": "last_modified_date", "name": "last_modified_date", "title": "Modified Date", "autowidth": true },
                    {
                        "title": "View",
                        "render": function (data, type, full, meta) {

                            if (full.is_lock == 1) {
                                return '<a href="#" onclick="ViewSalarySlip(' + full.emp_id + ',' + full.payroll_month_year + ')" >View</a>';
                            }
                            else {
                                return '<label></label>';
                            }



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
            _GUID_New();
            $('#loader').hide();
            alert(err.responseText);


        }
    });
}

function ViewSalarySlip(employeeid, monthyear) {
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //headerss["salt"] = $("#hdnsalt").val();


    var doc = new jsPDF();

    $("#loader").show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiMasters/GetPaySlip/" + employeeid + "/" + monthyear,
        type: "GET",
        contentType: "application.json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            //_GUID_New();
            //window.open(response.path);

            if (response.pdf_data == null || response.pdf_data == "") {
                messageBox("error", "salary slip not available");
                $("#loader").hide();
                return false
            }

            newWin = window.open("");
            newWin.document.write(response.pdf_data);
            newWin.print();
            newWin.close();
            $("#loader").hide();
            //$('#div_show_d').html(response.pdf_data);
            //var HTML_Width = 1100;
            //var HTML_Height = 500;
            //var top_left_margin = 3;
            //var PDF_Width = HTML_Width + (top_left_margin * 2);
            //var PDF_Height = HTML_Height + (top_left_margin * 2) //(PDF_Width * 1.5) + (top_left_margin * 2);
            //var canvas_image_width = HTML_Width+1;
            //var canvas_image_height = HTML_Height+1;


            //var login_emp_name = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

            //html2canvas($("#div_show_d")[0]).then(function (canvas) {
            //    var imgData = canvas.toDataURL("image", 1.0);
            //    //var pdf = new jsPDF('p', 'mm', [PDF_Width, PDF_Height]);                        
            //    var pdf = new jsPDF({
            //        orientation: 'landscape',
            //        unit: 'pt',
            //        format: [1100, 700]
            //    });
            //    pdf.addImage(imgData, 'JPG', 50, 50, 1000, 600);
            //    pdf.save("Salary Slip " + login_emp_name + " " + monthyear + "_.pdf");
            //     $("#loader").hide();
            //});

            $('#div_show_d').empty();

        },
        error: function (err) {
            alert(err.responseText);
            // _GUID_New();
            $("#loader").hide();
            $('#loader').hide();
        }
    });
}


