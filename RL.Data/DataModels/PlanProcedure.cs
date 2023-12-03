using RL.Data.DataModels.Common;

namespace RL.Data.DataModels;

public class PlanProcedure : IChangeTrackable
{
    public int ProcedureId { get; set; }
    public int PlanId { get; set; }
    public virtual Procedure Procedure { get; set; }
    public virtual Plan Plan { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
