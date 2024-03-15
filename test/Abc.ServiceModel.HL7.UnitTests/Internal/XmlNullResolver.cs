using System.IO;
using System.Security.Permissions;

namespace System.Xml
{
    /// <summary>
    /// This class implements an <see cref="XmlResolver"/> which will always fail a load request or resolution.
    /// </summary>
    /// <remarks>
    /// This class is used as an utility class if it is required that no access to the filesystem
    /// or the internet should be issued during resolution.
    /// </remarks>
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class XmlNullResolver : XmlResolver
    {
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
        /// The <see cref="XmlNullResolver"/> will always return <b>null</b> thus loading nothing.
        /// </remarks>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
                throw new NullReferenceException();

            throw new FileNotFoundException();
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
        /// The <see cref="XmlNullResolver"/> will call <see cref="XmlResolver.ResolveUri"/>
        /// </remarks>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return base.ResolveUri(baseUri, relativeUri);
        }
    }
}