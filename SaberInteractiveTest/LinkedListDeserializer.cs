using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SaberInteractiveTest
{
    public sealed class LinkedListDeserializer
    {
        private readonly int _nodePropertiesCount;

        private readonly List<NodeHolder> _nodeHolder;

        private const int SPLIT_PARTS_COUNT = 2;
        private const char STOP_CHAR = ',';
        private const char LINE_SEPARATOR = ':';

        public LinkedListDeserializer()
        {
            _nodeHolder = new List<NodeHolder>();
            _nodePropertiesCount = typeof(ListNode).GetFields().Length; // using GetFields to expect how many fields we have
        }

        public void DeserializeList(FileStream fileStream, ListRand list)
        {
            ReadFile(fileStream, list);
        }

        private void ReadFile(FileStream fileStream, ListRand listRand)
        {
            var inCurrentNode = false;
            var jsonString = (char)fileStream.ReadByte();

            while (true)
            {
                jsonString = (char)fileStream.ReadByte(); // reading our jsonFile
                if (jsonString <= 0)
                {
                    Console.WriteLine("Все поломалось, нужно срочно исправить, надо написать тессттт.....");
                    return;
                }
                
                if (jsonString == '{' && !inCurrentNode) // finding blocks and trying to deserialize them
                {
                    inCurrentNode = true;
                    var deserializedNode = DeserializeNode(fileStream);
                    _nodeHolder.Add(deserializedNode);
                }
                
                else if (jsonString == ',' && inCurrentNode)
                {
                    inCurrentNode = false;
                }

                else if (jsonString == ']')
                {
                    Console.WriteLine("Все прошло успешно");
                    FillLinkedList(listRand, _nodeHolder);
                    break;
                }
            }
        }

        private void FillLinkedList(ListRand listRand, List<NodeHolder> nodeHolders)
        {
            ConnectNodes(nodeHolders);
            listRand.Head = nodeHolders.First().Node;
            listRand.Tail = nodeHolders.Last().Node;
            listRand.Count = nodeHolders.Count;
        }

        private void ConnectNodes(List<NodeHolder> nodeHolders)
        {
            foreach (var node in nodeHolders)
            {
                if (node.Prev != "empty")
                    node.Node.Prev = nodeHolders[int.Parse(node.Prev)].Node;
                if (node.Rand != "empty")
                    node.Node.Rand = nodeHolders[int.Parse(node.Rand)].Node;
                if (node.Next != "empty")
                    node.Node.Next = nodeHolders[int.Parse(node.Next)].Node;
            }
        }

        private NodeHolder DeserializeNode(FileStream fileStream)
        {
            var node = new ListNode();
            var nodeCopy = new Dictionary<string, string>();
            
            for (var i = 0; i < _nodePropertiesCount - 1; i++)
            {
                var nodeFields = ReadNodeFields(fileStream, STOP_CHAR);
                SplitAndAddNode(nodeFields, nodeCopy);
            }

            var endNodeFiled = ReadNodeFields(fileStream, '}');
            SplitAndAddNode(endNodeFiled, nodeCopy);
            
            return new NodeHolder(node, nodeCopy["Next"], nodeCopy["Prev"], nodeCopy["Rand"],
                nodeCopy["Data"]);
        }

        private void SplitAndAddNode(string nodeFields, Dictionary<string, string> nodeCopy)
        {
            var nodeValues = nodeFields?.Split(new[] {LINE_SEPARATOR}, SPLIT_PARTS_COUNT); // counts of element means field and value of it
            if (nodeValues != null) 
                nodeCopy.Add(nodeValues[0], nodeValues[1]);
        }

        private string ReadNodeFields(FileStream fileStream, char stoppingChar)
        {
            var stringBuilder = new StringBuilder(); // add operation in default string is so expensive
            // that's the reason why u i use's string builder to concatenate strings
            while (true)
            {
                var bytes = fileStream.ReadByte();
                
                if ((char)bytes == stoppingChar) // stop the cycle then current field is over
                {
                    Console.WriteLine("Поле заполнено");
                    break;
                }
                
                stringBuilder.Append((char) bytes);
            }

            return stringBuilder.ToString();
        }
    }
}