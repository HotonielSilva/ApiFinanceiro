using ApiFinanceiro.Constants;
using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
    {
        if (await _roleManager.RoleExistsAsync(request.RoleName))
            return Conflict(new { Success = false, Message = MensagensSistema.RoleJaExiste });

        var result = await _roleManager.CreateAsync(new IdentityRole(request.RoleName));

        return result.Succeeded
            ? CreatedAtAction(nameof(GetRoles), new { Success = true, Message = MensagensSistema.RoleCriadaSucesso })
            : StatusCode(500, new { Success = false, result.Errors });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleResponse(r.Id, r.Name!))
            .ToList();

        return Ok(roles);
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole(string roleId, [FromBody] RoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
            return NotFound(new { Success = false, Message = MensagensSistema.RoleNaoEncontrada });

        if (role.Name?.ToUpper() == "ADMIN")
            return BadRequest(new { Success = false, Message = MensagensSistema.RoleAdminProtegida });

        role.Name = request.RoleName;
        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded
            ? Ok(new { Success = true, Message = MensagensSistema.RoleAtualizadaSucesso })
            : BadRequest(new { Success = false, Message = MensagensSistema.ErroRole });
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
            return NotFound(new { Success = false, Message = MensagensSistema.RoleNaoEncontrada });

        if (role.Name?.ToUpper() == "ADMIN")
            return BadRequest(new { Success = false, Message = MensagensSistema.RoleAdminProtegida });

        var result = await _roleManager.DeleteAsync(role);

        return result.Succeeded
            ? Ok(new { Success = true, Message = MensagensSistema.RoleDeletadaSucesso })
            : BadRequest(new { Success = false, Message = MensagensSistema.ErroExcluirRole });
    }

    [HttpPost("{userId}/roles")]
    public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] RoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        if (!await _roleManager.RoleExistsAsync(request.RoleName))
            return NotFound(new { Success = false, Message = MensagensSistema.RoleNaoEncontrada });

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);

        return result.Succeeded
            ? Ok(new { Success = true, Message = MensagensSistema.RoleAtribuidaSucessoDinamica(request.RoleName, user.Email!) })
            : BadRequest(new { Success = false, result.Errors });
    }

    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [HttpDelete("{userId}/roles")]
    public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] RoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

        return result.Succeeded
            ? Ok(new { Success = true, Message = MensagensSistema.RoleRemovidaSucessoDinamica(request.RoleName) })
            : BadRequest(new { Success = false, Message = MensagensSistema.ErroRole, result.Errors });
    }
}