using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Ничего не значащий комментарий для проверки git
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
    public abstract class ISet<T>: IEnumerable where T : IEquatable<T>
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
            if (!Contains(value))//Если такого элемента еще нет
            {
                Array.Resize<T>(ref array, Count + 1);
                array[Count - 1] = value;
            }
        }

        public override void Clear()
        {
            array = new T[0];
        }

        public override bool Contains(T value)
        {
            foreach (T item in array)
            {
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

    public class LinkedSet<T> : ISet<T> where T : IEquatable<T>
    {
        class Node
        {
            public T data;
            public Node next;

            public Node(T data, Node next)
            {
                this.data = data;
                this.next = next;
            }
        }

        Node head;

        int count;

        public override int Count
        {
            get
            {
                return count;
            }
        }

        public override bool isEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public LinkedSet()
        {
            head = null;
            count = 0;
        }

        public override void Add(T value)
        {
            if (head == null)
            {
                head = new Node(value, null);
                count++;
            }
            else
            {
                if (!Contains(value))
                {
                    head = new Node(value, head);
                    count++;
                }
                
            }
        }

        public override void Clear()
        {
            head = null;
            count = 0;
        }

        public override bool Contains(T value)
        {
            if (isEmpty)
            {
                return false;
            }
            Node tmp = head;

            while(tmp != null)
            {
                if (tmp.data.Equals(value))
                {
                    return true;
                }
                tmp = tmp.next;
            }
            return false;
        }

        public override IEnumerator GetEnumerator()
        {
            Node tmp = head;
            while(tmp != null)
            {
                yield return tmp.data;
                tmp = tmp.next;
            }
            
        }

        public override void Remove(T value)
        {
            if (head == null)
            {
                return;
            }
            if (head.data.Equals(value))
            {
                Clear();
                return;
            }

            Node tmp = head;
            while(tmp.next != null && !tmp.next.data.Equals(value))
            {
                tmp = tmp.next;
            }
            if (tmp.next != null)
            {
                tmp.next = tmp.next.next;
                count--;
            }

        }
    }

    public class HashSet<T> : ISet<T> where T : IEquatable<T>
    {
        T[][] htable;
        const int htLength = 1000;

        public HashSet()
        {
            htable = new T[htLength][];//Типа хеш таблица
            for (int i=0; i<htLength; i++)
            {
                htable[i] = new T[0];
            }
        }

        public override int Count
        {
            get
            {
                int count = 0;
                foreach (Array arr in htable)
                {
                    count += arr.Length;
                }
                return count;
            }
        }

        public override bool isEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public override void Add(T value)
        {
            if (!Contains(value))
            {
                int hash = value.GetHashCode() % htLength;
                //T[] arrayOnHash = htable[hash];
                //Ну и почему я дальше не могу использовать arrayOnHash вместо htable[hash]?
                Array.Resize(ref htable[hash], htable[hash].Length + 1);
                htable[hash][htable[hash].Length - 1] = value;
            }
        }

        public override void Clear()
        {
            htable = new T[htLength][];//Типа хеш таблица
            for (int i = 0; i < htLength; i++)
            {
                htable[i] = new T[0];
            }
        }

        public override bool Contains(T value)
        {
            int hash = value.GetHashCode() % htLength;
            T[] arrayOnHash = htable[hash];
            foreach (T item in arrayOnHash)
            {
                if (item.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (T[] item in htable)
            {
                //yield return item.GetEnumerator();
                foreach(T item2 in item)
                {
                    yield return item2;
                }
            }
        }

        public override void Remove(T value)
        {
            int hash = value.GetHashCode() % htLength;
            htable[hash] = htable[hash].Where((item) => {
                return !(item.Equals(value));
            }).ToArray();
        }
    }
}
