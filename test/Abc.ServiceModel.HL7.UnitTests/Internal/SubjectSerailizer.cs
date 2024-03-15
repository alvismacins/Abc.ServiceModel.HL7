using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using Xml.Schema.Linq;

namespace Abc.ServiceModel.HL7.UnitTests
{
    public class SubjectSerializer : XmlObjectSerializer
    {
        XName name;

        // Constructors
        public SubjectSerializer(XName name)
        {
            this.name = name;
        }

        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            throw new NotImplementedException();
        }

        public override void WriteEndObject(XmlDictionaryWriter writer)
        {

        }

        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            var element = graph as XElement;
            if (element == null)
            {
                var typedElement = graph as XTypedElement;
                if (typedElement != null)
                {
                    element = typedElement.Untyped;
                }
            }

            if (element == null)
            {
                throw new InvalidOperationException();
            }

            if (name != null)
            {
                element.Name = name;
            }

            element.WriteTo(writer);
        }

        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {

        }
    }
}