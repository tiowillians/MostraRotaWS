using MostraRotaWS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MostraRotaWS
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "MostraRotaService" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione MostraRotaService.svc ou MostraRotaService.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class MostraRotaService : IMostraRotaService
    {
        private static NumberFormatInfo formatinfo = null;

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
                        NumRota = rot.num_rota,
                        DtHrIni = rot.dthr_ini.ToString("G"),
                        DtHrFim = rot.dthr_fim.ToString("G"),
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
                            NumRota = r.num_rota,
                            DtHrIni = r.dthr_ini.ToString("G"),
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

        public bool InsertRotaCompleta(RotaDataContract dados)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    rotas novaRota = new rotas
                    {
                        email_usr = dados.EmailUsuario,
                        num_rota = dados.NumRota,
                        dthr_ini = DateTime.Parse(dados.DtHrIni),
                        dthr_fim = DateTime.Parse(dados.DtHrFim),
                        distancia = dados.Distancia
                    };

                    // adiciona rota no DBSet
                    database.rotas.Add(novaRota);

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    // obtem coordenadas da rota
                    coordenadas novaCoord;
                    foreach (CoordenadaDataContract c in dados.Coordenadas)
                        InsertCoordenada(c);

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertRotaCompleta Exception: " + e.Message);
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

        private List<CoordenadaDataContract> GetCoordenadas(string emailUsr, int idRota)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    // retorna os registros da tabela coordenadas
                    var reg = from l in database.coordenadas
                              where l.num_rota == idRota
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
                            EmailUsr = c.email_usr,
                            NumRota = c.num_rota,
                            Seq = c.seq,
                            DataHora = c.datahora.ToString("G"),
                            Latitude = c.latitude.ToString(),
                            Longitude = c.longitude.ToString()
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

        private bool InsertCoordenada(CoordenadaDataContract dados)
        {
            try
            {
                // Conecta ao banco de dados MostraRota usando Entity Framework
                using (mostrarotaEntities database = new mostrarotaEntities())
                {
                    coordenadas novaCoord = new coordenadas
                    {
                        email_usr = dados.EmailUsr,
                        num_rota = dados.NumRota,
                        seq = dados.Seq,
                        datahora = DateTime.Parse(dados.DataHora),
                        latitude = GetFloat(dados.Latitude),
                        longitude = GetFloat(dados.Longitude)
                    };

                    // adiciona coordenada no DBSet
                    database.coordenadas.Add(novaCoord);

                    // salva DBSet na base de dados
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertRotaSimples Exception: " + e.Message);
                return false;
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
                        rota = database.rotas.Find(r.email_usr, r.num_rota);

                        // elimina as coordendas da rota
                        EliminaCoordenadasRota(r.email_usr, r.num_rota);

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
                              where l.email_usr == emailUsr && l.num_rota == rotaId
                              select l;

                    if (reg == null)
                        return true;

                    // percorre todos os registros retornados na consulta anterior
                    coordenadas coord;
                    foreach (coordenadas c in reg)
                    {
                        coord = database.coordenadas.Find(c.email_usr, c.num_rota, c.seq);

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

        private float GetFloat(string s)
        {
            float d;

            if (formatinfo == null)
                formatinfo = new NumberFormatInfo();

            formatinfo.NumberDecimalSeparator = ".";

            if (float.TryParse(s, NumberStyles.Float, formatinfo, out d))
            {
                return d;
            }

            formatinfo.NumberDecimalSeparator = ",";

            if (float.TryParse(s, NumberStyles.Float, formatinfo, out d))
            {
                return d;
            }

            return 0;
        }
    }
}
