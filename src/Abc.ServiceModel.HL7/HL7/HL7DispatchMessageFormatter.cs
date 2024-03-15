#if NETFRAMEWORK || CoreWCF

namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Abc.ServiceModel.Protocol.HL7;

    public partial class HL7MessageFormatter : IDispatchMessageFormatter
    {
        /// <summary>
        /// Deserializes a message into an array of parameters.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <param name="parameters">The objects that are passed to the operation as parameters.</param>
        [SuppressMessage("Microsoft.Contracts", "Requires", Justification = "No contract for base class")]
        public void DeserializeRequest(Message message, object[] parameters)
        {
            if (parameters == null) {  throw new ArgumentNullException("parameters", "parameters != null"); }
            if (!(parameters.Length > 0)) {  throw new ArgumentException("parameters", "parameters.Length > 0"); }

            // Set operation Context
            var operationContext = new HL7OperationContext();
            operationContext.OperationContract = this.attribute;
            OperationContext.Current.Extensions.Add(operationContext);
            var messageHL7 = HL7MessageExtension.ReadHL7Message(message, this.attribute.Interaction);

            HL7Request request = messageHL7 as HL7Request;
            if (request == null)
            {
                throw new FormatException("is not request");
            }

            this.queryRequest = this.attribute.QueryParameterPayload;

            switch (this.attribute.QueryParameterPayload)
            {
                case HL7Request.RequestType.QueryParamRequest:

                    if (request.QueryControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.QueryControlAct"));
                    }

                    parameters[0] = request.QueryControlAct.QueryByParameterPayload.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.QueryParamRequest));
                    break;

                case HL7Request.RequestType.MessageRequest:

                    if (request.ControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.ControlAct"));
                    }

                    var param = this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.MessageRequest);

                    if (request.ControlAct != null && request.ControlAct.Subject != null)
                    {
                        parameters[0] = request.ControlAct.Subject.GetBody(param);
                    }

                    break;

                case HL7Request.RequestType.QueryContinuationRequest:

                    if (request.QueryControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.QueryControlAct"));
                    }

                    parameters[0] = request.QueryControlAct.QueryContinuation.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.QueryContinuationRequest));
                    break;

                default:

                    if (request.ControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.ControlAct"));
                    }

                    parameters[0] = request.ControlAct.Subject.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.MessageRequest));
                    break;
            }

            if (messageHL7 != null)
            {
                if (messageHL7.IdentificationId != null)
                {
                    operationContext.MessageId = messageHL7.IdentificationId.Extension;
                }

                operationContext.Sender = messageHL7.Sender;
                operationContext.Receiver = messageHL7.Receiver;
                operationContext.CreationTime = messageHL7.CreationTime;

                if (messageHL7.ControlAct != null && messageHL7.ControlAct.ReasonCodes != null)
                {
                    foreach (var item in messageHL7.ControlAct.ReasonCodes)
                    {
                        if (OId.Compare(item.CodeSystem, HL7Constants.OIds.ReasonCodeId) == 0)
                        {
                            operationContext.ReasonCode = item;
                        }

                        if (OId.Compare(item.CodeSystem, HL7Constants.OIds.ActionCodeId) == 0)
                        {
                            operationContext.ActionCode = item;
                        }
                    }

                    HL7MessageControlAct messageControlAct = (HL7MessageControlAct)messageHL7.ControlAct;
                    if (messageControlAct != null)
                    {
                        ICollection<HL7AuthorOrPerformer> authorOrPerformer = messageControlAct.AuthorOrPerformers;
                        operationContext.AuthorOrPerformers = authorOrPerformer;
                        operationContext.GetDataEnterers = messageControlAct.DataEnterers;
                        operationContext.GetOverseers = messageControlAct.Overseer;
                    }

                    HL7QueryControlAcknowledgement queryControlAck = messageHL7.ControlAct as HL7QueryControlAcknowledgement;
                    if (queryControlAck != null)
                    {
                        operationContext.QueryAcknowledgement = queryControlAck.QueryAcknowledgement;
                        operationContext.GetOverseers = queryControlAck.Overseer;
                        operationContext.GetDataEnterers = queryControlAck.DataEnterers;
                    }

                    if (messageHL7.AttentionLineCollection != null && messageHL7.AttentionLineCollection.Count > 0)
                    {
                        operationContext.GetAttentionLine = messageHL7.AttentionLineCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes a reply message from a specified message version, array of parameters, and a return value.
        /// </summary>
        /// <param name="messageVersion">The SOAP message version.</param>
        /// <param name="parameters">The out parameters.</param>
        /// <param name="result">The return value.</param>
        /// <returns>
        /// The serialized reply message.
        /// </returns>
        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            if (this.attribute == null)
            {
                throw new ArgumentException(SrProtocol.HL7AtributesIsNotSet);
            }

            if (string.IsNullOrEmpty(this.attribute.ReplyTemplate) || !UrnType.IsUrn(this.attribute.ReplyTemplate))
            {
                if (string.IsNullOrEmpty(this.attribute.ReplyInteraction))
                {
                    throw new FormatException(SrProtocol.IncorrectAction);
                }
                else
                {
                    if (string.IsNullOrEmpty(this.attribute.ReplyTemplate))
                    {
                        if (Helper.CheckSchema(this.attribute.ReplyInteraction) == Helper.SchemaType.Lvext || Helper.CheckSchema(this.attribute.ReplyInteraction) == Helper.SchemaType.Multicacheschemas)
                        {
                            this.attribute.ReplyTemplate = Helper.GetUrnType(this.attribute.ReplyInteraction, this.attribute.Version).ToString();
                        }
                        else
                        {
                            throw new FormatException(SrProtocol.IncorrectAction);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(this.attribute.ReplyTemplate))
            {
                throw new ArgumentNullException(SrProtocol.IncorrectAction);
            }

            string interactionId = this.attribute.ReplyInteraction;

            // UrnType templateId = new UrnType(this.attribute.ReplyTemplate);
            if (OperationContext.Current == null)
            {
                throw new OperationCanceledException(SrProtocol.OperationContextIsNotSet);
            }

            var operationContext = HL7OperationContext.Current;

            HL7Device sender;

            // if (operationContext.Sender != null)
            if (this.attribute.Sender != null)
            {
                sender = new HL7Device(this.attribute.Sender, HL7Constants.AttributesValue.Sender);
            }
            else
            {
                if (operationContext.Receiver != null)
                {

                    sender = new HL7Device(operationContext.Receiver.Id.Extension, HL7Constants.AttributesValue.Sender);
                }
                else
                {
                    sender = new HL7Device("Unknown", HL7Constants.AttributesValue.Sender);
                }
            }

            HL7Device receiver;
            if (this.attribute.Receiver != null)
            {
                receiver = new HL7Device(this.attribute.Receiver, HL7Constants.AttributesValue.Receiver);
            }
            else
            {
                if (operationContext.Sender != null)
                {
                    receiver = new HL7Device(operationContext.Sender.Id.Extension, HL7Constants.AttributesValue.Receiver);
                }
                else
                {
                    receiver = new HL7Device("Unknown", HL7Constants.AttributesValue.Receiver);
                }
            }

            if (this.attribute.GetsActionCode == null)
            {
                this.attribute.GetsActionCode = operationContext.ActionCode.Code;
            }

            if (this.attribute.GetsReasonCode == null)
            {
                this.attribute.GetsReasonCode = operationContext.ReasonCode.Code;
            }

            HL7TransmissionWrapper response = this.CreateResponse(operationContext, interactionId, receiver, sender, this.attribute.GetsActionCode, this.attribute.GetsReasonCode, this.attribute.ControlActDescription, this.attribute.PriorityCode, this.attribute.LanguageCode, this.queryRequest, result);

            if (response != null && response.AttentionLineCollection != null && response.AttentionLineCollection.Count > 0)
            {
                operationContext.GetAttentionLine = response.AttentionLineCollection;
            }

            if (response == null || response.Acknowledgement == null || response.Acknowledgement.TargetMessage == null || response.Acknowledgement.TargetMessage.Extension == null)
            {
                throw new ArgumentException("Acknowledgement identification is not set {response.Acknowledgement.TargetMessage.Extension}");
            }
            else
            {
                if (OperationContext.Current != null && operationContext.TargetMessage != null && operationContext.TargetMessage != Guid.Empty)
                {
                    response.Acknowledgement.TargetMessage.Extension = operationContext.TargetMessage;
                }
                else
                {
                    response.Acknowledgement.TargetMessage.Extension = operationContext.MessageId;
                }
            }

            if (this.attribute.Sender != null)
            {
                if (response?.Acknowledgement?.AcknowledgementDetails != null && !string.IsNullOrEmpty(sender?.Id?.Extension))
                {
                    foreach (var item in response.Acknowledgement.AcknowledgementDetails)
                    {
                        item.SetNewCode(sender.Id.Extension);
                    }
                }
            }
            else
            {
                if (response?.Acknowledgement?.AcknowledgementDetails != null && !string.IsNullOrEmpty(operationContext.Receiver?.Id?.Extension))
                {
                    foreach (var item in response.Acknowledgement.AcknowledgementDetails)
                    {
                        item.SetNewCode(operationContext.Receiver.Id.Extension);
                    }
                }
            }

            return HL7MessageExtension.CreateHL7Message(messageVersion, interactionId, response);
        }
    }
}

#endif