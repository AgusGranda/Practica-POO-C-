using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace Pokemons
{
    public partial class frmAgregar : Form
    {

        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;


        public frmAgregar()
        {
            InitializeComponent();
        }
        public frmAgregar(Pokemon pokemon)
        {
            this.pokemon = pokemon; 
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            

            try
            {
                if(pokemon  == null)
                    pokemon = new Pokemon();

                pokemon.Numero = int.Parse(tbxNumero.Text);
                pokemon.Nombre = tbxNombre.Text;
                pokemon.Descripcion = tbxDescripcion.Text;
                pokemon.UrlImagen = tbxUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbxTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbxDebilidad.SelectedItem;


                


                if(pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Pokemon Modificado exitosamente");

                }
                else
                { 
                    negocio.agregar(pokemon);
                    MessageBox.Show("Pokemon Agregado exitosamente");
                }


                // guardamos la imagen en local solo cuando traemos una imagen local
                if (archivo != null && !(tbxUrlImagen.Text.Contains("http")))
                {
                    // origen de archivo -- Destino(lo configuramos en el app config para no tenerlo hardcodeado);
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName);
                }
                
                

                Close();
            }
               catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            ElementoNegocio eleNegocio = new ElementoNegocio();

            cbxTipo.DataSource = eleNegocio.listar();
            cbxTipo.ValueMember = "Id";
            cbxTipo.DisplayMember = "Descripcion";


            cbxDebilidad.DataSource = eleNegocio.listar();
            cbxDebilidad.ValueMember = "Id";
            cbxDebilidad.DisplayMember = "Descripcion";


            if (pokemon != null)
            {
                tbxNumero.Text = pokemon.Numero.ToString();
                tbxNombre.Text = pokemon.Nombre;
                tbxDescripcion.Text = pokemon.Descripcion;
                tbxUrlImagen.Text = pokemon.UrlImagen;
                cambiarImagen(pokemon.UrlImagen);
                cbxTipo.SelectedValue = pokemon.Tipo.Id;
                cbxDebilidad.SelectedValue = pokemon.Debilidad.Id;


            }


        }

        private void cambiarImagen(string imagen)
        {
            try
            {
                pbxPreview.Load(imagen);
            }
            catch (Exception)
            {

                pbxPreview.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbxUrlImagen_Leave(object sender, EventArgs e)
        {

            cambiarImagen(tbxUrlImagen.Text);
         
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                tbxUrlImagen.Text= archivo.FileName;
                cambiarImagen(archivo.FileName);
            }

        }
    }
}
