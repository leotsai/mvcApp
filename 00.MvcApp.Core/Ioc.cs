using System;
using System.Linq;
using System.Reflection;
using MvcApp.Core.Extensions;
using Unity;

namespace MvcApp
{
    public class Ioc
    {
        private static readonly UnityContainer _container;

        static Ioc()
        {
            _container = new UnityContainer();
        }

        public static void RegisterInheritedTypes(Assembly assembly, Type baseType)
        {
            var allTypes = assembly.GetTypes();
            var baseInterfaces = baseType.GetInterfaces();
            foreach (var type in allTypes)
            {
                if (type.BaseType != null && type.BaseType.Eq(baseType))
                {
                    var typeInterfaces = type.GetInterfaces().Where(x => !baseInterfaces.Any(bi => bi.Eq(x)));
                    foreach (var typeInterface in typeInterfaces)
                    {
                        _container.RegisterType(typeInterface, type);
                    }
                }
            }
        }

        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>();
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }
        
    }
}
