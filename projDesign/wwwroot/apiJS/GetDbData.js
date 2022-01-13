function CheckIdExists_IndexDb(id, StoreName) {
    let dBVersion = localStorage.getItem('dBVersion');
    return new Promise(
        function (resolve, reject) {
            var openRequest = indexedDB.open("dpbs", dBVersion);
            openRequest.onsuccess = async function (e) {
                var db = e.target.result;
                let ObjectStoreState = await db.transaction(StoreName, "readwrite")
                    .objectStore(StoreName);
                var getAllRequest = await ObjectStoreState.getKey(parseInt( id));
                getAllRequest.onsuccess = async function () {
                    if (getAllRequest.result)
                        resolve(true);
                    else
                        resolve(false);
                }
            }
            openRequest.onerror == function () {
                reject(Error("Some Error in DB"));
            }
        });
}

function GetData_IndexDb(id, StoreName, dataType) {
    let dBVersion = localStorage.getItem('dBVersion');
    return new Promise(
        function (resolve, reject) {
            var openRequest = indexedDB.open("dpbs", dBVersion);
            openRequest.onsuccess = function (e) {
                var db = e.target.result;
                let ObjectStoreState = db.transaction(StoreName, "readwrite")
                    .objectStore(StoreName);
                var getAllRequest = ObjectStoreState.get(dataType == "int" ? parseInt(id) : dataType == "float" ? parseFloat(id) : id);
                getAllRequest.onsuccess = function (event) {
                    if (getAllRequest.result)
                        resolve(getAllRequest.result);
                    else
                        reject(Error('object not found'));
                };
            }
            openRequest.onerror == function () {
                reject(Error("Some Error in DB"));
            }
        });
}

function GetDataFromIndex_IndexDb(data, StoreName,IndexName) {
    let dBVersion = localStorage.getItem('dBVersion');
    return new Promise(
        function (resolve, reject) {
            var openRequest = indexedDB.open("dpbs", dBVersion);
            openRequest.onsuccess = function (e) {
                var db = e.target.result;
                let ObjectStoreState = db.transaction(StoreName, "readwrite").objectStore(StoreName);
                var getAllRequest = ObjectStoreState.index(IndexName).getAll(data);
                getAllRequest.onsuccess = function () {
                    resolve(getAllRequest.result);
                }
                getAllRequest.onerror = function () {
                    reject(Error("Some Error"));
                }
            }
            openRequest.onerror == function () {
                reject(Error("Some Error in DB"));
            }
        });
}

function GetDataAll_IndexDb(StoreName) {
    let dBVersion = localStorage.getItem('dBVersion');    
    return new Promise(
        function (resolve, reject) {
            var openRequest = indexedDB.open("dpbs", dBVersion);
            openRequest.onsuccess = function (e) {
                var db = e.target.result;
                let ObjectStoreState = db.transaction(StoreName, "readwrite")
                    .objectStore(StoreName);
                var getAllRequest = ObjectStoreState.getAll();
                getAllRequest.onsuccess = function () {                    
                    resolve(getAllRequest.result);
                }
                getAllRequest.onerror = function () {
                    reject(Error("Some Error"));
                }                
            }
            openRequest.onerror == function () {
                reject(Error("Some Error in DB"));
            }
        }        
    );
}
