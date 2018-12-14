namespace MEF.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using Caliburn.Micro;
    using MEF.Launcher.ViewModels;
    using ILog = log4net.ILog;
    using LogManager = log4net.LogManager;

    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Bootstrapper));

        /// <summary>
        /// The container
        /// </summary>
        private CompositionContainer container;

        /// <summary>
        /// The service locator adapter
        /// </summary>
        private MefServiceLocatorAdapter serviceLocatorAdapter = new MefServiceLocatorAdapter();

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            Logger.Debug("[Configure] Scanning dll...");
            var aggassemblyCatalog = new AggregateCatalog();
            var assemblies = FilterAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var ass in assemblies)
            {
                var assemblyCatalog = new AssemblyCatalog(ass);
                aggassemblyCatalog.Catalogs.Add(assemblyCatalog);
                Logger.Debug($"[Configure] Loaded dll: {ass.Location}");
            }

            this.container = new CompositionContainer(aggassemblyCatalog);
            this.container.ComposeExportedValue<IWindowManager>(new WindowManager());
            this.container.ComposeExportedValue<IEventAggregator>(new EventAggregator());

            var batch = new CompositionBatch();
            batch.AddExportedValue(this.container);
            this.container.Compose(batch);
            Logger.Debug("[Configure] Scanning dll... Done");
        }

        /// <summary>
        /// Filters the assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        private IList<Assembly> FilterAssemblies(Assembly[] assemblies)
        {
            var result = new List<Assembly>();
            foreach (var assembly in assemblies)
            {
                using (var assemblyCatalog = new AssemblyCatalog(assembly))
                {
                    if (assemblyCatalog.Parts.Any())
                    {
                        result.Add(assembly);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <returns></returns>
        private static IList<Assembly> GetAssemblies()
        {
            var result = new List<Assembly>();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.TopDirectoryOnly).ToList();

            // Load current execuable file
            files.Add(Assembly.GetExecutingAssembly().Location);

            // Load external dll
            foreach (var file in files)
            {
                var ass = Assembly.LoadFile(file);
                using (var assemblyCatalog = new AssemblyCatalog(ass))
                {
                    if (assemblyCatalog.Parts.Any())
                    {
                        result.Add(ass);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        /// <exception cref="System.Exception">Could not locate any instances</exception>
        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = this.container.GetExportedValues<object>(contract);

            var enumerable = exports as object[] ?? exports.ToArray();
            if (enumerable.Any())
            {
                return enumerable.First();
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<MainViewModel>();
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>
        /// A list of assemblies to inspect.
        /// </returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return GetAssemblies();
        }
    }
}
