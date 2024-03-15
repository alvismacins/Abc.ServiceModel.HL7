// ----------------------------------------------------------------------------
// <copyright file="XmlValidator.cs" company="ABC software">
//    Copyright © ABC SOFTWARE. All rights reserved.
//    The source code or its parts to use, reproduce, transfer, copy or
//    keep in an electronic form only from written agreement ABC SOFTWARE.
// </copyright>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using System.Xml.Schema;

namespace System.Xml
{
    /// <summary>
    /// XML Validator
    /// </summary>
    public class XMLValidator
    {
        private Dictionary<string, XmlSeverityType> result = null;
        private int ind;

        /// <summary>
        /// Initializes a new instance of the <see cref="XMLValidator"/> class.
        /// </summary>
        public XMLValidator()
        {
            result = new Dictionary<string, XmlSeverityType>();
            ind = 0;
        }

        /// <summary>
        /// Validates the XML.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <param name="schemaFilePath">The schema file path.</param>
        /// <param name="schemaNamespace">The schema namespace.</param>
        /// <returns>Dictionary of validate</returns>
        public Dictionary<string, XmlSeverityType> ValidateXml(XmlDocument xmlDocument, string schemaFilePath, string schemaNamespace)
        {
            xmlDocument.Schemas.Add(schemaNamespace, schemaFilePath);
            xmlDocument.Schemas.Compile();
            ValidationEventHandler eventhandler = new ValidationEventHandler(ValidationEventHandler);
            xmlDocument.Validate(eventhandler);

            return this.result;
        }

        public Dictionary<string, XmlSeverityType> ValidateXml(XmlDocument xmlDocument, XmlReader schemaReader, string schemaNamespace)
        {
            xmlDocument.Schemas.XmlResolver = new XmlResourceResolver(Assembly.GetExecutingAssembly());
            xmlDocument.Schemas.Add(schemaNamespace, schemaReader);
            xmlDocument.Schemas.Compile();
            ValidationEventHandler eventhandler = new ValidationEventHandler(ValidationEventHandler);
            xmlDocument.Validate(eventhandler);

            return this.result;
        }

        /// <summary>
        /// Validations the event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            ind++;
            result.Add(e.Message + " num:" + ind.ToString(), e.Severity);
        }
    }
}