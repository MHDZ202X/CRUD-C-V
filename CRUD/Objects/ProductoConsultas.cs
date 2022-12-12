using CRUD.BD;
using CRUD.Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD.Objects
{
    internal class ProductoConsultas
    { 
        private ConexionMySql mConexion;
        private List<Producto> mProductos;

        public ProductoConsultas()
        {
            mConexion = new ConexionMySql();
            mProductos = new List<Producto>();
        }

        public bool agregarProducto(Producto mProducto)
        {
            string INSERT = "INSERT INTO producto (nombre, descripcion, precio)" + " values (@nombre, @descripcion, @precio);";

            MySqlCommand mCommand = new MySqlCommand(INSERT, mConexion.getConexion());
            mCommand.Parameters.Add(new MySqlParameter("@nombre", mProducto.nombre));
            mCommand.Parameters.Add(new MySqlParameter("@descripcion", mProducto.descripcion));
            mCommand.Parameters.Add(new MySqlParameter("@precio", mProducto.precio));
            //mCommand.Parameters.Add(new MySqlParameter("@categoria", mProducto.categoria));

            return mCommand.ExecuteNonQuery() > 0;
        }

        public bool modificarProducto(Producto mProducto)
        {
            string UPDATE = " UPDATE producto " +
                "SET nombre = @nombre, " +
                "descripcion = @descripcion, " +
                "precio = @precio, " +
                //"categoria = @categoria " +
                "WHERE idProducto = @id";

            MySqlCommand mCommand = new MySqlCommand(UPDATE, mConexion.getConexion());
            mCommand.Parameters.Add(new MySqlParameter("@nombre", mProducto.nombre));
            mCommand.Parameters.Add(new MySqlParameter("@descripcion", mProducto.descripcion));
            mCommand.Parameters.Add(new MySqlParameter("@precio", mProducto.precio));
            //mCommand.Parameters.Add(new MySqlParameter("@categoria", mProducto.categoria));

            return mCommand.ExecuteNonQuery() > 0;
        }

        public bool eliminarProducto(Producto mProducto)
        {
            string DELETE = " DELETE FROM producto WHERE idProducto=@id";
            MySqlCommand mCommand = new MySqlCommand(DELETE, mConexion.getConexion());
            mCommand.Parameters.Add(new MySqlParameter("@id", mProducto.id));
            return mCommand.ExecuteNonQuery() > 0;
        }

        public List<Producto> consultarProductos(string filtro)
        {
            string CONSULTA = "SELECT * FROM producto";

            MySqlDataReader mReader = null;
            Producto mProducto;
            try
            {
                if (filtro != "")
                {
                    CONSULTA += " WHERE " +
                        "id LIKE '%" + filtro + "%' OR " +
                        "nombre LIKE '%" + filtro + "%' OR " +
                        "precio LIKE '%" + filtro + "%' OR " +
                        "descripcion LIKE '%" + filtro + "%'OR;";
                }

                MySqlCommand mCommand = new MySqlCommand(CONSULTA);
                mCommand.Connection = mConexion.getConexion();
                mReader = mCommand.ExecuteReader();

                while (mReader.Read())
                {
                    mProducto = new Producto();
                    mProducto.id = mReader.GetInt16("idProducto");
                    mProducto.nombre = mReader.GetString("nombre");
                    mProducto.precio = mReader.GetInt16("precio");
                    mProducto.descripcion = mReader.GetString("descripcion");
                    //mProducto.categoria = (byte[]) mReader.GetValue(4);
                    mProductos.Add(mProducto);
                }
                mReader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mConexion.closeConexion();
            }

            return mProductos;
        }
    }
}
