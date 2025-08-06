-- Insert reports
INSERT INTO report (name, enddate) VALUES
('First report', '2025-08-01 14:32:00'),
('Second report', '2025-08-15 14:32:00'),
('Third report', '2025-08-30 14:32:00');

-- Insert expenses
INSERT INTO expense (description, amount, created_at, report_id) VALUES
-- First report
('Groceries - Aldi', 45.90, '2025-08-01 14:32:00', 1),
('Dinner - Italian Restaurant', 28.50, '2025-08-02 19:15:00', 1),
('Fuel - Shell Station', 65.00, '2025-08-03 09:05:00', 1),
('Movie Tickets', 22.00, '2025-08-03 21:00:00', 1),
('Bus Ticket', 3.20, '2025-08-04 07:45:00', 1),
('Coffee - Starbucks', 4.50, '2025-08-04 09:30:00', 1),
('Gym Membership', 40.00, '2025-08-04 08:00:00', 2),
('Online Subscription - Netflix', 12.99, '2025-08-05 07:30:00', 2),
('Laptop Mouse', 25.00, '2025-08-06 16:12:00', 2),
('Lunch - Thai Express', 14.80, '2025-08-07 12:45:00', 2),
('Water Bill', 30.25, '2025-08-08 08:15:00', 2),
('Groceries - Lidl', 38.75, '2025-08-21 15:05:00', 3),
('Fuel - BP', 60.00, '2025-08-22 10:22:00', 3),
('Cinema Snacks', 8.50, '2025-08-22 20:30:00', 3),
('Clothing - T-Shirt', 19.90, '2025-08-23 11:00:00', 3),
('Electricity Bill', 75.40, '2025-08-25 09:00:00', 3),
('Coffee Beans - Local Roaster', 12.50, '2025-08-26 09:00:00', 3);
