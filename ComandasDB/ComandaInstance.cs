using ComandasDB.Data.Internal;

namespace ComandasDB
{
    // TODO: Documentar
    public class ComandaInstance
    {
        public static IComandas GetInstance()
        {
            var serverInfo = RetrieveFromMRPDV.GetServerInfo();
            var PdvInfo = RetrieveFromMRPDV.PdvInfo();

            if (serverInfo == null || int.Parse(serverInfo.VALOR_PAR) == PdvInfo.NUMERO_PDV)
            {
                return new ComandasLocal();
            }
            else
            {
                return new ComandasServer(serverInfo.TEXTO_PAR);
            }
        }
    }
}