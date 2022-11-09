using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Tutorial4_webApp.Models;

namespace Tutorial4_webApp.Services
{
    public class SqlServerDatabaseService : IDatabaseService
    {
        private IConfiguration _configuration;

        public SqlServerDatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Animal> GetAnimals(string orderBy)
        {
            var listOfAnimals = new List<Animal>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                var sqlQuery = "";

                //check the orderBy parameter

                if (orderBy.Equals("name"))
                {
                    sqlQuery = "SELECT * FROM Animal ORDER BY Name";
                }
                else if (orderBy.Equals("category"))
                {
                    sqlQuery = "SELECT * FROM Animal ORDER BY Category";
                }
                else if (orderBy.Equals("description"))
                {
                    sqlQuery = "SELECT * FROM Animal ORDER BY Description";
                }
                else if (orderBy.Equals("area"))
                {
                    sqlQuery = "SELECT * FROM Animal ORDER BY Area";
                }

                com.CommandText = sqlQuery;    

                try
                {
                    con.Open();
               
                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        listOfAnimals.Add(new Animal
                        {
                            IdAnimal = (int)dr["IdAnimal"],
                            Name = dr["Name"].ToString(),
                            Description = dr["Description"].ToString(),
                            Category = dr["Category"].ToString(),
                            Area = dr["Area"].ToString()
                        });
                    }
                }
                catch (SqlException)
                {
                    return null;
                }
                if (listOfAnimals.Count == 0)
                    return null;
            }

            return listOfAnimals;
        }

        public int AddAnimal(Animal newAnimal)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                var sqlQuery = "";                

                if (newAnimal.Description is not null)
                {
                    com.Parameters.AddWithValue("@animal_description", newAnimal.Description);
                    sqlQuery = "INSERT INTO ANIMAL (Name, Description, Category, Area) VALUES (@animal_name, @animal_description, @animal_category, @animal_area)";
                }
                else
                    sqlQuery = "INSERT INTO ANIMAL (Name, Category, Area) VALUES (@animal_name, @animal_category, @animal_area)";

                com.Parameters.AddWithValue("@animal_name", newAnimal.Name);
                com.Parameters.AddWithValue("@animal_category", newAnimal.Category);
                com.Parameters.AddWithValue("@animal_area", newAnimal.Area);

                com.CommandText = sqlQuery;

                try
                {
                    con.Open();
                
                    com.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    return 500;
                }

                return 201;
            }
        }

        public int ModifyAnimal(int idAnimal, Animal updAnimal)
        {
            List<Animal> listOfAnimals = GetAnimals("name").ToList();

            if (!listOfAnimals.Any(a => a.IdAnimal == idAnimal)) //check if animal exist in the database
                return 400;

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {                
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                var sqlQuery = "";                

                if (updAnimal.Description is not null)
                {
                    com.Parameters.AddWithValue("@animal_description", updAnimal.Description);
                    sqlQuery = "UPDATE ANIMAL SET Name=@animal_name, Description=@animal_description, Category=@animal_category, Area=@animal_area WHERE IdAnimal=@animal_id";
                }
                else
                    sqlQuery = "UPDATE ANIMAL SET Name=@animal_name, Description=null, Category=@animal_category, Area=@animal_area WHERE IdAnimal=@animal_id";

                com.Parameters.AddWithValue("@animal_id", idAnimal);

                com.Parameters.AddWithValue("@animal_name", updAnimal.Name);
                com.Parameters.AddWithValue("@animal_category", updAnimal.Category);
                com.Parameters.AddWithValue("@animal_area", updAnimal.Area);

                com.CommandText = sqlQuery;

                try
                {
                    con.Open();

                    com.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    return 500;
                }

                return 201;
            }
        }

        public int DeleteAnimal(int idAnimal)
        {
            List<Animal> listOfAnimals = GetAnimals("name").ToList();

            if (!listOfAnimals.Any(a => a.IdAnimal == idAnimal))
                return 400;

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                com.CommandText = "DELETE FROM ANIMAL WHERE IdAnimal=@id_animal";

                com.Parameters.AddWithValue("@id_animal", idAnimal);

                
                try
                {
                    con.Open();

                    com.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    return 500;
                }

                return 204;
            }
        }

    }
}
