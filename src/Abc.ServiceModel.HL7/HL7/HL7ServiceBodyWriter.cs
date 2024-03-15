// ----------------------------------------------------------------------------
// <copyright file="HL7ServiceBodyWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using System.Xml;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Service Body Writer
    /// </summary>
    internal class HL7ServiceBodyWriter : BodyWriter
    {
        private string localName;
        private HL7TransmissionWrapper message;
        private HL7Serializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ServiceBodyWriter"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="localName">Name of the local.</param>
        public HL7ServiceBodyWriter(HL7TransmissionWrapper message, HL7Serializer serializer, string localName)
            : base(true)
        {
            this.serializer = serializer;
            this.message = message;
            this.localName = localName;
        }

        /// <summary>
        /// When implemented, provides an extensibility point when the body contents are written.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write out the message body.</param>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            this.serializer.WriteMessage(writer, this.message, this.localName);
        }
    }
}