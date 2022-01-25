using System;

namespace Gateways.NET.SDK.Contracts
{
    public class ApiException : Exception
    {
        public ApiException(string message, string code)
            :base(message)
        {
            Code = code;
        }

        public string Code { get; protected set; }        
    }
}
