using System.IO;
using System.Text;
using System.Xml;
using Abc.ServiceModel.UnitTests;
using NUnit.Framework;

namespace Abc.ServiceModel.HL7.UnitTests
{
    [TestFixture]
    public class XmlSerializerObjectSerializerFixture
    {
        [Test]
        public void SerializeComplex()
        {
            var body = new XsdTestStructure() { attr = "attribute", Elem = "element" };

            var serializer = new XmlSerializerObjectSerializer(body.GetType());
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body);
            }

            var s = sb.ToString();
            object retval;
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                retval = serializer.ReadObject(reader);
            }

            Assert.IsInstanceOf<XsdTestStructure>(retval);
            var ret = (XsdTestStructure)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeComplexLocalName()
        {
            var body = new XsdTestStructure() { attr = "attribute", Elem = "element" };

            var serializer = new XmlSerializerObjectSerializer(body.GetType(), "subject", "urn:hl7-org:v3");
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body);
            }

            var s = sb.ToString();
            object retval;
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                retval = serializer.ReadObject(reader);
            }

            Assert.IsInstanceOf<XsdTestStructure>(retval);
            var ret = (XsdTestStructure)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeSimple()
        {
            var body = 10;

            var serializer = new XmlSerializerObjectSerializer(body.GetType());
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body);
            }

            var s = sb.ToString();
            object retval;
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                retval = serializer.ReadObject(reader);
            }

            Assert.IsInstanceOf<int>(retval);
            Assert.AreEqual(body, retval);
        }

        [Test]
        public void SerializeSimpleLocalName()
        {
            var body = 10;

            var serializer = new XmlSerializerObjectSerializer(body.GetType(), "subject", "urn:hl7-org:v3");
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body);
            }

            var s = sb.ToString();
            object retval;
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                retval = serializer.ReadObject(reader);
            }

            Assert.IsInstanceOf<int>(retval);
            Assert.AreEqual(body, retval);
        }
    }
}