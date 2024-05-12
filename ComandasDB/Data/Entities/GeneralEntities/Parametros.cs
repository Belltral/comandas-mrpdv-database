using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandasDB
{
    public partial class Parametros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PAR { get; set; }

        public int? ID_TPPR { get; set; }

        [StringLength(200)]
        public string VALOR_PAR { get; set; }

        public string TEXTO_PAR { get; set; }

        public int? COD_LOJA { get; set; }

        public override string ToString()
        {
            return $"ID Par�metro: {ID_PAR}\n" +
                $"ID Tipo Par�metro: {ID_TPPR}\n" +
                $"Valor: {VALOR_PAR}\n" +
                $"Texto: {TEXTO_PAR}\n" +
                $"C�digo Loja: {COD_LOJA}\n";
        }
    }
}
