using System.Runtime.Serialization;

/*
 * Dados da tabela Coordenadas que o serviço WCF irá passar para a aplicação cliente.
 */
namespace MostraRotaWS
{
    [DataContract]
    public class CoordenadaDataContract
    {
        [DataMember]
        public string EmailUsr { get; set; }

        [DataMember]
        public int NumRota { get; set; }

        [DataMember]
        public int Seq { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string DataHora { get; set; }
    }
}