$('#loader').show();
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        var qs = getQueryStrings();
        var company_id = qs["company_id"];

        if (token == null && company_id == 'undefined') {
            window.location = '/Login';
        }

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $("form").keypress(function (e) {
            //Enter key
            if (e.which == 13) {
                return false;
            }
        });

        if (company_id != null) {
            $('#btnUpdate').show();
            $('#btnSave').hide();
            GetDataByCompanyId(company_id);

        }
        else {
            // Get Last Company Id
            GetLastCompanyId();
            $('#btnUpdate').hide();
            $('#btnSave').show();

        }
        //Get All Company 
        GetAllCompany();


        //Get All Countery
        BindCountryList('ddlcountry', 0);

        // Get State On Countery Change
        //if ($("#dtpFromDt").val() != undefined && $("#dtpFromDt").val() != null && $("#dtpFromDt").val()!="" && $("#dtpToDt").val() != undefined && $("#dtpToDt").val() != null && $("#dtpToDt").val() != "") {

        //    GetAllCompany();
        //}

        $('#loader').hide();


        $('#ddlcountry').bind("change", function () {
            $('#loader').show();
            BindStateList('ddlstate', 0, $(this).val());
            $('#loader').hide();
        });

        //Get City On State Change
        $('#ddlstate').bind("change", function () {
            $('#loader').show();
            BindCityList('ddlcity', $(this).val(), 0);
            $('#loader').hide();
        });

        EL("File1").addEventListener("change", readFile, false);

        //$("#File1").bind("change", function () {
        //    EL("File1").addEventListener("change", readFile, false);
        //});


        $('#txtprimary_email_id').bind("change", function () {
            ValidEmailId("txtprimary_email_id");
        });

        $('#txtsecondary_email_id').bind("change", function () {
            ValidEmailId("txtsecondary_email_id");
        });

        //$("#File1").bind("change", function () {
        //    EL("File1").addEventListener("change", readFile, false);
        //});


        //Reset form
        $('#btnReset').bind("click", function () {
            //window.location.href = '/CompanyMaster/Create';
            window.location.href = '/CompanyMaster/View';

        });

        // Save Company Master Data
        $('#btnSave').bind("click", function () {

            // EL("File1").addEventListener("change", readFile, false);

            var company_name = $.trim($("#txtcompany_name").val());
            var company_code = $("#txtcompany_code").val();
            var prefix_for_employee_code = $("#txtprefix_for_employee_code").val();
            var number_of_character_for_employee_code = $("#txtnumber_of_character_for_employee_code").val();
            var address_line_one = $("#txtaddress_line_one").val();
            var address_line_two = $("#txtaddress_line_two").val();
            var country_id = $("#ddlcountry").val();
            var state_id = $("#ddlstate").val();
            var city_id = $("#ddlcity").val();
            var pin_code = $("#txtpin_code").val();
            var primary_email_id = $("#txtprimary_email_id").val();
            var secondary_email_id = $("#txtsecondary_email_id").val();
            var primary_contact_number = $("#txtprimary_contact_number").val();
            var secondary_contact_number = $("#txtsecondary_contact_number").val();
            var company_website = $("#txtcompany_website").val();
            var file_company_logo = $("#HFb64").val();
            var is_active = 1;
            var emp_id = login_emp_id;


            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }
            if ($("#is_in_active").is(":checked")) {
                is_active = 0;
            }


            var is_emp_code_manual_genrate = 0;
            if ($("#is_emp_code_manual_genrate").is(":checked")) {
                is_emp_code_manual_genrate = 1;
            }


            //Validation
            if (company_name == '') {
                messageBox("error", "Please Enter Company Name...!");
                return;
            }
            if (company_code == '') {
                messageBox("error", "Please Enter Company Code...!");
                return;
            }
            if (prefix_for_employee_code == '') {
                messageBox("error", "Please Enter Prefix For Employee Code...!");
                return;
            }

            if (number_of_character_for_employee_code == '') {
                messageBox("error", "Please Enter Number Of Character For Employee Code...!");
                return;
            }
            if (address_line_one == '' || address_line_one == null) {
                messageBox("error", "Please Enter Company Address...!");
                return;
            }

            if (country_id == 0) {
                messageBox("error", "Please Select Country...!");
                return;
            }

            if (state_id == 0) {
                messageBox("error", "Please Select State...!");
                return;
            }

            if (city_id == 0) {
                messageBox("error", "Please Select City...!");
                return;
            }

            if (pin_code == '') {
                messageBox("error", "Please Enter Pin Code...!");
                return;
            }

            if (primary_email_id == '') {
                messageBox("error", "Please Enter Primary Email Id...!");
                return;
            }


            if (primary_contact_number == '') {
                messageBox("error", "Please Enter Primary Contact Number...!");
                return;
            }


            var myData = {

                'company_name': company_name,
                'company_code': company_code,
                'prefix_for_employee_code': prefix_for_employee_code,
                'number_of_character_for_employee_code': number_of_character_for_employee_code,
                'address_line_one': address_line_one,
                'address_line_two': address_line_two,
                'country_id': country_id,
                'state_id': state_id,
                'city_id': city_id,
                'pin_code': pin_code,
                'primary_email_id': primary_email_id,
                'secondary_email_id': secondary_email_id,
                'primary_contact_number': primary_contact_number,
                'secondary_contact_number': secondary_contact_number,
                'company_website': company_website,
                'company_logo': file_company_logo,
                'is_active': is_active,
                'created_by': emp_id,
                'last_modified_by': emp_id,
                'is_emp_code_manual_genrate': is_emp_code_manual_genrate,
            };

            console.log(JSON.stringify(myData));
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $('#loader').show();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/",
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;


                    _GUID_New();

                    //if data save
                    if (statuscode == "1") {
                        //  GetAllCompany();
                        alert(Msg);
                        $('#loader').hide();
                        window.location.href = '/CompanyMaster/View';

                    }
                    else if (statuscode == "0") {
                        messageBox("error", Msg);
                        $('#loader').hide();
                        return false;

                    }
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });
        });


        // Save Company Master Data
        $('#btnUpdate').bind("click", function () {

            // // debugger;
            var company_id = $("#hdnCompany_Id").val();
            var company_name = $.trim($("#txtcompany_name").val());
            var company_code = $("#txtcompany_code").val();
            var prefix_for_employee_code = $("#txtprefix_for_employee_code").val();
            var number_of_character_for_employee_code = $("#txtnumber_of_character_for_employee_code").val();
            var address_line_one = $("#txtaddress_line_one").val();
            var address_line_two = $("#txtaddress_line_two").val();
            var country_id = $("#ddlcountry").val();
            var state_id = $("#ddlstate").val();
            var city_id = $("#ddlcity").val();
            var pin_code = $("#txtpin_code").val();
            var primary_email_id = $("#txtprimary_email_id").val();
            var secondary_email_id = $("#txtsecondary_email_id").val();
            var primary_contact_number = $("#txtprimary_contact_number").val();
            var secondary_contact_number = $("#txtsecondary_contact_number").val();
            var company_website = $("#txtcompany_website").val();
            var emp_id = login_emp_id;


            var file_company_logo = null;
            if ($("#HFb64").val() != null && $("#HFb64").val() != '') {
                //if ($("#File1").val() != undefined && $("#File1").val() != null && $("#File1").val() != "") {
                file_company_logo = $("#HFb64").val();//$("#File1").val(); //
            }
            else {
                file_company_logo = $("#hdnCompany_logo").val();
            }

            var is_active = 1;

            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }
            if ($("#is_in_active").is(":checked")) {
                is_active = 0;
            }

            var is_emp_code_manual_genrate = 0;
            if ($("#is_emp_code_manual_genrate").is(":checked")) {
                is_emp_code_manual_genrate = 1;
            }
            //Validation
            if (company_name == '') {
                messageBox("error", "Please Enter Company Master...!");
                return;
            }

            if (company_code == '') {
                messageBox("error", "Please Enter Company Code...!");
                return;
            }

            if (primary_email_id == '') {
                messageBox("error", "Please Enter Primary Email Id...!");
                return;
            }

            if (primary_contact_number == '') {
                messageBox("error", "Please Enter Primary Email Id...!");
                return;
            }

            if (number_of_character_for_employee_code == '') {
                messageBox("error", "Please Enter Number Of Character For Employee Code...!");
                return;
            }

            if (country_id == '') {
                messageBox("error", "Please Select Country...!");
                return;
            }

            if (state_id == '') {
                messageBox("error", "Please Select State...!");
                return;
            }

            if (city_id == '') {
                messageBox("error", "Please Select City...!");
                return;
            }

            if (pin_code == '') {
                messageBox("error", "Please Enter Pin Code...!");
                return;
            }

            var myData = {
                //'company_id': company_id,
                'company_name': company_name,
                'company_code': company_code,
                'prefix_for_employee_code': prefix_for_employee_code,
                'number_of_character_for_employee_code': number_of_character_for_employee_code,
                'address_line_one': address_line_one,
                'address_line_two': address_line_two,
                'country_id': country_id,
                'state_id': state_id,
                'city_id': city_id,
                'pin_code': pin_code,
                'primary_email_id': primary_email_id,
                'secondary_email_id': secondary_email_id,
                'primary_contact_number': primary_contact_number,
                'secondary_contact_number': secondary_contact_number,
                'company_website': company_website,
                'company_logo': file_company_logo,
                'is_active': is_active,
                'last_modified_by': emp_id,
                'is_emp_code_manual_genrate': is_emp_code_manual_genrate,
            };
            // // debugger;
            // console.log(JSON.stringify(myData));
            // Save

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/" + company_id,
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    _GUID_New();


                    //if data save
                    if (statuscode == "1") {
                        // GetAllCompany();
                        alert(Msg);
                        $('#loader').hide();
                        window.location.href = '/CompanyMaster/View';

                    }
                    else if (statuscode == "0") {
                        messageBox("error", Msg);
                        $('#loader').hide();
                        return false;
                    }
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });
        });

        //$("#btnGetData").bind("click", function () {
        //    var fromdt = $("#dtpFromDt").val();
        //    var todt = $("#dtpToDt").val();

        //    if (fromdt == "" || fromdt == null) {
        //        messageBox("error", "Please select From Date");
        //        return false;
        //    }


        //    if (todt == "" || todt == null) {
        //        messageBox("error", "Please select To Date");
        //        return false;
        //    }

        //    $('#loader').show();
        //    GetAllCompany();
        //    $('#loader').hide();

        //});


    }, 2000);// end timeout

});



//Get Last Company Id
function GetLastCompanyId() {

    //// debugger;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/GetLastCompanyId",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;
            $("#txtcompany_code").val('C0000' + (res.company_id + 1));

        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
            return false;
        }
    });

}


function GetAllCompany() {

    // // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/",
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false
            }



            $("#tblCompanyMaster").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Company List',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    },
                ],
                "columnDefs":
                    [
                        {
                            targets: [9],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [10],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [11],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return new Date(row.last_modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],
                "columns": [
                    { "data": null },
                    { "data": "company_code", "name": "company_code", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "prefix_for_employee_code", "name": "prefix_for_employee_code", "autoWidth": true },
                    { "data": "number_of_character_for_employee_code", "name": "number_of_character_for_employee_code", "autoWidth": true },
                    { "data": "address_line_one", "name": "address_line_one", "autoWidth": true },
                    { "data": "primary_email_id", "name": "primary_email_id", "autoWidth": true },
                    { "data": "primary_contact_number", "name": "primary_contact_number", "autoWidth": true },
                    { "data": "company_website", "name": "company_website", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true },
                    //{ "data": "username", "name": "username", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true },
                    //{ "data": "username", "name": "username", "autoWidth": true },
                    { "data": "last_modified_date", "name": "last_modified_date", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="Create?company_id=' + full.company_id + '" title="Edit"  style=" float: left;"><i class="fa text-success fa-pencil-square-o"></i></a>  <a  onclick="DeleteCompany(' + full.company_id + ')" title = "Delete" > <i class="fa text-danger fa-trash"></i></a > ';
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
        error: function (error) {
            messageBox("error", error.responseText);
            //messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });
}

//Edit Company Master
function GetDataByCompanyId(company_id) {

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
            $("#hdnCompany_logo").val(res[0].company_logo);

            if (res[0].is_active == 1) {
                $("#is_active").attr('checked', 'checked');
            }
            else {
                $("#is_in_active").attr('checked', 'checked');
            }
            if (res[0].is_emp_code_manual_genrate == 1) {
                $("#is_emp_code_manual_genrate").attr('checked', 'checked');
            }



        },
        error: function (error) {
            messageBox("error", error.responseText);
            //messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });
}

function DeleteCompany(Company_Id) {
    $('#loader').show();
    if (confirm("Do you want to delete this?")) {

        // deletion code

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({
            url: localStorage.getItem("ApiUrl") + "apiCompanyMaster/" + Company_Id,
            type: 'Delete',
            dataType: 'json',
            headers: headerss,
            success: function (data) {
                _GUID_New();
                var statuscode = data.statusCode;
                var Msg = data.message;

                //GetAllCompany();
                //if data save
                if (statuscode == "1") {
                    alert(Msg);
                    $('#loader').hide();
                    location.reload();
                }
                else if (statuscode == "0") {
                    alert(Msg);
                    $('#loader').hide();
                }

            },
            error: function (error) {
                _GUID_New();
                messageBox("error", error.responseText);
                //messageBox("error", "Server busy please try again later...!");
                $('#loader').hide();
                return false;
            }
        });


    }

    $('#loader').hide();
    return false;
}


function readFile() {



    //var filee = document.getElementById("File1").files;
    //if (selection.length == 1) {

    //    for (var i = 0; i < selection.length; i++) {
    //        var imgsizee = selection[i].size;
    //        var sizekb = imgsizee / 1024;
    //        sizekb = sizekb.toFixed(0);

    //    //  $('#HFSizeOfPhoto').val(sizekb);
    //        if (sizekb < 10 || sizekb > 500) {
    //            $("#File1").val("");
    //            alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
    //            return false;
    //        }


    //        var ext = selection[i].name.substr(-3);
    //        if (ext != "png" && ext != "jpeg" && ext != "jpg" && ext != "PNG" && ext != "JPEG" && ext != "JPG") {
    //            $("#File1").val('');
    //            alert('Please select only JPG,JPEG files');
    //            return false;

    //        }
    //    }
    //}
    //else {
    //    $("#File1").val('');
    //    alert("Only 1 File can be upload for Logo");
    //    return;
    //}



    // if (this.files && this.files[0]) {

    var imgsizee = this.files[0].size;
    var sizekb = imgsizee / 1024;
    sizekb = sizekb.toFixed(0);

    //  $('#HFSizeOfPhoto').val(sizekb);
    if (sizekb < 10 || sizekb > 500) {
        $("#File1").val("");
        alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
        return false;
    }
    var ftype = this;
    var fileupload = ftype.value;
    if (fileupload == '') {
        $("#File1").val("");
        alert("Photograph only allows file types of PNG, JPG, JPEG. ");
        return;
    }
    else {
        var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
        if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "PNG" || Extension == "JPEG" || Extension == "JPG") {

        }
        else {
            $("#File1").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
    }
    var img_name = this.files[0].name;

    var FR = new FileReader();
    FR.onload = function (e) {
        //  EL("myImg").src = e.target.result;

        EL("HFb64").value = e.target.result;

    };
    //$("#lbl_file_name").text(img_name);
    FR.readAsDataURL(this.files[0]);
    //}
}

function EL(id) { return document.getElementById(id); }

