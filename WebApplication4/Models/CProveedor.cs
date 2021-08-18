using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CProveedor : CEntidad
    {
        //Atributos de constructor
        public int? id { get; set; }
        public string cedula { get; set; }
        public string nombreComercial {get ; set;}
        public int estado { get; set; }

        //Atributos de vista
        public string nombreEstado { get; set; }
        public CProveedor(int? id,string cedula,string nombreComercial,int estado)
        {
            this.id = id;
            this.cedula = cedula;
            this.nombreComercial = nombreComercial;
            this.estado = estado;
        }

        public async override Task<int> Insert() =>
            await ExecuteCommand(@"INSERT INTO PROVEEDOR
                                   (CEDULA, NOMBRE_COMERCIAL,ESTADO )
                                   VALUES(@CEDULA,@NOMBRE_COMERCIAL,@ESTADO);", SqlLlenaParametros());

        public async override Task<int> Update() =>
            await ExecuteCommand(@"UPDATE PROVEEDOR SET
                                                    CEDULA=@CEDULA,
                                                    NOMBRE_COMERCIAL=@NOMBRE_COMERCIAL,
                                                    ESTADO=@ESTADO
                                                WHERE ID=@ID;",SqlLlenaParametros());
        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM PROVEEDOR WHERE ID={id}");

        public async static Task<List<CProveedor>> Select(string searchString = null)
        {
            try
            {
                List<CProveedor> proveedores = new List<CProveedor>();
                CProveedor proveedor = null;


                using(MySqlDataReader reader= await ExecuteReader($"SELECT * FROM PROVEEDOR P INNER JOIN ESTADO E ON E.ID=P.ESTADO {searchString}"))
                {
                    while (await reader.ReadAsync())
                    {
                        proveedor = new CProveedor((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3]) 
                        {
                            nombreEstado=(string)reader[5]
                        };
                        proveedores.Add(proveedor);
                    }

                    await _connection.CloseAsync();
                    return proveedores;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected override List<MySqlParameter> SqlLlenaParametros()
        {
            List<MySqlParameter> parameteros;
            MySqlParameter SqlId = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter SqlCedula = new MySqlParameter("@Cedula", MySqlDbType.VarChar,11);
            MySqlParameter SqlNombreComercial = new MySqlParameter("@NOMBRE_COMERCIAL", MySqlDbType.VarChar, 50);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);

            SqlCedula.Value = cedula;
            SqlNombreComercial.Value = nombreComercial;
            SqlEstado.Value = estado;

            if(id.HasValue)
            {
                SqlId.Value = id;
                parameteros = new List<MySqlParameter>() { SqlId,SqlCedula,SqlNombreComercial,SqlEstado};
            }
            else
                parameteros = new List<MySqlParameter>() {SqlCedula, SqlNombreComercial, SqlEstado };

            return parameteros;

        }

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            yield return new KeyValuePair<string, object>("ID", id);
            yield return new KeyValuePair<string, object>("Cedula", cedula);
            yield return new KeyValuePair<string, object>("Nombre Comercial", nombreComercial);
            yield return new KeyValuePair<string, object>("Estado", nombreEstado);
        }
    }
}
