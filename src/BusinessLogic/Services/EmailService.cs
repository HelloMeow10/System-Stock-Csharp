// src/BusinessLogic/Services/EmailService.cs
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLogic.Configuration;
using Microsoft.Extensions.Options;

namespace BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                // No lanzar excepción, solo registrar o ignorar si no hay correo.
                // En un sistema real, aquí habría un logging.
                Console.WriteLine("No se proporcionó una dirección de correo para la recuperación de contraseña.");
                return;
            }

            try
            {
                using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.UseSsl
                };

                var from = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                var to = new MailAddress(toEmail);

                using var mailMessage = new MailMessage(from, to)
                {
                    Subject = "Recuperación de Contraseña",
                    IsBodyHtml = true,
                    Body = GetEmailBody(newPassword)
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // En una aplicación real, esto debería ser logueado a un sistema de logging.
                // Por ahora, lanzaremos una excepción para que el desarrollador vea el problema de configuración.
                throw new InvalidOperationException($"Error al enviar el correo. Verifique la configuración SMTP en appsettings.json. Detalles: {ex.Message}", ex);
            }
        }

        private static string GetEmailBody(string newPassword)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h1 style='color: #0056b3;'>Recuperación de Contraseña</h1>
                    <p>Se ha solicitado un reinicio de su contraseña.</p>
                    <p>Su nueva contraseña temporal es:</p>
                    <h2 style='color: #darkorange; border: 1px solid #ddd; padding: 10px; display: inline-block;'>{newPassword}</h2>
                    <p>Por su seguridad, se le pedirá que cambie esta contraseña la próxima vez que inicie sesión.</p>
                    <hr>
                    <p style='font-size: 0.8em; color: #777;'>Si usted no solicitó este cambio, por favor ignore este correo electrónico.</p>
                </div>";
        }

        public async Task Send2faCodeEmailAsync(string toEmail, string code)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                Console.WriteLine("No se proporcionó una dirección de correo para el código 2FA.");
                return;
            }

            try
            {
                using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.UseSsl
                };

                var from = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                var to = new MailAddress(toEmail);

                using var mailMessage = new MailMessage(from, to)
                {
                    Subject = "Código de Autenticación de Dos Factores",
                    IsBodyHtml = true,
                    Body = Get2faEmailBody(code)
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al enviar el correo 2FA. Verifique la configuración SMTP. Detalles: {ex.Message}", ex);
            }
        }

        private static string Get2faEmailBody(string code)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h1 style='color: #0056b3;'>Código de Verificación</h1>
                    <p>Use el siguiente código para completar su inicio de sesión. El código es válido por 5 minutos.</p>
                    <h2 style='color: #darkorange; border: 1px solid #ddd; padding: 10px; display: inline-block;'>{code}</h2>
                    <hr>
                    <p style='font-size: 0.8em; color: #777;'>Si usted no intentó iniciar sesión, por favor ignore este correo electrónico.</p>
                </div>";
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                Console.WriteLine("No se proporcionó una dirección de correo para el correo de bienvenida.");
                return;
            }

            try
            {
                using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.UseSsl
                };

                var from = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                var to = new MailAddress(toEmail);

                using var mailMessage = new MailMessage(from, to)
                {
                    Subject = "¡Bienvenido a Nuestro Sistema!",
                    IsBodyHtml = true,
                    Body = GetWelcomeEmailBody(username, password)
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error but do not throw, so user creation succeeds even if email fails
                // In a real app, you might want to queue this email for retry
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private static string GetWelcomeEmailBody(string username, string password)
        {
            return $@"
        <div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
            <h1 style='color: #0056b3;'>¡Bienvenido!</h1>
            <p>Hola {username},</p>
            <p>¡Le damos la bienvenida a nuestro sistema! Su cuenta ha sido creada exitosamente.</p>
            <p>A continuación, encontrará sus credenciales de acceso. Le recomendamos guardarlas en un lugar seguro.</p>

            <div style='background-color: #f2f2f2; padding: 15px; border-left: 5px solid #0056b3;'>
                <p><strong>Nombre de Usuario:</strong> {username}</p>
                <p><strong>Contraseña Temporal:</strong> <span style='color: #darkorange; font-weight: bold;'>{password}</span></p>
            </div>

            <p>Por su seguridad, se le pedirá que cambie esta contraseña la próxima vez que inicie sesión.</p>

            <p>Si tiene alguna pregunta, no dude en contactarnos.</p>
            <br>
            <p>Saludos cordiales,</p>
            <p><strong>El Equipo de Soporte</strong></p>
        </div>";
        }
    }
}
