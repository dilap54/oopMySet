﻿using System;
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
    public class SetException : Exception
    {
        public SetException() : base()
        {

        }
    }

    [Serializable]
    public class NotAllowedMethodException : Exception
    {
        public NotAllowedMethodException() : base()
        {

        }
    }


    //Базовый класс
    public abstract class ISet<T>: IEnumerable<T> where T : IEquatable<T>
    {
        //Объявление метода, нужного для IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        //Тоже нужно для IEnumerable. И но я хз что это.
        public abstract IEnumerator<T> GetEnumerator();
        

        //Кучка нужных методов, реализуемых в дочерних классах
        public abstract void Add(T value);

        public abstract void Clear();

        public abstract bool Contains(T value);

        public abstract void Remove(T value);

        public abstract int Count
        {
            get;
            /* Можно сделать так, но в дочерних методах есть более быстрые реализации
            {
                return Enumerable.Count(this);
            }
            */
        }

        public bool isEmpty
        {
            get
            {
                return Count == 0;
            }
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

        public ArraySet(){
            array = new T[0];
        }

        public override void Add(T value)
        {
            if (!Contains(value))//Если такого элемента еще нет
            {
                Array.Resize(ref array, Count + 1);
                array[Count - 1] = value;
            }
        }

        public override void Clear()
        {
            array = new T[0];
        }

        public override bool Contains(T value)
        {
            return array.Contains(value);
            //return array.Any(item => item.Equals(value));
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return array.AsEnumerable().GetEnumerator();
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
            get { return count; }
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
            return Enumerable.Contains(this,value);
        }

        public override IEnumerator<T> GetEnumerator()
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
            return arrayOnHash.Contains(value);
        }

        public override IEnumerator<T> GetEnumerator()
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

    public class UnmutableSet<T> : ISet<T> where T : IEquatable<T>
    {
        ISet<T> iset;

        public UnmutableSet(ISet<T> iset)
        {
            this.iset = iset;
        }

        public override int Count => iset.Count;

        public override void Add(T value)
        {
            throw new NotAllowedMethodException();
        }

        public override void Clear()
        {
            throw new NotAllowedMethodException();
        }

        public override bool Contains(T value) => iset.Contains(value);

        public override IEnumerator<T> GetEnumerator() => iset.GetEnumerator();

        public override void Remove(T value)
        {
            throw new NotAllowedMethodException();
        }
    }

    public static class SetUtils //<T> where T : IEquatable<T>
    {

        public static bool Exists<T>(ISet<T> iset, Func<T, bool> check) where T : IEquatable<T>
        {
            foreach (T item in iset)
            {
                if (check(item))
                {
                    return true;
                }
            }
            return false;
        }

        //Эта штука принимает <T, TO>, где TO - возвращаемый тип данных (ArraySet, HashSet, ...)
        //Иначе я не знаю как сделать ссылку на конструктор нужного типа данных
        public static TO FindAll<T, TO>(ISet<T> iset, Func<T, bool> check) where T : IEquatable<T> where TO : ISet<T>, new()
        {
            TO newSet = new TO();
            foreach (T item in iset)
            {
                if (check(item))
                {
                    newSet.Add(item);
                }
            }
            return newSet;
        }

        public static TO ConvertAll<T, TO>(ISet<T> iset) where T : IEquatable<T> where TO : ISet<T>, new()
        {
            TO newSet = new TO();
            foreach (T item in iset)
            {
                newSet.Add(item);
            }
            return newSet;
        }

        public static void ForEach<T>(ISet<T> iset, Action<T> func) where T : IEquatable<T>
        {
            foreach (T item in iset)
            {
                func(item);
            }
        }

        public static bool CheckForAll<T>(ISet<T> iset, Func<T, bool> check) where T : IEquatable<T>
        {
            return Enumerable.All(iset, check);
        }
    }
}
