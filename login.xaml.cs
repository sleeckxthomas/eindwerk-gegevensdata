using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;


namespace eindopdracht
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        private List<gebruiker> gebruikers { get; set; } = new List<gebruiker> { };
        public login()
        {
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                var gebruiker = db.gebruikers.FirstOrDefault(gebruiker => gebruiker.naam == tbx_gebruiker.Text);

                if(gebruiker == null)
                {
                    MessageBox.Show("gebruikersnaam bestaat niet of is verkeerd.");
                }
                else
                {
                    /*
                    string hash = BC.HashPassword("Mijn_eindwerk!");
                    gebruiker.paswoord = hash;
                    db.SaveChanges();
                    */
                    if(BC.Verify(pwb_wachtwoord.Password, gebruiker.paswoord))
                    {
                        MainWindow MainWindow = new MainWindow();
                        MainWindow.Show();
                        this.Close();
                    }
                }
            }
        }

        private void nieuwe_gebruiker_Click(object sender, RoutedEventArgs e)
        {
            Window toevoegen_login = new toevoegen_login();
            toevoegen_login.Show();
            this.Close();
        }
    }
}
