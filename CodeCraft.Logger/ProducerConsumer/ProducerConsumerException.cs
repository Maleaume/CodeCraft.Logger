﻿using System;
using System.Runtime.Serialization;

namespace CodeCraft.Logger.ProducerConsumer
{
    public class ProducerConsumerException : Exception
    {
        /// <summary>
        ///  Initializes a new instance of the ProducerConsumerException class.
        /// </summary>
        public ProducerConsumerException() : base() { }

        /// <summary>
        ///  Initializes a new instance of the ProducerConsumerException class with a specified error
        ///     message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProducerConsumerException(string message) :
            base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the ProducerConsumerException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference 
        ///     if no inner exception is specified</param>
        public ProducerConsumerException(string message, Exception innerException) :
            base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the ProducerConsumerException class with serialized data.
        /// </summary>
        /// <param name="info"> The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///  object data about the exception being thrown.</param>
        /// <param name="context"> The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or ProducerConsumerException.HResult is zero (0).</exception>
        protected ProducerConsumerException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }
    }
}
