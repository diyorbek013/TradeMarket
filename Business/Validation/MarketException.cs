using Business.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Validation
{
    [Serializable]
    public class MarketException : Exception, ISerializable
    {
        public MarketException() : base()
        {
        }
        public MarketException(string message) : base(message)
        {
        }

        public MarketException(string message, Exception innnerException): base(message, innnerException) 
        {
        }
        public MarketException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}
