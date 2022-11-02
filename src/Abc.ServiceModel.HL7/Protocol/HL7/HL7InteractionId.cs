namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase. Konkr�ta inform�cijas apmai�as vienuma identifikators. Atrib�tu v�rt�bas ir atvasin�tas no HL7 MDF mijiedarb�bas nosaukumiem, piem�ri iek�auj �POLB IN300652 un COMT_IN300652�. Sa��m�ja pien�kums (ieskaitot apstiprin�jumus/funkcion�l�s atbildes) tiks noteikts ar mijiedarb�bu, ko �is identifikators identific�. Satur sakni (Root), kas parasti ir vien�da ar �2.16.840.1.113883� (Health Level Seven, Inc. (HL7)) vai �1.3.6.1.4.1.38760� (Latvijas e-vesel�bas iniciat�va) un papla�in�jumu, kas parasti atbilst saknes elementa nosaukumam, piem�ram, RCMR_IN000005UV01 vai PRPA_IN201102UV01_LV01.
    /// </summary>
    public class HL7InteractionId : HL7II
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InteractionId"/> class.
        /// </summary>
        /// <param name="id">The id. value</param>
        public HL7InteractionId(HL7II id)
            : base(id.Root, id.Extension)
        {
            if (id == null) {  throw new ArgumentNullException("id", "id != null"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InteractionId"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public HL7InteractionId(string extension)
            : base(HL7Constants.OIds.IdentificationId, extension)
        {
            if (!(!string.IsNullOrEmpty(extension))) {  throw new ArgumentNullException("extension", "!string.IsNullOrEmpty(extension)"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InteractionId"/> class.
        /// </summary>
        protected HL7InteractionId()
        {
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public override string Extension
        {
            get
            {
                return base.Extension;
            }

            set
            {
                // regex
                base.Extension = value;
            }
        }
    }
}