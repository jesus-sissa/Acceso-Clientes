function showModal(titulo, mensaje) {
    $("#myModal .modal-title").html(titulo);
    $("#myModal .modal-body").html(mensaje);
    $("#myModal").modal();
}



function cadenaNumero(cadena) {
    if (isNaN(cadena)) {
        return false;
    }
    return true;
}

function validarEntrada() {
    var tbxTelefono = document.getElementById("<%= tbx_Telefono.ClientID%>")
    var valorTelefono = tbxTelefono.value;

    if (!validarEntrada(valorTelefono)) {
        showModal("Login", "Introduzca un número de telefono correcto.");
        return false;
    }

    return true;
}
function openTab(tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');

}
