using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class CustomerModel
    {
        [Required]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]     
        public string GSTNO { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "This field cannot be empty...")]
        public int UserID { get; set; }
    }

    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
