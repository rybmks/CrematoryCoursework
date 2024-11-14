using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DatabaseServices
{
    public static class SqlQueries
    {
        public const string InsertOrder = "INSERT INTO Orders (OrderDate, ContactPerson, ContactPersonId, DeceasedId, CrematoryId, StandardPrice) VALUES (@OrderDate, @ContactPerson, @DeceasedId, @CrematoryId, @StandardPrice);";
        public const string GetAllOrders = "SELECT * FROM Orders";
        public const string UpdateOrders = "UPDATE Orders SET ContactPerson = @ContactPerson, ContactPersonId = @ContactPersonId, DeceasedId = @DeceasedId,  StandardPrice = @StandardPrice; WHERE Id = @Id";
        public const string DeleteOrder = "DELETE FROM Orders WHERE Id = @Id;";

        public const string InsertContactPerson = "INSERT INTO ContactPerson (FullName, PhoneNumber, Address)";
        public const string GetContactPersonId = "SELECT Id FROM ContactPerson WHERE FullName = @FullName AND PhoneNumber = @PhoneNumber AND Address = @Address;";

        public const string GetAllCrematoryServices = "SELECT * FROM Service;";
        public const string GetAllCrematories = "SELECT * FROM Crematory;";
        public const string GetAllCrematorySchedule = "SELECT * FROM CrematorySchedule ORDER BY DayOfWeek;";
        public const string GetScheduleOfCrematory = "SELECT * FROM CrematorySchedule WHERE CrematoryId = 3 ORDER BY DayOfWeek, OpenTime;";
        public const string GetScheduleForDay = "SELECT * FROM CrematorySchedule WHERE CrematoryId = 3 AND DayOfWeek = @DayOfWeek ORDER BY OpenTime;";

        public const string InsertService = "INSERT INTO Service (Name, Price) VALUES (@Name, @Price);";
        public const string DeleteService = "DELETE FROM Service WHERE Id = @Id;";
        public const string UpdateService = "UPDATE Service SET Name = @Name, Price = @Price WHERE Id = @Id;";
        
        public const string InsertCrematory = "INSERT INTO Crematory (Name, Address, ContactInfo) VALUES (@Name, @Address, @ContactInfo);";
        public const string DeleteCrematory = "DELETE FROM Crematory WHERE Id = @Id;";
        public const string UpdateCrematory = "UPDATE Crematory SET Name = @Name, Address = @Address, ContactInfo = @ContactInfo WHERE Id = @Id;";

        public const string InsertSchedule = "INSERT INTO CrematorySchedule (CrematoryId, DayOfWeek, OpenTime, CloseTime) VALUES (@CrematoryId, @DayOfWeek::day_of_week, @OpenTime, @CloseTime);";
        public const string DeleteSchedule = "DELETE FROM CrematorySchedule WHERE Id = @Id;";
        public const string UpdateSchedule = "UPDATE CrematorySchedule SET CrematoryId = @CrematoryId, DayOfWeek = @DayOfWeek::day_of_week, OpenTime = @OpenTime, CloseTime = @CloseTime WHERE Id = @Id;";

        public const string InsertDeceased = "INSERT INTO Deceased (FullName, BirthDate, DeathDate, Gender) VALUES (@FullName, @BirthDate, @DeathDate, @Gender);";
        public const string DeleteDeceased = "DELETE FROM Deceased WHERE Id = @Id;";
        public const string UpdateDeceased = "UPDATE Deceased SET FullName = @FullName, BirthDate = @BirthDate, DeathDate = @DeathDate, Gender = @Gender::gender_type;";
        public const string GetDeceasedId = "SELECT Id FROM Deceased WHERE FullName = @FullName AND BirthDate = @BirthDate AND DeathDate = @DeathDate AND Gender = @Gender::gender_type;";

        public const string IntersectionOfSchedules =
            "SELECT * FROM CrematorySchedule WHERE CrematoryId = @CrematoryId AND DayOfWeek = @DayOfWeek::day_of_week AND (OpenTime < @CloseTime AND CloseTime > @OpenTime);";

    }
}
