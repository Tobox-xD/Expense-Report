SELECT *
FROM expense;

SELECT * 
FROM report;


SELECT * 
FROM expense JOIN report ON expense.report_id = report.id
WHERE report_id = 2;

SELECT SUM(expense.amount)
FROM expense JOIN report ON expense.report_id = report.id
WHERE report_id = 1;