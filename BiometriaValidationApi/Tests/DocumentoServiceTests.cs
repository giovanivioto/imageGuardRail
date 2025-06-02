using Xunit;
using Moq;
using System.Threading.Tasks;
using BiometriaValidacaoAPI.Services;
using BiometriaValidacaoAPI.Models;
using Microsoft.Extensions.Logging;

namespace BiometriaValidacaoAPI.Tests
{
    public class DocumentoServiceTests
    {
        private readonly DocumentoService _service;
        private readonly Mock<INotificacaoService> _notificacaoServiceMock;
        private readonly Mock<ILogger<DocumentoService>> _loggerMock;

        public DocumentoServiceTests()
        {
            _notificacaoServiceMock = new Mock<INotificacaoService>();
            _loggerMock = new Mock<ILogger<DocumentoService>>();

            _service = new DocumentoService(_loggerMock.Object, _notificacaoServiceMock.Object);
        }

        [Fact]
        public async Task ValidarDocumentoAsync_DeveRetornarFraude_QuandoImagemSuspeita()
        {
            // Arrange
            var request = new DocumentoRequest
            {
                ImagemDocumentoBase64 = "base64-foto-de-foto-suspeita",
                ImagemFaceBase64 = "base64-imagem-face"
            };

            // Act
            var resultado = await _service.ValidarDocumentoAsync(request);

            // Assert
            Assert.True(resultado.FraudeDetectada);
            Assert.Equal("Foto de foto detectada no documento", resultado.Mensagem);
            _notificacaoServiceMock.Verify(ns => ns.NotificarFraudeAsync(It.IsAny<ResultadoValidacao>()), Times.Once);
        }

        [Fact]
        public async Task ValidarDocumentoAsync_DeveRetornarSucesso_QuandoDocumentoValido()
        {
            // Arrange
            var request = new DocumentoRequest
            {
                ImagemDocumentoBase64 = "base64-documento-valido",
                ImagemFaceBase64 = "base64-face-valida"
            };

            // Act
            var resultado = await _service.ValidarDocumentoAsync(request);

            // Assert
            Assert.False(resultado.FraudeDetectada);
            Assert.Equal("Documento válido e face reconhecida com sucesso", resultado.Mensagem);
            _notificacaoServiceMock.Verify(ns => ns.NotificarFraudeAsync(It.IsAny<ResultadoValidacao>()), Times.Never);
        }

        [Fact]
        public async Task ValidarDocumentoAsync_DeveRetornarErro_SeImagemDocumentoForNula()
        {
            // Arrange
            var request = new DocumentoRequest
            {
                ImagemDocumentoBase64 = null,
                ImagemFaceBase64 = "base64-face-valida"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.ValidarDocumentoAsync(request));
        }
    }
}
