using FactuLib.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FactuLib.Library
{
    public class LApertura : ListObject
    {
        public LApertura(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ValidaApertura(string idUser)
        {
            var valor = false;
            var apertura = _context.TRegisroApertura.Where(a => a.Estado == true && a.TUser.IdUser.Equals(idUser)).ToList();
            if (apertura.Count()>0)
            {
                valor = false;
            }
            else
            {
                valor= true;
            }
            return valor;
        }

        public float getTotalCajasApertura(String IdUser)
        {
            float montoApertura = 0;
            var apertura = _context.TRegisroApertura.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado == true).ToList();
            if (apertura.Count() > 0)
            {
                montoApertura = apertura.Last().Dinero_Cajas;
            }
            else
            {
                montoApertura = 0;
            }
        
            return montoApertura;
        }

        public float getTotalCuentasApertura(String IdUser)
        {
            float montoApertura = 0;
            var apertura = _context.TRegisroApertura.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado == true).ToList();
            if (apertura.Count()>0)
            {
                montoApertura = apertura.Last().Dinero_Cuentas;
            }
            else
            {
                montoApertura = 0;
            }
            
            return montoApertura;
        }
    }
}
