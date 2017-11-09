using System.Runtime.Serialization;

/*
 * Dados da tabela Usuarios que o serviço WCF irá passar para a aplicação cliente.
 */
namespace MostraRotaWS
{
    [DataContract]
    public class UsuarioDataContract
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public int Login { get; set; }
    }
}
