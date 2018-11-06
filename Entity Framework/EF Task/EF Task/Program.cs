using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;

namespace EF_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            using (var context = new AppDbContext())
            {
                await context.Customers.CountAsync(); //Do not delete this line. It triggers configuration check for database

                await Process(context);
            }

            Console.WriteLine("Main Async Completed");
        }
      
        static async Task Process(AppDbContext context)
        {
            var result = context.Customers
                .Include(c => c.COrders)
                .Include(c => c.COrders.Select(o => o.OOrderItems));
        }
    }
}
