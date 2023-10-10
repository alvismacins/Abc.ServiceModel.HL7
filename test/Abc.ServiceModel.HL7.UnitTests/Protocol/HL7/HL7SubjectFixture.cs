using Abc.ServiceModel.Protocol.HL7;
using Abc.ServiceModel.UnitTests;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Abc.ServiceModel.HL7.UnitTests
{
    [TestFixture]
    public class HL7SubjectFixture
    {
        //[Test]
        public void SerializeXElementComplex()
        {
            XElement body = XElement.Parse(File.ReadAllText(@"test\Abc.ServiceModel.HL7.UnitTests\_Data\Test.xml"));
            var subject = HL7Subject.CreateSubject(body);

            StringBuilder sb = new StringBuilder();
            using (var writer = XmlDictionaryWriter.Create(sb))
            {
                subject.WriteSubject(writer);
            }

            string s = sb.ToString();
            XElement res;
            using (var reader = XmlDictionaryReader.Create(new StringReader(s)))
            {
                res = HL7Subject.CreateSubject(reader).GetBody<XElement>();
            }

            Assert.AreEqual("attribute", res.Attribute("attr").Value);
            Assert.AreEqual("element", res.Element(XName.Get("Elem", "http://test")).Value);
        }

        [Test]
        public void SerializeXElementLinq()
        {
            var body = new TestStructure() { attr = "attribute", Elem = "element" };
            var subject = HL7Subject.CreateSubject(body.Untyped);

            StringBuilder sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                subject.WriteSubject(writer);
            }

            string s = sb.ToString();
            TestStructure ret;
            using (var reader = XmlDictionaryReader.Create(new StringReader(s)))
            {
                ret = HL7Subject.CreateSubject(reader).GetBody<TestStructure>();
            }

            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeXsd()
        {
            var body = new XsdTestStructure() { attr = "attribute", Elem = "element" };

            var subject = HL7Subject.CreateSubject(body);
            StringBuilder sb = new StringBuilder();

            using (var writer = XmlDictionaryWriter.Create(sb))
            {
                subject.WriteSubject(writer);
            }

            string s = sb.ToString();
            XsdTestStructure ret;
            using (var reader = XmlDictionaryReader.Create(new StringReader(s)))
            {
                ret = HL7Subject.CreateSubject(reader).GetBody<XsdTestStructure>();
            }

            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }
    }
}