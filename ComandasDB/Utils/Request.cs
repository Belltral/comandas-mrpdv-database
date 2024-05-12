
using ComandasDB.Data;
using Newtonsoft.Json;

namespace ComandasDB.Utils
{
    internal class Request
    {
        [JsonProperty]
        internal RequestTypes RequestType { get; set; }
        [JsonProperty]
        internal int ComandaNumber { get; set; }
        [JsonProperty]
        internal bool IsPreVenda { get; set; }
        [JsonProperty]
        internal bool IsItensPreVendas { get; set; }
        [JsonProperty]
        internal int BufferSize { get; set; }
    }
}
