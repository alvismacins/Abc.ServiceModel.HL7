namespace Abc.ServiceModel.HL7
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Message extension.
    /// </summary>
    public static class HL7MessageExtension
    {
        /// <summary>
        /// Creates the Hl7 message.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="messageHL7">The message HL7.</param>
        /// <returns>Message data</returns>
        public static Message CreateHL7Message(MessageVersion version, string interactionId, HL7TransmissionWrapper messageHL7)
        {
            if (!string.IsNullOrEmpty(interactionId) && (Helper.CheckSchema(interactionId) == Helper.SchemaType.Lvext || Helper.CheckSchema(interactionId) == Helper.SchemaType.Multicacheschemas))
            {
                string action = HL7Constants.Namespace + ":" + interactionId;
                Message mess = Message.CreateMessage(version, action, (BodyWriter)new HL7ServiceBodyWriter(messageHL7, new HL7Serializer(), interactionId));

                // TODO:Reply
                // mess.Headers.ReplyTo = new EndpointAddress(this);
                return mess;
            }
            else
            {
                throw new FormatException(SrProtocol.IncorectInteractionId);
            }
        }

        /// <summary>
        /// Reads the HL7 message.
        /// </summary>
        /// <param name="message">The Servcie message.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <returns>The HL7 Message.</returns>
        public static HL7TransmissionWrapper ReadHL7Message(this Message message, string interactionId)
        {
            if (message == null) {  throw new ArgumentNullException("message", "message != null"); }

            ////string interacionId = message.Headers.Action.Substring(HL7Constants.Namespace.Length + 1);

            HL7TransmissionWrapper messageHl7;
            using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
            {
                HL7Serializer serializer = new HL7Serializer();
                messageHl7 = serializer.ReadMessage(reader, interactionId);

                if (message.Version.Envelope != EnvelopeVersion.None)
                {
                    HL7Serializer.EndElement(reader);
                }

                reader.MoveToContent();
            }

            return messageHl7;
        }

        /// <summary>
        /// Reads the H l7 message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>HL7Transmission Wrapper</returns>
        public static HL7TransmissionWrapper ReadHL7Message(this Message message)
        {
            if (message == null) {  throw new ArgumentNullException("message", "message != null"); }

            ////string interacionId = message.Headers.Action.Substring(HL7Constants.Namespace.Length + 1);

            HL7TransmissionWrapper messageHl7;
            using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
            {
                HL7Serializer serializer = new HL7Serializer();
                messageHl7 = serializer.ReadMessage(reader);

                if (message.Version.Envelope != EnvelopeVersion.None)
                {
                    HL7Serializer.EndElement(reader);
                }

                reader.MoveToContent();
            }

            return messageHl7;
        }
    }
}