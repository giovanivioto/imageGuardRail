public async Task<ResultadoValidacao> ValidarFacialAsync(BiometriaRequest request)
{
    BiometriaValidator.ValidarRequisicao(request);

    ImageMetadataValidator.ValidarMetadados(request.ImagemBase64);

    var imagem = Convert.FromBase64String(request.ImagemBase64);

    var suspeito = DetectarFraude(imagem);

    var resultado = new ResultadoValidacao
    {
        Tipo = "facial",
        Sucesso = !suspeito,
        Data = DateTime.UtcNow,
        UsuarioId = request.UsuarioId
    };

    await _cassandraContext.SalvarResultado(resultado);

    if (suspeito)
        await _notificacaoService.NotificarFraude(resultado);

    return resultado;
}
