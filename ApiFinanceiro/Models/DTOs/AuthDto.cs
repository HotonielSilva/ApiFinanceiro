using System.ComponentModel.DataAnnotations;

namespace ApiFinanceiro.Models.DTOs;

// --- DTOs de Autenticação ---
public class LoginRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? Nome { get; set; }
}

public class RegisterRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;
}

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "A senha atual é obrigatória.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nova senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres.")]
    public string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "A confirmação não coincide com a nova senha.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O token é obrigatório.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nova senha é obrigatória.")]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

// Usamos 'record' aqui por serem apenas transportadores de dados imutáveis
public record UpdateUserRequest(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    string Nome,
    
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    string Email
);
public record RoleRequest(string RoleName);
public record RoleResponse(string Id, string Name);
public record StatusRequest(bool Ativo);
public record UserListItemResponse(string Id, string Nome, string Email, bool Ativo);
public record UserDetailsResponse(string Id, string Nome, string Email, bool Ativo, IList<string> Roles);
public record AdminResetPasswordRequest(
    [Required(ErrorMessage = "A nova senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    string NewPassword
);
public record ForgotPasswordRequest([Required][EmailAddress] string Email);



