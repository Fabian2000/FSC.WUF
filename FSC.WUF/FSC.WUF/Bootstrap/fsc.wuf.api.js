//let idCollection = {};

addEventListener = (element, eventType, id) => {
    console.log(`Add Event with id ${id}`);
    try {
        for (let el of document.querySelectorAll(element)) {
            el.addEventListener(eventType, runEvent);
            //idCollection[el] = id;
            el.setAttribute(`${eventType}event`, id);
        }
    }
    catch (ex) { console.log("error: can not add event " + ex); }
}

removeEventListener = (element, eventType) => {
    console.log(`Remove Event with id ${id}`);
    try {
        for (let el of document.querySelectorAll(element)) {
            el.removeEventListener(eventType, runEvent);
            //delete idCollection[el];
            el.removeAttribute(`${eventType}event`);
        }
    }
    catch (ex) { console.log("error: can not remove event " + ex); }
}

runEvent = (e) => {
    //window.chrome.webview.postMessage(`event:${idCollection[e.target]}`);
    window.chrome.webview.postMessage(`event:${e.target.getAttribute(`${e.type}event`)}:${e.target.getAttribute('element-guid')}`);
}