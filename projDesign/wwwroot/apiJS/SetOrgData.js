

//Set DB Data
$(document).ready(function () {
    let OrgId = localStorage.getItem("currentOrganisation");    
    GetData_IndexDb(parseInt(OrgId), "tblOrganisation", "int").then(p => {        
        $("#hdrOrganisation").text(p.code + " - " + p.name);
    });
    let CompanyId = localStorage.getItem("currentCompany");
    if (CompanyId == null) {
        localStorage.setItem("currentCompany", -1);
        CompanyId = -1;
    }
    if (CompanyId == -1) {
        $("#hdrCompany").text("All");
    }
    else {
        GetData_IndexDb(parseInt(CompanyId), "tblCompany", "int").then(p => {
            $("#hdrCompany").text(p.code + " - " + p.name);
        });
    }
    let ZoneId = localStorage.getItem("currentZone");
    if (ZoneId == null) {
        localStorage.setItem("currentZone", -1);
        ZoneId= -1;
    }
    if (ZoneId == -1) {
        $("#hdrZone").text("All");
    }
    else {
        GetData_IndexDb(parseInt(ZoneId), "tblZone", "int").then(p => {
            $("#hdrZone").text(p.name);
        });
    }
    let LocationId = localStorage.getItem("currentLocation");
    
    if (LocationId == null) {
        localStorage.setItem("currentLocation", -1);
        LocationId = -1;
    }
    if (LocationId == -1) {
        $("#hdrLocation").text("All");
    }
    else {
        GetData_IndexDb(parseInt(LocationId), "tblLocation", "int").then(p => {
            $("#hdrLocation").text(p.name);
        });
    }
    
});