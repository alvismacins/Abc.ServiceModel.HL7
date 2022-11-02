namespace System.ServiceModel.Description
{
    using Abc.ServiceModel.HL7;
    using System;
    using System.Reflection;

    internal static class OperationDescriptionExtensions
    {
        public static Type GetReturnType(this OperationDescription operationDescription)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            Type outputType = null;
#if NET45_OR_GREATER || NETCOREAPP
            outputType = operationDescription.TaskMethod.ReturnType.GetGenericArguments()[0];
#endif
#if NET45_OR_GREATER
            if (outputType == null) {
                outputType = operationDescription.SyncMethod?.ReturnType;
            }
#elif !NETCOREAPP
            outputType = operationDescription.SyncMethod?.ReturnType;
#endif
            if (outputType is null)
            {
                throw new InvalidOperationException("wrong method definition");
            }

            return outputType;
        }

        public static MethodInfo GetMethodInfo(this OperationDescription operationDescription)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            var methodInfo = (MethodInfo)null;
#if NET45_OR_GREATER || NETCOREAPP
            methodInfo = operationDescription.TaskMethod;
#endif
#if NET45_OR_GREATER
            if (methodInfo == null) {
                methodInfo = operationDescription.SyncMethod;
            }
#elif !NETCOREAPP
            methodInfo = operationDescription.SyncMethod;
#endif

            return methodInfo;
        }

        public static Type GetInputType(this OperationDescription operationDescription)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            Type inputType = null;
            if (operationDescription.Messages != null 
                && operationDescription.Messages.Count > 0 
                && operationDescription.Messages[0].Body != null 
                && operationDescription.Messages[0].Body.Parts != null 
                && operationDescription.Messages[0].Body.Parts.Count > 0)
            {
                inputType = operationDescription.Messages[0].Body.Parts[0].Type;
            }

            if (inputType is null)
            {
                throw new InvalidOperationException("wrong method definition");
            }

            return inputType;
        }

        public static T GetOperationContract<T>(this OperationDescription operationDescription)
        {
            if (operationDescription is null)
            {
                throw new ArgumentNullException(nameof(operationDescription));
            }

            T operationContract = default;
            if (operationDescription.DeclaringContract != null
                && operationDescription.DeclaringContract.Operations != null
                && operationDescription.DeclaringContract.Operations.Count > 0)
            {
                operationContract = operationDescription.DeclaringContract.Operations[0].Behaviors.Find<T>();
            }

            if (operationContract is null)
            {
                throw new InvalidOperationException("wrong method definition");
            }

            return operationContract;
        }
    }
}