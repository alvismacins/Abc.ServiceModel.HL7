namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Abc.ServiceModel.Protocol.HL7;
    using System.Linq;
    public partial class HL7MessageFormatter : IDispatchMessageFormatter, IClientMessageFormatter
    {
        private HL7OperationContractAttribute attribute;
        private Type inputSerializerType;
        private Type outputParameterType;
        private Type outputSerializerType;
        private Type parameterType;
        private HL7Request.RequestType queryRequest;
        private string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7MessageFormatter"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="parameterType">Type of the input.</param>
        /// <param name="outputParameterType">Type of the output parameter.</param>
        /// <param name="inputSerializerType">Type of the input serializer.</param>
        /// <param name="outputSerializerType">Type of the output serializer.</param>
        internal HL7MessageFormatter(HL7OperationContractAttribute attribute, Type parameterType, Type outputParameterType, Type inputSerializerType, Type outputSerializerType)
        {
            if (attribute == null) {  throw new ArgumentNullException("attribute", "attribute != null"); }

            this.parameterType = parameterType;
            this.outputParameterType = outputParameterType;
            this.inputSerializerType = inputSerializerType;
            this.outputSerializerType = outputSerializerType;
            this.attribute = attribute;
            this.queryRequest = HL7Request.RequestType.MessageRequest;

            switch (attribute.Version)
            {
                case HL7Constants.Versions.HL7Version.HL72006:
                    this.version = HL7Constants.Versions.V3NE2006;
                    break;

                case HL7Constants.Versions.HL7Version.HL72011:
                    this.version = HL7Constants.Versions.V3NE2011;
                    break;

                default:
                    this.version = HL7Constants.Versions.V3NE2011;
                    break;
            }
        }

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

            if (this.attribute == null)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "Attributes"));
            }

            this.queryRequest = this.attribute.QueryParameterPayload;

            switch (this.attribute.QueryParameterPayload)
            {
                case HL7Request.RequestType.QueryParamRequest:

                    if (request == null || request.QueryControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.QueryControlAct"));
                    }

                    parameters[0] = request.QueryControlAct.QueryByParameterPayload.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.QueryParamRequest));
                    break;

                case HL7Request.RequestType.MessageRequest:

                    if (request == null || request.ControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.ControlAct"));
                    }

                    var param = this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.MessageRequest);

                    if (request != null && request.ControlAct != null && request.ControlAct.Subject != null)
                    {
                        parameters[0] = request.ControlAct.Subject.GetBody(param);
                    }

                    break;

                case HL7Request.RequestType.QueryContinuationRequest:

                    if (request == null || request.QueryControlAct == null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "request.QueryControlAct"));
                    }

                    parameters[0] = request.QueryControlAct.QueryContinuation.GetBody(this.CreateInputSerializer(this.parameterType, HL7Request.RequestType.QueryContinuationRequest));
                    break;

                default:

                    if (request == null || request.ControlAct == null)
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

        private static XmlObjectSerializer CreateSerializer(Type serializerType, Type type)
        {
            if (serializerType != null)
            {
                return HL7SubjectSerializerDefaults.CreateSerializer(serializerType, type);
            }

            return HL7SubjectSerializerDefaults.CreateSerializer(type);
        }

        private static XmlObjectSerializer CreateSerializerQueryContinuation(Type serializerType, Type type)
        {
            if (serializerType != null)
            {
                return HL7QueryContinuationPayloadSerializerDefaults.CreateSerializer(serializerType, type);
            }

            return HL7QueryContinuationPayloadSerializerDefaults.CreateSerializer(type);
        }

        private static XmlObjectSerializer CreateSerializerQueryParamRequest(Type serializerType, Type type)
        {
            if (serializerType != null)
            {
                return HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(serializerType, type);
            }

            return HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(type);
        }

        /*
        [System.Obsolete("obsolte", true)]
        private XmlObjectSerializer CreateOutputSerializer(Type type)
        {
            return CreateSerializer(this.outputSerializerType, type);
        }
        */

        // [System.Obsolete("obsolte", true)]
        // private XmlObjectSerializer CreateInputSerializer(Type type)
        // {
        //    return CreateSerializer(this.inputSerializerType, type);
        // }
        private XmlObjectSerializer CreateInputSerializer(Type type, HL7Request.RequestType subject)
        {
            switch (subject)
            {
                case HL7Request.RequestType.MessageRequest:
                    return CreateSerializer(this.inputSerializerType, type);

                case HL7Request.RequestType.QueryParamRequest:
                    return CreateSerializerQueryParamRequest(this.inputSerializerType, type);

                case HL7Request.RequestType.QueryContinuationRequest:
                    return CreateSerializerQueryContinuation(this.outputSerializerType, type);

                default:
                    return CreateSerializer(this.inputSerializerType, type);
            }
        }

        private XmlObjectSerializer CreateOutputSerializer(Type type, HL7Request.RequestType subject)
        {
            switch (subject)
            {
                case HL7Request.RequestType.MessageRequest:
                    return CreateSerializer(this.outputSerializerType, type);

                case HL7Request.RequestType.QueryParamRequest:
                    return CreateSerializerQueryParamRequest(this.outputSerializerType, type);

                case HL7Request.RequestType.QueryContinuationRequest:
                    return CreateSerializerQueryContinuation(this.outputSerializerType, type);

                default:
                    return CreateSerializer(this.outputSerializerType, type);
            }
        }
    }
}