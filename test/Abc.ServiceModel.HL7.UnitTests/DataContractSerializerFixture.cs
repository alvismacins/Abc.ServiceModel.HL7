using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Abc.ServiceModel.HL7.UnitTests
{
    [TestFixture]
    public class DataContractSerializerFixture
    {
        [Test]
        public void SerializeComplex()
        {
            var body = XElement.Parse(HelperTest.GetEmbeddedResourceContent("_Data.Test.xml"));

            var serializer = new DataContractSerializer(typeof(XElement));
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

        /// <summary>
        /// If DataContractSerializer with rootName and rootNamespace then Write first element
        /// </summary>
        [Test]
        public void SerializeComplexLocalName()
        {
            var body = XElement.Parse(HelperTest.GetEmbeddedResourceContent("_Data.Test.xml"));

            var serializer = new DataContractSerializer(typeof(XElement), "subject", "urn:hl7-org:v3");
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

        /// <summary>
        /// BUG: ToSerialize this XTypedElement, Write first element
        /// </summary>
        //[Test]
        public void SerializeComplexXTypedElement()
        {
            var body = new Test() { attr = "attribute", Elem = "element" };

            var serializer = new DataContractSerializer(body.GetType());
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body);
            }

            // BUG: ToSerialize this XTypedElement
            var s = sb.ToString();
            object retval;
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                retval = serializer.ReadObject(reader);
            }

            Assert.IsInstanceOf<Test>(retval);
            var ret = (Test)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        /// <summary>
        /// BUG: ToSerialize this XTypedElement, Write first element
        /// </summary>
        //[Test]
        public void SerializeComplexXTypedElementLocalName()
        {
            var body = new Test() { attr = "attribute", Elem = "element" };

            var serializer = new DataContractSerializer(body.GetType(), "subject", "urn:hl7-org:v3");
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

            Assert.IsInstanceOf<Test>(retval);
            var ret = (Test)retval;
            Assert.AreEqual(body.attr, ret.attr);
            Assert.AreEqual(body.Elem, ret.Elem);
        }

        [Test]
        public void SerializeComplexXUnTypedElement()
        {
            var body = new Test() { attr = "attribute", Elem = "element" };

            var serializer = new DataContractSerializer(typeof(XElement));
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body.Untyped);
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

        /// <summary>
        /// If DataContractSerializer with rootName and rootNamespace then Write first element
        /// </summary>
        [Test]
        public void SerializeComplexXUnTypedElementLocal()
        {
            var body = new Test() { attr = "attribute", Elem = "element" };

            var serializer = new DataContractSerializer(typeof(XElement), "subject", "urn:hl7-org:v3");
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, body.Untyped);
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
    }
}