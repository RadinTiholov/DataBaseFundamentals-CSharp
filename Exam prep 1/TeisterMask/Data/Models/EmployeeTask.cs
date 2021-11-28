﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [ForeignKey(nameof(Employee))]
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey(nameof(Task))]
        [Required]
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}