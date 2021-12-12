using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T2_Elevador
{
    public partial class tela1 : Form
    {
        private Elevador elevator;
        private Simulador simu;
        Task exec_modo_aut;
        public tela1()
        {
            InitializeComponent();
            this.elevator = new Elevador();
            this.elevator.update_painel_event += update_painel;
            this.simu = new Simulador(elevator);
            this.simu.pr_btn += press_btn;
            this.exec_modo_aut = new Task(simu.exec_pressiona_btn);
            Elevador.log.Info("Aplicacao em execucao...");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbx_status_elevador.Text = "Parado";
            tbx_status_andar.Text = "0";
            update_painel(this, System.EventArgs.Empty);
            exec_modo_aut.Start();
        }

        private void update_painel(object sender, System.EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate () {
                this.tbx_status_elevador.Text = elevator.get_status;
                this.tbx_status_andar.Text = elevator.get_andar_atual.ToString();
                update_color_btn_interno();
                this.tbx_fila_andar_ext.Text = $"Fila externa: {elevator.lst_andar_ext}";
                this.tbx_fila_andar_int.Text = $"Fila interna: {elevator.lst_andar_int}";
                this.tbx_and_ext_pro.Text = $"Chamada externa: {elevator.get_proc_andar_ext} andar";
                this.tbx_and_int_pro.Text = $"Chamada interna: {elevator.get_proc_andar_int} andar";
                this.tbx_int_sel_andar.BackColor = elevator.espera_btn_int ? System.Drawing.Color.Green : System.Drawing.Color.Gray;
            });
        }
        private void update_color_btn_interno()
        {
            switch(elevator.get_andar_atual.ToString())
            {
                case "0":
                    bt_int_andar0.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                       break;
                case "1":
                    bt_int_andar1.BackColor = System.Drawing.Color.Green;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "2":
                    bt_int_andar2.BackColor = System.Drawing.Color.Green;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "3":
                    bt_int_andar3.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "4":
                    bt_int_andar4.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "5":
                    bt_int_andar5.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "6":
                    bt_int_andar6.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    bt_int_andar7.UseVisualStyleBackColor = true;
                    break;
                case "7":
                    bt_int_andar7.BackColor = System.Drawing.Color.Green;
                    bt_int_andar1.UseVisualStyleBackColor = true;
                    bt_int_andar2.UseVisualStyleBackColor = true;
                    bt_int_andar3.UseVisualStyleBackColor = true;
                    bt_int_andar4.UseVisualStyleBackColor = true;
                    bt_int_andar5.UseVisualStyleBackColor = true;
                    bt_int_andar6.UseVisualStyleBackColor = true;
                    bt_int_andar0.UseVisualStyleBackColor = true;
                    break;
            }
            
        }
        private void press_btn(int andar, string sentido)
        {
            this.Invoke((MethodInvoker)delegate () {
                string btn_nome = $"btn_ext_{andar}andar_{sentido}";

                foreach (Button btn_da_vez in Simulador.GetAllControls(this, typeof(Button)))
                {
                    if (btn_da_vez.Name == btn_nome)
                    {
                        btn_da_vez.PerformClick();
                        Elevador.log.Info($"simulador: chamando andar {andar} sentido {sentido}");
                        return;
                    }
                }
                Elevador.log.Info($"simulador: andar {andar} sentido {sentido} nao existe");
            });
        }

        private void bt_int_emergencia_Click(object sender, EventArgs e)
        {
            elevator.chama_emergencia();
        }

        private void bt_int_andar1_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(1);
        }

        private void bt_int_andar0_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(0);
        }

        private void bt_int_andar3_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(3);
        }

        private void bt_int_andar2_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(2);
        }

        private void bt_int_andar5_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(5);
        }

        private void bt_int_andar4_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(4);
        }

        private void bt_int_andar6_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(6);
        }

        private void bt_int_andar7_Click(object sender, EventArgs e)
        {
            elevator.chama_and_int(7);
        }

        private void bt_ext_terreo_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(0, "sobe");
        }

        private void btn_ext_1andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(1, "sobe");
        }

        private void btn_ext_1andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(0, "desce");
        }

        private void btn_ext_2andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(2, "sobe");
        }

        private void btn_ext_2andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(2, "desce");
        }

        private void btn_ext_3andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(3, "sobe");

        }

        private void btn_ext_3andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(3, "desce");

        }

        private void btn_ext_4andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(4, "sobe");

        }

        private void btn_ext_4andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(4, "desce");

        }

        private void btn_ext_5andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(5, "sobe");

        }

        private void btn_ext_5andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(5, "desce");

        }

        private void btn_ext_6andar_sobe_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(6, "sobe");

        }

        private void btn_ext_6andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(6, "desce");

        }

        private void btn_ext_7andar_desce_Click(object sender, EventArgs e)
        {
            elevator.chama_and_ext(7, "desce");

        }

        private void chckbx_manual_CheckedChanged(object sender, EventArgs e)
        {
            if (chckbx_manual.Checked)
            {
                Elevador.log.Info("SIMULADOR: desativado modo automatico");
                simu.Active = false;
                elevator.limpa_fila_andar_ext();
            }
        }

        private void chckbx_automatico_CheckedChanged(object sender, EventArgs e)
        {
            if (chckbx_automatico.Checked)
            {
                Elevador.log.Info("SIMULADOR: ligado modo automatico");
                simu.Active = true;
            }
        }
    }
}
