// ----------------------------------------------------------------------------
// <copyright file="HL7MessageCreator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using Abc.ServiceModel.Protocol.HL7;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// General MessageCreator
    /// </summary>
    public partial class HL7MessageFormatter
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
            if (attribute == null) { throw new ArgumentNullException("attribute", "attribute != null"); }

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

        private Message CreateMessageRequest(object body, string interactionId, string deviceSender, string deviceReceiver, string actionCode, string reasonCode, HL7Request.RequestType requestType, MessageVersion messageVersion)
        {
            return this.CreateMessageRequest(body, interactionId, deviceSender, deviceReceiver, actionCode, reasonCode, null, null, null, requestType, messageVersion);
        }

        private Message CreateMessageRequest(object body, string interactionId, string deviceSender, string deviceReceiver, string actionCode, string reasonCode, string controlActDescription, string priorityCode, string languageCode, HL7Request.RequestType requestType, MessageVersion messageVersion)
        {
            // if (string.IsNullOrEmpty(actionCode))
            // {
            //    throw new FormatException(SrProtocol.ActionCodeIsNotSet);
            // }

            // if (string.IsNullOrEmpty(reasonCode))
            // {
            //    throw new FormatException(SrProtocol.ResonCodeIsNotSet);
            // }
            HL7ClassificatorId languageCodeClasif = null;

            if (!string.IsNullOrEmpty(languageCode))
            {
                languageCodeClasif = new HL7ClassificatorId(languageCode, HL7Constants.OIds.LanguageOId);
            }

            HL7ClassificatorId priorityCodeClassif = null;

            if (!string.IsNullOrEmpty(priorityCode))
            {
                priorityCodeClassif = new HL7ClassificatorId(priorityCode, HL7Constants.OIds.LanguageOId);
            }

            ICollection<HL7DataEnterer> dataEnterers = null;
            ICollection<HL7Overseer> overseers = null;
            if (HL7OperationContext.Current != null)
            {
                dataEnterers = HL7OperationContext.Current.SetDataEnterers;
                overseers = HL7OperationContext.Current.SetOverseers;
            }

            HL7ControlAct controlAct;

            switch (requestType)
            {
                case HL7Request.RequestType.MessageRequest:
                    controlAct = new HL7MessageControlAct(overseers, dataEnterers, null, null, null, controlActDescription, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(body, this.CreateOutputSerializer(body.GetType(), HL7Request.RequestType.MessageRequest)));
                    break;

                case HL7Request.RequestType.QueryParamRequest:
                    controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, null, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7QueryByParameterPayload.CreateQueryByParameterPayload(body, this.CreateOutputSerializer(body.GetType(), requestType)));
                    break;

                case HL7Request.RequestType.QueryContinuationRequest:
                    controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, null, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7QueryContinuation.CreateQueryContinuation(body, this.CreateOutputSerializer(body.GetType(), requestType)));
                    break;

                default:
                    controlAct = new HL7MessageControlAct(overseers, dataEnterers, null, null, null, controlActDescription, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(body, this.CreateOutputSerializer(body.GetType(), HL7Request.RequestType.MessageRequest)));
                    break;
            }

            HL7Request request = new HL7Request(interactionId, this.version, deviceSender, deviceReceiver, controlAct);

            // if (!string.IsNullOrEmpty(this.attribute.ReplyTo))
            // {
            //    Uri replyTo = new Uri(this.attribute.ReplyTo);
            //    return HL7MessageExtension.CreateHL7Message(messageVersion, interactionId, request, replyTo);
            // }
            return HL7MessageExtension.CreateHL7Message(messageVersion, interactionId, request);
        }

        private HL7TransmissionWrapper CreateResponse(HL7OperationContext operationContext, string interactionId, HL7Device receiver, HL7Device sender, string actionCode, string reasonCode, HL7Request.RequestType requestType, object result)
        {
            return this.CreateResponse(operationContext, interactionId, receiver, sender, actionCode, reasonCode, null, null, null, requestType, result);
        }

        private HL7TransmissionWrapper CreateResponse(HL7OperationContext operationContext, string interactionId, HL7Device receiver, HL7Device sender, string actionCode, string reasonCode, string controlActDescription, string priorityCode, string languageCode, HL7Request.RequestType requestType, object result)
        {
            ICollection<HL7DataEnterer> dataEnterers = null;
            ICollection<HL7Overseer> overseers = null;
            HL7AcknowledgementType? acknowledgementType = null;
            ICollection<HL7AttentionLine> attentionLine = null;
            if (HL7OperationContext.Current != null)
            {
                dataEnterers = HL7OperationContext.Current.SetDataEnterers;
                overseers = HL7OperationContext.Current.SetOverseers;
                acknowledgementType = HL7OperationContext.Current.SetAcknowledgementTypeInResponse;
                attentionLine = HL7OperationContext.Current.SetAttentionLine;
            }

            if (this.attribute.AcknowledgementResponse || this.outputParameterType == typeof(void)
#if NET45_OR_GREATER
                || this.outputParameterType == typeof(Task)
#endif
                )
            {
                /*
                return new HL7AcknowledgementResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.AcceptAcknowledgementCommitAccept, operationContext.AcknowledgementDetail));*/

                return new HL7AcknowledgementResponse(new HL7TemplateId(Helper.GetUrnType(interactionId, this.version)), new HL7IdentificationId(), version, DateTime.Now, new HL7InteractionId(interactionId), HL7ProcessingCode.Production, HL7ProcessingModeCode.OperationData, HL7AcceptAcknowledgementCode.Always, new HL7Device(sender.Id.Extension, HL7Constants.AttributesValue.Sender), new HL7Device(receiver.Id.Extension, HL7Constants.AttributesValue.Receiver), attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.AcceptAcknowledgementCommitAccept, operationContext.AcknowledgementDetail));
            }
            else
            {
                if (string.IsNullOrEmpty(actionCode))
                {
                    throw new FormatException(SrProtocol.ActionCodeIsNotSet);
                }

                if (string.IsNullOrEmpty(reasonCode))
                {
                    throw new FormatException(SrProtocol.ResonCodeIsNotSet);
                }

                HL7ClassificatorId languageCodeClasif = null;

                if (!string.IsNullOrEmpty(languageCode))
                {
                    languageCodeClasif = new HL7ClassificatorId(languageCode, HL7Constants.OIds.LanguageOId);
                }

                HL7ClassificatorId priorityCodeClassif = null;

                if (!string.IsNullOrEmpty(priorityCode))
                {
                    priorityCodeClassif = new HL7ClassificatorId(priorityCode, HL7Constants.OIds.LanguageOId);
                }



                HL7ControlAct controlAct;

                switch (requestType)
                {
                    case HL7Request.RequestType.MessageRequest:

                        if (result == null)
                        {
                            controlAct = new HL7MessageControlAct(overseers, dataEnterers, null, null, null, controlActDescription, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, null);
                        }
                        else
                        {
                            controlAct = new HL7MessageControlAct(overseers, dataEnterers, null, null, null, controlActDescription, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(result, this.CreateOutputSerializer(result.GetType(), HL7Request.RequestType.MessageRequest)));
                        }

                    // return new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                    return new HL7ApplicationResponse(interactionId, this.version, sender.Id.Extension, receiver.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device

                    case HL7Request.RequestType.QueryParamRequest:

                        if (result == null)
                        {
                            controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, DateTime.Now, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, operationContext.QueryAcknowledgement);
                        }
                        else
                        {
                            controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, DateTime.Now, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(result, this.CreateOutputSerializer(result.GetType(), HL7Request.RequestType.MessageRequest)), operationContext.QueryAcknowledgement);
                        }

                    //     return new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device

                        return new HL7ApplicationResponse(interactionId, this.version, sender.Id.Extension, receiver.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail));

                    case HL7Request.RequestType.QueryContinuationRequest:

                        if (result == null)
                        {
                            controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, DateTime.Now, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, operationContext.QueryAcknowledgement);
                        }
                        else
                        {
                            controlAct = new HL7QueryControlAcknowledgement(overseers, dataEnterers, null, null, null, controlActDescription, DateTime.Now, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(result, this.CreateOutputSerializer(result.GetType(), HL7Request.RequestType.MessageRequest)), operationContext.QueryAcknowledgement);
                        }

                        //   return new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                        return new HL7ApplicationResponse(interactionId, this.version, sender.Id.Extension, receiver.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail));

                    default:
                        if (result == null)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "return parameter"));
                        }

                        controlAct = new HL7MessageControlAct(overseers, dataEnterers, null, null, null, controlActDescription, priorityCodeClassif, actionCode, reasonCode, languageCodeClasif, HL7Subject.CreateSubject(result, this.CreateOutputSerializer(result.GetType(), HL7Request.RequestType.MessageRequest)));

                        // return new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                        return new HL7ApplicationResponse(interactionId, this.version, sender.Id.Extension, receiver.Id.Extension, controlAct, attentionLine, new HL7Acknowledgement(operationContext.MessageId, acknowledgementType != null && acknowledgementType.HasValue ? acknowledgementType.Value : HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                }
            }
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