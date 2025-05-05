using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DataAccess
{
    public static class SqlQueries
    {
        public const string GetOrderWithInfo = "SELECT * FROM AllOrdersWithInfo;";

        public const string GetNotCompletedOrders =
            "SELECT * FROM AllOrdersWithInfo WHERE status != 'Done';";

        public const string GetCompletedOrders =
            "SELECT * FROM AllOrdersWithInfo WHERE status = 'Done';";

        public const string MarkOrderAsCompleted =
            "UPDATE Orders SET status = 'Done' WHERE id = @OrderId;";

        // Service usage
        public const string InsertServiceUsage =
            "INSERT INTO CustomerServiceUsage (OrderId, ServiceId) VALUES (@OrderId, @ServiceId);";

        public const string DeleteServiceUsage =
            "DELETE FROM CustomerServiceUsage WHERE Id = @Id;";

        // Orders
        public const string InsertOrderReturningId =
            "INSERT INTO Orders (CrematoryId, StandardPrice, CremationDuration, DeceasedId, ContactPersonId, OrderDate, CremationDateTime, Status) " +
            "VALUES (@CrematoryId, @StandardPrice, @CremationDuration, @DeceasedId, @ContactPersonId, @OrderDate, @CremationDateTime, @Status::order_status) RETURNING Id;";

        public const string InsertOrder =
            "INSERT INTO Orders (OrderDate, ContactPersonId, DeceasedId, CrematoryId, StandardPrice, CremationDateTime, CremationDuration, Status) " +
            "VALUES (@OrderDate, @ContactPersonId, @DeceasedId, @CrematoryId, @StandardPrice, @CremationDateTime, @CremationDuration, 'pending'::order_status);";

        public const string GetAllOrders =
            "SELECT * FROM Orders;";

        public const string UpdateOrders =
            "UPDATE Orders SET ContactPersonId = @ContactPersonId, DeceasedId = @DeceasedId, StandardPrice = @StandardPrice WHERE Id = @Id;";

        public const string DeleteOrder =
            "DELETE FROM Orders WHERE id = @Id;";

        public const string GetOrderTime = @"
    SELECT 
        CAST(o.cremationdatetime AS time) AS StartTime, 
        CAST(o.cremationdatetime + o.cremationduration AS time) AS EndTime
    FROM Orders AS o
    WHERE 
        o.cremationdatetime::date = @DateTime AND
        o.crematoryid = @CrematoryId
    ORDER BY StartTime;";

        // Crematory schedule
        public const string GetCrematoryScheduleForDay = @"
    SELECT opentime AS StartTime, closetime AS EndTime
    FROM CrematorySchedule 
    WHERE CrematoryId = @CrematoryId AND 
          DayOfWeek = CAST(@DayOfWeek AS day_of_week);";

        // Contact person
        public const string InsertContactPerson =
            "INSERT INTO ContactPerson (FullName, PhoneNumber, Address) VALUES (@FullName, @PhoneNumber, @Address);";

        public const string GetContactPersonId =
            "SELECT Id FROM ContactPerson WHERE FullName = @FullName AND PhoneNumber = @PhoneNumber AND Address = @Address;";

        public const string DeleteContactPerson =
            "DELETE FROM ContactPerson WHERE Id = @Id;";

        public const string GetContactPersonById =
            "SELECT * FROM ContactPerson WHERE Id = @Id;";

        // Services
        public const string GetAllCrematoryServices =
            "SELECT * FROM Service;";

        public const string InsertService =
            "INSERT INTO Service (Name, Price) VALUES (@Name, @Price);";

        public const string DeleteService =
            "DELETE FROM Service WHERE Id = @Id;";

        public const string UpdateService =
            "UPDATE Service SET Name = @Name, Price = @Price WHERE Id = @Id;";

        public const string GetServicesForOrder =
            "SELECT s.Id, s.Name, s.Price FROM Service AS s " +
            "INNER JOIN CustomerServiceUsage AS su ON s.Id = su.ServiceId " +
            "WHERE su.OrderId = @OrderId;";

        // Crematories
        public const string GetAllCrematories =
            "SELECT * FROM Crematory;";

        public const string InsertCrematory =
            "INSERT INTO Crematory (Name, Address, ContactInfo) VALUES (@Name, @Address, @ContactInfo);";

        public const string DeleteCrematory =
            "DELETE FROM Crematory WHERE Id = @Id;";

        public const string UpdateCrematory =
            "UPDATE Crematory SET Name = @Name, Address = @Address, ContactInfo = @ContactInfo WHERE Id = @Id;";

        // Schedule
        public const string GetAllCrematorySchedule =
            "SELECT * FROM CrematorySchedule ORDER BY DayOfWeek;";

        public const string GetScheduleOfCrematory =
            "SELECT * FROM CrematorySchedule WHERE CrematoryId = @CrematoryId ORDER BY DayOfWeek, OpenTime;";

        public const string GetScheduleForDay =
            "SELECT * FROM CrematorySchedule WHERE CrematoryId = @CrematoryId AND DayOfWeek = @DayOfWeek::day_of_week ORDER BY OpenTime;";

        public const string InsertSchedule =
            "INSERT INTO CrematorySchedule (CrematoryId, DayOfWeek, OpenTime, CloseTime) VALUES (@CrematoryId, @DayOfWeek::day_of_week, @OpenTime, @CloseTime);";

        public const string DeleteSchedule =
            "DELETE FROM CrematorySchedule WHERE Id = @Id;";

        public const string UpdateSchedule =
            "UPDATE CrematorySchedule SET CrematoryId = @CrematoryId, DayOfWeek = @DayOfWeek::day_of_week, OpenTime = @OpenTime, CloseTime = @CloseTime WHERE Id = @Id;";

        // Deceased
        public const string InsertDeceased =
            "INSERT INTO Deceased (FullName, BirthDate, DeathDate, Gender) VALUES (@FullName, @BirthDate, @DeathDate, @Gender::gender_type);";

        public const string DeleteDeceased =
            "DELETE FROM Deceased WHERE Id = @Id;";

        public const string UpdateDeceased =
            "UPDATE Deceased SET FullName = @FullName, BirthDate = @BirthDate, DeathDate = @DeathDate, Gender = @Gender::gender_type WHERE Id = @Id;";

        public const string GetDeceasedId =
            "SELECT Id FROM Deceased WHERE FullName = @FullName AND BirthDate = @BirthDate AND DeathDate = @DeathDate;";

        public const string GetDeceasedById =
            "SELECT * FROM Deceased WHERE Id = @Id;";

        // Helpers
        public const string IsScheduleExistsToday =
            "SELECT * FROM CrematorySchedule WHERE DayOfWeek = @DayOfWeek::day_of_week AND CrematoryId = @CrematoryId;";

        public const string OrderByMonth =
            "SELECT c.Name AS CrematoryName, DATE_TRUNC('month', o.OrderDate) AS Month, COUNT(*) AS OrderCount FROM Orders o JOIN Crematory c ON o.CrematoryId = c.Id GROUP BY c.Name, DATE_TRUNC('month', o.OrderDate) ORDER BY Month DESC, OrderCount DESC;";

        public const string MostPopularServices = "SELECT s.Name AS ServiceName, COUNT(*) AS UsageCount FROM CustomerServiceUsage su JOIN Service s ON su.ServiceId = s.Id GROUP BY s.Name ORDER BY UsageCount DESC;";

        public const string MonthRevenue = "SELECT COALESCE(SUM(COALESCE(s.Price, 0) + o.StandardPrice), 0) AS RevenueCurrentMonth FROM Orders o LEFT JOIN CustomerServiceUsage su ON su.OrderId = o.Id LEFT JOIN Service s ON s.Id = su.ServiceId WHERE o.Status = 'Done' AND o.CremationDateTime >= date_trunc('month', CURRENT_DATE) AND o.CremationDateTime < date_trunc('month', CURRENT_DATE) + INTERVAL '1 month';";
    }
}
