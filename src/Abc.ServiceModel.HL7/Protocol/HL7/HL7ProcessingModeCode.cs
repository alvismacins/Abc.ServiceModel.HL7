// ----------------------------------------------------------------------------
// <copyright file="HL7ProcessingModeCode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public enum HL7ProcessingModeCode
    {
        /// <summary>
        /// arhīva (A)
        /// </summary>
        Archival,

        /// <summary>
        /// sākumielāde (I)
        /// </summary>
        InicialLoad,

        /// <summary>
        /// atjaunošana no arhīva (R)
        /// </summary>
        ArchiveRestoration,

        /// <summary>
        /// normāla apstrāde (T)
        /// </summary>
        OperationData
    }
}