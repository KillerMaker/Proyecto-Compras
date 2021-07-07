using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CDepartamento : CEntidad
    {
        public int? id { get; set; }
        public string nombre { get; set; }
        public int estado { get; set; }

        public CDepartamento(int? id,string nombre, int estado)
        {
            this.id = id;
            this.nombre = nombre;
            this.estado = estado;
        }

        public async override Task<int> Insert() => 
            await ExecuteCommand(@"INSERT INTO DEPARTAMENTO(NOMBRE,ESTADO)VALUES(@NOMBRE,@ESTADO)",SqlLlenaParametros());

        public async override Task<int> Update() => 
            await ExecuteCommand(@"UPDATE DEPARTAMENTO SET NOMBRE=@NOMBRE,ESTADO=@ESTADO WHERE ID=@ID",SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM DEPARTAMENTO WHERE ID={id}");

        public async static Task<List<CDepartamento>> Select(string searchString = null)
        {
            try
            {
                List<CDepartamento> departamentos = new List<CDepartamento>();
                CDepartamento departamento = null;
                string query = $@"SELECT * FROM DEPARTAMENTO {searchString}";

                using (MySqlDataReader reader =await ExecuteReader(query))
                    
                    while (await reader.ReadAsync())
                    {
                        departamento = new CDepartamento((int)reader[0], (string)reader[1], (int)reader[2]);
                        departamentos.Add(departamento);
                    }

                await _connection.CloseAsync();
                return departamentos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override List<MySqlParameter> SqlLlenaParametros()
        {
            List<MySqlParameter> parametros;
            MySqlParameter SqlId = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter SqlNombre = new MySqlParameter("@NOMBRE", MySqlDbType.VarChar,50);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            SqlNombre.Value = nombre;
            SqlEstado.Value = estado;

            if (id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>() { SqlId, SqlNombre, SqlEstado };
            }
            else
                parametros = new List<MySqlParameter>() {SqlNombre, SqlEstado };

            return parametros;

        }
    }
}
