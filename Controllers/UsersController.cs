using FormCRUDB_MAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace FormCRUDB_MAB.Controllers
{
    public class UsersController : Controller
    {

        private IConfiguration configuration;

        public UsersController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Users()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetAllUsers";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult AddUsersHelper()
        {
            return View();
        }

        public IActionResult UsersDelete(int UserID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.DeleteUser";
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            command.ExecuteNonQuery();
            return RedirectToAction("Users");
        }


        public IActionResult UsersSave(int UserID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region UserByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectByPK";
            command.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            UsersModel usersModel = new UsersModel();

            foreach (DataRow dataRow in table.Rows)
            {
                usersModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                usersModel.UserName = @dataRow["UserName"].ToString();
                usersModel.UserEmail = @dataRow["Email"].ToString();
                usersModel.Password = @dataRow["Password"].ToString();
                usersModel.MobileNo = @dataRow["MobileNo"].ToString();
                usersModel.Address = @dataRow["Address"].ToString();
                usersModel.IsActive = @dataRow["IsActive"].ToString();
            }


            #endregion

            return View("UsersSave", usersModel);
        }

        public IActionResult SaveUsers(UsersModel usersModel)
        {
            if (usersModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User ID is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            if (usersModel.UserID == 0)
                            {
                                command.CommandText = "PR_NewUser_Insert";
                            }
                            else
                            {
                                command.CommandText = "PR_User_Update";
                                command.Parameters.Add("@UserID", SqlDbType.Int).Value = usersModel.UserID;
                            }
                            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = usersModel.UserName;
                            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = usersModel.UserEmail;
                            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = usersModel.Password;
                            command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = usersModel.MobileNo;
                            command.Parameters.Add("@Address", SqlDbType.VarChar).Value = usersModel.Address;
                            command.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = usersModel.IsActive;
                            command.ExecuteNonQuery();
                        }
                    }
                    // Log successful operation
                    // e.g., _logger.LogInformation("User saved successfully.");
                    return RedirectToAction("Users");
                }
                catch (Exception ex)
                {
                    // Log exception
                    // e.g., _logger.LogError(ex, "Error saving user.");
                    ModelState.AddModelError("", "An error occurred while saving the user.");
                }
            }
            return View("UsersSave", usersModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        #region Login
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Users");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            return RedirectToAction("Login", "Users");
        }
        #endregion

        public IActionResult UserRegister(UsersModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Register";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.UserEmail;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                    sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
                    sqlCommand.ExecuteNonQuery();
                    return RedirectToAction("Login", "Users");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
