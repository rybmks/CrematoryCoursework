using Crematory.Models.AppModels;
using Crematory.Models.DatabaseModels;

namespace Crematory.Interfaces
{
    public interface IScheduleRepository
    {
        Task<bool> DeleteScheduleAsync(int id);
        Task<bool> InsertScheduleAsync(CrematoryScheduleModel schedule);
        Task<bool> UpdateScheduleAsync(CrematoryScheduleModel schedule);
        Task<List<CrematoryScheduleModel>> GetScheduleForCrematoryAsync(int crematoryId);
        Task<List<CrematoryScheduleModel>> GetCrematoryScheduleForDayAsync(int crematoryId, DayOfWeek dayOfWeek);
        Task<bool> IsScheduleExistsToday(CrematoryScheduleModel schedule);
        Task<List<TimePeriod>> GetFreeTimeAsync(int сrematoryId, DateOnly date);
    }
}
