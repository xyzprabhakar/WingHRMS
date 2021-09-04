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


        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
        //var HaveDisplay = ISDisplayMenu("Display Company List");

        //if (HaveDisplay == 0) {

        //    $('#ddlCompany').prop("disabled", "disabled");

        //    BindCompanyList('ddlCompany', default_company);
        //}
        //else {

        //    BindCompanyList('ddlCompany', 0);
        //}

        $("#myModal").hide();
        EL("AttandanceeFile").addEventListener("change", readFile, false);

        $('#loader').hide();

        $("#btnsave").bind("click", function () {
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlCompany").val();

            if (login_role_id == 2)//For Admin
            {
                company_id = default_company;
                //employee_id = localStorage.getItem("emp_id");
            }

            if (company_id == "0" || company_id == null || company_id == "undefined") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }



            var mydata = {

                company_idd: company_id,
                //remarks: "test",
                //created_by: login_emp_id
            };

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("AttandanceeFile").files[0];


            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);

            $('#loader').show();
            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/Attendance/SaveAttandance_ExcelUpload/",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (response) {
                    var data = response;

                    var duplist = data.duplicateddtl;
                    var missingdtllist = data.missingdtl;
                    var deetailmsg = data.dtlMessage;

                    $('#loader').hide();
                    _GUID_New();



                    if (duplist != null && duplist.length > 0) {
                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').dialog({
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
                                { "data": "emp_code", "name": "emp_code", "title": "Emp Code", "autoWidth": true },
                                { "data": "card_number", "name": "card_number", "title": "Card Number", "autoWidth": true },
                                { "data": "punch_time", "name": "punch_time", "title": "Punch ID", "autoWidth": true },
                                { "data": "machine_id", "name": "machine_id", "title": "Machine ID", "autoWidth": true }
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

                        $('#myModal').dialog({
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
                                { "data": "emp_code", "name": "emp_code", "title": "Emp Code", "autoWidth": true },
                                { "data": "card_number", "name": "card_number", "title": "Card Number", "autoWidth": true },
                                { "data": "punch_time", "name": "punch_time", "title": "Punch ID", "autoWidth": true },
                                { "data": "machine_id", "name": "machine_id", "title": "Machine ID", "autoWidth": true }
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
                error: function (err) {
                    _GUID_New();
                    messageBox(err.responseText);
                    $('#loader').hide();
                    return false;
                },
            });
        });

        $("#btnreset").bind("click", function () {
            location.reload();
        });


    }, 2000);// end timeout

});

function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#AttandanceeFile").val("");
            alert("Upload only allows file types of Excel. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#AttandanceeFile").val("");
                alert("Upload only allows file types of Excel. ");
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

