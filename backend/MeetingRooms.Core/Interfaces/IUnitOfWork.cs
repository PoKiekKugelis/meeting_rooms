namespace MeetingRooms.Core.Interfaces;

public interface IUnitOfWork
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
