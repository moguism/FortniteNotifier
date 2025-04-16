namespace backend;

using backend.Repositories;

public class UnitOfWork
{
    private readonly Context _context;
    private UserRepository _userRepository;
    private WishlistRepository _wishlistRepository;

    // poner todos los repositorys

    public UserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public WishlistRepository WishlistRepository => _wishlistRepository ??= new WishlistRepository(_context);
    
    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public Context Context => _context;

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
