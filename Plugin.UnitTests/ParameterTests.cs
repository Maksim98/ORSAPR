using NUnit.Framework;
using RodKernelParameters;
using System;

namespace Plugin.UnitTests
{
    public class ParameterTests
    {

        [Test(Description = "���������� ���� ������� MaxValue")]
        public void Test_MaxValue_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MaxValue = expected;
            var actual = parameter.MaxValue;
            Assert.AreEqual(expected, actual, "������ MaxValue ����������� ������������ ��������");
        }

        [Test(Description = "���������� ���� ������� MaxValue")]
        public void Test_MaxValue_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MaxValue = expected;
            Assert.AreEqual(expected, parameter.MaxValue, "������ MaxValue ����������� ���������� ��������");
        }

        [Test(Description = "���������� ���� ������� MinValue")]
        public void Test_MinValue_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MinValue = expected;
            Assert.AreEqual(expected, parameter.MinValue, "������ MinValue ����������� ���������� ��������");
        }

        [Test(Description = "���������� ���� ������� MinValue")]
        public void Test_MinValue_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 10, 10, "nameParameter");
            var expected = 20;
            parameter.MinValue = expected;
            var actual = parameter.MinValue;
            Assert.AreEqual(expected, actual, "������ MinValue ����������� ���������� ��������");
        }

        [Test(Description = "���������� ���� ������� Value")]
        public void Test_Value_Set_CorrectValue()
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            var expected = 20;
            parameter.Value = expected;
            Assert.AreEqual(expected, parameter.Value, "������ Value ����������� ���������� ��������");
        }

        [Test(Description = "���������� ���� ������� Value")]
        public void Test_Value_Get_CorrectValue()
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            var expected = 20;
            parameter.Value = expected;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual, "������ Value ����������� ���������� ��������");
        }

        [Test(Description = "���� ������������ Parameter")]
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
            Assert.AreEqual(expected, actual, "����������� Parameter ������� ������������ ���������");
        }

        [TestCase("-500", "������ ��������� ���������� ����, ������������ �������� ������ �������������",
           TestName = "���������� �������� ������ ������������")]
        [TestCase("200", "������ ��������� ���������� ����, ������������ �������� ������ �������������",
           TestName = "���������� �������� ������ �������������")]
        public void TestLastModTimeSet_ArgumentException(string wrongLastModTime, string messege)
        {
            var parameter = new Parameter(10, 30, 10, "nameParameter");
            Assert.Throws<ArgumentException>(() => { parameter.Value = double.Parse(wrongLastModTime); }, messege);
        }
    }
}