﻿using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailSender : IEmailSender
    {
        private readonly AmazonSimpleEmailServiceClient _client;

        public EmailSender(IConfiguration configuration)
        {
            var accessKey = configuration["EmailSettings:AccessKey"];
            var secretKey = configuration["EmailSettings:SecretKey"];

            _client = new AmazonSimpleEmailServiceClient(
                accessKey,
                secretKey,
                region: RegionEndpoint.SAEast1);
        }

        public async Task SendConfirmationEmailAsync(string receiver, string code)
        {
            try
            {
                var response = await _client.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
                            ToAddresses = new List<string> { receiver },
                        },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = @$"
                                                <html>
                                                <body>
                                                    <h2>Olá!</h2>
                                                    <p>Você está recebendo este e-mail porque solicitou um código de autenticação.</p>
                                                    <p>Seu código de autenticação é:</p>
                                                    <h3 style='background-color: #f0f0f0; padding: 10px; border-radius: 5px;'> {code} </h3>
                                                    <p>Por favor, utilize este código para concluir o processo de autenticação.</p>
                                                    <p>Obrigado!</p>
                                                    <p>Equipe Caramel!</p>
                                                </body>
                                                </html>
                                            "
                                }
                            },
                            Subject = new Content
                            {
                                Charset = "UTF-8",
                                Data = "Código de Confirmação"
                            }
                        },

                        Source = "contato@projetocaramel.com.br"
                    });
            }
            catch (Exception error)
            {
                throw new BusinessException(error.Message, StatusProcess.SESFailure, HttpStatusCode.InternalServerError);
            }
        }
    }
}
