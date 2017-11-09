using MostraRotaWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MostraRotaWS
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "MostraRotaService" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione MostraRotaService.svc ou MostraRotaService.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class MostraRotaService : IMostraRotaService
    {
        public UsuarioDataContract GetUsr(string email)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    usuarios usr = database.usuarios.Find(email);
                    if (usr == null)
                        return null;

                    UsuarioDataContract novo = new UsuarioDataContract
                    {
                        Email = usr.email,
                        Nome = usr.nome,
                        Login = usr.login
                    };

                    return novo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUsr Exception: " + e.Message);
                return null;
            }
        }

        public bool InsertUsr(UsuarioDataContract dados)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    usuarios novo = new usuarios
                    {
                        email = dados.Email,
                        nome = dados.Nome,
                        login = dados.Login
                    };

                    // adiciona usuário no DBSet
                    database.usuarios.Add(novo);

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertUsr Exception: " + e.Message);
                return false;
            }
        }

        public bool UpdateUsr(UsuarioDataContract dados)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    usuarios original = database.usuarios.Find(dados.Email);

                    if (original != null)
                    {
                        usuarios novo = new usuarios
                        {
                            email = dados.Email,
                            nome = dados.Nome,
                            login = dados.Login
                        };

                        // atualiza dados no DBSet
                        database.Entry(original).CurrentValues.SetValues(novo);

                        // salva DBSet na base de dados
                        database.SaveChanges();

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateUsr Exception: " + e.Message);
                return false;
            }
        }

        public bool DeleteUsr(string email)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    usuarios usr = database.usuarios.Find(email);
                    if (usr == null)
                        return false;

                    // elimina todas as rotas do usuário
                    if (EliminaRotasUsuario(email) == false)
                        return false;

                    // remove usuario do DBSet
                    database.usuarios.Remove(usr);

                    // atualiza banco de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteUsr Exception: " + e.Message);
                return false;
            }
        }

        public RotaDataContract GetRota(string usrEmail, int idRota)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    rotas rot = database.rotas.Find(usrEmail, idRota);
                    if (rot == null)
                        return null;

                    RotaDataContract novo = new RotaDataContract
                    {
                        EmailUsuario = rot.email_usr,
                        Id = rot.id,
                        DtHrIni = rot.dthr_ini,
                        DtHrFim = rot.dthr_fim,
                        Distancia = rot.distancia,
                        Coordenadas = GetCoordenadas(usrEmail, idRota)
                    };
                    ;

                    return novo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetRota Exception: " + e.Message);
                return null;
            }
        }

        public List<RotaResumoDataContract> GetRotas(string usrEmail)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    // retorna os registros da tabela rotas
                    var reg = from l in database.rotas
                              where l.email_usr == usrEmail
                              select l;

                    if (reg == null)
                        return null;

                    // estrutura contendo todos os registros retornados pela consulta
                    List<RotaResumoDataContract> rotasList = new List<RotaResumoDataContract>();

                    // percorre todos os registros retornados na consulta anterior
                    RotaResumoDataContract novaRota;
                    foreach (rotas r in reg)
                    {
                        novaRota = new RotaResumoDataContract
                        {
                            Id = r.id,
                            DtHrIni = r.dthr_ini,
                        };

                        rotasList.Add(novaRota);
                    }

                    // retorna lista dos registros
                    return rotasList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetRotas Exception: " + e.Message);
                return null;
            }
        }

        public bool InsertRota(RotaDataContract dados)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    rotas novaRota = new rotas
                    {
                        email_usr = dados.EmailUsuario,
                        id = dados.Id,
                        dthr_ini = dados.DtHrIni,
                        dthr_fim = dados.DtHrFim,
                        distancia = dados.Distancia
                    };

                    // adiciona rota no DBSet
                    database.rotas.Add(novaRota);

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    // obtem coordenadas da rota
                    coordenadas novaCoord;
                    foreach(CoordenadaDataContract c in dados.Coordenadas)
                    {
                        novaCoord = new coordenadas
                        {
                            id = 0,
                            email_usr = c.EmailUsr,
                            id_rota = c.IdRota,
                            datahora = c.DataHora,
                            latitude = c.Latitute,
                            longitude = c.Longitude
                        };

                        // adiciona coordenada ao DBSet
                        database.coordenadas.Add(novaCoord);
                    }

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertRota Exception: " + e.Message);
                return false;
            }
        }

        public bool DeleteRota(string usrEmail, int idRota)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    rotas rota = database.rotas.Find(usrEmail, idRota);
                    if (rota == null)
                        return false;

                    // elimina todas as coordenadas da rota
                    if (EliminaCoordenadasRota(usrEmail, idRota) == false)
                        return false;

                    // remove rota do DBSet
                    database.rotas.Remove(rota);

                    // atualiza banco de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteRota Exception: " + e.Message);
                return false;
            }
        }

        private List<CoordenadaDataContract> GetCoordenadas(string emailUsr, int rotaId)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    // retorna os registros da tabela coordenadas
                    var reg = from l in database.coordenadas
                              where l.id_rota == rotaId
                              select l;

                    if (reg == null)
                        return null;

                    // estrutura contendo todos os registros retornados pela consulta
                    List<CoordenadaDataContract> coordList = new List<CoordenadaDataContract>();

                    // percorre todos os registros retornados na consulta anterior
                    CoordenadaDataContract novaCoord;
                    foreach (coordenadas c in reg)
                    {
                        novaCoord = new CoordenadaDataContract
                        {
                            Id = c.id,
                            EmailUsr = c.email_usr,
                            IdRota = c.id_rota,
                            DataHora = c.datahora,
                            Latitute = c.latitude,
                            Longitude = c.longitude
                        };

                        coordList.Add(novaCoord);
                    }

                    // retorna lista dos registros
                    return coordList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetCoordenadas Exception: " + e.Message);
                return null;
            }
        }

        private bool EliminaRotasUsuario(string emailUser)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    // retorna os registros da tabela rotas
                    var reg = from l in database.rotas
                              where l.email_usr == emailUser
                              select l;

                    if (reg == null)
                        return true;

                    // percorre todos os registros retornados na consulta anterior
                    rotas rota;
                    foreach (rotas r in reg)
                    {
                        rota = database.rotas.Find(r.email_usr, r.id);

                        // elimina as coordendas da rota
                        EliminaCoordenadasRota(r.email_usr, r.id);

                        // remove rota do DBSet
                        database.rotas.Remove(rota);
                    }

                    // atualiza banco de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EliminaRotasUsuario Exception: " + e.Message);
                return false;
            }
        }

        private bool EliminaCoordenadasRota(string emailUsr, int rotaId)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    // retorna os registros da tabela coordenadas
                    var reg = from l in database.coordenadas
                              where l.email_usr == emailUsr && l.id_rota == rotaId
                              select l;

                    if (reg == null)
                        return true;

                    // percorre todos os registros retornados na consulta anterior
                    coordenadas coord;
                    foreach (coordenadas c in reg)
                    {
                        coord = database.coordenadas.Find(c.id);

                        // remove coordenada do DBSet
                        database.coordenadas.Remove(coord);
                    }

                    // atualiza banco de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EliminaRotasUsuario Exception: " + e.Message);
                return false;
            }
        }
    }
}
