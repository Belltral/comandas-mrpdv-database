using System;
using System.Linq;
using System.Collections.Generic;
using ComandasDB.Data;

namespace ComandasDB
{
    public class Comandas
    {
        /// <summary>
        /// Localiza no banco local da base de comandas uma pre venda de acordo com um número de pre venda fornecido.
        /// </summary>
        /// <param name="numeroPreVenda">Número da pré venda a ser localizada.</param>
        /// <returns>
        /// <para>Retorna um objeto PreVenda contendo uma pre venda do banco local da base de comandas.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static PreVenda GetPreVenda(int numeroPreVenda)
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
        /// Localiza todas as pré vendas da base de dados de comandas.
        /// </summary>
        /// <returns>
        /// <para>Retorna um objeto IEnumerable de PreVenda da base de comandas.</para>
        /// Obs.: Pode retornar um objeto vazio.
        /// </returns>
        public static IEnumerable<PreVenda> GetPreVendas()
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
        /// <param name="preVenda">Objeto que contém a pré venda a ser inserida.</param>
        /// /// <exception cref="ArgumentException"></exception>
        public static void InsertPreVenda(PreVenda preVenda)
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
        /// <param name="numeroPreVenda">Número da pré venda a ser localizada</param>
        /// <returns>Retorna um objeto IEnumerable de ItensPreVendas.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<ItensPreVenda> GetItensFromPreVenda(int numeroPreVenda)
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
        /// Recebe um objeto List de ItensPreVenda para inserir os itens na base de comandas.
        /// </summary>
        /// <param name="itensPreVenda"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void InsertItensOfPreVenda(List<ItensPreVenda> itensPreVenda)
        {
            if (itensPreVenda.Count() == 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                db.ItensPreVendas.AddRange(itensPreVenda);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gera uma comanda do MR PDV com todas as informações da base de comandas (tabelas PreVendas e ItensPreVendas) 
        /// através de um número de comanda fornecido como parâmetro.
        /// </summary>
        /// <param name="numeroComanda">Número da comanda.</param>
        /// <returns>
        ///<para>Retorna um objeto Comanda.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        public static Comanda GetComanda(int numeroComanda)
        {
            using (var db = new ComandasMRPDVContext())
            {
                var queryPreVenda = db.PreVendas.FirstOrDefault(p => p.COMANDA_PRVD == numeroComanda);

                if (queryPreVenda == null)
                {
                    return null;
                }

                var numeroPreVenda = queryPreVenda.NUMERO_PRVD;
                var queryItens = db.ItensPreVendas.Select(i => i).Where(i => i.NUMERO_PRVD == numeroPreVenda).ToList();

                return new Comanda(queryPreVenda, queryItens);
            }
        }

        /// <summary>
        /// Faz a atualização dos produtos e valor de uma comanda ou atualiza todos os dados referentes a comanda.
        /// </summary>
        /// <param name="numeroPreVenda">Número da pré venda que será atualizada</param>
        /// <param name="itensAtualizados">Itens atualizados da comanda. Obrigatórios mesmo que eles não sejam alterados.</param>
        /// <param name="preVendaAtualizada">Pré venda atualizada caso seja necessário.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void UpdateComanda(int numeroPreVenda, List<ItensPreVenda> itensAtualizados, 
            PreVenda preVendaAtualizada = null)
        {
            if (numeroPreVenda <= 0 || itensAtualizados.Count <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                var oldItens = db.ItensPreVendas.Select(i => i).Where(i => i.NUMERO_PRVD == numeroPreVenda);
                db.ItensPreVendas.RemoveRange(oldItens);

                if (!(preVendaAtualizada is null))
                {
                    var oldPreVenda = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);

                    db.PreVendas.Remove(oldPreVenda);

                    db.PreVendas.Add(preVendaAtualizada);

                    db.SaveChanges();

                    var numeroComanda = preVendaAtualizada.COMANDA_PRVD;

                    var newPreVenda = db.PreVendas.SingleOrDefault(p => p.COMANDA_PRVD == numeroComanda);

                    foreach (var item in itensAtualizados)
                    {
                        item.NUMERO_PRVD = newPreVenda.NUMERO_PRVD;
                    }

                    db.ItensPreVendas.AddRange(itensAtualizados);

                    db.SaveChanges();

                    return;
                }

                db.ItensPreVendas.AddRange(itensAtualizados);

                decimal? novoValor = 0;
                foreach (var valor in itensAtualizados)
                {
                    novoValor += (valor.PRECO_IPRV * valor.QTDE_IPRV);
                }

                var preVenda = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);
                preVenda.VALOR_PRVD = novoValor;

                db.SaveChanges();
            }
        }

        //TODO: Exclusão de comanda
        public static void DeleteComanda()
        {

        }
    }
}