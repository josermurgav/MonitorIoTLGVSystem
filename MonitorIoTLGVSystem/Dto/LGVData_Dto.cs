using MonitorIoTLGVSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorIoTLGVSystem
{
    public class LGVData_Dto:Dto
    {
        #region CONSTRUCTORS
        public LGVData_Dto() {
            LGVAlarm = new List<LGV_alarm>();
        }
        #endregion

        #region PROPERTIES                
        public double CoordenadaX { get; set; }
        public double CoordenadaY { get; set; }
        public double IDSegmento { get; set; }
        public double IDPunto { get; set; }
        public bool TareaAsginada { get; set; }
        public double TargetPointId { get; set; }
        public int TipoDeOperacion { get; set; }
        public int AlturaHorquillas { get; set; }
        public bool LGVCargado { get; set; }
        public bool LGVEnMovimiento { get; set; }
        public bool LGVBloqueadoPorTrafico { get; set; }
        public int NivelDeBateria { get; set; }
        public int NivelDeNavegacion { get; set; }
        public int AlarmasByte1 { get; set; }
        public int AlarmasByte2 { get; set; }
        public bool LGVEnAutomatico { get; set; }
        public bool LGVIntroducido { get; set; }
        public bool LGVCargandoBateria { get; set; }
        public List<LGV_alarm> LGVAlarm { get; set; }
        #endregion
    }
}
