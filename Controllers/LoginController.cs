using Microsoft.VisualBasic.ApplicationServices;
using Quiniegol.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Controllers
{

    /// <summary>
    /// Controller para la clase publica Login
    /// </summary>
    public class LoginController
    {
        public UsuarioController UsuarioController { get; set; }
        public LoginController(UsuarioController usuarioController)
        {  
            this.UsuarioController = usuarioController;
            this.UsuarioController.Load();   ///necesitamos conectar esto con el repositorio de usuarios
        }

        /// <summary>
        /// validacion de credenciales para login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if login is successful, otherwiser False</returns>
        public bool Login(string username, string password)
        { 
            foreach (var element in UsuarioController, ) ///referencia al valor que agreguemos en linea 18 ()
            {
                if ((element.Username == user || element.Email == user) && element.Password == password)
                { 
                return true;
                }

            }

            return false;

         }

        public bool Register(string name, string username, string password, string email)
        {
            var newUser = new username (name, username, password, email, "0");
            return this.UsuarioController.SaveUser(newUser);
        }

    }
}
