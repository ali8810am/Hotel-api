using HotelListing.Data;
using HotelListing.IRepository;

namespace HotelListing.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<City> _cities;
        private IGenericRepository<Hotel> _hotels;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<City> Cities  => _cities ??= new GenericRepository<City>(_context);
        public IGenericRepository<Hotel> Hotels =>_hotels ??= new GenericRepository<Hotel>(_context);
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
