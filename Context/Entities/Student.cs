using Homework_6.Context.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Homework_6.Context.Entities
{
    // Student has only one group and previous groups history ignored (may be stored in separated entity or database)

    public class Student : BaseEntity
    {
        [MinLength(1), MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(1), MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int GroupRefId { get; set; }

        [ForeignKey(nameof(GroupRefId))]
        public Group Group { get; set; }
    }
}
