#if NETFRAMEWORK || CoreWCF

namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Globalization;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 message formmater.
    /// </summary>
    public partial class HL7SaveResponseMessageFormatter : IDispatchMessageFormatter
    {
        /// <summary>
        /// Deserializes a message into an array of parameters.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <param name="parameters">The objects that are passed to the operation as parameters.</param>
        public void DeserializeRequest(Message message, object[] parameters)
        {
            if (parameters == null) {  throw new ArgumentNullException("parameters", "parameters != null"); }
            if (!(parameters.Length > 0)) {  throw new ArgumentNullException("parameters", "parameters.Length > 0"); }

            // Set operation Context
            var operationContext = new HL7OperationContext();
            operationContext.OperationSaveContract = this.attribute;
            OperationContext.Current.Extensions.Add(operationContext);
            var messageHL7 = HL7MessageExtension.ReadHL7Message(message, this.attribute.Interaction);

            if (messageHL7 is HL7Request)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotAllowed, "request"));
            }

            if (messageHL7 is HL7AcknowledgementResponse)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotAllowed, "HL7AcknowledgementResponse"));
            }

            // string interactionId = this.attribute.ReplyInteraction;
            // string templateId = this.attribute.ReplyTemplate;
            // HL7Device sender;
            // if (this.attribute.Sender != null)
            // {
            //    sender = new HL7Device(this.attribute.Sender, HL7Constants.AttributesValue.Sender);
            // }
            // else
            // {
            //    sender = operationContext.Sender;
            // }
            // HL7Device receiver;
            // if (this.attribute.Receiver != null)
            // {
            //    receiver = new HL7Device(this.attribute.Receiver, HL7Constants.AttributesValue.Receiver);
            // }
            // else
            // {
            //    receiver = operationContext.Receiver;
            // }
            var responseMess = messageHL7 as HL7ApplicationResponse;

            if (responseMess != null)
            {
                parameters[0] = responseMess.ControlAct.Subject.GetBody(
                    this.CreateInputSerializer(
                        this.parameterType,
                        HL7Request.RequestType.MessageRequest,
                        rootName: responseMess.ControlAct.Subject.SubjectElementName,
                        rootNamespace: HL7Constants.Namespace));
            }
            else
            {
                var responseMess2 = messageHL7 as HL7QueryApplicationResponse;

                if (responseMess2 != null)
                {
                    parameters[0] = responseMess2.ControlAct.Subject.GetBody(
                        this.CreateInputSerializer(
                            this.parameterType, 
                            HL7Request.RequestType.MessageRequest,
                            rootName: responseMess2.ControlAct.Subject.SubjectElementName,
                            rootNamespace: HL7Constants.Namespace));
                }
                else
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotAllowed, "unknown type"));
                }
            }

            operationContext.MessageId = messageHL7.IdentificationId.Extension;
            operationContext.Sender = messageHL7.Sender;
            operationContext.Receiver = messageHL7.Receiver;
            operationContext.CreationTime = messageHL7.CreationTime;
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
            string interactionId = this.attribute.ReplyInteraction;

            var operationContext = HL7OperationContext.Current;

            HL7Device sender;
            if (this.attribute.Sender != null)
            {
                sender = new HL7Device(this.attribute.Sender, HL7Constants.AttributesValue.Sender);
            }
            else
            {
                sender = operationContext.Sender;
            }

            HL7Device receiver;
            if (this.attribute.Receiver != null)
            {
                receiver = new HL7Device(this.attribute.Receiver, HL7Constants.AttributesValue.Receiver);
            }
            else
            {
                receiver = operationContext.Receiver;
            }

            HL7TransmissionWrapper response = this.CreateResponse(operationContext, interactionId, receiver, sender, null);

            // switch (this.queryRequest)
            // {
            //    case HL7Request.RequestType.QueryParamRequest:
            //        response = CreateQueryResponse(operationContext, interactionId, receiver, sender, "ActionCode", "ReasonCode", result);
            //        break;
            //    case HL7Request.RequestType.MessageRequest:
            //        response = CreateMessageResponse(operationContext, interactionId, receiver, sender, "ActionCode", "ReasonCode", result);
            //        //response = CreateMessageResponse(operationContext, interactionId, receiver, sender, this.attribute.ActionCode, this.attribute.ReasonCode, result);
            //        break;
            //    case HL7Request.RequestType.QueryContinuationRequest:
            //        response = CreateQueryResponse(operationContext, interactionId, receiver, sender, "ActionCode", "ReasonCode", result);
            //        //response = CreateQueryResponse(operationContext, interactionId, receiver, sender, this.attribute.ActionCode, this.attribute.ReasonCode, result);
            //        break;
            //    default:
            //        response = CreateMessageResponse(operationContext, interactionId, receiver, sender, "ActionCode", "ReasonCode", result);
            //        //response = CreateMessageResponse(operationContext, interactionId, receiver, sender, this.attribute.ActionCode, this.attribute.ReasonCode, result);
            //        break;
            // }
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

            return HL7MessageExtension.CreateHL7Message(messageVersion, interactionId, response);
        }
    }
}

#endif