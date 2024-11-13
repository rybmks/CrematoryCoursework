using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DatabaseServices
{
    public static class SqlQueries
    {
        public const string GetAllCrematoryServices = "SELECT * FROM Service;";
        public const string GetAllCrematories = "SELECT * FROM Crematory;";
        public const string GetAllCrematorySchedule = "SELECT * FROM CrematorySchedule;";
        public const string GetScheduleOfCrematory = "SELECT * FROM CrematorySchedule WHERE CrematoryId = @Id;";

        public const string InsertService = "INSERT INTO Service (Name, Price) VALUES (@Name, @Price);";
        public const string InsertCrematory = "INSERT INTO Crematory (Name, Address, ContactInfo) VALUES (@Name, @Address, @ContactInfo);";
        public const string InsertSchedule = "INSERT INTO CrematorySchedule (CrematoryId, DayOfWeek, OpenTime, CloseTime) VALUES (@CrematoryId, @DayOfWeek::day_of_week, @OpenTime, @CloseTime);";

        public const string DeleteService = "DELETE FROM Service WHERE Id = @Id;";
        public const string DeleteCrematory = "DELETE FROM Crematory WHERE Id = @Id;";
        public const string DeleteSchedule = "DELETE FROM CrematorySchedule WHERE Id = @Id;";

        public const string UpdateService = "UPDATE Service SET Name = @Name, Price = @Price WHERE Id = @Id;";
        public const string UpdateCrematory = "UPDATE Crematory SET Name = @Name, Address = @Address, ContactInfo = @ContactInfo WHERE Id = @Id;";
        public const string UpdateSchedule = "UPDATE CrematorySchedule SET CrematoryId = @CrematoryId, DayOfWeek = @DayOfWeek::day_of_week, OpenTime = @OpenTime, CloseTime = @CloseTime WHERE Id = @Id;";

        public const string IntersectionOfSchedules =
            "SELECT * FROM CrematorySchedule WHERE CrematoryId = @CrematoryId AND DayOfWeek = @DayOfWeek::day_of_week AND (OpenTime < @CloseTime AND CloseTime > @OpenTime);";

    }
}
