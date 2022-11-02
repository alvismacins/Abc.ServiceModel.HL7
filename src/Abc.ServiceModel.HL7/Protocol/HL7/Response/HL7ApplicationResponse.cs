namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public class HL7ApplicationResponse : HL7AcknowledgementResponse
    {
        /* /// <summary>
         /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
         /// </summary>
         /// <param name="interactionExtension">The interaction extension.</param>
         /// <param name="version">The version.</param>
         /// <param name="senderExtension">The sender extension.</param>
         /// <param name="receiverExtension">The receiver extension.</param>
         /// <param name="controlAct">The control act.</param>
         /// <param name="acknowledgement">The acknowledgement.</param>
         public HL7ApplicationResponse(string interactionExtension, string version, string senderExtension, string receiverExtension, HL7ControlAct controlAct, HL7Acknowledgement acknowledgement)
             : this(new HL7TemplateId(Helper.GetUrnType(interactionExtension, version)), new HL7IdentificationId(), version, DateTime.Now, new HL7InteractionId(interactionExtension), HL7ProcessingCode.Production, HL7ProcessingModeCode.OperationData, HL7AcceptAcknowledgementCode.Always, new HL7Device(senderExtension, HL7Constants.AttributesValue.Sender), new HL7Device(receiverExtension, HL7Constants.AttributesValue.Receiver), controlAct, acknowledgement)
         {
         }*/

        public HL7ApplicationResponse(string interactionExtension, string version, string senderExtension, string receiverExtension, HL7ControlAct controlAct, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement)
       : this(new HL7TemplateId(Helper.GetUrnType(interactionExtension, version)), new HL7IdentificationId(), version, DateTime.Now, new HL7InteractionId(interactionExtension), HL7ProcessingCode.Production, HL7ProcessingModeCode.OperationData, HL7AcceptAcknowledgementCode.Always, new HL7Device(senderExtension, HL7Constants.AttributesValue.Sender), new HL7Device(receiverExtension, HL7Constants.AttributesValue.Receiver), controlAct, attentionLines, acknowledgement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
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
        /// <param name="acknowledgement">The acknowledgement.</param>
        public HL7ApplicationResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct, HL7Acknowledgement acknowledgement)
            : this(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, controlAct, null, acknowledgement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
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
        /// <param name="acknowledgement">The acknowledgement.</param>
        public HL7ApplicationResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sender, receiver, attentionLines, acknowledgement)
        {
            if (controlAct == null) {  throw new FormatException("controlAct != null"); }
            this.ControlAct = controlAct;

            if (this.ControlAct.ReasonCodes.Count < 2)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.ReasonCodesIsNotSet));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
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
        /// <param name="acknowledgement">The acknowledgement.</param>
        public HL7ApplicationResponse(HL7TemplateId templateId, HL7IdentificationId identification, string version, DateTime creationTime, HL7InteractionId interactionId, HL7ProcessingCode processingCode, HL7ProcessingModeCode processingModeCode, HL7AcceptAcknowledgementCode acceptAcknowledgementCode, int sequenceNumber, HL7Device sender, HL7Device receiver, HL7ControlAct controlAct, IEnumerable<HL7AttentionLine> attentionLines, HL7Acknowledgement acknowledgement)
            : base(templateId, identification, version, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sequenceNumber, sender, receiver, attentionLines, acknowledgement, controlAct)
        {
            if (controlAct == null) {  throw new FormatException("controlAct != null"); }

            if (this.ControlAct.ReasonCodes.Count < 2)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.ReasonCodesIsNotSet));
            }

            // this.ControlAct = controlAct;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
        /// </summary>
        /// <param name="transmissionWrapper">The transmission wrapper.</param>
        internal HL7ApplicationResponse(HL7TransmissionWrapper transmissionWrapper)
            : base(transmissionWrapper.TemplateId, transmissionWrapper.IdentificationId, transmissionWrapper.VersionCode, transmissionWrapper.CreationTime, transmissionWrapper.InteractionId, transmissionWrapper.ProcessingCode, transmissionWrapper.ProcessingModeCode, transmissionWrapper.AcceptAcknowledgementCode, transmissionWrapper.SequenceNumber, transmissionWrapper.Sender, transmissionWrapper.Receiver, transmissionWrapper.AttentionLineCollection, transmissionWrapper.Acknowledgement, transmissionWrapper.ControlAct)
        {
            if (transmissionWrapper == null) {  throw new ArgumentNullException("transmissionWrapper", "transmissionWrapper != null"); }
            if (!(transmissionWrapper.ControlAct != null)) {  throw new FormatException("transmissionWrapper.ControlAct != null"); }
            if (!(transmissionWrapper.ControlAct.ReasonCodes.Count > 1)) {  throw new FormatException("transmissionWrapper.ControlAct.ReasonCodes.Count > 1"); }

            // this.ControlAct = transmissionWrapper.ControlAct;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ApplicationResponse"/> class.
        /// </summary>
        protected HL7ApplicationResponse()
        {
        }
    }
}