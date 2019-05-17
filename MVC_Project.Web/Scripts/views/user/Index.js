var UserIndexControlador = function (htmlTableId, baseUrl, modalEditAction, modalDeleteAction, modelEditPasswordAction, modalEditPasswordId, formEditPasswordId, submitEditPasswordId) {
    var self = this;
    this.htmlTable = $('#' + htmlTableId);
    this.baseUrl = baseUrl;
    this.dataTable = {};
    this.modalEditPassword = $('#' + modalEditPasswordId);
    const utils = new Utils();
    this.initModal = function () {

    }
    this.init = function () {
        self.dataTable = this.htmlTable.DataTable({
            language: { url: 'Scripts/template/plugins/dataTables/lang/es_MX.json' },
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": this.baseUrl,
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: [
                { data: 'Id', title: "Id", visible: false },
                { data: 'Email', title: "Email" },
                { data: 'RoleName', title: "Rol" },
                { data: 'Name', title: "Nombre" },
                {
                    data: null, orderName: "CreatedAt", title: "Fecha Creación", autoWidth: false, className: "dt-center td-actions thead-dark",
                    render: function (data, type, row, meta) {
                        if (data.CreatedAt !== null && data.CreatedAt !== "") {
                            return moment(data.CreatedAt).format('DD-MMM-YYYY');
                        }
                        return '';
                    }
                },
                {
                    data: null, orderName: "UpdatedAt", title: "Fecha último acceso", autoWidth: false, className: "dt-center td-actions thead-dark",
                    render: function (data, type, row, meta) {
                        if (data.LastLoginAt !== null && data.LastLoginAt !== "") {
                            return moment(data.LastLoginAt).format('DD-MMM-YYYY');
                        }
                        return '';
                    }
                },
                {
                    data: null,
                    className: 'personal-options',
                    render: function (data) {
                        var deshabilitar = data.Status ? '<button class="btn btn-light btn-delete" title="Desactivar" style="margin-left:5px;"><span class="far fa-check-square "></span></button>' :
                            '<button class="btn btn-light btn-active" title="Activar" style="margin-left:5px;"><span class="far fa-square"></span></button>';
                        var buttons = '<div class="btn-group" role="group" aria-label="Opciones">' +
                            deshabilitar +
                            '<button class="btn btn-light btn-edit"><span class="fas fa-user-edit"></span></button>' +
                            '<button class="btn btn-light btn-edit-password"><span class="fas fa-edit"></span></button>' +
                            '</div>';
                        return buttons;
                    }
                }
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                aoData.push({ "name": "filtros", "value": getFiltros("form#SearchForm") });

                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });

        $(this.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-active',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var id = row.data().Uuid;

                swal({
                    title: "Confirmación",
                    text: "¿Desea activar al usuario?",
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
                                            title: data.Mensaje.Titulo,
                                            text: data.Mensaje.Contenido
                                        })
                                    } else {
                                        swal("Estatus cambiado!");
                                        self.dataTable.ajax.reload();
                                    }
                                },
                                error: function (xhr) {
                                    console.log('error');
                                }
                            });
                        } else {
                            swal("Cancelado", "Operación cancelada", "error");
                        }
                    });
            });

        $(this.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-delete',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var id = row.data().Uuid;

                swal({
                    title: "Confirmación",
                    text: "¿Desea inactivar al usuario?",
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
                                            title: data.Mensaje.Titulo,
                                            text: data.Mensaje.Contenido
                                        })
                                    } else {
                                        swal("Estatus cambiado!");
                                        self.dataTable.ajax.reload();
                                    }
                                },
                                error: function (xhr) {
                                    console.log('error');
                                }
                            });
                        } else {
                            swal("Cancelado", "Operación cancelada", "error");
                        }
                    });
            });

        $(self.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-edit-password',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var uuid = row.data().Uuid;
                var action = modelEditPasswordAction + "?uuid=" + uuid;
                self.modalEditPassword.find('.modal-body').load(action, function () {
                    self.modalEditPassword.modal("show");
                    let form = $("#" + formEditPasswordId);
                    console.log(form);
                    utils.actualizarValidaciones(form);
                    form.submit(function (e) {
                        e.preventDefault();
                        if (form.valid()) {
                            submitEditPassword(form).then(function (data) {

                            }).catch(function (data) {
                                console.log("Prueba --->", data);
                                if (data.status == 422) {
                                    dataObj = data.responseJSON;
                                    dataObj.errors.forEach(function (error) {
                                        $("span[data-valmsg-for='" + error.propertyName + "']").html(error.errorMessage);
                                    });
                                } else {

                                }
                            });
                        }
                    });
                    $("#" + submitEditPasswordId).click(function () {
                        form.submit();
                    });

                });
            });

        $(self.htmlTable, "tbody").on('click',
            'td.personal-options .btn-group .btn-edit',
            function () {
                var tr = $(this).closest('tr');
                var row = self.dataTable.row(tr);
                var uuid = row.data().Uuid;

                var form = document.createElement('form');
                document.body.appendChild(form);
                form.method = 'GET';
                form.action = "/User/Edit?uuid=" + uuid;

                var input = document.createElement('input');
                input.type = 'hidden';
                input.name = "uuid";
                input.value = uuid;
                form.appendChild(input);
                form.submit();
            });
        function submitEditPassword(form) {
            let url = form.attr("action");
            let method = form.attr("method");
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: url,
                    method: method,
                    data: form.serialize(),
                    dataType: "json"
                }).done(function (data) {
                    resolve(data);
                }).fail(function (jqXHR, error) {
                    reject(jqXHR);
                });
            });
        }
        function getFiltros(form) {
            var $inputs = $(form + ' [filtro="true"]');
            var nFiltros = $inputs.length;
            var filtros = [];
            for (i = 1; i <= nFiltros; i++) {                
                var input = $.grep($inputs, function (item) { return $(item).attr('filtro-order') == i; });
                filtros.push($(input).val());
            }

            return JSON.stringify(filtros);
        }
    }
}