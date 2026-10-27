namespace MeetingRooms.Core.Interfaces;

public interface IUnitOfWork
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);

  Task<T> SerializeWrites<T>(
      Func<CancellationToken, Task<T>> operation,
      CancellationToken cancellationToken);
}
