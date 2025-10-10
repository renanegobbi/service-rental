namespace Rental.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}