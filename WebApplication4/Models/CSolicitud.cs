using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class CSolicitud : CEntidad
    {
        //Atributos de constructor
        public int? id { get; set; }
        public int empleado { get; set; }
        public string fecha { get; set; }
        public int articulo { get; set; }
        public int cantidad { get; set; }
        public int unidadMedida { get; set; }
        public int estado { get; set; }

        //Atributos de vista
        public string nombreEmpleado { get; set; }
        public string nombreArticulo { get; set; }
        public string nombreUnidadMedida { get; set; }
        public string nombreEstado { get; set;}

        public CSolicitud(int? id, int empleado,string fecha,int articulo,int cantidad, int unidadMedida,int estado)
        {
            this.id = id;
            this.empleado = empleado;
            this.fecha = fecha;
            this.articulo = articulo;
            this.cantidad = cantidad;
            this.unidadMedida = unidadMedida;
            this.estado = estado;
        }

        public async override Task<int> Insert() => 
            await ExecuteCommand(@"INSERT INTO SOLICITUD(EMPLEADO,FECHA,ARTICULO,CANTIDAD,UNIDAD_MEDIDA,ESTADO)
                                                                            VALUES(@EMPLEADO,@FECHA,@ARTICULO,@CANTIDAD,@UNIDAD_MEDIDA,@ESTADO)",SqlLlenaParametros());

        public async override Task<int> Update() => 
            await ExecuteCommand(@"UPDATE SOLICITUD SET EMPLEADO = @EMPLEADO, FECHA = @FECHA, ARTICULO = @ARTICULO, CANTIDAD= @CANTIDAD, UNIDAD_MEDIDA = @UNIDAD_MEDIDA
                                    WHERE ID = @ID", SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM SOLICITUD WHERE ID = {id}");

        public async static Task<List<CSolicitud>>Select(string searchString)
        {
            try
            {
                List<CSolicitud> solicitudes = new List<CSolicitud>();
                CSolicitud solicitud = null;

                string query = $"SELECT * FROM VISTA_SOLICITUD {searchString}";

                using (MySqlDataReader reader = await ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        solicitud = new CSolicitud
                            (
                                (int)reader[0],
                                (int)reader[1],
                                reader[2].ToString(),
                                (int)reader[3],
                                (int)reader[4],
                                (int)reader[5],
                                (int)reader[6]
                            )
                        { 
                            nombreEmpleado=(string)reader[7],
                            nombreArticulo=(string)reader[8],
                            nombreUnidadMedida=(string)reader[9],
                            nombreEstado=(string)reader[10]
                        };

                        solicitudes.Add(solicitud);
                    }

                    await _connection.CloseAsync();
                    return solicitudes;
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

            MySqlParameter id = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter empleado = new MySqlParameter("@EMPLEADO", MySqlDbType.Int32);
            MySqlParameter fecha = new MySqlParameter("@FECHA", MySqlDbType.VarChar, 8);
            MySqlParameter articulo = new MySqlParameter("@ARTICULO", MySqlDbType.Int32);
            MySqlParameter cantidad = new MySqlParameter("@CANTIDAD", MySqlDbType.Int32);
            MySqlParameter unidadMedida = new MySqlParameter("@UNIDAD_MEDIDA", MySqlDbType.Int32);
            MySqlParameter estado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            empleado.Value = this.empleado;
            fecha.Value = this.fecha;
            articulo.Value = this.articulo;
            cantidad.Value = this.cantidad;
            unidadMedida.Value = this.unidadMedida;
            estado.Value = this.estado;

            if(this.id.HasValue)
            {
                id.Value = this.id;

                parametros = new List<MySqlParameter>() { id,empleado,fecha,articulo,cantidad,unidadMedida,estado};
            }
            else
                parametros = new List<MySqlParameter>() {empleado, fecha, articulo, cantidad, unidadMedida, estado };

            return parametros;
        }

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            yield return new KeyValuePair<string, object>("ID", id);
            yield return new KeyValuePair<string, object>("Empleado", nombreEmpleado);
            yield return new KeyValuePair<string, object>("Fecha", fecha);
            yield return new KeyValuePair<string, object>("Articulo", articulo);
            yield return new KeyValuePair<string, object>("Cantidad", cantidad);
            yield return new KeyValuePair<string, object>("Unidad de Medida", nombreUnidadMedida);
            yield return new KeyValuePair<string, object>("Estado", estado);
        }
    }
}
