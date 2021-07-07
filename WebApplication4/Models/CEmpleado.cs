using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class CEmpleado:CEntidad
    {
        //atributos de constructor
        public int? id { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public int departamento { get; set; }
        public  int estado { get; set; }

        //atributos de vista
        public string nombreEstado { get; set; }
        public string nombreDepartamento { get; set; }

        public CEmpleado(int? id, string cedula,string nombre,int departamento,int estado)
        {
            this.id = id;
            this.cedula = cedula;
            this.nombre = nombre;
            this.departamento = departamento;
            this.estado = estado;
        }

        public async override Task<int> Insert() =>
            await ExecuteCommand(@"INSERT INTO EMPLEADO
                                    (CEDULA,
                                    NOMBRE,
                                    DEPARTAMENTO,
                                    ESTADO)
                                   VALUES(@CEDULA,@NOMBRE,@DEPARTAMENTO,@ESTADO);", SqlLlenaParametros());


        public async override Task<int> Update() =>
            await ExecuteCommand($@"UPDATE EMPLEADO SET 
                                    CEDULA=@CEDULA,
                                    NOMBRE=@NOMBRE, 
                                    DEPARTAMENTO=@DEPARTAMENTO, 
                                    ESTADO=@ESTADO
                                    WHERE ID=@ID",SqlLlenaParametros());
        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM EMPLEADO WHERE ID={id}");


        public async static Task<List<CEmpleado>> Select(string searchString = null)
        {
            try
            {
                List<CEmpleado> empleados = new List<CEmpleado>();
                CEmpleado empleado = null;

                using(MySqlDataReader reader= await ExecuteReader($@"SELECT * FROM VISTA_EMPLEADO {searchString}"))
                {
                    while (await reader.ReadAsync())
                    {
                        empleado = new CEmpleado((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3], (int)reader[4]) 
                            { nombreEstado=(string)reader[5],nombreDepartamento=(string)reader[6]};

                        empleados.Add(empleado);
                    }

                    await _connection.CloseAsync();
                    return empleados;
                }
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
            MySqlParameter SqlCedula = new MySqlParameter("@CEDULA", MySqlDbType.VarChar,11);
            MySqlParameter SqlNombre = new MySqlParameter("@NOMBRE", MySqlDbType.VarChar, 50);
            MySqlParameter SqlDepartamento = new MySqlParameter("@DEPARTAMENTO", MySqlDbType.Int32);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            SqlCedula.Value = cedula;
            SqlNombre.Value = nombre;
            SqlDepartamento.Value = departamento;
            SqlEstado.Value = estado;

            if(id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>() { SqlId, SqlCedula,SqlNombre, SqlDepartamento, SqlEstado };
            }
            else
                parametros = new List<MySqlParameter>() {SqlNombre,SqlCedula, SqlDepartamento, SqlEstado };

            return parametros;
        }
    }
}
