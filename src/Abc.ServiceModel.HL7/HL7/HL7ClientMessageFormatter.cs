namespace Abc.ServiceModel.HL7
{
    using Abc.ServiceModel.Protocol.HL7;
    using System;

    public partial class HL7MessageFormatter : IClientMessageFormatter
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
                HL7TransmissionWrapper messageHl7 = null;

                if (!string.Equals(interactionId, "*", StringComparison.OrdinalIgnoreCase))
                {
                    messageHl7 = message.ReadHL7Message(interactionId);
                }
                else
                {
                    var mess = message.CreateBufferedCopy(int.MaxValue);
                    messageHl7 = mess.CreateMessage().ReadHL7Message();

                    return mess.CreateMessage();
                }

                // Generate HL7 Fault Exception
                if (messageHl7.Acknowledgement != null)
                {
                    if (messageHl7.Acknowledgement.AcknowledgementDataType != HL7AcknowledgementType.AcceptAcknowledgementCommitAccept
                        && messageHl7.Acknowledgement.AcknowledgementDataType != HL7AcknowledgementType.ApplicationAcknowledgementAccept
                        && messageHl7.Acknowledgement.AcknowledgementDetails != null && messageHl7.Acknowledgement.AcknowledgementDetails.Count > 0)
                    {
                        throw new HL7FaultException(messageHl7.Acknowledgement.AcknowledgementDetails, null);
                    }
                }

                if (this.attribute != null && !this.attribute.AcknowledgementResponse && this.parameterType != typeof(void) && messageHl7 != null && messageHl7.ControlAct != null && messageHl7.ControlAct.Subject != null)
                {
                    body = messageHl7.ControlAct.Subject.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.MessageRequest));
                }

                var operationContext = new HL7OperationContext();
                operationContext.OperationContract = this.attribute;

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
                        operationContext.SetAttentionLine = messageHl7.AttentionLineCollection;

                        if (messageHl7.AttentionLineCollection != null)
                        {
                            operationContext.GetAttentionLine = messageHl7.AttentionLineCollection;
                        }

                        if (messageHl7.ControlAct != null && messageHl7.ControlAct.ReasonCodes.Count > 0)
                        {
                            foreach (var item in messageHl7.ControlAct.ReasonCodes)
                            {
                                if (item != null && OId.Compare(item.CodeSystem, HL7Constants.OIds.ActionCodeId) == 0)
                                {
                                    operationContext.ActionCode = item;
                                }

                                if (item != null && OId.Compare(item.CodeSystem, HL7Constants.OIds.ReasonCodeId) == 0)
                                {
                                    operationContext.ReasonCode = item;
                                }
                            }

                            HL7QueryControlAcknowledgement queryControlAck = messageHl7.ControlAct as HL7QueryControlAcknowledgement;
                            if (queryControlAck != null)
                            {
                                operationContext.QueryAcknowledgement = queryControlAck.QueryAcknowledgement;
                                operationContext.GetOverseers = queryControlAck.Overseer;
                                operationContext.GetDataEnterers = queryControlAck.DataEnterers;
                            }

                            HL7MessageControlAct messageControlAct = messageHl7.ControlAct as HL7MessageControlAct;
                            if (messageControlAct != null)
                            {
                                operationContext.GetOverseers = messageControlAct.Overseer;
                                operationContext.GetDataEnterers = messageControlAct.DataEnterers;
                            }
                        }

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

            string interactionId = this.attribute.Interaction;

            // UrnType templateId = new UrnType(this.attribute.Template);
            string deviceSender = this.attribute.Sender;
            string deviceReceiver = this.attribute.Receiver;
            object body = parameters[0];
            this.queryRequest = this.attribute.QueryParameterPayload;

            var operationContext = HL7OperationContext.Current;

            if (this.attribute.GetsActionCode == null && operationContext != null && operationContext.ActionCode != null)
            {
                this.attribute.GetsActionCode = operationContext.ActionCode.Code;
            }

            if (this.attribute.GetsReasonCode == null && operationContext != null && operationContext.ReasonCode != null)
            {
                this.attribute.GetsReasonCode = operationContext.ReasonCode.Code;
            }

            return this.CreateMessageRequest(body, interactionId, deviceSender, deviceReceiver, this.attribute.GetsActionCode, this.attribute.GetsReasonCode, this.attribute.ControlActDescription, this.attribute.PriorityCode, this.attribute.LanguageCode, this.queryRequest, messageVersion);
        }
    }
}