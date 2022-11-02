namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Operation contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HL7SaveResponseOperationContractAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7SaveResponseOperationContractAttribute"/> class.
        /// </summary>
        public HL7SaveResponseOperationContractAttribute()
        {
            this.Version = HL7Constants.Versions.HL7Version.HL72011;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7SaveResponseOperationContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public HL7SaveResponseOperationContractAttribute(Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version)
        {
            this.Version = version;
        }

        /// <summary>
        /// Gets or sets a value indicating whether ack response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ack response; otherwise, <c>false</c>.
        /// </value>
        public bool AcknowledgementResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [query param].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [query param]; otherwise, <c>false</c>.
        /// </value>
        public HL7Request.RequestType QueryParameterPayload { get; set; }

        /// <summary>
        /// Gets the receiver.
        /// </summary>
        public string Receiver { get; internal set; }

        /// <summary>
        /// Gets the reply template.
        /// </summary>
        public string ReplyTemplate { get; private set; }

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
        public string Template { get; private set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version Version { get; set; }

        /// <summary>
        /// Gets or sets the interaction.
        /// </summary>
        /// <value>
        /// The interaction.
        /// </value>
        internal string Interaction { get; set; }

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
            if (operationDescription == null) {  throw new ArgumentNullException("operationDescription", "operationDescription != null"); }
            if (!(operationDescription.DeclaringContract != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.DeclaringContract != null"); }
            if (!(operationDescription.DeclaringContract.Operations != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.DeclaringContract.Operations != null"); }
            if (!(operationDescription.DeclaringContract.Operations.Count > 0)) {  throw new ArgumentException("operationDescription", "operationDescription.DeclaringContract.Operations.Count > 0"); }
            if (!(operationDescription.Messages != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.Messages != null"); }
            if (!(operationDescription.Messages.Count > 0)) {  throw new ArgumentException("operationDescription", "operationDescription.Messages.Count > 0"); }

            var attribute = operationDescription.DeclaringContract.Operations[0].Behaviors.Find<HL7SaveResponseOperationContractAttribute>();
            var outputType = operationDescription.SyncMethod.ReturnType;

            Type inputType = null;
            if (operationDescription != null && operationDescription.Messages != null && operationDescription.Messages.Count > 0 && operationDescription.Messages[0].Body != null && operationDescription.Messages[0].Body.Parts != null && operationDescription.Messages[0].Body.Parts != null && operationDescription.Messages[0].Body.Parts.Count > 0)
            {
                inputType = operationDescription.Messages[0].Body.Parts[0].Type;
            }

            if (clientOperation != null)
            {
                // Interactions
                if (clientOperation.Action != null)
                {
                    attribute.Interaction = clientOperation.Action.Substring(HL7Constants.Namespace.Length + 1);
                    this.Template = Helper.GetUrnType(attribute.Interaction, attribute.Version);
                }

                if (clientOperation.ReplyAction != null)
                {
                    attribute.ReplyInteraction = clientOperation.ReplyAction.Substring(HL7Constants.Namespace.Length + 1);
                    this.ReplyTemplate = Helper.GetUrnType(attribute.ReplyInteraction, attribute.Version);
                }

                // Serializer
                var outputSerializerType = GetSerializerType(operationDescription.SyncMethod.GetParameters()[0]);
                var inputSerializerType = GetSerializerType(operationDescription.SyncMethod.ReturnTypeCustomAttributes);

                clientOperation.SerializeRequest = true;
                clientOperation.DeserializeReply = true;
                clientOperation.Formatter = new HL7SaveResponseMessageFormatter(attribute, outputType, inputType, inputSerializerType, outputSerializerType);
            }
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            if (operationDescription == null) {  throw new ArgumentNullException("operationDescription", "operationDescription != null"); }
            if (!(operationDescription.DeclaringContract != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.DeclaringContract != null"); }
            if (!(operationDescription.DeclaringContract.Operations != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.DeclaringContract.Operations != null"); }
            if (!(operationDescription.DeclaringContract.Operations.Count > 0)) {  throw new ArgumentException("operationDescription", "operationDescription.DeclaringContract.Operations.Count > 0"); }
            if (!(operationDescription.Messages != null)) {  throw new ArgumentNullException("operationDescription", "operationDescription.Messages != null"); }
            if (!(operationDescription.Messages.Count > 0)) {  throw new ArgumentException("operationDescription", "operationDescription.Messages.Count > 0"); }

            var attribute = operationDescription.DeclaringContract.Operations[0].Behaviors.Find<HL7SaveResponseOperationContractAttribute>();
            var inputType = operationDescription.Messages[0].Body.Parts[0].Type;
            var outputType = operationDescription.SyncMethod.ReturnType;

            // Interactions
            if (dispatchOperation != null)
            {
                if (dispatchOperation.Action != null)
                {
                    attribute.Interaction = dispatchOperation.Action.Substring(HL7Constants.Namespace.Length + 1);

                    this.Template = Helper.GetUrnType(attribute.Interaction, attribute.Version);
                }

                if (dispatchOperation.ReplyAction != null)
                {
                    attribute.ReplyInteraction = dispatchOperation.ReplyAction.Substring(HL7Constants.Namespace.Length + 1);
                    this.ReplyTemplate = Helper.GetUrnType(attribute.ReplyInteraction, attribute.Version);
                }

                // Serializer
                var inputSerializerType = GetSerializerType(operationDescription.SyncMethod.GetParameters()[0]);
                var outputSerializerType = GetSerializerType(operationDescription.SyncMethod.ReturnTypeCustomAttributes);

                dispatchOperation.Formatter = new HL7SaveResponseMessageFormatter(attribute, inputType, outputType, inputSerializerType, outputSerializerType);
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
    }
}