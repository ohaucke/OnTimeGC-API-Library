using System;

namespace OnTimeGC_API.Exception
{
    /// <summary>
    /// InvalidTokenException Exception
    /// </summary>
    [Serializable]
    public class InvalidTokenException : System.Exception
    {
        public InvalidTokenException() : base() { }
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected InvalidTokenException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// InvalidApiResponseException Exception
    /// </summary>
    [Serializable]
    public class InvalidApiResponseException : System.Exception
    {
        public InvalidApiResponseException() : base() { }
        public InvalidApiResponseException(string message) : base(message) { }
        public InvalidApiResponseException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected InvalidApiResponseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
