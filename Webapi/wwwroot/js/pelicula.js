var dataTable;
$(document).ready(function () {
    dataTable = $('#tlbPeliculas').DataTable({
        "ajax": {
            "url": "/Peliculas/GetTodoPeliculas",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "20%" },
            { "data": "duracion", "width": "5%" },
            {
                "data": "fechadeCreacion",
                "width": "20%",
                "render": function (data) {
                    return moment(data).format('YYYY/MM/DD');
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href='/Peliculas/Edit/${data}' class='btn btn-success text-white' style='cursor: pointer;'>
                            <i class='fa fa-edit'></i> Editar
                        </a>
                        &nbsp;
                        <a onclick="Delete('/Peliculas/Delete/${data}')" class="btn btn-danger text-white" style="cursor: pointer;">
                            <i class="fas fa-trash-alt"></i> Borrar
                        </a>
                    </div>`;
                },
                'width': '20%'
            }
        ]
    });
});

function Delete(url) {
    swal({
        title: "¿Estás seguro que quieres eliminar la información?",
        text: "Esta acción no puede ser revertida",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        // Use dataTable here safely
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}