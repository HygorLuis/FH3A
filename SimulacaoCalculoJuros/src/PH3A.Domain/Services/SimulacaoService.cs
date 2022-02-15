using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using PH3A.Domain.Interfaces;
using PH3A.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace PH3A.Domain.Services
{
    public class SimulacaoService : ISimulacaoService
    {
        private readonly IConfiguration _configuration;

        public SimulacaoService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<int> IncluirSimulacao(Simulacao simulacao)
        {
            return await Task.Run(() =>
            {
                try
                {
                    simulacao.DataInclusao = DateTime.Now;

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandTimeout = 0;
                            sqlCommand.CommandText = "INSERT INTO Simulacao (PorcentagemJuros,TipoCalculo,TotalJuros,TotalDivida,DataInclusao) VALUES (@PorcentagemJuros,@TipoCalculo,@TotalJuros,@TotalDivida,@DataInclusao);SELECT SCOPE_IDENTITY();";

                            sqlCommand.Parameters.AddWithValue("@PorcentagemJuros", simulacao.PorcentagemJuros);
                            sqlCommand.Parameters.AddWithValue("@TipoCalculo", simulacao.TipoCalculo.GetHashCode());
                            sqlCommand.Parameters.AddWithValue("@TotalJuros", Math.Round(simulacao.TotalJuros, 2));
                            sqlCommand.Parameters.AddWithValue("@TotalDivida", Math.Round(simulacao.TotalDivida, 2));
                            sqlCommand.Parameters.AddWithValue("@DataInclusao", simulacao.DataInclusao);

                            simulacao.Id = int.Parse(sqlCommand.ExecuteScalar().ToString());

                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                    return simulacao.Id;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            });
        }

        public async Task<List<Simulacao>> BuscarSimulacoes(int qtd)
        {
            try
            {
                var simulacoes = new List<Simulacao>();
                var client = new HttpClient();
                var response = await client.GetAsync($"https://localhost:44375/Simulacoes/buscarSimulacoes/{qtd}");

                if (response.IsSuccessStatusCode)
                    simulacoes = await response.Content.ReadAsAsync<List<Simulacao>>();

                return simulacoes ?? new List<Simulacao>();
            }
            catch (Exception e)
            {
                return new List<Simulacao>();
            }
        }
    }
}
