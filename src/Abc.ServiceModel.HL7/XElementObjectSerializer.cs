namespace Abc.ServiceModel
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Serializer for subject
    /// </summary>
    public class XElementObjectSerializer : XmlObjectSerializer
    {
        private string rootName;
        private string rootNamespace;
        ////private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="XElementObjectSerializer"/> class
        /// to serialize or deserialize an XElement.
        /// </summary>
        public XElementObjectSerializer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XElementObjectSerializer"/> class
        /// to serialize or deserialize an object of the XElement using the
        /// supplied XML root element and namespace.
        /// </summary>
        /// <param name="rootName">The name of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="rootNamespace">The namespace of the XML element that encloses the content to serialize or deserialize.</param>
        public XElementObjectSerializer(string rootName, string rootNamespace)
        {
            this.rootName = rootName ?? string.Empty;
            this.rootNamespace = rootNamespace ?? string.Empty;
        }

        /////// <summary>
        /////// Initializes a new instance of the <see cref="XElementObjectSerializer"/> class.
        /////// </summary>
        /////// <param name="type">The type.</param>
        /////// <param name="name">The name.</param>
        ////public XElementObjectSerializer(Type type, XName name) {
        ////    Contract.Requires<ArgumentNullException>(type != null, "type");
        ////    Contract.Requires<ArgumentException>(type.BaseType.Name == "XTypedElement", "type");

        ////    this.type = type;
        ////    this.name = name;
        ////}

        /// <summary>
        /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlDictionaryReader"/> is positioned over an XML element that can be read.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML stream or document.</param>
        /// <returns>
        /// true if the reader can read the data; otherwise, false.
        /// </returns>
        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            reader.MoveToElement();

            if (!string.IsNullOrEmpty(this.rootName))
            {
                if (!string.IsNullOrEmpty(this.rootNamespace))
                {
                    return reader.IsStartElement(this.rootName, this.rootNamespace);
                }
                else
                {
                    return reader.IsStartElement(this.rootName);
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
            var body = XElement.Load(reader);
            ////if (this.type == null) {
            return body;
            ////}

            ////var method = Type.GetType("Xml.Schema.Linq.XTypedServices, Xml.Schema.Linq").GetMethod("ToXTypedElement", new Type[] { typeof(XElement) });
            ////var generic = method.MakeGenericMethod(this.type);
            ////return generic.Invoke(null, new object[] { body });
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
            XElement element = graph as XElement;

            // Support XsdToLinq Classes
            if (element == null)
            {
                Type graphType = graph.GetType();
                if (graphType.BaseType.Name == "XTypedElement")
                {
                    var property = graphType.GetProperty("Untyped");
                    if (property == null)
                    {
                        throw new InvalidOperationException();
                    }

                    element = (XElement)property.GetValue(graph, new object[0]);
                }
            }

            if (element == null)
            {
                throw new InvalidOperationException();
            }

            if (!string.IsNullOrEmpty(this.rootName))
            {
                /*
                if (name.Namespace != element.Name.Namespace) {
                    element.Add(new XAttribute("xmlns:x", name.Namespace));
                    element.SetAttributeValue()
                }
                 */

                if (!string.IsNullOrEmpty(this.rootNamespace))
                {
                    element.Name = XName.Get(this.rootName, this.rootNamespace);
                }
                else
                {
                    element.Name = XName.Get(this.rootName);
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