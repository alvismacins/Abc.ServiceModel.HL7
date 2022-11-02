// ----------------------------------------------------------------------------
// <copyright file="HL7SubjectSerializerTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using System;

    /// <summary>
    /// HL7 Subject serializer types
    /// </summary>
    [Serializable]
    public enum HL7SubjectSerializerTypes
    {
        /// <summary>
        /// Use Default Subject serializer.
        /// </summary>
        AutoDetect,

        /// <summary>
        /// Use DataContractSerializer.
        /// </summary>
        DataContractSerializer,

        /// <summary>
        /// USe XmlSerializer.
        /// </summary>
        XmlSerializer,

        /// <summary>
        /// Custom defined XmlObjectSerializer.
        /// </summary>
        Custom
    }
}