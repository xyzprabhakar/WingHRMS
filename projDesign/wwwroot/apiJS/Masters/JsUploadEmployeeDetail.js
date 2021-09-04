$('#loader').show();

var login_role_id;
var default_company;

var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        //HaveDisplay = ISDisplayMenu("Display Company List");
        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlDefaultCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        }
        else {
            BindAllEmp_Company('ddlDefaultCompany', login_emp_id, default_company);
        }




        $("#QualificationDetailFile").bind("change", function () {
            EL("QualificationDetailFile").addEventListener("change", readFile, false);
        });


        $("#myModal").hide();

        $('#loader').hide();


        $("#btnsave").bind("click", function () {

            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlDefaultCompany").val();


            //if (HaveDisplay == 0) {
            //    company_id = default_company;
            //}

            if (company_id == "0" || company_id == null || company_id == "undefined" || company_id == "") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if ($("#QualificationDetailFile").length == 0) {
                errormsg = errormsg + "Please select File </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }



            var mydata = {

                default_company_id: company_id,
                created_by: login_emp_id
            };

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("QualificationDetailFile").files[0];
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            $('#loader').show();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_EmpOfficialDetailUpload/",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    var duplist = data.duplicatedetaillist;
                    var missingdtllist = data.missingdetaillist;
                    var deetailmsg = data.missingDtlMessage;
                    _GUID_New();

                    if (duplist != null && duplist.length > 0) {
                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Duplicate Detail'
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
                                //{ "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                //{ "data": "employee_first_name", "name": "employee_first_name", "title": "First Name", "autoWidth": true },
                                //{ "data": "employee_middle_name", "name": "employee_middle_name", "title": "Middle Name", "autoWidth": true },
                                //{ "data": "employee_last_name", "name": "employee_last_name", "title": "Last Name", "autoWidth": true },
                                { "data": "official_email_id", "name": "official_email_id", "title": "Official Email ID", "autoWidth": true },
                                { "data": "card_number", "name": "card_number", "title": "Card No", "autoWidth": true },
                                //{ "data": "religionname", "name": "religionname", "title": "Religion", "autoWidth": true }
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            //"fnFooterCallback": function (nRow, aaData, iDataStart, iDataEnd) {

                            //}
                            "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {

                                $('.footer').html('anshuman');
                            }


                        });


                        messageBox("error", deetailmsg);
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {
                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Missing Details'
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
                                //{ "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "employee_first_name", "name": "employee_first_name", "title": "First Name", "autoWidth": true },
                                { "data": "employee_middle_name", "name": "employee_middle_name", "title": "Middle Name", "autoWidth": true },
                                { "data": "employee_last_name", "name": "employee_last_name", "title": "Last Name", "autoWidth": true },
                                { "data": "official_email_id", "name": "official_email_id", "title": "Official Email ID", "autoWidth": true },
                                { "data": "card_number", "name": "card_number", "title": "Card No", "autoWidth": true },
                                { "data": "religionname", "name": "religionname", "title": "Religion", "autoWidth": true }
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            "fnFooterCallback": function (nRow, aaData, iStart, iEnd, aiDisplay) {

                                $('.footer').html('anshuman');
                            }

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

// START UPLOAD EMPLOYEE OFFICIAL SECTION DETAIL FROM EXCEL

function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#QualificationDetailFile").val("");
            alert("Upload only Excel file and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#QualificationDetailFile").val("");
                alert("Upload only Excel file and on define format. ");
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

//END UPLOAD EMPLOYEE OFFICIAL SECTION DETAIL FROM EXCEL
