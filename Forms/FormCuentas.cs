﻿using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class FormCuentas : Form
    {
        private List<CuentaConfiguracion> cuentas_cargadas;

        public FormCuentas()
        {
            InitializeComponent();
            cuentas_cargadas = new List<CuentaConfiguracion>();

            comboBox_Servidor.SelectedIndex = 0;
            cargar_Cuentas_Lista();
        }

        private void cargar_Cuentas_Lista()
        {
            listViewCuentas.Items.Clear();

            GlobalConfiguracion.get_Lista_Cuentas().ForEach(x =>
            {
                if(!FormPrincipal.get_Paginas_Cuentas_Cargadas().ContainsKey(x.get_Nombre_cuenta()))
                {
                    listViewCuentas.Items.Add(x.get_Nombre_cuenta()).SubItems.AddRange(new string[] { x.get_servidor(), string.IsNullOrEmpty(x.get_nombre_personaje()) ? "Default" : x.get_nombre_personaje() });
                }
            });
        }

        private void boton_Agregar_Cuenta_Click(object sender, EventArgs e)
        {
            if (textBox_Nombre_Cuenta.TextLength != 0 && textBox_Password.TextLength != 0)
            {
                if (GlobalConfiguracion.get_Cuenta(textBox_Nombre_Cuenta.Text) != null)
                {
                    MessageBox.Show("Ya existe una cuenta con el nombre de cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GlobalConfiguracion.agregar_Cuenta(textBox_Nombre_Cuenta.Text, textBox_Password.Text, comboBox_Servidor.SelectedItem.ToString(), textBox_Nombre_Personaje.Text);
                cargar_Cuentas_Lista();

                textBox_Nombre_Cuenta.Clear();
                textBox_Password.Clear();
                textBox_Nombre_Personaje.Clear();

                if (checkBox_Agregar_Retroceder.Checked)
                {
                    tabControlPrincipalCuentas.SelectedIndex = 0;
                }
                GlobalConfiguracion.guardar_Todas_Cuentas();
            }
            else
            {
                tableLayoutPanel6.Controls.OfType<TableLayoutPanel>().ToList().ForEach(panel =>
                {
                    panel.Controls.OfType<TextBox>().ToList().ForEach(textbox =>
                    {
                        textbox.BackColor = string.IsNullOrEmpty(textbox.Text) ? Color.Red : Color.White;
                    });
                });
            }
        }

        private void listViewCuentas_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listViewCuentas.Columns[e.ColumnIndex].Width;
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count > 0 && listViewCuentas.FocusedItem != null)
            {
                foreach (ListViewItem cuenta in listViewCuentas.SelectedItems)
                {
                    GlobalConfiguracion.eliminar_Cuenta(cuenta.Index);
                    cuenta.Remove();
                }
                GlobalConfiguracion.guardar_Todas_Cuentas();
                cargar_Cuentas_Lista();
            }
        }

        private void conectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count > 0 && listViewCuentas.FocusedItem != null)
            {
                foreach(ListViewItem cuenta in listViewCuentas.SelectedItems)
                {
                    cuentas_cargadas.Add(GlobalConfiguracion.get_Lista_Cuentas().FirstOrDefault(f => f.get_Nombre_cuenta() == cuenta.Text));
                }
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public List<CuentaConfiguracion> get_Cuentas_Cargadas()
        {
            return cuentas_cargadas;
        }

        private void listViewCuentas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            conectarToolStripMenuItem.PerformClick();
        }
    }
}
