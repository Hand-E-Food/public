const letterSets = [
    'eaoiuy',
    'tnsrhdlccmmffyywwggppbbvvkkkxxxqqqjjjzzz',
]

function createName() {
    let name = '';
    let length = Math.floor(Math.random() * 9) + 7
    let x = Math.floor(Math.random() * 2);
    for(let i = 0; i < length; i++) {
        const letterSet = letterSets[x];
        x = 1 - x;
        name += letterSet.charAt(Math.floor(Math.random() * letterSet.length));
    }
    return name;
}

window.onload = (e) => {
    window.onload = undefined;
    for(let count = 0; count < 30; count++) {
        const node = document.createElement('p');
        node.appendChild(document.createTextNode(createName()));
        document.body.appendChild(node);
    }
};
