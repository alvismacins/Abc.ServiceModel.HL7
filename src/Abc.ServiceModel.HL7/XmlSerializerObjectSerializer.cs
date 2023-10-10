namespace Abc.ServiceModel
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializerObjectSerializer : XmlObjectSerializer
    {
        private bool isSerializerSetExplicit;
        private string rootName;
        private string rootNamespace;
        private Type rootType;
        private XmlSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerObjectSerializer"/> class
        /// to serialize or deserialize an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the instances that are serialized or deserialized.</param>
        public XmlSerializerObjectSerializer(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Initialize(type, null, null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerObjectSerializer"/> class
        /// to serialize or deserialize an object of the specified type using the
        /// supplied XML root element and namespace.
        /// </summary>
        /// <param name="type">The type of the instances that are serialized or deserialized.</param>
        /// <param name="rootName">The name of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="rootNamespace">The namespace of the XML element that encloses the content to serialize or deserialize.</param>
        public XmlSerializerObjectSerializer(Type type, string rootName, string rootNamespace)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Initialize(type, rootName, rootNamespace, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerObjectSerializer"/> class.
        /// to serialize or deserialize an object of the specified type using the
        /// supplied XML root element and namespace.
        /// </summary>
        /// <param name="type">The type of the instances that are serialized or deserialized.</param>
        /// <param name="rootName">The name of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="rootNamespace">The namespace of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="xmlSerializer">The XML serializer.</param>
        public XmlSerializerObjectSerializer(Type type, string rootName, string rootNamespace, XmlSerializer xmlSerializer)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Initialize(type, rootName, rootNamespace, xmlSerializer);
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
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            reader.MoveToElement();

            if (!string.IsNullOrEmpty(this.rootName))
            {
                return reader.IsStartElement(this.rootName, this.rootNamespace);
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
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!this.isSerializerSetExplicit)
            {
                return this.serializer.Deserialize(reader);
            }

            object[] objArray = (object[])this.serializer.Deserialize(reader);
            if (objArray != null && objArray.Length > 0)
            {
                return objArray[0];
            }

            return null;
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
            // new NotImplementedException();
        }

        /// <summary>
        /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the content to the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObject(XmlDictionaryWriter writer, object graph)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.isSerializerSetExplicit)
            {
                this.serializer.Serialize((XmlWriter)writer, new object[] { graph });
            }
            else
            {
                this.serializer.Serialize((XmlWriter)writer, graph);
            }
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
            // new NotImplementedException();
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
            // new NotImplementedException();
        }

        private void Initialize(Type type, string rootNameParam, string rootNamespaceParam, XmlSerializer xmlSerializerParam)
        {
            this.rootType = type ?? throw new ArgumentNullException(nameof(type));
            this.rootName = rootNameParam ?? string.Empty;
            this.rootNamespace = rootNamespaceParam ?? string.Empty;
            this.serializer = xmlSerializerParam;

            if (this.serializer == null)
            {
                if (string.IsNullOrEmpty(this.rootName))
                {
                    this.serializer = new XmlSerializer(type);
                }
                else
                {
                    XmlRootAttribute root = new XmlRootAttribute
                    {
                        ElementName = this.rootName,
                        Namespace = this.rootNamespace,
                    };

                    this.serializer = new XmlSerializer(type, root);
                }
            }
            else
            {
                this.isSerializerSetExplicit = true;
            }

            if (string.IsNullOrEmpty(this.rootName))
            {
                XmlTypeMapping mapping = new XmlReflectionImporter().ImportTypeMapping(this.rootType);
                this.rootName = mapping.ElementName;
                this.rootNamespace = mapping.Namespace;
            }
        }
    }
}