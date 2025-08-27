using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CEIS400_ECS
{
    public class DatabaseAccess
    {
        private string _connection;

        // Constructor to create a DB instance and connect to the SQL DB
        public DatabaseAccess()
        {
            _connection = CONST.DB_CONN;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connection);
        }

        public BindingList<Employee> LoadEmployees(BindingList<ITrackable> trackables)
        {
            var list = new BindingList<Employee>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(CONST.SQL_LOAD_EMPLOYEES, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var converter = new GuidConverter();
                        Roles roleCheck = (Roles)System.Enum.Parse(typeof(Roles), reader["Role"].ToString());

                        if (roleCheck == Roles.Customer)
                        {
                            var cust = new Customer
                            {
                                EmpID = reader["EmpID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Status = (EmpStatus)System.Enum.Parse(typeof(EmpStatus), reader["Status"].ToString()),
                                Title = reader["Title"].ToString(),
                                PasswordHash = Convert.ToBase64String((byte[])reader["Password"]),
                                PasswordSalt = Convert.ToBase64String((byte[])reader["PasswordSalt"]),
                                Role = (Roles)System.Enum.Parse(typeof(Roles), reader["Role"].ToString()),
                                SecondRole = (Roles)System.Enum.Parse(typeof(Roles), reader["SecondRole"].ToString()),
                            };

                            List<string> certsID = converter.ConvertToString(reader["Certs"].ToString()).Split(',').ToList();
                            List<string> outItemsID = converter.ConvertToString(reader["OutItems"].ToString()).Split(',').ToList();

                            cust.Certs = trackables.Where(t => certsID.Contains(t.Source)).ToList();
                            cust.OutItems = trackables.Where(t => outItemsID.Contains(t.Source)).ToList();

                            list.Add(cust);
                        }
                        else
                        {
                            var emp = new Employee
                            {
                                EmpID = reader["EmpID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Status = (EmpStatus)System.Enum.Parse(typeof(EmpStatus), reader["Status"].ToString()),
                                Title = reader["Title"].ToString(),
                                PasswordHash = Convert.ToBase64String((byte[])reader["Password"]),
                                PasswordSalt = Convert.ToBase64String((byte[])reader["PasswordSalt"]),
                                Role = (Roles)System.Enum.Parse(typeof(Roles), reader["Role"].ToString()),
                                SecondRole = (Roles)System.Enum.Parse(typeof(Roles), reader["SecondRole"].ToString()),
                            };

                            list.Add(emp);
                        }
                    }
                }
            }
            return list;
        }

        public BindingList<ITrackable> LoadTrackables()
        {
            BindingList<ITrackable> items = new BindingList<ITrackable>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(CONST.SQL_LOAD_TRACKABLES, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Guid trackableID = Guid.Parse(reader["TrackableID"].ToString());
                    byte[] barcode = reader["Barcode"] as byte[];
                    var converter = new GuidConverter();

                    // Deserialize the CheckoutRecords
                    BindingList<CheckoutRecord> checkoutRecords;
                    if (!reader.IsDBNull(reader.GetOrdinal("CheckoutRecords")))
                    {
                        string json = reader["CheckoutRecords"].ToString();
                        checkoutRecords = JsonSerializer.Deserialize<BindingList<CheckoutRecord>>(json);
                    }
                    else
                    {
                        checkoutRecords = new BindingList<CheckoutRecord>();
                    }

                    // If BasicTool
                    if (!reader.IsDBNull(reader.GetOrdinal("BasicToolID")))
                    {
                        items.Add(new BasicTools
                        {
                            ToolID = converter.ConvertToString(reader["BasicToolID"]),
                            Name = reader["BasicToolName"].ToString(),
                            Included = reader["BasicToolIncluded"].ToString().Split(',').ToList(),
                            Remarks = reader["BasicToolRemarks"].ToString(),
                            InDate = reader["BasicToolInDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["BasicToolInDate"]),
                            OutDate = reader["BasicToolOutDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["BasicToolOutDate"]),
                            Status = (InvStatus)System.Enum.Parse(typeof(InvStatus), reader["BasicToolStatus"].ToString()),
                            CheckoutRecords = checkoutRecords
                        });
                    }
                    // If SpecialTool
                    else if (!reader.IsDBNull(reader.GetOrdinal("SpecialToolID")))
                    {
                        items.Add(new SpecialTool
                        {
                            SToolID = converter.ConvertToString(reader["SpecialToolID"]),
                            Name = reader["SpecialToolName"].ToString(),
                            Type = reader["SpecialToolType"].ToString(),
                            Status = (InvStatus)System.Enum.Parse(typeof(InvStatus), reader["SpecialToolStatus"].ToString()),
                            InDate = reader["SpecialToolInDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SpecialToolInDate"]),
                            OutDate = reader["SpecialToolOutDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SpecialToolOutDate"]),
                            Remarks = reader["SpecialToolRemarks"].ToString(),
                            CalDate = reader["SpecialToolCalDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SpecialToolCalDate"]),
                            CalDue = reader["SpecialToolCalDue"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SpecialToolCalDue"]),
                            CertRequired = Convert.ToBoolean(reader["SpecialToolCertRequired"]),
                            Included = reader["SpecialToolIncluded"].ToString().Split(',').ToList(),
                            CheckoutRecords = checkoutRecords
                        });
                    }
                    // If Vehicle
                    else if (!reader.IsDBNull(reader.GetOrdinal("VehicleID")))
                    {
                        items.Add(new Vehicle
                        {
                            VehicleID = converter.ConvertToString(reader["VehicleID"]),
                            Make = reader["Make"].ToString(),
                            Model = reader["Model"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            SerialNum = reader["SerialNum"].ToString(),
                            Status = (InvStatus)System.Enum.Parse(typeof(InvStatus), reader["Status"].ToString()),
                            InDate = reader["InDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["InDate"]),
                            OutDate = reader["OutDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["OutDate"]),
                            Remarks = reader["Remarks"].ToString(),
                            CertRequired = Convert.ToBoolean(reader["CertRequired"]),
                            CheckoutRecords = checkoutRecords
                        });
                    }
                }
            }
            return items;
        }

        public void SaveTrackables(BindingList<ITrackable> item)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                foreach (var t in item)
                {
                    var converter = new GuidConverter();
                    string trackableID = t.Source.ToString();
                    string jsonCheckouts = JsonSerializer.Serialize(t.CheckoutRecords);

                    using (var cmd = new MySqlCommand(CONST.SAVE_SQL_TRACKABLES, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", trackableID);
                        cmd.Parameters.AddWithValue("@Source", t.Source);
                        cmd.Parameters.AddWithValue("@Barcode", t.Barcode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CheckoutRecords", jsonCheckouts);
                        cmd.ExecuteNonQuery();
                    }

                    // If BasicTool
                    if (t is BasicTools bt)
                    {
                        using (var cmd = new MySqlCommand(CONST.SAVE_SQL_BASIC, conn))
                        {
                            cmd.Parameters.AddWithValue("@ToolID", bt.ToolID);
                            cmd.Parameters.AddWithValue("@Name", bt.Name);
                            cmd.Parameters.AddWithValue("@Included", string.Join(",", bt.Included));
                            cmd.Parameters.AddWithValue("@Remarks", bt.Remarks);
                            cmd.Parameters.AddWithValue("@InDate", bt.InDate);
                            cmd.Parameters.AddWithValue("@OutDate", bt.OutDate);
                            cmd.Parameters.AddWithValue("@Status", bt.Status.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // If SpecialTool
                    else if (t is SpecialTool st)
                    {
                        using (var cmd = new MySqlCommand(CONST.SAVE_SQL_SPECIAL, conn))
                        {
                            cmd.Parameters.AddWithValue("@SToolID", st.SToolID);
                            cmd.Parameters.AddWithValue("@Name", st.Name);
                            cmd.Parameters.AddWithValue("@Type", st.Type);
                            cmd.Parameters.AddWithValue("@Included", string.Join(",", st.Included));
                            cmd.Parameters.AddWithValue("@Remarks", st.Remarks);
                            cmd.Parameters.AddWithValue("@InDate", st.InDate);
                            cmd.Parameters.AddWithValue("@OutDate", st.OutDate);
                            cmd.Parameters.AddWithValue("@Status", st.Status.ToString());
                            cmd.Parameters.AddWithValue("@CalDate", st.CalDate);
                            cmd.Parameters.AddWithValue("@CalDue", st.CalDue);
                            cmd.Parameters.AddWithValue("@CertsRequired", st.CertRequired);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // If Vehicle
                    else if (t is Vehicle v)
                    {
                        using (var cmd = new MySqlCommand(CONST.SAVE_SQL_VEHICLES, conn))
                        {
                            cmd.Parameters.AddWithValue("@VehicleID", v.VehicleID);
                            cmd.Parameters.AddWithValue("@Make", v.Make);
                            cmd.Parameters.AddWithValue("@Model", v.Model);
                            cmd.Parameters.AddWithValue("@Year", v.Year);
                            cmd.Parameters.AddWithValue("@SerialNum", v.SerialNum);
                            cmd.Parameters.AddWithValue("@Remarks", v.Remarks);
                            cmd.Parameters.AddWithValue("@CertRequired", v.CertRequired);
                            cmd.Parameters.AddWithValue("@InDate", v.InDate);
                            cmd.Parameters.AddWithValue("@OutDate", v.OutDate);
                            cmd.Parameters.AddWithValue("@Status", v.Status.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                
                }
            }
        }

        public void SaveEmployees(BindingList<Employee> emp)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                foreach (var e in emp)
                {
                    using (var cmd = new MySqlCommand(CONST.SAVE_SQL_EMPLOYEES, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmpID", e.EmpID);
                        cmd.Parameters.AddWithValue("@Name", e.Name);
                        cmd.Parameters.AddWithValue("@Email", e.Email);
                        cmd.Parameters.AddWithValue("@Phone", e.Phone);
                        cmd.Parameters.AddWithValue("@Status", e.Status.ToString());
                        cmd.Parameters.AddWithValue("@Title", e.Title);
                        cmd.Parameters.AddWithValue("@Password", e.PasswordHash);
                        cmd.Parameters.AddWithValue("@PasswordSalt", e.PasswordSalt);
                        cmd.Parameters.AddWithValue("@Role", e.Role.ToString());
                        cmd.Parameters.AddWithValue("@SecondRole",  e.SecondRole.ToString() ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    // If Customer
                    if (e is Customer cust)
                    {
                        using (var cmd = new MySqlCommand(CONST.SAVE_SQL_CUSTOMER, conn))
                        {
                            cmd.Parameters.AddWithValue("@Certs", JsonSerializer.Serialize(cust.Certs.Select(t => t.Source.ToString())));
                            cmd.Parameters.AddWithValue("@OutItems", JsonSerializer.Serialize(cust.OutItems.Select(t => t.Source.ToString())));
                        }
                    }
                }
                
            }
        }

        // Query method that only requires the SQL code as a string
        // example use db.Query("SELECT * FROM Employees")
        // Can be used with any SQL Query
        public DataTable Query(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(_connection))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                DataTable dTable = new DataTable();
                adapter.Fill(dTable);
                return dTable;
            }
        }
        // Excute method performs INSERT, UPDATE and DELETE SQL functions
        // example use db.Execute("INSERT INTO Employees (EmpID, Name, Email) VALUES ('10001', 'John Doe', 'john@gamil.com')");
        public int Execute(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(_connection))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}