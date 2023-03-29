using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataSeeder : DropCreateDatabaseIfModelChanges<GameShopContext>
    {
        protected override void Seed(GameShopContext context)
        {
            base.Seed(context);
        }
    }
}
