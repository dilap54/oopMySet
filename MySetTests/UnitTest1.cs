using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySet;
using System.Collections.Generic;

namespace MySetTests
{
    //Базовый класс, тестирующий все методы ISet
    public abstract class BaseSetTest
    {
        protected MySet.ISet<int> mySet;

        //Count показывает то количество элементов, которое было добавлено
        [TestMethod]
        public void CountMustReturnRealCount()
        {
            Assert.AreEqual(0, mySet.Count);

            int numOfElements = 123;
            for (int i = 0; i < numOfElements; i++)
            {
                mySet.Add(i);
            }

            Assert.AreEqual(numOfElements, mySet.Count);
        }

        //Добавление разных элементов увеличивает Count
        [TestMethod]
        public void AddDifferentItemMustIncreaseCount()
        {
            int count = mySet.Count;
            mySet.Add(1);
            Assert.AreEqual(count + 1, mySet.Count);

            count = mySet.Count;
            mySet.Add(2);
            Assert.AreEqual(count + 1, mySet.Count);
        }

        //Добавление одинаковых элементов не увеличивает Count
        [TestMethod]
        public void AddDuplicateItemMustntIncreaseCount()
        {
            mySet.Add(1);
            int count = mySet.Count;
            mySet.Add(1);
            Assert.AreEqual(count, mySet.Count);
        }

        //Удаление существующего элемента уменьшает Count
        [TestMethod]
        public void RemoveExistedItemDecreaseCount()
        {
            mySet.Add(1);
            mySet.Add(2);

            int count = mySet.Count;
            mySet.Remove(1);
            Assert.IsTrue(mySet.Count < count);
        }

        //Удаление несуществующего элемента не уменьшает Count
        [TestMethod]
        public void RemoveNonExistedItemNotDecreaseCount()
        {
            mySet.Add(1);
            mySet.Add(2);

            int count = mySet.Count;
            mySet.Remove(10);
            Assert.AreEqual(count, mySet.Count);
        }

        //После Clear Count == 0
        [TestMethod]
        public void ClearClearsSet()
        {
            mySet.Add(1);
            mySet.Add(2);
            mySet.Add(3);

            mySet.Clear();

            Assert.AreEqual(0, mySet.Count);
        }

        //Contains возвращает корректное значение
        [TestMethod]
        public void ContainsReturnCorrectBool()
        {
            mySet.Add(11);
            mySet.Add(22);
            mySet.Add(33);

            Assert.IsTrue(mySet.Contains(22));
            Assert.IsFalse(mySet.Contains(23));
        }

        //Одинаковое ли количество отправленных и полученных значений
        [TestMethod]
        public void EnumeratorReturnEnaughtValues()
        {
            int numOfElements = 1234;
            for (int i=0; i<numOfElements; i++)
            {
                mySet.Add(i);
            }
            int recievedValues = 0;
            foreach (int item in mySet)
            {
                recievedValues++;
            }
            Assert.AreEqual(numOfElements, recievedValues);
        }

        //Проверяет, че там ваще хранится в этом mySet
        [TestMethod]
        public void EnumeratorWork()
        {
            mySet.Add(11);
            mySet.Add(22);
            mySet.Add(44);
            mySet.Add(33);
            mySet.Remove(44);

            System.Collections.Generic.HashSet<int> hs = new System.Collections.Generic.HashSet<int>();
            hs.Add(11);
            hs.Add(22);
            hs.Add(33);

            int foreachcount = 0;
            foreach (int item in mySet)
            {
                foreachcount++;
                Assert.IsTrue(hs.Contains(item));
                hs.Remove(item);//На случай, если в mySet 3 одинаковых элемента
            }
            Assert.AreEqual(3, foreachcount);
        }
    }

    
    [TestClass]
    public class ArraySetTest : BaseSetTest
    {
        [TestInitialize]
        public void Setup()//Подставляет ArraySet в mySet перед каждым тестом
        {
            this.mySet = new ArraySet<int>();
        }

    }

    [TestClass]
    public class LinkedSetTest : BaseSetTest
    {
        [TestInitialize]
        public void Setup()//Подставляет LinkedSet в mySet перед каждым тестом
        {
            this.mySet = new LinkedSet<int>();
        }
    }

    [TestClass]
    public class HashSetTest : BaseSetTest
    {
        [TestInitialize]
        public void Setup()//Подставляет HashSetTest в mySet перед каждым тестом
        {
            this.mySet = new MySet.HashSet<int>();
        }
    }

    [TestClass]
    public class UnmutableSetTest
    {
        UnmutableSet<int> mySet100;
        int setLength = 100;

        [TestInitialize]
        public void Setup()//Подставляет UnmutableSet в mySet перед каждым тестом
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            for (int i=0; i<setLength; i++)
            {
                arrSet.Add(i);
            }
            mySet100 = new UnmutableSet<int>(arrSet);
        }

        [TestMethod]
        public void CountMustReturnCorrectValue()
        {
            Assert.AreEqual(setLength, mySet100.Count);

            ArraySet<int> arrSet = new ArraySet<int>();
            UnmutableSet<int> mySet = new UnmutableSet<int>(arrSet);

            Assert.AreEqual(0, mySet.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAllowedMethodException))]
        public void AddMustThrowException()
        {
            mySet100.Add(10);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAllowedMethodException))]
        public void ClearMustThrowException()
        {
            mySet100.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotAllowedMethodException))]
        public void RemoveMustThrowException()
        {
            mySet100.Remove(10);
        }
    }

    [TestClass]
    public class SetUtilsTest
    {
        //Exist возвращает true, если элемент есть
        [TestMethod]
        public void ExistMustReturnTrueIfItemsExist()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            arrSet.Add(1);
            arrSet.Add(3);

            Assert.IsTrue(SetUtils.Exists(arrSet, (item) => {
                return item == 1;
            }));
        }

        //Exist возвращает false, если элемента нет
        [TestMethod]
        public void ExistMustReturnFalseIfItemsNonExist()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            arrSet.Add(1);
            arrSet.Add(3);

            Assert.IsFalse(SetUtils.Exists(arrSet, (item) => {
                return item == 2;
            }));
        }

        //FindAll возвращает множество с четными числами
        [TestMethod]
        public void FindAllReturnCorrectValue()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            for(int i=0; i<50; i++)
            {
                arrSet.Add(i);
            }

            ArraySet<int> newArrSet = SetUtils.FindAll<int, ArraySet<int>>(arrSet, (item) => {
                return item % 2 == 0;
            });

            foreach(int item in newArrSet)
            {
                Assert.IsTrue(item % 2 == 0);
            }
            Assert.AreEqual(25, newArrSet.Count);
        }

        //ConvertAll возвращает правильный тип данных
        [TestMethod]
        public void ConvertAllReturnCorrectValue()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            arrSet.Add(10);

            Assert.IsInstanceOfType(SetUtils.ConvertAll<int, MySet.HashSet<int>>(arrSet), typeof(MySet.HashSet<int>));
            Assert.IsInstanceOfType(SetUtils.ConvertAll<int, ArraySet<int>>(arrSet), typeof(ArraySet<int>));
            Assert.IsInstanceOfType(SetUtils.ConvertAll<int, LinkedSet<int>>(arrSet), typeof(LinkedSet<int>));
        }

        //ConvertAll возвращает правильное количество элементов
        [TestMethod]
        public void ConvertAllReturnCorrectNumberOfElements()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            int numOfElements = 50;
            for (int i = 0; i < numOfElements; i++)
            {
                arrSet.Add(i);
            }

            Assert.AreEqual(numOfElements, SetUtils.ConvertAll<int, MySet.HashSet<int>>(arrSet).Count);
            Assert.AreEqual(numOfElements, SetUtils.ConvertAll<int, ArraySet<int>>(arrSet).Count);
            Assert.AreEqual(numOfElements, SetUtils.ConvertAll<int, LinkedSet<int>>(arrSet).Count);
        }

        //ForEach запускает фукнцию для всех элементов
        [TestMethod]
        public void ForEachRunFuncForEverItem()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            int numOfElements = 50;
            bool[] testArray = new bool[numOfElements];
            for (int i = 0; i < numOfElements; i++)
            {
                arrSet.Add(i);
                testArray[i] = false;
            }

            SetUtils.ForEach<int>(arrSet, (item) => {
                testArray[item] = true;
            });

            for (int i = 0; i < numOfElements; i++)
            {
                Assert.IsTrue(testArray[i]);
            }
        }

        [TestMethod]
        public void CheckForAllReturnTrue()
        {
            ArraySet<int> arrSet = new ArraySet<int>();
            int numOfElements = 50;
            for (int i = 0; i < numOfElements; i++)
            {
                arrSet.Add(i);
            }

            Assert.IsTrue(SetUtils.CheckForAll<int>(arrSet, (item)=>{
                return item >= 0 && item <= numOfElements;
            }));
        }
    }
}
