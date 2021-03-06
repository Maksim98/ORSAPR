﻿using System;

namespace RodKernelParameters
{
    /// <summary>
    /// Класс хранит данные параметра модели
    /// Хранит текущее значение параметра
    /// название параметра
    /// минимально возможно значение параметра
    /// максимально возможное значение параметра
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Поле хранит название параметра
        /// </summary>
        private string _name;

        /// <summary>
        /// Поле хранит текущее значение параметра
        /// </summary>
        private double _value;

        /// <summary>
        /// Устанавливает и возвращает максимальное значение параметра
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// Устанавливает и возвращает минимальное значение параметра
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Устанавливает и возвращает текущее значение параметра
        /// Текущее значение будет 
        /// устанановлено, если пришедшее значение
        /// входит в диапозон параметров
        /// от _minValue до _maxValue
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                //Если значение пришедшее для записи больше максимально возможного
                //или меньше минимально возможного
                if (value.CompareTo(MinValue) < 0 || value.CompareTo(MaxValue) > 0)
                {
                    //Вызов ошибки с необходимым тестом
                    throw new ArgumentException("Необходимо, чтобы значение " + _name + " находилось в диапозоне от " +
                        MinValue + " до " + MaxValue);
                }
                else
                {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// Коструктор класса Parameter
        /// </summary>
        /// <param name="minValue">Минимально возможно значение параметра</param>
        /// <param name="maxValue">Максимально возможное значение параметра</param>
        /// <param name="value">Текущее значение параметра</param>
        /// <param name="name">Название создаваемого параметра</param>
        public Parameter(double minValue, double maxValue, double value, string name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            _value = value;
            _name = name;
        }
    }
}
