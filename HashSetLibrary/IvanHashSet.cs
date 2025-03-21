namespace HashSetLibrary
{
    class Node<TKey, TValue>
    {
        public TKey key = default(TKey);
        public TValue value = default(TValue);
        public bool exist = false;
        public int hash = default(int);
        public Node(ref TKey key, ref TValue value, int hash)
        {
            this.key = key;
            this.value = value;
            this.exist = true;
            this.hash = hash;
        }
        public Node() { }
    }


    public class IvanHashTableAlternative<TKey, TValue> where TKey : IEquatable<TKey>
    {
        Node<TKey, TValue>[] nodes;
        static int[] quadricConstants = new int[0];
        int count;
        int capacity;
        //int maxIterations = 5;
        int c1 = 3;
        int c2 = 10;
        int mod;


        Node<TKey, TValue> newNode;
        delegate bool Comparer<T>(object x, object y);
        Comparer<TKey> comp = Comparer<TKey>.Equals; //Делегат для лучшего сравнения ключей. Подстраивается под тип TKey
        double refillFactor = 0.75;
        int index; //глобальные переменные, чтобы не выделять постоянно память
        int hash;

        public int Count { get { return count; } }
        public int Capacity { get { return capacity; } }
        public IvanHashTableAlternative(int capacity)
        {
            nodes = new Node<TKey, TValue>[capacity];
            this.capacity = capacity;
            mod = capacity - 1;
            if (typeof(TKey) == typeof(string))
            {
                comp = string.Equals;
            }
            CountQuadricConst();
        }
        public IvanHashTableAlternative() : this(4) { }

        void CountQuadricConst() //Считаем константы суммы для квадратичного исследования, чтобы не терять время в процессе.
        {
            quadricConstants = new int[int.MaxValue / 1024];
            for(int i = 0; i <  quadricConstants.Length; i++)
            {
                quadricConstants[i] = c1 * i + c2 * i * i;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if(key is null) throw new ArgumentNullException("key is null");
            hash = key.GetHashCode();
            newNode = new Node<TKey, TValue>(ref key, ref value,hash);
            //newNode = new Node<TKey, TValue>(ref key, ref value, hash);
            if (!HashingItemInArray())
            {
                Resize();
                if (!HashingItemInArray())
                {
                    throw new ArithmeticException("Can't insert this key into hash set");
                }
            }
            if (count > capacity * refillFactor)
            {
                Resize();
            }
        }
        bool HashingItemInArray()
        {
            int i = 0;
            while (i < capacity)
            {
                index = (hash + quadricConstants[i]) & mod;//квадратичное исследование за счёт размера массива в степень двойки, побитовое И работает, как модуль
                if (nodes[index] == null || !nodes[index].exist)
                {
                    nodes[index] = newNode; //new Node<TKey, TValue>(ref key, ref value, hash);
                    ++count;
                    break;
                }
                if (comp(nodes[index].key, newNode.key))
                {
                    throw new ArgumentException("This key is already exist");
                }
                ++i;
            }
            if (i >= capacity) return false; //случай, когда мы не смогли добавить элемент в массив
            if (i > capacity * refillFactor) Resize();
            return true;
        }

        public bool Contains(TKey key)
        {
            if (count == 0) throw new InvalidOperationException();
            TValue tmp;
            return FindOrRemAction(key, false, out tmp);
        }

        public bool Remove(TKey key)
        {
            if (count == 0) throw new InvalidOperationException();
            TValue tmp;
            return FindOrRemAction(key, true, out tmp);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (count == 0)
            {
                value = default;
                return false;
            }
            TValue tmp;
            bool b = FindOrRemAction(key, false, out tmp);
            value = tmp;
            return b;
        }

        bool FindOrRemAction(TKey key, bool rm, out TValue value)
        {
            if (key == null) throw new ArgumentNullException("Key can't be null here");
            int i = 0;
            hash = key.GetHashCode(); ; //EqualityComparer.Default соответствующий TKey

            while (i < capacity)
            {
                index = (hash + quadricConstants[i]) & mod; //квадратичное исследование
                if (nodes[index] == null)
                {
                    value = default;
                    return false;
                }
                if (nodes[index].exist && comp(nodes[index].key, key))
                {
                    value = nodes[index].value;
                    if (rm)
                    {
                        nodes[index].exist = false;
                        --count;
                    }
                    return true;
                }
                ++i;
            }
            value = default;
            return false;
        }


        void Resize()
        {
            capacity = capacity << 1;
            mod = capacity - 1;
            Node<TKey, TValue>[] newNodes = new Node<TKey, TValue>[capacity];
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null && nodes[i].exist)
                    HashingInResize(i, ref newNodes);

            }
            nodes = newNodes;
        }
        void HashingInResize(int nodeIndex, ref Node<TKey,TValue>[] newNodes)
        {
            int i = 0;
            //int mod = capacity - 1;
            // & (capacity - 1);
            while (true)
            {
                index = (nodes[nodeIndex].hash + quadricConstants[i]) & mod;
                if (newNodes[index] == null)
                {
                    newNodes[index] = nodes[nodeIndex];
                    return;
                }
                ++i;
            }
            throw new ArithmeticException("Can't resize hash table properly");
        }


        public IEnumerable<KeyValuePair<TKey, TValue>> GetArray()
        {
            List<KeyValuePair<TKey, TValue>> lst = new List<KeyValuePair<TKey, TValue>>();
            for(int i = 0;i < nodes.Length; ++i)
            {
                if (nodes[i] != null && nodes[i].exist)
                {
                    lst.Add(new KeyValuePair<TKey, TValue>(nodes[i].key, nodes[i].value));
                }
            }
            return lst;
        }


        //int ReturnHash(TKey key)
        //{
        //    return key.GetHashCode() & (capacity - 1);
        //}

        //int ReturnHash2(ref TKey key)
        //{
        //    return (key.GetHashCode() & 0x7FFFFFFF) % capacity;
        //}
    }
}
