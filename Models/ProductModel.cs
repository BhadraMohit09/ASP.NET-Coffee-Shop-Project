using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class ProductModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Name cannot be empty...")]

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price cannot be empty...")]

        public decimal ProductPrice { get; set; }
        [Required]

        public string ProductCode { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public int UserID { get; set; }
    }


    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
