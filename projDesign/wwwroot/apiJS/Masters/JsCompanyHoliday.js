var emp_role_idd;
var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {


        $('#btnupdate').hide();
        $('#btnsave').show();
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        // var HaveDisplay = ISDisplayMenu("Display Company List");

        BindAllEmp_Company_hlday('ddlcompany', login_emp_id, login_company_id);
       // BindEmployeeCodeFromEmpMasterIncludingAll('ddlemployeetype', login_company_id, 0);
        //BindLocationList('ddllocation', login_company_id, 0);
  
        GetData(login_company_id);

        var currentYear = (new Date).getFullYear();
        $("#txtFromDate").val('01-Jan-' + currentYear);
        $("#txtToDate").val('31-Dec-' + currentYear);

        BindReligionList('ddlReligion', 0);
        //BindEmployementType('ddlemployeetype', 0);

        $('#ddlcompany').bind("change", function () {
            oncompanychange();
        });
        $('#ddllocation').bind("change", function () {
            onlocationchange();
        });

        $('#btnreset').bind('click', function () {

            location.reload();
        });


        $('#btnsave').bind("click", function () {
            $('#loader').show();

            var txtHolidayName = $("#txtHolidayName").val();
            // var txtHolidayDate = $("#txtHolidayDate").val();
            var txtFromDate = $("#txtFromDate").val();
            var txtToDate = $("#txtToDate").val();


            // var ddlcompany = $("#ddlcompany").val();
            var ddlReligion = $("#ddlReligion").val();
            //var ddlemployeetype = $("#ddlemployeetype").val();
            //var ddllocation = $("#ddllocation").val();


            var is_active = 1;
            var errormsg = '';
            var iserror = false;

            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }
            if ($("#is_in_active").is(":checked")) {
                is_active = 0;
            }

            //validation part

            //if (ddlcompany == null || ddlcompany == '' || ddlcompany == 0) {
            //    messageBox("error", "Please Enter Company Name...!");
            //    $('#loader').hide();
            //    return;
            //}
            //if (txtFromDate == null || txtFromDate == '') {
            //    messageBox("error", "Please Enter From Date...!");
            //    $('#loader').hide();
            //    return;
            //}
            //if (txtToDate == null || txtToDate == '') {
            //    messageBox("error", "Please Enter To Date...!");
            //    $('#loader').hide();
            //    return;
            //}
            if (txtHolidayName == null || txtHolidayName == '') {
                messageBox("error", "Please Enter Holiday Name...! ");
                $('#loader').hide();
                return;
            }
            if (txtFromDate == null || txtFromDate == '') {
                messageBox("error", "Please Select From Date...! ");
                $('#loader').hide();
                return;
            }
            if (txtToDate == null || txtToDate == '') {
                messageBox("error", "Please Select To Date...! ");
                $('#loader').hide();
                return;
            }
            //if (txtHolidayDate == null || txtHolidayDate == '') {
            //    messageBox("error", "Please Select Holiday Date...! ");
            //    $('#loader').hide();
            //    return;
            //}


            if (!$("#is_active").is(":checked") && !$("#is_in_active").is(":checked")) {
                messageBox("error", "Please Select Status...! ");
                $('#loader').hide();
                return;
            }

            if (ddlReligion == 0) {
                ddlReligion = 9;
            }



            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }

            var login_emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var selectcompanyall = 0;
            var companyidlist = [];
            $('#ddlcompany > option:selected').each(function () {
                var companyids = {};
                companyids.company_id = $(this).val();
                companyidlist.push(companyids);

                if (companyids.company_id == 0) {
                    selectcompanyall = 1; // all company selected  
                }
            });

            if (selectcompanyall == 1) { // if company all selected then here - all company ids getting
                companyidlist = [];
                $('#ddlcompany > option').each(function () {
                    var companyids = {};
                    companyids.company_id = $(this).val();
                    companyidlist.push(companyids);
                });
            }

            var selectlocall = 0;
            var locationlist = [];
            $('#ddllocation > option:selected').each(function () {
                var locitm = {};
                locitm.location_id = $(this).val();
                locationlist.push(locitm);

                if (locitm.location_id == 0) {
                    selectlocall = 1; // all location selected  
                }
            });

            var selectempall = 0;
            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);

                if (empitm.employee_id == 0) {
                    selectempall = 1; // all employee selected 
                }
            });

            var myData = {

                'holiday_name': txtHolidayName.trim(),
                //'holiday_date': txtHolidayDate,
                'from_date': txtFromDate,
                'to_date': txtToDate,
                'is_applicable_on_all_comp': selectcompanyall,
                'is_applicable_on_all_emp': selectempall,//ddlemployeetype,
                'is_applicable_on_all_religion': ddlReligion,
                'is_applicable_on_all_location': selectlocall,//ddllocation,
                //'company_id': ddlcompany,
                'is_active': is_active,
                'created_by': login_emp_idd,
                'last_modified_by': login_emp_idd,
                'company_id_list': companyidlist,
                'location_id_list': locationlist,
                'emp_id_list': employeelist
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Save_CompanyHoliday';
            var Obj = JSON.stringify(myData);
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            console.log(Obj);
            $.ajax({
                url: apiurl,
                type: "POST",
                data: Obj,
                //data: {
                //    'Obj': JSON.stringify(Obj),
                //    'locationlist': JSON.stringify(locationlist),
                //    'employeelist': JSON.stringify(employeelist)
                //}, 
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        //BindAllEmp_Company_hlday('ddlcompany', login_emp_id, login_company_id);
                        //$("#ddlcompany option[value=" + login_company_id + "]").attr("selected", "selected");
                        //BindLocationListForddl_holiday('ddllocation', login_company_id, 0);
                        //$("#ddllocation option[value='0']").attr("selected", "selected");
                        //$("#txtFromDate").val("");
                        //$("#txtToDate").val("");
                        //$("#txtHolidayName").val("");
                        //$("#txtFromDate").val("");
                        //$("#txtToDate").val("");
                        //// $("#txtHolidayDate").val("");
                        ////$("#ddlcompany").val("0");
                        //$("#ddlReligion").val("0");
                        //BindEmployeeCodeFromEmpMasterIncludingAll('ddlemployeetype', login_company_id, 0);
                        //$("#ddlemployeetype").val("0");

                        ////$("#ddllocation option[text=' --Please Select-- ']").attr("selected", "selected"); 
                        ////$("#ddllocation").prop("selectedIndex", 0); 
                        //GetData(login_company_id);
                        //$('input:radio[name=Status]:checked').prop('checked', false);
                        //$('#btnupdate').hide();
                        //$('#btnsave').show();
                        //$("btnupdate").text('Update').attr("disabled", false);
                        //// $("#select2-ddllocation-container").text(" --Please Select-- ");
                        messageBox("success", Msg);
                        //alert(Msg);
                        setTimeout(function () {
                            location.reload();
                        }, 2000);
                        //location.reload();
                    }
                    else {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
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
                    $('#loader').hide();
                }

            });

        });

        $("#btnupdate").bind("click", function () {
            // 
            $('#loader').show();

            var txtHolidayName = $("#txtHolidayName").val();
            // var txtHolidayDate = $("#txtHolidayDate").val();
            var txtFromDate = $("#txtFromDate").val();
            var txtToDate = $("#txtToDate").val();
            //  var ddlcompany = $("#ddlcompany").val();
            var ddlReligion = $("#ddlReligion").val();
            //var ddlemployeetype = $("#ddlemployeetype").val();
            //var ddllocation = $("#ddllocation").val();

            var is_active = 0;
            if ($("#is_active").is(":checked")) {
                is_active = 1;
            }

            // var is_active = 1;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (txtFromDate == null || txtFromDate == '') {
                errormsg = "Please Enter From Date...! ";
                iserror = true;
            }
            if (txtToDate == null || txtToDate == '') {
                errormsg = "Please Enter To Date...! ";
                iserror = true;
            }
            if (txtHolidayName == null || txtHolidayName == '') {
                errormsg = "Please Enter Holiday Name...! ";
                iserror = true;
            }
            if (txtFromDate == null || txtFromDate == '') {
                errormsg = "Please Enter From Date...! ";
                iserror = true;
            }
            if (txtToDate == null || txtToDate == '') {
                errormsg = "Please Enter To Date...! ";
                iserror = true;
            }
            //if (txtHolidayDate == null || txtHolidayDate == '') {
            //    errormsg = "Please Enter Holiday Date...! ";
            //    iserror = true;
            //}
            //if (ddlcompany == null || ddlcompany == '') {
            //    errormsg = "Please Select Company...! ";
            //    iserror = true;
            //}

            if (!$("#is_active").is(":checked") && !$("#is_in_active").is(":checked")) {
                errormsg = "Please Select Status...! "
                iserror = true;
            }


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                //  messageBox("info", "eror give");
                return false;
            }


            if (ddlReligion == 0) {
                ddlReligion = 9;
            }
            var holiday_id_ = $("#hdnid").val();

            var login_emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

            var selectcompanyall = 0;
            var companyidlist = [];

            $('#ddlcompany > option:selected').each(function () {
                var companyids = {};
                companyids.company_id = $(this).val();
                companyidlist.push(companyids);

                if (companyids.company_id == 0) {
                    selectcompanyall = 1; // all company selected  
                }
            });

            var selectlocall = 0;
            var locationlist = [];
            $('#ddllocation > option:selected').each(function () {
                var locitm = {};
                locitm.location_id = $(this).val();
                locationlist.push(locitm);

                if (locitm.location_id == 0) {
                    selectlocall = 1; // all location selected  
                }
            });

            var selectempall = 0;
            var employeelist = [];
            $('#ddlemployeetype > option:selected').each(function () {
                var empitm = {};
                empitm.employee_id = $(this).val();
                employeelist.push(empitm);

                if (empitm.employee_id == 0) {
                    selectempall = 1; // all employee selected 
                }
            });


            var myData = {

                'holidayno': holiday_id_,
                'holiday_name': txtHolidayName.trim(),
                //'holiday_date': txtHolidayDate,
                'from_date': txtFromDate,
                'to_date': txtToDate,
                'is_applicable_on_all_comp': selectcompanyall,
                'is_applicable_on_all_emp': selectempall,//ddlemployeetype,
                'is_applicable_on_all_religion': ddlReligion,
                'is_applicable_on_all_location': selectlocall,//ddllocation,
                //'company_id': ddlcompany,
                'is_active': is_active,
                'created_by': login_emp_idd,
                'last_modified_by': login_emp_idd,
                'company_id_list': companyidlist,
                'location_id_list': locationlist,
                'emp_id_list': employeelist
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Update_CompanyHoliday';
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
                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        //BindAllEmp_Company_hlday('ddlcompany', login_emp_id, login_company_id);
                        //$("#ddlcompany option[value=" + login_company_id + "]").attr("selected", "selected");
                        //$("#ddlcompany").attr("disabled", false);
                        //BindLocationListForddl_holiday('ddllocation', login_company_id, 0);
                        //$("#ddllocation option[value='0']").attr("selected", "selected");

                        //$("#txtFromDate").val("");
                        //$("#txtToDate").val("");
                        //$("#txtHolidayName").val("");
                        //$("#txtFromDate").val("");
                        //$("#txtToDate").val("");
                        ////$("#txtHolidayDate").val("");
                        ////$("#ddlcompany").val("0");
                        //$("#ddlReligion").val("0");
                        //$("#ddlemployeetype").val("0");
                        //$('#ddlemployeetype').trigger("select2:updated");
                        //$('#ddlemployeetype').select2();
                        ////$('#ddllocation option[value="0"]').attr("selected", "selected");
                        //$('#ddllocation').trigger("select2:updated");
                        //$('#ddllocation').select2();

                        //GetData(login_company_id);
                        //$('input:radio[name=Status]:checked').prop('checked', false);
                        //$('#btnupdate').hide();
                        //$('#btnsave').show();
                        //$("btnupdate").text('Update').attr("disabled", false);
                        messageBox("success", Msg);
                        setTimeout(function () {
                            location.reload();
                        }, 2000);
                        //alert(Msg);
                        //location.reload();
                    }
                    else {
                        messageBox("error", Msg);
                        return false;
                    }
                },
                error: function (request, status, error) {
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
                    $('#loader').hide();
                }

            });

        });

        $('#txtFromDate').change(function () {
            $('#txtToDate').val('');
        });

        $('#txtToDate').change(function () {
            if (Date.parse($("#txtFromDate").val()) > Date.parse($("#txtToDate").val())) {
                messageBox('error', 'To Date must be greater than or equal to from date !!');
                $('#txtToDate').val('');
            }
        });


    }, 2000);// end timeout

});

function BindAllEmp_Company_hlday(ControlId, EmployeeId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;

    var emp_company_lst;
    var lst = localStorage.getItem("emp_companies_lst");

    if (lst != undefined && lst != null && lst != "" && lst.length > 0) {

        emp_company_lst = JSON.parse(lst);
    }
    else {

        var listapi = localStorage.getItem("ApiUrl");

        $.ajax({
            type: "GET",
            url: listapi + "apiEmployee/Get_Emp_all_Company/" + EmployeeId,
            data: {},
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (response) {
                var res = response;

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    $('#loader').hide();
                    return false;
                }

                if (lst == "" || lst == null || lst.length == 0) {
                    localStorage.setItem("emp_companies_lst", JSON.stringify(res));
                }

                emp_company_lst = res;


            },
            error: function (err) {
                alert(err.responseText);
                $('#loader').hide();
                return false;
            }
        });

    }

    if (emp_company_lst != null && emp_company_lst != "" && emp_company_lst.length > 0) {


        if (SelectedVal == 0) {
            // $(ControlId).empty().append('<option selected="selected" value="">--Please Select--</option>');
            $(ControlId).empty().append('<option value="0">All</option>');
        }
        else if (SelectedVal == -1) { $(ControlId).empty().append('<option selected="selected" value="0">--Please Select--</option>'); }
        else {
            $(ControlId).empty();//.append('<option selected="selected" value="0">--Please Select--</option>');
        }

        $.each(emp_company_lst, function (data, value) {

            $(ControlId).append($("<option></option>").val(value.company_id).html(value.company_name));
        });

        //get and set selected value
        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            $(ControlId).val(SelectedVal);
            BindLocationListForddl_holiday('ddllocation', SelectedVal, 0); //bind location 
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        }

        //$(ControlId).trigger("select2:updated");
        //$(ControlId).select2();
    }


    $('#loader').hide();

}

var dd = jQuery.noConflict();
dd(function () {
    setTimeout(function () {
        dd('#ddlcompany').multiselect({
            includeSelectAllOption: true,
            maxHeight: '300',
            buttonWidth: '235',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            onChange: function (option, checked) {
                // alert(option.length + ' options ' + (checked ? 'selected' : 'deselected'));
                oncompanychange();
            },
            onSelectAll: function () {
                // alert("select-all-nonreq");
                oncompanychange();
            },
            onDeselectAll: function () {
                // alert("deselect-all-nonreq");
                oncompanychange();
            }
        });
    }, 3000);
    setTimeout(function () {
        dd('#ddllocation').multiselect({
            includeSelectAllOption: true,
            maxHeight: '300',
            buttonWidth: '235',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            onChange: function (option, checked) {
                // alert(option.length + ' options ' + (checked ? 'selected' : 'deselected'));
                onlocationchange();
            },
            onSelectAll: function () {
                // alert("select-all-nonreq");
                onlocationchange();
            },
            onDeselectAll: function () {
                // alert("deselect-all-nonreq");
                onlocationchange();
            }
        });
    }, 4000);
    setTimeout(function () {
        $('#loader').show();
        dd('#ddlemployeetype').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            includeSelectAllOption: true,
            // includeSelectAllDivider: true,
            // includeResetDivider: true,
            maxHeight: 400,
            //onDropdownShow: function (event) {
            //    alert('Dropdown shown.');
            //    oncompanychange_bindemployee();
            //}
            //onSelectAll: function () {
            //    alert("select-all-nonreq");
            //},
            //onDeselectAll: function () {
            //    alert("deselect-all-nonreq");
            //}
        });
        $('#loader').hide();
    }, 5000);
});


function oncompanychange() {
    dd('#loader').show();
    dd("#ddlemployeetype").empty();
    dd("#ddllocation").empty();
    BindLocation_by_multiple_company('ddllocation', 0);

    // dd('#ddllocation').multiselect('refresh');
    dd('#ddllocation').multiselect('destroy');

    setTimeout(function () {
        dd('#ddllocation').multiselect({
            includeSelectAllOption: true,
            maxHeight: '300',
            buttonWidth: '235',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
        });
    }, 3000);

    //BindEmployee_by_multiple_company('ddlemployeetype', 0);

    // dd('#ddlemployeetype').multiselect('refresh');
    dd("#ddlemployeetype").empty();
   dd('#ddlemployeetype').multiselect('destroy');

    //setTimeout(function () {
    //    $('#loader').show();
    //    dd('#ddlemployeetype').multiselect({
    //        enableFiltering: true,
    //        enableCaseInsensitiveFiltering: true,
    //        includeSelectAllOption: true,
    //        // includeSelectAllDivider: true,
    //        // includeResetDivider: true,
    //        maxHeight: 400,
    //        //onDropdownShow: function (event) {
    //        //    alert('Dropdown shown.');
    //        //    oncompanychange_bindemployee();
    //        //}
    //        //onSelectAll: function () {
    //        //    alert("select-all-nonreq");
    //        //},
    //        //onDeselectAll: function () {
    //        //    alert("deselect-all-nonreq");
    //        //}
    //    });
    //    $('#loader').hide();
    //}, 5000);

    GetData_by_multiple_company();
    dd('#loader').hide();
}
function onlocationchange() {
    //dd('#loader').show();
    dd("#ddlemployeetype").empty();
   // $('#loader').show();
    BindEmployee_by_multiple_location('ddlemployeetype', 0);

    // dd('#ddlemployeetype').multiselect('refresh');
    dd('#ddlemployeetype').multiselect('destroy');

    setTimeout(function () {
        //$('#loader').show();
        dd('#ddlemployeetype').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            includeSelectAllOption: true,          
            maxHeight: 400,           
        });
      //  $('#loader').hide();
    }, 5000);

    GetData_by_multiple_company();
   // dd('#loader').hide();
}

//--------bind data in jquery data table
function GetData(login_company_id) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompanyHoliday/' + login_company_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblCompanyholiday").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
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
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "company.company_name", "name": "company.company_name", "title": "Company", "autoWidth": true },
                    { "data": "holiday_name", "name": "holiday_name", "title": "Holiday Name", "autoWidth": true },
                    //{ "data": "holiday_date", "name": "holiday_date", "title": "Date", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    //{ "data": "created_by", "name": "created_by", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(\'' + full.holiday_no + '\',\'' + full.company.company_id + '\')" ><i class="fa fa-pencil-square-o"></i></a>';
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
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });

}

function GetData_by_multiple_company() {
    companyidlist = [];
    var selectallcomp = false;
    $('#loader').show();
    $('#ddlcompany > option:selected').each(function () {
        var companyids = {};
        companyids.company_id = $(this).val();
        companyidlist.push(companyids);

        if (companyids.company_id == 0) {
            selectallcomp = true;
        }
    });
    if (selectallcomp == true) {
        companyidlist = [];
        var companyids = {};
        companyids.company_id = "0";
        companyidlist.push(companyids);
    }
    var myData = {
        'company_id_list': companyidlist
    };
    var Obj = JSON.stringify(myData);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompanyHolidayByMultiCompany';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //  headerss["salt"] = $("#hdnsalt").val();
    console.log(Obj);
    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (res) {
           
            $("#tblCompanyholiday").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
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
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [5],
                            "class": "text-center"

                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "holiday_name", "name": "holiday_name", "title": "Holiday Name", "autoWidth": true },
                    //{ "data": "holiday_date", "name": "holiday_date", "title": "Date", "autoWidth": true },
                    { "data": "from_date", "name": "from_date", "title": "From Date", "autoWidth": true },
                    { "data": "to_date", "name": "to_date", "title": "To Date", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "title": "Status", "autoWidth": true },
                    //{ "data": "created_by", "name": "created_by", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(\'' + full.holiday_no + '\',\'' + full.company_id + '\')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
            //alert(error);
        }
    });


}



function GetEditData(id, companyid) {
    // 
    $('#loader').show();
    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        $('#loader').hide();
        return false;
    }
    // BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
    BindEmployeeCodeFromEmpMasterIncludingAll('ddlemployeetype', companyid, 0);
    //BindLocationList('ddllocation', login_company_id, 0);
    BindLocationListForddl_holiday('ddllocation', companyid, 0);

    dd('#ddllocation').multiselect('destroy');

    setTimeout(function () {
        dd('#ddllocation').multiselect({
            includeSelectAllOption: true,
            maxHeight: '300',
            buttonWidth: '235',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
        });
    }, 3000);

    // dd('#ddlemployeetype').multiselect('refresh');
    dd('#ddlemployeetype').multiselect('destroy');

    setTimeout(function () {
        $('#loader').show();
        dd('#ddlemployeetype').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            includeSelectAllOption: true,
            // includeSelectAllDivider: true,
            // includeResetDivider: true,
            maxHeight: 400,

            //onSelectAll: function () {
            //    alert("select-all-nonreq");
            //},
            //onDeselectAll: function () {
            //    alert("deselect-all-nonreq");
            //}
        });
        $('#loader').hide();
    }, 5000);

    $("#hdnid").val(id);

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CompanyHolidayById/' + id + "/" + companyid;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            //$("#txtFromDate").val(GetOnlyDate(data.from_date));
            //$("#txtToDate").val(GetOnlyDate(data.to_date));
            $("#txtHolidayName").val(data.holiday_name);
            $("#txtFromDate").val(GetDateFormatddMMyyyy(new Date(data.from_date)));
            $("#txtToDate").val(GetDateFormatddMMyyyy(new Date(data.to_date)));
            //$("#txtHolidayDate").val(GetDateFormatyyyyMMdd(new Date(data.holiday_date)));

            BindAllEmp_Company_hlday('ddlcompany', login_emp_id, data.company_id)
            //BindCompanyList('ddlcompany', data.company_id);
            dd('#ddlcompany').multiselect('refresh');
            $("#ddlcompany").attr("disabled", true);


            if (data.is_applicable_on_all_religion == "1") {
                BindReligionList('ddlReligion', 0);
            }
            else {
                BindReligionList('ddlReligion', data.is_applicable_on_all_religion);
            }
            //if (data.is_applicable_on_all_religion == 0 || data.is_applicable_on_all_religion == 9) {
            //    BindReligionList('ddlReligion', 0);
            //}
            //else {
            //    BindReligionList('ddlReligion', data.is_applicable_on_all_religion);
            //}


            if (data.is_active == 0) {
                $("#is_in_active").prop('checked', true);
            }
            else {
                //$("#is_active").prop('checked', false);
                $("#is_active").prop("checked", true)
            }

            $('#ddlemployeetype option[value="0"]').prop('selected', false);
            if (data.is_applicable_on_all_emp == 0) {
                for (var i = 0; i < data.emplist.length; i++) {
                    $('#ddlemployeetype').find('option').each(function () {
                        if (data.emplist[i] == $(this).val()) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }
            }
            else {
                $('#ddlemployeetype option[value="0"]').prop('selected', true);
            }
           
            dd('#ddlemployeetype').multiselect({
                    includeSelectAllOption: true,
                    maxHeight: 400,
                   // buttonWidth: '235',
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,

                });
            dd('#ddlemployeetype').multiselect('refresh');

            //$('#ddlemployeetype').trigger("select2:updated");
            // $('#ddlemployeetype').select2();

            $('#ddllocation option[value="0"]').prop('selected', false);

            if (data.is_applicable_on_all_location == "0") {

                for (var i = 0; i < data.locaitonlist.length; i++) {
                    $('#ddllocation').find('option').each(function () {
                        if (data.locaitonlist[i].location_id == $(this).val()) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }
            }
            else {
                $('#ddllocation option[value="0"]').prop('selected', true);
            }

            
            dd('#ddllocation').multiselect({
                includeSelectAllOption: true,
                maxHeight: 400,
               // buttonWidth: '235',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,

            });
            dd('#ddllocation').multiselect('refresh');
            // $('#ddllocation').trigger("select2:updated");
            // $('#ddllocation').select2();



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

var companyidlist = [];
function BindEmployee_by_multiple_company(ControlId, SelectedVal) {
    companyidlist = [];
    var selectallcomp = false;
    $('#ddlcompany > option:selected').each(function () {
        var companyids = {};
        companyids.company_id = $(this).val();
        companyidlist.push(companyids);

        if (companyids.company_id == 0) {
            selectallcomp = true;
        }
    });
    if (selectallcomp == true) {
        companyidlist = [];
        var companyids = {};
        companyids.company_id = "0";
        companyidlist.push(companyids);
    }
    var myData = {
        'company_id_list': companyidlist
    };
    var Obj = JSON.stringify(myData);
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_EmpNameAndCodeByMultiComp';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //  headerss["salt"] = $("#hdnsalt").val();
    console.log(Obj);
    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (response) {
            var res = response;

            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }


            // $(ControlId).trigger("select2:updated");
            // $(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });

}


var locationidlist = [];
function BindEmployee_by_multiple_location(ControlId, SelectedVal) {
    $('#loader').show();
    debugger;
    locationidlist = [];
    var selectallloc = false;
    $('#ddllocation > option:selected').each(function () {
        var locationids = {};
        locationids.location_id = $(this).val();
        locationidlist.push(locationids);

        if (locationids.location_id == 0) {
            selectallloc = true;
        }
    });
    if (selectallloc == true) {
        locationidlist = [];
        var locationids = {};
        locationids.location_id = "0";
        locationidlist.push(locationids);
    }
    var myData = {
        'location_id_list': locationidlist
    };
    var Obj = JSON.stringify(myData);
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_EmpNameAndCodeByMultiLoc';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //  headerss["salt"] = $("#hdnsalt").val();
    console.log(Obj);
    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (response) {
            var res = response;

            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            $('#loader').hide();

            // $(ControlId).trigger("select2:updated");
            // $(ControlId).select2();
        },
        error: function (err) {
            $('#loader').hide();
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });

}
function BindLocation_by_multiple_company(ControlId, SelectedVal) {

    companyidlist = [];
    var selectallcomp = false;
    $('#ddlcompany > option:selected').each(function () {
        var companyids = {};
        companyids.company_id = $(this).val();
        companyidlist.push(companyids);

        if (companyids.company_id == 0) {
            selectallcomp = true;
        }
    });
    if (selectallcomp == true) {
        companyidlist = [];
        var companyids = {};
        companyids.company_id = "0";
        companyidlist.push(companyids);
    }
    var myData = {
        'company_id_list': companyidlist
    };
    var Obj = JSON.stringify(myData);
    ControlId = '#' + ControlId;
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationByMultiCompany';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    //  headerss["salt"] = $("#hdnsalt").val();
    console.log(Obj);
    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (response) {
            var res = response;
            debugger;
            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }


            // $(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });

}
function BindEmployeeCode_by_location(ControlId, locationids, SelectedVal) {
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmpNameAndCodeByLoc/" + locationids,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });
}
function BindEmployeeCodeFromEmpMasterIncludingAll(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmpNameAndCodeByeComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_name_code));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
        },
        error: function (err) {
            // alert(err.responseText);
            messageBox("error", err.responseText);
        }
    });
}

//bind loaction by company id 

function BindLocationListForddl_holiday(ControlId, CompanyId, SelectedVal) {


    ControlId = '#' + ControlId;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiMasters/Get_LocationByCompany/' + CompanyId,
        data: {},
        contentType: "application/json; charset=utf-8",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty();//.append('<option selected="selected" value="0">All</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }
            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();
            //var res = response;
            //$(ControlId).empty().append('<option selected="selected" value="0"> All </option>');
            //if (res.statusCode != undefined) {
            //    $("#loader").hide();
            //    messageBox("error", res.message);
            //    return false;
            //}
            //$.each(res, function (data, value) {
            //    $(ControlId).append($("<option></option>").val(value.location_id).html(value.location_name));
            //});


            ////get and set selected value
            //if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
            //    $(ControlId).val(SelectedVal);
            //}

            //$(ControlId).trigger("select2:updated");
            //$(ControlId).select2();

            $('#loader').hide();
        },
        error: function (err) {
            alert(err.responseText);
            $('#loader').hide();
        }
    });
}


function GetDateFormatyyyyMMdd(date) {
    var month = (date.getMonth() + 1).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return date.getFullYear() + '-' + month + '-' + day;
}
