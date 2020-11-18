//const formulario = document.getElementById("formulario");
//const inputs = document.querySelectorAll("#formulario input");
const btn = document.getElementById("anadir");
const campoPatrimonio = document.getElementById("numeroDePatrimonio");
const expresiones = {
    patrimonio: /(^[A-Z]{3}[0-9]+$|\d{5,9})/,
    barras: /\d{9}/,
    ubicacion: /^[A-Z]-[0-9]+/
}
btn.disabled = false;
const comprobar = (e) => {
    if (expresiones.patrimonio.test()) {

    }
}

campoPatrimonio.addEventListener('keyup', comprobar);
campoPatrimonio.addEventListener('blur', comprobar);

