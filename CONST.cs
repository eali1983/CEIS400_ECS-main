using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class CONST
    {
        // A list of all Constants for the ECS program
        // Limits
        public const int DAYS_LATE_FLAG = 2;

        //MySql DB
        public const string DB_CONN = "Server=localhost;Database=team3ecs;User ID=root;Password=P@ssw0rd!!!;SslMode=none;";

        // sql queries
        public const string SQL_LOAD_EMPLOYEES = @"
            SELECT e.*, c.Certs, c.OutItems
            FROM employees e
            LEFT JOIN customer c ON e.EmpID = c.EmpID;
        ";

        public const string SQL_LOAD_TRACKABLES = @"
    SELECT 
        t.ID AS TrackableID,
        t.Source,
        t.Barcode,
        t.CheckoutRecords,
        bt.ToolID AS BasicToolID,
        bt.Name AS BasicToolName,
        bt.Included AS BasicToolIncluded,
        bt.Remarks AS BasicToolRemarks,
        bt.InDate AS BasicToolInDate,
        bt.OutDate AS BasicToolOutDate,
        bt.Status AS BasicToolStatus,
        st.SToolID AS SpecialToolID,
        st.Name AS SpecialToolName,
        st.Type AS SpecialToolType,
        st.Status AS SpecialToolStatus,
        st.InDate AS SpecialToolInDate,
        st.OutDate AS SpecialToolOutDate,
        st.Remarks AS SpecialToolRemarks,
        st.CalDate AS SpecialToolCalDate,
        st.CalDue AS SpecialToolCalDue,
        st.CertsRequired AS SpecialToolCertRequired,
        st.Included AS SpecialToolIncluded,
        v.VehicleID,
        v.Make,
        v.Model,
        v.Year,
        v.SerialNum,
        v.Status,
        v.InDate,
        v.OutDate,
        v.Remarks,
        v.CertRequired
    FROM trackable t
    LEFT JOIN basictools bt ON bt.ToolID = t.Source
    LEFT JOIN specialtools st ON st.SToolID = t.Source
    LEFT JOIN vehicles v ON v.VehicleID = t.Source;";

        public const string SAVE_SQL_TRACKABLES = @"
            INSERT INTO trackable (ID, Source, Barcode, CheckoutRecords)
            VALUES (@ID, @Source, @Barcode, @CheckoutRecords)
            ON DUPLICATE KEY UPDATE
                CheckoutRecords = @CheckoutRecords;
        ";

        public const string SAVE_SQL_BASIC = @"
                INSERT INTO basictools (ToolID, Name, Included, Remarks, InDate, OutDate, Status)
                VALUES (@ID, @Name, @Included, @Remarks, @InDate, @OutDate, @Status)
                ON DUPLICATE KEY UPDATE
                    Name = @Name,
                    Included = @Included,
                    Remarks = @Remarks,
                    InDate = @InDate,
                    OutDate = @OutDate,
                    Status = @Status;
            ";

        public const string SAVE_SQL_SPECIAL = @"
                INSERT INTO specialtools (SToolID, Name, Type, Included, Remarks, InDate, OutDate, Status, CalDate, CalDue, CertRequired)
                VALUES (@ID, @Name, @Type, @Included, @Remarks, @InDate, @OutDate, @Status, @CalDate, @CalDue, @CertRequired)
                ON DUPLICATE KEY UPDATE
                    Name = @Name,
                    Type = @Type,
                    Included = @Included,
                    Remarks = @Remarks,
                    InDate = @InDate,
                    OutDate = @OutDate,
                    Status = @Status,
                    CalDate = @CalDate,
                    CalDue = @CalDue,
                    CertRequired = @CertRequired;
            ";

        public const string SAVE_SQL_VEHICLES = @"
                INSERT INTO vehicles (VehicleID, Make, Model, Year, SerialNum, Remarks, CertRequired, InDate, OutDate, Status)
                VALUES (@ID, @Make, @Model, @Year, @SerialNum, @Remarks, @CertRequired, @InDate, @OutDate, @Status)
                ON DUPLICATE KEY UPDATE
                    Make = @Make,
                    Model = @Model,
                    Year = @Year,
                    SerialNum = @SerialNum,
                    Remarks = @Remarks,
                    CertRequired = @CertRequired,
                    InDate = @InDate,
                    OutDate = @OutDate,
                    Status = @Status;
            ";

        public const string SAVE_SQL_EMPLOYEES = @"
            INSERT INTO employees 
                (EmpID, Name, Email, Phone, Status, Title, Password, PasswordSalt, Role, SecondRole)
            VALUES 
                (@EmpID, @Name, @Email, @Phone, @Status, @Title, @Password, @PasswordSalt, @Role, @SecondRole)
            ON DUPLICATE KEY UPDATE
                Name = @Name,
                Email = @Email,
                Phone = @Phone,
                Status = @Status,
                Title = @Title,
                Password = @Password,
                PasswordSalt = @PasswordSalt,
                Role = @Role,
                SecondRole = @SecondRole;
        ";

        public const string SAVE_SQL_CUSTOMER = @"
                INSERT INTO customer (EmpID, Certs, OutItems)
                VALUES (@EmpID, @Certs, @OutItems)
                ON DUPLICATE KEY UPDATE
                    Certs=@Certs, OutItems=@OutItems;
            ";

        public const string GET_EQUIP_FROM_DB = @"
SELECT
    t.ID AS TrackableID,
    t.Source,
    t.Barcode,
    t.CheckoutRecords,
    -- BasicTools columns
    bt.ToolID AS BasicToolID,
    bt.Name AS BasicToolName,
    bt.Included AS BasicToolIncluded,
    bt.Remarks AS BasicToolRemarks,
    bt.InDate AS BasicToolInDate,
    bt.OutDate AS BasicToolOutDate,
    bt.Status AS BasicToolStatus,
    -- SpecialTools columns
    st.SToolID AS SpecialToolID,
    st.Name AS SpecialToolName,
    st.Type AS SpecialToolType,
    st.Included AS SpecialToolIncluded,
    st.Remarks AS SpecialToolRemarks,
    st.CalDate AS SpecialToolCalDate,
    st.CalDue AS SpecialToolCalDue,
    st.CertsRequired AS SpecialToolCertRequired,
    st.InDate AS SpecialToolInDate,
    st.OutDate AS SpecialToolOutDate,
    st.Status AS SpecialToolStatus,
    -- Vehicles columns
    v.VehicleID AS VehicleID,
    v.Make AS VehicleMake,
    v.Model AS VehicleModel,
    v.Year AS VehicleYear,
    v.SerialNum AS VehicleSerialNum,
    v.Remarks AS VehicleRemarks,
    v.CertRequired AS VehicleCertRequired,
    v.InDate AS VehicleInDate,
    v.OutDate AS VehicleOutDate,
    v.Status AS VehicleStatus
FROM trackable t
LEFT JOIN basictools bt ON bt.ToolID = t.Source
LEFT JOIN specialtools st ON st.SToolID = t.Source
LEFT JOIN vehicles v ON v.VehicleID = t.Source;";
    }
}
