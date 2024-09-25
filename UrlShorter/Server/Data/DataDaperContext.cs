using Microsoft.Data.SqlClient;
using System.Data;

namespace Server.Data
{
    public class DataDaperContext
    {
        private readonly IConfiguration _configuration;
        public DataDaperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection DatabaseConnect()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
