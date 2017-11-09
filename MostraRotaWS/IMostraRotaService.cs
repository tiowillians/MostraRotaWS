using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MostraRotaWS
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da interface "IMostraRotaService" no arquivo de código e configuração ao mesmo tempo.
    [ServiceContract]
    public interface IMostraRotaService
    {
        // ==========================================
        // Operações com Usuários
        //

        // Obter dados de um determinado usuário
        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "GetUser/{email}")]
        [Description("Retorna os dados do usuário cujo e-mail foi informado")]
        UsuarioDataContract GetUsr(string email);

        // Inserir um novo usuário
        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "User")]
        [Description("Insere um novo usuário")]
        bool InsertUsr(UsuarioDataContract dados);

        // Alterar os dados de um usuário já cadastrado
        [OperationContract]
        [WebInvoke(Method = "PUT",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "User")]
        [Description("Altera os dados de um usuário já cadastrado")]
        bool UpdateUsr(UsuarioDataContract dados);

        // Remover um determinado usuário
        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "DeleteUser/{email}")]
        [Description("Elimina o usuário cujo e-mail foi informado")]
        bool DeleteUsr(string email);

        // ==========================================
        // Operações com Rotas
        //
        // Obter dados das rotas
        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Rotas/{usrEmail}")]
        [Description("Retorna uma lista com resumo das rotas que foram feitas pelo usuário usrEmal")]
        List<RotaResumoDataContract> GetRotas(string usrEmail);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Rota?email={usrEmail}&rota={idRota}")]
        [Description("Retorna dados da rota solicitada.")]
        RotaDataContract GetRota(string usrEmail, int idRota);

        // Inserir uma nova rota
        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Rota")]
        [Description("Insere uma nova rota")]
        bool InsertRota(RotaDataContract dados);

        // Remover uma determinada rota do usuário
        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "DeleteRota?email={usrEmail}&rota={idRota}")]
        [Description("Elimina rota cujo ID foi informado")]
        bool DeleteRota(string usrEmail, int idRota);
    }
}
