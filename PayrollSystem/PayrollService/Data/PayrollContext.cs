using MongoDB.Driver;
using PayrollService.Model;

namespace PayrollService.Data
{
    public class PayrollContext
    {
        private readonly IMongoDatabase _database;

        public PayrollContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("PayrollDb");
        }

        public IMongoCollection<Payroll> Payrolls => _database.GetCollection<Payroll>("Payrolls");
    }
}
