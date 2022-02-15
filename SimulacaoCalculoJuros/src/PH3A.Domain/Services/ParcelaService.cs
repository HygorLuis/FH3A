using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PH3A.Domain.Interfaces;
using PH3A.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace PH3A.Domain.Services
{
    public class ParcelaService : IParcelaService
    {
        private readonly IConfiguration _configuration;

        public ParcelaService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<List<Parcela>> BuscarParcelas(int id = 0)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dtParcelas = new DataTable();

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            var condicao = id > 0 ? $"WHERE Id = {id}" : "";
                            sqlCommand.CommandText = $"SELECT * FROM Parcela {condicao};";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(dtParcelas);
                            }
                        }
                        connection.Close();
                    }

                    return new List<Parcela>(dtParcelas.AsEnumerable().ToList().Select(x => new Parcela
                    {
                        Id = x.Field<int>("Id"),
                        NumParc = x.Field<int>("NumParc"),
                        Valor = x.Field<decimal>("Valor"),
                        DataVencimento = x.Field<DateTime>("DataVencimento"),
                        DataInclusao = x.Field<DateTime>("DataInclusao")
                    }));
                }
                catch (Exception ex)
                {
                    return new List<Parcela>();
                }
            });
        }

        public async Task<Parcela> IncluirParcela(Parcela parcela)
        {
            return await Task.Run(() =>
            {
                try
                {
                    parcela.DataInclusao = DateTime.Now;

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandTimeout = 0;
                            sqlCommand.CommandText = "INSERT INTO Parcela (NumParc,Valor,DataVencimento,DataInclusao) VALUES (@NumParc,@Valor,@DataVencimento,@DataInclusao);SELECT SCOPE_IDENTITY();";

                            sqlCommand.Parameters.AddWithValue("@NumParc", parcela.NumParc);
                            sqlCommand.Parameters.AddWithValue("@Valor", parcela.Valor);
                            sqlCommand.Parameters.AddWithValue("@DataVencimento", parcela.DataVencimento);
                            sqlCommand.Parameters.AddWithValue("@DataInclusao", parcela.DataInclusao);

                            parcela.Id = int.Parse(sqlCommand.ExecuteScalar().ToString());

                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                    return parcela;
                }
                catch (Exception ex)
                {
                    return new Parcela();
                }
            });
        }

        public async Task<Parcela> AlterarParcela(Parcela parcela)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandText = $"UPDATE Parcela SET NumParc = @NumParc, Valor = @Valor, DataVencimento = @DataVencimento WHERE Id = {parcela.Id};";

                            sqlCommand.Parameters.AddWithValue("@NumParc", parcela.NumParc);
                            sqlCommand.Parameters.AddWithValue("@Valor", parcela.Valor);
                            sqlCommand.Parameters.AddWithValue("@DataVencimento", parcela.DataVencimento);
                            sqlCommand.ExecuteNonQuery();

                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                    return parcela;
                }
                catch (Exception ex)
                {
                    return new Parcela();
                }
            });
        }

        public async Task ExcluirParcela(int id)
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
                            sqlCommand.CommandText = $"DELETE FROM Parcela WHERE Id = {id};";

                            sqlCommand.ExecuteNonQuery();
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
