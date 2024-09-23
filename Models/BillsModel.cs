using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class BillsModel
    {
        [Required]
        public int BillID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public int UserId { get; set; }
    }
}
