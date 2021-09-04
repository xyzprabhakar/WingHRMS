var default_company;
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company('ddlcompany', login_emp, default_company);
        GetData(default_company);




        // debugger;
        // GetData(default_company);


        $('#ddlcompany').bind("change", function () {
            if ($(this).val() == "0") {
            }
            else {
                GetData($(this).val());
            }
            $('#ddl_criteria_level').val('');

        });


        $('#btnupdate').hide();
        $('#btnsave').show();



        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {

            }
        });

        $('#btnreset').bind('click', function () {
            BindAllEmp_Company('ddlcompany', login_emp, default_company);
            GetData(default_company);
            $('#ddl_criteria_level').val('');
            $('#btnupdate').hide();
            $('#btnsave').show();
        });
        $('#btnsave').bind("click", function () {
            // debugger;

            var company_id = $("#ddlcompany").val();
            var txtcriteria_name = $("#ddl_criteria_level").val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;


            if (company_id == null || company_id == '' || company_id == "0") {
                errormsg = "Please Select Company <br/>";
                iserror = true;
            }

            //validation part
            if (txtcriteria_name == null || txtcriteria_name == '') {
                errormsg = "Please Select Criteria Level <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                //  messageBox("info", "eror give");
                return false;
            }

            $('#loader').show();


            var myData = {
                'criteria_count': txtcriteria_name,
                'company_id': company_id,
                'created_by': login_emp,
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Save_KpiCriteria';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp, default_company);
                        $("#ddl_criteria_level").val(0);
                        GetData(default_company);
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err, exception) {
                    alert('error found');
                    $('#loader').hide();
                }
            });
        });

    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData(company_id) {
    // debugger;
    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_KpiCriteria/' + company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;
            if (data != undefined && data != null) {
                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    $("#loader").hide();
                    return false;
                }
                BindAllEmp_Company('ddlcompany', login_emp, data.company_id);

                $("#ddl_criteria_level").val(data.criteria_count);



            }

            $('#loader').hide();
        },
        error: function (error) {
            //alert(error);
            console.log("error");
        }
    });

}

//function GetEditData(id) {
//    $('#loader').show();
//    if (id == null || id == '') {
//        messageBox('info', 'There some problem please try after later !!');
//        return false;
//    }

//    $("#hdnid").val(id);

//    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_KpiCriteria/' + id + '/' + default_company;

//    $.ajax({
//        type: "GET",
//        url: apiurl,
//        data: {},
//        dataType: "json",
//        contentType: 'application/json; charset=utf-8',
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (res) {

//            data = res;

//            $("#txtcriteria_name").val(data.criteria_name);
//            $("#txtdescription").val(data.description);
//            $("input[name=chkstatus][value=" + data.is_active + "]").prop('checked', true);
//            $("#hdnid").val(id);

//            $('#btnupdate').show();
//            $('#btnsave').hide();

//            $('#loader').hide();
//        },
//        Error: function (err) {

//        }
//    });
//}


////-------update city data
//$("#btnupdate").bind("click", function () {
//    $('#loader').show();
//    var company_id = $("#ddlcompany").val();
//    var txtcriteria_name = $("#txtcriteria_name").val();
//    var txtdescription = $("#txtdescription").val();
//    var kpi_cr_id = $("#hdnid").val();

//    var is_active = 0;
//    var errormsg = '';
//    var iserror = false;


//    if (company_id == null || company_id == '') {
//        errormsg = "Please Select Company <br/>";
//        iserror = true;
//    }

//    if ($("input[name='chkstatus']:checked")) {
//        if ($("input[name='chkstatus']:checked").val() == '1') {
//            is_active = 1;
//        }
//    }
//    if (!$("input[name='chkstatus']:checked").val()) {
//        errormsg = errormsg + "Please select active/in-active status !! <br/>";
//        iserror = true;
//    }

//    //validation part
//    if (txtcriteria_name == null || txtcriteria_name == '') {
//        errormsg = "Please Enter Criteria Name <br/>";
//        iserror = true;
//    }

//    if (iserror) {
//        messageBox("error", errormsg);
//        $('#loader').hide();
//        //  messageBox("info", "eror give");
//        return false;
//    }

//    var myData = {
//        'kpi_cr_id': kpi_cr_id,
//        'criteria_name': txtcriteria_name,
//        'description': txtdescription,
//        'is_active': is_active,
//        'company_id': company_id,
//        'created_by': login_emp,
//    };
//    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Update_KpiCriteria';
//    var Obj = JSON.stringify(myData);
//    var headerss = {};
//    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
//    headerss["salt"] = $("#hdnsalt").val();

//    $.ajax({
//        url: apiurl,
//        type: "POST",
//        data: Obj,
//        dataType: "json",
//        contentType: "application/json",
//        headers: headerss,
//        success: function (data) {

//            // var resp = JSON.parse(data);
//            var statuscode = data.statusCode;
//            var Msg = data.message;
//            $('#loader').hide();
//            if (statuscode == "0") {
//                $("#hdnid").val('');
//                $('#txtcriteria_name').val('');
//                $('#txtdescription').val('');
//                $('input:radio[name=chkstatus]:checked').prop('checked', false);
//                $('#btnupdate').hide();
//                $('#btnsave').show();
//                GetData(company_id);
//                messageBox("success", Msg);
//            }
//            else if (statuscode == "1" || statuscode == '2') {
//                messageBox("error", Msg);
//            }
//        },
//        error: function (err, exception) {
//            alert('error found');
//            $('#loader').hide();
//        }
//    });
//});