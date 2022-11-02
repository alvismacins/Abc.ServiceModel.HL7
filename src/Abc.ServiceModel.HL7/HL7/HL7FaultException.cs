// ----------------------------------------------------------------------------
// <copyright file="HL7FaultException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// HL7 Fault Exception.
    /// </summary>
    [Serializable]
    public class HL7FaultException : CommunicationException
    {
        private ICollection<HL7AcknowledgementDetail> details = new Collection<HL7AcknowledgementDetail>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        public HL7FaultException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="message">A description of the reason for the exception.</param>
        /// <param name="errorNumber">The error number.</param>
        public HL7FaultException(string message, int errorNumber)
            : this(new HL7AcknowledgementDetail(errorNumber, message, null, HL7AcknowledgementDetailType.Error, !string.IsNullOrEmpty(HL7OperationContext.Current.OperationContract.Receiver) ? HL7OperationContext.Current.OperationContract.Receiver : HL7OperationContext.Current.Sender.Id.Extension), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="message">A description of the reason for the exception.</param>
        /// <param name="errorNumber">The error number.</param>
        /// <param name="innerException">The exception that caused the <see cref="T:HL7FaultException"></see>.</param>
        public HL7FaultException(string message, int errorNumber, Exception innerException)
            : this(new HL7AcknowledgementDetail(errorNumber, message, null, HL7AcknowledgementDetailType.Error, !string.IsNullOrEmpty(HL7OperationContext.Current.OperationContract.Receiver) ? HL7OperationContext.Current.OperationContract.Receiver : HL7OperationContext.Current.Sender.Id.Extension), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="detail">A HL7 details of the reason for the exception.</param>
        public HL7FaultException(HL7AcknowledgementDetail detail)
            : this(detail, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="detail">A HL7 details of the reason for the exception.</param>
        /// <param name="innerException">The exception that caused the <see cref="T:HL7FaultException"></see>.</param>
        public HL7FaultException(HL7AcknowledgementDetail detail, Exception innerException)
            : this(new HL7AcknowledgementDetail[] { detail }, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="details">The HL7 exception details collection.</param>
        /// <param name="innerException">The exception that caused the <see cref="T:HL7FaultException"></see>.</param>
        public HL7FaultException(IEnumerable<HL7AcknowledgementDetail> details, Exception innerException)
            : base(GetMessageText(details), innerException)
        {
            if (details != null)
            {
                foreach (var item in details)
                {
                    if (item != null)
                    {
                        this.details.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7FaultException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see>  that contains contextual information about the source or destination.</param>
        protected HL7FaultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var details = (Collection<HL7AcknowledgementDetail>)info.GetValue("HL7Details", typeof(Collection<HL7AcknowledgementDetail>));
            foreach (var item in details)
            {
                this.details.Add(item);
            }
        }

        /// <summary>
        /// Gets the HL7 exception details.
        /// </summary>
        public ICollection<HL7AcknowledgementDetail> Details
        {
            get
            {
                return this.details;
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("HL7Details", this.details);
        }

        private static string GetMessageText(IEnumerable<HL7AcknowledgementDetail> details)
        {
            if (details == null)
            {
                return "UnknownFaultNullDetails";
            }

            var sb = new System.Text.StringBuilder();
            foreach (var item in details)
            {
                if (item != null)
                {
                    sb.AppendFormat("{0}: {1}\t", item.DetailType, item.Text);
                }
            }

            return sb.ToString();
        }
    }
}