const campoCedula = document.getElementById("Cedula");
const campoNombre = document.getElementById("Nombre");
const check = document.getElementById("msjError");


const expresiones = {
    cedula: /^\d{9}$/,  //formato de cedula #########=9 numeros
    nombre: /^[ñA-Za-z _]*[ñA-Za-z][ñA-Za-z _]*$/
}    
           
//Comprueba que se cumplan las expresiones para Cedula
const comprobarCedula = (e) => {
    if (expresiones.cedula.test(campoCedula.value)) {
        document.getElementById("Registrar").disabled = false;//habilita el boton registrar
        check.classList.add("d-none");
    } else {
        document.getElementById("Registrar").disabled = true;//deshabilita el boton registrar
        check.classList.remove("d-none");
    }    
}

//Comprueba que se cumplan las expresiones para Nombre
const comprobarNombre = (e) => {   
    if (expresiones.nombre.test(campoNombre.value)) {
        document.getElementById("Registrar").disabled = false;//habilita el boton registrar
        check.classList.add("d-none");
    } else {
        document.getElementById("Registrar").disabled = true;//deshabilita el boton registrar
        check.classList.remove("d-none");
    }
 }

campoCedula.addEventListener('keyup', comprobarCedula);//ejecuta la validacion cuando se libera el click del mouse
campoCedula.addEventListener('blur', comprobarCedula);//ejecuta la validacion cuando el campo pierde el enfoque del mouse

campoNombre.addEventListener('keyup', comprobarNombre);//ejecuta la validacion cuando se libera el click del mouse
campoNombre.addEventListener('blur', comprobarNombre);//ejecuta la validacion cuando el campo pierde el enfoque del mouse