const campoPatrimonio = document.getElementById("numeroDePatrimonio");
const campoCodigo = document.getElementById("codigoDeBarras");
const campoUbicacion = document.getElementById("ubicacion");
const sumario = document.getElementById("v");
const expresiones = {
    patrimonio: /(^[A-Z]{3}[0-9]{3}$)|(^\d{9}$)/,
    barras: /^\d{9}/,
    ubicacion: /^[A-Z]-[0-9]/
}

const comprobar = (e) => {
    if (expresiones.patrimonio.test(campoPatrimonio.value)) {
        if (expresiones.barras.test(campoCodigo.value)) {
            if (expresiones.ubicacion.test(campoUbicacion.value)) {
                document.getElementById("anadir").disabled = false;
                sumario.classList.add("d-none");
            } else {
                document.getElementById("anadir").disabled = true;
                sumario.classList.remove("d-none");
            }
        } else {
            sumario.classList.remove("d-none");
            document.getElementById("anadir").disabled = true;
        }
    } else {
        sumario.classList.remove("d-none");
        document.getElementById("anadir").disabled = true;
    }
}

campoPatrimonio.addEventListener('keyup', comprobar);
campoPatrimonio.addEventListener('blur', comprobar);

campoCodigo.addEventListener('blur', comprobar);
campoCodigo.addEventListener('keyup', comprobar);

campoUbicacion.addEventListener('blur', comprobar);
campoUbicacion.addEventListener('keyup', comprobar);