var dataTable;

$(document).ready(function () {
  loadDataTable();
  loadDataTableSubjects();
  loadDataTableStudentSubjects();
});

function loadDataTable() {
  $("#tblData").DataTable({
    ajax: {
      url: "/User/StudentSubject/GetAllStudents",
    },
    columns: [
      { data: "code", autoWidth: true },
      { data: "name", autoWidth: true },
      {
        data: "id",
        render: function (data) {
          return `
                            <div class="text-center">
                                <a href="/User/StudentSubject/SetSubjects?studentId=${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="fas fa-book-reader"></i>&nbsp;&nbsp Subjects
                                </a>
                            </div>
                           `;
        },
        autoWidth: true,
      },
    ],
  });
}

function loadDataTableSubjects() {
  $("#tblDataSub").DataTable({
    ajax: {
      url: "/User/StudentSubject/GetAllSubjects",
    },
    columns: [
      { data: "code", autoWidth: true },
      { data: "name", autoWidth: true },
      {
        data: "id",
        render: function (data, type, row) {
          return `
                            <div class="text-center">
                                <a onclick=SelectSubject('${data}','${row.name}') class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-plus"></i>
                                </a>
                            </div>
                           `;
        },
        autoWidth: true,
      },
    ],
  });
}

function SelectSubject(subId, name) {
  Swal.fire({
    title: `Select ${name} For Subject?`,
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Select",
  }).then((select) => {
    if (select.isConfirmed) {
      AddSubject(subId);
    }
  });
}

function AddSubject(subId) {
  var url = "/User/StudentSubject/AddSubject";
  jQuery.getJSON(url, { fetch: subId }, function (data) {
    if (data.success) {
      toastr.success(data.message);
      dataTable.ajax.reload();
    } else {
      toastr.error(data.message);
    }
  });
}

function loadDataTableStudentSubjects() {
  dataTable = $("#tblDataStSub").DataTable({
    ajax: {
      url: "/User/StudentSubject/GetAllStudentSubjects",
    },
    columns: [
      { data: "code", autoWidth: true },
      { data: "name", autoWidth: true },
      {
        data: "id",
        render: function (data, type, row) {
          return `
                  <div class="text-center">
                    <a onclick=Delete("/User/StudentSubject/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                        <i class="fas fa-trash-alt"></i>
                    </a>
                  </div>`;
        },
        autoWidth: true,
      },
    ],
  });
}


function Delete(url) {
  Swal.fire({
    title: "Are you sure you want to Delete?",
    text: "You will not be able to restore the data!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!",
  }).then((willDelete) => {
    if (willDelete.isConfirmed) {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function (data) {
          if (data.success) {
            toastr.success(data.message);
            dataTable.ajax.reload();
          } else {
            toastr.error(data.message);
          }
        },
      });
    }
  });
}
