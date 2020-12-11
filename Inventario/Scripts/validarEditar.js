const campoCodigo = document.getElementById("codigoDeBarras");
const campoUbicacion = document.getElementById("ubicacion");
var prueba = document.getElementById("Validar");
const campoPatrimonio = document.getElementById("numeroPatrimonio");


const expresiones = {
    patrimonio: /(^[A-Z]{3}[0-9]{3}$)|(^\d{9}$)/,
    barras: /^\d{8,10}$/,
    ubicacion: /^[A-Z]-[0-9]+/
}
const comprobar2 = (e) => {
        if (expresiones.barras.test(campoCodigo.value)) {
            if (expresiones.ubicacion.test(campoUbicacion.value)) {
                document.getElementById("anadir").disabled = false;
                prueba.classList.add("d-none");
            } else {
                document.getElementById("anadir").disabled = true;
                prueba.classList.remove("d-none");
            }
        } else {
            prueba.classList.remove("d-none");
            document.getElementById("anadir").disabled = true;
        }
}
campoCodigo.addEventListener('keyup', comprobar2);
campoCodigo.addEventListener('blur', comprobar2);

campoUbicacion.addEventListener('keyup', comprobar2);
campoUbicacion.addEventListener('blur', comprobar2);

const comprobar = (e) => {
    if (expresiones.patrimonio.test(campoPatrimonio.value)) {
        document.getElementById("buscar").disabled = false;
        prueba.classList.add("d-none");
    } else {

        document.getElementById("buscar").disabled = true;
        prueba.classList.remove("d-none");
    }
}

campoPatrimonio.addEventListener('keyup', comprobar);
campoPatrimonio.addEventListener('blur', comprobar);





