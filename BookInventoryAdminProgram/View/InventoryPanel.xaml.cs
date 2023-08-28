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

namespace BookInventoryAdminProgram.View
{
    /// <summary>
    /// Interaction logic for InventoryPanel.xaml
    /// </summary>
    public partial class InventoryPanel : UserControl
    {
        public InventoryPanel()
        {
            InitializeComponent();
            List<Book> BookDatabase = new List<Book>();

            BookDatabase.Add(new Book() { ISBN = 5539588773482, Name = "Astounding Breathtaking Joe Rogan: A Quest of Joe Rogan", Authors = "Stephen King, Roald Dahl", SalesDay = 14, SalesMonth = 157, SalesYear = 8900, Genre = "Russian", ReleaseDate = new DateTime(1979, 11, 23) });
            BookDatabase.Add(new Book() { ISBN = 5612722679527, Name = "Stunning Astonishing Football: A Quest of Football", Authors = "Friedrich Nietzsche, Franz Kafka, Leo Tolstoy", SalesDay = 22, SalesMonth = 748, SalesYear = 6887, Genre = "Sci-fi", ReleaseDate = new DateTime(1897, 12, 16) });
            BookDatabase.Add(new Book() { ISBN = 2915188550408, Name = "Astounding Astonishing Monkey: A Quest of Monkey", Authors = "Karl Marx", SalesDay = 88, SalesMonth = 437, SalesYear = 7988, Genre = "Folklore, Survival, Mystery", ReleaseDate = new DateTime(1980, 10, 7) });
            BookDatabase.Add(new Book() { ISBN = 3379963480093, Name = "Extraordinary Spectacular Football: A Voyage of Football", Authors = "Friedrich Nietzsche", SalesDay = 90, SalesMonth = 847, SalesYear = 7544, Genre = "Russian", ReleaseDate = new DateTime(2077, 11, 16) });
            BookDatabase.Add(new Book() { ISBN = 9749074682481, Name = "Stunning Mind-Blowing Trashbin: A Expedition of Trashbin", Authors = "Friedrich Nietzsche, President Obama", SalesDay = 63, SalesMonth = 119, SalesYear = 2513, Genre = "Mystery, Lost World, Historic, Animal Tail", ReleaseDate = new DateTime(1917, 2, 1) });
            BookDatabase.Add(new Book() { ISBN = 3045364066670, Name = "Amazing Incredible Space: A Voyage of Space", Authors = "Leo Tolstoy", SalesDay = 34, SalesMonth = 468, SalesYear = 5627, Genre = "Polish, Animal Tail, Sword-and-Sorcery", ReleaseDate = new DateTime(2066, 2, 21) });
            BookDatabase.Add(new Book() { ISBN = 7963959611755, Name = "Stupendous Staggering Sports: A Journey of Sports", Authors = "George Orwell", SalesDay = 91, SalesMonth = 671, SalesYear = 3094, Genre = "Pirate, Russian, Fantasy", ReleaseDate = new DateTime(2133, 1, 29) });
            BookDatabase.Add(new Book() { ISBN = 9754879459652, Name = "Stupendous Staggering Godly being: A Journey of Godly being", Authors = "President Obama", SalesDay = 54, SalesMonth = 106, SalesYear = 2791, Genre = "Sci-fi", ReleaseDate = new DateTime(2040, 8, 17) });
            BookDatabase.Add(new Book() { ISBN = 4736058982691, Name = "Stupendous Breathtaking Trashbin: A Voyage of Trashbin", Authors = "Elon Musk, Sigmund Freud", SalesDay = 33, SalesMonth = 566, SalesYear = 8920, Genre = "Folklore", ReleaseDate = new DateTime(2009, 7, 10) });
            BookDatabase.Add(new Book() { ISBN = 7818009028186, Name = "Remarkable Staggering Joe Rogan: A Voyage of Joe Rogan", Authors = "President Obama, Friedrich Nietzsche", SalesDay = 99, SalesMonth = 687, SalesYear = 2157, Genre = "Parable, Russian", ReleaseDate = new DateTime(2104, 3, 5) });
            BookDatabase.Add(new Book() { ISBN = 7433893257791, Name = "Extraordinary Unbelievable Hockey: A Quest of Hockey", Authors = "Karl Marx", SalesDay = 82, SalesMonth = 257, SalesYear = 6798, Genre = "Fantasy, Animal Tail, Sci-fi, Legal", ReleaseDate = new DateTime(2043, 12, 19) });
            BookDatabase.Add(new Book() { ISBN = 4480229447671, Name = "Extraordinary Unbelievable Trashbin: A Journey of Trashbin", Authors = "Roald Dahl, Franz Kafka", SalesDay = 57, SalesMonth = 660, SalesYear = 5658, Genre = "Parable, Romance Suspense", ReleaseDate = new DateTime(1861, 4, 8) });
            BookDatabase.Add(new Book() { ISBN = 1348464890662, Name = "Extraordinary Mind-Blowing Space: A Journey of Space", Authors = "George Orwell, Joe Rogan, Friedrich Nietzsche", SalesDay = 93, SalesMonth = 458, SalesYear = 9629, Genre = "Animal Tail, Erotic, Christian, Sword-and-Sorcery", ReleaseDate = new DateTime(2127, 7, 2) });
            BookDatabase.Add(new Book() { ISBN = 9796493912608, Name = "Stunning Unbelievable Karol: A Pilgrimage of Karol", Authors = "Socrates, Dr, Seuss, Carl Jung", SalesDay = 95, SalesMonth = 634, SalesYear = 9702, Genre = "Christian, Sword-and-Sorcery, Sci-fi, Political", ReleaseDate = new DateTime(1935, 7, 18) });
            BookDatabase.Add(new Book() { ISBN = 9894177410327, Name = "Stunning Mind-Blowing Trashbin: A Expedition of Trashbin", Authors = "Plato", SalesDay = 65, SalesMonth = 901, SalesYear = 1016, Genre = "Folklore", ReleaseDate = new DateTime(1826, 11, 3) });
            BookDatabase.Add(new Book() { ISBN = 1132794757951, Name = "Remarkable Incredible Trashbin: A Journey of Trashbin", Authors = "Roald Dahl", SalesDay = 53, SalesMonth = 309, SalesYear = 1673, Genre = "Parable, Christian, Political, Pirate", ReleaseDate = new DateTime(2148, 9, 20) });
            BookDatabase.Add(new Book() { ISBN = 5454469909581, Name = "Stunning Incredible Hockey: A Journey of Hockey", Authors = "Dr, Seuss, Sigmund Freud, C.S. Lewis", SalesDay = 87, SalesMonth = 700, SalesYear = 9598, Genre = "Urban Legend", ReleaseDate = new DateTime(1827, 4, 16) });
            /*BookDatabase.Add(new Book() { ISBN = 4790207598515, Name = "Stunning Staggering Spaceship: A Journey of Spaceship", Authors = "Karl Marx, Franz Kafka, Sigmund Freud", SalesDay = 77, SalesMonth = 284, SalesYear = 5643, Genre = "Conspiracy, Western, Survival, Fantasy", ReleaseDate = new DateTime(1847, 2, 12) });
            BookDatabase.Add(new Book() { ISBN = 3195174399015, Name = "Stunning Incredible Spaceship: A Quest of Spaceship", Authors = "Leo Tolstoy", SalesDay = 86, SalesMonth = 706, SalesYear = 7548, Genre = "Western, Sword-and-Sorcery", ReleaseDate = new DateTime(1819, 12, 16) });
            BookDatabase.Add(new Book() { ISBN = 5941459418147, Name = "Stunning Astonishing Joe Rogan: A Journey of Joe Rogan", Authors = "Dr, Seuss", SalesDay = 20, SalesMonth = 508, SalesYear = 3995, Genre = "Legal", ReleaseDate = new DateTime(1821, 7, 26) });
            BookDatabase.Add(new Book() { ISBN = 3252617820108, Name = "Remarkable Mind-Blowing Karol: A Voyage of Karol", Authors = "President Obama, C.S. Lewis", SalesDay = 87, SalesMonth = 540, SalesYear = 8537, Genre = "Pirate, Political", ReleaseDate = new DateTime(1969, 10, 18) });
            BookDatabase.Add(new Book() { ISBN = 9755270246201, Name = "Stupendous Incredulous Sports: A Journey of Sports", Authors = "C.S. Lewis", SalesDay = 30, SalesMonth = 328, SalesYear = 6454, Genre = "Lost World, Mystery, Polish", ReleaseDate = new DateTime(1914, 9, 17) });
            BookDatabase.Add(new Book() { ISBN = 5471800601205, Name = "Striking Unbelievable Joe Rogan: A Pilgrimage of Joe Rogan", Authors = "Franz Kafka, Roald Dahl", SalesDay = 97, SalesMonth = 505, SalesYear = 4949, Genre = "Parable, Fantasy, Folklore", ReleaseDate = new DateTime(1901, 7, 9) });
            BookDatabase.Add(new Book() { ISBN = 6064058015873, Name = "Stupendous Mind-Blowing Trashbin: A Quest of Trashbin", Authors = "Friedrich Nietzsche", SalesDay = 47, SalesMonth = 834, SalesYear = 8090, Genre = "Lost World", ReleaseDate = new DateTime(2041, 7, 7) });
            BookDatabase.Add(new Book() { ISBN = 7290584887670, Name = "Remarkable Spectacular Karol: A Voyage of Karol", Authors = "Roald Dahl", SalesDay = 25, SalesMonth = 228, SalesYear = 9431, Genre = "Survival, Florida, Russian", ReleaseDate = new DateTime(2124, 10, 8) });
            BookDatabase.Add(new Book() { ISBN = 7659020945323, Name = "Amazing Incredulous Joe Rogan: A Quest of Joe Rogan", Authors = "Plato, Elon Musk", SalesDay = 70, SalesMonth = 221, SalesYear = 7159, Genre = "Survival, Sci-fi, Classic", ReleaseDate = new DateTime(2046, 8, 11) });
            BookDatabase.Add(new Book() { ISBN = 3627491399358, Name = "Remarkable Staggering Messi: A Expedition of Messi", Authors = "Elon Musk", SalesDay = 36, SalesMonth = 296, SalesYear = 4975, Genre = "Pirate, Political, Legal", ReleaseDate = new DateTime(2064, 8, 16) });
            BookDatabase.Add(new Book() { ISBN = 9142667897294, Name = "Striking Unbelievable Joe Rogan: A Journey of Joe Rogan", Authors = "Dr, Seuss, Plato, Karl Marx", SalesDay = 47, SalesMonth = 598, SalesYear = 2131, Genre = "Philosophy, Classic", ReleaseDate = new DateTime(1862, 10, 20) });
            BookDatabase.Add(new Book() { ISBN = 5603816268134, Name = "Astounding Spectacular Godly being: A Journey of Godly being", Authors = "Joe Rogan, George Orwell, Carl Jung", SalesDay = 33, SalesMonth = 433, SalesYear = 5419, Genre = "Polish", ReleaseDate = new DateTime(1819, 3, 1) });
            BookDatabase.Add(new Book() { ISBN = 6735148774024, Name = "Stupendous Incredible Sports: A Quest of Sports", Authors = "Tom Clancy", SalesDay = 10, SalesMonth = 497, SalesYear = 5962, Genre = "Sci-fi, Legal", ReleaseDate = new DateTime(1938, 6, 24) });
            BookDatabase.Add(new Book() { ISBN = 2203983762052, Name = "Remarkable Incredible Tolstoy: A Pilgrimage of Tolstoy", Authors = "Dr, Seuss, Plato", SalesDay = 30, SalesMonth = 613, SalesYear = 3396, Genre = "Parable, Folklore, Mystery", ReleaseDate = new DateTime(1822, 7, 29) });
            BookDatabase.Add(new Book() { ISBN = 8138939631968, Name = "Astounding Spectacular Hockey: A Voyage of Hockey", Authors = "C.S. Lewis", SalesDay = 42, SalesMonth = 957, SalesYear = 4190, Genre = "Urban Legend", ReleaseDate = new DateTime(1922, 2, 28) });
            BookDatabase.Add(new Book() { ISBN = 1808689669053, Name = "Stunning Astonishing Monkey: A Voyage of Monkey", Authors = "Elon Musk, Karl Marx", SalesDay = 64, SalesMonth = 251, SalesYear = 4644, Genre = "Survival", ReleaseDate = new DateTime(2129, 10, 23) });
            BookDatabase.Add(new Book() { ISBN = 3642615764659, Name = "Amazing Incredible Elon Musk: A Quest of Elon Musk", Authors = "Elon Musk", SalesDay = 32, SalesMonth = 319, SalesYear = 2232, Genre = "Pirate, Sci-fi, Political", ReleaseDate = new DateTime(2022, 5, 1) });
            BookDatabase.Add(new Book() { ISBN = 9436617421202, Name = "Striking Incredible Hockey: A Voyage of Hockey", Authors = "Tom Clancy, C.S. Lewis, Leo Tolstoy", SalesDay = 27, SalesMonth = 432, SalesYear = 3438, Genre = "Erotic", ReleaseDate = new DateTime(1926, 1, 12) });
            BookDatabase.Add(new Book() { ISBN = 4160945974002, Name = "Amazing Incredulous Trashbin: A Quest of Trashbin", Authors = "Sigmund Freud, Dr, Seuss", SalesDay = 52, SalesMonth = 549, SalesYear = 3225, Genre = "Russian", ReleaseDate = new DateTime(1911, 6, 9) });
            BookDatabase.Add(new Book() { ISBN = 3460673096303, Name = "Stunning Spectacular Monkey: A Adventure of Monkey", Authors = "Joe Rogan, C.S. Lewis", SalesDay = 76, SalesMonth = 658, SalesYear = 1263, Genre = "Fantasy, Philosophy, Lost World", ReleaseDate = new DateTime(2030, 1, 13) });
            BookDatabase.Add(new Book() { ISBN = 4883755873923, Name = "Striking Spectacular Andre: A Pilgrimage of Andre", Authors = "George Orwell, President Obama", SalesDay = 60, SalesMonth = 578, SalesYear = 8755, Genre = "Russian, Sword-and-Sorcery, Legal", ReleaseDate = new DateTime(1805, 8, 6) });
            BookDatabase.Add(new Book() { ISBN = 4134823483967, Name = "Stunning Incredulous Space: A Voyage of Space", Authors = "Franz Kafka", SalesDay = 34, SalesMonth = 532, SalesYear = 8492, Genre = "Survival, Pirate", ReleaseDate = new DateTime(2111, 11, 20) });
            BookDatabase.Add(new Book() { ISBN = 3411501786243, Name = "Stupendous Astonishing Trashbin: A Pilgrimage of Trashbin", Authors = "Elon Musk, Roald Dahl, George Orwell", SalesDay = 28, SalesMonth = 421, SalesYear = 7100, Genre = "Historic, Florida, Folklore, Urban Legend", ReleaseDate = new DateTime(2020, 1, 6) });
            BookDatabase.Add(new Book() { ISBN = 7781764845722, Name = "Amazing Incredible Messi: A Journey of Messi", Authors = "President Obama", SalesDay = 10, SalesMonth = 827, SalesYear = 1342, Genre = "Metaparody, Conspiracy", ReleaseDate = new DateTime(1922, 4, 8) });
            BookDatabase.Add(new Book() { ISBN = 6712137837808, Name = "Fascinating Spectacular Joe Rogan: A Quest of Joe Rogan", Authors = "Tom Clancy", SalesDay = 22, SalesMonth = 581, SalesYear = 4298, Genre = "Sci-fi", ReleaseDate = new DateTime(1839, 9, 20) });
            BookDatabase.Add(new Book() { ISBN = 3876473903026, Name = "Striking Incredible Monkey: A Journey of Monkey", Authors = "Socrates, Carl Jung, Tom Clancy", SalesDay = 48, SalesMonth = 980, SalesYear = 3527, Genre = "Political, Romance Suspense, Fantasy, Florida", ReleaseDate = new DateTime(2072, 4, 24) });
            BookDatabase.Add(new Book() { ISBN = 4602379855330, Name = "Stunning Spectacular Messi: A Journey of Messi", Authors = "Tom Clancy, Franz Kafka, Dr, Seuss", SalesDay = 71, SalesMonth = 415, SalesYear = 6310, Genre = "Lost World, Survival, Historic", ReleaseDate = new DateTime(2083, 9, 14) });
            BookDatabase.Add(new Book() { ISBN = 8183575682535, Name = "Stupendous Astonishing Programming: A Voyage of Programming", Authors = "Plato, Elon Musk", SalesDay = 57, SalesMonth = 980, SalesYear = 9616, Genre = "Erotic, Metaparody, Conspiracy, Legal", ReleaseDate = new DateTime(2139, 9, 8) });
            BookDatabase.Add(new Book() { ISBN = 3395099042899, Name = "Stupendous Mind-Blowing Godly being: A Journey of Godly being", Authors = "Plato", SalesDay = 12, SalesMonth = 226, SalesYear = 6353, Genre = "Fantasy, Parable", ReleaseDate = new DateTime(1984, 11, 10) });
            BookDatabase.Add(new Book() { ISBN = 8913974612772, Name = "Striking Mind-Blowing Football: A Pilgrimage of Football", Authors = "Leo Tolstoy, Franz Kafka", SalesDay = 50, SalesMonth = 297, SalesYear = 8131, Genre = "Political, Urban Legend, Russian, Survival", ReleaseDate = new DateTime(1865, 6, 14) });
            BookDatabase.Add(new Book() { ISBN = 9733677422094, Name = "Astounding Incredulous Trashbin: A Journey of Trashbin", Authors = "Sigmund Freud", SalesDay = 93, SalesMonth = 554, SalesYear = 8866, Genre = "Urban Legend, Mystery, Romance Suspense, Parable", ReleaseDate = new DateTime(1914, 10, 11) });
            BookDatabase.Add(new Book() { ISBN = 6571585689395, Name = "Stunning Staggering Andre: A Journey of Andre", Authors = "Karl Marx, Stephen King", SalesDay = 16, SalesMonth = 942, SalesYear = 4607, Genre = "Political", ReleaseDate = new DateTime(2087, 9, 11) });
            BookDatabase.Add(new Book() { ISBN = 9374941089474, Name = "Remarkable Incredible Space: A Quest of Space", Authors = "Friedrich Nietzsche, Socrates, President Obama", SalesDay = 19, SalesMonth = 564, SalesYear = 5842, Genre = "Animal Tail, Romance Suspense, Historic", ReleaseDate = new DateTime(2076, 3, 8) });
            BookDatabase.Add(new Book() { ISBN = 4101269429016, Name = "Remarkable Astonishing Sports: A Pilgrimage of Sports", Authors = "Tom Clancy, Friedrich Nietzsche", SalesDay = 74, SalesMonth = 370, SalesYear = 6046, Genre = "Urban Legend, Animal Tail", ReleaseDate = new DateTime(1816, 10, 7) });
            BookDatabase.Add(new Book() { ISBN = 1009315911557, Name = "Stupendous Spectacular Joe Rogan: A Quest of Joe Rogan", Authors = "Karl Marx, Socrates, Elon Musk", SalesDay = 43, SalesMonth = 195, SalesYear = 5355, Genre = "Polish, Survival", ReleaseDate = new DateTime(2098, 7, 26) });
            BookDatabase.Add(new Book() { ISBN = 7406982451912, Name = "Striking Incredible Space: A Pilgrimage of Space", Authors = "President Obama", SalesDay = 26, SalesMonth = 349, SalesYear = 5979, Genre = "Survival, Legal, Christian", ReleaseDate = new DateTime(2085, 9, 14) });
            BookDatabase.Add(new Book() { ISBN = 1008120022194, Name = "Extraordinary Breathtaking Trashbin: A Voyage of Trashbin", Authors = "Karl Marx, Plato, Tom Clancy", SalesDay = 73, SalesMonth = 428, SalesYear = 8656, Genre = "Mystery, Russian, Fantasy, Classic", ReleaseDate = new DateTime(1829, 11, 28) });
            BookDatabase.Add(new Book() { ISBN = 1488481528815, Name = "Amazing Spectacular Sports: A Journey of Sports", Authors = "Karl Marx", SalesDay = 84, SalesMonth = 348, SalesYear = 1553, Genre = "Erotic, Christian, Fantasy, Sci-fi", ReleaseDate = new DateTime(2011, 3, 27) });
            BookDatabase.Add(new Book() { ISBN = 2061852710389, Name = "Amazing Spectacular Sports: A Journey of Sports", Authors = "Carl Jung, Roald Dahl, Sigmund Freud", SalesDay = 73, SalesMonth = 474, SalesYear = 4458, Genre = "Legal, Romance Suspense", ReleaseDate = new DateTime(1813, 2, 13) });
            BookDatabase.Add(new Book() { ISBN = 1486883451169, Name = "Stunning Incredulous Tolstoy: A Quest of Tolstoy", Authors = "Leo Tolstoy, Franz Kafka, Joe Rogan", SalesDay = 11, SalesMonth = 217, SalesYear = 3096, Genre = "Political, Pirate, Animal Tail", ReleaseDate = new DateTime(2103, 12, 21) });
            BookDatabase.Add(new Book() { ISBN = 6676007373624, Name = "Fascinating Spectacular Tolstoy: A Voyage of Tolstoy", Authors = "Franz Kafka", SalesDay = 84, SalesMonth = 138, SalesYear = 6914, Genre = "Parable, Western, Animal Tail", ReleaseDate = new DateTime(1985, 5, 7) });
            BookDatabase.Add(new Book() { ISBN = 8288369548697, Name = "Remarkable Spectacular Godly being: A Journey of Godly being", Authors = "Socrates, Karl Marx", SalesDay = 20, SalesMonth = 646, SalesYear = 1226, Genre = "Historic, Folklore, Fantasy, Classic", ReleaseDate = new DateTime(2118, 6, 25) });
            BookDatabase.Add(new Book() { ISBN = 8135233524722, Name = "Extraordinary Mind-Blowing Elon Musk: A Expedition of Elon Musk", Authors = "C.S. Lewis, Carl Jung", SalesDay = 95, SalesMonth = 725, SalesYear = 9631, Genre = "Metaparody, Lost World, Survival", ReleaseDate = new DateTime(1947, 10, 13) });*/

            BookstoreInventoryDatagrid.ItemsSource = BookDatabase;
        }
    }

    public class Book
    {
        public long ISBN { get; set; }

        public string Name { get; set; }
        public string Authors { get; set; }

        public int SalesDay { get; set; }
        public int SalesMonth { get; set; }
        public int SalesYear { get; set; }

        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }
    }

}
