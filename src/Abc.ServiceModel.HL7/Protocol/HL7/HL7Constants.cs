// ----------------------------------------------------------------------------
// <copyright file="HL7Constants.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
#pragma warning disable 1692
#pragma warning disable EmptyGeneralCatchClause

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// HL7Constants class
    /// </summary>
    public sealed class HL7Constants
    {
        /// <summary>
        /// HL7 namespace
        /// </summary>
        public const string Namespace = "urn:hl7-org:v3";
        public const string NamespaceXsi = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// HL7 namespace prefix
        /// </summary>
        public const string Prefix = "hl7";

        /// <summary>
        /// Prevents a default instance of the <see cref="HL7Constants"/> class from being created.
        /// </summary>
        private HL7Constants()
        {
        }

        /// <summary>
        /// Action Codes
        /// </summary>
        public static class ActionCodes
        {
            /// <summary>
            /// Read  action code
            /// </summary>
            public const string Read = "READ";

            /// <summary>
            /// Request  action code
            /// </summary>
            public const string Request = "REQUEST";

            /// <summary>
            /// Response  action code
            /// </summary>
            public const string Response = "RESPONSE";

            /// <summary>
            /// Select  action code
            /// </summary>
            public const string Select = "SELECT";

            /// <summary>
            /// Write action code
            /// </summary>
            public const string Write = "WRITE";
        }

        /// <summary>
        /// Reason codes
        /// </summary>
        public static class ReasonCodes
        {
            /// <summary>
            /// Control And Inspection
            /// </summary>
            public const string ControlAndInspection = "CONTROL_AND_INSPECTION";

            /// <summary>
            /// Drug Treatment
            /// </summary>
            public const string DrugTreatment = "DRUG_TREATMENT";

            /// <summary>
            /// Du eRecord Or Refferal
            /// </summary>
            public const string DueRecordOrRefferal = "DUE_RECORD_OR_REFFERAL";

            /// <summary>
            /// Emergency code
            /// </summary>
            public const string Emergency = "EMERGENCY";

            /// <summary>
            /// Health Care Administration
            /// </summary>
            public const string HealthCareAdministration = "HEALTH_CARE_ADMINISTRATION";

            /// <summary>
            /// In Medical Treatment
            /// </summary>
            public const string InMedicalTreatment = "IN_MEDICAL_TREATMENT";

            /// <summary>
            /// On Patient Request
            /// </summary>
            public const string OnPatientRequest = "ON_PATIENT_REQUEST";

            /// <summary>
            /// Other code
            /// </summary>
            public const string Other = "OTHER";

            /// <summary>
            /// Scentific Research
            /// </summary>
            public const string ScentificResearch = "SCIENTIFIC_RESEARCH";

            /// <summary>
            /// With Patient Agreement
            /// </summary>
            public const string WithPatientAgreement = "WITH_PATIENT_AGREEMENT";
        }

        /// <summary>
        /// Actions constants
        /// </summary>
        public sealed class Actions
        {
            /// <summary>
            /// Accept Acknowledgement
            /// </summary>
            public const string AcceptAcknowledgement = HL7Constants.Namespace + ":AcceptAcknowledgement";

            /// <summary>
            /// Application Acknowledgement
            /// </summary>
            public const string ApplicationAcknowledgement = HL7Constants.Namespace + ":ApplicationAcknowledgement";

            /// <summary>
            /// Prevents a default instance of the <see cref="Actions"/> class from being created.
            /// </summary>
            private Actions()
            {
            }
        }

        /// <summary>
        /// Attributes constants
        /// </summary>
        public sealed class Attributes
        {
            /// <summary>
            /// atribute name
            /// </summary>
            public const string ClassCode = "classCode";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string Code = "code";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string CodeSystem = "codeSystem";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string DeterminerCode = "determinerCode";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string Extension = "extension";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string ItsVersion = "ITSVersion";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string MoodCode = "moodCode";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string Root = "root";

            /// <summary>
            /// The type
            /// </summary>
            public const string Type = "type";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string TypeCode = "typeCode";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string Use = "use";

            /// <summary>
            /// atribute name
            /// </summary>
            public const string Value = "value";

            /// <summary>
            /// Prevents a default instance of the <see cref="Attributes"/> class from being created.
            /// </summary>
            private Attributes()
            {
            }
        }

        /// <summary>
        /// AttributesValue constants
        /// </summary>
        public sealed class AttributesValue
        {
            /// <summary>
            /// atribute value
            /// </summary>
            public const string Aborted = "aborted";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string AcceptAcknowledgementCommitAccept = "CA";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string AcceptAcknowledgementCommitError = "CE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string AcceptAcknowledgementCommitReject = "CR";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Always = "AL";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ApplicationAcknowledgementAccept = "AA";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ApplicationAcknowledgementError = "AE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ApplicationAcknowledgementReject = "AR";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ApplicationError = "AE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Archival = "A";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ArchiveRestoration = "R";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Assigned = "ASSIGNED";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Author = "AUT";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Batch = "B";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Bolus = "T";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ControlAct = "CACT";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string DataFound = "OK";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Debug = "D";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string DeliveredResponse = "deliveredResponse";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Dev = "DEV";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Entity = "ENT";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Error = "ER";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Event = "EVN";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Executing = "executing";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string InicialLoad = "I";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Instance = "INSTANCE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string L = "L";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Mbr = "MBR";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ModifiedSubscription = "M";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Never = "NE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string New = "new";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string NewSubscription = "N";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string NoDataFound = "NF";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string OperationData = "T";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Organization = "ORG";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string ParticipationInformationRecipient = "PRCP";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Performer = "PRF";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Production = "P";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Psn = "PSN";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string QueryParameterError = "QE";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string RealTime = "R";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Receiver = "RCV";

            /// <summary>
            ///  atribute value
            /// </summary>
            public const string RGRP = "RGRP";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Sender = "SND";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Resp = "RESP";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Subject = "SUBJ";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string Test = "T";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string WaitContinuedQueryResponse = "waitContinuedQueryResponse";

            /// <summary>
            /// atribute value
            /// </summary>
            public const string XmlVersion = "XML_1.0";

            /// <summary>
            /// Prevents a default instance of the <see cref="AttributesValue"/> class from being created.
            /// </summary>
            private AttributesValue()
            {
            }
        }

        /// <summary>
        /// Elements constants
        /// </summary>
        public sealed class Elements
        {
            /// <summary>
            /// element name
            /// </summary>
            public const string AcceptAcknowledgementCode = "acceptAckCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string Acknowledgement = "acknowledgement";

            /// <summary>
            /// element name
            /// </summary>
            public const string AcknowledgementDetail = "acknowledgementDetail";

            /// <summary>
            /// element name
            /// </summary>
            public const string Addr = "addr";

            /// <summary>
            /// As licensed entity
            /// </summary>
            public const string AsLicensedEntity = "asLicensedEntity";

            /// <summary>
            /// element name
            /// </summary>
            public const string AsMember = "asMember";

            /// <summary>
            /// element name
            /// </summary>
            public const string AssignedDevice = "assignedDevice";

            /// <summary>
            /// element name
            /// </summary>
            public const string AssignedPerson = "assignedPerson";

            /// <summary>
            /// element name
            /// </summary>
            public const string AttentionLine = "attentionLine";

            /// <summary>
            /// element name
            /// </summary>
            public const string AuthorOrPerformer = "authorOrPerformer";

            /// <summary>
            /// element name
            /// </summary>
            public const string Overseer = "overseer";

            /// <summary>
            /// element name
            /// </summary>
            public const string Code = "code";

            /// <summary>
            /// The contact party
            /// </summary>
            public const string ContactParty = "contactParty";

            /// <summary>
            /// element name
            /// </summary>
            public const string ControlActProcess = "controlActProcess";

            /// <summary>
            /// element name
            /// </summary>
            public const string CreationTime = "creationTime";

            /// <summary>
            /// element name
            /// </summary>
            public const string DataEnterer = "dataEnterer";

            /// <summary>
            /// element name
            /// </summary>
            public const string Delimiter = "delimiter";

            /// <summary>
            /// element name
            /// </summary>
            public const string Device = "device";

            /// <summary>
            /// element name
            /// </summary>
            public const string DirectionCode = "directionCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string EffectiveTime = "effectiveTime";

            /// <summary>
            /// element name
            /// </summary>
            public const string ElementName = "elementName";

            /// <summary>
            /// element name
            /// </summary>
            public const string EventTimeframe = "eventTimeframe";

            /// <summary>
            /// element name
            /// </summary>
            public const string Family = "family";

            /// <summary>
            /// element name
            /// </summary>
            public const string Given = "given";

            /// <summary>
            /// element name
            /// </summary>
            public const string Group = "group";

            /// <summary>
            /// element name
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// element name
            /// </summary>
            public const string InformationRecipient = "informationRecipient";

            /// <summary>
            /// element name
            /// </summary>
            public const string InitialQuantity = "initialQuantity";

            /// <summary>
            /// element name
            /// </summary>
            public const string InteractionId = "interactionId";

            /// <summary>
            /// The issuing organization
            /// </summary>
            public const string IssuingOrganization = "issuingOrganization";

            /// <summary>
            /// element name
            /// </summary>
            public const string KeywordText = "keyWordText";

            /// <summary>
            /// element name
            /// </summary>
            public const string LanguageCode = "languageCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string Location = "location";

            /// <summary>
            /// element name
            /// </summary>
            public const string Low = "low";

            /// <summary>
            /// element name
            /// </summary>
            public const string ModifyCode = "modifyCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// element name
            /// </summary>
            public const string PatientId = "patientId";

            /// <summary>
            /// element name
            /// </summary>
            public const string Prefix = "prefix";

            /// <summary>
            /// element name
            /// </summary>
            public const string PriorityCode = "priorityCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string ProcessingCode = "processingCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string ProcessingModeCode = "processingModeCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string QueryAcknowledgement = "queryAck";

            /// <summary>
            /// element name
            /// </summary>
            public const string QueryByParameterPayload = "queryByParameterPayload";

            /// <summary>
            /// element name
            /// </summary>
            public const string QueryContinuation = "queryContinuation";

            /// <summary>
            /// element name
            /// </summary>
            public const string QueryId = "queryId";

            /// <summary>
            /// element name
            /// </summary>
            public const string QueryResponseCode = "queryResponseCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string ReasonCode = "reasonCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string Receiver = "receiver";

            /// <summary>
            /// element name
            /// </summary>
            public const string ReceiverDevice = "receiverDevice";

            /// <summary>
            /// element name
            /// </summary>
            public const string RepresentedOrganization = "representedOrganization";

            /// <summary>
            /// element name
            /// </summary>
            public const string ResponseModalityCode = "responseModalityCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string ResponsibleOrganization = "responsibleOrganization";

            /// <summary>
            /// element name
            /// </summary>
            public const string ResultCurrentQuantity = "resultCurrentQuantity";

            /// <summary>
            /// element name
            /// </summary>
            public const string ResultRemainingQuantity = "resultRemainingQuantity";

            /// <summary>
            /// element name
            /// </summary>
            public const string ResultTotalQuantity = "resultTotalQuantity";

            /// <summary>
            /// element name
            /// </summary>
            public const string SemanticsText = "semanticsText";

            /// <summary>
            /// element name
            /// </summary>
            public const string Sender = "sender";

            /// <summary>
            /// element name
            /// </summary>
            public const string SenderDevice = "senderDevice";

            /// <summary>
            /// element name
            /// </summary>
            public const string SequenceNumber = "sequenceNumber";

            /// <summary>
            /// element name
            /// </summary>
            public const string SignatureCode = "signatureCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string SignatureText = "signatureText";

            /// <summary>
            /// element name
            /// </summary>
            public const string SortControl = "sortControl";

            /// <summary>
            /// element name
            /// </summary>
            public const string StandardIndustryClassCode = "standardIndustryClassCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string StatusCode = "statusCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string StreetAddressLine = "streetAddressLine";

            /// <summary>
            /// element name
            /// </summary>
            public const string Subject = "subject";

            /// <summary>
            /// element name
            /// </summary>
            public const string Suffix = "suffix";

            /// <summary>
            /// element name
            /// </summary>
            public const string TargetMessage = "targetMessage";

            /// <summary>
            /// element name
            /// </summary>
            public const string Telecom = "telecom";

            /// <summary>
            /// element name
            /// </summary>
            public const string TemplateId = "templateId";

            /// <summary>
            /// element name
            /// </summary>
            public const string Text = "text";

            /// <summary>
            /// element name
            /// </summary>
            public const string Time = "time";

            /// <summary>
            /// element name
            /// </summary>
            public const string TypeCode = "typeCode";

            /// <summary>
            /// element name
            /// </summary>
            public const string Value = "value";

            /// <summary>
            /// element name
            /// </summary>
            public const string VersionCode = "versionCode";

            /// <summary>
            /// Prevents a default instance of the <see cref="Elements"/> class from being created.
            /// </summary>
            private Elements()
            {
            }
        }

        /// <summary>
        /// ElementsValue constants
        /// </summary>
        public sealed class ElementsValue
        {
            /// <summary>
            /// kļūda (Error)
            /// </summary>
            public const string Error = "E";

            /// <summary>
            /// I informācija (Information).
            /// </summary>
            public const string Information = "I";

            /// <summary>
            /// W brīdinājums (Warning)
            /// </summary>
            public const string Warning = "W";

            /// <summary>
            /// Prevents a default instance of the <see cref="ElementsValue"/> class from being created.
            /// </summary>
            private ElementsValue()
            {
            }
        }

        /// <summary>
        /// Formats constants
        /// </summary>
        public sealed class Formats
        {
            /// <summary>
            /// Date format
            /// </summary>
            public const string DateTimeFormat = "yyyyMMddHHmmss.ffffzz00";

            /// <summary>
            /// Date format
            /// </summary>
            public const string OldDateTimeFormat = "yyyyMMddHHmmss.fff";

            /// <summary>
            /// Prevents a default instance of the <see cref="Formats"/> class from being created.
            /// </summary>
            private Formats()
            {
            }
        }

        /// <summary>
        /// OIds constants
        /// </summary>
        public sealed class OIds
        {
            /// <summary>
            /// Root OId Value
            /// </summary>
            public const string RootOIdValue = "1.3.6.1.4.1.38760";

            // private const string PersonCodeOidValue = RootOidValue + ".3.1.1";
            // private const string LanguageOidValue = RootOidValue + ".2.2";
            // private const string TemplateIdOidValue = RootOidValue + ".1.2";
            // private const string IdentificationIdOidValue = RootOidValue + ".3.4.1";
            // private const string DeviceIdOidValue = RootOidValue + ".2.3";
            // private const string ReasonCodeIdOidValue = RootOidValue + ".2.4";
            // private const string priorityCodeOidValue = RootOidValue + ".2.5";

            /// <summary>
            /// Prevents a default instance of the <see cref="OIds"/> class from being created.
            /// </summary>
            private OIds()
            {
            }

            /// <summary>
            /// Gets the acknowledgement detail code O id.
            /// </summary>
            public static OId AcknowledgementDetailCodeOId
            {
                get
                {
                    return OId.EvesExpand(".2.14");
                }
            }

            /// <summary>
            /// Gets the action Code (2) ReasonCode
            /// </summary>
            public static OId ActionCodeId
            {
                get
                {
                    return OId.EvesExpand(".2.15");
                }
            }

            /// <summary>
            /// Gets as member group oid.
            /// </summary>
            public static OId AsMemberGroupOid
            {
                get
                {
                    return OId.EvesExpand(".3.4.2");
                }
            }

            /// <summary>
            /// Gets as member oid.
            /// </summary>
            public static OId AsMemberOid
            {
                get
                {
                    return OId.EvesExpand(".3.1.2.1");
                }
            }

            /// <summary>
            /// Gets the language O id.
            /// </summary>
            public static OId CodeOId
            {
                get
                {
                    return new OId("2.16.840.1.113883");
                }
            }

            /// <summary>
            /// Gets the device id.
            /// </summary>
            public static OId DeviceId
            {
                get
                {
                    return OId.EvesExpand(".2.3");
                }
            }

            /// <summary>
            /// Gets the event id.
            /// </summary>
            public static OId EventId
            {
                get
                {
                    return OId.EvesExpand(".2.16");
                }
            }

            /// <summary>
            /// Gets the identification id.
            /// </summary>
            public static OId IdentificationId
            {
                get
                {
                    return OId.EvesExpand(".3.4.1");
                }
            }

            /// <summary>
            /// Gets the language O id.
            /// </summary>
            public static OId LanguageOId
            {
                get
                {
                    return OId.EvesExpand(".2.2");
                }
            }

            /// <summary>
            /// Gets the person code O id.
            /// </summary>
            public static OId PersonCodeOId
            {
                get
                {
                    return OId.EvesExpand(".3.1.1");
                }
            }

            /// <summary>
            /// Gets the priority code id.
            /// </summary>
            public static OId PriorityCodeId
            {
                get
                {
                    return new OId("2.16.840.1.113883.5.7");
                }
            }

            /// <summary>
            /// Gets the query status code id.
            /// </summary>
            public static OId QueryStatusCodeId
            {
                get
                {
                    return new OId("2.16.840.1.113883.5.103");
                }
            }

            /// <summary>
            /// Gets the reason code id.
            /// </summary>
            public static OId ReasonCodeId
            {
                get
                {
                    return OId.EvesExpand(".2.4");
                }
            }

            /// <summary>
            /// Gets the template id.
            /// </summary>
            public static OId TemplateId
            {
                get
                {
                    return OId.EvesExpand(".3.4.2");
                }
            }
        }

        /// <summary>
        /// Versions constants
        /// </summary>
        public sealed class Versions
        {
            /// <summary>
            /// HL7 2006 version
            /// </summary>
            public const string V3NE2006 = "V3-NE-2006";

            /// <summary>
            /// HL7 2011 version
            /// </summary>
            public const string V3NE2011 = "V3-NE-2011";

            /// <summary>
            /// Prevents a default instance of the <see cref="Versions"/> class from being created.
            /// </summary>
            private Versions()
            {
            }

            /// <summary>
            /// HL7 Version
            /// </summary>
            public enum HL7Version : int
            {
                /// <summary>
                /// HL7 Version 2006
                /// </summary>
                HL72006 = 2006,

                /// <summary>
                /// HL7 Version 2011
                /// </summary>
                HL72011 = 2011
            }
        }
    }
}

#pragma warning restore EmptyGeneralCatchClause, 1692