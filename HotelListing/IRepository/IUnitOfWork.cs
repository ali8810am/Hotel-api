

using HotelListing.Data;

namespace HotelListing.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<City> Cities { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
