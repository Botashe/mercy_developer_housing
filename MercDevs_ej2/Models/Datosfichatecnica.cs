using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercDevs_ej2.Models
{
    public partial class Datosfichatecnica
    {
        [Key]
        public int IdDatosFichaTecnica { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        public string? PobservacionesRecomendaciones { get; set; }

        public int? Soinstalado { get; set; }

        public int? SuiteOfficeInstalada { get; set; }

        public int? LectorPdfinstalado { get; set; }

        public int? NavegadorWebInstalado { get; set; }

        [Column("Antivirus Instalado")] // Especifica el nombre exacto de la columna en la base de datos
        public string? AntivirusInstalado { get; set; }

        public int RecepcionEquipoId { get; set; }

        public virtual ICollection<Diagnosticosolucion> Diagnosticosolucions { get; set; } = new List<Diagnosticosolucion>();

        public virtual Recepcionequipo RecepcionEquipo { get; set; } = null!;
    }
}
