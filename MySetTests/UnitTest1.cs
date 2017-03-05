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
            for (int i=1; i<= numOfElements; i++)
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
        public void Setup()//Подставляет LinkedSet в mySet перед каждым тестом
        {
            this.mySet = new MySet.HashSet<int>();
        }
    }
}
