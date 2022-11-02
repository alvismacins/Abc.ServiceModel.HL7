namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase. Pamata zi�ojuma identifikators - zi�ojuma Message.id, kur�m ir �is apstiprin�jums.
    /// </summary>
    public class HL7IdentificationId : HL7II
    {
        /// <summary>
        /// HL7 schemas extension
        /// </summary>
        private Guid extension;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7IdentificationId"/> class.
        /// </summary>
        public HL7IdentificationId()
            : this(HL7Constants.OIds.IdentificationId, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7IdentificationId"/> class.
        /// </summary>
        /// <param name="id">The id. value</param>
        public HL7IdentificationId(HL7II id)
            : base(id.Root, id.Extension)
        {
            if (id == null) {  throw new ArgumentNullException("id", "id != null"); }
            if (!(!string.IsNullOrEmpty(id.Extension))) {  throw new ArgumentException("id", "!string.IsNullOrEmpty(id.Extension)"); }

            Guid result;

            try
            {
                result = new Guid(id.Extension);
            }
            catch (Exception)
            {
                throw new ArgumentException("Incorrect guid");
            }

            this.Extension = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7IdentificationId"/> class.
        /// </summary>
        /// <param name="root">The root. value</param>
        /// <param name="extension">The extension.</param>
        public HL7IdentificationId(OId root, Guid extension)
            : base(root, extension.ToString())
        {
            this.Extension = extension;
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public new Guid Extension
        {
            get
            {
                Guid result;
                try
                {
                    result = new Guid(base.Extension);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Incorrect guid");
                }

                this.extension = result;

                return result;
            }

            set
            {
                base.Extension = value.ToString();

                this.extension = value;
            }
        }
    }
}