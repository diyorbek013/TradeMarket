using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    public class Receipt : BaseEntity
    {
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; } 
    }
}
