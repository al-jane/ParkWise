using System;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public static class BillingHelper
    {
        public static string ConnStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

        public static decimal CalculateBill(string category, DateTime entryTime, DateTime exitTime)
        {
            decimal baseRate = 50, hourlyRate = 25;
            int graceMin = 15;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT base_rate, hourly_rate, grace_minutes FROM rates WHERE category=@cat", conn))
                    {
                        cmd.Parameters.AddWithValue("@cat", category);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                baseRate = Convert.ToDecimal(reader["base_rate"]);
                                hourlyRate = Convert.ToDecimal(reader["hourly_rate"]);
                                graceMin = Convert.ToInt32(reader["grace_minutes"]);
                            }
                        }
                    }
                }
            }
            catch { }

            TimeSpan duration = exitTime - entryTime;
            double totalMinutes = duration.TotalMinutes;

            if (totalMinutes <= 60 + graceMin)
                return baseRate;

            double extraMinutes = totalMinutes - 60;
            int extraHours = (int)Math.Ceiling(extraMinutes / 60.0);
            double remainingMin = extraMinutes % 60;
            if (remainingMin <= graceMin && remainingMin > 0)
                extraHours--;

            return baseRate + (extraHours * hourlyRate);
        }

        public static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes < 1) return "< 1 min";
            if (duration.TotalHours < 1) return $"{(int)duration.TotalMinutes} min";
            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }
    }
}