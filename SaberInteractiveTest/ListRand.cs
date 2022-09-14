using System.IO;

namespace SaberInteractiveTest
{
    public sealed class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        
        public int Count;
        
        public void Serialize(FileStream fileStream, LinkedListSerializer serializer) // можно сделать статическим сериализатор и 
        // не придется вставлять ссылку, но я не любитель статики, хоть и нарушает условие, мне кажется, что так лучше 
        {
            serializer.SerializeList(fileStream, this);
        }

        public void Deserialize(FileStream fileStream, LinkedListDeserializer deserializer)
        {
            deserializer.DeserializeList(fileStream, this);
        }
    }
}