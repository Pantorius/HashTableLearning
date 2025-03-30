using System;

namespace HashSetLibrary
{
    class Node<TKey, TValue>
    {
        public TKey Key = default;
        public TValue Value = default;
        public bool Exist = false;
        public int Hash = default(int);
        public Node(TKey Key, TValue Value, int Hash)
        {
            this.Key = Key;
            this.Value = Value;
            this.Exist = true;
            this.Hash = Hash;
        }
        public Node() { }
    }


    public class IvanHashTableAlternative<TKey, TValue> where TKey : IEquatable<TKey>
    {

        static int[] s_quadricConstants = new int[0];

        Node<TKey, TValue>[] _nodes;

        int _maxCapacity = int.MaxValue >> 10;
        int _count;
        int _capacity;
        const int C1 = 3;
        const int C2 = 10;

        // Переменная взятия по модулю должна быть степенью двойки для корректной работы
        int _mod; 

        // Делегат для лучшего сравнения ключей. Подстраивается под тип TKey
        delegate bool Comp<T>(object x, object y);
        Comp<TKey> _comparer = Comp<TKey>.Equals; 
        double _refillFactor = 0.75;

        // Глобальные переменные, чтобы не выделять постоянно память
        int _index; 
        int _hash;

        public int Count { get { return _count; } }
        public int Capacity { get { return _capacity; } private set { _capacity = ProperCapacityCheck(value); } }
        public IvanHashTableAlternative(int Capacity)
        {
            this.Capacity = Capacity;
            _nodes = new Node<TKey, TValue>[_capacity];
            _mod = _capacity - 1;
            if (typeof(TKey) == typeof(string))
            {
                _comparer = string.Equals;
            }
            CountQuadricConst();
        }
        public IvanHashTableAlternative() : this(16) { }

        int ProperCapacityCheck(int capa)
        {
            int i = 16;
            while(capa > i && i < _maxCapacity)
            {
                i <<= 1;
            }
            return i;
        }

        // Считаем константы суммы для квадратичного исследования, чтобы не терять время в процессе.
        void CountQuadricConst() 
        {
            // массив уже существует, не пересоздаём, лишнее место не занимаем. 
            if (s_quadricConstants.Length > 0) return;

            s_quadricConstants = new int[_maxCapacity];
            for (int i = 0; i < s_quadricConstants.Length; i++)
            {
                s_quadricConstants[i] = C1 * i + C2 * i * i;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (key is null) throw new ArgumentNullException("key is null");

            _hash = key.GetHashCode();
            Node<TKey, TValue>  newNode = new Node<TKey, TValue>(key, value, _hash);
            
            // Два ифа, для упрощения кода. Попробуем дважды добавить один ключ, иначе неразрешимая коллизия.
            if (!HashingItemInArray(newNode))
            {
                Resize();
                if (!HashingItemInArray(newNode))
                {
                    throw new ArithmeticException("Can't insert this key into hash set");
                }
            }
            if (_count > _capacity * _refillFactor)
            {
                Resize();
            }
        }
        bool HashingItemInArray(Node<TKey, TValue> newNode)
        {
            int i = 0;

            // Не более одного прохода по массиву, хэш функция обеспечивает перечисление всех индексов.
            while (i < _capacity)
            {
                _index = QuadraticProbing(i);
                if (_nodes[_index] == null || !_nodes[_index].Exist)
                {
                    _nodes[_index] = newNode;
                    ++_count;
                    break;
                }
                if (_comparer(_nodes[_index].Key, newNode.Key))
                {
                    throw new ArgumentException("This key is already exist");
                }
                ++i;
            }
            // Случай, когда мы не смогли добавить элемент в массив.
            if (i >= _capacity) return false;
           
            return true;
        }

        public bool Contains(TKey key)
        {
            if (_count == 0)
                return false;
            return Find(key) != null;
        }

        public bool Remove(TKey key)
        {
            if (_count == 0)
                return false;
            Node<TKey, TValue> node = Find(key);
            if (node == null)
                return false;
            node.Exist = false;
            --_count;
            return true;
        }



        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_count == 0)
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
            _hash = key.GetHashCode();


            // Не более одного прохода по массиву, хэш функция обеспечивает перечисление всех индексов.
            while (i < _capacity)
            {
                _index = QuadraticProbing(i); //квадратичное исследование
                if (_nodes[_index] == null)
                {
                    value = default;
                    return false;
                }
                if (_nodes[_index].Exist && _comparer(_nodes[_index].Key, key))
                {
                    value = _nodes[_index].Value;
                    if (rm)
                    {
                        _nodes[_index].Exist = false;
                        --_count;
                    }
                    return true;
                }
                ++i;
            }
            value = default;
            return false;
        }

        Node<TKey, TValue> Find(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can't be null here");

            int i = 0;
            _hash = key.GetHashCode();

            // Не более одного прохода по массиву, хэш функция обеспечивает перечисление всех индексов.
            while (i < _capacity)
            {
                _index = QuadraticProbing(i); //квадратичное исследование
                if (_nodes[_index] == null)
                {
                    return null;
                }
                if (_nodes[_index].Exist && _comparer(_nodes[_index].Key, key))
                {
                    return _nodes[_index];
                }
                ++i;
            }
            return null;
        }

        void Resize()
        {
            _capacity = _capacity << 1;
            _mod = _capacity - 1;
            Node<TKey, TValue>[] newNodes = new Node<TKey, TValue>[_capacity];
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null && _nodes[i].Exist)
                    HashingInResize(i, ref newNodes);

            }
            _nodes = newNodes;
        }

        void HashingInResize(int nodeIndex, ref Node<TKey, TValue>[] newNodes)
        {
            int i = 0;
            while (true)
            {
                _hash = _nodes[nodeIndex].Hash;
                _index = QuadraticProbing(i);
                if (newNodes[_index] == null)
                {
                    newNodes[_index] = _nodes[nodeIndex];
                    return;
                }
                ++i;
            }
            throw new ArithmeticException("Can't resize hash table properly");
        }

        // Квадратичное исследование. За счёт размера массива в степень двойки, побитовое И работает, как модуль.
        int QuadraticProbing(int index)
        {
            return (_hash + s_quadricConstants[index]) & _mod;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetArray()
        {
            List<KeyValuePair<TKey, TValue>> lst = new List<KeyValuePair<TKey, TValue>>();
            for (int i = 0; i < _nodes.Length; ++i)
            {
                if (_nodes[i] != null && _nodes[i].Exist)
                {
                    lst.Add(new KeyValuePair<TKey, TValue>(_nodes[i].Key, _nodes[i].Value));
                }
            }
            return lst;
        }

        public TValue this[TKey key]
        {
            get
            {
                var flag = FindOrRemAction(key, false, out TValue returnValue);
                if (!flag)
                    throw new KeyNotFoundException();
                return returnValue;
            }

            set
            {
                var node = Find(key) ?? throw new KeyNotFoundException();
                node.Value = value;
            }
        }
    }
}
