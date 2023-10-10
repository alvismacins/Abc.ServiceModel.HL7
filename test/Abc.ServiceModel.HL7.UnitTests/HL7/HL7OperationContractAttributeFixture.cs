using Abc.ServiceModel.Protocol.HL7;
using NUnit.Framework;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Abc.ServiceModel.HL7.UnitTests
{
    [TestFixture]
    public class HL7OperationContractAttributeFixture
    {
        private const string inName = "RCMR_IN000002UV01";
        private const string outName = "MCCI_IN000006UV01";

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContract")]
        public interface ITestContract
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            string TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContractAsync")]
        public interface ITestContractAsync
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = HL7Constants.Namespace + ":" + outName)]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = "Sender", Receiver = "Receiver")]
            [return: HL7SubjectSerializer(HL7SubjectSerializerTypes.XmlSerializer)]
            Task<string> TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }


        [ServiceContract(Namespace = HL7Constants.Namespace, ConfigurationName = "ITestContractWithReturnMessage")]
        public interface ITestContractWithReturnMessage
        {
            [OperationContract(Name = "ProcessInvoke", Action = HL7Constants.Namespace + ":" + inName, ReplyAction = "*")]
            [HL7OperationContract(ReasonCodes.Action.Read, ReasonCodes.Reason.DrugTreatment, Sender = "Sender", Receiver = "Receiver")]
            Message TestMethod([HL7SubjectSerializer(HL7SubjectSerializerTypes.DataContractSerializer)] int payload);
        }

        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class TestService : ITestContract
        {
            public string TestMethod(int payload)
            {
                return payload.ToString();
            }
        }

        [ServiceBehavior(Namespace = HL7Constants.Namespace)]
        public class LocalTestService : ITestContract
        {
            public string TestMethod(int payload)
            {
                return payload.ToString();
            }
        }


        [Test]
        public void ApplyDispatchBehaviorTest()
        {
            using (var host = new ServiceHost(typeof(LocalTestService)))
            {
                host.AddServiceEndpoint(typeof(ITestContract), new BasicHttpBinding(), new Uri("http://localhost:8000/"));
                host.Open();

                var hl7Operation = host.Description.Endpoints[0].Contract.Operations[0].Behaviors.Find<HL7OperationContractAttribute>();
                Assert.IsNotNull(hl7Operation);
                Assert.AreEqual("Receiver", hl7Operation.Receiver);
                Assert.AreEqual("Sender", hl7Operation.Sender);
                Assert.AreEqual(hl7Operation.Template, Helper.GetUrnType(inName, HL7Constants.Versions.HL7Version.HL72011).ToString());
                Assert.AreEqual(hl7Operation.ReplyTemplate, Helper.GetUrnType(outName, HL7Constants.Versions.HL7Version.HL72011).ToString());
            }
        }

        //[Test]
        //public void ApplyDispatchBehaviorTestWithService()
        //{
        //    using (ServiceHost host = new ServiceHost(typeof(TestService)))
        //    {
        //        host.Open();

        //        ChannelFactory<ITestContract> factory = new ChannelFactory<ITestContract>("basicClient");
        //        foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
        //        {
        //            Console.WriteLine(opBehavior);
        //        }

        //        ITestContract proxy = factory.CreateChannel();
        //        var aaa = proxy.TestMethod(1);
        //        Console.WriteLine(aaa);
        //    }
        //}


#if NET45_OR_GREATER
        [Test]
        public void ApplyDispatchBehaviorTestWithService()
        {
            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.Open();

                ChannelFactory<ITestContract> factory = new ChannelFactory<ITestContract>("basicClient");
                foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
                {
                    Console.WriteLine(opBehavior);
                }

                ITestContract proxy = factory.CreateChannel();
                var aaa = proxy.TestMethod(1);
                Console.WriteLine(aaa);
            }
        }

        [Test]
        public void ApplyDispatchBehaviorTestWithStar()
        {
            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.Open();

                ChannelFactory<ITestContractWithReturnMessage> factory = new ChannelFactory<ITestContractWithReturnMessage>("basicClient2");
                foreach (var opBehavior in factory.Endpoint.Contract.Operations[0].Behaviors)
                {
                    Console.WriteLine(opBehavior);
                }

                ITestContractWithReturnMessage proxy = factory.CreateChannel();

                using (OperationContextScope scope = new OperationContextScope((IContextChannel)proxy))
                {
                    var aaa = proxy.TestMethod(1);
                    Assert.AreNotEqual(aaa.State, MessageState.Closed);

                    Console.WriteLine(aaa);
                }
            }
        }
    }
#endif
}