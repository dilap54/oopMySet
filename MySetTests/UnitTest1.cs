using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySet;
namespace MySetTests
{
    [TestClass]
    public class ArraySetTest
    {
        [TestMethod]
        public void CreateArraySet()
        {
            ArraySet<int> arraySet = new ArraySet<int>();
        }
          
        [TestMethod]
        public void AddItemMustIncreaseCount()
        {
            ArraySet<int> arraySet = new ArraySet<int>();
            int count = arraySet.Count;
            arraySet.Add(1);
            Assert.IsTrue(arraySet.Count > count);

            count = arraySet.Count;
            arraySet.Add(1);
            Assert.IsTrue(arraySet.Count > count);
        }

        [TestMethod]
        public void RemoveExistedItemDecreaseCount()
        {
            ArraySet<int> arraySet = new ArraySet<int>();
            arraySet.Add(1);
            int count = arraySet.Count;
            arraySet.Remove(1);
            Assert.IsTrue(arraySet.Count < count);
        }

        [TestMethod]
        public void RemoveNonExistedItemDecreaseCount()
        {
            ArraySet<int> arraySet = new ArraySet<int>();
            arraySet.Add(1);
            int count = arraySet.Count;
            arraySet.Remove(10);
            Assert.AreEqual(count, arraySet.Count);
        }

        [TestMethod]
        public void ClearClearsSet()
        {
            ArraySet<int> arraySet = new ArraySet<int>();
            arraySet.Add(1);
            arraySet.Add(2);
            arraySet.Add(3);
            arraySet.Clear();

            Assert.AreEqual(0, arraySet.Count);
        }
    }
}
