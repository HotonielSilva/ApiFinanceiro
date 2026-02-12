using ApiFinanceiro.Constants;
using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Models.Entities;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest registerDto)
    {
        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            Nickname = registerDto.Nome,
            Ativo = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok(new RegisterResponse { Success = true, Message = MensagensSistema.UsuarioCadastradoSucesso });
        }

        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        return BadRequest(new RegisterResponse { Success = false, Message = $"{MensagensSistema.ErroCadastroUsuario} {errors}" });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

        if (!result.Succeeded)
        {
            if (result.IsNotAllowed)
                return Unauthorized(new LoginResponse { Success = false, Message = MensagensSistema.EmailPendenteConfirmacao });

            if (result.IsLockedOut)
                return Unauthorized(new LoginResponse { Success = false, Message = MensagensSistema.UsuarioBloqueadoTentativas });

            return Unauthorized(new LoginResponse { Success = false, Message = MensagensSistema.CredenciaisInvalidas });
        }

        var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

        if (usuario == null)
            return Unauthorized(new LoginResponse { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        if (!usuario.Ativo)
        {
            await _signInManager.SignOutAsync();
            return Unauthorized(new LoginResponse { Success = false, Message = MensagensSistema.UsuarioInativoOuNaoEncontrado });
        }

        try
        {
            var token = await _authService.GerarTokenAsync(loginDto.Email);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = MensagensSistema.LoginSucesso,
                Token = token,
                Nome = usuario.Nickname
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new LoginResponse { Success = false, Message = $"{MensagensSistema.ErroGerarToken} {ex.Message}" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { Success = true, Message = MensagensSistema.LogoutSucesso });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Ok(new { Success = true, Message = MensagensSistema.InstrucoesResetSenhaEnviadas });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // await _emailService.SendResetPasswordEmailAsync(user.Email, token);

        return Ok(new { Success = true, Message = MensagensSistema.TokenResetSenhaEnviado, Token = token });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(new { Success = false, Message = MensagensSistema.ErroGenerico });

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (result.Succeeded)
            return Ok(new { Success = true, Message = MensagensSistema.SenhaRedefinidaSucesso });

        return BadRequest(new { Success = false, result.Errors });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Success = false, Message = MensagensSistema.UsuarioNaoAutenticado("desconhecido") });

        var usuario = await _userManager.FindByIdAsync(userId);
        if (usuario == null)
            return NotFound(new { Success = false, Message = MensagensSistema.UsuarioNaoEncontrado });

        var result = await _userManager.ChangePasswordAsync(usuario, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Ok(new { Success = true, Message = MensagensSistema.SenhaAlteradaSucesso });

        return BadRequest(new { Success = false, Message = MensagensSistema.ErroAlterarSenha });
    }
}