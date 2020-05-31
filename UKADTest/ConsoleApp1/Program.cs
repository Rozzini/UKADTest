using System;

namespace ConsoleApp1
{
    class ExcpetionHandler
    {
        private static ExcpetionHandler instance;

        private ExcpetionHandler()
        { }

        public static ExcpetionHandler getInstance()
        {
            if (instance == null)
                instance = new ExcpetionHandler();
            return instance;
        }

        public void Handle<T>(Action op, Action<Exception> result)
        {
            try
            {
                op();
            }
            catch(Exception exception)
            {
                //check type of exceprion
                //if such exception has global handling ( e.g. show error message or log it - do it here
                // othervise call result with this exception
                result(exception);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
