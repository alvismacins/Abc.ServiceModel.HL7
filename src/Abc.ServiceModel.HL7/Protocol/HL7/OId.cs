namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    /// <summary>
    /// HL7 schemas class
    /// </summary>
    [Serializable]
    public struct OId : IComparable, IComparable<OId>, IEquatable<OId>
    {
        private static Regex oidRegex = new Regex(@"[0-2].[0-9](\.[0-9]+)+", RegexOptions.Compiled);
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OId"/> struct.
        /// </summary>
        /// <param name="identification"> The identification. </param>
        public OId(string identification)
        {
            this.value = identification ?? throw new ArgumentNullException(nameof(identification));
            if (!(OId.IsOId(identification))) { throw new FormatException("OId.IsOId(identification)"); }
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
        /// Compares two specified <see cref="OId"/> objects.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> An integer that indicates the lexical relationship between the two comparands. </returns>
        public static int Compare(OId identification1, OId identification2)
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
        /// Determines whether two specified <see cref="OId"/> objects have the same value.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> true if the value of a is the same as the value of b; otherwise, false. </returns>
        public static bool Equals(OId identification1, OId identification2)
        {
            // if (object.Equals(identification1, identification2)) { return true; } if
            // (object.Equals(identification1, null) || object.Equals(identification2, null)) {
            // return false; }
            return string.Equals(identification1.value, identification2.value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Eveses the expand.
        /// </summary>
        /// <param name="extension"> The extension. </param>
        /// <returns> Expanded Oid </returns>
        public static OId EvesExpand(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException($"'{nameof(extension)}' cannot be null or whitespace.", nameof(extension));
            }

            if (!extension.StartsWith(".", StringComparison.OrdinalIgnoreCase)) {  throw new FormatException("extension.StartsWith(\".\", StringComparison.OrdinalIgnoreCase)"); }

            return new OId(HL7Constants.OIds.RootOIdValue + extension);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="OId"/>.
        /// </summary>
        /// <param name="identification"> The identification. </param>
        /// <returns> The result of the conversion. </returns>
        public static explicit operator OId(string identification)
        {
            return new OId(identification);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OId"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="identification"> The identification. </param>
        /// <returns> The result of the conversion. </returns>
        public static implicit operator string(OId identification)
        {
            if (identification == null)
            {
                return null;
            }

            return identification.ToString();
        }
        public static bool IsOId(string identification)
        {
            return identification != null && oidRegex.IsMatch(identification);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator !=(OId identification1, OId identification2)
        {
            return !Equals(identification1, identification2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator <(OId identification1, OId identification2)
        {
            return Compare(identification1, identification2) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator <=(OId identification1, OId identification2)
        {
            return Compare(identification1, identification2) <= 0;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator ==(OId identification1, OId identification2)
        {
            return Equals(identification1, identification2);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator >(OId identification1, OId identification2)
        {
            return Compare(identification1, identification2) > 0;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="identification1"> The identification1. </param>
        /// <param name="identification2"> The identification2. </param>
        /// <returns> The result of the operator. </returns>
        public static bool operator >=(OId identification1, OId identification2)
        {
            return Compare(identification1, identification2) >= 0;
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input"> The input. </param>
        /// <returns> The OId type. </returns>
        public static OId Parse(string input)
        {
            if (input == null) {  throw new ArgumentNullException("input", "input != null"); }
            return new OId(input);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same
        /// position in the sort order as the other object.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance. </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value
        /// has these meanings: Value Meaning Less than zero This instance is less than <paramref
        /// name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero
        /// This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance.
        /// </exception>
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

            return Compare(this, (OId)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare with this instance. </param>
        /// <returns>
        /// <c> true </c> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <c> false </c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is OId))
            {
                return false;
            }

            return Equals(this, (OId)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"> An object to compare with this object. </param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(OId other)
        {
            return OId.Equals(this, other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
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
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other"> An object to compare with this object. </param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero This object is
        /// less than the other parameter.Zero This object is equal to other. Greater than zero This
        /// object is greater than other.
        /// </returns>
        int IComparable<OId>.CompareTo(OId other)
        {
            return OId.Compare(this, other);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns> A <see cref="string"/> that represents this instance. </returns>
        public override string ToString()
        {
            return this.value;
        }
    }
}