using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace WebApplication4.Models
{
    public class Excel
    {
        private readonly string nombre = "";
        private readonly _Application excel = new _Excel.Application();
        private Workbook wb;
        private Worksheet ws;
        public Excel(string nombre= "report")
        {
            this.nombre = nombre + " " + DateTime.Now.ToString().Replace(":", "_").Replace("\\","-").Replace("/","-");
            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            try
            {
                wb.SaveAs(this.nombre);
                wb.Close();
            }
            catch(Exception e)
            {
                wb.Close();
                throw new Exception(e.Message);
            }
        }
        public void Write(IEnumerable<CEntidad>entidades)
        {
            wb = excel.Workbooks.Open(nombre);

            try
            {
                ws = wb.Worksheets[1];
                int i = 1;
                int j = 1;

                foreach (var columna in entidades.ToArray()[0])
                {
                    ws.Cells[i, j].Value2 = columna.Key;
                    j++;
                }

                i = 2;
                j = 1;

                foreach (var entidad in entidades)
                {
                    foreach (var columna in entidad)
                    {
                        ws.Cells[i, j].Value2 = columna.Value.ToString();
                        j++;
                    }
                    j = 1;
                    i++;
                }
                wb.Save();
                wb.Close();
            }
            catch (Exception e)
            {
                wb.Close();
                throw new Exception(e.Message);
            }
        }
    }
}
