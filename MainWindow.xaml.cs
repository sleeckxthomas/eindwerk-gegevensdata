using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.EntityFrameworkCore;


namespace eindopdracht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<project> projecten { get; set; } = new List<project> { };
        private List<toegewezenen> toegewezenen { get; set; } = new List<toegewezenen> { };
        private List<wetenschapper> wetenschappers { get; set; } = new List<wetenschapper> { };
        public MainWindow()
        {
            InitializeComponent();
            using(var db = new ShopContext())
            {
                projecten = db.projecten.ToList();
                toegewezenen = db.toegewezenen.ToList();
                wetenschappers = db.wetenschappers.ToList();

                foreach(var wetenschapper in db.wetenschappers)
                {
                    lijst_wetenschappers.Items.Add(wetenschapper);
                    cmb_naam_wetenschapper.Items.Add(wetenschapper.naam);
                    cmb_kiezen_naam_wetenschapper.Items.Add(wetenschapper.naam);
                }
                foreach(var project in db.projecten)
                {
                    lijst_projecten.Items.Add(project);
                    cmb_naam_project.Items.Add(project.naam);
                    cmb_kiezen_naam_project.Items.Add(project.naam);
                }
            }
        }

        private void lijst_wetenschappers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var w = (wetenschapper)lijst_wetenschappers.SelectedItem;
            if(w != null)
            {
                tbx_wetenschapper.Text = w.naam;
            }
        }

        private void tbx_wetenschapper_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void add_wetenschapper_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                wetenschappers = db.wetenschappers.ToList();
                bool wetenschapper_bestaat = wetenschappers.Exists(w => w.naam == tbx_wetenschapper.Text);
                if (wetenschapper_bestaat)
                {
                    MessageBox.Show("wetenschapper bestaat al. gelieve eerste letter achternaam toe tevoegen.");
                }
                else if(tbx_wetenschapper.Text == "")
                {
                    MessageBox.Show("gelieven een wetenschapper in te geven");
                }
                else
                {
                    db.Add(new wetenschapper { naam = tbx_wetenschapper.Text });
                    cmb_naam_wetenschapper.Items.Add(tbx_wetenschapper.Text);
                    cmb_kiezen_naam_wetenschapper.Items.Add(tbx_wetenschapper.Text);
                    tbx_wetenschapper.Clear();
                }
                db.SaveChanges();
                lijst_refresh(lijst_wetenschappers);
            }
        }

        private void delete_wetenschapper_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                if(tbx_wetenschapper.Text == "")
                {
                    MessageBox.Show("gelieven een wetenschapper te selecteren");
                }
                else
                {
                    db.Remove((wetenschapper)lijst_wetenschappers.SelectedItem);
                    cmb_naam_wetenschapper.Items.Remove(tbx_wetenschapper.Text);
                    cmb_kiezen_naam_wetenschapper.Items.Remove(tbx_wetenschapper.Text);
                    db.SaveChanges();
                    tbx_wetenschapper.Clear();
                }
                lijst_refresh(lijst_wetenschappers);
            }
        }

        private void update_wetenschapper_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ShopContext())
            {
                bool wetenschapper_bestaat = wetenschappers.Exists(w => w.naam == tbx_wetenschapper.Text);
                if (wetenschapper_bestaat)
                {
                    MessageBox.Show("wetenschapper staat er al in. geef achternaam als extra");
                }
                else if(tbx_wetenschapper.Text == "")
                {
                    MessageBox.Show("gelieven een wetenschapper te selecteren");
                }
                else
                {
                    var w = (wetenschapper)lijst_wetenschappers.SelectedItem;
                    w.naam = tbx_wetenschapper.Text;
                    cmb_naam_wetenschapper.SelectedItem = w;
                    db.Update(w);
                    db.SaveChanges();
                    cmb_naam_wetenschapper.Items.Clear();
                    cmb_kiezen_naam_wetenschapper.Items.Clear();
                }
                lijst_refresh(lijst_wetenschappers);

                foreach(var w in db.wetenschappers)
                {
                    cmb_naam_wetenschapper.Items.Add(w.naam);
                    cmb_kiezen_naam_wetenschapper.Items.Add(w.naam);
                }
            }
        }
        public void lijst_refresh(ListBox gekozen_lijst)
        {
            int gekozen = gekozen_lijst.SelectedIndex;
            gekozen_lijst.Items.Clear();
            using (var db = new ShopContext())
            {

                if (gekozen_lijst == lijst_wetenschappers)
                {
                    var lijst_opnieuw = db.wetenschappers.ToList();
                    foreach (var v in lijst_opnieuw)
                    {
                        gekozen_lijst.Items.Add(v);
                    }

                }
                else
                {
                    var lijst_opnieuw = db.projecten.ToList();
                    foreach (var v in lijst_opnieuw)
                    {
                        gekozen_lijst.Items.Add(v);
                    }

                }
            }
            gekozen_lijst.SelectedIndex = gekozen;
        }

        private void lijst_projecten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (project)lijst_projecten.SelectedItem;
            if (p != null)
            {
                tbx_naam_project.Text = p.naam;
                tbx_aantal_uren.Text = p.uur.ToString();
            }
        }

        private void tbx_naam_project_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbx_aantal_uren_TextChanged(object sender, TextChangedEventArgs e)
        {
            float floaturen;
            if(tbx_aantal_uren.Text != "")
            {
                try
                {
                    floaturen = float.Parse(tbx_aantal_uren.Text);
                }
                catch(FormatException)
                {
                    MessageBox.Show("gelieven enkel een getal in te geven.");
                    tbx_aantal_uren.Clear();
                    return;
                }
            }
        }

        private void add_project_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ShopContext())
            {
                projecten = db.projecten.ToList();
                bool project_bestaat = projecten.Exists(w => w.naam == tbx_naam_project.Text);
                if (project_bestaat)
                {
                    MessageBox.Show("project bestaat al.");
                }
                else if(tbx_naam_project.Text == "" || tbx_aantal_uren.Text == "")
                {
                    MessageBox.Show("gelieven project en aantal uren in te geven.");
                }
                else
                {
                    db.Add(new project { naam = tbx_naam_project.Text, uur = float.Parse(tbx_aantal_uren.Text)});
                    cmb_naam_project.Items.Add(tbx_naam_project.Text);
                    cmb_kiezen_naam_project.Items.Add(tbx_naam_project.Text);
                    db.SaveChanges();
                    tbx_naam_project.Clear();
                    tbx_aantal_uren.Clear();
                }

                lijst_refresh(lijst_projecten);
            }
        }

        private void delete_project_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                if(tbx_naam_project.Text == "")
                {
                    MessageBox.Show("gelieven project te selecteren.");
                }
                else
                {
                    db.Remove((project)lijst_projecten.SelectedItem);
                    cmb_naam_project.Items.Remove(tbx_naam_project.Text);
                    cmb_kiezen_naam_project.Items.Remove(tbx_naam_project.Text);
                    db.SaveChanges();
                    tbx_naam_project.Clear();
                    tbx_aantal_uren.Clear();
                }

                lijst_refresh(lijst_projecten);
            }
        }

        private void update_project_Click(object sender, RoutedEventArgs e)
        {
            using (var db =  new ShopContext())
            {
                var p = (project)lijst_projecten.SelectedItem;
                bool project_bestaat = projecten.Exists(p => p.naam == tbx_naam_project.Text);
                if (tbx_naam_project.Text == "" || tbx_aantal_uren.Text == "")
                {
                    MessageBox.Show("gelieven project en aantal uren in te geven");
                }
                else if(project_bestaat)
                {                 
                    MessageBox.Show("project bestaat al");
                }
                else
                {
                    p.naam = tbx_naam_project.Text;
                    p.uur = float.Parse(tbx_aantal_uren.Text);
                    db.Update(p);
                    db.SaveChanges();
                    lijst_refresh(lijst_projecten);
                    cmb_naam_project.Items.Clear();
                    cmb_kiezen_naam_project.Items.Clear();
                }

                foreach (var pro in db.projecten)
                {
                    cmb_naam_project.Items.Add(pro.naam);
                    cmb_kiezen_naam_project.Items.Add(pro.naam);
                }
            }
        }
        private void delete_project_to_wetenschapper_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ShopContext())
            {
                if(lbx_wetnschapper_to_project.SelectedItem == null)
                {
                    MessageBox.Show("er zijn geen projecten toegewezen aan deze wetenschapper");
                }
                else
                {
                    var selected_project = lbx_wetnschapper_to_project.SelectedItem.ToString();
                    var selected_wetenschapper = cmb_naam_wetenschapper.SelectedItem.ToString();
                    var selected_project_id = db.projecten.Where(p => p.naam == selected_project).First().project_id;
                    var selected_wetenschapper_id = db.wetenschappers.Where(w => w.naam == selected_wetenschapper).First().wetenschapper_id;

                    var toegewezen_projecten = db.toegewezenen.Where(t => t.project_id == selected_project_id && t.wetenschapper_id == selected_wetenschapper_id);
                    foreach (var project in toegewezen_projecten)
                    {
                        db.Remove((toegewezenen)project);
                    }
                    db.SaveChanges();
                    lbx_wetnschapper_to_project.Items.Clear();
                    var wetenschapper_identity = db.wetenschappers.Where(w => w.naam == cmb_naam_wetenschapper.SelectedItem.ToString()).First();
                    var wetenschapper_id1 = wetenschapper_identity.wetenschapper_id;
                    var wetenschapper_projecten = db.toegewezenen.Where(i => i.wetenschapper_id == wetenschapper_id1);
                    foreach (var project in wetenschapper_projecten)
                    {
                        var assigned_project = db.projecten.Where(i => i.project_id == project.project_id).First().naam.ToString();
                        lbx_wetnschapper_to_project.Items.Add(assigned_project);
                    }
                    db.SaveChanges();
                }
            }
        }
        private void delete_wetenschapper_to_project_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new ShopContext())
            {
                if(lbx_project_to_wetenschapper.SelectedItem == null)
                {
                    MessageBox.Show("er zijn geen wetenschappers toegewezen aan dit project");
                }
                else
                {
                    var selected_wetenschapper = lbx_project_to_wetenschapper.SelectedItem.ToString();
                    var selected_project = cmb_naam_project.SelectedItem.ToString();
                    var selected_project_id = db.projecten.Where(p => p.naam == selected_project).First().project_id;
                    var selected_wetenschapper_id = db.wetenschappers.Where(w => w.naam == selected_wetenschapper).First().wetenschapper_id;

                    var toegewezen_wetenschappers = db.toegewezenen.Where(w => w.wetenschapper_id == selected_project_id && w.project_id == selected_project_id);
                    foreach (var wetenschapper in toegewezen_wetenschappers)
                    {
                        db.Remove((toegewezenen)wetenschapper);
                    }
                    db.SaveChanges();
                    lbx_project_to_wetenschapper.Items.Clear();
                    var project_identity = db.projecten.Where(p => p.naam == cmb_naam_project.SelectedItem.ToString()).First();
                    var project_id1 = project_identity.project_id;
                    var projecten_wetenschapper = db.toegewezenen.Where(i => i.project_id == project_id1);
                    foreach (var wetenschapper in projecten_wetenschapper)
                    {
                        var assigned_wetenschapper = db.wetenschappers.Where(i => i.wetenschapper_id == wetenschapper.wetenschapper_id).First().naam.ToString();
                        lbx_project_to_wetenschapper.Items.Add(assigned_wetenschapper);
                    }
                    db.SaveChanges();
                }
            }

        }
        private void cmb_naam_wetenschapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbx_wetnschapper_to_project.Items.Clear();

            using (var db = new ShopContext())
            {
                var wetenschapper_identity = db.wetenschappers.Where(w => w.naam == cmb_naam_wetenschapper.SelectedItem.ToString()).First();
                var wetenschapper_id1 = wetenschapper_identity.wetenschapper_id;
                var wetenschapper_projecten = db.toegewezenen.Where(i => i.wetenschapper_id == wetenschapper_id1);
                foreach (var project in wetenschapper_projecten)
                {
                    var assignedproject = db.projecten.Where(i => i.project_id == project.project_id).First().naam.ToString();
                    lbx_wetnschapper_to_project.Items.Add(assignedproject);
                }
            }
        }

        private void cmb_naam_project_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbx_project_to_wetenschapper.Items.Clear();
            using(var db = new ShopContext())
            {
                var project_identity = db.projecten.Where(p => p.naam == cmb_naam_project.SelectedItem.ToString()).First();
                var project_id1 = project_identity.project_id;
                var projecten_wetenschapper = db.toegewezenen.Where(i => i.project_id == project_id1);
                foreach(var wetenschapper in projecten_wetenschapper)
                {
                    var assigned_wetenschapper = db.wetenschappers.Where(i => i.wetenschapper_id == wetenschapper.wetenschapper_id).First().naam.ToString();
                    lbx_project_to_wetenschapper.Items.Add(assigned_wetenschapper);
                }
            }
        }

        private void cmb_kiezen_naam_wetenschapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmb_kiezen_naam_project_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void toewijzen_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ShopContext())
            {
                if (cmb_kiezen_naam_wetenschapper.Text == "" || cmb_kiezen_naam_project.Text == "")
                {
                    MessageBox.Show("gelieven een wetenschapper EN een project te kiezen");
                }
                else
                {
                    var w = db.wetenschappers;
                    var id = w.Where(w => w.naam == cmb_kiezen_naam_wetenschapper.Text);
                    var id1 = id.Select(i => i.wetenschapper_id).First();
                    //project id
                    var p = db.projecten;
                    var p_id = p.Where(p => p.naam == cmb_kiezen_naam_project.Text);
                    var p_id1 = p_id.Select(i => i.project_id).First();

                    var toegewezenen_projecten = db.toegewezenen.ToList();
                    bool bestaat_al = toegewezenen.Exists(w => w.wetenschapper_id == id1 && w.project_id == p_id1);
                    //toevoegen toegewezenen
                    if (bestaat_al)
                    {
                        MessageBox.Show("heeft het project al.");
                    }
                    else
                    {
                        db.Add(new toegewezenen { wetenschapper_id = id1, project_id = p_id1 });
                        db.SaveChanges();
                    }
                }

                if (cmb_naam_wetenschapper.SelectedIndex != -1)
                {
                    lbx_wetnschapper_to_project.Items.Clear();
                    var wetenschapper_identity = db.wetenschappers.Where(w => w.naam == cmb_naam_wetenschapper.SelectedItem.ToString()).First();
                    var wetenschapper_id1 = wetenschapper_identity.wetenschapper_id;
                    var wetenschapper_projecten = db.toegewezenen.Where(i => i.wetenschapper_id == wetenschapper_id1);
                    foreach (var project in wetenschapper_projecten)
                    {
                        var assignedproject = db.projecten.Where(i => i.project_id == project.project_id).First().naam.ToString();
                        if (!lbx_wetnschapper_to_project.Items.Contains(assignedproject))
                        lbx_wetnschapper_to_project.Items.Add(assignedproject);
                    }
                }
                if (cmb_naam_project.SelectedIndex != -1)
                {
                    lbx_project_to_wetenschapper.Items.Clear();
                    var project_identity = db.projecten.Where(p => p.naam == cmb_naam_project.SelectedItem.ToString()).First();
                    var project_id1 = project_identity.project_id;
                    var projecten_wetenschapper = db.toegewezenen.Where(i => i.project_id == project_id1);
                    foreach (var wetenschapper in projecten_wetenschapper)
                    {
                        var assigned_wetenschapper = db.wetenschappers.Where(i => i.wetenschapper_id == wetenschapper.wetenschapper_id).First().naam.ToString();
                        if(!lbx_project_to_wetenschapper.Items.Contains(assigned_wetenschapper))
                        lbx_project_to_wetenschapper.Items.Add(assigned_wetenschapper);
                    }
                }

                cmb_kiezen_naam_wetenschapper.SelectedIndex = -1;
                cmb_kiezen_naam_project.SelectedIndex = -1;
            }
        }

    }
}
