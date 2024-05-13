using ComandasDB.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComandasDB
{
    /// <summary>
    /// Classe que controla as requisições para a base local
    /// </summary>
    public class ComandasLocal : IComandas
    {
        /// <summary>
        /// Verifica se uma comanda já existe na base local de comandas
        /// </summary>
        /// <param name="comandaNumber">Número da comanda para verificar sua existência</param>
        /// <returns>Retorna verdadeiro caso exista uma comanda com o mesmo número ou falso caso não sejam encontrados dados</returns>
        public Task<bool> ComandaExistis(int comandaNumber)
        {
            return Task.FromResult(ComandasHandler.ComandaExistis(comandaNumber));
        }

        /// <summary>
        /// Localiza uma comanda no banco do servidor de acordo com o número de comanda fornecido
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser retornada</param>
        /// <returns>Retorna uma comanda do banco pelo servidor de comandas</returns>
        public Task<Comanda> GetComanda(int comandaNumber)
        {
            return Task.FromResult(ComandasHandler.GetComanda(comandaNumber));
        }

        /// <summary>
        /// Retorna os produtos de uma comanda específica do banco local de acordo com o número de comanda
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual os produtos fazem parte</param>
        /// <returns></returns>
        public Task<List<ItensPreVenda>> GetItensPreVenda(int comandaNumber)
        {
            return Task.FromResult(ComandasHandler.GetItensComanda(comandaNumber));
        }

        /// <summary>
        /// Retorna uma Pré Venda do banco do servidor de acordo com o número de comanda fornecido
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual pertence a Pré Venda</param>
        /// <returns></returns>
        public Task<PreVenda> GetPreVenda(int comandaNumber)
        {
            return Task.FromResult(ComandasHandler.GetPreVenda(comandaNumber));
        }

        /// <summary>
        /// Salva uma comanda na base do servidor
        /// </summary>
        /// <param name="comanda">Comanda que será salva</param>
        /// <returns>Retorna verdadeiro caso a comanda seja salva ou falso caso não seja</returns>
        public Task<bool> SaveComanda(Comanda comanda)
        {
            return Task.FromResult(ComandasHandler.SaveComanda(comanda));
        }

        /// <summary>
        /// Faz a atualização de uma comanda existente na base local
        /// </summary>
        /// <param name="comanda">Comanda atualizada a ser salva</param>
        /// <returns>Retorna verdadeiro caso a comanda seja atualizada</returns>
        public Task<bool> UpdateComanda(Comanda comanda)
        {
            return Task.FromResult(ComandasHandler.UpdateComanda(comanda));
        }

        /// <summary>
        /// Apaga todas as comandas da base local de comandas
        /// </summary>
        /// <returns>Retorna verdadeiro caso as comandas forem apagadas</returns>
        public Task<bool> DeleteAllComandas()
        {
            return Task.FromResult(ComandasHandler.DeleteAllComandas());
        }

        /// <summary>
        /// Remove uma comanda da base local de acordo com o número
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser removida</param>
        /// <returns>Retorna verdadeiro se o procedimento foi efetuado ou falso caso não exista a comanda informada</returns>
        public Task<bool> DeleteComanda(int comandaNumber)
        {
            return Task.FromResult(ComandasHandler.DeleteComanda(comandaNumber));
        }
    }
}
