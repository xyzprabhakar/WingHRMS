$('#loader').show();
var company_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        var qs = getQueryStrings();
        var company_id = qs["company_id"];

        if (token == null && company_id == 'undefined') {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetDataByCompanyId(company_id);

        $('#loader').hide();

    }, 2000);// end timeout

});


//Edit Company Master
function GetDataByCompanyId(company_id) {
    $('#loader').show();
    // // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/" + company_id,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {

            var res = data;
            // // debugger;
            $("#hdnCompany_Id").val(res[0].company_id);
            $("#txtcompany_name").val(res[0].company_name);
            $("#txtcompany_code").val(res[0].company_code);
            $("#txtprefix_for_employee_code").val(res[0].prefix_for_employee_code);
            $("#txtnumber_of_character_for_employee_code").val(res[0].number_of_character_for_employee_code);
            $("#txtaddress_line_one").val(res[0].address_line_one);
            $("#txtaddress_line_two").val(res[0].address_line_two);

            BindCountryList('ddlcountry', res[0].country_id);
            BindStateList('ddlstate', res[0].state_id, res[0].country_id);
            BindCityList('ddlcity', res[0].state_id, res[0].city_id);


            $("#txtpin_code").val(res[0].pin_code);
            $("#txtprimary_email_id").val(res[0].primary_email_id);
            $("#txtsecondary_email_id").val(res[0].secondary_email_id);
            $("#txtprimary_contact_number").val(res[0].primary_contact_number);
            $("#txtsecondary_contact_number").val(res[0].secondary_contact_number);
            $("#txtcompany_website").val(res[0].company_website);


            var _appSetting_domainn_dec = CryptoJS.AES.decrypt(localStorage.getItem("_appSetting_domainn"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
            $('#company_logo').attr("src", _appSetting_domainn_dec.toString() + res[0].company_logo);

            //$('#company_logo').attr("src", "http://192.168.10.129:1011" + res[0].company_logo);

            $('#loader').hide();

        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });
}
