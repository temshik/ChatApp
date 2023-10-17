namespace ChatApp.Bll.Exceptions
{
    /// <summary>
    /// Exception class for catchin not found items
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NotFoundException"/ class.>
        /// </summary>
        public NotFoundException()
        {

        }

        /// <summary>
        /// Initializes a new instance of<see cref= "NotFoundException" / class.>
        /// </summary>
        /// <param name="message">The information about catching exception.</param>
        public NotFoundException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of<see cref= "NotFoundException" / class.>
        /// </summary>
        /// <param name="message">The information about catching exception.</param>
        /// <param name="exception">The exception class for getting exception type.</param>
        public NotFoundException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
