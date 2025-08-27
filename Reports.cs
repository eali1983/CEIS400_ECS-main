﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;

namespace CEIS400_ECS
{
    public class Reports
    {
        private string _connection = CONST.DB_CONN;

        private List<ITrackable> GetEquipFromDB()
        {
            // Needs SQL Commands to be added
            List<ITrackable> equipment = new List<ITrackable>();
            using (MySqlConnection connection = new MySqlConnection(_connection))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(CONST.GET_EQUIP_FROM_DB);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = reader["Source"].ToString();
                    ITrackable equipmentItem;

                    switch (type)
                    {
                        // BasicTools
                        case "BasicTools":
                            equipmentItem = new BasicTools
                            {
                                ToolID = reader.GetString("BasicToolID"), // <--- These columns numbers can be replaced with the actual column names in DB
                                Name = reader.GetString("BasicToolName"),
                                InDate = reader.GetDateTime("BasicToolInDate"),
                                OutDate = reader.GetDateTime("BasicToolOutDate"),
                                Status = (InvStatus)reader.GetValue(reader.GetOrdinal("BasicToolStatus")),
                                Included = reader.GetString("BasicToolIncluded").Split(',').ToList(),
                                Remarks = reader.GetString("BasicToolRemarks"),
                                CheckoutRecords = JsonSerializer.Deserialize<BindingList<CheckoutRecord>>(reader.GetString(reader.GetOrdinal("CheckoutRecords")))
                            };
                            break;

                        // Special Tools
                        case "SpecialTool":
                            equipmentItem = new SpecialTool
                            {
                                SToolID = reader.GetString("SpecialToolID"),
                                Name = reader.GetString("SpecialToolName"),
                                Type = reader.GetString("SpecialToolType"),
                                Status = (InvStatus)reader.GetValue(reader.GetOrdinal("SpecialToolStatus")),
                                InDate = reader.GetDateTime("SpecialToolInDate"),
                                OutDate = reader.GetDateTime("SpecialToolOutDate"),
                                Remarks = reader.GetString("SpecialToolRemarks"),
                                CalDate = reader.GetDateTime("SpecialToolCalDate"),
                                CalDue = reader.GetDateTime("SpecialToolCalDue"),
                                CertRequired = reader.GetBoolean("SpecialToolCertsRequired"),
                                Included = reader.GetString("SpeicalToolIncluded").Split(',').ToList(),
                                CheckoutRecords = JsonSerializer.Deserialize<BindingList<CheckoutRecord>>(reader.GetString(reader.GetOrdinal("CheckoutRecords")))
                            };
                            break;

                        // Vehicle
                        case "Vehicle":
                            equipmentItem = new Vehicle
                            {
                                VehicleID = reader.GetString("VehicleID"),
                                Make = reader.GetString("VehicleMake"),
                                Model = reader.GetString("VehicleModel"),
                                Year = reader.GetInt32("VehicleYear"),
                                SerialNum = reader.GetString("SerialNum"),
                                Status = (InvStatus)reader.GetValue(reader.GetOrdinal("VehicleStatus")),
                                InDate = reader.GetDateTime("VehicleInDate"),
                                OutDate = reader.GetDateTime("VehicleOutDate"),
                                Remarks = reader.GetString("VehicleRemarks"),
                                CertRequired = reader.GetBoolean("VehicleCertRequired"),
                                CheckoutRecords = JsonSerializer.Deserialize<BindingList<CheckoutRecord>>(reader.GetString(reader.GetOrdinal("CheckoutRecords")))
                            };
                            break;

                        // Default
                        default:
                            continue;
                    }

                    equipment.Add(equipmentItem);
                }

                return equipment;
            }
        }

        // Methods for generating reports
        // These can be modified or other report methods can be added
        public List<ITrackable> GetOverDueItems()
        {
            var equip = GetEquipFromDB();
            return equip.Where(e => (DateTime.Now - e.OutDate.Value).TotalDays >= CONST.DAYS_LATE_FLAG).ToList();
        }

        public List<ITrackable> GetMissingItems()
        {
            var equip = GetEquipFromDB();
            return equip.Where(e => e.Status == InvStatus.Missing).ToList();
        }

        public List<ITrackable> GetOutForServiceItems()
        {
            var equip = GetEquipFromDB();
            return equip.Where(e => e.Status == InvStatus.OutForService).ToList();
        }

        public List<SpecialTool> GetCalDueItems()
        {
            return GetEquipFromDB().OfType<SpecialTool>().Where(st => st.DueForCalibration() == true).ToList();
        }
    }
}
