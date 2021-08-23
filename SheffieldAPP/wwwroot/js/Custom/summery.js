$(document).ready(function () {
  loadDataTable();
});

function getSummury() {
  $('#tblData').DataTable().destroy();
  // $('#tblData').empty()
  loadDataTable($("#searchString").val());
}

function loadDataTable(searchString = "") {
  $("#tblData").DataTable({
    ajax: {
      url: "/User/Home/GetAll?searchString=" + searchString,
    },
    columns: [
      { data: "code", autoWidth: true },
      { data: "name", autoWidth: true },
      { data: "subject", autoWidth: true },
      { data: "marks", autoWidth: true },
      { data: "grade", autoWidth: true },
    ],
  });
}
