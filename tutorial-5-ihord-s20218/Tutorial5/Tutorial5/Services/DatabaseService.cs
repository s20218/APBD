using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tutorial5.Models;

namespace Tutorial5.Services
{
   
    public class DatabaseService : IDatabaseService
    {
        private IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddWarehouseAsync(Purchase newPurchase, int idOrder)
        {
            
            decimal price;            

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;

                await con.OpenAsync();

                com.Parameters.AddWithValue("@idWarehouse", newPurchase.IdWarehouse);
                com.Parameters.AddWithValue("@idProduct", newPurchase.IdProduct);
                com.Parameters.AddWithValue("@createdAt", newPurchase.CreatedAt);
                com.Parameters.AddWithValue("@amount", newPurchase.Amount);

                com.Parameters.AddWithValue("@idOrder", idOrder);
                
                price = await GetPrice(newPurchase.IdProduct);

                com.Parameters.AddWithValue("@price", price);

                //updating order table
                await UpdateOrder(idOrder, newPurchase.CreatedAt);
                
                com.CommandText = "INSERT INTO Product_Warehouse(IdWarehouse," +
                                  "IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                                  "VALUES(@idWarehouse, @idProduct, @idOrder, @amount, @amount * @price, @createdAt)";

                com.ExecuteNonQuery();

                com.CommandText = "SELECT TOP 1 IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC";

                using (SqlDataReader dr = await com.ExecuteReaderAsync())
                {
                    await dr.ReadAsync();
                    return dr.GetInt32(0);

                }

            }
        }

        public async Task<decimal> GetPrice(int idProduct)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.Parameters.AddWithValue("@idProduct", idProduct);

                com.CommandText = "SELECT Price FROM Product " +
                                  "WHERE IdProduct = @idProduct";

                await con.OpenAsync();

                using (SqlDataReader dr = await com.ExecuteReaderAsync())
                {
                    await dr.ReadAsync();
                    return (decimal)dr["Price"];

                }
            }
        }

        public async Task<bool> ProductAndWarehouseExist(int idProduct, int idWarehouse)
        {

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;

                //check if product and warehouse exist
                com.CommandText = "SELECT p.IdProduct, w.IdWarehouse FROM Product p, Warehouse w " +
                                  "WHERE p.IdProduct = @idProduct AND w.IdWarehouse = @idWarehouse";

                com.Parameters.AddWithValue("@idWarehouse", idWarehouse);
                com.Parameters.AddWithValue("@idProduct", idProduct);

                await con.OpenAsync();

                using (SqlDataReader dr = await com.ExecuteReaderAsync())
                {
                    if (!dr.HasRows)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public async Task<int> GetOrderId(int idProduct, DateTime createdAt, int amount)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;
                await con.OpenAsync();

                com.Parameters.AddWithValue("@idProduct", idProduct);
                com.Parameters.AddWithValue("@createdAt", createdAt);
                com.Parameters.AddWithValue("@amount", amount);

                //check if requred order exist
                com.CommandText = "SELECT IdOrder FROM \"Order\"" +
                                  "WHERE IdProduct = @idProduct and Amount = @amount AND CreatedAt < @createdAt";

                using (SqlDataReader dr = await com.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        await dr.ReadAsync();
                        return dr.GetInt32(0);
                    }
                }

            }return -1;
        }

        public async Task<bool> ProductPurchaseExist(int idOrder)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.Parameters.AddWithValue("@idOrder", idOrder);
                await con.OpenAsync();

                //check if there are any records with the given idOrder
                com.CommandText = "SELECT IdOrder FROM Product_Warehouse WHERE IdOrder = @idOrder";


                using (SqlDataReader dr = await com.ExecuteReaderAsync())
                {

                    if (!dr.HasRows)
                    {
                        return false;
                    }
                }
            }return true;
        }

        public async Task UpdateOrder(int idOrder, DateTime createdAt)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using SqlCommand com = new SqlCommand();
                com.Connection = con;

                await con.OpenAsync();

                com.Parameters.AddWithValue("@idOrder", idOrder);

                com.Parameters.AddWithValue("@createdAt", createdAt);

                com.CommandText = "UPDATE \"Order\"" +
                                  "SET FulfilledAt = @CreatedAt " +
                                  "WHERE IdOrder = @IdOrder";

                com.ExecuteNonQuery();
            }
        }

        public async Task ExecuteProcudere(Purchase newPurchase)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                using var com = new SqlCommand("AddProductToWarehouse", con);

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@IdProduct", newPurchase.IdProduct);
                com.Parameters.AddWithValue("@IdWarehouse", newPurchase.IdWarehouse);
                com.Parameters.AddWithValue("@Amount", newPurchase.Amount);
                com.Parameters.AddWithValue("@CreatedAt", newPurchase.CreatedAt);

                await con.OpenAsync();

                com.ExecuteNonQuery();
            }
        }
    }
}
