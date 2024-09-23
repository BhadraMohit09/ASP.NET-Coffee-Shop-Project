using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    [CheckAccess]
    public class BillsController : Controller
    {

        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Bills()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetAllBills";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        // Loads the bill data by ID
        public IActionResult BillsSave(int BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region BillByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bill_SelectByPK";
            command.Parameters.AddWithValue("@BillID", BillID);

            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillsModel billsModel = new BillsModel();

            if (table.Rows.Count > 0) // Ensure there is at least one row to avoid index errors
            {
                DataRow dataRow = table.Rows[0]; // Just use the first row
                billsModel.BillID = Convert.ToInt32(dataRow["BillID"]);
                billsModel.BillDate = Convert.ToDateTime(dataRow["BillDate"]);
                billsModel.BillNumber = dataRow["BillNumber"].ToString();
                billsModel.OrderId = Convert.ToInt32(dataRow["OrderId"]);
                billsModel.TotalAmount = Convert.ToDecimal(dataRow["TotalAmount"]);
                billsModel.Discount = dataRow["Discount"] != DBNull.Value ? Convert.ToDecimal(dataRow["Discount"]) : 0; // Handle potential null values
                billsModel.NetAmount = Convert.ToDecimal(dataRow["NetAmount"]);
                billsModel.UserId = Convert.ToInt32(dataRow["UserId"]);
            }

            connection.Close();
            #endregion

            return View("BillsSave", billsModel);
        }

        // Handles save functionality for bill insertion or updating
        public IActionResult SaveBills(BillsModel billModel)
        {
            if (billModel.UserId <= 0) // Correct check for UserID validity
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (billModel.BillID == 0)
                    {
                        command.CommandText = "PR_Bill_Insert";
                    }
                    else
                    {
                        command.CommandText = "PR_Bill_Update";
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value = billModel.BillID;
                    }

                    // Adding parameters to the command
                    command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billModel.BillNumber;
                    command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billModel.BillDate;
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billModel.OrderId;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billModel.TotalAmount;
                    command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billModel.Discount;
                    command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billModel.NetAmount;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = billModel.UserId;

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return RedirectToAction("Bills");
            }

            return View("BillsSave", billModel);
        }


        public IActionResult AddBillsHelper()
        {
            return View();
        }
        public IActionResult BillDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.DeleteBill";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                command.ExecuteNonQuery();
                return RedirectToAction("Bills");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Bills");
            }
        }

    }
}
