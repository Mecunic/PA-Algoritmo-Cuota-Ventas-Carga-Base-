const PredefinedListImportControlador = function (importedList) {
  var self = this;
  this.fileTypes = [
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    "application/vnd.ms-excel",
  ];
  this.importedList = importedList;

  this.init = function () {
    $('#productsTable').DataTable({
      pageLength: 10,
      language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
      orderMulti: false,
      searching: false,
      ordering: false,
    });

    $('.custom-file-input').on('change', function () {
      let fileName = $(this).val().split('\\').pop();
      $(this).next('.custom-file-label').addClass("selected").html(fileName);
    });

    $('.start-date-picker, .end-date-picker').datepicker({
      format: 'dd/mm/yyyy',
      language: 'es',
      orientation: 'bottom',
      assumeNearbyYear: 20
    });

    $('#btnClearForm').click(function () {
      $('#import-form').find("span.field-validation-error").empty();
      $('#import-form').each(function () {
        this.reset();
      });
      $('.start-date-picker, .end-date-picker').val();
      $('#btnSaveData').hide();
      $('.custom-file-input').next('.custom-file-label').removeClass("selected").html('Seleccione un archivo...');
      $('#productsTable').DataTable().clear().draw();
    });

    $('#btnImport').click(function (e) {
      e.preventDefault();
      var fileValid = validateFile($('.custom-file-input')[0], self.fileTypes);
      if ($('#import-form').valid() && fileValid) {
        $('#import-form').submit();
      }
    });
  }
}