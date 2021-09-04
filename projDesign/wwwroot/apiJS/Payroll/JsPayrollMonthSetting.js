$('#loader').show();
var login_emp_id;
var company_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        $('#btnupdate').hide();
        $('#btnsave').show();
        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);



        $('#loader').hide();

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var txtFrom = $("#txtFrom").val();
            var txtFromDay = $("#txtFromDay").val();
            var txtToDay = $("#txtToDay").val();
            var ddlcompany = $("#ddlcompany").val();


            var is_deleted = 0;

            //validation part
            if (ddlcompany == '' || ddlcompany == 0) {
                messageBox("error", "Please Select Company...!");
                $('#loader').hide();
                return;
            }

            if (txtFrom == null || txtFrom == '') {
                messageBox("error", "Please Select From Month...!");
                $('#loader').hide();
                return;
            }
            if (txtFromDay == null || txtFromDay == '') {
                messageBox("error", "Please Select From Day...!");
                $('#loader').hide();
                return;
            }
            if (txtToDay == null || txtToDay == '') {
                messageBox("error", "Please Select To Day...!");
                $('#loader').hide();
                return;
            }


            var myData = {
                'from_month': txtFrom.substring(5, 7),
                'from_date': txtFromDay,
                'applicable_from_date': txtFromDay,
                'applicable_to_date': txtToDay,
                'company_id': ddlcompany,
                'is_deleted': is_deleted,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_PayrollMonthSet';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // debugger;
            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        BindAllEmp_Company('ddlcompany', login_emp_id, company_id);
                        $("#txtFrom").val('');
                        $("#txtFromDay").val('');
                        $("#txtToDay").val('');
                        messageBox("success", Msg);
                        return false;
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    alert(err.responseText);
                }
            });
        });

    }, 2000);// end timeout

});



