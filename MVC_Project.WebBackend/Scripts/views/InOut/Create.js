$(function () {
    const utils = new Utils();
    const createForm = document.getElementById('createForm');
    const createFormSubmitBtn = document.querySelector('button[form="createForm"]');
    const addProductForm = document.getElementById('addProductForm');
    let products = [];

    const toasterOptions = {
        closeButton: false,
        progressBar: true,
        positionClass: 'toast-top-right',
        "showDuration": "300",
        "hideDuration": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    function renderBoolToText(value) {
        return value ? 'Si' : 'No';
    }

    function addProduct(form) {
        const item = utils.UserInterfaceToData($(form));
        if (products.filter(product => product.Sku === item.Sku).length > 0) {
            toastr.warning('El producto ya se encuentra en la lista', "", toasterOptions);
            return;
        }
        item.Name = form.querySelector('option:checked').text;
        products.push(item);
        form.reset();
        $(form).find('.i-checks').iCheck('update');
        productsDataTable.row.add(item).draw();
        createFormSubmitBtn.disabled = false;
    }

    function removeProduct(sku) {
        products = products.filter(product => product.Sku !== sku)
        createFormSubmitBtn.disabled = products.length <= 0;
    }

    $('.date').datepicker({
        format: 'dd/mm/yyyy',
        language: 'es',
        orientation: 'bottom',
        assumeNearbyYear: 20
    });

    const productsDataTable = $('#productsTable').DataTable({
        pageLength: 10,
        language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
        orderMulti: false,
        searching: false,
        ordering: false,
        columnDefs: [
            { targets: 0, data: 'Sku' },
            { targets: 1, data: 'Name' },
            { targets: 2, data: 'Quantity', width: '120px' },
            { targets: 3, data: 'IsStrategic', width: '90px', render: renderBoolToText },
            { targets: 4, data: 'IsPrioritary', width: '90px', render: renderBoolToText },
            { targets: 5, data: 'IsTactic', width: '90px', render: renderBoolToText },
            {
                targets: 6,
                data: null,
                width: '90px',
                render: function (data) {
                    const buttons = `<div class="btn-group" role="group" aria-label="Opciones">
                                    <button type="button" class="btn btn-danger btn-remove" title="Eliminar"><span class="far fa-trash-alt"></span></button>
                                </div>
                            `;
                    return buttons;
                }
            }
        ]
    });

    $('#productsTable tbody').on('click', 'button.btn-remove', function () {
        const row = productsDataTable.row($(this).parents('tr'));
        removeProduct(row.data().Sku);
        row.remove().draw();
    });

    addProductForm.addEventListener('submit', function (evt) {
        evt.preventDefault();
        if ($(evt.target).valid()) {
            addProduct(evt.target);
        }
    })

    function createHiddenInput(name, value) {
        const input = document.createElement('input');
        input.setAttribute('name', name);
        input.setAttribute('value', value);
        input.setAttribute('type', 'hidden');
        return input;
    }

    createForm.addEventListener('submit', function (evt) {
        if ($(evt.target).valid()) {
            if (products.length <= 0) {
                createFormSubmitBtn.disabled = true;
                toastr.error('Debe añadir al menos un producto', "", toasterOptions);
                evt.preventDefault();
            }
            products.forEach((product, index) => {
                const variableName = `Products[${index}]`;
                const productSKUInput = createHiddenInput(`${variableName}.Sku`, product.Sku);
                const productQuantityInput = createHiddenInput(`${variableName}.Quantity`, product.Quantity);
                const productIsStrategicInput = createHiddenInput(`${variableName}.IsStrategic`, product.IsStrategic);
                const productIsPrioritaryInput = createHiddenInput(`${variableName}.IsPrioritary`, product.IsPrioritary);
                const productIsTacticInput = createHiddenInput(`${variableName}.IsTactic`, product.IsTactic);
                evt.target.appendChild(productSKUInput);
                evt.target.appendChild(productQuantityInput);
                evt.target.appendChild(productIsStrategicInput);
                evt.target.appendChild(productIsPrioritaryInput);
                evt.target.appendChild(productIsTacticInput);
            });
        }
    })

});