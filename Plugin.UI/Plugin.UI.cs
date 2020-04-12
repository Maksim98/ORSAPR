using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RodKernelParameters;
using Builder;


namespace Plugin.UI
{
    /// <summary>
    /// Cодержит методы обработки 
    /// взаимодействия с пользователем
    /// </summary>
    public partial class RodKernelParameters : Form
    {
        /// <summary>
        /// Поле хранит все параметры модели
        /// </summary>
        private ModelParameters _modelParameters = new ModelParameters();

        /// <summary>
        /// Хранит словарь соответствий TextBox и параметров моделей
        /// </summary>
        private Dictionary<TextBox, ParametersName> _formElements = new Dictionary<TextBox, ParametersName>();

        public RodKernelParameters()
        {
            //Инициализация формы
            InitializeComponent();
            //Создание списка элементов TextBox существующих на форме
            var elements = new List<(TextBox textBox, ParametersName parameter)>
                  {
                     (HandleRTextBox, ParametersName.HandleRadius),
                     (HandlelLTextBox, ParametersName.HandleLength),
                     (HandleCLTextBox, ParametersName.ConnectionLength),
                     (HandleHLTextBox, ParametersName.GripLength),
                     (LimiterRTextBox, ParametersName.LimiterRadius),
                     (HolderRTextBox, ParametersName.HolderRadius),
                     (HolderLTextBox,ParametersName.HolderLength)
                    };
            //Перебор всех элементов картежа
            foreach (var element in elements)
            {
                //Добавление параметра в словарь элементов TextBox формы
                _formElements.Add(element.textBox, element.parameter);
            }
        }

        /// <summary>
        /// Присваивает параметру значение 
        /// из соответствующего элемента TextBox
        /// при изменении пользователем значения Text
        /// для TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxChanged(object sender, EventArgs e)
        {
            //Преобразуем из object в TextBox
            var textBox = (TextBox)sender;
            //Блок ожидания ошибки
            try
            {
                //Получаем текст из элемента TextBox
                var value = double.Parse(textBox.Text);
                //Определяем имя параметра соответствующего
                //данному TextBox
                var parameterName = _formElements[textBox];
                //Присваиваем значение найденному параметру
                _modelParameters.Parameter(parameterName).Value = value;
                //Окрашиваем поле в зеленый цвет
                textBox.BackColor = Color.LightGreen;
                //При изменении длины места хвата
                if (parameterName == ParametersName.GripLength)
                {
                    //Пересчитать длину гладкой части
                    _modelParameters.CalculationLenghtConnection();
                    //Отобразить значение в TextBox
                    HandleCLTextBox.Text = 
                        _modelParameters.Parameter(ParametersName.ConnectionLength).Value.ToString();
                }
                //При изменении длины рукояти
                if (parameterName == ParametersName.HandleLength)
                {
                    //Пересчитать максимальную длину места хвата
                    _modelParameters.CalculationMaxLenghtGrip();
                    //Пересчитать минимальную дину места хвата
                    _modelParameters.CalculationMinValueLenghtGrip();
                    //Расчитать текущую дину рукояти
                    _modelParameters.CalculationLenghtGrip();
                    //Отобразить значение в TextBox
                    HandleHLTextBox.Text =
                        _modelParameters.Parameter(ParametersName.GripLength).Value.ToString();
                    //Отобразить новый интервал
                    DisplayInterval(ParametersName.GripLength, IntervalHandleHLLabel);
                }
                //При изменении длины гладкой части
                if (parameterName == ParametersName.ConnectionLength)
                {
                    //Пересчитать длину гладкой части
                    _modelParameters.CalculationLenghtGrip();
                    //Отобразить поулченное значение в TextBox
                    HandleHLTextBox.Text =
                        _modelParameters.Parameter(ParametersName.GripLength).Value.ToString();
                }
            }
            //Выполняется в случае выявления ошибки в try
            catch
            {
                //Окрашиваем полу в красный цвет
                textBox.BackColor = Color.Salmon;
            }
        }

        /// <summary>
        /// Метод присваивает
        /// элементу TextBox последнее
        /// корректное значение
        /// если при потере фокуса 
        /// цвет элемента красный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLeave(object sender, EventArgs e)
        {
            //Преобразуем из object в TextBox
            var textBox = (TextBox)sender;
            //Если цвет поля красный
            if(textBox.BackColor == Color.Salmon)
            {
                //Определяем название параметра
                var name = _formElements[textBox];
                //Присваиваем значение в соответствующий параметру TextBox
                textBox.Text = _modelParameters.Parameter(name).Value.ToString();
            }
        }

        /// <summary>
        /// Методя для отображаения 
        /// интервала значений 
        /// параметра
        /// </summary>
        /// <param name="name">Имя параметра</param>
        /// <param name="label">Label соответствующий параметру</param>
        private void DisplayInterval(ParametersName name, Label label)
        {
            //Получаем параметер по имени
            var parameter = _modelParameters.Parameter(name);
            //Отображаем его интервал
            label.Text = "Интервал ( от " +
                        parameter.MinValue + " до " +
                        parameter.MaxValue + " )";
        }

        /// <summary>
        /// Метод возвращает форму 
        /// к начальным параметра
        /// при нажатии на кнопку "Сбросить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            //Создание нового списка параметров
            _modelParameters = new ModelParameters();
            //Перебор всех TextBox формы
            foreach (var textBox in _formElements.Keys)
            {
                //Определение имени параметра по TextBox
                var parameterName = _formElements[textBox];
                //Окрашивание TextBox в зеленый цвет
                textBox.BackColor = Color.LightGreen;
                //Присваиваем текущее значение параметра в соответствующий TextBox
                textBox.Text =
                    string.Concat(_modelParameters.Parameter(parameterName).Value);
                //Отображение интервала значений места хвата
                DisplayInterval(ParametersName.GripLength, IntervalHandleHLLabel);
                //Отображение интервала значений гладкого места
                DisplayInterval(ParametersName.ConnectionLength, IntervalHandleCLLabel);
                //Отображение интервала значений длины рукояти
                DisplayInterval(ParametersName.HandleLength, IntervalHandleLLabel);
            }
        }

        /// <summary>
        /// Методя для создания нового
        /// экземпляра класса Manager
        /// при нажатии на кнопку "Создать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuilderButton_Click(object sender, EventArgs e)
        {
            var manager = new Manager(_modelParameters);
        }
    }
}
