// ----------------------------------------------------------------------------
// <copyright file="HL7AsMember.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// HL7AsMember class
    /// </summary>
    public class HL7AsMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AsMember"/> class.
        /// </summary>
        /// <param name="idExtension">The id extension.</param>
        /// <param name="groupIdExtension">The group id extension.</param>
        public HL7AsMember(string idExtension, string groupIdExtension)
            : this(new Collection<HL7II>() { new HL7II(HL7Constants.OIds.AsMemberOid, idExtension) }, new Collection<HL7II>() { new HL7II(HL7Constants.OIds.AsMemberGroupOid, groupIdExtension) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AsMember"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupId">The group id.</param>
        public HL7AsMember(ICollection<HL7II> id, ICollection<HL7II> groupId)
        {
            if (id != null)
            {
                this.Id = new Collection<HL7II>();

                foreach (var item in id)
                {
                    this.Id.Add(item);
                }
            }

            if (groupId != null)
            {
                this.GroupId = new Collection<HL7II>();

                foreach (var item in groupId)
                {
                    this.GroupId.Add(item);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7AsMember"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupId">The group id.</param>
        public HL7AsMember(HL7II id, HL7II groupId)
        {
            if (id != null)
            {
                this.Id = new Collection<HL7II>();
                this.Id.Add(id);
            }

            if (groupId != null)
            {
                this.GroupId = new Collection<HL7II>();
                this.GroupId.Add(groupId);
            }
        }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value>
        /// The group id.
        /// </value>
        public ICollection<HL7II> GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public ICollection<HL7II> Id
        {
            get;
            set;
        }
    }
}