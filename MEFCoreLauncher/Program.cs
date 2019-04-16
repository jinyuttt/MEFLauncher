using MEFLib;
using MEFLoader;
using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;


namespace MEFCoreLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            CatalogLoader loader = new CatalogLoader();
            loader.DefaultLoader<IPlugin>();
           var obj= loader.GetObj<IPlugin>();
            obj.Start();
            Console.ReadKey();
        }

        //private void Loader()
        //{
        //    var assembiles = Directory.GetFiles(AppContext.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
        //   .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath);
        //    var conventions = new ConventionBuilder();
        //    conventions.ForTypesDerivedFrom<IMainView>()
        //        .Export<IMainView>().Shared();

        //    var configuration = new ContainerConfiguration()
        //        .WithAssemblies(assembiles, conventions);

        //    using (var container = configuration.CreateContainer())
        //    {
        //        IEnumerable<IMainView> senders = container.GetExports<IMainView>();
        //    }

        //}

    }
}
