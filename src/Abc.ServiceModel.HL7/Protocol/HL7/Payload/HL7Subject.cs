namespace Abc.ServiceModel.Protocol.HL7
{
    using Abc.ServiceModel.HL7.Extensions;
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Linq;

    // using Xml.Schema.Linq;

    /// <summary>
    /// Satur pieprasījuma/atbildes rezultātu, kas ir atkarīgs no vaicājuma specifikācijas ziņojumā saņemtajiem parametriem.
    /// </summary>
    public class HL7Subject
    {
        private object data;
        private XmlObjectSerializer serializer;
        private XElement xmlElement;

        private HL7Subject()
        {
        }

        private HL7Subject(XmlObjectSerializer serializer, object body)
        {
            this.serializer = serializer;
            this.data = body;
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>The HL7 subject.</returns>
        public static HL7Subject CreateSubject(object body)
        {
            if (body == null) { throw new ArgumentNullException("body", "body != null"); }

            return CreateSubject(body, HL7SubjectSerializerDefaults.CreateSerializer(body.GetType(), rootName: null, rootNamespace: null));
        }

        /// <summary>
        /// Creates the subject with specified serializer.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="serializer">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7Subject CreateSubject(object body, XmlObjectSerializer serializer)
        {
            if (body == null) { throw new ArgumentNullException("body", "body != null"); }
            if (serializer == null) { throw new ArgumentNullException("serializer", "serializer != null"); }

            return new HL7Subject(serializer, body);
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7Subject CreateSubject(XmlReader reader)
        {
            if (reader == null) { throw new ArgumentNullException("reader", "reader != null"); }

            var subject = new HL7Subject();

            subject.ReadSubject(reader);
            return subject;
        }

        /// <summary>
        /// Gets the name of the subject element.
        /// </summary>
        /// <value>
        /// The name of the subject element.
        /// </value>
        internal string SubjectElementName { get; private set; }

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
            return (T)this.GetBody(HL7SubjectSerializerDefaults.CreateSerializer(typeof(T), rootName: this.SubjectElementName, rootNamespace: HL7Constants.Namespace));
        }

        /// <summary>
        /// Retrieves the body of this <see cref="HL7Subject"/> instance using the specified serializer.
        /// </summary>
        /// <param name="serializerParam">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>
        /// An object of type that contains the body of this message.
        /// </returns>
        public object GetBody(XmlObjectSerializer serializerParam)
        {
            if (serializerParam == null) { throw new ArgumentNullException("serializerParam", "serializerParam != null"); }

            using (XmlReader reader = this.xmlElement.CreateReader())
            {
                return serializerParam.ReadObject(reader);
            }
        }

        /// <summary>
        /// Writes the subject.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteSubject(XmlWriter writer)
        {
            this.SubjectElementName = null;
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
        public virtual void WriteSubject(XmlDictionaryWriter writer)
        {
            this.SubjectElementName = null;
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
        protected virtual void ReadSubject(XmlReader reader)
        {
            if (reader == null) { throw new ArgumentNullException("reader", "reader != null"); }

            // Subject
            if (!reader.IsStartElement(HL7Constants.Elements.Subject, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.Subject, HL7Constants.Namespace);
            }

            var prefix = reader.Prefix;
            this.xmlElement = (XElement)XElement.ReadFrom(reader);

            // secibai ir nozime. sim ir jaizpildas pirms prefix pievienosanas xmlElement, jo si vertiba ietekme serializatora cache key.
            this.SubjectElementName = this.xmlElement.GetElementNameWithPrefix();
            if (this.SubjectElementName == null)
            {
                return;
            }

            //var containsAttributesFromThisNamespace = this.xmlElement.ContainsAttributesFromThisNamespace(prefix: prefix);
            //this.SubjectElementName += containsAttributesFromThisNamespace
            //    ? ":HasAttrWithPrefix:1"
            //    : ":HasAttrWithPrefix:0";

            // Add missing hl7 namespace declaration if not present
            if (!string.IsNullOrEmpty(prefix) &&
                this.xmlElement.GetNamespaceOfPrefix(prefix) == null // &&
                //containsAttributesFromThisNamespace
                )
            {
                this.xmlElement.Add(new XAttribute(XNamespace.Xmlns + prefix, HL7Constants.Namespace));
            }
        }
    }
}