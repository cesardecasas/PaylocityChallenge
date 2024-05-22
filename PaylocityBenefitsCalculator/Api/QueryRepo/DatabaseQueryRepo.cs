using System;
using System.Data;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Npgsql;
using NpgsqlTypes;

namespace Api.QueryRepo
{
    public class DatabaseQueryRepo
    {
        private readonly string _connectionString;

        public DatabaseQueryRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var employees = new List<Employee>();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from table_employees";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var employee = new Employee()
                                {
                                    Id = (int)reader["employee_id"],
                                    FirstName = (string?)DBToNull(reader["first_name"]),
                                    LastName = (string?)DBToNull(reader["last_name"]),
                                    DateOfBirth = (DateTime)reader["date_of_birth"],
                                    Salary = (decimal)reader["salary"],
                                };

                                employees.Add(employee);   
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                 
                    }
    
                }
            }
                    return employees;
        }


        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = new Employee();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from table_employees where employee_id=:id";
                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                                employee = new Employee()
                                {
                                    Id = (int)reader["employee_id"],
                                    FirstName = (string?)DBToNull(reader["first_name"]),
                                    LastName = (string?)DBToNull(reader["last_name"]),
                                    DateOfBirth = (DateTime)reader["date_of_birth"],
                                    Salary = (decimal)reader["salary"],
                                };
                        }
                    }

                }
            }
            return employee;
        }


        public async Task<List<Dependent>> GetAllDependents()
        {
            var dependents = new List<Dependent>();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from table_dependents";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var dependent = new Dependent()
                                {
                                    Id = (int)reader["dependent_id"],
                                    Relationship = (Relationship)(int)reader["dependent_type"],
                                    FirstName = (string?)DBToNull(reader["first_name"]),
                                    LastName = (string?)DBToNull(reader["last_name"]),
                                    DateOfBirth = (DateTime)reader["date_of_birth"],
                                    EmployeeId = (int)reader["employee_id"]
                                };

                                dependents.Add(dependent);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }

                    }

                }
            }
            return dependents;
        }


        public async Task<Dependent> GetDependentById(int id)
        {
            var dependent = new Dependent();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from table_dependents where dependent_id=:id";
                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            dependent = new Dependent()
                            {
                                Id = (int)reader["dependent_id"],
                                Relationship = (Relationship)(int)reader["dependent_type"],
                                FirstName = (string?)DBToNull(reader["first_name"]),
                                LastName = (string?)DBToNull(reader["last_name"]),
                                DateOfBirth = (DateTime)reader["date_of_birth"],
                                EmployeeId = (int)reader["employee_id"]
                            };
                        }
                    }

                }
            }
            return dependent;
        }

        public async Task<List<Dependent>> GetDependentsByEmployeeId(int id)
        {
            var dependents = new List<Dependent>();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from table_dependents where employee_id=:id";
                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var dependent = new Dependent()
                                {
                                    Id = (int)reader["dependent_id"],
                                    Relationship = (Relationship)(int)reader["dependent_type"],
                                    FirstName = (string?)DBToNull(reader["first_name"]),
                                    LastName = (string?)DBToNull(reader["last_name"]),
                                    DateOfBirth = (DateTime)reader["date_of_birth"],
                                    EmployeeId = (int)reader["employee_id"]
                                };

                                dependents.Add(dependent);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }

                    }

                }
            }
            return dependents;
        }

        public async Task<int> CreateDependentAsync(Dependent dependent)
        {
            int id;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO table_dependents (employee_id, first_name,last_name, dependent_type, date_of_birth) VALUES (:employeeId, :firstName, :lastName, :relationship, :dateOfBirth) returning dependent_id";
                    cmd.Parameters.AddWithValue("employeeId", NpgsqlDbType.Integer, dependent.EmployeeId);
                    cmd.Parameters.AddWithValue("firstName", NpgsqlDbType.Text, NullableToDb(dependent?.FirstName));
                    cmd.Parameters.AddWithValue("lastName", NpgsqlDbType.Text, NullableToDb(dependent?.LastName));
                    cmd.Parameters.AddWithValue("relationship", NpgsqlDbType.Integer, (int)dependent.Relationship);
                    cmd.Parameters.AddWithValue("dateOfBirth", NpgsqlDbType.Date, dependent.DateOfBirth);


                    id = (int)await cmd.ExecuteScalarAsync();
                }
            }
            return id;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            int id;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd =  connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO table_employees (first_name, last_name, date_of_birth, salary) VALUES (:firstName, :lastName,:dateOfBirth, :salary) returning employee_id";

                    cmd.Parameters.AddWithValue("firstName", NpgsqlDbType.Text, NullableToDb(employee?.FirstName));
                    cmd.Parameters.AddWithValue("lastName", NpgsqlDbType.Text, NullableToDb(employee?.LastName));
                    cmd.Parameters.AddWithValue("dateOfBirth", NpgsqlDbType.Date, employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("salary", NpgsqlDbType.Numeric, employee.Salary);


                    id = (int)await cmd.ExecuteScalarAsync();
                }
            }
            return id;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
      
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM table_employees where employee_id = :id";

                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        
        }

        public async Task DeleteDependentsAsync(int id)
        {

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM table_dependents where employee_id = :id";

                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }

        private static object DBToNull(object obj)
        {
            return obj is DBNull ? null : obj;
        }


        private static object NullableToDb(object obj)
        {
            return obj ?? DBNull.Value;
        }

    }
}