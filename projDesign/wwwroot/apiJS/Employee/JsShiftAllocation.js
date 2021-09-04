$('#loader').show();
var login_emp_id;
var default_company;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        BindAllShiftListForddl('ddlShift', $("#ddlCompany").val(), 0);

        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');

            GetEmployeeShiftAllocation(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }


        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id"))); 
            GetEmployeeShiftAllocation(0);
        });

        $('#ddlEmployeeCode').change(function () {
            $('#ddlShift').val(0);
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));    
            GetEmployeeShiftAllocation($(this).val());
        });


        $('#loader').hide();


        $('#btnSaveShiftAllocation').bind("click", function () {
            $('#loader').show();
            var ddlCompany = $('#ddlCompany').val();
            var employee_id = $('#ddlEmployeeCode :selected').val();
            var ddlShift = $("#ddlShift").val();
            var shiftallocfromdate = $("#shiftallocfromdate").val();
            // var shiftalloctodate = $("#shiftalloctodate").val();
            if (ddlCompany == "0" || ddlCompany == null || ddlCompany == "") {
                messageBox("error", "Enter the company");
                $('#loader').hide();
                return;
            }
            if (employee_id == "0" || employee_id == null || employee_id == "") {
                messageBox("error", "Enter the Employee Code");
                $('#loader').hide();
                return;
            }
            if (ddlShift == "0" || ddlShift == null || ddlShift == "") {
                messageBox("error", "Enter the Shift");
                $('#loader').hide();
                return;
            }
            if (shiftallocfromdate == "0" || shiftallocfromdate == null || shiftallocfromdate == "") {
                messageBox("error", "Enter the From Date");
                $('#loader').hide();
                return;
            }

            var myData = {
                'employee_id': employee_id,
                'shift_id': ddlShift,
                'applicable_from_date': shiftallocfromdate,
                //'applicable_to_date': shiftalloctodate
            };

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeShiftAllocation',
                type: "POST",
                data: JSON.stringify(myData),
                dataType: "json",
                contentType: "application/json",
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();
                    //if data save
                    if (statuscode == "1") {
                        alert(Msg);
                        location.reload();
                        //messageBox("success", Msg);
                        // window.location.href = '/Employee/ShiftAllocation';
                    }
                    else if (statuscode == "0") {
                        messageBox("error", "Something went wrong please try again...!");
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



//////////////////////////////////////////////////// START SHIFT Allocation /////////////////////////////////////////////////////





function GetEmployeeShiftAllocation(employee_id) {



    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeShiftAllocation/' + employee_id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {

                if (data != null && data != "undefined") {
                    var res = data;
                    console.log( res)


                    //$("#ddlCompany").val(res.ddlcompany);
                    //$("#ddlEmployeeCode").val(res.employee_id);'
                    BindAllShiftListForddl('ddlShift', $("#ddlCompany").val(), res.shift_id);
                    //BindAllShiftListForddl('ddlShift', $("#ddlCompany").val(), 0);
                    //$("#ddlShift").val(res.shift_id);

                    //$("#ddlShift").val(res.shift_id);

                    $("#shiftallocfromdate").val(GetDateFormatddMMyyyy(new Date(res.applicable_from_date)));
                    $("#shiftalloctodate").val(GetDateFormatddMMyyyy(new Date(res.applicable_to_date)));
                    $("#tblShiftAllocation").DataTable({
                        "processing": true, // for show progress bar
                        "serverSide": false, // for process server side
                        "bDestroy": true,
                        "filter": true, // this is for disable filter (search box)
                        "orderMulti": false, // for disable multiple column at once
                        //"scrollY": 200,
                        "aaData": res.shift_Data,
                        "columnDefs":
                            [
                                {
                                    targets: [1, 5],
                                    render: function (data, type, row) {

                                        var date = new Date(data);
                                        return GetDateFormatddMMyyyy(date);
                                    }
                                }
                                //{
                                //    targets: [5],
                                //    "class": "text-center"

                                //}
                            ],

                        "columns": [
                            { "data": "emp_shift_id", "name": "emp_shift_id", "title": "SNo.", "autoWdith": true, "visible": false },
                            { "data": "applicable_from_date", "name": "applicable_from_date", "title": "Effective FromDt", "autoWidth": true },
                            { "data": "shift_id", "name": "shift_id", "title": "Shift Id", "autoWidth": false },
                            { "data": "shift_code", "name": "shift_code", "title": "Shift Code", "autoWidth": true },
                            { "data": "shift_name", "name": "shift_name", "title": "Shift Name", "autoWidth": true },
                            { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                            //{
                            //    "title": "Action", "autoWidth": true,
                            //    "render": function (data, type, full, meta) {
                            //        return '<a href="#" onclick="DeleteEmpWeekOff(' + full.emp_weekoff_id + ')" ><i class="fa fa-pencil-square-o">Delete</i></a>';
                            //    }
                            //}
                        ],
                        //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        //    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        //    return nRow;
                        //},
                        "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                    });


                    $('#loader').hide();
                }
                else {
                    messageBox("error", "Shift Not allocated");

                    $('#loader').hide();
                    return false;
                }
                //(JSON.stringify(data));


            },
            error: function (error) {
                $('#loader').hide();
                messageBox("error", error.responseText);

            }
        });

    }

}

//////////////////////////////////////////////////// END SHIFT Allocation /////////////////////////////////////////////////////
