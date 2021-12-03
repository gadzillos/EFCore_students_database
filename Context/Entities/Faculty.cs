using Homework_6.Context.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Homework_6.Context.Entities
{
    public class Faculty : BaseEntity
    {
        [MinLength(1), MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
