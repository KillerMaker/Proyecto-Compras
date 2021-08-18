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
        string ruta="";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        public Excel(string nombre= "report")
        {
            ruta = "C:\\Users\\emman\\OneDrive\\Escritorio\\Reportes\\"+ nombre+" "+DateTime.Now.ToString();
            //wb = excel.Workbooks.Open(ruta);
            //ws = wb.Worksheets[hoja];

            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = wb.Worksheets[1];
        }
        public void Write(IEnumerable<CEntidad>entidades)
        {
            int i=1;
            int j = 1;

            foreach(var columna in entidades.ToArray()[0])
            {
                ws.Cells[i, j].Value2 = columna.Key;
                j++;
            }

            i = 2;
            j = 1;

            foreach(var entidad in entidades)
            {
                foreach(var columna in entidad)
                {
                    ws.Cells[i, j].Value2 = columna.Value.ToString();
                    j++;
                }
                i++;
            }

            wb.SaveAs(ruta);
                
           
        }
    }
}
