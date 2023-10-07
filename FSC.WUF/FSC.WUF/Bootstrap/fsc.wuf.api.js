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
    let share = `event:${e.currentTarget.getAttribute(`${e.type}event`)}:${e.currentTarget.getAttribute('element-guid')}`;
    console.log(share);
    console.log(e.currentTarget);
    window.chrome.webview.postMessage(share);
}

restoreString = (str) => {
    //console.log('before convert: ' + str);

    /*let result = decodeURIComponent(
        str.replace(/\s+/g, '') // remove spaces
            .replace(/[0-9a-f]{2}/g, '%$&') // add '%' before each 2 characters
    );
    console.log('after convert: ' + result);*/

    const bytes = [];
    for (let i = 0; i < str.length; i += 2) {
        bytes.push(parseInt(str.substr(i, 2), 16));
    }
    const utf8String = new TextDecoder("utf-8").decode(new Uint8Array(bytes));
    //console.log('after convert: ' + utf8String);
    return utf8String;

    //return result;
}