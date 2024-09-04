using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGV_Device
{
    public class CSVDataReader
    {
        #region CONSTRUCTOR 

        public CSVDataReader()
        {
                
        }

        #endregion

        #region METHODS

        public List<LGV_Dto> GetLGVData(string fileName)
        {
            try
            {
                using (var reader = new StreamReader(fileName))
                {                 
                    var data=new List<LGV_Dto>();
                    int counter = 0;
                    while (!reader.EndOfStream)
                    {  
                        

                        var line = reader.ReadLine();
                        if (counter>0)
                        {
                            // Split the data line into an array of values
                            var values = line?.Split(',');
                            if (values != null)
                            {
                                LGV_Dto dto = new LGV_Dto();
                                //qui aggiungo i dati
                                dto.ID = Convert.ToInt32(values[0]);
                                dto.Date_Time = DateTime.Now;
                                dto.CoordenadaX = Convert.ToDouble(values[1]);
                                dto.CoordenadaY = Convert.ToDouble(values[2]);
                                dto.IDSegmento = Convert.ToDouble(values[3]);
                                dto.IDPunto = Convert.ToDouble(values[4]);
                                dto.TareaAsginada = Convert.ToInt32(values[5])==1?true:false;
                                dto.TargetPointId = Convert.ToDouble(values[6]);
                                dto.TipoDeOperacion = Convert.ToInt32(values[7]);
                                dto.AlturaHorquillas = Convert.ToInt32(values[8]);
                                dto.LGVCargado = Convert.ToInt32(values[9]) == 1 ? true : false;
                                dto.LGVEnMovimiento = Convert.ToInt32(values[10]) == 1 ? true : false;
                                dto.LGVBloqueadoPorTrafico = Convert.ToInt32(values[11]) == 1 ? true : false;
                                dto.NivelDeBateria = Convert.ToInt32(values[12]);
                                dto.NivelDeNavegacion = Convert.ToInt32(values[13]);
                                dto.AlarmasByte1 = Convert.ToInt32(values[14]);
                                dto.AlarmasByte2 = Convert.ToInt32(values[15]);
                                dto.LGVEnAutomatico = Convert.ToInt32(values[16]) == 1 ? true : false;
                                dto.LGVIntroducido = Convert.ToInt32(values[17]) == 1 ? true : false;
                                dto.LGVCargandoBateria = Convert.ToInt32(values[18]) == 1 ? true : false;
                                data.Add(dto);
                            }
                            //counter++;
                        }
                        counter++;
                    }
                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        #endregion
    }
}
