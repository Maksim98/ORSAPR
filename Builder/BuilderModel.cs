using Kompas6API5;
using RodKernelParameters;
using Kompas6Constants3D;


namespace Builder
{
    /// <summary>
    /// Класс для построения 3Д модели 
    /// в САПР Компас 3Д
    /// </summary>
    public class BuilderModel
    {
        /// <summary>
        /// Хранит ссылку на экземпляр объекта Компас 3Д
        /// </summary>
        private KompasObject _kompasObject;

        /// <summary>
        /// Хранит параметры модели грифа для штанги
        /// </summary>
        private ModelParameters _modelelParameters;

        /// <summary>
        /// Конструктор класса BuilderModel
        /// </summary>
        /// <param name="parameters">Параметры модели грифа</param>
        /// <param name="kompas">Экзепляр Компас 3Д</param>
        public BuilderModel(ModelParameters parameters, KompasObject kompas)
        {
            //Получение параметров модели грифа
            _modelelParameters = parameters;
            //Получение объекта KOMPAS 3D
            _kompasObject = kompas;
            //Создание модели грифа
            CreateModel();
        }

        /// <summary>
        /// Построения 3Д модели
        /// </summary>
        private void CreateModel()
        {
            //Получение интерфейса 3d документа
            ksDocument3D iDocument3D = (ksDocument3D)_kompasObject.Document3D();
            iDocument3D.Create(false, true);
            //Получение интерфейса детали
            ksPart iPart = (ksPart)iDocument3D.GetPart((short)Part_Type.pTop_Part);
            //Создание гладкой части
            CreateConnector(iPart);
            //Создание места для хвата
            CreateGrip(iPart, false);
            CreateGrip(iPart, true);
            //Окрашивание места для хвата в черный цвет
            CreateBlackColor(iPart, true);
            CreateBlackColor(iPart, false);
            //Создание ограничителя для блинов
            CreateLimiter(iPart, true);
            CreateLimiter(iPart, false);
            //Создание держателя для блинов
            CreateHolder(iPart, false);
            CreateHolder(iPart, true);
        }

        /// <summary>
        /// Построение гладкой части 
        /// </summary>
        private void CreateConnector(ksPart iPatr)
        {
            if(_modelelParameters.Parameter(ParametersName.ConnectionLength).Value != 0)
            {
                //Получаем интерфейс базовой плоскости ХОY
                ksEntity planeZOY =
                    (ksEntity)iPatr.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
                //Создаем новый эскиз
                ksEntity iSketch =
                    (ksEntity)iPatr.NewEntity((short)Obj3dType.o3d_sketch);
                //Получаем интерфейс свойств эскиза
                ksSketchDefinition iDefinitionSketch = (ksSketchDefinition)iSketch.GetDefinition();
                //Устанавливаем плоскость эскиза
                iDefinitionSketch.SetPlane(planeZOY);
                //Создание эскиза
                iSketch.Create();
                //Создание нового 2Д документа
                ksDocument2D iDocument2D = (ksDocument2D)iDefinitionSketch.BeginEdit();
                //Получение радиуса рукояти
                var rad = _modelelParameters.Parameter(ParametersName.HandleRadius).Value;
                //Построение круга
                iDocument2D.ksCircle(0, 0, rad + 0.1, 1);
                //Создание эскиза
                iDefinitionSketch.EndEdit();
                //Получение глубины выдавливания
                var depth = _modelelParameters.Parameter(ParametersName.ConnectionLength).Value / 2;
                //Выдавливание в обе стороны
                ExctrusionSketch(iPatr, iSketch, depth, false);
                ExctrusionSketch(iPatr, iSketch, depth, true);
            }
        }

        /// <summary>
        /// Окрашивание места для хвата в чёрный 
        /// </summary>
        /// <param name="iPart">Интерфейс детали</param>
        /// <param name="side">Направление</param>
        private void CreateBlackColor(ksPart iPart, bool side)
        {
            //Получение радиуса ручки
            var X = _modelelParameters.Parameter(ParametersName.HandleRadius).Value;
            //Получение длины гладкой части
            var Y = _modelelParameters.Parameter(ParametersName.ConnectionLength).Value + 5;
            var Z = 0;
            //Получаем массив граней объекта
            ksEntityCollection entityCollectionPart =
                    (ksEntityCollection)iPart.EntityCollection((short)Obj3dType.o3d_face);
            //Сортируем грани по заданным координатам
            if (side == true)
            {
                entityCollectionPart.SelectByPoint(X,  Y, Z);
            }
            else
            {
                entityCollectionPart.SelectByPoint(X, -Y, Z);
            }
            //Получаем первый элмент массива
            ksEntity element = entityCollectionPart.First();
            //Получаем интерфейс управления цветом детали
            ksColorParam colorParam = element.ColorParam();
            //Назначаем цвет
            colorParam.color = 333333;
            //Применяем изменения
            element.Update();
        }

        /// <summary>
        /// Построение держателя для блинов
        /// </summary>
        /// <param name="iPart">Интерфейс детали</param>
        /// <param name="side">Направление</param>
        private void CreateHolder(ksPart iPart, bool side)
        {
            //Получаем массив граней объекта
            ksEntityCollection entityCollectionPart =
                (ksEntityCollection)iPart.EntityCollection((short)Obj3dType.o3d_face);
            //Получаем длину гладкой части
            var Y = _modelelParameters.Parameter(ParametersName.ConnectionLength).Value / 2;
            //Получаем длину места хвата
            var Y1 = _modelelParameters.Parameter(ParametersName.GripLength).Value / 2;
            var Y2 = 15;
            //Сортируем грани по координатам
            if (side == false)
            {
                entityCollectionPart.SelectByPoint(0, -(Y + Y1 + Y2), 0);
            }
            if (side == true)
            {
                entityCollectionPart.SelectByPoint(0, (Y + Y1 + Y2), 0);
            }
            //Получаем первый элемент массива
            var planeDetal = entityCollectionPart.First();
            //Создаем новый эскиз
            ksEntity iSketch =
                (ksEntity)iPart.NewEntity((short)Obj3dType.o3d_sketch);
            //Получаем интерфейс свойств эскиза
            ksSketchDefinition iDefinitionSketch = (ksSketchDefinition)iSketch.GetDefinition();
            //Устанавливаем плоскость эскиза
            iDefinitionSketch.SetPlane(planeDetal);
            //Создание эскиза
            iSketch.Create();
            //Создание нового 2Д документа
            ksDocument2D iDocument2D = (ksDocument2D)iDefinitionSketch.BeginEdit();
            //Получение радиуса рукоятки
            var rad = _modelelParameters.Parameter(ParametersName.HolderRadius).Value;
            //Создание кругов
            iDocument2D.ksCircle(0, 0, 3, 1);
            iDocument2D.ksCircle(0, 0, rad, 1);
            //Создание эскиза
            iDefinitionSketch.EndEdit();
            //Получение глубины выдавливания
            var depth = _modelelParameters.Parameter(ParametersName.HolderLength).Value;
            //Выполнение выдавливания
            ExctrusionSketch(iPart, iSketch, depth, true);
        }

        /// <summary>
        /// Построение места для хвата
        /// </summary>
        private void CreateGrip(ksPart iPart, bool side)
        {
            //Объявление объекта плоскости 
            ksEntity planeDetal = null;
            var direction = true;
            //Если длина гладкого места равна 0
            if (_modelelParameters.Parameter(ParametersName.ConnectionLength).Value == 0)
            {
                //Получаем интерфейс базовой плоскости ХОY
                planeDetal =
                    (ksEntity)iPart.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
                if(side == true)
                {
                    direction = false;
                }
            }
            else
            {
                //Получаем массив граней объекта
                ksEntityCollection entityCollectionPart =
                    (ksEntityCollection)iPart.EntityCollection((short)Obj3dType.o3d_face);
                //Получаем длину гладкой части
                var Y = _modelelParameters.Parameter(ParametersName.ConnectionLength).Value / 2;
                //Сортируем грани по принадлежности к координатам 
                if (side == false)
                {
                    entityCollectionPart.SelectByPoint(0, -Y, 0);
                }
                if (side == true)
                {
                    entityCollectionPart.SelectByPoint(0, Y, 0);
                }
                //Получение первой грани массива
                planeDetal = entityCollectionPart.First();
            }
            //Создаем новый эскиз
            ksEntity iSketch =
                (ksEntity)iPart.NewEntity((short)Obj3dType.o3d_sketch);
            //Получаем интерфейс свойств эскиза
            ksSketchDefinition iDefinitionSketch = (ksSketchDefinition)iSketch.GetDefinition();
            //Устанавливаем плоскость эскиза
            iDefinitionSketch.SetPlane(planeDetal);
            //Создание эскиза
            iSketch.Create();
            //Создание нового 2Д документа
            ksDocument2D iDocument2D = (ksDocument2D)iDefinitionSketch.BeginEdit();
            //Получение радиуса ручки
            var rad = _modelelParameters.Parameter(ParametersName.HandleRadius).Value;
            //Создание круга
            iDocument2D.ksCircle(0, 0, rad, 1);
            //Создание эскиза
            iDefinitionSketch.EndEdit();
            //Получение глубины выдавливания
            var depth = _modelelParameters.Parameter(ParametersName.GripLength).Value / 2;
            //Выполнение выдавливания
            ExctrusionSketch(iPart, iSketch, depth, direction);
        }

        /// <summary>
        /// Построение ограничителя
        /// </summary>
        private void CreateLimiter(ksPart iPart, bool side)
        {
            //Получаем массив граней объекта
            ksEntityCollection entityCollectionPart =
                (ksEntityCollection)iPart.EntityCollection((short)Obj3dType.o3d_face);
            //Получаем длину гладкой части
            var Y = _modelelParameters.Parameter(ParametersName.ConnectionLength).Value / 2;
            //Получение длины места хвата
            var Y1 = _modelelParameters.Parameter(ParametersName.GripLength).Value / 2;
            //Сортируем грани по принадлежности к координатам
            if (side == false)
            {
                entityCollectionPart.SelectByPoint(0, -(Y + Y1), 0);
            }
            if (side == true)
            {
                entityCollectionPart.SelectByPoint(0, (Y + Y1), 0);
            }
            //Получение первой грани массива
            var planeDetal = entityCollectionPart.First();
            //Создаем новый эскиз
            ksEntity iSketch =
                (ksEntity)iPart.NewEntity((short)Obj3dType.o3d_sketch);
            //Получаем интерфейс свойств эскиза
            ksSketchDefinition iDefinitionSketch = (ksSketchDefinition)iSketch.GetDefinition();
            //Устанавливаем плоскость эскиза
            iDefinitionSketch.SetPlane(planeDetal);
            //Создание эскиза
            iSketch.Create();
            //Создание нового 2Д документа
            ksDocument2D iDocument2D = (ksDocument2D)iDefinitionSketch.BeginEdit();
            //Получение радиуса ограничителя
            var rad = _modelelParameters.Parameter(ParametersName.LimiterRadius).Value;
            //Создание круга
            iDocument2D.ksCircle(0, 0, rad, 1);
            //Создание эскиза
            iDefinitionSketch.EndEdit();
            //Глубина выдавливания
            var depth = 15;
            //Операция выдавливания
            ExctrusionSketch(iPart, iSketch, depth, true);
        }

        /// <summary>
        /// Выдавливание по эскизу
        /// </summary>
        /// <param name="iPart">Интерфейс детали</param>
        /// <param name="iSketch">Эскиз</param>
        /// <param name="depth">Глубина выдавливания</param>
        /// <param name="type">Тип выдавливания</param>
        private void ExctrusionSketch (ksPart iPart, ksEntity iSketch, double depth, bool type)
        {
            //Операция выдавливание
            ksEntity entityExtr = (ksEntity)iPart.NewEntity((short)Obj3dType.o3d_bossExtrusion);
            //Интерфейс операции выдавливания
            ksBossExtrusionDefinition extrusionDef =
                (ksBossExtrusionDefinition)entityExtr.GetDefinition();                
            //Интерфейс структуры параметров выдавливания
            ksExtrusionParam extrProp =
                (ksExtrusionParam)extrusionDef.ExtrusionParam();    
            //Эскиз операции выдавливания
            extrusionDef.SetSketch(iSketch);
            //Направление выдавливания
            if (type == false)
            {
                extrProp.direction = (short)Direction_Type.dtReverse;
            }
            if (type == true)
            {
                extrProp.direction = (short)Direction_Type.dtNormal;
            }
            //Тип выдавливания
            extrProp.typeNormal = (short)End_Type.etBlind;
            //Глубина выдавливания
            if (type == false)
            {
                extrProp.depthReverse = depth;
            }
            if (type == true)
            {
                extrProp.depthNormal = depth;
            }
            //Создание операции
            entityExtr.Create();
        }
    }
}
