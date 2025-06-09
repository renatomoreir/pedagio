// Services/UtilizacaoService.cs
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Models;
using Microsoft.Extensions.Caching.Distributed; // Adicione este using
using System.Text.Json; // Para serialização/deserialização (preferencialmente)
// using Newtonsoft.Json; // Ou este, se optar por Newtonsoft.Json

namespace Thunders.TechTest.ApiService.Services
{
    public class UtilizacaoService
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache; // Injete a interface de cache distribuído

        // Se estiver usando Rebus, mantenha a injeção do IBus
        // private readonly IBus _bus;

        public UtilizacaoService(AppDbContext context, IDistributedCache cache /*, IBus bus */)
        {
            _context = context;
            _cache = cache;
            // _bus = bus; // Se estiver usando Rebus
        }

        // --- READ Operations ---
        public async Task<IEnumerable<Utilizacao>> GetAllUtilizacoesAsync()
        {
            const string cacheKey = "AllUtilizacoes"; // Chave única para o cache

            // Tentar obter do cache
            var cachedUtilizacoesString = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedUtilizacoesString))
            {
                // Se estiver no cache, deserializa e retorna
                return JsonSerializer.Deserialize<List<Utilizacao>>(cachedUtilizacoesString)!;
                // Ou: return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Utilizacao>>(cachedUtilizacoesString)!;
            }

            // Se não estiver no cache, busca do banco de dados
            var utilizacoes = await _context.Utilizacoes.ToListAsync();

            // Configura opções de cache (opcional, mas recomendado)
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Expira em 5 minutos
                // SlidingExpiration = TimeSpan.FromMinutes(2) // Expira se não for acessado por 2 minutos
            };

            // Armazena no cache
            var utilizacoesString = JsonSerializer.Serialize(utilizacoes);
            // Ou: var utilizacoesString = Newtonsoft.Json.JsonConvert.SerializeObject(utilizacoes);
            await _cache.SetStringAsync(cacheKey, utilizacoesString, cacheOptions);

            return utilizacoes;
        }

        public async Task<Utilizacao?> GetUtilizacaoByIdAsync(int id)
        {
            var cacheKey = $"Utilizacao_{id}"; // Chave única para esta utilização

            // Tentar obter do cache
            var cachedUtilizacaoString = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedUtilizacaoString))
            {
                return JsonSerializer.Deserialize<Utilizacao>(cachedUtilizacaoString);
                // Ou: return Newtonsoft.Json.JsonConvert.DeserializeObject<Utilizacao>(cachedUtilizacaoString);
            }

            // Se não estiver no cache, busca do banco de dados
            var utilizacao = await _context.Utilizacoes.FindAsync(id);

            if (utilizacao != null)
            {
                // Armazena no cache
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                var utilizacaoString = JsonSerializer.Serialize(utilizacao);
                // Ou: var utilizacaoString = Newtonsoft.Json.JsonConvert.SerializeObject(utilizacao);
                await _cache.SetStringAsync(cacheKey, utilizacaoString, cacheOptions);
            }

            return utilizacao;
        }

        // --- CREATE Operation ---
        public async Task<Utilizacao> AddUtilizacaoAsync(Utilizacao utilizacao)
        {
            _context.Utilizacoes.Add(utilizacao);
            await _context.SaveChangesAsync();

            // --- Invalidar/Atualizar Cache ---
            await InvalidateCacheForUtilizacao(utilizacao.Id);
            // Se estiver usando Rebus, a lógica de publicação vai aqui:
            // await _bus.Publish(new UtilizacaoCriadaEvent(...));

            return utilizacao;
        }

        // --- UPDATE Operation ---
        public async Task<bool> UpdateUtilizacaoAsync(int id, Utilizacao updatedUtilizacao)
        {
            if (id != updatedUtilizacao.Id)
            {
                return false;
            }

            _context.Entry(updatedUtilizacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                // --- Invalidar/Atualizar Cache ---
                await InvalidateCacheForUtilizacao(id);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Utilizacoes.AnyAsync(e => e.Id == id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        // --- DELETE Operation ---
        public async Task<bool> DeleteUtilizacaoAsync(int id)
        {
            var utilizacao = await _context.Utilizacoes.FindAsync(id);
            if (utilizacao == null)
            {
                return false;
            }

            _context.Utilizacoes.Remove(utilizacao);
            await _context.SaveChangesAsync();

            // --- Invalidar/Atualizar Cache ---
            await InvalidateCacheForUtilizacao(id);
            return true;
        }

        // Método auxiliar para invalidar/remover do cache
        private async Task InvalidateCacheForUtilizacao(int id)
        {
            // Remover do cache da utilização específica
            await _cache.RemoveAsync($"Utilizacao_{id}");
            // Remover do cache da lista completa (para que a próxima requisição busque do DB)
            await _cache.RemoveAsync("AllUtilizacoes");
        }
    }
}