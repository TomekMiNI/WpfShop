using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.ComponentModel;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;

namespace WPF1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public enum Category
    {
        Food = 0,
        Electronics = 1,
        Clothes = 2
    }
    public partial class MainWindow : Window
    {
        private ObservableCollection<AboutProduct> prolist = null;
        private ObservableCollection<AboutProduct> comprarlist = null;
        private ObservableCollection<AboutProduct> searchlist = null;
        public MainWindow()
        {
            InitializeComponent();
            prolist = new ObservableCollection<AboutProduct>();
             comprarlist= new ObservableCollection<AboutProduct>();
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
            comprarlist.Add(new AboutProduct() { Name = "hej", Description = "bej", Kategoria = Category.Clothes, Price = 2 });
      
            datagrid1.ItemsSource = prolist;
            listbox2.ItemsSource = comprarlist;
            combobox1.ItemsSource = Enum.GetValues(typeof(Category));
            combobox1.SelectedIndex = 0;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is simple shop manager application","About",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NoMainWindow window = new NoMainWindow();
            window.Owner = this;
            window.ShowDialog();
            if (comprarlist==null)
                comprarlist = new ObservableCollection<AboutProduct>();

        }
       

        private void addproducts(object sender, RoutedEventArgs e)
        {
            listbox1.ItemsSource = prolist;
            prolist.Add(new AboutProduct() { Name = "Chrzaszcz", Description = "Brzmi w trzcinie", Kategoria = Category.Food, Price = 2.22 });
            prolist.Add(new AboutProduct() { Name = "Bozydar", Description = "Bozy dar", Kategoria = Category.Clothes, Price = 12 });
            prolist.Add(new AboutProduct() { Name = "Buty", Description = "Bez sznurówek", Kategoria = Category.Clothes, Price = 149.9 });
            prolist.Add(new AboutProduct() { Name = "Pralka", Description = "Szuszy", Kategoria = Category.Electronics, Price = 50 });
        }

        private void clearproducts(object sender, RoutedEventArgs e)
        {
            prolist.Clear();
        }

        private void saveit(object sender, RoutedEventArgs e)
        {

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML document (*.xml)|*.xml";
            save.FileName = "Product";
            if(save.ShowDialog()== true)
            {
                XDocument xdoc = new XDocument(new XElement("Products",
                    from ap in prolist
                    select new XElement("Product",
                          new XAttribute("Name", ap.Name),
                            new XAttribute("Description", ap.Description),
                            new XAttribute("Price", ap.Price.ToString()),
                             new XAttribute("Category", ap.Kategoria.ToString()))));
                xdoc.Save(save.FileName);      
            }
               
            
        }
        private void loadit(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "XML document (*.xml)|*.xml";
            if (open.ShowDialog() == true)
            {
                XDocument xml = XDocument.Load(open.FileName);
                var query = from ap in xml.Descendants("Product")
                            select new AboutProduct()
                            {
                                Name = (string)ap.Attribute("Name"),
                                Description = (string)ap.Attribute("Description"),
                                Price = (double)ap.Attribute("Price"),
                                Kategoria = 0
                            };
                foreach (var ap in query)
                    prolist.Add(new AboutProduct() { Name = ap.Name, Description = ap.Description, Kategoria = Category.Food, Price = ap.Price });
            }
        }

        private void name_Unchecked(object sender, RoutedEventArgs e)
        {
            namebox.IsEnabled = false;
        }

        private void name_Checked(object sender, RoutedEventArgs e)
        {
            namebox.IsEnabled = true;
        }
        private void price_Unchecked(object sender, RoutedEventArgs e)
        {
            priceboxmax.IsEnabled = false;
            priceboxmin.IsEnabled = false;
        }

        private void price_Checked(object sender, RoutedEventArgs e)
        {
            priceboxmax.IsEnabled = true;
            priceboxmin.IsEnabled = true;
        }

        private void cat_Checked(object sender, RoutedEventArgs e)
        {
            combobox1.IsEnabled = true;
        }

        private void cat_Unchecked(object sender, RoutedEventArgs e)
        {
            combobox1.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(searchlist!=null)
            searchlist.Clear();
            if (namecheck.IsChecked == false && pricecheck.IsChecked == false && catcheck.IsChecked == false)
                MessageBox.Show("You had to choose at least one property to search!", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {

                if (namecheck.IsChecked== true)
                {
                    if(namebox.Text=="")
                    {
                        listbox1.ItemsSource = prolist;
                        return;
                    }
                    foreach (AboutProduct ap in prolist)
                        if (ap.Name == namebox.Text)
                        {
                            
                            if (searchlist == null)
                                searchlist = new ObservableCollection<AboutProduct>();
                            if (!searchlist.Contains(ap))
                                searchlist.Add(ap);
                        }
                }
                if(pricecheck.IsChecked== true)
                {
                    double min = 0;
                    double max = 0;
                    if (Double.TryParse(priceboxmin.Text, out min)==false || Double.TryParse(priceboxmax.Text, out max)==false || min < 0 || max < 0)
                    {
                        MessageBox.Show("Invalid format of price", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                        foreach (AboutProduct ap in prolist)
                        if (ap.Price > min && ap.Price < max)
                        {
                            if (searchlist == null)
                                searchlist = new ObservableCollection<AboutProduct>();
                            if (!searchlist.Contains(ap))
                                searchlist.Add(ap);
                        }
                }
                if(catcheck.IsChecked== true)
                {
                    foreach (AboutProduct ap in prolist)
                        if (ap.Kategoria == (Category)Enum.Parse(typeof(Category),combobox1.Text))
                        {
                            if (searchlist == null)
                                searchlist = new ObservableCollection<AboutProduct>();
                            if (!searchlist.Contains(ap))
                                searchlist.Add(ap);
                        }
                }

            }
            listbox1.ItemsSource = searchlist;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            listbox1.ItemsSource = prolist;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listbox1.ItemsSource = prolist;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checkout completed. Total price:", "Checkout");
        }
    }
    public class AboutProduct 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Category Kategoria { get; set; }

    }
    public class ConvertPrice : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is double))
                return null;
            string PLN = "Price: " +((double)value).ToString("F2");
            PLN += "zl";
            return PLN;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertCategory : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Category))
                return null;
            string rodzaj = "Category: ";
            switch ((Category)value)
            {
                case Category.Food:
                    rodzaj += "Food";
                    break;
                case Category.Electronics:
                    rodzaj += "Electronics";
                    break;
                case Category.Clothes:
                    rodzaj += "Clothes";
                    break;
            }
            return rodzaj;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
