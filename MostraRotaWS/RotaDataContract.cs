using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * Dados da tabela Rotas que o serviço WCF irá passar para a aplicação cliente.
 * Campos da tabela no banco de dados:
 *     email_usr: string  -> email do usuário que percorreu a rota (chave)
 *     id: int            -> número interno de identificação da rota (chave)
 *     dthr_ini: datetime -> data e horário de início da rota
 *     dthr_fim: datetime -> data e horário de término da rota
 *     distancia: int     -> distância, em metros, percorrida na rota
 */
namespace MostraRotaWS
{
    [DataContract]
    public class RotaDataContract
    {
        [DataMember]
        public string EmailUsuario { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime DtHrIni { get; set; }

        [DataMember]
        public DateTime DtHrFim { get; set; }

        [DataMember]
        public int Distancia { get; set; }

        [DataMember]
        public List<CoordenadaDataContract> Coordenadas { get; set; }
    }
}