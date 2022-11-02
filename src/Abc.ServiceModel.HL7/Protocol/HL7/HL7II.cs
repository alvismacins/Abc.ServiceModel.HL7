namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase. Identifikatoru b�zes klase.
    /// </summary>
    public class HL7II
    {
        /// <summary>
        /// HL7 schemas extension
        /// </summary>
        private string extension;

        /// <summary>
        /// HL7 schemas root
        /// </summary>
        private OId root;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7II"/> class.
        /// </summary>
        /// <param name="root">The root. value</param>
        /// <param name="extension">The extension.</param>
        public HL7II(OId root, string extension)
        {
            if (extension == null) {  throw new ArgumentNullException("extension", "extension != null"); }
            this.root = root;
            this.extension = extension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7II"/> class.
        /// </summary>
        protected HL7II()
        {
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public virtual string Extension
        {
            get
            {
                return this.extension;
            }

            set
            {
                if (value == null) {  throw new ArgumentNullException("value", "value != null"); }
                this.extension = value;
            }
        }

        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>
        /// The root. value
        /// </value>
        public OId Root
        {
            get
            {
                return this.root;
            }

            set
            {
                this.root = value;
            }
        }
    }
}