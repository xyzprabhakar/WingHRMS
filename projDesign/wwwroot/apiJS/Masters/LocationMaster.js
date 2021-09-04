
var login_emp;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        $('#btnreset').bind('click', function () {

            location.reload();
        });

        $('#btnsave').bind("click", function () {

            var location_name = $.trim($("#txtlocationname").val());
            var type_of_location = $("#ddllocationtype").val();
            var address_line_one = $("#txtaddress1").val();
            var address_line_two = $("#txtaddress2").val();
            var pin_code = $("#txtpincode").val();
            var city = $("#ddlcity").val();
            var state = $("#ddlstate").val();
            var country = $("#ddlcountry").val();
            var primary_email_id = $("#txtprimaryemail").val();
            var secondary_email_id = $("#txtsecondaryemail").val();
            var primary_contact_number = $("#txtprimaryno").val();
            var secondary_contact_number = $("#txtsecondaryno").val();
            var company_id = $("#ddlcompany").val();
            var website = $("#txtwebsite").val();
            var image = $("#HFb64").val();
            var is_active = "";
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = true;
                }
                else if ($("input[name='chkstatus']:checked").val() == '0') {
                    is_active = false;
                }

            }

            login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


            //validation part
            if (company_id == '0' || company_id == '' || company_id == null) {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (location_name == null || location_name == '') {
                errormsg = errormsg + "Please enter location name !! <br/>";
                iserror = true;
            }
            if (type_of_location == null || type_of_location == '') {
                errormsg = errormsg + "Please enter location name !! <br/>";
                iserror = true;
            }
            if (address_line_one == null || address_line_one == '') {
                errormsg = errormsg + "Please enter address line 1 !! <br/>";
                iserror = true;
            }
            if (primary_email_id == null || primary_email_id == '') {
                errormsg = errormsg + "Please enter primary email id!! <br/>";
                iserror = true;
            }
            if (country == '0' || country == '') {
                errormsg = errormsg + "Please select country name !! <br/>";
                iserror = true;
            }
            if (state == '0' || state == '') {
                errormsg = errormsg + "Please select state name !! <br/>";
                iserror = true;
            }
            if (city == '0' || city == '') {
                errormsg = errormsg + "Please select city name !! <br/>";
                iserror = true;
            }
            if (primary_contact_number == null || primary_contact_number == '') {
                errormsg = errormsg + "Please enter primary contact number !! <br/>";
                iserror = true;
            }
            if ($("input[name='chkstatus']:checked").val() == "" || $("input[name='chkstatus']:checked").val() == null) {
                errormsg = errormsg + "Please select active/inactive status !! <br/>";
                iserror = true;
            }
            if (pin_code == null || pin_code == '') {
                errormsg = errormsg + "Please enter Pin  Code !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var myData = {

                'location_name': location_name,
                'type_of_location': type_of_location,
                'address_line_one': address_line_one,
                'address_line_two': address_line_two,
                'pin_code': pin_code,
                'city': city,
                'state': state,
                'country': country,
                'primary_email_id': primary_email_id,
                'secondary_email_id': secondary_email_id,
                'primary_contact_number': primary_contact_number,
                'secondary_contact_number': secondary_contact_number,
                'company_id': company_id,
                'website': website,
                'image': image,
                'is_active': is_active,
                'created_by': login_emp
            };

            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiLocationMaster';
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
                    // // debugger;
                    // var resp = JSON.parse(data);
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        window.location.href = "/Masters/DetailLocationMaster";
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
                    $('#loader').hide();
                    _GUID_New();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });

        $("#btnupdate").bind("click", function () {

            //var qs = getQueryStrings();
            var locationid = $("#hdn_loc_id").val();//qs["id"];

            if (!Validate()) {
                // $('#loader').hide();
                return false;
            }
            var location_name = $.trim($("#txtlocationname").val());
            var type_of_location = $("#ddllocationtype").val();
            var address_line_one = $("#txtaddress1").val();
            var address_line_two = $("#txtaddress2").val();
            var pin_code = $("#txtpincode").val();
            var city = $("#ddlcity").val();
            var state = $("#ddlstate").val();
            var country = $("#ddlcountry").val();
            var primary_email_id = $("#txtprimaryemail").val();
            var secondary_email_id = $("#txtsecondaryemail").val();
            var primary_contact_number = $("#txtprimaryno").val();
            var secondary_contact_number = $("#txtsecondaryno").val();
            var company_id = $("#ddlcompany").val();
            var website = $("#txtwebsite").val();
            var image = $("#HFb64").val();
            //adding lines of logic to set and reset is_active 
            //if (is_active) {
            //    var is_active = false;
            //}
            //else {
            //    var is_active = true;
            //}
            //added logic 
            var is_active = "";
            //if ($("#chkstatus").is(":checked")) {
            //    is_active = true;
            //}
            if ($("input[name='chkstatus']:checked")) {
                if ($("input[name='chkstatus']:checked").val() == '1') {
                    is_active = true;
                }
                else if ($("input[name='chkstatus']:checked").val() == '0') {
                    is_active = false;
                }

            }
            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            var emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });



            var myData = {

                'location_id': locationid,
                'location_name': location_name,
                'type_of_location': type_of_location,
                'address_line_one': address_line_one,
                'address_line_two': address_line_two,
                'pin_code': pin_code,
                'city': city,
                'state': state,
                'country': country,
                'primary_email_id': primary_email_id,
                'secondary_email_id': secondary_email_id,
                'primary_contact_number': primary_contact_number,
                'secondary_contact_number': secondary_contact_number,
                'company_id': company_id,
                'website': website,
                'image': image,
                'is_active': is_active,
                'created_by': login_empid
            };

            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiLocationMaster/Puttbl_location_master';
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
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        window.location.href = "/Masters/DetailLocationMaster";
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
                    $('#loader').hide();
                    _GUID_New();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }
            });
        });


        $("#btnreset_sub_loc").bind("click", function () {
            location.reload();
        });

        $('#btnsave_sub_loc').bind("click", function () {

            // debugger;
            var location_name = $("#txtsublocationname").val();
            // var sub_location_code = $('#txtsublocationcode').val();
            var location_id = $('#ddllocation_sub_loc').val();
            var company_id = $('#ddlcompany').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus_sub_loc']:checked")) {
                if ($("input[name='chkstatus_sub_loc']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (location_name == null || location_name == '') {
                errormsg = "Please enter sub location name !! <br/>";
                iserror = true;
            }
            //if (sub_location_code == null || sub_location_code == '') {
            //    errormsg = errormsg + "Please enter sub location code !! <br/>";
            //    iserror = true;
            //}
            if (company_id == null || company_id == '0') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (location_id == null || location_id == '0') {
                errormsg = errormsg + "Please select location name !! <br/>";
                iserror = true;
            }

            if (!$("input[name='chkstatus_sub_loc']:checked").val()) {

                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }
            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var myData = {

                'location_id': location_id,
                'location_name': location_name,
                'company_id': company_id,
                'is_active': is_active,
                'created_by': login_empid,
            };

            $('#loader').show();

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_SubLocationMaster';
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
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        window.location.href = "/Masters/DetailLocationMaster";
                        //location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
                    $('#loader').hide();
                    var error = "";
                    _GUID_New();
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


        $("#btnupdate_sub_loc").bind("click", function () {

            // debugger;
            var sub_location_id = $("#hdn_sub_loc_id").val();
            var location_name = $("#txtsublocationname").val();
            var location_id = $('#ddllocation_sub_loc').val();
            var company_id = $('#ddlcompany').val();

            var is_active = 0;
            var errormsg = '';
            var iserror = false;

            if ($("input[name='chkstatus_sub_loc']:checked")) {
                if ($("input[name='chkstatus_sub_loc']:checked").val() == '1') {
                    is_active = 1;
                }
            }

            //validation part
            if (location_name == null || location_name == '') {
                errormsg = "Please enter sub location name !! <br/>";
                iserror = true;
            }
            if (company_id == null || company_id == '0') {
                errormsg = errormsg + "Please select company name !! <br/>";
                iserror = true;
            }
            if (location_id == null || location_id == '0') {
                errormsg = errormsg + "Please select location name !! <br/>";
                iserror = true;
            }
            if (!$("input[name='chkstatus_sub_loc']:checked").val()) {

                errormsg = errormsg + "Please select active/in-active status !! <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }

            var login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


            var myData = {

                'sub_location_id': sub_location_id,
                'location_name': location_name,
                'company_id': company_id,
                'location_id': location_id,
                'is_active': is_active,
                'last_modified_by': login_empid,
            };

            $('#loader').show();

            $("btnupdate").attr("disabled", true).html('<i class="fa fa-spinner"></i> Please wait');

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_SubLocationMaster';
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
                    _GUID_New();
                    if (statuscode == "0") {
                        $("btnupdate").text('Update').attr("disabled", false);
                        alert(Msg);
                        window.location.href = "/Masters/DetailLocationMaster";
                        // location.reload();
                    }
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                        $("btnupdate").text('Update').attr("disabled", false);
                        return false;
                    }
                },
                error: function (request, status, error) {
                    $('#loader').hide();
                    _GUID_New();
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


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData() {
    // // debugger;
    //var qs = getQueryStrings();
    //var locationid = qs["id"];
    ////var cityid = $("#hdnid").val();
    //locationid = locationid == '' || locationid == 'undefined' ? 0 : locationid;
    //var myData = {
    //    'locationid': '0'      
    //};
    var apiurl = "";
    //var company_idd = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0)
    //{
    apiurl = localStorage.getItem("ApiUrl") + 'apiLocationMaster/Gettbl_location_masterByCompID/' + $("#ddlcompany").val() + "/" + $("#dtpFromDt").val() + "/" + $("#dtpToDt").val() + "/0";
    //}
    //else {
    // apiurl = localStorage.getItem("ApiUrl") + 'apiLocationMaster/0';
    //}

    $("#loader").show();
    $.ajax({
        type: "GET",
        url: apiurl,
        //data: JSON.stringify(myData),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //  // debugger;
            $("#tbllocation").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Location List',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8]
                        }
                    },
                ],
                "columnDefs":
                    [
                        {
                            targets: [7],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },

                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Comapny", "autoWidth": true },
                    { "data": "type_of_location", "name": "type_of_location", "title": "Location Type", "autoWidth": true },
                    { "data": "location_code", "name": "location_code", "title": "Location Code", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "state", "name": "state", "title": "State", "autoWidth": true },
                    { "data": "city", "name": "city", "title": "City", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    { "data": "created_on", "name": "created_on", "title": "Created On", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            // return '<a href="#" onclick="GetEditData(' + full.locationid+')"><i class="fa fa-pencil-square-o"></i></a>';
                            return '<a href="/Masters/AddLocationMaster?id=' + full.location_id + '_loc" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });

            $("#loader").hide();
        },
        error: function (error) {
            $("#loader").hide();
            alert(error.responseText);
        }
    });

}

function GetEditData(locationid) {
    // debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiLocationMaster/' + locationid;

    $.ajax({
        type: "GET",
        url: apiurl,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            // debugger;
            data = res;

            var emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            $("#txtlocationname").val(data.location_name);
            $('#txtlocationcode').val(data.location_code);
            $("#txtaddress1").val(data.address_line_one);
            $("#txtaddress2").val(data.address_line_two);
            $("#txtpincode").val(data.pin_code);
            $("#txtprimaryemail").val(data.primary_email_id);
            $("#txtsecondaryemail").val(data.secondary_email_id);
            $("#txtprimaryno").val(data.primary_contact_number);
            $("#txtsecondaryno").val(data.secondary_contact_number);
            $("#txtwebsite").val(data.website);
            // $('#chkstatus').prop("checked", data.is_active == 1 ? true : false);
            $("input[name=chkstatus][value=" + data.is_active + "]").attr('checked', 'checked');

            // BindAllEmp_Company('ddlcompany', login_emp, data.company_id);

            $('#ddlcompany option[value="' + data.company_id + '"]').attr("selected", "selected");
            $('#ddlcompany').trigger("select2:updated");
            $('#ddlcompany').select2();



            BindCountryList('ddlcountry', data.country_id);
            BindStateList('ddlstate', data.state_id, data.country_id);
            BindCityList('ddlcity', data.state_id, data.city_id);
            BindLocationType('ddllocationtype', data.type_of_location);

            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}


//-------update city data

function Rest() {
    $('#loader').show();
    $("#txtlocationname").val('');
    $("#ddllocationtype").val('0');
    $("#txtaddress1").val('');
    $("#txtaddress2").val('');
    $("#txtpincode").val('');
    $("#ddlcity").val('0');
    $("#ddlstate").val('0');
    $("#ddlcountry").val('0');
    $("#txtprimaryemail").val('');
    $("#txtsecondaryemail").val('');
    $("#txtprimaryno").val('');
    $("#txtsecondaryno").val('');
    $("#ddlcompany").val('0');
    $("#txtwebsite").val('');
    $("#File1").val('');
    // $("#chkstatus").prop("checked", false);
    $('input:radio[name=chkstatus]:checked').prop('checked', false);
    $("#txtlocationcode").val('');
    $('#loader').hide();
}

function readFile() {


    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 100) {
            $("#File1").val("");
            alert('The size of the photograph should fall between 20KB to 100KB. Your Photo Size is ' + sizekb + 'kb.');
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
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg") {

            }
            else {
                $("#File1").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); } // Get el by ID helper function


function Validate() {

    var location_name = $("#txtlocationname").val();
    var type_of_location = $("#ddllocationtype").val();
    var address_line_one = $("#txtaddress1").val();
    var address_line_two = $("#txtaddress2").val();
    var pin_code = $("#txtpincode").val();
    var city = $("#ddlcity").val();
    var state = $("#ddlstate").val();
    var country = $("#ddlcountry").val();
    var primary_email_id = $("#txtprimaryemail").val();
    var secondary_email_id = $("#txtsecondaryemail").val();
    var primary_contact_number = $("#txtprimaryno").val();
    var secondary_contact_number = $("#txtsecondaryno").val();
    var company_id = $("#ddlcompany").val();
    var website = $("#txtwebsite").val();
    var image = $("#HFb64").val();
    var is_active = false;
    var errormsg = '';
    var iserror = false;

    var emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0)
    //{
    //    company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
    //}

    //
    //validation part
    if (company_id == '0' || company_id == '' || company_id == null) {
        errormsg = "Please select company name !! <br/>";
        iserror = true;
    }
    if (location_name == null || location_name == '') {
        errormsg = errormsg + "Please enter location name !! <br/>";
        iserror = true;
    }
    if (type_of_location == null || type_of_location == '') {
        errormsg = errormsg + "Please enter location name !! <br/>";
        iserror = true;
    }
    if (address_line_one == null || address_line_one == '') {
        errormsg = errormsg + "Please enter address line 1 !! <br/>";
        iserror = true;
    }
    if (country == '0' || country == '') {
        errormsg = "Please select country name !! <br/>";
        iserror = true;
    }
    if (state == '0' || state == '') {
        errormsg = "Please select state name !! <br/>";
        iserror = true;
    }
    if (city == '0' || city == '') {
        errormsg = "Please select city name !! <br/>";
        iserror = true;
    }
    if (primary_contact_number == null || primary_contact_number == '') {
        errormsg = errormsg + "Please enter primary contact number !! <br/>";
        iserror = true;
    }
    if (pin_code == null || pin_code == '') {
        errormsg = errormsg + "Please enter Pin  Code !! <br/>";
        iserror = true;
    }

    // debugger;
    var a = $("input[name='chkstatus']:checked").val();
    if (!$("input[name='chkstatus']:checked").val()) {

        errormsg = errormsg + "Please select active/in-active status !! <br/>";
        iserror = true;
    }


    if (iserror) {
        // messageBox("success", "successfully save data");
        messageBox("error", errormsg);

        //  messageBox("info", "eror give");
        return false;
    }

    return true;
}

// START for SUB LOCATION

function GetSub_Location_DataByCompany() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubLocationMasterByCompany/' + $("#ddlcompany").val() + "/" + $("#dtpFromDt").val() + "/" + $("#dtpToDt").val() + "/0";
    $("#loader").show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblsublocation").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Sub Location List',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6]
                        }
                    },
                ],
                "columnDefs":
                    [
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                        ,
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "SNo", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "sub_location_name", "name": "sub_location_name", "title": "SubLocation", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "type_of_location", "name": "type_of_location", "title": "Location Type", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    { "data": "created_on", "name": "created_on", "title": "Created On", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="/Masters/AddLocationMaster?id=' + full.sub_location_id + '_sub" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });

            $("#loader").hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });

}

function Get_SubLocation_Data() {

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SubLocationMaster/0';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $("#tblsublocation").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'InActive'
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                        ,
                        {
                            targets: [7],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "SNo", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "sub_location_name", "name": "sub_location_name", "title": "SubLocation", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location", "autoWidth": true },
                    { "data": "type_of_location", "name": "type_of_location", "title": "Location Type", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    { "data": "created_on", "name": "created_on", "title": "Created On", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="/Masters/AddLocationMaster?id=' + full.sub_location_id + '_sub" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
        },
        error: function (error) {
            //alert(error);
            messageBox("error", error.responseText);
        }
    });

}


function GetEdit_sub_loc_Data(id) {

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        // $('#loader').hide();
        return false;
    }
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_SublocationMaster/' + id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $("#txtsublocationname").val(data.location_name);

            $('#ddlcompany option[value="' + data.company_id + '"]').attr("selected", "selected");
            $('#ddlcompany').trigger("select2:updated");
            $('#ddlcompany').select2();
            //BindAllEmp_Company('ddlcompany', login_emp, data.company_id);
            // $('#txtsublocationcode').val(data.sub_department_code);

            //  BindLocationList('ddllocation_sub_loc', data.company_id, -1);
            $('#ddllocation_sub_loc option[value="' + data.location_id + '"]').attr("selected", "selected");
            $('#ddllocation_sub_loc').trigger("select2:updated");
            $('#ddllocation_sub_loc').select2();
            // $("#ddllocation_sub_loc").val(data.location_id)



            $("input[name=chkstatus_sub_loc][value=" + data.is_active + "]").prop('checked', true);

            $("#hdnid").val(id);
            $('#btnupdate_sub_loc').show();
            $('#btnsave_sub_loc').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
            return false;
        }
    });
}


//END for SUB LOCATION


