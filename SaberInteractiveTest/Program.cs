using System;
using System.IO;

namespace SaberInteractiveTest
{
    internal sealed class Program
    {
        private LinkedListSerializer _listSerializer;
        private LinkedListDeserializer _listDeserializer;
        
        private static void Main()
        {
            var program = new Program();
            program.TestSerialize();
            program.TestDeserialize();
        }

        private void TestDeserialize()
        {
            var list = new ListRand();
            _listDeserializer = new LinkedListDeserializer();

            using var fileStream = new FileStream("SerealizationNodes.json", FileMode.Open);
            list.Deserialize(fileStream, _listDeserializer);

            Console.WriteLine($"List head data == {list.Head.Data}, List tail data == {list.Tail.Data}");
            
            fileStream.Dispose();
        }

        private void TestSerialize()
        {
            _listSerializer = new LinkedListSerializer
            {
                Offset = 0
            };
            
            //в данном блоке можно пихать что хочешь, только с умом, ничего не ломай
            var list = new ListRand();
            var head = new ListNode { Data = "Cortana" };
            var tail = new ListNode { Data = "Master Chief Petty Officer John-117", Prev = head };
            
            list.Head = head;
            list.Tail = tail;
            
            head.Next = tail;
            head.Rand = tail;
            
            tail.Rand = tail;

            using var fileStream = new FileStream("SerealizationNodes.json", FileMode.Create);
            list.Serialize(fileStream, _listSerializer);
            fileStream.Dispose();
        }
    }
}