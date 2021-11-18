/** 
 * Helper functions adapted from sample code from:
 * Nayuki - QR-Code-generator (MIT License)
 * https://www.nayuki.io/page/qr-code-generator-library
**/

function appendCanvas(outputElem) {
    let result = document.createElement("canvas");
    outputElem.appendChild(result);
    return result;
}

function drawCanvas(qr, canvas) {
    const scale = 5;
    const border = 2;
    const lightColor = "#FFFFFF";
    const darkColor = "#000000";
    const size = (qr.size + border * 2) * scale;
    
    canvas.width = size;
    canvas.height = size;
    
    let ctx = canvas.getContext("2d");
    for (let y = -border; y < qr.size + border; y++) {
        for (let x = -border; x < qr.size + border; x++) {
            ctx.fillStyle = qr.getModule(x, y) ? darkColor : lightColor;
            ctx.fillRect((x + border) * scale, (y + border) * scale, scale, scale);
        }
    }
}

window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    const outputElem = document.getElementById("qrCode");
    const qr = qrcodegen.QrCode.encodeText(uri, qrcodegen.QrCode.Ecc.MEDIUM);
    
    drawCanvas(qr, appendCanvas(outputElem));
});