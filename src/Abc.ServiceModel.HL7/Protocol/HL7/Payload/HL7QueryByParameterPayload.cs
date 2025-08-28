namespace Abc.ServiceModel.Protocol.HL7
{
    using Abc.ServiceModel.HL7.Extensions;
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Linq;

    // using Xml.Schema.Linq;

    /// <summary>
    /// HL7 shēmas klase. Parametru definēšanas struktūra vaicājumā query by parameter ir informatīva struktūra, kas varētu pēc formas būt līdzīga daudzu vaicājumu realizācijās, bet var atšķirties pēc vaicājuma atribūta nozīmes, piemērs aprakstīts tālāk. Ievadmainīgie tiek specificēti parametru atkārtojumos.  [3]
    /// </summary>
    public class HL7QueryByParameterPayload
    {
        private object data;
        private XmlObjectSerializer serializer;
        private XElement xmlElement;

        /// <summary>
        /// Prevents a default instance of the <see cref="HL7QueryByParameterPayload"/> class from being created.
        /// </summary>
        private HL7QueryByParameterPayload()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryByParameterPayload"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="body">The body.</param>
        private HL7QueryByParameterPayload(XmlObjectSerializer serializer, object body)
        {
            this.serializer = serializer;
            this.data = body;
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>The HL7 subject.</returns>
        public static HL7QueryByParameterPayload CreateQueryByParameterPayload(object body)
        {
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }

            return CreateQueryByParameterPayload(body, HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(body.GetType(), rootName: null, rootNamespace: null));
        }

        /// <summary>
        /// Creates the subject with specified serializer.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="serializer">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>
        /// The <see cref="HL7Subject"/>.
        /// </returns>
        public static HL7QueryByParameterPayload CreateQueryByParameterPayload(object body, XmlObjectSerializer serializer)
        {
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }
            if (serializer == null) {  throw new ArgumentNullException("serializer", "serializer != null"); }

            return new HL7QueryByParameterPayload(serializer, body);
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7QueryByParameterPayload CreateQueryByParameterPayload(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            var subject = new HL7QueryByParameterPayload();
            subject.ReadQueryByParameterPayload(reader);
            return subject;
        }

        /// <summary>
        /// Gets the name of the QueryByParameterPayload element.
        /// </summary>
        /// <value>
        /// The name of the QueryByParameterPayload element.
        /// </value>
        internal string QueryByParameterPayloadElementName { get; private set; }

        /// <summary>
        /// Creates the reader.
        /// </summary>
        /// <returns>The XmlReader</returns>
        public virtual XmlReader CreateReader()
        {
            return this.xmlElement.CreateReader();
        }

        /// <summary>
        ///  Retrieves the body of this <see cref="HL7Subject"/> instance.
        /// </summary>
        /// <typeparam name="T">The body of the message.</typeparam>
        /// <returns>An object of type T that contains the body of this message.</returns>
        public T GetBody<T>()
        {
            return (T)this.GetBody(HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(typeof(T), rootName: this.QueryByParameterPayloadElementName, rootNamespace: HL7Constants.Namespace));
        }

        /// <summary>
        /// Retrieves the body of this <see cref="HL7Subject"/> instance using the specified serializer.
        /// </summary>
        /// <param name="serializerBody">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>
        /// An object of type that contains the body of this message.
        /// </returns>
        public object GetBody(XmlObjectSerializer serializerBody)
        {
            if (serializerBody == null) {  throw new ArgumentNullException("serializerBody", "serializerBody != null"); }

            using (XmlReader reader = this.xmlElement.CreateReader())
            {
                return serializerBody.ReadObject(reader);
            }
        }

        /// <summary>
        /// Writes the subject.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteQueryByParameterPayload(XmlWriter writer)
        {
            this.QueryByParameterPayloadElementName = null;
            if (this.data != null)
            {
                this.serializer.WriteObject(writer, this.data);
            }

            if (this.xmlElement != null)
            {
                this.xmlElement.WriteTo(writer);
            }
        }

        /// <summary>
        /// Writes the subject.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void WriteQueryByParameterPayload(XmlDictionaryWriter writer)
        {
            this.QueryByParameterPayloadElementName = null;
            if (this.data != null)
            {
                this.serializer.WriteObject(writer, this.data);
            }

            if (this.xmlElement != null)
            {
                this.xmlElement.WriteTo(writer);
            }
        }

        /// <summary>
        /// Reads the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        protected virtual void ReadQueryByParameterPayload(XmlReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            // Subject
            if (!reader.IsStartElement(HL7Constants.Elements.QueryByParameterPayload, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.QueryByParameterPayload, HL7Constants.Namespace);
            }

            var prefix = reader.Prefix;
            this.xmlElement = (XElement)XElement.ReadFrom(reader);
            this.QueryByParameterPayloadElementName = this.xmlElement.GetElementNameWithPrefix();

            if (this.QueryByParameterPayloadElementName == null)
            {
                return;
            }

            //var containsAttributesFromThisNamespace = this.xmlElement.ContainsAttributesFromThisNamespace(prefix: prefix);
            //this.QueryByParameterPayloadElementName += containsAttributesFromThisNamespace
            //    ? ":HasAttrWithPrefix:1"
            //    : ":HasAttrWithPrefix:0";

            // Add missing hl7 namespace declaration if not present
            if (!string.IsNullOrEmpty(prefix) &&
                this.xmlElement.GetNamespaceOfPrefix(prefix) == null /* &&
                containsAttributesFromThisNamespace */
                )
            {
                this.xmlElement.Add(new XAttribute(XNamespace.Xmlns + prefix, HL7Constants.Namespace));
            }
        }
    }
}