using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Unity;

namespace GameShop.WebApi.App_Start
{
    public class UnityValidatorFactory : ValidatorFactoryBase
    {
        private readonly IUnityContainer _container;

        public UnityValidatorFactory(IUnityContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _container.Resolve(validatorType) as IValidator;
        }
    }
}
