namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 class
    /// </summary>
    public class HL7RepresentedOrganization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public HL7RepresentedOrganization(HL7II id)
            : this(new Collection<HL7II>() { id })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public HL7RepresentedOrganization(ICollection<HL7II> ids)
            : this(ids, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="code">The code.</param>
        public HL7RepresentedOrganization(HL7II id, HL7ClassificatorId code)
            : this(new Collection<HL7II>() { id }, code, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="code">The code.</param>
        public HL7RepresentedOrganization(ICollection<HL7II> ids, HL7ClassificatorId code)
            : this(ids, code, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        public HL7RepresentedOrganization(HL7II id, HL7ClassificatorId code, string name)
            : this(new Collection<HL7II>() { id }, code, new Collection<string>() { name }, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="code">The code.</param>
        /// <param name="names">The names.</param>
        public HL7RepresentedOrganization(ICollection<HL7II> ids, HL7ClassificatorId code, ICollection<string> names)
            : this(ids, code, names, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        /// <param name="streedAdress">The streed adress.</param>
        public HL7RepresentedOrganization(HL7II id, HL7ClassificatorId code, string name, string streedAdress)
            : this(new Collection<HL7II>() { id }, code, new Collection<string>() { name }, streedAdress, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="code">The code.</param>
        /// <param name="names">The names.</param>
        /// <param name="streedAdress">The streed adress.</param>
        public HL7RepresentedOrganization(ICollection<HL7II> ids, HL7ClassificatorId code, ICollection<string> names, string streedAdress)
            : this(ids, code, names, streedAdress, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        /// <param name="streedAdress">The streed adress.</param>
        /// <param name="standardIndustryClassCode">The standard industry class code.</param>
        public HL7RepresentedOrganization(HL7II id, HL7ClassificatorId code, string name, string streedAdress, HL7ClassificatorId standardIndustryClassCode)
            : this(new Collection<HL7II>() { id }, code, new Collection<string>() { name }, streedAdress, standardIndustryClassCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7RepresentedOrganization"/> class.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="code">The code.</param>
        /// <param name="names">The names.</param>
        /// <param name="streedAdress">The streed adress.</param>
        /// <param name="standardIndustryClassCode">The standard industry class code.</param>
        public HL7RepresentedOrganization(ICollection<HL7II> ids, HL7ClassificatorId code, ICollection<string> names, string streedAdress, HL7ClassificatorId standardIndustryClassCode)
        {
            if (ids == null) {  throw new ArgumentNullException("ids", "ids != null"); }

            this.Ids = ids;
            this.Code = code;
            this.Names = names;
            this.StreedAdress = streedAdress;
            this.StandardIndustryClassCodes = standardIndustryClassCode;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public HL7ClassificatorId Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        /// <value>
        /// The ids.
        /// </value>
        public ICollection<HL7II> Ids
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the names.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public ICollection<string> Names
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the standard industry class codes.
        /// </summary>
        /// <value>
        /// The standard industry class codes.
        /// </value>
        public HL7ClassificatorId StandardIndustryClassCodes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the streed adress.
        /// </summary>
        /// <value>
        /// The streed adress.
        /// </value>
        public string StreedAdress
        {
            get;
            set;
        }
    }
}