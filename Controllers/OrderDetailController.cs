using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    [CheckAccess]
    public class OrderDetailController : Controller
    {
        private IConfiguration configuration;

        public OrderDetailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult OrderDetail()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetAllOrderDetails";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult AddOrderDetail() 
        {
            return View();
        }

        public IActionResult AddOrderDetailHelper()
        {
            return View();
        }

        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
           
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.DeleteOrderDetail";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderDetail");   
        }

        public IActionResult OrderDetailSave(int OrderDetailID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region OrderDetailByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectByPK";
            command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderDetailModel orderdetailModel = new OrderDetailModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderdetailModel.OrderDetailID = Convert.ToInt32(@dataRow["OrderDetailID"]);
                orderdetailModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderdetailModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                orderdetailModel.Quantity = Convert.ToInt32(@dataRow["Quantity"]);
                orderdetailModel.Amount = Convert.ToDecimal(@dataRow["Amount"]);
                orderdetailModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderdetailModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("OrderDetailSave", orderdetailModel);
        }

        public IActionResult SaveOrderDetail(OrderDetailModel orderdetailModel)
        {
            if (orderdetailModel.UserID <= 0)
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
                if (orderdetailModel.OrderDetailID == 0)
                {
                    command.CommandText = "PR_OrderDetail_Insert";
                }
                else
                {
                    command.CommandText = "PR_OrderDetail_Update";
                    command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderdetailModel.OrderDetailID;
                }
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderdetailModel.OrderID;
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderdetailModel.ProductID;
                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderdetailModel.Quantity;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderdetailModel.Amount;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderdetailModel.TotalAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderdetailModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderDetail");
            }
            return View("OrderDetailSave", orderdetailModel);
        }

    }
}
