using System;
using System.Runtime.Serialization;

/*
 * Dados resumidos da tabela Rotas que o serviço WCF irá passar para a aplicação cliente.
 */
namespace MostraRotaWS
{
    [DataContract]
    public class RotaResumoDataContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime DtHrIni { get; set; }
    }
}