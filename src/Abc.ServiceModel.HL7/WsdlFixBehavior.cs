// ----------------------------------------------------------------------------
// <copyright file="WsdlFixBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.WSSecurityPolicy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Web.Services.Description;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Abc.ServiceModel.Protocol.HL7;
    using WsdlDescription = System.Web.Services.Description.ServiceDescription;

    using WsdlNS = System.Web.Services.Description;

    /// <summary>
    /// The WS-SecurityPoloicy v1.2 Bug Fix behaviour.
    /// </summary>
    public class WsdlFixBehavior : BehaviorExtensionElement, IWsdlExportExtension, IEndpointBehavior, IContractBehavior
    {
        private const string DefaultServiceName = "ServiceName";
        private const string MyPropertyName = "serviceName";
        private string serviceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WsdlFixBehavior"/> class.
        /// </summary>
        public WsdlFixBehavior()
            : this(DefaultServiceName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsdlFixBehavior"/> class.
        /// </summary>
        /// <param name="serviceName"> Name of the service. </param>
        public WsdlFixBehavior(string serviceName)
        {
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns> A <see cref="T:System.Type"/>. </returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(WsdlFixBehavior);
            }
        }

        // Private Methods

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value> The name of the service. </value>
        [ConfigurationProperty(MyPropertyName)]
        public string ServiceName
        {
            get
            {
                return (string)this[MyPropertyName];
            }

            set
            {
                this[MyPropertyName] = value;
            }
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">          The endpoint to modify. </param>
        /// <param name="bindingParameters">
        /// The objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="bindingParameters">The binding parameters.</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description to modify.</param>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">      The endpoint that is to be customized. </param>
        /// <param name="clientRuntime"> The client runtime to be customized. </param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Applies the client behavior.
        /// </summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="clientOperation">The client operation.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">           The endpoint that exposes the contract. </param>
        /// <param name="endpointDispatcher"> The endpoint dispatcher to be modified or extended. </param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="dispatchOperation">The dispatch operation.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description to be modified.</param>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="dispatchRuntime">The dispatch runtime that controls service execution.</param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            string aaa = "aaa";
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL
        /// for a contract.
        /// </summary>
        /// <param name="exporter">
        /// The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the
        /// contract information.
        /// </param>
        /// <param name="context">
        /// Provides mappings from exported WSDL elements to the contract description.
        /// </param>
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            var mess = exporter.GeneratedWsdlDocuments[0].Messages;

            var mm = context.WsdlPortType.Operations[0].Messages;

            foreach (OperationDescription op in context.Contract.Operations)
            {
                // Operation operation = context.GetOperation(op);

                // var mess1 = context.GetOperationMessage(op.Messages[0]);
                // mess1.Name = "PORX_IN000007UV01_LV01_Message";
                // mess1.Message = new XmlQualifiedName("aaaaa", "http:///aaa");

                // XmlDocument opOwner = operation.DocumentationElement.OwnerDocument;
                ParameterInfo[] args = op.SyncMethod.GetParameters();

                var in1 = op.SyncMethod.ReturnParameter;

                for (int i = 0; i < args.Length; i++)
                {
                    // object[] docAttrs= args[i].GetCustomAttributes(typeof(WsdlParameterDocumentationAttribute), false);
                    // if (docAttrs.Length != 0)
                    // {
                    //    // <param name="Int1">Text.</param>
                    //    XmlElement newParamElement = opOwner.CreateElement("param");
                    //    XmlAttribute paramName = opOwner.CreateAttribute("name");
                    //    paramName.Value = args[i].Name;
                    //    //newParamElement.InnerText
                    //    //  = ((WsdlParameterDocumentationAttribute)docAttrs[0]).ParamComment;
                    //    newParamElement.Attributes.Append(paramName);
                    //    operation.DocumentationElement.AppendChild(newParamElement);
                    // }
                }
            }

            // foreach (var item in context.Contract.Operations)
            // {
            //    Operation operation = context.GetOperation(item);
            //    operation.
            // }

            // if (operation != null)
            // {
            //    if (operation.Faults.Count > 0)
            //    {
            //        operation.Faults[0].Name = "aaa";
            //        operation.Faults[0].Message = new XmlQualifiedName("aaa", operation.Faults[0].Message.Namespace);
            //        var fault = context.GetFaultDescription(operation.Faults[0]);
            //        fault.Name = "aaa";
            //        exporter.GeneratedWsdlDocuments[0].Messages[8].Name = "aaa";
            //    }

            // }
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL
        /// for an endpoint.
        /// </summary>
        /// <param name="exporter">
        /// The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the
        /// endpoint information.
        /// </param>
        /// <param name="context">
        /// Provides mappings from exported WSDL elements to the endpoint description.
        /// </param>
        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            string inputName = "PORX_IN000007UV01_LV01";
            string outputName = "PORX_IN000006UV01_LV01";
            string serviceName = this.serviceName;

            bool isSync = true;
            string inputMessName = string.Empty;
            string outputMessName = string.Empty;

            foreach (OperationDescription op in context.Endpoint.Contract.Operations)
            {
                foreach (var opMessage in op.Messages)
                {
                    if (opMessage != null && opMessage.Direction == MessageDirection.Input && !string.IsNullOrEmpty(opMessage.Action))
                    {
                        inputName = opMessage.Action.Replace(HL7Constants.Namespace + ":", string.Empty);
                    }

                    if (opMessage != null && opMessage.Direction == MessageDirection.Output && !string.IsNullOrEmpty(opMessage.Action))
                    {
                        outputName = opMessage.Action.Replace(HL7Constants.Namespace + ":", string.Empty);
                    }

                    if (op.SyncMethod.ReturnType == typeof(void))
                    {
                        isSync = false;
                    }

                    outputMessName = op.SyncMethod.GetBaseDefinition().ToString();

                    var sssssss = context.ContractConversionContext.GetOperationMessage(opMessage);
                    if (sssssss is OperationInput)
                    {
                        sssssss.Name = inputName + "_Message";
                        sssssss.Message = new XmlQualifiedName(inputName + "_Message", "urn:hl7-org:v3");
                    }

                    if (sssssss is OperationOutput)
                    {
                        sssssss.Name = outputName + "_Message";
                        sssssss.Message = new XmlQualifiedName(outputName + "_Message", "urn:hl7-org:v3");
                    }
                }
            }

            if (exporter != null && exporter.GeneratedWsdlDocuments != null && exporter.GeneratedWsdlDocuments.Count > 0)
            {
                if (exporter.GeneratedWsdlDocuments[0].Messages != null)
                {
                    exporter.GeneratedWsdlDocuments[0].Messages.Clear();
                    var input = new WsdlNS.Message() { Name = inputName + "_Message" };
                    input.Parts.Add(new MessagePart() { Name = "Body", Element = new XmlQualifiedName(inputName + "_Message", "urn:hl7-org:v3") });

                    var output = new WsdlNS.Message() { Name = outputName + "_Message" };
                    output.Parts.Add(new MessagePart() { Name = "Body", Element = new XmlQualifiedName(outputName + "_Message", "urn:hl7-org:v3") });

                    exporter.GeneratedWsdlDocuments[0].Messages.Add(input);
                    exporter.GeneratedWsdlDocuments[0].Messages.Add(output);

                    if (!isSync)
                    {
                        exporter.GeneratedWsdlDocuments[0].Documentation = "ASync: " + outputMessName.Replace("urn.hl7.org.v3.", string.Empty);
                    }
                    else
                    {
                        exporter.GeneratedWsdlDocuments[0].Documentation = "Sync: " + outputMessName.Replace("urn.hl7.org.v3.", string.Empty);
                    }
                }

                if (exporter.GeneratedWsdlDocuments[0].Bindings.Count > 0)
                {
                    exporter.GeneratedWsdlDocuments[0].Bindings[0].Name = serviceName + "_Binding_Soap12";
                    exporter.GeneratedWsdlDocuments[0].Bindings[0].Namespaces.Add("nnn", "urn:hl7-org:v3");

                    exporter.GeneratedWsdlDocuments[0].Bindings[0].Type = new XmlQualifiedName(serviceName + "_PortType", "urn:hl7-org:v3");
                    // exporter.GeneratedWsdlDocuments[0].Bindings[0].Type = new XmlQualifiedName(serviceName + "_PortType", "http://schemas.xmlsoap.org/wsdl/");
                    if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations != null && exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations.Count > 0 && exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0] != null)
                    {
                        exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Name = serviceName + "_" + inputName;

                        if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input != null)
                        {
                            exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.Name = inputName + "_Message";

                            if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.OperationBinding != null)
                            {
                                exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.OperationBinding.Name = serviceName + "_" + inputName;
                            }

                            if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.OperationBinding.Namespaces == null)
                            {
                                exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.OperationBinding.Namespaces = new XmlSerializerNamespaces();
                            }

                            exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Input.OperationBinding.Namespaces.Add("nnn", "urn:hl7-org:v3");
                        }

                        if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output != null)
                        {
                            exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.Name = outputName + "_Message";

                            if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.OperationBinding != null)
                            {
                              // exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.OperationBinding.Name = serviceName + "_" + outputName;
                            }

                            if (exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.OperationBinding.Namespaces == null)
                            {
                                exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.OperationBinding.Namespaces = new XmlSerializerNamespaces();
                            }

                            exporter.GeneratedWsdlDocuments[0].Bindings[0].Operations[0].Output.OperationBinding.Namespaces.Add("nnn", "urn:hl7-org:v3");
                        }
                    }
                }

                if (exporter.GeneratedWsdlDocuments[0].Services != null && exporter.GeneratedWsdlDocuments[0].Services.Count > 0 && exporter.GeneratedWsdlDocuments[0].Services[0] != null)
                {
                    exporter.GeneratedWsdlDocuments[0].Services[0].Name = serviceName + "_Service";

                    if (exporter.GeneratedWsdlDocuments[0].Services[0].Namespaces == null)
                    {
                        exporter.GeneratedWsdlDocuments[0].Services[0].Namespaces = new XmlSerializerNamespaces();
                    }

                    exporter.GeneratedWsdlDocuments[0].Services[0].Namespaces.Add("nnn", "urn:hl7-org:v3");

                    if (exporter.GeneratedWsdlDocuments[0].Services[0].Ports != null && exporter.GeneratedWsdlDocuments[0].Services[0].Ports.Count > 0)
                    {
                        exporter.GeneratedWsdlDocuments[0].Services[0].Ports[0].Name = serviceName + "_PortSoap12";
                    }
                }

                if (exporter.GeneratedWsdlDocuments[0].PortTypes != null && exporter.GeneratedWsdlDocuments[0].PortTypes.Count > 0)
                {
                    exporter.GeneratedWsdlDocuments[0].PortTypes[0].Name = serviceName + "_PortType";

                    if (exporter.GeneratedWsdlDocuments[0].PortTypes[0].Operations != null && exporter.GeneratedWsdlDocuments[0].PortTypes[0].Operations.Count > 0 && exporter.GeneratedWsdlDocuments[0].PortTypes[0].Operations[0] != null)
                    {
                        exporter.GeneratedWsdlDocuments[0].PortTypes[0].Operations[0].Name = serviceName + "_" + inputName;
                    }
                }

                ClearWsdlOfExistingSchemas(exporter.GeneratedWsdlDocuments[0], inputName, outputName);
            }

            this.WSSecurityPolicy12BugFix(context);
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint"> The endpoint to validate. </param>
        public virtual void Validate(ServiceEndpoint endpoint)
        {
            endpoint.Name = this.serviceName + "_Binding_Soap12";
            endpoint.Binding.Namespace = HL7Constants.Namespace;
        }

        /// <summary>
        /// Validates the specified operation description.
        /// </summary>
        /// <param name="operationDescription"> The operation description. </param>
        public void Validate(OperationDescription operationDescription)
        {
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription"> The contract to validate. </param>
        /// <param name="endpoint">            The endpoint to validate. </param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            string aaa = "aaa";

            var tt = endpoint.Name;
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns> The behavior extension. </returns>
        protected override object CreateBehavior()
        {
            return new WsdlFixBehavior(this.ServiceName);
        }

        /// <summary>
        /// WS-SecurityPolicy12 bug fix.
        /// </summary>
        /// <param name="context">
        /// Provides mappings from exported WSDL elements to the endpoint description.
        /// </param>
        protected void WSSecurityPolicy12BugFix(WsdlEndpointConversionContext context)
        {
            // var nsmanager = new XmlNamespaceManager(new NameTable());
            // nsmanager.AddNamespace("sp", "http://docs.oasis-open.org/ws-sx/ws-securitypolicy/200702");
            // nsmanager.AddNamespace(WSPolicyConstants.Prefix, WSPolicyConstants.NamespaceURI);
            foreach (object obj in context.WsdlBinding.ServiceDescription.Extensions)
            {
                XmlElement element = obj as XmlElement;
            }
        }

        private static void AddAllReferencedSchemas(WsdlDescription wsdl, IEnumerable<XmlSchema> referencedXmlSchemas)
        {
            foreach (XmlSchema schema in referencedXmlSchemas)
            {
                wsdl.Types.Schemas.Add(schema);
            }
        }

        private static void AddReferencedXmlSchemasRecursively(XmlSchema schema, XmlSchemaSet generatedXmlSchemas, List<XmlSchema> referencedXmlSchemas)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas = generatedXmlSchemas.Schemas(import.Namespace);
                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!referencedXmlSchemas.Contains(ixsd))
                    {
                        referencedXmlSchemas.Add(ixsd);
                        AddReferencedXmlSchemasRecursively(ixsd, generatedXmlSchemas, referencedXmlSchemas);
                    }
                }
            }
        }

        private static void ClearWsdlOfExistingSchemas(WsdlDescription wsdl, string input, string output)
        {
            wsdl.Types.Schemas.Clear();

            XmlSchema schema = new XmlSchema();
            schema.SourceUri = "urn:hl7-org:v3/Imports";

            XmlSchemaImport import1 = new XmlSchemaImport();
            import1.Namespace = "urn:hl7-org:v3";
            import1.SchemaLocation = "http://ivis.eps.gov.lv/RC/HL7V3/2011/lvext/" + input + ".xsd";
            import1.Id = input;

            XmlSchemaImport import2 = new XmlSchemaImport();
            import2.Namespace = "urn:hl7-org:v3";
            import2.SchemaLocation = "http://ivis.eps.gov.lv/RC/HL7V3/2011/lvext/" + output + ".xsd";
            import2.Id = output;

            XmlSchema addressSchema = new XmlSchema();
            addressSchema.TargetNamespace = "http://www.example.com/IPO";
            import1.Schema = addressSchema;
            schema.Includes.Add(import1);
            schema.Includes.Add(import2);

            wsdl.Types.Schemas.Add(schema);
        }

        private static IEnumerable<XmlSchema> FindAllReferencedXmlSchemasRecursively(WsdlDescription wsdl, XmlSchemaSet generatedXmlSchemas)
        {
            var referencedXmlSchemas = new List<XmlSchema>();

            foreach (XmlSchema schema in wsdl.Types.Schemas)
            {
                AddReferencedXmlSchemasRecursively(schema, generatedXmlSchemas, referencedXmlSchemas);
            }

            return referencedXmlSchemas;
        }

        /// <summary>
        /// Recursively extract all the list of imported schemas
        /// </summary>
        /// <param name="schema">      Schema to examine </param>
        /// <param name="schemaSet">   SchemaSet with all referenced schemas </param>
        /// <param name="importsList"> List to add imports to </param>
        private void AddImportedSchemas(XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importsList)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas = schemaSet.Schemas(import.Namespace);
                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!importsList.Contains(ixsd))
                    {
                        importsList.Add(ixsd);
                        this.AddImportedSchemas(ixsd, schemaSet, importsList);
                    }
                }
            }
        }

        /// <summary>
        /// Remove any &lt;xsd:imports/&gt; in the schema
        /// </summary>
        /// <param name="schema"> Schema to process </param>
        private void RemoveXsdImports(XmlSchema schema)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                if (schema.Includes[i] is XmlSchemaImport)
                {
                    schema.Includes.RemoveAt(i--);
                }
            }
        }
    }
}