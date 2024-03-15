using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace System.Xml
{
    /// <summary>
    /// This class implements an <see cref="XmlResolver"/> which will convert load requests
    /// for URLs with a specific schema to loads against embedded resources.
    /// </summary>
    /// <remarks>
    /// This class can be used as an utility class to easyly access resources embedded into an assembly
    /// with the .NET Xml classes.
    /// </remarks>
    /// <example>Example of using the resource resolver.
    /// <code lang="C#">
    /// using SourceForge.xmlcatalogrslv;
    /// ...
    /// XmlResourceResolver aResourceResolver = new XmlResourceResolver( new XmlNullResolver(), typeof(ModularSchemaTest).Assembly, "ModularSchemaSample.Resources", "file" );
    /// </code>
    /// </example>
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class XmlResourceResolver : XmlResolver
    {
        private XmlResolver _InternalResolver;
        private string _PathPrefix;
        private Assembly _ResourceAssembly;
        private string _Schema;

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <remarks>
        /// The Internal Resolver defaults to an <see cref="XmlNullResolver"/>
        /// The Path prefix defaults to "".
        /// The Schema defaults to "res".
        /// </remarks>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(Assembly ResourceAssembly)
        {
            CommonConstructor(new XmlNullResolver(), ResourceAssembly, "", "res");
        }

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="InternalResolver"><see cref="XmlResolver"/> to forward the requests when resolution fails or the schema is not the one handles by this instance.</param>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <remarks>
        /// The Path prefix defaults to "".
        /// The Schema defaults to "res".
        /// </remarks>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(XmlResolver InternalResolver, Assembly ResourceAssembly)
        {
            CommonConstructor(InternalResolver, ResourceAssembly, "", "res");
        }

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <param name="PathPrefix">Prefix that should be prepended when building a path to the embedded resource</param>
        /// <remarks>
        /// The Internal Resolver defaults to an <see cref="XmlNullResolver"/>
        /// The Schema defaults to "res".
        /// </remarks>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(Assembly ResourceAssembly, string PathPrefix)
        {
            CommonConstructor(new XmlNullResolver(), ResourceAssembly, PathPrefix, "res");
        }

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="InternalResolver"><see cref="XmlResolver"/> to forward the requests when resolution fails or the schema is not the one handles by this instance.</param>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <param name="PathPrefix">Prefix that should be prepended when building a path to the embedded resource</param>
        /// <remarks>
        /// The Schema defaults to "res".
        /// </remarks>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(XmlResolver InternalResolver, Assembly ResourceAssembly, string PathPrefix)
        {
            CommonConstructor(InternalResolver, ResourceAssembly, PathPrefix, "res");
        }

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <param name="PathPrefix">Prefix that should be prepended when building a path to the embedded resource</param>
        /// <param name="Schema">URL Schema which should be handled by this instance</param>
        /// <remarks>
        /// The Internal Resolver defaults to an <see cref="XmlNullResolver"/>
        /// </remarks>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(Assembly ResourceAssembly, string PathPrefix, string Schema)
        {
            CommonConstructor(new XmlNullResolver(), ResourceAssembly, PathPrefix, Schema);
        }

        /// <summary>
        /// Constructs an <see cref="XmlResourceResolver"/> with the specified attributes.
        /// </summary>
        /// <param name="InternalResolver"><see cref="XmlResolver"/> to forward the requests when resolution fails or the schema is not the one handles by this instance.</param>
        /// <param name="ResourceAssembly"><see cref="Assembly"/> where the resources should be loaded from.</param>
        /// <param name="PathPrefix">Prefix that should be prepended when building a path to the embedded resource</param>
        /// <param name="Schema">URL Schema which should be handled by this instance</param>
        /// <example>Example of using the resource resolver: See <see cref="XmlResourceResolver"/></example>
        public XmlResourceResolver(XmlResolver InternalResolver, Assembly ResourceAssembly, string PathPrefix, string Schema)
        {
            CommonConstructor(InternalResolver, ResourceAssembly, PathPrefix, Schema);
        }

        /// <summary>
        /// An <see cref="System.Net.ICredentials"/> object. If this property is not set, the value defaults to a null reference (Nothing in Visual Basic);
        /// that is, the <see cref="XmlResolver"/> has no user credentials.
        /// </summary>
        /// <remarks>The <see cref="XmlCatalogResolver"/> does not support this property.</remarks>
        public override System.Net.ICredentials Credentials { set { throw new NotSupportedException("Setting Credentials is not supported in the XmlCatalogResolver"); } }

        /// <summary>
        /// Maps an URI to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from <see cref="ResolveUri"/></param>
        /// <param name="role">The current version does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios.</param>
        /// <param name="ofObjectToReturn">The type of object to return. The current version only returns <see cref="Stream"/> objects.</param>
        /// <returns>A <see cref="Stream"/> object or a <b>null</b> reference (Nothing in Visual Basic) if a type other than stream is specified.</returns>
        /// <remarks>
        /// The <see cref="XmlResourceResolver"/> first checks if the <paramref name="absoluteUri"/> is specified.
        /// Then it checks if the schema of the <paramref name="absoluteUri"/> is the one specified during object creation.
        /// If this is true, the Resolver converts the name to an resource path and tries to load the file.
        /// If the schema is not handled by this <see cref="XmlResourceResolver"/> the the request is forwarded to the Inner Resolver
        /// specified in the <see cref="XmlResourceResolver"/> constructor.
        /// </remarks>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
                throw new NullReferenceException();

            var resource = string.Empty;
            var workString = absoluteUri.ToString();
            var multicacheschemas = "multicacheschemas";
            var coreschemas = "coreschemas";
            var lvext = "lvext";
            var nameOfFile = string.Empty;
            var defNameOfFile = "lvext";
            var lentght = 0;

            if (absoluteUri.ToString().Contains(coreschemas))
            {
                nameOfFile = coreschemas;
                lentght = workString.IndexOf(nameOfFile);
            }
            else if (absoluteUri.ToString().Contains(lvext))
            {
                nameOfFile = lvext;
                lentght = workString.IndexOf(nameOfFile);
            }
            else if (absoluteUri.ToString().Contains(multicacheschemas))
            {
                nameOfFile = multicacheschemas;
                lentght = workString.IndexOf(nameOfFile);
            }
            else
            {
                lentght = workString.LastIndexOf("/");
            }

            var resourceName = workString.Substring(lentght + nameOfFile.Length + 1, workString.Length - lentght - nameOfFile.Length - 1);

            if (!string.IsNullOrEmpty(nameOfFile))
            {
                resource = string.Format("HL7V3_2011.{0}.{1}", nameOfFile, resourceName).Replace(".xsd", "");
            }
            else
            {
                resource = string.Format("HL7V3_2011.{0}.{1}", defNameOfFile, resourceName).Replace(".xsd", "");
            }

            var result = GetType().Assembly.GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + "." + resource);

            return result;

            //if (absoluteUri.Scheme == _Schema)
            //{
            //    if (ofObjectToReturn != null &&
            //        ofObjectToReturn != typeof(Stream))
            //        throw new ApplicationException("Resource Entities only support Stream Objects on return");

            //    string NewPath = absoluteUri.AbsolutePath.Replace('/', '.').Replace('-', '_').Replace(' ', '_');
            //    Stream ResourceFile = _ResourceAssembly.GetManifestResourceStream(String.Format("{0}{1}", _PathPrefix, NewPath));
            //    if (ResourceFile == null)
            //        return _InternalResolver.GetEntity(absoluteUri, role, ofObjectToReturn);

            //    return ResourceFile;
            //}
            //else
            //    return _InternalResolver.GetEntity(absoluteUri, role, ofObjectToReturn);
        }

        /// <summary>
        /// Gets an Entity specified as an <see cref="Uri"/>.
        /// </summary>
        /// <param name="absoluteUri">Resource to be loaded</param>
        /// <returns>A <see cref="Stream"/> object or a <b>null</b> reference if the resource could not be loaded.</returns>
        /// <remarks>
        /// This function is supplied for convenience. It allows quick and simple load from embedded assembly resources.
        /// </remarks>
        public Stream GetEntity(Uri absoluteUri)
        {
            return (Stream)GetEntity(absoluteUri, null, typeof(Stream));
        }

        /// <summary>
        /// Gets an Entity specified as an URI.
        /// </summary>
        /// <param name="absoluteUriAsString">Resource to be loaded</param>
        /// <returns>A <see cref="Stream"/> object or a <b>null</b> reference if the resource could not be loaded.</returns>
        /// <remarks>
        /// This function is supplied for convenience. It allows quick and simple load from embedded assembly resources.
        /// </remarks>
        public Stream GetEntity(string absoluteUriAsString)
        {
            return (Stream)GetEntity(new Uri(absoluteUriAsString), null, typeof(Stream));
        }

        /// <summary>
        /// Resolves the absolute URI from the base and relative URIs.
        /// </summary>
        /// <param name="baseUri">The base URI used to resolve the relative URI</param>
        /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative.
        /// If absolute, this value effectively replaces the baseUri value. If relative, it combines with
        /// the baseUri to make an absolute URI.</param>
        /// <returns>A <see cref="Uri"/> representing the absolute URI or a <b>null</b> reference (Nothing in Visual Basic) if the relative URI can not be resolved.</returns>
        /// <remarks>
        /// The <see cref="XmlResourceResolver"/> will always forward the request to the Inner Resolver.
        /// </remarks>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return _InternalResolver.ResolveUri(baseUri, relativeUri);
        }

        private void CommonConstructor(XmlResolver InternalResolver, Assembly ResourceAssembly, string PathPrefix, string Schema)
        {
            if (null == InternalResolver)
                throw new ArgumentNullException("InternalResolver");
            if (null == ResourceAssembly)
                throw new ArgumentNullException("ResourceAssembly");
            if (null == PathPrefix)
                throw new ArgumentNullException("PathPrefix");
            if (null == Schema)
                throw new ArgumentNullException("Schema");
            if (Schema.Length == 0)
                throw new ArgumentException("Should not be empty", "Schema");
            if (Schema.IndexOfAny(new char[] { '.', '/', '\\', ':' }) > 0)
                throw new ArgumentException("Should not contain './\\:'", "Schema");

            _InternalResolver = InternalResolver;
            _ResourceAssembly = ResourceAssembly;
            _PathPrefix = PathPrefix;
            _Schema = Schema;
        }
    }
}