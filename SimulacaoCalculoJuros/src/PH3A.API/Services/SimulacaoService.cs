using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PH3A.Domain.Enums;
using PH3A.API.Interfaces;
using PH3A.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace PH3A.API.Services
{
    public class SimulacaoService : ISimulacaoService
    {
        private readonly IConfiguration _configuration;

        public SimulacaoService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<List<Simulacao>> BuscarSimulacoes(int qtd)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dtSimulacao = new DataTable();

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            var condicao = qtd > 0 ? $"TOP {qtd}" : "";
                            sqlCommand.CommandText = $"SELECT {condicao} * FROM Simulacao ORDER BY DataInclusao DESC;";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(dtSimulacao);
                            }
                        }
                        connection.Close();
                    }

                    return new List<Simulacao>(dtSimulacao.AsEnumerable().ToList().Select(x => new Simulacao
                    {
                        Id = x.Field<int>("Id"),
                        PorcentagemJuros = x.Field<decimal>("PorcentagemJuros"),
                        TipoCalculo = (TipoJuros)x.Field<int>("TipoCalculo"),
                        TotalJuros = x.Field<decimal>("TotalJuros"),
                        TotalDivida = x.Field<decimal>("TotalDivida"),
                        DataInclusao = x.Field<DateTime>("DataInclusao"),
                    }));
                }
                catch (Exception ex)
                {
                    return new List<Simulacao>();
                }
            });
        }
    }
}
