using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Presentation.ApiClient;
using Presentation.Exceptions;

namespace Presentation
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            try
            {
                var host = CreateHostBuilder().Build();
                ServiceProvider = host.Services;

                Application.Run(ServiceProvider.GetRequiredService<LoginForm>());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register services from BusinessLogic layer
                    services.AddInfrastructure(context.Configuration);

                    // Register ApiClient
                    services.AddSingleton<ApiClient.ApiClient>();

                    // Register Forms
                    services.AddTransient<LoginForm>();
                    services.AddTransient<AdminForm>();
                    services.AddTransient<UserForm>();
                    services.AddTransient<ProfileForm>();
                    services.AddTransient<CambioContrasenaForm>();
                    services.AddTransient<RecuperarContrasenaForm>();
                    services.AddTransient<PreguntasSeguridadForm>();
                    services.AddTransient<TwoFactorAuthForm>();
                    services.AddTransient<frmError>();
                    services.AddTransient<frmNotification>();
                });

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception? ex)
        {
            if (ex == null) return;

            string errorId = Guid.NewGuid().ToString();
            LogException(ex, errorId);

            try
            {
                string userMessage;
                // For validation and business errors, we can show the actual exception message
                // as it is considered safe and user-friendly.
                if (ex is ValidationException || ex is BusinessLogicException)
                {
                    userMessage = ex.Message;
                }
                else
                {
                    // For all other unexpected exceptions, show a generic message.
                    userMessage = $"An unexpected error occurred. Please contact support and provide the following Error ID: {errorId}";
                }

                var errorForm = ServiceProvider?.GetService<frmError>();
                if (errorForm != null)
                {
                    errorForm.SetErrorDetails(userMessage, errorId, ex);
                    errorForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show(userMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show($"A critical error occurred and the application cannot continue. Error ID: {errorId}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            // Only exit for truly unexpected, critical errors.
            if (!(ex is ValidationException) && !(ex is BusinessLogicException) && !(ex is UILayerException))
            {
                Environment.Exit(1);
            }
        }

        private static void LogException(Exception ex, string errorId)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
                string errorMessage = $"[{DateTime.Now}] - Error ID: {errorId}{Environment.NewLine}" +
                                      $"Exception Type: {ex.GetType().FullName}{Environment.NewLine}" +
                                      $"Message: {ex.Message}{Environment.NewLine}" +
                                      $"StackTrace:{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}" +
                                      "--------------------------------------------------" +
                                      $"{Environment.NewLine}";
                File.AppendAllText(logPath, errorMessage);
            }
            catch
            {
                // If logging fails, there's not much else to do.
            }
        }
    }
}
