var ParametroIndexControlador = function (htmlTableId, baseUrl) {
    var self = this;
    this.htmlTable = $('#' + htmlTableId);
    this.baseUrl = baseUrl;
    this.dataTable = {};
    this.initModal = function () {

    }
    this.init = function () {
        self.dataTable = this.htmlTable.DataTable({
            language: { url: 'Scripts/custom/dataTableslang.es_MX.json' },
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": this.baseUrl,
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: [
                { data: 'Uuid', title: "Id", visible: false },
                { data: 'Clave', title: "Clave" },
                { data: 'Nombre', title: "Nombre" },
                { data: 'Valor', title: "Valor" },
                { data: 'Tipo', title: "Tipo" },
                { data: 'AlgoritmoUso', title: "Algoritmo de Uso" },
                {
                    data: null, title: "Estatus",
                    render: function (data, type, row, meta) {
                        if (data.Estatus) {
                            return 'Activo';
                        } else {
                            return 'Inactivo';
                        }
                    }
                },
                {
                    data: null,
                    className: 'personal-options',
                    title: 'Acciones',
                    render: function (data) {
                        var deshabilitarBtns = data.Estatus ?
                            '<button type="button" class="btn btn-light btn-delete" title="Borrar" style="margin-left:5px;"><span class="far fa-trash-alt "></span></button>' :
                            '';

                        var buttons = '<div class="btn-group" role="group" aria-label="Opciones">' +
                            '<button type="button" class="btn btn-light btn-edit" title="Editar Producto"><span class="fas fa-edit"></span></button>' +
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
                var uuid = row.data().Uuid;
                debugger;
                swal({
                    title: "Confirmación",
                    text: "¿Desea eliminar el Parámetro?",
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
                                data: { Uuid: uuid },
                                url: '/Parametros/Delete',
                                success: function (data) {
                                    if (!data) {
                                        swal({
                                            type: 'error',
                                            title: 'Error',
                                            text: 'Error al eliminar el parámetro'
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

        $(self.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-edit',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var uuid = row.data().Uuid;
                location.href = "/Parametros/Create?Uuid=" + uuid;

            });
    }
}