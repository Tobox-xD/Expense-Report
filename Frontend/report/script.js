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
            reportsum.innerText = "Total: " + sum;
        })
        .catch(err => reportsum.innerText = "Not a valid Id");
}


function addRow(description, amount) {
  const row = reportTable.insertRow();
  const descCell = row.insertCell(0);
  const amountCell = row.insertCell(1);
  descCell.textContent = description;
  amountCell.textContent = amount;
}

function clearTable() {
    while (reportTable.rows.length > 1) {
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


clearTable();
getSum(idFromURL);
getReportContent(idFromURL);
