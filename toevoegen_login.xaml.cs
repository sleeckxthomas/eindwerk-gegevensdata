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
using BC = BCrypt.Net.BCrypt;

namespace eindopdracht
{
    /// <summary>
    /// Interaction logic for toevoegen_login.xaml
    /// </summary>
    public partial class toevoegen_login : Window
    {
        private List<gebruiker> gebruikers { get; set; } = new List<gebruiker> { };
        public toevoegen_login()
        {
            InitializeComponent();

        }

        private void create_button_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                gebruikers = db.gebruikers.ToList();

                bool user_bestaat = gebruikers.Exists(gebruiker => gebruiker.naam == tbx_nieuwe_gebruiker.Text);

                if (user_bestaat)
                {
                    MessageBox.Show("dubbel");
                }
                else
                {
                    string hash = BC.HashPassword(pwb_nieuw_wachtwoord.Password);
                    db.Add(new gebruiker { naam = tbx_nieuwe_gebruiker.Text, paswoord = hash });
                    db.SaveChanges();
                    MessageBox.Show("gebruiker toegevoegd.");
                }
                tbx_nieuwe_gebruiker.Clear();
                pwb_nieuw_wachtwoord.Clear();
                Window login = new login();
                login.Show();
                this.Close();
            }
        }

        private void terug_login_Click(object sender, RoutedEventArgs e)
        {
            Window login = new login();
            login.Show();
            this.Close();
        }
    }
}
