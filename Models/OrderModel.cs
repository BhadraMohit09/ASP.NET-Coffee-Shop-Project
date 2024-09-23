using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class OrderModel
    {
        [Required]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public int UserID { get; set; }
    }
}
