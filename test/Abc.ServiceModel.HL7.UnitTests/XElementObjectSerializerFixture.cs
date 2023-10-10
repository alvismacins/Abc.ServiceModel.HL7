using NUnit.Framework;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xml.Schema.Linq;

namespace Abc.ServiceModel.HL7.UnitTests
{
    [TestFixture]
    public class XElementObjectSerializerFixture
    {
        [Test]
        public void Serialize()
        {
            var body = XElement.Parse("<int>10</int>");

            var serializer = new XElementObjectSerializer();
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

            Assert.IsInstanceOf<XElement>(retval);
            Assert.AreEqual(body.Value, ((XElement)retval).Value);
        }

        [Test]
        public void SerializeComplex()
        {
            var body = XElement.Parse(HelperTest.GetEmbeddedResourceContent("_Data.Test.xml"));

            var serializer = new XElementObjectSerializer();
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

            Assert.IsInstanceOf<XElement>(retval);
            var res = (XElement)retval;

            Assert.AreEqual("attribute", res.Attribute("attr").Value);
            Assert.AreEqual("element", res.Element(XName.Get("Elem", "http://test")).Value);
        }

        [Test]
        public void SerializeComplexLinq()
        {
            var body = new Test() { attr = "attribute", Elem = "element" };

            var serializer = new XElementObjectSerializer();
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

            Assert.IsInstanceOf<XElement>(retval);
            var ret = (Test)(XElement)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeComplexLinqLocalName()
        {
            var body = new TestStructure() { attr = "attribute", Elem = "element" };

            var serializer = new XElementObjectSerializer("subject", "urn:hl7-org:v3");
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

            Assert.IsInstanceOf<XElement>(retval);
            var ret = (TestStructure)XTypedServices.ToXTypedElement((XElement)retval, LinqToXsdTypeManager.Instance, body.GetType());

            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeComplexLinqTyped()
        {
            var body = new TestStructure() { attr = "attribute", Elem = "element" };

            var serializer = new XTypedElementObjectSerializer(body.GetType(), LinqToXsdTypeManager.Instance, "subject", "urn:hl7-org:v3");
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

            Assert.IsInstanceOf<TestStructure>(retval);
            var ret = (TestStructure)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        /// <summary>
        /// // UNDOEN: Cant't change xmlns
        /// </summary>
        [Test]
        public void SerializeComplexLocalName()
        {
            var body = XElement.Parse(HelperTest.GetEmbeddedResourceContent("_Data.Test.xml"));

            // UNDOEN: Cant't change xmlns
            var serializer = new XElementObjectSerializer("subject", "http://test");
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

            Assert.IsInstanceOf<XElement>(retval);
            var res = (XElement)retval;

            Assert.AreEqual("attribute", res.Attribute("attr").Value);
            Assert.AreEqual("element", res.Element(XName.Get("Elem", "http://test")).Value);
        }

        [Test]
        public void SerializeComplexLocalName2()
        {
            var body = XElement.Parse(HelperTest.GetEmbeddedResourceContent("_Data.Test2.xml"));

            var serializer = new XElementObjectSerializer("subject", "urn:hl7-org:v3");
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

            Assert.IsInstanceOf<XElement>(retval);
            var res = (XElement)retval;

            Assert.AreEqual("attribute", res.Attribute("attr").Value);
            Assert.AreEqual("element", res.Element(XName.Get("Elem", "http://test")).Value);
        }

        [Test]
        public void SerializeSimpleLocalName()
        {
            var body = XElement.Parse("<int>10</int>");

            var serializer = new XElementObjectSerializer("subject", "urn:hl7-org:v3");
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

            Assert.IsInstanceOf<XElement>(retval);
            Assert.AreEqual(body.Value, ((XElement)retval).Value);
        }
    }
}