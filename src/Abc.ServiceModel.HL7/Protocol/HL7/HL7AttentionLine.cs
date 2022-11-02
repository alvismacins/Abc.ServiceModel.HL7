namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Xml.Linq;

    /// <summary>
    /// HL7 sh�mas klase, Sada�a �auj nos�t�t inform�ciju, kas nepiecie�ama, lai piln�b� saprastu zi�ojumu. Eso�� realiz�cij� tiek izmantoti ��di papildus atrib�ti: activityId � defin� E-vesel�bas centr�l�s sist�mas �urn�la sasaistes identifikators, kur tas ir �1.3.6.1.4.1.38760.3.4.3�. [3]
    /// </summary>
    public class HL7AttentionLine
    {
        /// <summary>
        /// HL7 schemas AttentionLineValue
        /// </summary>
        private HL7II attentionLineValue;

        /// <summary>
        /// HL7 schemas keywordText
        /// </summary>
        private string keywordText;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AttentionLine"/> class.
        /// </summary>
        /// <param name="keywordText">The keyword text.</param>
        public HL7AttentionLine(string keywordText)
            : this(keywordText, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AttentionLine"/> class.
        /// </summary>
        /// <param name="keywordText">The keyword text.</param>
        /// <param name="attentionLineValue">The value.</param>
        public HL7AttentionLine(string keywordText, HL7II attentionLineValue)
        {
            if (!(!string.IsNullOrEmpty(keywordText))) {  throw new ArgumentNullException("keywordText", "!string.IsNullOrEmpty(keywordText)"); }

            this.KeywordText = keywordText;
            this.Value = attentionLineValue;
        }

        /// <summary>
        /// Gets or sets the keyword text.
        /// </summary>
        /// <value>
        /// The keyword text.
        /// </value>
        public string KeywordText
        {
            get
            {
                return this.keywordText;
            }

            set
            {
                this.keywordText = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public HL7II Value
        {
            get
            {
                return this.attentionLineValue;
            }

            set
            {
                this.attentionLineValue = value;
            }
        }
    }
}