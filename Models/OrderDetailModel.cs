using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class OrderDetailModel
    {
        [Required]

        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]

        public int OrderID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]

        public int ProductID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]

        public int Quantity { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]

        public decimal Amount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]


        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]

        public int UserID { get; set; }

    }
}
