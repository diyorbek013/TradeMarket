using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    [NotMapped]
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
