namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Service Operation context. Extension class for keeping instances of object stored in the
    /// current OperationContext.
    /// </summary>
    public class HL7OperationContext : IExtension<OperationContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7OperationContext"/> class.
        /// </summary>
        public HL7OperationContext()
        {
        }

        /// <summary>
        /// Gets the current HL7 Operation context.
        /// </summary>
        public static HL7OperationContext Current
        {
            get
            {
                if (OperationContext.Current != null && OperationContext.Current.Extensions != null)
                {
                    return OperationContext.Current.Extensions.Find<HL7OperationContext>();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the details. //DeserializeReply
        /// </summary>
        /// <value> operationContext.AcknowledgementDetail = messageHl7.Acknowledgement.AcknowledgementDetails </value>
        public ICollection<HL7AcknowledgementDetail> AcknowledgementDetail { get; internal set; }

        /// <summary>
        /// Gets the type of the acknowledgement. //DeserializeReply
        /// </summary>
        /// <value> operationContext.AcknowledgementType = messageHl7.Acknowledgement.AcknowledgementDataType </value>
        public HL7AcknowledgementType AcknowledgementType { get; internal set; }

        /// <summary>
        /// Gets or sets gets the action code.
        /// </summary>
        public HL7ClassificatorId ActionCode { get; set; }

        /// <summary>
        /// Gets the author or performer.
        /// </summary>
        /// <value> The author or performer. </value>
        public ICollection<HL7AuthorOrPerformer> AuthorOrPerformers { get; internal set; }

        /// <summary>
        /// Gets the creation time. messageHl7.CreationTime
        /// </summary>
        public DateTime CreationTime { get; internal set; }

        /// <summary>
        /// Gets the data enterers.
        /// </summary>
        /// <value> The data enterers. </value>
        public ICollection<HL7DataEnterer> GetDataEnterers { get; internal set; }

        /// <summary>
        /// Gets the get overseers.
        /// </summary>
        /// <value> The get overseers. </value>
        public ICollection<HL7Overseer> GetOverseers
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the get attention line.
        /// </summary>
        /// <value>
        /// The get attention line.
        /// </value>
        public ICollection<HL7AttentionLine> GetAttentionLine
        {
            get;
            internal set;
        }

        /// <summary>
        /// Sets the set attention line.
        /// </summary>
        /// <value>
        /// The set attention line.
        /// </value>
        public ICollection<HL7AttentionLine> SetAttentionLine
        {
            internal get;
            set;
        }

        /// <summary>
        /// Gets the message id. messageHl7.IdentificationId.Extension
        /// </summary>
        public Guid MessageId { get; internal set; }

        /// <summary>
        /// Gets or sets the query ack.
        /// </summary>
        /// <value> This value is by user in server side </value>
        public HL7QueryAcknowledgement QueryAcknowledgement { get; set; }

        /// <summary>
        /// Gets or sets the reason code.
        /// </summary>
        /// <value> The reason code. </value>
        public HL7ClassificatorId ReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        /// <value> messageHl7. Receiver </value>
        public HL7Device Receiver { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value> messageHl7. Sender </value>
        public HL7Device Sender { get; set; }

        /// <summary>
        /// Sets the set acknowledgment type in response.
        /// </summary>
        /// <value>
        /// The set acknowledgment type in response.
        /// </value>
        // TODO: fix typo
        public HL7AcknowledgementType? SetAcknowledgementTypeInResponse { internal get; set; }

        /// <summary>
        /// Sets the set data enterers.
        /// </summary>
        /// <value>
        /// The set data enterers.
        /// </value>
        public ICollection<HL7DataEnterer> SetDataEnterers { internal get; set; }

        /// <summary>
        /// Sets gets the author or performer.
        /// </summary>
        /// <value> The author or performer. </value>
        public ICollection<HL7Overseer> SetOverseers
        {
            internal get;
            set;
        }

        /// <summary>
        /// Gets the target message. response.Acknowledgement.TargetMessage.Extension = operationContext.MessageId
        /// </summary>
        public Guid TargetMessage { get; internal set; }

        /// <summary>
        /// Gets or sets the operation contract.
        /// </summary>
        /// <value> The operation contract. </value>
        internal HL7OperationContractAttribute OperationContract { get; set; }

        /// <summary>
        /// Gets or sets the operation save contract.
        /// </summary>
        /// <value> The operation save contract. </value>
        internal HL7SaveResponseOperationContractAttribute OperationSaveContract { get; set; }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">       The error. </param>
        /// <param name="errorNumber"> The error number. </param>
        public void AddError(string error, int errorNumber)
        {
            if (!(!string.IsNullOrEmpty(error))) {  throw new ArgumentNullException("error", "!string.IsNullOrEmpty(error)"); }

            if (this.AcknowledgementDetail == null)
            {
                this.AcknowledgementDetail = new Collection<HL7AcknowledgementDetail>();
            }

            this.AcknowledgementDetail.Add(new HL7AcknowledgementDetail(errorNumber, error, null, HL7AcknowledgementDetailType.Error, this.Sender.Id.Extension));
        }

        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="information">       The information. </param>
        /// <param name="informationNumber"> The information number. </param>
        public void AddInformation(string information, int informationNumber)
        {
            if (!(!string.IsNullOrEmpty(information))) {  throw new ArgumentNullException("information", "!string.IsNullOrEmpty(information)"); }

            if (this.AcknowledgementDetail == null)
            {
                this.AcknowledgementDetail = new Collection<HL7AcknowledgementDetail>();
            }

            this.AcknowledgementDetail.Add(new HL7AcknowledgementDetail(informationNumber, information, null, HL7AcknowledgementDetailType.Information, this.Sender.Id.Extension));
        }

        /// <summary>
        /// Adds the warning.
        /// </summary>
        /// <param name="warning">       The warning. </param>
        /// <param name="warningNumber"> The warning number. </param>
        public void AddWarning(string warning, int warningNumber)
        {
            if (!(!string.IsNullOrEmpty(warning))) {  throw new ArgumentNullException("warning", "!string.IsNullOrEmpty(warning)"); }

            if (this.AcknowledgementDetail == null)
            {
                this.AcknowledgementDetail = new Collection<HL7AcknowledgementDetail>();
            }

            this.AcknowledgementDetail.Add(new HL7AcknowledgementDetail(warningNumber, warning, null, HL7AcknowledgementDetailType.Warning, this.Sender.Id.Extension));
        }

        /// <summary>
        /// Attaches the specified owner.
        /// </summary>
        /// <param name="owner"> The owner. </param>
        public void Attach(OperationContext owner)
        {
        }

        /// <summary>
        /// Detaches the specified owner.
        /// </summary>
        /// <param name="owner"> The owner. </param>
        public void Detach(OperationContext owner)
        {
        }
    }
}