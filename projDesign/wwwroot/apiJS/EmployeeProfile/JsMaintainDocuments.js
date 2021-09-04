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


        BindCompanyList('ddlCompany', default_company);

        // BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, employee_id);

        BindEmployeeCodee('ddlEmployeeCode', default_company, login_emp_idd);

        GetData(default_company, login_emp_idd);

    }, 2000);// end timeout

});



function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}




function GetData(companyid_, empid_) {
    $('#loader').show();
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


                for (k = 0; k < response._emp_document_data.length; k++) {

                    if (response._remin_emp_doc_data[i].doc_type_id == response._emp_document_data[k].doc_type_id) {
                        doc_path[inc] = response._emp_document_data[k].doc_path.toString() + "," + response._emp_document_data[k].emp_id + "," + response._emp_document_data[k].emp_doc_id;
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
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    {
                        "title": "View", "autoWidth": true,

                        "render": function (data, type, full, meta) {

                            var clickeventt = "";
                            for (m = 0; m < full.doc_path.length; m++) {

                                //clickeventt += '<a href=' + full.doc_path[m].split(",")[0] + ' onclick=" window.open(' + _appSetting_domainn_dec + full.doc_path[m].split(",")[0] + ', "_blank");return false"><i class="fa fa-download"></i></a>';

                                clickeventt += '<a href="' + _appSetting_domainn_dec + full.doc_path[m].split(",")[0] + '" target="_blank"><i class="fa fa-download"></i></a>';

                            }
                            return clickeventt;
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







