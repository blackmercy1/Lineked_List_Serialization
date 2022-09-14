namespace SaberInteractiveTest
{
    public sealed class NodeHolder
    {
        public readonly string Prev;
        public readonly string Next;
        public readonly string Rand;
        public readonly ListNode Node;

        public NodeHolder(ListNode node, string next, string previous, string random, string data)
        {
            node.Data = data;
            
            Next = next;
            Prev = previous;
            Rand = random;
            Node = node;
        }
    }
}