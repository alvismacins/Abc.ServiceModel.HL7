namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    internal static class HL7SerializerCache
    {
        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static Dictionary<string, XmlObjectSerializer> _xmlSerializers = new Dictionary<string, XmlObjectSerializer>(StringComparer.OrdinalIgnoreCase);

        internal static DataContractSerializer GetDataContractSerializer(Type type, string rootName, string rootNamespace) =>
            GetXmlObjectSerializer<DataContractSerializer>(
                type: type,
                serializerType: null,
                rootName: rootName,
                rootNamespace: rootNamespace,
                serializerFactory: (_type, _serializerType, _rootName, _rootNamespace) =>
                    new DataContractSerializer(
                        type: _type,
                        rootName: _rootName,
                        rootNamespace: _rootNamespace));

        internal static XElementObjectSerializer GetXElementObjectSerializer(string rootName, string rootNamespace) =>
            GetXmlObjectSerializer<XElementObjectSerializer>(
               type: typeof(XElementObjectSerializer),
               serializerType: null,
               rootName: rootName,
               rootNamespace: rootNamespace,
               serializerFactory: (_type, _serializerType, _rootName, _rootNamespace) =>
                   new XElementObjectSerializer(
                       rootName: _rootName,
                       rootNamespace: _rootNamespace));


        internal static XmlSerializerObjectSerializer GetXmlSerializerObjectSerializer(Type type, string rootName, string rootNamespace) =>
            GetXmlObjectSerializer<XmlSerializerObjectSerializer>(
                type: type,
                serializerType: null,
                rootName: rootName,
                rootNamespace: rootNamespace,
                serializerFactory: (_type, _serializerType, _rootName, _rootNamespace) =>
                    new XmlSerializerObjectSerializer(
                        type: _type,
                        rootName: _rootName,
                        rootNamespace: _rootNamespace));

        internal static T GetXmlObjectSerializer<T>(Type type, Type serializerType, string rootName, string rootNamespace, Func<Type, Type, string, string, T> serializerFactory)
            where T : XmlObjectSerializer
        {
            XmlObjectSerializer xmlObjectSerializer = null;

            string key = typeof(T).Name + type?.FullName + ":" + rootNamespace + ":" + rootName;
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (!_xmlSerializers.TryGetValue(key, out xmlObjectSerializer))
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        if (!_xmlSerializers.TryGetValue(key, out xmlObjectSerializer))
                        {
                            if (xmlObjectSerializer == null)
                            {
                                string normalizedRootName = rootName;
                                if (normalizedRootName != null)
                                {
                                    normalizedRootName = normalizedRootName.Replace(":HasAttrWithPrefix:1", string.Empty);
                                    normalizedRootName = normalizedRootName.Replace(":HasAttrWithPrefix:0", string.Empty);
                                }

                                xmlObjectSerializer = serializerFactory(type, serializerType, normalizedRootName, rootNamespace);
                            }

                           _xmlSerializers.Add(key, xmlObjectSerializer);
                        }
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            return xmlObjectSerializer as T;
        }
    }
}