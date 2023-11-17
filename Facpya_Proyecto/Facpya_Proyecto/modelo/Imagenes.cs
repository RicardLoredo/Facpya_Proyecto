using System;
using System.Collections.Generic;
using System.Text;

namespace Facpya_Proyecto.modelo
{
    public class Edificio
    {
        public string Id { get; set; } 
        public string Name { get; set; } 
        public string ImageUrl { get; set; } 
        public string SketchImageUrl { get; set; } 
        public string VideoUrl { get; set; } 

    }
    /*public List<Salon> Salones { get; set; }

public Edificio()
{
    Salones = new List<Salon>(); 
}*/

    /*public class Salon
    {
        public string Nombre { get; set; }
        public string ImagenSalonUrl { get; set; }
        public string ImagenCroquisSalonUrl { get; set; }
        public string VideoLlegadaUrl { get; set; }
    }*/
}
