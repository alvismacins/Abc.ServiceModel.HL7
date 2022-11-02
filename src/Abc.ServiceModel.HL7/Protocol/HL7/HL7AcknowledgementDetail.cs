namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// HL7 sh�mas klase. P�rraides HL7Acknowledgement apvalka elementi.
    /// </summary>
    public class HL7AcknowledgementDetail
    {
        private HL7AcknowledgementDetailCode code;
        private int codeNumber;
        private string senderExtension;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        [Obsolete("Use constructor with Code", true)]
        public HL7AcknowledgementDetail(string text, HL7AcknowledgementDetailType type)
            : this(text, null, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public HL7AcknowledgementDetail(string codeNumber, string text, HL7AcknowledgementDetailType type)
            : this(codeNumber, text, null, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="text">The text.</param>
        /// <param name="location">The location.</param>
        /// <param name="type">The type.</param>
        /// <param name="senderExtension">The sender extension.</param>
        public HL7AcknowledgementDetail(int codeNumber, string text, string location, HL7AcknowledgementDetailType type, string senderExtension)
        {
            if (!(!string.IsNullOrEmpty(text))) {  throw new ArgumentNullException("text", "!string.IsNullOrEmpty(text)"); }

            this.Text = text;
            this.Location = location;
            this.DetailType = type;

            if (!string.IsNullOrEmpty(senderExtension))
            {
                this.codeNumber = codeNumber;
                this.senderExtension = senderExtension;
                this.Code = new HL7AcknowledgementDetailCode(senderExtension + "-" + codeNumber.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.codeNumber = codeNumber;
                this.Code = new HL7AcknowledgementDetailCode(codeNumber.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets the new code.
        /// </summary>
        /// <param name="senderExtension">The sender extension.</param>
        public void SetNewCode(string senderExtension)
        {
            if (!string.IsNullOrEmpty(senderExtension))
            {
                this.senderExtension = senderExtension;
                this.Code = new HL7AcknowledgementDetailCode(senderExtension + "-" + this.codeNumber.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.Code = new HL7AcknowledgementDetailCode(this.codeNumber.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        /// <param name="acknowledgementDetail">The acknowledgement detail.</param>
        /// <param name="text">The text.</param>
        /// <param name="location">The location.</param>
        /// <param name="type">The type.</param>
        public HL7AcknowledgementDetail(HL7AcknowledgementDetailCode acknowledgementDetail, string text, string location, HL7AcknowledgementDetailType type)
        {
            if (!(!string.IsNullOrEmpty(text))) {  throw new ArgumentNullException("text", "!string.IsNullOrEmpty(text)"); }

            this.Text = text;
            this.Location = location;
            this.DetailType = type;
            this.Code = acknowledgementDetail;

            if (acknowledgementDetail != null && !string.IsNullOrEmpty(acknowledgementDetail.CodeNumber))
            {
                if (acknowledgementDetail.CodeNumber.Contains("-"))
                {
                    this.senderExtension = acknowledgementDetail.CodeNumber.Split('-')[0];
                    int dataInt = -1;

                    if (int.TryParse(acknowledgementDetail.CodeNumber.Split('-')[1], out dataInt))
                    {
                        this.codeNumber = dataInt;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="text">The text.</param>
        /// <param name="location">The location.</param>
        /// <param name="type">The type.</param>
        public HL7AcknowledgementDetail(string codeNumber, string text, string location, HL7AcknowledgementDetailType type)
            : this(new HL7AcknowledgementDetailCode(codeNumber), text, location, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AcknowledgementDetail"/> class.
        /// </summary>
        protected HL7AcknowledgementDetail()
        {
        }

        /// <summary>
        /// Gets or sets the code. Identific� specifiskus zi�ojumus, kas tiek atgriezti (Piez�me: teksta v�rt�bu var nor�d�t k� drukas nosaukumu; nekoda zi�ojumiem, k� s�kotn�jo tekstu. Piem�ram, �Tr�kst nepiecie�am� atrib�ta xxx�, �Sist�ma neb�s pieejama 9.mart� no 0100 l�dz 0300�.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public HL7AcknowledgementDetailCode Code
        {
            get
            {
                return this.code;
            }

            set
            {
                this.code = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the detail.
        /// </summary>
        /// <value>
        /// The type of the detail.
        /// </value>
        public HL7AcknowledgementDetailType DetailType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the get number.
        /// </summary>
        public int GetNumber
        {
            get
            {
                return this.codeNumber;
            }
        }

        /// <summary>
        /// Gets the get sender.
        /// </summary>
        public string GetSender
        {
            get
            {
                return this.senderExtension;
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text. Probl�mas apraksts
        /// </summary>
        /// <value>
        /// The text. value
        /// </value>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (!(!string.IsNullOrEmpty(value))) {  throw new ArgumentNullException("value", "!string.IsNullOrEmpty(value)"); }

                this.text = value;
            }
        }
    }
}