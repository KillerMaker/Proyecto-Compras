using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;

namespace WebApplication4.Models
{
    public class CArticulo : CEntidad
    {
        //atributos de constructor
        public int? id { get; set; }
        public string descripcion { get; set; }
        public int marca { get; set; }
        public int unidadMedida { get; set; }
        public int existencia { get; set; }
        public int estado { get; set; }

        //atributos de vista
        public string nombreMarca { get; set; }
        public string nombreEstado { get; set; }
        public string nombreUnidadMedida { get; set; }

        public CArticulo(int? id, string descripcion, int marca, int unidadMedida, int existencia, int estado)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.marca = marca;
            this.unidadMedida = unidadMedida;
            this.existencia = existencia;
            this.estado = estado;
        }

        public async override Task<int> Insert() => 
            await ExecuteCommand(@"INSERT INTO ARTICULO
                                                    (DESCRIPCION,MARCA,UNIDAD_MEDIDA,EXISTENCIA,ESTADO)
                                                    VALUES(
                                                            @DESCRIPCION, 
                                                            @MARCA,
                                                            @UNIDAD_MEDIDA,
                                                            @EXISTENCIA,
                                                            @ESTADO)",SqlLlenaParametros());

        public async override Task<int> Update() => 
            await ExecuteCommand(@"UPDATE ARTICULO SET
                                                DESCRIPCION = @DESCRIPCION,
                                                MARCA = @MARCA,
                                                UNIDAD_MEDIDA = @UNIDAD_MEDIDA,
                                                EXISTENCIA = @EXISTENCIA,
                                                ESTADO = @ESTADO WHERE ID=@ID",SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($@"DELETE FROM ARTICULO WHERE ID={id}");

        public async static Task<List<CArticulo>> Select(string searchString = null)
        {
            try
            {
                List<CArticulo> articulos = new List<CArticulo>();
                CArticulo articulo = null;
                string query = $"SELECT * FROM VISTA_ARTICULO {searchString}";

                using (MySqlDataReader reader = await ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        articulo = new CArticulo
                            (
                                (int)reader[0],
                                (string)reader[1],
                                (int)reader[2],
                                (int)reader[3],
                                (int)reader[4],
                                (int)reader[5]
                            )
                        {   
                            nombreMarca=(string)reader[6],
                            nombreUnidadMedida=(string)reader[7],
                            nombreEstado=(string)reader[8]
                        };

                        articulos.Add(articulo);
                    }

                    await _connection.CloseAsync();
                    return articulos;
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
            MySqlParameter SqlDescripcion = new MySqlParameter("@DESCRIPCION", MySqlDbType.VarChar, 200);
            MySqlParameter SqlMarca = new MySqlParameter("@MARCA", MySqlDbType.Int32);
            MySqlParameter SqlUnidadMedida = new MySqlParameter("@UNIDAD_MEDIDA", MySqlDbType.Int32);
            MySqlParameter SqlExistencia = new MySqlParameter("@EXISTENCIA", MySqlDbType.Int32);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            SqlDescripcion.Value = descripcion;
            SqlMarca.Value = marca;
            SqlUnidadMedida.Value = unidadMedida;
            SqlExistencia.Value = existencia;
            SqlEstado.Value = estado;

            if (id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>()
                    {SqlId,SqlDescripcion,SqlMarca,SqlUnidadMedida,SqlExistencia,SqlEstado};
            }
            else
                parametros = new List<MySqlParameter>()
                    { SqlDescripcion,SqlMarca,SqlUnidadMedida,SqlExistencia,SqlEstado};

            return parametros;
        }

        public override IEnumerator<KeyValuePair<string,object>> GetEnumerator()
        {
            yield return new KeyValuePair<string, object>("ID", id);
            yield return new KeyValuePair<string, object>("Descripcion", descripcion);
            yield return new KeyValuePair<string, object>("Marca", nombreMarca);
            yield return new KeyValuePair<string, object>("Unidad de Medida", nombreUnidadMedida);
            yield return new KeyValuePair<string, object>("Existencia", existencia);
            yield return new KeyValuePair<string, object>("Estado", nombreEstado);
        }


    }
}
