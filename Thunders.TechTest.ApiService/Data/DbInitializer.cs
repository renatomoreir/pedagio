using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            const int maxTentativas = 10;
            var tentativa = 0;

            while (true)
            {
                try
                {
                    context.Database.Migrate();
                    break;
                }
                catch (SqlException ex) when (tentativa < maxTentativas)
                {
                    tentativa++;
                    Console.WriteLine($"[Seed] Tentativa {tentativa} falhou: {ex.Message}");
                    Thread.Sleep(3000); // espera 3 segundos e tenta de novo
                }
            }

            if (context.Utilizacoes != null && context.Utilizacoes.Any()) return;

            var random = new Random();
            var cidades = new[] { "São Paulo", "Campinas", "Rio de Janeiro", "Curitiba", "Salvador" };
            var estados = new[] { "SP", "RJ", "PR", "BA" };
            var pracas = new[] { "Pedágio 1", "Pedágio 2", "Pedágio 3", "Pedágio 4" };
            var tipos = Enum.GetValues<TipoVeiculo>();

            var utilizacoes = new List<Utilizacao>();

            for (int i = 0; i < 500; i++)
            {
                utilizacoes.Add(new Utilizacao
                {
                    DataHora = DateTime.UtcNow.AddHours(-random.Next(0, 720)),
                    Praca = pracas[random.Next(pracas.Length)],
                    Cidade = cidades[random.Next(cidades.Length)],
                    Estado = estados[random.Next(estados.Length)],
                    ValorPago = Math.Round((decimal)(random.NextDouble() * 20 + 5), 2),
                    TipoVeiculo = tipos[random.Next(tipos.Length)]
                });
            }

            context.Utilizacoes.AddRange(utilizacoes);
            context.SaveChanges();
        }
    }
}
