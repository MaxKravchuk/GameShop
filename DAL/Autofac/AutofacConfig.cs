using Autofac;
using DAL.Repository;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Autofac
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем споставление типов
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();
        }
    }
}
