var canvas = document.getElementById("signature-pad");
var clearButton = document.querySelector(".clear-signature");
var generatePdfButton = document.querySelector(".generate-pdf");
var signaturePad = new SignaturePad(canvas, {
  backgroundColor: 'rgb(255, 255, 255)'
});

function resizeCanvas() {
  var ratio = Math.max(window.devicePixelRatio || 1, 1);

  canvas.width = canvas.offsetWidth * ratio;
  canvas.height = canvas.offsetHeight * ratio;
  canvas.getContext("2d").scale(ratio, ratio);

  signaturePad.clear();
}

window.onresize = resizeCanvas;
resizeCanvas();

clearButton.addEventListener("click", function (event) {
  signaturePad.clear();
});

generatePdfButton.addEventListener("click", function (event) {
  if (signaturePad.isEmpty()) {
    alert("Firma primero");
  } else {
    var dataURL = signaturePad.toDataURL();
    var form = document.createElement("form");
    var signature = document.createElement("input");
    form.action = "/PdfSignature/GeneratePdf";
    form.method = "POST";

    signature.value = dataURL;
    signature.name = "signature";
    form.appendChild(signature);

    document.body.appendChild(form);

    form.submit();

    document.body.removeChild(form);
  }
});