using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Commands.Handlers.Plans;

public class AddProcedureToUserCommandHandler : IRequestHandler<AddProcedureToUserCommand, ApiResponse<Unit>>
{
    private readonly RLContext _context;

    public AddProcedureToUserCommandHandler(RLContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Unit>> Handle(AddProcedureToUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request.PlanId < 1)
            {
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid Plan"));
            }
            if (request.Procedures.Count < 1)
            {
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid Procedure"));
            }

            var invalidProcedureUser = request.Procedures.FirstOrDefault(x => x.UserIds.Count < 1);
            var invalidProcedureId = request.Procedures.FirstOrDefault(x => x.ProcedureId < 1);

            if (invalidProcedureId != null)
            {
                return ApiResponse<Unit>.Fail(new BadRequestException($"Invalid Procedure in ProcedureId: {invalidProcedureId.ProcedureId}"));
            }
            if (invalidProcedureUser != null)
            {
                return ApiResponse<Unit>.Fail(new BadRequestException($"Invalid Users in ProcedureId: {invalidProcedureUser.ProcedureId}"));
            }

            var UserPlanProcedures = await _context.UserPlanProcedure.Where(x => x.PlanId == request.PlanId).ToListAsync();
            var users = await _context.Users.ToListAsync();
            var procedures = await _context.Procedures.ToListAsync();

            var addProcedureToTable = new List<UserPlanProcedure>();
            var removeProcedureFromTable = new List<UserPlanProcedure>();

            foreach (var x in request.Procedures)
            {
                var procedure = procedures?.FirstOrDefault(y => y.ProcedureId == x.ProcedureId);
                if (procedure != null)
                {
                    var tableUserPlanProcedure = UserPlanProcedures?.Where(y => y.ProcedureId == x.ProcedureId).ToList();
                    var userIds = x.UserIds?.ToList();
                    var userRemovedList = tableUserPlanProcedure?.Where(user => userIds == null || !userIds.Contains(user.UserId)).ToList();
                    removeProcedureFromTable.AddRange(userRemovedList);

                    userIds?.ForEach(y =>
                    {
                        var user = users?.FirstOrDefault(u => u.UserId == y);
                        if (user != null)
                        {
                            var userPlanProcedureData = tableUserPlanProcedure?.Find(z => z.UserId == y);
                            if(userPlanProcedureData == null)
                            {
                                addProcedureToTable.Add(new UserPlanProcedure
                                {
                                    UserId = y,
                                    ProcedureId = x.ProcedureId,
                                    PlanId = request.PlanId,
                                });
                            }
                            
                        }
                    });
                }
            }

            if (addProcedureToTable.Count > 0)
            {
                _context.UserPlanProcedure.AddRange(addProcedureToTable);
                // addProcedureToTable?.ForEach(procedure => {
                //     _context.UserPlanProcedure.Add(procedure);
                // });
            }

            if (removeProcedureFromTable.Count > 0)
            {
                _context.UserPlanProcedure.RemoveRange(removeProcedureFromTable);
            }

            await _context.SaveChangesAsync();

            return ApiResponse<Unit>.Succeed(new Unit());
        }
        catch (Exception e)
        {
            return ApiResponse<Unit>.Fail(e);
        }
    }
}