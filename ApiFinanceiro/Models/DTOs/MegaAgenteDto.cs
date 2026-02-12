namespace ApiFinanceiro.Models.DTOs;

public record MegaAgenteDto
{
    public Agente Agente { get; set; } = new();
}

public class Agente
{
    public int? CodigoSistema { get; set; }
    public int? CodigoAgenteConsolidador { get; set; }

    public string? NomeFantasia { get; set; }
    public string? NomeAgente { get; set; }

    public string? TipoInscricaoESocial { get; set; }
    public string? TipoPessoa { get; set; }
    public string? TipoPessoaRural { get; set; }

    public bool? IsFluxoCaixa { get; set; }
    public int? NaturezaJuridica { get; set; }

    public string? CodigoCNAE { get; set; }
    public string? Email { get; set; }
    public string? Url { get; set; }

    public string? PaisSigla { get; set; }
    public string? UfSigla { get; set; }

    public string? EnquadramentoEmpresa { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? InscricaoMunicipal { get; set; }

    public string? CodigoCEI { get; set; }
    public string? Cnpj { get; set; }

    public int? CodigoMunicipio { get; set; }

    public string? TipoLogradouro { get; set; }
    public string? NomeLogradouro { get; set; }
    public string? NumeroEndereco { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public string? CaixaPostal { get; set; }
    public string? ComplementoEndereco { get; set; }
    public string? CepCaixaPostal { get; set; }
    public string? ReferenciaEndereco { get; set; }

    public bool? IsOrgaoPublico { get; set; }

    public string? NumeroCAEPF { get; set; }
    public string? NumeroCNO { get; set; }

    public string? TipoDespAduaneira { get; set; }
    public int? TipoCAEPF { get; set; }

    public string? NumeroPISMEI { get; set; }
    public string? CodigoCBOMEI { get; set; }

    public string? FretePesoValor { get; set; }

    public bool? IsDispensadoNIF { get; set; }
    public string? NumeroNIF { get; set; }

    public string? CodigoAlternativo { get; set; }
    public int? CodigoMunicipioIBGE { get; set; }

    public AgenteId? AgenteId { get; set; }
    public List<Endereco>? Endereco { get; set; }
    public List<Telefone>? Telefone { get; set; }
    public PessoaFisica? PessoaFisica { get; set; }
    public List<Contato>? Contato { get; set; }
    public List<Fiscal>? Fiscal { get; set; }
    public Parametro? Parametros { get; set; }

    public Cliente? Cliente { get; set; }
}

public class AgenteId
{
    public string? CodigoAlternativo { get; set; }
    public bool? IsExcecaoFiscal { get; set; }
    public bool? IsAtivadoSaldo { get; set; }
    public int? PercentualFUNRURAL { get; set; }

    public DateTime? DataInicioMovimentacao { get; set; }
    public string? Status { get; set; }
    public string? TipoDataBase { get; set; }
    public string? CondicaoPagamento { get; set; }

    public int? Categoria { get; set; }
}

public class Endereco
{
    public int? CodigoEndereco { get; set; }

    public string? PaisSigla { get; set; }
    public string? UfSigla { get; set; }
    public string? TipoLogradouro { get; set; }
    public string? TipoEndereco { get; set; }

    public int? CodigoMunicipio { get; set; }
    public int? CodigoIbge { get; set; }

    public string? NomeMunicipio { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }

    public string? CaixaPostal { get; set; }
    public string? Complemento { get; set; }
    public string? Referencia { get; set; }

    public string? Telefone { get; set; }
    public string? Fax { get; set; }

    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }

    public string? TipoInscricaoEndAg { get; set; }
    public string? EnderecoPertence { get; set; }

    public string? Nome { get; set; }
    public string? Email { get; set; }
}

public class Telefone
{
    public string? Numero { get; set; }
    public string? Tipo { get; set; }
}

public class PessoaFisica
{
    public string? TipoPessoa { get; set; }
    public string? Cpf { get; set; }
    public string? Rg { get; set; }

    public DateTime? DataNascimento { get; set; }
    public string? LocalNascimento { get; set; }

    public string? NomePai { get; set; }
    public string? NomeMae { get; set; }

    public string? LocalTrabalho { get; set; }
    public string? TelefoneTrabalho { get; set; }

    public DateTime? DataAdmissao { get; set; }

    public string? Cargo { get; set; }
    public int? Salario { get; set; }

    public string? CarteiraTrabalho { get; set; }
    public string? Nacionalidade { get; set; }

    public string? EstadoCivil { get; set; }
    public string? Sexo { get; set; }

    public string? Conjuge { get; set; }

    public DateTime? DataEmissaoRG { get; set; }
    public string? OrgaoEmissorRG { get; set; }

    public string? NumeroPIS { get; set; }
    public string? NumeroINSS { get; set; }
}

public class Contato
{
    public int? CodigoContato { get; set; }
    public string? Nome { get; set; }

    public int? CodigoCargo { get; set; }
    public DateTime? DataNascimento { get; set; }

    public string? TelefoneResidencial { get; set; }
    public string? Celular { get; set; }
    public string? TelefoneComercial { get; set; }

    public string? Email { get; set; }
}

public class Fiscal
{
    public DateTime? InicioVigencia { get; set; }

    public bool? IsEnquadradoIPI { get; set; }
    public bool? IsEnquadradoICMS { get; set; }
    public bool? IsEnquadradoISS { get; set; }

    public string? TipoRegimeTributario { get; set; }
}

public class Parametro
{
    public int FilInCodigo { get; set; } // obrigatório
    public string? CnpjFilial { get; set; }
    public int? CodigoUsuario { get; set; }
    public string? NomeUsuario { get; set; }
}

public class Cliente
{
    public int? CodigoTransportadora { get; set; }
    public int? NumeroBanco { get; set; }
}
