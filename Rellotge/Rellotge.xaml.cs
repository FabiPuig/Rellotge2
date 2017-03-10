using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Rellotge
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string path = "E:\\intelij\\M07\\C#\\PT_Rellotge\\Rellotge\\SavedAlarm.bin";
        public Alarma alarm;

        public MainWindow()
        {
            InitializeComponent();
            lbTime.Content = DateTime.Now.ToLongTimeString();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds( 1 );
            timer.Tick += timer_Tick;
            timer.Start();

            string[] hores = new string[24];

            for (int i = 0; i < 24; i++)
            {
                if ( i < 10)
                {
                    hores[i] = "0" + i.ToString();
                }
                else
                {
                    hores[i] = i.ToString();               
                }
            }
            cbHores.ItemsSource = hores;

            string[] minuts = new string[60];
            for ( int i = 0; i < 60; i++)
            {
                if ( i < 10 )
                {
                    minuts[i] = "0" + i.ToString();
                }
                else
                {
                    minuts[i] = i.ToString();
                }
            }
            cbMinuts.ItemsSource = minuts;

            alarm = new Alarma();
        }

        private void timer_Tick(object sender, EventArgs e)
        {           
            string date = DateTime.Now.ToLongTimeString();
            string fechaFinal = string.Format("{0:HH:mm:ss}", date);

            if ( !string.IsNullOrEmpty( fechaFinal ) )
            {
                lbTime.Content = fechaFinal;
            }


            if ( alarm.alarmaActiva )
            {
                if ( string.Compare( fechaFinal, alarm.hora + ":" + alarm.minut + ":00" ) == 0 )
                {
                    winAlarma();
                }
            }
        }

        private void Sortir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void sobre_programa_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Creat per Fabian Puig";
            string caption = "SOBRE L'AUTOR";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage img = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, img);
        }

        private void rbActivar_Checked(object sender, RoutedEventArgs e)
        {
            alarm.alarmaActiva = true;
            alarm.hora = cbHores.SelectedItem.ToString();
            alarm.minut = cbMinuts.SelectedItem.ToString();
        }

        private void rbDesactivar_Checked(object sender, RoutedEventArgs e)
        {
            alarm.alarmaActiva = false;
        }

        public void winAlarma()
        {
            string messageBoxText = "RRRRRRRRRRRIIIIIIIIIIIIIIIING!!!";
            string caption = "ALARMA";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage img = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, img);
            SystemSounds.Beep.Play();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Stream TestFileStream = File.Create( path );
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, alarm);
            TestFileStream.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ( File.Exists( path ) )
            {
                Stream TestFileStream = File.OpenRead( path );
                BinaryFormatter deserializer = new BinaryFormatter();
                alarm = (Alarma)deserializer.Deserialize( TestFileStream );
                TestFileStream.Close();

                cbHores.SelectedIndex = int.Parse ( alarm.hora );
                cbMinuts.SelectedIndex = int.Parse( alarm.minut );
                if ( alarm.alarmaActiva )
                {
                    rbActivar.IsChecked = true;
                }else
                {
                    rbDesactivar.IsChecked = true;
                }

            }else
            {
                cbHores.SelectedIndex = 0;
                cbMinuts.SelectedIndex = 0;
            }

        }

        private void cbHores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( !string.IsNullOrEmpty( cbHores.Text ) )
            {
                alarm.hora = cbHores.SelectedItem.ToString();
            }
        }

        private void cbMinuts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( !string.IsNullOrEmpty( cbMinuts.Text ) )
            {
                alarm.minut = cbMinuts.SelectedItem.ToString();
            }
        }
    }
}
