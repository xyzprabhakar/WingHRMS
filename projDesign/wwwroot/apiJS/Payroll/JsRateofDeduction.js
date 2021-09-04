
$('#loader').show();

var emp_role_id;
var login_emp_id;
var companyid;
$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        companyid = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlcompany', login_emp_id, companyid);
        BindDeductionComponent('ddldamageloss', 0, companyid);
        BindDeductionComponent('ddlWhetherWorkmanShowed', 0, companyid);
        BindDeductionComponent('ddlDateofDeductionImposed', 0, companyid);
        BindDeductionComponent('ddlAmtofDeductionImposed', 0, companyid);
        BindDeductionComponent('ddlnoofInstallment', 0, companyid);
        BindDeductionComponent('ddlDateOnWhichDeductionRealised', 0, companyid);
        BindDeductionComponent('ddlRemarks', 0, companyid);
        GetData(companyid);

        $('#loader').hide();

        //BindDeductionComponent('ddlDeduction', 0)






        $("#btnreset").bind("click", function () {
            location.reload();
        });

        $("#btnsave").bind("click", function () {


            var company_id = $("#ddlcompany").val();
            // var deduction_id = $("#ddlDeduction").val();
            var ddldamageloss = $("#ddldamageloss").val();
            var ddlWhetherWorkmanShowed = $("#ddlWhetherWorkmanShowed").val();
            var ddlDateofDeductionImposed = $("#ddlDateofDeductionImposed").val();
            var ddlAmtofDeductionImposed = $("#ddlAmtofDeductionImposed").val();
            var ddlnoofInstallment = $("#ddlnoofInstallment").val();
            var ddlDateOnWhichDeductionRealised = $("#ddlDateOnWhichDeductionRealised").val();
            var ddlRemarks = $("#ddlRemarks").val();
            var errormsg = "";
            var iserror = false;
            if (company_id == "" || company_id == "0" || company_id == null) {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }
            if (ddldamageloss == "0") {
                ddldamageloss = "";
            }
            if (ddlWhetherWorkmanShowed == "0") {
                ddlWhetherWorkmanShowed = "";
            }
            if (ddlDateofDeductionImposed == "0") {
                ddlDateofDeductionImposed = "";
            }
            if (ddlAmtofDeductionImposed == "0") {
                ddlAmtofDeductionImposed = "";
            }
            if (ddlnoofInstallment == "0") {
                ddlnoofInstallment = "";
            }
            if (ddlDateOnWhichDeductionRealised == "0") {
                ddlDateOnWhichDeductionRealised = "";
            }
            if (ddlRemarks == "0") {
                ddlRemarks = "";
            }
            //if (deduction_id == "" || deduction_id == "0" || deduction_id == null) {
            //    errormsg = errormsg + "Please Select Component";
            //    iserror = true;
            //}

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var mydata = {
                damage_orloss_and_date_c_id: ddldamageloss,
                whether_workman_c_id: ddlWhetherWorkmanShowed,
                date_ofdeduc_c_id: ddlDateofDeductionImposed,
                amt_ofdeduc_c_id: ddlAmtofDeductionImposed,
                no_of_installment_c_id: ddlnoofInstallment,
                date_realised_c_id: ddlDateOnWhichDeductionRealised,
                remarks_c_id: ddlRemarks,
                comp_id: company_id,
                //deduction_id: deduction_id,
                //comp_id: company_id,
                created_by: login_emp_id
            }
            $('#loader').show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({
                // url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RateofDeduction",
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_RegisterofDeductionFormIIMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        $("#ddldamageloss").val('0');
                        $("#ddlWhetherWorkmanShowed").val('0');
                        $("#ddlDateofDeductionImposed").val('0');
                        $("#ddlAmtofDeductionImposed").val('0');
                        $("#ddlnoofInstallment").val('0');
                        $("#ddlDateOnWhichDeductionRealised").val('0');
                        $("#ddlRemarks").val('0');
                        messageBox("success", Msg);
                        // alert(Msg);
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });
        });

        $("#ddlcompany").bind("change", function () {
            GetData($(this).val());

            BindDeductionComponent('ddldamageloss', 0, $(this).val());
            BindDeductionComponent('ddlWhetherWorkmanShowed', 0, $(this).val());
            BindDeductionComponent('ddlDateofDeductionImposed', 0, $(this).val());
            BindDeductionComponent('ddlAmtofDeductionImposed', 0, $(this).val());
            BindDeductionComponent('ddlnoofInstallment', 0, $(this).val());
            BindDeductionComponent('ddlDateOnWhichDeductionRealised', 0, $(this).val());
            BindDeductionComponent('ddlRemarks', 0, $(this).val());
        });

    }, 2000);// end timeout

});


function BindDeductionComponent(ControlId, SelectedVal, company_idd) {
    $('#loader').show();
    ControlId = '#' + ControlId;


    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_DeductionComponent/2/" + company_idd,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            if (res.length > 0) {

                $.each(res, function (data, value) {
                    $(ControlId).append($("<option></option>").val(value.component_id).html(value.property_details));
                })

                //get and set selected value
                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                    $(ControlId).val(SelectedVal);
                }
            }

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });

}


function GetData(company_id) {
    $('#loader').show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RegisterOfDeductionFormIIMaster/" + company_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            if (res.damage_orloss_and_date_c_id != null) {
                BindDeductionComponent('ddldamageloss', res.damage_orloss_and_date_c_id, company_id);
            }
            if (res.whether_workman_c_id != null) {
                BindDeductionComponent('ddlWhetherWorkmanShowed', res.whether_workman_c_id, company_id);
            }
            //if (res.rate_of_wages_c_id != null) {
            //    BindComponent('ddlRateOfWages', res.rate_of_wages_c_id);
            //}
            if (res.date_and_amt_c_id != null) {
                BindDeductionComponent('ddlDateAndAmountofDeductionImposed', res.date_and_amt_c_id, company_id);
            }
            if (res.no_of_installment_c_id != null) {
                BindDeductionComponent('ddlnoofInstallment', res.no_of_installment_c_id, company_id);
            }
            if (res.date_realised_c_id != null) {
                BindDeductionComponent('ddlDateOnWhichDeductionRealised', res.date_realised_c_id, company_id);
            }
            if (res.remarks_c_id != null) {
                BindDeductionComponent('ddlRemarks', res.remarks_c_id, company_id);
            }

        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}



//function GetData() {
//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_RateofDeudction",
//        type: "GET",
//        contentType: "application/json",
//        dataType: "json",
//        data: "{}",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;
//            $("#tblrateofdeduction").DataTable({
//                "processing": true, // for show progress bar
//                "serverSide": false, // for process server side
//                "bDestroy": true,
//                "filter": true, // this is for disable filter (search box)
//                "orderMulti": false, // for disable multiple column at once
//                "scrollX": 200,
//                "aaData": res,
//                "columnDefs": [

//                ],
//                "columns": [
//                    { "data": null, "title": "S.No", "autoWidth": true },
//                    { "data": "company_name", "name":"company_name", "title": "Company", "autoWidth": true },
//                    //{ "data": "property_details", "name": "property_details","title":"Rate of Deduction", "autoWidth": true },
//                    { "data": "created_date", "name": "created_date","title":"Created On", "autoWidth": true },
//                    { "data": "modified_date", "name": "modified_date","title":"Modified On", "autoWidth": true },
//                    {
//                        "title": "Is Deleted?", "autoWidth": true,
//                        "render": function (data, type, full, meta) {
//                            return '<a href="#" onclick="DeleteData(' + full.rate_id + ',' + full.comp_id+')" ><i class="fa fa-pencil-square-o"></i></a>';
//                        }
//                    }
//                ],
//                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
//                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
//                    return nRow;
//                },
//                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
//            });
//        },
//        error: function (err, Exception) {
//            messageBox(err, response.Text);
//        }


//    });
//}

//function DeleteData(rate_id,company_id) {

//    var mydata = {
//        rate_id: rate_id,
//        comp_id: company_id,
//        modified_by: localStorage.getItem("emp_id")
//    }

//    $.ajax({
//        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Delete_RateofDeductions",
//        type: "POST",
//        contentType: "application/json",
//        dataType: "json",
//        data: JSON.stringify(mydata),
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (data) {
//            var statuscode = data.statusCode;
//            var Msg = data.message;

//            if (statuscode == "0") {
//                alert(Msg);
//            }
//            else if (statuscode == "1" || statuscode == '2') {
//                messageBox("error", Msg);
//                return false;
//            }
//        },
//        error: function (err, Exception) {
//            messageBox(err, response.Text);
//        }
//    });
//}

