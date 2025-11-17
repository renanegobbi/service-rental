namespace Rental.Core.Data
{
    /// <summary>
    /// Contract for using the "Unit Of Work" pattern
    /// </summary>
    public interface IUnitOfWork
    {  
        /// <summary>
        /// Starts a transaction in the database
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Persists all changes made in the current unit of work to the database
        /// </summary>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Commits a transaction in the database
        /// </summary>
        Task<bool> CommitTransaction();

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        Task RollbackTransaction();
    }
}