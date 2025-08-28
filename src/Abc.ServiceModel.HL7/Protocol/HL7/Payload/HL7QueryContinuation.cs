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
    /// HL7 shēmas klase.  Vaicājuma turpinājuma ziņojums (Query Continuation Message) ir darbību kontroles ziņojums, kas tiek lietots vaicājuma loģiskās sesijas uzturēšanai. Šī HL7 V3 kompozīta ziņojuma informatīvā daļa sastāv no HL7 pārraides apvalka un minimālā vaicājuma vadības darbības apvalka, un parasti iekļauj esošā vaicājuma identifikatoru un citus parametrus, kas ir nepieciešami pieprasījuma turpināšanai vai pārtraukšanai. [3]
    /// </summary>
    public class HL7QueryContinuation
    {
        private object data;
        private XmlObjectSerializer serializer;
        private XElement xmlElement;

        private HL7QueryContinuation()
        {
        }

        private HL7QueryContinuation(XmlObjectSerializer serializer, object body)
        {
            this.serializer = serializer;
            this.data = body;
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>The HL7 subject.</returns>
        public static HL7QueryContinuation CreateQueryContinuation(object body)
        {
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }

            return CreateQueryContinuation(body, HL7SubjectSerializerDefaults.CreateSerializer(body.GetType(), rootName: null, rootNamespace: null));
        }

        /// <summary>
        /// Creates the subject with specified serializer.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="serializer">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7QueryContinuation CreateQueryContinuation(object body, XmlObjectSerializer serializer)
        {
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }
            if (serializer == null) {  throw new ArgumentNullException("serializer", "serializer != null"); }

            return new HL7QueryContinuation(serializer, body);
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7QueryContinuation CreateQueryContinuation(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            var subject = new HL7QueryContinuation();
            subject.ReadQueryContinuation(reader);
            return subject;
        }

        /// <summary>
        /// Gets the name of the QueryByParameterPayload element.
        /// </summary>
        /// <value>
        /// The name of the QueryByParameterPayload element.
        /// </value>
        internal string QueryContinuationElementName { get; private set; }

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
            return (T)this.GetBody(HL7SubjectSerializerDefaults.CreateSerializer(typeof(T), rootName: this.QueryContinuationElementName, rootNamespace: HL7Constants.Namespace));
        }

        /// <summary>
        /// Retrieves the body of this <see cref="HL7Subject"/> instance using the specified serializer.
        /// </summary>
        /// <param name="serializer">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>
        /// An object of type that contains the body of this message.
        /// </returns>
        public object GetBody(XmlObjectSerializer serializer)
        {
            if (serializer == null) {  throw new ArgumentNullException("serializer", "serializer != null"); }

            using (XmlReader reader = this.xmlElement.CreateReader())
            {
                return serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Writes the subject.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteQueryContinuation(XmlWriter writer)
        {
            this.QueryContinuationElementName = null;
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
        public virtual void WriteQueryContinuation(XmlDictionaryWriter writer)
        {
            this.QueryContinuationElementName = null;
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
        protected virtual void ReadQueryContinuation(XmlReader reader)
        {
            if (reader == null) 
            {
                throw new ArgumentNullException("reader", "reader != null"); 
            }

            // Subject
            if (!reader.IsStartElement(HL7Constants.Elements.QueryContinuation, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.QueryContinuation, HL7Constants.Namespace);
            }

            var prefix = reader.Prefix;
            this.xmlElement = (XElement)XElement.ReadFrom(reader);
            this.QueryContinuationElementName = this.xmlElement.GetElementNameWithPrefix();

            if (this.QueryContinuationElementName == null)
            {
                return;
            }

            //var containsAttributesFromThisNamespace = this.xmlElement.ContainsAttributesFromThisNamespace(prefix: prefix);
            //this.QueryContinuationElementName += containsAttributesFromThisNamespace
            //    ? ":HasAttrWithPrefix:1"
            //    : ":HasAttrWithPrefix:0";

            // Add missing hl7 namespace declaration if not present
            if (!string.IsNullOrEmpty(prefix) &&
                this.xmlElement.GetNamespaceOfPrefix(prefix) == null /* &&
                containsAttributesFromThisNamespace*/)
            {
                this.xmlElement.Add(new XAttribute(XNamespace.Xmlns + prefix, HL7Constants.Namespace));
            }
        }
    }
}