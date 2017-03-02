using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            IDatabase redisdb = Redis.redis.GetDatabase();
            Console.WriteLine(redisdb.KeyExists("es")); 
            //redisdb.sor
        }
    }



    public static class Redis
    {

        private static string constr = ConfigurationManager.ConnectionStrings["aaa"].ConnectionString;

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(constr);
        });

        public static ConnectionMultiplexer redis
        {
            get
            {
                return lazyConnection.Value;
            }
        }

    }
}
