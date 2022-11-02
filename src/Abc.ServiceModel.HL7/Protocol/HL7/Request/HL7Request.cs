namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public class HL7Request : HL7TransmissionWrapper
    {
        private HL7QueryControlAcknowledgement queryControlAct;
        private RequestType requestType;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        /// <param name="interactionExtension">The interaction extension.</param>
        /// <param name="version">The version.</param>
        /// <param name="senderExtension">The sender extension.</param>
        /// <param name="receiverExtension">The receiver extension.</param>
        /// <param name="controlAct">The control act.</param>
        public HL7Request(string interactionExtension, string version, string senderExtension, string receiverExtension, HL7ControlAct controlAct)
            : this(new HL7TemplateId(Helper.GetUrnType(interactionExtension, version)), new HL7IdentificationId(), version.ToString(), DateTime.Now, new HL7InteractionId(interactionExtension), HL7ProcessingCode.Production, HL7ProcessingModeCode.OperationData, HL7AcceptAcknowledgementCode.Always, new HL7Device(senderExtension, HL7Constants.AttributesValue.Sender), new HL7Device(receiverExtension, HL7Constants.AttributesValue.Receiver), controlAct, null)
        {
            if (!(!string.IsNullOrEmpty(version))) {  throw new ArgumentNullException("version", "!string.IsNullOrEmpty(version)"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <param name="identification">The identification.</param>
        /// <param name="version">The version.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="processingCode">The processing code.</param>
        /// <param name="processingModeCode">The processing mode code.</param>
        /// <param name="acceptAcknowledgementCode">The accept acknowledgement code.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="controlAct">The control act.</param>
        public HL7Request(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct)
            : this(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, controlAct, null)
        {
            if (!(!string.IsNullOrEmpty(version))) {  throw new ArgumentNullException("version", "!string.IsNullOrEmpty(version)"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <param name="identification">The identification.</param>
        /// <param name="version">The version.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="processingCode">The processing code.</param>
        /// <param name="processingModeCode">The processing mode code.</param>
        /// <param name="acceptAcknowledgementCode">The accept acknowledgement code.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="controlAct">The control act.</param>
        /// <param name="attentionLines">The attention lines.</param>
        public HL7Request(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct, IEnumerable<HL7AttentionLine> attentionLines)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, attentionLines, null, controlAct)
        {
            if (!(!string.IsNullOrEmpty(version))) {  throw new ArgumentNullException("version", "!string.IsNullOrEmpty(version)"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <param name="identification">The identification.</param>
        /// <param name="version">The version.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="interactionId">The interaction id.</param>
        /// <param name="processingCode">The processing code.</param>
        /// <param name="processingModeCode">The processing mode code.</param>
        /// <param name="acceptAcknowledgementCode">The accept acknowledgement code.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="controlAct">The control act.</param>
        /// <param name="attentionLines">The attention lines.</param>
        public HL7Request(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, int sequenceNumber, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct, IEnumerable<HL7AttentionLine> attentionLines)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sequenceNumber, sender, receiver, attentionLines, null, controlAct)
        {
            if (!(!string.IsNullOrEmpty(version))) {  throw new ArgumentNullException("version", "!string.IsNullOrEmpty(version)"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        /// <param name="transmissionWrapper">The transmission wrapper.</param>
        internal HL7Request(HL7TransmissionWrapper transmissionWrapper)
            : base(
                transmissionWrapper.TemplateId,
                transmissionWrapper.IdentificationId,
                transmissionWrapper.VersionCode,
                transmissionWrapper.CreationTime,
                transmissionWrapper.InteractionId,
                transmissionWrapper.ProcessingCode,
                transmissionWrapper.ProcessingModeCode,
                transmissionWrapper.AcceptAcknowledgementCode,
                transmissionWrapper.SequenceNumber,
                transmissionWrapper.Sender,
                transmissionWrapper.Receiver,
                transmissionWrapper.AttentionLineCollection,
                null,
                transmissionWrapper.ControlAct)
        {
            if (transmissionWrapper.ControlAct is HL7QueryControlAcknowledgement)
            {
                this.queryControlAct = (HL7QueryControlAcknowledgement)transmissionWrapper.ControlAct;

                if (this.queryControlAct != null)
                {
                    this.requestType = RequestType.QueryParamRequest;

                    if (this.queryControlAct.QueryByParameterPayload == null && this.queryControlAct.QueryContinuation == null)
                    {
                        if (this.queryControlAct.QueryByParameterPayload == null && this.queryControlAct.QueryContinuation != null)
                        {
                            throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.MustBeSet, HL7Constants.Elements.QueryByParameterPayload));
                        }

                        if (this.queryControlAct.QueryByParameterPayload != null && this.queryControlAct.QueryContinuation == null)
                        {
                            throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.MustBeSet, HL7Constants.Elements.QueryContinuation));
                        }
                    }

                    if (this.SequenceNumber.HasValue)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.CanNotBeSetReq, HL7Constants.Elements.SequenceNumber));
                    }

                    if (this.queryControlAct.Subject != null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.CanNotBeSetReq, HL7Constants.Elements.Subject));
                    }

                    if (this.queryControlAct.QueryAcknowledgement != null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.CanNotBeSetReq, HL7Constants.Elements.QueryAcknowledgement));
                    }
                }
                else
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.DataNotFull, HL7Constants.Elements.ControlActProcess));
                }
            }

            if (transmissionWrapper.ControlAct is HL7MessageControlAct)
            {
                this.requestType = RequestType.MessageRequest;
            }

            if (this.ControlAct?.ReasonCodes?.Count < 2)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.ReasonCodesIsNotSet));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Request"/> class.
        /// </summary>
        protected HL7Request()
        {
        }

        /// <summary>
        /// type of Request
        /// </summary>
        public enum RequestType
        {
            /// <summary>
            /// Request message for subject
            /// </summary>
            MessageRequest,

            /// <summary>
            /// Request message for queryByParameter
            /// </summary>
            QueryParamRequest,

            /// <summary>
            /// Request message for QueryContinuation
            /// </summary>
            QueryContinuationRequest
        }

        /// <summary>
        /// Gets the query control act.
        /// </summary>
        public HL7QueryControlAcknowledgement QueryControlAct
        {
            get
            {
                return this.queryControlAct;
            }
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public RequestType Request
        {
            get
            {
                return this.requestType;
            }
        }
    }
}