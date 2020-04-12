using System;
using Kompas6API5;
using RodKernelParameters;
using System.Runtime.InteropServices;

namespace Builder
{
    /// <summary>
    /// Класс используется для подключения 
    /// к САПР Компас 3Д и инициализации 
    /// экземпляра построителя модели
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Поле хранит экземпляр построителя 3D модели
        /// грифа для штанги
        /// </summary>
        private BuilderModel _builderModel;

        /// <summary>
        /// Подключение к экземпляру компас 3Д
        /// Если экземпляр есть создан,
        /// то подключиться к существующему
        /// Если экземпляр не создан,
        /// то создать и подключиться к новому
        /// </summary>
        /// <returns>Экземпляр KompasObject</returns>
        private KompasObject OpenKompas3D()
        {
            //Начальное присвоение 
            KompasObject kompas = null;
            //Экзмпляр уже существует
            //Отображение необходимо в каждом случае
            //так как возможна ошибка при подключении 
            //к уже закрытому экземпляру
            try
            {
                kompas = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
                kompas.Visible = true;
            }
            //Создание нового экзмепляра
            catch
            {
                Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
                kompas = (KompasObject)Activator.CreateInstance(t);
                kompas.Visible = true;
            }
            finally
            {
                //Активация API созданного экземпляра КОМПАС 3Д
                kompas.ActivateControllerAPI();
            }
            return kompas;
        }
        
        /// <summary>
        /// Конструктор класса Manager
        /// Вызывает метод для инициализации
        /// экземпляра построителя 3D модели 
        /// </summary>
        /// <param name="parameters"></param>
        public Manager(ModelParameters parameters)
        {
            //Инициализируем модель
            InirializeModel(parameters);
        }

        /// <summary>
        /// Метод создает экземпляр 
        /// класса построителья модели
        /// грифа для штанги
        /// </summary>
        /// <param name="parameters"></param>
        private void InirializeModel(ModelParameters parameters)
        {
            //Инициализация нового объекта построителя
            _builderModel = new BuilderModel(parameters,OpenKompas3D());
        }
    }
}
