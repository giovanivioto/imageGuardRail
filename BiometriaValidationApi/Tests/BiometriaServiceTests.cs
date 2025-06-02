[Fact]
public async Task ValidarFacialAsync_DeveDetectarFraude()
{
    var service = new BiometriaService(...);
    var request = new BiometriaRequest { ImagemBase64 = "..." };
    var resultado = await service.ValidarFacialAsync(request);
    Assert.True(resultado.FraudeDetectada);
}