
var canvas = document.getElementById("signature");

var signaturePad = new SignaturePad(canvas,{
     minWidth: 1,
    maxWidth: 3,
    penColor: "rgb(66, 133, 244)"
});

// // Returns signature image as data URL (see https://mdn.io/todataurl for the list of possible parameters)
// signaturePad.toDataURL(); // save image as PNG
// signaturePad.toDataURL("image/jpeg"); // save image as JPEG
// signaturePad.toDataURL("image/svg+xml"); // save image as SVG

// // Draws signature image from data URL
// signaturePad.fromDataURL("data:image/png;base64,iVBORw0K...");

// // Returns signature image as an array of point groups
// const data = signaturePad.toData();

// // Draws signature image from an array of point groups
// signaturePad.fromData(data);

// // Clears the canvas
// signaturePad.clear();

// // Returns true if canvas is empty, otherwise returns false
// signaturePad.isEmpty();

// // Unbinds all event handlers
// signaturePad.off();

// // Rebinds all event handlers
// signaturePad.on();

// var signaturePad = new SignaturePad(document.getElementById('signature'), {})
function resizeCanvas() {
    var ratio =  Math.max(window.devicePixelRatio || 1, 1);
    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    canvas.getContext("2d").scale(ratio, ratio);
    signaturePad.clear(); // otherwise isEmpty() might return incorrect value
}

window.addEventListener("resize", resizeCanvas);
resizeCanvas();