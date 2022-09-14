using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberInteractiveTest
{
    public sealed class LinkedListSerializer
    {
        public int Offset = 0;
        
        public void SerializeList(FileStream fileStream, ListRand list)
        {
            var nodeIndex = GetIndexForNode(list);
            var jsonList = CreateJsonNode(list, nodeIndex);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonList);
            fileStream.Write(jsonBytes, Offset, jsonBytes.Length);
        }

        private string CreateJsonNode(ListRand list, Dictionary<ListNode, int> nodeIndex)
        {
            var stringBuilder = new StringBuilder(); // + operations for string allocation improvement
            var currentNode = list.Head;

            stringBuilder.Append('[');

            while (currentNode != null)
            {
                CreateNode(nodeIndex, currentNode, stringBuilder);
                var switchNode = SwitchNode(ref currentNode);
                if (!switchNode)
                {
                    break;
                }

                stringBuilder.Append(',');
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        private bool SwitchNode(ref ListNode currentNode)
        {
            currentNode = currentNode.Next;
            var switchMode = currentNode != null;
            
            return switchMode;
        }

        private void CreateNode(Dictionary<ListNode, int> nodeIndex, ListNode currentNode, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append('{')
                .Append($"Prev:{(currentNode.Prev == null ? "empty" : nodeIndex[currentNode.Prev].ToString())},")
                .Append($"Next:{(currentNode.Next == null ? "empty" : nodeIndex[currentNode.Next].ToString())},")
                .Append($"Rand:{(currentNode.Rand == null ? "empty" : nodeIndex[currentNode.Rand].ToString())},")
                .Append($"Data:'{currentNode.Data}'")
                .Append('}');
        }

        private Dictionary<ListNode, int> GetIndexForNode(ListRand list)
        {
            var indexForNode = new Dictionary<ListNode, int>();
            var index = 0;
            var currentNode = list.Head;

            if (currentNode == null)
            {
                throw new NotImplementedException("Missing head reference, bro add it pls -_-");
            }

            while (currentNode != null &&(!indexForNode.ContainsKey(currentNode)))
            {
                indexForNode.Add(currentNode, index);
                index++;
                currentNode = currentNode.Next;
            }

            return indexForNode;
        }
    }
}