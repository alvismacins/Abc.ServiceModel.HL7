// ----------------------------------------------------------------------------
// <copyright file="HL7AcceptAcknowledgementCode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// HL7 schemas class
    /// </summary>
    public enum HL7AcceptAcknowledgementCode
    {
        /// <summary>
        /// AL (vienmēr) – atbilde tiek nosūtīta vienmēr;
        /// </summary>
        Always,

        /// <summary>
        /// NE (nekad) – atbilde netiek sūtīta;
        /// </summary>
        Never,

        /// <summary>
        /// ER (kļūda, tikai atteikums) – atbildes ziņojums tiek nosūtīts tikai atteikuma gadījumā.
        /// </summary>
        Error
    }
}