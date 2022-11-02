namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// HL7 sh�mas klase, zi�ojuma �semantisk� noz�me� ir ControlAct (vad�bas darb�bas) [3]
    /// </summary>
    public abstract class HL7ControlAct
    {
        private HL7ClassificatorId languageCode;
        private HL7ClassificatorId priorityCode;
        private ICollection<HL7ClassificatorId> reasonCode;
        private HL7Subject subject;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        ///// </summary>
        // protected HL7ControlAct()
        // {
        // }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="reasonCode">The reason code.</param>
        protected HL7ControlAct(string action, string reasonCode)
            : this(new HL7ClassificatorId[] { new HL7ClassificatorId(action, HL7Constants.OIds.ActionCodeId), new HL7ClassificatorId(reasonCode, HL7Constants.OIds.ReasonCodeId) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="reasonCode">The reason code.</param>
        protected HL7ControlAct(ICollection<HL7ClassificatorId> reasonCode)
        {
            if (reasonCode != null)
            {
                this.ReasonCodes = new Collection<HL7ClassificatorId>();
                foreach (var item in reasonCode)
                {
                    if (item != null)
                    {
                        this.ReasonCodes.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="reasonCode">The reason code.</param>
        /// <param name="subject">The subject.</param>
        protected HL7ControlAct(string action, string reasonCode, HL7Subject subject)
            : this(null, new HL7ClassificatorId[] { new HL7ClassificatorId(action, HL7Constants.OIds.ActionCodeId), new HL7ClassificatorId(reasonCode, HL7Constants.OIds.ReasonCodeId) }, null, subject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="reasonCodes">The reason codes.</param>
        /// <param name="subject">The subject.</param>
        protected HL7ControlAct(IEnumerable<HL7ClassificatorId> reasonCodes, HL7Subject subject)
            : this(null, reasonCodes, null, subject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="reasonCodes">The reason codes.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="subject">The subject.</param>
        protected HL7ControlAct(string text, IEnumerable<HL7ClassificatorId> reasonCodes, HL7ClassificatorId languageCode, HL7Subject subject)
            : this(null, null, text, null, reasonCodes, languageCode, subject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7ControlAct"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="effectiveTime">The effective time.</param>
        /// <param name="text">The text.</param>
        /// <param name="priorityCode">The priority code.</param>
        /// <param name="reasonCode">The reason code.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="subject">The subject.</param>
        protected HL7ControlAct(HL7ClassificatorId code, DateTime? effectiveTime, string text, HL7ClassificatorId priorityCode, IEnumerable<HL7ClassificatorId> reasonCode, HL7ClassificatorId languageCode, HL7Subject subject)
        {
            this.Code = code;
            this.EffectiveTime = effectiveTime;
            this.Text = text;
            this.PriorityCode = priorityCode;

            if (reasonCode != null)
            {
                this.ReasonCodes = new Collection<HL7ClassificatorId>();
                foreach (var item in reasonCode)
                {
                    if (item != null)
                    {
                        this.ReasonCodes.Add(item);
                    }
                }
            }

            this.LanguageCode = languageCode;

            if (subject != null)
            {
                this.Subject = subject;
            }
        }

        // /// <summary>
        // /// Gets the class code. Vienm�r satur v�rt�bu �CACT� � �a control act�.
        // /// </summary>
        // public string ClassCode
        // {
        //    get
        //    {
        //        return HL7Constants.AttributesValue.ControlAct;
        //    }
        // }
        // /// <summary>
        // /// Gets the mode code. Vienm�r satur v�rt�bu �EVN� � �event�.
        // /// </summary>
        // public string ModeCode
        // {
        //    get
        //    {
        //        return HL7Constants.AttributesValue.Event;
        //    }
        // }

        /// <summary>
        /// Gets or sets the code. Code identific� notikumu (Trigger Event) vad�bas darb�bai vai izs�t�tai notifik�cijai. V�rt�bas atvasina no MDF, piem�ram, �COMT_TE200200� vai �QURX_TE100001�. codeSystem identific� HL7 organiz�ciju. Izmantojam�s v�rt�bas ir defin�tas HL7TriggerEventCode dom�n�. [3]
        /// </summary>
        /// <value>
        /// The code. value
        /// </value>
        public HL7ClassificatorId Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the effective time. Vad�bas darb�bas vai notifik�cijas datums un laiks, kad t� ir izveidota un nodota p�rraidei, piem�ram, notikuma datums un laiks, kas izrais�jis darb�bu. �is datums un laiks parasti nesakrit�s ar datumu un laiku, kas nor�d�ts p�rraides apvalk�.
        /// </summary>
        /// <value>
        /// The effective time.
        /// </value>
        public DateTime? EffectiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language code. Nosaka vad�bas darb�bas prim�ro valodu, respekt�vi, t� ir valoda, kur� atrib�ts text ir izteikts. [3]
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
        public HL7ClassificatorId LanguageCode
        {
            get
            {
                return this.languageCode;
            }

            set
            {
                if (!(value == null || (value != null && value.CodeSystem == HL7Constants.OIds.LanguageOId))) { throw new FormatException("value == null || (value != null && value.CodeSystem == HL7Constants.OIds.LanguageOId)"); }
                this.languageCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the priority code. Kods, vai kodu komplekts, kas nosaka apst�k�us, k�dos ir radies notikums vai tam ir j�rodas vai tiek sagaid�ts, kad tas notiks, vai tiek piepras�ta t� notik�ana. Noklus�t� v�rt�ba ir �R�, parast� gad�jum� - (routine).
        /// </summary>
        /// <value>
        /// The priority code.
        /// </value>
        public HL7ClassificatorId PriorityCode
        {
            get
            {
                return this.priorityCode;
            }

            set
            {
                if (!(value == null || (value != null && value.CodeSystem == HL7Constants.OIds.PriorityCodeId))) { throw new FormatException("value == null || (value != null && value.CodeSystem == HL7Constants.OIds.PriorityCodeId)"); }
                this.priorityCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the reason code. Kods, kas nosaka darb�bas (audit�cijai) motiv�ciju, c�loni vai pamatojumu. reasonCode ir j�lieto tikai pamatiemesliem (common reasons), kas nav saist�ti ar iepriek��ju darb�bu (Act) vai citiem nosac�jumiem, kas iek�auti darb�b�s (Act).
        /// </summary>
        /// <value>
        /// The reason code.
        /// </value>
        public ICollection<HL7ClassificatorId> ReasonCodes
        {
            get
            {
                return this.reasonCode;
            }

            set
            {
                this.reasonCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the subject. Satur piepras�juma/ atbildes rezult�tu, kas ir atkar�gs no vaic�juma specifik�cijas zi�ojum� sa�emtajiem parametriem.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public HL7Subject Subject
        {
            get
            {
                return this.subject;
            }

            set
            {
                if (value == null) { throw new ArgumentNullException("value", "value != null"); }
                this.subject = value;
            }
        }

        /// <summary>
        /// Gets or sets the text. Atrib�ts text var tikt izmantots tekstu�lu vai multimedi�lu darb�bu (Act) paskaidrojumam.
        /// </summary>
        /// <value>
        /// The text. value
        /// </value>
        public string Text
        {
            get;
            set;
        }
    }
}