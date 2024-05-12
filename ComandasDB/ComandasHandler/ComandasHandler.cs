using System;
using System.Linq;
using System.Collections.Generic;
using ComandasDB.Data;
using ComandasDB.Context;

namespace ComandasDB
{
    /// <summary>
    /// Classe que compõe os métodos que manipulam as comandas no banco.
    /// </summary>
    internal class ComandasHandler
    {
        /// <summary>
        /// Localiza no banco local da base de comandas uma pre venda/comanda de acordo com um número de comanda fornecido.
        /// </summary>
        /// <param name="comandaNumber">Número da pré venda/comanda a ser localizada.</param>
        /// <returns>
        /// <para>Retorna um objeto PreVenda contendo uma pre venda do banco local da base de comandas.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        internal static PreVenda GetPreVenda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext())
            {
                var selectPreVenda = db.PreVendas.SingleOrDefault(p => p.COMANDA_PRVD == comandaNumber);

                return selectPreVenda;
            }
        }

        /// <summary>
        /// Localiza todas as pré vendas da base de dados de comandas.
        /// </summary>
        /// <returns>
        /// <para>Retorna um objeto List de PreVenda da base de comandas.</para>
        /// Obs.: Pode retornar um objeto vazio.
        /// </returns>
        internal static List<PreVenda> GetPreVendas()
        {
            using (var db = new ComandasDbContext())
            {
                var selectPreVenda = db.PreVendas.Select(s => s).ToList();

                return selectPreVenda;
            }
        }

        /// <summary>
        /// Localiza os itens de uma comanda específica através do número de pré venda da base de comandas.
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser localizada</param>
        /// <returns>Retorna um objeto List de ItensPreVenda.
        /// <para>Obs.: Retorna um objeto List de ItensPreVenda vazio se não forem encontrados resultados.</para>
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<ItensPreVenda> GetItensComanda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext())
            {
                var numeroPreVenda = db.PreVendas.FirstOrDefault(pv => pv.COMANDA_PRVD == comandaNumber);

                var itens = db.ItensPreVendas.Select(s => s).Where(n => n.NUMERO_PRVD == numeroPreVenda.NUMERO_PRVD).ToList();

                return itens;
            }
        }

        /// <summary>
        /// Gera uma comanda do MR PDV com todas as informações da base de comandas (tabelas PreVendas e ItensPreVendas) 
        /// através de um número de comanda fornecido como parâmetro.
        /// </summary>
        /// <param name="comandaNumber">Número da comanda.</param>
        /// <returns>
        ///<para>Retorna um objeto Comanda.</para>
        /// Obs.: Retorna null se a comanda não for encontrada.
        /// </returns>
        internal static Comanda GetComanda(int comandaNumber)
        {
            using (var db = new ComandasDbContext())
            {
                var queryPreVenda = db.PreVendas.FirstOrDefault(p => p.COMANDA_PRVD == comandaNumber);

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
        /// Salva uma nova comanda na base
        /// </summary>
        /// <param name="comanda">Comanda a ser salva na base</param>
        /// <returns>Retorna verdadeiro se a comanda for salva ou falso caso não</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static bool SaveComanda(Comanda comanda)
        {
            if (comanda is null)
            {
                throw new ArgumentException();
            }
            using (var db = new ComandasDbContext())
            {
                int comandaNumber = comanda.PreVenda.COMANDA_PRVD;

                db.PreVendas.Add(comanda.PreVenda);
                db.SaveChanges();

                var generetadPreVenda = db.PreVendas.SingleOrDefault(pv => pv.COMANDA_PRVD == comandaNumber);

                foreach (var item in comanda.ItensPreVenda)
                {
                    item.NUMERO_PRVD = generetadPreVenda.NUMERO_PRVD;
                }

                db.ItensPreVendas.AddRange(comanda.ItensPreVenda);

                db.SaveChanges();

                bool isComandaSaved = ComandaExistis(comandaNumber);

                return isComandaSaved;
            }
        }

        /// <summary>
        /// Faz a atualização dos produtos e valor de uma comanda ou atualiza todos os dados referentes a comanda.
        /// </summary>
        /// <param name="updatedComanda">Comanda com os itens atualizados.</param>
        /// <returns>Retorna verdadeiro se o procedimento foi efetuado ou falso caso não exista a comanda informada.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static bool UpdateComanda(Comanda updatedComanda)
        {
            if (updatedComanda is null)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext())
            {
                int comandaNumber = updatedComanda.PreVenda.COMANDA_PRVD;

                var oldPreVenda = db.PreVendas.SingleOrDefault(p => p.COMANDA_PRVD == comandaNumber);
                int OldPreVendaNumber = oldPreVenda.NUMERO_PRVD;

                var oldItens = db.ItensPreVendas.Select(i => i).Where(i => i.NUMERO_PRVD == OldPreVendaNumber);
                db.ItensPreVendas.RemoveRange(oldItens);

                db.PreVendas.Remove(oldPreVenda);
                db.PreVendas.Add(updatedComanda.PreVenda);
                db.SaveChanges();

                var generatedPreVenda = db.PreVendas.SingleOrDefault(pv => pv.COMANDA_PRVD == comandaNumber);

                foreach (var item in updatedComanda.ItensPreVenda)
                {
                    item.NUMERO_PRVD = generatedPreVenda.NUMERO_PRVD;
                }

                db.ItensPreVendas.AddRange(updatedComanda.ItensPreVenda);
                db.SaveChanges();

                return true;
            }
        }

        /// <summary>
        /// Remove uma comanda (pré venda e itens) do banco.
        /// </summary>
        /// <param name="comandaNumber">Número da comanda que será removida.</param>
        /// <returns>Retorna verdadeiro se o procedimento foi efetuado ou falso caso não exista a comanda informada.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static bool DeleteComanda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext())
            {
                var preVenda = db.PreVendas.SingleOrDefault(p => p.COMANDA_PRVD == comandaNumber);
                var itensComanda = db.ItensPreVendas.Select(i => i).Where(p => p.NUMERO_PRVD == preVenda.NUMERO_PRVD);

                if (preVenda is null)
                {
                    return false;
                }

                db.PreVendas.Remove(preVenda);
                db.ItensPreVendas.RemoveRange(itensComanda);
                db.SaveChanges();

                return true;
            }
        }

        /// <summary>
        /// Apaga todas as informações de comanda da base.
        /// </summary>
        /// <returns>Retorna verdadeiro se as comandas forem apagadas</returns>
        internal static bool DeleteAllComandas()
        {
            using (var db = new ComandasDbContext())
            {
                string deleteItensPreVendas = "DELETE FROM ItensPreVendas;";
                db.Database.ExecuteSqlCommand(deleteItensPreVendas);

                string deletePreVendas = "DELETE FROM PreVendas;";
                db.Database.ExecuteSqlCommand(deletePreVendas);

                db.SaveChanges();

                var checkDelete = db.PreVendas.Select(p => p);

                if (!checkDelete.Any())
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Verifica a existência de uma comanda
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser localizada</param>
        /// <returns>Retorna verdadeiro se a comanda existir ou falso se não existir</returns>
        internal static bool ComandaExistis(int comandaNumber)
        {
            using (var db = new ComandasDbContext())
            {
                var selectComanda = db.PreVendas.SingleOrDefault(p => p.COMANDA_PRVD == comandaNumber);

                if (selectComanda is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}