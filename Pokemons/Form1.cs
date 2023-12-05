using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;

namespace Pokemons
{
    public partial class frmPokemons : Form
    {
        List<Pokemon> pokemons;



        public frmPokemons()
        {
            InitializeComponent();
        }


        private void frmPokemons_Load(object sender, EventArgs e)
        {

            cargar();
            cbxCampo.Items.Add("Numerico");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Descripcion");


        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemons.CurrentRow != null)
            {

                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cambiarImagen(seleccionado.UrlImagen);
            }
        }

        private void cambiarImagen(string imagen)
        {
            try
            {
                pbxPokemons.Load(imagen);
            }
            catch (Exception )
            {

                pbxPokemons.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            pokemons = negocio.listar();
            dgvPokemons.DataSource = pokemons;
            cambiarImagen(pokemons[0].UrlImagen);
            ocultarColumnas();

        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;


            frmAgregar modificar = new frmAgregar(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisica_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminacionLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }


        private void eliminar(bool valor = false)
        {
            
            Pokemon seleccionado;

            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                DialogResult respuesta = MessageBox.Show("Ta seguro?", "Eliminando pobre pokemonsito", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    
                    if(valor)
                        negocio.eliminacionLogica(seleccionado.Id);
                    else
                    negocio.eliminar(seleccionado.Id);


                    cargar();
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            


        }

        private void tbxBusqueda_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> Listafiltrada;

            string busqueda = tbxBusqueda.Text;

            Listafiltrada = pokemons.FindAll(x => x.Nombre.ToUpper().Contains(busqueda.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(busqueda.ToUpper()));


            if (busqueda == "")
            {
                dgvPokemons.DataSource = pokemons;
                ocultarColumnas();
            }
            else
            {
                dgvPokemons.DataSource = null;
                dgvPokemons.DataSource = Listafiltrada;
                ocultarColumnas();
            }
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCampo.SelectedItem.ToString() == "Numerico")
            {
                cbxSubcampo.Items.Clear();
                cbxSubcampo.Items.Add("Mayor que");
                cbxSubcampo.Items.Add("Menor que");
                cbxSubcampo.Items.Add("Igual que");
            }
            else
            {
                cbxSubcampo.Items.Clear();
                cbxSubcampo.Items.Add("Comienza con");
                cbxSubcampo.Items.Add("Termina con");
                cbxSubcampo.Items.Add("Contiene");
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();


            try
            {

                string campo = cbxCampo.SelectedItem.ToString();
                string subcampo = cbxSubcampo.SelectedItem.ToString();
                string filtro = tbxFiltro.Text;

                dgvPokemons.DataSource = negocio.filtrar(campo, subcampo, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
