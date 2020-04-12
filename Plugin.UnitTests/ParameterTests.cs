using NUnit.Framework;
using RodKernelParameters;
using System;

namespace Plugin.UnitTests
{
    public class ParameterTests
    {

        [Test(Description = "Позитивный тест геттера MaxValue")]
        public void Test_MaxValue_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MaxValue = expected;
            var actual = parameter.MaxValue;
            Assert.AreEqual(expected, actual, "Геттер MaxValue некорректно возвращащает значение");
        }

        [Test(Description = "Позитивный тест сеттера MaxValue")]
        public void Test_MaxValue_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MaxValue = expected;
            Assert.AreEqual(expected, parameter.MaxValue, "Сеттер MaxValue некорректно записывает значение");
        }

        [Test(Description = "Позитивный тест сеттера MinValue")]
        public void Test_MinValue_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MinValue = expected;
            Assert.AreEqual(expected, parameter.MinValue, "Сеттер MinValue некорректно записывает значение");
        }

        [Test(Description = "Позитивный тест геттера MinValue")]
        public void Test_MinValue_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MinValue = expected;
            var actual = parameter.MinValue;
            Assert.AreEqual(expected, actual, "Геттер MinValue некорректно возвращает значение");
        }

        [Test(Description = "Позитивный тест сеттера Value")]
        public void Test_Value_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            var expected = 20;
            parameter.Value = expected;
            Assert.AreEqual(expected, parameter.Value, "Сеттер Value некорректно записывает значение");
        }

        [Test(Description = "Позитивный тест геттера Value")]
        public void Test_Value_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            var expected = 20;
            parameter.Value = expected;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual, "Геттер Value некорректно возвращает значение");
        }

        [Test(Description = "Тест конструктора Parameter")]
        public void Test_Parameter_Designer()
        {
            double[] expected = { 10, 20, 15};
            var parameter = new Parameter(
                expected[0],
                expected[1],
                expected[2],
                "nameParameter"
                );
            object[] actual = {
                parameter.MinValue,
                parameter.MaxValue,
                parameter.Value
                };
            Assert.AreEqual(expected, actual, "Конструктор Parameter создает некорректный экземпляр");
        }

        [TestCase("-500", "Должно возникать исключение если, записываемое значение меньше минимиального",
           TestName = "Присвоение значения меньше минимального")]
        [TestCase("200", "Должно возникать исключение если, записываемое значение больше максимального",
           TestName = "Присвоение значения больше максимального")]
        public void TestLastModTimeSet_ArgumentException(string wrongLastModTime, string messege)
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            Assert.Throws<ArgumentException>(() => { parameter.Value = double.Parse(wrongLastModTime); }, messege);
        }
    }
}