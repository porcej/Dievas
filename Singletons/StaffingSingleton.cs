using System;
using System.Collections.Generic;
using System.Linq;
using Dievas.Models.Staffing;

namespace Dievas.Services
{
    /// <summary>
    /// Singleton class that stores Staffing Roster Data.
    /// </summary>
    public sealed class StaffingSingleton
    {
        private static readonly Lazy<StaffingSingleton> _instance = new(() => new StaffingSingleton());

        /// <summary>
        /// Gets the singleton instance of StaffingSingleton.
        /// </summary>
        public static StaffingSingleton Instance => _instance.Value;

        private StaffingSingleton() { }

        /// <summary>
        /// Gets the timestamp of the last Staffing update.
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Gets the list of weather forecast periods.
        /// </summary>
        public Dictionary<DateTime, StaffingCache> rosters = new Dictionary<DateTime, StaffingCache>();

        /// <summary>
        /// Updates the Staffing Roster data.
        /// </summary>
        /// <param name="roster">Staffing infomration.</param>
        /// <param name="rosterDate">Date where the Staffing information is valid.</param>
        public void addRoster(StaffingCache roster, DateTime rosterDate) {
            rosters.TryAdd(rosterDate, roster);
        }

        /// <summary>
        ///     Returns a Staffing infomration for rosterDate if in Rosters.
        /// </summary>
        /// <param name="rosterDate">Date for staffing roster.</param>
        public StaffingCache getRoster(DateTime? rosterDate) {
            
            // Set the date to the provided rosterDate or current date if null          
            DateTime date = rosterDate  ?? DateTime.Now;

            // Make sure we have a date and not a date & time to key on
            date = date.Date;

            if (rosters.ContainsKey(date)) {
                if (rosters[date].IsValid()) {
                    return rosters[date];
                }
            }
            return null;
        }

        /// <summary>
        /// Clean up old Staffing Data
        /// </summary>
        public void cleanRosters(){
            var expiredRosters = rosters.Where(r => r.Value.IsExpired()).ToArray();
            foreach (var expiredRoster in expiredRosters) {
                rosters.Remove(expiredRoster.Key);
            }
        }
    }
}