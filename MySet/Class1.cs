using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySet
{
    //Кароче это класс исключений
    [Serializable]
    class SetException : Exception
    {
        public SetException(string message) : base(message)
        {

        }
    }

    //Базовый класс
    public abstract class ISet<T>: IEnumerable
    {
        //Объявление метода, нужного для IEnumerable
        public abstract IEnumerator GetEnumerator();

        /*
        //Тоже нужно для IEnumerable. И но я хз что это.
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        */

        //Кучка нужных методов, реализуемых в дочерних классах
        public abstract void Add(T value);

        public abstract void Clear();

        public abstract bool Contains(T value);

        public abstract void Remove(T value);

        public abstract int Count
        {
            get;
        }

        public abstract bool isEmpty
        {
            get;
        }
    }

    public class ArraySet<T> : ISet<T> where T : IEquatable<T>
    {
        T[] array;

        public override int Count
        {
            get
            {
                return array.Length;
            }
        }

        public override bool isEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public ArraySet(){
            array = new T[0];
        }

        public override void Add(T value)
        {
            Array.Resize<T>(ref array, Count+1);
            array[Count - 1] = value;
        }

        public override void Clear()
        {
            array = new T[0];
        }

        public override bool Contains(T value)
        {
            foreach (T item in array)
            {
                //Если ссылки равны
                if (item.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public override void Remove(T value)
        {
            array = array.Where((item) => {
                return !(item.Equals(value));
            }).ToArray();
        }
    }
    
}
