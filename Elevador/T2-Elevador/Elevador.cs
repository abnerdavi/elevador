using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace T2_Elevador
{
    class Elevador
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int andar;
        List<int> lista_and_ext;
        List<int> lista_and_int;
        string status;
        bool em_andamento;
        bool espera_botao_int;
        bool botao_emergencia;

        public delegate void update_painel_delegate(object sender, EventArgs args);
        public event update_painel_delegate update_painel_event;

        public Elevador()
        {
            log.Info("ELEVADOR: instanciada com sucesso");
            andar = 0;
            lista_and_ext = new List<int>();
            lista_and_int = new List<int>();
            status = "Parado";
            em_andamento = false;
            espera_botao_int = false;
            botao_emergencia = false;
        }

        public int get_andar_atual { get => andar; }
        public string get_status { get => status; }
        public bool Emergencia { get => botao_emergencia; }
        public int cont_andar_ext{ get => lista_and_ext.Count; }
        public string lst_andar_ext { get => string.Join(", ", lista_and_ext); }
        public string lst_andar_int { get => string.Join(", ", lista_and_int); }
        public string get_proc_andar_ext { get => lista_and_ext.Count > 0 ? $"{lista_and_ext[0]}" : "-"; }
        public string get_proc_andar_int { get => lista_and_int.Count > 0 ? $"{lista_and_int[0]}" : "-"; }
        public bool espera_btn_int { get => espera_botao_int; }

        public void update_painel() => update_painel_event?.Invoke(this, EventArgs.Empty);

        private void executa_chamada(List<int> lista_andar, int andar_desejado, string sentido = null)
        {
            bool veri_andar_lista = lista_andar.Contains(andar_desejado);
            string painel = sentido == null ? "interno" : "externo";

            if (!veri_andar_lista)
            {
                if (lista_andar.Count > 0 && em_andamento)
                {
                    bool se_sobe = lista_andar[0] > andar;
                    bool vai_subir = andar_desejado > andar;
                    bool comando_ativo = sentido == null || sentido == "sobe";
                    bool stop_subida = se_sobe && vai_subir && andar_desejado < lista_andar[0] && comando_ativo;
                    bool stop_descida = !se_sobe && !vai_subir && andar_desejado > lista_andar[0] && !comando_ativo;

                    if (stop_subida || stop_descida)
                    {
                        int aux = lista_andar[0];
                        lista_andar[0] = andar_desejado;
                        lista_andar.Add(aux);
                        log.Info($"painel_{painel}: andar {andar_desejado} no inicio fila");
                    }
                    else
                    {
                        lista_andar.Add(andar_desejado);
                        log.Info($"painel_{painel}: andar {andar_desejado} no final fila");
                    }
                }
                else
                {
                    lista_andar.Add(andar_desejado);
                    log.Info($"painel_{painel}: andar {andar_desejado} adicionado no final fila");
                }
                update_painel();
            }
        }

        private async Task<bool> move_elevador(List<int> lst_andares)
        {
            try
            {
                log.Info($"Movimentacao do elevador iniciada...");
                em_andamento = true;
                bool status_cabine = lst_andares[0] > andar;
                status = status_cabine ? "Subindo..." : "Descendo...";
                update_painel();
                log.Info($"Cabine: {status}");

                while (lst_andares.Count > 0 && lst_andares[0] != andar)
                {
                    await Task.Delay(2000);
                    if (status_cabine) andar++; else andar--;
                    update_painel();
                }
                lst_andares.RemoveAt(0);
                status = "Parado";
                update_painel();
                em_andamento = false;
                log.Info($"Movimentacao do elevador finalizada");
                return true;
            }
            catch (Exception)
            {
                log.Error("Movimentacao do elevador: erro no processamento");
                return false;
            }
        }

        public void chama_and_ext(int andar, string sentido)
        {
            log.Info($"painel_externo: chamando {andar}-{sentido}");
            if (!botao_emergencia)
            {
                executa_chamada(lista_and_ext, andar, sentido);

                if (!em_andamento && !espera_botao_int)
                {
                    exec_and_ext();
                }
            }
        }

        private async void exec_and_ext()
        {
            bool retorno = await move_elevador(lista_and_ext);
            if (retorno)
            {
                espera_botao_int = true;
                update_painel();
            }
        }

        private void ver_lst_ext()
        {
            log.Info("Verificando se ainda existem chamadas externas..."); 

            if (lista_and_ext.Count > 0)
            {
                exec_and_ext();
            }
        }

        public void chama_and_int(int andar)
        {
            log.Info($"painel_interno: chamado {andar} andar");

            if (espera_botao_int && !botao_emergencia)
            {
                executa_chamada(lista_and_int, andar);

                if (!em_andamento)
                {
                    exec_and_int();
                }
            }
        }

        private async void exec_and_int()
        {
            bool retorno = await move_elevador(lista_and_int);

            if (retorno)
            {
                await Task.Delay(2000);
                ver_lst_int();
            }
        }

        private void ver_lst_int()
        {
            log.Info("Verificando se ainda existem chamadas internas...");
            if (lista_and_int.Count > 0)
            {
                exec_and_int();
                return;
            }
            espera_botao_int = false;
            update_painel();
            ver_lst_ext();
        }

        public void chama_emergencia()
        {
            if (botao_emergencia)
            {
                botao_emergencia = false;
                status = "Parado";
                update_painel();

                log.Info($"EMERGENCIA: finalizada.");

                return;
            }

            botao_emergencia = true;
            status = "EMERGENCIA";

            lista_and_ext.Clear();
            lista_and_int.Clear();

            update_painel();

            log.Info($"EMERGENCIA: iniciada");
        }

        public void limpa_fila_andar_ext()
        {
            lista_and_ext.Clear();
        }
    }

}