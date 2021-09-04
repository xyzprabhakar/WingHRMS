$('#loader').show();

var login_emp_idd;
var default_company;
var HaveDisplay;
var _appSetting_domainn_dec;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        var key = CryptoJS.enc.Base64.parse("#base64Key#");
        var iv = CryptoJS.enc.Base64.parse("#base64IV#");

        _appSetting_domainn_dec = CryptoJS.AES.decrypt(localStorage.getItem("_appSetting_domainn"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', login_emp_idd, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);

            BindDocumentTypeByComp('ddldoctype', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);

        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_idd, default_company);

            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));

            BindDocumentTypeByComp('ddldoctype', default_company, 0);

        }



        // HaveDisplay = ISDisplayMenu("Display Company List");


        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        }

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

            BindDocumentTypeByComp('ddldoctype', $(this).val(), 0);
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            GetData($(this).val(), login_emp_idd);
        });

        $('#ddlEmployeeCode').change(function () {

            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            GetData($("#ddlCompany").val(), $(this).val());

        });

        $('#loader').hide();

        EL("empdocument").addEventListener("change", readFile, false);

        $("#btnreset").bind("click", function () {
            location.reload();
        });


        $("#btnsave").bind("click", function () {
            $('#loader').show();
            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlCompany").val();
            var employee_id = $("#ddlEmployeeCode").val();
            var doc_type = $("#ddldoctype").val();
            var docno = $("#txtdocno").val();

            var remarks = $("#txtremarks").val();

            if (company_id == "0" || company_id == null || company_id == "") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (employee_id == "0") {
                errormsg = errormsg + "Please Select Employee </br>";
                iserror = true;
            }

            //if (doc_type == "0") {
            //    errormsg = errormsg + "Please select Document Type</br>";
            //    iserror = true;
            //}


            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }



            var mydata = {
                company_id: company_id,
                doc_type_id: doc_type,
                emp_id: employee_id,
                doc_no: docno,
                remarks: remarks,
                created_by: login_emp_idd,
            };

            var Obj = JSON.stringify(mydata);

            // var file = document.getElementById("empdocument").files[0];


            var files = document.getElementById("empdocument").files;

            var formData = new FormData();
            formData.append('AllData', Obj);
            for (var i = 0; i < files.length; i++) {
                formData.append("fileInput", files[i]);
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiEmployee/Save_EmpDocuments",
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
    var file = document.getElementById("empdocument").files;

    for (var i = 0; i < file.length; i++) {
        var ftype = file[i];//this;
        var fileupload = ftype.name; //file[i].value;//ftype.value;
        if (fileupload == '') {
            $("#empdocument").val("");
            alert("Photograph only allows file types of PNG,PDF, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "pdf") {

            }
            else {
                $("#empdocument").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG,PDF. ");
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


    //if (this.files && this.files[0]) {

    //    var ftype = this;
    //    var fileupload = ftype.value;
    //    if (fileupload == '') {
    //        $("#empdocument").val("");
    //        alert("Photograph only allows file types of PNG,PDF, JPG, JPEG. ");
    //        return;
    //    }
    //    else {
    //        var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
    //        if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "pdf") {

    //        }
    //        else {
    //            $("#empdocument").val("");
    //            alert("Photograph only allows file types of PNG, JPG, JPEG,PDF. ");
    //            return;
    //        }
    //    }

    //    var FR = new FileReader();
    //    FR.onload = function (e) {
    //        //  EL("myImg").src = e.target.result;
    //        EL("HFb64").value = e.target.result;

    //    };
    //    FR.readAsDataURL(this.files[0]);
    //}
}

function EL(id) { return document.getElementById(id); }


function GetData(companyid_, empid_) {

    if ($.fn.DataTable.isDataTable('#tblempdocuments')) {
        $('#tblempdocuments').DataTable().clear().draw();
    }

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/GetEmpDocuments/" + companyid_ + "/" + empid_,
        type: "GET",
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        data: {},
        success: function (response) {
            debugger;
            var res = response;
            $('#loader').hide();
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            var listdata = [];
                       
            for (i = 0; i < response._remin_emp_doc_data.length; i++) {
                var templist = {};
                var doc_path = new Array();
                var inc = 0;
                templist["doc_name"] = response._remin_emp_doc_data[i].doc_name;
                templist["doc_no"] = response._remin_emp_doc_data[i].doc_no;
                templist["remarks"] = response._remin_emp_doc_data[i].remarks;
                templist["created_date"] = response._remin_emp_doc_data[i].created_date;
                templist["emp_id"] = response._remin_emp_doc_data[i].emp_id;
                templist["doc_type_id"] = response._remin_emp_doc_data[i].doc_type_id;
                templist["emp_doc_id"] = response._remin_emp_doc_data[i].emp_doc_id;


                for (k = 0; k < response._emp_document_data.length; k++) {

                    if (response._remin_emp_doc_data[i].doc_type_id == response._emp_document_data[k].doc_type_id) {
                        if (response._emp_document_data[k].doc_path != null)
                            doc_path[inc] = response._emp_document_data[k].doc_path.toString() + "," + response._emp_document_data[k].emp_id + "," + response._emp_document_data[k].emp_doc_id;
                        else
                            doc_path[inc] = "No any document found!!";
                        inc++;
                    }
                    templist["doc_path"] = doc_path;
                }

                listdata.push(templist);
            }





            $("#tblempdocuments").DataTable({
                "processing": false,//for show progress bar
                "serverSide": false,// for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at one
                "scrollX": 200,
                "data": listdata,
                // "aaData": response,
                "columnDefs": [
                ],
                "columns": [

                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "doc_name", "name": "doc_name", "title": "Document Type", "autoWidth": true },
                    { "data": "doc_no", "name": "doc_no", "title": "Document No.", "autoWidth": true },
                   // { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    {
                        "title": "View", "autoWidth": true,

                        "render": function (data, type, full, meta) {
                            _appSetting_domainn_dec = _appSetting_domainn_dec.replace("/api/", "");
                            var clickeventt = "";
                            for (m = 0; m < full.doc_path.length; m++) {

                                //clickeventt += '<a href=' + full.doc_path[m].split(",")[0] + ' onclick=" window.open(' + _appSetting_domainn_dec + full.doc_path[m].split(",")[0] + ', "_blank");return false"><i class="fa fa-download"></i></a>';
                                // clickeventt += '<a href="' + _appSetting_domainn_dec + full.doc_path[m].split(",")[0] + '" target="_blank"><i class="fa fa-download"></i></a>';

                                clickeventt += '<a onclick = "checkFileExist(\'' + _appSetting_domainn_dec + full.doc_path[m].split(",")[0] + '\')" target="_blank"><i class="fa fa-download"></i></a>';

                            }
                            return clickeventt;
                        }
                    },
                    {
                        "title": "Is Delete?", "autoWidth": true,
                        "render": function (data, type, full, meta) {

                            return '<a onclick="DeleteDocument(' + full.emp_id + ',' + full.emp_doc_id + ')"><i class="fas fa-trash-alt"></i></a>';

                        }

                    },

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
            _GUID_New();
            messageBox("error", error.responseText);
        }
    });

}



function DeleteDocument(emp_id,emp_doc_id) {
    $("#loader").show();
    if (confirm("Do you want to delete this?")) {


        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        var mydata = {
            //company_id: company_id,
            //doc_type_id: doc_type_id,
            emp_id: emp_id,
            doc_no: "",
            remarks: "",
            created_by: login_emp_idd,
            emp_doc_id:emp_doc_id
        };

        $.ajax({
            url: localStorage.getItem("ApiUrl") + "/apiEmployee/DeleteEmpDocument",
            type: "POST",
            contentType: "application/json",
            headers: headerss,
            dataType: "json",
            data: JSON.stringify(mydata),
            success: function (response) {
                var statuscode = response.statusCode;
                var msg = response.message;
                $('#loader').hide();
                _GUID_New();
                if (statuscode == "0") {
                    if ($("#ddlCompany").val() != null && $("#ddlCompany").val() != "" && $("#ddlEmployeeCode").val() != null && $("#ddlEmployeeCode").val() != "") {
                        GetData($("#ddlCompany").val(), $("#ddlEmployeeCode").val());
                    }
                    else {
                        GetData(default_company, login_emp_idd);
                    }

                    messageBox("success", msg);
                    return false;
                }

                if (statuscode == "1" || statuscode == "2") {
                    messageBox("error", msg);
                    return false;
                }
            },
            error: function (error) {
                $('#loader').hide();
                _GUID_New();
                messageBox(err.responseText);

            }
        });

    }
    else {

        $("#loader").hide();
        return false;
    }


}




