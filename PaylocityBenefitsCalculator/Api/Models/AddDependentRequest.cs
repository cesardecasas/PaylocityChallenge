﻿namespace Api.Models
{
    public class AddDependentRequest
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Relationship Relationship { get; set; }
        public int EmployeeId { get; set; }
    }
}
