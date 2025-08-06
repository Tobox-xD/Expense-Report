let reportsum = document.getElementById("report-sum");
let reportTable = document.getElementById("report-table");

const params = new URLSearchParams(window.location.search);
const idFromURL = params.get('id'); // returns string or null
console.log(params);

function getSum(id) {
    fetch('http://localhost:3000/getReportSumById/' + id)
        .then(res => res.json())
        .then(sum => {
            console.log("Total sum:", sum);
            reportsum.innerText = "Total: " + sum + "€";
        })
        .catch(err => reportsum.innerText = "Not a valid Id");
}


function addRow(description, amount) {
  const row = reportTable.insertRow();
  const descCell = row.insertCell(0);
  const amountCell = row.insertCell(1);
  descCell.textContent = description;
  amountCell.textContent = amount + " €";
}

function clearTable() {
    while (reportTable.rows.length > 0) {
        reportTable.deleteRow(0);
    }
}

function getReportContent(id) {
  fetch('http://localhost:3000/getReportById/' + id)
    .then(res => res.json())
    .then(reportItems => {
      let descriptionsList = reportItems.map(item => item.description);
      console.log("Descriptions:", descriptionsList);
      
      let amountList = reportItems.map(item => item.amount) 
      console.log("Amounts:", amountList);

        for (let index = 0; index < descriptionsList.length; index++) {
        const description = descriptionsList[index]; 
        const amount = amountList[index];
        
        addRow(description, amount);
     }
      
    })
    .catch(err => clearTable());
}

function addEntry(id, description, amount, created_at) {
  return fetch('http://localhost:3000/addEntry/' + id, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ description, amount, createdAt: created_at }) // note camelCase -> PascalCase binding
  })
  .then(res => {
    if (!res.ok) return res.text().then(t => { throw new Error(`Failed to add entry: ${res.status} - ${t}`); });
    return res.json().catch(() => null);
  });
}



function loadContent(){
  clearTable();
  getSum(idFromURL);
  getReportContent(idFromURL);
}

let addButton = document.getElementById("add-to-report");
let removeButton = document.getElementById("remove-from-report");

addButton.addEventListener("click", () => {
  addEntry(idFromURL, "Lol", 12.30, new Date().toISOString())
    .then(() => loadContent())
    .catch(err => console.error("Add entry failed:", err));
});



removeButton.addEventListener("click", () => {
  
});


loadContent();
