using System;
using System.Runtime.Serialization;

/*
 * Dados da tabela Coordenadas que o serviço WCF irá passar para a aplicação cliente.
 * Esta tabela contém as coordenadas (posições) pertencentes a uma determinada rota.
 * Campos da tabela no banco de dados:
 *     id: int            -> número interno de identificação da posição (chave)
 *     email_usr: string  -> email do usuário que percorreu a rota
 *     id_rota: int       -> identificação da rota na qual a posição pertence
 *     latitude: double   -> latitude da posição
 *     longitude: double  -> longitude da posição
 *     datahora: datetime -> data e hora na qual posição foi obtida
 */
namespace MostraRotaWS
{
    [DataContract]
    public class CoordenadaDataContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string EmailUsr { get; set; }

        [DataMember]
        public int IdRota { get; set; }

        [DataMember]
        public float Latitute { get; set; }

        [DataMember]
        public float Longitude { get; set; }

        [DataMember]
        public DateTime DataHora { get; set; }
    }
}