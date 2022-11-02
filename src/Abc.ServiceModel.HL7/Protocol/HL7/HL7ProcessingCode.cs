// ----------------------------------------------------------------------------
// <copyright file="HL7ProcessingCode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// Nosaka ziņojuma sūtīšanas režīmu. Iespējamie nosūtīšanas ceļi
    /// </summary>
    public enum HL7ProcessingCode
    {
        /// <summary>
        /// (produkcijas) – ražošanas vides, ko īsteno dzīvajā (LIVE); (P)
        /// </summary>
        Production,

        /// <summary>
        /// (apmācība) – testa vidē, demo/testa pielietojums; (T)
        /// </summary>
        Test,

        /// <summary>
        /// (atkļūdošana) – izstrādes, atkļūdošanas vidē. (D)
        /// </summary>
        Debug
    }
}