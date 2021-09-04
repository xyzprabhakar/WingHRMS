$('#loader').show();

var login_role_id;
var default_company;

var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        }


        $("#PersonalDetailFile").bind("change", function () {
            EL("PersonalDetailFile").addEventListener("change", readFile, false);
        });


        $("#myModal").hide();

        $('#loader').hide();


        $("#btnuploaddtl").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var filee_ = $("#PersonalDetailFile").val();

            var company_id = $("#ddlCompany").val();


            if (company_id == "0" || company_id == null || company_id == "undefined") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (filee_ == "" || filee_ == null || filee_ == undefined) {
                errormsg = errormsg + "Please Select File </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var mydata = {

                default_company_id: company_id,
                remarks: "test",
                created_by: login_emp_id
            };

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("PersonalDetailFile").files[0];

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);


            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_EmpPersonalDetailUpload/",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    var duplist = data.duplicatepersonaldtl;
                    var missingdtllist = data.missingpersonaldtl;
                    var deetailmsg = data.missingDtlMessage;
                    _GUID_New();
                    if (duplist != null && duplist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Duplicate Detail in Excel'
                        });

                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": duplist,
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "pan_card_number", "name": "pan_card_number", "title": "PAN Card No.", "autoWidth": true },
                                { "data": "aadha_card_number", "name": "aadha_card_number", "title": "Adhaar Card No.", "autoWidth": true },
                                { "data": "bank_acc", "name": "bank_acc", "title": "Bank Account", "autoWidth": true },
                                { "data": "uan", "name": "uan", "title": "UAN No.", "autoWidth": true },
                                { "data": "esic", "name": "esic", "title": "ESIC No.", "autoWidth": true },
                                { "data": "pf_number", "name": "pf_number", "title": "PF No.", "autoWidth": true },



                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            //"fnFooterCallback": function (nRow, aaData, iDataStart, iDataEnd) {

                            //}
                            //      "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {

                            //       $('.footer').html('anshuman');
                            //       }


                        });


                        messageBox("error", deetailmsg);
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Missing / Already exist in DB Details'
                        });
                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": missingdtllist,
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "pan_card_name", "name": "pan_card_name", "title": "PAN Name", "autoWidth": true },
                                { "data": "pan_card_number", "name": "pan_card_number", "title": "PAN Number", "autoWidth": true },
                                { "data": "aadha_card_name", "name": "aadha_card_name", "title": "Adhar Card Name", "autoWidth": true },
                                { "data": "aadha_card_number", "name": "aadha_card_number", "title": "Adhar Card Number", "autoWidth": true },
                                { "data": "blood_group_name", "name": "blood_group_name", "title": "Blood Group", "autoWidth": true },
                                { "data": "primary_contact_number", "name": "primary_contact_number", "title": "Primary Contact Number", "autoWidth": true },
                                { "data": "primary_email_id", "name": "primary_email_id", "title": "Primary Email ID", "autoWidth": true },
                                { "data": "permanent_address_line_one", "name": "permanent_address_line_one", "title": "Permanent Address", "autoWidth": true },
                                { "data": "permanent_country", "name": "permanent_country", "title": "Country", "autoWidth": true },
                                { "data": "permanent_state", "name": "permanent_state", "title": "State", "autoWidth": true },
                                { "data": "permanent_city", "name": "permanent_city", "title": "City", "autoWidth": true },
                                { "data": "permanent_pin_code", "name": "permanent_pin_code", "title": "PIN Code", "autoWidth": true },
                                { "data": "emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Name", "autoWidth": true },
                                { "data": "emergency_contact_relation", "name": "emergency_contact_relation", "title": "Relation", "autoWidth": true },
                                { "data": "emergency_contact_mobile_number", "name": "emergency_contact_mobile_number", "title": "Emergency Contact Number", "autoWidth": true },


                                { "data": "bank_name", "name": "bank_name", "title": "Bank Name", "autoWidth": true },
                                { "data": "bank_acc", "name": "bank_acc", "title": "Bank Account", "autoWidth": true },
                                { "data": "ifsc_code", "name": "ifsc_code", "title": "IFSC Code", "autoWidth": true },
                                { "data": "uan", "name": "uan", "title": "UAN No.", "autoWidth": true },
                                { "data": "esic", "name": "esic", "title": "ESIC No.", "autoWidth": true },
                                { "data": "pf_applicable", "name": "pf_applicable", "title": "PF Applicable(Yes/No)", "autoWidth": true },
                                { "data": "pf_number", "name": "pf_number", "title": "PF No.", "autoWidth": true },
                                { "data": "pf_group_name", "name": "pf_group_name", "title": "PF Group", "autoWidth": true },
                                { "data": "pf_ceilling", "name": "pf_ceilling", "title": "PF Ceilling", "autoWidth": true },

                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            //   "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {

                            //    $('.footer').html('anshuman');
                            //     }

                        });


                        messageBox("error", deetailmsg);

                    }
                    else {

                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        if (statuscode == "0") {
                            alert(Msg);
                            location.reload();
                        }
                        else if (statuscode == "1" || statuscode == "2") {
                            messageBox("error", Msg);
                        }
                    }


                },
                error: function (error) {
                    _GUID_New();
                    alert(error.responseText);
                    $('#loader').hide();
                }
            });
        });


    }, 2000);// end timeout

});

//START UPLOAD EMPLOYEE PERSONAL DETAIL FROM EXCEL

function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#PersonalDetailFile").val("");
            alert("Upload only allows file types of Excel. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#PersonalDetailFile").val("");
                alert("Upload only allows file types of Excel. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64upload_dtl").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }



//END UPLOAD EMPLOYEE PERSONAL DETAIL FROM EXCEL