namespace ApiFinanceiro.Constants;

public static class MensagensSistema
{
    #region Swagger
    public const string SwaggerTitulo = "Financeiro - API";
    public const string SwaggerDescricao = "Esta API serve para Cadastro e liberação de acessos dos usuários às APIs do Financeiro.";
    public const string SwaggerLicense = "https://opensource.org/licenses/MIT";
    #endregion

    #region Segurança
    public const string SecuritySchemeName = "Bearer";
    public const string SecurityHeaderName = "Authorization";
    public const string SecurityBearerFormat = "JWT";
    public const string SecurityDescription = "Insira o token JWT desta maneira: Bearer {seu token}";
    #endregion

    #region Autenticação e Senha
    public const string LoginSucesso = "Login realizado com sucesso.";
    public const string LogoutSucesso = "Logout realizado com sucesso.";
    public const string UsuarioCadastradoSucesso = "Usuário cadastrado com sucesso.";
    public const string ErroCadastroUsuario = "Erro ao cadastrar usuário.";
    public const string CredenciaisInvalidas = "Credenciais inválidas. Verifique seu usuário e senha.";
    public const string UsuarioInativoOuNaoEncontrado = "Sua conta está inativa. Entre em contato com o administrador.";
    public const string ErroGerarToken = "Erro interno ao gerar o token.";
    public const string EmailPendenteConfirmacao = "E-mail pendente de confirmação.";
    public const string UsuarioBloqueadoTentativas = "Usuário bloqueado por múltiplas tentativas.";
    public const string InstrucoesResetSenhaEnviadas = "Se o e-mail estiver cadastrado, você receberá as instruções para resetar a senha.";
    public const string TokenResetSenhaEnviado = "Instruções enviadas para o e-mail cadastrado.";
    public const string SenhaRedefinidaSucesso = "Senha redefinida com sucesso!";
    public const string SenhaAlteradaSucesso = "Senha alterada com sucesso!";
    public const string ErroAlterarSenha = "Erro ao alterar senha. Verifique se a senha atual está correta.";
    #endregion

    #region Usuários
    public const string UsuarioNaoEncontrado = "Usuário não encontrado.";
    public const string EmailJaEmUso = "Este e-mail já está sendo utilizado por outra conta.";
    public const string UsuarioAtualizadoSucesso = "Dados do usuário atualizados com sucesso.";
    public const string ErroAtualizarDadosUsuario = "Erro ao atualizar usuário.";

    public static string UsuarioStatusAtualizadoDinamico(bool ativo) =>
        $"Status atualizado para {(ativo ? "ativo" : "inativo")}.";

    public static string AdminSenhaResetadaSucesso(string email) =>
        $"Senha do usuário {email} resetada com sucesso.";
    #endregion

    #region Roles
    public const string RoleJaExiste = "A role já existe.";
    public const string RoleCriadaSucesso = "Role criada com sucesso.";
    public const string RoleNaoEncontrada = "Role não encontrada.";
    public const string RoleAtualizadaSucesso = "Role atualizada com sucesso.";
    public const string RoleDeletadaSucesso = "Role deletada com sucesso.";
    public const string RoleAdminProtegida = "A role 'Admin' é vital para o sistema e não pode ser alterada ou excluída.";
    public const string ErroRole = "Erro ao processar role.";
    public const string ErroExcluirRole = "Erro ao excluir a role.";

    public static string RoleAtribuidaSucessoDinamica(string role, string email) =>
        $"Role '{role}' atribuída a {email}.";

    public static string RoleRemovidaSucessoDinamica(string role) =>
        $"Role '{role}' removida.";
    #endregion

    #region Erros Gerais
    public const string ErroGenerico = "Ocorreu um erro inesperado. Tente novamente mais tarde.";

    public static string UsuarioNaoAutenticado(string usuario) =>
        $"Usuário {usuario} não autenticado ou token inválido.";

    public static string UsuarioSemPermissao(string recurso) =>
        $"Usuário não possui permissão para acessar o recurso: {recurso}.";
    #endregion
}
