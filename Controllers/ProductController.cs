using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        private IConfiguration configuration;

        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region ProductList
        public IActionResult Product()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.SelectProduct";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        public IActionResult AddProductHelper() 
        {
            return View();
        }

        public IActionResult ProductDelete(int ProductID) 
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.DeleteProduct";
            command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
            command.ExecuteNonQuery();
            return RedirectToAction("Product");
        }

        public IActionResult ProductSave(int ProductID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region Dropdown
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader1);
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel
                {
                    UserID = Convert.ToInt32(dataRow["UserID"]),
                    UserName = dataRow["UserName"].ToString()
                };
                userList.Add(userDropDownModel);
            }
            ViewBag.userList = userList;
            connection1.Close();
            #endregion



            #region ProductByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectByPK";
            command.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            ProductModel productModel = new ProductModel();

            foreach (DataRow dataRow in table.Rows)
            {
                productModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                productModel.ProductName = @dataRow["ProductName"].ToString();
                productModel.ProductCode = @dataRow["ProductCode"].ToString();
                productModel.ProductPrice = Convert.ToDecimal(@dataRow["ProductPrice"]);
                productModel.Description = @dataRow["Description"].ToString();
                productModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("ProductSave", productModel);
        }

        public IActionResult SaveProduct(ProductModel productModel)
        {
            if (productModel.UserID <= 0)
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
                if (productModel.ProductID == 0)
                {
                    command.CommandText = "PR_Product_Insert";
                }
                else
                {
                    command.CommandText = "PR_Product_Update";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                }
                command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Product");
            }
            return View("ProductSave",productModel);
        }

    }
}
