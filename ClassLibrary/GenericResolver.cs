using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

namespace ClassLibrary
{
    public class GenericResolver : DataContractResolver
    {
        private const string DefaultNamespace = "global";

        private readonly Dictionary<string, Dictionary<string, Type>> namesToType;
        private readonly Dictionary<Type, Tuple<string, string>> typeToNames;

        public GenericResolver() : this(ReflectTypes())
        {
        }

        public GenericResolver(Type[] typesToResolve)
        {
            typeToNames = new Dictionary<Type, Tuple<string, string>>();
            namesToType = new Dictionary<string, Dictionary<string, Type>>();
            foreach (Type type in typesToResolve)
            {
                string typeNamespace = GetNamespace(type);
                string typeName = GetName(type);
                typeToNames[type] = new Tuple<string, string>(typeNamespace, typeName);
                if (namesToType.ContainsKey(typeNamespace) == false)
                {
                    namesToType[typeNamespace] = new Dictionary<string, Type>();
                }
                namesToType[typeNamespace][typeName] = type;
            }
        }

        public Type[] KnownTypes
        {
            get { return typeToNames.Keys.ToArray(); }
        }

        // Get all types in calling assembly and referenced assemblies
        private static Type[] ReflectTypes()
        {
            Assembly mscorlib = typeof(string).Assembly;
            return mscorlib.GetTypes();
        }

        private static string GetNamespace(Type type)
        {
            return type.Namespace ?? DefaultNamespace;
        }

        private static string GetName(Type type)
        {
            return type.Name;
        }

        public static GenericResolver Merge(
            GenericResolver resolver1, GenericResolver resolver2)
        {
            if (resolver1 == null)
            {
                return resolver2;
            }
            if (resolver2 == null)
            {
                return resolver1;
            }
            var types = new List<Type>();
            types.AddRange(resolver1.KnownTypes);
            types.AddRange(resolver2.KnownTypes);
            return new GenericResolver(types.ToArray());
        }

        public override Type ResolveName(
            string typeName, string typeNamespace,
            Type declaredType,
            DataContractResolver knownTypeResolver)
        {
            if (namesToType.ContainsKey(typeNamespace))
            {
                if (namesToType[typeNamespace].ContainsKey(typeName))
                {
                    return namesToType[typeNamespace][typeName];
                }
            }
            return knownTypeResolver.ResolveName(
                typeName, typeNamespace, declaredType, null);
        }

        public override bool TryResolveType(
            Type type, Type declaredType,
            DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName,
            out XmlDictionaryString typeNamespace)
        {
            if (typeToNames.ContainsKey(type))
            {
                var dictionary = new XmlDictionary();
                typeNamespace = dictionary.Add(typeToNames[type].Item1);
                typeName = dictionary.Add(typeToNames[type].Item2);
                return true;
            }
            return knownTypeResolver.TryResolveType(
                type, declaredType, null, out typeName,
                out typeNamespace);
        }
    }
}