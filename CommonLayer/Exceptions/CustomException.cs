using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegistration.CommonLayer.Exceptions
{
    public class CustomExceptions : Exception
    {
        private ExceptionType _exceptionType;
        public enum ExceptionType
        {
            Null_Exception, Empty_String_Exception, Null_Empty_String_Exception, Password_Not_Match_Exception

        }
        public CustomExceptions(CustomExceptions.ExceptionType exceptionType, string message) : base(message)
        {
            _exceptionType = exceptionType;
        }

    }
      

}
