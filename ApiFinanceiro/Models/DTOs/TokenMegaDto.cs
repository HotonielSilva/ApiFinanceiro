namespace ApiFinanceiro.Models.DTOs;

public record TokenMegaDto(
    DateTime ExpirationToken,
    string AccessToken,
    DateTime ExpirationRefreshToken,
    string RefreshToken
);
