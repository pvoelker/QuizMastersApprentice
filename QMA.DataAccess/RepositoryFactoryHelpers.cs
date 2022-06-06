using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMA.DataAccess
{
    public class DatabaseType
    {
        public DatabaseType(string name, Guid id, Type type)
        {
            Name = name;
            Id = id;
            Type = type;
        }

        public string Name { get; private set; }
        public Guid Id { get; private set; }
        public Type Type { get; private set; }
    }

    static public class DatabaseTypeHelpers
    {
        static public List<DatabaseType> GetDatabaseTypes()
        {
            var retVal = new List<DatabaseType>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var allTypes = assembly.GetTypes();//System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                foreach (Type type in allTypes.Where(x => x.GetInterfaces().Contains(typeof(IRepositoryFactory))))
                {
                    var factory = Activator.CreateInstance(type) as IRepositoryFactory;

                    if (factory != null)
                    {
                        retVal.Add(new DatabaseType(factory.Name, factory.Id, type));
                    }
                }
            }

            return retVal;
        }
    }
}
