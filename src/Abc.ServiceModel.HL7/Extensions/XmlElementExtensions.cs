namespace Abc.ServiceModel.HL7.Extensions
{
    using System.Linq;
    using System.Xml.Linq;

    internal static class XmlElementExtensions
    {
        internal static string GetElementNameWithPrefix(this XElement element)
        {
            //string ret = element.GetPrefixOfNamespace(element.Name.Namespace);
            //if (!string.IsNullOrEmpty(ret))
            //{
            //    ret += ":";
            //}

            //ret += element.Name.LocalName;

            //return ret;

            return element.Name.LocalName;
        }

        internal static bool ContainsAttributesFromThisNamespace(this XElement element, string prefix)
        {
            if (element == null) 
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                return false;
            }

            var _prefix = prefix.Trim() + ":";
            var matchingElements = element
                .DescendantsAndSelf() // include the root and all its descendants
                .Attributes()
                .Where(attr => attr.Value != null && attr.Value.StartsWith(_prefix))
                .Select(attr => attr).ToList();

            if (matchingElements.Any())
            {
                return true;
            }

            return false;
        }
    }
}