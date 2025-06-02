public class CassandraContext
{
    private readonly ISession _session;

    public CassandraContext()
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            .Build();
        _session = cluster.Connect("validador");
    }

    public async Task SalvarResultado(ResultadoValidacao resultado)
    {
        var query = "INSERT INTO resultados (id, usuarioId, tipo, sucesso, data) VALUES (uuid(), ?, ?, ?, ?)";
        await _session.ExecuteAsync(new SimpleStatement(query, resultado.UsuarioId, resultado.Tipo, resultado.Sucesso, resultado.Data));
    }
}
