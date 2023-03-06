namespace HotelListing.Data
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}
