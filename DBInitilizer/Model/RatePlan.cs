//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RatePlan
    {
        public RatePlan()
        {
            this.Rates = new HashSet<Rate>();
            this.RoomPlanBookRuleMappings = new HashSet<RoomPlanBookRuleMapping>();
        }
    
        public int Id { get; set; }
        public Nullable<int> RatePlanCode { get; set; }
        public string RatePlanCategory { get; set; }
        public Nullable<bool> IsCommissionable { get; set; }
        public Nullable<bool> RateReturn { get; set; }
        public string MarketCode { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public string HoteID { get; set; }
    
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<RoomPlanBookRuleMapping> RoomPlanBookRuleMappings { get; set; }
    }
}
