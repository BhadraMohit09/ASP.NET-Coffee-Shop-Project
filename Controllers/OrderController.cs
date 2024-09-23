using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        private IConfiguration configuration;

        public OrderController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Order()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetAllOrders";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }


        public IActionResult AddOrderHelper() 
        {
            return View();      
        }

        public IActionResult OrderDelete(int OrderID)
        {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.DeleteOrder";
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                command.ExecuteNonQuery();
                return RedirectToAction("Order");
        }

        public IActionResult OrderSave(int OrderID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region OrderByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectByPK";
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderModel orderModel = new OrderModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@dataRow["OrderDate"]);
                orderModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                orderModel.PaymentMode = @dataRow["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderModel.ShippingAddress = @dataRow["ShippingAddress"].ToString();
                orderModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("OrderSave", orderModel);
        }

        public IActionResult SaveOrder(OrderModel orderModel)
        {
            if (orderModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == 0)
                {
                    command.CommandText = "PR_Order_Insert";
                }
                else
                {
                    command.CommandText = "PR_Order_Update";
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;
                }
                command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = orderModel.CustomerID;
                command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
                command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Order");
            }
            return View("OrderSave", orderModel);
        }


    }
}
