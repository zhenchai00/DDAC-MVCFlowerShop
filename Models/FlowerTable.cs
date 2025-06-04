using System.ComponentModel.DataAnnotations;

namespace MVCFlowerShop.Models
{
    public class FlowerTable
    {
        // define this column as primary key
        [Key]
        public int FlowerId { get; set; }
        public string FlowerName { get; set; }  // the value key in this column must be not null
        public string? FlowerType { get; set; } // optional value in column
        public DateTime FlowerProducedDate { get; set; }
        public double FlowerPrice { get; set; }
    }
}
