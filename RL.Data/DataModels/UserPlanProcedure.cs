using System.ComponentModel.DataAnnotations;
using RL.Data.DataModels.Common;

namespace RL.Data.DataModels;

public class UserPlanProcedure : IChangeTrackable
{
    [Key]
    public int UserPlanProcedureId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
    public int ProcedureId { get; set; }
    public Procedure Procedure { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
