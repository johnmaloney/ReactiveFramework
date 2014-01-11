using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Common.Extensions
{
    public static class TypeExtensions
    {
        public static string GetMessageType(this Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}
