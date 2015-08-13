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
        public static bool NotificarNovoCorredor(string to, string mensagem)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();
            html = html.Replace("{{mensagem}}", mensagem);

            message.To.Add(to);
            message.Body = html;
            message.Subject = "Corrida de Peso Informa: Solicitação de Inscrição";
            message.IsBodyHtml = true;

            return send(message);

        }


        public static bool NotificarNovoCadastro(string to, string NovaSenha, string Usuario,string link)
        {

            MailMessage message = new MailMessage();

            var html = HtmlTemplate();
            var mensagem = " Você acaba de criar um pista de competição! Agora é só convidar seus amigos para começar a corrida" + "<p><b>Usuário:</b> " + Usuario + "</p>" + "<p><b>Senha: </b>" + NovaSenha + "  </p> <br> Envie este o link da corrida os participantes "+link;
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
                        <p><b><i>O Sistema Corrida de Peso</i></b> - É uma aplicação em nuvens para ajudar e estimular de forma diverita, uma competição saldavel. Ele possibilita aos usuário o acompanhamento a cada pesagem, 
                                                                     garantindo a transparancia para os competidores com graficos de evoluçao, deixando sempre corredor com o foco no resultado.
                                                                     Com a utilização desta ferramenta, você terá em mãos uma facilitador para uma brincadeira divertida e estigante para voce e seus amigos.
                        </p>
                     </td>
                  </tr>
                  <tr>
                     <td style='font-family: Georgia; text-align: center; padding: 0px  0px  40px  0px;' valign='top'>
                        <p style='font-size: 12px text-align: center;;'>Para maiores informações
                           Para maiores informações acesse nosso site: <a href='http://www.corridadepeso.com.br'>http://www.corridadepeso.com.br</a>
                        </p>
                     </td>
                  </tr>
                  <tr>
                     <td colspan='3' style='background-color: #ffffff; text-align: center; border-top: 1px  solid  #ccc; padding: 20px; font-size: 11px; line-height: 100%; font-family: Arial;'>
                        <p>Corrida de Peso é um produto da <a href='#'>http://www.corridadepeso.com.br</a></p>
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

        public static string ImagePath
        {

            get
            {
                return "http://CorridadePeso.azurewebsites.net/images/logo.png";
            }

        }

    }
}