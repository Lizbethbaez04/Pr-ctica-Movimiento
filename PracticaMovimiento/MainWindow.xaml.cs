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
//Librerias para multiprocesamiento
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;

        //enum es un tipo de dato para definir opciones
        enum EstadoJuego { Gameplay, Gameover};
        EstadoJuego estadoActual = EstadoJuego.Gameplay;
        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            //1. Establecer instrucciones
            ThreadStart threadStart = new ThreadStart(actualizar);
            //2. Inicializar el Thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            //3. Ejecutar el Thread
            threadMoverEnemigos.Start();
        }

        void actualizar()
        {
            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    if(estadoActual == EstadoJuego.Gameplay)
                    {
                        double leftCarroActual = Canvas.GetLeft(imgCarro);
                        Canvas.SetLeft(imgCarro, leftCarroActual - (200 * deltaTime.TotalSeconds));
                        if (Canvas.GetLeft(imgCarro) <= -100)
                        {
                            Canvas.SetLeft(imgCarro, 800);
                        }

                        //Intersección en X
                        double xCarro = Canvas.GetLeft(imgCarro);
                        double xPanda = Canvas.GetLeft(imgPanda);
                        if (xPanda + imgPanda.Width >= xCarro && xPanda <= xCarro + imgCarro.Width)
                        {
                            lblInterseccionX.Text = "SI HAY INTERSECCION EN X!";
                        }
                        else
                        {
                            lblInterseccionX.Text = "No hay intersección en X";
                        }

                        //Interseccion en Y
                        double yCarro = Canvas.GetTop(imgCarro);
                        double yPanda = Canvas.GetTop(imgPanda);
                        if (yPanda + imgPanda.Height >= yCarro && yPanda <= yCarro + imgCarro.Height)
                        {
                            lblInterseccionY.Text = "SI HAY INTERSECCION EN Y!";
                        }
                        else
                        {
                            lblInterseccionY.Text = "No hay intersección en Y";
                        }
                        if (xPanda + imgPanda.Width >= xCarro && xPanda <= xCarro + imgCarro.Width && yPanda + imgPanda.Height >= yCarro && yPanda <= yCarro + imgCarro.Height)
                        {
                            lblColision.Text = "HAY COLISION!";
                            estadoActual = EstadoJuego.Gameover;
                            miCanvas.Visibility = Visibility.Collapsed;
                            canvasGameOver.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblColision.Text = "No hay colision";                           
                        }
                    }
                    else if(estadoActual == EstadoJuego.Gameover)
                    {

                    }
                    
                    tiempoAnterior = tiempoActual;
                });
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(estadoActual == EstadoJuego.Gameplay)
            {
                if (e.Key == Key.Up)
                {
                    //asi es como se mueve la imagen si aprieta el boton "arriba"
                    double topPandaActual = Canvas.GetTop(imgPanda);
                    Canvas.SetTop(imgPanda, topPandaActual - 15);
                }
                if (e.Key == Key.Left)
                {
                    double leftPandaActual = Canvas.GetLeft(imgPanda);
                    Canvas.SetLeft(imgPanda, leftPandaActual - 15);
                }
                if (e.Key == Key.Right)
                {
                    double leftPandaActual = Canvas.GetLeft(imgPanda);
                    Canvas.SetLeft(imgPanda, leftPandaActual + 15);
                }
                if (e.Key == Key.Down)
                {
                    double topPandaActual = Canvas.GetTop(imgPanda);
                    Canvas.SetTop(imgPanda, topPandaActual + 15);
                }
            }
            
        }
    }
}
