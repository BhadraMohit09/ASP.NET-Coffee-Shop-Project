using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    [CheckAccess]
    public class CustomerController : Controller
    {
        private IConfiguration configuration;

        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Customer()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetAllCustomers";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult AddCustomerHelper() 
        {
            return View();
        }

        public IActionResult CustomerDelete(int CustomerID)
        {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.DeleteCustomer";
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                command.ExecuteNonQuery();
                return RedirectToAction("Customer");            
        }

        public IActionResult CustomerSave(int CustomerID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region CustomerByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectByPK";
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CustomerModel customerModel = new CustomerModel();

            foreach (DataRow dataRow in table.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                customerModel.CustomerName = @dataRow["CustomerName"].ToString();
                customerModel.Address = @dataRow["HomeAddress"].ToString();
                customerModel.Email = @dataRow["Email"].ToString();
                customerModel.MobileNo = @dataRow["MobileNo"].ToString();
                customerModel.GSTNO = @dataRow["GST_NO"].ToString();
                customerModel.CityName = @dataRow["CityName"].ToString();
                customerModel.PinCode = @dataRow["PinCode"].ToString();
                customerModel.NetAmount = Convert.ToDecimal(@dataRow["NetAmount"]);
                customerModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("CustomerSave", customerModel);
        }

        public IActionResult SaveCustomer(CustomerModel customerModel)
        {
            if (customerModel.UserID <= 0)
            {
                ModelState.AddModelError("CustomerID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID == 0)
                {
                    command.CommandText = "PR_Customer_Insert";
                }
                else
                {
                    command.CommandText = "PR_Customer_Update";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerModel.CustomerID;
                }
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
                command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.Address;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo;
                command.Parameters.Add("@GST_NO", SqlDbType.VarChar).Value = customerModel.GSTNO;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = customerModel.CityName;
                command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = customerModel.PinCode;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Customer");
            }
            return View("CustomerSave", customerModel);
        }

    }
}
