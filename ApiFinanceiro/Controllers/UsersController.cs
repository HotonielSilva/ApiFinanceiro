using ApiFinanceiro.Constants;
using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> ListarUsuarios()
    {
        var usuarios = await _userManager.Users
            .Select(u => new UserListItemResponse(u.Id, u.Nickname, u.Email!, u.Ativo))
            .ToListAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterUsuarioPorId(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);

        if (usuario == null)
            return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var roles = await _userManager.GetRolesAsync(usuario);

        var response = new UserDetailsResponse(
            usuario.Id,
            usuario.Nickname,
            usuario.Email!,
            usuario.Ativo,
            roles
        );

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var usuarioComMesmoEmail = await _userManager.FindByEmailAsync(request.Email);
        if (usuarioComMesmoEmail != null && usuarioComMesmoEmail.Id != id)
        {
            return Conflict(new { Success = false, Message = MensagensSistema.EmailJaEmUso });
        }

        usuario.Nickname = request.Nome;
        usuario.Email = request.Email;
        usuario.UserName = request.Email;

        var result = await _userManager.UpdateAsync(usuario);

        if (result.Succeeded)
        {
            return Ok(new { Success = true, Message = MensagensSistema.UsuarioAtualizadoSucesso });
        }

        return BadRequest(new { Success = false, Message = MensagensSistema.ErroAtualizarDadosUsuario, result.Errors });
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatusUsuario(string id, [FromBody] StatusRequest request)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        usuario.Ativo = request.Ativo;
        var resultado = await _userManager.UpdateAsync(usuario);

        return resultado.Succeeded
            ? Ok(new { Success = true, Message = MensagensSistema.UsuarioStatusAtualizadoDinamico(request.Ativo) })
            : BadRequest(new { Success = false, resultado.Errors });
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> AdminResetPassword(string id, [FromBody] AdminResetPasswordRequest request)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        var result = await _userManager.ResetPasswordAsync(usuario, token, request.NewPassword);

        if (result.Succeeded)
        {
            await _userManager.SetLockoutEndDateAsync(usuario, null);
            return Ok(new { Success = true, Message = MensagensSistema.AdminSenhaResetadaSucesso(usuario.Email!) });
        }

        return BadRequest(new { Success = false, Message = MensagensSistema.ErroGenerico, result.Errors });
    }
}