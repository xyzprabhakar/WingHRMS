

//Load all data in index DB
var w;
if (localStorage.getItem("refreshData") == 1) {
    startWorker();
    localStorage.setItem("refreshData", 0);
}

function startstopWorker() {    
    if (document.getElementById("lblDataStatus").innerHTML.trim() == "Reload") {
        stopWorker();
        document.getElementById("lblDataStatus").innerHTML = "Reloading..";
        startWorker();
    }
    else {
        stopWorker();
        document.getElementById("lblDataStatus").innerHTML = "Reload";
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
}

function startWorker() {
    var baseUrl = localStorage.getItem("baseUrl");
    var dBVersion = localStorage.getItem("dBVersion");
    var token = localStorage.getItem('token');
    
    var workerData = { baseUrl_: baseUrl, dBVersion_: dBVersion, token_: token, startDownload_:1 };
    if (typeof (Worker) !== "undefined") {
        if (typeof (w) == "undefined") {
            w = new Worker("/apiJS/SetDbData.js");
            w.postMessage(workerData);
            }
            w.onmessage = function (event) {
                if (event.data == "Done") {
                    document.getElementById("lblDataStatus").innerHTML = "Reload";
                    //stopWorker();
                }
                else {
                    document.getElementById("lblDataStatus").innerHTML = event.data;
                }
                
            };
        
    } else {
        document.getElementById("lblDataStatus").innerHTML = "Sorry, your browser does not support Web Workers...";
    }
}

function stopWorker() {
    if (w != null) {
        if (w !== "undefined") {
            w.terminate();
            w = undefined;
        }
    }
}
//document.onreadystatechange = function () {
//    startWorker();
//    if (document.readyState !== "complete") {
//        document.querySelector(
//            "body").style.visibility = "hidden";
//        document.querySelector(
//            "#loader").style.visibility = "visible";
//    } else {
//        document.querySelector(
//            "#loader").style.display = "none";
//        document.querySelector(
//            "body").style.visibility = "visible";
//    }
//};

