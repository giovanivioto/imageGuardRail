using BiometriaValidacaoApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiometriaValidacaoApi.Services
{
    public interface INotificacaoService
    {
        Task NotificarFraudeAsync(ResultadoValidacao resultado);
    }

    public class NotificacaoService : INotificacaoService
    {
        private readonly HttpClient _httpClient;

        public NotificacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task NotificarFraudeAsync(ResultadoValidacao resultado)
        {
            var notificacao = new NotificacaoFraudeRequest
            {
                TransacaoId = Guid.NewGuid(), 
                TipoBiometria = resultado.Tipo,
                TipoFraude = "deepfake",  
                DataCaptura = resultado.Data,
                Dispositivo = new DispositivoInfo
                {
                    Fabricante = "Samsung",  
                    Modelo = "Galaxy S22", 
                    SistemaOperacional = "Android 13"  
                },
                CanalNotificacao = new List<string> { "sms", "email" }, 
                NotificadoPor = "sistema-de-monitoramento",  
                Metadados = new Metadados
                {
                    Latitude = -23.55052,  
                    Longitude = -46.633308,  
                    IpOrigem = "192.168.1.10"  
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(notificacao), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.quod.com/api/notificacao/fraude", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Falha ao enviar a notificação de fraude.");
            }
        }
    }
}
