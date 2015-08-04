using CorridaDePesso.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace CorridaDePesso.Email
{
    public class NotificaPorEmail : SendGridMailer
    {

        public static bool NotificarNecessidadeDeManutencao(string to, Corrida manutencao)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();

            html = html.Replace("{{mensagem}}", " Identificamos que o item " + manutencao.Titulo + " do veiculo " + manutencao.Regras + " " +
                                                "necessita de manutenção por ter atingido a sua quilometragem.");
            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Um Item de manuteção precisa ser trocado";
            message.IsBodyHtml = true;

            return send(message);

        }

        public static bool NotificarValidade(string to, string mensagem)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();

            html = html.Replace("{{mensagem}}", mensagem);

            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Um Aviso do Corrida de Peso";
            message.IsBodyHtml = true;

            return send(message);

        }


        public static bool NotificarContato(string to, string mensagem)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();
            html = html.Replace("{{mensagem}}", mensagem);

            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Solicitação de Contato Via Portal GestoMotors.Net";
            message.IsBodyHtml = true;

            return send(message);

        }

        public static string ImagePath
        {

            get
            {
                return "http://Corrida de Peso.azurewebsites.net/images/logo.png";
            }

        }

        public static bool NotificarNovoCadastro(string to, string NovaSenha, string Usuario)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();
            var mensagem = " você acaba de criar um pista de competição! Agora é só convidar seus amigos para começar a corrida" + "<p><b>Usuário:</b> " + Usuario + "</p>" + "<p><b>Senha: </b>" + NovaSenha + "  </p>";
            html = html.Replace("{{mensagem}}", mensagem);
            html = html.Replace("{{Link_Logo}}", ImagePath);

            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Seu Acesso ao Sistema";
            message.IsBodyHtml = true;

            return send(message);
        }

        public static bool NotificarMudancaSenha(string to, string NovaSenha)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplateRecuperaSenha();
            html = html.Replace("{{novasenha}}", NovaSenha);
            html = html.Replace("{{Link_Logo}}", ImagePath);

            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Sua senha foi resetada";
            message.IsBodyHtml = true;

            return send(message);

        }

        public static bool NotificarErro(Exception e, Dictionary<String, String> informacoesAdicionais = null)
        {
            String titulo = "[Gestor Motors][erro] Falha no sistema: ";

            StringBuilder mensagemHTML = new StringBuilder("Falha detectada no sistema Gestor Motors, descrita abaixo. Por favor, verificar.<br/><br/>");
            mensagemHTML.Append(DadosException(e));

            if (e.InnerException != null)
            {
                mensagemHTML.Append("Possui uma InnerException:");
                mensagemHTML.Append(DadosException(e.InnerException));
            }

            if (informacoesAdicionais != null)
            {
                mensagemHTML.Append("*** Informações adicionais *** <br/>");
                foreach (string info in informacoesAdicionais.Keys)
                    mensagemHTML.Append(info + ": " + informacoesAdicionais[info] + "<br/>");
            }

            MailMessage message = new MailMessage();

            message.To.Add(ConfigurationManager.AppSettings["ContatoEmailErros"]);
            message.Body = mensagemHTML.ToString();
            message.Subject = titulo;
            message.IsBodyHtml = true;

            return send(message);

        }

        private static string DadosException(Exception e)
        {
            StringBuilder dados = new StringBuilder();
            dados.Append("Type: ----<br/><b>" + e.GetType().Name + "</b><br/><br/>");
            dados.Append("Message: ----<br/><b>" + e.Message + "</b><br/><br/>");
            dados.Append("Source: ----<br/>" + e.Source + "<br/><br/>");
            dados.Append("StackTrace: ----<br/>" + e.StackTrace + "<br/><br/>");
            dados.Append("TargetSite: ----<br/>" + e.TargetSite + "<br/><br/>");
            return dados.ToString();
        }

        private static string HtmlTemplate()
        {

            return @"
            <table style='margin: 0px  auto; color: #666; line-height: 160%; font-size: 18px; font-weight: normal;' cellpadding='0' cellspacing='0' width='600' border='0'>
               <tbody>
                  <tr>
                     <td colspan='3' style='text-align: center;'> <img src={{Link_Logo}}></td>
                  </tr>
                  <tr>
                     <td colspan='3' style='font-family: Georgia; color: #444;'>
                        <h1 style='font-size: 50px; text-align: center; color: #000; line-height: 120%; margin: 10px  0; font-weight: normal; text-align: center;'>Aviso do Corrida de Peso</h1>
                        <p>Olá, {{mensagem}}</p>
                        <p><b><i>O sistema Corrida de Peso</i></b> - É um sistema em nuvens para o gerenciamento de frota de veículos. Ele possibilita ao usuário ter controle sobre os abastecimento, 
                            despesas, manutenções preventivas, trocas de óleos, licenciamento anual, controle de habilitação de motoristas, controle de saídas e retorno de veículos e 
                            vários outros serviços realizados no veículo. 
                            As informações sobre seu veículo são apresentadas através de relatórios e gráficos, tendo as informações de média de Km/litro, gastos mensais, médias por dia, 
                            combustíveis utilizados, entre outras.
            Com a utilização desta ferramenta, você terá em mãos o controle financeiro do seu veículo, facilitando sua escolha sobre a melhor opção para abastecimentos.
                        </p>
                     </td>
                  </tr>
                  <tr>
                     <td style='font-family: Georgia; text-align: center; padding: 0px  0px  40px  0px;' valign='top'>
                        <p style='font-size: 12px text-align: center;;'>Para maiores informações
                           Para maiores informações acesse nosso site: <a href='http://Corrida de Peso.net'>http://Corrida de Peso.net</a>
                        </p>
                     </td>
                  </tr>
                  <tr>
                     <td colspan='3' style='background-color: #ffffff; text-align: center; border-top: 1px  solid  #ccc; padding: 20px; font-size: 11px; line-height: 100%; font-family: Arial;'>
                        <p>Corrida de Peso é um produto da <a href='http://tecsoft.info/'>http://tecsoft.info/</a></p>
                        <p> </p>
                     </td>
                  </tr>
               </tbody>
            </table>";

        }

        private static string HtmlTemplateRecuperaSenha()
        {

            return @"
            <table style='margin: 0px  auto; color: #666; line-height: 160%; font-size: 18px; font-weight: normal;' cellpadding='0' cellspacing='0' width='600' border='0'>
               <tbody>
                  <tr>
                     <td colspan='3' style=' text-align: center;'> <img src={{Link_Logo}}></td>
                  </tr>
                  <tr>
                     <td colspan='3' style='font-family: Georgia; color: #444;'>
                        <h1 style='font-size: 50px; text-align: center; color: #000; line-height: 120%; margin: 10px  0; font-weight: normal; text-align: center;'>Aviso do Corrida de Peso</h1>
                        <p><b>Olá, Sua senha foi resetada com sucesso, sua senha agora é:</b> {{novasenha}}</p>
                        <p>
                        Você está recebendo este e-mail porque sua senha foi resetada,
                        por favor acesse o nosso site para realizar o login: <a href='http://Corrida de Peso.net'>http://Corrida de Peso.net</a>
                        </p>
                        <p> </p>
                        <p><b><i>O sistema Corrida de Peso</i></b> - É um sistema em nuvens para o gerenciamento de frota de veículos. Ele possibilita ao usuário ter controle sobre os abastecimento, 
                            despesas, manutenções preventivas, trocas de óleos, licenciamento anual, controle de habilitação de motoristas, controle de saídas e retorno de veículos e 
                            vários outros serviços realizados no veículo. 
                            As informações sobre seu veículo são apresentadas através de relatórios e gráficos, tendo as informações de média de Km/litro, gastos mensais, médias por dia, 
                            combustíveis utilizados, entre outras.
                            Com a utilização desta ferramenta, você terá em mãos o controle financeiro do seu veículo, facilitando sua escolha sobre a melhor opção para abastecimentos.
                        </p>
                     </td>
                  </tr>
                  <tr>
                     <td style='font-family: Georgia; padding: 0px  0px  40px  0px;' valign='top'>
                        <p style='font-size: 12px;'>Corrida de Peso é um produto da <a href='http://tecsoft.info/'>http://tecsoft.info/</a></p>
                     </td>
                  </tr>
               </tbody>
            </table>";
        }

    }
}