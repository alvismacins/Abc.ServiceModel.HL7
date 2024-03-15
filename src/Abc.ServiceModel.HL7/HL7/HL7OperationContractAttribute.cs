namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Operation contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HL7OperationContractAttribute : Attribute, IOperationBehavior
    {
        private string actionCodeData;
        private string reasonCodeData;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        public HL7OperationContractAttribute()
            : this(HL7Constants.Versions.HL7Version.HL72011)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version)
            : this(version, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(string actionCode, string reasonCode)
            : this(HL7Constants.Versions.HL7Version.HL72011, actionCode, reasonCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version, ReasonCodes.Action actionCode, ReasonCodes.Reason reasonCode)
            : this(version, TransformToAction(actionCode), TransformToReason(reasonCode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(ReasonCodes.Action actionCode, ReasonCodes.Reason reasonCode)
            : this(HL7Constants.Versions.HL7Version.HL72011, TransformToAction(actionCode), TransformToReason(reasonCode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(ReasonCodes.Action actionCode, string reasonCode)
            : this(HL7Constants.Versions.HL7Version.HL72011, TransformToAction(actionCode), reasonCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(string actionCode, ReasonCodes.Reason reasonCode)
            : this(HL7Constants.Versions.HL7Version.HL72011, actionCode, TransformToReason(reasonCode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version, string actionCode, string reasonCode)
            : this(version, actionCode, reasonCode, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        /// <param name="controlActDescription">The control act description.</param>
        /// <param name="priorityCode">The priority code.</param>
        /// <param name="languageCode">The language code.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version, ReasonCodes.Action actionCode, ReasonCodes.Reason reasonCode, string controlActDescription, string priorityCode, string languageCode)
            : this(version, TransformToAction(actionCode), TransformToReason(reasonCode), controlActDescription, priorityCode, languageCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="controlActDescription">The control act description.</param>
        /// <param name="priorityCode">The priority code.</param>
        /// <param name="languageCode">The language code.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version, string controlActDescription, string priorityCode, string languageCode)
            : this(version, null, null, controlActDescription, priorityCode, languageCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="reasonCode">The reason code.</param>
        /// <param name="controlActDescription">The control act text.</param>
        /// <param name="priorityCode">The priority code.</param>
        /// <param name="languageCode">The language code.</param>
        public HL7OperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version, string actionCode, string reasonCode, string controlActDescription, string priorityCode, string languageCode)
        {
            this.Version = version;

            if (!string.IsNullOrEmpty(actionCode))
            {
                this.GetsActionCode = actionCode;
            }

            if (!string.IsNullOrEmpty(reasonCode))
            {
                this.GetsReasonCode = reasonCode;
            }

            if (!string.IsNullOrEmpty(controlActDescription))
            {
                this.ControlActDescription = controlActDescription;
            }

            if (!string.IsNullOrEmpty(priorityCode))
            {
                this.PriorityCode = priorityCode;
            }

            if (!string.IsNullOrEmpty(languageCode))
            {
                this.LanguageCode = languageCode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ack response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ack response; otherwise, <c>false</c>.
        /// </value>
        public bool AcknowledgementResponse { get; internal set; }

        /// <summary>
        /// Gets the gets action code. Reason code with Oid .2.15
        /// </summary>
        public string GetsActionCode
        {
            get
            {
                return this.actionCodeData;
            }

            internal set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.actionCodeData = value;
                }
                else
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "ActionCode"));
                }
            }
        }

        /// <summary>
        /// Gets Reason code with Oid .2.4
        /// </summary>
        public string GetsReasonCode
        {
            get
            {
                return this.reasonCodeData;
            }

            internal set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.reasonCodeData = value;
                }
                else
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SrProtocol.IsNotSet, "ReasonCode"));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [query param].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [query param]; otherwise, <c>false</c>.
        /// </value>
        public HL7Request.RequestType QueryParameterPayload { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        /// <value>
        /// The receiver.
        /// </value>
        public string Receiver { get; set; }

        /// <summary>
        /// Gets the reply template.
        /// </summary>
        public string ReplyTemplate { get; internal set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public string Sender { get; set; }

        /// <summary>
        /// Gets the template.
        /// </summary>
        public string Template { get; internal set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version Version { get; internal set; }

        /// <summary>
        /// Gets or sets the control act description.
        /// </summary>
        /// <value>
        /// The control act description.
        /// </value>
        internal string ControlActDescription { get; set; }

        /// <summary>
        /// Gets or sets the interaction.
        /// </summary>
        /// <value>
        /// The interaction.
        /// </value>
        internal string Interaction { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
        internal string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the priority code.
        /// </summary>
        /// <value>
        /// The priority code.
        /// </value>
        internal string PriorityCode { get; set; }

        /// <summary>
        /// Gets or sets the reply interaction.
        /// </summary>
        /// <value>
        /// The reply interaction.
        /// </value>
        internal string ReplyInteraction { get; set; }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The collection of objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            var attribute = operationDescription.GetOperationContract<HL7OperationContractAttribute>();
            var outputType = operationDescription.GetReturnType();
            var inputType = operationDescription.GetInputType();

            if (clientOperation != null)
            {
                // Interactions
                if (clientOperation.Action != null)
                {
                    attribute.Interaction = clientOperation.Action.Substring(HL7Constants.Namespace.Length + 1);
                    this.Template = Helper.GetUrnType(attribute.Interaction, attribute.Version);
                }

                if (clientOperation.ReplyAction != null && clientOperation.ReplyAction != "*")
                {
                    attribute.ReplyInteraction = clientOperation.ReplyAction.Substring(HL7Constants.Namespace.Length + 1);
                    this.ReplyTemplate = Helper.GetUrnType(attribute.ReplyInteraction, attribute.Version);
                }
                else if (clientOperation.ReplyAction == "*")
                {
                    attribute.ReplyInteraction = "*";
                }
                else
                {
                    throw new OperationCanceledException("ReplyAction is incorrect");
                }

                // Serializer
                var methodInfo = operationDescription.GetMethodInfo();
                var outputSerializerType = GetSerializerType(methodInfo.GetParameters()[0]);
                var inputSerializerType = GetSerializerType(methodInfo.ReturnTypeCustomAttributes);

                clientOperation.SerializeRequest = true;
                clientOperation.DeserializeReply = true;

                clientOperation.Formatter = new HL7MessageFormatter(attribute, outputType, inputType, inputSerializerType, outputSerializerType);
            }
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            var attribute = operationDescription.GetOperationContract<HL7OperationContractAttribute>();
            var inputType = operationDescription.GetInputType();
            var outputType = operationDescription.GetReturnType();

            // Interactions
            if ( dispatchOperation != null)
            {
                if (dispatchOperation.Action != null)
                {
                    attribute.Interaction = dispatchOperation.Action.Substring(HL7Constants.Namespace.Length + 1);
                    this.Template = Helper.GetUrnType(attribute.Interaction, attribute.Version);
                }
#if NETFRAMEWORK || CoreWCF
                if (dispatchOperation.ReplyAction != null)
                {
                    attribute.ReplyInteraction = dispatchOperation.ReplyAction.Substring(HL7Constants.Namespace.Length + 1);
                    this.ReplyTemplate = Helper.GetUrnType(attribute.ReplyInteraction, attribute.Version);
                }

                // else if (dispatchOperation.ReplyAction== "*")
                // {
                //    attribute.ReplyInteraction = "*";
                // }
                // else
                // {
                //    throw new OperationCanceledException("ReplyAction is incorrect");
                // }
                // Serializer
                var methodInfo = operationDescription.GetMethodInfo();
                var outputSerializerType = GetSerializerType(methodInfo.GetParameters()[0]);
                var inputSerializerType = GetSerializerType(methodInfo.ReturnTypeCustomAttributes);

                dispatchOperation.Formatter = new HL7MessageFormatter(attribute, inputType, outputType, inputSerializerType, outputSerializerType);
#endif
            }
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription)
        {
            // TODO: validation
        }

        private static Type GetSerializerType(ICustomAttributeProvider attributeProvider)
        {
            object[] customAttributes = attributeProvider.GetCustomAttributes(typeof(HL7SubjectSerializerAttribute), false);
            if (customAttributes.Length == 0)
            {
                return null;
            }

            if (customAttributes.Length > 1)
            {
                throw new InvalidOperationException("tooManyAttributesOfTypeOn2"/*SR.GetString("tooManyAttributesOfTypeOn2", new object[] { attrType, attrProvider.ToString() }))*/);
            }

            HL7SubjectSerializerAttribute attribute = (HL7SubjectSerializerAttribute)customAttributes[0];

            // Detect Serializer Type
            switch (attribute.Serializer)
            {
                case HL7SubjectSerializerTypes.XmlSerializer:
                    return typeof(XmlSerializerObjectSerializer);
                case HL7SubjectSerializerTypes.DataContractSerializer:
                    return typeof(DataContractSerializer);
                case HL7SubjectSerializerTypes.Custom:
                    return attribute.CustomSerializerType;
            }

            return null;
        }

        /// <summary>
        /// Transforms to action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Transformed To Action</returns>
        private static string TransformToAction(ReasonCodes.Action action)
        {
            switch (action)
            {
                case ReasonCodes.Action.Read:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Read;
                case ReasonCodes.Action.Request:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Request;
                case ReasonCodes.Action.Response:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Response;
                case ReasonCodes.Action.Select:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Select;
                case ReasonCodes.Action.Write:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Write;
                default:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ActionCodes.Read;
            }
        }

        private static string TransformToReason(ReasonCodes.Reason reason)
        {
            switch (reason)
            {
                case ReasonCodes.Reason.ControlAndInspection:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.ControlAndInspection;
                case ReasonCodes.Reason.DrugTreatment:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.DrugTreatment;
                case ReasonCodes.Reason.DueRecordOrRefferal:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.DueRecordOrRefferal;
                case ReasonCodes.Reason.Emergency:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.Emergency;
                case ReasonCodes.Reason.HealthCareAdministration:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.HealthCareAdministration;
                case ReasonCodes.Reason.InMedicalTreatment:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.InMedicalTreatment;
                case ReasonCodes.Reason.OnPatientRequest:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.OnPatientRequest;
                case ReasonCodes.Reason.Other:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.Other;
                case ReasonCodes.Reason.ScentificResearch:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.ScentificResearch;
                case ReasonCodes.Reason.WithPatientAgreement:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.WithPatientAgreement;
                default:
                    return Abc.ServiceModel.Protocol.HL7.HL7Constants.ReasonCodes.ControlAndInspection;
            }
        }
    }
}