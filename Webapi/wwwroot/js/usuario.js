$(document).ready(function () {
    $('#tblUsuarios').DataTable({
        "ajax": {
            "url": "/Usuarios/GetTodosUsuarios",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "40%"},
            { "data": "userName", "width": "25%"},
            { "data": "nombre", "width": "25%"}
        ]
    });
})
