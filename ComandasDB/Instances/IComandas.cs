
using ComandasDB.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComandasDB
{
    public interface IComandas
    {
        Task<Comanda> GetComanda(int comandaNumber);
        Task<PreVenda> GetPreVenda(int comandaNumber);
        Task<List<ItensPreVenda>> GetItensPreVenda(int comandaNumber);
        Task<bool> ComandaExistis(int comandaNumber);
        Task<bool> SaveComanda(Comanda comanda);
        Task<bool> UpdateComanda(Comanda comanda);
        Task<bool> DeleteComanda(int comandaNumber);
        Task<bool> DeleteAllComandas();
    }
}
