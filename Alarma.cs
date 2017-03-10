using System;

namespace Rellotge
{
    [Serializable()]
    public class Alarma : System.ComponentModel.INotifyPropertyChanged
    {

        private string p_horaAlarma;

        public string horaAlarma
        {
            get { return p_horaAlarma; }
            set
            {
                horaAlarma = value;
                PropertyChanged(this,
                  new System.ComponentModel.PropertyChangedEventArgs("horaAlarma"));
            }
        }

        public Alarma()
        {
        }
    }
}
