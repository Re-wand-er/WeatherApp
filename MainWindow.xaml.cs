using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Net.Mail;
using DocumentFormat.OpenXml.Vml;
using System.Collections.ObjectModel;
using ClosedXML.Excel;

namespace WeatherApp
{
    public partial class MainWindow /*: Window*/
    {
        Cities value;
        public MainWindow()
        {
            InitializeComponent();

            
            this.Loaded += MainWindow_Loaded;
            
        }

        private void Cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

                if (sender is System.Windows.Controls.ComboBox)
                {
                    value = Cities_ComboBox.SelectedValue as Cities;
                }
                else
                {
                    value = Cities_ListBox.SelectedValue as Cities;
                }

                if (!(value is Cities)) return;

                Chosen_City.Text = value.City_Name_rus;

                string UrlString = "https://api.openweathermap.org/data/2.5/weather?q=" +
                    value.City_Name_eng + "," +
                    value.Country + "&lon=" +
                    value.longityde.ToString() + "&lat=" +
                    value.latitude.ToString() + "&appid=4ef16c1091ff58f06bdf640b80d809f1&mode=xml&units=metric&lang=ru";

                XmlReader xmlread = XmlReader.Create(UrlString);

                while (xmlread.Read())
                {
                    if ((xmlread.NodeType == XmlNodeType.Element) && (xmlread.Name == "temperature"))
                    {
                        if (xmlread.HasAttributes)
                            Output.Text = xmlread.GetAttribute("value");
                    }
                }
           
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            value.Add("cities_list.xlsx", 2);
            // Может что-то менее ресурсозатратное
            Cities_ListBox.ItemsSource = Cities.CitiesEnumerate("cities_list.xlsx", 2);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
                Cities_ComboBox.ItemsSource = Cities.CitiesEnumerate("cities_list.xlsx");
                Cities_ListBox.ItemsSource = Cities.CitiesEnumerate("cities_list.xlsx", 2);

                Cities_ListBox.SelectedIndex = 0;

                if (value == null)
                    Cities_ComboBox.SelectedIndex = 217;
        }

        private async void Remove_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Ага захотел!");
            //
            //Task task = new Task.Run(() =>
            //{
            //    
            //    using (var workbook = new XLWorkbook(Cities.Path("cities_list.xlsx")))
            //    {
            //        var worksheet = workbook.Worksheet(0);
            //
            //        worksheet.Row(1).Delete();
            //
            //        workbook.SaveAs(Cities.Path("cities_list.xlsx"));
            //    }
            //});
            await Cities.DeleteRow();
        }
    }
    
    
}
