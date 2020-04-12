using System.Collections.Generic;

namespace RodKernelParameters
{
    /// <summary>
    /// Класс хранить словарь параметров модели 
    /// и реализует методы перерасчета значений 
    /// параметров
    /// </summary>
    public class ModelParameters
    {
        /// <summary>
        /// Поле хранит словарь параметров модели
        /// </summary>
        private Dictionary<ParametersName, Parameter> _parameters = new Dictionary<ParametersName, Parameter>();

        /// <summary>
        /// Метод возвращает параметер
        /// соответствующий поданому имени
        /// </summary>
        /// <param name="name">Имя запрашиваемого параметра</param>
        /// <returns></returns>
        public Parameter Parameter(ParametersName name)
        {
            return _parameters[name];
        }

        /// <summary>
        /// Метод для перерасчета
        /// максимальной длины гладкой части
        /// по формуле:
        /// Длина гладкой части =
        /// длина рукояти - длина места хвата
        /// </summary>
        public void CalculationLenghtConnection()
        {
            Parameter(ParametersName.ConnectionLength).Value =
                Parameter(ParametersName.HandleLength).Value
                - Parameter(ParametersName.GripLength).Value;
        }

        /// <summary>
        /// Метод для перерасчета  
        /// текущей длины
        /// места хвата (ребристой части)
        /// Расчет по формуле:
        /// Длина места хвата = 
        /// длина рукояти - длина гладкой части
        /// </summary>
        public void CalculationLenghtGrip()
        {
            Parameter(ParametersName.GripLength).Value =
               Parameter(ParametersName.HandleLength).Value 
               - Parameter(ParametersName.ConnectionLength).Value;
            
        }

        /// <summary>
        /// Метод для перерасчета  
        /// максимальной длины
        /// места хвата (ребристой части)
        /// Метод приравнивает максимальную длину места хвата
        /// и текущую длину рукояти
        /// </summary>
        public void CalculationMaxLenghtGrip()
        {
            Parameter(ParametersName.GripLength).MaxValue =
               Parameter(ParametersName.HandleLength).Value;
        }

        /// <summary>
        /// Методя для перерасчета 
        /// минимальной длины места хвата
        /// Расчет по формуле:
        /// Минимальная длина места хвата = 
        /// длина рукояти - длина гладкого места
        /// </summary>
        public void CalculationMinValueLenghtGrip()
        {
            Parameter(ParametersName.GripLength).MinValue =
               Parameter(ParametersName.HandleLength).Value 
               - Parameter(ParametersName.ConnectionLength).MaxValue;
        }

        /// <summary>
        /// Конструктор класса ModelParameters
        /// </summary>
        public ModelParameters()
        {
            _parameters = new Dictionary<ParametersName, Parameter>();
            //Создаем кортеж со значениями параметров модели
            var values = new List<(double min, double max, ParametersName name)>
            {
                (890, 1100, ParametersName.HandleLength), //Длина рукоятки
                (680, 890, ParametersName.GripLength), //Длина места хвата
                (0, 210, ParametersName.ConnectionLength), //Длина гладкой части
                (5, 10, ParametersName.HandleRadius), //Радиус рукоятки
                (100, 320, ParametersName.HolderLength), //Длина держателя блинов
                (35, 50, ParametersName.HolderRadius), //Радиус держателя блинов
                (55, 75, ParametersName.LimiterRadius) //Радиус ограничителя
            };
            //Перебираем все значения картежа 
            foreach (var value in values)
            {
                //Создание нового параметра
                Parameter parameter = null;
                //Если обрабатываем значение длины рукояти
                if (value.name == ParametersName.GripLength)
                {
                    //Создаем новый параметр и передаем значения из кортежа 
                    //в конструктор
                    parameter = new Parameter(value.min, value.max, value.max, value.name.ToString());
                }
                else
                {
                    //Создаем новый параметр и передаем значения из кортежа 
                    //в конструктор
                    parameter = new Parameter(value.min, value.max, value.min, value.name.ToString());
                }
                //Добавляем созданный параметр в словарь параметров
                _parameters.Add(value.name,parameter);
            }
        }   
    }
}
