let showReportButton = document.getElementById("show-report");
let editReportButton = document.getElementById("edit-report");
let idInput = document.getElementById("report-id");


showReportButton.addEventListener("click", () => {
  const id = idInput.value.trim();
  if (!id)
  {
    alert("Please provid an id!");
    return;
  }
  window.location.href = `http://127.0.0.1:5500/Expense-Report/Frontend/report/?id=${id}`;
});

