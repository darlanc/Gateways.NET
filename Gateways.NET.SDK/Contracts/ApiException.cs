using System;

namespace Gateways.NET.SDK.Contracts
{
    public class ApiException : Exception
    {
        public ApiException(string message, int code)
            :base(message)
        {
            Code = code;
        }

        public int Code { get; protected set; }        
    }
}
