using FreeLancer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeLancer.App_Classes
{
    public class Context
    {
        private static FreeLancerEntities baglanti;

        public static FreeLancerEntities Baglanti
        {
            get
            {
                if (baglanti == null)
                    baglanti = new FreeLancerEntities();
                return baglanti;
            }
            set { baglanti = value; }
        }

    }
}