using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * Dados da tabela Rotas que o serviço WCF irá passar para a aplicação cliente.
 */
namespace MostraRotaWS
{
    [DataContract]
    public class RotaDataContract
    {
        [DataMember]
        public string EmailUsuario { get; set; }

        [DataMember]
        public int NumRota { get; set; }

        [DataMember]
        public string DtHrIni { get; set; }

        [DataMember]
        public string DtHrFim { get; set; }

        [DataMember]
        public int Distancia { get; set; }

        [DataMember]
        public List<CoordenadaDataContract> Coordenadas { get; set; }
    }
}