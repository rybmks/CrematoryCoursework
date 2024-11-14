using Crematory.Models;

namespace Crematory.Interfaces
{
    public interface IScheduleRepository
    {
        Task<bool> DeleteScheduleAsync(int id);
        Task<bool> InsertScheduleAsync(CrematoryScheduleModel schedule);
        Task<bool> UpdateScheduleAsync(CrematoryScheduleModel schedule);
        Task<List<CrematoryScheduleModel>> GetScheduleForCrematoryAsync(int crematoryId);
        Task<List<CrematoryScheduleModel>> GetCrematoryScheduleForDayAsync(int crematoryId, DayOfWeek dayOfWeek);
        Task<bool> IsScheduleIntersectsAsync(CrematoryScheduleModel schedule);
    }
}
