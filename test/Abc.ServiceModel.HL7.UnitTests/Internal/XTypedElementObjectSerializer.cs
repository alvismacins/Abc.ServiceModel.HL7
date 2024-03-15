using System.Xml;
using System.Xml.Linq;
using Xml.Schema.Linq;

namespace System.Runtime.Serialization
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XTypedElementObjectSerializer : XmlObjectSerializer
    {
        private string rootName;
        private string rootNamespace;
        private Type type;
        private ILinqToXsdTypeManager typeManager;

        public XTypedElementObjectSerializer(Type type, ILinqToXsdTypeManager typeManager)
            : this(type, typeManager, null, null)
        {
        }

        /// Initializes a new instance of the <see cref="XmlSerializerObjectSerializer"/> class
        /// to serialize or deserialize an object of the specified type using the
        /// supplied XML root element and namespace.
        /// </summary>
        /// <param name="type">The type of the instances that are serialized or deserialized.</param>
        /// <param name="rootName">The name of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="rootNamespace">The namespace of the XML element that encloses the content to serialize or deserialize.</param>
        public XTypedElementObjectSerializer(Type type, ILinqToXsdTypeManager typeManager, string rootName, string rootNamespace)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (typeManager == null)
            {
                throw new ArgumentNullException("typeManager");
            }

            if (!typeof(XTypedElement).IsAssignableFrom(type))
            {
                throw new ArgumentException("type");
            }

            this.type = type;
            this.typeManager = typeManager;
            this.rootName = rootName ?? string.Empty;
            this.rootNamespace = rootNamespace ?? string.Empty;
        }

        /// <summary>
        /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlDictionaryReader"/> is positioned over an XML element that can be read.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML stream or document.</param>
        /// <returns>
        /// true if the reader can read the data; otherwise, false.
        /// </returns>
        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            reader.MoveToElement();
            if (!string.IsNullOrEmpty(rootName))
            {
                if (!string.IsNullOrEmpty(rootNamespace))
                {
                    return reader.IsStartElement(rootName, rootNamespace);
                }
                else
                {
                    return reader.IsStartElement(rootName);
                }
            }

            return reader.IsStartElement();
        }

        /// <summary>
        /// Reads the XML stream or document with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; otherwise, false to skip the verification.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            var element = XElement.Load(reader);
            return XTypedServices.ToXTypedElement(element, typeManager, type);
        }

        /// <summary>
        /// Writes the end of the object data as a closing XML element to the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteEndObject(XmlDictionaryWriter writer)
        {
        }

        /// <summary>
        /// Writes only the content of the object to the XML document or stream using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            var body = graph as XTypedElement;
            if (body == null)
            {
                throw new ArgumentException("graph");
            }

            var element = body.Untyped;
            if (!string.IsNullOrEmpty(rootName))
            {
                if (!string.IsNullOrEmpty(rootNamespace))
                {
                    element.Name = XName.Get(rootName, rootNamespace);
                }
                else
                {
                    element.Name = XName.Get(rootName);
                }
            }

            element.WriteTo(writer);
        }

        /// <summary>
        /// Writes the start of the object's data as an opening XML element using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document.</param>
        /// <param name="graph">The object to serialize.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {
        }
    }
}