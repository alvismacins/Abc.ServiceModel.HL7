namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;
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
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }

            return CreateSubject(body, HL7SubjectSerializerDefaults.CreateSerializer(body.GetType()));
        }

        /// <summary>
        /// Creates the subject with specified serializer.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="serializer">The <see cref="XmlObjectSerializer"/>.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7Subject CreateSubject(object body, XmlObjectSerializer serializer)
        {
            if (body == null) {  throw new ArgumentNullException("body", "body != null"); }
            if (serializer == null) {  throw new ArgumentNullException("serializer", "serializer != null"); }

            return new HL7Subject(serializer, body);
        }

        /// <summary>
        /// Creates the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The <see cref="HL7Subject"/>.</returns>
        public static HL7Subject CreateSubject(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            var subject = new HL7Subject();
            subject.ReadSubject(reader);
            return subject;
        }

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
            return (T)this.GetBody(HL7SubjectSerializerDefaults.CreateSerializer(typeof(T)));
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
            if (serializerParam == null) {  throw new ArgumentNullException("serializerParam", "serializerParam != null"); }

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
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            // Subject
            if (!reader.IsStartElement(HL7Constants.Elements.Subject, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.Subject, HL7Constants.Namespace);
            }

            this.xmlElement = (XElement)XElement.ReadFrom(reader);
        }
    }
}