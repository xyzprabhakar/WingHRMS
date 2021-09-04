$('#loader').show();

var emp_idd;
var default_company;


$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', emp_idd, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', emp_idd, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }

        // var HaveDisplay = ISDisplayMenu("Display Company List");


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            if (CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }) > 0) {
                GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            }
        }

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));  
            $("#File1").val('');
            $("#txtremarks").val('');
            if ($.fn.DataTable.isDataTable('#tblhealthcard')) {
                $('#tblhealthcard').DataTable().clear().draw();
            }
        });

        $('#ddlEmployeeCode').change(function () {
            // localStorage.setItem("new_emp_id", $(this).val());
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id"))); 

            if ($(this).val() > 0) {
                GetData($(this).val());
            }
            else {
                if ($.fn.DataTable.isDataTable('#tblhealthcard')) {
                    $('#tblhealthcard').DataTable().clear().draw();
                }
            }

        });



        $('#loader').hide();

        $("#btnreset").bind("click", function () {
            location.reload();
        });

        EL("File1").addEventListener("change", readFile, false);

        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlCompany").val();
            var employee_id = $("#ddlEmployeeCode").val();

            var remarks = $("#txtremarks").val();

            if (login_role_id == 2)//For Admin
            {
                company_id = default_company;
                //employee_id = localStorage.getItem("emp_id");
            }

            if (company_id == "0") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (employee_id == "0") {
                errormsg = errormsg + "Please Select Employee </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var mydata = {

                company_id: company_id,
                employee_id: employee_id,
                //health_card_path: employee_photo_path,
                remarks: remarks,
                created_by: emp_idd
            };

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("File1").files[0];


            //var files = document.getElementById("File1").files;
            //var formData = new FormData();

            //for (var i = 0; i < files.length; i++) {
            //    formData.append("fileInput", files[i]);
            //}

            //formData.append('AllData', Obj);
            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);
            //formData.append('fileData', fileData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_HealthCard/",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {

                    var statuscode = data.statusCode;
                    var Msg = data.message;
                    $('#loader').hide();
                    _GUID_New();


                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                    }
                    else if (statuscode == "1" || statuscode == "2") {
                        messageBox("error", Msg);
                    }
                },
                error: function (error) {
                    _GUID_New();
                    messageBox("error", error.responseText);
                    $('#loader').hide();
                }
            });
        });

    }, 2000);// end timeout

});




function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#File1").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG,PDF ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "pdf") {

            }
            else {
                $("#File1").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG,PDF");
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

function EL(id) { return document.getElementById(id); }


function GetData(empidd) {

    if ($.fn.DataTable.isDataTable('#tblhealthcard')) {
        $('#tblhealthcard').DataTable().clear().draw();
    }

    if (empidd > 0) {

        $('#loader').show();
        var apiurll = "";

        //var EmpIds = 0;
        //if (localStorage.getItem("new_emp_id") != null) {
        //    CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        //}
        //if (EmpIds == null) {
        //    EmpIds = 0
        //}
        //else if (EmpIds == '') {
        //    EmpIds = 0
        //}

        //var HaveDisplay = ISDisplayMenu("Display Company List");
        //if (HaveDisplay == 0) {

        //    apiurll = localStorage.getItem("ApiUrl") + "/apiEmployee/Get_health_card_masterByCompID/" + default_company;
        //}
        //else {
        apiurll = localStorage.getItem("ApiUrl") + "/apiEmployee/Get_health_card_master/0/" + empidd;
        //}
        $.ajax({
            url: apiurll,
            type: "GET",
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            data: {},
            success: function (response) {
                var res = response;
                $('#loader').hide();

                if (res.statusCode != undefined) {
                    messageBox("info", res.message);
                    return false;
                }

                var data_ = [];
                data_.push(res);

                $("#tblhealthcard").DataTable({
                    "processing": false,//for show progress bar
                    "serverSide": false,// for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at one
                    "scrollX": 200,
                    "aaData": data_,
                    "columnDefs": [
                        {
                            targets: [5],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                        //{
                        //    targets: [6],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);

                        //    }
                        //}

                    ],
                    "columns": [

                        { "data": null, "title": "S.No", "autoWidth": true },
                        { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                        { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                        {
                            "title": "Action", "autoWidth": true,

                            "render": function (data, type, full, meta) {
                                return '<a href="#" onclick="OpenDocumentt(' + full.employee_id + ')"><i class="fa fa-download"></i></a>';
                            }
                        },
                        { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                        { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true }
                        //{ "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true }

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
            }
        });

    }


}

function OpenDocumentt(emp_id) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_health_card_master/1/" + emp_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {

            if (response.statusCode != undefined) {
                messageBox("info", response.message);
                return false;
            }
            window.open(response.healthcard_path);
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}

