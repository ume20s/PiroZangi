using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PiroZangi
{
    public partial class MainPage : ContentPage
    {
        private int counter;
        private bool running;

        public MainPage()
        {
            running = false;

            InitializeComponent();

            g11.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g12.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g13.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g21.Source = ImageSource.FromResource("PiroZangi.image.zangi.png"); ;
            g22.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g23.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g31.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g32.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;
            g33.Source = ImageSource.FromResource("PiroZangi.image.plain.png"); ;

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if (running == false)
                {
                    return true;
                }
                counter--;
                countBtn.Text = (counter/100).ToString() + "." + (counter%100).ToString("00");
                if(counter == 0)
                {
                    countBtn.Text = "も う １ 回 ？";
                    running = false;
                }
                return true;
            });
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            if (running == true)
            {
                return;
            }
            counter = 300;
            running = true;
        }
    }
}
