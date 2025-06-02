namespace BiometriaValidacaoApi.Models
{
    public class NotificacaoFraudeRequest
    {
        public Guid TransacaoId { get; set; }
        public string TipoBiometria { get; set; }
        public string TipoFraude { get; set; }
        public DateTime DataCaptura { get; set; }
        public DispositivoInfo Dispositivo { get; set; }
        public List<string> CanalNotificacao { get; set; }
        public string NotificadoPor { get; set; }
        public Metadados Metadados { get; set; }
    }

    public class DispositivoInfo
    {
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public string SistemaOperacional { get; set; }
    }

    public class Metadados
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string IpOrigem { get; set; }
    }
}
