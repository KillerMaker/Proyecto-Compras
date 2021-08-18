using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class COrdenCompra : CEntidad
    {
        //Atributos de constructor
        public int? id { get; set; }
        public int solicitud { get; set; }
        public string fecha { get; set; }
        public int articulo { get; set; }
        public int cantidad { get; set; }
        public int unidadMedida { get; set; }
        public int marca { get; set; }
        public int costoUnitario { get; set; }
        public int estado { get; set; }

        //Atributos de vista
        public string NombreArticulo { get; set; }
        public string NombreUnidadMedida { get; set; }
        public string nombreMarca { get; set; }
        public string nombreEstado { get; set; }

        public COrdenCompra(int? id,int solicitud,string fecha,int articulo,int cantidad,int unidadMedida,int marca, int costoUnitario,int estado)
        {
            this.id = id;
            this.solicitud = solicitud;
            this.fecha = fecha;
            this.articulo = articulo;
            this.cantidad = cantidad;
            this.unidadMedida = unidadMedida;
            this.marca = marca;
            this.costoUnitario = costoUnitario;
            this.estado = estado;
        }
        public async override Task<int> Insert() =>
            await ExecuteCommand(@"CALL INSERTA_ORDEN_COMPRA (@SOLICITUD,@FECHA,@ARTICULO,@CANTIDAD,@UNIDAD_MEDIDA,@MARCA,@COSTO_UNITARIO,@ESTADO);", SqlLlenaParametros());

        public async override Task<int> Update() =>
            await ExecuteCommand(@"UPDATE ORDEN_COMPRA SET SOLICITUD=@SOLICITUD, FECHA=@FECHA, ARTICULO=@ARTICULO, CANTIDAD=@CANTIDAD,
                                        UNIDAD_MEDIDA=@UNIDAD_MEDIDA, MARCA=@MARCA, COSTO_UNITARIO=@COSTO_UNITARIO, ESTADO=@ESTADO
                                            WHERE ID = @ID", SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM ORDEN_COMPRA WHERE ID = {id}");

        public async static Task<List<COrdenCompra>>Select(string SearchString)
        {
            List<COrdenCompra> ordenCompras = new List<COrdenCompra>();
            COrdenCompra ordenCompra = null;

            using(MySqlDataReader reader=await ExecuteReader("SELECT * FROM VISTA_ORDEN_COMPRA"))
            {
                while(await reader.ReadAsync())
                {
                    ordenCompra = new COrdenCompra
                        (
                            (int)reader[0],
                            (int)reader[1],
                            reader[2].ToString(),
                            (int)reader[3],
                            (int)reader[4],
                            (int)reader[5],
                            (int)reader[6],
                            (int)reader[7],
                            (int)reader[8]
                        )
                    {
                        NombreArticulo=(string)reader[9],
                        NombreUnidadMedida=(string)reader[10],
                        nombreMarca=(string)reader[11],
                        nombreEstado=(string)reader[12]
                    };

                    ordenCompras.Add(ordenCompra);
                }

                await _connection.CloseAsync();
                return ordenCompras;
            }
        }

        protected override List<MySqlParameter> SqlLlenaParametros()
        {
            List<MySqlParameter> parameteros;

            MySqlParameter id = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter solicitud = new MySqlParameter("@SOLICITUD", MySqlDbType.Int32);
            MySqlParameter fecha = new MySqlParameter("@FECHA", MySqlDbType.VarChar, 8);
            MySqlParameter articulo = new MySqlParameter("@ARTICULO", MySqlDbType.Int32);
            MySqlParameter cantidad = new MySqlParameter("@CANTIDAD", MySqlDbType.Int32);
            MySqlParameter unidadMedida = new MySqlParameter("@UNIDAD_MEDIDA", MySqlDbType.Int32);
            MySqlParameter marca = new MySqlParameter("@MARCA", MySqlDbType.Int32);
            MySqlParameter costoUnitario = new MySqlParameter("@COSTO_UNITARIO", MySqlDbType.Int32);
            MySqlParameter estado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            solicitud.Value = this.solicitud;
            fecha.Value = this.fecha;
            articulo.Value = this.articulo;
            cantidad.Value = this.cantidad;
            unidadMedida.Value = this.unidadMedida;
            marca.Value = this.marca;
            costoUnitario.Value = this.costoUnitario;
            estado.Value = this.estado;

            if (this.id.HasValue)
            {
                id.Value = this.id;
                parameteros = new List<MySqlParameter>() { id, solicitud, fecha, articulo, cantidad, unidadMedida, marca, costoUnitario, estado };
            }
            else
                parameteros = new List<MySqlParameter>() { id, solicitud, fecha, articulo, cantidad, unidadMedida, marca, costoUnitario, estado };

            return parameteros;
        }

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            yield return new KeyValuePair<string, object>("ID", id);
            yield return new KeyValuePair<string, object>("Solicitud", solicitud);
            yield return new KeyValuePair<string, object>("Fecha", fecha);
            yield return new KeyValuePair<string, object>("Articulo", NombreArticulo);
            yield return new KeyValuePair<string, object>("Cantidad", cantidad);
            yield return new KeyValuePair<string, object>("Unidad de Medida", NombreUnidadMedida);
            yield return new KeyValuePair<string, object>("Marca", nombreMarca);
            yield return new KeyValuePair<string, object>("Costo unitario", costoUnitario);
            yield return new KeyValuePair<string, object>("Estado", nombreEstado);
        }
    }
}
