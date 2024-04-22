using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WeatherApp
{
    internal class Cities
    {
        public string City_Name_eng { get; set; }
        public string City_Name_rus { get; set; }
        public double longityde { get; set; }
        public double latitude { get; set; }
        public string Country { get; set; }

        public static string Path(string xlsxpath_ar)
        {
            return String.Concat("D:\\Visual Studio\\Проекты\\C#\\WeatherApp\\", xlsxpath_ar);
        }

        public static IEnumerable<Cities> CitiesEnumerate(string xlsxpath_ar, int number_of_list = 1)
        {
            var xlsxpath = Path(xlsxpath_ar);
            // Открываем книгу
            using (var workbook = new XLWorkbook(xlsxpath))
            {
                // Берем в ней первый лист
                var worksheet = workbook.Worksheet(number_of_list);

                int row = 1;
                
                while (!worksheet.Cell(row, 1).IsEmpty())
                {   
                    
                    var metric = new Cities
                    {
                        City_Name_eng = worksheet.Cell(row, 1).GetValue<string>(),
                        City_Name_rus = worksheet.Cell(row, 2).GetValue<string>(),
                        longityde = worksheet.Cell(row, 3).GetValue<double>(),
                        latitude = worksheet.Cell(row, 4).GetValue<double>(),
                        Country = worksheet.Cell(row, 5).GetValue<string>(),
                    };

                    row++;

                    yield return metric;
                }
                
            }
        
        }

        internal void Add(string path_ar, int number_of_list)
        {
            var path = Path(path_ar);

            try
            {

                using (var workbook = new XLWorkbook(path))
                {
                    // Берем в ней первый лист
                    var worksheet = workbook.Worksheet(number_of_list);

                    int row = 1;

                    while (!worksheet.Cell(row, 1).IsEmpty()) {

                        if (worksheet.Cell(row, 1).GetValue<string>() == this.City_Name_eng) {
                            MessageBox.Show("Город уже добавлен!");
                            break; 
                        }

                        row++; 
                    }


                    worksheet.Cell(row, 1).Value = this.City_Name_eng.ToString();
                    worksheet.Cell(row, 2).Value = this.City_Name_rus.ToString();
                    worksheet.Cell(row, 3).Value = this.longityde.ToString();
                    worksheet.Cell(row, 4).Value = this.latitude.ToString();
                    worksheet.Cell(row, 5).Value = this.Country.ToString();

                    workbook.Save();

                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.ToString());
            }

        }

        public override string ToString()
        {
            return $"{City_Name_rus}";
        }

        internal static async Task DeleteRow()
        {
            await Task.Run(() =>
            {
                using (var workbook = new XLWorkbook(Cities.Path("cities_list.xlsx")))
                {
                    var worksheet = workbook.Worksheet(0);

                    // Удалить строку
                    worksheet.Row(1).Delete();


                    workbook.SaveAs(Cities.Path("cities_list.xlsx"));
                }
            });
        }
    }
}
