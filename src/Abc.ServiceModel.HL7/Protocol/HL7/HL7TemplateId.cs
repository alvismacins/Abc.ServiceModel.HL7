namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase. Unik�lais zi�ojuma �ablona identifikators, kur �1.3.6.1.4.1.38760.1.2� ir medic�nisko dokumentu standarti, bet �1.3.6.1.4.1.38760.1.2.1.1.1� Medicinas pamatdatu kopsavilkuma dokuments.
    /// </summary>
    public class HL7TemplateId : HL7II
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7TemplateId"/> class.
        /// </summary>
        /// <param name="id">The id. value</param>
        public HL7TemplateId(HL7II id)
            : this(id.Root, new UrnType(id.Extension))
        {
            if (id == null) {  throw new ArgumentNullException("id", "id != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7TemplateId"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public HL7TemplateId(UrnType extension)
            : this(HL7Constants.OIds.TemplateId, extension)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7TemplateId"/> class.
        /// </summary>
        /// <param name="root">The root. value</param>
        /// <param name="extension">The extension.</param>
        public HL7TemplateId(OId root, UrnType extension)
            : base(root, extension)
        {
            this.Extension = extension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7TemplateId"/> class.
        /// </summary>
        protected HL7TemplateId()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public override string Extension { get; set; }
    }
}