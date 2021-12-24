//Load all data in index DB
var w;
if (localStorage.getItem("refreshData") == 1) {
    startWorker();    
}

function startstopWorker() {    
    if (document.getElementById("lblDataStatus").innerHTML.trim() == "Reload") {
        document.getElementById("lblDataStatus").innerHTML = "Reloading..";
        startWorker();
    }
    else {
        stopWorker();
        document.getElementById("lblDataStatus").innerHTML = "Reload";
    }
}

function startWorker() {
    var baseUrl = localStorage.getItem("baseUrl");
    var dBVersion = localStorage.getItem("dBVersion");
    var token = localStorage.getItem('token');
    var workerData = { baseUrl_: baseUrl, dBVersion_: dBVersion, token_: token };
    if (typeof (Worker) !== "undefined") {
        if (typeof (w) == "undefined") {
            w = new Worker("/apiJS/SetDbData.js");
            w.postMessage(workerData);
            }
            w.onmessage = function (event) {
                if (event.data == "Done") {
                    document.getElementById("lblDataStatus").innerHTML = "Reload";
                    stopWorker();
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