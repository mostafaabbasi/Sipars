using System;
using System.Diagnostics;
using NUnit.Framework;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.IocConfig;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            IServiceProperties _ServiceProperties = SmObjectFactory.Container.GetInstance<IServiceProperties>();
            IUnitOfWork  _uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
            _ServiceProperties.AddServiceProperties(new ServiceProperties
            {
                Id = -123,
                payMin = 10
            });

            Trace.WriteLine(_uow.SaveAllChanges());
        }
    }
}