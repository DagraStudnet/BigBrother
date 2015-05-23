using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WcfServiceLibrary
{
    public static class KnownTypesProvider
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            Assembly dtoDefinitions =
                Assembly.Load("BigBrother, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var query = from t in dtoDefinitions.GetTypes()
                where t.IsClass && t.Namespace == "ClientBigBrother.Model"
                select t;
            return query.ToList();
        }
    }
}