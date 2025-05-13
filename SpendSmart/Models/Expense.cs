using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal value { get; set; }

        [Required]
        public string? description { get; set; }



    }
}
