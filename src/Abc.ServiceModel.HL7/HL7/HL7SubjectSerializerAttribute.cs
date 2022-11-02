// ----------------------------------------------------------------------------
// <copyright file="HL7SubjectSerializerAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using System;

    /// <summary>
    /// Define subject serializer type on parameter and return value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class HL7SubjectSerializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HL7SubjectSerializerAttribute"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public HL7SubjectSerializerAttribute(HL7SubjectSerializerTypes serializer)
        {
            this.Serializer = serializer;
        }

        /// <summary>
        /// Gets or sets the type of the custom subject serializer.
        /// </summary>
        /// <value>
        /// The type of the custom subject serializer.
        /// </value>
        public Type CustomSerializerType { get; set; }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        public HL7SubjectSerializerTypes Serializer { get; private set; }
    }
}