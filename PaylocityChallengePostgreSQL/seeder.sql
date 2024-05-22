DELETE FROM table_dependents WHERE employee_id IN (1, 2, 3, 4, 5);
DELETE FROM table_employees WHERE employee_id IN (1, 2, 3, 4, 5);

INSERT INTO table_employees (employee_id, first_name, last_name, date_of_birth, salary) VALUES (1, 'John', 'Doe', '1980-01-15', 50000.00);
INSERT INTO table_employees (employee_id, first_name, last_name, date_of_birth, salary) VALUES (2, 'Jane', 'Smith', '1975-05-22', 75000.50);
INSERT INTO table_employees (employee_id, first_name, last_name, date_of_birth, salary) VALUES (3, 'Michael', 'Johnson', '1985-03-10', 60000.00);
INSERT INTO table_employees (employee_id, first_name, last_name, date_of_birth, salary) VALUES (4, 'Emily', 'Davis', '1990-11-30', 82000.75);
INSERT INTO table_employees (employee_id, first_name, last_name, date_of_birth, salary) VALUES (5, 'Robert', 'Brown', '1978-07-19', 45000.20);

INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (1, 'Jane', 'Doe', 1, '1981-04-12'); -- Spouse
INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (1, 'Jack', 'Doe', 3, '2010-08-05'); -- Child
INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (1, 'Jill', 'Doe', 3, '2012-09-15'); -- Child

INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (2, 'Emily', 'Smith', 2, '1980-02-20'); -- Domestic partner
INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (2, 'Ella', 'Smith', 3, '2015-06-25'); -- Child

INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (3, 'Rachel', 'Johnson', 1, '1986-07-11'); -- Spouse

INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (4, 'Sam', 'Davis', 3, '2017-05-30'); -- Child

INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (5, 'Sophia', 'Brown', 2, '1979-10-15'); -- Domestic partner
INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (5, 'Sophie', 'Brown', 3, '2008-11-21'); -- Child
INSERT INTO table_dependents (employee_id, first_name, last_name, dependent_type, date_of_birth) VALUES (5, 'Sam', 'Brown', 3, '2011-12-10'); -- Child
