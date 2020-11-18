//const formulario = document.getElementById("formulario");
//const inputs = document.querySelectorAll("#formulario input");
var prueba = document.getElementById("Validar");
const campoPatrimonio = document.getElementById("numeroDePatrimonio");
const expresiones = {
    patrimonio: /(^[A-Z]{3}[0-9]{3}$)|(^\d{9}$)/,
    barras: /\d{9}/,
    ubicacion: /^[A-Z]-[0-9]+/
}
const comprobar = (e) => {
    if (expresiones.patrimonio.test(campoPatrimonio.value)) {
        document.getElementById("buscar").disabled = false;
        prueba.classList.add("d-none");
    } else
    {
 
        document.getElementById("buscar").disabled = true;
        prueba.classList.remove("d-none");
    }
}

campoPatrimonio.addEventListener('keyup', comprobar);
campoPatrimonio.addEventListener('blur', comprobar);

