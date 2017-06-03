using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.Globalization;

namespace voiceRecognition
{
    public partial class Form1 : Form
    {
        //Variaveis globais
        // variaveis para voz
        static CultureInfo ci = new CultureInfo("pt-BR");// linguagem utilizada
        static SpeechRecognitionEngine reconhecedor; // reconhecedor de voz
        SpeechSynthesizer resposta = new SpeechSynthesizer();// sintetizador de voz

        // Palavras aceitas
        public string[] listaPalavras = { "oi" };

        public Form1()
        {
            InitializeComponent();

            Init();
        }                

        public void Gramatica()
        {
            try
            {
                //reconhecedor = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao integrar lingua escolhida:" + ex.Message);
            }

            // criacao da gramatica simples que o programa vai entender
            // usando um objeto Choices
            var gramatica = new Choices();
            gramatica.Add(listaPalavras); // inclui a gramatica criada

            // cria o construtor gramatical
            // e passa o objeto criado com as palavras
            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            // cria a instancia e carrega a engine de reconhecimento
            // passando a gramatica construida anteriomente
            try
            {
                var g = new Grammar(gb);

                try
                {
                    // carrega o arquivo de gramatica
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);

                    // registra a voz como mecanismo de entrada para o evento de reconhecimento
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;

                    reconhecedor.SetInputToDefaultAudioDevice(); // microfone padrao
                    resposta.SetOutputToDefaultAudioDevice(); // auto falante padrao
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple); // multiplo reconhecimento
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramática: " + ex.Message);
            }
        }

        public void Init()
        {
            resposta.Volume = 100; // controla volume de saida
            resposta.Rate = 3; // velocidade de fala

            Gramatica(); // inicialização da gramatica
        }

        // funcao para reconhecimento de voz
        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;

            if(frase.Equals("oi"))
            {
                resposta.SpeakAsync("Como você está");
            }
            
        }
    }
}
