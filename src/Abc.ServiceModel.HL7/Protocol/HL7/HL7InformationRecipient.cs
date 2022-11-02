namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Darb�bai var b�t vair�ki vai neviens inform�cijas sa��m�js. Tiem ir paredz�ti
    /// zi�ojum� iek�autie dati. Inform�cijas sa��m�ji ir j�at��ir no zi�ojumu
    /// sa��m�jiem (kas ir nor�d�ti p�rraides apvalk�), jo inform�cijas sa��m�jiem nav
    /// nek�das noz�mes zi�ojuma vad�bas un p�rraid��anas proces�.
    /// Lai personas viet� lietotu organiz�ciju, <i>player </i>nenor�da, t� viet� lieto
    /// <i>scoper</i>.
    /// </summary>
    public class HL7InformationRecipient : HL7AssignedPerson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InformationRecipient"/> class.
        /// </summary>
        /// <param name="personCode">The person code.</param>
        /// <param name="code">The code.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="familyName">Name of the family.</param>
        public HL7InformationRecipient(string personCode, HL7ClassificatorId code, string givenName, string familyName)
            : base(personCode, code, givenName, familyName)
        {
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InformationRecipient"/> class.
        /// </summary>
        /// <param name="time">The time. value</param>
        /// <param name="personCode">The person code.</param>
        /// <param name="code">The code.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="familyName">Name of the family.</param>
        public HL7InformationRecipient(DateTime? time, string personCode, HL7ClassificatorId code, string givenName, string familyName)
            : base(personCode, code, givenName, familyName)
        {
            this.Time = time;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InformationRecipient"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="person">The person.</param>
        public HL7InformationRecipient(DateTime? time, HL7AssignedPerson person)
            : base(person.PersonId, person.Code, person.Persons, person.Telecoms, person.AsMembers, person.RepresentedOrganization, person.AsLicensedEntity)
        {
            if (person == null) {  throw new ArgumentNullException("person", "person != null"); }

            this.Time = time;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7InformationRecipient"/> class.
        /// </summary>
        protected HL7InformationRecipient()
        {
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time. value
        /// </value>
        public DateTime? Time { get; set; }
    }
}