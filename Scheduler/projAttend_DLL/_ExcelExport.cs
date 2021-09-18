using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projAttend_DLL
{
    public class _ExcelExport
    {
        public void ExcelExportFromDGV(DataGridView dgv, System.Windows.Forms.ToolStripLabel lbl)
        {
            int TotalRows = 0;

            lbl.Text = "Processing";
            try
            {
                // creating Excel Application
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();


                // creating new WorkBook within Excel application
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);


                // creating new Excelsheet in workbook
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                // see the excel sheet behind the program
                app.Visible = true;

                // get the reference of first sheet. By default its name is Sheet1.
                // store its reference to worksheet
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                // changing the name of active sheet

                worksheet.Name = "Sheet1";

                TotalRows = dgv.Rows.Count;

                // storing header part in Excel
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                }




                for (int i = 1; i <= TotalRows; i++)
                {
                    Application.DoEvents();
                    lbl.Text = "Processing " + i + " of " + TotalRows;


                    for (int j = 1; j <= dgv.Columns.Count; j++)
                    {
                        if (dgv.Columns[j - 1].HeaderText.Trim().ToLower().Contains("acno") || dgv.Columns[j - 1].HeaderText.Trim().ToLower().Equals("sendac"))
                        {
                            worksheet.Cells[i + 1, j] = "'" + dgv.Rows[i - 1].Cells[j - 1].Value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j] = dgv.Rows[i - 1].Cells[j - 1].Value;
                        }

                    }
                }



                lbl.Text = "Setting Layout ";
                Application.DoEvents();


                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, 1], (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, dgv.Columns.Count]);
                Microsoft.Office.Interop.Excel.Font x = myRange.Font;
                x.Name = "Arial";
                x.Size = 10;
                x.Bold = true;
                myRange.Interior.Color = System.Drawing.Color.Yellow;

                myRange = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[2, 1], (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[dgv.Rows.Count + 1, dgv.Columns.Count]);
                x = myRange.Font;
                x.Name = "Arial";
                x.Size = 8;


                myRange = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, 1], (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[dgv.Rows.Count + 1, dgv.Columns.Count]);
                myRange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders y = myRange.Borders;
                y.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                y.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                y.Weight = 2D;


                // Exit from the application
                app.Quit();
                lbl.Text = "Done";
            }
            catch (Exception ex) { MessageBox.Show("Some Error " + ex.Message); }

        }
    }
}
