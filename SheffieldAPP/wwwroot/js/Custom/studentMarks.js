$(document).ready(function () {
  loadDataTable();
  loadDataTableStudentSubjects();
});

function loadDataTable() {
  $("#tblData").DataTable({
    ajax: {
      url: "/User/StudentMarks/GetAllStudents",
    },
    columns: [
      { data: "code", autoWidth: true },
      { data: "name", autoWidth: true },
      {
        data: "id",
        render: function (data) {
          return `
                            <div class="text-center">
                                <a href="/User/StudentMarks/MarkSubjects?studentId=${data}" class="btn btn-primary text-white" style="cursor:pointer">
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

function loadDataTableStudentSubjects() {
  $("#tblDataStSub").DataTable({
    ajax: {
      url: "/User/StudentMarks/GetAllStudentSubjects",
    },
    columns: [
      { data: "subCode", autoWidth: true },
      { data: "subject", autoWidth: true },
      { data: "score", autoWidth: true },
      { data: "grade", autoWidth: true },
      {
        data: "id",
        render: function (data) {
          return `
                            <div class="text-center">
                                <a href="/User/StudentMarks/RoadToUpsert?studentSubjectId=${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="fas fa-check"></i>
                                </a>
                            </div>
                           `;
        },
        autoWidth: true,
      },
    ],
  });
}
