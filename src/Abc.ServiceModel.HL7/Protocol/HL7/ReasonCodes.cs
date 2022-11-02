// ----------------------------------------------------------------------------
// <copyright file="ReasonCodes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// ReasonCodes enumerations
    /// </summary>
    public class ReasonCodes
    {
        /// <summary>
        /// Action codes
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// Read action
            /// </summary>
            Read,

            /// <summary>
            /// request action
            /// </summary>
            Request,

            /// <summary>
            /// response action
            /// </summary>
            Response,

            /// <summary>
            /// select  action
            /// </summary>
            Select,

            /// <summary>
            /// write action
            /// </summary>
            Write
        }

        /// <summary>
        /// Reason codes
        /// </summary>
        public enum Reason
        {
            /// <summary>
            /// CONTROL AND INSPECTION
            /// </summary>
            ControlAndInspection,

            /// <summary>
            /// DRUG TREATMENT
            /// </summary>
            DrugTreatment,

            /// <summary>
            /// DUE_RECORD_ OR_REFFERAL
            /// </summary>
            DueRecordOrRefferal,

            /// <summary>
            /// EMERGENCY code
            /// </summary>
            Emergency,

            /// <summary>
            /// HEALTH_ CARE_ ADMINISTRATION code
            /// </summary>
            HealthCareAdministration,

            /// <summary>
            /// IN_MEDICAL_ TREATMENT code
            /// </summary>
            InMedicalTreatment,

            /// <summary>
            /// ON_PATIENT_ REQUEST code
            /// </summary>
            OnPatientRequest,

            /// <summary>
            /// OTHER code
            /// </summary>
            Other,

            /// <summary>
            /// SCIENTIFIC_RESEARCH code
            /// </summary>
            ScentificResearch,

            /// <summary>
            /// WITH_PATIENT_AGREEMENT code
            /// </summary>
            WithPatientAgreement
        }
    }
}