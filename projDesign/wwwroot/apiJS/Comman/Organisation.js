function BindOrganisationEvent(OrgInputName, CompanyInputName, ZoneInputName) {
    if (!(OrgInputName == "" || OrgInputName === undefined || OrgInputName== null)) {
        $('#' + OrgInputName).on('change paste', function () {
            BindCompany( $('#' + OrgInputName).val(), CompanyInputName, 0);
        });
    }
    if (!(CompanyInputName == "" || CompanyInputName === undefined || CompanyInputName == null)) {
        $('#' + CompanyInputName).on('change paste', function () {
            BindZone($('#' + CompanyInputName).val(), ZoneInputName, 0);
        });
    }
    if (!(ZoneInputName == "" || ZoneInputName === undefined || ZoneInputName == null)) {
        $('#' + ZoneInputName).on('change paste', function () {
            BindLocation($('#' + ZoneInputName).val(), LocationInputName, 0);
        });
    }
}

function BindOrganisation(OrgInputName, orgId) {
    if (OrgInputName == "" || OrgInputName === undefined || OrgInputName == null) {
        return;
    }
    let dBVersion = localStorage.getItem('dBVersion');
    var openRequest = indexedDB.open("dpbs", dBVersion);
    openRequest.onsuccess = function (e) {
        var db = e.target.result;
        let ObjectStoreCountry = db.transaction("tblOrganisation", "readwrite")
            .objectStore("tblOrganisation");

        $('#' + OrgInputName).empty();
        $('#' + OrgInputName).append(`<option value=""> -- Please Select --</option>`);
        ObjectStoreCountry.openCursor().onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                if (cursor.value.id == orgId) {
                    if (cursor.value.isActive) {
                        $('#' + OrgInputName).append(`<option value="${cursor.value.id}" selected> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' + OrgInputName).append(`<option value="${cursor.value.id}" disabled selected> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                }
                else {
                    if (cursor.value.isActive) {
                        $('#' + OrgInputName).append(`<option value="${cursor.value.id}" > ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' + OrgInputName).append(`<option value="${cursor.value.id}" disabled> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                }
                cursor.continue();
            }
            db.close();
        };
    }

}

function BindCompany(orgId, CompanyInputName, companyId) {
    
    if (!(CompanyInputName == "" || CompanyInputName === undefined || CompanyInputName == null)) {        
        let dBVersion = localStorage.getItem('dBVersion');
        var openRequest = indexedDB.open("dpbs", dBVersion);
        $('#' + CompanyInputName).empty();
        $('#' + CompanyInputName).append(`<option value=""> -- Please Select --</option>`);
        openRequest.onsuccess = function (e) {
            var db = e.target.result;
            let ObjectStoreState = db.transaction("tblCompany", "readwrite")
                .objectStore("tblCompany");
            var getAllRequest = ObjectStoreState.index("parentId").getAll(parseInt(orgId));
            getAllRequest.onsuccess = function () {
                for (var i in getAllRequest.result) {

                    if (getAllRequest.result[i].id == companyId) {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + CompanyInputName).append(`<option value="${getAllRequest.result[i].id}" selected> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + CompanyInputName).append(`<option value="${getAllRequest.result[i].id}" disabled selected> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                    else {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + CompanyInputName).append(`<option value="${getAllRequest.result[i].id}" > ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + CompanyInputName).append(`<option value="${getAllRequest.result[i].id}" disabled> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                }
                db.close();
            }
        }
    }
}

function BindZone(companyId, ZoneInputName, zoneId) {

    if (!(ZoneInputName == "" || ZoneInputName === undefined || ZoneInputName == null)) {
        let dBVersion = localStorage.getItem('dBVersion');
        var openRequest = indexedDB.open("dpbs", dBVersion);
        $('#' + ZoneInputName).empty();
        $('#' + ZoneInputName).append(`<option value=""> -- Please Select --</option>`);
        openRequest.onsuccess = function (e) {
            var db = e.target.result;
            let ObjectStoreState = db.transaction("tblZone", "readwrite")
                .objectStore("tblZone");
            var getAllRequest = ObjectStoreState.index("parentId").getAll(parseInt(companyId));
            getAllRequest.onsuccess = function () {
                for (var i in getAllRequest.result) {

                    if (getAllRequest.result[i].id == zoneId) {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + ZoneInputName).append(`<option value="${getAllRequest.result[i].id}" selected> ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + ZoneInputName).append(`<option value="${getAllRequest.result[i].id}" disabled selected>  ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                    else {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + ZoneInputName).append(`<option value="${getAllRequest.result[i].id}" > ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + ZoneInputName).append(`<option value="${getAllRequest.result[i].id}" disabled>  ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                }
                db.close();
            }
        }
    }
}

function BindLocation(zoneId, LocationInputName, locationId) {

    if (!(LocationInputName == "" || LocationInputName === undefined || LocationInputName == null)) {
        let dBVersion = localStorage.getItem('dBVersion');
        var openRequest = indexedDB.open("dpbs", dBVersion);
        $('#' + LocationInputName).empty();
        $('#' + LocationInputName).append(`<option value=""> -- Please Select --</option>`);
        openRequest.onsuccess = function (e) {
            var db = e.target.result;
            let ObjectStoreState = db.transaction("tblLocation", "readwrite")
                .objectStore("tblLocation");
            var getAllRequest = ObjectStoreState.index("parentId").getAll(parseInt(zoneId));
            getAllRequest.onsuccess = function () {
                for (var i in getAllRequest.result) {

                    if (getAllRequest.result[i].id == locationId) {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + LocationInputName).append(`<option value="${getAllRequest.result[i].id}" selected> ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + LocationInputName).append(`<option value="${getAllRequest.result[i].id}" disabled selected>  ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                    else {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + LocationInputName).append(`<option value="${getAllRequest.result[i].id}" > ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + LocationInputName).append(`<option value="${getAllRequest.result[i].id}" disabled>  ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                }
                db.close();
            }
        }
    }
}

