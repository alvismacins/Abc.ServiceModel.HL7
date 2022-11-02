namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Ja nepiecie�ams re�istr�t datu ievad�t�ju, to var izdar�t. Var re�istr�t
    /// vair�kus datu ievad�t�jus
    /// </summary>
    public class HL7DataEnterer : HL7AssignedPerson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7DataEnterer"/> class.
        /// </summary>
        /// <param name="personCode">The person code.</param>
        /// <param name="code">The code.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="familyName">Name of the family.</param>
        public HL7DataEnterer(string personCode, HL7ClassificatorId code, string givenName, string familyName)
            : base(personCode, code, givenName, familyName)
        {
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7DataEnterer"/> class.
        /// </summary>
        /// <param name="time">The time. value</param>
        /// <param name="personCode">The person code.</param>
        /// <param name="code">The code.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="familyName">Name of the family.</param>
        public HL7DataEnterer(DateTime? time, string personCode, HL7ClassificatorId code, string givenName, string familyName)
            : base(personCode, code, givenName, familyName)
        {
            this.Time = time.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7DataEnterer"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="person">The person.</param>
        public HL7DataEnterer(DateTime? time, HL7AssignedPerson person)
            : base(person.PersonId, person.Code, person.Persons, person.Telecoms, person.AsMembers, person.RepresentedOrganization, person.AsLicensedEntity)
        {
            if (person == null) {  throw new ArgumentNullException("person", "person != null"); }

            this.Time = time;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7DataEnterer"/> class.
        /// </summary>
        protected HL7DataEnterer()
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