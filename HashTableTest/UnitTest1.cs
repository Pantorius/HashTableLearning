using HashSetLibrary;
using Newtonsoft.Json.Linq;

namespace HashTableTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Initialization()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            Assert.IsNotNull(IvanHashTable);
        }

        [TestMethod]
        public void InitialCount()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            Assert.AreEqual(0, IvanHashTable.Count);
        }

        [TestMethod]
        public void InitialCapacity()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            Assert.AreEqual(16, IvanHashTable.Capacity);
        }


        [TestMethod]
        public void InitialCapacityInConstructor()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new(31);

            Assert.AreEqual(32, IvanHashTable.Capacity);
        }

        [TestMethod]
        public void AddMethod()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            string key = "0";
            int value = 0;

            try
            {
                IvanHashTable.Add(key, value);
            }
            catch
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AddNullKeyException()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            string key = null;
            int value = 0;

            Assert.ThrowsException<ArgumentNullException>(() => IvanHashTable.Add(key, value));
        }


        [TestMethod]
        public void AddSameKeyException()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            string key = "0";
            int value = 0;

            IvanHashTable.Add(key, value);
            
            Assert.ThrowsException<ArgumentException>(() => IvanHashTable.Add(key, value));
        }

        [TestMethod]
        public void AddMethodCountIsIncreasing()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            for(int i = 0; i < 10; i++)
            {
                IvanHashTable.Add(i.ToString(), i);
            }

            Assert.AreEqual(10, IvanHashTable.Count);
        }

        [TestMethod]
        public void ContainsMethod()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 0;

            IvanHashTable.Add(key, value);

            Assert.IsTrue(IvanHashTable.Contains(key));
        }

        [TestMethod]
        public void ContainsMethodNullRefException()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 0;
            string findKey = null;

            IvanHashTable.Add(key, value);

            Assert.ThrowsException<ArgumentNullException>(() => IvanHashTable.Contains(findKey));
        }

        [TestMethod]
        public void ContainsMethodNotFound()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 0;
            string findKey = "1";

            IvanHashTable.Add(key, value);


            Assert.IsFalse(IvanHashTable.Contains(findKey));
        }

        [TestMethod]
        public void RemoveMethod()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 0;

            IvanHashTable.Add(key, value);
            IvanHashTable.Remove(key);
            Assert.IsFalse(IvanHashTable.Contains(key));
        }

        [TestMethod]
        public void RemoveMethodCountDecrease()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 0;

            IvanHashTable.Add(key, value);
            IvanHashTable.Remove(key);
            Assert.AreEqual(0, IvanHashTable.Count);
        }

        [TestMethod]
        public void RemoveMethodMultipleElements()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();

            for (int i = 0; i < 10; i++)
            {
                IvanHashTable.Add(i.ToString(), i);
            }

            for (int i = 0; i < 10; i++)
            {
                IvanHashTable.Remove(i.ToString());
            }

            Assert.AreEqual(0, IvanHashTable.Count);
        }

        [TestMethod]
        public void IndexReplaceValueTesting()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            string key = "0";
            int value = 1;
            IvanHashTable.Add(key, value);
            IvanHashTable[key] = value + 1;

            Assert.AreEqual(value + 1, IvanHashTable[key]);
        }

        [TestMethod]
        public void GetArrayMethod()
        {
            IvanHashTableAlternative<string, int> IvanHashTable = new();
            List<KeyValuePair<string, int>> list;
            List<KeyValuePair<string, int>> expectedList = new();
            for (int i = 0; i < 10; i++)
            {
                expectedList.Add(new KeyValuePair<string, int>(i.ToString(), i));
                IvanHashTable.Add(i.ToString(), i);
            }
            list = IvanHashTable.GetArray().ToList();

            for(int i = 0; i < expectedList.Count; i++)
            {
                Assert.IsTrue(expectedList.Contains(list[i]));
            }
        }
    }
}