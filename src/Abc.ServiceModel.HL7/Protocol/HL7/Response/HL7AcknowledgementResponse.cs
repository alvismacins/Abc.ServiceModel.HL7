namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public class HL7AcknowledgementResponse : HL7TransmissionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
        /// </summary>
        /// <param name="interactionExtension">The interaction extension.</param>
        /// <param name="version">The version.</param>
        /// <param name="senderExtension">The sender extension.</param>
        /// <param name="receiverExtension">The receiver extension.</param>
        /// <param name="acknowledgement">The acknowledgement.</param>
       public HL7AcknowledgementResponse(string interactionExtension, string version, string senderExtension, string receiverExtension, HL7Acknowledgement acknowledgement)
            : this(new HL7TemplateId(Helper.GetUrnType(interactionExtension, version)), new HL7IdentificationId(), version, DateTime.Now, new HL7InteractionId(interactionExtension), HL7ProcessingCode.Production, HL7ProcessingModeCode.OperationData, HL7AcceptAcknowledgementCode.Always, new HL7Device(senderExtension, HL7Constants.AttributesValue.Sender), new HL7Device(receiverExtension, HL7Constants.AttributesValue.Receiver), null, acknowledgement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
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
        /// <param name="attentionLines">The attention lines.</param>
        /// <param name="acknowledgement">The acknowledgement.</param>
        public HL7AcknowledgementResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, attentionLines, acknowledgement, null)
        {
            // TODO: if (acknowledgement == null) {  throw new FormatException("acknowledgement", "acknowledgement != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
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
        /// <param name="attentionLines">The attention lines.</param>
        /// <param name="acknowledgement">The acknowledgement.</param>
        /// <param name="controlAct">The control act.</param>
        public HL7AcknowledgementResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement, HL7ControlAct controlAct)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, attentionLines, acknowledgement, controlAct)
        {
            //if (acknowledgement == null) {  throw new FormatException("acknowledgement != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
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
        /// <param name="attentionLines">The attention lines.</param>
        /// <param name="acknowledgement">The acknowledgement.</param>
        /// <param name="controlAct">The control act.</param>
        public HL7AcknowledgementResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, int? sequenceNumber, HL7Device sender, HL7Device receiver, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement, HL7ControlAct controlAct)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sequenceNumber, sender, receiver, attentionLines, acknowledgement, controlAct)
        {
           // if (acknowledgement == null) {  throw new FormatException( "acknowledgement != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
        /// </summary>
        /// <param name="transmissionWrapper">The transmission wrapper.</param>
        internal HL7AcknowledgementResponse(HL7TransmissionWrapper transmissionWrapper)
            : base(transmissionWrapper.TemplateId, transmissionWrapper.IdentificationId, transmissionWrapper.VersionCode, transmissionWrapper.CreationTime, transmissionWrapper.InteractionId, transmissionWrapper.ProcessingCode, transmissionWrapper.ProcessingModeCode, transmissionWrapper.AcceptAcknowledgementCode, transmissionWrapper.SequenceNumber, transmissionWrapper.Sender, transmissionWrapper.Receiver, transmissionWrapper.AttentionLineCollection, transmissionWrapper.Acknowledgement, transmissionWrapper.ControlAct)
        {
            if (!(transmissionWrapper.Acknowledgement != null)) {  throw new FormatException("transmissionWrapper.Acknowledgement != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementResponse"/> class.
        /// </summary>
        protected HL7AcknowledgementResponse()
        {
        }
    }
}