using Autofac;
using DAL.Repository.Interfaces;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Services.Interfaces;

namespace BAL.Services
{
    public class AutofacServicesConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем споставление типов
            builder.RegisterType<ComentService>().As<IComentService>();
            builder.RegisterType<GameService>().As<IGameService>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();
        }
    }
}
