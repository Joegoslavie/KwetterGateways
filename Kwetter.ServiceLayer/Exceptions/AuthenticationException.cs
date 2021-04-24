﻿namespace Kwetter.Business.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Authentication exception class.
    /// </summary>
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public AuthenticationException(string message)
            : base (message)
        {
        }
    }
}
