using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getAllMenus")]
        public string GetAllMenus()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MenuConnection").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Menu", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Menu> list = new List<Menu>();
            Response response = new Response();
            if (dt.Rows.Count > 0 )
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Menu menu = new Menu();
                    menu.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    menu.name = Convert.ToString(dt.Rows[i]["name"]);
                    if (!Convert.IsDBNull(dt.Rows[i]["parent"]))
                    {
                        menu.parent = Convert.ToInt32(dt.Rows[i]["parent"]);
                    }
                    list.Add(menu);
                }
            }
            if (list.Count > 0 )
            {
                return JsonConvert.SerializeObject(list);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = string.Empty;
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("createMenu")]
        public void CreateMenu(Menu menu) 
        {

            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MenuConnection").ToString());
            conn.Open();
            string insertCommand = "insert into Menu (name, parent) values (@name, @parent)";
            SqlCommand cmd = new SqlCommand(insertCommand, conn);
            cmd.Parameters.AddWithValue("@name", menu.name);
            cmd.Parameters.AddWithValue("@parent", menu.parent);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        [HttpDelete]
        [Route("deleteMenu")]
        public void DeleteMenu(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MenuConnection").ToString());
            conn.Open();
            string insertCommand = "delete Menu where id=@id";
            SqlCommand cmd = new SqlCommand(insertCommand, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        [HttpPut]
        [Route("updateMenu")]
        public void UpdateMenu(Menu menu)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MenuConnection").ToString());
            conn.Open();
            string insertCommand = "update Menu set name=@name where id=@id";
            SqlCommand cmd = new SqlCommand(insertCommand, conn);
            cmd.Parameters.AddWithValue("@name", menu.name);
            cmd.Parameters.AddWithValue("@id", menu.id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
