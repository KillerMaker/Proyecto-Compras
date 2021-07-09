using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Usuario : CEntidad
    {
        //Atributos de constructor
        public int? id { get; set; }
        public string nombre { get; set; }
        public string clave { get; set; }
        public int tipoUsuario { get; set; }
        public int estado { get; set; }

        //Atributos de vista
        public string nombreTipoUsuario { get; set; }
        public string nombreEstado { get; set; }

        public Usuario(int? id,string nombre, string clave,int tipoUsuario,int estado)
        {
            this.id = id;
            this.nombre = nombre;
            this.clave = clave;
            this.tipoUsuario = tipoUsuario;
            this.estado = estado;
        }
        public async override Task<int> Insert() =>
            await ExecuteCommand(@"INSERT INTO USUARIO(
                                        NOMBRE, CLAVE, TIPO_USUARIO, ESTADO)
                                        VALUES (@NOMBRE, @CLAVE, @TIPO_USUARIO, @ESTADO)", SqlLlenaParametros());

        public async override Task<int> Update() =>
            await ExecuteCommand(@"UPDATE USUARIO SET 
                                       NOMBRE = @NOMBRE, CLAVE = @CLAVE,
                                       TIPO_USUARIO = @TIPO_USUARIO, ESTADO = @ESTADO
                                       WHERE ID = @ID", SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM USUARIO WHERE ID = {id}");

        public async static Task<List<Usuario>>Select(string searchString=null)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario usuario = null;

            using(MySqlDataReader reader=await ExecuteReader($@"SELECT * FROM USUARIO U 
                                                                INNER JOIN TIPO_USUARIO TP ON TP.ID=U.TIPO_USUARIO
                                                                INNER JOIN ESTADO E ON E.ID =U.ESTADO {searchString}"))
            {
                while(await reader.ReadAsync())
                {
                    usuario = new Usuario((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3], (int)reader[4])
                    {
                        nombreTipoUsuario=(string)reader[6],
                        nombreEstado=(string)reader[9]
                    };

                    usuarios.Add(usuario);
                }

                await _connection.CloseAsync();
                return usuarios;
            }
        }

        public async static Task<Usuario>Login(string nombreUsuario, string Clave)
        {
            Usuario usuario = null;
            string loginValue = null;

            using (MySqlDataReader reader = await ExecuteReader($@"SELECT IF (EXISTS(SELECT * FROM USUARIO WHERE NOMBRE ='{nombreUsuario}' AND CLAVE ='{Clave}'),'TRUE','FALSE')"))
            {
                while (await reader.ReadAsync())
                    loginValue = reader[0].ToString();

                await _connection.CloseAsync();

                if (loginValue.Equals("TRUE"))
                {
                    MySqlDataReader reader2 = await ExecuteReader($"SELECT * FROM USUARIO WHERE NOMBRE ='{nombreUsuario}' AND CLAVE ='{Clave}'");

                    while (await reader2.ReadAsync())
                        usuario = new Usuario(
                            (int)reader2[0], 
                            (string)reader2[1], 
                            (string)reader2[2], 
                            (int)reader2[3], 
                            (int)reader2[4]);

                    reader2.Dispose();
                    await _connection.CloseAsync();

                    return usuario;
                }
                else
                    return null;
            }
        }

        protected override List<MySqlParameter> SqlLlenaParametros()
        {
            List<MySqlParameter> parametros;

            MySqlParameter SqlId = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter SqlNombre = new MySqlParameter("@NOMBRE", MySqlDbType.VarChar,50);
            MySqlParameter SqlClave = new MySqlParameter("@CLAVE", MySqlDbType.VarChar, 50);
            MySqlParameter SqlTipoUsuario= new MySqlParameter("TIPO_USUARIO", MySqlDbType.Int32);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            SqlNombre.Value = nombre;
            SqlClave.Value = clave;
            SqlTipoUsuario.Value = tipoUsuario;
            SqlEstado.Value = estado;


            if(id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>() { SqlId, SqlNombre, SqlClave, SqlTipoUsuario, SqlEstado };
            }
            else
                parametros = new List<MySqlParameter>() {SqlNombre, SqlClave, SqlTipoUsuario, SqlEstado };

            return parametros;
        }
    }
}
