var db, baseUrl, dBVersion, token, openRequest;

var Apidatas = [{
    id:1, name:"Authentication", url: "User/GetUserDocuments/false/true/true/true", methodType: "GET", isloaded:false,postdata:null,
    tables:
        [
            { tableName: "tblApplicationMaster", keyName:"_application" },
            { tableName: "tblModuleMaster", keyName:"_module" },
            { tableName: "tblSubModuleMaster", keyName:"_submodule" },
            { tableName: "tblDocumentMaster", keyName:"_document" },
            { tableName: "tblMenuMaster", keyName:"_muenuList" },
        ]

},
    { id: 2, name: "Country", url: "Masters/GetCountry/false", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblCountry", keyName: "" }] },
    { id: 3, name: "State", url: "Masters/GetState/0/true/false", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblState", keyName: "" }] },
    { id: 4, name: "Organisation", url: "User/GetCurrentUserOrganisation", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblOrganisation", keyName: "" }] },
    { id: 5, name: "Company", url: "User/GetCurrentUserCompany", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblCompany", keyName: "" }] },
    { id: 6, name: "Zone", url: "User/GetCurrentUserZone", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblZone", keyName: "" }] },
    { id: 7, name: "Location", url: "User/GetCurrentUserLocation?ClearCache=true", methodType: "GET", isloaded: false, postdata: null, tables: [{ tableName: "tblLocation", keyName: "" }] },
]

onmessage = function (e) {
    baseUrl = e.data.baseUrl_;
    dBVersion= e.data.dBVersion_;
    token = e.data.token_;
    if (e.data.startDownload_ == 1) {
        openRequest = indexedDB.open("dpbs", dBVersion);
        openRequest.onupgradeneeded = function (e) {
            fncCreateAllDb(e);
        }
        openRequest.onsuccess = function (e) {
            db = e.target.result;
            for (let i in Apidatas) {
                Apidatas[i].isloaded = false;
            }
            for (let i in Apidatas) {                
                LoadDataInDB(Apidatas[i], i);
            }
        }

    }
};

 function LoadDataInDB(Apidata, ApidataPosition) {
    let apiurl = baseUrl + Apidata.url;
    let headerss = new Headers();
    headerss.append("Authorization", 'Bearer ' + token);
    headerss.append("Content-Type", "application/json; charset=utf-8");
    //headerss["Authorization"] = 'Bearer ' + token;
    //headerss["Content-Type"] = "application/json; charset=utf-8";
    //headerss["Access-Control-Allow-Origin"] = "*";
     fetch(apiurl, {
         method: Apidata.methodType,
         headers: headerss,
         body: Apidata.postdata
     }).then(response => {
         if (response.ok) {
             return response.json();
         } else {
             throw new Error(response.statusText);
         }
     }).then(data => {
            if (data.messageType == 1) {
                for (var index in Apidata.tables) {
                    let tablename_ = Apidata.tables[index].tableName;
                    let keyname_ = Apidata.tables[index].keyName;
                    SaveinDb(keyname_, tablename_, data.returnId, Apidatas, ApidataPosition);
                }
         }
     }).catch((error) => { console.log(error) });
}

async function SaveinDb(KeyName, Tablename, returnId, Apidatas,i) {
    let ObjectStore = db.transaction(Tablename, "readwrite")
        .objectStore(Tablename);
    let ReqSuccess = await ObjectStore.clear();
    ReqSuccess.onsuccess = async function (event) {
        
        if (KeyName == "") {
            for (var j in returnId) {
                await ObjectStore.add(returnId[j]);                
            }
            isAllDataDownloaded();
            
        }
        else {
            let tempdata = returnId[KeyName];
            for (var j in tempdata) {
                await ObjectStore.add(tempdata[j]);
            }
            isAllDataDownloaded();
        }
    }
    function isAllDataDownloaded()
    {
        
        postMessage("Loaded " + Apidatas[i].name);
        Apidatas[i].isloaded = true;
        if (Apidatas.find(checkIsNotCompleted) === undefined) {
            postMessage("Done");
        }
        function checkIsNotCompleted(Apidatas) {
            return !Apidatas.isloaded;
        }
    }
}

function fncCreateAllDb(e) {
    var thisDB = e.target.result;
    if (!thisDB.objectStoreNames.contains("tblApplicationMaster")) {
        thisDB.createObjectStore("tblApplicationMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblModuleMaster")) {
        thisDB.createObjectStore("tblModuleMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblSubModuleMaster")) {
        thisDB.createObjectStore("tblSubModuleMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblDocumentMaster")) {
        thisDB.createObjectStore("tblDocumentMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblMenuMaster")) {
        thisDB.createObjectStore("tblMenuMaster", { keyPath: "applicationId" });
    }

    if (!thisDB.objectStoreNames.contains("tblCountry")) {
        thisDB.createObjectStore("tblCountry", { keyPath: "countryId" });
    }
    if (!thisDB.objectStoreNames.contains("tblState")) {
        thisDB.createObjectStore("tblState", { keyPath: "stateId" });
    }
    if (!thisDB.objectStoreNames.contains("tblOrganisation")) {
        thisDB.createObjectStore("tblOrganisation", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblCompany")) {
        thisDB.createObjectStore("tblCompany", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblZone")) {
        thisDB.createObjectStore("tblZone", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblLocation")) {
        thisDB.createObjectStore("tblLocation", { keyPath: "id" });
    }

    //create index coresponding to tables
    var tx = e.target.transaction;
    let tblModuleMaster_store = tx.objectStore("tblModuleMaster");
    if (!tblModuleMaster_store.indexNames.contains("enmApplication")) {
        tblModuleMaster_store.createIndex("enmApplication", "enmApplication", { unique: false });
    }
    let tblSubModuleMaster_store = tx.objectStore("tblSubModuleMaster");
    if (!tblSubModuleMaster_store.indexNames.contains("enmModule")) {
        tblSubModuleMaster_store.createIndex("enmModule", "enmModule", { unique: false });
    }
    let tblDocumentMaster_store = tx.objectStore("tblDocumentMaster");
    if (!tblDocumentMaster_store.indexNames.contains("enmModule")) {
        tblDocumentMaster_store.createIndex("enmModule", "enmModule", { unique: false });
    }
    if (!tblDocumentMaster_store.indexNames.contains("enmApplication")) {
        tblDocumentMaster_store.createIndex("enmApplication", "enmApplication", { unique: false });
    }
    let tblState_store = tx.objectStore("tblState");
    if (!tblState_store.indexNames.contains("countryId")) {
        tblState_store.createIndex("countryId", "countryId", { unique: false });
    }

    let tblCompany_store = tx.objectStore("tblCompany");
    if (!tblCompany_store.indexNames.contains("parentId")) {
        tblCompany_store.createIndex("parentId", "parentId", { unique: false });
    }
    let tblZone_store = tx.objectStore("tblZone");
    if (!tblZone_store.indexNames.contains("parentId")) {
        tblZone_store.createIndex("parentId", "parentId", { unique: false });
    }
    let tblLocation_store = tx.objectStore("tblLocation");
    if (!tblLocation_store.indexNames.contains("parentId")) {
        tblLocation_store.createIndex("parentId", "parentId", { unique: false });
    }
}



