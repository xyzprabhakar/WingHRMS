

//Set DB Data
$(document).ready(function () {
    BindOrganisation();
    async function BindOrganisation() {


        let OrgsanisationData = await GetDataAll_IndexDb("tblOrganisation");
        $("#hdrUlOrganisation").empty();
        for (let i in OrgsanisationData) {
            $("#hdrUlOrganisation").append(`<li> <a href="#" class="hdrOrganisation" data-id="${OrgsanisationData[i].id}">${OrgsanisationData[i].code} - ${OrgsanisationData[i].name}</a> </li>`);
        }
        let OrgId = localStorage.getItem("currentOrganisation")
        if (OrgId == null || OrgId == undefined) {
            OrgId = -1;
            localStorage.getItem("currentOrganisation", -1);
        }
        localStorage.setItem("currentOrganisation", OrgId);
        GetData_IndexDb(parseInt(OrgId), "tblOrganisation", "int").then(p => {
            $("#hdrOrganisation").text(p.code + " - " + p.name);
        });

        let LocationId = localStorage.getItem("currentLocation")

        let ZoneId = localStorage.getItem("currentZone")
        if (ZoneId == null || ZoneId === undefined) {
            ZoneId  = -1;
            localStorage.getItem("currentZone", -1);
        }

        let ComapnyId = localStorage.getItem("currentCompany")
        if (ComapnyId == null || ComapnyId === undefined) {
            ComapnyId  = -1;
            localStorage.getItem("currentCompany", -1);
        }
        await OrganisationClick(OrgId);
        await CompanyClick(ComapnyId);
        await ZoneClick(ZoneId);


        if (LocationId == null || LocationId === undefined) {
            LocationId = -1;
            localStorage.getItem("currentLocation", -1);
        }
        if (LocationId == -1) {
            $("#hdrLocation").text("ALL");
        }
        else {
            GetData_IndexDb(parseInt(LocationId), "tblLocation", "int").then(p => {
                $("#hdrLocation").text(p.name);
            });
        }
        localStorage.setItem("currentLocation", LocationId);
        
        

    }


    async function OrganisationClick(OrgId) {
        localStorage.setItem("currentOrganisation", OrgId);
        localStorage.setItem("currentCompany", -1);
        localStorage.setItem("currentZone", -1);
        localStorage.setItem("currentLocation", -1);
        $("#hdrCompany").text("All");
        $("#hdrZone").text("All");
        $("#hdrLocation").text("All");
        let AllCompany = [];
        let AllZone = [];
        let AllLocation = [];
        //Now get All Company
        let result = await GetDataFromIndex_IndexDb(parseInt(OrgId), "tblCompany", "parentId");
        for (let i in result) {
            AllCompany.push({ "id": result[i].id, "name": result[i].code + " - " + result[i].name });
            let zoneResult = await GetDataFromIndex_IndexDb(parseInt(result[i].id), "tblZone", "parentId");
            for (let j in zoneResult) {
                AllZone.push({ "id": zoneResult[j].id, "name": zoneResult[j].name });
                let locationResult = await GetDataFromIndex_IndexDb(parseInt(zoneResult[j].id), "tblLocation", "parentId");                
                for (let k in locationResult) {
                    AllLocation.push({ "id": locationResult[k].id, "name": locationResult[k].name });
                }
            }
        }
        AllCompany.sort(SortByName);
        AllZone.sort(SortByName);
        AllLocation.sort(SortByName);
        $("#hdrUlCompany").empty();
        $("#hdrUlZone").empty();
        $("#hdrUlLocation").empty();
        $("#hdrUlCompany").append(`<li> <a href="#" class="hdrCompany" data-id="-1">ALL</a> </li>`)
        $("#hdrUlZone").append(`<li> <a href="#" class="hdrZone" data-id="-1">ALL</a> </li>`)
        $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="-1">ALL</a> </li>`)
        for (let i in AllCompany) {
            $("#hdrUlCompany").append(`<li> <a href="#" class="hdrCompany" data-id="${AllCompany[i].id}">${AllCompany[i].name}</a> </li>`)
        }
        for (let i in AllZone) {
            $("#hdrUlZone").append(`<li> <a href="#" class="hdrZone" data-id="${AllZone[i].id}">${AllZone[i].name}</a> </li>`)
        }
        for (let i in AllLocation) {
            $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="${AllLocation[i].id}">${AllLocation[i].name}</a> </li>`)
        }


    }
    async function CompanyClick(CompanyId) {
        if (CompanyId == -1) {
            $("#hdrCompany").text("All");
            let OrgId = localStorage.getItem("currentOrganisation");
            await OrganisationClick(OrgId);
        }
        else {
            localStorage.setItem("currentCompany", CompanyId);
            localStorage.setItem("currentZone", -1);
            localStorage.setItem("currentLocation", -1);
            $("#hdrZone").text("All");
            $("#hdrLocation").text("All");
            let companyData= await GetData_IndexDb(parseInt(CompanyId), "tblCompany", "int");
            $("#hdrCompany").text(companyData.code + " - " + companyData.name);

            let AllZone = [];
            let AllLocation = [];
            //Now get All Company
            let zoneResult = await GetDataFromIndex_IndexDb(parseInt(CompanyId), "tblZone", "parentId");
            for (let j in zoneResult) {
                AllZone.push({ "id": zoneResult[j].id, "name": zoneResult[j].name });
                let locationResult = await GetDataFromIndex_IndexDb(parseInt(zoneResult[j].id), "tblLocation", "parentId");                
                for (let k in locationResult) {
                    AllLocation.push({ "id": locationResult[k].id, "name": locationResult[k].name });
                }
            }
            AllZone.sort(SortByName);
            AllLocation.sort(SortByName);
            
            $("#hdrUlZone").empty();
            $("#hdrUlLocation").empty();
            
            $("#hdrUlZone").append(`<li> <a href="#" class="hdrZone" data-id="-1">ALL</a> </li>`)
            $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="-1">ALL</a> </li>`)
            for (let i in AllZone) {
                $("#hdrUlZone").append(`<li> <a href="#" class="hdrZone" data-id="${AllZone[i].id}">${AllZone[i].name}</a> </li>`)
            }
            for (let i in AllLocation) {
                $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="${AllLocation[i].id}">${AllLocation[i].name}</a> </li>`)
            }


        }

        
        
    }
    async function ZoneClick(ZoneId) {
        if (ZoneId == -1) {
            $("#hdrZone").text("All");
            let CompanyId = localStorage.getItem("currentCompany");
            await CompanyClick(CompanyId);
        }
        else {

            localStorage.setItem("currentZone", ZoneId);
            localStorage.setItem("currentLocation", -1);
            $("#hdrLocation").text("All");
            let ZoneData = await GetData_IndexDb(parseInt(ZoneId), "tblZone", "int");
            $("#hdrZone").text(ZoneData.name);

            console.log(parseInt(ZoneId));
            let AllLocation = [];
            //Now get All Company
            let locationResult = await GetDataFromIndex_IndexDb(parseInt(ZoneId), "tblLocation", "parentId");
                for (let k in locationResult) {
                    AllLocation.push({ "id": locationResult[k].id, "name": locationResult[k].name });
                }
            
            AllLocation.sort(SortByName);

            $("#hdrUlLocation").empty();

            $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="-1">ALL</a> </li>`)
            for (let i in AllLocation) {
                $("#hdrUlLocation").append(`<li> <a href="#" class="hdrLocation" data-id="${AllLocation[i].id}">${AllLocation[i].name}</a> </li>`)
            }
        }
    }


    $(document).on("click", ".hdrOrganisation", async function (event) {
        let OrgId = event.target.getAttribute("data-id");
        await OrganisationClick(OrgId);
    });
    $(document).on("click", ".hdrCompany", async function (event) {
        let CompanyId = event.target.getAttribute("data-id");
        console.log(CompanyId);
        await CompanyClick(CompanyId );
    });
    $(document).on("click", ".hdrZone", async function (event) {
        let ZoneId = event.target.getAttribute("data-id");
        await ZoneClick(ZoneId );
    });
    $(document).on("click", ".hdrLocation", async function (event) {
        let LocationId = event.target.getAttribute("data-id");
        if (LocationId == -1) {
            $("#hdrLocation").text("ALL");
        }
        else {
            GetData_IndexDb(parseInt(LocationId), "tblLocation", "int").then(p => {
                $("#hdrLocation").text(p.name);
            });
        }
        localStorage.setItem("currentLocation", LocationId);
        
    });

    //let OrgId = localStorage.getItem("currentOrganisation");    
    //GetData_IndexDb(parseInt(OrgId), "tblOrganisation", "int").then(p => {        
    //    $("#hdrOrganisation").text(p.code + " - " + p.name);
    //});
    //let CompanyId = localStorage.getItem("currentCompany");
    //if (CompanyId == null) {
    //    localStorage.setItem("currentCompany", -1);
    //    CompanyId = -1;
    //}
    //if (CompanyId == -1) {
    //    $("#hdrCompany").text("All");
    //}
    //else {
    //    GetData_IndexDb(parseInt(CompanyId), "tblCompany", "int").then(p => {
    //        $("#hdrCompany").text(p.code + " - " + p.name);
    //    });
    //}
    //let ZoneId = localStorage.getItem("currentZone");
    //if (ZoneId == null) {
    //    localStorage.setItem("currentZone", -1);
    //    ZoneId= -1;
    //}
    //if (ZoneId == -1) {
    //    $("#hdrZone").text("All");
    //}
    //else {
    //    GetData_IndexDb(parseInt(ZoneId), "tblZone", "int").then(p => {
    //        $("#hdrZone").text(p.name);
    //    });
    //}
    //let LocationId = localStorage.getItem("currentLocation");
    
    //if (LocationId == null) {
    //    localStorage.setItem("currentLocation", -1);
    //    LocationId = -1;
    //}
    //if (LocationId == -1) {
    //    $("#hdrLocation").text("All");
    //}
    //else {
    //    GetData_IndexDb(parseInt(LocationId), "tblLocation", "int").then(p => {
    //        $("#hdrLocation").text(p.name);
    //    });
    //}



});