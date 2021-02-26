const PredefinedListImportControlador = function () {
  var self = this;
  this.importForm = $('#import-form');
  this.fileTypes = [
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    "application/vnd.ms-excel",
  ];
  console.log(this.importedList);

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
      validateFile(this, self.fileTypes);
    });

    $('.start-date-picker, .end-date-picker').datepicker({
      format: 'dd/mm/yyyy',
      language: 'es',
      orientation: 'bottom',
      assumeNearbyYear: 20
    });

    $('#btnClearForm').click(function () {
      self.importForm.find("span.field-validation-error").empty();
      sel.importForm.each(function () {
        this.reset();
      });
      $('.start-date-inputbox, .end-date-inputbox').val('');
      $('.custom-file-input').next('.custom-file-label').removeClass("selected").html('Seleccione un archivo...');
      $('#productsTable').DataTable().clear().draw();
    });

    $('#btnImport').click(function (e) {
      e.preventDefault();
      var fileValid = validateFile($('.custom-file-input')[0], self.fileTypes);
      if (self.importForm.valid() && fileValid) {
        var d = self.importForm.find('#StartDate').val().split("/");
        self.importForm.find('#StartDate').val(d[2] + "-" + d[1] + "-" + d[0]);
        d = self.importForm.find('#EndDate').val().split("/");
        self.importForm.find('#EndDate').val(d[2] + "-" + d[1] + "-" + d[0]);
        self.importForm.submit();
      }
    });

    $('#btnSaveData').click(function () {
      if (self.importForm.valid()) {
        self.importForm.submit();
      }
    });
  }
}