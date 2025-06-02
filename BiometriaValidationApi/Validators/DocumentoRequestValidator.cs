using BiometriaValidacaoAPI.Models;
using System.Text.RegularExpressions;

namespace BiometriaValidacaoAPI.Validations
{
    public static class DocumentoValidator
    {
        public static void ValidarRequisicao(DocumentoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImagemDocumentoBase64))
                throw new ArgumentException("Imagem do documento não pode ser nula ou vazia.");

            if (!EhBase64Valido(request.ImagemDocumentoBase64))
                throw new ArgumentException("Imagem do documento não está em formato Base64 válido.");

            if (string.IsNullOrWhiteSpace(request.ImagemFaceBase64))
                throw new ArgumentException("Imagem da face não pode ser nula ou vazia.");

            if (!EhBase64Valido(request.ImagemFaceBase64))
                throw new ArgumentException("Imagem da face não está em formato Base64 válido.");
        }

        private static bool EhBase64Valido(string base64)
        {
            base64 = base64.Trim();
            return Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,2}$");
        }
    }
}
