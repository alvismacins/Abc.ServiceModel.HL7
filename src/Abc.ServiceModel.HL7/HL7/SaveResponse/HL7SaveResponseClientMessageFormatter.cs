namespace Abc.ServiceModel.HL7
{
    using Abc.ServiceModel.Protocol.HL7;
    using System;
    using System.Globalization;

    /// <summary>
    /// HL7 message formmater.
    /// </summary>
    public partial class HL7SaveResponseMessageFormatter : IClientMessageFormatter
    {
        /// <summary>
        /// Converts a message into a return value and out parameters that are passed back to the calling operation.
        /// </summary>
        /// <param name="message">The inbound message.</param>
        /// <param name="parameters">Any out values.</param>
        /// <returns>
        /// The return value of the operation.
        /// </returns>
        public object DeserializeReply(Message message, object[] parameters)
        {
            if (message == null) {  throw new ArgumentNullException("message", "message != null"); }

            object body = null;

            // Set operation Context
            if (!message.IsFault)
            {
                string interactionId = this.attribute.ReplyInteraction;

                // string templateId = this.attribute.ReplyTemplate;
                var messageHl7 = message.ReadHL7Message(interactionId);

                // Generate HL7 Fault Exception
                if (messageHl7.Acknowledgement != null)
                {
                    if (messageHl7.Acknowledgement.AcknowledgementDataType != HL7AcknowledgementType.AcceptAcknowledgementCommitAccept
                        && messageHl7.Acknowledgement.AcknowledgementDataType != HL7AcknowledgementType.ApplicationAcknowledgementAccept)
                    {
                        throw new HL7FaultException(messageHl7.Acknowledgement.AcknowledgementDetails, null);
                    }
                }

                if (this.attribute != null && !this.attribute.AcknowledgementResponse && this.parameterType != typeof(void))
                {
                    body = messageHl7.ControlAct.Subject.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.MessageRequest));
                }

                var operationContext = new HL7OperationContext();
                operationContext.OperationSaveContract = this.attribute;

                if (OperationContext.Current != null)
                {
                    OperationContext.Current.Extensions.Add(operationContext);

                    if (messageHl7 != null)
                    {
                        operationContext.MessageId = messageHl7.IdentificationId.Extension;
                        operationContext.Sender = messageHl7.Sender;
                        operationContext.Receiver = messageHl7.Receiver;
                        operationContext.TargetMessage = messageHl7.Acknowledgement.TargetMessage.Extension;
                        operationContext.CreationTime = messageHl7.CreationTime;

                        if (messageHl7.Acknowledgement != null)
                        {
                            operationContext.AcknowledgementType = messageHl7.Acknowledgement.AcknowledgementDataType;

                            if (messageHl7.Acknowledgement.AcknowledgementDetails != null)
                            {
                                operationContext.AcknowledgementDetail = messageHl7.Acknowledgement.AcknowledgementDetails;
                            }
                        }
                    }
                }
            }

            return body;
        }

        /// <summary>
        /// Converts an <see cref="T:System.Object"/> array into an outbound <see cref="T:System.ServiceModel.Channels.Message"/>.
        /// </summary>
        /// <param name="messageVersion">The version of the SOAP message to use.</param>
        /// <param name="parameters">The parameters passed to the  client operation.</param>
        /// <returns>
        /// The SOAP message sent to the service operation.
        /// </returns>
        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            if (parameters == null) {  throw new ArgumentNullException("parameters", "parameters != null"); }
            if (!(parameters.Length > 0)) {  throw new ArgumentException("parameters", "parameters.Length > 0"); }
            if (!(parameters.Length <= 4)) {  throw new ArgumentException("parameters", "parameters.Length <= 4"); }

            string interactionId = this.attribute.Interaction;

            // UrnType templateId = new UrnType(this.attribute.Template);
            // string deviceSender = this.attribute.Sender;
            // string deviceReceiver = this.attribute.Receiver;
            HL7ControlAct controlAct = parameters[0] as HL7ControlAct;

            // if (controlAct != null)
            // {
            //    controlAct = (HL7ControlAct)parameters[0];
            // }
            // else
            // {
            //    // throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "controlAct"));
            // }
            string receiverExtension = string.Empty;

            if (parameters.Length > 1 && parameters[1] != null && (parameters[1] is string))
            {
                receiverExtension = (string)parameters[1];
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "receiverExtension"));
            }

            HL7Acknowledgement acknowledgementDetail = null;

            if (parameters.Length > 2 && parameters[2] != null)
            {
                if (parameters[2] is HL7Acknowledgement)
                {
                    acknowledgementDetail = (HL7Acknowledgement)parameters[2];
                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "acknowledgementDetail"));
                }
            }

            this.attribute.Receiver = receiverExtension;
            HL7Device sender = new HL7Device(this.attribute.Sender, HL7Constants.AttributesValue.Sender);
            HL7Device receiver = new HL7Device(this.attribute.Receiver, HL7Constants.AttributesValue.Receiver);
            HL7TransmissionWrapper response;
            response = this.CreateResponse(interactionId, receiver, sender, acknowledgementDetail, controlAct);

            // }
            // else
            // {
            //    if (!(controlAct is string))
            //    {
            //        response = CreateMessageResponse(interactionId, receiver, sender, acknowledgementDetail, controlAct);
            //    }
            //    else
            //    {
            //        response = CreateMessageResponse(interactionId, receiver, sender, acknowledgementDetail);
            //    }
            // }
            if (response == null || response.Acknowledgement == null || response.Acknowledgement.TargetMessage == null || response.Acknowledgement.TargetMessage.Extension == null)
            {
                throw new ArgumentException("Acknowledgement identification is not set {response.Acknowledgement.TargetMessage.Extension}");
            }

            return HL7MessageExtension.CreateHL7Message(messageVersion, interactionId, response);
        }
    }
}