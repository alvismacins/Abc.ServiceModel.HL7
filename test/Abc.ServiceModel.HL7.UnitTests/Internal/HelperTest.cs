namespace Abc.ServiceModel.HL7.UnitTests
{
    using System.IO;
    using System.Reflection;

    internal class HelperTest
    {
        public static string GetEmbeddedResourceContent(string resourceName)
        {
            var name = Assembly.GetExecutingAssembly().GetName().Name + "." + resourceName;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}