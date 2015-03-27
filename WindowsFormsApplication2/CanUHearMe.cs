using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{

    public partial class CanUHearMe : Form
    {

        public double WaveLength;
        public Transmitters Trans1;
        public Transmitters Trans2;

        public CanUHearMe()
        {
            InitializeComponent();
        }

        private void DrawPic_Click(object sender, EventArgs e)
        {
            double iXcoord1, iXcoord2, iYcoord1, iYcoord2;
            float dWavelength;
            
            if (
              double.TryParse(T1_X.Text, out iXcoord1) && !(iXcoord1 < 0 || iXcoord1 > 400)
              && double.TryParse(T1_Y.Text, out iYcoord1) && !(iYcoord1 < 0 || iYcoord1 > 400)
              && double.TryParse(T2_X.Text, out iXcoord2) && !(iXcoord2 < 0 || iXcoord2 > 400)
              && double.TryParse(T2_Y.Text, out iYcoord2) && !(iYcoord2 < 0 || iYcoord2 > 400)
              && float.TryParse(wavelength.Text, out dWavelength) && !(dWavelength < 0)
              )
            {

                Console.WriteLine("is:", iXcoord1, iYcoord1);
                Trans1 = new Transmitters(iXcoord1, iYcoord1);
                Trans2 = new Transmitters(iXcoord2, iYcoord2);
                WaveLength = (double)dWavelength;
                paintCanvas(pictureBox1);
                
            }
            else
            {
                MessageBox.Show("Invalid input detected. Please Check inputs.");
  
            }
           
        }

        private void ExitProgram_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void T1_X_TextChanged(object sender, EventArgs e)
        {

        }

        private void T1_Y_TextChanged(object sender, EventArgs e)
        {

        }

        private void T2_X_TextChanged(object sender, EventArgs e)
        {

        }

        private void T2_Y_TextChanged(object sender, EventArgs e)
        {

        }

        private void wavelength_TextChanged(object sender, EventArgs e)
        {

        }

        private void paintCanvas(PictureBox P)
        {
            System.Drawing.Pen RectanglePen = new System.Drawing.Pen(Color.Black, 1);

            int PBwidth = P.Width;
            int PBheight = P.Height;
            

            for (int Xpos = 0; Xpos < PBwidth; Xpos++) //Loops through all X coordinates
                for (int Ypos = 0; Ypos < PBheight; Ypos++) //Loops through all Y coordinates at each X
                {
                    Antenna MyPhone = new Antenna(Xpos, Ypos);  //Constructs a new antenna object at the coordinates

                    //Get the signal strength from the antenna object
                    double SignalStrength = MyPhone.getSignalStrength(Trans1, Trans2,WaveLength);

                    //Convert the signal strength into an RGB color with 100% strength equal to RGB 255,255,255
                    int ColorStrength = System.Convert.ToInt16(255.0 * (SignalStrength / 2.0));

                    
                    if (((Xpos + 3 >= Trans1.Get_X() && Xpos <= Trans1.Get_X()) || (Xpos - 3 <= Trans1.Get_X() && Xpos >= Trans1.Get_X())) 
                        && ((Ypos + 3 >= Trans1.Get_Y() && Ypos <= Trans1.Get_Y()) || (Ypos - 3 <= Trans1.Get_Y() && Ypos >= Trans1.Get_Y()))) 
                        RectanglePen.Color = System.Drawing.Color.FromArgb(255, 0, 0);              //draw a red square at transmitter1's position
                    else if (((Xpos + 3 >= Trans2.Get_X() && Xpos <= Trans2.Get_X()) || (Xpos - 3 <= Trans2.Get_X() && Xpos >= Trans2.Get_X()))
                        && ((Ypos + 3 >= Trans2.Get_Y() && Ypos <= Trans2.Get_Y()) || (Ypos - 3 <= Trans2.Get_Y() && Ypos >= Trans2.Get_Y())))
                        RectanglePen.Color = System.Drawing.Color.FromArgb(0, 0, 255);              //draw a blue square at transmitter2's position
                    else
                        RectanglePen.Color = System.Drawing.Color.FromArgb(ColorStrength, ColorStrength, ColorStrength); //Set the pen color based on the signal strength

                    //Draws the rectangle at the given coordinates
                    P.CreateGraphics().DrawRectangle(RectanglePen, Xpos, PBheight - Ypos, 1, 1);
                }

            RectanglePen.Dispose();  //Disposes the pen object before creating a new one with a new color and a new size
        }
       
      
         


     

    }

    public class Transmitters
    {
        private double x_coord;
        private double y_coord;

        public Transmitters(double a, double b)
        {
            x_coord = a;
            y_coord = b;
        }
        public double Get_X()
        {
            Console.WriteLine("x coord is", x_coord);
            return x_coord;
        }

        public double Get_Y()
        {
            return y_coord;
        }

    }

    public class Antenna
    {
        private double antenna_x;
        private double antenna_y;
        public Transmitters T1;
        public Transmitters T2;
        //double WaveLength;
        public Antenna(double a, double b)
        {
            antenna_x = a;
            antenna_y = b;
        }

        public double getSignalStrength(Transmitters T1, Transmitters T2, double WaveLength)
        {
            double d1 = Math.Sqrt(Math.Pow((Math.Abs(T1.Get_X() - antenna_x)), 2) + Math.Pow((Math.Abs(T1.Get_Y() - antenna_y)), 2));
            double d2 = Math.Sqrt(Math.Pow((Math.Abs(T2.Get_X() - antenna_x)), 2) + Math.Pow((Math.Abs(T2.Get_Y() - antenna_y)), 2));
            return (1 + Math.Sin((Math.PI / 2) + (Math.Abs(d1 - d2) / WaveLength * 2 * Math.PI)));
        }
    }

}