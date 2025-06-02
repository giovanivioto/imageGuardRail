[Authorize]
[ApiController]
[Route("api/biometria")]
public class BiometriaController : ControllerBase
{
    private readonly BiometriaService _biometriaService;

    public BiometriaController(BiometriaService service) => _biometriaService = service;

    [HttpPost("facial")]
    public async Task<IActionResult> ValidarFacial([FromBody] BiometriaRequest request)
    {
        var resultado = await _biometriaService.ProcessarFacialAsync(request);
        return Ok(resultado);
    }

    [HttpPost("digital")]
    public async Task<IActionResult> ValidarDigital([FromBody] BiometriaRequest request)
    {
        var resultado = await _biometriaService.ProcessarDigitalAsync(request);
        return Ok(resultado);
    }

    [HttpPost("documento")]
    public async Task<IActionResult> ValidarDocumento([FromBody] BiometriaRequest request)
    {
        var resultado = await _biometriaService.ProcessarDocumentoAsync(request);
        return Ok(resultado);
    }
}
