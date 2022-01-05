function BindOrganisationEvent(OrgInputName, CompanyInputName, ZoneInputName) {
    if (!(OrgInputName == "" || OrgInputName === undefined || OrgInputName== null)) {
        $('#' + OrgInputName).on('change paste', function (event, _Id) {
            BindCompany($('#' + OrgInputName).val(), CompanyInputName, _Id);
        });
    }
    if (!(CompanyInputName == "" || CompanyInputName === undefined || CompanyInputName == null)) {
        $('#' + CompanyInputName).on('change paste', function (event, _Id) {
            BindZone($('#' + CompanyInputName).val(), ZoneInputName,  _Id);
        });
    }
    if (!(ZoneInputName == "" || ZoneInputName === undefined || ZoneInputName == null)) {
        $('#' + ZoneInputName).on('change paste', function (event, _Id) {
            BindLocation($('#' + ZoneInputName).val(), LocationInputName, _Id);
        });
    }
}

function BindOrganisation(OrgInputName, orgId) {
    if (OrgInputName == "" || OrgInputName === undefined || OrgInputName == null) {
        return;
    }
    if (orgId == 0 || orgId == "" || orgId == undefined) {
        orgId = localStorage.getItem("currentOrganisation");        
    }
    GetDataAll_IndexDb("tblOrganisation").then((result) => {
        $('#' + OrgInputName).empty();
        $('#' + OrgInputName).append(`<option value=""> -- Please Select --</option>`);
        for (let i in result) {            
            if (result[i].id == orgId) {                
                if (result[i].isActive) {
                    $('#' + OrgInputName).append(`<option value="${result[i].id}" selected>${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + OrgInputName).append(`<option value="${result[i].id}" disabled selected>${result[i].code} - ${result[i].name}</option>`);
                }
            }
            else {
                if (result[i].isActive) {
                    $('#' + OrgInputName).append(`<option value="${result[i].id}" >${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + OrgInputName).append(`<option value="${result[i].id}" disabled>${result[i].code} - ${result[i].name}</option>`);
                }
            }
        }
    });
}

function BindCompany(orgId, CompanyInputName, companyId) {
    if (CompanyInputName == "" || CompanyInputName === undefined || CompanyInputName == null) {
        return;
    }
    if (orgId == 0 || orgId == "" || orgId == null || orgId== undefined) {
        return;
    }
    let _orgId = parseInt(orgId);
    var keyRangeValue = IDBKeyRange.only(_orgId);
    GetDataFromIndex_IndexDb(keyRangeValue, "tblCompany", "parentId").then((result) => {
        $('#' + CompanyInputName).empty();
        $('#' + CompanyInputName).append(`<option value=""> -- Please Select --</option>`);
        for (var i in result) {
            if (result[i].id == companyId) {
                if (result[i].isActive) {
                    $('#' + CompanyInputName).append(`<option value="${result[i].id}" selected>${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + CompanyInputName).append(`<option value="${result[i].id}" disabled selected>${result[i].code} - ${result[i].name}</option>`);
                }
            }
            else {
                if (result[i].isActive) {
                    $('#' + CompanyInputName).append(`<option value="${result[i].id}" >${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + CompanyInputName).append(`<option value="${result[i].id}" disabled>${result[i].code} - ${result[i].name}</option>`);
                }
            }
        }
    });

}

function BindZone(companyId, ZoneInputName, zoneId) {

    if (ZoneInputName == "" || ZoneInputName === undefined || ZoneInputName== null) {
        return;
    }
    if (companyId == 0 || companyId == "" || companyId == null || companyId== undefined) {
        return;
    }
    let _companyId = parseInt(companyId);
    var keyRangeValue = IDBKeyRange.only(_orgId);
    GetDataFromIndex_IndexDb(keyRangeValue, "tblZone", "parentId").then((result) => {
        $('#' + ZoneInputName).empty();
        $('#' + ZoneInputName).append(`<option value=""> -- Please Select --</option>`);
        for (var i in result) {
            if (result[i].id == zoneId) {
                if (result[i].isActive) {
                    $('#' + ZoneInputName).append(`<option value="${result[i].id}" selected>${result[i].name}</option>`);
                }
                else {
                    $('#' + ZoneInputName).append(`<option value="${result[i].id}" disabled selected> ${result[i].name}</option>`);
                }
            }
            else {
                if (result[i].isActive) {
                    $('#' + ZoneInputName).append(`<option value="${result[i].id}" > ${result[i].name}</option>`);
                }
                else {
                    $('#' + ZoneInputName).append(`<option value="${result[i].id}" disabled>${result[i].name}</option>`);
                }
            }
        }
    });

}

function BindLocation(zoneId, LocationInputName, locationId) {


    if (LocationInputName == "" || LocationInputName === undefined || LocationInputName== null) {
        return;
    }
    if (zoneId == 0 || zoneId == "" || zoneId == null || zoneId== undefined) {
        return;
    }
    let _zoneId= parseInt(zoneId);
    var keyRangeValue = IDBKeyRange.only(_zoneId);
    GetDataFromIndex_IndexDb(keyRangeValue, "tblLocation", "parentId").then((result) => {
        $('#' + LocationInputName).empty();
        $('#' + LocationInputName).append(`<option value=""> -- Please Select --</option>`);
        for (var i in result) {
            if (result[i].id == locationId) {
                if (result[i].isActive) {
                    $('#' + LocationInputName).append(`<option value="${result[i].id}" selected>${result[i].name}</option>`);
                }
                else {
                    $('#' + LocationInputName).append(`<option value="${result[i].id}" disabled selected>${result[i].name}</option>`);
                }
            }
            else {
                if (result[i].isActive) {
                    $('#' + LocationInputName).append(`<option value="${result[i].id}" >${result[i].name}</option>`);
                }
                else {
                    $('#' + LocationInputName).append(`<option value="${result[i].id}" disabled>${result[i].name}</option>`);
                }
            }
        }
    });

}

