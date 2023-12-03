using MediatR;
using RL.Backend.Models;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Backend.Commands;

public class AddProcedureToUserCommand : IRequest<ApiResponse<Unit>>
{
    public int PlanId { get; set; }
    public ICollection<AssignProcedure> Procedures { get; set; }
}

// public class AssignProcedure
// {
//     private ICollection<int> _userIds;

//     [JsonIgnore]
//     public ICollection<int> UserIds
//     {
//         get => _userIds;
//         set
//         {
//             _userIds = value;
//             SerializedUserIds = value?.ToArray(); // Convert to an array or another serializable type
//         }
//     }

//     [JsonPropertyName("UserIds")]
//     public int[] SerializedUserIds { get; set; }

//     public int ProcedureId { get; set; }
// }

public class AssignProcedure
{
    public ICollection<int> UserIds { get; set; }
    public int ProcedureId { get; set; }
}
