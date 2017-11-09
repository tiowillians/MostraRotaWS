using System.Runtime.Serialization;

/*
 * Dados da tabela Usuarios que o serviço WCF irá passar para a aplicação cliente.
 * Campos da tabela no banco de dados:
 *     email: nvarchar(128) -> e-mail do usuário (chave de identificação)
 *     nome: nvarchar(80)   -> nome do usuário
 *     login: int           -> serviço usado para autenticar o usuário (1-Google; 2-Facebook)
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
