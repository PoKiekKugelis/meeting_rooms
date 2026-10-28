namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a unit of work that manages database transactions
/// </summary>
public interface IUnitOfWork
{
  /// <summary>
  /// Saves all changes made in the context to the database
  /// </summary>
  /// <param name="cancellationToken">The cancellation token</param>
  /// <returns>The number of state entries written to the database</returns>
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);

  /// <summary>
  /// Executes a write operation in a serialized manner to ensure data consistency
  /// </summary>
  /// <typeparam name="T">The type of the result</typeparam>
  /// <param name="operation">The operation to execute</param>
  /// <param name="cancellationToken">The cancellation token</param>
  /// <returns>The result of the operation</returns>
  Task<T> SerializeWrites<T>(
      Func<CancellationToken, Task<T>> operation,
      CancellationToken cancellationToken);
}
