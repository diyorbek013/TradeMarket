using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    public class Customer : BaseEntity
    {
        public int DiscountValue { get; set; }
        [ForeignKey("PersonId")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
