using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T2_Elevador
{
    class Simulador
    {
        private bool active;
        private readonly Random random;
        private Elevador _elevator;

        public delegate void pr_btn_dlgt(int random, string direction);
        public event pr_btn_dlgt pr_btn;

        public Simulador(Elevador elevator)
        {
            Elevador.log.Info("SIMULADOR: instanciada com sucesso");
            active = false;
            random = new Random();
            _elevator = elevator;
        }

        public bool Active { get => active; set => active = value; }

        // Gera chamadas de andares
        public async void exec_pressiona_btn()
        {
            while (true)
            {
                if (active && _elevator.cont_andar_ext < 8)
                {
                    int random = this.random.Next(0, 8);
                    string sentido = this.random.Next(0, 1) > 0 ? "sobe" : "desce";

                    Elevador.log.Info($"simulador: pressionar botao andar {random} sentido {sentido}");

                    pr_btn?.Invoke(random, sentido);

                    await Task.Delay(2000);
                }
            }
        }

        // Com referência: https://stackoverflow.com/questions/3419159/how-to-get-all-child-controls-of-a-windows-forms-form-of-a-specific-type-button 
        //Buscando os botoes disponiveis para serem clicados
        public static IEnumerable<Control> GetAllControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }
    }
    
}
