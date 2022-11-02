namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// HL7Serializer class
    /// </summary>
    public partial class HL7Serializer
    {
        ///// <summary>
        ///// Response type
        ///// </summary>
        // private Responses responseType;

        /// <summary>
        /// Responses data
        /// </summary>
        private enum Responses
        {
            AcknowledgementResponse,
            ApplicationResponse,
            QueryApplicationResponse,
            QueryAcknowledgementResponse
        }

        /// <summary>
        /// Reads the message.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Transmission Wrapper</returns>
        public HL7TransmissionWrapper ReadMessage(XmlReader reader)
        {
            return this.ReadMessage(reader, "*");
        }

        /// <summary>
        /// Reads the message.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Transmission Wrapper</returns>
        public HL7TransmissionWrapper ReadMessage(XmlReader reader, string localName)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!this.CanRead(reader, localName))
            {
                throw new InvalidOperationException();
            }

            HL7TransmissionWrapper transmissionWrapper = this.ReadTransmissionWrapper(reader);

            if (transmissionWrapper.ControlAct != null)
            {
                if (transmissionWrapper.Acknowledgement == null)
                {
                    return new HL7Request(transmissionWrapper);
                }

                if (transmissionWrapper.ControlAct is HL7QueryControlAcknowledgement)
                {
                    HL7QueryControlAcknowledgement data = (HL7QueryControlAcknowledgement)transmissionWrapper.ControlAct;

                    if (data.QueryByParameterPayload != null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.CanNotBeSetResp, HL7Constants.Elements.QueryByParameterPayload));
                    }

                    if (data.QueryContinuation != null)
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.CanNotBeSetResp, HL7Constants.Elements.QueryContinuation));
                    }

                    // if (data.QueryAcknowledgement == null)
                    // {
                    //    throw new FormatException(string.Format(CultureInfo.InvariantCulture, SR.MustBeSet, HL7Constants.Elements.QueryAcknowledgement));
                    // }
                    if (data.QueryAcknowledgement != null)
                    {
                        return new HL7QueryApplicationResponse(transmissionWrapper);
                    }
                }

                if (transmissionWrapper.ControlAct is HL7MessageControlAct)
                {
                    // responseType = Responses.ApplicationResponse;
                    return new HL7ApplicationResponse(transmissionWrapper);
                }
            }
            else
            {
                // responseType = Responses.AcknowledgementResponse;
                return new HL7AcknowledgementResponse(transmissionWrapper);
            }

            if (transmissionWrapper.Acknowledgement == null && transmissionWrapper.ControlAct != null)
            {
                return new HL7Request(transmissionWrapper);
            }
            else if (transmissionWrapper.Acknowledgement != null && transmissionWrapper.ControlAct != null)
            {
                return new HL7ApplicationResponse(transmissionWrapper);
            }
            else if (transmissionWrapper.Acknowledgement != null && transmissionWrapper.ControlAct == null)
            {
                return new HL7AcknowledgementResponse(transmissionWrapper);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Writes the message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        /// <param name="localName">Name of the local.</param>
        public void WriteMessage(XmlWriter writer, HL7TransmissionWrapper data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ItsVersion, HL7Constants.AttributesValue.XmlVersion);
            this.WriteTransmissionWrapper(writer, data);
            writer.WriteEndElement();

            /*
            Type messageType = data.GetType();
            if (messageType == typeof(HL7Request)) {
                this.WriteRequest(writer, (HL7Request)data, localName);
            }
            else if (messageType == typeof(HL7AcknowledgementResponse)) {
                this.WriteAcknowlegementResponse(writer, (HL7AcknowledgementResponse)data, localName);
            }
            else if (messageType == typeof(HL7ApplicationResponse)) {
                this.WriteAcknowlegementResponse(writer, (HL7ApplicationResponse)data, localName);
            }
            else {
                throw new InvalidOperationException();
            }
             */
        }

        /*

        /// <summary>
        /// Writes the request.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        internal virtual void WriteRequest(XmlWriter writer, HL7Request data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ItsVersion, HL7Constants.AttributesValue.XmlVers);
            this.WriteTransmissionWrapper(writer, data);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the acknowlegement response.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        internal virtual void WriteAcknowlegementResponse(XmlWriter writer, HL7AcknowledgementResponse data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ItsVersion, HL7Constants.AttributesValue.XmlVers);
            this.WriteTransmissionWrapper(writer, data);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the application response.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        internal void WriteApplicationResponse(XmlWriter writer, HL7ApplicationResponse data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ItsVersion, HL7Constants.AttributesValue.XmlVers);
            this.WriteTransmissionWrapper(writer, data);
            writer.WriteEndElement();
        }
         */

        /// <summary>
        /// Writes the query by parameter payload.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        public virtual void WriteQueryByParameterPayload(XmlWriter writer, HL7QueryByParameterPayload data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.WriteQueryByParameterPayload(writer);
        }

        /// <summary>
        /// Writes the query by parameter payload.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        public virtual void WriteQueryContinuationPayload(XmlWriter writer, HL7QueryContinuation data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.WriteQueryContinuation(writer);
        }

        /// <summary>
        /// Writes the accept acknowledgement code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAcceptAcknowledgementCode(XmlWriter writer, HL7AcceptAcknowledgementCode data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AcceptAcknowledgementCode, HL7Constants.Namespace);

            // writer.WriteAttributeString(HL7Constants.Attributes.Code, data.ToString());
            switch (data)
            {
                case HL7AcceptAcknowledgementCode.Always:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Always);
                    break;

                case HL7AcceptAcknowledgementCode.Never:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Never);
                    break;

                case HL7AcceptAcknowledgementCode.Error:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Error);
                    break;

                default:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Always);
                    break;
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the acknowledgement.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAcknowledgement(XmlWriter writer, HL7Acknowledgement data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Acknowledgement, HL7Constants.Namespace);
            string code;

            switch (data.AcknowledgementDataType)
            {
                case HL7AcknowledgementType.ApplicationAcknowledgementAccept:
                    if (!this.CheckForErrors(data))
                    {
                        code = HL7Constants.AttributesValue.ApplicationAcknowledgementAccept;
                    }
                    else
                    {
                        code = HL7Constants.AttributesValue.ApplicationAcknowledgementError;
                    }

                    break;

                case HL7AcknowledgementType.ApplicationAcknowledgementError:
                    code = HL7Constants.AttributesValue.ApplicationAcknowledgementError;
                    break;

                case HL7AcknowledgementType.ApplicationAcknowledgementReject:
                    code = HL7Constants.AttributesValue.ApplicationAcknowledgementReject;
                    break;

                case HL7AcknowledgementType.AcceptAcknowledgementCommitAccept:

                    if (!this.CheckForErrors(data))
                    {
                        code = HL7Constants.AttributesValue.AcceptAcknowledgementCommitAccept;
                    }
                    else
                    {
                        code = HL7Constants.AttributesValue.AcceptAcknowledgementCommitError;
                    }

                    break;

                case HL7AcknowledgementType.AcceptAcknowledgementCommitError:
                    code = HL7Constants.AttributesValue.AcceptAcknowledgementCommitError;
                    break;

                case HL7AcknowledgementType.AcceptAcknowledgementCommitReject:
                    code = HL7Constants.AttributesValue.AcceptAcknowledgementCommitReject;
                    break;

                default:
                    throw new InvalidOperationException("Invalid Acknowledgement data type.");
            }

            if (this.version == HL7Version.HL7V3_2006)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.TypeCode, HL7Constants.Namespace);
                writer.WriteAttributeString(HL7Constants.Attributes.Code, code);
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, code);
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.TargetMessage, HL7Constants.Namespace);
            this.WriteId(writer, data.TargetMessage, HL7Constants.Elements.Id);
            writer.WriteEndElement();

            if (data.AcknowledgementDetails != null)
            {
                foreach (var acknowledgementDetails in data.AcknowledgementDetails)
                {
                    this.WriteAcknowledgementDetail(writer, acknowledgementDetails);
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the acknowledgement detail.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteAcknowledgementDetail(XmlWriter writer, HL7AcknowledgementDetail data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AcknowledgementDetail, HL7Constants.Namespace);

            string typeCode;
            switch (data.DetailType)
            {
                case HL7AcknowledgementDetailType.Information:
                    typeCode = HL7Constants.ElementsValue.Information;
                    break;

                case HL7AcknowledgementDetailType.Warning:
                    typeCode = HL7Constants.ElementsValue.Warning;
                    break;

                case HL7AcknowledgementDetailType.Error:
                    typeCode = HL7Constants.ElementsValue.Error;
                    break;

                default:
                    throw new InvalidOperationException(SR.AcknowledgementDetailTypeCode);
            }

            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, typeCode);
            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Code, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.Code, data.Code.Code);
            writer.WriteAttributeString(HL7Constants.Attributes.CodeSystem, data.Code.CodeSystem);
            writer.WriteEndElement();
            writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Text, HL7Constants.Namespace, data.Text);

            if (!string.IsNullOrEmpty(data.Location))
            {
                writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Location, HL7Constants.Namespace, data.Location);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the assigned person.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAssignedDevice(XmlWriter writer, HL7AssignedDevice data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Assigned);

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Dev);
            writer.WriteAttributeString(HL7Constants.Attributes.DeterminerCode, HL7Constants.AttributesValue.Instance);

            if (data.AsMembers != null)
            {
                this.WriteAsMember(writer, data.AsMembers);
            }

            writer.WriteEndElement();

            if (data.RepresentedOrganization != null)
            {
                this.WriteRepresentedOrganization(writer, data.RepresentedOrganization);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the assigned person.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAssignedPerson(XmlWriter writer, HL7AssignedPerson data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Assigned);

            if (data.PersonId != null)
            {
                this.WriteId(writer, data.PersonId, HL7Constants.Elements.Id);
            }

            if (data.Code != null)
            {
                this.WriteClassificator(writer, data.Code, HL7Constants.Elements.Code);
            }

            if (data.Telecoms != null)
            {
                foreach (var item in data.Telecoms)
                {
                    if (item != null)
                    {
                        writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Telecom, HL7Constants.Namespace);
                        writer.WriteAttributeString(HL7Constants.Attributes.Value, item);
                        writer.WriteEndElement();
                    }
                }
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Psn);
            writer.WriteAttributeString(HL7Constants.Attributes.DeterminerCode, HL7Constants.AttributesValue.Instance);

            if (data.Persons != null)
            {
                foreach (var item in data.Persons)
                {
                    this.WritePerson(writer, item, data.AsMembers);
                }
            }

            writer.WriteEndElement();

            if (data.RepresentedOrganization != null)
            {
                this.WriteRepresentedOrganization(writer, data.RepresentedOrganization);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the attention.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAttention(XmlWriter writer, HL7AttentionLine data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AttentionLine, HL7Constants.Namespace);
            writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.KeywordText, HL7Constants.Namespace, data.KeywordText);

            this.WriteId(writer, data.Value, HL7Constants.Elements.Value, true);

            // string objectString = Helper.SerializeObject(data.Value);
            // XmlDocument xml = new XmlDocument();
            // xml.LoadXml(objectString);
            // xml.WriteTo(writer);
            // TODO: object att
            // attentionLine.Value
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the attention lines.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteAttentionLines(XmlWriter writer, IEnumerable<HL7AttentionLine> data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            foreach (HL7AttentionLine attentionLine in data)
            {
                this.WriteAttention(writer, attentionLine);
            }
        }

        /// <summary>
        /// Writes the overseer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteOverseer(XmlWriter writer, HL7Overseer data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Overseer, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.Resp);

            this.WriteTime(writer, data.Time, HL7Constants.Elements.Time);

            if (data.AssignedPerson != null)
            {
                this.WriteAssignedPerson(writer, data.AssignedPerson);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the author or performer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteAuthorOrPerformer(XmlWriter writer, HL7AuthorOrPerformer data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AuthorOrPerformer, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.Author);

            if (data.Time != null && data.Time.HasValue)
            {
                this.WriteTime(writer, data.Time, HL7Constants.Elements.Time);
            }

            if (data.AssignedPerson != null)
            {
                this.WriteAssignedPerson(writer, data.AssignedPerson);
            }

            if (data.AssignedDevice != null)
            {
                this.WriteAssignedDevice(writer, data.AssignedDevice);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the classificator.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        protected virtual void WriteClassificator(XmlWriter writer, HL7ClassificatorId data, string localName)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.Code, data.Code);
            writer.WriteAttributeString(HL7Constants.Attributes.CodeSystem, data.CodeSystem.Value);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the classificators.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        /// <param name="localName">Name of the local.</param>
        protected virtual void WriteClassificators(XmlWriter writer, IEnumerable<HL7ClassificatorId> data, string localName)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }

            if (data != null)
            {
                foreach (var item in data)
                {
                    this.WriteClassificator(writer, item, localName);
                }
            }
        }

        /// <summary>
        /// Writes the control act.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteControlAct(XmlWriter writer, HL7ControlAct data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ControlActProcess, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.ControlAct);
            writer.WriteAttributeString(HL7Constants.Attributes.MoodCode, HL7Constants.AttributesValue.Event);

            if (data.Code != null)
            {
                this.WriteClassificator(writer, data.Code, HL7Constants.Elements.Code);
            }

            if (!string.IsNullOrEmpty(data.Text))
            {
                writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Text, HL7Constants.Namespace, data.Text);
            }

            if (data.EffectiveTime.HasValue)
            {
                this.WriteTime(writer, data.EffectiveTime.Value, HL7Constants.Elements.EffectiveTime);
            }

            if (data.PriorityCode != null)
            {
                this.WriteClassificator(writer, data.PriorityCode, HL7Constants.Elements.PriorityCode);
            }

            if (data.ReasonCodes != null)
            {
                this.WriteClassificators(writer, data.ReasonCodes, HL7Constants.Elements.ReasonCode);
            }

            if (data.LanguageCode != null)
            {
                this.WriteClassificator(writer, data.LanguageCode, HL7Constants.Elements.LanguageCode);
            }
        }

        /// <summary>
        /// Writes the data enterer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteDataEnterer(XmlWriter writer, HL7DataEnterer data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.DataEnterer, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.Entity);

            // writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Time, HL7Constants.Namespace, XmlConvert.ToString(data.Time, HL7Constants.Formats.DateTimeFormat));
            if (data.Time.HasValue)
            {
                this.WriteTime(writer, data.Time.Value, HL7Constants.Elements.Time);
            }

            this.WriteAssignedPerson(writer, data);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the device.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteDevice(XmlWriter writer, HL7II data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Device, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Dev);
            writer.WriteAttributeString(HL7Constants.Attributes.DeterminerCode, HL7Constants.AttributesValue.Instance);
            this.WriteId(writer, data, HL7Constants.Elements.Id);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the information recipient.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteInformationRecipient(XmlWriter writer, HL7InformationRecipient data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.InformationRecipient, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.ParticipationInformationRecipient);

            if (data.Time.HasValue)
            {
                this.WriteTime(writer, data.Time.Value, HL7Constants.Elements.Time);
            }

            this.WriteAssignedPerson(writer, data);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the control act.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteMessageControlAct(XmlWriter writer, HL7MessageControlAct data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }

            this.WriteControlAct(writer, data);

            if (data.Overseer != null)
            {
                foreach (var item in data.Overseer)
                {
                    this.WriteOverseer(writer, item);
                }
            }

            if (data.AuthorOrPerformers != null)
            {
                foreach (var item in data.AuthorOrPerformers)
                {
                    this.WriteAuthorOrPerformer(writer, item);
                }
            }

            if (data.DataEnterers != null)
            {
                foreach (var item in data.DataEnterers)
                {
                    this.WriteDataEnterer(writer, item);
                }
            }

            if (data.InformationRecipients != null)
            {
                foreach (var item in data.InformationRecipients)
                {
                    this.WriteInformationRecipient(writer, item);
                }
            }

            if (data.Subject != null)
            {
                this.WriteSubject(writer, data.Subject);
            }

            // HL7Constants.Elements.ControlActProcess
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the processing code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteProcessingCode(XmlWriter writer, HL7ProcessingCode data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ProcessingCode, HL7Constants.Namespace);

            // writer.WriteAttributeString(HL7Constants.Attributes.Code, data.ToString());
            switch (data)
            {
                case HL7ProcessingCode.Production:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Production);
                    break;

                case HL7ProcessingCode.Test:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Test);
                    break;

                case HL7ProcessingCode.Debug:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Debug);
                    break;

                default:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Production);
                    break;
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the processing mode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteProcessingModeCode(XmlWriter writer, HL7ProcessingModeCode data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ProcessingModeCode, HL7Constants.Namespace);

            // writer.WriteAttributeString(HL7Constants.Attributes.Code, data.ToString());
            switch (data)
            {
                case HL7ProcessingModeCode.Archival:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Archival);
                    break;

                case HL7ProcessingModeCode.InicialLoad:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.InicialLoad);
                    break;

                case HL7ProcessingModeCode.ArchiveRestoration:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.ArchiveRestoration);
                    break;

                case HL7ProcessingModeCode.OperationData:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.OperationData);
                    break;

                default:
                    writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.OperationData);
                    break;
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the query act.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteQueryAct(XmlWriter writer, HL7QueryAcknowledgement data)
        {
            if (writer == null) {  throw new ArgumentNullException("writer", "writer != null"); }
            if (data == null) {  throw new ArgumentNullException("data", "data != null"); }
            if (!(data.QueryResponseCode != null)) {  throw new ArgumentNullException("data", "data.QueryResponseCode != null"); }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.QueryAcknowledgement, HL7Constants.Namespace);

            if (data.QueryId != null)
            {
                this.WriteId(writer, data.QueryId, HL7Constants.Elements.QueryId);
            }

            if (data.StatusCode != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.StatusCode, HL7Constants.Namespace);

                // writer.WriteAttributeString(HL7Constants.Attributes.Code, data.StatusCode.Code.ToString());
                switch (data.StatusCode.Code)
                {
                    case HL7StatusCode.HL7StatusCodes.Aborted:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Aborted);
                        break;

                    case HL7StatusCode.HL7StatusCodes.DeliveredResponse:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.DeliveredResponse);
                        break;

                    case HL7StatusCode.HL7StatusCodes.Executing:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.Executing);
                        break;

                    case HL7StatusCode.HL7StatusCodes.WaitContinuedQueryResponse:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.WaitContinuedQueryResponse);
                        break;

                    case HL7StatusCode.HL7StatusCodes.New:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.New);
                        break;

                    default:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.New);
                        break;
                }

                writer.WriteEndElement();
            }

            if (data.QueryResponseCode != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.QueryResponseCode, HL7Constants.Namespace);
                switch (data.QueryResponseCode.Code)
                {
                    case HL7QueryResponseCode.HL7QueryResponseCodes.ApplicationError:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.ApplicationError);
                        break;

                    case HL7QueryResponseCode.HL7QueryResponseCodes.NoDataFound:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.NoDataFound);
                        break;

                    case HL7QueryResponseCode.HL7QueryResponseCodes.DataFound:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.DataFound);
                        break;

                    case HL7QueryResponseCode.HL7QueryResponseCodes.QueryParameterError:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.QueryParameterError);
                        break;

                    default:
                        writer.WriteAttributeString(HL7Constants.Attributes.Code, HL7Constants.AttributesValue.DataFound);
                        break;
                }

                writer.WriteEndElement();
            }

            if (data.ResultTotalQuantity != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ResultTotalQuantity, HL7Constants.Namespace);
                writer.WriteAttributeString(HL7Constants.Attributes.Value, data.ResultTotalQuantity.ToString());
                writer.WriteEndElement();
            }

            if (data.ResultCurrentQuantity != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ResultCurrentQuantity, HL7Constants.Namespace);
                writer.WriteAttributeString(HL7Constants.Attributes.Value, data.ResultCurrentQuantity.ToString());
                writer.WriteEndElement();
            }

            if (data.ResultRemainingQuantity != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ResultRemainingQuantity, HL7Constants.Namespace);
                writer.WriteAttributeString(HL7Constants.Attributes.Value, data.ResultRemainingQuantity.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the query control acknowledgement.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteQueryControlAcknowledgement(XmlWriter writer, HL7QueryControlAcknowledgement data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!(!(data.QueryByParameterPayload != null && data.Subject != null))) {  throw new FormatException("!(data.QueryByParameterPayload != null && data.Subject != null)"); }
            if (!(!(data.QueryByParameterPayload != null && data.QueryAcknowledgement != null))) {  throw new FormatException("!(data.QueryByParameterPayload != null && data.QueryAcknowledgement != null)"); }

            this.WriteControlAct(writer, data);

            if (data.Overseer != null)
            {
                foreach (var item in data.Overseer)
                {
                    this.WriteOverseer(writer, item);
                }
            }

            if (data.AuthorOrPerformers != null)
            {
                foreach (var item in data.AuthorOrPerformers)
                {
                    this.WriteAuthorOrPerformer(writer, item);
                }
            }

            if (data.DataEnterers != null)
            {
                foreach (var item in data.DataEnterers)
                {
                    this.WriteDataEnterer(writer, item);
                }
            }

            if (data.InformationRecipients != null)
            {
                foreach (var item in data.InformationRecipients)
                {
                    this.WriteInformationRecipient(writer, item);
                }
            }

            if (data.Subject != null)
            {
                this.WriteSubject(writer, data.Subject);
            }

            if (data.QueryAcknowledgement != null)
            {
                this.WriteQueryAct(writer, data.QueryAcknowledgement);
            }

            if (data.QueryByParameterPayload != null)
            {
                this.WriteQueryByParameterPayload(writer, data.QueryByParameterPayload);
            }

            if (data.QueryContinuation != null)
            {
                this.WriteQueryContinuationPayload(writer, data.QueryContinuation);
            }

            // HL7Constants.Elements.ControlActProcess
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the receiver.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteReceiver(XmlWriter writer, HL7Device data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Receiver, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.Receiver);
            this.WriteDevice(writer, data.Id);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the represented organization.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteRepresentedOrganization(XmlWriter writer, HL7RepresentedOrganization data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.RepresentedOrganization, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Organization);
            writer.WriteAttributeString(HL7Constants.Attributes.DeterminerCode, HL7Constants.AttributesValue.Instance);

            foreach (var item in data.Ids)
            {
                this.WriteId(writer, item, HL7Constants.Elements.Id);
            }

            if (data.Code != null)
            {
                this.WriteClassificator(writer, data.Code, HL7Constants.Elements.Code);
            }

            if (data.Names != null && data.Names.Count > 0)
            {
                foreach (var item in data.Names)
                {
                    writer.WriteElementString(HL7Constants.Elements.Name, HL7Constants.Namespace, item);
                }
            }

            if (!string.IsNullOrEmpty(data.StreedAdress))
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Addr, HL7Constants.Namespace);
                writer.WriteElementString(HL7Constants.Elements.StreetAddressLine, HL7Constants.Namespace, data.StreedAdress);

                writer.WriteEndElement();
            }

            if (data.StandardIndustryClassCodes != null)
            {
                this.WriteClassificator(writer, data.StandardIndustryClassCodes, HL7Constants.Elements.StandardIndustryClassCode);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the sender.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteSender(XmlWriter writer, HL7Device data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Sender, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.TypeCode, HL7Constants.AttributesValue.Sender);
            this.WriteDevice(writer, data.Id);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the sequence number.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        protected virtual void WriteSequenceNumber(XmlWriter writer, int data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.SequenceNumber, HL7Constants.Namespace);

            // TODO: date format
            writer.WriteAttributeString(HL7Constants.Attributes.Value, data.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the subject.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteSubject(XmlWriter writer, HL7Subject data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.WriteSubject(writer);
        }

        /// <summary>
        /// Writes the time.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        protected virtual void WriteTime(XmlWriter writer, DateTime? data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data.HasValue)
            {
                writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);

                string time = Helper.ConvertDateTimeToHL7(data.Value);
                writer.WriteAttributeString(HL7Constants.Attributes.Value, time);
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the transmission wrapper.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteTransmissionWrapper(XmlWriter writer, HL7TransmissionWrapper data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.WriteId(writer, data.TemplateId, HL7Constants.Elements.TemplateId);
            this.WriteId(writer, data.IdentificationId, HL7Constants.Elements.Id);
            this.WriteTime(writer, data.CreationTime, HL7Constants.Elements.CreationTime);
            if (this.version == HL7Version.HL7V3_2006)
            {
                this.WriteVersion(writer, HL7Constants.Versions.V3NE2006);
            }
            else
            {
                this.WriteVersion(writer, HL7Constants.Versions.V3NE2011);
            }

            this.WriteId(writer, data.InteractionId, HL7Constants.Elements.InteractionId);

            // createinTime
            this.WriteProcessingCode(writer, data.ProcessingCode);
            this.WriteProcessingModeCode(writer, data.ProcessingModeCode);
            this.WriteAcceptAcknowledgementCode(writer, data.AcceptAcknowledgementCode);

            if (data.SequenceNumber.HasValue)
            {
                this.WriteSequenceNumber(writer, data.SequenceNumber.Value);
            }

            // TODO: sequesnce
            this.WriteReceiver(writer, data.Receiver);
            this.WriteSender(writer, data.Sender);

            // TODO: WriteAttentionLines
            this.WriteAttentionLines(writer, data.AttentionLineCollection);

            if (data.Acknowledgement != null)
            {
                this.WriteAcknowledgement(writer, data.Acknowledgement);
            }

            if (data.ControlAct != null)
            {
                if (data.ControlAct is HL7QueryControlAcknowledgement)
                {
                    this.WriteQueryControlAcknowledgement(writer, (HL7QueryControlAcknowledgement)data.ControlAct);
                }
                else
                {
                    if (data.ControlAct is HL7MessageControlAct)
                    {
                        this.WriteMessageControlAct(writer, (HL7MessageControlAct)data.ControlAct);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the version.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        protected virtual void WriteVersion(XmlWriter writer, string data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.VersionCode, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.Code, data);
            writer.WriteEndElement();
        }

        /*

       /// <summary>
       /// Writes the query sender device.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQuerySenderDevice(XmlWriter writer, HL7QuerySenderDevice data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.SenderDevice, HL7Constants.Namespace);
           this.WriteId(writer, data.ValueData, HL7Constants.Elements.Value);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query receiver device.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQueryReceiverDevice(XmlWriter writer, HL7QueryReceiverDevice data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ReceiverDevice, HL7Constants.Namespace);
           this.WriteId(writer, data.ValueData, HL7Constants.Elements.Value);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query data enterer.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQueryDataEnterer(XmlWriter writer, HL7QueryDataEnterer data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.DataEnterer, HL7Constants.Namespace);
           this.WriteId(writer, data.ValueData, HL7Constants.Elements.Value);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query patient id.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQueryPatientId(XmlWriter writer, HL7QueryPatientId data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.PatientId, HL7Constants.Namespace);
           this.WriteId(writer, data.ValueData, HL7Constants.Elements.Value);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query responsible organization.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQueryResponsibleOrganization(XmlWriter writer, HL7QueryResponsibleOrganization data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.ResponsibleOrganization, HL7Constants.Namespace);
           this.WriteId(writer, data.ValueData, HL7Constants.Elements.Value);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query event timeframe.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQueryEventTimeframe(XmlWriter writer, HL7QueryEventTimeframe data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.EventTimeframe, HL7Constants.Namespace);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Value, HL7Constants.Namespace, data.ValueData);
           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.SemanticsText, HL7Constants.Namespace, data.SemanticsText);
           writer.WriteEndElement();
       }

       /// <summary>
       /// Writes the query sort control.
       /// </summary>
       /// <param name="writer">The writer.</param>
       /// <param name="data">The data.</param>
       protected virtual void WriteQuerySortControl(XmlWriter writer, HL7QuerySortControl data)
       {
           writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.SequenceNumber, HL7Constants.Namespace);
           if (data.SequenceNumber.HasValue)
           {
               writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Value, HL7Constants.Namespace, data.SequenceNumber.Value.ToString());
           }

           writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.ElementName, HL7Constants.Namespace, data.ElementName);

           if (data.DirectionCode != null)
           {
               writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.DirectionCode, HL7Constants.Namespace);
               writer.WriteAttributeString(HL7Constants.Attributes.Code, data.DirectionCode.Code);
               writer.WriteEndElement();
           }

           writer.WriteEndElement();
       }

        */

        /// <summary>
        /// Checks for errors.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>if contains errors true, else false</returns>
        private bool CheckForErrors(HL7Acknowledgement data)
        {
            if (data != null && data.AcknowledgementDetails != null)
            {
                foreach (var acknowledgementDetails in data.AcknowledgementDetails)
                {
                    if (acknowledgementDetails.DetailType == HL7AcknowledgementDetailType.Error)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void WriteAsMember(XmlWriter writer, IEnumerable<HL7AsMember> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item != null && item.GroupId != null)
                    {
                        writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.AsMember, HL7Constants.Namespace);
                        writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.Mbr);

                        if (item != null && item.Id != null)
                        {
                            foreach (var itemId in item.Id)
                            {
                                this.WriteId(writer, itemId, HL7Constants.Elements.Id);
                            }
                        }

                        if (item != null && item.GroupId != null)
                        {
                            this.WriteGroup(writer, item);
                        }

                        writer.WriteEndElement();
                    }
                }
            }
        }

        /// <summary>
        /// Writes the group.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        private void WriteGroup(XmlWriter writer, HL7AsMember data)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.GroupId != null)
            {
                writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Group, HL7Constants.Namespace);
                writer.WriteAttributeString(HL7Constants.Attributes.ClassCode, HL7Constants.AttributesValue.RGRP);
                writer.WriteAttributeString(HL7Constants.Attributes.DeterminerCode, HL7Constants.AttributesValue.Instance);

                foreach (var item in data.GroupId)
                {
                    if (item != null)
                    {
                        this.WriteId(writer, item, HL7Constants.Elements.Id);
                    }
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the id.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data. value</param>
        /// <param name="localName">Name of the local.</param>
        private void WriteId(XmlWriter writer, HL7II data, string localName)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.Root, data.Root.Value);
            writer.WriteAttributeString(HL7Constants.Attributes.Extension, data.Extension);
            writer.WriteEndElement();
        }

        private void WriteId(XmlWriter writer, HL7II data, string localName, bool xsiType)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            writer.WriteStartElement(HL7Constants.Prefix, localName, HL7Constants.Namespace);

            if (xsiType)
            {
                writer.WriteAttributeString(HL7Constants.Attributes.Type, HL7Constants.NamespaceXsi, $"{HL7Constants.Prefix}:II");
            }

            writer.WriteAttributeString(HL7Constants.Attributes.Root, data.Root.Value);
            writer.WriteAttributeString(HL7Constants.Attributes.Extension, data.Extension);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the person.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        /// <param name="asMember">As member.</param>
        private void WritePerson(XmlWriter writer, HL7Person data, ICollection<HL7AsMember> asMember)
        {
            writer.WriteStartElement(HL7Constants.Prefix, HL7Constants.Elements.Name, HL7Constants.Namespace);
            writer.WriteAttributeString(HL7Constants.Attributes.Use, HL7Constants.AttributesValue.L);

            if (data.FamilyName != null)
            {
                foreach (var item in data.FamilyName)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Family, HL7Constants.Namespace, item);
                    }
                }
            }

            if (data.GivenName != null)
            {
                foreach (var item in data.GivenName)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        writer.WriteElementString(HL7Constants.Prefix, HL7Constants.Elements.Given, HL7Constants.Namespace, item);
                    }
                }
            }

            writer.WriteEndElement();

            this.WriteAsMember(writer, asMember);
        }
    }
}