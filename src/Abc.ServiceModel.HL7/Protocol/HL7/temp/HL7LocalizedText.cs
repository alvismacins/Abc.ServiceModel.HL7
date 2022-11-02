// ----------------------------------------------------------------------------
// <copyright file="HL7LocalizedText.cs" company="ABC software">
//    Copyright © ABC SOFTWARE. All rights reserved.
//    The source code or its parts to use, reproduce, transfer, copy or
//    keep in an electronic form only from written agreement ABC SOFTWARE.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HL7LocalizedText
    {
        private string value;
        private HL7ClassificatorId language;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7LocalizedText"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="language">The language.</param>
        public HL7LocalizedText(string value, HL7ClassificatorId language)
        {
            this.value = value;
            this.language = language;
        }
    }
}
