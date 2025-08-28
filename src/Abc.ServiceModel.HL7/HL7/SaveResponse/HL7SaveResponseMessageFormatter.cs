namespace Abc.ServiceModel.HL7
{
    using Abc.ServiceModel.Protocol.HL7;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// HL7 message formmater.
    /// </summary>
    public partial class HL7SaveResponseMessageFormatter
    {
        private HL7SaveResponseOperationContractAttribute attribute;
        private Type inputSerializerType;
        private Type outputParameterType;
        private Type outputSerializerType;
        private Type parameterType;

        // private HL7Request.RequestType queryRequest;
        private string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7SaveResponseMessageFormatter"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="outputParameterType">Type of the output parameter.</param>
        /// <param name="inputSerializerType">Type of the input serializer.</param>
        /// <param name="outputSerializerType">Type of the output serializer.</param>
        internal HL7SaveResponseMessageFormatter(HL7SaveResponseOperationContractAttribute attribute, Type parameterType, Type outputParameterType, Type inputSerializerType, Type outputSerializerType)
        {
            this.parameterType = parameterType;
            this.outputParameterType = outputParameterType;
            this.inputSerializerType = inputSerializerType;
            this.outputSerializerType = outputSerializerType;
            this.attribute = attribute;

            // this.queryRequest = HL7Request.RequestType.MessageRequest;
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
        /// Creates the serializer.
        /// </summary>
        /// <param name="serializerType">Type of the serializer.</param>
        /// <param name="type">The type.</param>
        /// <returns>XmlObject Serializer</returns>
        private static XmlObjectSerializer CreateSerializer(Type serializerType, Type type, string rootName, string rootNamespace)
        {
            if (serializerType != null)
            {
                return HL7SubjectSerializerDefaults.CreateSerializer(serializerType, type, rootName: rootName, rootNamespace: rootNamespace);
            }

            return HL7SubjectSerializerDefaults.CreateSerializer(type, rootName: rootName, rootNamespace: rootNamespace);
        }

        /// <summary>
        /// Creates the serializer query continuation.
        /// </summary>
        /// <param name="serializerType">Type of the serializer.</param>
        /// <param name="type">The type.</param>
        /// <returns>XmlObject Serializer</returns>
        private static XmlObjectSerializer CreateSerializerQueryContinuation(Type serializerType, Type type, string rootName, string rootNamespace)
        {
            if (serializerType != null)
            {
                return HL7QueryContinuationPayloadSerializerDefaults.CreateSerializer(serializerType, type, rootName: rootName, rootNamespace: rootNamespace);
            }

            return HL7QueryContinuationPayloadSerializerDefaults.CreateSerializer(type, rootName: rootName, rootNamespace: rootNamespace);
        }

        /// <summary>
        /// Creates the serializer query param request.
        /// </summary>
        /// <param name="serializerType">Type of the serializer.</param>
        /// <param name="type">The type.</param>
        /// <returns>XmlObject Serializer</returns>
        private static XmlObjectSerializer CreateSerializerQueryParamRequest(Type serializerType, Type type, string rootName, string rootNamespace)
        {
            if (serializerType != null)
            {
                return HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(serializerType, type, rootName: rootName, rootNamespace: rootNamespace);
            }

            return HL7QueryByParameterPayloadSerializerDefaults.CreateSerializer(type, rootName: rootName, rootNamespace: rootNamespace);
        }

        [System.Obsolete("obsolte", true)]
        private XmlObjectSerializer CreateInputSerializer(Type type, string rootName, string rootNamespace)
        {
            return CreateSerializer(this.inputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
        }

        private XmlObjectSerializer CreateInputSerializer(Type type, HL7Request.RequestType subject, string rootName, string rootNamespace)
        {
            switch (subject)
            {
                case HL7Request.RequestType.MessageRequest:
                    return CreateSerializer(this.inputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                case HL7Request.RequestType.QueryParamRequest:
                    return CreateSerializerQueryParamRequest(this.inputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                case HL7Request.RequestType.QueryContinuationRequest:
                    return CreateSerializerQueryContinuation(this.outputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                default:
                    return CreateSerializer(this.inputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
            }
        }

        private XmlObjectSerializer CreateOutputSerializer(Type type, HL7Request.RequestType subject, string rootName, string rootNamespace)
        {
            switch (subject)
            {
                case HL7Request.RequestType.MessageRequest:
                    return CreateSerializer(this.outputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                case HL7Request.RequestType.QueryParamRequest:
                    return CreateSerializerQueryParamRequest(this.outputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                case HL7Request.RequestType.QueryContinuationRequest:
                    return CreateSerializerQueryContinuation(this.outputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
                default:
                    return CreateSerializer(this.outputSerializerType, type, rootName: rootName, rootNamespace: rootNamespace);
            }
        }

        /// <summary>
        /// Creates the message response.
        /// </summary>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="acknowledgementDetail">The acknowledgement detail.</param>
        /// <param name="controlAct">The control act.</param>
        /// <returns>
        /// Message Response
        /// </returns>
        private HL7TransmissionWrapper CreateResponse(string interactionId, HL7Device receiver, HL7Device sender, HL7Acknowledgement acknowledgementDetail, HL7ControlAct controlAct)
        {
            HL7TransmissionWrapper response;

            if (this.attribute.AcknowledgementResponse || this.outputParameterType == typeof(void) || controlAct == null)
            {
                response = new HL7AcknowledgementResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, acknowledgementDetail);
            }
            else
            {
                response = new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct,null, acknowledgementDetail);

                if (controlAct is HL7QueryControlAcknowledgement)
                {
                    return response = new HL7QueryApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, acknowledgementDetail);
                }

                if (controlAct is HL7MessageControlAct)
                {
                    response = new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, null, acknowledgementDetail);
                }
                else
                {
                    throw new InvalidOperationException("can not create Response");
                }
            }

            return response;
        }

        /// <summary>
        /// Creates the message response.
        /// </summary>
        /// <param name="operationContext">The operation context.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="controlAct">The control act.</param>
        /// <returns>
        /// Message Response
        /// </returns>
        private HL7TransmissionWrapper CreateResponse(HL7OperationContext operationContext, string interactionId, HL7Device receiver, HL7Device sender, HL7ControlAct controlAct)
        {
            HL7TransmissionWrapper response;

            if (this.attribute.AcknowledgementResponse || this.outputParameterType == typeof(void) || controlAct == null)
            {
                return new HL7AcknowledgementResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, new HL7Acknowledgement(operationContext.MessageId, HL7AcknowledgementType.AcceptAcknowledgementCommitAccept, operationContext.AcknowledgementDetail));
            }
            else
            {
                if (controlAct is HL7QueryControlAcknowledgement)
                {
                    return response = new HL7QueryApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct, new HL7Acknowledgement(operationContext.MessageId, HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                }

                if (controlAct is HL7MessageControlAct)
                {
                    return response = new HL7ApplicationResponse(interactionId, this.version, receiver.Id.Extension, sender.Id.Extension, controlAct,null, new HL7Acknowledgement(operationContext.MessageId, HL7AcknowledgementType.ApplicationAcknowledgementAccept, operationContext.AcknowledgementDetail)); // TODO: constructor with device
                }
                else
                {
                    throw new InvalidOperationException("can not create Response");
                }
            }
        }
    }
}
