namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase. Tie�i authorOrPerformer klase iesaist�ta ir ier�ce.
    /// </summary>
    public class HL7Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Device"/> class.
        /// </summary>
        /// <param name="id">The id. value</param>
        /// <param name="typeCode">The type code.</param>
        public HL7Device(HL7II id, string typeCode)
        {
            if (typeCode == null) {  throw new ArgumentNullException("typeCode", "typeCode != null"); }
            this.Id = id;
            this.TypeCode = typeCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Device"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="typeCode">The type code.</param>
        public HL7Device(string extension, string typeCode)
            : this(new HL7II(HL7Constants.OIds.DeviceId, extension), typeCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Device"/> class.
        /// </summary>
        protected HL7Device()
        {
        }

        /// <summary>
        /// Gets the class code.
        /// </summary>
        public string ClassCode
        {
            get
            {
                return HL7Constants.AttributesValue.Dev;
            }
        }

        /// <summary>
        /// Gets the determiner code.
        /// </summary>
        public string DeterminerCode
        {
            get
            {
                return HL7Constants.AttributesValue.Instance;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id. value
        /// </value>
        public HL7II Id { get; set; }

        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        /// <value>
        /// The type code.
        /// </value>
        public string TypeCode
        {
            get;
            set;
        }
    }
}