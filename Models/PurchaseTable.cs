using System.ComponentModel.DataAnnotations;

namespace MVCFlowerShop.Models
{
    public class PurchaseTable
    {
        [Key]
        public string PurchaseId { get; set; }
        public string CustomerName { get; set; }
        public double PurchaseAmount { get; set; }
        public int ItemQuantity { get; set; }
        public DateTime PurchaseDateTime { get; set; }
    }
}
