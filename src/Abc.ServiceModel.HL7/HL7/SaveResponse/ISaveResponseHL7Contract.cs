// ----------------------------------------------------------------------------
// <copyright file="ISaveResponseHL7Contract.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using Abc.ServiceModel.HL7;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// Intarface example
    /// </summary>
    public interface ISaveResponseHL7Contract
    {
        /// <summary>
        /// Processes the invoke.
        /// </summary>
        /// <param name="controlAct">The control act.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="acknowledgement">The acknowledgement.</param>
        /// <param name="queryAcknowledgement">The query acknowledgement.</param>
        void ProcessInvoke(HL7ControlAct controlAct, string receiver, HL7Acknowledgement acknowledgement, HL7QueryAcknowledgement queryAcknowledgement);
    }
}