// ----------------------------------------------------------------------------
// <copyright file="HL7AcknowledgementType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    /// <summary>
    /// HL7 schemas AcknowledgementType
    /// </summary>
    public enum HL7AcknowledgementType
    {
        /// <summary>
        /// AA
        /// Application Acknowledgement Accept
        /// Saņēmēja lietojums (receiving application) ir veiksmīgi apstrādājis ziņojumu.
        /// </summary>
        ApplicationAcknowledgementAccept,

        /// <summary>
        /// AE
        /// Application Acknowledgement Error
        /// Saņēmēja lietojums (receiving application) ziņojuma apstrādē ir konstatējis kļūdu un nosūtījis kļūdas paziņojumu ar detalizētu kļūdas informāciju.
        /// </summary>
        ApplicationAcknowledgementError,

        /// <summary>
        /// AR
        /// Application Acknowledgement Reject
        /// Saņēmēja lietojums (receiving application) nav apstrādājis ziņojumu citu iemeslu dēļ, kas nav saistīti ar ziņojuma saturu vai formātu. Ziņojuma sūtītājam ir jāizlemj, vai atkārtot ziņojuma sūtīšanu.
        /// </summary>
        ApplicationAcknowledgementReject,

        /// <summary>
        /// CA
        /// Accept Acknowledgement Commit Accept
        /// Saņemot ziņojumu, apstrādājošais serviss uzņemas atbildību par ziņojuma nodošanu saņēmēja lietojumam (application).
        /// </summary>
        AcceptAcknowledgementCommitAccept,

        /// <summary>
        /// CE
        /// Accept Acknowledgement Commit Error
        /// Saņemot ziņojumu, apstrādājošais serviss nevar pieņemt ziņojumu kļūdas dēļ (piemēram, ziņojuma numura dēļ u.c.).
        /// </summary>
        AcceptAcknowledgementCommitError,

        /// <summary>
        /// CR
        /// Accept Acknowledgement Commit Reject
        /// Saņemot ziņojumu, apstrādājošais serviss noraida ziņojumu, ja mijiedarbības identifikators, versija vai apstrādes režīms nav savietojams ar saņēmēja lietojuma (receiving application) lomu.
        /// </summary>
        AcceptAcknowledgementCommitReject
    }
}