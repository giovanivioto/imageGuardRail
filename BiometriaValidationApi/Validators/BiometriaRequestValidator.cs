using BiometriaValidacaoAPI.Models;
using System.Text.RegularExpressions;

namespace BiometriaValidacaoAPI.Validations
{
    public static class BiometriaValidator
    {
        public static void ValidarRequisicao(BiometriaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImagemBase64))
                throw new ArgumentException("Imagem de biometria não pode ser nula ou vazia.");

            if (!EhBase64Valido(request.ImagemBase64))
                throw new ArgumentException("Imagem de biometria não está em formato Base64 válido.");

            if (TamanhoExcedeLimite(request.ImagemBase64))
                throw new ArgumentException("Imagem de biometria excede o tamanho permitido.");
        }

        private static bool EhBase64Valido(string base64)
        {
            base64 = base64.Trim();
            return Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,2}$");
        }

        private static bool TamanhoExcedeLimite(string base64)
        {
            // Tamanho máximo arbitrário de 5MB (ajustável)
            var tamanhoBytes = Convert.FromBase64String(base64).Length;
            return tamanhoBytes > 5 * 1024 * 1024;
        }
    }
}
