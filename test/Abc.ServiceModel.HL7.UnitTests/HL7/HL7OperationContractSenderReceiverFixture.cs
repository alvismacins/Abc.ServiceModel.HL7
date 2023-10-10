// ----------------------------------------------------------------------------
// <copyright file="TestSenderReceiver.cs" company="ABC software">
//     Copyright © ABC SOFTWARE. All rights reserved. The source code or its parts to use,
//     reproduce, transfer, copy or keep in an electronic form only from written agreement ABC SOFTWARE.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7.UnitTests
{
    using NUnit.Framework;
    using Protocol.HL7;
    using System;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// TestSenderReceiver
    /// </summary>
    public class HL7OperationContractSenderReceiverFixture
    {
        private const string inName = "RCMR_IN000002UV01";
        private const string outName = "MCCI_IN000006UV01";

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContractSenderReceiver")]
        public interface ITestContractSenderReceiver
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContractSenderReceiverResponse")]
        public interface ITestContractSenderReceiverResponse
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = "Sender1", Receiver = "Receiver1")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContractSenderReceiverResponseNull")]
        public interface ITestContractSenderReceiverResponseNull
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = null, Receiver = null)]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class TestServiceSenderReceiver : ITestContractSenderReceiverResponse
        {
            public string TestMethod(int payload)
            {
                Assert.AreEqual(HL7OperationContext.Current.Sender.Id.Extension, "Sender");
                Assert.AreEqual(HL7OperationContext.Current.Receiver.Id.Extension, "Receiver");
                HL7OperationContext.Current.AddWarning("data", 1);

                return payload.ToString();
            }
        }


        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class TestServiceSenderReceiverOnNUll : ITestContractSenderReceiverResponseNull
        {
            public string TestMethod(int payload)
            {
                Assert.AreEqual(HL7OperationContext.Current.Sender.Id.Extension, "Sender");
                Assert.AreEqual(HL7OperationContext.Current.Receiver.Id.Extension, "Receiver");
                HL7OperationContext.Current.AddWarning("data", 1);

                return payload.ToString();
            }
        }


#if NET45_OR_GREATER
        /// <summary>
        /// Senders the receiver test.
        /// </summary>
        [Test]
        public void TestFilledInterfaces()
        {
            using (var host = new ServiceHost(typeof(TestServiceSenderReceiver)))
            {
                host.Open();

                var factory = new ChannelFactory<ITestContractSenderReceiver>("testSenderReceiver");
                foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
                {
                    Console.WriteLine(opBehavior);
                }

                var proxy = factory.CreateChannel();

                using (var scope = new OperationContextScope((IContextChannel)proxy))
                {
                    var aaa = proxy.TestMethod(1);
                    Console.WriteLine(aaa);

                    if (OperationContext.Current != null)
                    {
                        Assert.AreEqual(HL7OperationContext.Current.Sender.Id.Extension, "Sender1");
                        Assert.AreEqual(HL7OperationContext.Current.Receiver.Id.Extension, "Receiver1");
                        Assert.AreEqual(HL7OperationContext.Current.AcknowledgementDetail?.FirstOrDefault().GetSender, "Sender1");
                    }
                }
            }
        }

        [Test]
        public void TestServiceNullReceiver()
        {
            using (var host = new ServiceHost(typeof(TestServiceSenderReceiverOnNUll)))
            {
                host.Open();

                var factory = new ChannelFactory<ITestContractSenderReceiver>("testSenderReceiver");
                foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
                {
                    Console.WriteLine(opBehavior);
                }

                var proxy = factory.CreateChannel();

                using (var scope = new OperationContextScope((IContextChannel)proxy))
                {
                    var aaa = proxy.TestMethod(1);
                    Console.WriteLine(aaa);

                    if (OperationContext.Current != null)
                    {
                        Assert.AreEqual(HL7OperationContext.Current.Sender.Id.Extension, "Receiver");
                        Assert.AreEqual(HL7OperationContext.Current.Receiver.Id.Extension, "Sender");
                        Assert.AreEqual(HL7OperationContext.Current.AcknowledgementDetail?.FirstOrDefault().GetSender, "Receiver");
                    }
                }
            }
        }
    }
#endif
}
