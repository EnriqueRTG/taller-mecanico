using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Taller.Presentacion.Formularios.Administrador
{
    public partial class usuarios_eliminados : Form
    {
        //almacena en memoria el formulario que se le pase por parámetro
        private Form usuarios_Eliminados;
        public usuarios_eliminados(Form eliminados)
        {
            InitializeComponent();
            this.usuarios_Eliminados = eliminados;
        }
    }
}
