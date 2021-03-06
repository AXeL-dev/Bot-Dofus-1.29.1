﻿using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;
using System.IO;
using System.Xml.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Inventario
{
    public class ObjetosInventario
    {
        public uint id_inventario { get; private set; }
        public int id_modelo { get; private set; }
        public string nombre { get; private set; }
        public int cantidad { get; set; }
        public short posicion { get; private set; } //byte no puede ser negativo
        public short pods { get; private set; } //algunos objetos pesan mas de 128 por eso no puede ser byte
        public byte tipo { get; private set; }
        public TipoObjetosInventario tipo_inventario { get; private set; }
        private readonly XElement archivo_objeto;

        public ObjetosInventario(string paquete)
        {
            string[] separador = paquete.Split('~');

            id_inventario = Convert.ToUInt32(separador[0], 16);
            id_modelo = Convert.ToInt32(separador[1], 16);
            cantidad = Convert.ToInt32(separador[2], 16);

            if(!string.IsNullOrEmpty(separador[3]))
                posicion = Convert.ToInt16(separador[3], 16);
            else
                posicion = -1;

            FileInfo mapa_archivo = new FileInfo("items/" + id_modelo + ".xml");
            if (mapa_archivo.Exists)
            {
                archivo_objeto = XElement.Load(mapa_archivo.FullName);
                nombre = archivo_objeto.Element("NOMBRE").Value;
                pods = byte.Parse(archivo_objeto.Element("PODS").Value);
                tipo = byte.Parse(archivo_objeto.Element("TIPO").Value);
                tipo_inventario = get_Objetos_Inventario(tipo);

                archivo_objeto = null;
            }
            else
            {
                nombre = "Desconocido";
                tipo_inventario = TipoObjetosInventario.DESCONOCIDO;
            }
            mapa_archivo = null;
        }

        public bool objeto_esta_equipado() => posicion > -1;

        public static TipoObjetosInventario get_Objetos_Inventario(byte tipo)
        {
            switch (tipo)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                    return TipoObjetosInventario.EQUIPAMIENTO;

                case 12:
                case 13:
                    return TipoObjetosInventario.VARIOS;

                case 15:
                case 34:
                case 35:
                case 36:
                case 38:
                case 46:
                case 47:
                case 48:
                case 50:
                case 51:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 65:
                case 68:
                case 96:
                case 98:
                case 100:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 111:
                    return TipoObjetosInventario.RECURSOS;

                case 24:
                    return TipoObjetosInventario.OBJETOS_MISION;

                default:
                    return TipoObjetosInventario.DESCONOCIDO;
            }
        }
    }
}
