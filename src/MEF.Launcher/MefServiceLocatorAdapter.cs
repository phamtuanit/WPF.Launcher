namespace MEF.Launcher
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using CommonServiceLocator;
    using MEF.Launcher.Contract.IoC;

    public class MefServiceLocatorAdapter : ServiceLocatorImplBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MefServiceLocatorAdapter"/> class.
        /// </summary>
        public MefServiceLocatorAdapter()
        {
            ServiceLocator.SetLocatorProvider(() => this);

            // Config SimpleIoC
            SimpleIoC.GetAllInstances = this.GetAllInstances;
            SimpleIoC.GetInstance = this.DoGetInstance;
        }

        /// <summary>
        /// Resolves the instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return IoC.GetAllInstances(serviceType);
        }

        /// <summary>
        /// Resolves all the instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param><param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return IoC.GetInstance(serviceType, key);
        }
    }
}
