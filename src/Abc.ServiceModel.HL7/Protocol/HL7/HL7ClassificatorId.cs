namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase, klasifikatora v�rt�bas laukos Code un codeSystem, kur codeSystem ir Oid identifikators
    /// </summary>
    public class HL7ClassificatorId
    {
        /// <summary>
        /// HL7 schemas code
        /// </summary>
        private string code;

        /// <summary>
        /// HL7 schemas codeSystem
        /// </summary>
        private OId codeSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ClassificatorId"/> class.
        /// </summary>
        /// <param name="code">The code. param</param>
        /// <param name="codeSystem">The code system.</param>
        public HL7ClassificatorId(string code, OId codeSystem)
        {
            if (!(!string.IsNullOrEmpty(code))) {  throw new ArgumentNullException("code", "!string.IsNullOrEmpty(code)"); }
            this.code = code;
            this.codeSystem = codeSystem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ClassificatorId"/> class.
        /// </summary>
        protected HL7ClassificatorId()
        {
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// HL7 schemas code property
        /// </value>
        public virtual string Code
        {
            get
            {
                return this.code;
            }

            set
            {
                // Contract.Requires<ArgumentNullException>(value != null, "value");
                this.code = value;
            }
        }

        /// <summary>
        /// Gets or sets the code system.
        /// </summary>
        /// <value>
        /// The code system.
        /// </value>
        public OId CodeSystem
        {
            get
            {
                return this.codeSystem;
            }

            set
            {
                // Contract.Requires<ArgumentNullException>(value != null, "value");
                this.codeSystem = value;
            }
        }
    }
}