namespace WebServer.Routing
{
    public class ConstantNode : GraphNode
    {
        public ConstantNode(string value)
        {
            Template = value;
        }
    }
}
