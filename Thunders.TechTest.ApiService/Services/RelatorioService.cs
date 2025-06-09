using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;

namespace Thunders.TechTest.ApiService.Services
{
    public class RelatorioService
    {
        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> TotalPorHoraPorCidade()
        {
            return await _context.Utilizacoes
                .GroupBy(u => new { u.Cidade, Hora = u.DataHora.Hour })
                .Select(g => new
                {
                    g.Key.Cidade,
                    Hora = g.Key.Hora,
                    Total = g.Sum(x => x.ValorPago)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> PracasQueMaisFaturaramPorMes(int quantidade)
        {
            return await _context.Utilizacoes
                .GroupBy(u => new { u.Praca, Mes = u.DataHora.Month, Ano = u.DataHora.Year })
                .Select(g => new
                {
                    g.Key.Praca,
                    g.Key.Mes,
                    g.Key.Ano,
                    Total = g.Sum(x => x.ValorPago)
                })
                .OrderByDescending(x => x.Total)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> TiposDeVeiculosPorPraca(string praca)
        {
            return await _context.Utilizacoes
                .Where(u => u.Praca == praca)
                .GroupBy(u => u.TipoVeiculo)
                .Select(g => new
                {
                    Tipo = g.Key.ToString(),
                    Quantidade = g.Count()
                })
                .ToListAsync();
        }
    }
}
