using System;
using HomeTownPickEm.Abstract.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HomeTownPickEm.Services.DataSeed
{
    public class SeederFactory
    {
        private readonly Type _instanceType;

        public SeederFactory(Type instanceType)
        {
            _instanceType = instanceType;
        }

        public ISeeder Create(IServiceProvider provider)
        {
            return (ISeeder)ActivatorUtilities.CreateInstance(provider, _instanceType);
        }
    }
}