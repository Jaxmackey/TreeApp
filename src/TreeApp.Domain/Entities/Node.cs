namespace TreeApp.Domain.Entities;

public class Node
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long? ParentId { get; set; }
    public string TreeName { get; set; } = null!;
    
    public Node? Parent { get; set; }
    public List<Node> Children { get; set; } = new();
}