using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PH3A.Domain.Interfaces;
using PH3A.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace PH3A.Domain.Services
{
    public class JurosService :IJurosService
    {
        private readonly IConfiguration _configuration;

        public JurosService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task IncluirJuros(List<Juros> juros, int idSimulacao)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandTimeout = 0;

                            var count = 0;
                            juros.ForEach(x =>
                            {
                                x.IdSimulacao = idSimulacao;
                                sqlCommand.CommandText = $"INSERT INTO Juros (IdParcela,IdSimulacao,ValorJuros) VALUES (@IdParcela{count},@IdSimulacao{count},@ValorJuros{count});";

                                sqlCommand.Parameters.AddWithValue($"@IdParcela{count}", x.IdParcela);
                                sqlCommand.Parameters.AddWithValue($"@IdSimulacao{count}", x.IdSimulacao);
                                sqlCommand.Parameters.AddWithValue($"@ValorJuros{count}", Math.Round(x.ValorJuros, 2));

                                sqlCommand.ExecuteNonQuery();
                                count++;
                            });

                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                }
                catch (Exception ex)
                {
                }
            });
        }
    }
}
