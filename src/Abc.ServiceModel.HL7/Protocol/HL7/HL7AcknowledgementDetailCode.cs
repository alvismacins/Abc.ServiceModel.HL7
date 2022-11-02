namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HL7AcknowledgementDetailCode : HL7ClassificatorId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetailCode"/> class.
        /// </summary>
        /// <param name="codeNumber">The code number.</param>
        public HL7AcknowledgementDetailCode(string codeNumber)
            : base(codeNumber.ToString(), Abc.ServiceModel.Protocol.HL7.HL7Constants.OIds.AcknowledgementDetailCodeOId)
        {
            if (!(!string.IsNullOrEmpty(codeNumber))) {  throw new ArgumentNullException("codeNumber", "!string.IsNullOrEmpty(codeNumber)"); }

            this.CodeNumber = codeNumber;
        }

        /// <summary>
        /// Gets or sets the code number.
        /// </summary>
        /// <value>
        /// The code number.
        /// </value>
        public string CodeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Code;
        }
    }
}