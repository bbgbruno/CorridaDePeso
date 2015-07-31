using CorridaDePesso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorridaDePesso.Controllers
{
    public class ApplicationController : Controller
    {

        public ApplicationUser UsuarioSessao()
        {
            return (ApplicationUser)Session["Usuario"];
        }

        public void SalvarUsuarioSessao(ApplicationUser usuarioLogado)
        {
            Session["Usuario"] = usuarioLogado;
        }

        public Boolean UsuarioEstaLogado()
        {
            return Session["Usuario"] != null;
        }

        public void RemoverUsuarioLogado()
        {
           Session["Usuario"] = null;
        }
	}
}