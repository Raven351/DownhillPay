insert into prices.points (id, amount, price, pos_number) values 
(uuid_generate_v4(), 35, 350, 0),
(uuid_generate_v4(), 45, 450, 1),
(uuid_generate_v4(), 90, 900, 2),
(uuid_generate_v4(), 180, 1800, 3),
(uuid_generate_v4(), 270, 2700, 4),
(uuid_generate_v4(), 360, 3600, 5);

insert into prices.subscription(id, price, "period", pos_number, start_time, end_time, "name") values 
(uuid_generate_v4(), 5000, '1 hour', 0, null, null, '1 hour'),
(uuid_generate_v4(), 6000, '2 hours', 1, null, null, '2 hours'),
(uuid_generate_v4(), 7000, '3 hours', 2, null, null, '3 hours'),
(uuid_generate_v4(), 8000, '4 hours', 3, null, null, '4 hours'),
(uuid_generate_v4(), 8500, '5 hours', 4, null, null, '5 hours'),
(uuid_generate_v4(), 9500, '6 hours', 5, null, null, '6 hours'),
(uuid_generate_v4(), 6000, '6 hours', 6, '16:00', '21:00', '6 hour after 16:00');

insert into devices.ski_lift (id, "name", points_price) values 
('4171a50d-87be-47c2-833f-99930f2ba0af', 'Zwyrtlik', 40),
('c7f01f88-9b7f-4e0e-b262-493aa3014b20', 'Turnia', 55);

insert into devices.gate(id, "number", id_ski_lift) values 
(uuid_generate_v4(), 1, 'c7f01f88-9b7f-4e0e-b262-493aa3014b20'),
(uuid_generate_v4(), 1, 'c7f01f88-9b7f-4e0e-b262-493aa3014b20');
