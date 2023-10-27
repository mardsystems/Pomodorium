export function initialize() {
    let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    openDbRequest.onupgradeneeded = function () {
        let db = openDbRequest.result;

        var eventStore = db.createObjectStore("EventStore", { keyPath: "id" });

        eventStore.createIndex("name", "name", { unique: false });

        db.createObjectStore("PomodoroDetails", { keyPath: "id" });
        db.createObjectStore("PomodoroQueryItems", { keyPath: "id" });
    }
}

export async function getAll(collectionName) {
    let getAllPromise = new Promise((resolve, reject) => {
        let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        openDbRequest.onsuccess = function () {
            let db = openDbRequest.result;
            let transaction = db.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let getRequest = collection.getAll();

            getRequest.onsuccess = function (e) {
                resolve(getRequest.result);
            }

            getRequest.onerror = function (err) {
                reject(err);
            }
        }

        openDbRequest.onerror = function (err) {
            reject(err);
        }
    });

    let result = await getAllPromise;

    return result;
}

export async function getEvents(name) {
    let events = [];

    let getEventsPromise = new Promise((resolve, reject) => {
        let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        openDbRequest.onsuccess = function () {
            let db = openDbRequest.result;
            let transaction = db.transaction("EventStore", "readonly");
            let collection = transaction.objectStore("EventStore");
            let nameIndex = collection.index("name");

            let nameRange = IDBKeyRange.only(name)

            let openCursorRequest = nameIndex.openCursor(nameRange);

            openCursorRequest.onsuccess = function (e) {
                let cursor = e.target.result;

                if (cursor) {
                    events.push(cursor.value);

                    cursor.continue();
                }
                else {
                    resolve(events);
                }
            }

            openCursorRequest.onerror = function (err) {
                reject(err);
            }
        }

        openDbRequest.onerror = function (err) {
            reject(err);
        }
    });

    let result = await getEventsPromise;

    return result;
}

export async function get(collectionName, id) {
    let getPromise = new Promise((resolve, reject) => {
        let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        openDbRequest.onsuccess = function () {
            let db = openDbRequest.result;
            let transaction = db.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let getRequest = collection.get(id);

            getRequest.onsuccess = function (e) {
                resolve(getRequest.result);
            }

            getRequest.onerror = function (err) {
                reject(err);
            }
        }

        openDbRequest.onerror = function (err) {
            reject(err);
        }
    });

    let result = await getPromise;

    return result;
}

export function put(collectionName, value) {
    let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    openDbRequest.onsuccess = function () {
        let db = openDbRequest.result;
        let transaction = db.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName);
        collection.put(value);
    }

    openDbRequest.onerror = function (err) {
        console.log("Error on put:", value);
    }
}

export function remove(collectionName, id) {
    let openDbRequest = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    openDbRequest.onsuccess = function () {
        let db = openDbRequest.result;
        let transaction = db.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName);
        collection.delete(id);
    }

    openDbRequest.onerror = function (err) {
        console.log("Error on remove:", id);
    }
}

let CURRENT_VERSION = 1;
let DATABASE_NAME = "Pomodorium";
