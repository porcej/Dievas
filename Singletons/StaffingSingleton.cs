using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        /// Dictionary containing Staffing Information Keyed on Date
        /// </summary>
        private readonly ConcurrentDictionary<DateTime, StaffingCache> _rosters = new ConcurrentDictionary<DateTime, StaffingCache>();

        /// <summary>
        /// Updates the Staffing Roster data.
        /// </summary>
        /// <param name="roster">Staffing infomration.</param>
        /// <param name="rosterDate">Date where the Staffing information is valid.</param>
        public void AddRoster(StaffingCache roster, DateTime rosterDate) {
            _rosters.TryAdd(rosterDate.Date, roster);
            LastUpdated = DateTime.Now;  // Track last update time
        }

        /// <summary>
        ///     Returns a Staffing infomration for rosterDate if in Rosters.
        /// </summary>
        /// <param name="rosterDate">Date for staffing roster.</param>
        public StaffingCache GetRoster(DateTime? rosterDate) {
            
            // Set the date to the provided rosterDate or current date if null          
            DateTime date = (rosterDate  ?? DateTime.Now).Date;
            return _rosters.TryGetValue(date, out var roster) ? roster : null;
        }

        /// <summary>
        /// Clean up old Staffing Data
        /// </summary>
        public void CleanRosters() {
            DateTime now = DateTime.Now.Date;
            var keysToRemove = _rosters.Keys.Where(date => date < now).ToList();
            foreach (var date in keysToRemove) {
                _rosters.TryRemove(date, out _);
            }
        }
    }
}