const CedisIndexControlador = function (htmlTableId, baseUrl) {
    var self = this;
    this.htmlTable = $('#' + htmlTableId);
    this.baseUrl = baseUrl;
    this.dataTable = {};
    this.initModal = function () {

    }
    this.init = function () {
        self.dataTable = this.htmlTable.DataTable({
            language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": this.baseUrl,
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: [
                { data: 'Id', title: "Id", visible: false },
                { data: 'Code', title: "Clave" },
                { data: 'Name', title: "Nombre" },
                { data: 'Manager', title: "Responsable" },
                { data: 'Description', title: "Descripción" },
                {
                    data: null,
                    //className: 'personal-options',
                    title: 'Acciones',
                    render: function (data) {
                        var deshabilitarBtns = data.Status ?
                            '<button type="button" class="btn btn-light btn-delete" title="Borrar" style="margin-left:5px;"><span class="far fa-trash-alt "></span></button>' :
                            '';

                        var buttons = '<div class="btn-group" role="group" aria-label="Opciones">' +
                            '<button type="button" class="btn btn-light btn-edit" title="Editar"><span class="fa fa-pencil-square-o"></span></button>' +
                            deshabilitarBtns +
                            '</div>';
                        return buttons;
                    }
                }
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                aoData.push({ "name": "filtros", "value": $('form#SearchForm').serialize() });
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });

        $(this.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-delete',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var id = row.data().Uuid;
                swal({
                    title: "Confirmación",
                    text: "¿Desea eliminar CEDIS?",
                    showCancelButton: true,
                    confirmButtonClass: "btn-danger",
                    confirmButtonText: "Aceptar",
                    cancelButtonText: "Cancelar",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $.ajax({
                                type: 'POST',
                                async: true,
                                data: { uuid: id },
                                url: '/User/Delete',
                                success: function (data) {
                                    if (!data) {
                                        swal({
                                            type: 'error',
                                            title: 'Error',
                                            text: 'Error al eliminar el registro'
                                        })
                                    } else {
                                        swal("Registro eliminado");
                                        self.dataTable.ajax.reload();
                                    }
                                },
                                error: function (xhr) {
                                    console.log('error');
                                }
                            });
                        } else {
                            swal.close();
                        }
                    });
            });
    }
}

$('#btnClearForm').click(function () {
    $('#SearchForm').each(function () {
        this.reset();
    });
    $('#table').DataTable().draw();
});

$('.btn-filter').click(function () {
    $('#table').DataTable().draw();
});