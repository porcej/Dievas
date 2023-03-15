using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TS</c> Telestaff Constants and helper methods
    /// </summary>
	public class TS {

        /// <summary>
        ///     Relative endpoint for TS Roster Requests
        /// </summary>
        public const string RosterEndpoint = "api/v1/wfts/roster";

        /// <summary>
        ///     Relative endpoint for TS Postions Requests
        /// </summary>
        public const string PositionsEndpoint = "api/v1/wfts/organization/positions";

        /// <summary>
        ///     Relative endpoint for TS Schedule Requests
        /// </summary>
        public const string ScheduleEndpoint = "api/v1/wfts/schedule/multi_read";

        /// <summary>
        ///     Relative endpoint for TS Organizational Node Request
        /// </summary>
        public const string OrginizationEndpoint = "api/v1/wfts/organization/multi_read";

        /// <summary>
        ///     Relative endpoint for TS Extra Units
        /// </summary>
        public const string ExtraUnitsEndpoint = "api/v1/wfts/organization/extraUnits";

        /// <summary>
        ///     Contains a list of valid organizational nodes
        /// </summary>
        public static readonly string[] OrganizationNodes = {"INSTITUTION", "AGENCY", "REGION", "STATION", "UNIT"};

        /// <summary>
        ///     Contains a list of valid staffing record types
        /// </summary>
        public static readonly string[] RecordTypes = {"SCHEDULE", "REMOVE_EXCEPTION", "ASSIGNMENT", "POSITION", "EXCEPTION", "VACANCY"};

        /// <summary>
        ///     Working Telestaff WorkCodeTypes
        /// </summary>
        public static readonly string[] WorkingWorkCodeTypes = { "Regular Duty", "Working" };

        /// <summary>
        ///     Non-Working Telestaff WorkCodeTypes
        /// </summary>
        public static readonly string[] NonWorkingWorkCodeTypes = { "Non Working", "Sign Up" };

        /// <summary>
        ///    Checks is <paramref name="organizationNode" /> is a valid organizational node
        /// </summary>
        /// <param name="organizationNode">string Representing an organizational node</param>
        /// <returns> true iff <paramref name="organizationNode" /> is a valid organizational node. ignores case.</returns>
        public static bool IsValidOrganizationNode(string organizationNode){
            return Array.Exists(OrganizationNodes, node => node.ToUpper() == organizationNode.ToUpper());
        }

        /// <summary>
        ///    Checks is <paramref name="workCodeType" /> is a working code type
        /// </summary>
        /// <param name="workCodeType">string Representing a work code type</param>
        /// <returns> true iff <paramref name="workCodeType" /> is in WorkingCodeTypes check ignores case.</returns>
        public static bool IsWorkingCodeType(string workCodeType){
            // return WorkingCodeTypes.Contains(workCodeType, StringComparison.InvariantCultureIgnoreCase);
            return Array.Exists(WorkingWorkCodeTypes, type => type.ToUpper() == workCodeType.ToUpper());
        }

    }
}