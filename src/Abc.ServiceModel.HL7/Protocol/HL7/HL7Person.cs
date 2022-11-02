namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase.  Personas apraksts.
    /// </summary>
    public class HL7Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Person"/> class.
        /// </summary>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="familyName">Name of the family.</param>
        public HL7Person(string givenName, string familyName)
        {
            if (!(!string.IsNullOrEmpty(givenName))) {  throw new ArgumentNullException("givenName", "!string.IsNullOrEmpty(givenName)"); }
            if (!(!string.IsNullOrEmpty(familyName))) {  throw new ArgumentNullException("familyName", "!string.IsNullOrEmpty(familyName)"); }

            this.GivenName = new Collection<string>();
            this.FamilyName = new Collection<string>();
            this.GivenName.Add(givenName);
            this.FamilyName.Add(familyName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Person"/> class.
        /// </summary>
        /// <param name="givenNames">The given names.</param>
        /// <param name="familyNames">The family names.</param>
        public HL7Person(IEnumerable<string> givenNames, IEnumerable<string> familyNames)
        {
            if (givenNames != null)
            {
                this.GivenName = new Collection<string>();

                foreach (var item in givenNames)
                {
                    this.GivenName.Add(item);
                }
            }

            if (familyNames != null)
            {
                this.FamilyName = new Collection<string>();

                foreach (var item in familyNames)
                {
                    this.FamilyName.Add(item);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7Person"/> class.
        /// </summary>
        protected HL7Person()
        {
        }

        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>
        /// The name of the family.
        /// </value>
        public ICollection<string> FamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public ICollection<string> GivenName
        {
            get;
            set;
        }
    }
}