#if NETFRAMEWORK

namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Error handler.
    /// </summary>
    public class HL7ErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and the instance context in certain cases.
        /// Ja Exception ir vienāds ar Hl7FaultException, tad atgriež true, cita gadījuma atgriež false
        /// </summary>
        /// <param name="error">The exception thrown during processing.</param>
        /// <returns>
        /// true if  should not abort the session (if there is one) and instance context if the instance context is not <see cref="F:System.ServiceModel.InstanceContextMode.Single"/>; otherwise, false. The default is false.
        /// </returns>
        public bool HandleError(Exception error)
        {
            return error is HL7FaultException;
        }

        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an exception in the course of a service method.
        /// </summary>
        /// <param name="error">The <see cref="T:System.Exception"/> object thrown in the course of the service operation.</param>
        /// <param name="version">The SOAP version of the message.</param>
        /// <param name="fault">The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or service, in the duplex case.</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var operationContext = HL7OperationContext.Current;

            string interactionId = operationContext.OperationContract.ReplyInteraction;

            // UrnType templateId = new UrnType(operationContext.OperationContract.ReplyTemplate);
            HL7Device sender;
            if (operationContext.OperationContract.Sender != null)
            {
                sender = new HL7Device(operationContext.OperationContract.Sender, HL7Constants.AttributesValue.Sender);
            }
            else
            {
                sender = operationContext.Sender;
            }

            HL7Device receiver;
            if (operationContext.OperationContract.Receiver != null)
            {
                receiver = new HL7Device(operationContext.OperationContract.Receiver, HL7Constants.AttributesValue.Receiver);
            }
            else
            {
                receiver = operationContext.Receiver;
            }

            ICollection<HL7AcknowledgementDetail> details = null;
            HL7FaultException errorHL7 = error as HL7FaultException;

            if (errorHL7 != null)
            {
                details = errorHL7.Details;
            }
            else
            {
                details = new Collection<HL7AcknowledgementDetail>();

                if (error != null && !string.IsNullOrEmpty(error.Message))
                {
                    details.Add(new HL7AcknowledgementDetail("Abc.ServiceModel.HL7-1", error.Message, HL7AcknowledgementDetailType.Error));
                }
                else
                {
                    details.Add(new HL7AcknowledgementDetail("Abc.ServiceModel.HL7-1", SrProtocol.UnknownError, HL7AcknowledgementDetailType.Error));
                }
            }

            HL7AcknowledgementType ackType = operationContext.OperationContract.AcknowledgementResponse ? HL7AcknowledgementType.AcceptAcknowledgementCommitError : HL7AcknowledgementType.ApplicationAcknowledgementError;

            if (operationContext != null && operationContext.SetAcknowledgementTypeInResponse != null && operationContext.SetAcknowledgementTypeInResponse.HasValue)
            {
                ackType = operationContext.SetAcknowledgementTypeInResponse.Value;
            }
            else
            {
                ackType = operationContext.OperationContract.AcknowledgementResponse ? HL7AcknowledgementType.AcceptAcknowledgementCommitError : HL7AcknowledgementType.ApplicationAcknowledgementError;
            }

            HL7TransmissionWrapper response = new HL7AcknowledgementResponse(interactionId, HL7Constants.Versions.V3NE2011, receiver.Id.Extension, sender.Id.Extension, new HL7Acknowledgement(operationContext.MessageId, ackType, details));

            fault = HL7MessageExtension.CreateHL7Message(version, interactionId, response);
        }
    }
}

#endif