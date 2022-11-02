namespace Abc.ServiceModel.Protocol.HL7
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// HL7 sh�mas klase.  Vaic�juma atbildes vad�bas darb�bas prasa tikai vienu oblig�to atrib�tu (queryAck.queryId), lai identific�tu vaic�juma sesiju, ar kuru vaic�juma atbildes zi�ojums ir saist�ts. Parametru defin�ciju strukt�ra ir specifiska katram izmanto�anas gad�jumam. [3]
    /// </summary>
    public class HL7QueryAcknowledgement
    {
        private HL7II queryId;
        private HL7QueryResponseCode queryResponseCode;
        private int? resultCurrentQuantity;
        private int? resultRemainingQuantity;
        private int? resultTotalQuantity;
        private HL7StatusCode statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryResponseCode">The query response code.</param>
        public HL7QueryAcknowledgement(HL7QueryResponseCode queryResponseCode)
        {
            if (queryResponseCode == null) {  throw new ArgumentNullException("queryResponseCode", "queryResponseCode != null"); }
            this.QueryResponseCode = queryResponseCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryResponseCode">The query response code.</param>
        public HL7QueryAcknowledgement(Abc.ServiceModel.Protocol.HL7.HL7QueryResponseCode.HL7QueryResponseCodes queryResponseCode)
            : this(new HL7QueryResponseCode(queryResponseCode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryIdExtension">The query id extension.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="queryResponseCode">The query response code.</param>
        public HL7QueryAcknowledgement(string queryIdExtension, HL7StatusCode.HL7StatusCodes statusCode, HL7QueryResponseCode.HL7QueryResponseCodes queryResponseCode)
            : this(new HL7II(HL7Constants.OIds.IdentificationId, queryIdExtension), new HL7StatusCode(statusCode), new HL7QueryResponseCode(queryResponseCode), null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryIdExtension">The query id extension.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="queryResponseCode">The query response code.</param>
        /// <param name="resultTotalQuantity">The result total quantity.</param>
        /// <param name="resultCurrentQuantity">The result current quantity.</param>
        /// <param name="resultRemainingQuantity">The result remaining quantity.</param>
        public HL7QueryAcknowledgement(string queryIdExtension, HL7StatusCode.HL7StatusCodes statusCode, HL7QueryResponseCode.HL7QueryResponseCodes queryResponseCode, int? resultTotalQuantity, int? resultCurrentQuantity, int? resultRemainingQuantity)
            : this(new HL7II(HL7Constants.OIds.IdentificationId, queryIdExtension), new HL7StatusCode(statusCode), new HL7QueryResponseCode(queryResponseCode), resultTotalQuantity, resultCurrentQuantity, resultRemainingQuantity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="queryResponseCode">The query response code.</param>
        /// <param name="resultTotalQuantity">The result total quantity.</param>
        /// <param name="resultCurrentQuantity">The result current quantity.</param>
        /// <param name="resultRemainingQuantity">The result remaining quantity.</param>
        public HL7QueryAcknowledgement(HL7II queryId, HL7StatusCode statusCode, HL7QueryResponseCode queryResponseCode, int? resultTotalQuantity, int? resultCurrentQuantity, int? resultRemainingQuantity)
        {
            if (queryResponseCode == null) {  throw new ArgumentNullException("queryResponseCode", "queryResponseCode != null"); }

            this.QueryId = queryId;
            this.StatusCode = statusCode;
            this.QueryResponseCode = queryResponseCode;
            this.ResultTotalQuantity = resultTotalQuantity;
            this.ResultCurrentQuantity = resultCurrentQuantity;
            this.ResultRemainingQuantity = resultRemainingQuantity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="queryResponseCode">The query response code.</param>
        /// <param name="resultTotalQuantity">The result total quantity.</param>
        /// <param name="resultCurrentQuantity">The result current quantity.</param>
        /// <param name="resultRemainingQuantity">The result remaining quantity.</param>
        public HL7QueryAcknowledgement(HL7II queryId, Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes statusCode, Abc.ServiceModel.Protocol.HL7.HL7QueryResponseCode.HL7QueryResponseCodes queryResponseCode, int? resultTotalQuantity, int? resultCurrentQuantity, int? resultRemainingQuantity)
            : this(queryId, new HL7StatusCode(statusCode), new HL7QueryResponseCode(queryResponseCode), resultTotalQuantity, resultCurrentQuantity, resultRemainingQuantity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="queryResponseCode">The query response code.</param>
        /// <param name="resultTotalQuantity">The result total quantity.</param>
        /// <param name="resultCurrentQuantity">The result current quantity.</param>
        /// <param name="resultRemainingQuantity">The result remaining quantity.</param>
        public HL7QueryAcknowledgement(HL7II queryId, Abc.ServiceModel.Protocol.HL7.HL7StatusCode.HL7StatusCodes statusCode, HL7QueryResponseCode queryResponseCode, int? resultTotalQuantity, int? resultCurrentQuantity, int? resultRemainingQuantity)
            : this(queryId, new HL7StatusCode(statusCode), queryResponseCode, resultTotalQuantity, resultCurrentQuantity, resultRemainingQuantity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HL7QueryAcknowledgement"/> class.
        /// </summary>
        protected HL7QueryAcknowledgement()
        {
        }

        /// <summary>
        /// Gets or sets the query id.
        /// </summary>
        /// <value>
        /// The query id.
        /// </value>
        public HL7II QueryId
        {
            get
            {
                return this.queryId;
            }

            set
            {
                this.queryId = value;
            }
        }

        /// <summary>
        /// Gets or sets the query response code.
        /// </summary>
        /// <value>
        /// The query response code.
        /// </value>
        public HL7QueryResponseCode QueryResponseCode
        {
            get
            {
                return this.queryResponseCode;
            }

            set
            {
                this.queryResponseCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the result current quantity.
        /// </summary>
        /// <value>
        /// The result current quantity.
        /// </value>
        public int? ResultCurrentQuantity
        {
            get
            {
                return this.resultCurrentQuantity;
            }

            set
            {
                this.resultCurrentQuantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the result remaining quantity.
        /// </summary>
        /// <value>
        /// The result remaining quantity.
        /// </value>
        public int? ResultRemainingQuantity
        {
            get
            {
                return this.resultRemainingQuantity;
            }

            set
            {
                this.resultRemainingQuantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the result total quantity.
        /// </summary>
        /// <value>
        /// The result total quantity.
        /// </value>
        public int? ResultTotalQuantity
        {
            get
            {
                return this.resultTotalQuantity;
            }

            set
            {
                this.resultTotalQuantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HL7StatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }

            set
            {
                this.statusCode = value;
            }
        }
    }
}