
CREATE TABLE table_employees (
    employee_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATE NOT NULL,
    salary NUMERIC(10, 2) NOT NULL
);


CREATE TABLE table_dependents (
    dependent_id SERIAL PRIMARY KEY,
    employee_id INT NOT NULL REFERENCES table_employees(employee_id),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    dependent_type INT NOT NULL CHECK (dependent_type IN (1,2,3)),
    date_of_birth DATE NOT NULL
);

CREATE UNIQUE INDEX unique_spouse_or_partner_per_employee
ON table_dependents (employee_id)
WHERE dependent_type IN (1,2);