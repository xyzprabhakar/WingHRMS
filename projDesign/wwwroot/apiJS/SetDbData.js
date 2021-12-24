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
}]

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
    console.log( baseUrl)
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
     }).then(response => response.json())
         .then(data => {
            
            if (data.messageType == 1) {
                
                for (var index in Apidata.tables) {
                    
                    let tablename_ = Apidata.tables[index].tableName;
                    let keyname_ = Apidata.tables[index].keyName;
                    SaveinDb(keyname_, tablename_, data.returnId, Apidatas, ApidataPosition);
                }
                
            }
        });
}



async function SaveinDb(KeyName, Tablename, returnId, Apidatas,i) {
    let ObjectStore = db.transaction(Tablename, "readwrite")
        .objectStore(Tablename);
    let ReqSuccess = await ObjectStore.clear();
    ReqSuccess.onsuccess = async function (event) {
        console.log(KeyName)
        if (KeyName == "") {
            for (var j in returnId) {
                await ObjectStore.add(returnId[j]);
                isAllDataDownloaded();
            }
            
        }
        else {
            let tempdata = returnId[KeyName];
            console.log(JSON.stringify(tempdata));
            console.log(ObjectStore.name);
            for (var j in tempdata) {
                await ObjectStore.add(tempdata[j]);
                isAllDataDownloaded();
            }
            
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
}



