namespace BureauHexagonal.Core.Common.NotificationError
{
    public static class DefaultMessagesErrors
    {
        // Validações de Presença e Tipo
        public static string FieldIsRequired(string field) => $"O campo {field} é obrigatório.";
        public static string InvalidFormat(string field) => $"O campo {field} possui um formato inválido.";
        public static string MustBeString(string field) => $"O campo {field} deve ser uma sequência de caracteres.";
        public static string MustBeNumber(string field) => $"O campo {field} deve ser um número válido.";

        // Validações Numéricas
        public static string GreaterThanZero(string field) => $"O campo {field} deve ser maior que zero.";
        public static string LessThanZero(string field) => $"O campo {field} deve ser menor que zero.";
        public static string EqualToZero(string field) => $"O campo {field} deve ser igual a zero.";
        public static string NonNegative(string field) => $"O campo {field} não pode ser um valor negativo.";
        public static string MinValue(string field, object min) => $"O campo {field} deve ser no mínimo {min}.";
        public static string MaxValue(string field, object max) => $"O campo {field} deve ser no máximo {max}.";
        public static string RangeValue(string field, object min, object max) => $"O campo {field} deve estar entre {min} e {max}.";

        // Validações de Texto
        public static string MinLength(string field, int length) => $"O campo {field} deve ter pelo menos {length} caracteres.";
        public static string MaxLength(string field, int length) => $"O campo {field} não pode exceder {length} caracteres.";
        public static string ExactLength(string field, int length) => $"O campo {field} deve ter exatamente {length} caracteres.";

        // Validações de Identificadores
        public const string InvalidCpf = "O CPF informado é inválido.";
        public const string InvalidCnpj = "O CNPJ informado é inválido.";
        public const string InvalidCep = "O CEP informado é inválido.";
        public const string EnumIsInvalid = "O enum é inválido.";
        public const string RequestIsNull = "Os dados da requisição não foram informados.";
        public const string BureauTypeInvalid = "O tipo de bureau informado é inválido para esta operação.";
        public const string ProviderError = "Não foi possível obter uma resposta válida do provedor de busca.";

        // Validações de Data
        public static string InvalidDate(string field) => $"O campo {field} deve ser uma data válida.";
        public static string DateMustBeFuture(string field) => $"O campo {field} deve ser uma data futura.";
        public static string DateMustBePast(string field) => $"O campo {field} deve ser uma data passada.";

        // Banco de Dados
        public const string DatabaseConnectionError = "Não foi possível estabelecer conexão com o banco de dados.";
        public const string DatabaseSaveError = "Ocorreu um erro ao tentar salvar os dados no banco.";
        public const string DatabaseQueryError = "Houve uma falha ao processar a consulta no banco de dados.";
        public const string TransactionCommitError = "Não foi possível confirmar a transação de dados.";
        public const string EntityNotFoundMessage = "Registro não encontrado no banco de dados.";

        public static string EntityNotFound(string field, string value) => $"O registro com {field} igual a {value} não foi encontrado.";
        public static string RecordAlreadyExists(string entityName) => $"Já existe um registro de {entityName} cadastrado com esses dados.";
        public static string DuplicateKey(string field) => $"O valor informado para o campo {field} já está em uso.";
        public static string ConcurrencyError(string entityName) => $"O registro de {entityName} foi modificado ou excluído por outro usuário. Tente novamente.";
        public static string ForeignKeyViolation(string entityName) => $"Não é possível processar a operação porque este registro está vinculado a outra informação de {entityName}.";
    }
}
