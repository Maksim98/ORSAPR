using NUnit.Framework;
using RodKernelParameters;
using System.Collections.Generic;
using System;

namespace Plugin.UnitTests
{
    public class ModelParametersTests
    {
        [Test(Description = "Позитивный тест метода Parameter")]
        public void Test_Parameter()
        {
            var modelParameters = new ModelParameters();
            var result = true;
            if(modelParameters.Parameter(ParametersName.HandleLength).MinValue != 890
                || modelParameters.Parameter(ParametersName.HandleLength).MaxValue != 1100
                || modelParameters.Parameter(ParametersName.HandleLength).Value != 890)
            {
                result = false;
            }
            Assert.IsTrue(result, "Метод Parameter работает некорректно");
        }

        [Test(Description = "Позитивный тест метода CalculationLenghtConnection")]
        public void Test_CalculationLenghtConnection()
        {
            var modelParameters = new ModelParameters();
            var Grip = 890;
            var Handle = 1100;
            var expected = Handle - Grip;
            modelParameters.Parameter(ParametersName.GripLength).Value = Grip;
            modelParameters.Parameter(ParametersName.HandleLength).Value = Handle;
            modelParameters.CalculationLenghtConnection();
            var actual = modelParameters.Parameter(ParametersName.ConnectionLength).Value;
            Assert.AreEqual(expected, actual, "Метод CalculationLenghtConnection работает некорректно");
        }

        [Test(Description = "Позитивный тест метода CalculationLenghtGrip")]
        public void Test_CalculationLenghtGrip()
        {
            var modelParameters = new ModelParameters();
            var Handle = 1100;
            var Connector = 210;
            var expected = Handle - Connector;
            modelParameters.Parameter(ParametersName.ConnectionLength).Value = Connector;
            modelParameters.Parameter(ParametersName.HandleLength).Value = Handle;
            modelParameters.CalculationLenghtGrip();
            var actual = modelParameters.Parameter(ParametersName.GripLength).Value;
            Assert.AreEqual(expected, actual, "Метод CalculationLenghtGrip работает некорректно");
        }

        [Test(Description = "Позитивный тест метода CalculationMaxLenghtGrip")]
        public void Test_CalculationMaxLenghtGrip()
        {
            var modelParameters = new ModelParameters();
            var Handle = 1100;
            var expected = Handle;
            modelParameters.Parameter(ParametersName.HandleLength).Value = Handle;
            modelParameters.CalculationMaxLenghtGrip();
            var actual = modelParameters.Parameter(ParametersName.GripLength).MaxValue;
            Assert.AreEqual(expected, actual, "Метод CalculationMaxLenghtGrip работает некорректно");
        }

        [Test(Description = "Позитивный тест метода CalculationMinValueLenghtGrip")]
        public void Test_CalculationMinValueLenghtGrip()
        {
            var modelParameters = new ModelParameters();
            var Handle = 1100;
            var Connector = 210;
            var expected = Handle - Connector;
            modelParameters.Parameter(ParametersName.HandleLength).Value = Handle;
            modelParameters.CalculationMinValueLenghtGrip();
            var actual = modelParameters.Parameter(ParametersName.GripLength).MinValue;
            Assert.AreEqual(expected, actual, "Метод CalculationMinValueLenghtGrip работает некорректно");
        }

        [Test(Description = "Позитивный тест метода перечисления ToString")]
        public void Test_ToString()
        {
            var expected = "GripLength";
            var actual = ParametersName.GripLength.ToString();
            Assert.AreEqual(expected, actual, "Метод перечисления ToString работает некорректно");
        }

        [Test(Description = "Позитивный тест конструктора ModelParameters")]
        public void Test_ModelParameters()
        {
            var modelParameters = new ModelParameters();
            var result = true;
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

            foreach (var value in values)
            {
                if(value.name == ParametersName.GripLength)
                {
                    if (modelParameters.Parameter(value.name).MaxValue != value.max ||
                    modelParameters.Parameter(value.name).MinValue != value.min ||
                    modelParameters.Parameter(value.name).Value != value.max)
                    {
                        result = false;
                    }
                }
                else
                {
                    if (modelParameters.Parameter(value.name).MaxValue != value.max ||
                    modelParameters.Parameter(value.name).MinValue != value.min ||
                    modelParameters.Parameter(value.name).Value != value.min)
                    {
                        result = false;
                    }
                }
            }
            Assert.IsTrue(result, "Конструктор ModelParameters не создает корректный экземпляр класса");
        }
    }
}
