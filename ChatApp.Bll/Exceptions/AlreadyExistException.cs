namespace ChatApp.Bll.Exceptions
{
    /// <summary>
    /// Exception class for catchin existing items
    /// </summary>
    public class AlreadyExistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AlreadyExistException"/ class.>
        /// </summary>
        public AlreadyExistException()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="AlreadyExistException"/ class.>
        /// </summary>
        /// <param name="message">The information about catching exception.</param>
        public AlreadyExistException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="AlreadyExistException"/ class.>
        /// </summary>
        /// <param name="message">The information about catching exception.<param>
        /// <param name="exception">The exception class for getting exception type.</param>
        public AlreadyExistException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
