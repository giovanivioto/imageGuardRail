public async Task<ResultadoValidacao> ValidarDocumentoAsync(DocumentoRequest request)
{
    DocumentoValidator.ValidarRequisicao(request);

    ImageMetadataValidator.ValidarMetadados(request.ImagemDocumentoBase64);
    ImageMetadataValidator.ValidarMetadados(request.ImagemFaceBase64);

    var imagemDocumento = Convert.FromBase64String(request.ImagemDocumentoBase64);
    var imagemFace = Convert.FromBase64String(request.ImagemFaceBase64);

    var suspeito = DetectarFraude(imagemDocumento, imagemFace);

    var resultado = new ResultadoValidacao
    {
        Tipo = "documento",
        Sucesso = !suspeito,
        Data = DateTime.UtcNow,
        UsuarioId = request.UsuarioId
    };

    await _cassandraContext.SalvarResultado(resultado);

    if (suspeito)
        await _notificacaoService.NotificarFraude(resultado);

    return resultado;
}
