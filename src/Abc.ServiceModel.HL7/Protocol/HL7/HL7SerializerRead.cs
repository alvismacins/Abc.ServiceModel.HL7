namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// Serializatora klase, HL7 ziņojuma lasīšanas un rakstīšanai.
    /// </summary>
    public partial class HL7Serializer
    {
        private HL7Version version;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Serializer"/> class.
        /// </summary>
        public HL7Serializer()
            : this(HL7Version.HL7V3_2011)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Serializer"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public HL7Serializer(HL7Version version)
        {
            this.version = version;
        }

        /// <summary>
        /// Reads the query by parameter payload.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// Message HL7QueryByParameterPayload
        /// </returns>
        public virtual HL7QueryByParameterPayload ReadQueryByParameterPayload(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            return HL7QueryByParameterPayload.CreateQueryByParameterPayload(reader);
        }

        /// <summary>
        /// Reads the query continuation.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Message HL7 Query Continuation</returns>
        public virtual HL7QueryContinuation ReadQueryContinuation(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            return HL7QueryContinuation.CreateQueryContinuation(reader);
        }

        /// <summary>
        /// Ends the element.
        /// </summary>
        /// <param name="reader">The reader.</param>
        internal static void EndElement(XmlReader reader)
        {
            if (reader.MoveToContent() != XmlNodeType.EndElement)
            {
               // throw new XmlException("Have to be endElement");
            }

            if (reader.NodeType == XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Determines whether this instance can read the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>
        ///   <c>true</c> if this instance can read the specified reader; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanRead(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (localName == "*")
            {
                return this.CanReadFirst(reader, reader.LocalName);
            }
            else
            {
                return this.CanReadFirst(reader, localName);
            }
        }

        /// <summary>
        /// Determines whether this instance [can read first] the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can read first] the specified reader; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanReadFirst(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (reader.IsStartElement(localName, HL7Constants.Namespace))
            {
                reader.ReadStartElement();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads the accept acknowledgement code.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>HL7 AcceptAcknowledgement Code</returns>
        protected virtual HL7AcceptAcknowledgementCode ReadAcceptAcknowledgementCode(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.AcceptAcknowledgementCode, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.AcceptAcknowledgementCode));
            }
            else
            {
                HL7AcceptAcknowledgementCode acceptAcknowledgementCode = HL7.HL7AcceptAcknowledgementCode.Always;
                string code = this.ReadCodeElement(reader, HL7Constants.Elements.AcceptAcknowledgementCode);

                switch (code)
                {
                    case HL7Constants.AttributesValue.Always:
                        acceptAcknowledgementCode = HL7AcceptAcknowledgementCode.Always;
                        break;

                    case HL7Constants.AttributesValue.Never:
                        acceptAcknowledgementCode = HL7AcceptAcknowledgementCode.Never;
                        break;

                    case HL7Constants.AttributesValue.Error:
                        acceptAcknowledgementCode = HL7AcceptAcknowledgementCode.Error;
                        break;

                    default:
                        break;
                }

                return acceptAcknowledgementCode;
            }
        }

        /// <summary>
        /// Reads the acknowledgement.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Acknowledgement data</returns>
        protected virtual HL7Acknowledgement ReadAcknowledgement(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.Acknowledgement, HL7Constants.Namespace))
            {
                return null;
            }

            HL7AcknowledgementType code = HL7AcknowledgementType.ApplicationAcknowledgementAccept;

            if (this.version == HL7Version.HL7V3_2011)
            {
                string acknowledgementType = reader.GetAttribute(HL7Constants.Attributes.TypeCode);

                // HL7AcknowledgementType code = (HL7AcknowledgementType)Enum.Parse(typeof(HL7AcknowledgementType), reader.GetAttribute(HL7Constants.Attributes.Code));
                code = this.CheckAcknowledgement(acknowledgementType);

                reader.ReadStartElement(HL7Constants.Elements.Acknowledgement, HL7Constants.Namespace);

                EndElement(reader);
            }
            else
            {
                reader.ReadStartElement(HL7Constants.Elements.Acknowledgement, HL7Constants.Namespace);

                if (reader.IsStartElement(HL7Constants.Elements.TypeCode, HL7Constants.Namespace))
                {
                    string acknowledgementType = reader.GetAttribute(HL7Constants.Attributes.Code);

                    // HL7AcknowledgementType code = (HL7AcknowledgementType)Enum.Parse(typeof(HL7AcknowledgementType), reader.GetAttribute(HL7Constants.Attributes.Code));
                    code = this.CheckAcknowledgement(acknowledgementType);

                    reader.ReadStartElement(HL7Constants.Elements.TypeCode, HL7Constants.Namespace);

                    EndElement(reader);
                }
                else
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ACkTypeCodeElementNotSet));
                }
            }

            HL7IdentificationId interactionId = null;

            if (reader.IsStartElement(HL7Constants.Elements.TargetMessage, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.TargetMessage, HL7Constants.Namespace);

                // TODO: value
                HL7II identifier = this.ReadId(reader, HL7Constants.Elements.Id);
                interactionId = new HL7IdentificationId(identifier);

                // HL7Acknowledgement acknowledgement = new HL7Acknowledgement(new HL7IdentificationId(identifier), code);
                EndElement(reader);
            }

            Collection<HL7AcknowledgementDetail> col = new Collection<HL7AcknowledgementDetail>();

            while (reader.IsStartElement(HL7Constants.Elements.AcknowledgementDetail, HL7Constants.Namespace))
            {
                HL7AcknowledgementDetail acknowledgementDetail = this.ReadAcknowledgementDetail(reader);
                col.Add(acknowledgementDetail);
            }

            foreach (var details in col)
            {
                if ((details.DetailType == HL7AcknowledgementDetailType.Error) && (code != HL7AcknowledgementType.ApplicationAcknowledgementError && code != HL7AcknowledgementType.AcceptAcknowledgementCommitError))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SR.ErrorCheck, code.ToString()));
                }
            }

            HL7Acknowledgement acknowledgement = new HL7Acknowledgement(interactionId, code, col);

            EndElement(reader);

            return acknowledgement;
        }

        /// <summary>
        /// Reads the acknowledgement detail.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// HL7Acknowledgement Detail
        /// </returns>
        protected virtual HL7AcknowledgementDetail ReadAcknowledgementDetail(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.AcknowledgementDetail, HL7Constants.Namespace))
            {
                return null;
            }

            string typeCode = reader.GetAttribute(HL7Constants.Attributes.TypeCode);

            // string typeCode = ReadCodeElement(reader, HL7Constants.Elements.Code);
            HL7AcknowledgementDetailType acknowledgementDetailType;

            switch (typeCode)
            {
                case Abc.ServiceModel.Protocol.HL7.HL7Constants.ElementsValue.Error:
                    acknowledgementDetailType = HL7AcknowledgementDetailType.Error;
                    break;

                case Abc.ServiceModel.Protocol.HL7.HL7Constants.ElementsValue.Information:
                    acknowledgementDetailType = HL7AcknowledgementDetailType.Information;
                    break;

                case Abc.ServiceModel.Protocol.HL7.HL7Constants.ElementsValue.Warning:
                    acknowledgementDetailType = HL7AcknowledgementDetailType.Warning;
                    break;

                default:
                    throw new FormatException(SR.AcknowledgementDetailTypeCode);
            }

            reader.ReadStartElement(HL7Constants.Elements.AcknowledgementDetail, HL7Constants.Namespace);

            HL7AcknowledgementDetailCode acknowledgementDetailCode = this.ReadAcknowledgementDetailCode(reader);

            string text = null;

            if (reader.IsStartElement(HL7Constants.Elements.Text, HL7Constants.Namespace))
            {
                text = reader.ReadElementContentAsString(HL7Constants.Elements.Text, HL7Constants.Namespace);
            }

            int i = 0;
            string location = null;

            while (reader.IsStartElement(HL7Constants.Elements.Location, HL7Constants.Namespace))
            {
                if (i == 0)
                {
                    if (reader.IsStartElement(HL7Constants.Elements.Location, HL7Constants.Namespace))
                    {
                        location = reader.ReadElementContentAsString(HL7Constants.Elements.Location, HL7Constants.Namespace);
                    }
                }
                else
                {
                }
            }

            EndElement(reader);

            return new HL7AcknowledgementDetail(acknowledgementDetailCode, text, location, acknowledgementDetailType);
        }

        /// <summary>
        /// Reads the acknowledgement detail code.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7Acknowledgement DetailCode</returns>
        protected virtual HL7AcknowledgementDetailCode ReadAcknowledgementDetailCode(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code));
            }
            else
            {
                string code = reader.GetAttribute(HL7Constants.Attributes.Code);

                if (string.IsNullOrEmpty(code))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code, HL7Constants.Attributes.Code));
                }
                else
                {
                }

                string codeSystem = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                if (string.IsNullOrEmpty(codeSystem))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code, HL7Constants.Attributes.CodeSystem));
                }

                if (!OId.IsOId(codeSystem))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.CodeSystem, Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code));
                }

                if (OId.Compare(new OId(codeSystem), Abc.ServiceModel.Protocol.HL7.HL7Constants.OIds.AcknowledgementDetailCodeOId) != 0)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.CodeSystem, Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code));
                }

                reader.ReadStartElement(Abc.ServiceModel.Protocol.HL7.HL7Constants.Elements.Code, HL7Constants.Namespace);

                EndElement(reader);

                return new HL7AcknowledgementDetailCode(code);
            }
        }

        /// <summary>
        /// Reads as licence entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7AsLicensedEntity</returns>
        /// <exception cref="XmlException">HL7AsLicensedEntity 1</exception>
        protected virtual HL7AsLicensedEntity ReadAsLicenceEntity(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.AsLicensedEntity, HL7Constants.Namespace))
            {
                return null;
            }

            reader.ReadStartElement(HL7Constants.Elements.AsLicensedEntity, HL7Constants.Namespace);

            Collection<HL7II> idCollection = null;
            Collection<HL7II> orgIdCollection = null;

            while (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
            {
                if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                {
                    if (idCollection == null)
                    {
                        idCollection = new Collection<HL7II>();
                    }

                    HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                    idCollection.Add(id);
                }
            }

            HL7ClassificatorId code = null;

            if (reader.IsStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace))
            {
                string codeCode = reader.GetAttribute(HL7Constants.Attributes.Code);
                string codeCodeSystem = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                if (!OId.IsOId(codeCodeSystem))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.Code + " in " + HL7Constants.Elements.AssignedPerson));
                }

                code = new HL7ClassificatorId(codeCode, new OId(codeCodeSystem));

                // code
                reader.ReadStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace);
                EndElement(reader);
            }

            if (reader.IsStartElement(HL7Constants.Elements.IssuingOrganization, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.IssuingOrganization, HL7Constants.Namespace);

                while (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                {
                    if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                    {
                        if (orgIdCollection == null)
                        {
                            orgIdCollection = new Collection<HL7II>();
                        }

                        HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                        orgIdCollection.Add(id);
                    }
                }

                EndElement(reader);

                if (reader.IsStartElement(HL7Constants.Elements.ContactParty, HL7Constants.Namespace))
                {
                    reader.ReadStartElement(HL7Constants.Elements.ContactParty, HL7Constants.Namespace);
                    EndElement(reader);
                }
            }

            EndElement(reader);

            return new HL7AsLicensedEntity(idCollection, code, orgIdCollection);
        }

        /// <summary>
        /// Reads as member.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7 AsMember</returns>
        protected virtual HL7AsMember ReadAsMember(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.AsMember, HL7Constants.Namespace))
            {
                return null;
            }

            reader.ReadStartElement(HL7Constants.Elements.AsMember, HL7Constants.Namespace);

            // HL7AsMember asMember = new HL7AsMember();
            Collection<HL7II> idCollection = null;
            Collection<HL7II> groupIdCollection = null;

            while (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
            {
                if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                {
                    if (idCollection == null)
                    {
                        idCollection = new Collection<HL7II>();
                    }

                    HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                    idCollection.Add(id);
                }
            }

            if (reader.IsStartElement(HL7Constants.Elements.Group, HL7Constants.Namespace))
            {
                reader.ReadStartElement(HL7Constants.Elements.Group, HL7Constants.Namespace);

                if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                {
                    if (groupIdCollection == null)
                    {
                        groupIdCollection = new Collection<HL7II>();
                    }

                    HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                    groupIdCollection.Add(id);
                }

                EndElement(reader);
            }

            EndElement(reader);

            return new HL7AsMember(idCollection, groupIdCollection);
        }

        /// <summary>
        /// Reads the assigned device.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7Assigned Device</returns>
        protected virtual HL7AssignedDevice ReadAssignedDevice(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }
            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    string classCode = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                    if (string.IsNullOrEmpty(classCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson));
                    }

                    if (classCode != HL7Constants.AttributesValue.Assigned)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson, HL7Constants.AttributesValue.Assigned));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace);

                    if (reader.IsStartElement(HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace))
                    {
                        string classCodeA = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                        if (string.IsNullOrEmpty(classCodeA))
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedDevice));
                        }

                        if (classCodeA != HL7Constants.AttributesValue.Dev)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedDevice, HL7Constants.AttributesValue.Dev));
                        }

                        string determinerCode = reader.GetAttribute(HL7Constants.Attributes.DeterminerCode);

                        if (string.IsNullOrEmpty(determinerCode))
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.DeterminerCode, HL7Constants.Elements.AssignedDevice));
                        }

                        if (determinerCode != HL7Constants.AttributesValue.Instance)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.DeterminerCode, HL7Constants.Elements.AssignedDevice, HL7Constants.AttributesValue.Instance));
                        }

                        reader.ReadStartElement(HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace);
                    }
                    else
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.AssignedPerson));
                    }

                    HL7II ident = null;
                    if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                    {
                        ident = this.ReadId(reader, HL7Constants.Elements.Id);
                    }

                    ICollection<HL7AsMember> asMembers = new Collection<HL7AsMember>();

                    while (reader.IsStartElement(HL7Constants.Elements.AsMember, HL7Constants.Namespace))
                    {
                        HL7AsMember asMember = this.ReadAsMember(reader);

                        if (asMember != null)
                        {
                            asMembers.Add(asMember);
                        }
                    }

                    EndElement(reader);
                    HL7RepresentedOrganization org = this.ReadRepresentedOrganization(reader);

                    HL7AssignedDevice device = new HL7AssignedDevice(ident, asMembers, org);

                    return device;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the assigned person.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>person information</returns>
        protected virtual HL7AssignedPerson ReadAssignedPerson(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    string classCode = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                    if (string.IsNullOrEmpty(classCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson));
                    }

                    if (classCode != HL7Constants.AttributesValue.Assigned)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson, HL7Constants.AttributesValue.Assigned));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace);

                    HL7PersonId personId = null;
                    if (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                    {
                        HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                        personId = new HL7PersonId(id.Extension);
                    }

                    HL7ClassificatorId code = null;

                    if (reader.IsStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace))
                    {
                        string codeCode = reader.GetAttribute(HL7Constants.Attributes.Code);
                        string codeCodeSystem = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                        if (!OId.IsOId(codeCodeSystem))
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.Code + " in " + HL7Constants.Elements.AssignedPerson));
                        }

                        code = new HL7ClassificatorId(codeCode, new OId(codeCodeSystem));

                        // code
                        reader.ReadStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace);
                        EndElement(reader);
                    }

                    ICollection<string> telecoms = new Collection<string>();

                    while (reader.IsStartElement(HL7Constants.Elements.Telecom, HL7Constants.Namespace))
                    {
                        string telecom = this.ReadTelecom(reader);
                        telecoms.Add(telecom);
                    }

                    if (reader.IsStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace))
                    {
                        string classCodeA = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                        if (string.IsNullOrEmpty(classCodeA))
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson));
                        }

                        if (classCodeA != HL7Constants.AttributesValue.Psn)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.AssignedPerson, HL7Constants.AttributesValue.Psn));
                        }

                        string determinerCode = reader.GetAttribute(HL7Constants.Attributes.DeterminerCode);

                        if (string.IsNullOrEmpty(determinerCode))
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.DeterminerCode, HL7Constants.Elements.AssignedPerson));
                        }

                        if (determinerCode != HL7Constants.AttributesValue.Instance)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.DeterminerCode, HL7Constants.Elements.AssignedPerson, HL7Constants.AttributesValue.Instance));
                        }

                        reader.ReadStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace);
                    }
                    else
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.AssignedPerson));
                    }

                    ICollection<HL7Person> persons = new Collection<HL7Person>();

                    while (reader.IsStartElement(HL7Constants.Elements.Name, HL7Constants.Namespace))
                    {
                        HL7Person person = this.ReadAssignedPersonName(reader);

                        if (person != null)
                        {
                            persons.Add(person);
                        }
                    }

                    ICollection<HL7AsLicensedEntity> asLicensedEntityList = new Collection<HL7AsLicensedEntity>();

                    while (reader.IsStartElement(HL7Constants.Elements.AsLicensedEntity, HL7Constants.Namespace))
                    {
                        HL7AsLicensedEntity asLicensedEntity = this.ReadAsLicenceEntity(reader);

                        if (asLicensedEntityList != null)
                        {
                            asLicensedEntityList.Add(asLicensedEntity);
                        }
                    }

                    ICollection<HL7AsMember> asMembers = new Collection<HL7AsMember>();

                    while (reader.IsStartElement(HL7Constants.Elements.AsMember, HL7Constants.Namespace))
                    {
                        HL7AsMember asMember = this.ReadAsMember(reader);

                        if (asMember != null)
                        {
                            asMembers.Add(asMember);
                        }
                    }

                    // AssignedPerson
                    EndElement(reader);

                    HL7RepresentedOrganization org = this.ReadRepresentedOrganization(reader);

                    // AssignedPerson
                    EndElement(reader);

                    return new HL7AssignedPerson(personId, code, persons, telecoms, asMembers, org, asLicensedEntityList);
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the name of the assigned person.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7 Person</returns>
        protected virtual HL7Person ReadAssignedPersonName(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.Name, HL7Constants.Namespace))
            {
                return null;
            }

            reader.ReadStartElement(HL7Constants.Elements.Name, HL7Constants.Namespace);

            ICollection<string> givenName = new Collection<string>();
            ICollection<string> familyName = new Collection<string>();
            ICollection<HL7AsMember> asMember = new Collection<HL7AsMember>();

            while (reader.IsStartElement(HL7Constants.Elements.Delimiter, HL7Constants.Namespace) ||
                reader.IsStartElement(HL7Constants.Elements.Family, HL7Constants.Namespace) ||
                reader.IsStartElement(HL7Constants.Elements.Given, HL7Constants.Namespace) ||
                reader.IsStartElement(HL7Constants.Elements.Prefix, HL7Constants.Namespace) ||
                reader.IsStartElement(HL7Constants.Elements.Suffix, HL7Constants.Namespace))
            {
                string family = null;

                if (reader.IsStartElement(HL7Constants.Elements.Family, HL7Constants.Namespace))
                {
                    family = reader.ReadElementContentAsString(HL7Constants.Elements.Family, HL7Constants.Namespace);

                    familyName.Add(family);
                }

                if (reader.IsStartElement(HL7Constants.Elements.Given, HL7Constants.Namespace))
                {
                    string given = reader.ReadElementContentAsString(HL7Constants.Elements.Given, HL7Constants.Namespace);
                    givenName.Add(given);
                }
            }

            EndElement(reader);

            HL7Person person = new HL7Person(givenName, familyName);

            return person;
        }

        /// <summary>
        /// Reads the attention line.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>
        /// Attention Line
        /// </returns>
        protected virtual HL7AttentionLine ReadAttentionLine(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.AttentionLine, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, localName));
            }
            else
            {
                reader.ReadStartElement(localName, HL7Constants.Namespace);

                if (reader.IsStartElement(HL7Constants.Elements.KeywordText, HL7Constants.Namespace))
                {
                    string keyWordText = reader.ReadElementContentAsString(HL7Constants.Elements.KeywordText, HL7Constants.Namespace);

                    if (reader.IsStartElement())
                    {
                        // TODO: value
                        HL7II id = this.ReadId(reader, HL7Constants.Elements.Value);
                        HL7AttentionLine attentionLine = new HL7AttentionLine(keyWordText, id);

                        EndElement(reader);

                        return attentionLine;
                    }
                    else
                    {
                        HL7AttentionLine attentionLine = new HL7AttentionLine(keyWordText);

                        EndElement(reader);

                        return attentionLine;
                    }
                }
                else
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.KeywordText));
                }
            }
        }

        /// <summary>
        /// Reads the author or performer.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7 AuthorOrPerformer</returns>
        protected virtual HL7AuthorOrPerformer ReadAuthorOrPerformer(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.AuthorOrPerformer, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    reader.ReadStartElement(HL7Constants.Elements.AuthorOrPerformer, HL7Constants.Namespace);

                    DateTime? time = null;

                    if (reader.IsStartElement(HL7Constants.Elements.Time, HL7Constants.Namespace))
                    {
                        time = this.ReadTime(reader, HL7Constants.Elements.Time);
                    }

                    string signatureCode = null;
                    string signatureText = null;

                    if (reader.IsStartElement(HL7Constants.Elements.SignatureCode, HL7Constants.Namespace))
                    {
                        signatureCode = reader.ReadElementContentAsString(HL7Constants.Elements.SignatureCode, HL7Constants.Namespace);
                    }

                    if (reader.IsStartElement(HL7Constants.Elements.SignatureText, HL7Constants.Namespace))
                    {
                        signatureText = reader.ReadElementContentAsString(HL7Constants.Elements.SignatureText, HL7Constants.Namespace);
                    }

                    HL7AuthorOrPerformer authorOrPerformer;

                    if (reader.IsStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace))
                    {
                        HL7AssignedPerson person = this.ReadAssignedPerson(reader);

                        if (time.HasValue)
                        {
                            authorOrPerformer = new HL7AuthorOrPerformer(time.Value, signatureCode, signatureText, person);
                        }
                        else
                        {
                            authorOrPerformer = new HL7AuthorOrPerformer(signatureCode, signatureText, person);
                        }
                    }
                    else
                        if (reader.IsStartElement(HL7Constants.Elements.AssignedDevice, HL7Constants.Namespace))
                        {
                            HL7AssignedDevice device = this.ReadAssignedDevice(reader);

                            if (time.HasValue)
                            {
                                authorOrPerformer = new HL7AuthorOrPerformer(time.Value, signatureCode, signatureText, device);
                            }
                            else
                            {
                                authorOrPerformer = new HL7AuthorOrPerformer(signatureCode, signatureText, device);
                            }

                            EndElement(reader);
                        }
                        else
                        {
                            authorOrPerformer = null;
                        }

                    // AuthorOrPerformer (2)
                    EndElement(reader);

                    return authorOrPerformer;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the code element.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Code Element</returns>
        protected virtual string ReadCodeElement(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(localName, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, localName));
            }
            else
            {
                string value = reader.GetAttribute(HL7Constants.Attributes.Code);

                if (string.IsNullOrEmpty(value))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.Code));
                }

                reader.ReadStartElement(localName, HL7Constants.Namespace);

                EndElement(reader);

                return value;
            }
        }

        /// <summary>
        /// Reads the control act.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Message ControlAct</returns>
        protected virtual HL7ControlAct ReadControlAct(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.ControlActProcess, HL7Constants.Namespace))
            {
                // throw new XmlException("This element must starts with:" + localName);
                return null;
            }
            else
            {
                string classCode = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                if (string.IsNullOrEmpty(classCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.ClassCode));
                }

                if (classCode != HL7Constants.AttributesValue.ControlAct)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, localName, HL7Constants.AttributesValue.ControlAct));
                }

                string moodCode = reader.GetAttribute(HL7Constants.Attributes.MoodCode);

                if (string.IsNullOrEmpty(moodCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.MoodCode));
                }

                if (moodCode != HL7Constants.AttributesValue.Event)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, localName, HL7Constants.AttributesValue.Event));
                }

                if (reader.IsStartElement(HL7Constants.Elements.ControlActProcess, HL7Constants.Namespace))
                {
                    reader.ReadStartElement(HL7Constants.Elements.ControlActProcess, HL7Constants.Namespace);
                }

                HL7ClassificatorId codeClassificator = null;

                if (reader.IsStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace))
                {
                    string code = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(code))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.Code, HL7Constants.Attributes.Code));
                    }

                    string codeSystem = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                    if (string.IsNullOrEmpty(codeSystem))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.Code, HL7Constants.Attributes.CodeSystem));
                    }

                    if (!OId.IsOId(codeSystem))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.Code));
                    }

                    codeClassificator = new HL7ClassificatorId(code, new OId(codeSystem));

                    // TODO: code check
                    reader.ReadStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace);

                    EndElement(reader);
                }

                string text = null;

                if (reader.IsStartElement(HL7Constants.Elements.Text, HL7Constants.Namespace))
                {
                    text = reader.ReadElementContentAsString(HL7Constants.Elements.Text, HL7Constants.Namespace);
                }

                DateTime? effectiveTime = null;

                if (reader.IsStartElement(HL7Constants.Elements.EffectiveTime, HL7Constants.Namespace))
                {
                    // string value = reader.GetAttribute(HL7Constants.Attributes.Value);
                    // if (string.IsNullOrEmpty(value))
                    // {
                    //    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.EffectiveTime, HL7Constants.Attributes.Value));
                    // }

                    // reader.ReadStartElement(HL7Constants.Elements.EffectiveTime, HL7Constants.Namespace);
                    // EndElement(reader);
                    // creationTime = DateTime.ParseExact(value, HL7Constants.Formats.DateTimeFormat, CultureInfo.InvariantCulture);
                    effectiveTime = this.ReadTime(reader, HL7Constants.Elements.EffectiveTime);
                }

                HL7ClassificatorId priorityCodeclassificator = null;

                if (reader.IsStartElement(HL7Constants.Elements.PriorityCode, HL7Constants.Namespace))
                {
                    string codePriority = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(codePriority))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.PriorityCode, HL7Constants.Attributes.Code));
                    }

                    var codeSystemPriority = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                    if (string.IsNullOrEmpty(codeSystemPriority))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.PriorityCode, HL7Constants.Attributes.CodeSystem));
                    }

                    if (!OId.IsOId(codeSystemPriority))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.PriorityCode));
                    }

                    priorityCodeclassificator = new HL7ClassificatorId(codePriority, new OId(codeSystemPriority));
                    reader.ReadStartElement(HL7Constants.Elements.PriorityCode, HL7Constants.Namespace);

                    EndElement(reader);
                }

                ICollection<HL7ClassificatorId> reasonCodeClassificator = new Collection<HL7ClassificatorId>();
                while (reader.IsStartElement(HL7Constants.Elements.ReasonCode, HL7Constants.Namespace))
                {
                    HL7ClassificatorId reasonCode = this.ReadReasonCode(reader);
                    reasonCodeClassificator.Add(reasonCode);
                }

                HL7ClassificatorId languageCodeClassificator = null;

                if (reader.IsStartElement(HL7Constants.Elements.LanguageCode, HL7Constants.Namespace))
                {
                    string codeLanguageCode = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(codeLanguageCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.LanguageCode, HL7Constants.Attributes.Code));
                    }

                    var codeSystemLanguageCode = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                    if (string.IsNullOrEmpty(codeSystemLanguageCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.LanguageCode, HL7Constants.Attributes.CodeSystem));
                    }

                    if (!OId.IsOId(codeSystemLanguageCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.LanguageCode));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.LanguageCode, HL7Constants.Namespace);

                    EndElement(reader);

                    languageCodeClassificator = new HL7ClassificatorId(codeLanguageCode, new OId(codeSystemLanguageCode));
                }

                ICollection<HL7Overseer> overseers = new Collection<HL7Overseer>();
                while (reader.IsStartElement(HL7Constants.Elements.Overseer, HL7Constants.Namespace))
                {
                    var overseer = this.ReadOverseer(reader);
                    overseers.Add(overseer);
                }

                Collection<HL7AuthorOrPerformer> authorOrPerformerCol = new Collection<HL7AuthorOrPerformer>();

                while (reader.IsStartElement(HL7Constants.Elements.AuthorOrPerformer, HL7Constants.Namespace))
                {
                    var authorOrPerformer = this.ReadAuthorOrPerformer(reader);
                    authorOrPerformerCol.Add(authorOrPerformer);
                }

                Collection<HL7DataEnterer> dataEntererCol = new Collection<HL7DataEnterer>();

                while (reader.IsStartElement(HL7Constants.Elements.DataEnterer, HL7Constants.Namespace))
                {
                    var dataEnterer = this.ReadDataEnter(reader);
                    dataEntererCol.Add(dataEnterer);
                }

                Collection<HL7InformationRecipient> informationRecipientCol = new Collection<HL7InformationRecipient>();

                while (reader.IsStartElement(HL7Constants.Elements.InformationRecipient, HL7Constants.Namespace))
                {
                    var informationRecipient = this.ReadInformationRecipient(reader);
                    informationRecipientCol.Add(informationRecipient);
                }

                HL7Subject subject = null;

                if (reader.IsStartElement(HL7Constants.Elements.Subject, HL7Constants.Namespace))
                {
                    subject = this.ReadSubject(reader);
                }

                HL7QueryAcknowledgement queryAck = null;
                if (reader.IsStartElement(HL7Constants.Elements.QueryAcknowledgement, HL7Constants.Namespace))
                {
                    queryAck = this.ReadQueryAcknowledgement(reader);
                }

                HL7QueryByParameterPayload queryByParameterPayload = null;

                if (reader.IsStartElement(HL7Constants.Elements.QueryByParameterPayload, HL7Constants.Namespace))
                {
                    queryByParameterPayload = this.ReadQueryByParameterPayload(reader);
                }

                HL7QueryContinuation queryContinuation = null;
                if (reader.IsStartElement(HL7Constants.Elements.QueryContinuation, HL7Constants.Namespace))
                {
                    queryContinuation = this.ReadQueryContinuation(reader);
                }

                if (subject != null && queryAck == null)
                {
                    HL7MessageControlAct messCackt = new HL7MessageControlAct(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, subject);

                    return messCackt;
                }

                if (subject == null && queryAck != null)
                {
                    HL7QueryControlAcknowledgement messCackt = new HL7QueryControlAcknowledgement(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, queryAck);

                    return messCackt;
                }

                if (subject != null && queryAck != null)
                {
                    HL7QueryControlAcknowledgement queryControlAct = new HL7QueryControlAcknowledgement(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, subject, queryAck);

                    return queryControlAct;
                }

                if (subject == null && queryAck == null && queryContinuation == null && queryByParameterPayload != null)
                {
                    HL7QueryControlAcknowledgement queryControlAct = new HL7QueryControlAcknowledgement(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, queryByParameterPayload);

                    return queryControlAct;
                }

                if (subject == null && queryAck == null && queryByParameterPayload == null && queryContinuation != null)
                {
                    HL7QueryControlAcknowledgement queryControlAct = new HL7QueryControlAcknowledgement(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, queryContinuation);

                    return queryControlAct;
                }

                if (subject == null && queryAck == null && queryByParameterPayload == null && queryContinuation == null)
                {
                    HL7MessageControlAct messCackt = new HL7MessageControlAct(overseers, dataEntererCol, authorOrPerformerCol, informationRecipientCol, codeClassificator, text, effectiveTime, priorityCodeclassificator, reasonCodeClassificator, languageCodeClassificator, null);

                    return messCackt;
                }

                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.UnknownMessageType));
            }
        }

        /// <summary>
        /// Reads the data enter.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Data Enterer</returns>
        protected virtual HL7DataEnterer ReadDataEnter(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.DataEnterer, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    string typeCode = reader.GetAttribute(HL7Constants.Attributes.TypeCode);

                    if (string.IsNullOrEmpty(typeCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.DataEnterer, HL7Constants.Attributes.TypeCode));
                    }

                    if (typeCode != HL7Constants.AttributesValue.Entity)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.TypeCode, HL7Constants.Elements.DataEnterer, HL7Constants.AttributesValue.Entity));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.DataEnterer, HL7Constants.Namespace);

                    DateTime? time = null;

                    if (reader.IsStartElement(HL7Constants.Elements.Time, HL7Constants.Namespace))
                    {
                        time = this.ReadTime(reader, HL7Constants.Elements.Time);
                    }

                    var person = this.ReadAssignedPerson(reader);

                    EndElement(reader);
                    HL7DataEnterer dataEnterer = new HL7DataEnterer(time, person);
                    return dataEnterer;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the device.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Device value</returns>
        protected virtual HL7Device ReadDevice(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(localName, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, localName));
            }
            else
            {
                string typeCode = reader.GetAttribute(HL7Constants.Attributes.TypeCode);

                if (string.IsNullOrEmpty(typeCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.TypeCode));
                }

                switch (localName)
                {
                    case HL7Constants.Elements.Receiver:
                        if (typeCode != HL7Constants.AttributesValue.Receiver)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.TypeCode, localName, HL7Constants.AttributesValue.Receiver));
                        }
                        else
                        {
                            reader.ReadStartElement(HL7Constants.Elements.Receiver, HL7Constants.Namespace);
                        }

                        break;

                    case HL7Constants.Elements.Sender:
                        if (typeCode != HL7Constants.AttributesValue.Sender)
                        {
                            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.TypeCode, localName, HL7Constants.AttributesValue.Sender));
                        }
                        else
                        {
                            reader.ReadStartElement(HL7Constants.Elements.Sender, HL7Constants.Namespace);
                        }

                        break;

                    default:
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.ClassCode, localName));
                }

                if (reader.IsStartElement(HL7Constants.Elements.Device, HL7Constants.Namespace))
                {
                    string classCode = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                    if (classCode != HL7Constants.AttributesValue.Dev)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, localName, HL7Constants.AttributesValue.Dev));
                    }

                    string determinerCode = reader.GetAttribute(HL7Constants.Attributes.DeterminerCode);

                    if (determinerCode != HL7Constants.AttributesValue.Instance)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.DeterminerCode, localName, HL7Constants.AttributesValue.Instance));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.Device, HL7Constants.Namespace);

                 // EndElement(reader);
                }
                else
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.Device));
                }

                HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);

                HL7Device device = new HL7Device(id, typeCode);

                EndElement(reader);
                EndElement(reader);

                return device;
            }
        }

        /// <summary>
        /// Reads the information recipient.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7 InformationRecipient</returns>
        protected virtual HL7InformationRecipient ReadInformationRecipient(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.InformationRecipient, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    string typeCode = reader.GetAttribute(HL7Constants.Attributes.TypeCode);

                    if (string.IsNullOrEmpty(typeCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.DataEnterer, HL7Constants.Attributes.TypeCode));
                    }

                    if (typeCode != HL7Constants.AttributesValue.ParticipationInformationRecipient)
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.TypeCode, HL7Constants.Elements.DataEnterer, HL7Constants.AttributesValue.ParticipationInformationRecipient));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.InformationRecipient, HL7Constants.Namespace);

                    DateTime? time = null;
                    if (reader.IsStartElement(HL7Constants.Elements.Time, HL7Constants.Namespace))
                    {
                        time = this.ReadTime(reader, HL7Constants.Elements.Time);
                    }

                    var person = this.ReadAssignedPerson(reader);

                    EndElement(reader);

                    HL7InformationRecipient informationRecipient = new HL7InformationRecipient(time, person);
                    return informationRecipient;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the author or performer.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7 AuthorOrPerformer</returns>
        protected virtual HL7Overseer ReadOverseer(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.Overseer, HL7Constants.Namespace))
                {
                    return null;
                }
                else
                {
                    reader.ReadStartElement(HL7Constants.Elements.Overseer, HL7Constants.Namespace);

                    DateTime? time = null;

                    if (reader.IsStartElement(HL7Constants.Elements.Time, HL7Constants.Namespace))
                    {
                        time = this.ReadTime(reader, HL7Constants.Elements.Time);
                    }

                    string signatureCode = null;
                    string signatureText = null;

                    if (reader.IsStartElement(HL7Constants.Elements.SignatureCode, HL7Constants.Namespace))
                    {
                        signatureCode = reader.ReadElementContentAsString(HL7Constants.Elements.SignatureCode, HL7Constants.Namespace);
                    }

                    if (reader.IsStartElement(HL7Constants.Elements.SignatureText, HL7Constants.Namespace))
                    {
                        signatureText = reader.ReadElementContentAsString(HL7Constants.Elements.SignatureText, HL7Constants.Namespace);
                    }

                    HL7Overseer overseer = null;

                    if (reader.IsStartElement(HL7Constants.Elements.AssignedPerson, HL7Constants.Namespace))
                    {
                        HL7AssignedPerson person = this.ReadAssignedPerson(reader);

                        if (time.HasValue)
                        {
                            overseer = new HL7Overseer(time.Value, signatureCode, signatureText, person);
                        }
                        else
                        {
                            overseer = new HL7Overseer(signatureCode, signatureText, person);
                        }
                    }

                    // AuthorOrPerformer (2)
                    EndElement(reader);

                    return overseer;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the processing code.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>HL7 Processing Code</returns>
        protected virtual HL7ProcessingCode ReadProcessingCode(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.ProcessingCode, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.ProcessingCode));
            }
            else
            {
                HL7ProcessingCode processingCode = HL7.HL7ProcessingCode.Production;
                string code = this.ReadCodeElement(reader, HL7Constants.Elements.ProcessingCode);

                switch (code)
                {
                    case HL7Constants.AttributesValue.Production:
                        processingCode = HL7.HL7ProcessingCode.Production;
                        break;

                    case HL7Constants.AttributesValue.Test:
                        processingCode = HL7.HL7ProcessingCode.Test;
                        break;

                    case HL7Constants.AttributesValue.Debug:
                        processingCode = HL7.HL7ProcessingCode.Debug;
                        break;

                    default:
                        break;
                }

                return processingCode;
            }
        }

        /// <summary>
        /// Reads the processing mode code.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>HL7 Processing Mode Code</returns>
        protected virtual HL7ProcessingModeCode ReadProcessingModeCode(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.ProcessingModeCode, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.ProcessingModeCode));
            }
            else
            {
                HL7ProcessingModeCode processingModeCode = HL7.HL7ProcessingModeCode.Archival;
                string code = this.ReadCodeElement(reader, HL7Constants.Elements.ProcessingModeCode);

                switch (code)
                {
                    case HL7Constants.AttributesValue.Archival:
                        processingModeCode = HL7.HL7ProcessingModeCode.Archival;
                        break;

                    case HL7Constants.AttributesValue.InicialLoad:
                        processingModeCode = HL7.HL7ProcessingModeCode.InicialLoad;
                        break;

                    case HL7Constants.AttributesValue.ArchiveRestoration:
                        processingModeCode = HL7.HL7ProcessingModeCode.ArchiveRestoration;
                        break;

                    case HL7Constants.AttributesValue.OperationData:
                        processingModeCode = HL7.HL7ProcessingModeCode.OperationData;
                        break;

                    default:
                        break;
                }

                return processingModeCode;
            }
        }

        /// <summary>
        /// Reads the query ack.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Message HL7QueryAck</returns>
        protected virtual HL7QueryAcknowledgement ReadQueryAcknowledgement(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.QueryAcknowledgement, HL7Constants.Namespace))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.QueryAcknowledgement));
                }
                else
                {
                    reader.ReadStartElement(HL7Constants.Elements.QueryAcknowledgement, HL7Constants.Namespace);

                    EndElement(reader);

                    HL7II queryId = null;

                    if (reader.IsStartElement(HL7Constants.Elements.QueryId, HL7Constants.Namespace))
                    {
                        queryId = this.ReadId(reader, HL7Constants.Elements.QueryId);
                    }

                    HL7StatusCode statusCode = null;

                    if (reader.IsStartElement(HL7Constants.Elements.StatusCode, HL7Constants.Namespace))
                    {
                        string code = reader.GetAttribute(HL7Constants.Attributes.Code);
                        Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes codeData = HL7StatusCode.HL7StatusCodes.New;

                        switch (code)
                        {
                            case HL7Constants.AttributesValue.Aborted:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.Aborted;
                                break;

                            case HL7Constants.AttributesValue.DeliveredResponse:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.DeliveredResponse;
                                break;

                            case HL7Constants.AttributesValue.Executing:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.Executing;
                                break;

                            case HL7Constants.AttributesValue.New:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.New;
                                break;

                            case HL7Constants.AttributesValue.WaitContinuedQueryResponse:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.WaitContinuedQueryResponse;
                                break;

                            default:
                                codeData = Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes.New;
                                break;
                        }

                        statusCode = new HL7StatusCode(codeData);

                        reader.ReadStartElement(HL7Constants.Elements.StatusCode, HL7Constants.Namespace);

                        EndElement(reader);
                    }

                    HL7QueryResponseCode queryResponseCode = null;

                    if (reader.IsStartElement(HL7Constants.Elements.QueryResponseCode, HL7Constants.Namespace))
                    {
                        string code = reader.GetAttribute(HL7Constants.Attributes.Code);
                        Abc.ServiceModel.Protocol.HL7.HL7QueryResponseCode.HL7QueryResponseCodes codeData;

                        switch (code)
                        {
                            case HL7Constants.AttributesValue.ApplicationError:
                                codeData = HL7QueryResponseCode.HL7QueryResponseCodes.ApplicationError;
                                break;

                            case HL7Constants.AttributesValue.NoDataFound:
                                codeData = HL7QueryResponseCode.HL7QueryResponseCodes.NoDataFound;
                                break;

                            case HL7Constants.AttributesValue.DataFound:
                                codeData = HL7QueryResponseCode.HL7QueryResponseCodes.DataFound;
                                break;

                            case HL7Constants.AttributesValue.QueryParameterError:
                                codeData = HL7QueryResponseCode.HL7QueryResponseCodes.QueryParameterError;
                                break;

                            default: codeData = HL7QueryResponseCode.HL7QueryResponseCodes.DataFound;
                                break;
                        }

                        queryResponseCode = new HL7QueryResponseCode(codeData);
                        reader.ReadStartElement(HL7Constants.Elements.QueryResponseCode, HL7Constants.Namespace);

                        EndElement(reader);
                    }

                    int? resultTotalQuantity = null;

                    if (reader.IsStartElement(HL7Constants.Elements.ResultTotalQuantity, HL7Constants.Namespace))
                    {
                        int tempInt = -1;
                        bool pars = int.TryParse(reader.GetAttribute(HL7Constants.Attributes.Value), out tempInt);

                        if (pars && tempInt >= 0)
                        {
                            resultTotalQuantity = tempInt;
                        }

                        reader.ReadStartElement(HL7Constants.Elements.ResultTotalQuantity, HL7Constants.Namespace);
                    }

                    int? resultCurrentQuantity = null;

                    if (reader.IsStartElement(HL7Constants.Elements.ResultCurrentQuantity, HL7Constants.Namespace))
                    {
                        int tempInt = -1;
                        bool pars = int.TryParse(reader.GetAttribute(HL7Constants.Attributes.Value), out tempInt);

                        if (pars && tempInt >= 0)
                        {
                            resultCurrentQuantity = tempInt;
                        }

                        reader.ReadStartElement(HL7Constants.Elements.ResultCurrentQuantity, HL7Constants.Namespace);
                    }

                    int? resultRemainingQuantity = null;

                    if (reader.IsStartElement(HL7Constants.Elements.ResultRemainingQuantity, HL7Constants.Namespace))
                    {
                        int tempInt = -1;
                        bool pars = int.TryParse(reader.GetAttribute(HL7Constants.Attributes.Value), out tempInt);

                        if (pars && tempInt >= 0)
                        {
                            resultRemainingQuantity = tempInt;
                        }

                        reader.ReadStartElement(HL7Constants.Elements.ResultRemainingQuantity, HL7Constants.Namespace);
                    }

                    HL7QueryAcknowledgement queryAck = new HL7QueryAcknowledgement(queryId, statusCode, queryResponseCode, resultTotalQuantity, resultCurrentQuantity, resultRemainingQuantity);
                    return queryAck;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);

                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the reason code.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>HL7Classificator Identificator</returns>
        protected virtual HL7ClassificatorId ReadReasonCode(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.ReasonCode, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.ReasonCode));
            }
            else
            {
                string codeReasonCode = reader.GetAttribute(HL7Constants.Attributes.Code);

                if (string.IsNullOrEmpty(codeReasonCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.ReasonCode, HL7Constants.Attributes.Code));
                }

                var codeSystemReasonCode = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                if (string.IsNullOrEmpty(codeSystemReasonCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.ReasonCode, HL7Constants.Attributes.CodeSystem));
                }

                if (!OId.IsOId(codeSystemReasonCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.ReasonCode));
                }

                if (OId.Compare(new OId(codeSystemReasonCode), HL7Constants.OIds.ReasonCodeId) != 0 && OId.Compare(new OId(codeSystemReasonCode), HL7Constants.OIds.ActionCodeId) != 0)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.ReasonCode));
                }

                HL7ClassificatorId reasonCodeClassificator = new HL7ClassificatorId(codeReasonCode, new OId(codeSystemReasonCode));
                reader.ReadStartElement(HL7Constants.Elements.ReasonCode, HL7Constants.Namespace);

                EndElement(reader);

                return reasonCodeClassificator;
            }
        }

        /// <summary>
        /// Reads the represented organization.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Represented Organization</returns>
        protected virtual HL7RepresentedOrganization ReadRepresentedOrganization(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.RepresentedOrganization, HL7Constants.Namespace))
            {
                return null;
            }
            else
            {
                string classCode = reader.GetAttribute(HL7Constants.Attributes.ClassCode);

                if (string.IsNullOrEmpty(classCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.RepresentedOrganization));
                }

                if (classCode != HL7Constants.AttributesValue.Organization)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.RepresentedOrganization, HL7Constants.AttributesValue.Organization));
                }

                string determinerCode = reader.GetAttribute(HL7Constants.Attributes.DeterminerCode);

                if (string.IsNullOrEmpty(determinerCode))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Attributes.DeterminerCode, HL7Constants.Elements.RepresentedOrganization));
                }

                if (classCode != HL7Constants.AttributesValue.Organization)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ResultIsNot, HL7Constants.Attributes.ClassCode, HL7Constants.Elements.RepresentedOrganization, HL7Constants.AttributesValue.Instance));
                }

                reader.ReadStartElement(HL7Constants.Elements.RepresentedOrganization, HL7Constants.Namespace);

                Collection<HL7II> ids = new Collection<HL7II>();

                while (reader.IsStartElement(HL7Constants.Elements.Id, HL7Constants.Namespace))
                {
                    HL7II id = this.ReadId(reader, HL7Constants.Elements.Id);
                    ids.Add(id);
                }

                HL7ClassificatorId codeClassificator = null;

                if (reader.IsStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace))
                {
                    string codeCode = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(codeCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.Code, HL7Constants.Attributes.Code));
                    }

                    var codeSystemCode = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                    if (string.IsNullOrEmpty(codeSystemCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.Code, HL7Constants.Attributes.CodeSystem));
                    }

                    if (!OId.IsOId(codeSystemCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.Code));
                    }

                    // if (OId.Compare(new OId(codeSystemCode), HL7Constants.OIds.ReasonCodeId) != 0 && OId.Compare(new OId(codeSystemCode), HL7Constants.OIds.ActionCodeId) != 0)
                    // {
                    // throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.Code));
                    // }
                    codeClassificator = new HL7ClassificatorId(codeCode, new OId(codeSystemCode));
                    reader.ReadStartElement(HL7Constants.Elements.Code, HL7Constants.Namespace);
                    EndElement(reader);
                }

                Collection<string> names = new Collection<string>();

                while (reader.IsStartElement(HL7Constants.Elements.Name, HL7Constants.Namespace))
                {
                    string name = reader.ReadElementContentAsString(HL7Constants.Elements.Name, HL7Constants.Namespace);
                    names.Add(name);
                }

                string adress = null;

                if (reader.IsStartElement(HL7Constants.Elements.Addr, HL7Constants.Namespace))
                {
                    reader.ReadStartElement(HL7Constants.Elements.Addr, HL7Constants.Namespace);
                    adress = reader.ReadElementContentAsString(HL7Constants.Elements.StreetAddressLine, HL7Constants.Namespace);

                    EndElement(reader);
                }

                HL7ClassificatorId standardIndustryClassCode = null;

                if (reader.IsStartElement(HL7Constants.Elements.StandardIndustryClassCode, HL7Constants.Namespace))
                {
                    string standardIndustryClassCodeCode = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(standardIndustryClassCodeCode))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.StandardIndustryClassCode, HL7Constants.Attributes.Code));
                    }

                    var standardIndustryClassCodeCodeSystem = reader.GetAttribute(HL7Constants.Attributes.CodeSystem);

                    if (string.IsNullOrEmpty(standardIndustryClassCodeCodeSystem))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.StandardIndustryClassCode, HL7Constants.Attributes.CodeSystem));
                    }

                    if (!OId.IsOId(standardIndustryClassCodeCodeSystem))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NotOid, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.StandardIndustryClassCode));
                    }

                    // if (OId.Compare(new OId(standardIndustryClassCodeCodeSystem), HL7Constants.OIds.ReasonCodeId) != 0 && OId.Compare(new OId(standardIndustryClassCodeCodeSystem), HL7Constants.OIds.ActionCodeId) != 0)
                    // {
                    // throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Attributes.CodeSystem, HL7Constants.Elements.StandardIndustryClassCode));
                    // }
                    standardIndustryClassCode = new HL7ClassificatorId(standardIndustryClassCodeCode, new OId(standardIndustryClassCodeCodeSystem));
                    reader.ReadStartElement(HL7Constants.Elements.StandardIndustryClassCode, HL7Constants.Namespace);
                }

                HL7RepresentedOrganization representedOrganization = new HL7RepresentedOrganization(ids, codeClassificator, names, adress, standardIndustryClassCode);

                EndElement(reader);

                return representedOrganization;
            }
        }

        // protected virtual DateTime ReadAck(XmlReader reader, string localName)
        // {
        //    if (!reader.IsStartElement(localName, HL7Constants.Namespace))
        //    {
        //        throw new XmlException("This element must starts with:" + localName);
        //    }
        //    else
        //    {
        //        string value = reader.GetAttribute(HL7Constants.Attributes.Value);
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            throw new XmlException("CreationTime is null or empty");
        //        }
        //        reader.ReadStartElement(localName, HL7Constants.Namespace);
        //        return DateTime.ParseExact(value, HL7Constants.Formats.DateTimeFormat, CultureInfo.InvariantCulture);
        //    }
        // }
        // protected virtual DateTime ReadAck1(XmlReader reader, string localName)
        // {
        //    if (!reader.IsStartElement(localName, HL7Constants.Namespace))
        //    {
        //        throw new XmlException("This element must starts with:" + localName);
        //    }
        //    else
        //    {
        //        string value = reader.GetAttribute(HL7Constants.Attributes.Value);
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            throw new XmlException("CreationTime is null or empty");
        //        }
        //        reader.ReadStartElement(localName, HL7Constants.Namespace);
        //        return DateTime.ParseExact(value, HL7Constants.Formats.DateTimeFormat, CultureInfo.InvariantCulture);
        //    }
        // }

        /// <summary>
        /// Reads the sequence number.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>readed data</returns>
        protected virtual int? ReadSequenceNumber(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(localName, HL7Constants.Namespace))
            {
                return null;
            }
            else
            {
                int value = -1;
                bool pars = int.TryParse(reader.GetAttribute(HL7Constants.Attributes.Value), out value);

                if (!pars || value < 0)
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.SequenceNumber, HL7Constants.Attributes.Value));
                }

                reader.ReadStartElement(localName, HL7Constants.Namespace);

                EndElement(reader);

                return value;
            }
        }

        /// <summary>
        /// Reads the subject.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The subject.</returns>
        protected virtual HL7Subject ReadSubject(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            return HL7Subject.CreateSubject(reader);
        }

        /// <summary>
        /// Reads the telecom.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>telecom string</returns>
        protected virtual string ReadTelecom(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(HL7Constants.Elements.Telecom, HL7Constants.Namespace))
            {
                return null;
            }
            else
            {
                string value = reader.GetAttribute(HL7Constants.Attributes.Value);
                reader.ReadStartElement(HL7Constants.Elements.Telecom, HL7Constants.Namespace);

                EndElement(reader);

                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsIncorrect, HL7Constants.Elements.Value, HL7Constants.Elements.Telecom));
                }
            }
        }

        /// <summary>
        /// Reads the time.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>Date time</returns>
        protected virtual DateTime ReadTime(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (!reader.IsStartElement(localName, HL7Constants.Namespace))
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, localName));
            }
            else
            {
                string value = reader.GetAttribute(HL7Constants.Attributes.Value);

                if (string.IsNullOrEmpty(value))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.Value));
                }

                reader.ReadStartElement(localName, HL7Constants.Namespace);
                EndElement(reader);

                return Helper.TimeParsing(value, false);
            }
        }

        /// <summary>
        /// Reads the transmission wrapper.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Transmission Wrapper</returns>
        protected virtual HL7TransmissionWrapper ReadTransmissionWrapper(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                var templateId = new HL7TemplateId(this.ReadId(reader, HL7Constants.Elements.TemplateId));
                var identificationId = new HL7IdentificationId(this.ReadId(reader, HL7Constants.Elements.Id));
                var creationTime = this.ReadTime(reader, HL7Constants.Elements.CreationTime);
                var versionValue = this.ReadVersion(reader);
                var interactionId = new HL7InteractionId(this.ReadId(reader, HL7Constants.Elements.InteractionId));
                HL7ProcessingCode processingCode = this.ReadProcessingCode(reader, HL7Constants.Elements.ProcessingCode);

                // (HL7ProcessingCode)Enum.Parse(typeof(HL7ProcessingCode), this.ReadCodeElement(reader, HL7Constants.Elements.ProcessingCode));
                HL7ProcessingModeCode processingModeCode = this.ReadProcessingModeCode(reader, HL7Constants.Elements.ProcessingModeCode);

                // (HL7ProcessingModeCode)Enum.Parse(typeof(HL7ProcessingModeCode), this.ReadCodeElement(reader, HL7Constants.Elements.ProcessingModeCode));
                HL7AcceptAcknowledgementCode acceptAcknowledgementCode = this.ReadAcceptAcknowledgementCode(reader, HL7Constants.Elements.AcceptAcknowledgementCode);

                // (HL7AcceptAcknowledgementCode)Enum.Parse(typeof(HL7AcceptAcknowledgementCode), this.ReadCodeElement(reader, HL7Constants.Elements.AcceptAcknowledgementCode));
                var sequenceNumber = this.ReadSequenceNumber(reader, HL7Constants.Elements.SequenceNumber);
                var receiver = this.ReadDevice(reader, HL7Constants.Elements.Receiver);
                var sender = this.ReadDevice(reader, HL7Constants.Elements.Sender);
                Collection<HL7AttentionLine> col = new Collection<HL7AttentionLine>();

                while (reader.IsStartElement(HL7Constants.Elements.AttentionLine, HL7Constants.Namespace))
                {
                    HL7AttentionLine attentionLine = this.ReadAttentionLine(reader, HL7Constants.Elements.AttentionLine);
                    col.Add(attentionLine);
                }

                var acc = this.ReadAcknowledgement(reader, HL7Constants.Elements.Acknowledgement);
                var controlAct = this.ReadControlAct(reader, HL7Constants.Elements.ControlActProcess);

                return new HL7TransmissionWrapper(templateId, identificationId, versionValue, creationTime, interactionId, processingCode, processingModeCode, acceptAcknowledgementCode, sequenceNumber, sender, receiver, col, acc, controlAct);
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the version.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Version value</returns>
        protected virtual string ReadVersion(XmlReader reader)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(HL7Constants.Elements.VersionCode, HL7Constants.Namespace))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, HL7Constants.Elements.VersionCode));
                }
                else
                {
                    string value = reader.GetAttribute(HL7Constants.Attributes.Code);

                    if (string.IsNullOrEmpty(value))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, HL7Constants.Elements.VersionCode, HL7Constants.Attributes.Code));
                    }

                    if ((value != HL7Constants.Versions.V3NE2006 && this.version == HL7Version.HL7V3_2006)
                        && (value != HL7Constants.Versions.V3NE2011 && this.version == HL7Version.HL7V3_2011))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IsNot, HL7Constants.Elements.VersionCode, HL7Constants.Versions.V3NE2006.ToString()));
                    }

                    reader.ReadStartElement(HL7Constants.Elements.VersionCode, HL7Constants.Namespace);

                    EndElement(reader);

                    return value;
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);

                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Tries the wrap read exception.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>Exception value</returns>
        private static Exception TryWrapReadException(XmlReader reader, Exception innerException)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            if (innerException is FormatException
                || innerException is ArgumentException
                || innerException is InvalidOperationException
                || innerException is OverflowException)
            {
                // return DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2001, innerException);
                throw new XmlException("Serialization Exception", innerException);
            }

            return null;
        }

        private HL7AcknowledgementType CheckAcknowledgement(string acknowledgementType)
        {
            HL7AcknowledgementType code = HL7AcknowledgementType.ApplicationAcknowledgementAccept;

            switch (acknowledgementType)
            {
                case HL7Constants.AttributesValue.ApplicationAcknowledgementAccept:
                    code = HL7AcknowledgementType.ApplicationAcknowledgementAccept;
                    break;

                case HL7Constants.AttributesValue.ApplicationAcknowledgementError:
                    code = HL7AcknowledgementType.ApplicationAcknowledgementError;
                    break;

                case HL7Constants.AttributesValue.ApplicationAcknowledgementReject:
                    code = HL7AcknowledgementType.ApplicationAcknowledgementReject;
                    break;

                case HL7Constants.AttributesValue.AcceptAcknowledgementCommitAccept:
                    code = HL7AcknowledgementType.AcceptAcknowledgementCommitAccept;
                    break;

                case HL7Constants.AttributesValue.AcceptAcknowledgementCommitError:
                    code = HL7AcknowledgementType.AcceptAcknowledgementCommitError;
                    break;

                case HL7Constants.AttributesValue.AcceptAcknowledgementCommitReject:
                    code = HL7AcknowledgementType.AcceptAcknowledgementCommitReject;
                    break;

                default:
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ACkTypeCodeElementNotSet));
            }

            return code;
        }

        /// <summary>
        /// Reads the id.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns>identification value</returns>
        private HL7II ReadId(XmlReader reader, string localName)
        {
            if (reader == null) {  throw new ArgumentNullException("reader", "reader != null"); }

            try
            {
                if (!reader.IsStartElement(localName, HL7Constants.Namespace))
                {
                    throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.ElementStart, localName));
                }
                else
                {
                    string root = reader.GetAttribute(HL7Constants.Attributes.Root);

                    if (string.IsNullOrEmpty(root))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.Root));
                    }

                    string extension = reader.GetAttribute(HL7Constants.Attributes.Extension);
                    if (string.IsNullOrEmpty(extension))
                    {
                        throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.NullOrEmptyAtribute, localName, HL7Constants.Attributes.Extension));
                    }

                    reader.ReadStartElement(localName, HL7Constants.Namespace);

                    EndElement(reader);

                    return new HL7II(new OId(root), extension);
                }
            }
            catch (Exception exception)
            {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null)
                {
                    throw wrapedException;
                }

                throw;
            }
        }

        // public string Elments { get; set; }
    }
}