window.quiniegol = window.quiniegol || {};

window.quiniegol.descargar = function (nombre, tipo, contenidoBase64) {
    const enlace = document.createElement("a");
    enlace.download = nombre;
    enlace.href = `data:${tipo};base64,${contenidoBase64}`;
    document.body.appendChild(enlace);
    enlace.click();
    enlace.remove();
};
