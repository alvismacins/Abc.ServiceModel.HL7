V0.2.0.0
FIX: TemplateId "1.3.6.1.4.1.38760.3.4.2"
ADD: HL7AssignedPerson.cs
FIX: HL7AuthorOrPerformer
FIX: ReadAssignedPerson, ReadAssignedDevice, ReadAuthorOrPerformer
FIX: WriteAuthorOrPerformer, WriteDataEnterer, WriteAssignedPerson, writePerson
FIX: HL7TemplateId Oid-> URN 
FIX: ReasonCode > 1
ADD: Second reasonCode .2.15 - ActionCode
Add: telecom[] ReadAuthorOrPerformer
FIX: HL7FaultException + Sender
FIX: templeteId and ReplyTemplateId automatiski aizpīldas
FIX: URN for . and -, 2006 and 2011
NEW: version atribute in HL7Operation (2006/2011)
FIX: templateId deleted form Request and Responses auto fild
FIX: Abc.ServiceModel.HL7AcknowledgementDetailCode namespace change to Abc.ServiceModel.Protocol.HL7

V0.1.0.14
FIX: errorCode

V0.1.0.13
FIX: void and AcknowledgementResponse is needed only void
FIX: CE, CA for Async 

V0.1.0.12
ADD: HL7AcknowledgementDetailCode.cs
FIX: HL7AcknowledgementDetail.cs
FIX: HL7AcknowledgementDetail Error typeCode and code element,
NEW: HL7AcknowledgementDetailCode element with Oid


V0.1.0.11
FIX: HL7DataEnterer Read Write
NEW: ReadAssignedPerson WriteAssignedPerson
FIX: HL7InformationRecipient
NEW: CreateQueryContinuation
FIX: HL7ControlAct  code priorityCode reasonCode languageCode is optional

V0.1.0.10
protected emtpy contructors,
versijas norade ir obligāta 
FIX: HL7QueryByParameterPayload lidzīgi subject
NEW: atribute QueryParam
BUG: AcknowledgementDetail izmainits typeCode-> Code elements
NEW: HL7OperationContext.AcknowledgementType
FIX: PriorityCodeId: 2.16.840.1.113883.5.7
FIX: StatusCodeRead, StatusCodeWrite
FIX: HL7SubjectSerilizerAttribute->HL7SubjectSerializerAttribute
FIX: name Ack -> Acknowledgement
FIX: processingModeCode, processingCode AcceptAcknowledgementCode Read Write, Enumeration naming

V0.1.0.9
operationContext.CreationTime

V0.1.0.8
operationContext.TargetMessage get BUG


V0.1.07
operationContext.CorrelationId izmainits uz operationContext.MessageId
operationContext.TargetMessage add
response.Acknowledgement.TargetMessage.Extension = operationContext.MessageId;

V0.1.0.6

 
V0.1.0.5 
sequenceNumber pievienots

V0.1.0.4
File:	HL7QueryAck.cs  HL7QueryByParameterPayload.cs HL7QueryDataEnterer.cs HL7QueryDevice.cs HL7QueryEventTimeframe.cs HL7QueryPatientId.cs HL7QueryReceiverDevice.cs HL7QueryResponsibleOrganization.cs HL7QuerySenderDevice.cs HL7QuerySortControl.cs
NEW:	Query data
CHANGE: partial class HL7Serializer/ Read write
BUG:  receiver = new HL7Device(operationContext.OperationContract.!Sender!, HL7Constants.AttributesValue.Rcv);  HL7MessageFormatter.cs  HL7ErrorHandler.cs


V0.1.0.4
File:	HL7ErrorHandler.cs, HL7FaultException.cs, HL7SericeBehavior.cs HL7AcknowledgementType.cs
NEW:	Pieveinojās dantes kļūdas apstrādei.
NEW:    enum HL7AcknowledgementType nosaukumi Serializacijas pārveidošana tipiem.

File:	HL7MessageFormatter.cs, HL7OperationContext.cs
CHG:	Izmaiņas saistītas ar kūdas apstrādi.

V0.1.0.3
File:	HL7Serializer.cs
CHANGE:	WhiteSpace cheking

V0.1.0.2
File:	HL7MessageFormatter.cs, HL7OperationContractAttribute.cs
NEW:	Pieveinojās atrībūts AckResponse

V0.1.0.1
File:	HL7SubjectSerilizerAttribute.cs, HL7MessageFormatter.cs, HL7OperationContractAttribute.cs
NEW:	Tika pieveinota iespeja kponfigurēt SubjectSerializer uz Ieejas un izejas paramtriem atseviški

CHG:	Izlaboti dažadi CodeAnalysis warnings

File:	Oid.cs
CHG:	Izlabota funkcija Equals un pieveinota funkcijas Compare

V0.1.0.0
Pirmā versija