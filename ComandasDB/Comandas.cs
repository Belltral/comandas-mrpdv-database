using System;
using System.Linq;
using System.Collections.Generic;
using ComandasDB.Data;

namespace ComandasDB
{
    public class Comandas
    {
        //internal DataMRPDVContext _dbConnection { get; set; }

        /// <summary>
        /// Localiza no banco local da base de comandas uma pre venda de acordo com um número de pre venda fornecido.
        /// </summary>
        /// <param name="numeroPreVenda"></param>
        /// <returns>
        /// <para>Retorna um objeto PreVendas contendo uma pre venda do banco local da base de comandas.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static PreVendas GetPreVenda(int numeroPreVenda)
        {
            if (numeroPreVenda <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                var selectPreVenda = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);

                return selectPreVenda;
            }
        }

        /// <summary>
        /// Localiza todas as comandas (pré vendas) da base de dados de comandas.
        /// </summary>
        /// <returns>
        /// <para>Retorna um objeto IEnumerable de PreVendas da base de comandas.</para>
        /// Obs.: Pode retornar um objeto vazio.
        /// </returns>
        public static IEnumerable<PreVendas> GetPreVendas()
        {
            using (var db = new ComandasMRPDVContext())
            {
                var selectPreVenda = db.PreVendas.Select(s => s).ToList();

                return selectPreVenda;
            }
        }

        /// <summary>
        /// Recebe um objeto PreVendas e realiza a inserção da pré venda na base de comandas.
        /// </summary>
        /// <param name="preVenda"></param>
        /// /// <exception cref="ArgumentException"></exception>
        public static void InsertPreVenda(PreVendas preVenda)
        {
            if (preVenda.COMANDA_PRVD <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                db.PreVendas.Add(preVenda);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Localiza os itens de uma pré venda específica através do número de pré venda da base de comandas.
        /// </summary>
        /// <param name="numeroPreVenda"></param>
        /// <returns>Retorna um objeto IEnumerable de ItensPreVendas.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<ItensPreVendas> GetItensFromPreVenda(int numeroPreVenda)
        {
            if (numeroPreVenda <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                var itens = db.ItensPreVendas.Select(s => s).Where(n => n.NUMERO_PRVD == numeroPreVenda).ToList();

                return itens;
            }
        }

        /// <summary>
        /// Recebe um objeto List de ItensPreVendas para inserir os itens na base de comandas.
        /// </summary>
        /// <param name="itensPreVenda"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void InsertItensOfPreVenda(List<ItensPreVendas> itensPreVenda)
        {
            if (itensPreVenda.Count() == 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                itensPreVenda.Select(i => db.ItensPreVendas.Add(i));

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gera uma comanda do MR PDV com todas as informações da base de comandas (tabelas PreVendas e ItensPreVendas) 
        /// através de um número de comanda fornecido como parâmetro.
        /// </summary>
        /// <param name="numeroComanda"></param>
        /// <returns>
        ///<para>Retorna um objeto Comanda.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static Comanda GetComanda(int numeroComanda)
        {
            if (numeroComanda <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                var queryPreVenda = db.PreVendas.FirstOrDefault(p => p.COMANDA_PRVD == numeroComanda);

                var queryItens = db.PreVendas
                                .Join(
                                    db.ItensPreVendas,
                                    p => p.NUMERO_PRVD,
                                    i => i.NUMERO_PRVD,
                                    (p, i) => new {PreVendas = p, ItensPreVendas = i}
                                )
                                .Where(p => p.PreVendas.COMANDA_PRVD == numeroComanda);

                if (queryPreVenda != null && queryItens.Count() > 0)
                {
                    ICollection<ItensPreVendas> itens = new List<ItensPreVendas>();

                    foreach (var item in queryItens)
                    {
                        itens.Add(item.ItensPreVendas);
                    }

                    return new Comanda(queryPreVenda, itens);
                }
            }

            return null;
        }
    }
}