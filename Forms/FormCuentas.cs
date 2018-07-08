﻿using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class FormCuentas : Form
    {
        private List<CuentaConfiguracion> cuentas_para_conectar;

        public FormCuentas()
        {
            InitializeComponent();
            cuentas_para_conectar = new List<CuentaConfiguracion>();

            comboBox_Servidor.SelectedIndex = 0;
            cargar_Cuentas_Lista();
        }

        private void cargar_Cuentas_Lista()
        {
            listViewCuentas.Items.Clear();

            GlobalConfiguracion.get_Lista_Cuentas().ForEach(x =>
            {
                listViewCuentas.Items.Add(x.get_Nombre_cuenta()).SubItems.AddRange(new string[] {x.get_servidor(), string.IsNullOrEmpty(x.get_nombre_personaje()) ? "Default" : x.get_nombre_personaje() });
            });
        }

        private void boton_Agregar_Cuenta_Click(object sender, EventArgs e)
        {
            if (textBox_Nombre_Cuenta.TextLength != 0 || textBox_Password.TextLength != 0)
            {
                if (GlobalConfiguracion.get_Cuenta(textBox_Nombre_Cuenta.Text) != null)
                {
                    MessageBox.Show("Ya existe una cuenta con el nombre de cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GlobalConfiguracion.agregar_Cuenta_Guardar(textBox_Nombre_Cuenta.Text, textBox_Password.Text, comboBox_Servidor.SelectedItem.ToString(), textBox_Nombre_Personaje.Text);
                cargar_Cuentas_Lista();

                textBox_Nombre_Cuenta.Clear();
                textBox_Password.Clear();
                textBox_Nombre_Personaje.Clear();

                if (checkBox_Agregar_Retroceder.Checked)
                {
                    tabControlPrincipalCuentas.SelectedIndex = 0;
                }
            }
        }

        private void listViewCuentas_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listViewCuentas.Columns[e.ColumnIndex].Width;
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.FocusedItem != null)
            {
                GlobalConfiguracion.eliminar_Cuenta_Guardar(listViewCuentas.FocusedItem.Index);
                cargar_Cuentas_Lista();
            }
            else
            {
                return;
            }
        }
    }
}
