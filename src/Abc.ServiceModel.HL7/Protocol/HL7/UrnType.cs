namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrnType : IComparable, IComparable<UrnType>, IEquatable<UrnType>
    {
        private const string Regex = @"^(?'URN'URN:IVIS:100001:XSD-HL7V3(-|\.)(2011|2006))-(multicacheschemas|lvext)-(?'lv'[A-Z]{2})[A-Z]{2}_[A-Z]{2}[0-9]{6}(UV([0-9]{2}){0,1})_{0,1}((?'lv'LV)[0-99]{2}){0,1}$";
        private static Regex urnTypeRegex = new Regex(Regex, RegexOptions.Compiled);

        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrnType"/> class.
        /// </summary>
        /// <param name="identification">The identification.</param>
        public UrnType(string identification)
        {
            if (!(!string.IsNullOrEmpty(identification))) {  throw new ArgumentNullException("!string.IsNullOrEmpty(identification)"); }
            if (!(UrnType.IsUrn(identification))) {  throw new FormatException("UrnType.IsUrn(identification)"); }

            this.value = identification;
        }

        /// <summary>
        /// Gets the regex property.
        /// </summary>
        public static string RegexProperty
        {
            get
            {
                return Regex;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Compares the specified identification1.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>Compare result</returns>
        public static int Compare(UrnType identification1, UrnType identification2)
        {
            if (identification1 == identification2)
            {
                return 0;
            }

            if (identification1 == null)
            {
                return -1;
            }

            if (identification2 == null)
            {
                return 1;
            }

            return string.Compare(identification1.value, identification2.value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Equalses the specified identification1.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>Equals result</returns>
        public static bool Equals(UrnType identification1, UrnType identification2)
        {
            if ((identification1 as object) == (identification2 as object))
            {
                return true;
            }

            if (((identification1 as object) == null) || ((identification2 as object) == null))
            {
                return false;
            }

            return string.Equals(identification1.ToString(), identification2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="Abc.ServiceModel.Protocol.HL7.UrnType"/>.
        /// </summary>
        /// <param name="identification">The identification.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator UrnType(string identification)
        {
            return new UrnType(identification);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Abc.ServiceModel.Protocol.HL7.UrnType"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="identification">The identification.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(UrnType identification)
        {
            if (identification == null)
            {
                return null;
            }

            return identification.ToString();
        }
        public static bool IsUrn(string identification)
        {
            if (!(!string.IsNullOrEmpty(identification))) {  throw new ArgumentNullException("identification", "!string.IsNullOrEmpty(identification)"); }
            var match = urnTypeRegex.Match(identification);

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(UrnType identification1, UrnType identification2)
        {
            return !Equals(identification1, identification2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(UrnType identification1, UrnType identification2)
        {
            return Compare(identification1, identification2) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(UrnType identification1, UrnType identification2)
        {
            return Compare(identification1, identification2) <= 0;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(UrnType identification1, UrnType identification2)
        {
            return Equals(identification1, identification2);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(UrnType identification1, UrnType identification2)
        {
            return Compare(identification1, identification2) > 0;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="identification1">The identification1.</param>
        /// <param name="identification2">The identification2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(UrnType identification1, UrnType identification2)
        {
            return Compare(identification1, identification2) >= 0;
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The OId type.</returns>
        public static UrnType Parse(string input)
        {
            if (input == null) {  throw new ArgumentNullException("input", "input != null"); }

            return new UrnType(input);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
        /// </returns>
        int IComparable<UrnType>.CompareTo(UrnType other)
        {
            return UrnType.Compare(this, other);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is OId))
            {
                throw new ArgumentException("Argument Must Be OId");
            }

            return Compare(this, (UrnType)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            UrnType objVar = obj as UrnType;

            if (objVar == null)
            {
                return false;
            }

            return Equals(this, objVar);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(UrnType other)
        {
            return UrnType.Equals(this, other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            if (this.value != null)
            {
                hashCode = this.value.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.value;
        }
    }
}