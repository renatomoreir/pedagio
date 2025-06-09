using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Thunders.TechTest.ApiService.Models
{
    public enum TipoVeiculo
    {
        /// <summary>Moto.</summary>
        Moto = 0,
        /// <summary>Carro.</summary>
        Carro = 1,
        /// <summary>Caminhao.</summary>
        Caminhao = 2,
        /// <summary>CarroPasseio.</summary>
        CarroPasseio = 3,
        /// <summary>Onibus.</summary>
        Onibus = 4,
        /// <summary>Van.</summary>
        Van = 5,
        /// <summary>Outro.</summary>
        Outro = 6
    }
    public class Utilizacao
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Praca { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Estado { get; set; } = null!;
        [Precision(18, 2)]
        public decimal ValorPago { get; set; } = 0;
        public TipoVeiculo TipoVeiculo { get; set; }
    }
}
