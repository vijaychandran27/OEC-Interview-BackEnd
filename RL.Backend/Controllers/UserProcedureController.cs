using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RL.Backend.Commands;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserProcedureController : ControllerBase
{
    private readonly ILogger<UserProcedureController> _logger;
    private readonly RLContext _context;
    private readonly IMediator _mediator;

    public UserProcedureController(ILogger<UserProcedureController> logger, RLContext context, IMediator mediator)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    }

    [HttpGet]
    [EnableQuery]
    public IEnumerable<UserPlanProcedure> GetAll(int planId)
    {
        return _context.UserPlanProcedure.Where(x => x.PlanId == planId);
    }

    [HttpPost]
    public async Task<IActionResult> AsignProcedure([FromBody] AddProcedureToUserCommand command, CancellationToken token)
    {
        var response = await _mediator.Send(command, token);

        return response.ToActionResult();
    }
}
