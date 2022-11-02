namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public class HL7QueryDevice
    {
        private object valueData;
        private string semanticsText;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryDevice"/> class.
        /// </summary>
        /// <param name="valueData">The value data.</param>
        /// <param name="semanticsText">The semantics text.</param>
        public HL7QueryDevice(object valueData, string semanticsText)
        {
            if (valueData == null) {  throw new ArgumentNullException("valueData", "valueData != null"); }
            if (!(!string.IsNullOrEmpty(semanticsText))) {  throw new ArgumentException("semanticsText", "!string.IsNullOrEmpty(semanticsText)"); }

            Value = valueData;
            SemanticsText = semanticsText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryDevice"/> class.
        /// </summary>
        protected HL7QueryDevice()
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get
            {
                return this.valueData;
            }

            set
            {
                this.valueData = value;
            }
        }

        /// <summary>
        /// Gets or sets the semantics text.
        /// </summary>
        /// <value>
        /// The semantics text.
        /// </value>
        public string SemanticsText
        {
            get
            {
                return this.semanticsText;
            }

            set
            {
                this.semanticsText = value;
            }
        }
    }
}