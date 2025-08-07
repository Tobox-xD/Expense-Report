let showReportButton = document.getElementById("show-report");
let editReportButton = document.getElementById("edit-report");
let idInput = document.getElementById("report-id");
let reportTable = document.getElementById("report-table");

function addRow(id, name) {
  const row = reportTable.insertRow();
  const descCell = row.insertCell(0);
  const amountCell = row.insertCell(1);
  descCell.textContent = id;
  amountCell.textContent = name;
}

function clearTable() {
  while (reportTable.rows.length > 0) {
    reportTable.deleteRow(0);
  }
}

async function getReports() {
  await fetch("http://localhost:3000/getReports")
    .then((res) => res.json())
    .then((reports) => {
      clearTable();
      reports.forEach((item) => {
        addRow(item.id, item.name);
      });
    })
    .catch((err) => {
      clearTable();
      console.error(err);
    });
}

showReportButton.addEventListener("click", () => {
  const id = idInput.value.trim();
  if (!id) {
    alert("Please provid an id!");
    return;
  }
  window.location.href = `http://127.0.0.1:5500/Expense-Report/Frontend/report/?id=${id}`;
});

getReports();
