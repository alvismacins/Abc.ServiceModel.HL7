### V0.4.0
Added support of CoreWCF

### V0.3.0
Added support of task based methods.
Added support of .NET CORE

### V0.2.0.27 (05.06.2017)
BUG: Sender and Receiver Attribute using 
BUG: AddError, AddWarinig, AddInformation sender using

### V0.2.0.26 (21.02.2017)
BUG: AttentionLine write with xs:type

### V0.2.0.25 (14.07.2016)
BUG: Organization names

### V0.2.0.24 (20.06.2016)
ADD: Attention line error

### V0.2.0.23 (20.06.2016)
ADD: Attention line client Set

### V0.2.0.22 (16.06.2016)
ADD: Attention line client Get

### V0.2.0.21 (06.06.2016)
FIX: Action and Reason dynamic

### V0.2.0.20 (26.05.2016)
FIX: Error when read XML end tag autorOfPerformer	

### V0.2.0.19 (18.05.2016)
FIX: working with white spaces CDATA and trimming

### V0.2.0.18 (06.04.2016)
FIX: missing parameter  informationRecipient and dataEnterer (representedOrganization)

### V0.2.0.17 (07.01.2016)
FIX: AttentionLineRead for Value
FIX: AttentionLineWrite for Value

### V0.2.0.16 (07.12.2015)
BUG: WSDL generator binding Name linked input output

### V0.2.0.15 (07.12.2015)
ADD: GetAttentionLine Context

### V0.2.0.14 (19.06.2015)
ADD: SetAcknowledgementTypeInResponse
CHANGE: formatter throw Exception if messageHl7.Acknowledgement.AcknowledgementDetails.Count > 0

### V0.2.0.13 (19.06.2015)
CHANGE: AuthorOrPerformer Time Datetime to DateTime?, as in Xml 

### V0.2.0.12 (28.05.2015)
CHANGE: REGEX 0-99

### V0.2.0.11 (19.05.2015)
BUG: AssignedDevice incorrect element Read

### V0.2.0.10 (08.05.2015)
BUG:  HL7OperationContext Current Get object reference repear

### V0.2.0.9 (29.10.2014)
NEW:  operationContext.SetDataEnterers, GetDataEnterers
NEW:  operationContext.SetOverseer, GetOverseer

### V0.2.0.8.1 (04.09.2014)
NEW:  operationContext.DataEnterers Get
NEW:  wsdl generator + behavior

### V0.2.0.8 (04.08.2014)
ADD: HL7AssignedPerson + HL7AsLicensedEntity

### V0.2.0.7 (23.04.2013)
FIX: "Receiver Service"=Response Sender, "Sender Client"= Response Receiver 
FIX: Exception attribute "Receiver Service"

### V0.2.0.6
BUG: Contract.messageId, targetMessageId, Identification class set error
FIX: Sender for error priority for contract attribute !string.IsNullOrEmpty(HL7OperationContext.Current.OperationContract.Sender) ? HL7OperationContext.Current.OperationContract.Sender 

### V0.2.0.5
FIX: 3.5 support
FIX: deep styleCop

### V0.2.0.4
ADD: AssignedDevice; Read,write
FIX: HL7MessageFormatter; object reference if not ControlAct exception
FIX: AsMemberOid  .3.2.1 -> .3.1.2.1 

### V0.2.0.3
FIX: SaveResponse ControlAct optional
FIX: DataEnterer, InformationReceipment time optional
ADD: AssignedPerson - HL7RepresentedOrganization.cs Read,Write
ADD: HL7OperationContext.AuthorOrPerformers
FIX: return MessageRequest subject null is allowed
FIX: subject response null is allowed
FIX: operationContext.QueryAcknowledgement set in formater

### V0.2.0.2
FIX: HL7AcknowledgementDetail, sender as string; Context replaced to OperationContext
FIX: ReplyAction (*)  HL7OperationContractAttribute.cs
FIX: ControlAct effectionTime Read ReadTime function
ADD: HL7AcknowledgementDetail GetCodeNumber, GetSender
ADD: time parsing helper Helper.cs
FIX: reader Time parsing using Helper
ADD: ReasonCode, ActionCode in OperationContext is requeried
ADD: HL7MessageCreator
ADD: AsMember
ADD: constructors HL7OperationContract
ADD: receiver null on service ->> for client sender copy

### V0.2.0.1
BUG: EndElementReader, protocol level for elements <a></a> (EndElement(reader));
BUG: URN 2006 UrnType.cs
ADD: AddError throw HL7OperationContext.Current.AddError, HL7OperationContext.cs
FIX: check for errors in HL7Acknowledgement in write (WriteAcknowledgement, HL7SerializerWrite.cs)
ADD: check for AckType (AE, CE) HL7SerializerRead.cs
FIX: removed obsoled constructors
ADD: HL7QueryAcknowledgement.cs new contructors
FIX: Changed DateTime format -> yyyyMMddHHmmss.ffffzz00
Remove: HL7QueryAcknowledgementResponse.cs
FIX: queryAcknowledgement is not requeried
FIX: HL7SaveResponseMessageFormatter.cs + receiver  +HL7Acknowledgement -targetMessageId
FIX HL7SaveResponseOperationContractAttribute.cs -receiver in interface
FIX: old timeFormat parsing for support HL7SerializerRead.cs ReadTime()

### V0.2.0.0
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

### V0.2.0.0
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

### V0.1.0.14
FIX: errorCode

### V0.1.0.13
FIX: void and AcknowledgementResponse is needed only void
FIX: CE, CA for Async 

### V0.1.0.12
ADD: HL7AcknowledgementDetailCode.cs
FIX: HL7AcknowledgementDetail.cs
FIX: HL7AcknowledgementDetail Error typeCode and code element,
NEW: HL7AcknowledgementDetailCode element with Oid

### V0.1.0.11
FIX: HL7DataEnterer Read Write
NEW: ReadAssignedPerson WriteAssignedPerson
FIX: HL7InformationRecipient
NEW: CreateQueryContinuation
FIX: HL7ControlAct  code priorityCode reasonCode languageCode is optional

### V0.1.0.10
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

### V0.1.0.9
operationContext.CreationTime

### V0.1.0.8
operationContext.TargetMessage get BUG

### V0.1.07
operationContext.CorrelationId izmainits uz operationContext.MessageId
operationContext.TargetMessage add
response.Acknowledgement.TargetMessage.Extension = operationContext.MessageId;

### V0.1.0.6
 
### V0.1.0.5 
sequenceNumber pievienots

### V0.1.0.4
File:	HL7QueryAck.cs  HL7QueryByParameterPayload.cs HL7QueryDataEnterer.cs HL7QueryDevice.cs HL7QueryEventTimeframe.cs HL7QueryPatientId.cs HL7QueryReceiverDevice.cs HL7QueryResponsibleOrganization.cs HL7QuerySenderDevice.cs HL7QuerySortControl.cs
NEW:	Query data
CHANGE: partial class HL7Serializer/ Read write
BUG:  receiver = new HL7Device(operationContext.OperationContract.!Sender!, HL7Constants.AttributesValue.Rcv);  HL7MessageFormatter.cs  HL7ErrorHandler.cs

### V0.1.0.4
File:	HL7ErrorHandler.cs, HL7FaultException.cs, HL7SericeBehavior.cs HL7AcknowledgementType.cs
NEW:	Pieveinojās dantes kļūdas apstrādei.
NEW:    enum HL7AcknowledgementType nosaukumi Serializacijas pārveidošana tipiem.

File:	HL7MessageFormatter.cs, HL7OperationContext.cs
CHG:	Izmaiņas saistītas ar kūdas apstrādi.

### V0.1.0.3
File:	HL7Serializer.cs
CHANGE:	WhiteSpace cheking

### V0.1.0.2
File:	HL7MessageFormatter.cs, HL7OperationContractAttribute.cs
NEW:	Pieveinojās atrībūts AckResponse

### V0.1.0.1
File:	HL7SubjectSerilizerAttribute.cs, HL7MessageFormatter.cs, HL7OperationContractAttribute.cs
NEW:	Tika pieveinota iespeja kponfigurēt SubjectSerializer uz Ieejas un izejas paramtriem atseviški

CHG:	Izlaboti dažadi CodeAnalysis warnings

File:	Oid.cs
CHG:	Izlabota funkcija Equals un pieveinota funkcijas Compare

### V0.1.0.0
Pirmā versija
