using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Abc.ServiceModel.HL7;
using Abc.ServiceModel.Protocol.HL7;
using NUnit.Framework;

namespace Abc.ServiceModel.UnitTests
{
    [TestFixture]
    public class OperationContractAttributeCheckFixture
    {
        public const string inName = "RCMR_IN000002UV01";

        public const string outName = "MCCI_IN000006UV01";

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContract")]
        public interface ITestContract
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestSenderReceiverContract")]
        public interface ITestSenderReceiverContract
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.ControlAndInspection, Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestSenderReceiverContract1")]
        public interface ITestSenderReceiverContractClient
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.ControlAndInspection, Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        // [Test]
        public void FormaterAttributeNotNull()
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(TestService)))
            {
                serviceHost.AddServiceEndpoint(typeof(ITestContract), new BasicHttpBinding(), new Uri("http://localhost/unittest/testservice"));

                // Open the ServiceHostBase to create listeners and start listening for messages.
                serviceHost.Open();

                var hl7Operation = serviceHost.Description.Endpoints[0].Contract.Operations[0].Behaviors.Find<HL7OperationContractAttribute>();

                HL7MessageFormatter formater = new HL7MessageFormatter(hl7Operation, null, null, null, null);

                Assert.IsNotNull(hl7Operation);
                Assert.AreEqual(hl7Operation.Receiver, "Receiver");
                Assert.AreEqual(hl7Operation.Sender, "Sender");
                Assert.AreEqual(hl7Operation.Template, Helper.GetUrnType(inName, HL7Constants.Versions.HL7Version.HL72011).ToString());
                Assert.AreEqual(hl7Operation.ReplyTemplate, Helper.GetUrnType(outName, HL7Constants.Versions.HL7Version.HL72011).ToString());
            }
        }

        [Test]
        public void FormaterConstructed()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormaterConstructedTestDeserializeReplyNotSetparameterType()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            atribute.ReplyInteraction = "LVAU_IN000001UV01";
            atribute.ReplyTemplate = null;
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);

            Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, atribute.ReplyInteraction, Serializer.SubjectTEst.SerializerTestRequest.wrapperFull);
            formater.DeserializeReply(mess, null);
            formater.SerializeReply(null, null, null);
            Assert.AreEqual(atribute.ReplyTemplate, Helper.GetUrnType(atribute.ReplyInteraction, atribute.Version).ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormaterConstructedTestDeserializeReplyNullMessage()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            atribute.ReplyInteraction = "LVAU_IN000001UV01";
            atribute.ReplyTemplate = "aa";
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
            formater.DeserializeReply(null, null);
        }

        [Test]
        [ExpectedException(typeof(OperationCanceledException))]
        public void FormaterConstructedTestDeserializeReplySetparameterType()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            atribute.ReplyInteraction = "LVAU_IN000001UV01";
            atribute.ReplyTemplate = null;
            Type type = typeof(void);
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, type, null, null, null);

            Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, atribute.ReplyInteraction, Serializer.SubjectTEst.SerializerTestRequest.wrapperFull);
            formater.DeserializeReply(mess, null);
            formater.SerializeReply(null, null, null);
            Assert.AreEqual(atribute.ReplyTemplate, Helper.GetUrnType(atribute.ReplyInteraction, atribute.Version).ToString());
        }

        //[Test]
        // TODO: test
        public void FormaterConstructedTestDeserializeReplySetparameterTypeCheckWithBehavior()
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(TestService)))
            {
                serviceHost.AddServiceEndpoint(typeof(ITestContract), new BasicHttpBinding(), new Uri("http://localhost/unittest/testservice"));

                // Open the ServiceHostBase to create listeners and start listening for messages.
                serviceHost.Open();

                var hl7Operation = serviceHost.Description.Endpoints[0].Contract.Operations[0].Behaviors.Find<HL7OperationContractAttribute>();

                HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
                atribute.ReplyInteraction = "LVAU_IN000001UV01";
                atribute.ReplyTemplate = null;
                Type type = typeof(void);
                HL7MessageFormatter formater = new HL7MessageFormatter(atribute, type, null, null, null);

                Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, atribute.ReplyInteraction, Serializer.SubjectTEst.SerializerTestRequest.wrapperFull);
                formater.DeserializeReply(mess, null);
                formater.SerializeReply(null, null, null);
                Assert.AreEqual(atribute.ReplyTemplate, Helper.GetUrnType(atribute.ReplyInteraction, atribute.Version).ToString());

                Assert.IsNotNull(hl7Operation);
                Assert.AreEqual(hl7Operation.Receiver, "Receiver");
                Assert.AreEqual(hl7Operation.Sender, "Sender");
                Assert.AreEqual(hl7Operation.Template, Helper.GetUrnType(inName, HL7Constants.Versions.HL7Version.HL72011).ToString());
                Assert.AreEqual(hl7Operation.ReplyTemplate, Helper.GetUrnType(outName, HL7Constants.Versions.HL7Version.HL72011).ToString());
            }
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void FormaterConstructedTestSerializeReplyNulls()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
            formater.SerializeReply(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void FormaterConstructedTestSerializeReplySetAtributeReplyTemplateEmptyNulls()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            atribute.ReplyTemplate = "";
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
            formater.SerializeReply(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void FormaterConstructedTestSerializeReplySetAtributeReplyTemplateStringNulls()
        {
            HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
            atribute.ReplyTemplate = "aa";
            HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
            formater.SerializeReply(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormaterNull()
        {
            HL7MessageFormatter formater = new HL7MessageFormatter(null, null, null, null, null);
        }

        //[Test]
        //[ExpectedException(typeof(FormatException))]
        //public void FormaterConstructedTestSerializeReplySetAtributeReplyTemplateStringSetInteractionNulls()
        //{
        //    HL7OperationContractAttribute atribute = new HL7OperationContractAttribute();
        //    atribute.ReplyInteraction = "LVAU_IN000001UV01";
        //    atribute.ReplyTemplate = "aa";
        //    HL7MessageFormatter formater = new HL7MessageFormatter(atribute, null, null, null, null);
        //    formater.SerializeReply(null, null, null);
        //}
        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestCreateMessageNulls()
        {
            Message mess = HL7MessageExtension.CreateHL7Message(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestCreateMessageSetVersionInteractionEmpty()
        {
            Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, string.Empty, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCreateMessageSetVersionInteractionIncorect()
        {
            Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, "aa", null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestCreateMessageSetVersionInteractionNull()
        {
            Message mess = HL7MessageExtension.CreateHL7Message(MessageVersion.Soap11, null, null);
        }

        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class TestSenderReceiverService : ITestSenderReceiverContract
        {
            public string TestMethod(int payload)
            {
                return payload.ToString();
            }
        }

        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class TestService : ITestContract
        {
            public string TestMethod(int payload)
            {
                return payload.ToString();
            }
        }

        //[Test]
        //public void ApplyDispatchBehaviorTestWithService()
        //{
        //    using (ServiceHost host = new ServiceHost(typeof(TestSenderReceiverService)))
        //    {
        //        host.AddServiceEndpoint(typeof(ITestSenderReceiverContract), new BasicHttpBinding(), new Uri("http://localhost:8000/Service"));
        //        host.Open();

        //        ChannelFactory<ITestSenderReceiverContractClient> factory = new ChannelFactory<ITestSenderReceiverContractClient>("basicClient3");
        //        foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
        //        {
        //            Console.WriteLine(opBehavior);
        //        }

        //        ITestSenderReceiverContractClient proxy = factory.CreateChannel();
        //        var aaa = proxy.TestMethod(1);
        //        var sender = HL7OperationContext.Current.Sender;
        //        var receiver = HL7OperationContext.Current.Receiver;

        //        Console.WriteLine(aaa);
        //    }

        //}
    }
}