namespace HotelListing.Exceptions
{
    public class NameException:Exception
    {
        public string Name { get; set; }

        public NameException()
        {
            
        }

        public NameException(string message):base(message)
        {
            
        }
        public NameException(string message,Exception innerException):base(message,innerException)
        {

        }
        public NameException(string message,string name):base(message)
        {
            Name = name;
        }

    }
}
