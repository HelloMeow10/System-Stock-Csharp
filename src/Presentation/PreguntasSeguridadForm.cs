using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Presentation.Controles;
using Presentation.Theme;

namespace Presentation
{
    public partial class PreguntasSeguridadForm : Form
    {
        private readonly ISecurityQuestionService _securityQuestionService;
        private string _username = string.Empty;
        private List<PreguntaSeguridadDto> _preguntas = new List<PreguntaSeguridadDto>();
        private List<ComboBox> _comboBoxes = new List<ComboBox>();
        private List<RoundedTextBox> _textBoxes = new List<RoundedTextBox>();

        public PreguntasSeguridadForm(ISecurityQuestionService securityQuestionService)
        {
            InitializeComponent();
            _securityQuestionService = securityQuestionService;
            btnGuardar.Click += BtnGuardar_Click;
        }

        public void Initialize(string username)
        {
            _username = username;
        }

        private void PreguntasSeguridadForm_Load(object sender, EventArgs e)
        {
            try
            {
                _preguntas = _securityQuestionService.GetPreguntasSeguridad();
                var politica = _securityQuestionService.GetPoliticaSeguridad();
                int cantidadPreguntas = politica?.CantPreguntas ?? 3; // Default a 3

                for (int i = 0; i < cantidadPreguntas; i++)
                {
                    var panel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true,
                        Margin = new Padding(3, 3, 3, 10) // Add some margin
                    };

                    var label = new Label
                    {
                        Text = $"Pregunta {i + 1}:",
                        Width = 80,
                        Anchor = AnchorStyles.Left,
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                        ForeColor = ThemeColors.TextPrimary
                    };

                    var comboBox = new ComboBox
                    {
                        DataSource = new BindingSource(_preguntas, null),
                        DisplayMember = "Pregunta",
                        ValueMember = "IdPregunta",
                        Width = 300,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        BackColor = ThemeColors.Surface,
                        ForeColor = ThemeColors.TextPrimary,
                        FlatStyle = FlatStyle.Flat
                    };
                    _comboBoxes.Add(comboBox);

                    var textBox = new RoundedTextBox
                    {
                        Width = 300,
                        Margin = new Padding(10, 0, 0, 0)
                    };
                    _textBoxes.Add(textBox);

                    var answerLabel = new Label
                    {
                        Text = "Respuesta:",
                        Width = 80,
                        Anchor = AnchorStyles.Left,
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                        Margin = new Padding(20, 0, 0, 0),
                        ForeColor = ThemeColors.TextPrimary
                    };

                    panel.Controls.Add(label);
                    panel.Controls.Add(comboBox);
                    panel.Controls.Add(answerLabel);
                    panel.Controls.Add(textBox);

                    flowLayoutPanel.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las preguntas de seguridad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            var respuestas = new Dictionary<int, string>();
            var selectedQuestions = new HashSet<int>();

            for (int i = 0; i < _comboBoxes.Count; i++)
            {
                var comboBox = _comboBoxes[i];
                var textBox = _textBoxes[i];

                if (comboBox.SelectedValue == null)
                {
                    MessageBox.Show($"Por favor, seleccione una pregunta para la fila {i + 1}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int selectedQuestionId = (int)comboBox.SelectedValue;

                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show($"Por favor, ingrese una respuesta para la pregunta seleccionada en la fila {i + 1}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!selectedQuestions.Add(selectedQuestionId))
                {
                    MessageBox.Show("No puede seleccionar la misma pregunta más de una vez.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                respuestas.Add(selectedQuestionId, textBox.Text);
            }

            try
            {
                await _securityQuestionService.GuardarRespuestasSeguridadAsync(_username, respuestas);
                MessageBox.Show("Respuestas de seguridad guardadas exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar las respuestas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
            // This method can be used for custom drawing if needed
            // Currently, it does not perform any custom drawing
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
