using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandasDB
{
    public partial class Pdvs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PDV { get; set; }

        public int? ID_ECF { get; set; }

        public int? ID_BAL { get; set; }

        public int? COD_LOJA { get; set; }

        public int? NUMERO_PDV { get; set; }

        [StringLength(50)]
        public string NOME_LOJA { get; set; }

        [StringLength(50)]
        public string DESCRICAO_PDV { get; set; }

        public int? COD_LISTA { get; set; }

        public int? COD_GRPD { get; set; }

        public int? ID_UF { get; set; }

        [StringLength(32)]
        public string IDEQUIP_PDV { get; set; }

        [StringLength(1)]
        public string COD_STPD { get; set; }

        [StringLength(60)]
        public string DESCRICAO_STPD { get; set; }

        [StringLength(1)]
        public string COMPORTAMENTO_STPD { get; set; }

        [StringLength(1)]
        public string COD_STLJ { get; set; }

        [StringLength(60)]
        public string DESCRICAO_STLJ { get; set; }

        [StringLength(1)]
        public string COMPORTAMENTO_STLJ { get; set; }

        [StringLength(60)]
        public string NOME_PESSOA { get; set; }

        [StringLength(50)]
        public string FANTASIA_PESSOA { get; set; }

        [StringLength(200)]
        public string LOGR_END { get; set; }

        [StringLength(50)]
        public string NUMERO_END { get; set; }

        [StringLength(50)]
        public string COMPL_END { get; set; }

        [StringLength(8)]
        public string CEP_END { get; set; }

        [StringLength(50)]
        public string NOME_BAIRRO { get; set; }

        [StringLength(50)]
        public string NOME_CID { get; set; }

        [StringLength(2)]
        public string SIGLA_UF { get; set; }

        public int? CODIBGE_CID { get; set; }

        public int? CODIBGE_UF { get; set; }

        [StringLength(40)]
        public string CNPJ_PDV { get; set; }

        [StringLength(40)]
        public string IE_PDV { get; set; }

        [StringLength(40)]
        public string IM_PDV { get; set; }

        [StringLength(20)]
        public string NUMERO_FONE { get; set; }

        public string CPOAUT_PDV { get; set; }

        public DateTime? ULTACC_PDV { get; set; }

        public DateTime? ULTACCAPI_PDV { get; set; }
    }
}
