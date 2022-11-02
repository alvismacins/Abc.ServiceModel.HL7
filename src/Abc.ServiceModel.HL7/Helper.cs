namespace Abc.ServiceModel
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Abc.ServiceModel.Protocol.HL7;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Helper
    {
        private const string SchemaRegex = @"^(?'lv'[A-Z]{2})[A-Z]{2}_[A-Z]{2}[0-9]{6}(UV([0-9]{2}){0,1})_{0,1}((?'lv'LV)[0-99]{2}){0,1}$";

        /// <summary>
        /// SchemaType enumeration
        /// </summary>
        public enum SchemaType
        {
            /// <summary>
            /// multicacheschemas type
            /// </summary>
            Multicacheschemas,

            /// <summary>
            /// lvext type
            /// </summary>
            Lvext
        }

        /// <summary>
        /// Checks interaction name
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> Schema Type </returns>
        public static SchemaType CheckSchema(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            // MCAI_MT900001UV01 PRPA_MT000011UV01_LV01 LVAU_IN000001UV01
            var match = Regex.Match(value, SchemaRegex);

            if (!match.Success)
            {
                throw new InvalidOperationException("Incorrect name of schema");
            }

            if (match.Groups["lv"].Value == "LV")
            {
                return SchemaType.Lvext;
            }
            else
            {
                return SchemaType.Multicacheschemas;
            }
        }

        /// <summary>
        /// Converts the date time to H l7.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        /// <returns> DateTime format </returns>
        public static string ConvertDateTimeToHL7(DateTime dateTime)
        {
            return XmlConvert.ToString(dateTime, HL7Constants.Formats.DateTimeFormat);
        }

        /// <summary>
        /// Converts the date time to H l7.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        /// <returns> Converted DateTime ToHL7 </returns>
        public static string ConvertDateTimeToHL7(string dateTime)
        {
            DateTime timeOut = TimeParsing(dateTime, false);

            return XmlConvert.ToString(timeOut, HL7Constants.Formats.DateTimeFormat);
        }

        /// <summary>
        /// Converts the date time to H l7 with local time.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        /// <returns> Converted date </returns>
        public static string ConvertDateTimeToHL7WithLocalTime(string dateTime)
        {
            DateTime timeOut = TimeParsing(dateTime, true);

            return XmlConvert.ToString(timeOut, HL7Constants.Formats.DateTimeFormat);
        }

        /// <summary>
        /// Gets the h l7 date time string UTC.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        /// <returns> date string </returns>
        public static string GetHL7DateTimeStringUTC(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                return XmlConvert.ToString(dateTime.ToUniversalTime(), HL7Constants.Formats.OldDateTimeFormat);
            }

            return XmlConvert.ToString(dateTime, HL7Constants.Formats.OldDateTimeFormat);
        }

        /// <summary>
        /// Gets the URN.
        /// </summary>
        /// <param name="interactionId"> The value. </param>
        /// <param name="version">       The version. </param>
        /// <returns> URN Type </returns>
        public static UrnType GetUrnType(string interactionId, Abc.ServiceModel.Protocol.HL7.HL7Constants.Versions.HL7Version version)
        {
            if (!(!string.IsNullOrEmpty(interactionId))) {  throw new ArgumentNullException("interactionId", "!string.IsNullOrEmpty(interactionId)"); }

            var match = Regex.Match(interactionId, SchemaRegex);

            if (!match.Success)
            {
                throw new InvalidOperationException("Incorrect name of schema");
            }

            string ns = "multicacheschemas";
            if (match.Groups["lv"].Value == "LV")
            {
                ns = "lvext";
            }

            string urnType = string.Format(CultureInfo.InvariantCulture, "URN:IVIS:100001:XSD-HL7V3.{0}-{1}-{2}", (int)version, ns, interactionId);

            return new UrnType(urnType);
        }

        /// <summary>
        /// Gets the type of the URN.
        /// </summary>
        /// <param name="interactionId"> The interaction id. </param>
        /// <param name="version">       The version. </param>
        /// <returns> Urn Type </returns>
        public static UrnType GetUrnType(string interactionId, string version)
        {
            if (!(!string.IsNullOrEmpty(interactionId))) {  throw new ArgumentNullException("interactionId", "!string.IsNullOrEmpty(interactionId)"); }

            int versionInt = (int)HL7Constants.Versions.HL7Version.HL72011;

            if (version == HL7Constants.Versions.V3NE2006)
            {
                versionInt = (int)HL7Constants.Versions.HL7Version.HL72006;
            }
            else if (version == HL7Constants.Versions.V3NE2011)
            {
                versionInt = (int)HL7Constants.Versions.HL7Version.HL72011;
            }
            else
            {
                versionInt = (int)HL7Constants.Versions.HL7Version.HL72011;
            }

            var match = Regex.Match(interactionId, SchemaRegex);

            if (!match.Success)
            {
                throw new InvalidOperationException("Incorrect name of schema");
            }

            string ns = "multicacheschemas";
            if (match.Groups["lv"].Value == "LV")
            {
                ns = "lvext";
            }

            string urnType = string.Format(CultureInfo.InvariantCulture, "URN:IVIS:100001:XSD-HL7V3.{0}-{1}-{2}", (int)versionInt, ns, interactionId);

            return new UrnType(urnType);
        }

        /// <summary>
        /// Times the parsing.
        /// </summary>
        /// <param name="value">         The value. </param>
        /// <param name="withLocalTime"> if set to <c> true </c> [with local time]. </param>
        /// <returns> Parsed to HL7 DateTime </returns>
        public static DateTime TimeParsing(string value, bool withLocalTime)
        {
            DateTime timeOut = DateTime.Now;
            string dateFormatAsDate = "yyyy.MM.dd. HH:mm:ss";
            string dateFormatAsDate2 = "dd.MM.yyyy. HH:mm:ss";
            string dateFormatAsDate3 = "yyyy-MM-dd'T'HH:mm:ss.fffffffzzz";
            string dateFormatAsDate4 = "yyyy-MM-dd'T'HH:mm:ss";
            string dateFormatAsDate5 = "yyyy-MM-dd'T'HH:mm:ss.fff";
            string dateFormatAsDate6 = "yyyy-MM-dd'T'HH:mm:ss.fffZ";
            string dateFormatAsDate7 = "yyyy-MM-dd'T'HH:mm:ss.ffZ";
            string dateFormatAsDate8 = "yyyy-MM-dd'T'HH:mm:ss.fZ";
            string dateFormatAsDate9 = "yyyy-MM-dd'T'HH:mm:ssZ";
            string dateFormatAsDate10 = "u";

            // 2012-07-06T10:42:23 2012-06-14T09:58:46.517 Biznesa servisa kļūda!
            // 2012-07-05T15:19:15.930 date is incorrect for format yyyyMMddHHmmss.ffffzz00 bool
            // timeParsing = DateTime.TryParseExact(value, HL7Constants.Formats.DateTimeFormat,
            // CultureInfo.InvariantCulture, DateTimeStyles.None, out timeOut);
            string[] formats = new string[] { HL7Constants.Formats.DateTimeFormat, HL7Constants.Formats.OldDateTimeFormat, dateFormatAsDate, dateFormatAsDate2, dateFormatAsDate3, dateFormatAsDate4, dateFormatAsDate5, dateFormatAsDate6, dateFormatAsDate7, dateFormatAsDate8, dateFormatAsDate9, dateFormatAsDate10 };

            bool timeParsing = DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeOut);

            if (timeParsing)
            {
                if (withLocalTime)
                {
                    return timeOut.ToLocalTime();
                }
                else
                {
                    return timeOut;
                }
            }

            throw new XmlException(string.Format(CultureInfo.InvariantCulture, SR.IncorrectDateTimeFormat, value, HL7Constants.Formats.DateTimeFormat));
        }
    }
}