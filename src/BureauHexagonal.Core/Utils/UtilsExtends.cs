using System.Text;

namespace BureauHexagonal.Core.Utils
{
    public static class UtilsExtends
    {
        public static string OnlyNumbers(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var sb = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static bool IsValidCpf(this string input)
        {
            string cpf = input.OnlyNumbers();

            if (cpf.Length != 11) return false;

            // Verifica se todos os dígitos são iguais
            bool allSame = true;
            for (int i = 1; i < 11; i++)
                if (cpf[i] != cpf[0]) { allSame = false; break; }

            if (allSame) return false;

            // Lógica de cálculo dos dígitos verificadores
            int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += (tempCpf[i] - '0') * multiplier1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += digit1;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += (tempCpf[i] - '0') * multiplier2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return cpf.EndsWith($"{digit1}{digit2}");
        }

        public static bool IsValidCnpj(this string input)
        {
            string cnpj = input.OnlyNumbers();

            if (cnpj.Length != 14) return false;

            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += (tempCnpj[i] - '0') * multiplier1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += digit1;
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += (tempCnpj[i] - '0') * multiplier2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return cnpj.EndsWith($"{digit1}{digit2}");
        }

        public static bool IsValidCep(this string input)
        {
            string cep = input.OnlyNumbers();
            return cep.Length == 8;
        }
    }
}
