using System.Collections.Generic;
using MySql.Data.MySqlClient;

public static class DatabaseHelper
{
    public static string ConnStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

    // Get all slots with their status
    public static Dictionary<string, string> GetAllSlots()
    {
        var slots = new Dictionary<string, string>();
        using (var conn = new MySqlConnection(ConnStr))
        {
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT slot_id, status FROM slots", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    slots[reader["slot_id"].ToString()] = reader["status"].ToString();
            }
        }
        return slots;
    }

    // Get first available slot
    public static string GetFirstAvailableSlot()
    {
        using (var conn = new MySqlConnection(ConnStr))
        {
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT slot_id FROM slots WHERE status='available' ORDER BY slot_id LIMIT 1", conn))
            {
                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }
    }

    // Update slot status
    public static void UpdateSlotStatus(string slotId, string status)
    {
        using (var conn = new MySqlConnection(ConnStr))
        {
            conn.Open();
            using (var cmd = new MySqlCommand("UPDATE slots SET status=@s WHERE slot_id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@s", status);
                cmd.Parameters.AddWithValue("@id", slotId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Register vehicle
    public static void RegisterVehicle(string reg, string comp, string cat, string owner, string contact, string slotId)
    {
        using (var conn = new MySqlConnection(ConnStr))
        {
            conn.Open();
            using (var cmd = new MySqlCommand(@"INSERT INTO vehicles 
                (reg_number, company, category, owner_name, owner_contact, slot_id) 
                VALUES (@r,@c,@cat,@o,@ct,@s)", conn))
            {
                cmd.Parameters.AddWithValue("@r", reg);
                cmd.Parameters.AddWithValue("@c", comp ?? "");
                cmd.Parameters.AddWithValue("@cat", cat);
                cmd.Parameters.AddWithValue("@o", owner);
                cmd.Parameters.AddWithValue("@ct", contact);
                cmd.Parameters.AddWithValue("@s", slotId);
                cmd.ExecuteNonQuery();
            }
            // Mark slot as occupied
            UpdateSlotStatus(slotId, "occupied");
        }
    }
}