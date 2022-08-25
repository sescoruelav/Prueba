using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class AsignarRecursoScheme
    {
        [JsonProperty]
        public int IdTarea { get; set; }
        public int IdRecurso { get; set; }
        public string Info { get; set; }
    }
    public class Recurso
    {
        public List<AsignarRecursoScheme> asignarRecursos { get; set; }
    }
}
