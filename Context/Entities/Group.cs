using Homework_6.Context.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Homework_6.Context.Entities
{
    public class Group : BaseEntity
    {
        [MinLength(1), MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MinLength(1), MaxLength(15)]
        [Required]
        public string Number { get; set; }

        public int FacultyRefId { get; set; }

        [ForeignKey(nameof(FacultyRefId))]
        public Faculty Faculty { get; set; }
    }
}
